/****************************************************
    文件：CutScreen.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：挂载到UI物体上生成对应的图片
*****************************************************/

using System.Collections;
using System.IO;
using UnityEngine;

namespace YFramework
{
    public class YPicture : MonoBehaviour 
    {
        public enum PictureType
        {
            PNG,
            JPG,
            EXR,
            TGA
        }

        public PictureType Type;
        private RectTransform _rectTrans;
        private byte[] _data;
        private string _defaultName;
        private int _startX;
        private int _startY;
        private int _width;
        private int _height;

        private void Start()
        {
            _rectTrans = GetComponent<RectTransform>();
            _startX = (int)_rectTrans.offsetMin.x;
            _startY = (int)_rectTrans.offsetMin.y;
            _width = (int) (Screen.width - _startX + _rectTrans.offsetMax.x);
            _height = (int) (Screen.height - _startY + _rectTrans.offsetMax.y);
        }

        public byte[] GetData() => GetData(Type);

        public byte[] GetData(PictureType type)
        {
            StartCoroutine(CorCut(type));
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
        public byte[] CutByWebCam(WebCamTexture webCamTexture) => CutByWebCam(webCamTexture, Type);
        public byte[] CutByWebCam(WebCamTexture webCamTexture,PictureType type)
        {
            StartCoroutine(CorWebCam(webCamTexture, type));
            return _data;
        }

        private IEnumerator CorWebCam(WebCamTexture webCamTexture,PictureType type)
        {
            yield return new WaitForEndOfFrame();
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

        public void SaveLocalFile(string path, byte[] pictureData, string pictureName) => SaveLocalFile(path, pictureData, Type, pictureName);
        public void SaveLocalFile(string path, byte[] pictureData,PictureType type) => SaveLocalFile(path, pictureData, Type, _defaultName);
        public void SaveLocalFile(string path, byte[] pictureData) => SaveLocalFile(path, pictureData, Type, _defaultName);

    }
}