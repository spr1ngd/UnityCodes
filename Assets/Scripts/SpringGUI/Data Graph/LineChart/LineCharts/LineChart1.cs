
/*=========================================
* Author: Administrator
* DateTime:2017/7/16 15:51:53
* Description:$safeprojectname$
==========================================*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public class LineChart1 : BaseLineChart
    {
        public override VertexHelper DrawLineChart(VertexHelper vh, Rect vRect, LineChartData VBasis)
        {
            vh =  base.DrawLineChart(vh, vRect, VBasis);
            foreach (KeyValuePair<int, VertexStream> line in lines)
            {
                if ( line.Value.vertexs.Count <= 1 )
                    continue;
                var startPos = GetPos(line.Value.vertexs[0]);
                UIVertex[] oldVertexs = new UIVertex[] {};
                for ( int i = 1 ; i < line.Value.vertexs.Count ; i++ )
                {
                    var endPos = GetPos(line.Value.vertexs[i]);
                    var newVertexs = GetQuad(startPos , endPos , line.Value.color);
                    if ( oldVertexs.Length.Equals(0) )
                    {
                        oldVertexs = newVertexs;
                    }
                    else
                    {
                        vh.AddUIVertexQuad(new UIVertex[]
                        {
                            oldVertexs[1],
                            newVertexs[1],
                            oldVertexs[2],
                            newVertexs[0]
                        });
                        vh.AddUIVertexQuad(new UIVertex[]
                        {
                            newVertexs[0],
                            oldVertexs[1],
                            newVertexs[3],
                            oldVertexs[2]
                        });
                        oldVertexs = newVertexs;
                    }
                    vh.AddUIVertexQuad(newVertexs);
                    startPos = endPos;
                }
            }
            return vh;
        }
    }
}