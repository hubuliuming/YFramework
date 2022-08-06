/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;
using YFramework;
using YFramework.UI;

public struct MsgData
{
    public struct Test1
    {
        public int index;
    }
}

public class Test1 : YMonoBehaviour
{
    private MsgData.Test1 msgData = new MsgData.Test1();
    public void Start()
    {
        msgData.index = 2;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MsgDispatcher.Send("123",msgData);
            
        }
    }
}

