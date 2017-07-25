
/*=========================================
* Author: Administrator
* DateTime:2017/7/24 17:52:30
* Description:$safeprojectname$
==========================================*/

using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public interface IRadarFactory
    {
        /// <summary>
        /// 绘制数据线条
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        VertexHelper DrawRadar( VertexHelper vh , Rect rect ,RadarBaseData radardata );  
        /// <summary>
        /// Draws the line.
        /// </summary>
        /// <returns>The line.</returns>
        /// <param name="vh">Vh.</param>
        VertexHelper DrawLine( VertexHelper vh );
        /// <summary>
        /// 绘制雷达基础图
        /// </summary>
        /// <param name="vh"></param>
        /// <returns></returns>
        VertexHelper DrawBase( VertexHelper vh );
        /// <summary>
        /// 绘制xy坐标
        /// </summary>
        /// <param name="vh"></param>
        /// <returns></returns>
        VertexHelper DrawAxis( VertexHelper vh ); 
    }
}