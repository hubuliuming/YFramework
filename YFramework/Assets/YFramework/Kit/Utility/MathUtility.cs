/****************************************************
    文件：MathUtility.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：数学相关的工具
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace YFramework.Kit.Utility
{
    public static class MathUtility 
    {
        /// <summary>
        /// 获取列表中的除去当前元素的随机元素
        /// </summary>
        public static T GetRandomRemoveSelf<T>(List<T> list,T self)
        {
            if (!list.Contains(self))
            {
                Debug.LogError("this list is not contains the self:" + self);
            }
            List<T> temps = new List<T>();
            foreach (var t in list)
            {
                if (!Equals(t, self))
                {
                    temps.Add(t);
                }
            }
            
            return temps[Random.Range(0, temps.Count)];
        }
    }
}