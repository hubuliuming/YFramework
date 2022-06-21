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

    private RenderTexture _texRender;   //画布
    private float _lastDistance;
    private Vector3 _startPosition = Vector3.zero;
    private Vector3 _endPosition = Vector3.zero;
    private readonly Vector3[] _positionArray = new Vector3[3];
    private readonly Vector3[] _positionArray1 = new Vector3[4];
    // private int _width;
    // private int _height;
    private int a = 0;
    private int b = 0;
    private int s = 0;
    private float[] speedArray = new float[4];

    void Start()
    {
        //Debug.Log(raw.rectTransform.sizeDelta);
        // _width = (int) raw.rectTransform.sizeDelta.x;
        // _height = (int) raw.rectTransform.sizeDelta.y;
         _texRender = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        //_texRender = new RenderTexture(_width, _height, 24, RenderTextureFormat.ARGB32);
        Clear(_texRender);
    }
    
    void Update()
    {
        if (Input.GetMouseButton(0))
        { 
            if (Input.mousePosition.y > Screen.height + validPaintArea.offsetMax.y || 
                Input.mousePosition.y < validPaintArea.offsetMin.y ||
                Input.mousePosition.x < validPaintArea.offsetMin.x || 
                Input.mousePosition.x > Screen.width +validPaintArea.offsetMax.x)
            {
                return;
            }

            OnMouseMove(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseUp();
        }
        DrawImage();
    }
    
    public void RemoveText()
    {
        _texRender = null;
        _texRender = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        Clear(_texRender);
    }


    void OnMouseUp()
    {
        _startPosition = Vector3.zero;
        //brushScale = 0.5f;
        a = 0;
        b = 0;
        s = 0;
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
        //brushScale = SetScale(distance);
        ThreeOrderBézierCurse(pos, distance, 4.5f);

        _startPosition = _endPosition;
        _lastDistance = distance;
    }

    void Clear(RenderTexture destTexture)
    {
        Graphics.SetRenderTarget(destTexture);
        GL.PushMatrix();
        // GL.Clear(true, true, Color.red);
        GL.Clear(true, true, new Color(0,0,0,0));
        GL.PopMatrix();
    }

    void DrawBrush(RenderTexture destTexture, int x, int y, Texture sourceTexture, Color color, float scale)
    {
        DrawBrush(destTexture, new Rect(x, y, sourceTexture.width, sourceTexture.height), sourceTexture, color, scale);
    }
    void DrawBrush(RenderTexture destTexture, Rect destRect, Texture sourceTexture, Color color, float scale)
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
    void DrawImage()
    {
        raw.texture = _texRender;
    }
    public void OnClickClear()
    {
        Clear(_texRender);
    }

    //二阶贝塞尔曲线
    public void TwoOrderBézierCurse(Vector3 pos, float distance)
    {
        _positionArray[a] = pos;
        a++;
        if (a == 3)
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
            a = 2;
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
        _positionArray1[b] = pos;
        b++;
        //记录速度
        speedArray[s] = distance;
        s++;
        if (b == 4)
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
                float deltaspeed = (float)(speedArray[3] - speedArray[0]) / num;
                //float randomOffset = Random.Range(-1/(speedArray[0] + (deltaspeed * index1)), 1 / (speedArray[0] + (deltaspeed * index1)));
                //模拟毛刺效果
                float randomOffset = Random.Range(-targetPosOffset, targetPosOffset);
                DrawBrush(_texRender, (int)(target.x + randomOffset), (int)(target.y + randomOffset), brushTypeTexture, brushColor, SetScale(speedArray[0] + (deltaspeed * index1)));
            }

            _positionArray1[0] = temp1;
            _positionArray1[1] = temp2;
            _positionArray1[2] = _positionArray1[3];

            speedArray[0] = speedArray[1];
            speedArray[1] = speedArray[2];
            speedArray[2] = speedArray[3];
            b = 3;
            s = 3;
        }
        else
        {
            DrawBrush(_texRender, (int)_endPosition.x, (int)_endPosition.y, brushTypeTexture,
                brushColor, brushScale);
        }

    }
}
