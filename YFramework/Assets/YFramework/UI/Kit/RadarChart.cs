/****************************************************
    文件：RardChart.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace YFramework.UI.Kit
{
    /// <summary>
    /// 雷达图
    /// </summary>
    public class RadarChart : Image
    {
        [SerializeField]
        private int _pointCount;
        [SerializeField]
        private List<RectTransform> _points;
        [SerializeField] 
        private Vector2 _pointSize = new Vector2(10, 10);
        [SerializeField]
        private Sprite _pointSprite;
        [SerializeField]
        private Color _pointColor = Color.white;
        [SerializeField]
        private float[] _handlerRadio;
        [SerializeField]
        private List<RadarChartHandler> _handlers;
        public void InitPoint()
        {
            ClearPoints();
            _points = new List<RectTransform>();
            SpawnPoint();
            SetPointPos();
        }

        public void InitHandle()
        {
            _handlers = new List<RadarChartHandler>();
            SpawnPointHandler();
            SetHandlerPos();
        }

        private void Update()
        {
            //这里性能开销比较大
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            if (_handlers == null || _handlers.Count == 0)
            {
                base.OnPopulateMesh(toFill);
                return;
            }
            toFill.Clear();
            AddVerts(toFill);
            // AddVertsTemplate(toFill);
            AddTriangle(toFill);
        }

          //对贴图没有特殊要求uv设置为0
        private void AddVerts(VertexHelper toFill)
        {
            //生成方式改成从中心设定一个原点
            toFill.AddVert(Vector3.zero,color,Vector4.zero);
            foreach (var handler in _handlers)
            {
                toFill.AddVert(handler.transform.localPosition,color,Vector4.zero);
            }
        }

        //五边形顶点uv拉伸的模板
        private void AddVertsTemplate(VertexHelper toFill)
        {
            toFill.AddVert(_handlers[0].transform.localPosition,color,new Vector2(0.5f,1));
            toFill.AddVert(_handlers[1].transform.localPosition,color,new Vector2(0f,1));
            toFill.AddVert(_handlers[2].transform.localPosition,color,new Vector2(0f,0));
            toFill.AddVert(_handlers[3].transform.localPosition,color,new Vector2(1f,0));
            toFill.AddVert(_handlers[4].transform.localPosition,color,new Vector2(1f,1));
        }

        private void AddTriangle(VertexHelper toFill)
        {
            for (int i = 1; i < _pointCount + 1; i++)
            {
                toFill.AddTriangle(0,i % _pointCount + 1, i);
            }
        }

        private void SpawnPoint()
        {
            for (int i = 0; i < _pointCount; i++)
            {
                GameObject point = new GameObject("Point_" + i);
                _points.Add(point.AddComponent<RectTransform>());
                point.transform.SetParent(transform);
            }
        }

        private void SetPointPos()
        {
            var radian = 2 * Mathf.PI / _pointCount;
            var radius = 100f;
            var curRadian = 2 * Mathf.PI / 4;
            for (int i = 0; i < _pointCount; i++)
            {
                var x = Mathf.Cos(curRadian) * radius;
                var y = Mathf.Sin(curRadian) * radius;
                curRadian += radian;
                _points[i].anchoredPosition = new Vector2(x, y);
            }
        }

        private void SpawnPointHandler()
        {
            RadarChartHandler handler = null;
            for (int i = 0; i < _pointCount; i++)
            {
                GameObject point = new GameObject("Handle_" + i);
                point.AddComponent<RectTransform>();
                point.AddComponent<Image>();
                handler = point.AddComponent<RadarChartHandler>();
                handler.SetParent(transform);
                handler.SetColor(_pointColor);
                handler.SetSprite(_pointSprite);
                handler.SetSize(_pointSize);
                //handler.AddOnDragCBLister(data =>{ SetVerticesDirty();});
                _handlers.Add(handler);
            }
        }

        private void ClearPoints()
        {
            if(_points == null) return;
            foreach (var point in _points)
            {
                if(point != null) DestroyImmediate(point.gameObject);
            }
        }

        private void SetHandlerPos()
        {
            if (_handlerRadio == null || _handlerRadio.Length != _pointCount)
            {
                for (int i = 0; i < _pointCount; i++)
                {
                    _handlers[i].SetPos(_points[i].anchoredPosition);
                }
            }
            else
            {
                for (int i = 0; i < _pointCount; i++)
                {
                    _handlers[i].SetPos(_points[i].anchoredPosition*_handlerRadio[i]);
                }
            }
        }
    }
}