/****************************************************
    文件：PhotoWallItem.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhotoWallItem : MonoBehaviour,IPointerClickHandler
{
    public RectTransform rectTransform { get; private set; }
    public bool IsCenter;
    private int _id;
    private float _maxScale;
    private float _showScale;
    private float _time;
    public void Init(int id,float maxScale,float showScale)
    {
        _id = id;
        rectTransform = GetComponent<RectTransform>();
        IsCenter = false;
        _maxScale = maxScale;
        _showScale = showScale;
        _time = 0.1f;
        GetComponentInChildren<Text>().text = id.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsCenter)
        {
            IsCenter = true;
            var sequence = DOTween.Sequence();
            sequence.Append(rectTransform.DOScale(_maxScale, _time));
            sequence.Append(rectTransform.DOScale(_showScale, _time));
        }
    }
}