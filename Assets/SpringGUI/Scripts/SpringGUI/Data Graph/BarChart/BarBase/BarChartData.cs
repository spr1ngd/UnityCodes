
/*=========================================
* Author: springdong
* DateTime:2017/8/7 12:46:37
* Description: bar chart model
* 1.auto
==========================================*/

using System.Collections.Generic;
using UnityEngine;

namespace SpringGUI
{
    public class bar
    {
        public float value { get; set; }
        public Color color { get; set; }

        public bar(){}
        public bar(float value)
        {
            this.value = value;
        }
    }

    public class Bar
    {
        public float value { get; set; }
        public Color color { get; set; }
        public IList<bar> values = new List<bar>();

        public Bar(){}
        public Bar(float value)
        {
            this.value = value;
        }
        public Bar( IList<float> values )
        {
            foreach (float v in values)
                this.values.Add(new bar(v));
        }
    }

    public class Bars
    {
        public IList<Bar> bars = new List<Bar>();
        public Color color { get; set; }

        public Bars(){}
        public Bars( IList<Bar> bars )
        {
            this.bars = bars;
        }
        public Bars( IList<float> bars )
        {
            foreach (float v in bars)
                this.bars.Add(new Bar(v));
        }
        public Bars( IList<IList<float>> bars )
        {
            foreach (IList<float> barList in bars)
                this.bars.Add(new Bar(barList));
        }

        public void AddBar( Bar bar )
        {
            bars.Add(bar);
        }

        public void ReplaceBar( Bar oldBar,Bar newBar )
        {
            bars.Remove(oldBar);
            bars.Add(newBar);
        }

        public void RemoveBar( Bar bar )
        {
            bars.Remove(bar);
        }

        public void RemoveAll()
        {
            bars.Clear();
        }
    }

    public class BarChartData
    {
        private Dictionary<int, IList<Bars>> barDic = new Dictionary<int, IList<Bars>>();
        public Dictionary<int,IList<Bars>> BarDic 
        {
            get{ return barDic;}
        }

        public BarChartData(){}
        public BarChartData( Dictionary<int,IList<Bars>> barDic )
        {
            this.barDic = barDic;
        }

        public void AddBars( IList<Bars> bars )
        {
            barDic.Add(barDic.Count,bars);
        }
        public void AddBars( IList<Bars>[] bars )
        {
            foreach (var v in bars)
                AddBars(v);
        }

        public void ReplaceBars( int id ,IList<Bars> bars )
        {
            if (!barDic.ContainsKey(id))
            {
                Debug.LogWarning(string.Format("The key{0} you want to replace is not exist in the dictionary{2}",id,barDic.ToString()));
                return;
            }
            barDic[id] = bars;
        }

        public void RemoveBars( int id )  
        {
            if (!barDic.ContainsKey(id))
            {
                Debug.LogWarning(string.Format("The key{0} you want to remove is not exist in the dictionary{2}",id,barDic.ToString()));
                return;
            }
            barDic.Remove(id);
        }

        public void RemoveBars( int[] ids )
        {
            foreach (int id in ids)
                RemoveBars(id);
        }

        public void RemoveAll( )
        {
            barDic.Clear();
        }
    }
}