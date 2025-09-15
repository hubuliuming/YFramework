/****************************************************
    文件：AutoBingEditor.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEditor;
using UnityEngine;

namespace YFramework.Editor
{
    public class AutoBingEditor  
    {
        [MenuItem("CONTEXT/MonoBehaviour/AutoBing")]
        private static void AutoBing()
        {
            Debug.Log("bing");
        }
    }
}