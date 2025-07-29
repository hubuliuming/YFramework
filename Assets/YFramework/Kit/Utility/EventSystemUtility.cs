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
        
        /// <summary>
        /// 放在UI的IPointClickHand方法后
        /// </summary>
        /// <param name="go"></param>
        /// <param name="eventData"></param>
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
        
        public static bool IsPointerOverUIElement()
        {
#if UNITY_IOS || UNITY_ANDROID && !UNITY_EDITOR
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        List<RaycastResult> results = new List<RaycastResult>();
        foreach (Touch touch in Input.touches)
        {
            eventData.position = touch.position;
            EventSystem.current.RaycastAll(eventData, results);
            if (results.Count > 0)
            {
                Debug.Log(results[0].gameObject.name);
                return true;
            }
        }
        return false;
#elif UNITY_EDITOR
            return EventSystem.current.IsPointerOverGameObject();
#endif
        }
    }
}