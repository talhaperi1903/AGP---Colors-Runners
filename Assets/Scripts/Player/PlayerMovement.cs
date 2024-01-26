using HyperCasual.Runner;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IBodyManager
{
    void GrowBody(GameObject bodyPrefab, Transform playerTransform);
    void ShrinkBody();
    void ResetBody();
    int BodyCount { get; }
    void UpdateBodyCountText(Text bodyCountText);
}

public interface IColorManager
{
    void ChangeColor(Material playerMaterial, Color newColor, List<GameObject> bodyParts);
}

public interface IInteractionHandler
{
    void HandleFinishArea(ref bool isFinished);
    void HandleBankLockArea(ref bool isBankLock, PlayerController playerController);
    // Update the method signature to include the additional parameters
    void HandlePlayerObs(GameObject other, Material playerMaterial, IBodyManager bodyManager, GameObject bodyPrefab, Transform playerTransform);
}


public class BodyManager : IBodyManager
{
    private List<GameObject> bodyParts = new List<GameObject>();
    private int bodyCount = 0;

    public int BodyCount => bodyCount;

    public void GrowBody(GameObject bodyPrefab, Transform playerTransform)
    {
        var body = GameObject.Instantiate(bodyPrefab, playerTransform.position, playerTransform.rotation);
        bodyParts.Add(body);
        bodyCount++;
    }

    public void ShrinkBody()
    {
        if (bodyParts.Count > 0)
        {
            GameObject lastBodyPart = bodyParts[bodyParts.Count - 1];
            GameObject.Destroy(lastBodyPart);
            bodyParts.RemoveAt(bodyParts.Count - 1);
            bodyCount--;
        }
    }

    public void ResetBody()
    {
        foreach (var body in bodyParts)
        {
            GameObject.Destroy(body);
        }
        bodyParts.Clear();
        bodyCount = 0;
    }

    public void UpdateBodyCountText(Text bodyCountText)
    {
        if (bodyCountText != null)
        {
            bodyCountText.text = bodyCount.ToString();
        }
    }
}

public class ColorManager : IColorManager
{
    public void ChangeColor(Material playerMaterial, Color newColor, List<GameObject> bodyParts)
    {
        if (playerMaterial != null)
        {
            playerMaterial.color = newColor;
        }

        foreach (var part in bodyParts)
        {
            var renderer = part.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = newColor;
            }
        }
    }
}

public class InteractionHandler : IInteractionHandler
{
    public void HandleFinishArea(ref bool isFinished)
    {
        isFinished = true;
        // Additional logic for when the player reaches the finish area
    }

    public void HandleBankLockArea(ref bool isBankLock, PlayerController playerController)
    {
        isBankLock = true;
        playerController.enabled = false;
        // Additional logic for when the player reaches the bank lock area
    }

    public void HandlePlayerObs(GameObject other, Material playerMaterial, IBodyManager bodyManager, GameObject bodyPrefab, Transform playerTransform)
    {
        var playerObsScript = other.GetComponent<PlayerObsScript>();
        if (playerObsScript != null && playerMaterial.color == playerObsScript.GetColor())
        {
            GameObject.Destroy(other, 0.005f); // Destroy the obstacle
            bodyManager.GrowBody(bodyPrefab, playerTransform); // Grow the player's body with the provided prefab and transform
        }
        else
        {
            bodyManager.ShrinkBody(); // Shrink the player's body
        }
    }

}

public class PlayerMovement : MonoBehaviour
{
    public Text bodyCountText;
    public GameObject bodyPrefab;
    public Material playerMaterial;

    private IBodyManager bodyManager;
    private IColorManager colorManager;
    private IInteractionHandler interactionHandler;


    private bool isFinished = false;
    private bool isBankLock = false;

    public GameObject finishArea;
    public GameObject bankLockArea;
    private PlayerController playerController;

    public class BuildingLockManager : IBuildingLockManager
    {
        private Material buildingMaterial;
        private Color lockedColor;
        private Color unlockedColor;
        private Text lockStatusText;
        private int lockValue;

        public BuildingLockManager(Material buildingMaterial, Color lockedColor, Color unlockedColor, Text lockStatusText, int lockValue)
        {
            this.buildingMaterial = buildingMaterial;
            this.lockedColor = lockedColor;
            this.unlockedColor = unlockedColor;
            this.lockStatusText = lockStatusText;
            this.lockValue = lockValue;
        }

        public void UnlockBuilding(int unlockValue)
        {
          
        }

        public void UpdateLockStatusText()
        {
            
        }
    }

    public void ChangeColor(Color newColor)
    {
        // Change the player's color
        // You might need to access the Renderer component or a specific Material to apply the new color
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = newColor;
        }
    }
    void Awake()
    {
        bodyManager = new BodyManager();
        colorManager = new ColorManager();
        interactionHandler = new InteractionHandler();
        playerController = GetComponent<PlayerController>();
    }

    void Start()
    {
        bodyManager.ResetBody();
    }

    void Update()
    {
        if (isFinished)
        {
            // Handle game logic for when the player has finished
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == finishArea)
        {
            interactionHandler.HandleFinishArea(ref isFinished);
        }
        else if (other.gameObject == bankLockArea)
        {
            interactionHandler.HandleBankLockArea(ref isBankLock, playerController);
        }
        else if (other.tag == "PlayerObs")
        {
            interactionHandler.HandlePlayerObs(other.gameObject, playerMaterial, bodyManager, bodyPrefab, transform);

        }
    }

    void LateUpdate()
    {
        bodyManager.UpdateBodyCountText(bodyCountText);
    }
}

