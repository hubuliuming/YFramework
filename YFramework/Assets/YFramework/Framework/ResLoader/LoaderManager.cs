/****************************************************
    文件：ManagerLoader.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：加载器管理
*****************************************************/

using UnityEngine;

namespace YFramework
{
    public class LoaderManager : ILoader
    {
        private ILoader m_loader;
        public LoaderManager(ILoader loader) => m_loader = loader;
        public GameObject LoadGameObject(string path, Transform parent = null) => m_loader.LoadGameObject(path, parent);
        public string LoadConfig(string path) => m_loader.LoadConfig(path);
    }
}