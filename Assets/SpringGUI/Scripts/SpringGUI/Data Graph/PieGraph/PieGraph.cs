
/*=========================================
* Author: springDong
* Description: SpringGUI.PieGraph, you can set radian and hollow size.
* Feature:
*   1.no assets
*   2.multi properties custom
==========================================*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SpringGUI  
{
    //Pie Base Data ,use this data to build Pie Graph
    [System.Serializable]
    public class PieData
    {
        public float Percent;
        public Color Color;
        public PieData(){}
        public PieData(float percent,Color color0)
        {
            Percent = percent / 100.0f;
            Color = color0;
        }

        public PieData(float percent)
        {
            Percent = percent / 100.0f;
            // auto set color by percent
            Color = Color.white * Percent;
        }
    };

    //Use this data struct to draw a Pie Graph
    [System.Serializable]
    public class Pies
    {
        public IList<PieData> PieDatas = new List<PieData>();
        public Pies(){}
        public Pies( IList<PieData> pieDatas )
        {
            PieDatas = pieDatas;
        }

        public void AddPieData( Pies piedatas )
        {
            foreach (PieData pieData in piedatas.PieDatas )
                PieDatas.Add(pieData);
        }
    };

    //GUI Text information for pie Graph
    [System.Serializable]
    public class PieText
    {
        public string Content = null;
        public Vector2 Position;
        public bool IsLeft = true;
        public PieText(){}

        public PieText( string content,Vector2 position ,bool isLeft )
        {
            Content = content;
            Position = position;
            IsLeft = isLeft;
        }
    }

    // core class
    public class PieGraph : MaskableGraphic
    {
        [Header("Pie Base Setting")]
        [Range(5 , 150)]public float PieRadius   = 60.0f;
        [Range(0 , 120)]public float HollowWidth = 0.0f;
        [Range(0, 15)] public float BoomDegree = 1.5f;
        [Range(20, 200)] public float Smooth = 100;

        [Header("Percent Text Setting")]
        public bool IsShowPercnet = true;
        [Range(0.5f, 4)] public float BrokenLineWidth = 2;

        private Pies PieData = new Pies();
        private List<PieText> _pieText = null;
        private Vector3 _realPosition;

        public void Inject( IList<float> percents,IList<Color> colors )
        {
            IList<PieData> piedatas= new List<PieData>();
            for ( int i = 0 ; i < percents.Count ; i++ )
                piedatas.Add(new PieData(percents[i] , colors[i]));
            Inject(piedatas);
        }
        public void Inject( IList<float> percents )
        {
            IList<PieData> piedatas = new List<PieData>();
            foreach (float percent in percents)
                piedatas.Add(new PieData(percent));
            Inject(piedatas); 
        }
        public void Inject( IList<PieData> pieData )
        {
            Inject(new Pies(pieData));
        }
        public void Inject( Pies pies )
        {
            PieData.AddPieData(pies);
        }

        #region draw pie

        private void OnGUI()
        {
            if ( null == _pieText ) return;
            if (!IsShowPercnet) return;
            Vector3 result = transform.localPosition;
            _realPosition = getScreenPosition(transform , ref result);
            foreach ( PieText text in _pieText )
            {
                Vector2 position = local2Screen(_realPosition , text.Position);
                GUIStyle guiStyle = new GUIStyle();
                guiStyle.normal.textColor = Color.black;
                guiStyle.fontStyle = FontStyle.Bold;
                guiStyle.alignment = TextAnchor.MiddleLeft;
                if ( !text.IsLeft )
                    guiStyle.alignment = TextAnchor.MiddleRight;
                if(text.IsLeft)
                    position += new Vector2(3,-10);
                else
                    position += new Vector2(-23,-10);
                GUI.Label(new Rect(position , new Vector2(20 , 20)) , text.Content , guiStyle);
            }
        }

        //draw pie core method
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if ( null == PieData ) return;
            vh.Clear();
            vh.AddUIVertexTriangleStream(DrawPie());
        }
        private List<UIVertex> DrawPie()
        {
            if (PieData == null || PieData.PieDatas.Count <= 0)
                return new List<UIVertex>();
            if (IsShowPercnet)
                _pieText = new List<PieText>();
            List<UIVertex> vertexs = new List<UIVertex>();
            float perRadian = Mathf.PI * 2 / Smooth;
            float totalRadian = 0;
            float boomRadian = BoomDegree / 180 * Mathf.PI;
            float pieRadianBase = Mathf.PI * 2 - boomRadian * PieData.PieDatas.Count;
            for ( int i = 0 ; i < PieData.PieDatas.Count ; i++ )
            {
                PieData data = PieData.PieDatas[i];
                float endRadian = boomRadian + data.Percent * pieRadianBase + totalRadian;
                for ( float r = boomRadian + totalRadian ; r < endRadian ; r += perRadian )
                {
                    Vector2 first = new Vector2(Mathf.Cos(r) , Mathf.Sin(r)) * HollowWidth;
                    Vector2 second = new Vector2(Mathf.Cos(r + perRadian) , Mathf.Sin(r + perRadian)) * HollowWidth;
                    Vector2 third = new Vector2(Mathf.Cos(r) , Mathf.Sin(r)) * PieRadius;
                    Vector2 four = new Vector2(Mathf.Cos(r + perRadian) , Mathf.Sin(r + perRadian)) * PieRadius;
                    vertexs.Add(GetUIVertex(first , data.Color));
                    vertexs.Add(GetUIVertex(third , data.Color));
                    vertexs.Add(GetUIVertex(second , data.Color));
                    vertexs.Add(GetUIVertex(second , data.Color));
                    vertexs.Add(GetUIVertex(third , data.Color));
                    vertexs.Add(GetUIVertex(four , data.Color));
                }

                if (IsShowPercnet)
                {
                    float middleRadian = boomRadian + data.Percent * pieRadianBase / 2 + totalRadian;
                    float brokenLineLength = PieRadius * 0.2f;
                    Vector2 middlePoint = new Vector2(Mathf.Cos(middleRadian) , Mathf.Sin(middleRadian)) * PieRadius;
                    Vector2 secondPoint = middlePoint + new Vector2(Mathf.Cos(middleRadian) , Mathf.Sin(middleRadian)) * brokenLineLength;
                    Vector2 first = middlePoint + new Vector2(Mathf.Sin(middleRadian) , -Mathf.Cos(middleRadian)) * BrokenLineWidth / 2;
                    Vector2 second = middlePoint + new Vector2(-Mathf.Sin(middleRadian) , Mathf.Cos(middleRadian)) * BrokenLineWidth / 2;
                    Vector2 third = secondPoint + new Vector2(Mathf.Sin(middleRadian) , -Mathf.Cos(middleRadian)) * BrokenLineWidth / 2;
                    Vector2 four = secondPoint + new Vector2(-Mathf.Sin(middleRadian) , Mathf.Cos(middleRadian)) * BrokenLineWidth / 2;
                    Vector2 five;
                    Vector2 six;

                    if ( middleRadian > Mathf.PI / 2 && middleRadian < Mathf.PI * 3 / 2 )
                    {
                        five = third + new Vector2(-brokenLineLength , 0);
                        six = four + new Vector2(-brokenLineLength , 0);
                        // right text
                        _pieText.Add(new PieText(data.Percent * 100 + "%" , six , false));
                    }
                    else
                    {
                        five = third + new Vector2(brokenLineLength , 0);
                        six = four + new Vector2(brokenLineLength , 0);
                        // left text
                        _pieText.Add(new PieText(data.Percent * 100 + "%" , six ,true));
                    }

                    vertexs.Add(GetUIVertex(first , data.Color));
                    vertexs.Add(GetUIVertex(second , data.Color));
                    vertexs.Add(GetUIVertex(third , data.Color));

                    vertexs.Add(GetUIVertex(third , data.Color));
                    vertexs.Add(GetUIVertex(second , data.Color));
                    vertexs.Add(GetUIVertex(four , data.Color));

                    vertexs.Add(GetUIVertex(third , data.Color));
                    vertexs.Add(GetUIVertex(four , data.Color));
                    vertexs.Add(GetUIVertex(five , data.Color));

                    vertexs.Add(GetUIVertex(five , data.Color));
                    vertexs.Add(GetUIVertex(four , data.Color));
                    vertexs.Add(GetUIVertex(six , data.Color));
                }
                totalRadian = endRadian;
            }
            return vertexs;
        }

        #endregion

        #region draw helper methods

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
        
        private Vector2 local2Screen( Vector2 parentPos,Vector2 localPosition )
        {
            Vector2 pos = localPosition + parentPos;
            float xValue, yValue = 0;
            if ( pos.x > 0 )
                xValue = pos.x + Screen.width / 2.0f;
            else
                xValue = Screen.width / 2.0f - Mathf.Abs(pos.x);
            if ( pos.y > 0 )
                yValue = Screen.height / 2.0f - pos.y;
            else
                yValue = Screen.height / 2.0f + Mathf.Abs(pos.y);
            return new Vector2(xValue,yValue);
        }
        
        private Vector2 getScreenPosition( Transform trans, ref Vector3 result )
        {
            if ( null != trans.parent && null != trans.parent.parent )
            {
                result += trans.parent.localPosition;
                getScreenPosition(trans.parent , ref result);
            }
            if ( null != trans.parent && null == trans.parent.parent )
                return result;
            return result;
        }

        #endregion
    }
}