 
/*=========================================
* Author: Administrator
* DateTime:2017/7/16 16:18:21
* Description:$safeprojectname$
==========================================*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public class LineChart2 : BaseLineChart
    {
        public override VertexHelper DrawLineChart( VertexHelper vh , Rect vRect , LineChartData VBasis )
        {
            vh = base.DrawLineChart(vh , vRect , VBasis);
            foreach ( KeyValuePair<int , VertexStream> line in lines )
            {
                if ( line.Value.vertexs.Count <= 1 )
                    continue;
                var startPos = GetPos(line.Value.vertexs[0]);
                for ( int i = 1 ; i < line.Value.vertexs.Count ; i++ )
                {
                    var endPos = GetPos(line.Value.vertexs[i]);
                    var startBottom = new Vector2(startPos.x , origin.y);
                    var endBottom = new Vector2(endPos.x, origin.y);
                    Color color1 = new Color(line.Value.color.r * 0.7f , line.Value.color.g * 0.7f , line.Value.color.b * 0.7f , 1);
                    vh.AddUIVertexQuad(new UIVertex[]
                    {
                        GetUIVertex(startBottom,line.Value.color),
                        GetUIVertex(startPos, color1),
                        GetUIVertex(endPos,color1),
                        GetUIVertex(endBottom,line.Value.color),
                    });
                    startPos = endPos;
                }
            }
            return vh;
        }
    }
}