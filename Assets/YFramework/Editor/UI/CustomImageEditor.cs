/****************************************************
    文件：CustomImageEditor.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using YFramework.UI;

namespace YFramework.Editor.UI
{
    public class CustomImageEditor : UnityEditor.Editor
    {
        private const int UI_Layer = 5;

        [MenuItem("GameObject/UI/CustomImage", priority = 0)]
        private static void AddImage()
        {
            var  canvasTrans = GetCanvasTrans();
            var image = AddCustomImage();

            if (Selection.activeGameObject != null && Selection.activeGameObject.layer == UI_Layer)
            {
                image.SetParent(Selection.activeGameObject.transform);
            }
            else
            {
                image.SetParent(canvasTrans);
            }
            image.localPosition = Vector3.zero;
        }

        private static Transform GetCanvasTrans()
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                var canvasGo = new GameObject("Canvas");
                SetLayer(canvasGo);
                canvasGo.AddComponent<RectTransform>();
                canvasGo.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                canvasGo.AddComponent<CanvasScaler>();
                canvasGo.AddComponent<GraphicRaycaster>();
                return canvasGo.transform;
            }

            return canvas.transform;
        }

        private static void SetLayer(GameObject go)
        {
            go.layer = UI_Layer;
        }

        private static Transform AddCustomImage()
        {
            GameObject image = new GameObject("CustomImage");
            SetLayer(image);
            image.AddComponent<RectTransform>();
            image.AddComponent<PolygonCollider2D>();
            image.AddComponent<CustomImage>();
            return image.transform;
        }
    }
}