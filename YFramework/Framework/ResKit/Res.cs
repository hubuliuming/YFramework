/****************************************************
    文件：Res.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：负责加载，卸载
*****************************************************/

using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace YFramework
{
    public class Res : SimplePC
    {
        public Object Asset { get; private set; }
        public string Name { get; private set; }
        private string mAssetPath;

        public Res(string assetPath)
        {
            mAssetPath = assetPath;
            Name = mAssetPath;
        }

        public bool LoadSync()
        {
            return Asset = Resources.Load(mAssetPath);
        }

        public void LoadAsync(Action<Res> onLoad)
        {
            var request = Resources.LoadAsync(mAssetPath);
            request.completed += operation =>
            {
                Asset = request.asset;
                onLoad(this);
            };
        }

        protected override void OnZeroRef()
        {
            base.OnZeroRef();
            if (Asset is GameObject)
            {
                Resources.UnloadUnusedAssets();
            }
            else
            {
                Resources.UnloadAsset(Asset);
                ResMgr.Instance.mShareLoadRes.Remove(this);
                Asset = null;
            }
        }
    }
}