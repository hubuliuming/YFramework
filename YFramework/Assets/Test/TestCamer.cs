/****************************************************
    文件：TestCamer.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;

public class TestCamer : MonoBehaviour
{
    public Transform targetTrans;
    private float _mouseX;
    private float _mouseY;
    public float rotateSpeedX;
    public float rotateSpeedY;
    public float lerpSpeed;

    public float minDistance;
    public float maxDistance;
    
    public float minRotateYClampValue;
    public float maxRotateYClampValue;
    
    private float _distanceScaleTargetValue;
    private float _distanceScaleCurrentValue;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _mouseX = Input.GetAxis("Mouse X") * rotateSpeedX;
            _mouseY = Input.GetAxis("Mouse Y") * rotateSpeedY;
            _mouseY = Mathf.Clamp(_mouseY, minRotateYClampValue, maxRotateYClampValue);
        }
    }

    private void LateUpdate()
    {
        var targetRotation = Quaternion.Euler(_mouseY, _mouseY, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,Time.deltaTime * lerpSpeed);
        var targetPos = transform.rotation * new Vector3(0, 0, -GetCurrentPos()) + targetTrans.position;
        transform.position = targetPos;
    }

    private float GetCurrentPos()
    {
        _distanceScaleCurrentValue = Mathf.Lerp(_distanceScaleCurrentValue, _distanceScaleTargetValue, Time.deltaTime * lerpSpeed);
        return minDistance + (maxDistance - minDistance) * _distanceScaleCurrentValue;
    }
}