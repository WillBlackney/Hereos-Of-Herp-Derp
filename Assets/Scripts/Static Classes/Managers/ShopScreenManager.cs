using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScreenManager : MonoBehaviour
{
    public static ShopScreenManager Instance;

    [Header("Component References")]
    public GameObject visualParent;
    public GameObject continueButton;

    [Header("Item Slot References")]
    public ItemSlot itemSlotOne;
    public ItemSlot itemSlotTwo;
    public ItemSlot itemSlotThree;
    public ItemSlot itemSlotFour;
    public ItemSlot itemSlotFive;
    public ItemSlot itemSlotSix;
    public ItemSlot itemSlotSeven;
    public ItemSlot itemSlotEight;

    [Header("Artifact Slot References")]
    public ArtifactSlot artifactSlotOne;
    public ArtifactSlot artifactSlotTwo;
    public ArtifactSlot artifactSlotThree;

    [Header("Properties")]
    public List<ItemDataSO> itemsInShopData;
    public List<ItemSlot> allItemsSlots;

    // Initialization + Setup
    #region
    private void Awake()
    {
        Instance = this;
        allItemsSlots.Add(itemSlotOne);
        allItemsSlots.Add(itemSlotTwo);
        allItemsSlots.Add(itemSlotThree);
        allItemsSlots.Add(itemSlotFour);
        allItemsSlots.Add(itemSlotFive);
        allItemsSlots.Add(itemSlotSix);
        allItemsSlots.Add(itemSlotSeven);
        allItemsSlots.Add(itemSlotEight);
    }
    #endregion

    // Visibility + View Logic
    #region
    public void EnableShopScreenView()
    {
        visualParent.SetActive(true);
    }
    public void DisableShopScreenView()
    {
        visualParent.SetActive(false);
    }
    #endregion

    // Conditional Checks + Boolean logic
    #region
    public bool IsItemAlreadyInShop(ItemSlot slotToCheck)
    {
        bool alreadyInShop = false;
        ItemDataSO itemChecked = slotToCheck.myItemCard.myItemDataSO;

        foreach (ItemSlot slot in allItemsSlots)
        {
            if (slot.myItemCard.myItemDataSO != null &&
                slot.myItemCard.myItemDataSO.Name == itemChecked.Name &&
                slot != slotToCheck)
            {
                alreadyInShop = true;
                break;
            }
        }

        return alreadyInShop;
    }
    #endregion

    // Populate Shop Elements
    #region
    public void LoadShopScreenEntities()
    {
        PopulateItemSlots();
    }
    public void PopulateItemSlots()
    {
        itemsInShopData = new List<ItemDataSO>();

        foreach (ItemSlot slot in allItemsSlots)
        {
            slot.myItemCard.myItemDataSO = null;
        }

        while (itemSlotOne.myItemCard.myItemDataSO == null || IsItemAlreadyInShop(itemSlotOne))
        {
            itemSlotOne.SetUpItemSlot(ItemDataSO.ItemRarity.Common);
            itemsInShopData.Add(itemSlotOne.myItemCard.myItemDataSO);
        }
        while (itemSlotTwo.myItemCard.myItemDataSO == null || IsItemAlreadyInShop(itemSlotTwo))
        {
            itemSlotTwo.SetUpItemSlot(ItemDataSO.ItemRarity.Common);
            itemsInShopData.Add(itemSlotTwo.myItemCard.myItemDataSO);
        }
        while (itemSlotThree.myItemCard.myItemDataSO == null || IsItemAlreadyInShop(itemSlotThree))
        {
            itemSlotThree.SetUpItemSlot(ItemDataSO.ItemRarity.Common);
            itemsInShopData.Add(itemSlotThree.myItemCard.myItemDataSO);
        }
        while (itemSlotFour.myItemCard.myItemDataSO == null || IsItemAlreadyInShop(itemSlotFour))
        {
            itemSlotFour.SetUpItemSlot(ItemDataSO.ItemRarity.Common);
            itemsInShopData.Add(itemSlotFour.myItemCard.myItemDataSO);
        }
        while (itemSlotFive.myItemCard.myItemDataSO == null || IsItemAlreadyInShop(itemSlotFive))
        {
            itemSlotFive.SetUpItemSlot(ItemDataSO.ItemRarity.Common);
            itemsInShopData.Add(itemSlotFive.myItemCard.myItemDataSO);
        }
        while (itemSlotSix.myItemCard.myItemDataSO == null || IsItemAlreadyInShop(itemSlotSix))
        {
            itemSlotSix.SetUpItemSlot(ItemDataSO.ItemRarity.Rare);
            itemsInShopData.Add(itemSlotSix.myItemCard.myItemDataSO);
        }
        while (itemSlotSeven.myItemCard.myItemDataSO == null || IsItemAlreadyInShop(itemSlotSeven))
        {
            itemSlotSeven.SetUpItemSlot(ItemDataSO.ItemRarity.Rare);
            itemsInShopData.Add(itemSlotSeven.myItemCard.myItemDataSO);
        }
        while (itemSlotEight.myItemCard.myItemDataSO == null || IsItemAlreadyInShop(itemSlotEight))
        {
            itemSlotEight.SetUpItemSlot(ItemDataSO.ItemRarity.Epic);
            itemsInShopData.Add(itemSlotEight.myItemCard.myItemDataSO);
        }

    }
    
    #endregion

    // Get Item Data Logic
    #region
    public List<ItemDataSO> GetAllOtherItemSlots(ItemDataSO itemToExclude)
    {
        List<ItemDataSO> allOtherItems = new List<ItemDataSO>();
        allOtherItems.AddRange(itemsInShopData);
        if (allOtherItems.Contains(itemToExclude))
        {
            allOtherItems.Remove(itemToExclude);
        }
        
        return allOtherItems;
    }
    #endregion

    // Mouse + Click Events
    #region
    public void OnContinueButtonClicked()
    {
        UIManager.Instance.EnableWorldMapView();
    }
    #endregion
}
