using System.Collections.Generic;
using UnityEngine;

public class CharacterPresetLibrary : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Properties + Component References")]
    [HideInInspector] public List<CharacterPresetData> allOriginCharacterPresets;
    public List<OriginCharacterDataSO> allOriginCharacterData;
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
        allOriginCharacterPresets = new List<CharacterPresetData>();
        PopulateOriginCharacterLibraryOnStart();
    }
    private void PopulateOriginCharacterLibraryOnStart()
    {
        foreach(OriginCharacterDataSO data in allOriginCharacterData)
        {
            AddCharacterPresetToOriginList(CreateOriginCharacterPresetFromOriginCharacterData(data));
        }
    }
    #endregion

    // Get + Set Char Data
    #region
    public void AddCharacterPresetToOriginList(CharacterPresetData charDataAdded)
    {
        allOriginCharacterPresets.Add(charDataAdded);
        Debug.Log(JsonUtility.ToJson(charDataAdded));
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
    public CharacterPresetData CreateOriginCharacterPresetFromOriginCharacterData(OriginCharacterDataSO data)
    {
        Debug.Log("CharacterPresetLibrary.CreateOriginCharacterPresetFromOriginCharacterData() called, " +
            "building origin preset from origin data with name: " + data.characterName);

        CharacterPresetData newPreset = new CharacterPresetData();

        // Origin Data
        newPreset.characterName = data.characterName;
        newPreset.characterDescription = data.characterDescription;
        newPreset.backgrounds.AddRange(data.characterBackgrounds);
        newPreset.modelRace = data.characterRace;

        // Combat data
        newPreset.knownAbilities = data.knownAbilities;
        newPreset.knownPassives = data.knownPassives;
        newPreset.knownTalents = data.knownTalents;
        newPreset.mhWeapon = data.mhWeapon;
        newPreset.ohWeapon = data.ohWeapon;

        // Model elements
        foreach(string modelElement in data.modelViewElements)
        {
            newPreset.activeModelElements.Add(new ModelElementData(modelElement));
        }

        return newPreset;
    }
    public CharacterPresetData GetNextOriginPreset(CharacterPresetData currentPreset)
    {
        Debug.Log("MainMenuManager.GetNextOriginCharacter() called...");

        CharacterPresetData dataReturned = null;

        int currentListIndex = allOriginCharacterPresets.IndexOf(currentPreset);
        int maxCount = allOriginCharacterPresets.Count - 1;
        int nextIndex = 0;

        if(currentListIndex == maxCount)
        {
            nextIndex = 0;
        }
        else
        {
            nextIndex = currentListIndex + 1;
        }

        dataReturned = allOriginCharacterPresets[nextIndex];

        return dataReturned;
    }
    public CharacterPresetData GetPreviousOriginPreset(CharacterPresetData currentPreset)
    {
        Debug.Log("MainMenuManager.GetPreviousOriginPreset() called...");

        CharacterPresetData dataReturned = null;

        int currentListIndex = allOriginCharacterPresets.IndexOf(currentPreset);
        int maxCount = allOriginCharacterPresets.Count - 1;
        int previousIndex = 0;

        if (currentListIndex == 0)
        {
            previousIndex = maxCount;
        }
        else
        {
            previousIndex = currentListIndex - 1;
        }

        dataReturned = allOriginCharacterPresets[previousIndex];

        return dataReturned;
    }
    public CharacterPresetData GetRandomOriginCharacter()
    {
        List<CharacterPresetData> allOriginCharacters = new List<CharacterPresetData>();
        allOriginCharacters.AddRange(allOriginCharacterPresets);
        allOriginCharacters.Remove(GetOriginCharacterPresetByName("Random"));

        int randomIndex = Random.Range(0, allOriginCharacters.Count);
        return allOriginCharacters[randomIndex];
    }
    public CharacterPresetData GetOriginCharacterPresetByName(string characterName)
    {
        Debug.Log("CharacterPresetLibrary.GetOriginCharacterPresetByName() called, searching for: " + characterName);

        CharacterPresetData dataReturned = null;
        foreach(CharacterPresetData preset in allOriginCharacterPresets)
        {
            if(preset.characterName == characterName)
            {
                dataReturned = preset;
                break;
            }
        }

        if(dataReturned == null)
        {
            Debug.Log("CharacterPresetLibrary.GetOriginCharacterPresetByName() did not find any preset data that matches the name " +
                characterName + ", returning null...");
        }

        return dataReturned;
    }
    #endregion

    // JSON Functions
    public string ConvertCharacterPresetDataToJsonString(CharacterPresetData charData)
    {
        return JsonUtility.ToJson(charData);         
    }
    public CharacterPresetData ConvertJsonStringToCharacterPresetData(string jsonString)
    {
        CharacterPresetData data = JsonUtility.FromJson<CharacterPresetData>(jsonString);
        return data;
    }


}
