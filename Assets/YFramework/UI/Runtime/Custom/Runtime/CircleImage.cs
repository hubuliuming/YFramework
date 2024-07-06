/****************************************************
    文件：CircleImage.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace YFramework.UI
{
    public class CircleImage : Image
    {

        [Tooltip("圆形由多少三角块形成")] 
        [SerializeField]
        private int segments = 100;
        [Tooltip("显示占圆的百分比")] 
        [SerializeField] 
        private float showPercent = 1;
        //所有顶点坐标不包括圆心点
        private List<Vector3> _vertexPos;

        protected int maxSegment = 1000;

        private Vector4 _uv; 
        private Vector3 _originPos;

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            if (showPercent < 0) return;
            if (segments > maxSegment)
            {
                Debug.LogWarning("Segments数值过大，上限为 ：" + maxSegment);
                return;
            }

            _vertexPos = new List<Vector3>();
            toFill.Clear();
            //纠正中心点偏移情况
            _originPos = new Vector3((0.5f - rectTransform.pivot.x) * rectTransform.rect.width, (0.5f - rectTransform.pivot.y) * rectTransform.rect.height);
            _uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;
            AddOriginVert(toFill);
            AddVert(toFill, (int) (segments * showPercent));
            AddTriangle(toFill);

        }

        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out localPoint);
            return IsValid(localPoint);
        }
        //判断有效的方法是在点击点在x轴上向右延申至两个顶点组成的线之间交点的奇偶，奇数为在范围内有效
        private bool IsValid(Vector2 localPoint)
        {
            return GetCrossPointNum(localPoint,_vertexPos) % 2 == 1;
        }

        private int GetCrossPointNum(Vector2 localPoint,List<Vector3> vertexPos)
        {
            int count = 0;
            Vector3 vec1 = Vector3.zero;
            Vector3 vec2 = Vector3.zero;
            for (int i = 0; i < vertexPos.Count; i++)
            {
                vec1 = vertexPos[i];
                vec2 = vertexPos[(i+1)% vertexPos.Count];
                if (IsYInRange(localPoint,vec1,vec2))
                {
                    if (GetInRangeX(localPoint, vec1, vec2) > localPoint.x) count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 判断点击y轴是否在两个顶点之间，之间才有效
        /// </summary>
        /// <returns></returns>
        private bool IsYInRange(Vector2 localPoint, Vector3 vec1, Vector3 vec2)
        {
            if (vec1.y > vec2.y)
            {
                return localPoint.y < vec1.y && localPoint.y > vec2.y;
            }
            else
            {
                return localPoint.y > vec1.y && localPoint.y < vec2.y;
            }
        }

        /// <summary>
        /// 获取点击点y轴在两个顶点之间的x值
        /// </summary>
        /// <returns></returns>
        private float GetInRangeX(Vector2 localPos,Vector3 vec1,Vector3 vec2)
        {
            //直线的斜率
            float k = (vec1.y - vec2.y) / (vec1.x - vec2.x);
            return (localPos.y - vec1.y) / k + vec1.x;
        }

        private void AddOriginVert(VertexHelper vh)
        {
            var width = rectTransform.rect.width;
            var height = rectTransform.rect.height;
            //计算中心点
            float uvWidth = (_uv.z - _uv.x);
            float uvHeight = (_uv.w - _uv.y);
            var uvCenter = new Vector2(uvWidth * (rectTransform.pivot.x), uvHeight * (rectTransform.pivot.y));
            //算一下uv和顶点坐标换算倍率
            var convertRatio = new Vector2(uvWidth / width, uvHeight / height);
       
            //中心原点
            UIVertex origin = new UIVertex();
      
            origin.position = _originPos;
            //百分比颜色渐变，最低灰色，色度60
            byte tempPercent = (byte) (60 + (255 - 60) * showPercent);
            origin.color = new Color32(tempPercent, tempPercent, tempPercent, 255);
            //初始位置有用零点的
            origin.uv0 = new Vector2(origin.position.x * convertRatio.x + uvCenter.x,
                origin.position.y * convertRatio.y + uvCenter.y);
            vh.AddVert(origin);
        }

        private void AddVert(VertexHelper vh,int curSegments)
        {
            var width = rectTransform.rect.width;
            var height = rectTransform.rect.height;
            float uvWidth = (_uv.z - _uv.x);
            float uvHeight = (_uv.w - _uv.y);
            //获取弧度值
            var radian = (2 * Mathf.PI) / segments;
            var uvCenter = new Vector2(uvWidth * (rectTransform.pivot.x), uvHeight * (rectTransform.pivot.y));
            //算一下uv和顶点坐标换算倍率
            var convertRatio = new Vector2(uvWidth / width, uvHeight / height);
            //计算所有顶点
            var curRadian = 0f;
            for (int i = 0; i < segments + 1; i++)
            {
                var x = Mathf.Cos(curRadian) * rectTransform.rect.width/2;
                var y = Mathf.Sin(curRadian) * rectTransform.rect.height/2;
                //+=从坐标逆时针生成顶点，显示的百分比顺序为顺时针
                curRadian += radian;
                //反之可以采用-=
                //curRadian -= radian;
                UIVertex temp = new UIVertex();
                temp.position = _originPos + new Vector3(x, y);
                //todo 最后有点颜色显示错误
                temp.color = i <= curSegments ? color : Color.gray;
                temp.uv0 = new Vector2(temp.position.x * convertRatio.x + uvCenter.x,
                    temp.position.y * convertRatio.y + uvCenter.y);
                vh.AddVert(temp);
                _vertexPos.Add(temp.position);
            }
        }

        private void AddTriangle(VertexHelper vh)
        {
            //生成三角片
            for (int i = 1; i < segments + 1; i++)
            {
                vh.AddTriangle(i, 0, i + 1);
            }
        }
    }
}