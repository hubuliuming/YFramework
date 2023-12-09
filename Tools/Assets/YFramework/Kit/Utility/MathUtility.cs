/****************************************************
    文件：MathUtility.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：数学相关的工具
*****************************************************/

using System;
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


        /// <summary>
        /// 定义一个方法，输入一个整数x和一个整数数组arr，返回数组中最接近x的值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="arr"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int FindClosest(int x, int[] arr)
        {
            //如果数组为空，抛出异常
            if (arr == null || arr.Length == 0)
            {
                throw new ArgumentException("数组不能为空");
            }

            //如果数组只有一个元素，直接返回该元素
            if (arr.Length == 1)
            {
                return arr[0];
            }

            //定义两个指针，分别指向数组的头和尾
            int left = 0;
            int right = arr.Length - 1;
            //定义一个变量，存储最接近x的值
            int closest = arr[0];
            //定义一个变量，存储最小的差值
            int minDiff = Math.Abs(x - arr[0]);
            //使用二分法查找最接近x的值
            while (left <= right)
            {
                //计算中间位置
                int mid = (left + right) / 2;
                //计算中间位置的值与x的差值
                int diff = Math.Abs(x - arr[mid]);
                //如果差值为0，说明找到了x，直接返回x
                if (diff == 0)
                {
                    return x;
                }

                //如果差值小于最小差值，更新最接近x的值和最小差值
                if (diff < minDiff)
                {
                    closest = arr[mid];
                    minDiff = diff;
                }

                //根据中间位置的值与x的大小关系，调整左右指针的位置
                if (arr[mid] < x)
                {
                    left = mid + 1;
                }
                else
                {
                    right = mid - 1;
                }
            }

            //返回最接近x的值
            return closest;
        }

        //从中心点生成一圈扩散子弹
        // public static void CrateBullet(Vector2 orginPos,int num)
        // {
        // todo 完善
        //     for (int i = 0; i < 20; i++)
        //     {
        //         //var go =  Object.Instantiate(_bullet.gameObject,OtherBoxControl.Control.transform);
        //         go.transform.position = Actor.transform.position;
        //         go.SetActive(true);
        //         var g = 360f / 10 * i;
        //         var q = Quaternion.Euler(new Vector3(0, 0, g));
        //         Vector2 dir = q * Vector2.right;
        //         Debug.Log(dir);
        //         go.GetComponent<AttackControl>().RB.velocity = dir * 5;
        //     }
        // }
    }
}