using UnityEngine;

namespace ZPlugin {
    public enum GUIType {
        Monitor,
        Tip
    }

    /// <summary>
    /// 使用 OnGUI 实时在屏幕上 Debug 一些数据
    /// </summary>
    public class OnGUITool : MonoBehaviour {
        static OnGUITool onGUITool;

        static string strToShow;

        static GUIType GUIType;

        public static void Show<T>(T str, GUIType GUIType = GUIType.Monitor) {
            if (onGUITool == null) {
                GameObject temp = new GameObject("OnGUITool");
                onGUITool = temp.AddComponent<OnGUITool>();
            }

            OnGUITool.GUIType = GUIType;
            strToShow = str.ToString();
        }

        public static void Stop(float t = 0) {
            Destroy(onGUITool.gameObject, t);
        }

        private void OnGUI() {
            // TODO: more cheet style state
            switch (GUIType) {
                case GUIType.Monitor:
                    GUI.Box(new Rect(10, 10, 100, 50), strToShow);
                    break;
                case GUIType.Tip:
                    GUI.Box(new Rect(10, 10, 100, 50), strToShow);
                    Stop(0.5f);
                    break;
            }
        }
    }
}