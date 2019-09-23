using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardScreen : MonoBehaviour
{
    public static RewardScreen Instance;
    private void Awake()
    {
        Instance = this;        
    }

    public GameObject RewardButtonParent;
    public GameObject SkipRewardsButton;
    public GameObject Background;
    public GameObject TitleImage;
    public GameObject ChooseItemScreenParent;
    public GameObject ChooseItemScreenContent;    

    public GameObject currentItemOne;
    public GameObject currentItemTwo;
    public GameObject currentItemThree;

    public GameObject currentGoldRewardButton;
    public GameObject currentItemRewardButton;
    public GameObject currentArtifactRewardButton;

    public void CreateGoldRewardButton()
    {
        currentGoldRewardButton = Instantiate(PrefabHolder.Instance.GoldRewardButton, RewardButtonParent.transform);
        GoldRewardButton gwrButton = currentGoldRewardButton.GetComponent<GoldRewardButton>();
        gwrButton.InitializeSetup();
    }

    public void CreateArtifactRewardButton()
    {
        currentArtifactRewardButton = Instantiate(PrefabHolder.Instance.ArtifactRewardButton, RewardButtonParent.transform);
        ArtifactRewardButton arButton = currentArtifactRewardButton.GetComponent<ArtifactRewardButton>();
        arButton.InitializeSetup();        
    }

    public void CreateItemRewardButton()
    {
        currentItemRewardButton = Instantiate(PrefabHolder.Instance.ItemRewardButton, RewardButtonParent.transform);
    }

    public void PopulateItemScreen()
    {
        GameObject newItemOne = Instantiate(PrefabHolder.Instance.ItemCard, ChooseItemScreenContent.transform);
        ItemCard itemOne = newItemOne.GetComponent<ItemCard>();
        itemOne.RunSetupFromItemData(ItemLibrary.Instance.GetRandomLootRewardItem());
        currentItemOne = newItemOne;

        GameObject newItemTwo = Instantiate(PrefabHolder.Instance.ItemCard, ChooseItemScreenContent.transform);
        ItemCard itemTwo = newItemTwo.GetComponent<ItemCard>();
        itemTwo.RunSetupFromItemData(ItemLibrary.Instance.GetRandomLootRewardItem());
        currentItemTwo = newItemTwo;

        GameObject newItemThree = Instantiate(PrefabHolder.Instance.ItemCard, ChooseItemScreenContent.transform);
        ItemCard itemThree = newItemThree.GetComponent<ItemCard>();
        itemThree.RunSetupFromItemData(ItemLibrary.Instance.GetRandomLootRewardItem());
        currentItemThree = newItemThree;
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

    public void OnSkipRewardsButtonClicked()
    {
        EventManager.Instance.EndNewLootRewardEvent();
    }

    public void OnChooseItemPageBackButtonClicked()
    {

    }

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

    public void EnableItemLootScreen()
    {
        RewardButtonParent.SetActive(false);
        SkipRewardsButton.SetActive(false);
        Background.SetActive(false);
        TitleImage.SetActive(false);
        ChooseItemScreenParent.SetActive(true);
    }

    public void DisableItemLootScreen()
    {
        RewardButtonParent.SetActive(true);
        SkipRewardsButton.SetActive(true);
        Background.SetActive(true);
        TitleImage.SetActive(true);
        ChooseItemScreenParent.SetActive(false);
    }
}
