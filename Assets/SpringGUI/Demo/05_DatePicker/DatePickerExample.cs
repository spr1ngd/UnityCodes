
/*=========================================
* Author: springDong
* Description: SpringGUI.Calendar example.
==========================================*/

using UnityEngine;
using SpringGUI;

public class DatePickerExample : MonoBehaviour
{
    public DatePicker DatePicker = null;
    public Calendar Calendar = null;

    public void Awake()
    {
        Calendar.onDayClick.AddListener(time =>
        {
            Debug.Log(string.Format("Today is {0}Yeah{1}Month{2}Day" ,
                time.Year , time.Month , time.Day));
        });
        Calendar.onMonthClick.AddListener(time =>
        {
            Debug.Log(string.Format("This month is {0}Yeah{1}Month" ,
            time.Year , time.Month));
        });
        Calendar.onYearClick.AddListener(time =>
        {
            Debug.Log(string.Format("This yeah{0}Yeah" , time.Year));
        });
    }
}