/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using DG.Tweening;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using YFramework.Extension;
using YFramework.Kit.Net;

public class Test1 : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Text>().StartTypewrite(0.1f);
    }
}