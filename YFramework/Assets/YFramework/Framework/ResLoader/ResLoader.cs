/****************************************************
    文件：ResLoader.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：资源加载器
*****************************************************/

using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace YFramework
{
    public class ResLoader : ILoader 
    {
        public GameObject LoadGameObject(string path, Transform parent = null)
        {
            var prefab = Resources.Load<GameObject>(path);
            var go = Object.Instantiate(prefab, parent);
            return go;
        }
        
        public string LoadConfig(string path)
        {
            return File.ReadAllText(path);
        }

        public T Load<T>(string path) where T : Object
        {
            T res = Resources.Load<T>(path);
            if (res != null) return res;
            Debug.LogError("未找到对应类型："+typeof(T).Name+"的资源，路径："+path);
            return null;
        }

        public T[] LoadAll<T>(string path) where T : Object
        {
            T[] res = Resources.LoadAll<T>(path);
            if (res != null && res.Length > 0) return res;
            Debug.LogError("未找到对应类型："+typeof(T).Name+"的资源，路径："+path);
            return null;
        }
    }
}