using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DoubleLongClickButton : MonoBehaviour
{
    public Button DoubleClickButton = null;
    public Button LongClickButton = null;

    public void Start()
    {
        DoubleClickButton.onClick.AddListener(() =>
        {
            Debug.Log("Double click button");
        });
        LongClickButton.onClick.AddListener(() =>
        {
            Debug.Log("Long click button");
        });
    }
}