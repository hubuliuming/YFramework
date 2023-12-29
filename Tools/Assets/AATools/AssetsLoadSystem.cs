/****************************************************
    文件：ResourcesLoadSystem.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using YFramework;
using Object = UnityEngine.Object;

namespace MirrorOfWitch
{
    public class AssetsLoadSystem
    {
        private static AssetsLoadSystem _instance;

        public static AssetsLoadSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AssetsLoadSystem();
                }

                return _instance;
            }
        }
        /// <summary>
        ///  采用携程方式
        /// </summary>
        public void AALoadAssetAsync<T>(string pathKey,Action<T> completed)
        {
            MonoManager.Instance.StartCoroutine(CorAALoadAssetAsync<T>(pathKey, completed));
        }
        

        public void AARelease(UnityEngine.Object tObj)
        {
            if(tObj == null) return;
            Addressables.Release(tObj);
            //下面这个未验证完整性
            // Resources.UnloadAsset(tObj);
        }
        private IEnumerator CorAALoadAssetAsync<T>(string pathKey,Action<T> completed) {
            var assetAsync = Addressables.LoadAssetAsync<T>(pathKey);
            yield return new WaitUntil(()=> assetAsync.IsDone);
            if (assetAsync.Status == AsyncOperationStatus.Succeeded) {
                completed.Invoke(assetAsync.Result);
            } else {
                Debug.LogErrorFormat("Load {0}failed!,path{1}",typeof(T).Name,pathKey);
            }
        }
    }
}