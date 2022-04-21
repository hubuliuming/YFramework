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
        private static T m_instance;

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    var go = new GameObject(typeof(T).Name);
                    DontDestroyOnLoad(go);
                    m_instance = go.AddComponent<T>();
                }

                return m_instance;
            }
        }
    }
}