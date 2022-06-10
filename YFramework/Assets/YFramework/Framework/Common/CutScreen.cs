/****************************************************
    文件：CutScreen.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：截取屏幕上的像素
*****************************************************/

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
    public CutScreen(Rect rect)
    {
        _rect = rect;
    }

    public byte[] Cut(PictureType type)
    {
        Texture2D texture2D = new Texture2D((int) _rect.width, (int) _rect.height, TextureFormat.ARGB32, false);
        texture2D.ReadPixels(_rect,0,0);
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

    public byte[] CurByWebCam(PictureType type,WebCamTexture webCamTexture)
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


}