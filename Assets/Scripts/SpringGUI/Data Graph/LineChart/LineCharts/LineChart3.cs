
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
        public override VertexHelper DrawLineChart( VertexHelper vh , Rect vRect , LineChartBasis VBasis )
        {
            vh = base.DrawLineChart(vh , vRect , VBasis);
            foreach ( KeyValuePair<int , IList<Vector2>> line in lines )
            {
                if ( line.Value.Count <= 1 )
                    continue;
                var startPos = GetPos(line.Value[0]);
                UIVertex[] oldVertexs = new UIVertex[] { };
                Color color;
                if ( line.Key.Equals(0) ) color = Color.yellow;
                else color = Color.cyan;
                for ( int i = 1 ; i < line.Value.Count ; i++ )
                {
                    var endPos = GetPos(line.Value[i]);
                    var startBottom = new Vector2(startPos.x , origin.y);
                    var endBottom = new Vector2(endPos.x , origin.y);
                    vh.AddUIVertexQuad(new UIVertex[]
                    {
                        GetUIVertex(startBottom,color),
                        GetUIVertex(startPos,new Color(color.r,color.g,color.b,0.6f)),
                        GetUIVertex(endPos,new Color(color.r,color.g,color.b,0.6f)),
                        GetUIVertex(endBottom,color),
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