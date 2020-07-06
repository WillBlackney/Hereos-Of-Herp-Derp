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
    public bool ignoreChanceModifiers;
    public List<SuccessChanceModifier> successChanceModifiers; 

    [Header("Requirements To Unlock Choice")]
    public List<ChoiceRequirment> choiceRequirements;

    [Header("Rewards On Success")]
    public List<ChoiceConsequence> onSuccessConsequences;

    [Header("Consequences On Failure")]
    public List<ChoiceConsequence> onFailureConsequences;

    [Header("GUI Events On Success")]
    public List<ChoiceResolvedGuiEvent> onSuccessGuiEvents;

    [Header("GUI Events On Failure")]
    public List<ChoiceResolvedGuiEvent> onFailureGuiEvents;

}

[Serializable]
public class ChoiceRequirment
{
    public enum RequirementType { None, HasEnoughGold, HasBackground, HasRace, HasTalent};

    [Header("Properties")]
    public RequirementType requirementType;

    [ShowIf("requirementType", RequirementType.HasEnoughGold)]
    public int goldAmountRequired;

    [ShowIf("requirementType", RequirementType.HasRace)]
    public UniversalCharacterModel.ModelRace raceRequirement;

    [ShowIf("requirementType", RequirementType.HasBackground)]
    public CharacterData.Background backgroundRequirement;

    [ShowIf("requirementType", RequirementType.HasTalent)]
    public AbilityDataSO.AbilitySchool talentTypeRequirement;
    [ShowIf("requirementType", RequirementType.HasTalent)]
    public int talentTierRequirement;
}


[Serializable]
public class ChoiceConsequence
{
    public enum ConsequenceType { None, EventEnds, GainGold, AllCharactersGainXP, TriggerCombatEvent, GainSpecificItem, 
        GainSpecificAffliction, GainSpecificState, RandomPartyMemberDies, LoseAllInventoryItems, GainRandomItem, GainRandomWeapon};

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

    [ShowIf("consequenceType", ConsequenceType.GainSpecificAffliction)]
    public StateDataSO afflictionGained;

    [ShowIf("consequenceType", ConsequenceType.GainSpecificState)]
    public StateDataSO stateGained;

    [ShowIf("consequenceType", ConsequenceType.RandomPartyMemberDies)]
    public int partyMembersKilled;

    [ShowIf("consequenceType", ConsequenceType.GainRandomItem)]
    public int randomItemsGained;
    [ShowIf("consequenceType", ConsequenceType.GainRandomItem)]
    public ItemDataSO.ItemRarity randomItemRarity;

    [ShowIf("consequenceType", ConsequenceType.GainRandomWeapon)]
    public int randomWeaponsGained;
    [ShowIf("consequenceType", ConsequenceType.GainRandomWeapon)]
    public ItemDataSO.ItemRarity randomWeaponRarity;
}
[Serializable]
public class ChoiceResolvedGuiEvent
{
    public enum GuiEvent { None, LoadNextEventPage, LoadEventPageByIndex, DestroyAllChoiceButtons, UpdateEventDescription, EnableContinueButton};

    [Header("Properties")]
    public GuiEvent guiEvent;

    [ShowIf("guiEvent", GuiEvent.UpdateEventDescription)]
    [TextArea(10,10)]
    public string newEventDescription;

    [ShowIf("guiEvent", GuiEvent.LoadEventPageByIndex)]
    public int pageIndex;
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

    [ShowIf("chanceTypeModifier", ChanceModifierType.HasState)]
    public StateDataSO stateRequirement;

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

[Serializable]
public class StoryPage
{
    public List<StoryChoiceDataSO> pageChoices;
}
