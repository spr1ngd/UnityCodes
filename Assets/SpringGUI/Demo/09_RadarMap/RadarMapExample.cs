
/*=========================================
* Author: spring
* DateTime:2017/7/25 14:59:38
* Description:SpringGUI.RadarMap example.
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
    public RadarMap RadarMap1 = null;
    public RadarMap RadarMap2 = null;
    public RadarMap RadarMap3 = null;
    public RadarMap RadarMap4 = null;
    public RadarMap RadarMap5 = null;
    public RadarMap RadarMap6 = null;

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

        RadarMap1.Inject(radarone);
        RadarMap1.Inject(radartwo);

        RadarMap2.Inject(radarone);
        RadarMap2.Inject(radartwo);

        RadarMap3.Inject(radarone);
        RadarMap3.Inject(radartwo);

        RadarMap4.Inject(radarone);
        RadarMap4.Inject(radartwo);

        RadarMap5.Inject(radarone);
        RadarMap5.Inject(radartwo);

        IList<RMExampleData> radarthree = new List<RMExampleData>()
            {
                new RMExampleData(0.0f),
                new RMExampleData(0.1f),
                new RMExampleData(0.2f),
                new RMExampleData(0.3f),
                new RMExampleData(0.4f),
                new RMExampleData(0.5f),
                new RMExampleData(0.6f),
                new RMExampleData(0.7f),
                new RMExampleData(0.8f),
                new RMExampleData(0.9f),
                new RMExampleData(1f),
            };
        RadarMap6.Inject(radarthree);
    }
}