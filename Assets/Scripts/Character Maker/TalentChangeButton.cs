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
        Debug.Log("TalentChangeButton.SetTalentTierCount() called, new value: " + talentSchool.ToString() + " ("+
            newValue.ToString() +")");
        talentTierCount = newValue;
        currentTalentTierText.text = newValue.ToString();
    }
    public void SetUpFromTalentPairing(TalentPairing talentPairing)
    {
        Debug.Log("TalentChangeButton.SetUpFromTalentPairing() called for talent pairing: " + talentPairing.talentType.ToString() + " ("
            + talentPairing.talentStacks.ToString() + ")");
        SetTalentTierCount(talentPairing.talentStacks);
    }
    public void OnTalentPointPlusButtonClicked()
    {
        Debug.Log("TalentChangeButton.OnTalentPointPlusButtonClicked() called");
        CharacterMakerController.Instance.OnTalentPointPlusButtonClicked(this);
    }
    public void OnTalentPointMinusButtonClicked()
    {
        Debug.Log("TalentChangeButton.OnTalentPointMinusButtonClicked() called");
        CharacterMakerController.Instance.OnTalentPointMinusButtonClicked(this);
    }
}
