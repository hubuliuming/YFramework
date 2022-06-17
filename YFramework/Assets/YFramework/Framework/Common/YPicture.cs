/****************************************************
    文件：CutScreen.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：挂载到UI物体上生成对应的图片
*****************************************************/

using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace YFramework
{
    public class YPicture 
    {
        public enum PictureType
        {
            PNG,
            JPG,
            EXR,
            TGA
        }

        private PictureType _type;
        private RectTransform _rectTrans;
        private byte[] _data;
        private string _defaultName;
        private int _startX;
        private int _startY;
        private int _width;
        private int _height;

        public YPicture(RectTransform rectTrans,PictureType type = PictureType.PNG)
        {
            this._rectTrans = rectTrans;
            this._type = type;
            _defaultName = DateTime.Now.ToString("yyyyMMddHHmmss");
            _startX = (int)_rectTrans.position.x;
            _startY = (int)_rectTrans.position.y;
            _width = (int)_rectTrans.sizeDelta.x;
            _height = (int)_rectTrans.sizeDelta.y;
            Debug.Log(_startX);
            Debug.Log(_startY);
            Debug.Log(_width);
            Debug.Log(_height);
        }

        public byte[] GetData() => GetData(_type);

        public byte[] GetData(PictureType type)
        {
            Texture2D texture2D = new Texture2D(_width, _height, TextureFormat.ARGB32, false);
            texture2D.ReadPixels(new Rect(_startX,_startY,_width,_height),0,0,false);
            texture2D.Apply();
        
            switch (type)
            {
                case PictureType.PNG:
                    _data = texture2D.EncodeToPNG();
                    break;
                case PictureType.JPG:
                    _data = texture2D.EncodeToJPG();
                    break;
                case PictureType.EXR:
                    _data = texture2D.EncodeToEXR();
                    break;
                case PictureType.TGA:
                    _data = texture2D.EncodeToTGA();
                    break;
            }

            if (_data == null)
                Debug.LogError("转化图片失败");
            return _data;
        }

        private IEnumerator CorCut(PictureType type)
        {
            yield return new WaitForEndOfFrame();
            Texture2D texture2D = new Texture2D(_width, _height, TextureFormat.ARGB32, false);
            texture2D.ReadPixels(new Rect(_startX,_startY,_width,_height),0,0,false);
            texture2D.Apply();
        
            switch (type)
            {
                case PictureType.PNG:
                    _data = texture2D.EncodeToPNG();
                    break;
                case PictureType.JPG:
                    _data = texture2D.EncodeToJPG();
                    break;
                case PictureType.EXR:
                    _data = texture2D.EncodeToEXR();
                    break;
                case PictureType.TGA:
                    _data = texture2D.EncodeToTGA();
                    break;
            }

            if (_data == null)
                Debug.LogError("转化图片失败");
        }
        public byte[] CutByWebCam(WebCamTexture webCamTexture) => CutByWebCam(webCamTexture, _type);
        public byte[] CutByWebCam(WebCamTexture webCamTexture,PictureType type)
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
            return _data;
        }
        

        public void SaveLocalFile(string path,byte[] pictureData,PictureType type,string pictureName )
        {
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
}