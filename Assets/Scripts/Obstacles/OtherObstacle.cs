using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherObstacle : MonoBehaviour
{
    public Renderer obstacleRenderer1; // Ýnspektörde birinci düzlemin renderer'ýný ata

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

            if (playerMovement != null && playerMovement.playerMaterial != null)
            {
                // Oyuncu ve engel düzlemlerinin malzemelerini al
                Material playerMaterial = playerMovement.playerMaterial;
                Material obstacleMaterial1 = obstacleRenderer1.material;

                // Malzemeleri karþýlaþtýr
                if (playerMaterial.color == obstacleMaterial1.color)
                {

                }
                else
                {
                    // Engelle ve oyuncu farklý renkte, oyuncunun vücudu küçültülecek
                    playerMovement.ResetBody();

                }
            }

        }

    }
}
