/****************************************************
    文件：Texture2DUtility.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/


using System;
using System.IO;
using UnityEngine;

namespace YFramework
{
    public class Texture2DUtility 
    {
        
        public byte[] CutScreen(float startX,float startY,float width,float height)
        {
            //直接传送相机数据
            // Texture2D png = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.ARGB32, false);
            // Color[] colors = webCamTexture.GetPixels();
            // png.SetPixels(colors);
    
            //截图传送
            Texture2D png = new Texture2D((int)width, (int)height, TextureFormat.ARGB32, false);
            png.ReadPixels(new Rect(startX, startY, width, height), 0, 0, false);
            png.Apply();
            //imgTest.texture = png;
            byte[] data = png.EncodeToPNG();
            return data;
        }

        public byte[] CutScreenForPngByWebCam(float startX, float startY, float width, float height, WebCamTexture webCamTexture)
        {
            Texture2D png = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.ARGB32, false);
            Color[] colors = webCamTexture.GetPixels();
            png.SetPixels(colors);
            var data = png.EncodeToPNG();
            return data;
        }
        
        public static void SaveDataForPNG(string path,string pictureName)
        {
            //如果存储路径不存在，则创建文件夹
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string name = DateTime.Now.Year.ToString() + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour +
                          DateTime.Now.Minute + DateTime.Now.Second;
            //以 bytes 的数据在指定位置生成一个png文件

            File.WriteAllBytes(path + "/" + name + ".png", File.ReadAllBytes(path + pictureName));
           
        }
    }
}