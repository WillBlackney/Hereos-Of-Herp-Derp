using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldRewardButton : MonoBehaviour
{
    [Header("Component References")]
    public TextMeshProUGUI goldAmountText;

    [Header("Properties")]
    public int goldAmount;

    public void InitializeSetup()
    {
        if(EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.BasicEnemy)
        {
            goldAmount = Random.Range(10, 30);
        }
        else if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.EliteEnemy)
        {
            goldAmount = Random.Range(50, 100);
        }
        goldAmountText.text = goldAmount.ToString();
    }
    public void OnGoldRewardButtonClicked()
    {
        PlayerDataManager.Instance.ModifyGold(goldAmount);
        Destroy(gameObject);
    }
}
