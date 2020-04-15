    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardScreen : MonoBehaviour
{
    [Header("Properties")]
    public float screenFadeTime;
    public int itemCardSortingOrder;

    [Header("Canvas Groups")]
    public CanvasGroup commonItemRewardCG;
    public CanvasGroup rareItemRewardCG;
    public CanvasGroup epicItemRewardCG;
    public CanvasGroup stateRewardCG;
    public CanvasGroup frontPageCG;
    public CanvasGroup masterCG;    
    public CanvasGroup blackScreenCG;

    [Header("View Object References")]
    public GameObject RewardButtonParent;
    public GameObject SkipRewardsButton;
    public GameObject ChooseCommonItemScreenParent;
    public GameObject ChooseRareItemScreenParent;
    public GameObject ChooseEpicItemScreenParent;
    public GameObject ChooseCommonItemScreenContent;
    public GameObject ChooseRareItemScreenContent;
    public GameObject ChooseEpicItemScreenContent;
    public GameObject ChooseStateScreenParent;
    public GameObject ChooseStateScreenContent;
    public GameObject RewardScreenParent;
    public GameObject BlackScreenParent;   

    [Header("Current Button References")]
    public GameObject currentGoldRewardButton;
    public GameObject currentCommonItemRewardButton;
    public GameObject currentRareItemRewardButton;
    public GameObject currentEpicItemRewardButton;
    public GameObject currentStateRewardButton;
    public GameObject currentConsumableRewardButton;

    [Header("Current Item References")]
    public GameObject currentCommonItemOne;
    public GameObject currentCommonItemTwo;
    public GameObject currentCommonItemThree;

    [Header("Current Rare Item References")]
    public GameObject currentRareItemOne;
    public GameObject currentRareItemTwo;
    public GameObject currentRareItemThree;

    [Header("Current Epic Item References")]
    public GameObject currentEpicItemOne;
    public GameObject currentEpicItemTwo;
    public GameObject currentEpicItemThree;

    [Header("Current State References")]
    public GameObject currentStateOne;
    public GameObject currentStateTwo;
    public GameObject currentStateThree;

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
        Debug.Log("RewardScreen.CreateGoldRewardButton() called...");
        currentGoldRewardButton = Instantiate(PrefabHolder.Instance.GoldRewardButton, RewardButtonParent.transform);
        GoldRewardButton gwrButton = currentGoldRewardButton.GetComponent<GoldRewardButton>();
        gwrButton.InitializeSetup();
    }
    public void CreateConsumableRewardButton()
    {
        Debug.Log("RewardScreen.CreateConsumableRewardButton() called...");
        currentConsumableRewardButton = Instantiate(PrefabHolder.Instance.ConsumableRewardButton, RewardButtonParent.transform);
        ConsumableRewardButton crButton = currentConsumableRewardButton.GetComponent<ConsumableRewardButton>();
        crButton.InitializeSetup();

    }
    public void CreateStateRewardButton()
    {
        Debug.Log("RewardScreen.CreateStateRewardButton() called...");
        currentStateRewardButton = Instantiate(PrefabHolder.Instance.stateRewardButton, RewardButtonParent.transform);       
    }
    public void CreateCommonItemRewardButton()
    {
        Debug.Log("RewardScreen.CreateCommonItemRewardButton() called...");
        currentCommonItemRewardButton = Instantiate(PrefabHolder.Instance.ItemRewardButton, RewardButtonParent.transform);
        currentCommonItemRewardButton.GetComponent<ItemRewardButton>().InitializeSetup(ItemRewardButton.RarityReward.Common);
    }
    public void CreateRareItemRewardButton()
    {
        Debug.Log("RewardScreen.CreateRareItemRewardButton() called...");
        currentRareItemRewardButton = Instantiate(PrefabHolder.Instance.ItemRewardButton, RewardButtonParent.transform);
        currentRareItemRewardButton.GetComponent<ItemRewardButton>().InitializeSetup(ItemRewardButton.RarityReward.Rare);
    }
    public void CreateEpicItemRewardButton()
    {
        Debug.Log("RewardScreen.CreateEpicItemRewardButton() called...");
        currentEpicItemRewardButton = Instantiate(PrefabHolder.Instance.ItemRewardButton, RewardButtonParent.transform);
        currentEpicItemRewardButton.GetComponent<ItemRewardButton>().InitializeSetup(ItemRewardButton.RarityReward.Epic);
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

        if (currentCommonItemRewardButton != null)
        {
            Destroy(currentCommonItemRewardButton);
        }

        if (currentRareItemRewardButton != null)
        {
            Destroy(currentCommonItemRewardButton);
        }

        if (currentEpicItemRewardButton != null)
        {
            Destroy(currentCommonItemRewardButton);
        }

        if (currentStateRewardButton != null)
        {
            Destroy(currentStateRewardButton);
        }
        if (currentConsumableRewardButton != null)
        {
            Destroy(currentStateRewardButton);
        }

        DestroyAllCommonItemCards();
        DestroyAllRareItemCards();
        DestroyAllEpicItemCards();
        DestroyAllStateCards();
    }
    public void PopulateStateRewardScreen()
    {
        Debug.Log("RewardScreen.PopulateStateRewardScreen() called...");

        // Generate random state for state reward 1
        GameObject newStateOne = Instantiate(PrefabHolder.Instance.StateCard, ChooseStateScreenContent.transform);
        StateCard stateOne = newStateOne.GetComponent<StateCard>();
        stateOne.BuildFromStateData(StateLibrary.Instance.GetRandomStateReward());
        currentStateOne = newStateOne;

        // Generate random state for state reward 2
        GameObject newStateTwo = Instantiate(PrefabHolder.Instance.StateCard, ChooseStateScreenContent.transform);
        StateCard stateTwo = newStateTwo.GetComponent<StateCard>();
        stateTwo.BuildFromStateData(StateLibrary.Instance.GetRandomStateReward());
        currentStateTwo = newStateTwo;

        // Prevent duplicate items appearing as reward: Reroll item until we get a non-duplicate
        while (stateTwo.myStateData.stateName == stateOne.myStateData.stateName)
        {
            Debug.Log("RewardScreen.PopulateStateScreen() detected a duplicate state reward, rerolling state two...");
            stateTwo.BuildFromStateData(StateLibrary.Instance.GetRandomStateReward());
        }

        // Generate random state for state reward 3
        GameObject newStateThree = Instantiate(PrefabHolder.Instance.StateCard, ChooseStateScreenContent.transform);
        StateCard stateThree = newStateThree.GetComponent<StateCard>();
        stateThree.BuildFromStateData(StateLibrary.Instance.GetRandomStateReward());
        currentStateThree = newStateThree;

        // Prevent duplicate items appearing as reward: Reroll item until we get a non-duplicate
        while (stateThree.myStateData.stateName == stateOne.myStateData.stateName ||
               stateThree.myStateData.stateName == stateTwo.myStateData.stateName)
        {
            Debug.Log("RewardScreen.PopulateStateScreen() detected a duplicate state reward, rerolling state three...");
            stateThree.BuildFromStateData(StateLibrary.Instance.GetRandomStateReward());
        }
    }
    public void PopulateBossStateRewardScreen()
    {
        Debug.Log("RewardScreen.PopulateBossStateRewardScreen() called...");

        // Generate random state for state reward 1
        GameObject newStateOne = Instantiate(PrefabHolder.Instance.StateCard, ChooseStateScreenContent.transform);
        StateCard stateOne = newStateOne.GetComponent<StateCard>();
        stateOne.BuildFromStateData(StateLibrary.Instance.GetRandomBossState());
        currentStateOne = newStateOne;

        // Generate random state for state reward 2
        GameObject newStateTwo = Instantiate(PrefabHolder.Instance.StateCard, ChooseStateScreenContent.transform);
        StateCard stateTwo = newStateTwo.GetComponent<StateCard>();
        stateTwo.BuildFromStateData(StateLibrary.Instance.GetRandomBossState());
        currentStateTwo = newStateTwo;

        // Prevent duplicate items appearing as reward: Reroll item until we get a non-duplicate
        while (stateTwo.myStateData.stateName == stateOne.myStateData.stateName)
        {
            Debug.Log("RewardScreen.PopulateStateScreen() detected a duplicate state reward, rerolling state two...");
            stateTwo.BuildFromStateData(StateLibrary.Instance.GetRandomBossState());
        }

        // Generate random state for state reward 3
        GameObject newStateThree = Instantiate(PrefabHolder.Instance.StateCard, ChooseStateScreenContent.transform);
        StateCard stateThree = newStateThree.GetComponent<StateCard>();
        stateThree.BuildFromStateData(StateLibrary.Instance.GetRandomBossState());
        currentStateThree = newStateThree;

        // Prevent duplicate items appearing as reward: Reroll item until we get a non-duplicate
        while (stateThree.myStateData.stateName == stateOne.myStateData.stateName ||
               stateThree.myStateData.stateName == stateTwo.myStateData.stateName)
        {
            Debug.Log("RewardScreen.PopulateStateScreen() detected a duplicate state reward, rerolling state three...");
            stateThree.BuildFromStateData(StateLibrary.Instance.GetRandomBossState());
        }
    }
    public void PopulateItemScreen()
    {
        if(EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.BasicEnemy)
        {
            PopulateCommonItemScreen();
        }

        else if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.EliteEnemy)
        {
            PopulateCommonItemScreen();
            PopulateRareItemScreen();
        }
        else if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.Boss)
        {
            PopulateCommonItemScreen();
            PopulateRareItemScreen();
            PopulateEpicItemScreen();
        }

    }
    public void PopulateCommonItemScreen()
    {
        // Generate random item for item reward 1
        GameObject newItemOne = Instantiate(PrefabHolder.Instance.ItemCard, ChooseCommonItemScreenContent.transform);
        ItemCard itemOne = newItemOne.GetComponent<ItemCard>();
        itemOne.RunSetupFromItemData(ItemLibrary.Instance.GetRandomCommonItem(), itemCardSortingOrder);
        currentCommonItemOne = newItemOne;

        // Generate random item for item reward 2
        GameObject newItemTwo = Instantiate(PrefabHolder.Instance.ItemCard, ChooseCommonItemScreenContent.transform);
        ItemCard itemTwo = newItemTwo.GetComponent<ItemCard>();
        itemTwo.RunSetupFromItemData(ItemLibrary.Instance.GetRandomCommonItem(), itemCardSortingOrder);
        currentCommonItemTwo = newItemTwo;

        // Prevent duplicate items appearing as reward: Reroll item until we get a non-duplicate
        while (itemTwo.myName == itemOne.myName)
        {
            Debug.Log("RewardScreen.PopulateItemScreen() detected a duplicate item reward, rerolling item two");
            itemTwo.RunSetupFromItemData(ItemLibrary.Instance.GetRandomCommonItem(), itemCardSortingOrder);
            currentCommonItemTwo = newItemTwo;
        }

        // Generate random item for item reward 3
        GameObject newItemThree = Instantiate(PrefabHolder.Instance.ItemCard, ChooseCommonItemScreenContent.transform);
        ItemCard itemThree = newItemThree.GetComponent<ItemCard>();
        itemThree.RunSetupFromItemData(ItemLibrary.Instance.GetRandomCommonItem(), itemCardSortingOrder);
        currentCommonItemThree = newItemThree;

        // Prevent duplicate items appearing as reward: Reroll item until we get a non-duplicate
        while (itemThree.myName == itemOne.myName ||
            itemThree.myName == itemTwo.myName)
        {
            Debug.Log("RewardScreen.PopulateItemScreen() detected a duplicate item reward, rerolling item three");
            itemThree.RunSetupFromItemData(ItemLibrary.Instance.GetRandomCommonItem(), itemCardSortingOrder);
            currentCommonItemThree = newItemThree;
        }

        itemOne.location = ItemCard.Location.LootScreen;
        itemTwo.location = ItemCard.Location.LootScreen;
        itemThree.location = ItemCard.Location.LootScreen;
    }
    public void PopulateRareItemScreen()
    {
        // Generate random item for item reward 1
        GameObject newItemOne = Instantiate(PrefabHolder.Instance.ItemCard, ChooseRareItemScreenContent.transform);
        ItemCard itemOne = newItemOne.GetComponent<ItemCard>();
        itemOne.RunSetupFromItemData(ItemLibrary.Instance.GetRandomRareItem(), itemCardSortingOrder);
        currentRareItemOne = newItemOne;

        // Generate random item for item reward 2
        GameObject newItemTwo = Instantiate(PrefabHolder.Instance.ItemCard, ChooseRareItemScreenContent.transform);
        ItemCard itemTwo = newItemTwo.GetComponent<ItemCard>();
        itemTwo.RunSetupFromItemData(ItemLibrary.Instance.GetRandomRareItem(), itemCardSortingOrder);
        currentRareItemTwo = newItemTwo;

        // Prevent duplicate items appearing as reward: Reroll item until we get a non-duplicate
        while (itemTwo.myName == itemOne.myName)
        {
            Debug.Log("RewardScreen.PopulateItemScreen() detected a duplicate item reward, rerolling item two");
            itemTwo.RunSetupFromItemData(ItemLibrary.Instance.GetRandomRareItem(), itemCardSortingOrder);
            currentRareItemTwo = newItemTwo;
        }

        // Generate random item for item reward 3
        GameObject newItemThree = Instantiate(PrefabHolder.Instance.ItemCard, ChooseRareItemScreenContent.transform);
        ItemCard itemThree = newItemThree.GetComponent<ItemCard>();
        itemThree.RunSetupFromItemData(ItemLibrary.Instance.GetRandomRareItem(), itemCardSortingOrder);
        currentRareItemThree = newItemThree;

        // Prevent duplicate items appearing as reward: Reroll item until we get a non-duplicate
        while (itemThree.myName == itemOne.myName ||
            itemThree.myName == itemTwo.myName)
        {
            Debug.Log("RewardScreen.PopulateItemScreen() detected a duplicate item reward, rerolling item three");
            itemThree.RunSetupFromItemData(ItemLibrary.Instance.GetRandomRareItem(), itemCardSortingOrder);
            currentRareItemThree = newItemThree;
        }

        itemOne.location = ItemCard.Location.LootScreen;
        itemTwo.location = ItemCard.Location.LootScreen;
        itemThree.location = ItemCard.Location.LootScreen;
    }
    public void PopulateEpicItemScreen()
    {
        // Generate random item for item reward 1
        GameObject newItemOne = Instantiate(PrefabHolder.Instance.ItemCard, ChooseEpicItemScreenContent.transform);
        ItemCard itemOne = newItemOne.GetComponent<ItemCard>();
        itemOne.RunSetupFromItemData(ItemLibrary.Instance.GetRandomEpicItem(), itemCardSortingOrder);
        currentEpicItemOne = newItemOne;

        // Generate random item for item reward 2
        GameObject newItemTwo = Instantiate(PrefabHolder.Instance.ItemCard, ChooseEpicItemScreenContent.transform);
        ItemCard itemTwo = newItemTwo.GetComponent<ItemCard>();
        itemTwo.RunSetupFromItemData(ItemLibrary.Instance.GetRandomEpicItem(), itemCardSortingOrder);
        currentEpicItemTwo = newItemTwo;

        // Prevent duplicate items appearing as reward: Reroll item until we get a non-duplicate
        while (itemTwo.myName == itemOne.myName)
        {
            Debug.Log("RewardScreen.PopulateItemScreen() detected a duplicate item reward, rerolling item two");
            itemTwo.RunSetupFromItemData(ItemLibrary.Instance.GetRandomEpicItem(), itemCardSortingOrder);
            currentEpicItemTwo = newItemTwo;
        }

        // Generate random item for item reward 3
        GameObject newItemThree = Instantiate(PrefabHolder.Instance.ItemCard, ChooseEpicItemScreenContent.transform);
        ItemCard itemThree = newItemThree.GetComponent<ItemCard>();
        itemThree.RunSetupFromItemData(ItemLibrary.Instance.GetRandomEpicItem(), itemCardSortingOrder);
        currentEpicItemThree = newItemThree;

        // Prevent duplicate items appearing as reward: Reroll item until we get a non-duplicate
        while (itemThree.myName == itemOne.myName ||
            itemThree.myName == itemTwo.myName)
        {
            Debug.Log("RewardScreen.PopulateItemScreen() detected a duplicate item reward, rerolling item three");
            itemThree.RunSetupFromItemData(ItemLibrary.Instance.GetRandomEpicItem(), itemCardSortingOrder);
            currentEpicItemThree = newItemThree;
        }

        itemOne.location = ItemCard.Location.LootScreen;
        itemTwo.location = ItemCard.Location.LootScreen;
        itemThree.location = ItemCard.Location.LootScreen;
    }
    public void DestroyAllCommonItemCards()
    {
        Destroy(currentCommonItemOne);
        currentCommonItemOne = null;

        Destroy(currentCommonItemTwo);
        currentCommonItemTwo = null;

        Destroy(currentCommonItemThree);
        currentCommonItemThree = null;
    }
    public void DestroyAllRareItemCards()
    {
        Destroy(currentRareItemOne);
        currentRareItemOne = null;

        Destroy(currentRareItemTwo);
        currentRareItemTwo = null;

        Destroy(currentRareItemThree);
        currentRareItemThree = null;
    }
    public void DestroyAllEpicItemCards()
    {
        Destroy(currentEpicItemOne);
        currentEpicItemOne = null;

        Destroy(currentEpicItemTwo);
        currentEpicItemTwo = null;

        Destroy(currentEpicItemThree);
        currentEpicItemThree = null;
    }
    public void DestroyAllStateCards()
    {
        Destroy(currentStateOne);
        currentStateOne = null;

        Destroy(currentStateTwo);
        currentStateTwo = null;

        Destroy(currentStateThree);
        currentStateThree = null;
    }
    #endregion

    // Mouse + Click Events
    #region
    public void OnSkipRewardsButtonClicked()
    {
        Debug.Log("RewardScreen.OnSkipRewardsButtonClicked() called...");
        EventManager.Instance.EndNewLootRewardEvent();
    }
    #endregion

    // Visibility + View Logic
    #region
    public void EnableCommonItemLootScreen()
    {
        StartCoroutine(EnableCommonItemLootScreenCoroutine());
    }
    private IEnumerator EnableCommonItemLootScreenCoroutine()
    {
        RewardButtonParent.SetActive(false);
        SkipRewardsButton.SetActive(false);
        ChooseCommonItemScreenParent.SetActive(true);

        commonItemRewardCG.alpha = 0;
        frontPageCG.alpha = 1;

        while (commonItemRewardCG.alpha < 1)
        {
            commonItemRewardCG.alpha += 0.2f * Time.deltaTime * screenFadeTime;
            frontPageCG.alpha -= 0.2f * Time.deltaTime * screenFadeTime;
            yield return new WaitForEndOfFrame();
        }
    }
    public void EnableRareItemLootScreen()
    {
        StartCoroutine(EnableRareItemLootScreenCoroutine());
    }
    private IEnumerator EnableRareItemLootScreenCoroutine()
    {
        RewardButtonParent.SetActive(false);
        SkipRewardsButton.SetActive(false);
        ChooseRareItemScreenParent.SetActive(true);

        rareItemRewardCG.alpha = 0;
        frontPageCG.alpha = 1;

        while (rareItemRewardCG.alpha < 1)
        {
            rareItemRewardCG.alpha += 0.2f * Time.deltaTime * screenFadeTime;
            frontPageCG.alpha -= 0.2f * Time.deltaTime * screenFadeTime;
            yield return new WaitForEndOfFrame();
        }
    }
    public void EnableEpicItemLootScreen()
    {
        StartCoroutine(EnableEpicItemLootScreenCoroutine());
    }
    private IEnumerator EnableEpicItemLootScreenCoroutine()
    {
        RewardButtonParent.SetActive(false);
        SkipRewardsButton.SetActive(false);
        ChooseEpicItemScreenParent.SetActive(true);

        epicItemRewardCG.alpha = 0;
        frontPageCG.alpha = 1;

        while (epicItemRewardCG.alpha < 1)
        {
            epicItemRewardCG.alpha += 0.2f * Time.deltaTime * screenFadeTime;
            frontPageCG.alpha -= 0.2f * Time.deltaTime * screenFadeTime;
            yield return new WaitForEndOfFrame();
        }
    }
    public void EnableChooseStateRewardScreen()
    {
        StartCoroutine(EnableChooseStateRewardScreenCoroutine());
    }
    public IEnumerator EnableChooseStateRewardScreenCoroutine()
    {
        RewardButtonParent.SetActive(false);
        SkipRewardsButton.SetActive(false);
        ChooseStateScreenParent.SetActive(true);

        stateRewardCG.alpha = 0;
        frontPageCG.alpha = 1;

        while (stateRewardCG.alpha < 1)
        {
            stateRewardCG.alpha += 0.2f * Time.deltaTime * screenFadeTime;
            frontPageCG.alpha -= 0.2f * Time.deltaTime * screenFadeTime;
            yield return new WaitForEndOfFrame();
        }

    }
    public void DisableCommonItemLootScreen()
    {
        StartCoroutine(DisableCommonItemLootScreenCoroutine());
    }
    public IEnumerator DisableCommonItemLootScreenCoroutine()
    {
        RewardButtonParent.SetActive(true);
        SkipRewardsButton.SetActive(true);
        ChooseCommonItemScreenParent.SetActive(false);

        commonItemRewardCG.alpha = 1;
        frontPageCG.alpha = 0;

        while (commonItemRewardCG.alpha > 0)
        {
            commonItemRewardCG.alpha -= 0.2f * Time.deltaTime * screenFadeTime;
            frontPageCG.alpha += 0.2f * Time.deltaTime * screenFadeTime;
            yield return new WaitForEndOfFrame();
        }
    }
    public void DisableRareItemLootScreen()
    {
        StartCoroutine(DisableRareItemLootScreenCoroutine());
    }
    public IEnumerator DisableRareItemLootScreenCoroutine()
    {
        RewardButtonParent.SetActive(true);
        SkipRewardsButton.SetActive(true);
        ChooseRareItemScreenParent.SetActive(false);

        rareItemRewardCG.alpha = 1;
        frontPageCG.alpha = 0;

        while (rareItemRewardCG.alpha > 0)
        {
            rareItemRewardCG.alpha -= 0.2f * Time.deltaTime * screenFadeTime;
            frontPageCG.alpha += 0.2f * Time.deltaTime * screenFadeTime;
            yield return new WaitForEndOfFrame();
        }
    }
    public void DisableEpicItemLootScreen()
    {
        StartCoroutine(DisableEpicItemLootScreenCoroutine());
    }
    public IEnumerator DisableEpicItemLootScreenCoroutine()
    {
        RewardButtonParent.SetActive(true);
        SkipRewardsButton.SetActive(true);
        ChooseEpicItemScreenParent.SetActive(false);

        epicItemRewardCG.alpha = 1;
        frontPageCG.alpha = 0;

        while (epicItemRewardCG.alpha > 0)
        {
            epicItemRewardCG.alpha -= 0.2f * Time.deltaTime * screenFadeTime;
            frontPageCG.alpha += 0.2f * Time.deltaTime * screenFadeTime;
            yield return new WaitForEndOfFrame();
        }
    }
    public void DisableStateRewardScreen()
    {
        StartCoroutine(DisableStateRewardScreenCoroutine());
    }
    public IEnumerator DisableStateRewardScreenCoroutine()
    {
        RewardButtonParent.SetActive(true);
        SkipRewardsButton.SetActive(true);
        ChooseStateScreenParent.SetActive(false);

        stateRewardCG.alpha = 1;
        frontPageCG.alpha = 0;

        while (stateRewardCG.alpha > 0)
        {
            stateRewardCG.alpha -= 0.2f * Time.deltaTime * screenFadeTime;
            frontPageCG.alpha += 0.2f * Time.deltaTime * screenFadeTime;
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
            masterCG.alpha += 0.2f * Time.deltaTime * 10;
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
            masterCG.alpha -= 0.2f * Time.deltaTime * 15;
            yield return new WaitForEndOfFrame();
        }

        RewardScreenParent.SetActive(false);
    }
    #endregion
    

}
