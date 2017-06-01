
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace SpringGUI
{
    public class UITree : UIBehaviour
    {
        #region custom icon by is open children 根据是否打开子级自定义图标

        [Tooltip("打开子级的图标")]
        public Sprite m_openIcon = null;
        [Tooltip("关闭子级的图标")]
        public Sprite m_closeIcon = null;
        [Tooltip("没有子级的图标")]
        public Sprite m_lastLayerIcon = null; 
        
        #endregion

        #region private && public members

        [HideInInspector]
        public UITreeData TreeRoot = null;
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

        #region import data function

        public void SetData( UITreeData rootData )
        {
            if ( null == m_container )
                getComponent();
            TreeRootNode.SetData(rootData);
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
            treeNode.GetComponent<UITreeNode>().SetData(data);
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