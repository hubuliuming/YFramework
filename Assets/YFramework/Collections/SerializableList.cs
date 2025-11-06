/****************************************************
    文件：SerializableList.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YFramework.Collections
{
    [Serializable]
    public class SerializableList<T> : IEnumerable<T>
    {
        [SerializeField] private List<T> list;

        public List<T> List => list;
        public SerializableList()
        {
            list = new List<T>();
        }
        public SerializableList(List<T> list)
        {
            this.list = list;
        }
        public SerializableList(int capacity)
        {
            list = new List<T>(capacity);
        }
        public int Capacity
        {
            get => list.Capacity;
            set => list.Capacity = value;
        }
        
        public int Count => list.Count;
     

        public void Add(T item) => list.Add(item);
        
        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public void Remove(T item) => list.Remove(item);
        public bool Contains(T item) => list.Contains(item);
        public void Clear() => list.Clear();
        

        public T this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}