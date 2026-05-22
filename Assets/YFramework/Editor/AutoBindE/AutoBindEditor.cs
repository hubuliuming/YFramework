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
                    if(yMono.IgnoreSelf) continue;
                    var t = yMono.GetType();
                    var memberName = GetProcessMemberName(child.gameObject.name,false);
                    localData.type2MemberName.Add(t.FullName, memberName);
                    var arrayStr = ProcessArrayMember(localData, t,memberName,child.gameObject.name, child);
                    ProcessWriteMember(localData, sb, child.gameObject.name, memberName, arrayStr, t, tabSpace, child);
                    AutoBind(cacheData,new MonoLocalData(yMono.MonoSelf,localData.mono));
                    continue;
                }
                Type filter = null;
                foreach (var bindElementType in AutoBindRules.BindElementTypes
                             .OrderByDescending(type => type.GetField("Name").GetValue(null).ToString().Length))
                {
                    var startWithStr = bindElementType.GetField("Name").GetValue(null).ToString();
                    var tStr = bindElementType.GetField("TName").GetValue(null).ToString();
                   
                    if (child.name.StartsWith(startWithStr))
                    {
                        var targetType = TypeResolverForUnity.ResolveType(tStr, true, "YFramework");
                        if (targetType == null)
                        {
                            Debug.LogWarning($"AutoBind skipped object '{child.gameObject.name}' because component type '{tStr}' is not available in the current project.");
                            break;
                        }
                        var memberName = GetProcessMemberName(child.gameObject.name);
                        if (!string.IsNullOrEmpty(memberName))
                        {
                            localData.type2MemberName.Add(targetType.FullName, memberName);
                            var arrayStr = ProcessArrayMember(localData, targetType, memberName,child.gameObject.name, child);
                            ProcessWriteMember(localData,sb, child.gameObject.name, memberName, arrayStr, targetType, tabSpace, child);
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
                    if (fieldInfo.FieldType.IsSubclassOf(typeof(Component)))
                    {
                        if (fieldInfo.Name.Equals(parentMonoMemberName))
                        {
                            fieldInfo.SetValue(localData.mono, localData.parentMono);
                            continue;
                        }
                        var tran = GetBindTransform(localData, fieldInfo.Name, objName);
                        if (tran !=null)
                        {
                            var type = tran.GetComponent(fieldInfo.FieldType);
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
                            if (!localData.memberArrayName2ObjName.ContainsKey(fieldInfo.Name))
                            {
                                Debug.LogWarning($"this scripts {localData.mono.name} has no bind element {fieldInfo.Name}");
                                continue;
                            }
                            var originNames = localData.memberArrayName2ObjName[fieldInfo.Name];
                            if (originNames == null || originNames.Count == 0)
                            {
                                Debug.LogWarning($"this scripts {localData.mono.name} has no bind element {fieldInfo.Name}");
                                continue;
                            }
                            Array array = Array.CreateInstance(arrayType, originNames.Count);
                            for (int i = 0; i < originNames.Count; i++)
                            {
                                var originName = originNames[i];
                                var tran = GetBindArrayTransform(localData, fieldInfo.Name, i, originName);
                                if (tran != null)
                                {
                                    var type = tran.GetComponent(arrayType);
                                   
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
                        var tran = GetBindTransform(localData, fieldInfo.Name, objName);
                        if (tran == null)
                        {
                            Debug.LogWarning($"this scripts {localData.mono.name} has no bind element {fieldInfo.Name}");
                            continue;
                        }
                        fieldInfo.SetValue(localData.mono, tran.gameObject);
                        localData.processedTrans.Add(tran);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("fail:"+fieldInfo.FieldType +" Exception:" + e);
                    Debug.Log($"check {localData.mono.name} reference GameObjects children name is same"  );
                }
               
            }
            EditorUtility.SetDirty(localData.mono);
        }

        private static Transform GetBindTransform(MonoLocalData localData, string fieldName, string objName)
        {
            if (localData.memberNewName2Transform != null && localData.memberNewName2Transform.ContainsKey(fieldName))
            {
                var tran = localData.memberNewName2Transform[fieldName];
                if (tran != null)
                {
                    return tran;
                }
            }

            if (string.IsNullOrEmpty(objName))
            {
                return null;
            }

            return localData.mono.transform.FindRecursive(objName);
        }

        private static Transform GetBindArrayTransform(MonoLocalData localData, string fieldName, int index, string originName)
        {
            if (localData.memberArrayName2Transforms != null && localData.memberArrayName2Transforms.ContainsKey(fieldName))
            {
                var trans = localData.memberArrayName2Transforms[fieldName];
                if (trans != null && index >= 0 && index < trans.Count && trans[index] != null)
                {
                    return trans[index];
                }
            }

            if (string.IsNullOrEmpty(originName))
            {
                return null;
            }

            var foundTrans = localData.mono.transform.FindsRecursive(originName);
            if (foundTrans.Count <= 0)
            {
                return null;
            }

            var tran = foundTrans[0];
            if (foundTrans.Count > 1)
            {
                foreach (var tran1 in foundTrans)
                {
                    if (!localData.processedTrans.Contains(tran1))
                    {
                        tran = tran1;
                        break;
                    }
                }
            }

            return tran;
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
            origin = origin.Trim();
    
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
                memberName = Regex.Replace(memberName, @"\d+$", "");
            }
            return memberName;
        }

        private static string ProcessArrayMember(MonoLocalData localData,Type t,string memberName,string objName, Transform objTrans)
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
                var arrayMemberName = memberName + "s";
                if (localData.memberArrayName2ObjName.ContainsKey(arrayMemberName))
                {
                    localData.memberArrayName2ObjName[arrayMemberName].Add(objName);
                    localData.memberArrayName2Transforms[arrayMemberName].Add(objTrans);
                }
                else
                {
                    var firstObjName = objName;
                    Transform firstObjTrans = objTrans;
                    if (localData.memberNewName2ObjName.ContainsKey(memberName))
                    {
                        firstObjName = localData.memberNewName2ObjName[memberName];
                        firstObjTrans = localData.memberNewName2Transform[memberName];
                        localData.memberArrayName2ObjName.Add(arrayMemberName,new SerializableList<string>() {firstObjName});
                        localData.memberArrayName2ObjName[arrayMemberName].Add(objName);
                        localData.memberArrayName2Transforms.Add(arrayMemberName,new SerializableList<Transform>() {firstObjTrans});
                        localData.memberArrayName2Transforms[arrayMemberName].Add(objTrans);
                    }
                    else
                    {
                        Debug.LogWarning($"this scripts {localData.mono.name} has no first bind element {memberName}, fallback to current object.");
                        localData.memberArrayName2ObjName.Add(arrayMemberName,new SerializableList<string>() {objName});
                        localData.memberArrayName2Transforms.Add(arrayMemberName,new SerializableList<Transform>() {objTrans});
                    }
                }
            }
            
            targetValues.Clear();
            return arrayStr;
        }
        private static void ProcessWriteMember(MonoLocalData localData, StringBuilder sb, string objName, string memberName, string arrayStr, Type targetType, string tabSpace, Transform targetTrans)
        {
            if (string.IsNullOrEmpty(arrayStr))
            {
                sb.AppendLine(tabSpace + "\t[YFramework.AutoBindField]");
                sb.AppendLine(tabSpace + "\tpublic " + targetType.FullName + " " + memberName + ";");
                localData.memberNewName2ObjName.Add(memberName, objName);
                localData.memberNewName2Transform.Add(memberName, targetTrans);
            }
            else
            {
                var arrayMemberName = memberName + "s";
                if (localData.memberArrayName2ObjName.ContainsKey(arrayMemberName) && localData.memberArrayName2ObjName[arrayMemberName].Count > 0)
                {
                    var oldMember = tabSpace + "\tpublic " + targetType.FullName + " " + memberName + ";";
                    var newArrayMember = tabSpace + "\tpublic " + targetType.FullName + arrayStr + " " + arrayMemberName+";";
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
            public SerializableKeyValue<string,Transform> memberNewName2Transform;
            public SerializableKeyValue<string, SerializableList<string>> memberArrayName2ObjName;
            public SerializableKeyValue<string, SerializableList<Transform>> memberArrayName2Transforms;
            public SerializableKeyValue<string,string> type2MemberName;
            public List<Transform> processedTrans;

            public MonoLocalData(MonoBehaviour mono,MonoBehaviour parentMono = null)
            {
                this.mono = mono;
                this.parentMono = parentMono;
                memberNewName2ObjName = new SerializableKeyValue<string, string>();
                memberNewName2Transform = new SerializableKeyValue<string, Transform>();
                memberArrayName2ObjName = new SerializableKeyValue<string, SerializableList<string>>();
                memberArrayName2Transforms = new SerializableKeyValue<string, SerializableList<Transform>>();
                type2MemberName = new SerializableKeyValue<string, string>();
                processedTrans = new List<Transform>();
            }

            public override string ToString()
            {
                return $"mono:{mono.name} memberNewName2ObjName:{memberNewName2ObjName.Count} memberNewName2Transform:{memberNewName2Transform.Count} memberArrayName2ObjName:{memberArrayName2ObjName.Count} memberArrayName2Transforms:{memberArrayName2Transforms.Count} type2MemberName:{type2MemberName.Count}";
            }
        }
    }
}
