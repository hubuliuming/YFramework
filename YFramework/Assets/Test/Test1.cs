/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;

public class Test1 : MonoBehaviour 
{
    private void Start()
    {
        int num = 1;
        Add1(num);
        Debug.Log(num);
        Add2(ref num);
        Debug.Log(num);
    }

    private void Add1(int num)
    {
        num += 55;
    }

    private void Add2(ref int num)
    {
        num += 55;
    }
}