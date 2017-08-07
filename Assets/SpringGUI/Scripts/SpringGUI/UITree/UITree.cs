
/*=========================================
* Author: springDong
* Description: SpringGUI.UITree/TreeView
* All operation auto
* You only can set three sprite for open icon/close icon/leaf node icon
==========================================*/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace SpringGUI
{
    public class UITree : UIBehaviour
    {
        #region custom icon by is open children 
        
        public Sprite m_openIcon = null;
        public Sprite m_closeIcon = null;
        public Sprite m_lastLayerIcon = null;

        #endregion

        #region external call interface

        public void Inject( UITreeData rootData )
        {
            if ( null == m_container )
                getComponent();
            TreeRootNode.Inject(rootData);
        }

        // insert new node method. The next version will add this funcion.
        [Obsolete("Next version will add this funcion")]
        public void Inject( UITreeData insertData , UITreeData parentData )
        {

        }
        
        [Obsolete("This method is replaced by Inject.")]
        public void SetData( UITreeData rootData )
        {
            if ( null == m_container )
                getComponent();
            TreeRootNode.SetData(rootData);
        }

        #endregion

        #region private && public members
        
        [HideInInspector]
        public UITreeNode TreeRootNode = null; 
        private Transform m_container = null;
        private GameObject m_nodePrefab = null;
        public GameObject NodePrefab
        {
            get { return m_nodePrefab ?? ( m_nodePrefab = m_container.GetChild(0).gameObject ); }
            set { m_nodePrefab = value; }
        }

        #endregion
        
        #region get Component

        private void getComponent( )
        {
            m_container = transform.FindChild("Viewport/Content");
            if(m_container.childCount.Equals(0))
                throw new Exception("UITreeNode Template can not be null! Create a Template!");
            TreeRootNode = m_container.GetChild(0).GetComponent<UITreeNode>();
        }

        #endregion
        
        #region cache pool functions

        private readonly List<GameObject> m_pool = new List<GameObject>();
        private Transform m_poolParent = null;

        public List<GameObject> pop( List<UITreeData> datas ,int siblingIndex )
        {
            List<GameObject> result = new List<GameObject>();
            for ( int i = datas.Count - 1 ; i >= 0 ; i-- )
                result.Add(pop(datas[i] , siblingIndex));
            return result;
        }
        public GameObject pop( UITreeData data ,int siblingIndex )
        {
            GameObject treeNode = null;
            if ( m_pool.Count > 0 )
            {
                treeNode = m_pool[0];
                m_pool.RemoveAt(0);
            }
            else
                treeNode = cloneTreeNode();
            treeNode.transform.SetParent(m_container);
            treeNode.SetActive(true);
            //treeNode.GetComponent<UITreeNode>().SetData(data);
            treeNode.GetComponent<UITreeNode>().Inject(data);
            treeNode.transform.SetSiblingIndex(siblingIndex + 1);
            return treeNode;
        }

        public void push( List<GameObject> treeNodes )
        {
            foreach ( GameObject node in treeNodes )
                push(node);
        }
        public void push( GameObject treeNode )
        {
            if(null == m_poolParent)
                m_poolParent = new GameObject("CachePool").transform;
            treeNode.transform.SetParent(m_poolParent);
            treeNode.SetActive(false);
            m_pool.Add(treeNode);
        }
        
        private GameObject cloneTreeNode( )
        {
            GameObject result = GameObject.Instantiate(NodePrefab) as GameObject;
            result.transform.SetParent(m_container);
            return result;
        }

        #endregion
    }
}