
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

/// <summary>
/// 依赖官方注册的Addressables， web中不需要内存缓存，会自动缓存在浏览器中,所有的key,lable值不要命名重复,暂时不支持
/// </summary>
public class WebAssetsManager : MonoBehaviour
{
    private static WebAssetsManager _webAssetsManager;
    public static WebAssetsManager Manager
    {
        get
        {
            if (_webAssetsManager == null)
            {
                var go = new GameObject("WebAssetsManager");
                _webAssetsManager = go.AddComponent<WebAssetsManager>();
                DontDestroyOnLoad(go);
            }
            return _webAssetsManager;
        }
    }

    private static Dictionary<string, Object> _cacheDic;
    private void Awake()
    {
        _cacheDic = new Dictionary<string, Object>();
    }

    /// <summary>
    /// GameObject类型使用InstantiateGameObject方法
    /// </summary>
    /// <param name="key"></param>
    /// <param name="callBack"></param>
    /// <typeparam name="T"></typeparam>
    public void LoadAssetAsync<T>(string key,Action<T> callBack) where T : Object
    {
        StartCoroutine(CorLoadAssetAsync(key, callBack));
    }

    /// <summary>
    /// GameObjet采用这种方式加载生成，会自动回收
    /// </summary>
    public void InstantiateGameObject(string objName,Transform parent = null,Action<GameObject> callBack = null)
    {
        StartCoroutine(CorInstantiateGameObject(objName,parent,callBack));
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="label"></param>
    /// <param name="callBack"></param>
    /// <typeparam name="T"></typeparam>
    public void LoadAssetsAsync<T>(string label, Action<T> callBack) where T : Object
    {
        StartCoroutine(CorLoadAssetsAsync<T>(label, callBack));
    }

    public void DownLoadAssets(string key)
    {
        StartCoroutine(CorDownLoadAssets(key));
    }

    public bool CheckReleaseCache(string key)
    {
        if (_cacheDic.ContainsKey(key))
        {
            Addressables.Release(_cacheDic[key]);
            _cacheDic.Remove(key);
            return true;
        }
        else
        {
            return false;
        }
    }

    internal static void AddBind(string key, Object obj)
    {
        _cacheDic.TryAdd(key, obj);
    }

    private IEnumerator CorInstantiateGameObject(string objName, Transform parent,Action<GameObject> callBack)
    {
        CheckReleaseCache(objName);
        var handle = Addressables.InstantiateAsync(objName,parent);
        yield return handle;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            var res = handle.Result;
            callBack?.Invoke(res);
            AddBind(objName,res);
        }
        else
        {
            Debug.Log("加载资源失败：" + objName);
            Addressables.Release(handle);
        }
    }
    
    private IEnumerator CorLoadAssetAsync<T>(string key,Action<T> callBack) where T : Object
    {
        CheckReleaseCache(key);
        var handle = Addressables.LoadAssetAsync<T>(key);
        yield return handle;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            T result = handle.Result;
            handle.Completed += op =>
            {
                callBack?.Invoke(result);
                AddBind(key,result);
            };
        }
        else
        {
            Debug.Log("加载资源失败：" + key);
            Addressables.Release(handle);
        }
    
    }
    private IEnumerator CorLoadAssetsAsync<T>(string label,Action<T> callBack) where T : Object
    {
        CheckReleaseCache(label);
        AssetLabelReference assetLabelReference = new AssetLabelReference();
        assetLabelReference.labelString = label;
        callBack += t =>
        {
            AddBind(t.name,t);
        };
        var handle = Addressables.LoadAssetsAsync<T>(assetLabelReference,callBack);
        yield return handle;
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("获取Label资源失败！：" + label);
            Addressables.Release(handle);
        }
    }
    
    private IEnumerator CorDownLoadAssets(string key)
    {
       var handle = Addressables.DownloadDependenciesAsync(key,true);
       yield return handle;
       if (handle.Status != AsyncOperationStatus.Succeeded)
       {
           Debug.LogError("下载资源失败,Key: " + key);
       }
    }
  
}


