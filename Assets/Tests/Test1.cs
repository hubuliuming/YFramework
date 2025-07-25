/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;
using YFramework;
using YFramework.Event;
using YFramework.Extension;
using YFramework.Kit;
using Convert = YFramework.Kit.Convert.Convert;

public class Test1 : YMonoBehaviour
{
    public GameObject target;
    public Image img;

    private ActionFixedUpdate _actionFixedUpdate;

    private List<Vector3> _list;

    private void Start()
    {
        int[] index = new int[] { 0, 1, 2, 3 };
        Debug.Log(Convert.ToStringAnyItem(index));
    }

    private void Update()
    {
       
    
    }
    
  

  
    

}

