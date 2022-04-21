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
    public interface ILoader
    {
        GameObject LoadGameObject(string path, Transform parent = null);
        string LoadConfig(string path);
    }
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
    }
}