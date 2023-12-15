using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private float velocity;
    private Camera maincam;
    public float roadEndPoint;

    public Text bodyCountText; // Reference to the UI Text component
    private int bodyCount = 0; // Variable to store the body count


    public Material playerMaterial; // Add this line

    private Transform player;
    private Vector3 firstMousepos, firstPlayerpos;
    private bool moveTheBall;


    private float camVelocity;
    public float camSpeed = 0.4f;
    private Vector3 offset;

    public float playerZSpeed = 15f;

    public GameObject bodyPrefab;
    public int gap = 2;
    public float bodyspeed = 15f;

    private List<GameObject> bodyparts = new List<GameObject>();
    private List<int> bodyPartsIndex = new List<int>();
    private List<Vector3> PositionHistory = new List<Vector3>();



    void Start()
    {
        maincam = Camera.main;
        player = this.transform;
        UpdateBodyCountText(); // Call the method to update the UI Text initially
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            moveTheBall = true;

        }else if(Input.GetMouseButtonUp(0))
        {
            moveTheBall= false;
        }

        if(moveTheBall)
        {
            Plane newplane= new Plane(Vector3.up,0.8f);
            Ray ray=maincam.ScreenPointToRay(Input.mousePosition);

            if(newplane.Raycast(ray,out var distance))
            {
                Vector3 newmousepos = ray.GetPoint(distance) - firstMousepos;
                Vector3 newplayerpos = newmousepos + firstPlayerpos;
                newplayerpos.x = Mathf.Clamp(newplayerpos.x, -roadEndPoint, roadEndPoint);
                player.position = new Vector3(Mathf.SmoothDamp(player.position.x, newplayerpos.x, ref velocity, speed), player.position.y, player.position.x);
            }
        }
        
    }

    public void ChangeColor(Color newColor)
    {
        // Check if the playerMaterial has a Renderer
        if (playerMaterial != null)
        {
            playerMaterial.color = newColor; // Change the player's color
        }

        // Change the color of all body parts
        foreach (var body in bodyparts)
        {
            Renderer bodyRenderer = body.GetComponent<Renderer>();

            // Check if the body part has a Renderer
            if (bodyRenderer != null)
            {
                bodyRenderer.material.color = newColor;
            }
        }
    }



    private void FixedUpdate()
    {
        player.position += Vector3.forward * playerZSpeed * Time.fixedDeltaTime;
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

    private void LateUpdate()
    {
        Vector3 newcampos = maincam.transform.position;
        maincam.transform.position = new Vector3(Mathf.SmoothDamp(newcampos.x, player.position.x, ref camVelocity, camSpeed),
            newcampos.y, player.position.z + offset.z);
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
            bodyCountText.text = "Score: " + bodyCount.ToString();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="PlayerObs")
        {
            Destroy(other.gameObject,0.005f);
            GrowBody();
        }
    }
}
