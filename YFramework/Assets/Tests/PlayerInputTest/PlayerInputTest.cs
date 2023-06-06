/****************************************************
    文件：PlayerInputTest.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputTest : MonoBehaviour
{
    public GameObject Select;
    //private InputSystemUIInputModule _uiInputModule;

    private void Start()
    {
        GetComponent<Dropdown>().Show();
    }

    void Update()
    {
        
        //Debug.Log(PlayerInputSystem.MyInput);
    }
    
}