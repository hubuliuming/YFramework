/****************************************************
    文件：Test2.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    private List<int> _num1s = new List<int>();
    private List<int> _num2s = new List<int>();


    private void Start()
    {
        A();
        var a = "555";
        var b = "666";
        unsafe
        {
            
        }
    }

    private unsafe void A()
    {
        int i = 3;
        int* x = &i;
        Debug.Log((int)x);//819584904
    }

    private unsafe void B()
    {
        
    }

    

}