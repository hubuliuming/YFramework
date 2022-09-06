/****************************************************
    文件：TypeEventSystem.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace YFramework
{
    public interface ITypeEventSystem
    {
        void Send<T>() where T : new();
        void Send<T>(T t);
        IUnRegister Register<T>(Action<T> onEvent);
        void UnRegister<T>(Action<T> onEvent);
    }

    public interface IUnRegister
    {
        void UnRegister();
    }

    public struct TypeEventSystemUnRegister<T> : IUnRegister
    {
        public ITypeEventSystem TypeEventSystem;
        public Action<T> OnEvent;
        public void UnRegister()
        {
            TypeEventSystem.UnRegister<T>(OnEvent);
            TypeEventSystem = null;
            OnEvent = null;
        }
    }

    /// <summary>
    /// 把这脚本挂在游戏物体上，物体销毁会自动注销事件
    /// </summary>
    public class UnRegisterOnDestroyTrigger : MonoBehaviour
    {
        private HashSet<IUnRegister> _unRegisters = new HashSet<IUnRegister>();

        public void AddUnRegister(IUnRegister unRegister)
        {
            _unRegisters.Add(unRegister);
        }

        private void OnDestroy()
        {
            foreach (var unRegister in _unRegisters)
            {
                unRegister.UnRegister();
            }
            _unRegisters.Clear();
        }
    }
    public static class UnRegisterExtensive
    {
        public static void UnRegisterWhenGameObjectDestroy(this IUnRegister self ,GameObject go)
        {
            var trigger = go.GetComponent<UnRegisterOnDestroyTrigger>();
            if (trigger == null)
            {
                trigger = go.AddComponent<UnRegisterOnDestroyTrigger>();
                
            }
            trigger.AddUnRegister(self);
        }
    }
    public class TypeEventSystem : ITypeEventSystem
    {
        public interface IRegistrations
        {
            
        }
        public class Registrations<T> : IRegistrations
        {
            public Action<T> OnEvent = e => { };
        }
        private Dictionary<Type, IRegistrations> _EventRegistrationsMap = new Dictionary<Type, IRegistrations>();
        public void Send<T>() where T : new()
        {
            var t = new T();
            Send<T>(t);
        }

        public void Send<T>(T t)
        {
            var type = typeof(T);
            IRegistrations registrations;
            if (_EventRegistrationsMap.TryGetValue(type,out registrations))
            {
                (registrations as Registrations<T>).OnEvent(t);
            }
        }

        public IUnRegister Register<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations registrations;
            if (!_EventRegistrationsMap.TryGetValue(type,out registrations))
            {
                registrations = new Registrations<T>();
                _EventRegistrationsMap.Add(type,registrations);
            }

            (registrations as Registrations<T>).OnEvent += onEvent;
            return new TypeEventSystemUnRegister<T>()
            {
                OnEvent = onEvent,
                TypeEventSystem = this
            };
        }
        public void UnRegister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations registrations;
            if (_EventRegistrationsMap.TryGetValue(type,out registrations))
            {
                (registrations as Registrations<T>).OnEvent -= onEvent;
            }
        }
    }
}