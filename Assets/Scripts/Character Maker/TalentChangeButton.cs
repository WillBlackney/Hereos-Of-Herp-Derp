using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TalentChangeButton : MonoBehaviour
{
    [Header("Component References")]
    public TextMeshProUGUI currentTalentTierText;

    [Header("Properties")]
    public AbilityDataSO.AbilitySchool talentSchool;
    public int talentTierCount;

    public void SetTalentTierCount(int newValue)
    {
        talentTierCount = newValue;
        currentTalentTierText.text = newValue.ToString();
    }

}
