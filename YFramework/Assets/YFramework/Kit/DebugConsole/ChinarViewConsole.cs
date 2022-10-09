#define MACRO_CHINAR
using System.Collections.Generic;
using UnityEngine;


namespace YFramework.Kit.DebugConsole
{
    /// <summary>
    /// Chinar可视控制台
    /// </summary>
    public class ChinarViewConsole : MonoBehaviour
    {
#if MACRO_CHINAR
        struct Log
        {
            public string Message;
            public string StackTrace;
            public LogType LogType;
        }


        #region Inspector 面板属性

        [Tooltip("快捷键-开/关控制台")] public KeyCode ShortcutKey = KeyCode.F2;
        [Tooltip("摇动开启控制台？")] public bool ShakeToOpen = true;
        [Tooltip("窗口打开加速度")] public float shakeAcceleration = 3f;
        [Tooltip("是否保持一定数量的日志")] public bool restrictLogCount = false;
        [Tooltip("最大日志数")] public int maxLogs = 1000;

        #endregion

        private readonly List<Log> _logs = new List<Log>();
        private Log _log;
        private Vector2 _scrollPosition;
        private bool _visible;
        public bool collapse;

        private static readonly Dictionary<LogType, Color> LOGTypeColors = new Dictionary<LogType, Color>
        {
            {LogType.Assert, Color.white},
            {LogType.Error, Color.red},
            {LogType.Exception, Color.red},
            {LogType.Log, Color.white},
            {LogType.Warning, Color.yellow},
        };

        private const string ChinarWindowTitle = "Chinar-控制台";
        private const int Edge = 20;
        private readonly GUIContent _clearLabel = new GUIContent("清空", "清空控制台内容");
        private readonly GUIContent _hiddenLabel = new GUIContent("合并信息", "隐藏重复信息");

        private readonly Rect _titleBarRect = new Rect(0, 0, 10000, 20);
        private Rect _windowRect = new Rect(Edge, Edge, Screen.width - (Edge * 2), Screen.height - (Edge * 2));


        private void OnEnable()
        {
#if UNITY_4
            Application.RegisterLogCallback(HandleLog);
#else
            Application.logMessageReceived += HandleLog;
#endif
        }
        private void OnDisable()
        {
#if UNITY_4
            Application.RegisterLogCallback(null);
#else
            Application.logMessageReceived -= HandleLog;
#endif
        }
        private void Update()
        {
            if (Input.GetKeyDown(ShortcutKey)) _visible = !_visible;
            if (ShakeToOpen && Input.acceleration.sqrMagnitude > shakeAcceleration) _visible = true;
        }
        private void OnGUI()
        {
            if (!_visible) return;
            _windowRect = GUILayout.Window(666, _windowRect, DrawConsoleWindow, ChinarWindowTitle);
        }
        private void DrawConsoleWindow(int windowid)
        {
            DrawLogsList();
            DrawToolbar();
            GUI.DragWindow(_titleBarRect);
        }
        private void DrawLogsList()
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            for (var i = 0; i < _logs.Count; i++)
            {
                if (collapse && i > 0)
                    if (_logs[i].Message != _logs[i - 1].Message)
                        continue;
                GUI.contentColor = LOGTypeColors[_logs[i].LogType];
                GUILayout.Label(_logs[i].Message);
            }

            GUILayout.EndScrollView();
            GUI.contentColor = Color.white;
        }
        private void DrawToolbar()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(_clearLabel))
            {
                _logs.Clear();
            }

            collapse = GUILayout.Toggle(collapse, _hiddenLabel, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
        }
        private void HandleLog(string message, string stackTrace, LogType type)
        {
            _logs.Add(new Log
            {
                Message = message,
                StackTrace = stackTrace,
                LogType = type,
            });
            DeleteExcessLogs();
        }
        private void DeleteExcessLogs()
        {
            if (!restrictLogCount) return;
            var amountToRemove = Mathf.Max(_logs.Count - maxLogs, 0);
            print(amountToRemove);
            if (amountToRemove == 0)
            {
                return;
            }

            _logs.RemoveRange(0, amountToRemove);
        }
#endif
    }
}