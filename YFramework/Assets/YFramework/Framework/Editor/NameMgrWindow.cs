/****************************************************
    文件：NameMgrWindow.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
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

        private void OnGUI()
        {
            GUILayout.Label("资源名称管理器");
        }
    }

    public class NameMgrWindowData
    {
        private static Dictionary<FileData, List<string>> m_SpriteDict = new Dictionary<FileData, List<string>>();

        public static void Add(FileData data,string value)
        {
            if (!m_SpriteDict.ContainsKey(data))
            {
                m_SpriteDict[data] = new List<string>();
            }
            m_SpriteDict[data].Add(value);
        }
    }

    public class FileData
    {
        public string Path;
        public string NameTip;
    }
}