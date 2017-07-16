
/*=========================================
* Author : SpringDong 
* DateTime : 2017/6/11 14:56:23
* Email : 540637360@qq.com
* Description : 最终选中色，在调色板的最顶部
*               绘制最终颜色的框,因为要显示透明值效果，所以自行绘制,继承至ColoredTape即可
*               这样还可以共用外边框，以为系统提供的外边框要比自己绘制更加消耗性能,有兴趣
*               的可以查看一下Outline源码。 
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