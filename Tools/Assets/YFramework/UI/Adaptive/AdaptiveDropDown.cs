/****************************************************
    文件：AdaptiveDropDown.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YFramework.UI
{
    [RequireComponent(typeof(Dropdown))]
    public class AdaptiveDropDown : Dropdown 
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
        }
    }
}