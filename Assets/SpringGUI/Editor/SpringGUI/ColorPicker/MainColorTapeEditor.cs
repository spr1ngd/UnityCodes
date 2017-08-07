
/*=========================================
* Author: Administrator
* DateTime:2017/6/11 15:17:39
* Description:$safeprojectname$
==========================================*/

using UnityEditor;
using UnityEditor.UI;

namespace SpringGUI
{
    [CustomEditor(typeof(MainColorTape))]
    [CanEditMultipleObjects]
    internal class MainColorTapeEditor : GraphicEditor
    {
        private SerializedProperty _Color = null;
        private SerializedProperty m_ouline = null;
        private SerializedProperty m_outlineWidth = null;
        private SerializedProperty m_outlineColor = null;

        protected override void OnEnable( )
        {
            _Color = serializedObject.FindProperty("m_Color");
            m_ouline = serializedObject.FindProperty("Outline");
            m_outlineWidth = serializedObject.FindProperty("OuelineWidth");
            m_outlineColor = serializedObject.FindProperty("OutlineColor");
        }

        protected override void OnDisable( )
        {

        }

        public override void OnInspectorGUI( )
        {
            serializedObject.Update();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Selected Color" , EditorStyles.boldLabel);
            EditorGUI.indentLevel = 1;
            _Color.colorValue = EditorGUILayout.ColorField("MainColor" , _Color.colorValue);
            EditorGUI.indentLevel = 0;
            EditorGUILayout.LabelField("Outline Setting" , EditorStyles.boldLabel);
            EditorGUI.indentLevel = 1;
            EditorGUILayout.PropertyField(m_ouline);
            if ( m_ouline.boolValue )
            {
                EditorGUILayout.PropertyField(m_outlineWidth);
                EditorGUILayout.PropertyField(m_outlineColor);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}