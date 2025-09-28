/****************************************************
    文件：AutoBingEditor.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using YFramework.Extension;

namespace YFramework.Editor
{
    public static class AutoBingEditor
    {
        private static readonly string tempName = "YFrameworkAutoBingTemp";
        private static bool isChange;

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
                var temp = new GameObject(tempName).AddComponent<AutoBingCacheData>();
                temp.targetMono = mono;
                var text = scriptAsset.text;
                if (text.IndexOf("partial class " + className) == -1)
                {
                    var index = text.IndexOf("class");
                    var newText = text.Insert(index, "partial ");
                    File.WriteAllText(fullPath, newText);
                    isChange = !text.Equals(newText);
                }

                CreateDesigner(mono, temp, fullPath);
            }
            else
            {
                Debug.LogWarning("未能找到对应的脚本资源文件。");
            }
        }

        private static void CreateDesigner(MonoBehaviour mono, AutoBingCacheData temp, string fullPath)
        {
            var directory = Path.GetDirectoryName(fullPath);
            string newFileName = $"{mono.GetType().Name}.Designer.cs";
            string fullNewPath = Path.Combine(directory, newFileName);
            CreateDesignerText(mono, temp, fullNewPath);
        }

        private static void CreateDesignerText(MonoBehaviour mono, AutoBingCacheData temp, string fullNewPath)
        {
            string oldText = "";
            if (File.Exists(fullNewPath))
            {
                oldText = File.ReadAllText(fullNewPath);
            }
            StringBuilder str = new StringBuilder();
            str.AppendLine();
            str.Append("public partial class ");
            str.AppendLine(mono.GetType().Name);
            str.AppendLine("{");

            var bingElementTypes = AutoBingRules.BingElementTypes;
            foreach (var eType in bingElementTypes)
            {
                List<GameObject> bingGos = new List<GameObject>();
                mono.transform.FindRecursiveWithStart(eType.GetField("Name").GetValue(null).ToString(), bingGos);
                foreach (var go in bingGos)
                {
                    str.AppendLine("   public " + eType.GetField("TName").GetValue(null) + " " + go.name + ";");
                }
            }

            str.AppendLine("}");
            var newText = str.ToString();
            File.WriteAllText(fullNewPath, newText);
            isChange = !oldText.Equals(newText);
            AssetDatabase.Refresh();
            if (!isChange)
            {
                BingMember();
            }
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void BingMember()
        {
            var cache = SceneManager.GetActiveScene().GetRootGameObjects().FirstOrDefault(x => x.name == tempName)?.GetComponent<AutoBingCacheData>();
            if (cache != null)
            {
                var t = cache.targetMono.GetType();
                //引用赋值
                var fieldInfos = t.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (var fieldInfo in fieldInfos)
                {
                    var tran = cache.targetMono.transform.FindRecursive(fieldInfo.Name);
                    if (tran)
                    {
                        if (!fieldInfo.FieldType.IsSubclassOf(typeof(MonoBehaviour)))
                        {
                            Debug.LogWarning("不是继承mono的组件:" + fieldInfo.Name);
                            continue;
                        }
                        var type = tran.GetComponent(fieldInfo.FieldType.FullName);
                        fieldInfo.SetValue(cache.targetMono, type);
                    }
                }
                Object.DestroyImmediate(cache.gameObject);
            }
        }


        public class AutoBingCacheData : MonoBehaviour
        {
            public MonoBehaviour targetMono;
        }
    }
}