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
        public float spped =3;
        private CircleFollowRounding2D _rounding2D;

        private void Start()
        {
            _rounding2D = new CircleFollowRounding2D(Vector2.zero, 3);
            
        }

        private void Update()
        {
           _rounding2D.SetSpeed(spped);
            var pos = _rounding2D.OnUpdate();
            //不建议使用trans来遍历，建议使用pos得到的坐标来遍历
            // for (int i = 0; i < transform.childCount; i++)
            // {
            //     transform.GetChild(i).position = pos[i];
            // }
            if (pos !=null)
            {
                for (int i = 0; i < pos.Length; i++)
                {
                    transform.GetChild(i).position = pos[i];
                }
            }
            

            if (Input.GetKeyDown(KeyCode.P))
            {
                _rounding2D.AddChild(3);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                _rounding2D.AddChild(5);
            }
        }
    }
}