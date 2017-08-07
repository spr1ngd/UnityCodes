
/*=========================================
* Author: springDong
* DateTime:2017/7/24 17:46:17
* Description: SpringGUI.RadarMap core
==========================================*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SpringGUI
{
    public enum RadarType
    {
        Base,
        Type1,
        Type2,
        Type3,
        Type4,
        Type5,
    }

    public class RadarMap : MaskableGraphic
    {
        [Header("RadarMap Type Setting")]
        [SerializeField]
        public RadarType radarType = RadarType.Base;

        [Header("RadarMap Base Data Setting")]
        [SerializeField] public RadarBaseData RadarBaseData = null;

        // radar map draw factory
        private IRadarFactory m_radarFactory
        {
            get
            {
                switch ( radarType )
                {
                    case RadarType.Base:
                        return new BaseRadarFactory();
                    case RadarType.Type1:
                        return new RadarFactory1();
                    case RadarType.Type2:
                        return new RadarFactory2();
                    case RadarType.Type3:
                        return new RadarFactory3();
                    case RadarType.Type4:
                        return new RadarFactory4();
                    case RadarType.Type5:
                        return new RadarFactory5();
                }
                return null;
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            Rect rect = GetPixelAdjustedRect();
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