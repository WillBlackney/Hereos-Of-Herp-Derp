    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardScreen : MonoBehaviour
{
    [Header("Properties")]
    public float screenFadeTime;

    [Header("Canvas Groups")]
    public CanvasGroup itemRewardCG;
    public CanvasGroup stateRewardCG;
    public CanvasGroup frontPageCG;
    public CanvasGroup masterCG;    
    public CanvasGroup blackScreenCG;

    [Header("View Object References")]
    public GameObject RewardButtonParent;
    public GameObject SkipRewardsButton;
    public GameObject ChooseItemScreenParent;
    public GameObject ChooseItemScreenContent;
    public GameObject ChooseStateScreenParent;
    public GameObject ChooseStateScreenContent;
    public GameObject RewardScreenParent;
    public GameObject BlackScreenParent;   

    [Header("Current Button References")]
    public GameObject currentGoldRewardButton;
    public GameObject currentItemRewardButton;
    public GameObject currentStateRewardButton;
    public GameObject currentConsumableRewardButton;

    [Header("Current Item References")]
    public GameObject currentItemOne;
    public GameObject currentItemTwo;
    public GameObject currentItemThree;

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
    public void CreateItemRewardButton()
    {
        Debug.Log("RewardScreen.CreateItemRewardButton() called...");
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

        if (currentStateRewardButton != null)
        {
            Destroy(currentStateRewardButton);
        }
        if (currentConsumableRewardButton != null)
        {
            Destroy(currentStateRewardButton);
        }

        DestroyAllItemCards();
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
        ChooseItemScreenParent.SetActive(true);

        itemRewardCG.alpha = 0;
        frontPageCG.alpha = 1;

        while (itemRewardCG.alpha < 1)
        {
            itemRewardCG.alpha += 0.2f * Time.deltaTime * screenFadeTime;
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
    public void DisableItemLootScreen()
    {
        StartCoroutine(DisableItemLootScreenCoroutine());
    }
    public IEnumerator DisableItemLootScreenCoroutine()
    {
        RewardButtonParent.SetActive(true);
        SkipRewardsButton.SetActive(true);
        ChooseItemScreenParent.SetActive(false);

        itemRewardCG.alpha = 1;
        frontPageCG.alpha = 0;

        while (itemRewardCG.alpha > 0)
        {
            itemRewardCG.alpha -= 0.2f * Time.deltaTime * screenFadeTime;
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
