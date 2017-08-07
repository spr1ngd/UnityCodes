
/*=========================================
* Author: Administrator
* DateTime:2017/7/24 17:55:06
* Description:$safeprojectname$
==========================================*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    [Serializable]
    public class RadarBaseData
    {
        [Header("RadarMap XY Axis Setting")]
        public bool Axis = false;
        public bool ShowArrow = false;
        public bool ShowAsixMesh = true;
        public float AxisWidth = 2.0f;
        public float ArrowSize = 4.0f;
        public Color AxisColor = Color.black;
        public Color AxisMeshColor = Color.cyan;

        [Header("RadarMap Base Data Setting")]
        public bool Base = true;
        public float Radius = 80.0f;
        public int ItemCount = 6;
        [Header("RadarMap Base Mesh Setting")]
        public bool ShowInternalMesh = true;
        public Color MeshColor = Color.gray;
        public Color InternalMeshColor = Color.yellow;
        public float MeshWidth = 2.0f;
        public float InternalMeshWidth = 1.0f;

        [Header("RadarMap colorful Setting")]
        public bool Colorful = true;
        public int Layers = 5;
        public float LineWidth = 2.0f;
        public List<Color> LayerColors = new List<Color>();
        public List<Color> LineColors = new List<Color>();

        [NonSerialized]
        private RadarDatas datas = new RadarDatas();
        [NonSerialized]
        public Transform parent = null;

        // 操作接口
        public RadarDatas Getdata()
        {
            return datas;
        }
        public void Adddata( RadarData data )
        {
            datas.AddRadarData(data);
        }
        public void Adddata( IList<RadarData> vdatas ) 
        {
            this.datas.AddRadarData(vdatas);
        }
        public void RemoveAllData()
        {
            datas.RemoveAll();
        }
        
        public Color GetLayerColor( int id  )
        {
            if (id >= LayerColors .Count)
            {
                Debug.LogError("Can not found layer color,please set layer colors in inspector panel.");
                return Color.magenta;
            }
            return LayerColors[id];
        }

        public Color GetLineColor( int id  )
        {
            if (id >= LineColors.Count)
            {
                Debug.LogError("Can not found lien color ,please set line colors in inspector panel.");
                return Color.magenta;
            }
            return LineColors[id];
        }
    }

    public class BaseRadarFactory : sgBase, IRadarFactory
    {
        protected Vector2 origin = Vector2.zero;
        protected RadarBaseData radarData = null;
        protected RadarDatas radarDatas = null;
        protected Vector2 size = Vector2.zero;

        //使用SpringGUIBase继承或者直接获取一个实例,如果你需要继承其他类型时，可以使用构造实例的方法，如下
        //private ISpringGUIBase springGUIBase = new SpringGUIBase();

        private IRadarBase radarbase = null;
        private IRadarline radarline = null;

        public BaseRadarFactory(){}

        public BaseRadarFactory( IRadarBase radarBase,IRadarline radarLine )
        {
            this.radarbase = radarBase;
            this.radarline = radarLine;
        }

        /// <summary>
        /// 绘制雷达线段
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="rect"></param>
        /// <param name="radardata"></param>
        /// <returns></returns>
        public virtual VertexHelper DrawRadar( VertexHelper vh , Rect rect ,RadarBaseData radardata)
        {
            if (null == radardata)
                return vh;
            this.radarData = radardata;
            this.radarDatas = this.radarData.Getdata();
            this.size = rect.size;
            this.origin = new Vector2(-size.x / 2.0f , -size.y / 2.0f);
            DrawBase(vh);
            DrawAxis(vh);
            DrawLine(vh);
            return vh;
        }

        /// <summary>
        /// Drwas the line. 在这里绘制基础的线段
        /// </summary>
        /// <returns>The line.</returns>
        /// <param name="vh">Vh.</param>
        public virtual VertexHelper DrawLine( VertexHelper vh )
        {
            float perRadian = Mathf.PI * 2.0f / radarData.ItemCount;
            for( int i = 0 ; i < radarDatas.datas.Count ; i++ )
            {
                RadarData radarline = radarDatas.datas[i];
                Color lineColor = radarData.GetLineColor(i);
                UIVertex[] oldVertexs = null;
                for( int j = 0 ; j < radarline.keypoints.Count ;j++ )
                {
                    float startradian = perRadian * j;
                    float startradius = radarData.Radius * radarline.keypoints[j];
                    int index = j + 1;
                    if (index >= radarline.keypoints.Count)
                        index = 0;
                    float endRadian = perRadian * index;
                    float endRadius = radarData.Radius * radarline.keypoints[index];
                    var startPos = new Vector2( Mathf.Cos(startradian),Mathf.Sin(startradian)) *startradius;
                    Vector2 endPos = new Vector2( Mathf.Cos(endRadian),Mathf.Sin(endRadian)) * endRadius;
                    var newVertexs = GetQuad(startPos, endPos, lineColor, radarData.LineWidth);
                    vh.AddUIVertexQuad(newVertexs);
                    if (j > 0)
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

        /// <summary>
        /// 绘制xy坐标
        /// </summary>
        /// <param name="vh"></param>
        /// <returns></returns>
        public virtual VertexHelper DrawAxis( VertexHelper vh )
        {
            if (radarData.Axis)
            {
                Vector2 startPosX = Vector2.zero - new Vector2(size.x / 2.0f , 0);
                Vector2 endPosX = startPosX + new Vector2(size.x , 0);

                Vector2 startPosY = Vector2.zero - new Vector2(0 , size.y / 2.0f);
                Vector2 endPosY = startPosY + new Vector2(0 , size.y);
                vh.AddUIVertexQuad(GetQuad(startPosX , endPosX , radarData.AxisColor , radarData.AxisWidth));
                vh.AddUIVertexQuad(GetQuad(startPosY , endPosY , radarData.AxisColor , radarData.AxisWidth));
                if ( radarData.ShowArrow )
                {
                    var xFirst = endPosX + new Vector2(0 , radarData.ArrowSize);
                    var xSecond = endPosX + new Vector2(1.73f * radarData.ArrowSize , 0);
                    var xThird = endPosX + new Vector2(0 , -radarData.ArrowSize);
                    vh.AddUIVertexQuad(new UIVertex[]
                    {
                        GetUIVertex(xFirst,radarData.AxisColor),
                        GetUIVertex(xSecond,radarData.AxisColor),
                        GetUIVertex(xThird,radarData.AxisColor),
                        GetUIVertex(endPosX,radarData.AxisColor),
                    });

                    var yFirst = endPosY + new Vector2(-radarData.ArrowSize , 0);
                    var ySecond = endPosY + new Vector2(0 , 1.73f * radarData.ArrowSize);
                    var yThird = endPosY + new Vector2(radarData.ArrowSize , 0);
                    vh.AddUIVertexQuad(new UIVertex[]
                    {
                        GetUIVertex(yFirst,radarData.AxisColor),
                        GetUIVertex(ySecond,radarData.AxisColor),
                        GetUIVertex(yThird,radarData.AxisColor),
                        GetUIVertex(endPosY,radarData.AxisColor),
                    });
                }
            }
            if (radarData.ShowAsixMesh)
            {
                float perRadian = Mathf.PI * 2.0f / radarData.ItemCount;
                for (int i = 0; i < radarData.ItemCount; i++)
                {
                    float radian = perRadian * i;
                    Vector2 endPos = new Vector2(Mathf.Cos(radian) , Mathf.Sin(radian)) * radarData.Radius;
                    vh.AddUIVertexQuad(GetQuad(Vector2.zero, endPos, radarData.AxisMeshColor , radarData.AxisWidth));
                }
            }
            return vh;
        }

        /// <summary>
        /// 绘制雷达基础图
        /// </summary>
        /// <param name="vh"></param>
        /// <returns></returns>
        public virtual VertexHelper DrawBase( VertexHelper vh )
        {
            if ( !radarData.Base )
                return vh;
            float perRadian = Mathf.PI * 2.0f / radarData.ItemCount;
            UIVertex[] vertexs = null;
            for ( int i = 0 ; i <= radarData.ItemCount ; i++ )
            {
                float startRadian = perRadian * i;
                float endRadian = perRadian * ( i + 1 );
                Vector2 startPos = new Vector2(Mathf.Cos(startRadian) , Mathf.Sin(startRadian)) * radarData.Radius;
                Vector2 endPos = new Vector2(Mathf.Cos(endRadian) , Mathf.Sin(endRadian)) * radarData.Radius;
                var newVertexs = GetQuad(startPos , endPos , radarData.MeshColor , radarData.MeshWidth);
                vh.AddUIVertexQuad(newVertexs);
                if ( null == vertexs )
                    vertexs = newVertexs;
                else
                {
                    vh.AddUIVertexQuad(new UIVertex[]
                    {
                        vertexs[1],
                        newVertexs[0],
                        vertexs[2],
                        newVertexs[3]
                    });
                    vertexs = newVertexs;
                }
            }
            if (radarData.ShowInternalMesh)
            {
                float perRadius = radarData.Radius / radarData.Layers;
                UIVertex[] oldVertexs = null;
                for ( int i = 1 ; i < radarData.Layers ; i++ )
                {
                    float radius = perRadius * i;
                    for ( int j = 0 ; j <= radarData.ItemCount ; j++ )
                    {
                        float startRadian = perRadian * j;
                        float endRadian = perRadian * ( j + 1 );
                        Vector2 startPos = new Vector2(Mathf.Cos(startRadian) , Mathf.Sin(startRadian)) * radius;
                        Vector2 endPos = new Vector2(Mathf.Cos(endRadian) , Mathf.Sin(endRadian)) * radius;
                        var newVertexs = GetQuad(startPos , endPos , radarData.InternalMeshColor , radarData.InternalMeshWidth);
                        vh.AddUIVertexQuad(newVertexs);
                        if ( null == oldVertexs )
                            oldVertexs = newVertexs;
                        else
                        {
                            vh.AddUIVertexQuad(new UIVertex[]
                            {
                                oldVertexs[1],
                                newVertexs[0],
                                oldVertexs[2],
                                newVertexs[3]
                            });
                            oldVertexs = newVertexs;
                        }
                        if ( j == radarData.ItemCount )
                            oldVertexs = null;
                    }
                }
            }
            return vh;
        }
        
        /// <summary>
        /// Show Icon
        /// </summary>
        /// <param name="iconPrefab"></param>
        /// <param name="position"></param>
        protected void ShowIcon( GameObject iconPrefab , Vector2 position )
        {
            if (null == iconPrefab)
            {
                Debug.LogWarning("prefab"+iconPrefab.name+"can not be loaded");
                return;
            }
            GameObject icon = GameObject.Instantiate(iconPrefab) as GameObject;
            if (null == icon)
            {
                Debug.LogWarning("prefab" + iconPrefab.name+ "can not be loaded");
                return;
            }
            icon.transform.SetParent(radarData.parent);
            icon.transform.localScale = Vector2.one;
        }
    }
}