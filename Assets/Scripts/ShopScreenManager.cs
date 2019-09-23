using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScreenManager : MonoBehaviour
{
    public static ShopScreenManager Instance;

    [Header("Component References")]
    public GameObject visualParent;
    public GameObject continueButton;

    [Header("Item Slots")]
    public ItemSlot itemSlotOne;
    public ItemSlot itemSlotTwo;
    public ItemSlot itemSlotThree;
    public ItemSlot itemSlotFour;
    public ItemSlot itemSlotFive;
    public ItemSlot itemSlotSix;
    public ItemSlot itemSlotSeven;
    public ItemSlot itemSlotEight;

    [Header("Artifact Slots")]
    public ArtifactSlot artifactSlotOne;
    public ArtifactSlot artifactSlotTwo;
    public ArtifactSlot artifactSlotThree;

    public List<ItemDataSO> itemsInShopData;

    private void Awake()
    {
        Instance = this;
    }

    public void EnableShopScreenView()
    {
        visualParent.SetActive(true);
    }
    public void DisableShopScreenView()
    {
        visualParent.SetActive(false);
    }

    public void LoadShopScreenEntities()
    {
        PopulateItemSlots();
        PopulateArtifactSlots();
    }

    public void PopulateItemSlots()
    {
        itemsInShopData = new List<ItemDataSO>();

        /*
        while(itemSlotOne.myItemCard.myItemDataSO == null || itemsInShopData.Contains(itemSlotOne.myItemCard.myItemDataSO))
        {
            itemSlotOne.SetUpItemSlot(ItemDataSO.ItemRarity.Common);
            itemsInShopData.Add(itemSlotOne.myItemCard.myItemDataSO);
        }
        while (itemSlotTwo.myItemCard.myItemDataSO == null || itemsInShopData.Contains(itemSlotTwo.myItemCard.myItemDataSO))
        {
            itemSlotTwo.SetUpItemSlot(ItemDataSO.ItemRarity.Common);
            itemsInShopData.Add(itemSlotTwo.myItemCard.myItemDataSO);
        }
        while (itemSlotThree.myItemCard.myItemDataSO == null || itemsInShopData.Contains(itemSlotThree.myItemCard.myItemDataSO))
        {
            itemSlotThree.SetUpItemSlot(ItemDataSO.ItemRarity.Common);
            itemsInShopData.Add(itemSlotThree.myItemCard.myItemDataSO);
        }
        while (itemSlotFour.myItemCard.myItemDataSO == null || itemsInShopData.Contains(itemSlotFour.myItemCard.myItemDataSO))
        {
            itemSlotFour.SetUpItemSlot(ItemDataSO.ItemRarity.Common);
            itemsInShopData.Add(itemSlotFour.myItemCard.myItemDataSO);
        }
        while (itemSlotFive.myItemCard.myItemDataSO == null || itemsInShopData.Contains(itemSlotFive.myItemCard.myItemDataSO))
        {
            itemSlotFive.SetUpItemSlot(ItemDataSO.ItemRarity.Common);
            itemsInShopData.Add(itemSlotFive.myItemCard.myItemDataSO);
        }
        while (itemSlotSix.myItemCard.myItemDataSO == null || itemsInShopData.Contains(itemSlotSix.myItemCard.myItemDataSO))
        {
            itemSlotSix.SetUpItemSlot(ItemDataSO.ItemRarity.Rare);
            itemsInShopData.Add(itemSlotSix.myItemCard.myItemDataSO);
        }
        while (itemSlotSeven.myItemCard.myItemDataSO == null || itemsInShopData.Contains(itemSlotSeven.myItemCard.myItemDataSO))
        {
            itemSlotSeven.SetUpItemSlot(ItemDataSO.ItemRarity.Rare);
            itemsInShopData.Add(itemSlotSeven.myItemCard.myItemDataSO);
        }
        while (itemSlotEight.myItemCard.myItemDataSO == null || itemsInShopData.Contains(itemSlotEight.myItemCard.myItemDataSO))
        {
            itemSlotEight.SetUpItemSlot(ItemDataSO.ItemRarity.Epic);
            itemsInShopData.Add(itemSlotEight.myItemCard.myItemDataSO);
        }
        */
        
        itemSlotOne.SetUpItemSlot(ItemDataSO.ItemRarity.Common);
        itemSlotTwo.SetUpItemSlot(ItemDataSO.ItemRarity.Common);
        itemSlotThree.SetUpItemSlot(ItemDataSO.ItemRarity.Common);
        itemSlotFour.SetUpItemSlot(ItemDataSO.ItemRarity.Common);
        itemSlotFive.SetUpItemSlot(ItemDataSO.ItemRarity.Common);
        itemSlotSix.SetUpItemSlot(ItemDataSO.ItemRarity.Rare);
        itemSlotSeven.SetUpItemSlot(ItemDataSO.ItemRarity.Rare);
        itemSlotEight.SetUpItemSlot(ItemDataSO.ItemRarity.Epic);
        
    }

    public void PopulateArtifactSlots()
    {
        Debug.Log("PopulateArtifactSlots() called...");

        ArtifactDataSO adOne = ArtifactLibrary.Instance.GetRandomViableArtifact();
        ArtifactDataSO adTwo = ArtifactLibrary.Instance.GetRandomViableArtifact();
        ArtifactDataSO adThree = ArtifactLibrary.Instance.GetRandomViableArtifact();

        if (adTwo.artifactName == adOne.artifactName)
        {
            if (adTwo.artifactName == "Stinky Poo")
            {
               // do nothing
            }
            else
            {
                while (adTwo.artifactName == adOne.artifactName)
                {
                    adTwo = ArtifactLibrary.Instance.GetRandomViableArtifact();                    
                }
            }
        }

        if(adThree.artifactName == adOne.artifactName || adThree.artifactName == adTwo.artifactName)
        {
            if (adThree.artifactName == "Stinky Poo")
            {
                // do nothing
            }
            else
            {
                while (adThree.artifactName == adOne.artifactName || adThree.artifactName == adTwo.artifactName)
                {
                    adThree = ArtifactLibrary.Instance.GetRandomViableArtifact();
                }
            }
        }      

        artifactSlotOne.InitializeSetup(adOne);
        artifactSlotTwo.InitializeSetup(adTwo);
        artifactSlotThree.InitializeSetup(adThree);
    }

    public void OnContinueButtonClicked()
    {
        UIManager.Instance.EnableWorldMapView();
    }
}
