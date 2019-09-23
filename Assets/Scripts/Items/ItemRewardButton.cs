using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemRewardButton : MonoBehaviour
{
    public TextMeshProUGUI chooseNewItemText;
    public Image chooseNewItemImage;       

    public void OnItemRewardButtonClicked()
    {
        RewardScreen.Instance.EnableItemLootScreen();
    }

    

    
}
