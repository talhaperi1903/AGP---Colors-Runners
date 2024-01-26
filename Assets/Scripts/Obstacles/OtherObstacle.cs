using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherObstacle : MonoBehaviour
{
    public Renderer obstacleRenderer1; // Assign the renderer of the first plane in the Inspector
    public IObstacleInteractionHandler obstacleInteractionHandler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player'�n Renderer'�n� al�n, playerMaterial'i almak i�in
            Material playerMaterial = other.GetComponent<Renderer>()?.material;

            if (playerMaterial != null)
            {
                Material obstacleMaterial1 = obstacleRenderer1.material;

                // Malzemeleri kar��la�t�r
                if (!obstacleInteractionHandler.CompareColors(playerMaterial, obstacleMaterial1))
                {
                    // Engelle ve oyuncu farkl� renkte, oyuncunun v�cudu k���lt�lecek
                    obstacleInteractionHandler.ResetPlayerBody();
                }
            }
        }
    }
}

