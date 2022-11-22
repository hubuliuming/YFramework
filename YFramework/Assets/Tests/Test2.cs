/****************************************************
    文件：Test2.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using YFramework.Kit.Net;
using YFramework.Kit.UI;

public class Test2 : MonoBehaviour
{
    private void Start()
    {
        var client = new TcpClient("192.168.2.105", 6666);
        client.onReceived = str =>
        {
            Debug.Log(str);
        };
        
    }
    
}