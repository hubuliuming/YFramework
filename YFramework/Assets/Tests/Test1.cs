/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Pool;
using UnityEngine.UI;
using UnityEngine.Windows.WebCam;
using YFramework;
public class Test1 : YMonoBehaviour
{
  

    private int id;
    private List<int> ins = new List<int>();
    private void UpdateID(int id)
    {
        LinkedList<int> list = new LinkedList<int>();
      
    }
    

    private IEnumerator WebCam()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            var deviceName = devices[0].name;
            var webCamTexture = new WebCamTexture(deviceName, 1920 / 2, 1080 / 2, 60);

            var rawImage = transform.GetComponent<RawImage>();
            rawImage.texture = webCamTexture;
            Texture t1 = webCamTexture;

            RenderTexture t = new RenderTexture(1920, 1080, 16);
            
            //webCamTexture.Play();
        }
    }
}