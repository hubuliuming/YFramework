/****************************************************
    文件：YMonoBehaviour.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/11 16:40:53
    功能：
*****************************************************/

using System;
using System.Collections;
using UnityEngine;
using YFramework.Extension;
using YFramework.Kit.Utility;
using YFramework.Math;

namespace YFramework
{
    public abstract class YMonoBehaviour : MonoBehaviour
    {
        public virtual bool IsActive()
        {
            return isActiveAndEnabled;
        }
        
    }

    /// <summary>
    /// 全局唯一共享MonoBehaviour
    /// </summary>
    public class MonoGlobal : YMonoBehaviour
    {
        private static MonoGlobal _instance;
        public static MonoGlobal Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject(nameof(MonoGlobal));
                    var t = go.AddComponent<MonoGlobal>();
                    _instance = t;
                }
                return _instance;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
    }
}