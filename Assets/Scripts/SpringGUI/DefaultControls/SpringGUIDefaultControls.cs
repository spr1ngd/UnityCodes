
using UnityEngine;
using UnityEngine.UI;

namespace SpringGUI
{
    public static class SpringGUIDefaultControls
    {
        public struct Resources
        {
            public Sprite standard;
            public Sprite background;
            public Sprite inputField;
            public Sprite knob;
            public Sprite checkmark;
            public Sprite dropdown;
            public Sprite mask;
        }

        private static readonly Vector2 _defaultUITreeSize = new Vector2(300,400);
        private static readonly Vector2 _defaultUITreeNodeSize = new Vector2(300,25);

        private static GameObject CreateUIElementRoot( string name , Vector2 size )
        {
            GameObject child = new GameObject(name);
            RectTransform rectTransform = child.AddComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            return child;
        }

        private static GameObject CreateUIObject( string name ,Transform parent)
        {
            GameObject go = new GameObject(name);
            go.AddComponent<RectTransform>();
            SetParentAndAlign(go,parent);
            return go;
        }

        private static void SetParentAndAlign( GameObject child,Transform parent )
        {
            if(null == parent)
                return;
            child.transform.SetParent(parent,false);
            SetLayerRecursively(child, parent.gameObject.layer);
        }
        
        private static void SetLayerRecursively(GameObject child,int layer)
        {
            child.layer = layer;
            Transform t = child.transform;
            for ( int i = 0 ; i < t.childCount ; i++ )
                SetLayerRecursively(t.GetChild(i).gameObject , layer);
        }
        
        /// <summary>
        /// Create UITree Component
        /// </summary>
        public static GameObject CreaatUITree( Resources resources )
        {
            //create ui tree
            GameObject uiTree = DefaultControls.CreateScrollView(convertToDefaultResources(resources));
            uiTree.AddComponent<UITree>();
            ScrollRect uiTreeScrollRect = uiTree.GetComponent<ScrollRect>();
            uiTree.GetComponent<RectTransform>().sizeDelta = _defaultUITreeSize;
            uiTreeScrollRect.horizontal = false;
            uiTree.name = "UITree";
            Transform content = uiTree.transform.FindChild("Viewport/Content");
            GridLayoutGroup glg = content.gameObject.AddComponent<GridLayoutGroup>();
            glg.cellSize = _defaultUITreeNodeSize;
            glg.spacing = new Vector2(0 , 2);
            glg.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            glg.constraintCount = 1;
            ContentSizeFitter csf = content.gameObject.AddComponent<ContentSizeFitter>();
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            //create root node
            GameObject rootNode = CreateUITreeNode(resources);
            SetParentAndAlign(rootNode, content);
            return uiTree;
        }

        /// <summary>
        /// Create UITreeNode
        /// </summary>
        public static GameObject CreateUITreeNode( Resources resources )
        {
            //create tree node
            GameObject treeNode = CreateUIElementRoot("TreeNodeTemplate", _defaultUITreeNodeSize);
            RectTransform treeNodeRect = treeNode.GetComponent<RectTransform>();
            float size = treeNodeRect.sizeDelta.x;
            treeNodeRect.anchorMin = new Vector2(0,1);
            treeNodeRect.anchorMax = new Vector2(1,1);
            treeNodeRect.pivot = new Vector2(0.5f,1);
            treeNodeRect.sizeDelta = new Vector2(0,_defaultUITreeNodeSize.y);
            treeNode.AddComponent<UITreeNode>();

            //create container for toggle/icon/text 
            GameObject container = CreateUIObject("Container",treeNode.transform);
            SetParentAndAlign(container,treeNode.transform);
            RectTransform containerRect = container.GetComponent<RectTransform>();
            containerRect.sizeDelta = new Vector2(_defaultUITreeSize.x ,_defaultUITreeNodeSize.y);

            //create toggle 
            GameObject toggle = CreateUIElementRoot("Toggle" , _defaultUITreeNodeSize);
            toggle.AddComponent<Toggle>();
            SetParentAndAlign(toggle, container.transform);
            RectTransform toggleRect = toggle.GetComponent<RectTransform>();
            toggleRect.anchorMax = new Vector2(0,1);
            toggleRect.anchorMin = Vector2.zero;
            toggleRect.pivot = new Vector2(0 , 0.5f);
            toggleRect.sizeDelta = new Vector2(_defaultUITreeNodeSize.y , 0);

            //create toogleimage
            GameObject toggleImage = DefaultControls.CreateImage(convertToDefaultResources(resources));
            SetParentAndAlign(toggleImage , toggle.transform);
            RectTransform imageRect = toggleImage.GetComponent<RectTransform>();
            imageRect.anchorMax = Vector2.one;
            imageRect.anchorMin = Vector2.zero;
            imageRect.sizeDelta = new Vector2(-8 , -8);
            toggleImage.GetComponent<Image>().sprite = resources.dropdown;
            toggle.GetComponent<Toggle>().targetGraphic = toggleImage.GetComponent<Image>();

            //create icon container
            GameObject iconContainer = CreateUIObject("IconContainer", container.transform);
            SetParentAndAlign(iconContainer, container.transform);
            RectTransform icRect = iconContainer.GetComponent<RectTransform>();
            icRect.anchorMax = new Vector2(0 , 1);
            icRect.anchorMin = Vector2.zero;
            icRect.pivot = new Vector2(0 , 0.5f);
            icRect.sizeDelta = new Vector2(_defaultUITreeNodeSize.y , 0);
            icRect.transform.localPosition = toggle.transform.localPosition + new Vector3(_defaultUITreeNodeSize.y , 0);
            
            //create icon
            GameObject icon = DefaultControls.CreateImage(convertToDefaultResources(resources));
            icon.name = "Icon";
            SetParentAndAlign(icon , iconContainer.transform);
            RectTransform iconRect = icon.GetComponent<RectTransform>();
            iconRect.sizeDelta = new Vector2(_defaultUITreeNodeSize.y , _defaultUITreeNodeSize.y);

            //create text
            GameObject text = DefaultControls.CreateText(convertToDefaultResources(resources));
            text.name = "Text";
            SetParentAndAlign(text , container.transform);
            RectTransform textRect = text.GetComponent<RectTransform>();
            textRect.anchorMax = new Vector2(0 , 1);
            textRect.anchorMin = Vector2.zero;
            textRect.pivot = new Vector2(0 , 0.5f);
            textRect.sizeDelta = new Vector2(size - 2 * _defaultUITreeNodeSize.y - 8 , 0 );
            text.transform.localPosition = iconContainer.transform.localPosition + new Vector3(_defaultUITreeNodeSize.y + 8 , 0);
            Text textText = text.GetComponent<Text>();
            textText.alignment = TextAnchor.MiddleLeft;
            return treeNode;
        }

        /// <summary>
        /// Create double click button
        /// </summary>
        public static GameObject CreateDoubleClickButton( Resources resources )
        {
            GameObject dcButton = DefaultControls.CreateButton(convertToDefaultResources(resources));
            dcButton.name = "DoubleClickButton";
            dcButton.transform.FindChild("Text").GetComponent<Text>().text = "双击按钮";
            Object.DestroyImmediate(dcButton.GetComponent<Button>());
            dcButton.AddComponent<DoubleClickButton>();
            return dcButton;
        }

        /// <summary>
        /// Create long click button
        /// </summary>
        public static GameObject CreateLongClickButton( Resources resources )
        {
            GameObject lcButton = DefaultControls.CreateButton(convertToDefaultResources(resources));
            lcButton.name = "LongClickButton";
            lcButton.transform.FindChild("Text").GetComponent<Text>().text = "长击按钮";
            Object.DestroyImmediate(lcButton.GetComponent<Button>());
            lcButton.AddComponent<LongClickButton>();
            return lcButton;
        }

        private static DefaultControls.Resources convertToDefaultResources( Resources resources )
        {
            DefaultControls.Resources res = new DefaultControls.Resources();
            res.background = resources.background;
            res.checkmark = resources.checkmark;
            res.dropdown = resources.dropdown;
            res.inputField = resources.inputField;
            res.knob = resources.knob;
            res.mask = resources.mask;
            res.standard = resources.standard;
            return res;
        }
    }
}