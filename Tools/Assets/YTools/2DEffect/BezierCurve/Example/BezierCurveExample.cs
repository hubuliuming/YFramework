/****************************************************
    文件：BezierCurveExample.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;

public class BezierCurveExample : MonoBehaviour 
{
    public BezierCurve BezierCurve;
    
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