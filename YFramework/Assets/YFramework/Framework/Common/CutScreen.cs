/****************************************************
    文件：CutScreen.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：截取屏幕上的像素
*****************************************************/

using System;
using System.IO;
using UnityEngine;

public class CutScreen
{
    public enum PictureType
    {
        PNG,
        JPG,
        EXR,
        TGA
    }
    
    private Rect _rect;
    private RectTransform _rectTrans;
    private PictureType _type;
    private string _defaultName;

    /// <summary>
    /// 截取屏幕上的像素操作类
    /// </summary>
    /// <param name="rect">目标范围Rect</param>
    /// <param name="type"></param>
    public CutScreen(Rect rect,PictureType type = PictureType.PNG)
    {
        _defaultName = DateTime.Now.ToString("yyyyMMddHHmmss");
        this._rect = rect;
        this._type = type;
    }
    public byte[] Cut() => Cut(_type);

    public byte[] Cut(PictureType type)
    {
        Texture2D texture2D = new Texture2D((int) _rect.width, (int) _rect.height, TextureFormat.ARGB32, false);
        texture2D.ReadPixels(new Rect(0,0,400,400),0,0,false);
        texture2D.Apply();
        byte[] data = null;
        switch (type)
        {
            case PictureType.PNG:
                data = texture2D.EncodeToPNG();
                break;
            case PictureType.JPG:
                data = texture2D.EncodeToJPG();
                break;
            case PictureType.EXR:
                data = texture2D.EncodeToEXR();
                break;
            case PictureType.TGA:
                data = texture2D.EncodeToTGA();
                break;
        }

        if (data == null)
            Debug.LogError("转化图片失败");
        return data;
    }
    public byte[] CurByWebCam(WebCamTexture webCamTexture) => CurByWebCam(webCamTexture, _type);
    public byte[] CurByWebCam(WebCamTexture webCamTexture,PictureType type)
    {
        Texture2D texture2D = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.ARGB32, false);
        Color[] colors = webCamTexture.GetPixels();
        texture2D.SetPixels(colors);
        byte[] data = null;
        switch (type)
        {
            case PictureType.PNG:
                data = texture2D.EncodeToPNG();
                break;
            case PictureType.JPG:
                data = texture2D.EncodeToJPG();
                break;
            case PictureType.EXR:
                data = texture2D.EncodeToEXR();
                break;
            case PictureType.TGA:
                data = texture2D.EncodeToTGA();
                break;
        }

        if (data == null)
            Debug.LogError("转化图片失败");
        return data;
    }

    public void SaveLocalFile(string path,byte[] pictureData,PictureType type,string pictureName )
    {
        //如果存储路径不存在，则创建文件夹
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        File.WriteAllBytes(path + "/" + pictureName+"."+ type.ToString(), pictureData);
    }

    public void SaveLocalFile(string path, byte[] pictureData, string pictureName) => SaveLocalFile(path, pictureData, _type, pictureName);
    public void SaveLocalFile(string path, byte[] pictureData,PictureType type) => SaveLocalFile(path, pictureData, _type, _defaultName);
    public void SaveLocalFile(string path, byte[] pictureData) => SaveLocalFile(path, pictureData, _type, _defaultName);

}