
/*=========================================
* Author: springDong
* Description: SpringGUI.UITree/TreeView.UITreeNode
* UITreeNode is equivalent to the Controller in MVC, used to responsible for UITree and UITreeData interaction
==========================================*/

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace SpringGUI
{
    public class UITreeNode : UIBehaviour
    {
        #region private && public  members
        
        private UITreeData TreeData = null;
        private UITree UITree = null;
        private Toggle toggle = null;
        private Image icon = null;
        private Text text = null;
        private Transform _toggleTransform = null;
        private Transform _myTransform = null;
        private Transform _container = null;

        private List<GameObject> _children = new List<GameObject>();

        #endregion

        #region get && reset ui component
        
        private void getComponent( )
        {
            _myTransform = this.transform;
            _container = _myTransform.FindChild("Container");
            toggle = _container.FindChild("Toggle").GetComponent<Toggle>();
            icon = _container.FindChild("IconContainer/Icon").GetComponent<Image>();
            text = _container.FindChild("Text").GetComponent<Text>();
            _toggleTransform = toggle.transform.FindChild("Image");
            UITree = _myTransform.parent.parent.parent.GetComponent<UITree>();
        }
        private void resetComponent( )
        {
            _container.localPosition = new Vector3(0 , _container.localPosition.y , 0);
            _toggleTransform.localEulerAngles = new Vector3(0 , 0 , 90);
            _toggleTransform.gameObject.SetActive(true);
        }

        #endregion

        #region external call interface

        public void Inject( UITreeData data )
        {
            if ( null == _myTransform )
                getComponent();
            resetComponent();
            TreeData = data;
            text.text = data.Name;
            toggle.isOn = false;
            toggle.onValueChanged.AddListener(openOrClose);
            _container.localPosition += new Vector3(_container.GetComponent<RectTransform>().sizeDelta.y * TreeData.Layer , 0 , 0);
            if ( data.ChildNodes.Count.Equals(0) )
            {
                _toggleTransform.gameObject.SetActive(false);
                icon.sprite = UITree.m_lastLayerIcon;
            }
            else
                icon.sprite = toggle.isOn ? UITree.m_openIcon : UITree.m_closeIcon;
        }

        [Obsolete("This method is replaced by Inject")]
        public void SetData( UITreeData data )
        {
            if(null == _myTransform)
                getComponent();
            resetComponent();
            TreeData = data;
            text.text = data.Name;
            toggle.isOn = false;
            toggle.onValueChanged.AddListener(openOrClose);
            _container.localPosition += new Vector3(_container.GetComponent<RectTransform>().sizeDelta.y * TreeData.Layer , 0 , 0);
            if (data.ChildNodes.Count.Equals(0))
            {
                _toggleTransform.gameObject.SetActive(false);
                icon.sprite = UITree.m_lastLayerIcon;
            }
            else
                icon.sprite = toggle.isOn ? UITree.m_openIcon : UITree.m_closeIcon;
        }

        #endregion

        #region open && close

        private void openOrClose( bool isOn )
        {
            if ( isOn ) openChildren();
            else closeChildren();
            _toggleTransform.localEulerAngles = isOn ? new Vector3(0 , 0 , 0) : new Vector3(0 , 0 , 90);
            icon.sprite = toggle.isOn ? UITree.m_openIcon : UITree.m_closeIcon;
        }
        private void openChildren()
        {
            _children = UITree.pop(TreeData.ChildNodes,transform.GetSiblingIndex());
        }

        protected void closeChildren( ) 
        {
            for (int i = 0; i < _children.Count; i++)
            {
                UITreeNode node = _children[i].GetComponent<UITreeNode>();
                node.RemoveListener();
                node.closeChildren();
            }
            UITree.push(_children);
            _children = new List<GameObject>();
        }
        private void RemoveListener()
        {
            toggle.onValueChanged.RemoveListener(openOrClose);
        }

        #endregion
    }
}