
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
        private static readonly Vector2 _defaultColoredTapeSize = new Vector2(20 , 200);
        private static readonly Vector2 _defaultColorPicker = new Vector2(237 , 421);
        private static readonly Vector2 _defaultRadarMap = new Vector2(250 , 250);

        private static GameObject CreateUIElementRoot( string name , Vector2 size )
        {
            GameObject child = new GameObject(name);
            RectTransform rectTransform = child.AddComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            return child;
        }

        private static GameObject CreateUIObject( string name ,Transform parent ,Vector2 size  )
        {
            var go = CreateUIObject(name, parent);
            go.GetComponent<RectTransform>().sizeDelta = size;
            return go;
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
        public static GameObject CreateUITree( Resources resources )
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

        /// <summary>
        /// Create Color Picker
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        public static GameObject CreateColorPicker( Resources resources )
        {
            GameObject colorPicker = CreateUIElementRoot("ColorPicker" , _defaultColorPicker);
            var mainCT = colorPicker.AddComponent<ColoredTape>();
            mainCT.TapeDirection = ColoredTape.E_DrawDirection.Vertical;
            mainCT.SetColors(new Color[] { new Color(0.8f , 0.8f , 0.8f , 1) , new Color(0 , 0.7f , 1 , 1) });

            #region 主色板
            //MainColor 主色板
            var mainColor = CreateUIObject("MainColor" , colorPicker.transform , new Vector2(185 , 22)).AddComponent<MainColorTape>().gameObject.transform;
            mainColor.localPosition = new Vector3(16.2f , 188.2f);
            #endregion

            #region 吸管
            //Sucker 吸管
            var sucker = CreateUIObject("ColorSucker", colorPicker.transform, new Vector2(22, 22));
            sucker.AddComponent<SuckerImage>();
            sucker.AddComponent<Button>();
            sucker.transform.localPosition = new Vector3(-95.6f , 186.8f);
            sucker.transform.localEulerAngles = new Vector3(0 , 0 , 321.0f);
            #endregion

            #region 调色板
            //Color Palette 调色板
            var colorPalette = CreateUIObject("ColorPalette" , colorPicker.transform , new Vector2(150 , 150));
            colorPalette.AddComponent<ColorPalette>();
            colorPalette.transform.localPosition = new Vector3(-11.5f , 75);
            //第一层色带
            var firstCTGO = CreateUIObject("FirstLayerColoredTape" , colorPalette.transform , new Vector2(150 , 150));
            var fisrtCT = firstCTGO.AddComponent<ColoredTape>();
            fisrtCT.TapeDirection  = ColoredTape.E_DrawDirection.Horizontal;;
            fisrtCT.OuelineWidth = 0.8f;
            fisrtCT.SetColors(new Color[] { Color.white , Color.red });
            //第二层色带
            var secondCTGO = CreateUIObject("SecondLayerColoredTape" , colorPalette.transform , new Vector2(150 , 150));
            var secondCT = secondCTGO.AddComponent<ColoredTape>();
            secondCT.Outline = false;
            secondCT.SetColors(new Color[] { new Color(0 , 0 , 0 , 0) , Color.black });
            //多色色带
            var multiCTGO = CreateUIObject("MultiColoredTape" , colorPalette.transform , new Vector2(150 , 150));
            var multiCT = multiCTGO.AddComponent<MultiColoredTape>();
            multiCT.OuelineWidth = 0.8f;
            multiCT.SetColors(new Color[] { Color.yellow , Color.white , Color.red , Color.magenta });
            multiCTGO.SetActive(false);
            //左侧文字
            var leftText = DefaultControls.CreateText(convertToDefaultResources(resources));
            leftText.name = "LeftText";
            leftText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            leftText.GetComponent<Text>().fontSize = 10;
            leftText.GetComponent<Text>().text = "Brightness";
            leftText.transform.SetParent(colorPalette.transform);
            leftText.GetComponent<RectTransform>().sizeDelta = new Vector2(143.6f , 17.1f);
            leftText.transform.localPosition = new Vector3(-86.17f , 1.7f);
            leftText.transform.localEulerAngles = new Vector3(0 , 0 , 90);
            //底部文字
            var bottomtText = DefaultControls.CreateText(convertToDefaultResources(resources));
            bottomtText.name = "BottomText";
            bottomtText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            bottomtText.GetComponent<Text>().fontSize = 10;
            bottomtText.GetComponent<Text>().text = "Saturation";
            bottomtText.transform.SetParent(colorPalette.transform);
            bottomtText.GetComponent<RectTransform>().sizeDelta = new Vector2(143.6f , 17.1f);
            bottomtText.transform.localPosition = new Vector3(0 , -87.0f);
            //右侧文字
            var rightText = DefaultControls.CreateText(convertToDefaultResources(resources));
            rightText.name = "RightText";
            rightText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            rightText.GetComponent<Text>().fontSize = 10;
            rightText.GetComponent<Text>().text = "Hue";
            rightText.transform.SetParent(colorPalette.transform);
            rightText.GetComponent<RectTransform>().sizeDelta = new Vector2(143.6f , 17.1f);
            rightText.transform.localPosition = new Vector3(115.64f , 0);
            rightText.transform.localEulerAngles = new Vector3(0 , 0 , -90);
            //游标
            var nonius = CreateUIObject("ColorNonius", colorPalette.transform, new Vector2(10, 10));
            nonius.AddComponent<ColorNonius>();
            nonius.transform.localPosition = new Vector3(-75 , 75);
            #endregion

            #region 颜色拾取功能
            // SuckScreen 截屏
            var suckScreen = CreateUIObject("SuckScreen" , colorPicker.transform , new Vector2(160 , 160));
            suckScreen.transform.localPosition = new Vector3(0 , 75);
            // Texture 截图纹理
            var screenTex = CreateUIObject("Texture" , suckScreen.transform , new Vector2(160 , 160));
            screenTex.AddComponent<Image>();
            // Mesh 创建网格
            var mesh = CreateUIObject("Mesh" , suckScreen.transform , new Vector2(160 , 160));
            var imageMesh = mesh.AddComponent<ImageMesh>();
            imageMesh.XAxisCount = 16;
            imageMesh.YAxisCount = 16;
            imageMesh.LineWidth = 1;
            imageMesh.Color = Color.gray;
            imageMesh.FocusColor = Color.red;
            imageMesh.FocuslineWidth = 1;
            suckScreen.SetActive(false);
            #endregion

            #region 垂直滑动条
            //ColoredTape Slider 垂直滑动条
            var verticalSlider = CreateUIObject("ColoredTapeSlider", colorPicker.transform, new Vector2(20, 150));
            verticalSlider.transform.localPosition = new Vector2(80 , 75);
            var vSlider = verticalSlider.AddComponent<Slider>();
            vSlider.direction = Slider.Direction.TopToBottom;
            vSlider.value = 0;
            // firstCT 
            var verticaCT = CreateUIObject("FirstLayerColoredTape", verticalSlider.transform, new Vector2(20, 150));
            var vCT = verticaCT.AddComponent<ColoredTape>();
            vCT.OuelineWidth = 0.8f;
            vCT.SetColors(new Color[] { Color.red,Color.magenta,Color.blue,Color.cyan,Color.green,Color.yellow,Color.red });
            // handle slider area
            var handleArea = CreateUIObject("Handle Slider Area", verticalSlider.transform, new Vector2(0, 0));
            var handleAreaRect = handleArea.GetComponent<RectTransform>();
            handleAreaRect.anchorMin = Vector2.zero;
            handleAreaRect.anchorMax = Vector2.one;
            var handle = CreateUIObject("Handle", handleArea.transform, new Vector2(20, 20));
            var handleRect = handle.GetComponent<RectTransform>();
            handleRect.anchorMin = new Vector2(0 , 1);
            handleRect.anchorMax = new Vector2(1 , 1);
            var sh = handle.AddComponent<SliderHandler>();
            sh.arrowSize = 5;
            sh.distance = 10;
            vSlider.targetGraphic = handle.GetComponent<SliderHandler>();
            vSlider.handleRect = handle.GetComponent<RectTransform>();
            #endregion

            #region 调色板模式按钮
            // Palette Mode Button 调色板模式按钮
            var pmButton = CreateUIObject("PaletteModeButton" , colorPicker.transform , new Vector2(12 , 9));
            pmButton.transform.localPosition = new Vector3(102.7f , 164.2f);
            var pmButtonTarget = CreateUIObject("Target" , pmButton.transform , new Vector2(12 , 9));
            pmButtonTarget.AddComponent<Image>();
            pmButton.AddComponent<Button>().targetGraphic = pmButtonTarget.GetComponent<Image>();
            var targetContent = CreateUIObject("GameObject" , pmButtonTarget.transform , new Vector2(9 , 9));
            targetContent.transform.localPosition = new Vector3(-1.5f , 0);
            var indexCT1 = CreateUIObject("CT1", targetContent.transform, new Vector2(9, 9));
            var indexct = indexCT1.AddComponent<ColoredTape>();
            indexct.TapeDirection = ColoredTape.E_DrawDirection.Horizontal;
            indexct.SetColors(new Color[] { Color.white , new Color(0.6f , 0 , 0.6f , 1) });
            var indexCT2 = CreateUIObject("CT2", targetContent.transform, new Vector2(9, 9));
            indexCT2.AddComponent<ColoredTape>().SetColors(new Color[] { new Color(1 , 1 , 1 , 0) , new Color(0 , 0 , 0 , 1) });
            var indexCT = CreateUIObject("CT", pmButtonTarget.transform, new Vector2(3, 9));
            indexCT.transform.localPosition = new Vector2(4.5f , 0);
            indexCT.AddComponent<ColoredTape>().SetColors(new Color[]
            {
                Color.red,Color.magenta, Color.blue,Color.cyan,Color.green,Color.yellow,Color.red
            });
            #endregion

            #region 颜色模式按钮
            // Color Mode Button 颜色模式按钮
            var cmButton = CreateUIObject("ColorModeButton" , colorPicker.transform , new Vector2(9 , 9));
            cmButton.transform.localPosition = new Vector3(102.7f,-15);
            var cmButtonTarget = CreateUIObject("Target" , cmButton.transform , new Vector2(9 , 9));
            cmButtonTarget.AddComponent<Image>();
            cmButton.AddComponent<Button>().targetGraphic = cmButtonTarget.GetComponent<Image>();
            var hCT1 = CreateUIObject("CT1" , cmButtonTarget.transform , new Vector2(9 , 3));
            hCT1.transform.localPosition = new Vector3(0,3);
            var ct1 = hCT1.AddComponent<ColoredTape>();
            ct1.TapeDirection = ColoredTape.E_DrawDirection.Horizontal;
            ct1.SetColors(new Color[] { Color.red , Color.magenta , Color.blue , Color.cyan , Color.green , Color.yellow , Color.red });
            var hCT2 = CreateUIObject("CT2" , cmButtonTarget.transform , new Vector2(9 , 3));
            var ct2 = hCT2.AddComponent<ColoredTape>();
            ct2.TapeDirection = ColoredTape.E_DrawDirection.Horizontal;
            ct2.SetColors(new Color[] { Color.white , new Color(0.6f , 0 , 0.6f) });
            var hCT3 = CreateUIObject("CT3" , cmButtonTarget.transform , new Vector2(9 , 3));
            hCT3.transform.localPosition = new Vector3(0 , -3);
            var ct3 = hCT3.AddComponent<ColoredTape>();
            ct3.TapeDirection = ColoredTape.E_DrawDirection.Horizontal;
            ct3.SetColors(new Color[] { Color.black,Color.white });
            #endregion
            
            #region RGBA Inputfield && slider  RGBA四通道
            //
            var rgba = CreateUIObject("RGBA", colorPicker.transform, new Vector2(216, 112));
            rgba.transform.localPosition = new Vector3(0 , -80);
            var glg = rgba.AddComponent<GridLayoutGroup>();
            glg.cellSize = new Vector2(216 , 28);
            // R
            var rModule = CreateUIObject("R" , rgba.transform , new Vector2(216 , 28));
            var rText = CreateUIObject("Text" , rModule.transform , new Vector2(18.3f , 28));
            rText.transform.localPosition = new Vector3(-94.6f , 0);
            rText.AddComponent<Text>().text = "R";
            rText.GetComponent<Text>().color = new Color(0.2f,0.2f,0.2f,1);
            rText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            var rValue = DefaultControls.CreateInputField(convertToDefaultResources(resources));
            rValue.GetComponent<RectTransform>().sizeDelta = new Vector2(27.4f , 20);
            rValue.transform.SetParent(rModule.transform);
            rValue.transform.localPosition = new Vector3(94.3f,0);
            rValue.name = "Value";
            rValue.GetComponent<InputField>().text = "255";
            rValue.transform.FindChild("Text").GetComponent<Text>().fontSize = 12;
            rValue.transform.FindChild("Text").GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            var rRect = rValue.transform.FindChild("Text").GetComponent<RectTransform>();
            rRect.sizeDelta = new Vector2(27.4f , 20);
            rRect.anchorMin = Vector2.zero;
            rRect.anchorMax = Vector2.one;
            var rSlider = createColoredTapeSlider(rModule.transform,
                new Color[] {Color.cyan,Color.white});
            rSlider.name = "Slider";
            // G
            var gModule = CreateUIObject("G" , rgba.transform , new Vector2(216 , 28));
            var gText = CreateUIObject("Text" , gModule.transform , new Vector2(18.3f , 28));
            gText.transform.localPosition = new Vector3(-94.6f , 0);
            gText.AddComponent<Text>().text = "G";
            gText.GetComponent<Text>().color = new Color(0.2f , 0.2f , 0.2f , 1);
            gText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            var gValue = DefaultControls.CreateInputField(convertToDefaultResources(resources));
            gValue.GetComponent<RectTransform>().sizeDelta = new Vector2(27.4f,20);
            gValue.transform.SetParent(gModule.transform);
            gValue.transform.localPosition = new Vector3(94.3f , 0);
            gValue.name = "Value";
            gValue.GetComponent<InputField>().text = "255";
            gValue.transform.FindChild("Text").GetComponent<Text>().fontSize = 12;
            gValue.transform.FindChild("Text").GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            var gRect = gValue.transform.FindChild("Text").GetComponent<RectTransform>();
            gRect.sizeDelta = new Vector2(27.4f , 20);
            gRect.anchorMin = Vector2.zero;
            gRect.anchorMax = Vector2.one;
            var gSlider = createColoredTapeSlider(gModule.transform ,
                new Color[] {Color.magenta,Color.white});
            gSlider.name = "Slider";

            // B
            var bModule = CreateUIObject("B" , rgba.transform , new Vector2(216 , 28));
            var bText = CreateUIObject("Text" , bModule.transform , new Vector2(18.3f , 28));
            bText.transform.localPosition = new Vector3(-94.6f , 0);
            bText.AddComponent<Text>().text = "B";
            bText.GetComponent<Text>().color = new Color(0.2f , 0.2f , 0.2f , 1);
            bText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            var bValue = DefaultControls.CreateInputField(convertToDefaultResources(resources));
            bValue.GetComponent<RectTransform>().sizeDelta = new Vector2(27.4f , 20);
            bValue.transform.SetParent(bModule.transform);
            bValue.transform.localPosition = new Vector3(94.3f , 0);
            bValue.name = "Value";
            bValue.GetComponent<InputField>().text = "255";
            bValue.transform.FindChild("Text").GetComponent<Text>().fontSize = 12;
            bValue.transform.FindChild("Text").GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            var bRect = bValue.transform.FindChild("Text").GetComponent<RectTransform>();
            bRect.sizeDelta = new Vector2(27.4f , 20);
            bRect.anchorMin = Vector2.zero;
            bRect.anchorMax = Vector2.one;
            var bSlider = createColoredTapeSlider(bModule.transform ,
                new Color[] { Color.yellow,Color.white });
            bSlider.name = "Slider";

            // A
            var aModule = CreateUIObject("A" , rgba.transform , new Vector2(216 , 28));
            var aText = CreateUIObject("Text" , aModule.transform , new Vector2(18.3f , 28));
            aText.transform.localPosition = new Vector3(-94.6f , 0);
            aText.AddComponent<Text>().text = "A";
            aText.GetComponent<Text>().color = new Color(0.2f , 0.2f , 0.2f , 1);
            aText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            var aValue = DefaultControls.CreateInputField(convertToDefaultResources(resources));
            aValue.GetComponent<RectTransform>().sizeDelta = new Vector2(27.4f , 20);
            aValue.transform.SetParent(aModule.transform);
            aValue.transform.localPosition = new Vector3(94.3f , 0);
            aValue.name = "Value";
            aValue.GetComponent<InputField>().text = "255";
            aValue.transform.FindChild("Text").GetComponent<Text>().fontSize = 12;
            aValue.transform.FindChild("Text").GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            var aRect = aValue.transform.FindChild("Text").GetComponent<RectTransform>();
            aRect.sizeDelta = new Vector2(27.4f , 20);
            aRect.anchorMin = Vector2.zero;
            aRect.anchorMax = Vector2.one;
            var aSlider = createColoredTapeSlider(aModule.transform ,
                new Color[] { Color.black,Color.white });
            aSlider.name = "Slider";

            #endregion

            #region 十六进制
            // 十六进制显示输入
            var hexColor = CreateUIObject("HexColor", colorPicker.transform, new Vector2(216, 25));
            hexColor.transform.localPosition = new Vector2(2 , -154.5f);
            // 创建文字
            var hexText = CreateUIObject("Text" , hexColor.transform , new Vector2(137.7f , 24));
            hexText.transform.localPosition = new Vector3(-36.8f , 0);
            var text = hexText.AddComponent<Text>();
            text.fontSize = 14;
            text.alignment = TextAnchor.MiddleLeft;
            text.text = "HexColor                 #";
            text.color = new Color(0.2f , 0.2f , 0.2f , 1);
            // 输入框
            var hexInput = DefaultControls.CreateInputField(convertToDefaultResources(resources));
            hexInput.name = "Value";
            hexInput.transform.SetParent(hexColor.transform);
            hexInput.GetComponent<RectTransform>().sizeDelta = new Vector2(76.9f , 25);
            hexInput.transform.localPosition = new Vector3(66.85f , 0);
            hexInput.GetComponent<InputField>().text = "FFFFFFFF";
            var textRect = hexInput.transform.FindChild("Text").GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(76.9f , 25);
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            #endregion

            #region 预制色模块
            // 预制色按钮
            var presets = CreateUIObject("Presets" , colorPicker.transform , new Vector2(220 , 40));
            presets.transform.localPosition = new Vector3(0 , -190.5f);
            var presetsText = CreateUIObject("Text" , presets.transform , new Vector2(154.4f , 20));
            presetsText.transform.localPosition = new Vector3(-27.3f , 10);
            presetsText.AddComponent<Text>().text = "Presets";
            presetsText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            presetsText.GetComponent<Text>().color = new Color(0.2f , 0.2f , 0.2f , 1);
            // 预制色Colors
            var colors = CreateUIObject("Colors" , presets.transform , new Vector2(214.5f , 12));
            colors.transform.localPosition = new Vector3(2.75f , -10);
            colors.AddComponent<GridLayoutGroup>().cellSize = new Vector2(12 , 12);
            colors.GetComponent<GridLayoutGroup>().spacing = new Vector2(0 , 1);
            // 预制色模板
            var colorTamplate = CreateUIObject("ColorItemTamplate" , colors.transform , new Vector2(12 , 12));
            colorTamplate.AddComponent<Image>();
            colorTamplate.AddComponent<Button>();
            colorTamplate.AddComponent<Outline>();
            colorTamplate.SetActive(false);
            // 添加按钮
            var addButton = DefaultControls.CreateButton(convertToDefaultResources(resources));
            addButton.name = "AddButton";
            addButton.transform.SetParent(colors.transform);
            //GameObject.Destroy(addButton.transform.FindChild("Text").gameObject);
            #endregion
            return colorPicker;
        }
        private static GameObject createColoredTapeSlider( Transform parent , Color[] colors )
        {
            //ColoredTape Slider 垂直滑动条
            var verticalSlider = CreateUIObject("Slider" , parent , new Vector2(164.55f , 16));
            verticalSlider.transform.localPosition = new Vector3(-3.1f,-1.2f);
            var vSlider = verticalSlider.AddComponent<Slider>();
            vSlider.direction = Slider.Direction.LeftToRight;
            vSlider.value = 1;
            // firstCT 
            var verticaCT = CreateUIObject("ColoredTape" , verticalSlider.transform , new Vector2(164.55f , 16));
            var vCT = verticaCT.AddComponent<ColoredTape>();
            vCT.TapeDirection = ColoredTape.E_DrawDirection.Horizontal;
            vCT.OuelineWidth = 0.8f;
            vCT.SetColors(colors);
            // handle slider area
            var handleArea = CreateUIObject("Handle Slider Area" , verticalSlider.transform , new Vector2(0 , 0));
            var handleAreaRect = handleArea.GetComponent<RectTransform>();
            handleAreaRect.anchorMin = Vector2.zero;
            handleAreaRect.anchorMax = Vector2.one;
            var handle = CreateUIObject("Handle" , handleArea.transform , new Vector2(20 , 16));
            var handleRect = handle.GetComponent<RectTransform>();
            handleRect.anchorMin = new Vector2(1,0);
            handleRect.anchorMax = Vector2.one;
            handle.transform.localPosition = new Vector3(82 , 0);
            handle.transform.localEulerAngles = new Vector3(0 , 0 , 90);
            var sh = handle.AddComponent<SliderHandler>();
            sh.arrowSize = 5;
            sh.distance = 8.58f;
            vSlider.targetGraphic = handle.GetComponent<SliderHandler>(); 
            vSlider.handleRect = handle.GetComponent<RectTransform>();
            return verticalSlider.gameObject;
        }

        /// <summary>
        /// Create Line Chart Graph
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        public static GameObject CreateLineChartGraph( Resources resources )
        {
            // line chart 
            GameObject lienChart = CreateUIElementRoot("LineChart" , new Vector2(425 , 200));

            // x axis unit
            GameObject xUnit = DefaultControls.CreateText(convertToDefaultResources(resources));
            xUnit.transform.SetParent(lienChart.transform);
            var xrect = xUnit.GetComponent<RectTransform>();
            xrect.pivot = new Vector2(1 , 0.5f);
            xUnit.transform.localPosition = new Vector3(-215,-100);

            // y axis unit 
            GameObject yUnit = DefaultControls.CreateText(convertToDefaultResources(resources));
            yUnit.transform.SetParent(lienChart.transform);
            var yrect = yUnit.GetComponent<RectTransform>();
            yrect.pivot = new Vector2(0.5f , 0f);
            yrect.transform.localPosition = new Vector3(-212.5f , 105);
            return lienChart;
        }

        /// <summary>
        /// Create Radar Map
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        public static GameObject CreateRadarMap( Resources resources )
        {
            GameObject radarmap = CreateUIElementRoot("RadarMap" , _defaultRadarMap);
            return radarmap;
        }
    }
}