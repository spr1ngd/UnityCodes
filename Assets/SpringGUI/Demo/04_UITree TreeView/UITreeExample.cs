
/*=========================================
* Author: springDong
* Description: SpringGUI.UITree example
==========================================*/

using UnityEngine;
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
                new UITreeData("high-level syntax",new List<UITreeData>()
                {
                    new UITreeData("Action",new List<UITreeData>()
                        {
                            new UITreeData("One parameter"),
                            new UITreeData("Two parameter"),
                            new UITreeData("Three parameter"),
                            new UITreeData("Four parameter"),
                            new UITreeData("Five parameter")
                        }),
                    new UITreeData("Func"),
                    new UITreeData("delegate")
                }),
                new UITreeData("Reflect")
            })
        });
        //UITree.SetData(data);
        UITree.Inject(data);
    }
}