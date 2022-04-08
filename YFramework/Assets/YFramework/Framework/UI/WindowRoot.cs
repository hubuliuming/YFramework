/****************************************************
    文件：WindowRoot.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：2021/10/25 9:54:40
    功能：UIWindow的基类
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour
{
    public void SetWindState(bool isActive = true)
    {
        if (gameObject.activeSelf != isActive)
        {
            gameObject.SetActive(isActive);
        }
    }

    #region ToolFuntios
    protected void SetActive(GameObject go, bool isActive = true)
    {
        go.SetActive(isActive);
    }
    protected void SetActive(Transform trans, bool isActive = true)
    {
        trans.gameObject.SetActive(isActive);
    }
    protected void SetActive(RectTransform rectTrans, bool isActive = true)
    {
        rectTrans.gameObject.SetActive(isActive);
    }
    protected void SetActive(Text text, bool isActive = true)
    {
        text.gameObject.SetActive(isActive);
    }
    protected void SetActive(Image img, bool isActive = true)
    {
        img.gameObject.SetActive(isActive);
    }
    protected void SetText(Text text, string context = "")
    {
        text.text = context;
    }
    protected void SetText(Text text,int num =0)
    {
        SetText(text,num.ToString());
    }
    protected void SetText(Transform trans,string context ="")
    {
        SetText(trans.GetComponent<Text>(),context);
    }
    protected void SetText(Transform trans,int num =0)
    {
       SetText(trans.GetComponent<Text>(),num.ToString());
    }
    protected void SetImage(Image image,string path)
    {
        Sprite sprite = Resources.Load<Sprite>(path);
        image.sprite = sprite;
    }
    protected T GetOrAddComponent<T>(GameObject go) where T: Component
    {
        T t = go.GetComponent<T>();
        if (t == null)
        {
            t =go.AddComponent<T>();
        }

        return t;
    }

    #endregion

    #region ClickEvents

    protected void OnClick(GameObject go, Action<object> cb,object args)
    {
        YListener listener = GetOrAddComponent<YListener>(go);
        listener.OnClick = cb;
        listener.args = args;
    }
    protected void OnClickDown(GameObject go, Action<PointerEventData> cb)
    {
        YListener listener = GetOrAddComponent<YListener>(go);
        listener.OnClickDown = cb;
    }
    protected void OnClickUp(GameObject go, Action<PointerEventData> cb)
    {
        YListener listener = GetOrAddComponent<YListener>(go);
        listener.OnClickUp = cb;
    }
    protected void OnClickDrag(GameObject go, Action<PointerEventData> cb)
    {
        YListener listener = GetOrAddComponent<YListener>(go);
        listener.OnClickDrag = cb;
    }
    #endregion
}