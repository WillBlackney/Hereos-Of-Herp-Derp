using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPresetLibrary : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Properties + Component References")]
    public List<CharacterPresetData> allOriginCharacters;
    public List<CharacterPresetData> allPlayerMadeCharacters;
    public List<ClassPresetDataSO> allClassPresets;
    public List<WeaponPresetDataSO> allWeaponPresets;
    #endregion

    // Singleton Pattern
    #region
    public static CharacterPresetLibrary Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    #endregion

    // Set up + Initialization
    #region
    private void Start()
    {
        allPlayerMadeCharacters = new List<CharacterPresetData>();
        allOriginCharacters = new List<CharacterPresetData>();
        PopulateCharacterLibraryOnStart();
    }
    private void PopulateCharacterLibraryOnStart()
    {
        AddCharacterPresetToOriginList(CreateRagnarData());
    }
    #endregion

    // Get + Set Char Data
    #region
    public void AddCharacterPresetToOriginList(CharacterPresetData charDataAdded)
    {
        allOriginCharacters.Add(charDataAdded);
    }
    public void AddCharacterPresetToPlayerMadeList(CharacterPresetData charDataAdded)
    {
        allPlayerMadeCharacters.Add(charDataAdded);
    }
    public void PrintPresetData(CharacterPresetData data)
    {
        Debug.Log("Printing character preset data with name: " + data.characterName);

        // Print backgrounds
        Debug.Log("Backgrounds: ");
        foreach (CharacterData.Background bg in data.backgrounds)
        {
            Debug.Log(bg.ToString());
        }

        // Print abilities
        Debug.Log("Known abilities: ");
        foreach(AbilityDataSO ability in data.knownAbilities)
        {
            Debug.Log(ability.abilityName);
        }

        // Print passives
        Debug.Log("Known passives: ");
        foreach (StatusPairing passive in data.knownPassives)
        {
            Debug.Log(passive.statusData.statusName + "(stacks = " + passive.statusStacks.ToString() +")");
        }

        // Print talents
        Debug.Log("Known talents: ");
        foreach (TalentPairing tp in data.knownTalents)
        {
            Debug.Log(tp.talentType.ToString() + " +" + tp.talentStacks.ToString());
        }

        // Print weapons
        Debug.Log("Active weapons: ");
        if(data.mhWeapon)
        {
            Debug.Log("Main hand weapon: " + data.mhWeapon.Name);
        }
        if (data.ohWeapon)
        {
            Debug.Log("Off hand weapon: " + data.ohWeapon.Name);
        }

        // Print active model view elements
        Debug.Log("Active model parts: ");
        foreach (ModelElementData modelPart in data.activeModelElements)
        {
            Debug.Log(modelPart.elementName);
        }

    }
    #endregion

    // Create Specific Origin Characters
    #region
    public CharacterPresetData CreateRagnarData()
    {
        // TO DO !!!: should make a way to build CharacterPresetData objects from from scriptable object files
        // so that we can create origin characters via the inspectors instead of programatically
        // e.g. OriginCharacterDataSO

        CharacterPresetData cpd = new CharacterPresetData();

        // Set General Data
        cpd.characterName = "Ragnar The Eternal";
        cpd.characterDescription = "Place holder description.....";
        cpd.backgrounds.Add(CharacterData.Background.Wanderer);
        cpd.backgrounds.Add(CharacterData.Background.Recluse);

        // Set Race
        cpd.modelRace = UniversalCharacterModel.ModelRace.Undead;

        // Set Talents
        cpd.knownTalents = new List<TalentPairing>
        {
            new TalentPairing(AbilityDataSO.AbilitySchool.Brawler, 2),
            new TalentPairing (AbilityDataSO.AbilitySchool.Corruption, 1)
        };

        // Learn abilities
        cpd.knownAbilities = new List<AbilityDataSO>
        {
            AbilityLibrary.Instance.GetAbilityByName("Smash"),
            AbilityLibrary.Instance.GetAbilityByName("Charge"),
            AbilityLibrary.Instance.GetAbilityByName("Blood Offering")
        };

        // Learn passives
        cpd.knownPassives = new List<StatusPairing>
        {
            new StatusPairing(StatusIconLibrary.Instance.GetStatusIconByName("Tenacious"), 2)
        };

        // Set weapons

        // Set body view elements

        // Set clothing views

        return cpd;
    }
    #endregion

}
