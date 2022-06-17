/****************************************************
    文件：TestPictures.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.IO;
using UnityEngine;
using UnityEngine.UI;
using YFramework;

public class TestPictures : YMonoBehaviour
{
   
    public  RectTransform areaTran;
    private string path = "D:/拍照照片";
    private void Start()
    {
        Delay(0.01f, ()=>
        {
            // YPicture pic = new YPicture(areaTran);
            // var data = pic.GetData();
            // Debug.Log(data);
            // pic.SaveLocalFile(path,data);
            
            Texture2D texture2D = new Texture2D((int) areaTran.sizeDelta.x, (int) areaTran.sizeDelta.y, TextureFormat.ARGB32, false);
            texture2D.ReadPixels(areaTran.rect,0,0,false);
            var data = texture2D.EncodeToPNG();
            texture2D.Apply();
            File.WriteAllBytes(path+"222.PNG",data);
        });
      
    }
   
}