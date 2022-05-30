/****************************************************
    文件：ManagerLoader.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：加载器管理
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace YFramework
{
    public interface ILoader
    {
        GameObject LoadGameObject(string path, Transform parent = null);
        string LoadConfig(string path);
        T Load<T>(string path) where T : Object;
        T[] LoadAll<T>(string path) where T : Object;
    }

    public enum LoaderType
    {
        ResLoader,
        ABLoader
    }
    public class LoaderManager : NormalSingleton<LoaderManager>
    {
        private readonly Dictionary<LoaderType, ILoader> m_loaders;
        public LoaderManager()
        {
            m_loaders = new Dictionary<LoaderType, ILoader>()
            {
                {LoaderType.ResLoader, new ResLoader()},
                {LoaderType.ABLoader, new ABLoader()}
            };
        }
        public GameObject LoadGameObject(string path,
            Transform parent = null,
            LoaderType type = LoaderType.ResLoader) 
            => m_loaders[type].LoadGameObject(path, parent);
        public string LoadConfig(string path,LoaderType type = LoaderType.ResLoader) => m_loaders[type].LoadConfig(path);
        
        [Obsolete("已弃用，请使用带前缀名字的Load方法")]
        public T Load<T>(string path,LoaderType type = LoaderType.ResLoader) where T : Object => m_loaders[type].Load<T>(path);
        [Obsolete("已弃用，请使用带前缀名字的Load方法")]
        public T[] LoadAll<T>(string path,LoaderType type = LoaderType.ResLoader) where T : Object => m_loaders[type].LoadAll<T>(path);
        public T ResLoad<T>(string path) where T : Object => m_loaders[LoaderType.ResLoader].Load<T>(path);
        public T ABLoad<T>(string path) where T : Object => m_loaders[LoaderType.ABLoader].Load<T>(path);
        public T[] ResLoadAll<T>(string path) where T : Object => m_loaders[LoaderType.ResLoader].LoadAll<T>(path);
        public T[] ABLoadAll<T>(string path) where T : Object => m_loaders[LoaderType.ABLoader].LoadAll<T>(path);
    }
}