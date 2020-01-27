using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRoster : Singleton<CharacterRoster>
{
    [Header("Component References")]
    public GameObject CharacterRosterVisualParent;
    public GameObject CharacterRosterCloseButton;
    public GameObject inventoryParent;

    public CharacterData characterOne;
    public CharacterData characterTwo;
    public CharacterData characterThree;
    public CharacterData characterFour;

    [Header("Properties")]
    public CharacterData selectedCharacterData;
    public List<CharacterData> allCharacterDataObjects;

    // Initialization + Setup
    #region
    private void Start()
    {
        InitializeSetup();
    }
    public void InitializeSetup()
    {
        allCharacterDataObjects = new List<CharacterData>();

        bool characterOneSetupComplete = false;
        bool characterTwoSetupComplete = false;
        bool characterThreeSetupComplete = false;
        bool characterFourSetupComplete = false;

        // for testing
        if(SceneChangeDataStorage.Instance == null)
        {
            return;
        }

        foreach (string characterData in SceneChangeDataStorage.Instance.chosenCharacters)
        {
            if(characterOneSetupComplete == false)
            {
                characterOne.InitializeSetupFromPresetString(characterData);
                characterOneSetupComplete = true;
                allCharacterDataObjects.Add(characterOne);
                CampSiteManager.Instance.SetupCampSiteCharacter(CampSiteManager.Instance.allCharacterSlots[0], characterOne);
                StoryEventManager.Instance.SetupStoryWindowCharacter(StoryEventManager.Instance.allCharacterSlots[0], characterOne);
            }

            else if (characterTwoSetupComplete == false)
            {
                characterTwo.InitializeSetupFromPresetString(characterData);
                characterTwoSetupComplete = true;
                allCharacterDataObjects.Add(characterTwo);
                CampSiteManager.Instance.SetupCampSiteCharacter(CampSiteManager.Instance.allCharacterSlots[1], characterTwo);
                StoryEventManager.Instance.SetupStoryWindowCharacter(StoryEventManager.Instance.allCharacterSlots[1], characterTwo);
            }

            else if (characterThreeSetupComplete == false)
            {
                characterThree.InitializeSetupFromPresetString(characterData);
                characterThreeSetupComplete = true;
                allCharacterDataObjects.Add(characterThree);
                CampSiteManager.Instance.SetupCampSiteCharacter(CampSiteManager.Instance.allCharacterSlots[2], characterThree);
                StoryEventManager.Instance.SetupStoryWindowCharacter(StoryEventManager.Instance.allCharacterSlots[2], characterThree);
            }

            else if (characterFourSetupComplete == false)
            {
                characterFour.InitializeSetupFromPresetString(characterData);
                characterFourSetupComplete = true;
                allCharacterDataObjects.Add(characterFour);
                CampSiteManager.Instance.SetupCampSiteCharacter(CampSiteManager.Instance.allCharacterSlots[3], characterFour);
                StoryEventManager.Instance.SetupStoryWindowCharacter(StoryEventManager.Instance.allCharacterSlots[3], characterFour);
            }
        }

        // hardcoded names, for testing. REMOVE LATER
        characterOne.SetMyName("Will");
        characterTwo.SetMyName("Cecilie");
        characterThree.SetMyName("Leo");
        characterFour.SetMyName("Ella");

        // enable character one's panel view by default
        OnCharacterDataButtonClicked(characterOne);

        // Disables the roster view on scene load
        //CharacterRosterVisualParent.SetActive(false);
    }
    public void InstantiateDefenders()
    {
        foreach(CharacterData character in allCharacterDataObjects)
        {
            // Prevent spawning dead characters
            if (CanCharacterSpawn(character))
            {
                character.CreateMyDefenderGameObject();
            }
            
        }
    }
    #endregion

    // Enable/Disable Views
    public void DisablesAllCharacterDataViews()
    {
        characterOne.DisableMainWindowView();
        characterTwo.DisableMainWindowView();
        characterThree.DisableMainWindowView();
        characterFour.DisableMainWindowView();
    }
    public void OnCharacterDataButtonClicked(CharacterData characterSelected)
    {
        DisablesAllCharacterDataViews();
        characterSelected.EnableMainWindowView();
    }
    public void EnableInventoryView()
    {
        inventoryParent.SetActive(true);
    }
    public void DisableInventoryView()
    {
        inventoryParent.SetActive(false);
    }


    // Logic
    #region
    public void RewardAllCharactersXP(int xpRewarded)
    {
        foreach(CharacterData cd in allCharacterDataObjects)
        {
            RewardCharacterXP(cd, xpRewarded);
        }
    }
    public void RewardCharacterXP(CharacterData character, int xpRewarded)
    {
        character.ModifyCurrentXP(xpRewarded);
    }
    public bool CanCharacterSpawn(CharacterData character)
    {
        if(character.currentHealth > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion



}
