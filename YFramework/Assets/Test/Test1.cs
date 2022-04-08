/****************************************************
    文件：Test1.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class Test1 : MonoBehaviour
{
    public GameObject go;
    public Vector3 startPos = Vector3.zero;
    public Vector3 endPos = new Vector3(100, 0, 0);
    private bool isMove;
    private float curTime;
    private float totalTime = 5;

    private void Start()
    {
        startPos = go.transform.localPosition;
        Debug.Log(startPos);
    }

    private void Update()
    {
        Test2();
    }
    
    
    private void Test2()
    {
        if (isMove)
        {
            curTime += Time.deltaTime;
        }
      
        if (curTime >= totalTime)
        {
            curTime = 0;
            isMove = false;
        }
        
        Debug.Log(curTime);
        
        go.transform.localPosition = Move(startPos, endPos, curTime, 5);
    }
    
    
    private float EaseInSide(float x)
    {
        return 1 - Mathf.Cos(x * Mathf.PI / 2);
    }

    private float Move(float a, float b, float curTime,float totalTime)
    {
        isMove = true;
        return EaseInSide(curTime/totalTime) * (b - a) + a;
    }
    private Vector3 Move(Vector3 a, Vector3 b, float curTime,float totalTime)
    {
        isMove = true;
        return EaseInSide(curTime/totalTime) * (b - a) + a;
    }
    
}