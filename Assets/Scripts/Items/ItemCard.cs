using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemCard : MonoBehaviour
{
    //public enum ItemRarity { NoRarity, Common, Rare, Epic };

    public string myName;
    public ItemDataSO myItemDataSO;
    public ItemDataSO.ItemRarity myItemRarity;
    public Image myImageComponent;
    public Image myRarityFrame;
    public TextMeshProUGUI myNameText;
    public TextMeshProUGUI myDescriptionText;

    public bool inInventory;
    public bool inShop;
    public ItemSlot myItemSlot;

    public void OnItemCardClicked()
    {
        if (inInventory)
        {
            if(Inventory.Instance.readyToAcceptNewItem == true)
            {
                CharacterRoster.Instance.selectedCharacterData.AddItemToEquiptment(this);
                Inventory.Instance.RemoveItemFromInventory(this);
                Inventory.Instance.readyToAcceptNewItem = false;
            }
            return;
        }

        else if (inShop)
        {
            myItemSlot.BuyItem();
            return;
        }

        Debug.Log("Adding Item to inventory: " + myName);
        // add item to inventory
        Inventory.Instance.AddItemToInventory(this);
        RewardScreen.Instance.DestroyAllItemCards();
        Destroy(RewardScreen.Instance.currentItemRewardButton);
        RewardScreen.Instance.currentItemRewardButton = null;        
        RewardScreen.Instance.DisableItemLootScreen();
    }

    public void RunSetupFromItemData(ItemDataSO data)
    {
        myItemDataSO = data;
        Debug.Log("RunSetupFromItemData() called...");
        myName = data.itemName;
        myNameText.text = myName;
        myDescriptionText.text = data.itemDescription;
        myImageComponent.sprite = data.itemImage;       
        myItemRarity = data.itemRarity;

        if(myItemRarity == ItemDataSO.ItemRarity.Common)
        {
            myRarityFrame.color = data.commonColour;
        }
        else if (myItemRarity == ItemDataSO.ItemRarity.Rare)
        {
            myRarityFrame.color = data.rareColour;
        }
        else if (myItemRarity == ItemDataSO.ItemRarity.Epic)
        {
            myRarityFrame.color = data.epicColour;
        }

    }
}
