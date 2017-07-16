
using UnityEngine;
using UnityEngine.UI;

public class Program : MonoBehaviour
{
    public Image image = null;

    public void SetColor( Color color )
    {
        image.color = color;
    }
}