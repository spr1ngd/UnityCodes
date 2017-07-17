
/*=========================================
* Author: Administrator
* DateTime:2017/7/16 15:40:51
* Description:$safeprojectname$
==========================================*/

using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public interface ILineChart
    {
        VertexHelper DrawLineChart( VertexHelper vh , Rect rect , LineChartData basis);
        VertexHelper DrawMesh( VertexHelper vh );
        VertexHelper DrawAxis( VertexHelper vh );
    }
}