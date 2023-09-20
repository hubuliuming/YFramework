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
    public class CircleFollowRounding
    {
        private Vector2 _targetPos;
        private float _speed;
        private float _angle;

        private readonly List<Grid> _trans;



        /// <summary>
        /// 添加需要选择的物体
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="speed">旋转速度</param>
        /// <param name="r">半径</param>
        public void Add(Transform trans, float r)
        {
            var grid = new Grid()
            {
                trans = trans,
                r = r
            };
            this._trans.Add(grid);
        }

        public void SetSpeed(float speed)
        {
            this._speed = speed;
        }
        
        /// <summary>
        /// 实时更新旋转
        /// </summary>
        public void OnUpdate() 
        {
            _angle += Time.deltaTime * _speed;
            if (_angle >= Mathf.PI * (_trans.Count -1)) _angle = 0;
            for (int i = 0; i < _trans.Count; i++)
            {
                float x = Mathf.Cos(_angle + 2 * Mathf.PI / _trans.Count * i) * _trans[i].r;
                float y = Mathf.Sin(_angle + 2 * Mathf.PI / _trans.Count * i) * _trans[i].r;
                _trans[i].trans.position = new Vector2(x, y) + _targetPos;
            }
        }
        
        private struct Grid
        {
            public Transform trans;
            public float r;
        }
        public CircleFollowRounding(Vector2 targetPos,float speed)
        {
            _trans = new List<Grid>();
            this._targetPos = targetPos;
            this._speed = speed;
        }
        ~CircleFollowRounding()
        {
            _trans.Clear();
        }
    }
}