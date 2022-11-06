/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using DG.Tweening;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using YFramework.Kit.Net;

public class Test1 : MonoBehaviour
{
   
    private void Start()
    {
        
        
        //KeyCode.Joystick1Button0 = A
        //KeyCode.Joystick1Button1 = b
        //KeyCode.Joystick1Button2 = X
        //KeyCode.Joystick1Button3 = y
        //KeyCode.Joystick1Button4 = LB
        //KeyCode.Joystick1Button5 = RB
        //KeyCode.Joystick1Button6 = Select
        //KeyCode.Joystick1Button7 = Start
        //KeyCode.Joystick1Button8 = C
        //KeyCode.Joystick1Button9 = Z
        
        //Input.GetAxis("Horizontal") = 左边轮盘输入
        //Input.GetAxis("Vertical") = 左边轮盘输入
        
        //Input.GetAxis("Jump") = Y
        //Input.GetAxis("Submit") = A
        var names =Input.GetJoystickNames();
        foreach (var str in names)
        {
            Debug.Log(str);
        }
        
        
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Joystick1Button4))
        // {
        //     Debug.Log("4");
        // }
        // if (Input.GetKeyDown(KeyCode.Joystick1Button5))
        // {
        //     Debug.Log("5");
        // }
        // if (Input.GetKeyDown(KeyCode.Joystick1Button6))
        // {
        //     Debug.Log("6");
        // }
        // if (Input.GetKeyDown(KeyCode.Joystick1Button7))
        // {
        //     Debug.Log("7");
        // }
        // if (Input.GetKeyDown(KeyCode.Joystick1Button8))
        // {
        //     Debug.Log("8");
        // }
        // if (Input.GetKeyDown(KeyCode.Joystick1Button9))
        // {
        //     Debug.Log("9");
        // }
        // if (Input.GetKeyDown(KeyCode.Joystick1Button10))
        // {
        //     Debug.Log("10");
        // }
        // if (Input.GetKeyDown(KeyCode.Joystick1Button11))
        // {
        //     Debug.Log("11");
        // }
        // if (Input.GetKeyDown(KeyCode.Joystick1Button12))
        // {
        //     Debug.Log("12");
        // }
        // if (Input.GetKeyDown(KeyCode.Joystick1Button13))
        // {
        //     Debug.Log("13");
        // }
        // if (Input.GetKeyDown(KeyCode.Joystick1Button14))
        // {
        //     Debug.Log("14");
        // }
        // if (Input.GetKeyDown(KeyCode.Joystick1Button15))
        // {
        //     Debug.Log("15");
        // }
        // if (Input.GetKeyDown(KeyCode.Joystick1Button16))
        // {
        //     Debug.Log("16");
        // }
        // if (Input.GetKeyDown(KeyCode.Joystick1Button17))
        // {
        //     Debug.Log("17");
        // }
        // if (Input.GetKeyDown(KeyCode.Joystick1Button18))
        // {
        //     Debug.Log("18");
        // }
        // if (Input.GetKeyDown(KeyCode.Joystick1Button19))
        // {
        //     Debug.Log("19");
        // }
        // if (Input.GetKeyDown(KeyCode.JoystickButton0))
        // {
        //     Debug.Log("0");
        // }
      

        
        Debug.Log(Input.GetAxis("Horizontal"));
        Debug.Log(Input.GetAxis("Vertical"));
        
        //Debug.Log(Input.GetAxis("Cancel"));
        // Debug.Log(Input.GetAxis("Fire1"));
        // Debug.Log(Input.GetAxis("Fire2"));
        // Debug.Log(Input.GetAxis("Fire3"));
        // Debug.Log(Input.GetAxis("Jump"));
        // Debug.Log(Input.GetAxis("Submit"));
        
        
    }

    private void OnDestroy()
    {
        
    }
}