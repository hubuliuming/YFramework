/****************************************************
    文件：MathUtility.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：数学相关的工具
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

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
        
        /// <summary>
        /// 得到一个从大总集合里面随机不重复小集合
        /// </summary>
        /// <param name="sumList"></param>
        /// <param name="subsetsLength"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetRandomSubsetsInSums<T>(List<T> sumList, int subsetsLength)
        {
            if (subsetsLength >= sumList.Count) return null;
            var temps = new List<T>();
            foreach (var t in sumList) temps.Add(t);
            var values = new List<T>();
            for (int i = 0; i < subsetsLength; i++)
            {
                var t = temps[Random.Range(0, temps.Count)];
                values.Add(t);
                temps.Remove(t);
            }

            return values;
        }
    }
}