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
using YFrameworkOld;

namespace YFramework.Kit
{
    public abstract class YMonoBehaviour : MonoBehaviour
    {
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