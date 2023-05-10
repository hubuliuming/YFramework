/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using YFramework.Extension;
using YFramework.Kit;
using YFramework.Kit.Managers;
using YFramework.Kit.Net;
using YFramework.Kit.Utility;

[RequireComponent(typeof(Text))]
public class Test1 : YMonoBehaviour
{
    
    private void Start()
    {
        GetComponent<Text>().StartTypewrite();
    }
    
}