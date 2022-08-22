/****************************************************
    文件：TempTest.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using YFramework.IO;

public class Test2 : YMonoBehaviour
{
    private ScrollRect _scrollRect;

    private void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _scrollRect.UpdateContentLength(2,3,10);
        }
    }
}