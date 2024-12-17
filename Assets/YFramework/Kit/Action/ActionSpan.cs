/****************************************************
    文件：ActionSpan.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections;
using UnityEngine;

namespace YFramework.Kit
{
    public class ActionSpan : MonoBehaviour 
    {
        
    }
    
    //todo 先放这里,先实现Seconds
    public class FixedUpdate : MonoBehaviour
    {
        public TimeType type;

        public int frequency;
        public Action Action;

        public void Init(TimeType type,int frequency,Action action)
        {
            this.type = type;
            this.frequency = frequency;
            this.Action = action;
        }

        private void Start()
        {
            StartCoroutine(ExecuteEveryFrame());
        }
            
        IEnumerator ExecuteEveryFrame()
        {
            yield return null;
            while (true)
            {
                float waitTime = 1.0f / frequency;
                Action.Invoke();
                yield return new WaitForSeconds(waitTime);
            }
        }
            
        public enum TimeType
        {
            Seconds,
            Minutes,
            Hours,
        }
    }
}