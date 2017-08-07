
/*=========================================
* Author: Administrator
* DateTime:2017/8/6 17:43:41
* Description:$safeprojectname$
==========================================*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SpringGUI
{
    public class BarChartFactory : sgDataGraphBase , IBarChartFactory
    {
        protected BarChartData Data = null;
        protected BarChartSetting Setting = null;
        protected Vector2 size = Vector2.zero;
        protected Vector2 origin = Vector2.zero;

        public VertexHelper DrawBarChart( VertexHelper vh  ,Rect rect , sgSettingBase baseSetting,BarChartSetting barChartSetting, BarChartData data )
        {
            BaseSetting = baseSetting;
            Setting = barChartSetting;
            this.size = rect.size;
            this.origin = new Vector2(-size.x / 2.0f , -size.y / 2.0f);
            Data = data;
            DrawMesh(vh,rect);
            return vh;
        }

        public override VertexHelper DrawMesh(VertexHelper vh, Rect rect )
        {             
            if (null == Data || null == Setting)
                return vh;            
            switch (Setting.BarChartType)
            {
                case BarChartType.Vertical:
                    DrawVertical(vh);
                    break;
                case BarChartType.Horizontal:
                    DrawHorizontal(vh);
                    break;
            }

            return base.DrawMesh(vh, rect);
        }

        private void DrawVertical( VertexHelper vh )
        {
            float halfWidth = Setting.BarWidth / 2.0f;
            Vector2 leftStart = origin + new Vector2(Setting.BarSpacing/2.0f,0);
            foreach( var bars in Data.BarDic )
            {
                foreach (var bar in bars.Value)
                {
                    Vector2 startPos = leftStart + new Vector2(halfWidth,0);                   
                    for (int m = 0; m < bar.bars.Count; m++)
                    { 
                        Color barColor = BarColor(m);
                        float height = bar.bars[m].value * size.y;
                        Vector2 endPos = startPos + new Vector2(0,height);
                        vh.AddUIVertexQuad(GetQuad(startPos,endPos,barColor,Setting.BarWidth));
                        startPos = startPos + new Vector2(Setting.BarInterval + Setting.BarWidth,0);
                    }
                    leftStart = leftStart + new Vector2(Setting.BarSpacing,0) + new Vector2(Setting.BarWidth * bar.bars.Count,0)
                        + new Vector2(Setting.BarInterval *( bar.bars.Count - 1),0);
                }
            } 
        }

        private void DrawHorizontal( VertexHelper vh )
        {
            float halfWidth = Setting.BarWidth / 2.0f;
            Vector2 leftStart = origin + new Vector2(0,Setting.BarSpacing/2.0f);
            foreach( var bars in Data.BarDic )
            {
                foreach (var bar in bars.Value)
                {
                    Vector2 startPos = leftStart + new Vector2(0,halfWidth);                   
                    for (int m = 0; m < bar.bars.Count; m++)
                    { 
                        Color barColor = BarColor(m);
                        float height = bar.bars[m].value * size.x;
                        Vector2 endPos = startPos + new Vector2(height,0);
                        vh.AddUIVertexQuad(GetQuad(startPos,endPos,barColor,Setting.BarWidth));
                        startPos = startPos + new Vector2(0,Setting.BarInterval + Setting.BarWidth);
                    }
                    leftStart = leftStart + new Vector2(0,Setting.BarSpacing) + new Vector2(0,Setting.BarWidth * bar.bars.Count)
                        + new Vector2(0,Setting.BarInterval *( bar.bars.Count- 1));
                }
            } 
        }

        private Color BarColor( int index = 0 )
        {
            Color result = Color.white;
            if (Setting.BarColors.Length.Equals(0))
            {
                Debug.LogWarning("Please set bar color properties on BarChart component's inspector panel.");
                return result;
            }
            switch (Setting.ColorStyle)
            {
                case ColorStyle.SingleNormal:
                    result = Setting.BarColors[0];
                    break;
                case ColorStyle.SingleColorful:
                    if (index >= Setting.BarColors.Length)
                    {
                        Debug.LogError("Please set bar color properties on BarChart component's inspector panel.");
                        return result;
                    }
                    result = Setting.BarColors[index];
                    break;
                case ColorStyle.MultiNormal:
                    if (index >= Setting.BarColors.Length)
                    {
                        Debug.LogError("Please set bar color properties on BarChart component's inspector panel.");
                        return result;
                    }
                    result = Setting.BarColors[0];
                    break;
                case ColorStyle.MultiColorufl:
                    result = Setting.BarColors[index];
                    break;
            }
            return result;
        }

        public void UnitEnable( GameObject xPrefab,GameObject yPrefab ,Rect rect,Transform parent,sgSettingBase baseSetting,BarChartSetting setting,BarChartData data)
        {
            Setting = setting;
            BaseSetting = baseSetting;
            Data = data;
            this.size = rect.size;
            this.origin = new Vector2(-size.x / 2.0f , -size.y / 2.0f);
            // x axis text
            Vector2 leftStart = origin + new Vector2(Setting.BarSpacing/2.0f,0);
            foreach (var bars in Data.BarDic)
            {
                for (int i = 0; i < bars.Value.Count; i++) // 4 bars 
                {
                    int count = bars.Value[i].bars.Count;
                    Vector2 pos = leftStart + new Vector2(Setting.BarWidth * count,0) /2.0f + new Vector2(Setting.BarInterval *( count - 1),0)/2.0f + new Vector2(0,-5);
                    GenerateUnit(xPrefab,parent,pos,BaseSetting.UnitXScale * i +BaseSetting.XUnit);
                   
                    leftStart = leftStart + new Vector2(Setting.BarSpacing,0) + new Vector2(Setting.BarWidth * count,0) + new Vector2(Setting.BarInterval *( count - 1),0);
                }
            }           

            // y axis text
            for( float i = 0 ,height = origin.y ; height <= size.y /2.0f ; i ++, height += BaseSetting.VerticalSpacing )
            {
                Vector2 pos = new Vector2(origin.x , height) + new Vector2(-5,0);
                GenerateUnit(yPrefab,parent,pos,BaseSetting.UnitYScale * i +BaseSetting.YUnit);
            }
        }

        private void GenerateUnit( GameObject prefab , Transform parent, Vector3 position , string unitContent = "" )
        {
            GameObject unit = GameObject.Instantiate(prefab);
            unit.SetActive(true);
            unit.transform.SetParent(parent);
            unit.transform.localPosition = position;
            unit.GetComponent<Text>().text = unitContent;
            unit.transform.localScale = Vector3.one;
        }
    }
}