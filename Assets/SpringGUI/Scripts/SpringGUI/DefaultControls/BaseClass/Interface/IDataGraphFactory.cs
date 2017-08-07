
/*=========================================
* Author: Administrator
* DateTime:2017/8/6 17:43:41
* Description:$safeprojectname$
==========================================*/

using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public interface IDataGraphFactory
    {
        /// <summary>
        /// 绘制xy坐标轴
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        VertexHelper DrawAxis( VertexHelper vh , Rect rect );

        /// <summary>
        /// 绘制底图网格
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="rect"></param>
        /// <param name="cellSize"></param>
        /// <returns></returns>
        VertexHelper DrawBaseMesh( VertexHelper vh , Rect rect  );

        /// <summary>
        /// 客户端绘制调用接口
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        VertexHelper DrawMesh( VertexHelper vh , Rect rect);
    }
}