using HyperCasual.Runner;
using UnityEngine;
using UnityEngine.UI;

public class BuildingLock : MonoBehaviour
{
    public GameObject buildingModel; // Bina modelini belirtin
    public Material buildingMaterial; // Bina modelinin malzemesini belirtin
    public Color lockedColor = Color.black; // Kilitli renk
    public Color unlockedColor = Color.white; // Kilitsiz renk
    public int lockValue = 10; // Kilit de�eri
    private PlayerMovement player; // Player referans�n�z
    public GameObject plane; // Plane referans�n�z
    private PlayerController playerController; // PlayerController referans�n�z
    public Text BanklockedText;


    void Start()
    {
        playerController = GetComponent<PlayerController>();

        BanklockedText.text=lockValue.ToString();
        //bodyCountText.text=player.bodyCount.ToString();
        // Player'� al�n
        player = FindObjectOfType<PlayerMovement>();
        // Bina modelinin rengini siyahla�t�r�n
        buildingMaterial.color = lockedColor;

    }

    public void UpdateBanklockedText()
    {
        if (BanklockedText != null)
        {
            BanklockedText.text = lockValue.ToString();
        }
    }

    // Use OnTriggerEnter instead of Update
    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {


            // Player'�n body count birer birer azalarak 0'a kadar getir
            for (int i = player.bodyCount; i > 0; i--)
            {
                player.ShrinkBody();
                lockValue -= 1;
                
            
            if(lockValue < 0)
            {
                lockValue= 0;
            }

           

            // Kilit say�s� ile body count�u oranlay�p bina modelinin rengini eski hale getir
            float ratio = (float)player.bodyCount / lockValue;
            buildingMaterial.color = Color.Lerp(lockedColor, unlockedColor, ratio);

            }
            player.bodyCount = lockValue - player.bodyCount;
            if (player.bodyCount > 0 || player.bodyCount<0)
            {
                player.bodyCount = 0;
            }
            player.UpdateBodyCountText();
            UpdateBanklockedText();

            if (lockValue == player.bodyCount)
            {
                buildingMaterial.color = unlockedColor;
            }
        }
    }

}
