/****************************************************
    文件：NameMgrWindow.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using YFramework.Kit.IO;

namespace YFramework.Editor
{
#if UNITY_EDITOR
    [Obsolete("this class have bug")]
    public class NameMgrWindow : EditorWindow
    {
        private  Dictionary<string, string> cur_ChangeNameDict = new Dictionary<string, string>();
        public new static void Show()
        {
            GetWindow<NameMgrWindow>();
        }
        private void OnGUI()
        {
            GUILayout.Label("资源名称管理器");
            // foreach中修改访问错误，未修改bug
            NameMgrWindowData.UpdateData();
            GUILayout.BeginHorizontal();
            foreach (var pair in NameMgrWindowData.SpriteDict)
            {
                foreach (var path in pair.Value)
                {
                    GUILayout.BeginVertical();
                    var texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                    GUILayout.Box(texture2D,GUILayout.Height(80),GUILayout.Width(80));
                    var name = Path.GetFileNameWithoutExtension(path);
                    if (!cur_ChangeNameDict.ContainsKey(name))
                    {
                        cur_ChangeNameDict[name] = name;
                    }
                    GUILayout.BeginHorizontal();
                    cur_ChangeNameDict[name] =GUILayout.TextArea(cur_ChangeNameDict[name], GUILayout.Width(80));
                    if (GUILayout.Button("确认",GUILayout.Width(50)))
                    {
                        if (name != cur_ChangeNameDict[name])
                        {
                            YFile.ReName(path,cur_ChangeNameDict[name]);
                            cur_ChangeNameDict.Remove(path);
                        }
                        AssetDatabase.Refresh();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndHorizontal();
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

        public static void UpdateData()
        {
            foreach (var pair in SpriteDict)
            {
                var count = pair.Value.Count;
                for (int i = 0; i < count; i++)
                {
                    if (!File.Exists(pair.Value[i]))
                    {
                        pair.Value.Remove(pair.Value[i]);
                        i--;
                    }
                }
            }
            // for (int i = 0; i < SpriteDict.Keys.Count; i++)
            // {
            //     for (int j = 0; j < SpriteDict.Values.Count; j++)
            //     {
            //         if (File.Exists(SpriteDict.Keys[i][j]))
            //         {
            //             
            //         }
            //     }
            // }
        }
    }

    public class FileData
    {
        public string Path;
        public string NameTip;
    }
#endif
}