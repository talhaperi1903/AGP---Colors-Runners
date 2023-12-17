using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherObstacle : MonoBehaviour
{
    public Renderer obstacleRenderer1; // �nspekt�rde birinci d�zlemin renderer'�n� ata

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

            if (playerMovement != null && playerMovement.playerMaterial != null)
            {
                // Oyuncu ve engel d�zlemlerinin malzemelerini al
                Material playerMaterial = playerMovement.playerMaterial;
                Material obstacleMaterial1 = obstacleRenderer1.material;

                // Malzemeleri kar��la�t�r
                if (playerMaterial.color == obstacleMaterial1.color)
                {

                }
                else
                {
                    // Engelle ve oyuncu farkl� renkte, oyuncunun v�cudu k���lt�lecek
                    playerMovement.ResetBody();

                }
            }

        }

    }
}
