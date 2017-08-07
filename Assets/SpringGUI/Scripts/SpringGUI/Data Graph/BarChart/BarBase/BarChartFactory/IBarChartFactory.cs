
using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public interface IBarChartFactory
    {
        VertexHelper DrawBarChart( VertexHelper vh , Rect rect , sgSettingBase baseSetting ,BarChartSetting barChartSetting, BarChartData data = null );
    }
}
