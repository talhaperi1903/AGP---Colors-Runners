using HyperCasual.Runner;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public GameObject buildingModel; // Bina modelini belirtin
    public Material buildingMaterial; // Bina modelinin malzemesini belirtin
    public Color lockedColor = Color.black; // Kilitli renk
    public Color unlockedColor = Color.white; // Kilitsiz renk
    public int lockValue = 50; // Kilit deðeri
    private PlayerMovement player; // Player referansýnýz
    public GameObject plane; // Plane referansýnýz
    private PlayerController playerController; // PlayerController referansýnýz


    void Start()
    {
        playerController = GetComponent<PlayerController>();

        // Player'ý alýn
        player = FindObjectOfType<PlayerMovement>();
        // Bina modelinin rengini siyahlaþtýrýn
        buildingMaterial.color = lockedColor;
    }

    void Update()
    {
        // Player plane'in üzerine geldiðinde
        if (player.transform.position == plane.transform.position)
        {
            Debug.Log("Player plane üzerinde!");

            // Player'ýn hýzýný sýfýrla
            playerController.CancelMovement();
            // Player'ýn body count birer birer azalarak 0'a kadar getir
            while (player.bodyCount > 0)
            {
                player.ShrinkBody();
                lockValue -= 1;
            }

            // Kilit sayýsý ile body count'u oranlayýp bina modelinin rengini eski hale getir
            float ratio = (float)player.bodyCount / lockValue;
            buildingMaterial.color = Color.Lerp(lockedColor, unlockedColor, ratio);

            // Kilit ile bodycount ayný sayý olursa tüm rengi gözükmesini saðla
            if (lockValue == player.bodyCount)
            {
                buildingMaterial.color = unlockedColor;
            }
        }
    }
}
