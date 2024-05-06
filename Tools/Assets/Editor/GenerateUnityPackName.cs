

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

public class GenerateUnityPackName
{
    public static string GetPackName()
    {
        return "Tools" + DateTime.Now.ToString("yyyyMMdd_HH");
    }
#if UNITY_EDITOR
    //总结
    [UnityEditor.MenuItem("Tools/自动导出unitypackage %e", false, 1)]
    private static void ClickExportPack()
    {
        UnityEditor.AssetDatabase.ExportPackage("Assets/YTools", GetPackName() + ".unitypackage",
            UnityEditor.ExportPackageOptions.Recurse);
        Application.OpenURL("file:///" + Path.Combine(Application.dataPath, "../"));
    }
#endif
}

