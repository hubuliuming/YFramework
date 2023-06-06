/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;
using YFramework.Extension;
using YFramework.Kit;
using YFramework.Kit.Managers;
using YFramework.Kit.Net;
using YFramework.Kit.Utility;

public class Test1 : YMonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 60;
        GetComponent<BoxCollider2D>();
    }


    private void Update()
    {
        if (Keyboard.current.dKey.IsPressed())
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.right * 3;
        }

        if (Keyboard.current.aKey.IsPressed())
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.left * 3;
        }

        if (Keyboard.current.spaceKey.IsPressed())
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * 3;
        }
    }
}