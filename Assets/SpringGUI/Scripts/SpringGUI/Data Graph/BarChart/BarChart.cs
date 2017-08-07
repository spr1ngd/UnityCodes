
/*=========================================
* Author: Administrator
* DateTime:2017/8/6 17:43:41
* Description:$safeprojectname$
==========================================*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public class BarChart : MaskableGraphic, IInject
    {
        [SerializeField]
        public sgSettingBase BaseSetting ;

        [SerializeField]
        public BarChartSetting BarChartSetting;

        private BarChartData barChartData = new BarChartData();
        private BarChartDataProxy dataProxy = new BarChartDataProxy();

        private IBarChartFactory m_barChartFactory = null;
        private IBarChartFactory barChartFactory
        {
            get 
            {
                if (null == m_barChartFactory)
                    m_barChartFactory = new BarChartFactory();
                return m_barChartFactory;
            }
        }

        protected override void OnPopulateMesh( VertexHelper vh )
        {
            vh.Clear();
            Rect rect = GetPixelAdjustedRect();
            barChartFactory.DrawBarChart(vh , rect , BaseSetting ,BarChartSetting , barChartData);
        }

        public void UnitEnable( bool enable = true )
        {
            if (!enable)
                return;
            GameObject horizontalText = transform.FindChild("HorizontalUnitTemplate").gameObject;
            GameObject verticalText = transform.FindChild("VerticalUnitTemplate").gameObject;
            horizontalText.SetActive(false);
            verticalText.SetActive(false);
            (barChartFactory as BarChartFactory).UnitEnable(verticalText,horizontalText,GetPixelAdjustedRect(),this.transform,BaseSetting ,BarChartSetting , barChartData);
        }

        // inject data for simple bar graph
        public void Inject( IList<float> data )  
        {
            var bars = dataProxy.Convert2BD(data);
            barChartData.AddBars(bars);
        }

        // inject data for multi-bar bar graph
        public void Inject( IList<float>[] datas )
        {
            var bars = dataProxy.Convert2BD(datas);
            barChartData.AddBars(bars);
        }

        // inject data for simple bar graph
        public void Inject<T>( IList<T> data )
        {
            IList<Bars> bars = dataProxy.Convert2BD(data);
            barChartData.AddBars(bars);
        }

        // inject data for multi-bar bar graph
        public void Inject<T>( IList<T>[] datas )
        {
            IList<Bars> bars = dataProxy.Convert2BD(datas);
            barChartData.AddBars(bars);
        }
    }
}