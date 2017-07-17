using System.Collections.Generic;
using SpringGUI;
using UnityEngine;

public class LineChartGraphExample : MonoBehaviour
{
    public class TestData
    {
        public float xValue { get; set; }
        public float yValue { get; set; }

        public TestData( float x ,float y )
        {
            xValue = x;
            yValue = y;
        }
    }

    public LineChart LineChart = null;

    public void Awake()
    {
        var data1 = new List<TestData>()
            {
                new TestData(0.0f,0.0f),
                new TestData(0.1f,0.9f),
                new TestData(0.2f,0.2f),
                new TestData(0.3f,0.8f),
                new TestData(0.4f,0.3f),
                new TestData(0.5f,0.7f),
                new TestData(0.6f,0.4f),
                new TestData(0.7f,0.6f),
                new TestData(0.8f,0.5f),
                new TestData(0.9f,0.2f),
                new TestData(1f,0.5f),
            };
        var data2 = new List<TestData>()
            {
                new TestData(0.0f,0.7f),
                new TestData(0.1f,0.1f),
                new TestData(0.2f,0.5f),
                new TestData(0.3f,0.6f),
                new TestData(0.4f,0.7f),
                new TestData(0.5f,0.2f),
                new TestData(0.6f,0.72f),
                new TestData(0.7f,0.24f),
                new TestData(0.8f,0.52f),
                new TestData(0.9f,0.1f),
                new TestData(1f,0.0f),
            };
        LineChart.Inject<TestData>(data1);
        LineChart.Inject<TestData>(data2);
        LineChart.ShowUnit();
    }

    public void OnGUI()
    {
        if (GUILayout.Button("重置刷新"))
        {
            LineChart.Refresh();
        }
        GUILayout.Label("静态数据模式");
        if (GUILayout.Button("模式一"))
        {
            LineChart.lineChartType = LineChart.LineChartType.LineChart1;
            LineChart.Refresh();
        }
        if (GUILayout.Button("模式二"))
        {
            LineChart.lineChartType = LineChart.LineChartType.LineChart2;
            LineChart.Refresh();
        }
        if (GUILayout.Button("模式三"))
        {
            LineChart.lineChartType = LineChart.LineChartType.LineChart3;
            LineChart.Refresh();
        }
        if (GUILayout.Button("删除一条折线"))
        {
            LineChart.RemoveLine(1);
        }
        GUILayout.Label("动态数据模式");
        GUILayout.Space(5);
        if (GUILayout.Button("一次性数据替换"))
        {
            LineChart.Replace(1 , new List<TestData>()
                {
                    new TestData(0.0f,0.11f),
                    new TestData(0.1f,0.1f),
                    new TestData(0.2f,0.27f),
                    new TestData(0.3f,0.16f),
                    new TestData(0.4f,0.1f),
                    new TestData(0.5f,0.14f),
                    new TestData(0.6f,0.52f),
                    new TestData(0.7f,0.2f),
                    new TestData(0.8f,0.11f),
                    new TestData(0.9f,0.8f),
                    new TestData(1f,0.18f),
                });
        }
        GUILayout.Space(5);
        if ( GUILayout.Button("数据流模式动态数据") )
        {
            LineChart.InjectVertexStream(1 , new List<TestData>()
                {
                    new TestData(0.0f,0.9f),
                    new TestData(0.0f,0.2f),
                    new TestData(0.0f,0.4f),
                    new TestData(0.0f,0.5f),
                    new TestData(0.0f,0.6f),
                    new TestData(0.0f,0.1f),
                    new TestData(0.0f,0.23f),
                    new TestData(0.0f,0.15f),
                    new TestData(0.0f,0.61f),
                    new TestData(0.0f,0.1f),
                    new TestData(0.0f,0.5f),
                    new TestData(0.0f,0.1f),
                });
        }
    }
}