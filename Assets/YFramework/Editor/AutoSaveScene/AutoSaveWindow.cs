using UnityEditor;
using UnityEngine;

namespace YFramework.Editor
{
    public class AutoSaveWindow : EditorWindow
    {
        public static bool autoSaveScene
        {
            get => AutoSaveSettings.AutoSaveScene;
            set => AutoSaveSettings.AutoSaveScene = value;
        }

        public static bool showMessage
        {
            get => AutoSaveSettings.ShowMessage;
            set => AutoSaveSettings.ShowMessage = value;
        }

        public static int intervalTime
        {
            get => AutoSaveSettings.IntervalTime;
            set => AutoSaveSettings.IntervalTime = value;
        }

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

            EditorGUI.BeginChangeCheck();
            bool nextAutoSaveScene = EditorGUILayout.BeginToggleGroup("自动保存", autoSaveScene);
            int nextIntervalTime = EditorGUILayout.IntField("时间间隔(秒)", intervalTime);
            EditorGUILayout.EndToggleGroup();

            bool nextShowMessage = EditorGUILayout.BeginToggleGroup("显示消息", showMessage);
            EditorGUILayout.EndToggleGroup();

            if (EditorGUI.EndChangeCheck())
            {
                autoSaveScene = nextAutoSaveScene;
                intervalTime = nextIntervalTime;
                showMessage = nextShowMessage;
            }
        }
    }
}
