/****************************************************
    文件：TestPictures.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

public class TestPictures : YMonoBehaviour
{
   
    public  RectTransform areaTran;

    private string path = "D:/拍照照片";
    private IEnumerator Start()
    {
        var pic = new YPicture(areaTran);
        yield return new WaitForEndOfFrame();

        var data = pic.Cut();
        pic.SaveLocalFile(path,data);
    }
    
    // public IEnumerator Init()
    // {
    //     rawImage = transform.GetComponent<RawImage>();
    //     yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
    //     if (Application.HasUserAuthorization(UserAuthorization.WebCam))
    //     {
    //         WebCamDevice[] devices = WebCamTexture.devices;
    //         deviceName = devices[0].name;
    //         webCamTexture = new WebCamTexture(deviceName, (int)rawImage.GetComponent<RectTransform>().rect.width, (int)rawImage.GetComponent<RectTransform>().rect.height, 60);
    //
    //         
    //         rawImage.texture = webCamTexture;
    //
    //         //webCamTexture.Play();
    //     }
    // }
   
}