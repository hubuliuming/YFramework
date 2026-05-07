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
