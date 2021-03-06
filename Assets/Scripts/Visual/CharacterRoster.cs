﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRoster : MonoBehaviour
{
    [Header("Component References")]
    public GameObject visualParent;
    public GameObject canvasParent;
    public GameObject CharacterRosterCloseButton;

    [Header("Character Model References")]
    public CharacterData characterOne;
    public CharacterData characterTwo;
    public CharacterData characterThree;
    public CharacterData characterFour;

    [Header("Character Button Model References")]
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
         //InitializeSetup();
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

        foreach (CharacterPresetData characterData in SceneChangeDataStorage.Instance.chosenCharacters)
        {
            if(characterOneSetupComplete == false)
            {
                characterOne.InitializeSetupFromPresetData(characterData);
                characterOneSetupComplete = true;
                allCharacterDataObjects.Add(characterOne);
                CampSiteManager.Instance.SetupCampSiteCharacter(CampSiteManager.Instance.allCharacterSlots[0], characterOne);
                ShopScreenManager.Instance.SetUpVillageCharacter(ShopScreenManager.Instance.characterOne, characterOne);
                StoryEventManager.Instance.SetupStoryWindowCharacter(StoryEventManager.Instance.allCharacterSlots[0], characterOne);
                TreasureRoomManager.Instance.SetUpTreasureRoomCharacter(TreasureRoomManager.Instance.allCharacterSlots[0], characterOne);
                CharacterModelController.BuildModelFromCharacterPresetData(characterOneButtonModel, characterData);
                CharacterModelController.BuildModelFromCharacterPresetData(KingsBlessingManager.Instance.modelOne, characterData);
                characterOneButtonModel.SetBaseAnim();
                characterOne.myModelOnButton = characterOneButtonModel;
            }

            else if (characterTwoSetupComplete == false)
            {
                characterTwo.InitializeSetupFromPresetData(characterData);
                characterTwoSetupComplete = true;
                allCharacterDataObjects.Add(characterTwo);
                CampSiteManager.Instance.SetupCampSiteCharacter(CampSiteManager.Instance.allCharacterSlots[1], characterTwo);
                ShopScreenManager.Instance.SetUpVillageCharacter(ShopScreenManager.Instance.characterTwo, characterTwo);
                StoryEventManager.Instance.SetupStoryWindowCharacter(StoryEventManager.Instance.allCharacterSlots[1], characterTwo);
                TreasureRoomManager.Instance.SetUpTreasureRoomCharacter(TreasureRoomManager.Instance.allCharacterSlots[1], characterTwo);
                CharacterModelController.BuildModelFromCharacterPresetData(characterTwoButtonModel, characterData);
                CharacterModelController.BuildModelFromCharacterPresetData(KingsBlessingManager.Instance.modelTwo, characterData);
                characterTwoButtonModel.SetBaseAnim();
                characterTwo.myModelOnButton = characterTwoButtonModel;
            }

            else if (characterThreeSetupComplete == false)
            {
                characterThree.InitializeSetupFromPresetData(characterData);
                characterThreeSetupComplete = true;
                allCharacterDataObjects.Add(characterThree);
                CampSiteManager.Instance.SetupCampSiteCharacter(CampSiteManager.Instance.allCharacterSlots[2], characterThree);
                ShopScreenManager.Instance.SetUpVillageCharacter(ShopScreenManager.Instance.characterThree, characterThree);
                StoryEventManager.Instance.SetupStoryWindowCharacter(StoryEventManager.Instance.allCharacterSlots[2], characterThree);
                TreasureRoomManager.Instance.SetUpTreasureRoomCharacter(TreasureRoomManager.Instance.allCharacterSlots[2], characterThree);
                CharacterModelController.BuildModelFromCharacterPresetData(characterThreeButtonModel, characterData);
                CharacterModelController.BuildModelFromCharacterPresetData(KingsBlessingManager.Instance.modelThree, characterData);
                characterThreeButtonModel.SetBaseAnim();
                characterThree.myModelOnButton = characterThreeButtonModel;
            }

            else if (characterFourSetupComplete == false)
            {
                characterFour.InitializeSetupFromPresetData(characterData);
                characterFourSetupComplete = true;
                allCharacterDataObjects.Add(characterFour);
                CampSiteManager.Instance.SetupCampSiteCharacter(CampSiteManager.Instance.allCharacterSlots[3], characterFour);
                ShopScreenManager.Instance.SetUpVillageCharacter(ShopScreenManager.Instance.characterFour, characterFour);
                StoryEventManager.Instance.SetupStoryWindowCharacter(StoryEventManager.Instance.allCharacterSlots[3], characterFour);
                TreasureRoomManager.Instance.SetUpTreasureRoomCharacter(TreasureRoomManager.Instance.allCharacterSlots[3], characterFour);
                CharacterModelController.BuildModelFromCharacterPresetData(characterFourButtonModel, characterData);
                CharacterModelController.BuildModelFromCharacterPresetData(KingsBlessingManager.Instance.modelFour, characterData);
                characterFourButtonModel.SetBaseAnim();
                characterFour.myModelOnButton = characterFourButtonModel;
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
    public void DisableInventoryView()
    {
        InventoryController.Instance.visualParent.SetActive(false);
        InventoryController.Instance.canvasParent.SetActive(false);
    }
    public void PlayIdleAnimOnAllModels()
    {
        characterOne.myCharacterModel.SetIdleAnim();
        characterTwo.myCharacterModel.SetIdleAnim();
        characterThree.myCharacterModel.SetIdleAnim();
        characterFour.myCharacterModel.SetIdleAnim();
    }
    public void SetDefaultViewState()
    {
        DisablesAllCharacterDataViews();

        characterOne.EnableMainWindowView();
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
