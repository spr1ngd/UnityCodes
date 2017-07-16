
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
        public override VertexHelper DrawLineChart( VertexHelper vh , Rect vRect , LineChartBasis VBasis )
        {
            vh = base.DrawLineChart(vh , vRect , VBasis);
            foreach ( KeyValuePair<int , IList<Vector2>> line in lines )
            {
                if ( line.Value.Count <= 1 )
                    continue;
                var startPos = GetPos(line.Value[0]);
                Color color;
                if (line.Key.Equals(0))
                    color = Color.yellow;
                else
                    color = Color.cyan;
                for ( int i = 1 ; i < line.Value.Count ; i++ )
                {
                    var endPos = GetPos(line.Value[i]);
                    var startBottom = new Vector2(startPos.x , origin.y);
                    var endBottom = new Vector2(endPos.x, origin.y);
                    vh.AddUIVertexQuad(new UIVertex[]
                    {
                        GetUIVertex(startBottom,color),
                        GetUIVertex(startPos,new Color(color.r * 0.7f,color.g* 0.7f,color.b* 0.7f,1)),
                        GetUIVertex(endPos,new Color(color.r * 0.7f,color.g* 0.7f,color.b* 0.7f,1)),
                        GetUIVertex(endBottom,color),
                    });
                    startPos = endPos;
                }
            }
            return vh;
        }
    }
}