
/*=========================================
* Author: Administrator
* DateTime:2017/7/25 14:11:38
* Description:$safeprojectname$
==========================================*/

using System.Collections.Generic;

namespace SpringGUI
{
    public class RadarData
    {
        public int id{get;set;}
        public List<float> keypoints = new List<float>();

        public RadarData(){}

        public RadarData( int id,float[] keypoints )
        {
            this.id = id;
            foreach (var point in keypoints)
                AddData(point);
        }

        public RadarData( float[] keypoints )
        {
            foreach (var point in keypoints)
                AddData(point);
        }

        public void AddData( float keypoint )
        {
            keypoints.Add(keypoint);
        }
    }

    public class RadarDatas
    {
        public Dictionary<int,RadarData> datas = new Dictionary<int, RadarData>();

        public void AddRadarData( int id, RadarData data )
        {
            if (datas.ContainsKey(id))
                return;
            datas.Add(id,data);
        }

        public void AddRadarData( RadarData data )
        {
            datas.Add(datas.Count,data);
        }

        public void AddRadarData( IList<RadarData> datas )
        {
            foreach (var data in datas)
                AddRadarData(data);
        }

        public void UpdateRadarData( int id, RadarData data )
        {
            if (!datas.ContainsKey(id))
                return;
            datas[id] = data;
        }

        public void RomoveRadarData( int id )
        {
            if (!datas.ContainsKey(id))
                return;
            datas.Remove(id);
        }

        public void RemoveAll()
        {
            datas.Clear();
        }
    }
}