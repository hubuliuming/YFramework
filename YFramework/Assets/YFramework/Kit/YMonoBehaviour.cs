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

namespace YFramework
{
    public abstract class YMonoBehaviour : MonoBehaviour
    {

        #region TimeDelay
        //利用协程实现定时
        public void Delay(float delay, Action onFinished)
        {
            StartCoroutine(CorDelay(delay, onFinished));
        }
        private IEnumerator CorDelay(float delay, Action onFinished = null)
        {
            yield return new WaitForSeconds(delay);
            onFinished?.Invoke();
        }

        public void DelayOneFrame(Action callback)
        {
            StartCoroutine(CorDelayOneFrame(callback));
        }

        private IEnumerator CorDelayOneFrame(Action callback)
        {
            yield return null;
            callback?.Invoke();
        }
        
        #endregion

        protected virtual void OnDestroy()
        {
            Kit.MsgDispatcher.UnRegisterAll();
        }
    }

    public class YMonoManager : YMonoBehaviour
    {
        private static YMonoManager _instance;
        public static YMonoManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject(nameof(YMonoManager));
                    var t = go.AddComponent<YMonoManager>();
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