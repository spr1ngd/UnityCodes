
/*=========================================
* Author : SpringDong 
* Email : springu3d@yeah.net
* Description : draw sucker
==========================================*/

using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public class SuckerImage : MaskableGraphic
    {
        private float m_halfHeight = 10.0f;
        private float m_suckerRadius = 2.0f;
        private float m_suckerWidth = 4.0f;
        
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            Vector3 o = new Vector3(0 , -m_halfHeight);
            Vector3 a = new Vector3(-m_suckerRadius , -m_halfHeight + m_suckerRadius);
            Vector3 b = new Vector3(-m_suckerRadius , m_halfHeight / 2.0f);
            Vector3 c = b + new Vector3(-m_suckerWidth , 0);
            Vector3 d = c + new Vector3(0 , m_suckerWidth , 0);
            Vector3 e = b + new Vector3(0 , m_suckerWidth , 0);
            Vector3 f = e + new Vector3(0 , m_suckerWidth , 0);
            Vector3 g = f + new Vector3(m_suckerWidth , 0 , 0);
            Vector3 h = g + new Vector3(0 , -m_suckerWidth , 0);
            Vector3 i = h + new Vector3(m_suckerWidth , 0 , 0);
            Vector3 j = i + new Vector3(0 , -m_suckerWidth , 0);
            Vector3 k = j + new Vector3(-m_suckerWidth , 0 , 0);
            Vector3 l = new Vector3(-a.x , a.y , 0);

            vh.AddUIVertexQuad(GetQuad(c , d , i , j , Color.black));
            vh.AddUIVertexQuad(GetQuad(e , f , g , h , Color.black));
            vh.AddUIVertexQuad(GetQuad(a , b , Color.black , 1));
            vh.AddUIVertexQuad(GetQuad(o , a , Color.black , 1));
            vh.AddUIVertexQuad(GetQuad(k , l , Color.black , 1));
            vh.AddUIVertexQuad(GetQuad(o , l , Color.black , 1));

            //弥补缺陷
            float lenght = 0.5f / Mathf.Sqrt(2);
            var first = a - new Vector3(0.5f , 0 , 0);
            var second = a + new Vector3(0.5f , 0 , 0);
            var third = a - new Vector3(lenght , lenght);
            var four = a + new Vector3(lenght , lenght);
            vh.AddUIVertexQuad(GetQuad(first , four , third , second , Color.black));
            var first0 = o + new Vector3(-lenght, lenght);
            var second0 = o + new Vector3(lenght , lenght);
            var third0 = o + new Vector3(lenght , -lenght);
            var four0 = o + new Vector3(-lenght , -lenght);
            vh.AddUIVertexQuad(GetQuad(first0 , second0 , third0 , four0 , Color.black));
            var first1 = l - new Vector3(0.5f , 0 , 0);
            var second1 = l + new Vector3(0.5f , 0 , 0);
            var third1 = l + new Vector3(-lenght , lenght);
            var four1 = l + new Vector3(lenght , -lenght);
            vh.AddUIVertexQuad(GetQuad(first1 , third1 , second1 , four1 , Color.black));
        }

        private UIVertex[] GetQuad( Vector2 startPos , Vector2 endPos , Color color0 , float LineWidth = 2.0f )
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
        private UIVertex[] GetQuad( Vector2 first , Vector2 second , Vector2 third , Vector2 four , Color color0 )
        {
            UIVertex[] vertexs = new UIVertex[4];
            vertexs[0] = GetUIVertex(first , color0);
            vertexs[1] = GetUIVertex(second , color0);
            vertexs[2] = GetUIVertex(third , color0);
            vertexs[3] = GetUIVertex(four , color0);
            return vertexs;
        }
        private UIVertex GetUIVertex( Vector2 point , Color color0 )
        {
            UIVertex vertex = new UIVertex
            {
                position = point ,
                color = color0 ,
                uv0 = new Vector2(0 , 0)
            };
            return vertex;
        }
    }
}