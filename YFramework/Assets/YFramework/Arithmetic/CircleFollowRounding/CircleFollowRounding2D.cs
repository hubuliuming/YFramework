/****************************************************
    文件：CircleFollowRounding.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System.Collections.Generic;
using UnityEngine;

namespace YFramework.Arithmetic
{
    
    /// <summary>
    /// 圆形等距离环绕某个点旋转
    /// </summary>
    public class CircleFollowRounding2D
    {
        private Vector2 _rootPos;
        private float _speed;
        private float _angle;

        private readonly List<Grid> _child;

        private int _index;


        /// <summary>
        /// 添加需要选择的物体
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="speed">旋转速度</param>
        /// <param name="r">半径</param>
        public void AddChild(float r)
        {
            if (_child == null)
            {
                Debug.LogError("先new该类后再添加");
                return;
            }

            _index++;
            var grid = new Grid()
            {
                index = _index,
                r = r
            };
            this._child.Add(grid);
        }

        public void SetSpeed(float speed)
        {
            this._speed = speed;
        }
        
        //可拓展写SetIndexR()
        
        /// <summary>
        /// 实时返回坐标
        /// </summary>
        public Vector2[] OnUpdate() 
        {
            if (_child.Count > 0)
            {
                _angle += Time.deltaTime * _speed;
                if (_angle -  Mathf.PI >= Mathf.PI * _child.Count) _angle = 0;
                Vector2[] pos = new Vector2[_child.Count];
                for (int i = 0; i < _child.Count; i++)
                {
                    float x = Mathf.Cos(_angle + 2 * Mathf.PI / _child.Count * i) * _child[i].r;
                    float y = Mathf.Sin(_angle + 2 * Mathf.PI / _child.Count * i) * _child[i].r;
                    pos[i] = new Vector2(x, y) + _rootPos;
                }

                return pos;
            }
            return null;
        }
        
        private struct Grid
        {
            public int index;
            public float r;
        }
        public CircleFollowRounding2D(Vector2 rootPos, float speed)
        {
            //拓展调整顺逆旋转
            this._child = new List<Grid>();
            this._index = -1;
            this._rootPos = rootPos;
            this._speed = speed;
        }

        ~CircleFollowRounding2D()
        {
            this._child.Clear();
        }
    }
}