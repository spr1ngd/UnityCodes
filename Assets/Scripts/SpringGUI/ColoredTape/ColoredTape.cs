/*=========================================
* Author: Administrator
* DateTime:2017/6/8 14:11:57
* Description:渐变工具,可作为蒙版对别的物体进行遮罩,比如遮罩彩色字体
==========================================*/

using System;
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
        [SerializeField]
        public E_DrawDirection TapeDirection = E_DrawDirection.Vertical;
        [SerializeField]
        public bool Outline = false;
        [SerializeField]
        public float OuelineWidth = 1.0f;
        [SerializeField]
        public Color OutlineColor = Color.black;
            
        [SerializeField]
        private List<Color> m_Colors = new List<Color>() {Color.red,Color.magenta};
        private Vector2 _rectSize;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            _rectSize = GetPixelAdjustedRect().size;
            vh.Clear();
            if (TapeDirection == E_DrawDirection.Vertical) DrawVerticalColoredTape(vh);
            else DrawHorizontalColoredTape(vh);
            if (Outline) DrawOutline(vh);
        }
         
        private void DrawVerticalColoredTape( VertexHelper vh )
        {
            int colorNumber = m_Colors.Count;
            float offset = _rectSize.y / ( colorNumber -1);
            Vector2 topLeftPos = new Vector2(-_rectSize.x / 2.0f , _rectSize.y / 2.0f);
            Vector2 topRightPos = new Vector2(_rectSize.x / 2.0f , _rectSize.y / 2.0f);
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
            float offset = _rectSize.x / ( colorNumber - 1 );
            Vector2 topLeftPos = new Vector2(-_rectSize.x / 2.0f , _rectSize.y / 2.0f);
            Vector2 bottomLeftPos = topLeftPos - new Vector2(0 , _rectSize.y);
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
        private void DrawOutline( VertexHelper vh )
        {
            var topLeft = new Vector2(-_rectSize.x / 2.0f , _rectSize.y / 2.0f);
            var a = topLeft + new Vector2(-OuelineWidth / 2.0f , OuelineWidth);
            var b = a - new Vector2(0, _rectSize.y + OuelineWidth *2);
            var c = a + new Vector2(_rectSize.x + OuelineWidth , 0);
            var d = c - new Vector2(0 , _rectSize.y + OuelineWidth * 2);
            var e = topLeft + new Vector2(0 , OuelineWidth / 2.0f);
            var f = e + new Vector2(_rectSize.x, 0);
            var g = e - new Vector2(0, _rectSize.y + OuelineWidth);
            var h = f - new Vector2(0 , _rectSize.y + OuelineWidth);
            vh.AddUIVertexQuad(GetQuad(a , b , OutlineColor , OuelineWidth));
            vh.AddUIVertexQuad(GetQuad(c , d , OutlineColor , OuelineWidth));
            vh.AddUIVertexQuad(GetQuad(e , f , OutlineColor , OuelineWidth));
            vh.AddUIVertexQuad(GetQuad(g , h , OutlineColor , OuelineWidth));
        }

        private UIVertex GetUIVertex( Vector2 point , Color color0 )
        {
            UIVertex vertex = new UIVertex
            {
                position = point ,
                color = color0 ,
            };
            return vertex;
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
    }
}