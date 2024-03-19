/****************************************************
    文件：HpBarControl.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：血条UI简单的变化
*****************************************************/

using System;
using UnityEngine;

public class HpBarControl : MonoBehaviour
{
    public RectTransform hp;
    public RectTransform hpFade;
    public int maxHp = 100;
    public float fadeSpeed = 2;
    
    /// <summary>
    /// 血条可变化的长度，一般为rect的长度，有特殊减掉一点的需要另外设置
    /// </summary>
    private float _hpWidth;
    public int curHp => _curHp;
    private int _curHp;
    private int _changHp;
    //true 为加血，false减血
    private bool _addOrSub;

    private void Start()
    {
        //把中心点归在左边
        // hp.pivot = new Vector2(0, 0.5);
        // hpFade.pivot = new Vector2(0, 0.5f);
        _hpWidth = hp.rect.width;
        _curHp = maxHp;
    }

    //简单做一下血条渐变
    private void Update()
    {
        if(Math.Abs(hp.sizeDelta.x - hpFade.sizeDelta.x) < 0.01f) return;
       
        //true 为加血，false减血
        if (_addOrSub)
        {
            var x = Mathf.Lerp(hp.sizeDelta.x, hpFade.sizeDelta.x, fadeSpeed * Time.deltaTime);
            if (Math.Abs(hp.sizeDelta.x - hpFade.sizeDelta.x) < 0.1f) x = hpFade.sizeDelta.x;
            hp.sizeDelta = new Vector2(x, hp.sizeDelta.y);  
        }
        else
        {
            var x = Mathf.Lerp(hpFade.sizeDelta.x, hp.sizeDelta.x, fadeSpeed * Time.deltaTime);
            if (Math.Abs(hp.sizeDelta.x - hpFade.sizeDelta.x) < 0.1f) x = hp.sizeDelta.x;
            hpFade.sizeDelta = new Vector2(x, hpFade.sizeDelta.y);
        }
    }

    public void AddHp(int value)
    {
        if (curHp + value >= maxHp)
        {
            hpFade.sizeDelta = new Vector2(_hpWidth, hpFade.sizeDelta.y);
            _curHp = maxHp;
        }
        else
        {
            var ratio = (curHp + value) / (float) maxHp;
            hpFade.sizeDelta = new Vector2(_hpWidth * ratio, hpFade.sizeDelta.y);
            _curHp += value;
        }

        _addOrSub = true;
    }

    public void SubtractHp(int value)
    {
        if(curHp - value <= 0)
        {
            hp.sizeDelta = new Vector2(0, hp.sizeDelta.y);
            _curHp = 0;
        }
        else
        {
            var ratio = (curHp - value) / (float) maxHp;
            hp.sizeDelta = new Vector2(_hpWidth * ratio, hp.sizeDelta.y);
            _curHp -= value;
        }
        _addOrSub = false;
    }
}