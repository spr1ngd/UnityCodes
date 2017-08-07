
//SpringGUI.

using SpringGUI;
using UnityEngine;
using UnityEngine.UI;

public class DoubleLongClickButton : MonoBehaviour
{
    public DoubleClickButton DoubleClickButton = null;
    public LongClickButton LongClickButton = null;

    public void Start()
    {
        DoubleClickButton.onDoubleClick.AddListener(() =>
        {
            Debug.Log("Double click button");
        });
        LongClickButton.onLongClick.AddListener(() =>
        {
            Debug.Log("Long click button");
        });
    }
}