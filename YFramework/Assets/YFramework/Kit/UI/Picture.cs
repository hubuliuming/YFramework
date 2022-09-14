/****************************************************
    文件：CutScreen.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：挂载到相应UGUI物体上即可以生成对应区域的图片
*****************************************************/

using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace YFramework.Kit
{
    /// <summary>
    /// 方法需要等这帧渲染完调用
    /// </summary>
    public class Picture : MonoBehaviour
    {
        public enum PictureType
        {
            PNG,
            JPG,
            EXR,
            TGA
        }
        public PictureType Type = PictureType.PNG;
        private RectTransform _rectTrans;
        private byte[] _data;

        public byte[] Data
        {
            get => _data;
        }

        private string _defaultName;
        private int _startX;
        private int _startY;
        private int _width;
        private int _height;


        private void Start()
        {
            _rectTrans = GetComponent<RectTransform>();
            _defaultName = DateTime.Now.ToString("yyyyMMddHHmmss");
            _startX = (int)(_rectTrans.position.x - MathF.Abs(_rectTrans.rect.xMin)); 
            _startY = (int)(_rectTrans.position.y - MathF.Abs(_rectTrans.rect.yMin)); 
            _width = (int)_rectTrans.sizeDelta.x;
            _height = (int)_rectTrans.sizeDelta.y;
        }
        

        public void CreatePictureToLocalFile(string path)
        {
            StartCoroutine(CorCreatePictureToLocalFile(path));
        }
     
        private IEnumerator CorCreatePictureToLocalFile(string path)
        {
            yield return new WaitForEndOfFrame();
            var data = CutData();
            SaveLocalFile(path,data,Type);
        }
        public byte[] Cut()
        {
            StartCoroutine(CorCut());
            return _data;
        }
        public byte[] CutByWebCam(WebCamTexture webCamTexture)
        {
            StartCoroutine(CorCutByWebCam(webCamTexture));
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
        public void SaveLocalFile(string path, byte[] pictureData,PictureType type) => SaveLocalFile(path, pictureData,type,_defaultName);

        private IEnumerator CorCut()
        {
            yield return new WaitForEndOfFrame();
            CutData();
        }

        private byte[] CutData()
        {
            Texture2D texture2D = new Texture2D(_width, _height, TextureFormat.ARGB32, false);
            texture2D.ReadPixels(new Rect(_startX,_startY,_width,_height),0,0,false);
            texture2D.Apply();

            byte[] data = null;
            switch (Type)
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
        private IEnumerator CorCutByWebCam(WebCamTexture webCamTexture)
        {
            yield return new WaitForEndOfFrame();
            CutDataByWebCam(webCamTexture);
        }

        private void CutDataByWebCam(WebCamTexture webCamTexture)
        {
            Texture2D texture2D = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.ARGB32, false);
            Color[] colors = webCamTexture.GetPixels();
            texture2D.SetPixels(colors);
            byte[] data = null;
            switch (Type)
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
        }
    }
}