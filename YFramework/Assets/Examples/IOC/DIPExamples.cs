/****************************************************
    文件：DIPExamples.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;

namespace YFramework.Examples
{
    public class DIPExamples : MonoBehaviour 
    {
        public interface IStorage
        {
            void SaveString(string key, string value);
            string LoadString(string key, string defaultValue = "");
        }

        public class PlayerPrefabsStorage : IStorage
        {
            public void SaveString(string key, string value)
            {
                PlayerPrefs.SetString(key,value);
            }

            public string LoadString(string key, string defaultValue = "")
            {
                return PlayerPrefs.GetString(key, defaultValue);
            }
        }

        public class EditorPrefsStorage : IStorage
        {
            public void SaveString(string key, string value)
            {
#if UNITY_EDITOR
                UnityEditor.EditorPrefs.SetString(key,value);
#endif
            }

            public string LoadString(string key, string defaultValue = "")
            {
#if UNITY_EDITOR
                return UnityEditor.EditorPrefs.GetString(key);
#endif
                return "";
            }
        }


        private void Start()
        {
            var container = new IOCContainer();
            container.Register<IStorage>(new PlayerPrefabsStorage());
            var storage = container.Get<IStorage>();
            storage.SaveString("name","运行中");
            Debug.Log(storage.LoadString("name"));
            
            container.Register<IStorage>(new EditorPrefsStorage());
            storage = container.Get<IStorage>();
            Debug.Log(storage.LoadString("name"));
        }
    }
}