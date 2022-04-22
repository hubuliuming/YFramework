/****************************************************
    文件：ABLoader.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：AssetBundles加载器
*****************************************************/

using UnityEngine;

namespace YFramework
{
    //todo 
    public class ABLoader : ILoader 
    {
        public GameObject LoadGameObject(string path, Transform parent = null)
        {
            Debug.LogWarning("未开发AB包加载");
            return null;
        }

        public string LoadConfig(string path)
        {
            Debug.LogWarning("未开发AB包加载");
            return null;
        }

        public T Load<T>(string path) where T : Object
        {
            Debug.LogWarning("未开发AB包加载");
            return null;
        }

        public T[] LoadAll<T>(string path) where T : Object
        {
            Debug.LogWarning("未开发AB包加载");
            return null;
        }
    }
}