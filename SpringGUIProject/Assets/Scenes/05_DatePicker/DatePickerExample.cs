using UnityEngine;
using SpringGUI;

public class DatePickerExample : MonoBehaviour
{
    public DatePicker DatePicker = null;
    public Calendar Calendar = null;

    public void Awake()
    {
        Debug.Log("当前时间 : " + DatePicker.DateTime);
        Calendar.onDayClick.AddListener(time => { Debug.Log(string.Format("今天是{0}年{1}月{2}日" , time.Year , time.Month , time.Day)); });
        Calendar.onMonthClick.AddListener(time => { Debug.Log(string.Format("本月是{0}年{1}月" , time.Year , time.Month)); });
        Calendar.onYearClick.AddListener(time => { Debug.Log(string.Format("今年是{0}年" , time.Year)); });
    }
}