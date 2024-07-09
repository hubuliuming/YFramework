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
        #region TimeDelay
        //利用协程实现定时
        public void Delay(Action onFinished,float delay)
        {
            CorDelay(delay, onFinished).StartCoroutine(this);
        }
     
        public void DelayOneFrame(Action callback)
        {
            CorDelayOneFrame(callback).StartCoroutine(this);
        }
  
        private IEnumerator CorDelay(float delay, Action onFinished = null)
        {
            yield return new WaitForSeconds(delay);
            onFinished?.Invoke();
        }
        private IEnumerator CorDelayOneFrame(Action callback)
        {
            yield return null;
            callback?.Invoke();
        }
        
        #endregion
        
        public T GetOrAddComponent<T>() where T: Component
        {
            T t = gameObject.GetComponent<T>();
            if (t == null)
            {
                t = gameObject.AddComponent<T>();
            }

            return t;
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