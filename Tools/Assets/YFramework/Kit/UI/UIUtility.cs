/****************************************************
    文件：UIUtility.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YFramework.Extension;

namespace YFramework.Kit.UI
{
    public class UIUtility : MonoBehaviour
    {
        private Dictionary<string, UIUtilityData> _datas;
        public void Init()
        {
            _datas = new Dictionary<string, UIUtilityData>();
            var rectTrans = transform.GetComponent<RectTransform>();
            foreach (RectTransform rectTran in rectTrans)
            {
                _datas.Add(rectTran.name,new UIUtilityData(rectTran));
            }
        }
        /// <summary>
        /// 获取当前物体下的UIUtilityData
        /// </summary>
        /// <param name="nameStr">物体名字（可以传路径）</param>
        /// <returns></returns>
        public UIUtilityData Get(string nameStr)
        {
            if (!_datas.TryGetValue(nameStr,out var data))
            {
                Transform temp = transform.Find(nameStr);
                if (temp == null)
                {
                    Debug.LogError("无法按照路径找到物体，路径名字为："+nameStr);
                }
                else
                {
                    _datas.Add(nameStr, new UIUtilityData(temp.GetComponent<RectTransform>()));
                }

                return _datas[nameStr];
            }

            return data;
        }
    }

    /// <summary>
    /// 单个UI物体的所有参数和操作
    /// </summary>
    public class UIUtilityData
    {
        public GameObject Go { get; private set; }
        public RectTransform RectTrans { get; private set; }
        public Image Img { get; private set; }
        public Button Btn { get; private set; }
        public Text Text { get; private set; }

        public UIUtilityData(RectTransform rectTrans)
        {
            this.RectTrans = rectTrans;
            this.Go = rectTrans.gameObject;
            this.Img = rectTrans.GetComponent<Image>();
            this.Btn = rectTrans.GetComponent<Button>();
            this.Text = rectTrans.GetComponent<Text>();
        }

        public void AddListener(Action action)
        {
            if (Btn != null)
            {
                Btn.onClick.AddListener(()=>action());
            }
            else
            {
                Debug.LogError("当前物体上没有Button组件，物体名字为："+Go.name);
            }
        }

        public void SetActive(bool isActive = true)
        {
            Go.SetActive(isActive);
        }
        public void SetImage(Sprite sprite)
        {
            if (Img != null)
            {
                Img.sprite = sprite;
            }
            else
            {
                Debug.LogError("当前物体上没有Image组件，物体名字为："+Go.name);
            }
        }
        

        public void SetImage(Color color)
        {
            if (Img != null)
            {
                Img.color = color;
            }
            else
            {
                Debug.LogError("当前物体上没有Image组件，物体名字为："+Go.name);
            }
        }

        public void SetText(string str)
        {
            if (Text != null)
            {
                Text.text = str;
            }
            else
            {
                Debug.LogError("当前物体上没有Text组件，物体名字为："+Go.name);
            }
        }
        #region ClickEvents
        //有待验证
        protected void OnClick(Action<object> cb,object args)
        {
            YListener listener = Go.GetOrAddComponent<YListener>();
            listener.OnClick = cb;
            listener.args = args;
        }
        protected void OnClickDown(Action<PointerEventData> cb)
        {
            YListener listener = Go.GetOrAddComponent<YListener>();
            listener.OnClickDown = cb;
        }
        protected void OnClickUp(Action<PointerEventData> cb)
        {
            YListener listener = Go.GetOrAddComponent<YListener>();
            listener.OnClickUp = cb;
        }
        protected void OnClickDrag(Action<PointerEventData> cb)
        {
            YListener listener = Go.GetOrAddComponent<YListener>();
            listener.OnClickDrag = cb;
        }
        #endregion
    }
}