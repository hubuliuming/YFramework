/****************************************************
    文件：Architecture.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System;
using System.Collections.Generic;

// Base 凉鞋QFramework
namespace YFramework
{
    public interface IArchitecture
    {
        void RegisterModel<T>(T model) where T : IModel;
        void RegisterSystem<T>(T system) where T : ISystem;
        void RegisterUtility<T>(T utility) where T : IUtility;
        T GetModel<T>() where T : class, IModel;
        T GetUtility<T>() where T : class, IUtility ;
        T GetSystem<T>() where T : class, ISystem;
        void SendCommand<T>() where T : ICommand ,new();
        void SendCommand<T>(T command) where T : ICommand;
        void SendEvent<T>() where T : new();
        void SendEvent<T>(T t);
        IUnRegister RegisterEvent<T>(Action<T> onEvent);
        void UnRegisterEvent<T>(Action<T> onEvent);
    }
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        private static T _architecture;
        private bool _inited = false;
        private readonly List<IModel> _models = new List<IModel>();
        private readonly List<ISystem> _systems = new List<ISystem>();
        public static Action<T> OnRegisterPatch = architecture => { };

        public static IArchitecture Interface
        {
            get
            {
                if (_architecture == null)
                {
                    MakeSureArchitecture();
                }

                return _architecture;
            }
        }
        private static void MakeSureArchitecture()
        {
            if (_architecture == null)
            {
                _architecture = new T();
                _architecture.Init();
                OnRegisterPatch?.Invoke(_architecture);
                foreach (var model in _architecture._models)
                {
                    model.Init();
                }
                foreach (var architectureSystem in _architecture._systems)
                {
                    architectureSystem.Init();
                }
                _architecture._models.Clear();
                _architecture._systems.Clear();
                _architecture._inited = true;
            }
        }

        private IOCContainer _container = new IOCContainer();
        protected abstract void Init();

     
        public void RegisterModel<T1>(T1 model) where T1 : IModel
        {
            model.SetArchitecture(this);
            _container.Register<T1>(model);
            if (!_inited)
            {
                _models.Add(model);
            }
            else
            {
                model.Init();
            }
            
        }
        public void RegisterSystem<T1>(T1 system) where T1 : ISystem
        {
            system.SetArchitecture(this);
            _container.Register<T1>(system);
            if (!_inited)
            {
                _systems.Add(system);
            }
            else
            {
                system.Init();
            }
        }
        public void RegisterUtility<T1>(T1 utility) where T1 : IUtility
        {
            _container.Register<T1>(utility);
        }
        public T1 GetModel<T1>() where T1 : class, IModel
        {
            return _container.Get<T1>();
        }
        public T1 GetUtility<T1>() where T1 : class,IUtility
        {
            return _container.Get<T1>();
        }

        public T1 GetSystem<T1>() where T1 : class, ISystem
        {
            return _container.Get<T1>();
        }

        public void SendCommand<T1>() where T1 : ICommand , new()
        {
            var command = new T1();
            command.SetArchitecture(this);
            command.Execute();
        }
        public void SendCommand<T1>(T1 command) where T1 : ICommand
        {
            command.SetArchitecture(this);
            command.Execute();
        }

        private ITypeEventSystem _typeEventSystem = new TypeEventSystem();
        public void SendEvent<T1>() where T1 : new()
        {
            _typeEventSystem.Send<T1>();
        }
        public void SendEvent<T1>(T1 t)
        {
            _typeEventSystem.Send<T1>(t);
        }
        public IUnRegister RegisterEvent<T1>(Action<T1> onEvent)
        {
            return _typeEventSystem.Register(onEvent);
        }
        public void UnRegisterEvent<T1>(Action<T1> onEvent)
        {
            _typeEventSystem.UnRegister(onEvent);
        }
    }
}