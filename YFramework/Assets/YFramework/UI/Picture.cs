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
        

        public void CreatePictureToLocalFile(string path,UnityEngine.Camera camera)
        {
            StartCoroutine(CorCreatePictureToLocalFile(path,camera));
        }
     
        private IEnumerator CorCreatePictureToLocalFile(string path,UnityEngine.Camera camera)
        {
            yield return new WaitForEndOfFrame();
            var data = CutData(camera);
            SaveLocalFile(path,data,Type);
        }
        public byte[] Cut(UnityEngine.Camera camera)
        {
            StartCoroutine(CorCut(camera));
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

        private IEnumerator CorCut(UnityEngine.Camera camera)
        {
            yield return new WaitForEndOfFrame();
            CutData(camera);
        }

        private byte[] CutData(UnityEngine.Camera camera)
        {
            var texture2D = CaptureCamera(camera, new Rect(_startX, _startY, _width, _height));

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
        private Texture2D CaptureCamera(UnityEngine.Camera camera, Rect rect)   
        {  
            // 创建一个RenderTexture对象  
            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 0);  
            // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
            camera.targetTexture = rt;  
            camera.Render();  
            //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  
            //ps: camera2.targetTexture = rt;  
            //ps: camera2.Render();  
            //ps: -------------------------------------------------------------------  

            // 激活这个rt, 并从中中读取像素。  
            RenderTexture.active = rt;  
            Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32,false);  
            screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
            screenShot.Apply();  

            // 重置相关参数，以使用camera继续在屏幕上显示  
            camera.targetTexture = null;  
            //ps: camera2.targetTexture = null;  
            RenderTexture.active = null; // JC: added to avoid errors  
            GameObject.Destroy(rt);  
            // 最后将这些纹理数据，成一个png图片文件  
            // byte[] bytes = screenShot.EncodeToPNG();  
            // string filename = Application.dataPath + "/Screenshot.png";  
            // System.IO.File.WriteAllBytes(filename, bytes);  
            // Debug.Log(string.Format("截屏了一张照片: {0}", filename));  

            return screenShot;  
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