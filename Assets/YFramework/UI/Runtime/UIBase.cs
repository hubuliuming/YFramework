/****************************************************
    文件：ViewBase.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：UI基类
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace YFramework.UI
{
    public interface IUIBase
    {
        void Show();
        void Hide();
    }

    public abstract class UIBase : YMonoBehaviour,IUIBase
    {
        private RectTransform m_rectTransform;

        public RectTransform rectTransform
        {
            get
            {
                if (m_rectTransform == null) m_rectTransform = GetComponent<RectTransform>();
                return m_rectTransform;
            }
        }

        public virtual void Show()
        {
            var success = UIManager.Instance.ShowUI(gameObject);
            if(success) OnShow();
        }

        public virtual void Hide()
        {
            var success = UIManager.Instance.HideUI(gameObject);
            if (success) OnHide();
        }

        protected abstract void OnShow();
        protected abstract void OnHide();

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
        
        private Stack<GameObject> _lastUIs = new Stack<GameObject>();
        public GameObject CurUI { get;  private set; }
        public bool IsNullUI  => CurUI == null;
        
        public bool ShowUI(GameObject curUI) 
        {
            if (curUI == null)
            {
                Debug.LogWarning("要展示的物体为空！");
                return false;
            }
            if (curUI == CurUI)
            {
                Debug.Log("重复打开当前的UI："+curUI);
                return false;
            }
            if (CurUI != null) 
            {
                _lastUIs.Push(CurUI);
            }
            CurUI = curUI;
            CurUI.gameObject.SetActive(true);
            return true;
        }
        
        public bool HideUI()
        {
            if (CurUI == null) return false;
            CurUI.SetActive(false);
            if (_lastUIs.Count > 0)
            { 
                CurUI = _lastUIs.Pop();
            }
            else
            {
                CurUI = null;
                _lastUIs.Clear();
            }
            return true;
        }

        public bool HideUI(GameObject curUI)
        {
            if (CurUI == curUI)
            {
                return HideUI();
            }
            else
            {
                Debug.LogError("不允许跨索引隐藏:" + curUI.name + "," + "当前UI为：" + CurUI.name);
                return false;
            }
        }

        public void HideAllUI()
        {
            for (int i = 0; i < _lastUIs.Count +1; i++)
            {
                HideUI();
            }
        }
    }
}