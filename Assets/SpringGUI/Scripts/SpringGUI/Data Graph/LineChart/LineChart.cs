
/*=========================================
* Author: springDong
* DateTime:2017/7/16 15:39:49
* Description: draw linechart
==========================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public class LineChart : MaskableGraphic
    {
        public enum LineChartType
        {
            LineChart1,
            LineChart2,
            LineChart3
        }


        [Header("LineChart Type Setting")]
        [SerializeField]
        public LineChartType lineChartType  = LineChartType.LineChart1;
        private ILineChart LineChartCreator
        {
            get
            {
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

        [Header("LineChart Base Data Setting")]
        [SerializeField]
        public LineChartData LineChartBasis = null;
        private LineChartDataMediator m_dataMediator = null;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if ( LineChartBasis.Lines.Count.Equals(0) )
                return;
            var rect = base.GetPixelAdjustedRect();
            LineChartCreator.DrawLineChart(vh, rect,LineChartBasis);
        }

        public void Refresh()
        {
            OnEnable();
        }

        private List<GameObject> m_units = new List<GameObject>();
        private void ClearUnit()
        {
            foreach (GameObject mUnit in m_units)
                GameObject.Destroy(mUnit);
        }

        public void ShowUnit()
        {
            LineChartBasis.XUnitTemplate.gameObject.SetActive(false);
            LineChartBasis.YUnitTemplate.gameObject.SetActive(false);
            if (!LineChartBasis.IsShowUnit)
                return;
            ClearUnit();
            Vector2 size = GetPixelAdjustedRect().size;
            Vector2 origin = new Vector2(-size.x / 2.0f , -size.y / 2.0f);
            if ( null != LineChartBasis.XUnitTemplate )
            {
                for ( float y = 0, count = 0 ; y < size.y ; y += LineChartBasis.MeshCellYSize, count++ )
                {
                    float value = count * LineChartBasis.MeshCellYSize;
                    GeneratorUnit(LineChartBasis.XUnitTemplate ,
                        origin + new Vector2(0 , count * LineChartBasis.MeshCellYSize) + new Vector2(-5 , 0)).text = value.ToString("F");
                }
            }
            if ( null != LineChartBasis.YUnitTemplate )
            {
                for ( float x = 0, count = 0 ; x < size.x ; x += LineChartBasis.MeshCellXSize, count++ )
                {
                    float value = count * LineChartBasis.MeshCellXSize;
                    GeneratorUnit(LineChartBasis.YUnitTemplate ,
                        origin + new Vector2(count * LineChartBasis.MeshCellXSize , size.y) + new Vector2(0 , 5)).text = value.ToString("F");
                }
            }
        }
        private Text GeneratorUnit( Text prefab ,Vector3 position )
        {
            Text go = GameObject.Instantiate(prefab);
            go.gameObject.SetActive(true);
            go.transform.SetParent(transform);
            go.transform.localPosition = position;
            go.transform.localScale = Vector3.one;
            m_units.Add(go.gameObject);
            return go;
        }

        #region data interfaces

        public void Inject<T>( IList<T> vertexs )
        {
            if( null == m_dataMediator)
                m_dataMediator = new LineChartDataMediator();
            LineChartBasis.AddLine(m_dataMediator.Inject(vertexs));
        }

        public void Inject( IList<Vector2> vertexs )
        {
            if ( null == m_dataMediator )
                m_dataMediator = new LineChartDataMediator();
            LineChartBasis.AddLine(m_dataMediator.Inject(vertexs));
        }

        /// <summary>
        /// 移除折线
        /// </summary>
        /// <param name="id"></param>
        public void RemoveLine( int id )
        {
            RemoveLine(new int[] { id });
        }
        /// <summary>
        /// 移除折线
        /// </summary>
        /// <param name="ids"></param>
        public void RemoveLine( int[] ids )
        {
            LineChartBasis.RemoveLine(ids);
            OnEnable();
        }
        
        /// <summary>
        /// 替换折线数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="vertexs"></param>
        public void Replace<T>( int id , IList<T> vertexs )
        {
            Replace(new int[] { id } , new IList<T>[] { vertexs });
        }
        /// <summary>
        /// 替换折线数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <param name="vertexs"></param>
        public void Replace<T>( int[] ids , IList<T>[] vertexs )
        {
            LineChartBasis.ReplaceLines(ids , m_dataMediator.Inject(vertexs));
            OnEnable();
        }

        /// <summary>
        /// 流式顶点数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="vertexs"></param>
        public void InjectVertexStream<T>( int id , IList<T> vertexs )
        {
            IList<Vector2> vertex = m_dataMediator.Inject(vertexs);
            StartCoroutine(StreamInject(id , vertex));
        }
        /// <summary>
        /// 流式顶点数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="vertexs"></param>
        public void InjectVertexStream<T>(int[] ids , IList<T>[] vertexs )
        {
            for (int i = 0; i < ids.Length; i++)
                InjectVertexStream(ids[i] , vertexs[i]);
        }
        private IEnumerator StreamInject( int id , IList<Vector2> vertexs )
        {
            for (int m = 0; m < vertexs .Count; m++)
            {
                IList<Vector2> oldVertexs = LineChartBasis.GetLine(id);
                var last = oldVertexs[oldVertexs.Count - 1];
                oldVertexs.Add(new Vector2(last.x + 0.1f , vertexs[m].y));
                var startOffset = oldVertexs[1].y - oldVertexs[0].y;
                var endOffset = oldVertexs[oldVertexs.Count - 1].y - oldVertexs[oldVertexs.Count - 2].y;
                for ( int i = 1 ; i <= 10 ; i++ )
                {
                    IList<Vector2> newVertexs = new List<Vector2>();
                    newVertexs.Add(new Vector2(0 , oldVertexs[0].y + i * 0.1f * startOffset));
                    for ( int j = 0 ; j < oldVertexs.Count - 2 ; j++ )
                        newVertexs.Add(oldVertexs[j + 1] - new Vector2(0.01f * i , 0));
                    newVertexs.Add(new Vector2(1, oldVertexs[oldVertexs.Count - 2].y + 0.1f * i * endOffset));
                    LineChartBasis.ReplaceLines(new int[] { id } , new IList<Vector2>[] { newVertexs });
                    OnEnable();
                    if ( i.Equals(10) )
                    {
                        newVertexs.RemoveAt(0);
                        LineChartBasis.ReplaceLines(new int[] { id } , new IList<Vector2>[] { newVertexs });
                    }
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }

        #endregion
    }
}