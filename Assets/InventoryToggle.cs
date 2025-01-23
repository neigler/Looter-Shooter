using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [HideInInspector] public bool opened;

    [SerializeField] private GameObject inventoryGUI;

    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private WeaponScript weaponController;

    [Header("Camera")]
    [SerializeField] private GameObject cameraGameObject;
    [SerializeField] private GameObject cameraMoveToLocation;
    [SerializeField] private PlayerCamera cameraController;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !opened)
        {
            // Variables
            opened = true;
            playerMovement.speed = 0;
            cameraController.canMove = false;
            weaponController.canShoot = false;

            // Move Camera
            cameraGameObject.transform.position = new Vector3(0, 0, -10);
            cameraGameObject.transform.position = new Vector3(cameraMoveToLocation.transform.position.x, cameraMoveToLocation.transform.position.y, -10);
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && opened)
        {
            // Variables
            opened = false;
            playerMovement.speed = 5;
            cameraController.canMove = true;
            weaponController.canShoot = true;
        }

        if (opened)
        {
            inventoryGUI.SetActive(true);
        }
        else if (!opened)
        {
            inventoryGUI.SetActive(false);
        }
    }
}
