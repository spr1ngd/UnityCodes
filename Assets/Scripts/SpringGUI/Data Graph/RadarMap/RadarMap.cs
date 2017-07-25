
/*=========================================
* Author: Administrator
* DateTime:2017/7/24 17:46:17
* Description:$safeprojectname$
==========================================*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SpringGUI
{
    public enum RadarMapType
    {
        RadarBase,
        Radar1,
        Radar2,
        Radar3,
        Radar4,
        Radar5 
    }

    public class RadarMap : MaskableGraphic
    {
        [SerializeField]
        public RadarMapType radarMapType = RadarMapType.Radar5;

        [SerializeField]
        public RadarBaseData RadarBaseData = null;

        // 雷达图绘制工厂
        private IRadarFactory m_radarFactory
        {
            get
            {
                switch ( radarMapType )
                {
                    case RadarMapType.RadarBase:
                        return new BaseRadarFactory();
                    case RadarMapType.Radar1:
                        return new RadarFactory1();
                    case RadarMapType.Radar2:
                        return new RadarFactory2();
                    case RadarMapType.Radar3:
                        return new RadarFactory3();
                    case RadarMapType.Radar4:
                        return new RadarFactory4();
                    case RadarMapType.Radar5:
                        return new RadarFactory5();
                    default:
                        return new BaseRadarFactory();
                }
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            Rect rect = GetPixelAdjustedRect();
            RadarBaseData.parent = this.transform;
            m_radarFactory.DrawRadar(vh , rect, RadarBaseData);
        }

        public void Inject<T>( IList<T> datas )
        { 
            var radardata = RadarDataProxy.Convert2RD(datas);
            RadarBaseData.Adddata(radardata);   
            OnEnable();
        }

        public void Inject<T>( IList<T>[] datas )
        {
            var radardatas = RadarDataProxy.Convert2RD(datas);
            RadarBaseData.Adddata(radardatas);
            OnEnable();
        }
    }
}