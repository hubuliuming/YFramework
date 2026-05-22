using System;
using System.Collections.Generic;
using UnityEngine;

namespace YFramework.UI
{
    public class UIRoot : MonoBehaviour, IUIRoot
    {
        [SerializeField] private Transform m_bgRoot;
        [SerializeField] private Transform m_playerRoot;
        [SerializeField] private Transform m_normalRoot;
        [SerializeField] private Transform m_effectRoot;
        [SerializeField] private Transform m_highestRoot;

        private Dictionary<int, Transform> m_layerRoots;
        private Dictionary<Type, IUIWindow> m_windowsByType;
        private UIKitRuntime m_runtime;
        private UIRuntimeCallbacks m_callbacks;

        private void Awake()
        {
            EnsureLayerRoots();
            BuildLayerMap();

            m_runtime = new UIKitRuntime(this);
            m_callbacks = new UIRuntimeCallbacks
            {
                NotifyOpen = (go, layer, data) => GameUIKit.NotifyOpen(go, (UILayer)layer, data),
                NotifyClose = go => GameUIKit.NotifyClose(go),
            };

            GameUIKit.RegisterRoot(this);
            m_runtime.RegisterSceneUI(CreateLayerRoots(), m_callbacks);
            BuildWindowCache();
        }

        private void OnDestroy()
        {
            GameUIKit.UnregisterRoot(this);
        }

        public GameObject OpenUI(UILayer layer, GameObject uiObject, object userData = null)
        {
            Transform layerRoot = GetLayerRoot(layer);
            return m_runtime.Open((int)layer, layerRoot, uiObject, userData, m_callbacks);
        }

        public bool CloseUI(GameObject uiObject)
        {
            return m_runtime.Close(uiObject, m_callbacks);
        }

        public bool CloseTopUI(UILayer layer)
        {
            return m_runtime.CloseTop((int)layer, m_callbacks);
        }

        public void CloseLayerUI(UILayer layer)
        {
            m_runtime.CloseLayer((int)layer, m_callbacks);
        }

        public void CloseAllUI()
        {
            for (int i = (int)UILayer.Highest; i >= (int)UILayer.Bg; i--)
            {
                m_runtime.CloseLayer(i, m_callbacks);
            }
        }

        public bool IsUIOpen(GameObject uiObject)
        {
            return m_runtime.IsOpen(uiObject);
        }

        public bool ReleaseUI(GameObject uiObject)
        {
            return m_runtime.Release(uiObject, m_callbacks);
        }

        public GameObject GetOpenedUI(GameObject uiObject)
        {
            return m_runtime.GetOpened(uiObject);
        }

        public T GetWindow<T>() where T : Component, IUIWindow
        {
            IUIWindow window;
            return m_windowsByType.TryGetValue(typeof(T), out window) ? window as T : null;
        }

        private Transform GetLayerRoot(UILayer layer)
        {
            Transform root;
            if (!m_layerRoots.TryGetValue((int)layer, out root) || root == null)
            {
                root = transform;
            }

            return root;
        }

        private void BuildLayerMap()
        {
            m_layerRoots = new Dictionary<int, Transform>
            {
                { (int)UILayer.Bg, m_bgRoot },
                { (int)UILayer.Player, m_playerRoot },
                { (int)UILayer.Normal, m_normalRoot },
                { (int)UILayer.Effect, m_effectRoot },
                { (int)UILayer.Highest, m_highestRoot },
            };
        }

        private IEnumerable<UILayerRoot> CreateLayerRoots()
        {
            return new[]
            {
                new UILayerRoot((int)UILayer.Bg, m_bgRoot),
                new UILayerRoot((int)UILayer.Player, m_playerRoot),
                new UILayerRoot((int)UILayer.Normal, m_normalRoot),
                new UILayerRoot((int)UILayer.Effect, m_effectRoot),
                new UILayerRoot((int)UILayer.Highest, m_highestRoot),
            };
        }

        private void BuildWindowCache()
        {
            m_windowsByType = new Dictionary<Type, IUIWindow>();

            foreach (UILayerRoot layerRoot in CreateLayerRoots())
            {
                if (layerRoot.Root == null)
                {
                    continue;
                }

                IUIWindow[] windows = layerRoot.Root.GetComponentsInChildren<IUIWindow>(true);
                for (int i = 0; i < windows.Length; i++)
                {
                    RegisterWindow(windows[i]);
                }
            }
        }

        private void RegisterWindow(IUIWindow window)
        {
            Type windowType = window.GetType();
            if (m_windowsByType.ContainsKey(windowType))
            {
                throw new InvalidOperationException("UIRoot found duplicate IUIWindow type: " + windowType.Name);
            }

            m_windowsByType.Add(windowType, window);
        }

        private void EnsureLayerRoots()
        {
            CreateIfNull(ref m_bgRoot, "Bg");
            CreateIfNull(ref m_playerRoot, "Player");
            CreateIfNull(ref m_normalRoot, "Normal");
            CreateIfNull(ref m_effectRoot, "Effect");
            CreateIfNull(ref m_highestRoot, "Highest");
        }

        private void CreateIfNull(ref Transform target, string name)
        {
            if (target != null)
            {
                return;
            }

            Transform existing = transform.Find(name);
            if (existing != null)
            {
                target = existing;
                return;
            }

            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(transform, false);
            target = go.transform;
        }
    }
}
