
/*=========================================
* Author: Administrator
* DateTime:2017/8/6 17:43:41
* Description:$safeprojectname$
==========================================*/

using System;
using System.Collections.Generic;
using System.Reflection;

namespace SpringGUI
{
    public class BarChartDataProxy
    {
        public IList<Bars> Convert2BD( IList<float> datas )
        {
            List<Bars> bars = new List<Bars>();
            foreach (float bar in datas)
                bars.Add(new Bars(new List<float>(){bar}));
            return bars;
        }

        public IList<Bars> Convert2BD ( IList<float>[] datas )
        {
            List<Bars> bars = new List<Bars>();
            foreach (IList<float> bar in datas)
                bars.Add(new Bars(bar));
            return bars;
        }

        public IList<Bars> Convert2BD<T>( IList<T> datas )
        {
            List<Bars> bars = new List<Bars>();
            return bars;
        }

        public IList<Bars> Convert2BD<T>( IList<T>[] datas )
        {
            List<Bars> bars = new List<Bars>();
            return bars;
        }
    }
}