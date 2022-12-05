/****************************************************
    文件：ViewBase.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：UI基类
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace YFramework.Kit.UI
{
    public interface IUIBase
    {
        void Show();
        void Hide();
    }

    public abstract class UIBase : YMonoBehaviour,IUIBase
    {
        private UIUtility _uiUtility;

        protected UIUtility UiUtility
        {
            get
            {
                if (_uiUtility == null)
                {
                    _uiUtility = gameObject.AddComponent<UIUtility>();
                    _uiUtility.Init();
                }

                return _uiUtility;
            }
        }

        public virtual void Show()
        {
            UISystem.Instance.ShowUI(gameObject);
        }

        public virtual void Hide()
        {
            UISystem.Instance.HideUI(gameObject);
        }
    }

    public class UISystem 
    {
        private static UISystem _instance;
        public static UISystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    UISystem uiSystem = new UISystem();
                    _instance = uiSystem;
                }
                return _instance;
            }
        }
        
        private static Stack<GameObject> _lastUIs = new Stack<GameObject>();
        private static GameObject _curUI;
        public bool IsNullUI { get; private set; } = true;
        
        public void ShowUI(GameObject curUI,Action onShow = null) 
        {
            if (_curUI != null) 
            {
                _lastUIs.Push(_curUI);
            }
            _curUI = curUI;
            _curUI.gameObject.SetActive(true);
            IsNullUI = false;
          
            onShow?.Invoke();
        }
        
        public void HideUI(Action onHide = null)
        {
            _curUI.gameObject.SetActive(false);
            if (_lastUIs.Count > 0)
            { 
                _curUI = _lastUIs.Pop();
            }
            else
            {
                _curUI = null;
                IsNullUI = true;
                _lastUIs.Clear();
            }
            onHide?.Invoke();
        }

        public void HideUI(GameObject curUI, Action onHide = null)
        {
            if (_curUI == curUI)
            {
                HideUI(onHide);
            } else {
               Debug.LogError("不允许跨索引隐藏,请检查控制UI界面逻辑是否纳入UISystem控制"); 
            }
        }

        public void HideAllUI(Action onHideAll = null)
        {
            foreach (var go in _lastUIs)
            {
                HideUI(go,onHideAll);
            }
        }
    }
}