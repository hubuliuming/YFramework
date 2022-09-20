using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Painting : MonoBehaviour
{
    public RawImage raw; //使用UGUI的RawImage显示，方便进行添加UI
    public RectTransform validPaintArea; //有效画的区域，注意改UI要锚点要全屏
    public Material mat;     //给定的shader新建材质
    public Texture brushTypeTexture;   //画笔纹理，半透明
    public float brushScale = 0.2f;
    public Color brushColor = Color.black;
    public int num = 50;
    public Button BtnCutPicture;
    public RectTransform CutPictureArea;
    public string SavePicturePath;
    public YPicture.PictureType PictureType = YPicture.PictureType.PNG;

    private RenderTexture _texRender; //画布 
    private bool _draw;
    private float _lastDistance;
    private Vector3 _startPosition = Vector3.zero;
    private Vector3 _endPosition = Vector3.zero;
    private readonly Vector3[] _positionArray = new Vector3[3];
    private readonly Vector3[] _positionArray1 = new Vector3[4];
    private int _a = 0;
    private int _b = 0;
    private int _s = 0;
    private float[] _speedArray = new float[4];

    void Start()
    {
        _texRender = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        Clear(_texRender);
        UpdateRawTextrue();
        if (BtnCutPicture != null)
        {
            BtnCutPicture.onClick.AddListener(() =>
            {
                StartCoroutine(CorCutPicture());
            });
        }
        
    }
    void Update()
    {
        
        if (Input.GetMouseButton(0))
        {
            _draw = true;
        }

        if (_draw)
        {
            if (Input.mousePosition.y > Screen.height + validPaintArea.offsetMax.y || 
                Input.mousePosition.y < validPaintArea.offsetMin.y ||
                Input.mousePosition.x < validPaintArea.offsetMin.x || 
                Input.mousePosition.x > Screen.width +validPaintArea.offsetMax.x)
            {
                _draw = false;
                return;
            }
            OnMouseMove(Input.mousePosition);
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            _draw = false;
            OnMouseUp();
        }
       
    }
    public void RemoveText()
    {
        _texRender = null;
        _texRender = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        Clear(_texRender);
        UpdateRawTextrue();
    }
    void OnMouseUp()
    {
        _startPosition = Vector3.zero;
        _a = 0;
        _b = 0;
        _s = 0;
    }
    //设置画笔宽度
    float SetScale(float distance)
    {
        float Scale = 0.1f;

        return Scale;
    }

    void OnMouseMove(Vector3 pos)
    {
        if (_startPosition == Vector3.zero)
        {
            _startPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        }

        _endPosition = pos;
        float distance = Vector3.Distance(_startPosition, _endPosition);
        ThreeOrderBézierCurse(pos, distance, 4.5f);
        _startPosition = _endPosition;
        _lastDistance = distance;
    }
    private void Clear(RenderTexture destTexture)
    {
        Graphics.SetRenderTarget(destTexture);
        GL.PushMatrix();
        // GL.Clear(true, true, Color.red);
        GL.Clear(true, true, new Color(0,0,0,0));
        GL.PopMatrix();
    }
    private void DrawBrush(RenderTexture destTexture, int x, int y, Texture sourceTexture, Color color, float scale)
    {
        DrawBrush(destTexture, new Rect(x, y, sourceTexture.width, sourceTexture.height), sourceTexture, color, scale);
    }
    private void DrawBrush(RenderTexture destTexture, Rect destRect, Texture sourceTexture, Color color, float scale)
    {
        float left = destRect.xMin - destRect.width * scale / 2.0f;
        float right = destRect.xMin + destRect.width * scale / 2.0f;
        float top = destRect.yMin - destRect.height * scale / 2.0f;
        float bottom = destRect.yMin + destRect.height * scale / 2.0f;

        Graphics.SetRenderTarget(destTexture);

        GL.PushMatrix();
        GL.LoadOrtho();

        mat.SetTexture("_MainTex", brushTypeTexture);
        mat.SetColor("_Color", color);
        mat.SetPass(0);

        GL.Begin(GL.QUADS);

        GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(left / Screen.width, top / Screen.height, 0);
        GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(right / Screen.width, top / Screen.height, 0);
        GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(right / Screen.width, bottom / Screen.height, 0);
        GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(left / Screen.width, bottom / Screen.height, 0);

        GL.End();
        GL.PopMatrix();
    }
    private void UpdateRawTextrue()
    {
        raw.texture = _texRender;
    }
    public void OnClickClear()
    {
        Clear(_texRender);
    }

    //二阶贝塞尔曲线
    private void TwoOrderBézierCurse(Vector3 pos, float distance)
    {
        _positionArray[_a] = pos;
        _a++;
        if (_a == 3)
        {
            for (int index = 0; index < num; index++)
            {
                Vector3 middle = (_positionArray[0] + _positionArray[2]) / 2;
                _positionArray[1] = (_positionArray[1] - middle) / 2 + middle;

                float t = (1.0f / num) * index / 2;
                Vector3 target = Mathf.Pow(1 - t, 2) * _positionArray[0] + 2 * (1 - t) * t * _positionArray[1] +
                                 Mathf.Pow(t, 2) * _positionArray[2];
                float deltaSpeed = (float)(distance - _lastDistance) / num;
                DrawBrush(_texRender, (int)target.x, (int)target.y, brushTypeTexture, brushColor, SetScale(_lastDistance + (deltaSpeed * index)));
            }
            _positionArray[0] = _positionArray[1];
            _positionArray[1] = _positionArray[2];
            _a = 2;
        }
        else
        {
            DrawBrush(_texRender, (int)_endPosition.x, (int)_endPosition.y, brushTypeTexture,
                brushColor, brushScale);
        }
    }
    //三阶贝塞尔曲线，获取连续4个点坐标，通过调整中间2点坐标，画出部分（我使用了num/1.5实现画出部分曲线）来使曲线平滑;通过速度控制曲线宽度。
    private void ThreeOrderBézierCurse(Vector3 pos, float distance, float targetPosOffset)
    {
        //记录坐标
        _positionArray1[_b] = pos;
        _b++;
        //记录速度
        _speedArray[_s] = distance;
        _s++;
        if (_b == 4)
        {
            Vector3 temp1 = _positionArray1[1];
            Vector3 temp2 = _positionArray1[2];

            //修改中间两点坐标
            Vector3 middle = (_positionArray1[0] + _positionArray1[2]) / 2;
            _positionArray1[1] = (_positionArray1[1] - middle) * 1.5f + middle;
            middle = (temp1 + _positionArray1[3]) / 2;
            _positionArray1[2] = (_positionArray1[2] - middle) * 2.1f + middle;

            for (int index1 = 0; index1 < num / 1.5f; index1++)
            {
                float t1 = (1.0f / num) * index1;
                Vector3 target = Mathf.Pow(1 - t1, 3) * _positionArray1[0] +
                                 3 * _positionArray1[1] * t1 * Mathf.Pow(1 - t1, 2) +
                                 3 * _positionArray1[2] * t1 * t1 * (1 - t1) + _positionArray1[3] * Mathf.Pow(t1, 3);
                //float deltaspeed = (float)(distance - lastDistance) / num;
                //获取速度差值（存在问题，参考）
                float deltaspeed = (float)(_speedArray[3] - _speedArray[0]) / num;
                //float randomOffset = Random.Range(-1/(speedArray[0] + (deltaspeed * index1)), 1 / (speedArray[0] + (deltaspeed * index1)));
                //模拟毛刺效果
                float randomOffset = Random.Range(-targetPosOffset, targetPosOffset);
                DrawBrush(_texRender, (int)(target.x + randomOffset), (int)(target.y + randomOffset), brushTypeTexture, brushColor, SetScale(_speedArray[0] + (deltaspeed * index1)));
            }

            _positionArray1[0] = temp1;
            _positionArray1[1] = temp2;
            _positionArray1[2] = _positionArray1[3];

            _speedArray[0] = _speedArray[1];
            _speedArray[1] = _speedArray[2];
            _speedArray[2] = _speedArray[3];
            _b = 3;
            _s = 3;
        }
        else
        {
            DrawBrush(_texRender, (int)_endPosition.x, (int)_endPosition.y, brushTypeTexture,
                brushColor, brushScale);
        }

    }

    private IEnumerator CorCutPicture()
    {
        var pic = new YPicture(CutPictureArea,PictureType);
        yield return new WaitForEndOfFrame();
        var data = pic.Cut();
        pic.SaveLocalFile(SavePicturePath,data);
        RemoveText();
    }
}

/// <summary>
/// 方法需要等这帧渲染完调用,和YFramework里名字功能一样的，如有重名功能按照YFramework为准
/// </summary>
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

    public YPicture(RectTransform rectTrans, PictureType type = PictureType.PNG)
    {
        this._rectTrans = rectTrans;
        this._type = type;
        _defaultName = DateTime.Now.ToString("yyyyMMddHHmmss");
        _startX = (int) (_rectTrans.position.x - MathF.Abs(_rectTrans.rect.xMin));
        _startY = (int) (_rectTrans.position.y - MathF.Abs(_rectTrans.rect.yMin));
        _width = (int) _rectTrans.sizeDelta.x;
        _height = (int) _rectTrans.sizeDelta.y;
    }

    public byte[] Cut(PictureType type = PictureType.PNG)
    {
        Texture2D texture2D = new Texture2D(_width, _height, TextureFormat.ARGB32, false);
        texture2D.ReadPixels(new Rect(_startX, _startY, _width, _height), 0, 0, false);
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

    public byte[] CutByWebCam(WebCamTexture webCamTexture) => CutByWebCam(webCamTexture, _type);

    public byte[] CutByWebCam(WebCamTexture webCamTexture, PictureType type)
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

    public void SaveLocalFile(string path, byte[] pictureData, PictureType type, string pictureName)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        File.WriteAllBytes(path + "/" + pictureName + "." + type.ToString(), pictureData);
    }

    public void SaveLocalFile(string path, byte[] pictureData, string pictureName) => SaveLocalFile(path, pictureData, _type, pictureName);
    public void SaveLocalFile(string path, byte[] pictureData, PictureType type) => SaveLocalFile(path, pictureData, _type, _defaultName);
    public void SaveLocalFile(string path, byte[] pictureData) => SaveLocalFile(path, pictureData, _type, _defaultName);
}
