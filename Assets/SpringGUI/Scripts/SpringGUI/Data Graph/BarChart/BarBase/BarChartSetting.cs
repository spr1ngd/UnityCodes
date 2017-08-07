
using System;
using UnityEngine;
using System.Collections.Generic;

namespace SpringGUI
{
    public enum BarChartType
    {
        Horizontal,
        Vertical
    }

    public enum ColorStyle
    {
        SingleNormal,
        SingleColorful,
        MultiNormal,
        MultiColorufl
    }

    [Serializable]
    public class BarChartSetting
    {
        [Header("BarChart Type Setting")]
        public BarChartType BarChartType = BarChartType.Vertical;       

        [Space(5)]
        [Header("BarChart Style Setting")]
        public float BarWidth = 24.0f;
        public float BarInterval = 10.0f;
        public float BarSpacing = 20.0f;

        [Space(5)]
        [Header("BarChart Color Setting")]
        public ColorStyle ColorStyle = ColorStyle.SingleColorful;
        public Color[] BarColors = new Color[]{};
    }
}