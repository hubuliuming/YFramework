/****************************************************
    文件：C.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CustomUnity
{
    public class CustomMonoBehaviour : MonoBehaviour ,IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("dd");
        }
    }
}