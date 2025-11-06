/****************************************************
    文件：SerializableKeyValue.cs
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
    public class SerializableKeyValue<T,V> :ISerializationCallbackReceiver,IEnumerable<KeyValuePair<T,V>>
    {
        [SerializeField]
        public List<T> Keys;
        [SerializeField]
        public List<V> Values;
        
        public int Count => kvs.Count;
        public int Capacity => kvs.Capacity;

    
        private List<KeyValuePair<T,V>> kvs;

        public SerializableKeyValue()
        {
            Keys = new List<T>();
            Values = new List<V>();
            kvs = new List<KeyValuePair<T, V>>();
        }
        public SerializableKeyValue(T t, V v)
        {
            Keys = new List<T>();
            Values = new List<V>();
            kvs = new List<KeyValuePair<T, V>>();
            Add(t, v);
        }
                
        public V this[T t]
        {
            get
            {
                foreach (var kv in kvs)
                {
                    if (kv.Key.Equals(t))
                    {
                        return kv.Value;
                    }
                }
                return default(V);
            }
        }

        public void Add(T t, V v)
        {
            Keys.Add(t);
            Values.Add(v);
            kvs.Add(new KeyValuePair<T, V>(t, v));
        }
                
        public void Remove(T t)
        {
            for (int i = 0; i < kvs.Count; i++)
            {
                if (kvs[i].Key.Equals(t))
                {
                    kvs.RemoveAt(i);
                    break;
                }
            }
        }
                
        public bool ContainsKey(T t)
        {
            foreach (var kv in kvs)
            {
                if (kv.Key.Equals(t))
                {
                    return true;
                }
            }
            return false;
        }
        
        public int IndexOf(T t)
        {
            for (int i = 0; i < kvs.Count; i++)
            {
                if (kvs[i].Key.Equals(t))
                {
                    return i;
                }
            }
            return -1;
        }



        public IEnumerator<KeyValuePair<T, V>> GetEnumerator()
        {
            return kvs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void OnBeforeSerialize()
        {
            Keys.Clear();
            Values.Clear();
            foreach (var kv in kvs)
            {
                Keys.Add(kv.Key);
                Values.Add(kv.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            kvs.Clear();
            for (int i = 0; i < Keys.Count; i++)
            {
                kvs.Add(new KeyValuePair<T, V>(Keys[i], Values[i]));
            }
        }
    }
}