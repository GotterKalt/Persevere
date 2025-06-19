using UnityEngine;
using System;

public class InventoryControl : MonoBehaviour
{
    public InventorySlot[] inventorySlots;

    public event Action OnInventoryChanged;
    public GameObject inventoryUI;


    public void AddItem(Item item)
    {
        OnInventoryChanged?.Invoke();
    }
    void SpawnNewItem(Item item, InventorySlot slot)
    {
        OnInventoryChanged?.Invoke();
    }
    public Item GetItemAt(int index)
    {
        if (index >= 0 && index < inventorySlots.Length)
        {
            if (inventorySlots[index].transform.childCount > 0)
            {
                InventoryItem inventoryItem = inventorySlots[index].transform.GetChild(0).GetComponent<InventoryItem>();
                return inventoryItem?.item;
            }
        }
        return null;
    }



    void Start()
    {
        inventoryUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }
}
