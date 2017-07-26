
/*=========================================
* Author: dong@spring
* DateTime:2017/7/26 11:27:16
* Description:$safeprojectname$
==========================================*/

using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public class Circle : MaskableGraphic
    {
        public enum CircleType
        {
            One,
            Two 
        }

        public Color Color = Color.white;
        public float Radian = 10.0f;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}