/****************************************************
    文件：HpBarExample.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;

public class HpBarExample : MonoBehaviour 
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<HpBarControl>().AddHp(30);
        }
        if (Input.GetMouseButtonDown(1))
        {
            GetComponent<HpBarControl>().SubtractHp(30);
        }
    }
}