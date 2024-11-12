/****************************************************
    文件：MonoSingleton.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Mono类单例基类
*****************************************************/

using UnityEngine;

namespace YFramework.Kit.Singleton
{
    public  class MonoSingleton<T> : YMonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                }
                if (_instance == null)
                {
                    var go = new GameObject(typeof(T).Name);
                    var t = go.AddComponent<T>();
                    _instance = t;
                }
                return _instance;
            }
        }

        public virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (_instance == null) _instance = this as T;
        }
    }
}