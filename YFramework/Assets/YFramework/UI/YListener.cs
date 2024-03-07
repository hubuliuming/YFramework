/****************************************************
    文件：PEListener.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2021/11/23 16:36:9
    功能：UI事件监听
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace YFramework.Kit.UI
{
    public class YListener : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler,IDragHandler
    {
        public Action<object> OnClick;
        public Action<PointerEventData> OnClickDown;
        public Action<PointerEventData> OnClickUp;
        public Action<PointerEventData> OnClickDrag;
        public object args;
    
        public void OnPointerClick(PointerEventData eventData)
        {
            if (OnClick != null)
            {
                OnClick(args);
            }
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (OnClickDown != null)
            {
                OnClickDown(eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (OnClickUp != null)
            {
                OnClickUp(eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (OnClickDrag != null)
            {
                OnClickDrag(eventData);
            }
        }
    }
}