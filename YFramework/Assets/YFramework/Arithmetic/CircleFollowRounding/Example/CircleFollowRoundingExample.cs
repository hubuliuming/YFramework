/****************************************************
    文件：CircleFollowRoundingExample.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using YFramework.Arithmetic;

namespace YFramework.Examples
{
    public class CircleFollowRoundingExample : MonoBehaviour 
    {
        public Transform[] trans;
        public float spped =3;
        private CircleFollowRounding _rounding;

        private void Start()
        {
            _rounding = new CircleFollowRounding(Vector2.zero, spped);
            foreach (var tran in trans)
            {
                _rounding.Add(tran,2);
            }
        }

        private void Update()
        {
            _rounding.SetSpeed(spped);
            _rounding.OnUpdate();
        }
    }
}