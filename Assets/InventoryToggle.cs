using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    [HideInInspector] public bool opened;

    [SerializeField] private GameObject inventoryGUI;

    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private WeaponScript weaponController;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Camera")]
    [SerializeField] private GameObject cameraGameObject;
    [SerializeField] private GameObject cameraMoveToLocation;
    [SerializeField] private PlayerCamera cameraController;

    Vector3 refVel;
    bool updatingCamPosition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !opened)
        {
            // Variables
            opened = true;
            playerMovement.speed = 0;
            playerMovement.canRotate = false;
            cameraController.canMove = false;
            weaponController.canShoot = false;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && opened)
        {
            // Variables
            opened = false;
            playerMovement.speed = 5;
            playerMovement.canRotate = true;
            cameraController.canMove = true;
            weaponController.canShoot = true;
        }

        if (opened)
        {
            canvasGroup.alpha = 1;
        }
        else if (!opened)
        {
            canvasGroup.alpha = 0;
        }
    }
}
