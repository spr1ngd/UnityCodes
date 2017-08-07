
/*=========================================
* Author: Administrator
* DateTime:2017/7/16 15:46:43
* Description:$safeprojectname$
==========================================*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public class BaseLineChart : ILineChart
    {
        protected Rect rect;
        protected Vector2 size;
        protected Vector2 origin;
        protected LineChartData basis;
        protected Dictionary<int, VertexStream> lines;

        public virtual VertexHelper DrawLineChart( VertexHelper vh , Rect vRect , LineChartData vBasis )
        {
            this.basis = vBasis;
            lines = vBasis.Lines;
            this.rect = vRect;
            size = rect.size;
            origin = new Vector2(-size.x / 2.0f , -size.y / 2.0f);
            vh = DrawMesh(vh);
            vh = DrawAxis(vh);
            return vh;
        }

        public virtual VertexHelper DrawAxis( VertexHelper vh )
        {
            if(!basis.IsDrawAxis )
                return vh;
            Vector2 startPosX = origin + new Vector2(-basis.AxisWidth / 2.0f , 0);
            Vector2 endPosX = startPosX + new Vector2(size.x + basis.AxisWidth / 2.0f , 0);
            Vector2 startPosY = origin + new Vector2(0 , -basis.AxisWidth / 2.0f);
            Vector2 endPosY = startPosY + new Vector2(0 , size.y + basis.AxisWidth / 2.0f);
            vh.AddUIVertexQuad(GetQuad(startPosX,endPosX,basis.AxisColor,basis.AxisWidth));
            vh.AddUIVertexQuad(GetQuad(startPosY , endPosY , basis.AxisColor , basis.AxisWidth));
            if (basis.ShowArrow)
            {
                var xFirst = endPosX + new Vector2(0, basis.AxisWidth);
                var xSecond = endPosX + new Vector2(1.73f * basis.AxisWidth, 0);
                var xThird = endPosX + new Vector2(0 , -basis.AxisWidth);
                vh.AddUIVertexQuad(new UIVertex[]
                {
                    GetUIVertex(xFirst,basis.AxisColor),
                    GetUIVertex(xSecond,basis.AxisColor),
                    GetUIVertex(xThird,basis.AxisColor),
                    GetUIVertex(endPosX,basis.AxisColor),
                });

                var yFirst = endPosY + new Vector2(-basis.AxisWidth, 0);
                var ySecond = endPosY + new Vector2(0 , 1.73f * basis.AxisWidth);
                var yThird = endPosY + new Vector2(basis.AxisWidth , 0);
                vh.AddUIVertexQuad(new UIVertex[]
                {
                    GetUIVertex(yFirst,basis.AxisColor),
                    GetUIVertex(ySecond,basis.AxisColor),
                    GetUIVertex(yThird,basis.AxisColor),
                    GetUIVertex(endPosY,basis.AxisColor),
                });
            }
            return vh;
        }

        public virtual VertexHelper DrawMesh( VertexHelper vh )
        {
            if(!basis.IsDrawMeshX &&!basis.IsDrawMeshY )
                return vh;
            if ( basis.IsDrawMeshX )
            {
                if (!basis.IsImaginaryLine)
                {
                    for ( float y = 0 ; y <= size.y ; y += basis.MeshCellSize.y )
                    {
                        Vector2 startPoint = origin + new Vector2(0 , y);
                        Vector2 endPoint = startPoint + new Vector2(size.x , 0);
                        vh.AddUIVertexQuad(GetQuad(startPoint , endPoint , basis.MeshColor , basis.MeshWidth));
                    }
                }
                else
                {
                    for ( float y = 0 ; y <= size.y ; y += basis.MeshCellSize.y )
                    {
                        Vector2 startPoint = origin + new Vector2(0 , y);
                        Vector2 endPoint = startPoint + new Vector2(8 , 0);
                        for (float x = 0; x < size.x; x += (8 + 2))
                        {
                            vh.AddUIVertexQuad(GetQuad(startPoint , endPoint , basis.MeshColor , basis.MeshWidth));
                            startPoint = startPoint + new Vector2(10 , 0);
                            endPoint = startPoint + new Vector2(8 , 0);
                            if(endPoint.x > size.x/2.0f )
                                endPoint = new Vector2(size.x / 2.0f , endPoint.y);
                        }
                    }
                }
            }
            if ( basis.IsDrawMeshY )
            {
                if (!basis.IsImaginaryLine)
                {
                    for (float x = 0; x <= size.x; x += basis.MeshCellSize.x)
                    {
                        Vector2 startPoint = origin + new Vector2(x, 0);
                        Vector2 endPoint = startPoint + new Vector2(0, size.y);
                        vh.AddUIVertexQuad(GetQuad(startPoint, endPoint, basis.MeshColor, basis.MeshWidth));
                    }
                }
                else
                {
                    for ( float x = 0 ; x <= size.x ; x += basis.MeshCellSize.x )
                    {
                        Vector2 startPoint = origin + new Vector2(x , 0);
                        Vector2 endPoint = startPoint + new Vector2(0 , 8);
                        for (float y = 0; y < size.y; y += (8 + 2))
                        {
                            vh.AddUIVertexQuad(GetQuad(startPoint , endPoint , basis.MeshColor , basis.MeshWidth));
                            startPoint = startPoint + new Vector2(0 , 10);
                            endPoint = startPoint + new Vector2(0 , 8);
                            if (endPoint.y > size.y/2.0f )
                                endPoint = new Vector2(endPoint.x, size.y / 2.0f);
                        }
                    }
                }
            }
            return vh;
        }

        /// <summary>
        /// draw line by two points
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="color0"></param>
        /// <param name="LineWidth"></param>
        /// <returns></returns>
        protected UIVertex[] GetQuad( Vector2 startPos , Vector2 endPos , Color color0 , float LineWidth = 2.0f )
        {
            float dis = Vector2.Distance(startPos , endPos);
            float y = LineWidth * 0.5f * ( endPos.x - startPos.x ) / dis;
            float x = LineWidth * 0.5f * ( endPos.y - startPos.y ) / dis;
            if ( y <= 0 )
                y = -y;
            else
                x = -x;
            UIVertex[] vertex = new UIVertex[4];
            vertex[0].position = new Vector3(startPos.x + x , startPos.y + y);
            vertex[1].position = new Vector3(endPos.x + x , endPos.y + y);
            vertex[2].position = new Vector3(endPos.x - x , endPos.y - y);
            vertex[3].position = new Vector3(startPos.x - x , startPos.y - y);
            for ( int i = 0 ; i < vertex.Length ; i++ )
                vertex[i].color = color0;
            return vertex;
        }

        /// <summary>
        /// get uivertex
        /// </summary>
        /// <param name="point"></param>
        /// <param name="color0"></param>
        /// <returns></returns>
        protected UIVertex GetUIVertex( Vector2 point , Color color0 )
        {
            UIVertex vertex = new UIVertex
            {
                position = point ,
                color = color0 ,
            };
            return vertex;
        }

        protected Vector2 GetPos( Vector2 pos )
        {
            pos += new Vector2(-0.5f , -0.5f);
            return new Vector2(pos.x * size.x , pos.y * size.y);
        }
    }
}