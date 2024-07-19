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
    private void Start()
    {
        var go = transform.GetChild(0).gameObject;
        // EventSystem.current.SetSelectedGameObject(go);
        go.AddComponent<ButtonEvent>().onClick.AddListener(()=> go.GetComponent<MeshRenderer>().material.color = Color.cyan);
    }
    

}

