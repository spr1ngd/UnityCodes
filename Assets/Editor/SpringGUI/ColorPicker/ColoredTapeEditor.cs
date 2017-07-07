
/*=========================================
* Author: SpringDong
* DateTime:2017/6/9 16:41:36
* Description:
*                 对ColoredTape面板进行重写但是博主在重写ColorList时遇到问题，无法将其展示在Inpector面板上
*             如果你知道如何在OnInspector方法中重写List<Color>，可以联系博主告诉我(Email:540637360@qq.com)
*             Thanks!
==========================================*/

using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace SpringGUI
{
    //[CustomEditor(typeof(ColoredTape))]
    //[CanEditMultipleObjects]
    internal class ColoredTapeEditor : GraphicEditor
    {
        private SerializedProperty m_tapeDirection = null;
        private SerializedProperty m_ouline = null;
        private SerializedProperty m_outlineWidth = null;
        private SerializedProperty m_outlineColor = null;
        private SerializedProperty m_colors = null;
        private SerializedProperty _colors = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            //m_tapeDirection = serializedObject.FindProperty("TapeDirection");
            //m_ouline = serializedObject.FindProperty("Outline");
            //m_outlineWidth = serializedObject.FindProperty("OuelineWidth");
            //m_outlineColor = serializedObject.FindProperty("OutlineColor");
            //m_colors = serializedObject.FindProperty("m_Colors");
            //_colors = serializedObject.FindProperty("_colors");
        }

        protected override void OnDisable( )
        {

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //EditorGUILayout.Space();
            //serializedObject.Update();
            //EditorGUI.indentLevel = 0;
            //EditorGUILayout.LabelField("TapeDirection" , EditorStyles.boldLabel);
            //EditorGUI.indentLevel = 1;
            //EditorGUILayout.PropertyField(m_tapeDirection);
            //EditorGUI.indentLevel = 0;
            //EditorGUILayout.LabelField("Outline Setting" , EditorStyles.boldLabel);
            //EditorGUI.indentLevel = 1;
            //EditorGUILayout.PropertyField(m_ouline);
            //if ( m_ouline.boolValue )
            //{
            //    EditorGUILayout.PropertyField(m_outlineWidth);
            //    EditorGUILayout.PropertyField(m_outlineColor);
            //}
            //EditorGUI.indentLevel = 0;
            //EditorGUILayout.LabelField("Tape Color Number" , EditorStyles.boldLabel);
            //EditorGUI.indentLevel = 1;
            //todo 重写m_Colors  List失败，无法用更好的方法将其展示
            //EditorGUILayout.PropertyField(m_colors);
            //EditorGUILayout.PropertyField(_colors);
            //serializedObject.ApplyModifiedProperties();
        }

        private Color getColor( int i  )
        {
            if ( i.Equals(0) )
                return Color.red;
            if ( i.Equals(1) )
                return Color.magenta;
            if ( i.Equals(2) )
                return Color.blue;
            if ( i.Equals(3) )
                return Color.cyan;
            if ( i.Equals(4) )
                return Color.green;
            if ( i.Equals(5) )
                return Color.yellow;
            if(i.Equals(6))
                return Color.red;
            return Color.white;
        }
    }
}