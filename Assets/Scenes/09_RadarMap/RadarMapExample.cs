
/*=========================================
* Author: Administrator
* DateTime:2017/7/25 14:59:38
* Description:$safeprojectname$
==========================================*/

using UnityEngine;
using SpringGUI;
using System.Collections.Generic;

public class RMExampleData
{
    public float value{get;set;}

    public RMExampleData(float value)
    {
        this.value = value;
    }
}

public class RadarMapExample : MonoBehaviour 
{
    public RadarMap RadarMap = null;

    private void Awake()
    {
        IList<RMExampleData> radarone = new List<RMExampleData>()
            {
                new RMExampleData(0.36f),
                new RMExampleData(0.6f),
                new RMExampleData(0.69f),
                new RMExampleData(0.9f),
                new RMExampleData(0.2f),
                new RMExampleData(0.5f)
            };
        IList<RMExampleData> radartwo = new List<RMExampleData>()
            {
                new RMExampleData(0.1f),
                new RMExampleData(0.3f),
                new RMExampleData(0.5f),
                new RMExampleData(0.7f),
                new RMExampleData(0.9f),
                new RMExampleData(0.0f)
            };

        RadarMap.Inject(radarone);
        RadarMap.Inject(radartwo);
    }
}