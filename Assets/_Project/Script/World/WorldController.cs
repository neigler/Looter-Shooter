using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldController : MonoBehaviour
{
    [SerializeField] private GameObject LeaveButton;
    [SerializeField] private GameObject LeavePrompt;
    [SerializeField] private GameObject MapGUI;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private WeaponScript ws;
    [SerializeField] private GameObject bulletCount;
    bool inCollider;

    [Header("Can Go?")]
    [SerializeField] private bool area1;
    [SerializeField] private bool area2;
    [SerializeField] private bool area3;
    [SerializeField] private bool area4;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inCollider)
        {
            Leave();
        }
    }

    private void Leave()
    {
        LeaveButton.SetActive(true);
        bulletCount.SetActive(false);
        player.canMove = false;
        player.canRotate = false;
        ws.canShoot = false;
    }

    public void ButtonYes()
    {
        //Open Map
        MapGUI.SetActive(true);
        LeaveButton.SetActive(false);
        LeavePrompt.SetActive(false);
    }
    public void ButtonNo()
    {
        //Close Button
        bulletCount.SetActive(true);
        LeaveButton.SetActive(false);
        player.canMove = true;
        player.canRotate = true;
        ws.canShoot = true;
    }

    //Triggers
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            inCollider = true;
            LeavePrompt.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            inCollider = false;
            LeavePrompt.SetActive(false);
        }
    }

    //Switch facilities
    public void TravelArea1()
    {
        if (area1 == true)
        {
            //Load scene
            SceneManager.LoadScene(sceneName: "Factory");
            Time.timeScale = 1;

            //Re activate player.
            MapGUI.SetActive(false);
            bulletCount.SetActive(true);
            player.canMove = true;
            player.canRotate = true;
            ws.canShoot = true;
        }
    }

    public void TravelArea2()
    {
        if (area2 == true)
        {
            //Load scene
            SceneManager.LoadScene(sceneName: "TestingScene");
            Time.timeScale = 1;

            //Re activate player.
            MapGUI.SetActive(false);
            bulletCount.SetActive(true);
            player.canMove = true;
            player.canRotate = true;
            ws.canShoot = true;
        }
    }

    public void TravelArea3()
    {
        if (area3 == true)
        {
            Debug.Log("Going to area 3");
        }
    }

    public void TravelArea4()
    {
        if (area4 == true)
        {
            Debug.Log("Going to area 4");
        }
    }
}
