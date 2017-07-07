
/*=========================================
* Author: Administrator
* DateTime:2017/6/9 14:49:57
* Description:$safeprojectname$
==========================================*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    internal class SuperImage : Image
    {
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            base.OnPopulateMesh(toFill);
            List<UIVertex> list = new List<UIVertex>();
            toFill.GetUIVertexStream(list);
        }
    }
}
