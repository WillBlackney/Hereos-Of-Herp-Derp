using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{    
    [Header("Component References")]
    public GameObject inventoryContent;
    public GameObject inventoryParent;

    [Header("Properties")]
    public bool readyToAcceptNewItem;
    public List<ItemCard> allItemsInInventory;


    // Initialization + Singleton Pattern
    #region
    public static InventoryManager Instance;   
    private void Awake()
    {
        Instance = this;
        allItemsInInventory = new List<ItemCard>();        
    }
    #endregion

    // Modify Inventory Contents
    #region
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
    #endregion

    // Visibility + View Logic
    #region
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
    #endregion
}
