/****************************************************
    文件：ThreeDToUIEffect.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：3D物体飞向背包格子的效果
*****************************************************/

using System.Collections;
using UnityEngine;

public class ThreeDToUIEffect : MonoBehaviour 
{
    public RectTransform targetUI;
    public float moveSpeed = 5f;
    public float endMoveDistance = 60f;
    public bool open = false;
    private Vector3 _targetUIPos;
    private Vector3 _selfPos;
    private float _shrink = 1;
    void Start()
    {
        //Canvas 默认设置 用下面的
        _targetUIPos = (targetUI.position);
        //Canvas设置成Screen Space-Camera  可以用下面的
        //_targetUIPos = Camera.main.WorldToScreenPoint(target.position);
    }
    private void OnMouseDown()
    {     
        open = true;
        if (endMoveDistance < 0)
        {
            Debug.LogError("结束判断距离不可为负数");
            return;
        }
        StartCoroutine(CorClicked());
    }
    IEnumerator CorClicked()
    {
        while (open)
        {
            _selfPos = Camera.main.WorldToScreenPoint(transform.position);
            if (Vector3.Distance(_targetUIPos, _selfPos) > endMoveDistance)
            {
                _shrink -= Time.deltaTime;
                _selfPos = Vector3.MoveTowards (_selfPos, _targetUIPos, moveSpeed);               
                transform.position = Camera.main.ScreenToWorldPoint(_selfPos);
                transform.localScale = new Vector3(_shrink, _shrink, _shrink);
                if (_shrink<=0)
                {
                    transform.localScale = Vector3.zero;
                    open = false;
                }
            }
            else
            {
                transform.localScale = Vector3.zero;
                open = false;
                StartCoroutine(CorClicked());
            }
            yield return null;
        }

    }
}