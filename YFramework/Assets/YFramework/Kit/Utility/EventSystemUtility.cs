/****************************************************
    文件：EventSystemUtility.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace YFramework.Kit.Utility
{
    public class EventSystemUtility 
    {
        private EventSystemUtility(){}
        
        public static void ExecuteClickAll(GameObject go,PointerEventData eventData) 
        {
            var list = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData,list);
            foreach (var result in list)
            {
                if (result.gameObject == go) continue;
                ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.pointerClickHandler);
            }
        }
    }
}