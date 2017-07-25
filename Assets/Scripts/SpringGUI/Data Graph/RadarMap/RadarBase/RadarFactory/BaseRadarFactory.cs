
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
        // xy坐标的基础数据
        public bool Axis = false;
        public bool ShowArrow = false;
        public bool ShowAsixMesh = true;
        public float AxisWidth = 2.0f;
        public float ArrowSize = 4.0f;
        public Color AxisColor = Color.black;
        public Color AxisMeshColor = Color.cyan;
        
        [Space]
        // 雷达图的基础数据
        public bool Base = true;
        public float Radius = 80.0f;
        public int ItemCount = 6;
        public bool ShowInternalMesh = true;
        public Color MeshColor = Color.gray;
        public Color InternalMeshColor = Color.yellow;
        public float MeshWidth = 2.0f;
        public float InternalMeshWidth = 1.0f;
        public bool Colorful = true;
        public int Layers = 5;
        public float LineWidth = 2.0f;
        public List<Color> LayerColors = new List<Color>();

        [Space]
        public List<Color> LineColors = new List<Color>();
        public List<GameObject> Icons = new List<GameObject>();

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
        public void Adddata( IList<RadarData> datas )
        {
            this.datas.AddRadarData(datas);
        }
        public void RemoveAllData()
        {
            datas.RemoveAll();
        }
    }

    public class BaseRadarFactory :IRadarFactory
    {
        protected Vector2 origin = Vector2.zero;
        protected RadarBaseData radarData = null;
        protected RadarDatas radarDatas = null;
        protected Vector2 size = Vector2.zero;
        
        /// <summary>
        /// 绘制雷达线段
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="rect"></param>
        /// <param name="radardata"></param>
        /// <returns></returns>
        public virtual VertexHelper DrawRadar( VertexHelper vh , Rect rect ,RadarBaseData radardata)
        {
            this.radarData = radardata;
            this.radarDatas = this.radarData.Getdata();
            this.size = rect.size;
            origin = new Vector2(-size.x / 2.0f , -size.y / 2.0f);
            vh = DrawBase(vh);
            vh = DrawAxis(vh);
            vh = DrawLine(vh);
            return vh;
        }

        /// <summary>
        /// Drwas the line.
        /// </summary>
        /// <returns>The line.</returns>
        /// <param name="vh">Vh.</param>
        public virtual VertexHelper DrawLine( VertexHelper vh )
        {
            // 在这里绘制基础的线段
            float perRadian = Mathf.PI * 2.0f / radarData.ItemCount;
            for( int i = 0 ; i < radarDatas.datas.Count ; i++ )
            {
                RadarData radarline = radarDatas.datas[i];
                Color lineColor = radarData.LineColors[i];
                //获取icon图标
                GameObject iconPrefab = null;
                if (i < radarData.Icons.Count)
                    iconPrefab = radarData.Icons[i];
                Vector2 startPos;
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
                    startPos = new Vector2( Mathf.Cos(startradian),Mathf.Sin(startradian)) *startradius;
                    Vector2 endPos = new Vector2( Mathf.Cos(endRadian),Mathf.Sin(endRadian)) * endRadius;
                    var newVertexs = GetQuad(startPos, endPos, lineColor, radarData.LineWidth);
                    vh.AddUIVertexQuad(newVertexs);
                    if (j > 0)
                    {
                        //todo 弥补线段的缺陷
                        vh.AddUIVertexQuad(new UIVertex[]
                            {
                                oldVertexs[1],
                                newVertexs[0],
                                oldVertexs[2],
                                newVertexs[3]
                            });
                    }
                    oldVertexs = newVertexs;
                    //ShowIcon(iconPrefab,startPos);
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
        /// Gets the quad.
        /// </summary>
        /// <returns>The quad.</returns>
        /// <param name="startPos">Start position.</param>
        /// <param name="endPos">End position.</param>
        /// <param name="color0">Color0.</param>
        /// <param name="LineWidth">Line width.</param>
        protected UIVertex[] GetQuad( Vector2 startPos , Vector2 endPos , Color color0 , float LineWidth = 2.0f )
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

        /// <summary>
        /// Gets the user interface vertex.
        /// </summary>
        /// <returns>The user interface vertex.</returns>
        /// <param name="point">Point.</param>
        /// <param name="color0">Color0.</param>
        protected UIVertex GetUIVertex( Vector2 point , Color color0 )
        {
            UIVertex vertex = new UIVertex
            {
                position = point ,
                color = color0 ,
            };
            return vertex;
        }

        /// <summary>
        /// Gets the quad dottedline.
        /// </summary>
        /// <returns>The quad dottedline.</returns>
        /// <param name="startpos">Startpos.</param>
        /// <param name="endPos">End position.</param>
        /// <param name="color0">Color0.</param>
        /// <param name="lineWidth">Line width.</param>
        protected UIVertex[] GetQuadDottedline( Vector2 startpos,Vector2 endPos , Color color0 ,float lineWidth = 2.0f )
        {
            return null;
        }

        /// <summary>
        /// Shows the icon.
        /// </summary>
        /// <param name="iconPrefab">Icon prefab.</param>
        /// <param name="position">Position.</param>
        protected void ShowIcon( GameObject iconPrefab , Vector2 position )
        {
            if (null == iconPrefab)
            {
                Debug.LogWarning("预制物"+iconPrefab.name+"不存在");
                return;
            }
            GameObject icon = GameObject.Instantiate(iconPrefab) as GameObject;
            if (null == icon)
            {
                Debug.LogWarning("实例化"+iconPrefab.name+"失败");
                return;
            }
            icon.transform.SetParent(radarData.parent);
            icon.transform.localScale = Vector2.one;
        }
    }
}