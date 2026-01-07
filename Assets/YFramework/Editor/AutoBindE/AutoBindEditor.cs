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
using YFramework.Collections;
using YFramework.Extension;
using YFramework.Kit;

namespace YFramework.Editor
{
    internal static class AutoBindEditor
    {
        private static readonly string tempName = "YFrameworkAutoBindTemp";
        private static readonly string parentMonoMemberName = "ParentMono";
        private static bool isScriptsChange;

        [MenuItem("CONTEXT/MonoBehaviour/AutoBind")]
        private static void AutoBind(MenuCommand command)
        {
            if(EditorApplication.isPlaying) return;
            if(EditorApplication.isPaused) return;
            var mono = (MonoBehaviour) command.context;
            AutoBindCacheData cacheData = null;
            ClearCacheData();
            cacheData = new GameObject(tempName).AddComponent<AutoBindCacheData>();
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

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            var nameSpace = localData.mono.GetType().Namespace;
            if (!string.IsNullOrEmpty(nameSpace))
            {
                sb.AppendLine("namespace " + nameSpace);
                sb.AppendLine("{");
            }
            else
            {
                tabSpace = "";
            }
            sb.Append(tabSpace + "public partial class ");
            sb.AppendLine(localData.mono.GetType().Name);
            sb.AppendLine(tabSpace + "{");

            if (localData.parentMono)
            {
                sb.AppendLine(tabSpace + "\t[YFramework.AutoBindField]");
                sb.AppendLine(tabSpace + "\tpublic " + localData.parentMono.GetType().FullName + " " + parentMonoMemberName + ";");
            }
            WriteRecursiveWithType(cacheData,localData, localData.mono.transform, sb, tabSpace);
            sb.AppendLine(tabSpace + "}");
            if (!string.IsNullOrEmpty(nameSpace))
            {
                sb.AppendLine("}");
            }

            var newText = sb.ToString();
            File.WriteAllText(fullNewPath, newText);
            isScriptsChange = !oldText.Equals(newText) || isScriptsChange;
        }

        private static void WriteRecursiveWithType(AutoBindCacheData cacheData,MonoLocalData localData,Transform curTrans,StringBuilder sb, string tabSpace)
        {
            foreach (Transform child in curTrans)
            {
                var yMono = child.GetComponent<YFramework.IAutoBindMono>();
                if (yMono != null)
                {
                    var t = yMono.GetType();
                    var memberName = GetProcessMemberName(child.gameObject.name,false);
                    localData.type2MemberName.Add(t.FullName, memberName);
                    var arrayStr = ProcessArrayMember(localData, t,memberName,child.gameObject.name);
                    ProcessWriteMember(localData, sb, child.gameObject.name, memberName, arrayStr, t, tabSpace);
                    AutoBind(cacheData,new MonoLocalData(yMono.MonoSelf,localData.mono));
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
                            localData.type2MemberName.Add(targetType.FullName, memberName);
                            var arrayStr = ProcessArrayMember(localData, targetType, memberName,child.gameObject.name);
                            ProcessWriteMember(localData,sb, child.gameObject.name, memberName, arrayStr, targetType, tabSpace);
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
                    WriteRecursiveWithType(cacheData,localData,child,sb, tabSpace);
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
            var newFieldInfos = fieldInfos.Where(f => f.IsDefined(typeof(AutoBindFieldAttribute), false));
            foreach (var fieldInfo in newFieldInfos)
            {
                try
                {
                    var objName = localData.memberNewName2ObjName[fieldInfo.Name];
                    if (fieldInfo.FieldType.IsSubclassOf(typeof(MonoBehaviour)))
                    {
                        if (fieldInfo.Name.Equals(parentMonoMemberName))
                        {
                            fieldInfo.SetValue(localData.mono, localData.parentMono);
                            continue;
                        }
                        var tran = localData.mono.transform.FindRecursive(objName);
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
                                localData.processedTrans.Add(tran);
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
                            var originNames = localData.memberType2ArrayObjName[arrayType.FullName];
                            Array array = Array.CreateInstance(arrayType, originNames.Count);
                            for (int i = 0; i < originNames.Count; i++)
                            {
                                var originName = originNames[i];
                                var trans = localData.mono.transform.FindsRecursive(originName);
                                if (trans.Count > 0)
                                {
                                    var tran = trans[0];
                                    if (trans.Count > 1)
                                    {
                                        foreach (var tran1 in trans)
                                        {
                                            if (!localData.processedTrans.Contains(tran1))
                                            {
                                                tran = tran1;
                                                break;
                                            }
                                        }
                                    }
                                    var type = tran.GetComponent(arrayType.FullName);
                                   
                                    if (type == null)
                                    {
                                        Debug.LogError($"this element {fieldInfo.Name} is not subclass of {arrayType.FullName}");
                                    }
                                    else
                                    {
                                        array.SetValue(type, i);
                                        localData.processedTrans.Add(tran);
                                    }
                                }
                                else
                                {
                                    Debug.LogWarning($"this scripts {localData.mono.name} has no bind element {fieldInfo.Name}");
                                    continue;
                                }
                               
                            }
                            fieldInfo.SetValue(localData.mono, array);
                        }
                    }
                    else
                    {
                        var tran = localData.mono.transform.FindRecursive(objName);
                        fieldInfo.SetValue(localData.mono, tran.gameObject);
                        localData.processedTrans.Add(tran);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("fail:"+fieldInfo.FieldType +" Exception:" + e);
                    Debug.Log($"check {localData.mono.name} reference GameObjects children name is same"  );
                    throw;
                }
               
            }
            EditorUtility.SetDirty(localData.mono);
        }
        
        private static AutoBindCacheData GetCacheData()
        {
            var go = SceneManager.GetActiveScene().GetRootGameObjects().FirstOrDefault(o => o.name == tempName);
            return go ? go.GetComponent<AutoBindCacheData>() : null;
        }

        private static string GetProcessMemberName(string origin,bool removeLastNum = true)
        {
            if (string.IsNullOrEmpty(origin))
                return string.Empty;
    
            string memberName = Regex.Replace(origin, @"[^a-zA-Z0-9_]", "");
    
            if (string.IsNullOrEmpty(memberName))
                return string.Empty;
    
            memberName = memberName.TrimEnd('_');
    
            if (string.IsNullOrEmpty(memberName))
                return string.Empty;
    
            if (char.IsDigit(memberName[0]))
            {
                var num = memberName[0];
                var str = memberName.Remove(0, 1);
                memberName = str + num;
            }

            if (removeLastNum)
            {
                if (char.IsDigit(memberName[^1]))
                {
                    memberName = memberName.Remove(memberName.Length - 1);
                }
            }
            return memberName;
        }

        private static string ProcessArrayMember(MonoLocalData localData,Type t,string memberName,string objName)
        {
            string arrayStr = "";
            List<string> targetValues = new List<string>();
            foreach (var key in localData.type2MemberName)
            {
                if (key.Key == t.FullName)
                {
                    if (key.Value.Equals(memberName))
                    {
                        targetValues.Add(key.Value);
                    }
                }
            }

            if (targetValues.Count > 1)
            {
                arrayStr = "[]";
                if (localData.memberType2ArrayObjName.ContainsKey(t.FullName))
                {
                    localData.memberType2ArrayObjName[t.FullName].Add(objName);
                }
                else
                {
                    localData.memberType2ArrayObjName.Add(t.FullName,new SerializableList<string>() {targetValues[0]});
                    localData.memberType2ArrayObjName[t.FullName].Add(objName);
                }
            }
            
            targetValues.Clear();
            return arrayStr;
        }
        private static void ProcessWriteMember(MonoLocalData localData, StringBuilder sb, string objName, string memberName, string arrayStr, Type targetType, string tabSpace)
        {
            if (string.IsNullOrEmpty(arrayStr))
            {
                sb.AppendLine(tabSpace + "\t[YFramework.AutoBindField]");
                sb.AppendLine(tabSpace + "\tpublic " + targetType.FullName + " " + memberName + ";");
                localData.memberNewName2ObjName.Add(memberName, objName);
            }
            else
            {
                if (localData.memberType2ArrayObjName[targetType.FullName].Count > 0)
                {
                    var oldMember = tabSpace + "\tpublic " + targetType.FullName + " " + memberName + ";";
                    memberName += "s";
                    var newArrayMember = tabSpace + "\tpublic " + targetType.FullName + arrayStr + " " + memberName+";";
                    sb.Replace(oldMember, newArrayMember);
                }
            }
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

        [Serializable]
        private class MonoLocalData
        {
            public MonoBehaviour mono;
            public MonoBehaviour parentMono;
            public SerializableKeyValue<string,string> memberNewName2ObjName;
            public SerializableKeyValue<string, SerializableList<string>> memberType2ArrayObjName;
            public SerializableKeyValue<string,string> type2MemberName;
            public List<Transform> processedTrans;

            public MonoLocalData(MonoBehaviour mono,MonoBehaviour parentMono = null)
            {
                this.mono = mono;
                this.parentMono = parentMono;
                memberNewName2ObjName = new SerializableKeyValue<string, string>();
                memberType2ArrayObjName = new SerializableKeyValue<string, SerializableList<string>>();
                type2MemberName = new SerializableKeyValue<string, string>();
                processedTrans = new List<Transform>();
            }

            public override string ToString()
            {
                return $"mono:{mono.name} memberNewName2ObjName:{memberNewName2ObjName.Count} memberType2ArrayObjName:{memberType2ArrayObjName.Count} type2MemberName:{type2MemberName.Count}";
            }
        }
    }
}