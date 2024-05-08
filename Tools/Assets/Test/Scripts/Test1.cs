

using UnityEngine;
using UnityEngine.U2D;


public class Test1 : MonoBehaviour
{
    public BezierCurve BezierCurve;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BezierCurve.Show(BezierCurve.card.position);
        }
        if (Input.GetMouseButtonUp(0))
        {
            BezierCurve.Hide();
        }
    }
}

