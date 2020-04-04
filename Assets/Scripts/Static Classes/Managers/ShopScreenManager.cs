using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScreenManager : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Component References")]
    public GameObject visualParent;
    public GameObject continueButton;

    [Header("Character References")]
    public VillageCharacter characterOne;
    public VillageCharacter characterTwo;
    public VillageCharacter characterThree;
    public VillageCharacter characterFour;

    [Header("Armoury Component References")]
    public GameObject armouryVisualParent;
    public CanvasGroup armouryScreenCg;
    public Button armouryScreenCancelButton;

    [Header("Library Component References")]
    public GameObject libraryVisualParent;
    public CanvasGroup libraryScreenCg;
    public Button libraryScreenCancelButton;

    [Header("Potion Lab Component References")]
    public GameObject potionLabVisualParent;
    public CanvasGroup potionLabScreenCg;
    public Button potionLabScreenCancelButton;

    [Header("Item Slot References")]
    public ItemSlot itemSlotOne;
    public ItemSlot itemSlotTwo;
    public ItemSlot itemSlotThree;
    public ItemSlot itemSlotFour;
    public ItemSlot itemSlotFive;
    public ItemSlot itemSlotSix;
    public ItemSlot itemSlotSeven;
    public ItemSlot itemSlotEight;

    [Header("Consumable Slot References")]
    public ConsumableInShop consumableSlotOne;
    public ConsumableInShop consumableSlotTwo;
    public ConsumableInShop consumableSlotThree;
    public ConsumableInShop consumableSlotFour;
    public ConsumableInShop consumableSlotFive;
    public ConsumableInShop consumableSlotSix;
    public ConsumableInShop consumableSlotSeven;
    public ConsumableInShop consumableSlotEight;

    [Header("Ability Tome Slot References")]
    public AbilityTomeInShop abilityTomeSlotOne;
    public AbilityTomeInShop abilityTomeSlotTwo;
    public AbilityTomeInShop abilityTomeSlotThree;
    public AbilityTomeInShop abilityTomeSlotFour;
    public AbilityTomeInShop abilityTomeSlotFive;
    public AbilityTomeInShop abilityTomeSlotSix;
    public AbilityTomeInShop abilityTomeSlotSeven;
    public AbilityTomeInShop abilityTomeSlotEight;
    public AbilityTomeInShop abilityTomeSlotNine;
    public AbilityTomeInShop abilityTomeSlotTen;

    [Header("Artifact Slot References")]
    public ArtifactSlot artifactSlotOne;
    public ArtifactSlot artifactSlotTwo;
    public ArtifactSlot artifactSlotThree;

    [Header("Properties")]
    public List<ItemDataSO> itemsInShopData;
    public List<ItemSlot> allItemsSlots;
    public float windowFadeSpeed;
    #endregion

    // Initialization + Setup
    #region
    public static ShopScreenManager Instance;
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

    // Enable + Disable Main View
    public void EnableShopScreenView()
    {
        visualParent.SetActive(true);
    }
    public void DisableShopScreenView()
    {
        visualParent.SetActive(false);
    }


    // Enable specific windows
    public void EnableArmouryScreenView()
    {
        StartCoroutine(EnableArmouryScreenViewCoroutine());
    }
    private IEnumerator EnableArmouryScreenViewCoroutine()
    {
        // Enable parent view
        armouryVisualParent.SetActive(true);

        // Fade in screen
        armouryScreenCg.alpha = 0;
        while (armouryScreenCg.alpha < 1)
        {
            armouryScreenCg.alpha += 0.25f * windowFadeSpeed * Time.deltaTime;            
            yield return new WaitForEndOfFrame();
        }

        // Enable cancel button
        armouryScreenCancelButton.interactable = true;
    }
    public void EnableLibraryScreenView()
    {
        StartCoroutine(EnableLibraryScreenViewCoroutine());
    }
    private IEnumerator EnableLibraryScreenViewCoroutine()
    {
        // Enable parent view
        libraryVisualParent.SetActive(true);

        // Fade in screen
        libraryScreenCg.alpha = 0;
        while (libraryScreenCg.alpha < 1)
        {
            libraryScreenCg.alpha += 0.25f * windowFadeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Enable cancel button
        libraryScreenCancelButton.interactable = true;
    }
    public void EnablePotionLabScreenView()
    {
        StartCoroutine(EnablePotionLabScreenViewCoroutine());
    }
    private IEnumerator EnablePotionLabScreenViewCoroutine()
    {
        // Enable parent view
        potionLabVisualParent.SetActive(true);

        // Fade in screen
        potionLabScreenCg.alpha = 0;
        while (potionLabScreenCg.alpha < 1)
        {
            potionLabScreenCg.alpha += 0.25f * windowFadeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Enable cancel button
        potionLabScreenCancelButton.interactable = true;
    }


    // Disable specific windows
    public void DisableArmouryScreenView()
    {
        StartCoroutine(DisableArmouryScreenViewCoroutine());
    }
    private IEnumerator DisableArmouryScreenViewCoroutine()
    {
        // Disable cancel button
        armouryScreenCancelButton.interactable = false;

        // Fade out screen
        armouryScreenCg.alpha = 1;
        while (armouryScreenCg.alpha > 0)
        {
            armouryScreenCg.alpha -= 0.25f * windowFadeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }               

        // Disable parent view
        armouryVisualParent.SetActive(false);
    }
    public void DisableLibraryScreenView()
    {
        StartCoroutine(DisableLibraryScreenViewCoroutine());
    }
    private IEnumerator DisableLibraryScreenViewCoroutine()
    {
        // Disable cancel button
        libraryScreenCancelButton.interactable = false;

        // Fade out screen
        libraryScreenCg.alpha = 1;
        while (libraryScreenCg.alpha > 0)
        {
            libraryScreenCg.alpha -= 0.25f * windowFadeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Disable parent view
        libraryVisualParent.SetActive(false);
    }
    public void DisablePotionLabScreenView()
    {
        StartCoroutine(DisablePotionLabScreenViewCoroutine());
    }
    private IEnumerator DisablePotionLabScreenViewCoroutine()
    {
        // Disable cancel button
        potionLabScreenCancelButton.interactable = false;

        // Fade out screen
        potionLabScreenCg.alpha = 1;
        while (potionLabScreenCg.alpha > 0)
        {
            potionLabScreenCg.alpha -= 0.25f * windowFadeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Disable parent view
        potionLabVisualParent.SetActive(false);
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
        PopulateConsumableSlots();
        PopulateAbilityTomeSlots();
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
    public void PopulateConsumableSlots()
    {
        consumableSlotOne.BuildFromData(ConsumableLibrary.Instance.GetRandomConsumable());
        consumableSlotTwo.BuildFromData(ConsumableLibrary.Instance.GetRandomConsumable());
        consumableSlotThree.BuildFromData(ConsumableLibrary.Instance.GetRandomConsumable());
        consumableSlotFour.BuildFromData(ConsumableLibrary.Instance.GetRandomConsumable());
        consumableSlotFive.BuildFromData(ConsumableLibrary.Instance.GetRandomConsumable());
        consumableSlotSix.BuildFromData(ConsumableLibrary.Instance.GetRandomConsumable());
        consumableSlotSeven.BuildFromData(ConsumableLibrary.Instance.GetRandomConsumable());
        consumableSlotEight.BuildFromData(ConsumableLibrary.Instance.GetRandomConsumable());
    }
    public void PopulateAbilityTomeSlots()
    {
        abilityTomeSlotOne.BuildFromData(AbilityLibrary.Instance.GetRandomValidAbilityTomeAbility());
        abilityTomeSlotTwo.BuildFromData(AbilityLibrary.Instance.GetRandomValidAbilityTomeAbility());
        abilityTomeSlotThree.BuildFromData(AbilityLibrary.Instance.GetRandomValidAbilityTomeAbility());
        abilityTomeSlotFour.BuildFromData(AbilityLibrary.Instance.GetRandomValidAbilityTomeAbility());
        abilityTomeSlotFive.BuildFromData(AbilityLibrary.Instance.GetRandomValidAbilityTomeAbility());
        abilityTomeSlotSix.BuildFromData(AbilityLibrary.Instance.GetRandomValidAbilityTomeAbility());
        abilityTomeSlotSeven.BuildFromData(AbilityLibrary.Instance.GetRandomValidAbilityTomeAbility());
        abilityTomeSlotEight.BuildFromData(AbilityLibrary.Instance.GetRandomValidAbilityTomeAbility());
        abilityTomeSlotNine.BuildFromData(AbilityLibrary.Instance.GetRandomValidAbilityTomeAbility());
        abilityTomeSlotTen.BuildFromData(AbilityLibrary.Instance.GetRandomValidAbilityTomeAbility());
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

    // Characters 
    #region
    public void SetUpVillageCharacter(VillageCharacter characterSlot, CharacterData characterData)
    {
        characterSlot.InitializeSetup(characterData);
    }
    #endregion
}
