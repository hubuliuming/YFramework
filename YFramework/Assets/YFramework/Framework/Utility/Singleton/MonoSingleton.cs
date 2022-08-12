/****************************************************
    文件：MonoSingleton.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Mono类单例基类
*****************************************************/

using System;
using UnityEngine;

namespace YFramework
{
    public class MonoSingleton<T> : YMonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;
        public static T Instance => m_instance;

        private void Awake()
        {
            m_instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}