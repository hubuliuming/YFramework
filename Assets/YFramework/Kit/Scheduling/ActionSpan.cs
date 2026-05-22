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
    
    /// <summary>
    /// 用ActionKit里调用,常用于减少Update的频率，注意如果是Unity生命周期内的监听执行的方法，则不适合使用
    /// </summary>
    public class ActionFixedUpdate : MonoBehaviour
    {
        private int _fixedSeconds;
        private int _frequency;
        private Action _fixAction;
        private float _tempTime;
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fixedSeconds">固定多少秒</param>
        /// <param name="frequency">固定多少秒内执行次数</param>
        /// <param name="action">需要执行的方法</param>
        public void InitState(int fixedSeconds,int frequency,Action action)
        {
            SetFixed(fixedSeconds);
            SetFrequency(frequency);
            AddFixedAction(action);
        }

        public void SetFixed(int fixedSeconds)
        {
            if (fixedSeconds > 0)
            {
                this._fixedSeconds = fixedSeconds;
            }
        }

        public void SetFrequency(int frequency)
        {
            if (frequency > 0)
            {
                this._frequency = frequency;
            }
        }

        public void AddFixedAction(Action action)
        {
            _fixAction += action;
        }

        public void RemoveFixedAction(Action action)
        {
            _fixAction -= action;
        }
        

        private void Start()
        {
            _tempTime = Time.time;
        }

        private void Update()
        {
            if(_fixedSeconds <= 0 || _frequency <= 0 || _fixAction == null) return;
            float waitTime = (float)_fixedSeconds / _frequency;
            if (Time.time - _tempTime >= waitTime)
            {
                _tempTime = Time.time;
                _fixAction?.Invoke();
            }
        }
    }
}