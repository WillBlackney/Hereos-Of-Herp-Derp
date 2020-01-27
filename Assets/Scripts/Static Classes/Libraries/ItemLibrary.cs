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
        ItemDataSO itemReturned = null;

        foreach (ItemDataSO itemData in allItems)
        {
            if (itemData.Name == itemName)
            {
                itemReturned = itemData;
            }
        }

        if (itemReturned == null)
        {
            Debug.Log("ItemLibrary.GetItem() search error: could not find a matching item in library with the name " + itemName);
        }

        return itemReturned;
    }
    public ItemDataSO GetRandomItem()
    {
        return allItems[Random.Range(0, allItems.Count)];
    }
    public ItemDataSO GetRandomCommonItem()
    {
        List<ItemDataSO> allCommonItems = new List<ItemDataSO>();
        int randomIndex;

        foreach(ItemDataSO item in allItems)
        {
            if(item.itemRarity == ItemDataSO.ItemRarity.Common)
            {
                allCommonItems.Add(item);
            }
        }

        randomIndex = Random.Range(0, allCommonItems.Count);
        return allCommonItems[randomIndex];
    }
    public ItemDataSO GetRandomRareItem()
    {
        List<ItemDataSO> allRareItems = new List<ItemDataSO>();
        int randomIndex;

        foreach (ItemDataSO item in allItems)
        {
            if (item.itemRarity == ItemDataSO.ItemRarity.Rare)
            {
                allRareItems.Add(item);
            }
        }

        randomIndex = Random.Range(0, allRareItems.Count);
        return allRareItems[randomIndex];
    }
    public ItemDataSO GetRandomEpicItem()
    {
        List<ItemDataSO> allEpicItems = new List<ItemDataSO>();
        int randomIndex;

        foreach (ItemDataSO item in allItems)
        {
            if (item.itemRarity == ItemDataSO.ItemRarity.Epic)
            {
                allEpicItems.Add(item);
            }
        }

        randomIndex = Random.Range(0, allEpicItems.Count);
        return allEpicItems[randomIndex];
    }
    public ItemDataSO GetRandomLootRewardItem()
    {
        ItemDataSO itemReturned = null;
        int randomNumber = Random.Range(0, 100);

        if(randomNumber >= 0 && randomNumber <= 69)
        {
            itemReturned = GetRandomCommonItem();
        }
        else if (randomNumber >= 70 && randomNumber <= 97)
        {
            itemReturned = GetRandomRareItem();
        }
        else if (randomNumber >= 98 && randomNumber <= 99)
        {
            itemReturned = GetRandomEpicItem();
        }

        return itemReturned;

    }
    #endregion

    
}
