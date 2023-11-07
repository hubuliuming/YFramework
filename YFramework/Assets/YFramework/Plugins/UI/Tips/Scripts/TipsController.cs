/****************************************************
    文件：TipsController.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YFramework;
using YFramework.Extension;

/****************************************************
    文件：TipsController.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

/// <summary>
/// 实用性不高，等要用再完善
/// </summary>
[Obsolete("实用性不高，只复制过功能，未完成适配",true)]
public class TipsController : MonoBehaviour
{
    public Text[] txts;
    public Image[] imgs;
    public Sprite[] sprs;
    
    private CanvasGroup _canvasGroup;
    private ContentSizeFitter[] _txtsFitters;
    private RectTransform _rectTransform;
    private HorizontalLayoutGroup _layout;

    private void Awake()
    {
        _layout = GetComponent<HorizontalLayoutGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _txtsFitters = new ContentSizeFitter[txts.Length];
        _canvasGroup = GetComponent<CanvasGroup>();
        for (int i = 0; i < txts.Length; i++)
        {
            _txtsFitters[i] = txts[i].GetComponent<ContentSizeFitter>();
        }
    }

    private void OnEnable()
    {
        // GameLoop.Instance.EventToDo(() =>
        // {
        //     var texts = new string[]
        //     {
        //         LanguagesSystem.GetUIString(MsgUIText.Use),
        //         LanguagesSystem.GetUIString(MsgUIText.CreatePlayerAddPoiont),
        //         LanguagesSystem.GetUIString(MsgUIText.Confirmation)
        //     };
        //     foreach (var spr in sprs)
        //     {
        //         Debug.Log(spr.name);
        //     }
        //
        //     ConditionsShow(texts, sprs);
        // }, new WaitForSeconds(0.03f));
    }

    private void ConditionsShow(string[] texts, Sprite[] sprites)
    {
        for (int i = 0; i < txts.Length; i++)
        {
            if (i <= texts.Length - 1)
            {
                txts[i].gameObject.SetActive(true);
                txts[i].text = texts[i];
            }
            else
            {
                txts[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < imgs.Length; i++)
        {
            if (i <= sprites.Length - 1)
            {
                imgs[i].gameObject.SetActive(true);
                imgs[i].sprite = sprites[i];
                imgs[i].SetNativeSize();
            }
            else
            {
                imgs[i].gameObject.SetActive(false);
            }
        }
        MonoManager.Instance.StartCoroutine(FitterOperation(texts.Length));
    }

    /// <summary>
    /// 根据不同操作文字展示数量适配框
    /// </summary>
    /// <param name="num">展示的文字数</param>
    /// <returns></returns>
    private IEnumerator FitterOperation(int num)
    {
        _canvasGroup.alpha = 0;
        for (int i = 0; i < num; i++)
        {
            _txtsFitters[i].enabled = true;
            yield return null;
        }

        SetCenterSize();
        yield return null;
        for (int i = 0; i < num; i++)
        {
            _txtsFitters[i].enabled = false;
        }

        _canvasGroup.alpha = 1;
    }

    private void SetCenterSize()
    {
        var sumWidth = 0;
        var index = 0;
        foreach (var t in transform.GetActiveGameObjectsInChildren())
        {
            sumWidth += (int) t.GetComponent<RectTransform>().sizeDelta.x;
        }

        for (int i = 0; i < index - 1; i++)
        {
            sumWidth += (int) _layout.spacing;
        }

        _layout.padding.left = -sumWidth / 2 + (int) _rectTransform.sizeDelta.x / 2;
        _layout.CalculateLayoutInputHorizontal();
    }
}