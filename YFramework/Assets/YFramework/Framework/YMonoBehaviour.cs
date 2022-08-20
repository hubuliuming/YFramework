/****************************************************
    文件：YMonoBehaviour.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/11 16:40:53
    功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YFramework
{
    public interface IMono : IInit
    {
    }

    public interface IInit
    {
        void Init();
    }

    public static class Event
    {
        private static Dictionary<int, IMono> _orderForMonoMap = new Dictionary<int, IMono>();

        public static void RegisterEvent(IMono mono, int order)
        {
            if (!_orderForMonoMap.TryAdd(order,mono))
            {
                Debug.LogError("注册时当前顺序已经存在order:" +order);
            }
        }
    }
   
    public abstract class YMonoBehaviour : MonoBehaviour,IMono
    {

        public virtual void Init()
        {
            
        }

     

        #region TimeDelay
        //利用协程实现定时
        public void Delay(float delay, Action onFinished)
        {
            StartCoroutine(DelayCor(delay, onFinished));
        }
        private IEnumerator DelayCor(float delay, Action onFinished = null)
        {
            yield return new WaitForSeconds(delay);
            if (onFinished != null)
            {
                onFinished();
            }
        }
        
        #endregion

        private void OnDestroy()
        {
            MsgDispatcher.UnRegisterAll();
        }

       
    }
}