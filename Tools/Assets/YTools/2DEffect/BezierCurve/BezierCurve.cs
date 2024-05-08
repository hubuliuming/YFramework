using UnityEngine;

/// <summary>
/// 半成品，需要按需修改,注意不要放在UI下
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class BezierCurve : MonoBehaviour //IController
{
    private Vector3 start;
    private Vector3 head;
    private LineRenderer lineRenderer;
    private const int NumPoints = 50;
    private Vector3[] positions = new Vector3[NumPoints];

    private Camera mainCamera;
    //private CardDragSystem cardDragSystem;

    public Transform card;

    public GameObject goStart, goHead;

    private void Awake()
    {
        //mainCamera = this.GetSystem<CamSystem>().GameCamera;
        mainCamera = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = NumPoints;

        // this.RegisterEvent<CardDownEvent>(OnCardDown);
        // this.RegisterEvent<CardUpEvent>(OnCardUp);
        gameObject.SetActive(false);
        goStart.SetActive(false);
        goHead.SetActive(false);
    }

    private void OnDestroy()
    {
        // this.UnRegisterEvent<CardDownEvent>(OnCardDown);
        // this.UnRegisterEvent<CardUpEvent>(OnCardUp);
    }

    // private void OnCardUp(CardUpEvent _)
    // {
    //     Hide();
    // }

    // private void OnCardDown(CardDownEvent e)
    // {
    //     Show(e.card.transform.position);
    // }


    public void Show(Vector3 start)
    {
        gradient.mode = GradientMode.Fixed;
        positions = new Vector3[NumPoints];
        lineRenderer.SetPositions(positions);
        gameObject.SetActive(true);
        this.start = head = start;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        goStart.SetActive(false);
        goHead.SetActive(false);
    }


    private float distance;


    #region Exaple

    //另一个mono里调用
    // private void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         BezierCurve.Show(BezierCurve.card.position);
    //     }
    //     if (Input.GetMouseButtonUp(0))
    //     {
    //         BezierCurve.Hide();
    //     }
    // }

    #endregion
    private void Update()
    {
        distance = Vector3.Distance(start, head);
        var mousePos = Input.mousePosition;
        mousePos.z = 10.0f;
        head = mainCamera.ScreenToWorldPoint(mousePos);
        DrawBezierCurve();
        SetStart();
        SetHead();
        SetColor();
    }

    private void DrawBezierCurve()
    {
        var p1 = Vector3.Lerp(start, head, 0.5f) + Vector3.down * 5f;
        if (p1.y < start.y)
        {
            p1.y = start.y;
        }

        for (var i = 0; i < NumPoints; i++)
        {
            var t = i / (float) (NumPoints - 1);
            positions[i] = CalculateBezierPoint(t, head, p1, start);
        }

        lineRenderer.SetPositions(positions);
    }

    private void SetHead()
    {
        goHead.transform.position = head;
        goHead.transform.rotation = Quaternion.LookRotation(Vector3.forward, positions[0] - positions[5]);
        goHead.SetActive(distance > 1.2f);
    }

    private void SetStart()
    {
        goStart.transform.position = start;
        goStart.SetActive(true);
    }

    private readonly Gradient gradient = new Gradient();

    private void SetColor()
    {
        //小于0.5f距离的部分为透明
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(Color.white, 0),
                new GradientColorKey(Color.white, 1)
            },
            new[]
            {
                new GradientAlphaKey(0, 1.2f / distance),
                new GradientAlphaKey(1, 1)
            });
        lineRenderer.colorGradient = gradient;
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        return (1 - t) * (1 - t) * p0 + 2 * (1 - t) * t * p1 + t * t * p2;
    }

    // public IArchitecture GetArchitecture()
    // {
    //     return Main.I;
    // }
}