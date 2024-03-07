/****************************************************
    文件：CustomImage.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace YFramework.UI
{
    public class CustomImage : Image
    {
        private PolygonCollider2D _polygonCollider;

        private PolygonCollider2D PolygonCollider
        {
            get
            {
                if (_polygonCollider == null) _polygonCollider = GetComponent<PolygonCollider2D>();
                return _polygonCollider;
            }
        }
        

        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            Vector3 point;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPoint, eventCamera, out point);
            return PolygonCollider.OverlapPoint(point);
        }
    }
}