
using UnityEditor;
using UnityEngine;

namespace SpringGUI
{
    [CustomEditor(typeof(UITree),true)]
    [CanEditMultipleObjects]
    public class UITreeEditor : Editor
    {
        private SerializedProperty m_closeIcon = null;
        private SerializedProperty m_openIcon = null;
        private SerializedProperty m_lastLayerIcon = null;

        protected virtual void OnEnable()
        {
            m_closeIcon = serializedObject.FindProperty("m_closeIcon");
            m_openIcon = serializedObject.FindProperty("m_openIcon");
            m_lastLayerIcon = serializedObject.FindProperty("m_lastLayerIcon");
        }

        public override void OnInspectorGUI( )
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Icon Setting",EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_closeIcon);
            EditorGUILayout.PropertyField(m_openIcon);
            EditorGUILayout.PropertyField(m_lastLayerIcon);
            serializedObject.ApplyModifiedProperties();
        }
    }
}