
/*=========================================
* Author: Administrator
* DateTime:2017/7/16 15:39:49
* Description:$safeprojectname$
==========================================*/

using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    [Serializable]
    public class LineChartBasis
    {
        public bool IsDrawAxis = true;
        public float AxisWidth = 2.0f;
        public Color AxisColor = Color.white;
        public bool ShowArrow = false;
        
        public bool IsDrawMeshX = true;
        public bool IsDrawMeshY = true;
        public float MeshWidth = 2.0f;
        public Color MeshColor = Color.gray;
        [Range(5, 1000)]
        public float MeshCellXSize = 25.0f;
        [Range(5 , 1000)]
        public float MeshCellYSize = 25.0f;
        [HideInInspector]
        public Vector2 MeshCellSize {
            get
            { return new Vector2(MeshCellXSize, MeshCellYSize);}
        }

        public Dictionary<int , IList<Vector2>> Lines = new Dictionary<int , IList<Vector2>>();
    }

    public class LineChart : MaskableGraphic
    {
        public enum LineChartType
        {
            LineChart1,
            LineChart2,
            LineChart3
        }
        public LineChartType lineChartType  = LineChartType.LineChart1;
        public ILineChart LineChartCreator
        {
            get
            {
                InjectLine( new List<Vector2>()
                {
                    new Vector2(0.0f,0.0f),
                    new Vector2(0.1f,0.9f),
                    new Vector2(0.3f,0.2f),
                    new Vector2(0.4f,0.8f),
                    new Vector2(0.5f,0.3f),
                    new Vector2(0.6f,0.7f),
                    new Vector2(0.7f,0.4f),
                    new Vector2(0.8f,0.6f),
                    new Vector2(1,0.5f),
                } );

                InjectLine(new List<Vector2>()
                {
                    new Vector2(0.0f,0.9f),
                    new Vector2(0.1f,0.1f),
                    new Vector2(0.3f,0.1f),
                    new Vector2(0.4f,0.1f),
                    new Vector2(0.5f,0.3f),
                    new Vector2(0.6f,0.3f),
                    new Vector2(0.7f,0.3f),
                    new Vector2(0.8f,0.4f),
                    new Vector2(1,0.2f),
                });
                switch ( lineChartType )
                {
                    case LineChartType.LineChart1:
                        return new LineChart1();
                    case LineChartType.LineChart2:
                        return new LineChart2();
                    case LineChartType.LineChart3:
                        return new LineChart3();
                    default:
                        return new LineChart1();
                }
            }
        }

        [SerializeField]
        public LineChartBasis LineChartBasis = null;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            var rect = base.GetPixelAdjustedRect();
            LineChartCreator.DrawLineChart(vh, rect,LineChartBasis);
        }

        /// <summary>
        /// 左下角为（0,0） 右上角(1,1)
        /// </summary>
        /// <param name="points"></param>
        public void InjectLine( IList<Vector2> points )
        {
            LineChartBasis.Lines.Add(LineChartBasis.Lines.Count,points); 
        }
    }
}