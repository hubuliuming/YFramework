/****************************************************
    文件：ViewBase.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：UI基类
*****************************************************/

using System;

namespace YFramework.UI
{
    public interface IView
    {
        void Init();
        void Show();
        void Hide();
    }

    public abstract class UIBase : YMonoBehaviour,IView
    {
        protected Action<object> onUpdatePage;
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
        public virtual void Init()
        {
            MsgDispatcher.Register("OnUpdatePage", o =>
            {
                onUpdatePage?.Invoke(o);
            });
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}