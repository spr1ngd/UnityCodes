
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
        }
    }
}