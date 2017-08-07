
/*=========================================
* Author: Administrator
* DateTime:2017/8/6 17:32:08
* Description:$safeprojectname$
==========================================*/

using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public class sgDataGraphBase: sgBase, IDataGraphFactory
    {
        protected sgSettingBase BaseSetting = null;

        /// <summary>
        /// Draw the asix
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public virtual VertexHelper DrawAxis( VertexHelper vh , Rect rect )
        {
            if (!BaseSetting.AxisEnable)
                return vh;
            var size = rect.size;
            var origin = new Vector2(-size.x / 2.0f , -size.y / 2.0f);
            Vector2 startPosX = origin + new Vector2(-BaseSetting.AxisWidth / 2.0f , 0);
            Vector2 endPosX = startPosX + new Vector2(size.x + BaseSetting.AxisWidth / 2.0f , 0);
            Vector2 startPosY = origin + new Vector2(0 , -BaseSetting.AxisWidth / 2.0f);
            Vector2 endPosY = startPosY + new Vector2(0 , size.y + BaseSetting.AxisWidth / 2.0f);
            vh.AddUIVertexQuad(GetQuad(startPosX , endPosX , BaseSetting.AxisColor , BaseSetting.AxisWidth));
            vh.AddUIVertexQuad(GetQuad(startPosY , endPosY , BaseSetting.AxisColor , BaseSetting.AxisWidth));
            if ( BaseSetting.AxisArrowEnable )
            {
                float arrowSize = BaseSetting.AxisWidth * 2;
                var xFirst = endPosX + new Vector2(0 , arrowSize);
                var xSecond = endPosX + new Vector2(1.73f * arrowSize , 0);
                var xThird = endPosX + new Vector2(0 , -arrowSize);
                vh.AddUIVertexQuad(new UIVertex[]
                {
                    GetUIVertex(xFirst,BaseSetting.AxisColor),
                    GetUIVertex(xSecond,BaseSetting.AxisColor),
                    GetUIVertex(xThird,BaseSetting.AxisColor),
                    GetUIVertex(endPosX,BaseSetting.AxisColor),
                });

                var yFirst = endPosY + new Vector2(-arrowSize , 0);
                var ySecond = endPosY + new Vector2(0 , 1.73f * arrowSize);
                var yThird = endPosY + new Vector2(arrowSize , 0);
                vh.AddUIVertexQuad(new UIVertex[]
                {
                    GetUIVertex(yFirst,BaseSetting.AxisColor),
                    GetUIVertex(ySecond,BaseSetting.AxisColor),
                    GetUIVertex(yThird,BaseSetting.AxisColor),
                    GetUIVertex(endPosY,BaseSetting.AxisColor),
                });
            }
            return vh;
        }

        /// <summary>
        /// Draw the base (background) mesh 
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="rect"></param>
        /// <param name="cellSize"></param>
        /// <returns></returns>
        public virtual VertexHelper DrawBaseMesh( VertexHelper vh , Rect rect )
        {
            if (!BaseSetting.MeshEnable)
                return vh;
            var size = rect.size;
            var origin = new Vector2(-size.x / 2.0f , -size.y / 2.0f);
            if ( BaseSetting.HorizontalMeshEnable )
            {
                if ( !BaseSetting.ImaginaryLine )
                {
                    for ( float y = 0 ; y <= size.y ; y += BaseSetting.VerticalSpacing )
                    {
                        Vector2 startPoint = origin + new Vector2(0 , y);
                        Vector2 endPoint = startPoint + new Vector2(size.x , 0);
                        vh.AddUIVertexQuad(GetQuad(startPoint , endPoint , BaseSetting.MeshColor , BaseSetting.MeshWidth));
                    }
                }
                else
                {
                    for ( float y = 0 ; y <= size.y ; y += BaseSetting.VerticalSpacing )
                    {
                        Vector2 startPoint = origin + new Vector2(0 , y);
                        Vector2 endPoint = startPoint + new Vector2(8 , 0);
                        for ( float x = 0 ; x < size.x ; x += ( 8 + 2 ) )
                        {
                            vh.AddUIVertexQuad(GetQuad(startPoint , endPoint , BaseSetting.MeshColor , BaseSetting.MeshWidth));
                            startPoint = startPoint + new Vector2(10 , 0);
                            endPoint = startPoint + new Vector2(8 , 0);
                            if ( endPoint.x > size.x / 2.0f )
                                endPoint = new Vector2(size.x / 2.0f , endPoint.y);
                        }
                    }
                }
            }
            if ( BaseSetting.VerticalMeshEnable )
            {
                if ( !BaseSetting.ImaginaryLine )
                {
                    for ( float x = 0 ; x <= size.x ; x += BaseSetting.HorizontalSpacing )
                    {
                        Vector2 startPoint = origin + new Vector2(x , 0);
                        Vector2 endPoint = startPoint + new Vector2(0 , size.y);
                        vh.AddUIVertexQuad(GetQuad(startPoint , endPoint , BaseSetting.MeshColor , BaseSetting.MeshWidth));
                    }
                }
                else
                {
                    for ( float x = 0 ; x <= size.x ; x += BaseSetting.HorizontalSpacing )
                    {
                        Vector2 startPoint = origin + new Vector2(x , 0);
                        Vector2 endPoint = startPoint + new Vector2(0 , 8);
                        for ( float y = 0 ; y < size.y ; y += ( 8 + 2 ) )
                        {
                            vh.AddUIVertexQuad(GetQuad(startPoint , endPoint , BaseSetting.MeshColor , BaseSetting.MeshWidth));
                            startPoint = startPoint + new Vector2(0 , 10);
                            endPoint = startPoint + new Vector2(0 , 8);
                            if ( endPoint.y > size.y / 2.0f )
                                endPoint = new Vector2(endPoint.x , size.y / 2.0f);
                        }
                    }
                }
            }
            return vh;
        }

        /// <summary>
        /// Draw the main mesh
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public virtual VertexHelper DrawMesh(VertexHelper vh, Rect rect)
        {
            DrawBaseMesh(vh , rect);
            DrawAxis(vh , rect);
            return vh;
        }
    }
}
