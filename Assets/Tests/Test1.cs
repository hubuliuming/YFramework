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

    private FixedUpdate _fixedUpdate;
    private void Start()
    {
       _fixedUpdate = ActionKit.SecondsFixedUpdate(30, DeA);
    }
    

    private void DeA()
    {
        Debug.Log("A");
    }

    private void DeB()
    {
        Debug.Log("B");
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            _fixedUpdate.frequency = 3;
            _fixedUpdate.Action = DeA;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            _fixedUpdate.frequency = 600;
            _fixedUpdate.Action = DeB;
        }
    }
}

