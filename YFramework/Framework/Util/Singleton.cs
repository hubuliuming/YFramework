/****************************************************
    文件：Singleton.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/18 15:57:23
    功能：Nothing
*****************************************************/

using System;
using System.Reflection;

namespace YFramework
{
    public class Singleton<T> where T: Singleton<T>
    {
        private static T mInstance;

        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    var cors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    var cor = Array.Find(cors, c => c.GetParameters().Length == 0);
                    if (cor == null)
                    {
                        throw new Exception("Non-Public ctor() not found!");
                    }

                    mInstance = cor.Invoke(null) as T;
                }

                return mInstance;
            }
        }
        
        protected Singleton(){}
    }
}