using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [Tooltip("The material to change the color of")]
    public Material material;

    [Tooltip("The new color for the material")]
    public Color newColor;

    private void Start()
    {
        ChangeColor();
    }

    public void ChangeColor()
    {
        material.color = newColor;
    }
}