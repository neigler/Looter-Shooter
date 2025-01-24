using UnityEngine;
using System;

public class Interactable : MonoBehaviour
{
    public bool item, weapon;
    public Item invItem;
    public WeaponProperties weaponProperties;
    public Inventory inventory;
    public WeaponScript ws;
    public GameObject highLight;
    bool inCollider = false;

    void Start()
    {
        ws = GameObject.Find("Weapon Controller").GetComponent<WeaponScript>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inCollider && item)
        {
            inventory.items.Clear();
            inventory.items.Add(invItem);
            inventory.SpawnInventoryItem();
            Destroy(this.gameObject);
        }

        if (Input.GetKeyDown(KeyCode.E) && inCollider && weapon)
        {
            // Drop original weapon
            if (ws.currentWeapon != null)
                Instantiate(ws.currentWeapon.weaponPrefab, transform.position, Quaternion.identity);

            // Equip new one
            ws.currentWeapon = weaponProperties;
            ws.holdingWeapon = true;
            ws.bulletsLeft = ws.currentWeapon.magSize;
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
