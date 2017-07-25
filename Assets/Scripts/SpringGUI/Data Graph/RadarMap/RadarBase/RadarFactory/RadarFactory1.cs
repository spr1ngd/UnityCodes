
/*=========================================
* Author: Administrator
* DateTime:2017/7/24 18:04:02
* Description:$safeprojectname$
==========================================*/

using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    /// <summary>
    /// 彩色填充
    /// </summary>
    public class RadarFactory1 : BaseRadarFactory
    {
        public override VertexHelper DrawRadar(VertexHelper vh, Rect rect, RadarBaseData radardata)
        {
            return base.DrawRadar(vh, rect, radardata);
        }

        public override VertexHelper DrawBase(VertexHelper vh)
        {
            float perRadian = Mathf.PI * 2.0f / radarData.ItemCount;
            if ( radarData.Colorful )
            {
                float perRadius = radarData.Radius / radarData.Layers;
                for ( int i = 1 ; i <= radarData.Layers ; i++ )
                {
                    float radius = perRadius * i;
                    Color color = radarData.LayerColors[i - 1];
                    for ( int j = 0 ; j <= radarData.ItemCount ; j++ )
                    {
                        float startRadian = perRadian * j;
                        float endRadian = perRadian * ( j + 1 );
                        Vector2 startPosF = new Vector2(Mathf.Cos(startRadian) , Mathf.Sin(startRadian)) * radius;
                        Vector2 endPosF = new Vector2(Mathf.Cos(endRadian) , Mathf.Sin(endRadian)) * radius;
                        Vector2 startPosS = new Vector2(Mathf.Cos(startRadian) , Mathf.Sin(startRadian)) * ( radius - perRadius );
                        Vector2 endPosS = new Vector2(Mathf.Cos(endRadian) , Mathf.Sin(endRadian)) * ( radius - perRadius );

                        vh.AddUIVertexQuad(new UIVertex[]
                        {
                            GetUIVertex(startPosF,color),
                            GetUIVertex(startPosS,color),
                            GetUIVertex(endPosS,color),
                            GetUIVertex(endPosF,color)
                        });
                    }
                }
            }
            vh = base.DrawBase(vh);
            return vh;
        }

        public override VertexHelper DrawAxis(VertexHelper vh)
        {
            return base.DrawAxis(vh);
        }
    }

    public class RadarFactory2 : BaseRadarFactory
    {
        public override VertexHelper DrawRadar( VertexHelper vh , Rect rect , RadarBaseData radardata )
        {
            return base.DrawRadar(vh , rect , radardata);
        }

        public override VertexHelper DrawBase( VertexHelper vh )
        {
            return base.DrawBase(vh);
        }

        public override VertexHelper DrawAxis( VertexHelper vh )
        {
            return base.DrawAxis(vh);
        }
    }

    public class RadarFactory3 : BaseRadarFactory
    {
        public override VertexHelper DrawRadar( VertexHelper vh , Rect rect , RadarBaseData radardata )
        {
            return base.DrawRadar(vh , rect , radardata);
        }

        public override VertexHelper DrawBase( VertexHelper vh )
        {
            return base.DrawBase(vh);
        }

        public override VertexHelper DrawAxis( VertexHelper vh )
        {
            return base.DrawAxis(vh);
        }
    }

    public class RadarFactory4 : BaseRadarFactory
    {
        public override VertexHelper DrawRadar( VertexHelper vh , Rect rect , RadarBaseData radardata )
        {
            return base.DrawRadar(vh , rect , radardata);
        }

        public override VertexHelper DrawBase( VertexHelper vh )
        {
            return base.DrawBase(vh);
        }

        public override VertexHelper DrawAxis( VertexHelper vh )
        {
            return base.DrawAxis(vh);
        }
    }

    public class RadarFactory5 : BaseRadarFactory
    {
        public override VertexHelper DrawRadar( VertexHelper vh , Rect rect , RadarBaseData radardata )
        {
            return base.DrawRadar(vh , rect , radardata);
        }

        public override VertexHelper DrawBase( VertexHelper vh )
        {
            return base.DrawBase(vh);
        }

        public override VertexHelper DrawAxis( VertexHelper vh )
        {
            return base.DrawAxis(vh);
        }
    }

    //todo 如果你想拓展请在此处编写
}