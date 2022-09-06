/****************************************************
    文件：TypeEventSystemExample.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using CounterApp;
using UnityEngine;

namespace YFramework.Examples
{
    public class EventTest : Architecture<EventTest>
    {
        protected override void Init()
        {
          
        }

    }
    public class TypeEventSystemExample : MonoBehaviour 
    {
        public struct EventA
        {
            
        }

        public struct EventB
        {
            public int paramB;
        }

        public interface IEventGroup
        {
            
        }

        public struct EventC : IEventGroup
        {
            
        }
        public struct EventD : IEventGroup
        {
            
        }

        private TypeEventSystem _typeEventSystem = new TypeEventSystem();
        private void Start()
        {
            _typeEventSystem.Register<EventA>(onEvent => { Debug.Log("EventA"); });
            _typeEventSystem.Register<EventB>(onEvent =>
                {
                    Debug.Log("EventB + Param:"+onEvent.paramB);
                })
                .UnRegisterWhenGameObjectDestroy(gameObject);
            _typeEventSystem.Register<IEventGroup>(e =>
            {
                Debug.Log(e.GetType());
            }).UnRegisterWhenGameObjectDestroy(gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _typeEventSystem.Send<EventA>();
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                _typeEventSystem.Send(new EventB()
                {
                    paramB = 123
                });
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _typeEventSystem.Send<IEventGroup>(new EventC());
                _typeEventSystem.Send<IEventGroup>(new EventD());
            }
        }
    }
}