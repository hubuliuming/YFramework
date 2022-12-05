/****************************************************
    文件：Test2.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using YFramework.Kit.Net;
using YFramework.Kit.UI;

public class Test2 : MonoBehaviour
{
    private GameObject go;
    private void Start()
    {
      
        go?.SetActive(true);
    }

    private void Update()
    {
        
    }


    public void OnSelected(int index)
    {
        Debug.Log(index);
    }
    
}