
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SpringFramework.Line
{
    [System.Serializable]
    public class LineGraphPointNew
    {
        public float Key;
        public float Value;

        public LineGraphPointNew( ) { }
        public LineGraphPointNew( float key , float value )
        {
            Key = key;
            Value = value;
        }
    }

    [System.Serializable]
    public class LineGraphDataNew
    {
        public int Index;
        public Color LineColor = Color.white;
        public Color FillColor = new Color(0.6f , 0.6f , 0.6f , 0.6f);
        public List<LineGraphPointNew> Datas;

        public LineGraphDataNew( ) { }
        public LineGraphDataNew( int index , Color color0 , Color color1 , List<LineGraphPointNew> datas )
        {
            Index = index;
            LineColor = color0;
            FillColor = color1;
            Datas = datas;
        }
    }

    [System.Serializable]
    public class LineGraphMeshDataNew
    {
        public int XAxisMeshCount ;
        public int YAxisMeshCount;
        public float XAxisUnit;
        public float YAxisUnit;
        [Range(0.5f,5f)] public float LineWidth;
        public Color LineColor;
        public bool IsImaginaryLine = true;
        [Range(0.5f,10f)] public float RealLineLenght = 5;
        [Range(0.5f, 10)] public float LineSpacing = 2;

        public LineGraphMeshDataNew(){}
        public LineGraphMeshDataNew( int xCount ,int yCount ,float xUnit ,float yUnit ,float lineWidth ,Color color0 ,bool isImaginaryLine)
        {
            XAxisMeshCount = xCount;
            YAxisMeshCount = yCount;
            XAxisUnit = xUnit;
            YAxisUnit = yUnit;
            LineWidth = lineWidth;
            LineColor = color0;
            IsImaginaryLine = isImaginaryLine;
        }
    }

    public class LineGraph : MaskableGraphic
    {
        //todo 绘制X、Y轴 
        //todo 绘制 表格 可虚线展示
        //todo 绘制折线
        //todo 绘制折线到XY轴的颜色覆盖
        //todo 绘制折线拐点的标记 并显示值
        //todo 绘制表格标记值 //以及单位

        private RectTransform _rect = null;
        public bool IsNeedXAxis = true;
        public bool IsNeedYAxis = true;
        public bool IsNeedArrow = false;
        public bool IsShowMesh = false;
        
        public LineGraphMeshDataNew MeshData = null;
        [Range(0.5f , 5)]
        public float LevelWidth = 2;
        public Color AxisColor = Color.black;
        private Vector2 XAxisPoint = Vector2.one;
        private Vector2 YAxisPoint = Vector2.one;

        [Range(0.5f , 5)]
        public float LineWidth = 2;
        public Color LineColor = Color.white;

        public bool IsFill = true;
        private List<LineGraphDataNew> _lineGraphData = null;

        public List<LineGraphDataNew> LineGraphDatas
        {
            get { return _lineGraphData; }
            set
            {
                this.enabled = false;
                _lineGraphData = value;
                this.enabled = true;
            }
        }

        protected override void Awake( )
        {
            LineGraphDatas = new List<LineGraphDataNew>()
            {
                new LineGraphDataNew(1,Color.white,new Color(0.9f,0.1f,0.1f,0.5f), new List<LineGraphPointNew>()
                {
                    new LineGraphPointNew(1,30),
                    new LineGraphPointNew(2,150),
                    new LineGraphPointNew(3,40),
                    new LineGraphPointNew(4,116),
                    new LineGraphPointNew(5,80),
                    new LineGraphPointNew(6,155),
                    new LineGraphPointNew(7,37),
                    new LineGraphPointNew(8,137),
                }),
                new LineGraphDataNew(2,Color.black,new Color(0.1f,0.1f,0.9f,0.5f),new List<LineGraphPointNew>()
                {
                    new LineGraphPointNew(1,80),
                    new LineGraphPointNew(2,22),
                    new LineGraphPointNew(3,140),
                    new LineGraphPointNew(4,16),
                    new LineGraphPointNew(5,180),
                    new LineGraphPointNew(6,55),
                    new LineGraphPointNew(7,137),
                    new LineGraphPointNew(8,87),
                }),
            };

            MeshData = new LineGraphMeshDataNew(10 , 8 , 2.0f , 5.0f , 2 , Color.gray , true);
        }

        protected override void OnPopulateMesh( VertexHelper vh )
        {
            if ( null == LineGraphDatas ) return;

            vh.Clear();
            _rect = this.rectTransform;
            Vector2 origin = new Vector2(-_rect.sizeDelta.x / 2.0f , -_rect.sizeDelta.y / 2.0f);

            #region 绘制X、Y轴

            if ( IsNeedXAxis )
            {
                Vector2 hFirst = origin;
                Vector2 hSecond = new Vector2(origin.x , origin.y - LevelWidth);
                Vector2 hThird = new Vector2(_rect.sizeDelta.x / 2.0f , origin.y - LevelWidth);
                Vector2 hFour = new Vector2(_rect.sizeDelta.x / 2.0f , origin.y);
                XAxisPoint = hFour;
                vh.AddUIVertexQuad(GetQuad(hFour , hThird , hSecond , hFirst , AxisColor));
            }

            if ( IsNeedYAxis )
            {
                Vector2 vFirst = new Vector2(origin.x , origin.y - LevelWidth);
                Vector2 vSecond = new Vector2(origin.x , _rect.sizeDelta.y / 2.0f);
                Vector2 vThird = new Vector2(origin.x - LevelWidth , _rect.sizeDelta.y / 2.0f);
                Vector2 vFour = new Vector2(origin.x - LevelWidth , origin.y - LevelWidth);
                YAxisPoint = vThird;
                vh.AddUIVertexQuad(GetQuad(vFour , vThird , vSecond , vFirst , AxisColor));
            }

            #endregion

            #region 绘制网格

            if ( IsShowMesh )
            {
                float xUnitLenght = _rect.sizeDelta.x / MeshData.XAxisMeshCount;
                float yUnitHeight = _rect.sizeDelta.y / MeshData.YAxisMeshCount;
                if (!MeshData.IsImaginaryLine)
                {
                    for (int x = 1; x <= MeshData.XAxisMeshCount; x++)
                    {
                        Vector2 startPos = origin + new Vector2(xUnitLenght * x, 0);
                        Vector2 endPos = startPos + new Vector2(0, _rect.sizeDelta.y);
                        vh.AddUIVertexQuad(GetQuad(startPos, endPos, MeshData.LineColor, MeshData.LineWidth));
                    }
                    for (int y = 1; y <= MeshData.YAxisMeshCount; y++)
                    {
                        Vector2 startPos = origin + new Vector2(0, yUnitHeight * y);
                        Vector2 endPos = startPos + new Vector2(_rect.sizeDelta.x, 0);
                        vh.AddUIVertexQuad(GetQuad(startPos, endPos, MeshData.LineColor, MeshData.LineWidth));
                    }
                }
                else
                {
                    float offsetUnit = MeshData.RealLineLenght + MeshData.LineSpacing;
                    //for ( int x = 1 ; x <= MeshData.XAxisMeshCount ; x++ )
                    //{
                    //    Vector2 startPos = origin + new Vector2(xUnitLenght * x , 0);
                    //    Vector2 endPos = startPos + new Vector2(0 , MeshData.RealLineLenght);
                    //    //vh.AddUIVertexQuad(GetQuad(startPos , endPos , MeshData.LineColor , MeshData.LineWidth));
                    //    //todo 把这个替换为虚线
                    //    for ( float height = origin.y, index = 1 ; height <= _rect.sizeDelta.y /2 ; height += offsetUnit, index++ )
                    //    {
                    //        vh.AddUIVertexQuad(GetQuad(startPos , endPos , MeshData.LineColor , MeshData.LineWidth));
                    //        startPos = startPos + new Vector2(0 * x , index);
                    //        endPos = endPos + new Vector2(0 , index);
                    //    }
                    //}
                    //for ( int y = 1 ; y <= MeshData.YAxisMeshCount ; y++ )
                    //{
                    //    Vector2 startPos = origin + new Vector2(0 , yUnitHeight * y);
                    //    Vector2 endPos = startPos + new Vector2(_rect.sizeDelta.x , 0);
                    //    //vh.AddUIVertexQuad(GetQuad(startPos , endPos , MeshData.LineColor , MeshData.LineWidth));
                    //    //todo 把这个替换为虚线
                    //}
                }
            }

            #endregion

            #region 绘制折线图

            float xUnit = _rect.sizeDelta.x / ( LineGraphDatas[0].Datas.Count - 1 );
            float yUnit = 0.2f;//todo 待改进

            for ( int index = 0 ; index < LineGraphDatas.Count ; index++ )
            {
                LineGraphDataNew data = LineGraphDatas[index];
                for ( int i = 0 ; i < data.Datas.Count - 1 ; i++ )
                {
                    LineGraphPointNew firstData = data.Datas[i];
                    LineGraphPointNew secondData = data.Datas[i + 1];
                    Vector2 firstPos = new Vector2(origin.x + xUnit * i , origin.y + firstData.Value * yUnit);
                    Vector2 secondPos = new Vector2(origin.x + xUnit * ( i + 1 ) , origin.y + secondData.Value * yUnit);
                    vh.AddUIVertexQuad(GetQuad(firstPos , secondPos , data.LineColor , LineWidth));

                    #region 折线图颜色填充 

                    if ( IsFill )
                    {
                        Vector2 xFirst = new Vector2(origin.x + xUnit * i , origin.y);
                        Vector2 xSecond = new Vector2(origin.x + xUnit * ( i + 1 ) , origin.y);
                        vh.AddUIVertexQuad(GetQuad(xFirst , xSecond , secondPos , firstPos , data.FillColor));
                    }

                    #endregion
                }
            }

            #endregion
        }

        //通过两个端点绘制矩形
        private UIVertex[] GetQuad( Vector2 startPos , Vector2 endPos , Color color0 , float lineWidth = 2.0f )
        {
            float dis = Vector2.Distance(startPos , endPos);
            float y = lineWidth * 0.5f * ( endPos.x - startPos.x ) / dis;
            float x = lineWidth * 0.5f * ( endPos.y - startPos.y ) / dis;
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

        //通过四个顶点绘制矩形
        private UIVertex[] GetQuad( Vector2 first , Vector2 second , Vector2 third , Vector2 four , Color color0 )
        {
            UIVertex[] vertexs = new UIVertex[4];
            vertexs[0] = GetUIVertex(first , color0);
            vertexs[1] = GetUIVertex(second , color0);
            vertexs[2] = GetUIVertex(third , color0);
            vertexs[3] = GetUIVertex(four , color0);
            return vertexs;
        }

        //设置顶点
        private UIVertex GetUIVertex( Vector2 point , Color color0 )
        {
            UIVertex vertex = new UIVertex
            {
                position = point ,
                color = color0 ,
                uv0 = new Vector2(0 , 0)
            };
            return vertex;
        }

        //private void DrawImaginaryLineMesh ( VertexHelper vh )
        //{

        //}
    }
}