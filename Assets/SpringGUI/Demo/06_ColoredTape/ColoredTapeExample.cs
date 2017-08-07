
/*=========================================
* Author: springDong
* Description: SpringGUI.ColoredTape example.
==========================================*/

using UnityEngine;
using SpringGUI;

public class ColoredTapeExample : MonoBehaviour
{
    private Color[] Colors = new Color[]
    {
        UnityEngine.Color.red,
        UnityEngine.Color.magenta,
        UnityEngine.Color.blue,
        UnityEngine.Color.cyan,
        UnityEngine.Color.green,
        UnityEngine.Color.yellow,
        UnityEngine.Color.red
    };

    private Color[] Color = new Color[]
    {
        UnityEngine.Color.red,
        UnityEngine.Color.green
    };

    public ColoredTape VerticalCT = null;
    public ColoredTape HorizontalCT = null;

    private void OnGUI()
    {
        if (GUILayout.Button("Change vertical colored tape's color"))
        {
            VerticalCT.SetColors(Colors);
        }

        if (GUILayout.Button("Change horizontal colored tape's color") )
        {
            HorizontalCT.SetColors(Colors);
        }

        if (GUILayout.Button("Reset"))
        {
            VerticalCT.SetColors(Color);
            HorizontalCT.SetColors(Color);
        }
    }
}