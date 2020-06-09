using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPresetLibrary : MonoBehaviour
{
    [Header("Properties + Component References")]
    public List<CharacterPresetData> allOriginCharacters;
    public List<CharacterPresetData> allPlayerMadeCharacters;
    public List<ClassPresetDataSO> allClassPresets;

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
    #endregion

    // Create Specific Origin Characters
    #region
    public CharacterPresetData CreateRagnarData()
    {
        CharacterPresetData cpd = new CharacterPresetData();

        // Set General Data
        cpd.characterName = "Ragnar The Eternal";
        cpd.characterDescription = "Place holder description.....";
        cpd.characterBackground = "Recluse";
        cpd.characterRacialBackground = "Ancient One";

        // Set Race
        cpd.modelRace = UniversalCharacterModel.ModelRace.Human;

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

        return cpd;
    }
    #endregion

}
