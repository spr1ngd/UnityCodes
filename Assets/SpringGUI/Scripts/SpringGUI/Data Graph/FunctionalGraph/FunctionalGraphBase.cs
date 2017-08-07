
/*=========================================
* Author: springDong
* Description: 
* functional formula graph base class
* The appearance of the components control by this class.
==========================================*/

using System;
using UnityEngine;

namespace SpringGUI
{
    [Serializable]
    public class FunctionalGraphBase
    {
        [Header("XY Axis Setting")]
        [Tooltip("Show XYAxisUnit")]
        public bool ShowXYAxisUnit = true;
        // X axis unit 
        public string XAxisUnit = "XUnit";
        // Y axis unit
        public string YAxisUnit = "YUnit";
        // XY axis unit text font size 
        [Range(12 , 30)]
        public int UnitFontSize = 16;
        // XY axis unit text color
        public Color UnitFontColor = Color.black;
        // XY axis line width 
        [Range(2f , 20f)]
        public float XYAxisWidth = 2.0f;
        // XY axis line color
        public Color XYAxisColor = Color.gray;

        [Header("Scale Setting")]
        [Tooltip("var isScale")]
        public bool ShowScale = false;
        [Range(20f , 100f)]
        public float ScaleValue = 50f;
        [Tooltip("Scale lenght")]
        [Range(2 , 10)]
        public float ScaleLenght = 5.0f;

        // background mesh type 
        public enum E_MeshType
        {
            None,
            FullLine,
            ImaglinaryLine
        }
        [Header("Background Mesh Setting")]
        public E_MeshType MeshType = E_MeshType.None;
        [Range(1.0f , 10f)]
        public float MeshLineWidth = 2.0f;
        public Color MeshColor = Color.gray;
        [Range(0.5f , 20)]
        public float ImaglinaryLineWidth = 8.0f;
        [Range(0.5f , 10f)]
        public float SpaceingWidth = 5.0f;
    }

    [Serializable]
    public class FunctionFormula
    {
        //  formulas 
        public Func<float , float> Formula;
        public Color FormulaColor;
        public float FormulaWidth;

        public FunctionFormula( ) { }
        public FunctionFormula( Func<float , float> formula , Color formulaColor , float width )
        {
            Formula = formula;
            FormulaColor = formulaColor;
            FormulaWidth = width;
        }

        // get formula yvalue by xvalue
        public Vector2 GetResult( float xValue , float scaleValue )
        {
            return new Vector2(xValue , Formula(xValue / scaleValue) * scaleValue);
        }
    }
}