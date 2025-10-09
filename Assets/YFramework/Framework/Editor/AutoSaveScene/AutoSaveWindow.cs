using UnityEditor;
using UnityEngine;

namespace YFramework.Editor
{
    public class AutoSaveWindow : EditorWindow
    {
        public static bool autoSaveScene = true;
        public static bool showMessage = false;
        public static int intervalTime = 30;

        [MenuItem("YFramework/AutoSaveScene")]
        static void Init()
        {
            EditorWindow saveWindow = GetWindow(typeof(AutoSaveWindow));
            saveWindow.minSize = new Vector2(200, 200);
            saveWindow.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("信息", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("保存场景:", "" + XPAutoSave.nowScene.path);
            GUILayout.Label("选择", EditorStyles.boldLabel);
            autoSaveScene = EditorGUILayout.BeginToggleGroup("自动保存", autoSaveScene);
            intervalTime = EditorGUILayout.IntField("时间间隔(秒)", intervalTime);
            EditorGUILayout.EndToggleGroup();
            showMessage = EditorGUILayout.BeginToggleGroup("显示消息", showMessage);
            EditorGUILayout.EndToggleGroup();
        }
    }
}