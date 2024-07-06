/****************************************************
    文件：RadarChartEditor.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEditor;
using UnityEngine;
using YFramework.UI.Kit;

namespace YFramework.Editor.UI
{
    [CustomEditor(typeof(RadarChart))]
    [CanEditMultipleObjects]
    public class RadarChartEditor : UnityEditor.UI.ImageEditor
    {
        private SerializedProperty _pointCount;
        private SerializedProperty _pointSprite;
        private SerializedProperty _pointColor;
        private SerializedProperty _pointSize;
        private SerializedProperty _handlerRadio;

        protected override void OnEnable()
        {
            base.OnEnable();
            _pointCount  = serializedObject.FindProperty("_pointCount");
            _pointSprite = serializedObject.FindProperty("_pointSprite");
            _pointColor = serializedObject.FindProperty("_pointColor");
            _pointSize = serializedObject.FindProperty("_pointSize");
            _handlerRadio = serializedObject.FindProperty("_handlerRadio");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(_pointCount);
            EditorGUILayout.PropertyField(_pointSprite);
            EditorGUILayout.PropertyField(_pointColor);
            EditorGUILayout.PropertyField(_pointSize);
            EditorGUILayout.PropertyField(_handlerRadio,true);
            
            RadarChart radar = target as RadarChart;

            if (radar != null)
            {
                if (GUILayout.Button("生成雷达图顶点"))
                {
                    radar.InitPoint();
                }

                if (GUILayout.Button("生成内部可操作的顶点"))
                {
                    radar.InitHandle();
                }
            }

            serializedObject.ApplyModifiedProperties();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}