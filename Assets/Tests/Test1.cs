/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YFramework;
using YFramework.Event;
using YFramework.Extension;
using YFramework.Kit;

public class Test1 : YMonoBehaviour
{
    public GameObject target;
    private void Start()
    {
        
        target.AddComponentFrom(GetComponent<FPCharacter>());
    }

    private void Update()
    {
        
    }
}

