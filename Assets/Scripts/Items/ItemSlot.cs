using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [Header("Component References")]
    public TextMeshProUGUI myGoldCostText;
    public ItemCard myItemCard;

    [Header("Properties")]
    public int goldCost;

    // Initialization + Setup
    #region
    public void SetUpItemSlot(ItemDataSO.ItemRarity rarity)
    {
        EnableItemSlotView();

        ItemDataSO itemData = null;
        int randomGoldCost = 0;
        
        if(rarity == ItemDataSO.ItemRarity.Common)
        {
            itemData = ItemLibrary.Instance.GetRandomCommonItem();
            randomGoldCost = Random.Range(4, 8);
        }
        else if (rarity == ItemDataSO.ItemRarity.Rare)
        {
            itemData = ItemLibrary.Instance.GetRandomRareItem();
            randomGoldCost = Random.Range(8, 12);
        }
        else if (rarity == ItemDataSO.ItemRarity.Epic)
        {
            itemData = ItemLibrary.Instance.GetRandomEpicItem();
            randomGoldCost = Random.Range(14, 18);
        }

        myItemCard.RunSetupFromItemData(itemData, ShopScreenManager.Instance.itemSlotSortingLayer);
        myItemCard.myItemSlot = this;
        myItemCard.location = ItemCard.Location.Shop;
        SetGoldCost(randomGoldCost);
    }
    #endregion

    // Item Logic
    #region
    public void SetGoldCost(int cost)
    {
        goldCost = cost;
        myGoldCostText.text = goldCost.ToString();
    }
    public void BuyItem()
    {
        Debug.Log("ItemSlot.BuyItem() called for: " + myItemCard.myName);

        if (PlayerDataManager.Instance.currentGold >= goldCost)
        {
            Debug.Log("Buying Item " + myItemCard.myName + " for " + goldCost.ToString());
            InventoryController.Instance.AddItemToInventory(myItemCard.myItemDataSO, true);
            PlayerDataManager.Instance.ModifyGold(-goldCost);
            DisableItemSlotView();
        }
        else
        {
            Debug.Log("Cannot buy item: Not enough gold...");
        }
    }
    #endregion

    // Visility + View Logic
    #region
    public void DisableItemSlotView()
    {
        gameObject.SetActive(false);
    }
    public void EnableItemSlotView()
    {
        gameObject.SetActive(true);

    }
    #endregion

}
