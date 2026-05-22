using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YFramework.UI
{
    public enum UILayer
    {
        Bg = 0,
        Player = 1,
        Normal = 2,
        Effect = 3,
        Highest = 4
    }

    public interface IUIRoot
    {
        GameObject OpenUI(UILayer layer, GameObject uiObject, object userData = null);
        bool CloseUI(GameObject uiObject);
        bool CloseTopUI(UILayer layer);
        void CloseLayerUI(UILayer layer);
        void CloseAllUI();
        bool IsUIOpen(GameObject uiObject);
        bool ReleaseUI(GameObject uiObject);
        GameObject GetOpenedUI(GameObject uiObject);
        T GetWindow<T>() where T : Component, IUIWindow;
    }

    public interface IUIWindow
    {
        void OnUIOpen(UILayer layer, object userData);
        void OnUIClose();
    }

    public class UIWindowBase : MonoBehaviour, IUIWindow
    {
        public virtual void OnUIOpen(UILayer layer, object userData)
        {
        }

        public virtual void OnUIClose()
        {
        }

        public void CloseSelf()
        {
            GameUIKit.Close(gameObject);
        }
    }

    public static class GameUIKit
    {
        private static readonly HashSet<int> s_defaultClickBoundButtons = new HashSet<int>();
        private static IUIRoot s_root;

        public static event Action DefaultClick;

        public static bool HasRoot
        {
            get { return s_root != null; }
        }

        public static void RegisterRoot(IUIRoot root)
        {
            s_root = root;
        }

        public static void UnregisterRoot(IUIRoot root)
        {
            if (s_root == root)
            {
                s_root = null;
            }
        }

        public static GameObject Open(UILayer layer, GameObject uiObject, object userData = null)
        {
            if (s_root == null)
            {
                Debug.LogError("GameUIKit.Open failed because no UI root is registered.");
                return null;
            }

            return s_root.OpenUI(layer, uiObject, userData);
        }

        public static bool Close(GameObject uiObject)
        {
            return s_root != null && s_root.CloseUI(uiObject);
        }

        public static bool CloseTop(UILayer layer)
        {
            return s_root != null && s_root.CloseTopUI(layer);
        }

        public static void CloseLayer(UILayer layer)
        {
            if (s_root != null)
            {
                s_root.CloseLayerUI(layer);
            }
        }

        public static void CloseAll()
        {
            if (s_root != null)
            {
                s_root.CloseAllUI();
            }
        }

        public static bool IsOpen(GameObject uiObject)
        {
            return s_root != null && s_root.IsUIOpen(uiObject);
        }

        public static bool Release(GameObject uiObject)
        {
            return s_root != null && s_root.ReleaseUI(uiObject);
        }

        public static GameObject GetOpened(GameObject uiObject)
        {
            return s_root != null ? s_root.GetOpenedUI(uiObject) : null;
        }

        public static T GetWindow<T>() where T : Component, IUIWindow
        {
            return s_root != null ? s_root.GetWindow<T>() : null;
        }

        public static void NotifyOpen(GameObject uiObject, UILayer layer, object userData)
        {
            if (uiObject == null)
            {
                return;
            }

            IUIWindow[] windows = uiObject.GetComponentsInChildren<IUIWindow>(true);
            for (int i = 0; i < windows.Length; i++)
            {
                windows[i].OnUIOpen(layer, userData);
            }

            BindDefaultClick(uiObject);
        }

        public static void NotifyClose(GameObject uiObject)
        {
            if (uiObject == null)
            {
                return;
            }

            IUIWindow[] windows = uiObject.GetComponentsInChildren<IUIWindow>(true);
            for (int i = 0; i < windows.Length; i++)
            {
                windows[i].OnUIClose();
            }

            UnbindDefaultClick(uiObject);
        }

        public static void BindDefaultClick(GameObject uiObject)
        {
            if (uiObject == null)
            {
                return;
            }

            Button[] buttons = uiObject.GetComponentsInChildren<Button>(true);
            for (int i = 0; i < buttons.Length; i++)
            {
                Button button = buttons[i];
                if (button == null)
                {
                    continue;
                }

                int instanceId = button.GetInstanceID();
                if (!s_defaultClickBoundButtons.Add(instanceId))
                {
                    continue;
                }

                button.onClick.AddListener(NotifyDefaultClick);
            }
        }

        public static void UnbindDefaultClick(GameObject uiObject)
        {
            if (uiObject == null)
            {
                return;
            }

            Button[] buttons = uiObject.GetComponentsInChildren<Button>(true);
            for (int i = 0; i < buttons.Length; i++)
            {
                Button button = buttons[i];
                if (button == null)
                {
                    continue;
                }

                int instanceId = button.GetInstanceID();
                if (s_defaultClickBoundButtons.Remove(instanceId))
                {
                    button.onClick.RemoveListener(NotifyDefaultClick);
                }
            }
        }

        private static void NotifyDefaultClick()
        {
            Action defaultClick = DefaultClick;
            if (defaultClick != null)
            {
                defaultClick();
            }
        }
    }
}
