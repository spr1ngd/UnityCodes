
/*=========================================
* Author: SpringDong
* DateTime : 2017/6/9 17:34:59
* Email : 540637360@qq.com
* Description : 颜色拾取器V0.7.69 此版本暂不包含颜色模式的切换只有RGB模式，HSV模式预留
==========================================*/

using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace SpringGUI
{
    public class ColorPicker : UIBehaviour
    {
        #region private members 私有成员
        #region 常量
        private const string BRIGHTNESS = "Brightness";
        private const string SATURATION = "Saturatoin";
        private const string HUE = "Hue";
        private const string H = "H";
        private const string S = "S";
        private const string V = "V";
        private const string R = "R";
        private const string G = "G";
        private const string B = "B";
        private const string RED = "Red";
        private const string GREEN = "Green";
        private const string BLUE = "Blue";
        private readonly Color[] SEVENCOLOR = new Color[] { Color.red , Color.magenta , Color.blue , Color.cyan , Color.green , Color.yellow , Color.red , };
        #endregion

        #region 枚举变量
        //显示模式
        public enum E_PaletteMode : int
        {
            Hue = 0,
            Saturation = 1,
            Brightness = 2,
            Red = 3,
            Green = 4,
            Blue = 5
        }
        private E_PaletteMode PaletteMode = E_PaletteMode.Hue;
        //色彩模式
        public enum E_ColorMode : int
        {
            RGB = 0,
            //色调（H），饱和度（S），明度（V）
            HSV = 1,
        }
        private E_ColorMode ColorMode = E_ColorMode.RGB;
        //工作状态
        internal enum E_WorkState : int
        {
            Normal = 0 ,
            Sucker = 1 ,
        }
        private E_WorkState m_workState = E_WorkState.Normal;
        private E_WorkState WorkState
        {
            get { return m_workState; }
            set
            {
                m_workState = value;
                switch (value)
                {
                    case E_WorkState.Normal:
                        m_suckScreen.gameObject.SetActive(false);
                        m_colorPalette.gameObject.SetActive(true);
                        m_coloredPate.gameObject.SetActive(true);
                        break;
                    case E_WorkState.Sucker:
                        m_suckScreen.gameObject.SetActive(true);
                        m_colorPalette.gameObject.SetActive(false);
                        m_coloredPate.gameObject.SetActive(false);
                        break;
                }
            }
        }
        #endregion

        #region base field 记录Transform和用于隐藏的游戏物体
        private Transform m_transform = null;
        private Transform m_suckScreen = null;
        private Transform m_colorPalette = null;
        private ColorPalette m_colorPaletteScript = null;
        private Transform m_coloredPate = null;
        #endregion

        #region MainColorTape && ColoredTape && Texts
        //Brightness && Saturation && Hue Text
        private Text m_leftText = null;
        private Text m_rightText = null;
        private Text m_bottomText = null;
        //Brightness && Saturation && Hue ColoredTape
        private MainColorTape m_mainColorTape = null;
        private ColoredTape m_firstLayerCT = null;
        private ColoredTape m_secondLayerCT = null;
        private MultiColoredTape m_multiCT = null;

        private Slider m_verticalCTSlider = null;
        private ColoredTape m_verticalFirstCT = null;
        //private ColoredTape m_verticalSecondCT = null;
        #endregion

        #region 吸管功能
        private Texture2D m_texture = null;
        private Image m_screenImage = null;
        private ImageMesh m_imageMesh = null;
        #endregion

        #region RGBAColorTape 基础色 
        private Text m_rText = null;
        private InputField m_rValue = null;
        private Slider m_rSlider = null;
        private ColoredTape m_rColoredTape = null;

        private Text m_gText = null;
        private InputField m_gValue = null;
        private Slider m_gSlider = null;
        private ColoredTape m_gColoredTape = null;

        private Text m_bText = null;
        private InputField m_bValue = null;
        private Slider m_bSlider = null;
        private ColoredTape m_bColoredTape = null;

        private Text m_aText = null;
        private InputField m_aValue = null;
        private Slider m_aSlider = null;
        private ColoredTape m_aColoredTape = null;
        #endregion

        #region Hex Color 十六进制
        private InputField m_hexColor = null;
        #endregion

        #region Preset Color 预制色
        private Transform m_presetParent = null;
        private GameObject m_colorItemTamplate = null;
        private Image m_presetAddButton = null;
        #endregion

        #region 游标

        private Transform m_nonius = null;

        #endregion
        #endregion
        //当前选中的颜色
        [SerializeField]
        private Color m_Color = default(Color);
        public Color Color
        {
            get { return m_Color; }
            set
            {
                m_Color = new Color(value.r,value.g,value.b,value.a);
                Reset();
            }
        }

        #region UIBehaviour functions
        protected override void Start()
        {
            //todo 不同模块从这个方法分离出去 
            m_transform = this.transform;
            m_mainColorTape = m_transform.FindChild("MainColor").GetComponent<MainColorTape>();

            #region  RGBA ColoredTape RGBA组件获取与事件监听
            var RGBA = m_transform.FindChild("RGBA");

            m_rText = RGBA.transform.FindChild("R/Text").GetComponent<Text>();
            m_rValue = RGBA.FindChild("R/Value").GetComponent<InputField>();
            m_rSlider = RGBA.FindChild("R/Slider").GetComponent<Slider>();
            m_rSlider.onValueChanged.AddListener(OnRedSliderChanged);
            m_rColoredTape = m_rSlider.transform.FindChild("ColoredTape").GetComponent<ColoredTape>();
            m_rValue.onValueChanged.AddListener(SetColorbyR);

            m_gText = RGBA.transform.FindChild("G/Text").GetComponent<Text>();
            m_gValue = RGBA.FindChild("G/Value").GetComponent<InputField>();
            m_gSlider = RGBA.FindChild("G/Slider").GetComponent<Slider>();
            m_gSlider.onValueChanged.AddListener(OnGreenSliderChanged);
            m_gColoredTape = m_gSlider.transform.FindChild("ColoredTape").GetComponent<ColoredTape>();
            m_gValue.onValueChanged.AddListener(SetColorbyG);

            m_bText = RGBA.transform.FindChild("B/Text").GetComponent<Text>();
            m_bValue = RGBA.FindChild("B/Value").GetComponent<InputField>();
            m_bSlider = RGBA.FindChild("B/Slider").GetComponent<Slider>();
            m_bSlider.onValueChanged.AddListener(OnBlueSliderChanged);
            m_bColoredTape = m_bSlider.transform.FindChild("ColoredTape").GetComponent<ColoredTape>();
            m_bValue.onValueChanged.AddListener(SetColorbyB);

            m_aText = RGBA.transform.FindChild("A/Text").GetComponent<Text>();
            m_aValue = RGBA.FindChild("A/Value").GetComponent<InputField>();
            m_aSlider = RGBA.FindChild("A/Slider").GetComponent<Slider>();
            m_aSlider.onValueChanged.AddListener(OnAlphaSliderChanged);
            m_aColoredTape = m_aSlider.transform.FindChild("ColoredTape").GetComponent<ColoredTape>();
            m_aValue.onValueChanged.AddListener(SetColorbyA);
            #endregion

            m_transform.FindChild("ColorSucker").GetComponent<Button>().onClick.AddListener(() => { WorkState = E_WorkState.Sucker; });
            m_transform.FindChild("PaletteModeButton").GetComponent<Button>().onClick.AddListener(ChangePaletteMode);
            //m_transform.FindChild("ColorModeButton").GetComponent<Button>().onClick.AddListener(ChangeColorMode);

            // Suck Color
            m_suckScreen = m_transform.FindChild("SuckScreen");
            m_screenImage = m_suckScreen.FindChild("Texture").GetComponent<Image>();
            m_imageMesh = m_suckScreen.FindChild("Mesh").GetComponent<ImageMesh>();

            // 调色板
            m_colorPalette = m_transform.FindChild("ColorPalette");
            m_colorPaletteScript = m_colorPalette.GetComponent<ColorPalette>();
            m_nonius = m_colorPalette.FindChild("ColorNonius");
            m_firstLayerCT = m_colorPalette.FindChild("FirstLayerColoredTape").GetComponent<ColoredTape>();
            m_secondLayerCT = m_colorPalette.FindChild("SecondLayerColoredTape").GetComponent<ColoredTape>();
            m_multiCT = m_colorPalette.FindChild("MultiColoredTape").GetComponent<MultiColoredTape>();
            m_leftText = m_colorPalette.FindChild("LeftText").GetComponent<Text>();
            m_bottomText = m_colorPalette.FindChild("BottomText").GetComponent<Text>();

            // 垂直滑动条
            m_coloredPate = m_transform.FindChild("ColoredTapeSlider");
            m_verticalCTSlider = m_coloredPate.GetComponent<Slider>();
            m_verticalFirstCT = m_coloredPate.FindChild("FirstLayerColoredTape").GetComponent<ColoredTape>();
            //m_verticalSecondCT = m_coloredPate.FindChild("SecondLayerColoredTape").GetComponent<ColoredTape>();
            m_rightText = m_verticalCTSlider.transform.FindChild("RightText").GetComponent<Text>();
            m_verticalCTSlider.onValueChanged.AddListener(verticalSliderChanged);

            // 预制颜色
            m_presetParent = m_transform.FindChild("Presets/Colors");
            m_colorItemTamplate = m_presetParent.FindChild("ColorItemTamplate").gameObject;
            m_presetAddButton =  m_presetParent.FindChild("AddButton").GetComponent<Image>();
            m_presetParent.FindChild("AddButton").GetComponent<Button>().onClick.AddListener(AddPresetColor);

            // 初始化操作
            InitHexColor();
            Color = Color.white;
            Reset();
        }
        protected virtual void Update()
        {   
            //吸取颜色
            if (WorkState == E_WorkState.Sucker)
            {
                StartCoroutine(ScreenShot());
                if ( Input.GetMouseButtonDown(0) )
                {
                    Color = m_screenImage.sprite.texture.GetPixel(m_imageMesh.XAxisCount / 2 + 1 , m_imageMesh.YAxisCount / 2 + 1);
                    SetNoniusPositionByColor();
                    WorkState = E_WorkState.Normal;
                }
            }
        }
        protected override void Reset()
        {
            m_presetAddButton.color = Color;
            SetHexByColor();
            m_mainColorTape.Color = Color;
            switch (ColorMode)
            {
                case E_ColorMode.RGB:
                    SetRGBA();
                    break;
                case E_ColorMode.HSV:
                    break;
            }
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            m_transform.FindChild("ColorSucker").GetComponent<Button>().onClick.RemoveAllListeners();
            m_transform.FindChild("PaletteModeButton").GetComponent<Button>().onClick.RemoveAllListeners();
            m_transform.FindChild("ColorModeButton").GetComponent<Button>().onClick.RemoveAllListeners();
            m_presetParent.FindChild("AddButton").GetComponent<Button>().onClick.RemoveAllListeners();
            m_rValue.onValueChanged.RemoveAllListeners();
            m_gValue.onValueChanged.RemoveAllListeners();
            m_bValue.onValueChanged.RemoveAllListeners();
            m_aValue.onValueChanged.RemoveAllListeners();
        }
        #endregion

        #region 垂直色带 和 调色板

        /// <summary>
        /// 通过垂直滑动条修改调色板
        /// </summary>
        /// <param name="value"></param>
        private void verticalSliderChanged( float value )
        {
            float height = m_verticalFirstCT.transform.GetComponent<RectTransform>().sizeDelta.y;
            switch (ColorMode)
            {
                case E_ColorMode.RGB:
                    switch ( PaletteMode )
                    {
                        case E_PaletteMode.Hue:
                            Color color = m_verticalFirstCT.GetColor(new Vector2(0 , height * ( 1 - value ) - height / 2.0f));
                            changedFirstLayerColoredTape(color);
                            Color = mixedTwoColoredTapeColor(m_nonius.localPosition);
                            break;
                        case E_PaletteMode.Saturation:
                            m_secondLayerCT.SetColors(new Color[]
                            {
                                new Color(0, 0, 0, 0) * (1 - value) + new Color(1, 1, 1, 1) * (value) ,
                                new Color(0,0,0,1), 
                            });
                            Color = mixedTwoColoredTapeColor(m_nonius.localPosition);
                            break;
                        case E_PaletteMode.Brightness:
                            m_secondLayerCT.SetColors(new Color[]
                            {
                                new Color(1,1,1,0) * (1-value) + new Color(0,0,0,1) * (value),
                                new Color(1,1,1,1) * (1-value)  + new Color(0,0,0,1) * (value)
                            });
                            Color = mixedTwoColoredTapeColor(m_nonius.localPosition);
                            break;
                        case E_PaletteMode.Red:
                            var redTL = m_multiCT.TopLeft;
                            var redTR = m_multiCT.TopRight;
                            var redBL = m_multiCT.BottomLeft;
                            var redBR = m_multiCT.BottomRight;
                            redTL.r = 1 - value;
                            redTR.r = 1 - value;
                            redBL.r = 1 - value;
                            redBR.r = 1 - value;
                            m_multiCT.SetColors(new Color[] { redTL , redTR , redBL , redBR });
                            setRedSliderValue(1 - value);
                            var temp1 = m_multiCT.GetColor(m_nonius.localPosition);
                            Color = new Color(1 - value , temp1.g , temp1.b , Color.a);
                            break;
                        case E_PaletteMode.Green:
                            var greenTL = m_multiCT.TopLeft;
                            var greenTR = m_multiCT.TopRight;
                            var greenBL = m_multiCT.BottomLeft;
                            var greenBR = m_multiCT.BottomRight;
                            greenTL.g = 1 - value;
                            greenTR.g = 1 - value;
                            greenBL.g = 1 - value;
                            greenBR.g = 1 - value;
                            m_multiCT.SetColors(new Color[] { greenTL , greenTR , greenBL , greenBR });
                            setGreenSliderValue(1 - value);
                            var temp2 = m_multiCT.GetColor(m_nonius.localPosition);
                            Color = new Color(temp2.r , 1 - value , temp2.b , Color.a);
                            break;
                        case E_PaletteMode.Blue:
                            var blueTL = m_multiCT.TopLeft;
                            var blueTR = m_multiCT.TopRight;
                            var blueBL = m_multiCT.BottomLeft;
                            var blueBR = m_multiCT.BottomRight;
                            blueTL.b = 1 - value;
                            blueTR.b = 1 - value;
                            blueBL.b = 1 - value;
                            blueBR.b = 1 - value;
                            m_multiCT.SetColors(new Color[] { blueTL , blueTR , blueBL , blueBR });
                            setBlueSliderValue(1 - value);
                            var temp3 = m_multiCT.GetColor(m_nonius.localPosition);
                            Color = new Color(temp3.r , temp3.g , 1 - value , Color.a);
                            break;
                    }
                    break;
                case E_ColorMode.HSV:
                    switch (PaletteMode)
                    {
                        case E_PaletteMode.Hue:
                            //todo
                            break;
                        case E_PaletteMode.Saturation:
                            //todo 
                            break;
                        case E_PaletteMode.Brightness:
                            //todo 
                            break;
                        case E_PaletteMode.Red:
                            //todo 
                            break;
                        case E_PaletteMode.Green:
                            //todo 
                            break;
                        case E_PaletteMode.Blue:
                            //todo 
                            break;
                    }
                    break;
            }
        }

        //todo 核心算法分析出堆叠颜色的值
        private Vector2 getPositionByMixedCT( Color color0 )
        {
            var result = Vector2.zero;
            var size = m_colorPalette.GetComponent<RectTransform>().sizeDelta;
            switch (PaletteMode)
            {
                case E_PaletteMode.Hue:
                    break;
                case E_PaletteMode.Saturation:
                    var x1 = m_firstLayerCT.GetPosition(color0).x;
                    var red1 = color0.r;
                    var green1 = color0.g;
                    var blue1 = color0.b;
                    ArrayList array1 = new ArrayList() { red1 , green1 , blue1 };
                    array1.Sort();
                    Color mainColor1;
                    if(array1[0].Equals(red1))
                        mainColor1 = new Color(0 , green1 , blue1 , 1);
                    else if(array1[0].Equals(green1))
                        mainColor1 = new Color(red1,0,blue1,1);
                    else
                        mainColor1 = new Color(red1,green1,0,1);
                    m_verticalFirstCT.SetColors(new Color[] { mainColor1 , Color.white });
                    m_secondLayerCT.SetColors(new Color[]
                    {
                        new Color(0, 0, 0, 0) * (1 - (float)array1[0]) + new Color(1, 1, 1, 1) * ((float)array1[0]) ,
                        Color.black
                    });
                    setValueForVerticalSlider((float)array1[0]);
                    float y1 = (1- (float)array1[0] ) * size.y - size.y/2.0f;
                    result = new Vector2(x1,y1);
                    break;
                case E_PaletteMode.Brightness:
                    var x2 = m_firstLayerCT.GetPosition(color0).x;
                    var red2 = color0.r;
                    var green2 = color0.g;
                    var blue2 = color0.b;
                    ArrayList array2 = new ArrayList() { red2 , green2 , blue2 };
                    array2.Sort();
                    Color mainColor2;
                    if ( array2[0].Equals(red2) )
                        mainColor2 = new Color(0 , green2 , blue2 , 1);
                    else if ( array2[0].Equals(green2) )
                        mainColor2 = new Color(red2 , 0 , blue2 , 1);
                    else
                        mainColor2 = new Color(red2 , green2 , 0 , 1);
                    m_verticalFirstCT.SetColors(new Color[] { mainColor2 , Color.black });
                    m_secondLayerCT.SetColors(new Color[] { new Color(1 , 1 , 1 , (float)array2[0]) , Color.white });
                    setValueForVerticalSlider((float)array2[0]);
                    float y2 = ( 1 - (float)array2[0] ) * size.y - size.y / 2.0f;
                    result = new Vector2(x2 , y2);
                    break;
            }
            return result;
        }

        /// <summary>
        /// 给垂直滑动条设置颜色
        /// </summary>
        /// <param name="value"></param>
        private void setValueForVerticalSlider( float value )
        {
            m_verticalCTSlider.onValueChanged.RemoveAllListeners();
            m_verticalCTSlider.value = value;
            m_verticalCTSlider.onValueChanged.AddListener(verticalSliderChanged);
        }

        /// <summary>
        /// 修改叠加模式中第一层的颜色
        /// </summary>
        /// <param name="color"></param>
        private void changedFirstLayerColoredTape( Color color )
        {
            m_firstLayerCT.SetColors(new Color[] { Color.white , color });
        }
        
        /// <summary>
        /// 获取叠加色带的颜色值
        /// </summary>
        /// <param name="noniusPos"></param>
        /// <returns></returns>
        private Color mixedTwoColoredTapeColor( Vector2 noniusPos )
        {
            Color first = m_firstLayerCT.GetColor(noniusPos);
            Color second = m_secondLayerCT.GetColor(noniusPos);
            float alpha = second.a;
            Color result = Color.white;
            switch ( PaletteMode )
            {
                case E_PaletteMode.Hue:
                    result = new Color(second.r , second.g , second.b) * alpha +
                        new Color(first.r , first.g , first.b) * ( 1 - alpha );
                    result.a = Color.a;
                    break;
                case E_PaletteMode.Saturation:
                    result = new Color(second.r , second.g , second.b) * alpha +
                        new Color(first.r , first.g , first.b) * ( 1 - alpha );
                    result.a = Color.a;
                    break;
                case E_PaletteMode.Brightness:
                    result = new Color(second.r , second.g , second.b) * ( alpha ) +
                        new Color(first.r , first.g , first.b) * ( 1 - alpha );
                    result.a = Color.a;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 获取多颜色混合色带的颜色
        /// </summary>
        /// <param name="noniusPos"></param>
        /// <returns></returns>
        private Color getMultiColoredTapeColor( Vector2 noniusPos )
        {
            return m_multiCT.GetColor(noniusPos);
        }

        #endregion

        #region Operate Color Palette Mode && ColorMode  改变色板模式和颜色模式

        /// <summary>
        /// 更改调色板模式
        /// </summary>
        private void ChangePaletteMode()
        {
            PaletteMode = (int)++PaletteMode > 5 ? 0 : PaletteMode++;
            switch (PaletteMode)
            {
                case E_PaletteMode.Hue:
                    m_firstLayerCT.gameObject.SetActive(true);
                    m_secondLayerCT.gameObject.SetActive(true);
                    m_multiCT.gameObject.SetActive(false);
                    m_firstLayerCT.SetColors(new Color[] { Color.white , Color.red });
                    m_secondLayerCT.SetColors(new Color[] { new Color(0 , 0 , 0 , 0) , Color.black });
                    m_verticalFirstCT.SetColors(SEVENCOLOR);
                    m_leftText.text = BRIGHTNESS;
                    m_rightText.text = HUE;
                    m_bottomText.text = SATURATION;
                    m_firstLayerCT.SetColors(new Color[] { Color.white , new Color(Color.r , Color.g , Color.b , 1) });
                    Vector2 size = m_colorPalette.GetComponent<RectTransform>().sizeDelta;
                    m_nonius.localPosition = new Vector3(size.x / 2.0f , size.y / 2.0f);
                    break;
                case E_PaletteMode.Saturation:
                    m_firstLayerCT.SetColors(SEVENCOLOR);
                    m_secondLayerCT.SetColors(new Color[] { new Color(0 , 0 , 0 , 0) , Color.black });
                    m_verticalFirstCT.SetColors(new Color[] { Color.red , Color.white });
                    m_leftText.text = BRIGHTNESS;
                    m_rightText.text = SATURATION;
                    m_bottomText.text = HUE;
                    m_nonius.localPosition = getPositionByMixedCT(Color);
                    break;
                case E_PaletteMode.Brightness:
                    m_firstLayerCT.SetColors(SEVENCOLOR);
                    m_secondLayerCT.SetColors(new Color[] { new Color(1 , 1 , 1 , 0) , Color.white });
                    m_verticalFirstCT.SetColors(new Color[] { Color.red , Color.black });
                    m_leftText.text = SATURATION;
                    m_rightText.text = BRIGHTNESS;
                    m_bottomText.text = HUE;
                    m_nonius.localPosition = getPositionByMixedCT(Color);
                    break;
                case E_PaletteMode.Red:
                    m_firstLayerCT.gameObject.SetActive(false);
                    m_secondLayerCT.gameObject.SetActive(false);
                    m_multiCT.gameObject.SetActive(true);
                    m_verticalFirstCT.SetColors(new Color[] { new Color(1 , Color.g , Color.b) , new Color(0 , Color.g , Color.b) });
                    m_multiCT.SetColors(new Color[] { new Color(Color.r , 1 , 0) , new Color(Color.r , 1 , 1) , new Color(Color.r , 0 , 0) , new Color(Color.r , 0 , 1) , });
                    m_leftText.text = GREEN;
                    m_bottomText.text = BLUE;
                    m_rightText.text = RED;
                    m_nonius.localPosition = m_multiCT.GetPosition(Color , E_PaletteMode.Red);
                    setValueForVerticalSlider(m_verticalFirstCT.GetScale(Color));
                    break;
                case E_PaletteMode.Green:
                    m_verticalFirstCT.SetColors(new Color[] { new Color(Color.r , 1 , Color.b) , new Color(Color.r , 0 , Color.b) });
                    m_multiCT.SetColors(new Color[] { new Color(1 , Color.g , 0) , new Color(1 , Color.g , 1) , new Color(0 , Color.g , 0) , new Color(0 , Color.g , 1) , });
                    m_leftText.text = RED;
                    m_bottomText.text = BLUE;
                    m_rightText.text = GREEN;
                    m_nonius.localPosition = m_multiCT.GetPosition(Color , E_PaletteMode.Green);
                    setValueForVerticalSlider(m_verticalFirstCT.GetScale(Color));
                    break;
                case E_PaletteMode.Blue:
                    m_verticalFirstCT.SetColors(new Color[] { new Color(Color.r , Color.g , 1) , new Color(Color.r , Color.g , 0) });
                    m_multiCT.SetColors(new Color[] { new Color(0 , 1 , Color.b) , new Color(1 , 1 , Color.b) , new Color(0 , 0 , Color.b) , new Color(1 , 0 , Color.b) , });
                    m_leftText.text = GREEN;
                    m_bottomText.text = RED;
                    m_rightText.text = BLUE;
                    m_nonius.localPosition = m_multiCT.GetPosition(Color , E_PaletteMode.Blue);
                    setValueForVerticalSlider(m_verticalFirstCT.GetScale(Color));
                    break;
            }
        }
        /// <summary>
        /// 更改颜色模式
        /// </summary>
        private void ChangeColorMode()
        {
            ColorMode = (int)++ColorMode > 1 ? 0 : ColorMode++;
            switch (ColorMode)
            {
                case E_ColorMode.RGB:
                    m_rText.text = R;
                    m_gText.text = G;
                    m_bText.text = B;
                    SetRGBA();
                    break;
                case E_ColorMode.HSV:
                    m_rText.text = H;
                    m_gText.text = S;
                    m_bText.text = V;
                    m_rColoredTape.SetColors(SEVENCOLOR);
                    var red = Color.r;
                    var green = Color.g;
                    var blue = Color.b;
                    var maxValue = red;
                    if (blue > maxValue)
                        maxValue = blue;
                    if (green > maxValue)
                        maxValue = green;
                    if (maxValue.Equals(red))
                    {
                        m_gColoredTape.SetColors(new Color[] { Color.white , Color.red });
                        m_bColoredTape.SetColors(new Color[] { Color.black , Color.red });
                    }
                    else if (maxValue.Equals(green))
                    {
                        m_gColoredTape.SetColors(new Color[] { Color.white , Color.green });
                        m_bColoredTape.SetColors(new Color[] { Color.black , Color.green });
                    }
                    else
                    {
                        m_gColoredTape.SetColors(new Color[] { Color.white , Color.blue });
                        m_bColoredTape.SetColors(new Color[] { Color.black , Color.blue });
                    }
                    break;
            }
        }

        #endregion
        
        /// <summary>
        /// 设置RGBA输入框的值
        /// </summary>
        private void SetRGBA()
        {
            var red = Color.r;
            var green = Color.g;
            var blue = Color.b;
            m_rValue.text = ( (int)( red * 255 ) ).ToString();
            m_gValue.text = ( (int)( green * 255 ) ).ToString();
            m_bValue.text = ( (int)( blue * 255 ) ).ToString();
            m_aValue.text = ( (int)( Color.a * 255 ) ).ToString();
            //set red 
            var startColor = new Color(0 , green , blue , 1);
            var endColor = new Color(1 , green , blue , 1);
            m_rColoredTape.SetColors(new Color[] { startColor , endColor });
            //set green
            startColor = new Color(red , 0 , blue , 1);
            endColor = new Color(red , 1 , blue , 1);
            m_gColoredTape.SetColors(new Color[] { startColor , endColor });
            //set blue
            startColor = new Color(red , green , 0 , 1);
            endColor = new Color(red , green , 1 , 1);
            m_bColoredTape.SetColors(new Color[] { startColor , endColor });
        }

        /// <summary>
        /// R inputfiled 监听方法
        /// </summary>
        /// <param name="red"></param>
        private void SetColorbyR( string red )
        {
            var value = 0;
            if (!int.TryParse(red, out value))
            {
                m_rValue.text = "0";
                return;
            }
            setRedSliderValue(value / 255.0f);
        }
        private void setRedSliderValue( float value )
        {
            m_rSlider.onValueChanged.RemoveAllListeners();
            m_rSlider.value = value ;
            m_rSlider.onValueChanged.AddListener(OnRedSliderChanged);
        }

        /// <summary>
        /// G inputfiled 监听方法
        /// </summary>
        /// <param name="green"></param>
        private void SetColorbyG( string green )
        {
            var value = 0;
            if (!int.TryParse(green, out value))
            {
                m_gValue.text = "0";
                return;
            }
            setGreenSliderValue(value / 255.0f);
        }
        private void setGreenSliderValue( float value )
        {
            m_gSlider.onValueChanged.RemoveAllListeners();
            m_gSlider.value = value ;
            m_gSlider.onValueChanged.AddListener(OnGreenSliderChanged);
        }

        /// <summary>
        /// blue inputfiled 监听方法
        /// </summary>
        /// <param name="blue"></param>
        private void SetColorbyB( string blue )
        {
            var value = 0;
            if (!int.TryParse(blue, out value))
            {
                m_bValue.text = "0";
                return;
            }
            setBlueSliderValue(value / 255.0f);
        }
        private void setBlueSliderValue( float value )
        {
            m_bSlider.onValueChanged.RemoveAllListeners();
            m_bSlider.value = value;
            m_bSlider.onValueChanged.AddListener(OnBlueSliderChanged);
        }
        /// <summary>
        /// alpha inputfiled 监听方法 
        /// </summary>
        /// <param name="alpha"></param>
        private void SetColorbyA( string alpha )
        {
            var value = 0;
            if (!int.TryParse(alpha, out value))
            {
                m_aValue.text = "0";
                return;
            }
            m_aSlider.onValueChanged.RemoveAllListeners();
            m_aSlider.value = value / 255.0f;
            m_aSlider.onValueChanged.AddListener(OnAlphaSliderChanged);
        }

        /// <summary>
        /// 通过red slider 改变颜色
        /// </summary>
        /// <param name="value"></param>
        private void OnRedSliderChanged( float value )
        {
            switch (ColorMode)
            {
                case E_ColorMode.RGB:
                    Color = new Color(value , Color.g , Color.b , Color.a);
                    switch ( PaletteMode )
                    {
                        case E_PaletteMode.Hue:
                            changedFirstLayerColoredTape(Color);
                            setValueForVerticalSlider(m_verticalFirstCT.GetScale(Color));
                            break;
                        case E_PaletteMode.Saturation:
                            m_nonius.localPosition = getPositionByMixedCT(Color);
                            //todo 
                            break;
                        case E_PaletteMode.Brightness:
                            m_nonius.localPosition = getPositionByMixedCT(Color);
                            //todo 
                            break;
                        case E_PaletteMode.Red:
                            m_multiCT.SetColors(value , E_PaletteMode.Red);
                            setValueForVerticalSlider(1 - value);
                            break;
                        case E_PaletteMode.Green:
                            m_nonius.localPosition = m_multiCT.GetPosition(Color , E_PaletteMode.Green);
                            m_verticalFirstCT.SetColors(value , E_PaletteMode.Red);
                            break;
                        case E_PaletteMode.Blue:
                            m_nonius.localPosition = m_multiCT.GetPosition(Color , E_PaletteMode.Blue);
                            m_verticalFirstCT.SetColors(value , E_PaletteMode.Red);
                            break;
                    }
                    break;
                case E_ColorMode.HSV:
                    switch ( PaletteMode )
                    {
                        case E_PaletteMode.Hue:
                            //todo 
                            break;
                        case E_PaletteMode.Saturation:
                            //todo 
                            break;
                        case E_PaletteMode.Brightness:
                            //todo 
                            break;
                        case E_PaletteMode.Red:
                            //todo 
                            break;
                        case E_PaletteMode.Green:
                            //todo 
                            break;
                        case E_PaletteMode.Blue:
                            //todo 
                            break;
                    }
                    break;
            }
        }
        /// <summary>
        /// 通过green slider 改变颜色
        /// </summary>
        /// <param name="value"></param>
        private void OnGreenSliderChanged( float value )
        {
            switch ( ColorMode )
            {
                case E_ColorMode.RGB:
                    Color = new Color(Color.r , value , Color.b , Color.a);
                    switch ( PaletteMode )
                    {
                        case E_PaletteMode.Hue:
                            changedFirstLayerColoredTape(Color);
                            setValueForVerticalSlider(m_verticalFirstCT.GetScale(Color));
                            break;
                        case E_PaletteMode.Saturation:
                            m_nonius.localPosition = getPositionByMixedCT(Color);
                            //todo 
                            break;
                        case E_PaletteMode.Brightness:
                            m_nonius.localPosition = getPositionByMixedCT(Color);
                            //todo 
                            break;
                        case E_PaletteMode.Red:
                            m_nonius.localPosition = m_multiCT.GetPosition(Color , E_PaletteMode.Red);
                            m_verticalFirstCT.SetColors(value , E_PaletteMode.Green);
                            break;
                        case E_PaletteMode.Green:
                            m_multiCT.SetColors(value , E_PaletteMode.Green);
                            setValueForVerticalSlider(value);
                            break;
                        case E_PaletteMode.Blue:
                            m_nonius.localPosition = m_multiCT.GetPosition(Color , E_PaletteMode.Blue);
                            m_verticalFirstCT.SetColors(value , E_PaletteMode.Green);
                            break;
                    }
                    break;
                case E_ColorMode.HSV:
                    switch ( PaletteMode )
                    {
                        case E_PaletteMode.Hue:
                            //todo
                            break;
                        case E_PaletteMode.Saturation:
                          
                            //todo 
                            break;
                        case E_PaletteMode.Brightness:
                            //todo 
                            break;
                        case E_PaletteMode.Red:
                            //todo 
                            break;
                        case E_PaletteMode.Green:
                            //todo 
                            break;
                        case E_PaletteMode.Blue:
                            //todo 
                            break;
                    }
                    break;
            }
        }
        /// <summary>
        /// 通过 blue slider 改变颜色
        /// </summary>
        /// <param name="value"></param>
        private void OnBlueSliderChanged( float value )
        {
            switch ( ColorMode )
            {
                case E_ColorMode.RGB:
                    Color = new Color(Color.r , Color.g , value , Color.a);
                    switch ( PaletteMode )
                    {
                        case E_PaletteMode.Hue:
                            changedFirstLayerColoredTape(Color);
                            setValueForVerticalSlider(m_verticalFirstCT.GetScale(Color));
                            break;
                        case E_PaletteMode.Saturation:
                            m_nonius.localPosition = getPositionByMixedCT(Color);
                            //todo 
                            break;
                        case E_PaletteMode.Brightness:
                            m_nonius.localPosition = getPositionByMixedCT(Color);
                            //todo 
                            break;
                        case E_PaletteMode.Red:
                            m_nonius.localPosition = m_multiCT.GetPosition(Color , E_PaletteMode.Red);
                            m_verticalFirstCT.SetColors(value , E_PaletteMode.Blue);
                            break;
                        case E_PaletteMode.Green:
                            m_nonius.localPosition = m_multiCT.GetPosition(Color , E_PaletteMode.Green);
                            m_verticalFirstCT.SetColors(value , E_PaletteMode.Blue);
                            break;
                        case E_PaletteMode.Blue:
                            m_multiCT.SetColors(value , E_PaletteMode.Blue);
                            setValueForVerticalSlider(value);
                            break;
                    }
                    break;
                case E_ColorMode.HSV:
                    switch ( PaletteMode )
                    {
                        case E_PaletteMode.Hue:
                            //todo 
                            break;
                        case E_PaletteMode.Saturation:
                            //todo 
                            break;
                        case E_PaletteMode.Brightness:
                            //todo 
                            break;
                        case E_PaletteMode.Red:
                            //todo 
                            break;
                        case E_PaletteMode.Green:
                            //todo 
                            break;
                        case E_PaletteMode.Blue:
                            //todo 
                            break;
                    }
                    break;
            }
        }
        /// <summary>
        /// 通过 aplga slider 改变颜色
        /// </summary>
        /// <param name="value"></param>
        private void OnAlphaSliderChanged( float value )
        {
            Color = new Color(Color.r , Color.g , Color.b , value);
        }

        #region 十六进制显示

        /// <summary>
        /// 初始化十六进制
        /// </summary>
        private void InitHexColor( )
        {
            m_hexColor = m_transform.FindChild("HexColor/Value").GetComponent<InputField>();
            m_hexColor.onValueChanged.AddListener(SetColorByHex);
        }
        /// <summary>
        /// 通过十六进制Inputfield设置颜色
        /// </summary>
        /// <param name="hexColor"></param>
        private void SetColorByHex( string hexColor )
        {
            //hexColor = LimitInputField(hexColor);
            //float temp = 16 / 255.0f;
            //if (hexColor.Length.Equals(3))
            //{
            //    float red = (float)hexColor[0] * temp;
            //    float green = (float)hexColor[1] * temp;
            //    float blue = (float)hexColor[2] * temp;
            //    if (red > 1)
            //        red = 1;
            //    if (green > 1)
            //        green = 1;
            //    if (blue > 1)
            //        blue = 1;
            //    m_Color = new Color(red,green,blue,1);
            //    m_presetAddButton.color = Color;
            //    m_mainColorTape.Color = Color;
            //    switch ( ColorMode )
            //    {
            //        case E_ColorMode.RGB:
            //            SetRGBA();
            //            break;
            //        case E_ColorMode.HSV:
            //            break;
            //    }
            //    SetNoniusPositionByColor();
            //}
        }
        /// <summary>
        /// 设置十六进制显示
        /// </summary>
        private void SetHexByColor()
        {
            string hexValue = "";
            hexValue += DecimalToHexadecimal((int)( Color.r * 255 ));
            hexValue += DecimalToHexadecimal((int)( Color.g * 255 ));
            hexValue += DecimalToHexadecimal((int)( Color.b * 255 ));
            hexValue += DecimalToHexadecimal((int)( Color.a * 255 ));
            m_hexColor.text = hexValue;
        }
        private string DecimalToHexadecimal( int dec )
        {
            string result = Convert.ToString(dec , 16);
            result = result.ToUpper();
            if ( result.Length.Equals(1) )
                result = "0" + result;
            return result;
        }

        private List<char> matchChars = new List<char> { '0' , '1' , '2' , '3' , '4' , '5' , '6' , '7' , '8' , '9' , 'A' , 'B' , 'C' , 'D' , 'E' , 'F' };
        /// <summary>
        /// 限制颜色值的输入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string LimitInputField( string input )
        {
            var result = input;
            if (!matchChars.Contains(result[result.Length - 1]))
                return result.Remove(result.Length - 1);
            return result;
        }

        #endregion
        
        #region 颜色吸取预览图

        /// <summary>
        /// 范围截屏，数据内存流
        /// </summary>
        /// <returns></returns>
        private IEnumerator ScreenShot( )
        {
            var xCount = m_imageMesh.XAxisCount;
            var yCount = m_imageMesh.YAxisCount;
            m_texture = new Texture2D(xCount , yCount , TextureFormat.RGB24 , false);
            yield return new WaitForEndOfFrame();
            m_texture.ReadPixels(new Rect((int)Input.mousePosition.x - (int)( xCount / 2 ) ,
                (int)Input.mousePosition.y - (int)( yCount / 2 ) , xCount , yCount) , 0 , 0);
            m_texture.Apply();
            m_screenImage.sprite = Sprite.Create(m_texture , new Rect(0 , 0 , xCount , yCount) , Vector2.zero);
        }

        /// <summary>
        /// 通过游标吸取颜色
        /// </summary>
        /// <param name="noniusPos"></param>
        public void SuckColorByNonius( Vector3 noniusPos ) 
        {
            switch (PaletteMode)
            {
                case E_PaletteMode.Hue:
                    Color = mixedTwoColoredTapeColor(noniusPos);
                    m_mainColorTape.Color = Color;
                    break;
                case E_PaletteMode.Saturation:
                    Color = mixedTwoColoredTapeColor(noniusPos);
                    var main1 = m_firstLayerCT.GetColor(noniusPos);
                    m_mainColorTape.Color = Color;
                    m_verticalFirstCT.SetColors(new Color[] { main1 , Color.white});
                    break;
                case E_PaletteMode.Brightness:
                    Color = mixedTwoColoredTapeColor(noniusPos);
                    var main2 = m_firstLayerCT.GetColor(noniusPos);
                    m_mainColorTape.Color = Color;
                    m_verticalFirstCT.SetColors(new Color[] { main2 , Color.black });
                    break;
                case E_PaletteMode.Red:
                    var color11 = getMultiColoredTapeColor(noniusPos);
                    Color = new Color(color11.r , color11.g , color11.b , Color.a);
                    m_mainColorTape.Color = Color;
                    m_verticalFirstCT.SetColors(new Color[] { new Color(1 , Color.g , Color.b , 1) , new Color(0 , Color.g , Color.b , 1) });
                    break;
                case E_PaletteMode.Green:
                    var color22 = getMultiColoredTapeColor(noniusPos);
                    Color = new Color(color22.r , color22.g , color22.b , Color.a);
                    m_mainColorTape.Color = Color;
                    m_verticalFirstCT.SetColors(new Color[] { new Color(Color.r , 1 , Color.b , 1) , new Color(Color.r , 0 , Color.b , 1) });
                    break;
                case E_PaletteMode.Blue:
                    var color3 = getMultiColoredTapeColor(noniusPos);
                    Color = new Color(color3.r , color3.g , color3.b , Color.a);
                    m_mainColorTape.Color = Color;
                    m_verticalFirstCT.SetColors(new Color[] { new Color(Color.r , Color.g , 1 , 1) , new Color(Color.r , Color.g , 0 , 1) });
                    break;
            }
        }

        /// <summary>
        /// 通过吸取的颜色给游标定位
        /// </summary>
        private void SetNoniusPositionByColor()
        {
            switch ( PaletteMode )
            {
                case E_PaletteMode.Hue:
                    changedFirstLayerColoredTape(Color);
                    m_nonius.localPosition = m_transform.GetComponent<RectTransform>().sizeDelta;
                    setValueForVerticalSlider(m_verticalFirstCT.GetScale(Color));
                    break;
                case E_PaletteMode.Saturation:
                    m_nonius.localPosition = getPositionByMixedCT(Color);
                    break;
                case E_PaletteMode.Brightness:
                    m_nonius.localPosition = getPositionByMixedCT(Color);
                    break;
                case E_PaletteMode.Red:
                    m_nonius.localPosition = m_multiCT.GetPosition(Color , E_PaletteMode.Red);
                    m_verticalCTSlider.value = 1 -Color.r;
                    break;
                case E_PaletteMode.Green:
                    m_nonius.localPosition = m_multiCT.GetPosition(Color , E_PaletteMode.Green);
                    m_verticalCTSlider.value = 1 - Color.g;
                    break;
                case E_PaletteMode.Blue:
                    m_nonius.localPosition = m_multiCT.GetPosition(Color , E_PaletteMode.Blue);
                    m_verticalCTSlider.value = 1 - Color.b;
                    break;
            }
        }
        
        #endregion
         
        #region Add preset color 添加预制色

        private void AddPresetColor( )
        {
            GameObject newPreset = GameObject.Instantiate(m_colorItemTamplate);
            newPreset.SetActive(true);
            newPreset.GetComponent<Image>().color = Color;
            newPreset.transform.SetParent(m_presetParent);
            newPreset.transform.SetSiblingIndex(m_presetParent.childCount - 2);
            newPreset.GetComponent<Button>().onClick.AddListener(() =>
            {
                Color = newPreset.GetComponent<Image>().color;
                SetNoniusPositionByColor();
                switch (PaletteMode)
                {
                    case E_PaletteMode.Hue:

                        break;
                    case E_PaletteMode.Saturation:

                        break;
                    case E_PaletteMode.Brightness:

                        break;
                    case E_PaletteMode.Red:

                        break;
                    case E_PaletteMode.Green:

                        break;
                    case E_PaletteMode.Blue:

                        break;
                }
            });
        }

        #endregion
    }
}