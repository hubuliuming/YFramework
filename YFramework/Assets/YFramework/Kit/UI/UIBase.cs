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
            UIManager.Instance.ShowUI(gameObject);
        }

        public virtual void Hide()
        {
            UIManager.Instance.HideUI(gameObject);
        }
    }

    public class UIManager 
    {
        private static UIManager _instance;
        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    UIManager uiManager = new UIManager();
                    _instance = uiManager;
                }
                return _instance;
            }
        }
        
        private static Stack<GameObject> _lastUIs = new Stack<GameObject>();
        private static GameObject _curUI;
        public bool IsNullUI  => _curUI == null;
        
        public void ShowUI(GameObject curUI,Action onShow = null) 
        {
            if (curUI == null)
            {
                Debug.LogWarning("要展示的物体为空！");
                return;
            }
            if (_curUI != null) 
            {
                _lastUIs.Push(_curUI);
            }
            _curUI = curUI;
            _curUI.gameObject.SetActive(true);
          
            onShow?.Invoke();
        }
        
        public void HideUI(Action onHide = null)
        {
            if (_curUI == null) return;
            _curUI.SetActive(false);
            if (_lastUIs.Count > 0)
            { 
                _curUI = _lastUIs.Pop();
            }
            else
            {
                _curUI = null;
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
                Debug.LogError("不允许跨索引隐藏:"+ curUI.name +","+"当前UI为："+_curUI.name); 
            }
        }

        public void HideAllUI(Action onHide = null)
        {
            for (int i = 0; i < _lastUIs.Count +1; i++)
            {
                HideUI(onHide);
            }
        }
    }
}