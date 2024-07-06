/****************************************************
    文件：CricleImageEditor.cs
    作者：Y
    邮箱: 916111418@qq.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using UnityEditor;
using UnityEngine;

namespace YFramework.Editor.UI
{
    [CustomEditor(typeof(YFramework.UI.CircleImage),true)]
    [CanEditMultipleObjects]
    public class CricleImageEditor : UnityEditor.UI.ImageEditor
    {
        private SerializedProperty _fillPercent;
        private SerializedProperty _segement;
        private SerializedProperty _h;

        protected override void OnEnable()
        {
            base.OnEnable();
            _fillPercent = serializedObject.FindProperty("showPercent");
            _segement = serializedObject.FindProperty("segments");
      
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.Slider(_fillPercent, 0, 1, new GUIContent("showPercent"));
            EditorGUILayout.PropertyField(_segement);
            serializedObject.ApplyModifiedProperties();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}