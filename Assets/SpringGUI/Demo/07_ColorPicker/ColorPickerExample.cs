
/*=========================================
* Author: springDong
* Description: SpringGUI.Calendar example.
==========================================*/

using UnityEngine;
using SpringGUI;
using UnityEngine.UI;

public class ColorPickerExample : MonoBehaviour
{
    public ColorPicker ColorPicker = null;
    public Image Image = null;

    private void Awake()
    {
        ColorPicker.onPicker.AddListener(color => { Image.color = color; });
    }
}