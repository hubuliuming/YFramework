/****************************************************
    文件：RadarChartHandler.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YFramework.UI.Kit
{
    public class RadarChartHandler : MonoBehaviour,IDragHandler
    {
        private Image m_image;
        private Image image
        {
            get
            {
                if (m_image == null) m_image = GetComponent<Image>();
                return m_image;
            }
        }

        private RectTransform m_rect;

        private RectTransform rect
        {
            get
            {
                if (m_rect == null) m_rect = GetComponent<RectTransform>();
                return m_rect;
            }
        }

        private Action<PointerEventData> _onDragCB;
        internal void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        internal void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }

        internal void SetColor(Color color)
        {
            image.color = color;
        }

        internal void SetPos(Vector2 pos)
        {
            rect.anchoredPosition = pos;
        }

        internal void SetSize(Vector2 size)
        {
            rect.sizeDelta = size;
        }

        // internal void AddOnDragCBLister(Action<PointerEventData> onDragCB)
        // {
        //     _onDragCB = onDragCB;
        // }
        public void OnDrag(PointerEventData eventData)
        {
            rect.anchoredPosition += eventData.delta / GetScale();
            //_onDragCB.Invoke(eventData);
        }

        private Vector2 GetScale()
        {
            return transform.lossyScale;
        }
    }
}