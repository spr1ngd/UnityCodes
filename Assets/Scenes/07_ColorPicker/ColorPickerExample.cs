using UnityEngine;
using System.Collections;
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

    private void OnGUI()
    {
        GUILayout.Label("1.修改颜色拾取器的颜色即可修改image的颜色");
        GUILayout.Label("2.目前不支持HSV色彩模式");
        GUILayout.Label("3.监听办法看代码");
    }
}