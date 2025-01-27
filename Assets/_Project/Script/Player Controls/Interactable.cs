using UnityEngine;
using System;

public class Interactable : MonoBehaviour
{
    [Header("Booleans")]
    public bool item, weapon;
    public Item invItem;

    [Header("Weapon Values")]
    public WeaponProperties weaponProperties;
    public WeaponScript ws;
    public int magSize;
    public int mags;
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
            ws.mags = mags;
            ws.magSize = magSize;

            // Destroy Object
            Destroy(this.gameObject);
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            inCollider = true;
            highLight.SetActive(true);
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
