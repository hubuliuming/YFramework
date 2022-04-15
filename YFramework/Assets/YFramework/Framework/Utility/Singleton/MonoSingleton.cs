/****************************************************
    文件：MonoSingleton.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Mono类单例基类
*****************************************************/

using UnityEngine;

namespace YFramework
{
    public class MonoSingleton<T> : MonoBehaviour where  T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if(_instance == null)
                        Debug.LogError("场景中未找到类的对象，类名为："+typeof(T).Name);
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}