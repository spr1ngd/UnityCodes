
/*=========================================
* Author: springdong
* DateTime:2017/7/17 16:11:39
* Description: line chart base data 
==========================================*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public class VertexStream
    {
        public IList<Vector2> vertexs = null;
        public Color color;

        public VertexStream( IList<Vector2> vertexs , Color color0 )
        {
            this.vertexs = vertexs;
            color = color0;
        }
    }

    [Serializable]
    public class LineChartData
    {
        [Header("LineChart Axis Setting")]
        public bool IsDrawAxis = true;
        public float AxisWidth = 2.0f;
        public Color AxisColor = Color.white;
        public bool ShowArrow = false;
        
        [Header("LineChart Mesh Setting")]
        public bool IsDrawMeshX = true;
        public bool IsDrawMeshY = true;
        public float MeshWidth = 2.0f;
        public Color MeshColor = Color.gray;
        [Range(5 , 1000)]
        public float MeshCellXSize = 25.0f;
        [Range(5 , 1000)]
        public float MeshCellYSize = 25.0f;
        public bool IsImaginaryLine = false;

        [HideInInspector]
        public Vector2 MeshCellSize { get { return new Vector2(MeshCellXSize , MeshCellYSize); } }
        
        [Header("LineChart Unit Setting")]
        public Color[] LineColors = new Color[] { };
        public bool IsShowUnit = false;
        public float XUnit = 1;
        public float YUnit = 10;
        public Text XUnitTemplate = null;
        public Text YUnitTemplate = null;
        [HideInInspector]
        public Dictionary<int , VertexStream> Lines = new Dictionary<int , VertexStream>();

        public void AddLine( IList<Vector2> vertexs )
        {
            Color color = Color.black;
            if ( LineColors.Length >= Lines.Count )
                color = LineColors[Lines.Count];
            Lines.Add(Lines.Count , new VertexStream(vertexs , color));
        }

        public IList<Vector2> GetLine( int id )
        {
            return Lines[id].vertexs;
        }

        public void ReplaceLines( int[] ids , IList<Vector2>[] vertexs )
        {
            for (int i = 0; i < ids.Length; i++)
                Lines[ids[i]] = new VertexStream(vertexs[i] , LineColors[ids[i]]);
        }

        public void RemoveLine( int[] ids )
        {
            foreach (int id in ids) 
                Lines.Remove(id);
        }

        public void ClearLines( )
        {
            Lines.Clear();
        }
    }
}