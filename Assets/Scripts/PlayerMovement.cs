using HyperCasual.Runner;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    public Text bodyCountText;
    public int bodyCount = 0;
    public Material playerMaterial;
    public GameObject playerMesh; // Player'�n mesh'ini belirtin


    public GameObject finishArea; // Finish alan�n� belirtin
    private bool isFinished = false; // Player'�n finish alan�na ula��p ula�mad���n� kontrol etmek i�in
    private PlayerController playerController; // PlayerController referans�n�z

    public GameObject buildingModel; // Bina modelini belirtin
    public Material buildingMaterial; // Bina modelinin malzemesini belirtin
    public Color lockedColor = Color.black; // Kilitli renk
    public Color unlockedColor = Color.white; // Kilitsiz renk
    public int lockValue = 50; // Kilit de�eri

    public GameObject bodyPrefab;
    public int gap = 2;
    public float bodyspeed = 15f;

    private List<GameObject> bodyparts = new List<GameObject>();
    private List<int> bodyPartsIndex = new List<int>();
    private List<Vector3> PositionHistory = new List<Vector3>();


    void Start()
    {
        playerController = GetComponent<PlayerController>();
        //buildingMaterial.color = lockedColor;


        UpdateBodyCountText();
    }

    void Update()
    {

        if (isFinished)
        {
            //playerController.CancelMovement(); // PlayerController'daki durma fonksiyonunu �a��r�n
            DestroyBodies();
            GrowPlayer();
            //// Player'�n h�z�n� s�f�rla
            //// Player'�n body count birer birer azalarak 0'a kadar getir
            //while (bodyCount > 0)
            //{
            //    ShrinkBody();
            //    lockValue -= 1;
            //}  

            //// Kilit say�s� ile body count'u oranlay�p bina modelinin rengini eski hale getir
            //float ratio = (float)bodyCount / lockValue;
            //buildingMaterial.color = Color.Lerp(lockedColor, unlockedColor, ratio);

            //// Kilit ile bodycount ayn� say� olursa t�m rengi g�z�kmesini sa�la
            //if (lockValue == bodyCount)
            //{
            //    buildingMaterial.color = unlockedColor;
            //}

        }


    }

    public void ResetBody()
    {
        // Destroy all body parts
        foreach (var body in bodyparts)
        {
            Destroy(body);
        }

        // Clear lists and reset body count
        bodyparts.Clear();
        bodyPartsIndex.Clear();
        bodyCount = 0;

        UpdateBodyCountText(); // Update the UI Text
    }


    public void ChangeColor(Color newColor)
    {
        if (playerMaterial != null)
        {
            playerMaterial.color = newColor; // Change the player's color
        }

        foreach (var body in bodyparts)
        {
            Renderer bodyRenderer = body.GetComponent<Renderer>();

            if (bodyRenderer != null)
            {
                bodyRenderer.material.color = newColor;
            }
        }
    }


    private void FixedUpdate()
    {
        PositionHistory.Insert(0, transform.position);
        int index = 0;
        foreach (var body in bodyparts)
        {
            Vector3 point = PositionHistory[Mathf.Min(index * gap, PositionHistory.Count - 1)];
            Vector3 moveDir = point - body.transform.position;
            body.transform.position += moveDir * bodyspeed * Time.fixedDeltaTime;
            body.transform.LookAt(point);
            index++;
        }
    }

    

    public void GrowBody()
    {
        GameObject body = Instantiate(bodyPrefab, transform.position, transform.rotation);
        bodyparts.Add(body);
        int index = 0;
        index++;
        bodyPartsIndex.Add(index);

        bodyCount++; // Increment the body count
        UpdateBodyCountText(); // Update the UI Text
    }

    private void UpdateBodyCountText()
    {
        if (bodyCountText != null)
        {
            bodyCountText.text = bodyCount.ToString();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Player'�n finish alan�na ula��p ula�mad���n� kontrol et
        if (other.gameObject == finishArea)
        {
            isFinished = true;
        }

        // Player'�n bir "PlayerObs" ile �arp���p �arp��mad���n� kontrol et
        else if (other.gameObject.tag == "PlayerObs")
        {
            PlayerObsScript playerObsScript = other.gameObject.GetComponent<PlayerObsScript>();

            if (playerObsScript != null && playerMaterial != null)
            {
                if (playerObsScript.material.color == playerMaterial.color)
                {
                    Destroy(other.gameObject, 0.005f);
                    GrowBody();
                }
                else
                {
                    ShrinkBody();
                }
            }
        }
    }


    // T�m body'leri player ile birle�tir
    private void CombineBodies()
    {
        foreach (var body in bodyparts)
        {
            body.transform.position = transform.position;
        }
    }

    private void DestroyBodies()
    {
        foreach (var body in bodyparts)
        {
            Destroy(body);
        }
        bodyparts.Clear();
    }

    // Player'�n boyutunu art�r
    public void GrowPlayer()
    {
        playerMesh.transform.localScale = new Vector3(2, 2, 2);
    }
    public void ShrinkBody()
    {
        if (bodyCount > 0)
        {
            int lastIndex = bodyparts.Count - 1;

            if (lastIndex >= 0)
            {
                Destroy(bodyparts[lastIndex]);
                bodyparts.RemoveAt(lastIndex);
                bodyPartsIndex.RemoveAt(lastIndex);
                bodyCount--;

                UpdateBodyCountText(); 
            }
        }
    }




    



}
