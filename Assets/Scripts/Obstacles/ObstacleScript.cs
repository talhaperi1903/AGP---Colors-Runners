using UnityEngine;

public interface IObstacleInteractionHandler
{
    bool CompareColors(Material playerMaterial, Material obstacleMaterial);
    void ResetPlayerBody();
}
public class PlayerObstacleInteractionHandler : IObstacleInteractionHandler
{
    private PlayerMovement playerMovement;

    public PlayerObstacleInteractionHandler(PlayerMovement playerMovement)
    {
        this.playerMovement = playerMovement;
    }

    public bool CompareColors(Material playerMaterial, Material obstacleMaterial)
    {
        return playerMaterial.color == obstacleMaterial.color;
    }

    private IBodyManager bodyManager;

    public PlayerObstacleInteractionHandler(IBodyManager bodyManager)
    {
        this.bodyManager = bodyManager;
    }

    public void ResetPlayerBody()
    {
        bodyManager.ResetBody();
    }
}
public class ObstacleScript : MonoBehaviour
{
    public Renderer obstacleRenderer1; // Assign the renderer of the first plane in the Inspector
    public Animator animator; // Assign the animator in the Inspector

    public IObstacleInteractionHandler obstacleInteractionHandler;

    private void Start()
    {
        animator.enabled = false;

        // Example of setting the handler, adjust according to your project setup
        var playerMovement = FindObjectOfType<PlayerMovement>(); // Find the PlayerMovement component in the scene
        if (playerMovement != null)
        {
            obstacleInteractionHandler = new PlayerObstacleInteractionHandler(playerMovement);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (obstacleInteractionHandler != null)
            {
                Material playerMaterial = other.GetComponent<Renderer>()?.material; // Assuming the player's Renderer has the material
                Material obstacleMaterial1 = obstacleRenderer1.material;

                // Compare materials
                if (obstacleInteractionHandler.CompareColors(playerMaterial, obstacleMaterial1))
                {
                    animator.enabled = true;
                }
                else
                {
                    // If the player and obstacle are of different colors, shrink the player's body
                    obstacleInteractionHandler.ResetPlayerBody();
                }
            }
        }
    }
}
