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
        public static T GetRandomRemoveSelf<T>(List<T> list, T self)
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
            int minDiff = System.Math.Abs(x - arr[0]);
            //使用二分法查找最接近x的值
            while (left <= right)
            {
                //计算中间位置
                int mid = (left + right) / 2;
                //计算中间位置的值与x的差值
                int diff = System.Math.Abs(x - arr[mid]);
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

        /// <summary>
        /// 查找x在a数组中中间的两个值，example x=3 ,a ={1,2,4,5} return 2,4
        /// </summary>
        /// <param name="a"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Tuple<int, int> FindTwoNumbersNearMiddle(List<int> a, int x)
        {
            a.Sort();
            int left = 0, right = a.Count - 1;

            // 使用二分查找定位 x 所在的两个数值之间的位置
            while (left <= right)
            {
                int mid = (left + right) / 2;
                if (a[mid] == x)
                    return Tuple.Create(a[mid], a[mid]);
                else if (a[mid] < x)
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            // 返回 x 所在的两个数值之间的位置
            int smaller = right >= 0 ? a[right] : int.MinValue;
            int larger = left < a.Count ? a[left] : int.MaxValue;

            return Tuple.Create(smaller, larger);
        }

        /// <summary>
        /// 查找x在a数组中中间的两个值，example x=3 ,a ={1,2,4,5} return 2,4
        /// </summary>
        /// <param name="a"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Tuple<float, float> FindTwoNumbersNearMiddle(List<float> a, float x)
        {
            a.Sort();
            int left = 0, right = a.Count - 1;

            // 使用二分查找定位 x 所在的两个数值之间的位置
            while (left <= right)
            {
                int mid = (left + right) / 2;
                if (a[mid].Equals(x))
                    return Tuple.Create(a[mid], a[mid]);
                else if (a[mid] < x)
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            // 返回 x 所在的两个数值之间的位置
            float smaller = right >= 0 ? a[right] : float.MinValue;
            float larger = left < a.Count ? a[left] : float.MaxValue;

            return Tuple.Create(smaller, larger);
        }
        
        /// <summary>
        /// 从Vector2抽离一类值
        /// </summary>
        /// <param name="v3s"></param>
        /// <param name="a">1为x,2为y</param>
        /// <returns></returns>
        public static List<float> ExtractForVector2(List<Vector2> v3s, int a)
        {
            List<float> temp = new List<float>(v3s.Count);
            if (a == 1)
            {
                foreach (var v3 in v3s)
                {
                    temp.Add(v3.x);
                }
            }
            else if (a == 2)
            {
                foreach (var v3 in v3s)
                {
                    temp.Add(v3.y);
                }
            }
            else
            {
                return null;
            }

            return temp;
        }
        
        /// <summary>
        /// 从Vector3抽离一类值
        /// </summary>
        /// <param name="v3s"></param>
        /// <param name="a">1为x,2为y,3为z</param>
        /// <returns></returns>
        public static List<float> ExtractForVector3(List<Vector3> v3s, int a)
        {
            List<float> temp = new List<float>(v3s.Count);
            if (a == 1)
            {
                foreach (var v3 in v3s)
                {
                   temp.Add(v3.x);
                }
            }
            else if (a == 2)
            {
                foreach (var v3 in v3s)
                {
                    temp.Add(v3.y);
                }
            }
            else if (a == 3)
            {
                foreach (var v3 in v3s)
                {
                    temp.Add(v3.z);
                }
            }
            else
            {
                return null;
            }
            return temp;
        }
    }
}