/****************************************************
    文件：TimerManager.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine.Pool;
using YFramework.Kit.Singleton;

namespace YFramework.Kit
{
    public class TimerManager : MonoSingleton<TimerManager> 
    {
        #region 成员变量
        private int _curId = 0;
        
        private Dictionary<int, TimerItem> _timerDic = new Dictionary<int, TimerItem>();
        private List<int> _clearId = new List<int>();
        
        private float _timer;
        #endregion
        

        public int Register(TimerItem iterm)
        {
            _timerDic.TryAdd(_curId, iterm);
            iterm.start = true;
            var index = _curId;
            _curId++;
            return index;
        }

        public void UnRegister(int id)
        {
            if (_timerDic.ContainsKey(id))
            {
                StopTimer(id);
                _clearId.Add(id);
            }
        }

        public TimerItem GetItem(int id)
        {
            if (_timerDic.ContainsKey(id))
            {
                return _timerDic[id];
            }
            return null;
        }
        
        public void Pause(int id)
        {
            if (_timerDic.ContainsKey(id))
            {
                _timerDic[id].start = false;
            }
        }

        public void Resume(int id)
        {
            if (_timerDic.ContainsKey(id))
            {
                _timerDic[id].start = true;
            }
        }

        public void ReStart(int id)
        {
            StopTimer(id);
            Resume(id);
        }

        public void StopTimer(int id)
        {
            if (_timerDic.ContainsKey(id))
            {
                _timerDic[id].start = false;
                _timerDic[id].Time = 0;
                _timer = 0;
            }
        }
        

        private void Update()
        {
            foreach (var id in _timerDic.Keys)
            {
                var item = _timerDic[id];
                if (!item.start) continue;
                item.Time += UnityEngine.Time.deltaTime;
                if (item.interval > 0)
                {
                    _timer += UnityEngine.Time.deltaTime;
                    if (_timer >= item.interval)
                    {
                        _timer = 0;
                        item.OnUpdate?.Invoke(item.Time);
                    }
                }
                else
                {
                    item.OnUpdate?.Invoke(item.Time);
                }
                if (item.endTime > 0)
                {
                    if (item.Time >= item.endTime)
                    {
                        item.onEnd?.Invoke();
                        UnRegister(id);
                        continue;
                    }
                }
            }

            foreach (var id in _clearId)
            {
                _timerDic.Remove(id);
            }
            _clearId.Clear();
        }
    }
    

    public class TimerItem
    {
        public float Time { get; internal set; }
        public Action<float> OnUpdate;
        internal Action onEnd;
        internal float endTime;
        internal bool start;
        internal float interval;
        internal float timer;

        public TimerItem(float interval = 0, Action<float> onUpdate = null, Action onEnd = null)
        {
            this.interval = interval;
            this.OnUpdate = onUpdate;
            this.onEnd = onEnd;
        }

        public void AddEndTask(float endTime,Action onEnd)
        {
            this.endTime = endTime;
            this.onEnd = onEnd;
        }

        public void RemoveEndTask()
        {
            onEnd = null;
        }
    }
}