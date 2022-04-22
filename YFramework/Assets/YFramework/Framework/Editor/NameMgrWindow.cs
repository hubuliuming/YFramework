/****************************************************
    文件：NameMgrWindow.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace YFramework.Editor
{
    public class NameMgrWindow : EditorWindow 
    {
        public static void Show()
        {
            GetWindow<NameMgrWindow>();
        }
    }

    public class NameMgrWindowData
    {
        public static Dictionary<FileData, List<string>> SpriteDict = new Dictionary<FileData, List<string>>();

        public static void Add(FileData data,string value)
        {
            if (!SpriteDict.ContainsKey(data))
            {
                SpriteDict[data] = new List<string>();
            }
            SpriteDict[data].Add(value);
        }
    }

    public class FileData
    {
        public string Path;
        public string NameTip;
    }
}