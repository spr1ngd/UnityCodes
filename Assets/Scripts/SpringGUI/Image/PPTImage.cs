
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public class PPTImage : Image
    {
        public enum AnimationType
        {
            // 百叶窗
            Window_Shade,
            // 翻书页
            Page_Turning
        }

        [SerializeField]
        private AnimationType m_animation = AnimationType.Window_Shade;
        public AnimationType Animation 
        {
            get { return m_animation; }
            set { m_animation = value; }
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            //List<UIVertex> vertexs = new List<UIVertex>();
            //base.OnPopulateMesh(toFill);
            //toFill.GetUIVertexStream(vertexs);
            var size = GetPixelAdjustedRect().size;
            var first = new Vector2(-size.x / 2.0f, -size.y / 2.0f);
            var second = new Vector2(-size.x / 2.0f, size.y / 2.0f);
            var third = new Vector2(0, -size.y / 2.0f);
            var fouth = new Vector2(0, size.y / 2.0f);
            var five = new Vector2(size.x / 2.0f, -size.y / 2.0f);
            var six = new Vector2(size.x / 2.0f, size.y / 2.0f);
            toFill.Clear();
            toFill.AddUIVertexQuad(new UIVertex[]
            {
                GetUIVertex(first,Color.white,new Vector2(0,0)),
                GetUIVertex(second,Color.white,new Vector2(0,1)),
                GetUIVertex(fouth,Color.white,new Vector2(0.5f,1)),
                GetUIVertex(third,Color.white,new Vector2(0.5f,0))
            });
            //toFill.AddUIVertexQuad(new UIVertex[]
            //{
            //    GetUIVertex(third,Color.white,new Vector2(0.5f,0)),
            //    GetUIVertex(fouth,Color.white,new Vector2(0.5f,1)),
            //    GetUIVertex(six,Color.white,new Vector2(1,1)),
            //    GetUIVertex(five,Color.white,new Vector2(1,0))
            //});
            //toFill.AddUIVertexQuad(new UIVertex[]
            //{
            //    GetUIVertex(first,Color.white,new Vector2(0,0)),
            //    GetUIVertex(second,Color.white,new Vector2(0,1)),
            //    GetUIVertex(six,Color.white,new Vector2(1,1)),
            //    GetUIVertex(five,Color.white,new Vector2(1,0))
            //});
        }

        private UIVertex GetUIVertex( Vector2 pos  )
        {
            UIVertex vertex= new UIVertex();
            vertex.position = pos;
            vertex.color = Color.white;
            vertex.uv0 = new Vector2(0,0);
            return vertex;
        }
        private UIVertex GetUIVertex(Vector2 pos, Color color0)
        {
            UIVertex vertex = new UIVertex();
            vertex.position = pos;
            vertex.color = color0;
            vertex.uv0 = new Vector2(0, 0);
            return vertex;
        }
        private UIVertex GetUIVertex(Vector2 pos, Color color0, Vector2 uv0)
        {
            UIVertex vertex = new UIVertex();
            vertex.position = pos;
            vertex.color = color0;
            vertex.uv0 = uv0;
            return vertex;
        }
    }
}