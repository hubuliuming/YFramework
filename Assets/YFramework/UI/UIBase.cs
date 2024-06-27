/****************************************************
    文件：ViewBase.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：UI基类
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        
        private  Stack<GameObject> _lastUIs = new Stack<GameObject>();
        public GameObject CurUI { get;  private set; }
        public bool IsNullUI  => CurUI == null;
        
        public void ShowUI(GameObject curUI) 
        {
            if (curUI == null)
            {
                Debug.LogWarning("要展示的物体为空！");
                return;
            }
            if (curUI == CurUI)
            {
                Debug.Log("重复打开当前的UI："+curUI);
                return;
            }
            if (CurUI != null) 
            {
                _lastUIs.Push(CurUI);
            }
            CurUI = curUI;
            CurUI.gameObject.SetActive(true);
        }
        
        public void HideUI()
        {
            if (CurUI == null) return;
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
        }

        public void HideUI(GameObject curUI)
        {
            if (CurUI == curUI)
            {
                HideUI();
            } else {
                Debug.LogError("不允许跨索引隐藏:"+ curUI.name +","+"当前UI为："+CurUI.name); 
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