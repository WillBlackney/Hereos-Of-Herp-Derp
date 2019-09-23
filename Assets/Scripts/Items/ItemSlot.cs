using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [Header("Component References")]
    public TextMeshProUGUI goldCostText;
    public ItemCard myItemCard;


    [Header("Properties")]
    public int goldCost;

    public void SetUpItemSlot(ItemDataSO.ItemRarity rarity)
    {
        EnableItemSlotView();

        ItemDataSO itemData = null;
        int randomGoldCost = 0;
        
        if(rarity == ItemDataSO.ItemRarity.Common)
        {
            itemData = ItemLibrary.Instance.GetRandomCommonItem();
            randomGoldCost = Random.Range(40, 60);
        }
        else if (rarity == ItemDataSO.ItemRarity.Rare)
        {
            itemData = ItemLibrary.Instance.GetRandomRareItem();
            randomGoldCost = Random.Range(60, 100);
        }
        else if (rarity == ItemDataSO.ItemRarity.Epic)
        {
            itemData = ItemLibrary.Instance.GetRandomEpicItem();
            randomGoldCost = Random.Range(100, 200);
        }

        myItemCard.RunSetupFromItemData(itemData);
        myItemCard.myItemSlot = this;
        myItemCard.inShop = true;
        SetGoldCost(randomGoldCost);
    }

    public void SetGoldCost(int cost)
    {
        goldCost = cost;
        goldCostText.text = goldCost.ToString();
    }

    public void BuyItem()
    {
        if (PlayerDataManager.Instance.currentGold >= goldCost)
        {
            Debug.Log("Buying Item " + myItemCard.myName + " for " + goldCost.ToString());
            Inventory.Instance.AddItemToInventory(myItemCard);
            PlayerDataManager.Instance.ModifyGold(-goldCost);
            DisableItemSlotView();
        }
        else
        {
            Debug.Log("Cannot buy item: Not enough gold...");
        }
    }

    public void DisableItemSlotView()
    {
        gameObject.SetActive(false);
    }

    public void EnableItemSlotView()
    {
        gameObject.SetActive(true);

    }

}
