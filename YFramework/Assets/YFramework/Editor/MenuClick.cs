/****************************************************
    文件：MenuClik.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：base MenuClick
*****************************************************/

using UnityEditor;
using UnityEngine;

public class MenuClick : MonoBehaviour 
{
    [MenuItem("Tools/防卡")]
    private static void Click1()
    {
        EditorUtility.ClearProgressBar();
    }
}