
/*=========================================
* Author : SpringDong 
* Description : draw main color ct
==========================================*/

using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public class MainColorTape : ColoredTape
    {
        [SerializeField]
        private Color _Color = Color.white;
        public Color Color
        {
            get { return _Color; }
            set
            {
                _Color = value;
                OnEnable();
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            RectSize = GetPixelAdjustedRect().size;
            var height = RectSize.y;
            var width = RectSize.x;
            var alphaHeight = height * 0.1f;
            var halfAlphaHeight = alphaHeight / 2.0f;
            var a = new Vector2(-width/2.0f , halfAlphaHeight);
            var b = a + new Vector2(width , 0);
            var c = a - new Vector2(0 , height / 2.0f);
            var d = c + new Vector2(width , 0);
            var e = d - new Vector2((1-Color.a) * width , 0);
            vh.AddUIVertexQuad(GetQuad(a , b , new Color(Color.r,Color.g,Color.b,1) , height - alphaHeight));
            vh.AddUIVertexQuad(GetQuad(c , e , Color.white , alphaHeight));
            vh.AddUIVertexQuad(GetQuad(e , d , Color.black , alphaHeight));
            if ( Outline ) DrawOutline(vh);
        }
    }
}