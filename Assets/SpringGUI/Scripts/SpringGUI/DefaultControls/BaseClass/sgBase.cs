
/*=========================================
* Author: dong@spring
* DateTime:2017/7/26 11:09:31
* Description: SpringGUI基类，陆续拓展绘制的接口
==========================================*/

using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public class sgBase : ISpringGUIBase
    {
        /// <summary>
        /// Gets the quad.
        /// </summary>
        /// <returns>The quad.</returns>
        /// <param name="startPos">Start position.</param>
        /// <param name="endPos">End position.</param>
        /// <param name="color0">Color0.</param>
        /// <param name="LineWidth">Line width.</param>
        public virtual UIVertex[] GetQuad( Vector2 startPos , Vector2 endPos , Color color0 , float LineWidth = 2.0f )
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
        /// Get the quad
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="color0"></param>
        /// <param name="LineWidth"></param>
        /// <returns></returns>
        public virtual VertexHelper GetQuad( VertexHelper vh , Vector2 startPos , Vector2 endPos , Color color0 , float LineWidth = 2.0f )
        {
            vh.AddUIVertexQuad(GetQuad(startPos,endPos,color0,LineWidth));
            return vh;
        }

        /// <summary>
        /// Gets the user interface vertex.
        /// </summary>
        /// <returns>The user interface vertex.</returns>
        /// <param name="point">Point.</param>
        /// <param name="color0">Color0.</param>
        public virtual UIVertex GetUIVertex( Vector2 point , Color color0 )
        {
            UIVertex vertex = new UIVertex
            {
                position = point ,
                color = color0 ,
            };
            return vertex;
        }

        /// <summary>
        /// Gets the user interface vertex.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="color0"></param>
        /// <param name="uv"></param>
        /// <returns></returns>
        public virtual UIVertex GetUIVertex( Vector2 point,Color color0,Vector2 uv )
        {
            UIVertex vertex = new UIVertex
            {
                position = point,
                color= color0,
                uv0 = uv
            };
            return vertex;
        }

        /// <summary>
        /// Gets the quad dottedline.
        /// </summary>
        /// <returns>The quad dottedline.</returns>
        /// <param name="vh"></param>
        /// <param name="startpos">Startpos.</param>
        /// <param name="endPos">End position.</param>
        /// <param name="color0">Color0.</param>
        /// <param name="lineWidth">Line width.</param>
        public virtual VertexHelper GetQuadDottedline( VertexHelper vh , Vector2 startpos , Vector2 endPos , Color color0 , float lineWidth = 2.0f )
        {
            Vector2 dir = ( endPos - startpos ).normalized;
            float lenght = Vector3.Distance(startpos , endPos);
            float perLenght = 7;
            float perSpace = 2;
            for ( float total = 0 ; total < lenght ; total += perLenght + perSpace )
            {
                float addLenght = perLenght;
                if ( total + perSpace >= lenght )
                    return vh;
                if ( total + perSpace + perLenght >= lenght )
                    addLenght = lenght - total;
                Vector2 tempEnd = startpos + dir * addLenght;
                vh.AddUIVertexQuad(GetQuad(startpos , tempEnd , color0 , lineWidth));
                startpos = tempEnd + dir * perSpace;
            }
            return vh;
        }
    }
}