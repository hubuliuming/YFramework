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
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using YFramework.Extension;
using YFramework.Kit;

namespace YFramework.Editor
{
    internal static class AutoBindEditor
    {
        private static readonly string tempName = "YFrameworkAutoBindTemp";
        private static bool isScriptsChange;

        [MenuItem("CONTEXT/MonoBehaviour/AutoBind")]
        private static void AutoBind(MenuCommand command)
        {
            if(EditorApplication.isPlaying) return;
            if(EditorApplication.isPaused) return;
            var mono = (MonoBehaviour) command.context;
            var tempGo = SceneManager.GetActiveScene().GetRootGameObjects().FirstOrDefault(x => x.name == tempName);
            AutoBindCacheData cacheData = null;
            cacheData = tempGo == null ? new GameObject(tempName).AddComponent<AutoBindCacheData>() : tempGo.GetComponent<AutoBindCacheData>();
            
            AutoBind(cacheData,new MonoLocalData(mono));
            AssetDatabase.Refresh();
            if (!isScriptsChange)
            {
                BindAfterReload();
            }
        }

        private static void AutoBind(AutoBindCacheData cacheData,MonoLocalData localData)
        {
            foreach (var targetMono in cacheData.targetMonos)
            {
                if (targetMono.GetType() == localData.mono.GetType())
                {
                    Debug.LogWarning($"GameObject:{targetMono.mono} already bind type {localData.mono.GetType().FullName},The GameObject:{localData.mono.name} can not bingElement!");
                    return;
                }
            }
            MonoScript scriptAsset = MonoScript.FromMonoBehaviour(localData.mono);
            if (scriptAsset != null)
            {  
                string scriptAssetPath = AssetDatabase.GetAssetPath(scriptAsset);
                var className = scriptAsset.name;
                string fullPath = Path.GetFullPath(scriptAssetPath);
                if (fullPath.Contains("Library")) return;
                if (!fullPath.Contains("Assets")) return;
                cacheData.targetMonos.Add(localData);
                var text = scriptAsset.text;
                if (text.IndexOf("partial class " + className) == -1)
                {
                    var index = text.IndexOf("class");
                    var newText = text.Insert(index, "partial ");
                    File.WriteAllText(fullPath, newText);
                    isScriptsChange = !text.Equals(newText);
                }
                
                CreateDesigner(cacheData,localData, fullPath);
            }
            else
            {
                Debug.LogWarning("未能找到对应的脚本资源文件。");
            }
        }

        private static void CreateDesigner(AutoBindCacheData cacheData,MonoLocalData localData, string fullPath)
        {
            var directory = Path.GetDirectoryName(fullPath);
            string newFileName = $"{localData.mono.GetType().Name}.Designer.cs";
            if (directory != null)
            {
                string fullNewPath = Path.Combine(directory, newFileName);
                CreateDesignerText(cacheData,localData, fullNewPath);
            }
            else
            {
                Debug.LogWarning("fail find scrips path:" + fullPath);
            }
        }

        private static void CreateDesignerText(AutoBindCacheData cacheData,MonoLocalData localData, string fullNewPath)
        {
            string oldText = "";
            string tabSpace = "\t";
            if (File.Exists(fullNewPath))
            {
                oldText = File.ReadAllText(fullNewPath);
            }

            StringBuilder str = new StringBuilder();
            str.AppendLine();
            var nameSpace = localData.mono.GetType().Namespace;
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
            str.AppendLine(localData.mono.GetType().Name);
            str.AppendLine(tabSpace + "{");

            WriteRecursiveWithType(cacheData,localData, localData.mono.transform, str, tabSpace);
            str.AppendLine(tabSpace + "}");
            if (!string.IsNullOrEmpty(nameSpace))
            {
                str.AppendLine("}");
            }

            var newText = str.ToString();
            File.WriteAllText(fullNewPath, newText);
            isScriptsChange = !oldText.Equals(newText) || isScriptsChange;
        }

        private static void WriteRecursiveWithType(AutoBindCacheData cacheData,MonoLocalData localData,Transform curTrans,StringBuilder str, string tabSpace)
        {
            foreach (Transform child in curTrans)
            {
                var yMono = child.GetComponent<YFramework.YMonoBehaviour>();
                if (yMono)
                {
                    var t = yMono.GetType();
                    var memberName = GetProcessMemberName(child.gameObject.name);
                    var arrayStr = GetProcessArrayMember(localData, t,memberName,child.gameObject.name);
                    //var memberSb = str.AppendLine(tabSpace + "\tpublic " + t.FullName + " " + memberName + ";");
                    if (string.IsNullOrEmpty(arrayStr))
                    {
                        str.AppendLine(tabSpace + "\tpublic " + t.FullName + " " + memberName + ";");
                        localData.memberNewName2Origin.Add(memberName, child.gameObject.name);
                    }
                    else
                    {
                        if (localData.memberType2ArrayOrigin[t].Count == 1)
                        {
                            str.AppendLine(tabSpace + "\tpublic " + t.FullName + arrayStr + " " + memberName + "s;");
                        }
                    }
                    localData.type2MemberName.Add(new KeyValuePair<Type, string>(t, memberName));
                    AutoBind(cacheData,new MonoLocalData(yMono));
                    continue;
                }
                Type filter = null;
                foreach (var bindElementType in AutoBindRules.BindElementTypes)
                {
                    var startWithStr = bindElementType.GetField("Name").GetValue(null).ToString();
                    var tStr = bindElementType.GetField("TName").GetValue(null).ToString();
                   
                    if (child.name.StartsWith(startWithStr))
                    {
                        var targetType = TypeResolverForUnity.ResolveType(tStr, true, "YFramework");
                        var memberName = GetProcessMemberName(child.gameObject.name);
                        if (!string.IsNullOrEmpty(memberName))
                        {
                            var arrayStr = GetProcessArrayMember(localData, targetType, memberName,child.gameObject.name);
                            if (string.IsNullOrEmpty(arrayStr))
                            {
                                str.AppendLine(tabSpace + "\tpublic " + targetType.FullName + " " + memberName + ";");
                                localData.memberNewName2Origin.Add(memberName, child.gameObject.name);
                            }
                            else
                            {
                                if (localData.memberType2ArrayOrigin[targetType].Count == 1)
                                {
                                    str.AppendLine(tabSpace + "\tpublic " + targetType.FullName + arrayStr + " " + memberName + "s;");
                                }
                            }
                            localData.type2MemberName.Add(new KeyValuePair<Type, string>(targetType, memberName));
                        }
                        else
                        {
                            Debug.LogWarning($"The object's name：{child.gameObject.name} does not conform to the naming convention" );
                        }
                        
                        if (targetType != null)
                        {
                            filter = AutoBindRules.FiltrationElementTypes.Find(f => targetType.IsSubclassOf(f));
                        }
                        if (filter != null)
                        {
                            var mono = child.GetComponent(filter) as MonoBehaviour;
                            AutoBind(cacheData,new MonoLocalData(mono));
                        }
                        break;
                    }
                }

                if (filter != null)
                {
                    continue;
                }
                if (child.childCount > 0)
                {
                    WriteRecursiveWithType(cacheData,localData,child,str, tabSpace);
                }
            }
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void BindAfterReload()
        {
            var cache = GetCacheData();
            if (cache != null)
            {
                foreach (var mono in cache.targetMonos)
                {
                    BindElements(mono);
                }
                ClearCacheData();
            }
        }

        private static void BindElements(MonoLocalData localData)
        {
            var t = localData.mono.GetType();
            var fieldInfos = t.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var fieldInfo in fieldInfos)
            {
                try
                {
                    if (fieldInfo.FieldType.IsSubclassOf(typeof(MonoBehaviour)))
                    {
                        var tran = localData.mono.transform.FindRecursive(localData.memberNewName2Origin[fieldInfo.Name]);
                        if (tran !=null)
                        {
                            var type = tran.GetComponent(fieldInfo.FieldType.FullName);
                            if (type == null)
                            {
                                Debug.LogError($"this element {fieldInfo.Name} is not subclass of {fieldInfo.FieldType.FullName}");
                            }
                            else
                            {
                                fieldInfo.SetValue(localData.mono, type);
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"this scripts {localData.mono.name} has no bind element {fieldInfo.Name}");
                            continue;
                        }
                    }
                    else if (fieldInfo.FieldType.IsArray)
                    {
                        var arrayType = fieldInfo.FieldType.GetElementType();
                        if (arrayType != null)
                        {
                            var originNames = localData.memberType2ArrayOrigin[arrayType];
                            foreach (var originName in originNames)
                            {
                                var tran = localData.mono.transform.FindRecursive(originName);
                                if (tran !=null)
                                {
                                    var type = tran.GetComponent(arrayType.FullName);
                                    if (type == null)
                                    {
                                        Debug.LogError($"this element {fieldInfo.Name} is not subclass of {arrayType.FullName}");
                                    }
                                    else
                                    {
                                        fieldInfo.SetValue(localData.mono, type);
                                    }
                                }
                                else
                                {
                                    Debug.LogWarning($"this scripts {localData.mono.name} has no bind element {fieldInfo.Name}");
                                    continue;
                                }
                            }
                        }
                    }
                    else
                    {
                        var tran = localData.mono.transform.FindRecursive(localData.memberNewName2Origin[fieldInfo.Name]);
                        fieldInfo.SetValue(localData.mono, tran.gameObject);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("fail:"+fieldInfo.FieldType);
                    throw;
                }
               
            }
            EditorUtility.SetDirty(localData.mono);
        }
        
        private static AutoBindCacheData GetCacheData()
        {
            return SceneManager.GetActiveScene().GetRootGameObjects().FirstOrDefault(x => x.name == tempName)?.GetComponent<AutoBindCacheData>();
        }

        private static string GetProcessMemberName(string origin)
        {
            if (string.IsNullOrEmpty(origin))
                return string.Empty;
    
            string cleaned = Regex.Replace(origin, @"[^a-zA-Z0-9_]", "");
    
            if (string.IsNullOrEmpty(cleaned))
                return string.Empty;
    
            cleaned = cleaned.TrimEnd('_');
    
            if (string.IsNullOrEmpty(cleaned))
                return string.Empty;
    
            if (char.IsDigit(cleaned[0]))
            {
                var num = cleaned[0];
                var str = cleaned.Remove(0, 1);
                cleaned = str + num;
            }
            
            if (char.IsDigit(cleaned[cleaned.Length - 1]))
            {
                cleaned = cleaned.Remove(cleaned.Length - 1);
            }
    
            return cleaned;
        }

        private static string GetProcessArrayMember(MonoLocalData localData,Type t,string memberName,string objName)
        {
            string arrayStr = "";

            foreach (var key in localData.type2MemberName)
            {
                if (key.Key == t)
                {
                    if (key.Value.Equals(memberName))
                    {
                        arrayStr = "[]";
                        if (localData.memberType2ArrayOrigin.ContainsKey(t))
                        {
                            localData.memberType2ArrayOrigin[t].Add(objName);
                        }
                        else
                        {
                            localData.memberType2ArrayOrigin.Add(t,new List<string>{objName});
                        }
                    }
                }
            }
            return arrayStr;
        }
        
        private static void ClearCacheData()
        {
            var cache = GetCacheData();
            if (cache)
            {
                UnityEngine.Object.DestroyImmediate(cache.gameObject);
            }
        }

        private class AutoBindCacheData : MonoBehaviour
        {
            public List<MonoLocalData> targetMonos = new List<MonoLocalData>();

            private void OnDestroy()
            {
                targetMonos.Clear();
            }
        }
        
        private class MonoLocalData
        {
            public MonoBehaviour mono;
            public Dictionary<string, string> memberNewName2Origin;
            public Dictionary<Type, List<string>> memberType2ArrayOrigin;
            public List<KeyValuePair<Type,string>> type2MemberName;

            public MonoLocalData(MonoBehaviour mono)
            {
                this.mono = mono;
                memberNewName2Origin = new Dictionary<string, string>();
                memberType2ArrayOrigin = new Dictionary<Type, List<string>>();
                type2MemberName = new List<KeyValuePair<Type, string>>();
            }
        }
    }
}