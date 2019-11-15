using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRoster : Singleton<CharacterRoster>
{
    [Header("Component References")]
    public GameObject CharacterRosterVisualParent;
    public GameObject CharacterRosterCloseButton;
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
                characterOne.InitializeSetup(characterData);
                characterOneSetupComplete = true;
                allCharacterDataObjects.Add(characterOne);
            }

            else if (characterTwoSetupComplete == false)
            {
                characterTwo.InitializeSetup(characterData);
                characterTwoSetupComplete = true;
                allCharacterDataObjects.Add(characterTwo);
            }

            else if (characterThreeSetupComplete == false)
            {
                characterThree.InitializeSetup(characterData);
                characterThreeSetupComplete = true;
                allCharacterDataObjects.Add(characterThree);
            }

            else if (characterFourSetupComplete == false)
            {
                characterFour.InitializeSetup(characterData);
                characterFourSetupComplete = true;
                allCharacterDataObjects.Add(characterFour);
            }
        }

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
        if(character.CurrentHealth > 0)
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
