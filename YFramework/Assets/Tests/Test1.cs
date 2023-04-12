/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections;
using CustomUnity;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using YFramework.Extension;
using YFramework.Kit.Managers;
using YFramework.Kit.Net;

public class Test1 : CustomMonoBehaviour
{
    
    private void Start()
    {
        CoroutineKit.StartCoroutine(Cor());
    }

    private IEnumerator Cor()
    {
        yield return 0;
        Debug.Log("ddd");
    }
}