/****************************************************
    文件：GenerateUnityPackName.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：自动导出UnityPackage包完整步骤
*****************************************************/
using System;
using System.IO;
using UnityEngine;

namespace YFramework
{
    public class GenerateUnityPackName
    {
        public static string GetPackName()
        {
            return "YFramework" + DateTime.Now.ToString("yyyyMMdd_HH");
        }
#if UNITY_EDITOR
        //总结
        [UnityEditor.MenuItem("YFramework/Framework/Editor/自动导出unitypackage %e",false,1)]
        private static void ClickExportPack()
        {
            UnityEditor.AssetDatabase.ExportPackage("Assets/YFramework", GetPackName() + ".unitypackage",
                UnityEditor.ExportPackageOptions.Recurse);
            Application.OpenURL("file:///" + Path.Combine(Application.dataPath, "../"));
        }
#endif
        #region Old
#if Old_0_0_2
        public static void OpenFolder(string folderPath)
        {
            ClickExport();
            Application.OpenURL(folderPath);
        }
        public static void ExportPack(string path, string fileName)
        {
            AssetDatabase.ExportPackage(path, fileName, ExportPackageOptions.Recurse);
        }
        //Fist step
        //[MenuItem("YFramework/1.生成UnityPactName",false,1)]
        private static void ClickDebugTime()
        {
            Debug.Log(GetPackName());
        }
        //two step
        //[MenuItem("YFramework/2.复制到剪切板",false,2)]
        private static void ClickCopyBuffer()
        {
            CommonUtil.CopyStrBuffer(GetPackName());
        }
        //Three step
       // [MenuItem("YFramework/3.导出unitypackage",false,3)]
        private static void ClickExport()
        {
            ExportPack("Assets/YFramework", GetPackName() + ".unitypackage");
        }
        // four step
        //[MenuItem("YFramework/4.打开文件夹 %e",false,4)]
        private static void ClickOpenFolder()
        {
            OpenFolder("file:///" + Path.Combine(Application.dataPath, "../"));
        }
#endif
        #endregion
    }
}
