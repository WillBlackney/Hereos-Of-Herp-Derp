using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterPresetData
{
    // General Data
    public string characterName;
    public string characterDescription;
    public List<CharacterData.Background> backgrounds;

    // Model Properties
    public List<ModelElementData> activeModelElements;
    public UniversalCharacterModel.ModelRace modelRace;

    // Combat Properties
    public List<AbilityDataSO> knownAbilities;
    public List<AbilityDataSO> knownRacialAbilities;
    public List<StatusPairing> knownPassives;
    public List<StatusPairing> knownRacialPassives;
    public List<TalentPairing> knownTalents;

    // Weapons
    public ItemDataSO mhWeapon;
    public ItemDataSO ohWeapon;

    // Weapon set (only for loading into char maker screen!)
    public WeaponPresetDataSO weaponSetData;

    // Constructors
    public CharacterPresetData()
    {
        // Initialize lists
        activeModelElements = new List<ModelElementData>();
        knownAbilities = new List<AbilityDataSO>();
        knownRacialAbilities = new List<AbilityDataSO>();
        knownPassives = new List<StatusPairing>();
        knownRacialPassives = new List<StatusPairing>();
        knownTalents = new List<TalentPairing>();
        backgrounds = new List<CharacterData.Background>();
    }
}

[Serializable]
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

[Serializable]
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

