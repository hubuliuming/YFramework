/****************************************************
    文件：IOCContair.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System;
using System.Collections.Generic;

namespace YFramework
{
    public class IOCContainer
    {
        private Dictionary<Type, object> _instanceDict = new Dictionary<Type, object>();

        public void Register<T>(T instance)
        {
            var key = typeof(T);
            if (_instanceDict.ContainsKey(key))
            {
                _instanceDict[key] = instance;
            }
            else
            {
                _instanceDict.Add(key,instance);
            }
        }

        public T Get<T>() where T : class
        {
            var key = typeof(T);
            if (_instanceDict.TryGetValue(key,out var retInstance))
            {
                return retInstance as T;
            }

            return null;
        }
    }
}