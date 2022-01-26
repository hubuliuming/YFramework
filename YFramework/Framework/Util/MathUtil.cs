/****************************************************
    文件：MathUtil.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/13 16:49:31
    功能：Math工具类
*****************************************************/

using UnityEngine;

namespace YFramework
{

    public class MathUtil : MonoBehaviour
    {
        public static T GetRandomValueForm<T>(params T[] values)
        {
            return values[Random.Range(0, values.Length)];
        }
    }
}