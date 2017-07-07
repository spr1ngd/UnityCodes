
using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

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

        private static readonly Vector2 _defaultUITreeSize = new Vector2(300 , 400);
        private static readonly Vector2 _defaultUITreeNodeSize = new Vector2(300 , 25);
        private static readonly Vector2 _defaultCalendarSize = new Vector2(220 , 160);
        private static readonly Vector2 _defaultDatePickerSize = new Vector2(180 , 25);
        private static readonly Vector2 _defaultColoredTapeSize = new Vector2(20,200);

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

        /// <summary>
        /// Create Calendar
        /// </summary>
        public static GameObject CreateCalendar( Resources resources )
        {
            DefaultControls.Resources res = convertToDefaultResources(resources);
            #region Create Calendar Container
            //create calendar
            GameObject calendar = DefaultControls.CreateImage(res);
            calendar.name = "Calendar";
            calendar.AddComponent<Calendar>();
            RectTransform calendarRect = calendar.GetComponent<RectTransform>();
            calendarRect.sizeDelta = _defaultCalendarSize;
            #endregion
            #region Create Title

            //create title
            GameObject title = CreateUIObject("Title",calendar.transform);
            RectTransform titleRect = title.GetComponent<RectTransform>();
            titleRect.pivot = new Vector2(0.5f,1);
            titleRect.anchorMin = new Vector2(0,1);
            titleRect.anchorMax = Vector2.one;
            titleRect.sizeDelta = new Vector2(0,30);
            
            //create last button
            GameObject lastButton = DefaultControls.CreateImage(res);
            lastButton.AddComponent<Button>();
            lastButton.GetComponent<Image>().sprite = res.dropdown;
            lastButton.transform.localEulerAngles = new Vector3(0,0,-90);
            lastButton.name = "LastButton";
            SetParentAndAlign(lastButton,title.transform);
            RectTransform lastButtonRect = lastButton.GetComponent<RectTransform>();
            lastButtonRect.anchorMin = new Vector2(0,1);
            lastButtonRect.anchorMax = new Vector2(0,1);
            lastButtonRect.sizeDelta = new Vector2(20,20);
            lastButtonRect.transform.localPosition += new Vector3(15,-15);

            //create next button
            GameObject nextButton = DefaultControls.CreateImage(res);
            nextButton.AddComponent<Button>();
            nextButton.GetComponent<Image>().sprite = res.dropdown;
            nextButton.transform.localEulerAngles = new Vector3(0 , 0 ,90);
            nextButton.name = "NextButton";
            SetParentAndAlign(nextButton , title.transform);
            RectTransform nextButtonRect = nextButton.GetComponent<RectTransform>();
            nextButtonRect.anchorMin = new Vector2(1 , 1);
            nextButtonRect.anchorMax = new Vector2(1 , 1);
            nextButtonRect.sizeDelta = new Vector2(20 , 20);
            nextButtonRect.transform.localPosition += new Vector3(-15 , -15);

            //create time button
            GameObject timeButton = CreateUIObject("TimeButton", title.transform);
            timeButton.AddComponent<Button>();
            RectTransform timeButtonRect = timeButton.GetComponent<RectTransform>();
            timeButtonRect.anchorMin = new Vector2(0.5f , 0);
            timeButtonRect.anchorMax = new Vector2(0.5f , 1);
            timeButtonRect.sizeDelta = new Vector2(160,0);

            //create time button text
            GameObject timeText = DefaultControls.CreateText(res);
            SetParentAndAlign(timeText,timeButton.transform);
            Text indexText = timeText.GetComponent<Text>();
            indexText.alignment = TextAnchor.MiddleCenter;
            indexText.text = DateTime.Now.ToShortDateString();
            RectTransform textRect = timeText.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            timeButton.GetComponent<Button>().targetGraphic = indexText;

            #endregion
            #region Create Container

            //create container
            GameObject container = DefaultControls.CreateImage(res);
            container.name = "Container";
            SetParentAndAlign(container , calendar.transform);
            RectTransform rectContainer = container.GetComponent<RectTransform>();
            rectContainer.anchorMin = new Vector2(0 , 0.5f);
            rectContainer.anchorMax = new Vector2(1 , 0.5f);
            rectContainer.sizeDelta = new Vector2(-20 , 126);
            rectContainer.localPosition -= new Vector3(0,17);

            //create weeks
            GameObject weeks = DefaultControls.CreateImage(res);
            weeks.name = "Weeks";
            SetParentAndAlign(weeks , container.transform);
            RectTransform weeksRect = weeks.GetComponent<RectTransform>();
            weeksRect.anchorMin = new Vector2(0 , 1);
            weeksRect.anchorMax = Vector2.one;
            weeksRect.pivot = new Vector2(0.5f , 1);
            weeksRect.sizeDelta = new Vector2(0 , 18);
            GridLayoutGroup glg = weeks.AddComponent<GridLayoutGroup>();
            glg.cellSize = new Vector2(26 , 18);
            glg.spacing = new Vector2(3 , 0);
            weeks.GetComponent<Image>().color = Color.gray;
            //create week Text
            GameObject weekText = DefaultControls.CreateText(res);
            weekText.name = "WeekTemplate";
            Text text = weekText.GetComponent<Text>();
            text.alignment = TextAnchor.MiddleCenter;
            text.text = "Sunday";
            SetParentAndAlign(weekText , weeks.transform);

            //create days
            GameObject days = DefaultControls.CreateImage(res);
            days.name = "Days";
            SetParentAndAlign(days , container.transform);
            RectTransform daysRect = days.GetComponent<RectTransform>();
            daysRect.anchorMin = Vector2.zero;
            daysRect.anchorMax = Vector2.one;
            daysRect.sizeDelta = new Vector2(0 , -18);
            daysRect.localPosition -= new Vector3(0 , 9 , 0);
            GridLayoutGroup glg2 = days.AddComponent<GridLayoutGroup>();
            glg2.cellSize = new Vector2(27.7f , 17f);
            glg2.spacing = new Vector2(1 , 1);
            //create day text
            GameObject dayText = DefaultControls.CreateButton(res);
            dayText.name = "DayTemplate";
            dayText.transform.FindChild("Text").GetComponent<Text>().text = "31";
            dayText.GetComponent<Image>().sprite = null;
            dayText.GetComponent<Image>().color = Color.cyan;
            SetParentAndAlign(dayText , days.transform);

            //create months
            GameObject months = DefaultControls.CreateImage(res);
            months.name = "Months";
            SetParentAndAlign(months , container.transform);
            RectTransform monthRect = months.GetComponent<RectTransform>();
            monthRect.anchorMin = Vector2.zero;
            monthRect.anchorMax = Vector2.one;
            monthRect.sizeDelta = new Vector2(0 , 0);
            GridLayoutGroup glg3 = months.AddComponent<GridLayoutGroup>();
            glg3.cellSize = new Vector2(47 , 36);
            glg3.spacing = new Vector2(4 , 9);
            //create monthText
            GameObject monthText = DefaultControls.CreateButton(res);
            monthText.name = "MonthTemplate";
            monthText.transform.FindChild("Text").GetComponent<Text>().text = "January";
            monthText.GetComponent<Image>().sprite = null;
            monthText.GetComponent<Image>().color = Color.cyan;
            SetParentAndAlign(monthText , months.transform);
            months.SetActive(false);

            #endregion
            return calendar;
        }

        /// <summary>
        /// Create Date Picker
        /// </summary>
        public static GameObject CreateDatePicker( Resources resources )
        {
            DefaultControls.Resources res = convertToDefaultResources(resources);
            // Create date picker
            GameObject datePicker = DefaultControls.CreateImage(res);
            datePicker.name = "DatePicker";
            datePicker.AddComponent<DatePicker>();
            RectTransform pickerRect = datePicker.GetComponent<RectTransform>();
            pickerRect.sizeDelta = _defaultDatePickerSize;

            // Create date text
            GameObject dateText = DefaultControls.CreateText(res);
            dateText.name = "DateText";
            SetParentAndAlign(dateText , datePicker.transform);
            RectTransform textRect = dateText.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = new Vector2(0,1);
            textRect.pivot = new Vector2(0,0.5f);
            textRect.sizeDelta = new Vector2(150,0);
            Text textText = dateText.GetComponent<Text>();
            textText.text = System.DateTime.Today.ToShortDateString();
            textText.alignment = TextAnchor.MiddleLeft;

            // Create pick button
            GameObject pickButton = DefaultControls.CreateImage(res);
            pickButton.name = "PickButton";
            SetParentAndAlign(pickButton,datePicker.transform);
            pickButton.AddComponent<Button>();
            RectTransform buttonRect = pickButton.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(1,0);
            buttonRect.anchorMax = Vector2.one;
            buttonRect.sizeDelta = new Vector2(_defaultDatePickerSize.y,0);
            pickButton.GetComponent<Image>().sprite = res.dropdown;
            buttonRect.transform.localPosition -= new Vector3(_defaultDatePickerSize.y / 2,0,0);

            //Create Calendar
            GameObject calendar = CreateCalendar(resources);
            calendar.transform.SetParent(datePicker.transform);
            calendar.transform.localPosition -= new Vector3(0, _defaultDatePickerSize.y/2+_defaultCalendarSize.y/2, 0);
            calendar.SetActive(false);
            return datePicker;
        }

        /// <summary>
        /// Create Vertical Colored Tape
        /// </summary>
        public static GameObject CreataVerticalColoredTape( Resources resources )
        {
            GameObject coloredTape = CreateUIElementRoot("ColoredTapte" , _defaultColoredTapeSize);
            coloredTape.AddComponent<ColoredTape>();
            return coloredTape;
        }

        /// <summary>
        /// Create Horizontal Colored Tape
        /// </summary>
        public static GameObject CreataHorizontalColoredTape( Resources resources )
        {
            GameObject coloredTape = CreateUIElementRoot("ColoredTapte" , new Vector2(_defaultColoredTapeSize.y , _defaultColoredTapeSize.x));
            coloredTape.AddComponent<ColoredTape>();
            return coloredTape;
        }

        public static GameObject CreateColorPicker( Resources resources )
        {
            return null;
        }
    }
}