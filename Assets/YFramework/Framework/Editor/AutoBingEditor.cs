/****************************************************
    文件：AutoBingEditor.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using YFramework.Extension;

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
                var className = scriptAsset.name;
                string fullPath = Path.GetFullPath(scriptAssetPath);
                if (fullPath.Contains("Library")) return;
                if (!fullPath.Contains("Assets")) return;
                var text = scriptAsset.text;
                if (text.IndexOf("partial class " + className) == -1)
                {
                    var index = text.IndexOf("class");
                    var newText = text.Insert(index, "partial ");
                    File.WriteAllText(fullPath, newText);
                }

                CreateDesigner(mono, fullPath);
                AssetDatabase.Refresh();

            }
            else
            {
                Debug.LogWarning("未能找到对应的脚本资源文件。");
            }
        }

        private static void CreateDesigner(MonoBehaviour mono,string fullPath)
        {
            var directory = Path.GetDirectoryName(fullPath);
            string newFileName = $"{mono.GetType().Name}.Designer.cs"; // 新文件名
            string fullNewPath = Path.Combine(directory, newFileName);
            var text = GetDesignerText(mono);
            File.WriteAllText(fullNewPath, text);
        }
        
        private static string GetDesignerText(MonoBehaviour mono)
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine();
            str.Append("public partial class ");
            str.AppendLine(mono.GetType().Name);
            str.AppendLine("{");

            var signDic = AutoBingRules.SignToTypeDic;
            foreach (var key in signDic.Keys)
            {
                List<GameObject> bingGos = new List<GameObject>();
                mono.transform.RecursiveFind(key, bingGos);
                foreach (var go in bingGos)
                {
                    var t = go.GetComponent(signDic.GetType().Name);
                    str.AppendLine("   public " + signDic[key] + " " + go.name + ";");
                }
            }

            EditorApplication.delayCall += () =>
            {
                var t = mono.GetType();
                Debug.Log("完成:" + mono.GetType().Name);
                //引用赋值
            };
            str.AppendLine("}");
            
            return str.ToString();
        }
        
    }
}