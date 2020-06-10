using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPresetData
{
    // General Data
    public string characterName;
    public string characterDescription;
    public string characterBackground;
    public string characterRacialBackground;

    // Model Properties
    public List<string> activeModelElements;
    public UniversalCharacterModel.ModelRace modelRace;

    // Combat Properties
    public List<AbilityDataSO> knownAbilities;
    public List<StatusPairing> knownPassives;
    public List<TalentPairing> knownTalents;

    // Weapons
    public ItemDataSO mhWeapon;
    public ItemDataSO ohWeapon;

    // Constructors
    public CharacterPresetData()
    {
        // Initialize lists
        activeModelElements = new List<string>();
        knownAbilities = new List<AbilityDataSO>();
        knownPassives = new List<StatusPairing>();
        knownTalents = new List<TalentPairing>();
    }
}

[System.Serializable]
public class StatusPairing
{
    public StatusIconDataSO statusData;
    public int statusStacks;

    public StatusPairing(StatusIconDataSO data, int stacks)
    {
        statusData = data;
        statusStacks = stacks;
    }
}

[System.Serializable]
public class TalentPairing
{    
    public AbilityDataSO.AbilitySchool talentType;
    public int talentStacks;

    public TalentPairing(AbilityDataSO.AbilitySchool tType, int tStacks)
    {
        talentType = tType;
        talentStacks = tStacks;
    }
}
