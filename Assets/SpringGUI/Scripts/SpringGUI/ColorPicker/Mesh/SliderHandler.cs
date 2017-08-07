
/*=========================================
* Author : Spring Dong 
* DateTime : 2017/7/3 10:58:27
* Description : draw slider handler
==========================================*/

using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{ 
    public class SliderHandler : MaskableGraphic
    {
        public float arrowSize = 5.0f;
        public float distance = 5.0f;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            var rightFirst = new Vector3(distance , 0 , 0);
            var rightSecond = new Vector3(distance + arrowSize , -arrowSize / 2.0f);
            var rightThird = new Vector3(distance + arrowSize , 0 , 0);
            var rightFouth = new Vector3(distance + arrowSize , arrowSize / 2.0f);

            UIVertex right0 = GetUIVertex(rightFirst, Color.black);
            UIVertex right1 = GetUIVertex(rightSecond , Color.black);
            UIVertex right2 = GetUIVertex(rightThird , Color.black);
            UIVertex right3 = GetUIVertex(rightFouth , Color.black);

            var leftFirst = new Vector3(-rightFirst.x , rightFirst.y);
            var leftSecond = new Vector3(-rightSecond.x , rightSecond.y);
            var leftThird = new Vector3(-rightThird.x , rightThird.y);
            var leftFouth = new Vector3(-rightFouth.x , rightFouth.y);

            UIVertex right4 = GetUIVertex(leftFirst , Color.black);
            UIVertex right5 = GetUIVertex(leftSecond , Color.black);
            UIVertex right6 = GetUIVertex(leftThird , Color.black);
            UIVertex right7 = GetUIVertex(leftFouth , Color.black);

            vh.AddUIVertexQuad(new UIVertex[] { right0 , right1 , right2 , right3 });
            vh.AddUIVertexQuad(new UIVertex[] { right4 , right7 , right6 , right5 });
        }

        public UIVertex GetUIVertex( Vector2 point , Color color0 )
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