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
public class EditorTemp
{
    private static string _path = @"F:\UnityProjects\Y_Person\项目素材\华科_毕业照\原始";

    [MenuItem("TempClick/Click1")]
    private static void Click1()
    {
        //YFile.Classify(_path, "届");

        // string sumKey = "sum";
        // float sum = 0;
        // //保存值时候用这个
        // PlayerPrefs.SetFloat(sumKey,sum);
        // //读取时候用这个
        // sum = PlayerPrefs.GetFloat(sumKey, sum);
    }

    public class Solution {
        // public bool ContainsDuplicate(int[] nums)
        // {
        //     for (int i = 0; i < nums.Length; i++)
        //     {
        //         
        //     }
        // }
    }
    
}
#endif