using UnityEngine;
using System.Collections;
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
        if (GUILayout.Button("切换垂直色带的颜色"))
        {
            VerticalCT.SetColors(Colors);
        }

        if (GUILayout.Button("切换水平色带的颜色"))
        {
            HorizontalCT.SetColors(Colors);
        }

        if (GUILayout.Button("重置"))
        {
            VerticalCT.SetColors(Color);
            HorizontalCT.SetColors(Color);
        }
    }
}