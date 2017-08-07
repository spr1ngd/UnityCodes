
/*=========================================
* Author: springDong
* Description: 
* functional formula graph core algorithm.
* This class override OnPoplulateMesh method to get the functional graph component.
==========================================*/

using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SpringGUI
{
    public class FunctionalGraph : MaskableGraphic
    {
        [Header("FuntionalGraph Base Setting")]
        public FunctionalGraphBase GraphBase = new FunctionalGraphBase();
        
        private IList<FunctionFormula> Formulas = new List<FunctionFormula>();

        /// <summary>
        /// inject formula by base datas
        /// </summary>
        /// <param name="formula"></param>
        /// <param name="lineColor"></param>
        /// <param name="lineWidth"></param>
        public void Inject( Func<float , float> formula , Color lineColor , float lineWidth = 2.0f )
        {
            var functionalFormula = new FunctionFormula(formula , lineColor , lineWidth);
            Inject(functionalFormula);
        }

        /// <summary>
        /// inject one formula 
        /// </summary>
        /// <param name="formula"></param>
        public void Inject( FunctionFormula formula )
        {
            Formulas.Add(formula);
        }

        /// <summary>
        /// inject multi formulas the same time.
        /// </summary>
        /// <param name="formulas"></param>
        public void Inject( IList<FunctionFormula> formulas )
        {
            foreach (FunctionFormula formula in formulas)
                Inject(formula);
        }
        
        #region draw 

        private Vector2 m_xPoint;
        private Vector2 m_yPoint;
        // draw the xy axis unit text
        private void OnGUI( )
        {
            if ( GraphBase.ShowXYAxisUnit )
            {
                var result = transform.localPosition;
                Vector3 realPosition = getScreenPosition(transform , ref result);
                var guiStyleX = new GUIStyle();
                guiStyleX.normal.textColor = GraphBase.UnitFontColor;
                guiStyleX.fontSize = GraphBase.UnitFontSize;
                guiStyleX.fontStyle = FontStyle.Bold;
                guiStyleX.alignment = TextAnchor.MiddleLeft;
                GUI.Label(new Rect(local2Screen(realPosition , m_xPoint) + new Vector2(20 , 0) , new Vector2(0 , 0)) , GraphBase.XAxisUnit , guiStyleX);

                var guiStyleY = new GUIStyle();
                guiStyleY.normal.textColor = GraphBase.UnitFontColor;
                guiStyleY.fontSize = GraphBase.UnitFontSize;
                guiStyleY.fontStyle = FontStyle.Bold;
                guiStyleY.alignment = TextAnchor.MiddleCenter;
                GUI.Label(new Rect(local2Screen(realPosition , m_yPoint) - new Vector2(0 , 20) , new Vector2(0 , 0)) , GraphBase.YAxisUnit , guiStyleY);
            }
        }

        protected override void OnPopulateMesh( VertexHelper vh )
        {
            vh.Clear();
            var size = GetPixelAdjustedRect().size;

            #region draw graph base

            // draw x axis 
            var lenght = size.x;
            var leftPoint = new Vector2(-lenght / 2.0f , 0);
            var rightPoint = new Vector2(lenght / 2.0f , 0);
            vh.AddUIVertexQuad(GetQuad(leftPoint , rightPoint , GraphBase.XYAxisColor , GraphBase.XYAxisWidth));
            // draw x axis arrow
            var arrowUnit = GraphBase.XYAxisWidth * 3;
            var firstPointX = rightPoint + new Vector2(0 , arrowUnit);
            var secondPointX = rightPoint;
            var thirdPointX = rightPoint + new Vector2(0 , -arrowUnit);
            var fourPointX = rightPoint + new Vector2(Mathf.Sqrt(3) * arrowUnit , 0);
            vh.AddUIVertexQuad(GetQuad(firstPointX , secondPointX , thirdPointX , fourPointX , GraphBase.XYAxisColor));
            // draw y axis
            var height = size.y;
            var downPoint = new Vector2(0 , -height / 2.0f);
            var upPoint = new Vector2(0 , height / 2.0f);
            vh.AddUIVertexQuad(GetQuad(downPoint , upPoint , GraphBase.XYAxisColor , GraphBase.XYAxisWidth));
            // draw y axis arrow
            var firstPointY = upPoint + new Vector2(arrowUnit , 0);
            var secondPointY = upPoint;
            var thirdPointY = upPoint + new Vector2(-arrowUnit , 0);
            var fourPointY = upPoint + new Vector2(0 , Mathf.Sqrt(3) * arrowUnit);
            vh.AddUIVertexQuad(GetQuad(firstPointY , secondPointY , thirdPointY , fourPointY , GraphBase.XYAxisColor));

            if ( GraphBase.ShowXYAxisUnit )
            {
                m_xPoint = rightPoint;
                m_yPoint = upPoint;
            }

            #region draw scale 

            if ( GraphBase.ShowScale )
            {
                for ( var i = 1 ; i * GraphBase.ScaleValue < size.x / 2.0f ; i++ )
                {
                    var firstPoint = Vector2.zero + new Vector2(GraphBase.ScaleValue * i , 0);
                    var secongPoint = firstPoint + new Vector2(0 , GraphBase.ScaleLenght);
                    vh.AddUIVertexQuad(GetQuad(firstPoint , secongPoint , GraphBase.XYAxisColor));
                }
                for ( var i = 1 ; i * -GraphBase.ScaleValue > -size.x / 2.0f ; i++ )
                {
                    var firstPoint = Vector2.zero + new Vector2(-GraphBase.ScaleValue * i , 0);
                    var secongPoint = firstPoint + new Vector2(0 , GraphBase.ScaleLenght);
                    vh.AddUIVertexQuad(GetQuad(firstPoint , secongPoint , GraphBase.XYAxisColor));
                }
                for ( var y = 1 ; y * GraphBase.ScaleValue < size.y / 2.0f ; y++ )
                {
                    var firstPoint = Vector2.zero + new Vector2(0 , y * GraphBase.ScaleValue);
                    var secongPoint = firstPoint + new Vector2(GraphBase.ScaleLenght , 0);
                    vh.AddUIVertexQuad(GetQuad(firstPoint , secongPoint , GraphBase.XYAxisColor));
                }
                for ( var y = 1 ; y * -GraphBase.ScaleValue > -size.y / 2.0f ; y++ )
                {
                    var firstPoint = Vector2.zero + new Vector2(0 , y * -GraphBase.ScaleValue);
                    var secongPoint = firstPoint + new Vector2(GraphBase.ScaleLenght , 0);
                    vh.AddUIVertexQuad(GetQuad(firstPoint , secongPoint , GraphBase.XYAxisColor));
                }
            }

            #endregion

            #region draw mesh

            switch ( GraphBase.MeshType )
            {
                case FunctionalGraphBase.E_MeshType.None:
                    break;
                case FunctionalGraphBase.E_MeshType.FullLine:
                    for ( var i = 1 ; i * GraphBase.ScaleValue < size.x / 2.0f ; i++ )
                    {
                        var firstPoint = Vector2.zero + new Vector2(GraphBase.ScaleValue * i , -size.y / 2.0f);
                        var secongPoint = firstPoint + new Vector2(0 , size.y);
                        vh.AddUIVertexQuad(GetQuad(firstPoint , secongPoint , GraphBase.MeshColor , GraphBase.MeshLineWidth));
                    }
                    for ( var i = 1 ; i * -GraphBase.ScaleValue > -size.x / 2.0f ; i++ )
                    {
                        var firstPoint = Vector2.zero + new Vector2(-GraphBase.ScaleValue * i , -size.y / 2.0f);
                        var secongPoint = firstPoint + new Vector2(0 , size.y);
                        vh.AddUIVertexQuad(GetQuad(firstPoint , secongPoint , GraphBase.MeshColor , GraphBase.MeshLineWidth));
                    }
                    for ( var y = 1 ; y * GraphBase.ScaleValue < size.y / 2.0f ; y++ )
                    {
                        var firstPoint = Vector2.zero + new Vector2(-size.x / 2.0f , y * GraphBase.ScaleValue);
                        var secongPoint = firstPoint + new Vector2(size.x , 0);
                        vh.AddUIVertexQuad(GetQuad(firstPoint , secongPoint , GraphBase.MeshColor , GraphBase.MeshLineWidth));
                    }
                    for ( var y = 1 ; y * -GraphBase.ScaleValue > -size.y / 2.0f ; y++ )
                    {
                        var firstPoint = Vector2.zero + new Vector2(-size.x / 2.0f , -y * GraphBase.ScaleValue);
                        var secongPoint = firstPoint + new Vector2(size.x , 0);
                        vh.AddUIVertexQuad(GetQuad(firstPoint , secongPoint , GraphBase.MeshColor , GraphBase.MeshLineWidth));
                    }
                    break;
                case FunctionalGraphBase.E_MeshType.ImaglinaryLine:
                    for ( var i = 1 ; i * GraphBase.ScaleValue < size.x / 2.0f ; i++ )
                    {
                        var firstPoint = Vector2.zero + new Vector2(GraphBase.ScaleValue * i , -size.y / 2.0f);
                        var secondPoint = firstPoint + new Vector2(0 , size.y);
                        GetImaglinaryLine(ref vh , firstPoint , secondPoint , GraphBase.MeshColor , GraphBase.ImaglinaryLineWidth , GraphBase.SpaceingWidth);
                    }
                    for ( var i = 1 ; i * -GraphBase.ScaleValue > -size.x / 2.0f ; i++ )
                    {
                        var firstPoint = Vector2.zero + new Vector2(-GraphBase.ScaleValue * i , -size.y / 2.0f);
                        var secondPoint = firstPoint + new Vector2(0 , size.y);
                        GetImaglinaryLine(ref vh , firstPoint , secondPoint , GraphBase.MeshColor , GraphBase.ImaglinaryLineWidth , GraphBase.SpaceingWidth);
                    }
                    for ( var y = 1 ; y * GraphBase.ScaleValue < size.y / 2.0f ; y++ )
                    {
                        var firstPoint = Vector2.zero + new Vector2(-size.x / 2.0f , y * GraphBase.ScaleValue);
                        var secondPoint = firstPoint + new Vector2(size.x , 0);
                        GetImaglinaryLine(ref vh , firstPoint , secondPoint , GraphBase.MeshColor , GraphBase.ImaglinaryLineWidth , GraphBase.SpaceingWidth);
                    }
                    for ( var y = 1 ; y * -GraphBase.ScaleValue > -size.y / 2.0f ; y++ )
                    {
                        var firstPoint = Vector2.zero + new Vector2(-size.x / 2.0f , -y * GraphBase.ScaleValue);
                        var secondPoint = firstPoint + new Vector2(size.x , 0);
                        GetImaglinaryLine(ref vh , firstPoint , secondPoint , GraphBase.MeshColor , GraphBase.ImaglinaryLineWidth , GraphBase.SpaceingWidth);
                    }
                    break;
            }

            #endregion

            #endregion

            #region draw formula

            if ( null == Formulas || Formulas.Count <= 0 )
                return;
            var unitPixel = 100 / GraphBase.ScaleValue;
            foreach ( var functionFormula in Formulas )
            {
                var startPos = functionFormula.GetResult(-size.x / 2.0f , GraphBase.ScaleValue);
                for ( var x = -size.x / 2.0f + 1 ; x < size.x / 2.0f ; x += unitPixel )
                {
                    var endPos = functionFormula.GetResult(x , GraphBase.ScaleValue);
                    vh.AddUIVertexQuad(GetQuad(startPos , endPos , functionFormula.FormulaColor , functionFormula.FormulaWidth));
                    startPos = endPos;
                }
            }

            #endregion
        }

        #endregion
        
        #region help methods 

        private UIVertex[] GetQuad( Vector2 startPos , Vector2 endPos , Color color0 , float lineWidth = 2.0f )
        {
            var dis = Vector2.Distance(startPos , endPos);
            var y = lineWidth * 0.5f * ( endPos.x - startPos.x ) / dis;
            var x = lineWidth * 0.5f * ( endPos.y - startPos.y ) / dis;
            if ( y <= 0 ) y = -y;
            else x = -x;
            var vertex = new UIVertex[4];
            vertex[0].position = new Vector3(startPos.x + x , startPos.y + y);
            vertex[1].position = new Vector3(endPos.x + x , endPos.y + y);
            vertex[2].position = new Vector3(endPos.x - x , endPos.y - y);
            vertex[3].position = new Vector3(startPos.x - x , startPos.y - y);
            for ( var i = 0 ; i < vertex.Length ; i++ ) vertex[i].color = color0;
            return vertex;
        }
        
        private UIVertex[] GetQuad( Vector2 first , Vector2 second , Vector2 third , Vector2 four , Color color0 )
        {
            var vertexs = new UIVertex[4];
            vertexs[0] = GetUIVertex(first , color0);
            vertexs[1] = GetUIVertex(second , color0);
            vertexs[2] = GetUIVertex(third , color0);
            vertexs[3] = GetUIVertex(four , color0);
            return vertexs;
        }

        private UIVertex GetUIVertex( Vector2 point , Color color0 )
        {
            var vertex = new UIVertex
            {
                position = point ,
                color = color0 ,
                uv0 = new Vector2(0 , 0)
            };
            return vertex;
        }
        
        private void GetImaglinaryLine(ref VertexHelper vh, Vector2 first , Vector2 second , Color color0 ,float imaginaryLenght, float spaceingWidth , float lineWidth = 2.0f )
        {
            if ( first.y.Equals(second.y) ) //  X
            {
                var indexSecond = first + new Vector2(imaginaryLenght , 0);
                while (indexSecond.x < second.x)
                {
                    vh.AddUIVertexQuad(GetQuad(first , indexSecond , color0));
                    first = indexSecond + new Vector2(spaceingWidth , 0);
                    indexSecond = first + new Vector2(imaginaryLenght , 0);
                    if ( indexSecond.x > second.x )
                    {
                        indexSecond = new Vector2(second.x , indexSecond.y);
                        vh.AddUIVertexQuad(GetQuad(first , indexSecond , color0));
                    }
                }
            }
            if ( first.x.Equals(second.x) ) //  Y
            {
                var indexSecond = first + new Vector2(0 , imaginaryLenght);
                while (indexSecond.y < second.y)
                {
                    vh.AddUIVertexQuad(GetQuad(first , indexSecond , color0));
                    first = indexSecond + new Vector2(0 , spaceingWidth);
                    indexSecond = first + new Vector2(0 , imaginaryLenght);
                    if ( indexSecond.y > second.y )
                    {
                        indexSecond = new Vector2(indexSecond.x , second.y);
                        vh.AddUIVertexQuad(GetQuad(first , indexSecond , color0));
                    }
                }
            }
        }

        private Vector2 local2Screen( Vector2 parentPos , Vector2 localPosition )
        {
            var pos = localPosition + parentPos;
            float xValue, yValue = 0;
            if ( pos.x > 0 )
                xValue = pos.x + Screen.width / 2.0f;
            else
                xValue = Screen.width / 2.0f - Mathf.Abs(pos.x);
            if ( pos.y > 0 )
                yValue = Screen.height / 2.0f - pos.y;
            else
                yValue = Screen.height / 2.0f + Mathf.Abs(pos.y);
            return new Vector2(xValue , yValue);
        }

        private Vector2 getScreenPosition( Transform trans , ref Vector3 result )
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