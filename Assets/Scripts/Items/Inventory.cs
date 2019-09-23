using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryContent;
    public GameObject inventoryParent;
    public bool readyToAcceptNewItem;

    public static Inventory Instance;

    public List<ItemCard> allItemsInInventory;

    private void Awake()
    {
        Instance = this;
        allItemsInInventory = new List<ItemCard>();        
    }

    public void AddItemToInventory(ItemCard itemAdded)
    {
        Debug.Log("AddItemToInventory() called...");
        ItemCard newItem = Instantiate(itemAdded, inventoryContent.transform);
        Debug.Log("New item instantiated, added to inventoryParent");
        newItem.RunSetupFromItemData(ItemLibrary.Instance.GetItemByName(itemAdded.myName));
        newItem.inInventory = true;
        newItem.inShop = false;
    }

    public void RemoveItemFromInventory(ItemCard itemRemoved)
    {
        allItemsInInventory.Remove(itemRemoved);
        Destroy(itemRemoved.gameObject);
    }

    public void SetInventoryView(bool onOrOff)
    {
        if (onOrOff == true)
        {
            inventoryParent.SetActive(true);
        }
        else
        {
            inventoryParent.SetActive(false);
        }
        
    }
}
