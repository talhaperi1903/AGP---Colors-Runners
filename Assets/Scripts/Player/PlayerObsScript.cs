using UnityEngine;

// Interface to abstract the functionality for material handling
public interface IMaterialService
{
    void ApplyMaterial(Renderer renderer, Material material);
    Color GetMaterialColor(Material material);
}

// Concrete implementation of the material service
public class MaterialService : IMaterialService
{
    public void ApplyMaterial(Renderer renderer, Material material)
    {
        if (renderer != null && material != null)
        {
            renderer.material = material;
        }
    }

    public Color GetMaterialColor(Material material)
    {
        return material != null ? material.color : Color.clear;
    }
}

public class PlayerObsScript : MonoBehaviour
{
    public Material material; // This material should be set in the Unity Editor.

    private IMaterialService _materialService;

    void Awake()
    {
        // Dependency injection (could also be done via a constructor or a DI framework)
        _materialService = new MaterialService();
    }

    void Start()
    {
        // Apply material using the material service
        Renderer renderer = GetComponent<Renderer>();
        _materialService.ApplyMaterial(renderer, material);
    }

    public Color GetColor()
    {
        // Retrieve material color using the material service
        return _materialService.GetMaterialColor(material);
    }
}
