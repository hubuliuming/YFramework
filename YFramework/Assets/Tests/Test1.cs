/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using YFramework.Kit.Net;

public class Test1 : MonoBehaviour
{
   
    private void Start()
    {
        var curPos = transform.localPosition;
        transform.DOLocalPath(new[] { curPos, curPos + Vector3.right * 10f ,curPos}, 3,PathType.Linear);

    }
    public enum JoyInput
    {
        A,
        B,
        X,
        Y
    }
    private Dictionary<KeyCode, JoyInput> curInputSetingDic = new Dictionary<KeyCode, JoyInput>()
    {
        { KeyCode.Joystick1Button0, JoyInput.A },
        { KeyCode.Joystick1Button1, JoyInput.B }
    };

    private Dictionary<JoyInput, KeyCode> lastInputSetingDic = new Dictionary<JoyInput, KeyCode>()
    {
        {JoyInput.A,KeyCode.Joystick1Button0},
        {JoyInput.B,KeyCode.Joystick1Button1}
    };
    //如果用户要把原来B建改成Ａ键盘功能
    private void CustomInput(JoyInput updateInput)
    {
        var keycode = lastInputSetingDic[updateInput];
        curInputSetingDic[keycode] = updateInput;
        //把新的输入更新到老的字典里
        lastInputSetingDic[updateInput] = keycode;
    }
    private void Update()
    {
        //初始默认是 Joystick1Button0 对应 A手柄键
        //初始默认是 Joystick1Button1 对应 B手柄键
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            var input = curInputSetingDic[KeyCode.Joystick1Button0];
            //input才是正真的操作指令，再对接实际游戏里的操作
        }
        
    }

    private void OnDestroy()
    {
        
    }
}