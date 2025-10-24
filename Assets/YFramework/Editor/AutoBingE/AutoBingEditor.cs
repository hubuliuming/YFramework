/****************************************************
    文件：AutoBingEditor.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using YFramework.Extension;
using YFramework.Kit;

namespace YFramework.Editor
{
    internal static class AutoBingEditor
    {
        private static readonly string tempName = "YFrameworkAutoBingTemp";
        private static bool isChange;

        [MenuItem("CONTEXT/MonoBehaviour/AutoBing")]
        private static void AutoBing(MenuCommand command)
        {
            if(EditorApplication.isPlaying) return;
            if(EditorApplication.isPaused) return;
            var mono = (MonoBehaviour) command.context;
            AutoBing(mono);
            AssetDatabase.Refresh();
            if (!isChange)
            {
                BingAfterReload();
            }
        }

        private static void AutoBing(MonoBehaviour mono)
        {
            MonoScript scriptAsset = MonoScript.FromMonoBehaviour(mono);
            if (scriptAsset != null)
            {
                string scriptAssetPath = AssetDatabase.GetAssetPath(scriptAsset);
                var className = scriptAsset.name;
                string fullPath = Path.GetFullPath(scriptAssetPath);
                if (fullPath.Contains("Library")) return;
                if (!fullPath.Contains("Assets")) return;
                var tempGo = SceneManager.GetActiveScene().GetRootGameObjects().FirstOrDefault(x => x.name == tempName);
                AutoBingCacheData cacheData = null;
                cacheData = tempGo == null ? new GameObject(tempName).AddComponent<AutoBingCacheData>() : tempGo.GetComponent<AutoBingCacheData>();
                cacheData.targetMonos.Add(mono);
                var text = scriptAsset.text;
                if (text.IndexOf("partial class " + className) == -1)
                {
                    var index = text.IndexOf("class");
                    var newText = text.Insert(index, "partial ");
                    File.WriteAllText(fullPath, newText);
                    isChange = !text.Equals(newText);
                }

                CreateDesigner(mono, fullPath);
            }
            else
            {
                Debug.LogWarning("未能找到对应的脚本资源文件。");
            }
        }

        private static void CreateDesigner(MonoBehaviour mono, string fullPath)
        {
            var directory = Path.GetDirectoryName(fullPath);
            string newFileName = $"{mono.GetType().Name}.Designer.cs";
            if (directory != null)
            {
                string fullNewPath = Path.Combine(directory, newFileName);
                CreateDesignerText(mono, fullNewPath);
            }
            else
            {
                Debug.LogWarning("未能找到对应的脚本资源文件。path:" + fullPath);
            }
        }

        private static void CreateDesignerText(MonoBehaviour mono, string fullNewPath)
        {
            string oldText = "";
            string tabSpace = "\t";
            if (File.Exists(fullNewPath))
            {
                oldText = File.ReadAllText(fullNewPath);
            }

            StringBuilder str = new StringBuilder();
            str.AppendLine();
            var nameSpace = mono.GetType().Namespace;
            if (!string.IsNullOrEmpty(nameSpace))
            {
                str.AppendLine("namespace " + nameSpace);
                str.AppendLine("{");
            }
            else
            {
                tabSpace = "";
            }

            str.Append(tabSpace + "public partial class ");
            str.AppendLine(mono.GetType().Name);
            str.AppendLine(tabSpace + "{");

            WriteRecursiveWithType(mono.transform, str, tabSpace);
            str.AppendLine(tabSpace + "}");
            if (!string.IsNullOrEmpty(nameSpace))
            {
                str.AppendLine("}");
            }

            var newText = str.ToString();
            File.WriteAllText(fullNewPath, newText);
            isChange = !oldText.Equals(newText) || isChange;
        }

        private static void WriteRecursiveWithType(Transform parent, StringBuilder str, string tabSpace)
        {
            foreach (Transform child in parent)
            {
                Type filter = null;
                foreach (var bingElementType in AutoBingRules.BingElementTypes)
                {
                    var startWithStr = bingElementType.GetField("Name").GetValue(null).ToString();
                    var t = bingElementType.GetField("TName").GetValue(null).ToString();
                
                    if (child.name.StartsWith(startWithStr))
                    {
                        var target = TypeResolverForUnity.ResolveType(t, true, "YFramework");
                        if (target != null)
                        {
                            filter = AutoBingRules.FiltrationElementTypes.Find(f => target.IsSubclassOf(f));
                        }

                        if (filter != null)
                        {
                            var mono = child.GetComponent(filter) as MonoBehaviour;
                            MonoScript.FromMonoBehaviour(mono);
                            AutoBing(mono);
                        }
                        var objT = child.gameObject.GetComponent(t);
                        var tName = "";
                        tName = objT == null ? t : objT.GetType().FullName;
                        str.AppendLine(tabSpace + "\tpublic " + tName + " " + child.gameObject.name + ";");
                        break;
                    }
                }

                if (filter != null)
                {
                    continue;
                }
                if (child.childCount > 0)
                {
                    WriteRecursiveWithType(child, str, tabSpace);
                }
            }
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void BingAfterReload()
        {
            var cache = SceneManager.GetActiveScene().GetRootGameObjects().FirstOrDefault(x => x.name == tempName)?.GetComponent<AutoBingCacheData>();
            if (cache != null)
            {
                foreach (var mono in cache.targetMonos)
                {
                    BingElements(mono);
                }

                UnityEngine.Object.DestroyImmediate(cache.gameObject);
            }
        }

        private static void BingElements(MonoBehaviour mono)
        {
            var t = mono.GetType();
            var fieldInfos = t.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var fieldInfo in fieldInfos)
            {
                var tran = mono.transform.FindRecursive(fieldInfo.Name);
                if (fieldInfo.FieldType.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    var type = tran.GetComponent(fieldInfo.FieldType.FullName);
                    if (type == null)
                    {
                        Debug.LogError($"this element {fieldInfo.Name} is not subclass of {fieldInfo.FieldType.FullName}");
                    }
                    else
                    {
                        fieldInfo.SetValue(mono, type);
                    }
                }
                else
                {
                    fieldInfo.SetValue(mono, tran.gameObject);
                }
            }
        }

        private class AutoBingCacheData : MonoBehaviour
        {
            public List<MonoBehaviour> targetMonos = new List<MonoBehaviour>();
        }
    }
}