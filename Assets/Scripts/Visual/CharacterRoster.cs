using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRoster : MonoBehaviour
{
    [Header("Component References")]
    public GameObject CharacterRosterVisualParent;
    public GameObject CharacterRosterCloseButton;
    public GameObject inventoryParent;

    public CharacterData characterOne;
    public CharacterData characterTwo;
    public CharacterData characterThree;
    public CharacterData characterFour;

    public UniversalCharacterModel characterOneButtonModel;
    public UniversalCharacterModel characterTwoButtonModel;
    public UniversalCharacterModel characterThreeButtonModel;
    public UniversalCharacterModel characterFourButtonModel;

    [Header("Properties")]
    public CharacterData selectedCharacterData;
    public List<CharacterData> allCharacterDataObjects;

    // Initialization + Setup
    #region
    public static CharacterRoster Instance;
    private void Awake()
    {
        Instance = this;
    }
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
                characterOne.SetMyName(characterData);
                CampSiteManager.Instance.SetupCampSiteCharacter(CampSiteManager.Instance.allCharacterSlots[0], characterOne);
                StoryEventManager.Instance.SetupStoryWindowCharacter(StoryEventManager.Instance.allCharacterSlots[0], characterOne);
                CharacterModelController.BuildModelFromPresetString(characterOneButtonModel, characterData);
                CharacterModelController.BuildModelFromPresetString(KingsBlessingManager.Instance.modelOne, characterData);
                characterOneButtonModel.SetBaseAnim();
            }

            else if (characterTwoSetupComplete == false)
            {
                characterTwo.InitializeSetupFromPresetString(characterData);
                characterTwoSetupComplete = true;
                allCharacterDataObjects.Add(characterTwo);
                characterTwo.SetMyName(characterData);
                CampSiteManager.Instance.SetupCampSiteCharacter(CampSiteManager.Instance.allCharacterSlots[1], characterTwo);
                StoryEventManager.Instance.SetupStoryWindowCharacter(StoryEventManager.Instance.allCharacterSlots[1], characterTwo);
                CharacterModelController.BuildModelFromPresetString(characterTwoButtonModel, characterData);
                CharacterModelController.BuildModelFromPresetString(KingsBlessingManager.Instance.modelTwo, characterData);
                characterTwoButtonModel.SetBaseAnim();
            }

            else if (characterThreeSetupComplete == false)
            {
                characterThree.InitializeSetupFromPresetString(characterData);
                characterThreeSetupComplete = true;
                allCharacterDataObjects.Add(characterThree);
                characterThree.SetMyName(characterData);
                CampSiteManager.Instance.SetupCampSiteCharacter(CampSiteManager.Instance.allCharacterSlots[2], characterThree);
                StoryEventManager.Instance.SetupStoryWindowCharacter(StoryEventManager.Instance.allCharacterSlots[2], characterThree);
                CharacterModelController.BuildModelFromPresetString(characterThreeButtonModel, characterData);
                CharacterModelController.BuildModelFromPresetString(KingsBlessingManager.Instance.modelThree, characterData);
                characterThreeButtonModel.SetBaseAnim();
            }

            else if (characterFourSetupComplete == false)
            {
                characterFour.InitializeSetupFromPresetString(characterData);
                characterFourSetupComplete = true;
                allCharacterDataObjects.Add(characterFour);
                characterFour.SetMyName(characterData);
                CampSiteManager.Instance.SetupCampSiteCharacter(CampSiteManager.Instance.allCharacterSlots[3], characterFour);
                StoryEventManager.Instance.SetupStoryWindowCharacter(StoryEventManager.Instance.allCharacterSlots[3], characterFour);
                CharacterModelController.BuildModelFromPresetString(characterFourButtonModel, characterData);
                CharacterModelController.BuildModelFromPresetString(KingsBlessingManager.Instance.modelFour, characterData);
                characterFourButtonModel.SetBaseAnim();
            }
        }

        // enable character one's panel view by default
        OnCharacterDataButtonClicked(characterOne);
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
