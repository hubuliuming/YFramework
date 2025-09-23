/****************************************************
    文件：AutoBingEditor.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace YFramework.Editor
{
    public class AutoBingEditor
    {
        [MenuItem("CONTEXT/MonoBehaviour/AutoBing")]
        private static void AutoBing(MenuCommand command)
        {
            var mono = (MonoBehaviour) command.context;
            MonoScript scriptAsset = MonoScript.FromMonoBehaviour(mono);

            if (scriptAsset != null)
            {
                string scriptAssetPath = AssetDatabase.GetAssetPath(scriptAsset);

                string fullPath = Path.GetFullPath(scriptAssetPath);
                if (fullPath.Contains("Library")) return;
                if (!fullPath.Contains("Assets")) return;
                var text = File.ReadAllText(fullPath);

                if (text.IndexOf("partial class") == -1)
                {
                    var index = text.IndexOf("class");
                    var newText = text.Insert(index, "partial ");
                    File.WriteAllText(fullPath, newText);
                    Debug.Log(newText);
                }

                AssetDatabase.Refresh();

                Debug.Log(text);
            }
            else
            {
                Debug.LogWarning("未能找到对应的脚本资源文件。");
            }
        }
    }
}