using UnityEngine;
using System;

public class Interactable : MonoBehaviour
{
    [Header("Booleans")]
    public bool item;
    public bool ammoBox;
    public bool weapon;
    public Item invItem;

    [Header("Weapon Values")]
    public WeaponProperties weaponProperties;
    public WeaponScript ws;
    public int magSize;
    public int bulletsLeft;

    [Header("Item Values")]
    public Inventory inventory;
    public GameObject highLight;
    bool inCollider = false;

    void Start()
    {
        ws = GameObject.Find("Weapon Controller").GetComponent<WeaponScript>();
    }

    void Update()
    {
        if (inventory == null && item)
        {
            inventory = GameObject.Find("Inventory Background").GetComponent<Inventory>();
        }
        if (ws == null && weapon)
        {
            ws = GameObject.Find("WeaponController").GetComponent<WeaponScript>();
        }
        if (Input.GetKeyDown(KeyCode.E) && inCollider && item)
        {
            inventory.items.Clear();
            inventory.items.Add(invItem);
            inventory.SpawnInventoryItem();
            AudioManager.PlaySound(SoundType.ITEMPICKUP);
            Destroy(this.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.E) && inCollider && weapon)
        {
            // Drop original weapon
            if (ws.currentWeapon != null)
                Instantiate(ws.currentWeapon.weaponPrefab, transform.position, Quaternion.identity);

            // Play Sound
            AudioManager.PlaySound(SoundType.RELOAD);

            // Equip gun
            ws.currentWeapon = weaponProperties;
            ws.holdingWeapon = true;

            // Add Proper Mag Size
            ws.bulletsLeft = bulletsLeft;
            ws.magSize = magSize;

            //Update UI
            ws.UpdateAmmoUI();

            // Destroy Object
            Destroy(this.gameObject);
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            inCollider = true;

            //Highlight
            if (weapon || item)
                highLight.SetActive(true);

            //Ammo
            if (ammoBox && ws.bulletsLeft != ws.magSize && ws.holdingWeapon)
            {
                ws.Reload();
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            inCollider = false;
            highLight.SetActive(false);
        }
    }
}
