
/*=========================================
* Author: Administrator
* DateTime:2017/6/12 15:58:54
* Description:$safeprojectname$
==========================================*/

using UnityEditor;
using UnityEditor.UI;

namespace SpringGUI
{
    [CustomEditor(typeof(MultiColoredTape))]
    [CanEditMultipleObjects]
    public class MultiColoredTapeEditor : GraphicEditor
    {
        private SerializedProperty TopLeft = null;
        private SerializedProperty TopRight = null;
        private SerializedProperty BottomLeft = null;
        private SerializedProperty BottomRight = null;
        private SerializedProperty m_ouline = null;
        private SerializedProperty m_outlineWidth = null;
        private SerializedProperty m_outlineColor = null;

        protected override void OnEnable( )
        {
            TopLeft = serializedObject.FindProperty("TopLeft");
            TopRight = serializedObject.FindProperty("TopRight");
            BottomLeft = serializedObject.FindProperty("BottomLeft");
            BottomRight = serializedObject.FindProperty("BottomRight");
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
            EditorGUILayout.LabelField("Colors" , EditorStyles.boldLabel);
            EditorGUI.indentLevel = 1;
            TopLeft.colorValue = EditorGUILayout.ColorField("TopLeft" , TopLeft.colorValue);
            TopRight.colorValue = EditorGUILayout.ColorField("TopRight" , TopRight.colorValue);
            BottomLeft.colorValue = EditorGUILayout.ColorField("BottomLeft" , BottomLeft.colorValue);
            BottomRight.colorValue = EditorGUILayout.ColorField("BottomRight" , BottomRight.colorValue);
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