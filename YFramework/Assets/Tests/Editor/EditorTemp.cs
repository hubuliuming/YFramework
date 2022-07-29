/****************************************************
    文件：EditorTemp1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEditor;
using UnityEngine;
using YFramework.IO;

#if UNITY_EDITOR
public class EditorTemp : MonoBehaviour
{
    private static string _path = @"F:\UnityProjects\Y_Person\项目素材\华科_毕业照\原始";

    [MenuItem("TempClick/Click1")]
    private static void Click1()
    {
        YFile.Classify(_path, "届");
    }
    
}
#endif