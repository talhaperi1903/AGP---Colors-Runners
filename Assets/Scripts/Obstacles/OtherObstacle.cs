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
            // Player'ýn Renderer'ýný alýn, playerMaterial'i almak için
            Material playerMaterial = other.GetComponent<Renderer>()?.material;

            if (playerMaterial != null)
            {
                Material obstacleMaterial1 = obstacleRenderer1.material;

                // Malzemeleri karþýlaþtýr
                if (!obstacleInteractionHandler.CompareColors(playerMaterial, obstacleMaterial1))
                {
                    // Engelle ve oyuncu farklý renkte, oyuncunun vücudu küçültülecek
                    obstacleInteractionHandler.ResetPlayerBody();
                }
            }
        }
    }
}

