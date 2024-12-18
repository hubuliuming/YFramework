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

    private ActionFixedUpdate _actionFixedUpdate;
    private void Start()
    {
       _actionFixedUpdate = ActionKit.SecondsFixedUpdate(30, DeA);
    }
    

    private void DeA()
    {
        Debug.Log("A");

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("UP");
        }
    }

    private void DeB()
    {
        Debug.Log("B");
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            _actionFixedUpdate.SetFrequency(3);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            _actionFixedUpdate.SetFrequency(300);;
            _actionFixedUpdate.AddFixedAction(DeB);
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            _actionFixedUpdate.RemoveFixedAction(DeB);
        }
    }
}

