/****************************************************
    文件：JsonExample.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace YFramework.Examples
{
    public class AppForPhone
    {
        public int appNum;
        public bool phoneState;
        //对象为类，枚举等需要加序列化才能成功写入。
        public List<AppProperty> AppProperties; 
    }
    [Serializable]
    public class AppProperty
    {
        public string name;
        public float size;
    }
    public class JsonExample : MonoBehaviour 
    {
#if UNITY_EDITOR
        private static string savePath = Application.dataPath + "/YFramework/Examples/TempAgs/json.json";
        [UnityEditor.MenuItem("YFramework/Examples/3/JsonSave")]
        private static void MenuClick()
        {
            AppForPhone app = new AppForPhone()
             {
                 appNum = 3,
                 phoneState = true,
                 AppProperties = new List<AppProperty>()
                 {
                     new AppProperty()
                     {
                         name = "QQ",
                         size = 123f,
                     }
                 }
             };
            YJsonUtility.Save(app,savePath);
            
            UnityEditor.AssetDatabase.Refresh();
        }

        [UnityEditor.MenuItem("YFramework/Examples/3/JsonLoad")]
        private static void MenuClick2()
        {
            var app = YJsonUtility.Load<AppForPhone>(savePath);
            Debug.Log(app.appNum);
            Debug.Log(app.phoneState);
            foreach (var a in app.AppProperties)
            {
                Debug.Log(a.name);
                Debug.Log(a.size);
            }
        }
#endif
      
    }
}