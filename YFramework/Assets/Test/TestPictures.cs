/****************************************************
    文件：TestPictures.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;

public class TestPictures : MonoBehaviour
{
    public RectTransform area;
    public Rect rect;
    private void Start()
    {
        CutScreen cutScreen = new CutScreen(area.rect);

        Debug.Log(area.rect);
        //Debug.Log(Screen.width area.rect);
        cutScreen.SaveLocalFile("D://拍照照片/",cutScreen.Cut());
    }
}