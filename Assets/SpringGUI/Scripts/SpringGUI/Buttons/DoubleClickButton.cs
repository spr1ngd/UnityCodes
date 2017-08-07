
/*=========================================
* Author: springDong
* Description: SpringGUI.DoubleClickButton,you can use it like use normal button by addListener.
==========================================*/

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpringGUI
{
    [AddComponentMenu("UI/DoubleClickButton")]
    public class DoubleClickButton : Button 
    {
        [Serializable]
        public class DoubleClickedEvent : UnityEvent {}

        [SerializeField]
        private DoubleClickedEvent m_onDoubleClick = new DoubleClickedEvent();
        public DoubleClickedEvent onDoubleClick
        {
            get { return m_onDoubleClick; }
            set { m_onDoubleClick = value; }
        }

        private DateTime m_firstTime;
        private DateTime m_secondTime;
        
        private void Press()
        {
            if (null != onDoubleClick )
                onDoubleClick.Invoke();
            resetTime();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (m_firstTime.Equals(default(DateTime)))
                m_firstTime = DateTime.Now;
            else
                m_secondTime = DateTime.Now;
        }   

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (!m_firstTime.Equals(default(DateTime)) && !m_secondTime.Equals(default(DateTime)))
            {
                var intervalTime = m_secondTime - m_firstTime;
                float milliSeconds = intervalTime.Seconds * 1000 + intervalTime.Milliseconds;
                if (milliSeconds < 400)
                    Press();
                else
                    resetTime();
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            resetTime();
        }

        private void resetTime()
        {
            m_firstTime = default(DateTime);
            m_secondTime = default(DateTime);
        }
    }
}