
/*=========================================
* Author: Administrator
* DateTime:2017/7/17 14:27:18
* Description:$safeprojectname$
==========================================*/

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SpringGUI
{
    public class LineChartDataMediator
    {
        /// <summary>
        /// 反射取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="vertexs"></param>
        /// <returns></returns>
        public IList<Vector2> Inject<T>( IList<T> vertexs )
        {
            IList<Vector2> result = new List<Vector2>();
            Type type = typeof(T);
            PropertyInfo[] PropertyInfo = type.GetProperties();
            foreach ( T vertex in vertexs )
            {
                float x = 0.0f;
                float y = 0.0f;
                foreach ( PropertyInfo info in PropertyInfo )
                {
                    if ( info.Name.Equals("xValue") )
                        x = (float)info.GetValue(vertex , null);
                    if ( info.Name.Equals("yValue") )
                        y = (float)info.GetValue(vertex , null);
                }
                result.Add(new Vector2(x , y));
            }
            return result;
        }

        public IList<Vector2>[] Inject<T>( IList<T>[] vertexs )
        {
            IList<Vector2>[] result = new IList<Vector2>[vertexs.Length];
            for (int i = 0; i < vertexs.Length; i++)
                result.SetValue(Inject(vertexs[i]) , i);
            return result;
        }

        /// <summary>
        /// 备用方法
        /// </summary>
        /// <param name="vertexs"></param>
        /// <returns></returns>
        public IList<Vector2> Inject(IList<Vector2> vertexs)
        {
            //如果你不太会用上面的反射获取数值的方法，你也可以直接在这里操作；
            //替换掉参数的类型，提出成你自己的类型
            //然后解析为0到1的值
            return vertexs;
        }
    }
}