/****************************************************
    文件：Test2.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;
using YFramework;

public class Test2 : YMonoBehaviour 
{
    private void Start()
    {
        MsgSend("1","the Test2 send is date");
    }

    protected override void OnBeforeDestroy()
    {
        
    }
}