
/*=========================================
* Author : SpringDong
* DateTime : 2017/6/13 14:03:45
* Email : 540637360@qq.com
* Description : 拾取颜色的调色板/游标
==========================================*/

using UnityEngine;
using UnityEngine.EventSystems;

namespace SpringGUI
{
    internal class ColorPalette : MonoBehaviour , IPointerUpHandler , IDragHandler ,IPointerDownHandler
    {
        private ColorPicker m_colorPicker = null;
        private Transform m_transform = null;
        private Transform m_nonius = null;
        private bool m_canDrag = false;
        private Vector2 m_size;
        private float m_halfX;
        private float m_halfY;

        private void Start()
        {
            m_transform = transform;
            m_size = m_transform.GetComponent<RectTransform>().sizeDelta;
            m_halfX = m_size.x / 2.0f;
            m_halfY = m_size.y / 2.0f;
            m_nonius = transform.FindChild("ColorNonius");
            m_colorPicker = m_transform.parent.GetComponent<ColorPicker>();    
        }
        private void Update()
        {
            var x = Mathf.Clamp(m_nonius.localPosition.x , -m_halfX , m_halfX);
            var y = Mathf.Clamp(m_nonius.localPosition.y , -m_halfY , m_halfY);
            m_nonius.localPosition = new Vector3(x , y);
            if(m_canDrag)
                m_colorPicker.SuckColorByNonius(m_nonius.localPosition);
        }

        public void ResetNoniusPosition( Vector3 position )
        {
            if ( null == m_nonius )
                m_nonius = transform.FindChild("ColorNonius");
            m_nonius.localPosition = position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_canDrag = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if( m_canDrag )
                m_nonius.localPosition = mousPosition();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_canDrag = true;
        }

        private Vector3 mousPosition( )
        {
            return Input.mousePosition - m_transform.position;
        }
    }
}