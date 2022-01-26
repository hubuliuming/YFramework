/****************************************************
    文件：LevelExample.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2022/1/18 10:4:28
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using YFramework.Managers;

namespace YFramework
{
    public class LevelExample : YMonoBehaviour 
    {
#if UNITY_EDITOR
        [MenuItem("YFramework/Examples/Managers/LevelExample")]
        private static void MenuClick()
        {
            EditorApplication.isPlaying = true;
            new GameObject("LevelExample").AddComponent<LevelExample>();
        }
#endif
        private void Start()
        {
            DontDestroyOnLoad(this);
            LevelManager.Init(new List<string>()
            {
                "Game","Home"
            });
            LevelManager.LoadCurrent();
            Delay(5,()=>LevelManager.LoadNext());
        }
        
        protected override void OnBeforeDestroy()
        {
            
        }
    }
}