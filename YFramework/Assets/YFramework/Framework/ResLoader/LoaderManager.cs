/****************************************************
    文件：ManagerLoader.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

namespace YFramework
{
    public class LoaderManager : NormalSingleton<LoaderManager>
    {
        private ILoader m_loader;

        public LoaderManager()
        {
            m_loader = new ResLoader();
        }

        public GameObject LoadGameObject(string path, Transform parent = null) => m_loader.LoadGameObject(path, parent);
    }
}