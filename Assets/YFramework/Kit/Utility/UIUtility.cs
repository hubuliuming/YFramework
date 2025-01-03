using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YFramework.Kit
{
    public class UIUtility
    {
        public static bool IsUI(GraphicRaycaster gr)
        {
            PointerEventData data = new PointerEventData(EventSystem.current);
            data.pressPosition = Input.mousePosition;
            data.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(data,results);
            return results.Count > 0;
        }
    }
}