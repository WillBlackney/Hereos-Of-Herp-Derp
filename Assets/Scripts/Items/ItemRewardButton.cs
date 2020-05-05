using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemRewardButton : MonoBehaviour
{
    public enum RarityReward { Common, Rare, Epic };

    [Header("Component References")]    
    public TextMeshProUGUI chooseNewItemText;
    public Image chooseNewItemImage;
    public RarityReward rarityReward;

    [Header("Rarity GO References")]
    public GameObject commonGemParent;
    public GameObject rareGemParent;
    public GameObject epicGemParent;

    public void InitializeSetup(RarityReward rarity)
    {
        rarityReward = rarity;

        if(rarity == RarityReward.Common)
        {
            chooseNewItemText.text = "Common Item";
            commonGemParent.SetActive(true);
        }
        else if (rarity == RarityReward.Rare)
        {
            chooseNewItemText.text = "Rare Item";
            rareGemParent.SetActive(true);
        }
        if (rarity == RarityReward.Epic)
        {
            chooseNewItemText.text = "Epic Item";
            epicGemParent.SetActive(true);
        }
    }

    public void OnItemRewardButtonClicked()
    {
        if(rarityReward == RarityReward.Common)
        {
            RewardScreen.Instance.EnableCommonItemLootScreen();
        }
        else if (rarityReward == RarityReward.Rare)
        {
            RewardScreen.Instance.EnableRareItemLootScreen();
        }
        else if (rarityReward == RarityReward.Epic)
        {
            RewardScreen.Instance.EnableEpicItemLootScreen();
        }

    }



}
