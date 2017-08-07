
/*=========================================
* Author: Administrator
* DateTime:2017/6/12 15:34:59
* Description: draw multi-ct
==========================================*/

using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public class MultiColoredTape : MainColorTape
    {
        public Color TopLeft = Color.yellow;
        public Color TopRight = Color.white;
        public Color BottomLeft = Color.red;
        public Color BottomRight = Color.magenta;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            RectSize = GetPixelAdjustedRect().size;
            var halfX = RectSize.x / 2.0f;
            var halfY = RectSize.y / 2.0f;
            UIVertex topLeft = GetUIVertex(new Vector2(-halfX , halfY) , TopLeft);
            UIVertex topRight = GetUIVertex(new Vector2(halfX , halfY) , TopRight);
            UIVertex bottomLeft = GetUIVertex(new Vector2(-halfX , -halfY) , BottomLeft);
            UIVertex bottomRight = GetUIVertex(new Vector2(halfX , -halfY) , BottomRight);
            vh.AddUIVertexQuad(new UIVertex[] { topLeft , topRight , bottomRight , bottomLeft });
            if ( Outline ) DrawOutline(vh);
        }
        
        public override void SetColors(Color[] colors)
        {
            TopLeft = colors[0];
            TopRight = colors[1];
            BottomLeft = colors[2];
            BottomRight = colors[3];
            OnEnable();
        }

        public void SetColors( float value , ColorPicker.E_PaletteMode mode )
        {
            switch (mode)
            {
                case ColorPicker.E_PaletteMode.Red:
                    TopLeft.r = value;
                    TopRight.r = value;
                    BottomLeft.r = value;
                    BottomRight.r = value;
                    break;
                case ColorPicker.E_PaletteMode.Green:
                    TopLeft.g = value;
                    TopRight.g = value;
                    BottomLeft.g = value;
                    BottomRight.g = value;
                    break;
                case ColorPicker.E_PaletteMode.Blue:
                    TopLeft.b = value;
                    TopRight.b = value;
                    BottomLeft.b = value;
                    BottomRight.b = value;
                    break;
            }
            Rebuild();
        }

        public override Color GetColor( Vector2 position )
        {
            float halfLenght = RectSize.x / 2.0f;
            var topleft = ( getsacle(position , new Vector2(-halfLenght , halfLenght)) ) * TopLeft;
            var topright = ( getsacle(position , new Vector2(halfLenght , halfLenght)) ) * TopRight;
            var bottomleft = ( getsacle(position , new Vector2(-halfLenght , -halfLenght)) ) * BottomLeft;
            var bottomright = ( getsacle(position , new Vector2(halfLenght , -halfLenght)) ) * BottomRight;
            var color = topleft + topright + bottomleft + bottomright;
            return color;
        }
        private float getsacle( Vector2 position , Vector2 index )
        {
            float lenght = Vector3.Distance(position , index);
            if ( lenght > RectSize.x )
                return 0;
            return 1 - lenght / RectSize.x;
        }

        public Vector2 GetPosition(Color color0, ColorPicker.E_PaletteMode mode)
        {
            var red = color0.r;
            var green = color0.g;
            var blue = color0.b;
            var origin = new Vector2(-RectSize.x / 2.0f , -RectSize.y / 2.0f);
            switch (mode)
            {
                case ColorPicker.E_PaletteMode.Red:
                    return origin + new Vector2(RectSize.x * blue , RectSize.y * green);
                case ColorPicker.E_PaletteMode.Green:
                    return origin + new Vector2(RectSize.x * blue, RectSize.y * red);
                case ColorPicker.E_PaletteMode.Blue:
                    return origin + new Vector2(RectSize.x * red , RectSize.y * green);
            }
            return Vector2.zero;
        }
    }
}