
/*=========================================
* Author: Administrator
* DateTime:2017/7/25 14:11:38
* Description:$safeprojectname$
==========================================*/

using System;
using System.Reflection;
using System.Collections.Generic;

namespace SpringGUI
{
    public class RadarDataProxy
    {
        public static RadarData Convert2RD<T>(IList<T> datas)
        {
            RadarData data = new RadarData();
            Type type = typeof(T);
            PropertyInfo[] infos = type.GetProperties();
            foreach(var v in datas)
            {
                foreach (var info in infos)
                {
                    if (info.Name.Equals("value"))
                        data.AddData((float)info.GetValue(v,null));
                } 
            }
            return data;
        }

        public static IList<RadarData> Convert2RD<T>( IList<T>[] datas )
        {
            IList<RadarData> result = new List<RadarData>();
            foreach (var data in datas)
                result.Add(Convert2RD(data));
            return result;
        }
    }
}
