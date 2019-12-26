using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardScreen : MonoBehaviour
{
    [Header("Canvas Groups")]
    public CanvasGroup itemRewardCG;
    public CanvasGroup frontPageCG;
    public CanvasGroup masterCG;    
    public CanvasGroup blackScreenCG;

    [Header("View Object References")]
    public GameObject RewardButtonParent;
    public GameObject SkipRewardsButton;
    public GameObject Background;
    public GameObject TitleImage;
    public GameObject ChooseItemScreenParent;
    public GameObject ChooseItemScreenContent;
    public GameObject RewardScreenParent;
    public GameObject BlackScreenParent;   

    [Header("Current Button References")]
    public GameObject currentGoldRewardButton;
    public GameObject currentItemRewardButton;
    public GameObject currentArtifactRewardButton;

    [Header("Current Item References")]
    public GameObject currentItemOne;
    public GameObject currentItemTwo;
    public GameObject currentItemThree;

    // Initialization + Singleton Pattern
    #region
    public static RewardScreen Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Create Buttons + Game Objects
    #region
    public void CreateGoldRewardButton()
    {
        Debug.Log("CreateGoldRewardButton() called...");
        currentGoldRewardButton = Instantiate(PrefabHolder.Instance.GoldRewardButton, RewardButtonParent.transform);
        GoldRewardButton gwrButton = currentGoldRewardButton.GetComponent<GoldRewardButton>();
        gwrButton.InitializeSetup();
    }
    public void CreateArtifactRewardButton()
    {
        Debug.Log("CreateArtifactRewardButton() called...");
        currentArtifactRewardButton = Instantiate(PrefabHolder.Instance.ArtifactRewardButton, RewardButtonParent.transform);
        ArtifactRewardButton arButton = currentArtifactRewardButton.GetComponent<ArtifactRewardButton>();
        arButton.InitializeSetup();         
    }
    public void CreateItemRewardButton()
    {
        Debug.Log("CreateItemRewardButton() called...");
        currentItemRewardButton = Instantiate(PrefabHolder.Instance.ItemRewardButton, RewardButtonParent.transform);
    }
    #endregion

    // Populate + Destroy/Clear Screen Elements
    #region
    public void ClearRewards()
    {
        if (currentGoldRewardButton != null)
        {
            Destroy(currentGoldRewardButton);
        }

        if (currentItemRewardButton != null)
        {
            Destroy(currentItemRewardButton);
        }
        if (currentArtifactRewardButton != null)
        {
            Destroy(currentArtifactRewardButton);
        }

        DestroyAllItemCards();
    }
    public void PopulateItemScreen()
    {
        if(EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.BasicEnemy)
        {
            // Generate random item for item reward 1
            GameObject newItemOne = Instantiate(PrefabHolder.Instance.ItemCard, ChooseItemScreenContent.transform);
            ItemCard itemOne = newItemOne.GetComponent<ItemCard>();
            itemOne.RunSetupFromItemData(ItemLibrary.Instance.GetRandomCommonItem());
            currentItemOne = newItemOne;

            // Generate random item for item reward 2
            GameObject newItemTwo = Instantiate(PrefabHolder.Instance.ItemCard, ChooseItemScreenContent.transform);
            ItemCard itemTwo = newItemTwo.GetComponent<ItemCard>();
            itemTwo.RunSetupFromItemData(ItemLibrary.Instance.GetRandomCommonItem());
            currentItemTwo = newItemTwo;

            // Prevent duplicate items appearing as reward: Reroll item until we get a non-duplicate
            while (itemTwo.myName == itemOne.myName)
            {
                Debug.Log("RewardScreen.PopulateItemScreen() detected a duplicate item reward, rerolling item two");
                itemTwo.RunSetupFromItemData(ItemLibrary.Instance.GetRandomCommonItem());
                currentItemTwo = newItemTwo;
            }

            // Generate random item for item reward 3
            GameObject newItemThree = Instantiate(PrefabHolder.Instance.ItemCard, ChooseItemScreenContent.transform);
            ItemCard itemThree = newItemThree.GetComponent<ItemCard>();
            itemThree.RunSetupFromItemData(ItemLibrary.Instance.GetRandomCommonItem());
            currentItemThree = newItemThree;

            // Prevent duplicate items appearing as reward: Reroll item until we get a non-duplicate
            while (itemThree.myName == itemOne.myName ||
                itemThree.myName == itemTwo.myName)
            {
                Debug.Log("RewardScreen.PopulateItemScreen() detected a duplicate item reward, rerolling item three");
                itemThree.RunSetupFromItemData(ItemLibrary.Instance.GetRandomCommonItem());
                currentItemThree = newItemThree;
            }
        }

        else if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.EliteEnemy)
        {
            // Generate random item for item reward 1
            GameObject newItemOne = Instantiate(PrefabHolder.Instance.ItemCard, ChooseItemScreenContent.transform);
            ItemCard itemOne = newItemOne.GetComponent<ItemCard>();
            itemOne.RunSetupFromItemData(ItemLibrary.Instance.GetRandomRareItem());
            currentItemOne = newItemOne;

            // Generate random item for item reward 2
            GameObject newItemTwo = Instantiate(PrefabHolder.Instance.ItemCard, ChooseItemScreenContent.transform);
            ItemCard itemTwo = newItemTwo.GetComponent<ItemCard>();
            itemTwo.RunSetupFromItemData(ItemLibrary.Instance.GetRandomRareItem());
            currentItemTwo = newItemTwo;

            // Prevent duplicate items appearing as reward: Reroll item until we get a non-duplicate
            while (itemTwo.myName == itemOne.myName)
            {
                Debug.Log("RewardScreen.PopulateItemScreen() detected a duplicate item reward, rerolling item two");
                itemTwo.RunSetupFromItemData(ItemLibrary.Instance.GetRandomRareItem());
                currentItemTwo = newItemTwo;
            }

            // Generate random item for item reward 3
            GameObject newItemThree = Instantiate(PrefabHolder.Instance.ItemCard, ChooseItemScreenContent.transform);
            ItemCard itemThree = newItemThree.GetComponent<ItemCard>();
            itemThree.RunSetupFromItemData(ItemLibrary.Instance.GetRandomRareItem());
            currentItemThree = newItemThree;

            // Prevent duplicate items appearing as reward: Reroll item until we get a non-duplicate
            while (itemThree.myName == itemOne.myName ||
                itemThree.myName == itemTwo.myName)
            {
                Debug.Log("RewardScreen.PopulateItemScreen() detected a duplicate item reward, rerolling item three");
                itemThree.RunSetupFromItemData(ItemLibrary.Instance.GetRandomRareItem());
                currentItemThree = newItemThree;
            }
        }
        else if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.Boss)
        {
            // Generate random item for item reward 1
            GameObject newItemOne = Instantiate(PrefabHolder.Instance.ItemCard, ChooseItemScreenContent.transform);
            ItemCard itemOne = newItemOne.GetComponent<ItemCard>();
            itemOne.RunSetupFromItemData(ItemLibrary.Instance.GetRandomEpicItem());
            currentItemOne = newItemOne;

            // Generate random item for item reward 2
            GameObject newItemTwo = Instantiate(PrefabHolder.Instance.ItemCard, ChooseItemScreenContent.transform);
            ItemCard itemTwo = newItemTwo.GetComponent<ItemCard>();
            itemTwo.RunSetupFromItemData(ItemLibrary.Instance.GetRandomEpicItem());
            currentItemTwo = newItemTwo;

            // Prevent duplicate items appearing as reward: Reroll item until we get a non-duplicate
            while (itemTwo.myName == itemOne.myName)
            {
                Debug.Log("RewardScreen.PopulateItemScreen() detected a duplicate item reward, rerolling item two");
                itemTwo.RunSetupFromItemData(ItemLibrary.Instance.GetRandomEpicItem());
                currentItemTwo = newItemTwo;
            }

            // Generate random item for item reward 3
            GameObject newItemThree = Instantiate(PrefabHolder.Instance.ItemCard, ChooseItemScreenContent.transform);
            ItemCard itemThree = newItemThree.GetComponent<ItemCard>();
            itemThree.RunSetupFromItemData(ItemLibrary.Instance.GetRandomEpicItem());
            currentItemThree = newItemThree;

            // Prevent duplicate items appearing as reward: Reroll item until we get a non-duplicate
            while (itemThree.myName == itemOne.myName ||
                itemThree.myName == itemTwo.myName)
            {
                Debug.Log("RewardScreen.PopulateItemScreen() detected a duplicate item reward, rerolling item three");
                itemThree.RunSetupFromItemData(ItemLibrary.Instance.GetRandomEpicItem());
                currentItemThree = newItemThree;
            }
        }

    }
    public void DestroyAllItemCards()
    {
        Destroy(currentItemOne);
        currentItemOne = null;

        Destroy(currentItemTwo);
        currentItemTwo = null;

        Destroy(currentItemThree);
        currentItemThree = null;
    }
    #endregion

    // Mouse + Click Events
    #region
    public void OnSkipRewardsButtonClicked()
    {
        EventManager.Instance.EndNewLootRewardEvent();
    }
    #endregion

    // Visibility + View Logic
    #region
    public void EnableItemLootScreen()
    {
        StartCoroutine(EnableItemLootScreenCoroutine());
    }
    public IEnumerator EnableItemLootScreenCoroutine()
    {
        RewardButtonParent.SetActive(false);
        SkipRewardsButton.SetActive(false);
        Background.SetActive(false);
        TitleImage.SetActive(false);
        ChooseItemScreenParent.SetActive(true);

        itemRewardCG.alpha = 0;
        frontPageCG.alpha = 1;

        while (itemRewardCG.alpha < 1)
        {
            itemRewardCG.alpha += 0.2f;
            frontPageCG.alpha -= 0.2f;
            yield return new WaitForEndOfFrame();
        }

    }
    public void DisableItemLootScreen()
    {
        StartCoroutine(DisableItemLootScreenCoroutine());
    }
    public IEnumerator DisableItemLootScreenCoroutine()
    {
        RewardButtonParent.SetActive(true);
        SkipRewardsButton.SetActive(true);
        Background.SetActive(true);
        TitleImage.SetActive(true);
        ChooseItemScreenParent.SetActive(false);

        itemRewardCG.alpha = 1;
        frontPageCG.alpha = 0;

        while (itemRewardCG.alpha > 0)
        {
            itemRewardCG.alpha -= 0.2f;
            frontPageCG.alpha += 0.2f;
            yield return new WaitForEndOfFrame();
        }
    }
    public void EnableRewardScreenView()
    {
        StartCoroutine(EnableRewardScreenViewCoroutine());
    }
    public IEnumerator EnableRewardScreenViewCoroutine()
    {
        RewardScreenParent.SetActive(true);
        
        masterCG.alpha = 0;

        while (masterCG.alpha < 1)
        {
            masterCG.alpha += 0.2f;
            yield return new WaitForEndOfFrame();
        }
        
    }
    public void DisableRewardScreenView()
    {
        StartCoroutine(DisableRewardScreenViewCoroutine());
    }
    public IEnumerator DisableRewardScreenViewCoroutine()
    {
        masterCG.alpha = 1;

        while (masterCG.alpha > 0)
        {
            masterCG.alpha -= 0.2f;
            yield return new WaitForEndOfFrame();
        }

        RewardScreenParent.SetActive(false);
    }
    #endregion
    

}
