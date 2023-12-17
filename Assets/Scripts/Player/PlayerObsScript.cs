using UnityEngine;

public class PlayerObsScript : MonoBehaviour
{
    public Material material; // Bu PlayerObs nesnesinin materyali

    void Start()
    {
        // Materyali baþlat
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }

    public Color GetColor()
    {
        // Materyalin rengini döndür
        if (material != null)
        {
            return material.color;
        }
        else
        {
            return Color.clear; // Materyal atanmamýþsa, þeffaf bir renk döndür
        }
    }
}
