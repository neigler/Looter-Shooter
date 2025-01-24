using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum WeightClass
{
    noHeavy,
    lilHeavy,
    mediumHeavy,
    largeHeavy
}

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;

    [Header("GUI Objects")]
    [SerializeField] private TextMeshProUGUI weightText;
    [SerializeField] InventoryItem itemPrefab;
    [SerializeField] PlayerMovement player;
    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventorySlot[] inventorySlots;

    [Header("Variables")]
    [SerializeField] public float currentWeight;
    [SerializeField] public WeightClass weightClass;
    [SerializeField] public List<Item> items = new List<Item>();

    void Awake()
    {
        Singleton = this;
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    public void SpawnInventoryItem(Item item = null)
    {
        Item _item = item;
        if (_item == null)
        {
            int random = Random.Range(0, items.Count);
            _item = items[random];
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // Check if the slot is empty
            if (inventorySlots[i].myItem == null)
            {
                Instantiate(itemPrefab, inventorySlots[i].transform).Initialize(_item, inventorySlots[i]);
                break;
            }
        }
    }

    void Update()
    {
        WeightCalculation();

        if (carriedItem == null) return;
        carriedItem.transform.position = Input.mousePosition;
    }

    public void SetCarriedItem(InventoryItem item)
    {
        if (carriedItem != null)
        {
            if (item.activeSlot.myTag != SlotTag.None && item.activeSlot.myTag != carriedItem.myItem.itemTag) return;
            item.activeSlot.SetItem(carriedItem);
        }

        if (item.activeSlot.myTag != SlotTag.None)
        { EquipEquipment(item.activeSlot.myTag, null); }

        carriedItem = item;
        carriedItem.canvasGroup.blocksRaycasts = false;
        item.transform.SetParent(draggablesTransform);
    }

    public void EquipEquipment(SlotTag tag, InventoryItem item = null)
    {
        switch (tag)
        {
            case SlotTag.Head:
                if (item == null)
                {
                    // Destroy item.equipmentPrefab on the player object
                    Debug.Log("Unequipped helmet on " + tag);
                }
                else
                {
                    // Instanitiate item.equipmentPrefab on the player object;
                    Debug.Log("Equipped " + item.myItem.name + " on " + tag);
                }
                break;
            case SlotTag.Chest:
                break;
            case SlotTag.Legs:
                break;
            case SlotTag.Feet:
                break;
        }
    }

    private void WeightCalculation()
    {
        // Weight class calculation
        if (currentWeight >= 15)
            weightClass = WeightClass.lilHeavy;
        else if (currentWeight <= 0)
            weightClass = WeightClass.noHeavy;

        if (currentWeight >= 45)
            weightClass = WeightClass.mediumHeavy;
        else if (currentWeight <= 0)
            weightClass = WeightClass.noHeavy;

        if (currentWeight >= 80)
            weightClass = WeightClass.largeHeavy;
        else if (currentWeight <= 0)
            weightClass = WeightClass.noHeavy;

        // Player Speed calculation
        if (weightClass == WeightClass.noHeavy)
        {
            player.currentSpeed = player.noHeavySpeed;
        }
        if (weightClass == WeightClass.lilHeavy)
        {
            player.currentSpeed = player.lilHeavySpeed;
        }
        if (weightClass == WeightClass.mediumHeavy)
        {
            player.currentSpeed = player.mediumHeavySpeed;
        }
        if (weightClass == WeightClass.largeHeavy)
        {
            player.currentSpeed = player.largeHeavySpeed;
        }

        // Weight Text
        weightText.text = currentWeight + " lb";
    }
}
