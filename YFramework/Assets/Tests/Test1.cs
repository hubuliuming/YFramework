/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnityEngine.InputSystem;
using YFramework.Arithmetic;
using YFramework.Kit;

public class Test1 : YMonoBehaviour
{
    public Transform[] trans;

    private CircleFollowRounding _rounding;
    private void Start()
    {
        _rounding = new CircleFollowRounding(Vector2.zero,1f);
        // foreach (var tran in trans)
        // {
        //     _rounding.Add(tran,3);
        // };
        _rounding.Add(trans[0],1);
        _rounding.Add(trans[1],2);
        _rounding.Add(trans[2],3);
    }


    private void Update()
    {
       _rounding.OnUpdate();
    }
}