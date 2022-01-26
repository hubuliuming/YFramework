/****************************************************
    文件：ResLoader.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：负责记录目标脚本已经加载的资源
*****************************************************/

using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace YFramework
{
    
    public class ResLoader
    {
        private List<Res> mResRecord = new List<Res>();
        
        public T LoadSync<T>(string assetName) where T : Object
        {
            //查询当前的资源记录
            var res = GetResFromRecord(assetName);
            if (res != null)
            {
                return res.Asset as T;
            }

            //查找已经有的全局资源
            res = GetResFromResMgr(assetName);
            if (res != null)
            {
              AddRes2Record(res);
                return res.Asset as T;
            }
            //真正加载资源
            res = new Res(assetName);
            res.LoadSync();
            res.Retain();
            ResMgr.Instance.mShareLoadRes.Add(res);
            mResRecord.Add(res);
            
            return res.Asset as T;
        }

        public void LoadAsync<T>(string assetName, Action<T> onLoad) where T : Object
        {
            //查询当前的资源记录
            var res = GetResFromRecord(assetName);
            if (res != null)
            {
                onLoad(res.Asset as T);
                return;
            }

            //查找已经有的全局资源
            res = GetResFromResMgr(assetName);
            if (res != null)
            {
                AddRes2Record(res);
                onLoad(res.Asset as T);
                return;
            }
            //真正加载资源
            res = new Res(assetName);
            res.LoadAsync(loadAsset =>
            {
                res.Retain();
                ResMgr.Instance.mShareLoadRes.Add(res);
                mResRecord.Add(res);
                onLoad(res.Asset as T);
            });
            
        }
        /// <summary>
        ///  卸载单独资产
        /// </summary>
        public void ReleaseResAll()
        {
            mResRecord.ForEach(res => res.Release());
            mResRecord.Clear();
        }
        
        #region private Methon

        private Res GetResFromRecord(string assetName)
        {
            return mResRecord.Find(loadAsset => loadAsset.Name == assetName);
        }

        private Res GetResFromResMgr(string assetName)
        {
            return ResMgr.Instance.mShareLoadRes.Find(loadAsset => loadAsset.Name == assetName);
        }

        private void AddRes2Record(Res resFromResMgr)
        {
            mResRecord.Add(resFromResMgr);
            resFromResMgr.Retain();
        }

        #endregion
    }
}