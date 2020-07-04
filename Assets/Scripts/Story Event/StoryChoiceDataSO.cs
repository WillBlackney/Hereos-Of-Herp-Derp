using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New StoryChoiceDataSO", menuName = "StoryChoiceDataSO", order = 52)]
public class StoryChoiceDataSO : ScriptableObject
{
    [Header("General Properties")]
    public string description;
    public int baseSuccessChance;
    public List<SuccessChanceModifier> successChanceModifiers; 

    [Header("Requirements To Unlock Choice")]
    public List<ChoiceRequirment> choiceRequirements;

    [Header("Rewards On Success")]
    public List<ChoiceConsequence> onSuccessConsequences;

    [Header("Consequences On Failure")]
    public List<ChoiceConsequence> onFailureConsequences;

}

[Serializable]
public class ChoiceRequirment
{
    public enum RequirementType { None, HasEnoughGold, HasBackground, HasRace};

    [Header("Properties")]
    public RequirementType requirementType;

    [ShowIf("requirementType", RequirementType.HasEnoughGold)]
    public int goldAmountRequired;

    [ShowIf("requirementType", RequirementType.HasRace)]
    public UniversalCharacterModel.ModelRace raceRequirement;

    [ShowIf("requirementType", RequirementType.HasBackground)]
    public CharacterData.Background backgroundRequirement;
}


[Serializable]
public class ChoiceConsequence
{
    public enum ConsequenceType { None, EventEnds, GainGold, AllCharactersGainXP, TriggerCombatEvent, GainSpecificItem};

    [Header("Properties")]
    public ConsequenceType consequenceType;

    [ShowIf("consequenceType", ConsequenceType.AllCharactersGainXP)]
    public int xpGainAmount;

    [ShowIf("consequenceType", ConsequenceType.GainGold)]
    public int goldGainAmount;

    [ShowIf("consequenceType", ConsequenceType.TriggerCombatEvent)]
    public EnemyWaveSO combatEvent;

    [ShowIf("consequenceType", ConsequenceType.GainSpecificItem)]
    public ItemDataSO specificItemGained;
}

[Serializable]
public class SuccessChanceModifier
{
    public enum ChanceModifierType { None, HasBackground, HasRace, HasState};

    [Header("Properties")]
    public ChanceModifierType chanceTypeModifier;

    [ShowIf("ShowPercentageModifier")]
    public int chancePercentageModifier;

    [ShowIf("chanceTypeModifier", ChanceModifierType.HasRace)]
    public UniversalCharacterModel.ModelRace raceRequirement;

    [ShowIf("chanceTypeModifier", ChanceModifierType.HasBackground)]
    public CharacterData.Background backgroundRequirement;

    // Show + Hide Boolean checkers
    public bool ShowPercentageModifier()
    {
        if(chanceTypeModifier != ChanceModifierType.None)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
