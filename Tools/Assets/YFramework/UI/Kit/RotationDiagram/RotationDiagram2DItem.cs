/****************************************************
    文件：RotationDiagram2DGrid.cs
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
    public class RotationDiagram2DItem : UIBase,IDragHandler,IEndDragHandler
    {
        internal int posId;

        private float _offsetX;
        private float _animTime = 1;
        private Action<float> _moveCB;

        private Image m_image;

        private Image image
        {
            get
            {
                if (m_image == null) m_image = GetComponent<Image>();
                return m_image;
            }
        }
        
        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }

        public void SetItemData(ItemPosData data)
        {
            //需要DOTween插件
            // DOTween.To( (() => rect.anchoredPosition),  (x => rect.anchoredPosition = x), Vector2.right * data.X, _animTime);
            // rect.DOScale(Vector3.one * data.ScaleTimes, _animTime);
            // MonoManager.Instance.Delay(()=>transform.SetSiblingIndex(data.Order),_animTime);
            //下面是不开启动画的
            rectTransform.anchoredPosition = Vector2.right * data.X;
            rectTransform.localScale = Vector3.one * data.ScaleTimes;
            transform.SetSiblingIndex(data.Order);
        }
        public void OnDrag(PointerEventData eventData)
        {
            _offsetX += eventData.delta.x;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            _moveCB(_offsetX);
            _offsetX = 0;
        }

        public void AddMoveListener(Action<float> moveCB)
        {
            _moveCB = moveCB;
        }

        public void SetPosID(int symbol, int totalNum)
        {
            int id = posId;
            id += symbol;
            if (id < 0)
            {
                id += totalNum;
            }
            //超过总数情况下用求余的方法
            posId = id % totalNum;
        }

    }
}