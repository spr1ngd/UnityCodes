
/*=========================================
* Author: Administrator
* DateTime:2017/7/16 16:18:26
* Description:$safeprojectname$
==========================================*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public class LineChart3 : BaseLineChart
    {
        public override VertexHelper DrawLineChart( VertexHelper vh , Rect vRect , LineChartData VBasis )
        {
            vh = base.DrawLineChart(vh , vRect , VBasis);
            foreach ( KeyValuePair<int , VertexStream> line in lines )
            {
                if ( line.Value.vertexs.Count <= 1 )
                    continue;
                var startPos = GetPos(line.Value.vertexs[0]);
                UIVertex[] oldVertexs = new UIVertex[] { };
                for ( int i = 1 ; i < line.Value.vertexs.Count ; i++ )
                {
                    var endPos = GetPos(line.Value.vertexs[i]);
                    var startBottom = new Vector2(startPos.x , origin.y);
                    var endBottom = new Vector2(endPos.x , origin.y);
                    Color color = new Color(line.Value.color.r , line.Value.color.g , line.Value.color.b , 0.6f);
                    vh.AddUIVertexQuad(new UIVertex[]
                    {
                        GetUIVertex(startBottom,line.Value.color),
                        GetUIVertex(startPos,color),
                        GetUIVertex(endPos,color),
                        GetUIVertex(endBottom,line.Value.color),
                    });
                    var newVertexs = GetQuad(startPos , endPos , Color.red,1.0f);
                    if ( oldVertexs.Length.Equals(0) )
                    {
                        oldVertexs = newVertexs;
                    }
                    // 补足缺陷
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