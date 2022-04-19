/****************************************************
    文件：JsonReader.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;

namespace YFramework
{
    public class JsonData
    {
        
    }
    public class JsonReader : MonoBehaviour,IReader
    {
        private object m_data;
        private object m_tempData;
        public IReader this[int key]
        {
            get
            {
                //m_tempData = m_tempData[key];
                return this;
            }
        }

        public IReader this[string key] => throw new NotImplementedException();

        public void Get<T>(Action<T> callback)
        {
            throw new NotImplementedException();
        }

        public void SetDate(object data)
        {
            if (data is string)
            {
               m_data = JsonUtility.FromJson<object>(data as string);
            }
            else
            {
                Debug.LogError("当前传入数据类型错误，当前只能解析json");
            }
        }
    }
}