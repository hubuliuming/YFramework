/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.UI;

public class Test1 : MonoBehaviour
{
    public ScrollRect scrollRect;


    private void Start()
    {
        scrollRect.onValueChanged.AddListener(value =>
        {
            Debug.Log(value);
        });
    }
}