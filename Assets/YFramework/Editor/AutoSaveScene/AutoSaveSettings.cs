using UnityEditor;
using UnityEngine;

namespace YFramework.Editor
{
    public class AutoSaveSettings : ScriptableObject
    {
        private const string AssetPath = "Assets/YFramework/Editor/AutoSaveScene/AutoSaveSettings.asset";

        [SerializeField] private bool autoSaveScene = true;
        [SerializeField] private bool showMessage = false;
        [SerializeField] private int intervalTime = 30;

        private static AutoSaveSettings instance;

        public static bool AutoSaveScene
        {
            get => Instance.autoSaveScene;
            set
            {
                if (Instance.autoSaveScene == value) return;
                Instance.autoSaveScene = value;
                Save();
            }
        }

        public static bool ShowMessage
        {
            get => Instance.showMessage;
            set
            {
                if (Instance.showMessage == value) return;
                Instance.showMessage = value;
                Save();
            }
        }

        public static int IntervalTime
        {
            get => Instance.intervalTime;
            set
            {
                value = Mathf.Max(1, value);
                if (Instance.intervalTime == value) return;
                Instance.intervalTime = value;
                Save();
            }
        }

        private static AutoSaveSettings Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                instance = AssetDatabase.LoadAssetAtPath<AutoSaveSettings>(AssetPath);
                if (instance != null)
                {
                    return instance;
                }

                instance = CreateInstance<AutoSaveSettings>();
                AssetDatabase.CreateAsset(instance, AssetPath);
                AssetDatabase.SaveAssets();
                return instance;
            }
        }

        private static void Save()
        {
            EditorUtility.SetDirty(Instance);
            AssetDatabase.SaveAssets();
        }

        private void OnValidate()
        {
            intervalTime = Mathf.Max(1, intervalTime);
        }
    }
}
