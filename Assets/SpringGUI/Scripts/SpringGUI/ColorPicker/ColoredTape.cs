
/*=========================================
* Author: springDong
* Description: SpringGUI.ColoredTape.
==========================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public class ColoredTape : MaskableGraphic
    {
        public enum E_DrawDirection
        {
            Vertical,
            Horizontal
        }

        [Header("Colored tape type setting")]

        [SerializeField]
        public E_DrawDirection TapeDirection = E_DrawDirection.Vertical;

        [Header("Colored tape outline setting")]

        [SerializeField]
        public bool Outline = true;
        [SerializeField]
        public float OuelineWidth = 1.0f;
        [SerializeField]
        public Color OutlineColor = Color.black;

        [Header("Colored tape colors setting")]
        [SerializeField]
        private List<Color> m_Colors = new List<Color>() { Color.red , Color.magenta };
        [HideInInspector]
        public Vector2 RectSize;

        #region draw

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            RectSize = GetPixelAdjustedRect().size;
            vh.Clear();
            if (TapeDirection == E_DrawDirection.Vertical) DrawVerticalColoredTape(vh);
            else DrawHorizontalColoredTape(vh);
            if (Outline) DrawOutline(vh);
        }
        private void DrawVerticalColoredTape( VertexHelper vh )
        {
            int colorNumber = m_Colors.Count;
            float offset = RectSize.y / ( colorNumber -1);
            Vector2 topLeftPos = new Vector2(-RectSize.x / 2.0f , RectSize.y / 2.0f);
            Vector2 topRightPos = new Vector2(RectSize.x / 2.0f , RectSize.y / 2.0f);
            Vector2 bottomLeftPos = topLeftPos - new Vector2(0 , offset);
            Vector2 bottomRightPos = topRightPos - new Vector2(0 , offset);
            for ( int i = 0 ; i < colorNumber - 1; i++ )
            {
                Color startColor = m_Colors[i];
                Color endColor = m_Colors[i + 1];
                var first = GetUIVertex(topLeftPos, startColor);
                var second = GetUIVertex(topRightPos, startColor);
                var third = GetUIVertex(bottomRightPos,endColor);
                var four = GetUIVertex(bottomLeftPos, endColor);
                vh.AddUIVertexQuad(new UIVertex[] { first , second , third , four });
                topLeftPos = bottomLeftPos;
                topRightPos = bottomRightPos;
                bottomLeftPos = topLeftPos - new Vector2(0 , offset);
                bottomRightPos = topRightPos - new Vector2(0 , offset);
            }
        }
        private void DrawHorizontalColoredTape( VertexHelper vh )
        {
            int colorNumber = m_Colors.Count;
            float offset = RectSize.x / ( colorNumber - 1 );
            Vector2 topLeftPos = new Vector2(-RectSize.x / 2.0f , RectSize.y / 2.0f);
            Vector2 bottomLeftPos = topLeftPos - new Vector2(0 , RectSize.y);
            Vector2 topRightPos = topLeftPos + new Vector2(offset, 0);
            Vector2 bottomRightPos = bottomLeftPos + new Vector2(offset, 0);
            for ( int i = 0 ; i < colorNumber - 1 ; i++ )
            {
                Color startColor = m_Colors[i];
                Color endColor = m_Colors[i + 1];
                var first = GetUIVertex(topLeftPos , startColor);
                var second = GetUIVertex(topRightPos , endColor);
                var third = GetUIVertex(bottomRightPos , endColor);
                var four = GetUIVertex(bottomLeftPos , startColor);
                vh.AddUIVertexQuad(new UIVertex[] { first , second , third , four });
                topLeftPos = topRightPos;
                bottomLeftPos = bottomRightPos;
                topRightPos = topLeftPos + new Vector2(offset,0);
                bottomRightPos = bottomLeftPos + new Vector2(offset , 0);
            }
        }
        public void DrawOutline( VertexHelper vh )
        {
            var topLeft = new Vector2(-RectSize.x / 2.0f , RectSize.y / 2.0f);
            var a = topLeft + new Vector2(-OuelineWidth / 2.0f , OuelineWidth);
            var b = a - new Vector2(0, RectSize.y + OuelineWidth *2);
            var c = a + new Vector2(RectSize.x + OuelineWidth , 0);
            var d = c - new Vector2(0 , RectSize.y + OuelineWidth * 2);
            var e = topLeft + new Vector2(0 , OuelineWidth / 2.0f);
            var f = e + new Vector2(RectSize.x, 0);
            var g = e - new Vector2(0, RectSize.y + OuelineWidth);
            var h = f - new Vector2(0 , RectSize.y + OuelineWidth);
            vh.AddUIVertexQuad(GetQuad(a , b , OutlineColor , OuelineWidth));
            vh.AddUIVertexQuad(GetQuad(c , d , OutlineColor , OuelineWidth));
            vh.AddUIVertexQuad(GetQuad(e , f , OutlineColor , OuelineWidth));
            vh.AddUIVertexQuad(GetQuad(g , h , OutlineColor , OuelineWidth));
        }

        #endregion

        #region Helper methods
        public virtual void SetColors( Color[] colors )
        {
            m_Colors.Clear();
            foreach (Color color1 in colors)
                m_Colors.Add(color1);
            OnEnable();
        }

        public void SetColors( float value,ColorPicker.E_PaletteMode mode )
        {
            List<Color> colors = new List<Color>();
            switch (mode)
            {
                case ColorPicker.E_PaletteMode.Red:
                    foreach (Color mColor in m_Colors)
                        colors.Add(new Color(value , mColor.g , mColor.b , mColor.a));
                    break;
                case ColorPicker.E_PaletteMode.Green:
                    foreach ( Color mColor in m_Colors )
                        colors.Add(new Color(mColor.r , value , mColor.b , mColor.a));
                    break;
                case ColorPicker.E_PaletteMode.Blue:
                    foreach ( Color mColor in m_Colors )
                        colors.Add(new Color(mColor.r , mColor.g , value , mColor.a));
                    break;
            }
            m_Colors.Clear();
            m_Colors = colors;
            Rebuild();
        }

        public virtual Color GetColor( Vector2 position )
        {
            int colorCount = m_Colors.Count;
            switch (TapeDirection)
            {
                case E_DrawDirection.Horizontal:
                    var perX = RectSize.x / ( ( colorCount - 1 ) * 2 );
                    var doubelPer = perX * 2;
                    var lenght0 = position.x + RectSize.x / 2.0f;
                    int index0 = (int)( lenght0 / doubelPer );
                    var temp0 = lenght0 % doubelPer / doubelPer;
                    if (lenght0.Equals(RectSize.x))
                    {
                        index0--;
                        temp0++;
                    }
                    Color start = m_Colors[index0];
                    Color end = m_Colors[index0 + 1];
                    return start * ( 1 - temp0 ) + end * temp0;
                case E_DrawDirection.Vertical:
                    var perY = RectSize.y / ( ( colorCount - 1 ) * 2 );
                    var doublePer = perY * 2;
                    var lenght1 = RectSize.y / 2.0f -  position.y ;
                    var index1 = (int)( lenght1 / doublePer );
                    var temp1 = lenght1 % doublePer / doublePer;
                    if ( lenght1.Equals(RectSize.y) )
                    {
                        index1--;
                        temp1++;
                    }
                    Color start1 = m_Colors[index1];
                    Color end1 = m_Colors[index1 + 1];
                    return start1 * (1 - temp1) + end1 * temp1;
                default:
                    return Color.white;
            }
        }
        public Color GetColor( int index )
        {
            return m_Colors[index];
        }

        public virtual Vector2 GetPosition( Color color0 ) 
        {
            var red = color0.r;
            var green = color0.g;
            var blue = color0.b;
            var color1 = color0;
            var color2 = color0;
            var offset = 0.0f;
            ArrayList array = new ArrayList() { red , green , blue };
            array.Sort();

            if (array[2].Equals(red))
                color1 = Color.red;
            else if (array[2].Equals(green))
                color1 = Color.green;
            else if (array[2].Equals(blue))
                color1 = Color.blue;


            if ( array[1].Equals(red) )
            {
                color2 = Color.red;
                offset = color0.r;
            }
            if ( array[1].Equals(green) )
            {
                color2 = Color.green;
                offset = color0.g;
            }
            if ( array[1].Equals(blue) )
            {
                color2 = Color.blue;
                offset = color0.b;
            }
            var pos1 = returnIndex(color1);
            var pos2 = returnIndex(color2);
            if ( color1 == Color.red && color2 == Color.green )
                pos1 = m_Colors.Count - 1;
            if ( color2 == Color.red && color1 == Color.green )
                pos2 = m_Colors.Count - 1;
            switch (TapeDirection)
            {
                case E_DrawDirection.Vertical:
                    var position1 = new Vector2(0 , RectSize.y / 2.0f - pos1 * RectSize.y / ( m_Colors.Count - 1 ));
                    var position2 = new Vector2(0 , RectSize.y / 2.0f - pos2 * RectSize.y / ( m_Colors.Count - 1 ));
                    int sign1 = 1;
                    if (position1.y > position2.y)
                        sign1 = -1;
                    else
                        sign1 = 1;
                    return position1 + new Vector2(0 , ( RectSize.y / ( m_Colors.Count - 1 ) ) * offset) * sign1;
                case E_DrawDirection.Horizontal:
                    var hp1 = new Vector2(-RectSize.x / 2.0f + pos1 * RectSize.x / ( m_Colors.Count - 1 ) , 0);
                    var hp2 = new Vector2(-RectSize.x / 2.0f + pos2 * RectSize.x / ( m_Colors.Count - 1 ) , 0);
                    int sign2 = 1;
                    if (hp1.x > hp2.x)
                        sign2 = -1;
                    else
                        sign2 = 1;
                    return hp1 + new Vector2(RectSize.x / ( m_Colors.Count - 1 ) * offset * sign2 , 0);
            }
            return Vector2.zero;
        }
        public virtual float GetScale( Color color0 )
        {
            Vector2 pos = GetPosition(color0);
            switch (TapeDirection)
            {
                case E_DrawDirection.Vertical:
                    return Mathf.Abs(( RectSize.y / 2.0f - pos.y ) / RectSize.y);
                case E_DrawDirection.Horizontal:
                    return Mathf.Abs(( pos.x - RectSize.x / 2.0f ) / RectSize.x);
            }
            return 0;
        }
        private int returnIndex( Color color0 )
        {
            for ( int i = 0 ; i < m_Colors.Count ; i++ )
            {
                if ( m_Colors[i].Equals(color0) )
                    return i;
            }
            return 0;
        }
        
        public virtual void Rebuild()
        {
            OnEnable();
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

        #endregion
    }
}