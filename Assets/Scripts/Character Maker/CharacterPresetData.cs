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
}
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
public class TalentPairing
{
    public int talentStacks;
    public AbilityDataSO.AbilitySchool talentType;

    public TalentPairing(AbilityDataSO.AbilitySchool tType, int tStacks)
    {
        talentType = tType;
        talentStacks = tStacks;
    }
}
