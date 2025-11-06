/****************************************************
    文件：SerializableDictionary.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using System;
using System.Collections.Generic;

namespace YFramework.Collections
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();

        [SerializeField] private List<TValue> values = new List<TValue>();

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            HashSet<TKey> seenKeys = new HashSet<TKey>();

            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                if (!seenKeys.Contains(pair.Key))
                {
                    seenKeys.Add(pair.Key);
                    keys.Add(pair.Key);
                    values.Add(pair.Value);
                }
            }
        }

        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count)
            {
                int minCount = Mathf.Min(keys.Count, values.Count);
                
                for (int i = 0; i < minCount; i++)
                {
                    TryAddKeyValue(keys[i], values[i]);
                }

                return;
            }

            for (int i = 0; i < keys.Count; i++)
            {
                TryAddKeyValue(keys[i], values[i]);
            }
        }

        private void TryAddKeyValue(TKey key, TValue value)
        {
            if (key == null)
            {
                return;
            }

            if (this.ContainsKey(key))
            {
                return;
            }

            this.Add(key, value);
        }
    }
}