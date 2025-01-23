using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool item, weapon;
    public Item invItem;
    public Inventory slot;
    [Header("Item List")]
    [SerializeField] Item[] items;
    bool inCollider = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inCollider && item)
        {
            slot.SpawnInventoryItem(invItem);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            inCollider = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            inCollider = false;
        }
    }
}
