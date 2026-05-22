using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using YFramework.UI;

namespace YFramework.Editor.UI
{
    public static class UIRootMenu
    {
        private const string kUILayerName = "UI";

        [MenuItem("GameObject/YFramework/UI/UIRoot", false, 2000)]
        private static void CreateUIRoot()
        {
            GameObject root = ObjectFactory.CreateGameObject(
                "UIRoot",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster),
                typeof(UIRoot));

            Canvas canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            root.layer = LayerMask.NameToLayer(kUILayerName);

            UIRoot uiRoot = root.GetComponent<UIRoot>();
            Undo.RegisterCreatedObjectUndo(root, "Create UIRoot");

            CreateLayerChild(uiRoot, root, "Bg");
            CreateLayerChild(uiRoot, root, "Player");
            CreateLayerChild(uiRoot, root, "Normal");
            CreateLayerChild(uiRoot, root, "Effect");
            CreateLayerChild(uiRoot, root, "Highest");

            StageUtility.PlaceGameObjectInCurrentStage(root);
            Selection.activeGameObject = root;
        }

        private static void CreateLayerChild(UIRoot uiRoot, GameObject parent, string name)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent.transform, false);

            SerializedObject so = new SerializedObject(uiRoot);
            SerializedProperty prop = so.FindProperty("m_" + ToCamelCase(name) + "Root");
            if (prop != null)
            {
                prop.objectReferenceValue = go.transform;
                so.ApplyModifiedProperties();
            }
        }

        private static string ToCamelCase(string pascal)
        {
            if (string.IsNullOrEmpty(pascal))
            {
                return pascal;
            }

            return char.ToLower(pascal[0]) + pascal.Substring(1);
        }
    }
}
