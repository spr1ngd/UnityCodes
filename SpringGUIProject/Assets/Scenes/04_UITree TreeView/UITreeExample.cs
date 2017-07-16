using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SpringGUI;

public class UITreeExample : MonoBehaviour
{
    public UITree UITree = null;

    public void Awake()
    {
        var data = new UITreeData("SpringGUI",new List<UITreeData>()
        {
            new UITreeData("Button",new List<UITreeData>()
            {
                new UITreeData("DoubleClickButton"),
                new UITreeData("LongClickButton")
            }),
            new UITreeData("Pie"),
            new UITreeData("DatePicker"),
            new UITreeData("C#",new List<UITreeData>()
            {
                new UITreeData("委托",new List<UITreeData>()
                {
                    new UITreeData("Action"),
                    new UITreeData("Func"),
                    new UITreeData("delegate")
                }),
                new UITreeData("反射")
            })
        });
        UITree.SetData(data);
    }
}