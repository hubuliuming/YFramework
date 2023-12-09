/****************************************************
    文件：GameLoop.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;

namespace YFramework
{
    public class GameLoop : MonoBehaviour 
    {
        private static GameLoop _instance;

        public static GameLoop Instance {
            get {
                if (_instance == null) {
                    var go = new GameObject("GameLoop");
                    go.AddComponent<GameLoop>();
                }

                return _instance;
            }   
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
    }
}