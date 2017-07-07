
/*=========================================
* Author : SpringDong
* DateTime : 2017/6/11 15:40:14
* Email : 540637360@qq.com
* Description : 重写ScukerImage Inspector面板
==========================================*/

using UnityEditor;
using UnityEditor.UI;

namespace SpringGUI
{
    [CustomEditor(typeof(SuckerImage))]
    [CanEditMultipleObjects]
    internal class SuckerImageEditor : GraphicEditor
    {
        protected override void OnEnable()
        {

        }
        protected override void OnDisable()
        {

        }
        public override void OnInspectorGUI()
        {

        }
    }
}