/****************************************************
    文件：ActionKit.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using YFramework.Extension;

namespace YFramework.Kit
{
    public class ActionKit 
    {
        public static void Delay(Action onFinished,float delay)
        {
            
            CorDelay(delay, onFinished).StartCoroutine(MonoGlobal.Instance);
        }
     
        public static void DelayOneFrame(Action callback)
        {
            CorDelayOneFrame(callback).StartCoroutine(MonoGlobal.Instance);
        }

        /// <summary>
        /// 一秒内执行指定次数的方法
        /// </summary>
        /// <param name="frequency">执行的频率</param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ActionFixedUpdate SecondsFixedUpdate(int frequency,Action action)
        {
            if (frequency <= 0 || action == null ) return null;
            var go = new GameObject("FixedUpdate");
            var fixedUpdate = go.AddComponent<ActionFixedUpdate>();
            fixedUpdate.InitState(1,frequency,action);
            return fixedUpdate;
        }
        
  
        private static IEnumerator CorDelay(float delay, Action onFinished = null)
        {
            yield return new WaitForSeconds(delay);
            onFinished?.Invoke();
        }
        private static IEnumerator CorDelayOneFrame(Action callback)
        {
            yield return null;
            callback?.Invoke();
        }
    }
}