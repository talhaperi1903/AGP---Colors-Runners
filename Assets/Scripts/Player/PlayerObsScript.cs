using UnityEngine;

public class PlayerObsScript : MonoBehaviour
{
    public Material material; // Bu PlayerObs nesnesinin materyali

    void Start()
    {
        // Materyali ba�lat
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }

    public Color GetColor()
    {
        // Materyalin rengini d�nd�r
        if (material != null)
        {
            return material.color;
        }
        else
        {
            return Color.clear; // Materyal atanmam��sa, �effaf bir renk d�nd�r
        }
    }
}
