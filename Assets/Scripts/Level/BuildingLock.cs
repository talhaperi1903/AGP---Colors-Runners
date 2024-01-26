using HyperCasual.Runner;
using UnityEngine;
using UnityEngine.UI;
using static PlayerMovement;

public interface IBodyManagerbl
{
    int BodyCount { get; }
    void ShrinkBody();
    void UpdateBodyCountText();
}

public interface IBuildingLockManager
{
    void UnlockBuilding(int unlockValue);
    void UpdateLockStatusText();
}

public class BuildingLock : MonoBehaviour
{
    public Material buildingMaterial;
    public Color lockedColor = Color.black;
    public Color unlockedColor = Color.white;
    public Text lockStatusText;
    public int lockValue = 10;

    private IBuildingLockManager buildingLockManager;
    private IBodyManagerbl bodyManager;

    void Start()
    {
        bodyManager = FindObjectOfType<PlayerMovement>() as IBodyManagerbl;
        buildingLockManager = new BuildingLockManager(buildingMaterial, lockedColor, unlockedColor, lockStatusText, lockValue);

        buildingLockManager.UpdateLockStatusText();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int unlockValue = bodyManager.BodyCount; // Oyuncunun vücut sayýsýný kullanarak kilidi açma deðerini alýn
            buildingLockManager.UnlockBuilding(unlockValue);
            bodyManager.UpdateBodyCountText();
            buildingLockManager.UpdateLockStatusText();
        }
    }
}
