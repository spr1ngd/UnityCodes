
/*=========================================
* Author: Administrator
* DateTime:2017/8/6 17:43:41
* Description:$safeprojectname$
==========================================*/

using UnityEngine;
using System.Collections.Generic;
using SpringGUI;

public class BarChartExample : MonoBehaviour
{
    public BarChart BarChart = null;

    private void Awake()
    {
        IList<float> bars = new List<float>(){ 0.6f,0.8f,0.1f,0.2f,0.5f};

        IList<float>[] barss = new IList<float>[]
            {
                new List<float>(){0.1f,0.7f,0.2f},
                new List<float>(){0.2f,0.4f,0.9f},
                new List<float>(){0.5f,0.1f,0.2f},
                new List<float>(){0.8f,0.2f,0.8f},
            };

        BarChart.Inject(barss);
        BarChart.UnitEnable(true);
    }
}