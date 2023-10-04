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
using YFramework.Kit;

public class Test1 : YMonoBehaviour
{
    private void Start()
    {
        A(()=> 2);
    }

    public void A(Func<int> dd)
    {
        Debug.Log(dd.Invoke());
    }

}