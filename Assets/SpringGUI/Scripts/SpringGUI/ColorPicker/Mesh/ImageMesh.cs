
/*=========================================
* Author: springDong
* Description : draw mesh 
==========================================*/

using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    internal class ImageMesh : MaskableGraphic
    {
        public int XAxisCount = 16;
        public int YAxisCount = 16;
        public float LineWidth = 1.0f;
        public Color Color = Color.black;
        public Color FocusColor = Color.red;
        public float FocuslineWidth = 0.8f;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            var rectSize = GetPixelAdjustedRect().size;
            var width = rectSize.x;
            var height = rectSize.y;
            var perWidth = width / XAxisCount;
            var perHeight = height / YAxisCount;

            Vector2 origin = new Vector2(-rectSize.x / 2.0f , -rectSize.y / 2.0f);
            //x axis 
            for ( int i = 0 ; i <= YAxisCount ; i++ )
            {
                Vector2 startPos = origin + new Vector2(0 , i * perHeight);
                Vector2 endPos = startPos + new Vector2(width , 0);
                vh.AddUIVertexQuad(GetQuad(startPos , endPos , Color , LineWidth));
            }
            //y axis
            for ( int i = 0 ; i <= XAxisCount ; i++ )
            {
                Vector2 startPos = origin + new Vector2(i * perWidth , 0);
                Vector2 endPos = startPos + new Vector2(0 , height);
                vh.AddUIVertexQuad(GetQuad(startPos , endPos , Color , LineWidth));
            }

            var x = (int)( XAxisCount / 2 );
            var y = (int)( YAxisCount / 2 );
            var bottomLeft = origin + new Vector2(perWidth * x , perHeight * y);
            var bottomRight = bottomLeft + new Vector2(perWidth , 0);
            var topLeft = bottomLeft + new Vector2(0 , perHeight);
            var topRight = topLeft + new Vector2(perWidth , 0);
            vh.AddUIVertexQuad(GetQuad(topLeft , topRight , FocusColor , FocuslineWidth));
            vh.AddUIVertexQuad(GetQuad(bottomLeft , bottomRight , FocusColor , FocuslineWidth));
            vh.AddUIVertexQuad(GetQuad(topLeft , bottomLeft , FocusColor , FocuslineWidth));
            vh.AddUIVertexQuad(GetQuad(topRight , bottomRight , FocusColor , FocuslineWidth));
        }

        public UIVertex[] GetQuad( Vector2 startPos , Vector2 endPos , Color color0 , float LineWidth = 2.0f )
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
        public UIVertex GetUIVertex( Vector2 point , Color color0 )
        {
            UIVertex vertex = new UIVertex
            {
                position = point ,
                color = color0 ,
            };
            return vertex;
        }
    }
}
