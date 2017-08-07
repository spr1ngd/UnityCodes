
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
        public override VertexHelper DrawBase(VertexHelper vh)
        {
            float perRadian = Mathf.PI * 2.0f / radarData.ItemCount;
            if ( radarData.Colorful )
            {
                float perRadius = radarData.Radius / radarData.Layers;
                for ( int i = 1 ; i <= radarData.Layers ; i++ )
                {
                    float radius = perRadius * i;
                    Color color = radarData.GetLayerColor(i - 1);
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
    }

    /// <summary>
    /// 非封闭线条绘制
    /// </summary>
    public class RadarFactory2 : BaseRadarFactory
    {
        public override VertexHelper DrawLine(VertexHelper vh)
        {
            float perRadian = Mathf.PI * 2.0f / radarData.ItemCount;
            for ( int i = 0 ; i < radarDatas.datas.Count ; i++ )
            {
                RadarData radarline = radarDatas.datas[i];
                Color lineColor = radarData.GetLineColor(i);
                UIVertex[] oldVertexs = null;
                for ( int j = 0 ; j < radarline.keypoints.Count - 1 ; j++ )
                {
                    float startradian = perRadian * j;
                    float startradius = radarData.Radius * radarline.keypoints[j];
                    int index = j + 1;
                    if ( index >= radarline.keypoints.Count )
                        index = 0;
                    float endRadian = perRadian * index;
                    float endRadius = radarData.Radius * radarline.keypoints[index];
                    var startPos = new Vector2(Mathf.Cos(startradian) , Mathf.Sin(startradian)) * startradius;
                    Vector2 endPos = new Vector2(Mathf.Cos(endRadian) , Mathf.Sin(endRadian)) * endRadius;
                    var newVertexs = GetQuad(startPos , endPos , lineColor , radarData.LineWidth);
                    vh.AddUIVertexQuad(newVertexs);
                    if ( j > 0 )
                    {
                        vh.AddUIVertexQuad(new UIVertex[]
                            {
                                oldVertexs[1],
                                newVertexs[0],
                                oldVertexs[2],
                                newVertexs[3]
                            });
                    }
                    oldVertexs = newVertexs;
                }
            }
            return vh;
        }
    }

    /// <summary>
    /// 透明闭环绘制
    /// </summary>
    public class RadarFactory3 : BaseRadarFactory
    {
        public override VertexHelper DrawLine( VertexHelper vh )
        {
            float perRadian = Mathf.PI * 2.0f / radarData.ItemCount;
            for ( int i = 0 ; i < radarDatas.datas.Count ; i++ )
            {
                RadarData radarline = radarDatas.datas[i];
                Color lineColor = radarData.GetLineColor(i);
                Color fillColor = new Color(lineColor.r,lineColor.g,lineColor.b,0.6f);
                UIVertex[] oldVertexs = null;
                for ( int j = 0 ; j < radarline.keypoints.Count ; j++ )
                {
                    float startradian = perRadian * j;
                    float startradius = radarData.Radius * radarline.keypoints[j];
                    int index = j + 1;
                    if ( index >= radarline.keypoints.Count )
                        index = 0;
                    float endRadian = perRadian * index;
                    float endRadius = radarData.Radius * radarline.keypoints[index];
                    var startPos = new Vector2(Mathf.Cos(startradian) , Mathf.Sin(startradian)) * startradius;
                    Vector2 endPos = new Vector2(Mathf.Cos(endRadian) , Mathf.Sin(endRadian)) * endRadius;
                    var newVertexs = GetQuad(startPos , endPos , lineColor , radarData.LineWidth);
                    vh.AddUIVertexQuad( new UIVertex[]
                    {
                        GetUIVertex(startPos,fillColor),
                        GetUIVertex(endPos,fillColor),
                        GetUIVertex(Vector2.zero, fillColor),
                        GetUIVertex(startPos,fillColor)
                    } );
                    vh.AddUIVertexQuad(newVertexs);
                    if ( j > 0 )
                    {
                        vh.AddUIVertexQuad(new UIVertex[]
                            {
                                oldVertexs[1],
                                newVertexs[0],
                                oldVertexs[2],
                                newVertexs[3]
                            });
                    }
                    oldVertexs = newVertexs;
                }
            }
            return vh;
        }
    }

    /// <summary>
    /// 绘制透明边框
    /// </summary>
    public class RadarFactory4 : BaseRadarFactory
    {
        public override VertexHelper DrawBase( VertexHelper vh )
        {
            if ( !radarData.Base )
                return vh;
            float perRadian = Mathf.PI * 2.0f / radarData.ItemCount;
            for ( int i = 0 ; i <= radarData.ItemCount ; i++ ) 
            {
                float startRadian = perRadian * i;
                float endRadian = perRadian * ( i + 1 );
                Vector2 startPos = new Vector2(Mathf.Cos(startRadian) , Mathf.Sin(startRadian)) * radarData.Radius;
                Vector2 endPos = new Vector2(Mathf.Cos(endRadian) , Mathf.Sin(endRadian)) * radarData.Radius;
                GetQuadDottedline( vh, startPos , endPos , radarData.MeshColor , radarData.MeshWidth);
            }
            if ( radarData.ShowInternalMesh )
            {
                float perRadius = radarData.Radius / radarData.Layers;
                for ( int i = 1 ; i < radarData.Layers ; i++ )
                {
                    float radius = perRadius * i;
                    for ( int j = 0 ; j <= radarData.ItemCount ; j++ )
                    {
                        float startRadian = perRadian * j;
                        float endRadian = perRadian * ( j + 1 );
                        Vector2 startPos = new Vector2(Mathf.Cos(startRadian) , Mathf.Sin(startRadian)) * radius;
                        Vector2 endPos = new Vector2(Mathf.Cos(endRadian) , Mathf.Sin(endRadian)) * radius;
                        GetQuadDottedline(vh,startPos , endPos , radarData.InternalMeshColor , radarData.InternalMeshWidth);
                    }
                }
            }
            return vh;
        }
    }

    /// <summary>
    /// 绘制虚线边框，并可填充颜色
    /// </summary>
    public class RadarFactory5 : BaseRadarFactory
    {
        public override VertexHelper DrawBase( VertexHelper vh )
        {
            // 绘制边框虚线
            if ( !radarData.Base )
                return vh;
            float perRadian = Mathf.PI * 2.0f / radarData.ItemCount;
            for ( int i = 0 ; i <= radarData.ItemCount ; i++ )
            {
                float startRadian = perRadian * i;
                float endRadian = perRadian * ( i + 1 );
                Vector2 startPos = new Vector2(Mathf.Cos(startRadian) , Mathf.Sin(startRadian)) * radarData.Radius;
                Vector2 endPos = new Vector2(Mathf.Cos(endRadian) , Mathf.Sin(endRadian)) * radarData.Radius;
                GetQuadDottedline(vh , startPos , endPos , radarData.MeshColor , radarData.MeshWidth);
            }
            if ( radarData.ShowInternalMesh )
            {
                float perRadius = radarData.Radius / radarData.Layers;
                for ( int i = 1 ; i < radarData.Layers ; i++ )
                {
                    float radius = perRadius * i;
                    for ( int j = 0 ; j <= radarData.ItemCount ; j++ )
                    {
                        float startRadian = perRadian * j;
                        float endRadian = perRadian * ( j + 1 );
                        Vector2 startPos = new Vector2(Mathf.Cos(startRadian) , Mathf.Sin(startRadian)) * radius;
                        Vector2 endPos = new Vector2(Mathf.Cos(endRadian) , Mathf.Sin(endRadian)) * radius;
                        GetQuadDottedline(vh , startPos , endPos , radarData.InternalMeshColor , radarData.InternalMeshWidth);
                    }
                }
            }
            if ( radarData.Colorful )
            {
                float perRadius = radarData.Radius / radarData.Layers;
                for ( int i = 1 ; i <= radarData.Layers ; i++ )
                {
                    float radius = perRadius * i;
                    Color color = radarData.GetLayerColor(i - 1);
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
            return vh;
        }
    }

    //todo 如果你想拓展请在此处编写
}