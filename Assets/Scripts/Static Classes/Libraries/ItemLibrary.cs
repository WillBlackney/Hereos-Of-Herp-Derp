using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLibrary : MonoBehaviour
{  
    [Header("Properties")]
    public List<ItemDataSO> allItems;

    // Initialization + Singleton Pattern
    #region
    public static ItemLibrary Instance;
    private void Awake()
    {
        Instance = this;        
    }
    #endregion

    // Get ItemDataSO Logic
    #region
    public ItemDataSO GetItemByName(string itemName)
    {
        Debug.Log("ItemLibrary.GetItemByName() called, searching for " + itemName);

        ItemDataSO itemReturned = null;

        foreach (ItemDataSO itemData in allItems)
        {
            if (itemData.Name == itemName)
            {
                itemReturned = itemData;
                break;
            }
        }

        if (itemReturned == null)
        {
            Debug.Log("ItemLibrary.GetItemByName() search error: could not find a matching item in library with the name " + itemName);
        }

        return itemReturned;
    }
    
    public ItemDataSO GetRandomCommonItem(ItemDataSO.ItemType type = ItemDataSO.ItemType.None)
    {
        Debug.Log("ItemLibrary.GetRandomCommonItem() called...");

        List <ItemDataSO> allCommonItems = new List<ItemDataSO>();
        int randomIndex;

        foreach(ItemDataSO item in allItems)
        {
            if(item.itemRarity == ItemDataSO.ItemRarity.Common &&
                item.startingItem == false)
            {
                allCommonItems.Add(item);
            }
        }

        Debug.Log("ItemLibrary.GetRandomCommonItem() found " + allCommonItems.Count.ToString() +
            " common items");

        randomIndex = Random.Range(0, allCommonItems.Count);
        Debug.Log("ItemLibrary.GetRandomCommonItem() returning " + allCommonItems[randomIndex].Name);
        return allCommonItems[randomIndex];
    }
    public ItemDataSO GetRandomCommonWeaponItem()
    {
        Debug.Log("ItemLibrary.GetRandomCommonWeaponItem() called...");

        List<ItemDataSO> allCommonWeaponItems = new List<ItemDataSO>();
        int randomIndex;

        foreach (ItemDataSO item in allItems)
        {
            if (item.itemRarity == ItemDataSO.ItemRarity.Common &&                
                item.startingItem == false &&
                (item.itemType == ItemDataSO.ItemType.MeleeOneHand || item.itemType == ItemDataSO.ItemType.MeleeTwoHand ||
                item.itemType == ItemDataSO.ItemType.RangedTwoHand || item.itemType == ItemDataSO.ItemType.Shield)
                )
            {
                allCommonWeaponItems.Add(item);
            }
        }

        Debug.Log("ItemLibrary.GetRandomCommonWeaponItem() found " + allCommonWeaponItems.Count.ToString() +
            " common items");

        randomIndex = Random.Range(0, allCommonWeaponItems.Count);
        Debug.Log("ItemLibrary.GetRandomCommonWeaponItem() returning " + allCommonWeaponItems[randomIndex].Name);
        return allCommonWeaponItems[randomIndex];
    }
    public ItemDataSO GetRandomRareItem()
    {
        Debug.Log("ItemLibrary.GetRandomRareItem() called...");

        List<ItemDataSO> allRareItems = new List<ItemDataSO>();
        int randomIndex;

        foreach (ItemDataSO item in allItems)
        {
            if (item.itemRarity == ItemDataSO.ItemRarity.Rare)
            {
                allRareItems.Add(item);
            }
        }
        Debug.Log("ItemLibrary.GetRandomRareItem() found " + allRareItems.Count.ToString() +
           " rare items");
        randomIndex = Random.Range(0, allRareItems.Count);
        Debug.Log("ItemLibrary.GetRandomRareItem() returning " + allRareItems[randomIndex].Name);
        return allRareItems[randomIndex];
    }
    public ItemDataSO GetRandomRareWeaponItem()
    {
        Debug.Log("ItemLibrary.GetRandomRareItem() called...");

        List<ItemDataSO> allRareWeapons = new List<ItemDataSO>();
        int randomIndex;

        foreach (ItemDataSO item in allItems)
        {
            if (item.itemRarity == ItemDataSO.ItemRarity.Rare &&
                (item.itemType == ItemDataSO.ItemType.MeleeOneHand ||
                item.itemType == ItemDataSO.ItemType.MeleeTwoHand ||
                item.itemType == ItemDataSO.ItemType.RangedTwoHand ||
                item.itemType == ItemDataSO.ItemType.Shield))
            {
                allRareWeapons.Add(item);
            }
        }
        Debug.Log("ItemLibrary.GetRandomRareItem() found " + allRareWeapons.Count.ToString() +
           " rare items");
        randomIndex = Random.Range(0, allRareWeapons.Count);
        Debug.Log("ItemLibrary.GetRandomRareItem() returning " + allRareWeapons[randomIndex].Name);
        return allRareWeapons[randomIndex];
    }
    public ItemDataSO GetRandomEpicItem(bool includeStoryItems = false)
    {
        
        Debug.Log("ItemLibrary.GetRandomEpicItem() called...");

        List<ItemDataSO> allEpicItems = new List<ItemDataSO>();
        int randomIndex;

        foreach (ItemDataSO item in allItems)
        {
            if (item.itemRarity == ItemDataSO.ItemRarity.Epic)
            {
                if(!item.storyEventItem ||
                    (item.storyEventItem && includeStoryItems == true))
                {
                    allEpicItems.Add(item);
                }               
                
            }
        }

        Debug.Log("ItemLibrary.GetRandomEpicItem() found " + allEpicItems.Count.ToString() +
          " epic items");

        randomIndex = Random.Range(0, allEpicItems.Count);
        Debug.Log("ItemLibrary.GetRandomEpicItem() returning " + allEpicItems[randomIndex].Name);
        return allEpicItems[randomIndex];
        
    }
    public ItemDataSO GetRandomEpicWeaponItem()
    {
        Debug.Log("ItemLibrary.GetRandomEpicWeaponItem() called...");

        List<ItemDataSO> allEpicWeapons = new List<ItemDataSO>();
        int randomIndex;

        foreach (ItemDataSO item in allItems)
        {
            if (item.itemRarity == ItemDataSO.ItemRarity.Epic &&
                (item.itemType == ItemDataSO.ItemType.MeleeOneHand ||
                item.itemType == ItemDataSO.ItemType.MeleeTwoHand ||
                item.itemType == ItemDataSO.ItemType.RangedTwoHand ||
                item.itemType == ItemDataSO.ItemType.Shield))
            {
                allEpicWeapons.Add(item);
            }
        }
        Debug.Log("ItemLibrary.GetRandomEpicWeaponItem() found " + allEpicWeapons.Count.ToString() +
           " rare items");
        randomIndex = Random.Range(0, allEpicWeapons.Count);
        Debug.Log("ItemLibrary.GetRandomEpicWeaponItem() returning " + allEpicWeapons[randomIndex].Name);
        return allEpicWeapons[randomIndex];
    }

    #endregion


}
