
/*=========================================
* Author: SpringDong
* Description:draw nonius by code
==========================================*/

using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    internal class ColorNonius : MaskableGraphic 
    {
        private const float m_radius = 3f;
        private const float m_loopWidth = 1.5f;
        private const int m_smoothDegree = 16;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            var perRadian = Mathf.PI * 2 / m_smoothDegree;
            var outRadius = m_radius + m_loopWidth;
            for (int i = 0; i < m_smoothDegree ; i++)
            {
                var startRadian = perRadian * i;
                var endRadian = perRadian * (i + 1);
                var outStartPos = new Vector2(Mathf.Cos(startRadian) , Mathf.Sin(startRadian)) * outRadius;
                var inStartPos = new Vector2(Mathf.Cos(startRadian) , Mathf.Sin(startRadian)) * m_radius;
                var outEndPos = new Vector2(Mathf.Cos(endRadian) , Mathf.Sin(endRadian)) * outRadius;
                var inEndPos = new Vector2(Mathf.Cos(endRadian) , Mathf.Sin(endRadian)) * m_radius;
                vh.AddUIVertexQuad(new UIVertex[]
                {
                    GetUIVertex(outStartPos,Color.black),
                    GetUIVertex(inStartPos,Color.black),
                    GetUIVertex(inEndPos,Color.black),
                    GetUIVertex(outEndPos,Color.black)
                });
            }
        }
        private UIVertex GetUIVertex( Vector2 point , Color color0 )
        {
            UIVertex vertex = new UIVertex
            {
                position = point ,
                color = color0 ,
            };
            return vertex;
        }
    }
}