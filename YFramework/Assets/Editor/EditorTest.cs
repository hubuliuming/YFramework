/****************************************************
    文件：EditorTest.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public class EditorTest
{
    private static int _count;
    private static int Count
    {
        get { return _count;}
        set
        {
            _count = value;
            Debug.Log(Count);
        }
    }
    [MenuItem("TestClick/AddCount")]
    private static void AddCount()
    {
        Count++;
    }
    [MenuItem("TestClick/SubCount")]
    private static void SubCount()
    {
        Count--;
    }
    
}
#endif