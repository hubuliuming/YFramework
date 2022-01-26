/****************************************************
    文件：AssetBundleExample.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.IO;
using UnityEngine;

namespace YFramework
{
    public class AssetBundleExample : MonoBehaviour 
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("YFramework/Examples/9.AssetBundleExample", false, 9)]
        private static void MenuClick()
        {
                if (!Directory.Exists(Application.streamingAssetsPath))
                {
                    Directory.CreateDirectory(Application.streamingAssetsPath);
                }
                
                UnityEditor.BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, UnityEditor.BuildAssetBundleOptions.None, 
                    UnityEditor.BuildTarget.StandaloneWindows);
        }

        [UnityEditor.MenuItem("YFramework/Examples/9.AssetBundleExample/run", false, 9)]
        private static void MenuClick2()
        {
            UnityEditor.EditorApplication.isPlaying = true;
            new GameObject("AssetBundleExample").AddComponent<AssetBundleExample>(); 
        }
#endif
        private void Start()
        {
            var ab =  AssetBundle.LoadFromFile(Application.streamingAssetsPath+"/gameobject");
            var go = ab.LoadAsset<GameObject>("GameObject");
            Instantiate(go);
        }
    }
}