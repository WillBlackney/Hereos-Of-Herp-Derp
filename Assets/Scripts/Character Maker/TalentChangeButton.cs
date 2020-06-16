using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TalentChangeButton : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Component References")]
    public TextMeshProUGUI currentTalentTierText;

    [Header("Properties")]
    public AbilityDataSO.AbilitySchool talentSchool;
    public int talentTierCount;
    #endregion

    // Core Logic
    #region
    public void SetTalentTierCount(int newValue)
    {
        Debug.Log("TalentChangeButton.SetTalentTierCount() called, new value: " + talentSchool.ToString() + " ("+
            newValue.ToString() +")");
        talentTierCount = newValue;
        AutoApplyTextColour();
    }
    public void SetUpFromTalentPairing(TalentPairing talentPairing)
    {
        Debug.Log("TalentChangeButton.SetUpFromTalentPairing() called for talent pairing: " + talentPairing.talentType.ToString() + " ("
            + talentPairing.talentStacks.ToString() + ")");
        SetTalentTierCount(talentPairing.talentStacks);
    }
    public void AutoApplyTextColour()
    {
        // color text green if stat is in bonus
        if (talentTierCount == 0)
        {
            currentTalentTierText.text = TextLogic.ReturnColoredText(talentTierCount.ToString(), TextLogic.white);
        }
        // color text red if stat is in negative
        else if (talentTierCount > 0)
        {
            currentTalentTierText.text = TextLogic.ReturnColoredText(talentTierCount.ToString(), TextLogic.yellow);
        }
    }
    #endregion

    // Click + Mouse Events
    #region
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
    #endregion
}
