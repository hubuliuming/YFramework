/****************************************************
    文件：MathUtilityExam.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace YFramework.Examples
{
    public class YMathUtilityExample 
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem("YFramework/Examples/2/MathUtility")]
        private static void MenuClick()
        {
            List<string> list = new List<string>() {"a", "b", "c", "d"};
            for (int i = 0; i < 20; i++)
            {
                Debug.Log(MathYUtility.GetRandomRemoveSelf(list,"b"));
            }
        }
    }
#endif
}