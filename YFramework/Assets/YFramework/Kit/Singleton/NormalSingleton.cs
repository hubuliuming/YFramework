/****************************************************
    文件：NormalSingleton.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：普通类的单例基类
*****************************************************/


using UnityEngine;

namespace YFramework.Kit.Singleton
{
    public class NormalSingleton<T> where T : class,new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T t = new T();
                    if (t is MonoBehaviour)
                    {
                        Debug.LogError("Mono类请使用MonoSingleton");
                    }

                    _instance = t;
                }
                return _instance;
            }
        }
    }
}