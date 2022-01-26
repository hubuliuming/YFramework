/****************************************************
    文件：SimpleObjectPool.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/17 16:10:24
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;

namespace YFramework
{
    public interface IPool<T>
    {
        T Allocate();
        bool Recycle(T obj);
    }

    public interface IObjectFactory<T>
    {
        T Create();
    }
    public abstract class Pool<T> : IPool<T>
    {
        protected Stack<T> cacheStack = new Stack<T>();
        protected IObjectFactory<T> factory;

        public int curCount => cacheStack.Count;

        public T Allocate()
        {
            return cacheStack.Count > 0 ? cacheStack.Pop() : factory.Create();
        }

        public abstract bool Recycle(T obj);
    }

    public class CustomObjectFactory<T>: IObjectFactory<T>
    {
        private Func<T> mFactoryMethod;
        public CustomObjectFactory(Func<T> factoryMethod)
        {
            mFactoryMethod = factoryMethod;
        }
        public T Create()
        {
            return mFactoryMethod();
        }
    }
    
    public class SimpleObjectPool<T> : Pool<T>
    {
        private Action<T> mResetMethod;

        public SimpleObjectPool(Func<T> factoryMethod, Action<T> resetMethod = null,int initCount = 0)
        {
            factory = new CustomObjectFactory<T>(factoryMethod);
            mResetMethod = resetMethod;
            for (int i = 0; i < initCount; i++)
            {
                cacheStack.Push(factory.Create());
            }
        }
        public override bool Recycle(T obj)
        {
            if (mResetMethod != null)
            {
                mResetMethod(obj);
            }
            cacheStack.Push(obj);
            return true;
        }
    }
}