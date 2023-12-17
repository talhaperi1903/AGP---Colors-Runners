using HyperCasual.Runner;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public GameObject buildingModel; // Bina modelini belirtin
    public Material buildingMaterial; // Bina modelinin malzemesini belirtin
    public Color lockedColor = Color.black; // Kilitli renk
    public Color unlockedColor = Color.white; // Kilitsiz renk
    public int lockValue = 50; // Kilit de�eri
    private PlayerMovement player; // Player referans�n�z
    public GameObject plane; // Plane referans�n�z
    private PlayerController playerController; // PlayerController referans�n�z


    void Start()
    {
        playerController = GetComponent<PlayerController>();

        // Player'� al�n
        player = FindObjectOfType<PlayerMovement>();
        // Bina modelinin rengini siyahla�t�r�n
        buildingMaterial.color = lockedColor;
    }

    void Update()
    {
        // Player plane'in �zerine geldi�inde
        if (player.transform.position == plane.transform.position)
        {
            Debug.Log("Player plane �zerinde!");

            // Player'�n h�z�n� s�f�rla
            playerController.CancelMovement();
            // Player'�n body count birer birer azalarak 0'a kadar getir
            while (player.bodyCount > 0)
            {
                player.ShrinkBody();
                lockValue -= 1;
            }

            // Kilit say�s� ile body count'u oranlay�p bina modelinin rengini eski hale getir
            float ratio = (float)player.bodyCount / lockValue;
            buildingMaterial.color = Color.Lerp(lockedColor, unlockedColor, ratio);

            // Kilit ile bodycount ayn� say� olursa t�m rengi g�z�kmesini sa�la
            if (lockValue == player.bodyCount)
            {
                buildingMaterial.color = unlockedColor;
            }
        }
    }
}
