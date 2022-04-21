/****************************************************
    文件：PlayerPrefsMemory.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace YFramework
{
    public class PlayerPrefsMemory : IDataMemory
    {
        private Dictionary<Type, Func<string, object>> m_dataGetter = new Dictionary<Type, Func<string, object>>()
        {
            {typeof(int), (key) => PlayerPrefs.GetInt(key, -1)},
            {typeof(float), (key) => PlayerPrefs.GetFloat(key, -1f)},
            {typeof(string), (key) => PlayerPrefs.GetString(key, "")},
        };
        private Dictionary<Type, Action<string, object>> m_dataSetter = new Dictionary<Type, Action<string, object>>()
        {
            {typeof(int), (key,data) =>  PlayerPrefs.SetInt(key, (int)data)},
            {typeof(float), (key,data) => PlayerPrefs.SetFloat(key, (float)data)},
            {typeof(string), (key,data) => PlayerPrefs.SetString(key, (string)data)},
        };
        public T Get<T>(string key)
        {
            Type type = typeof(T);
            var converter = TypeDescriptor.GetConverter(type);
            if (m_dataGetter.ContainsKey(type))
            {
                return (T)converter.ConvertTo(m_dataGetter[type](key),type);
            }
            else
            {
                Debug.LogError("当前数据储存中无此类型数据，类型名字:"+typeof(T).Name);
                return default(T);
            }
        }
        public void Set<T>(string key, T value)
        {
            Type type = typeof(T);
            if (m_dataSetter.ContainsKey(type))
            {
                m_dataSetter[type](key, value);
            }
            else
            {
                Debug.LogError("当前数据储存中无此类型数据，数据未key:"+key+",value:"+value);
            }
        }
        public void Clear(string key) => PlayerPrefs.DeleteKey(key);
        public void ClearAll() => PlayerPrefs.DeleteAll();
    }
}