using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [Header("Component References")]
    public GameObject HeroSelectionScreen;

    [Header("Properties")]
    public List<string> characterClassNames;
    public bool startGameEventTriggered;

    [Header("Color Stuff")]
    public Color normalColour;
    public Color highlightColour;

    [Header("Menu Character References")]
    public MenuCharacter characterOne;
    public MenuCharacter characterTwo;
    public MenuCharacter characterThree;
    public MenuCharacter characterFour;


    // Mouse + Button + Click Events
    #region
    public void OnNewGameButtonClicked()
    {
        HeroSelectionScreen.SetActive(true);
    }
    public void OnBackToMainMenuButtonCicked()
    {
        HeroSelectionScreen.SetActive(false);
    }
    public void OnStartGameButtonClicked()
    {
        if(startGameEventTriggered == false)
        {
            startGameEventTriggered = true;
            StartCoroutine(OnStartGameButtonClickedCoroutine());
        }
        
    }
    public IEnumerator OnStartGameButtonClickedCoroutine()
    {
        Debug.Log("Start Game Button Clicked...");

        // Start screen fade transistion
        Action fadeAction = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything,2, 1, true);
        yield return new WaitUntil(() => fadeAction.ActionResolved() == true);        

        CharacterBox[] chosenCharacters = FindObjectsOfType<CharacterBox>();

        List<string> chosenClasses = new List<string>();

        foreach (CharacterBox character in chosenCharacters)
        {
            chosenClasses.Add(character.currentSelectedCharacter);
        }

        // choose character classes for characters that were set as random

        foreach (CharacterBox character in chosenCharacters)
        {
            if (character.currentSelectedCharacter == "Random")
            {
                character.currentSelectedCharacter = "Warrior";
                /*
                while (chosenClasses.Contains(character.currentSelectedCharacter))
                {
                    character.currentSelectedCharacter = GetRandomClassString();
                }
                */
            }
        }

        foreach (CharacterBox character in chosenCharacters)
        {
            SceneChangeDataStorage.Instance.chosenCharacters.Add(character.currentSelectedCharacter);
        }

        SceneManager.LoadScene("Game Scene");
    }
    #endregion

    // Misc Logic
    #region
    public string GetRandomClassString()
    {
        int randomIndex = Random.Range(0, characterClassNames.Count);
        return characterClassNames[randomIndex];
    }
    #endregion

    // Main Menu Character Logic
    public void SetSelectedCharacter(MenuCharacter character)
    {
        characterOne.DisableInfoPanel();
        characterTwo.DisableInfoPanel();
        characterThree.DisableInfoPanel();
        characterFour.DisableInfoPanel();

        character.EnableInfoPanel();
    }
    public string GetNextPresetString(string currentPresetString)
    {
        Debug.Log("MainMenuManager.GetNextPresetString() called...");

        string presetNameResturned = "Unnassigned";
        int currentListIndex = 0;

        // Get current index
        foreach(string presetName in characterClassNames)
        {
            if(presetName == currentPresetString)
            {
                currentListIndex = characterClassNames.IndexOf(presetName);
            }
        }

        // if the current preset string is last in the list, get the first
        if(currentListIndex == characterClassNames.Count - 1)
        {
            presetNameResturned = characterClassNames[0];
        }
        else
        {
            presetNameResturned = characterClassNames[currentListIndex + 1];
        }

        Debug.Log("MainMenuManager.GetNextPresetString() returning " + presetNameResturned);
        return presetNameResturned;
    }
    public string GetPreviousPresetString(string currentPresetString)
    {
        Debug.Log("MainMenuManager.GetNextPresetString() called...");

        string presetNameResturned = "Unnassigned";
        int currentListIndex = 0;

        // Get current index
        foreach (string presetName in characterClassNames)
        {
            if (presetName == currentPresetString)
            {
                currentListIndex = characterClassNames.IndexOf(presetName);
            }
        }

        // if the current preset string is first in the list, get the last
        if (currentListIndex == 0)
        {
            presetNameResturned = characterClassNames[characterClassNames.Count - 1];
        }
        else
        {
            presetNameResturned = characterClassNames[currentListIndex - 1];
        }

        Debug.Log("MainMenuManager.GetPreviousPresetString() returning " + presetNameResturned);
        return presetNameResturned;
    }
    public void BuildCharacterAbilityTabs(MenuCharacter character)
    {
        Debug.Log("MainMenuManager.BuildCharacterAbilityTabs() called...");

        if (character.myPresetName == "Paladin")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Sword And Board");
            character.tabTwo.SetUpAbilityTabAsAbility("Invigorate");
            character.tabThree.SetUpAbilityTabAsAbility("Inspire");
            character.tabFour.SetUpAbilityTabAsPassive("Encouraging Aura", 10);
        }

        else if (character.myPresetName == "Knight")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Dash");
            character.tabTwo.SetUpAbilityTabAsAbility("Provoke");
            character.tabThree.SetUpAbilityTabAsAbility("Guard");
            character.tabFour.SetUpAbilityTabAsPassive("Cautious", 5);
        }

        else if (character.myPresetName == "Barbarian")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Whirlwind");
            character.tabTwo.SetUpAbilityTabAsAbility("Blood Offering");
            character.tabThree.SetUpAbilityTabAsAbility("Charge");
            character.tabFour.SetUpAbilityTabAsPassive("Tenacious", 3);
        }

        else if (character.myPresetName == "Spell Blade")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Phoenix Dive");
            character.tabTwo.SetUpAbilityTabAsAbility("Whirlwind");
            character.tabThree.SetUpAbilityTabAsAbility("Fire Nova");
            character.tabFour.SetUpAbilityTabAsPassive("Fiery Aura", 3);
        }

        else if (character.myPresetName == "Mage")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Fire Ball");
            character.tabTwo.SetUpAbilityTabAsAbility("Telekinesis");
            character.tabThree.SetUpAbilityTabAsAbility("Frost Nova");
            character.tabFour.SetUpAbilityTabAsPassive("Flux", 1);
        }

        else if (character.myPresetName == "Shadow Blade")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Shadow Step");
            character.tabTwo.SetUpAbilityTabAsAbility("Vanish");
            character.tabThree.SetUpAbilityTabAsAbility("Cheap Shot");
            character.tabFour.SetUpAbilityTabAsPassive("Opportunist", 20);
        }
        else if (character.myPresetName == "Rogue")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Dash");
            character.tabTwo.SetUpAbilityTabAsAbility("Blood Offering");
            character.tabThree.SetUpAbilityTabAsAbility("Shank");
            character.tabFour.SetUpAbilityTabAsPassive("Poisonous", 1);
        }

        else if (character.myPresetName == "Priest")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Holy Fire");
            character.tabTwo.SetUpAbilityTabAsAbility("Invigorate");
            character.tabThree.SetUpAbilityTabAsAbility("Shroud");
            character.tabFour.SetUpAbilityTabAsPassive("Encouraging Aura", 10);
        }
        else if (character.myPresetName == "Monk")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Consecrate");
            character.tabTwo.SetUpAbilityTabAsAbility("Whirlwind");
            character.tabThree.SetUpAbilityTabAsAbility("Dash");
            character.tabFour.SetUpAbilityTabAsPassive("Encouraging Aura", 10);
        }

        else if (character.myPresetName == "Wayfarer")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Haste");
            character.tabTwo.SetUpAbilityTabAsAbility("Steady Hands");
            character.tabThree.SetUpAbilityTabAsAbility("Telekinesis");
            character.tabFour.SetUpAbilityTabAsPassive("Flux", 1);
        }
        else if (character.myPresetName == "Marksman")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Vanish");
            character.tabTwo.SetUpAbilityTabAsAbility("Snipe");
            character.tabThree.SetUpAbilityTabAsAbility("Head Shot");
            character.tabFour.SetUpAbilityTabAsPassive("Predator", 1);
        }
        else if (character.myPresetName == "Warlock")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Blight");
            character.tabTwo.SetUpAbilityTabAsAbility("Noxious Fumes");
            character.tabThree.SetUpAbilityTabAsAbility("Hex");
            character.tabFour.SetUpAbilityTabAsPassive("Venomous", 1);
        }
    }
    public void BuildAttributeTexts(MenuCharacter character)
    {
        Debug.Log("MainMenuManager.BuildAttributeTexts() called...");

        // disable all text first
        character.attributeOneText.gameObject.SetActive(false);
        character.attributeTwoText.gameObject.SetActive(false);
        character.attributeThreeText.gameObject.SetActive(false);

        if (character.myPresetName == "Paladin")
        {
            character.attributeOneText.text = "Divinity +2";
            character.attributeTwoText.text = "Guardian +1";

            character.attributeOneText.gameObject.SetActive(true);
            character.attributeTwoText.gameObject.SetActive(true);
        }

        else if (character.myPresetName == "Knight")
        {
            character.attributeOneText.text = "Guardian +2";
            character.attributeTwoText.text = "Duelist +1";

            character.attributeOneText.gameObject.SetActive(true);
            character.attributeTwoText.gameObject.SetActive(true);
        }

        else if (character.myPresetName == "Barbarian")
        {
            character.attributeOneText.text = "Brawler +2";
            character.attributeTwoText.text = "Corruption +1";

            character.attributeOneText.gameObject.SetActive(true);
            character.attributeTwoText.gameObject.SetActive(true);
        }

        else if (character.myPresetName == "Spell Blade")
        {
            character.attributeOneText.text = "Pyromania +2";
            character.attributeTwoText.text = "Brawler +1";

            character.attributeOneText.gameObject.SetActive(true);
            character.attributeTwoText.gameObject.SetActive(true);
        }

        else if (character.myPresetName == "Mage")
        {
            character.attributeOneText.text = "Pyromania +1";
            character.attributeTwoText.text = "Manipulation +1";
            character.attributeThreeText.text = "Cyromancy +1";

            character.attributeOneText.gameObject.SetActive(true);
            character.attributeTwoText.gameObject.SetActive(true);
            character.attributeThreeText.gameObject.SetActive(true);
        }

        else if (character.myPresetName == "Shadow Blade")
        {
            character.attributeOneText.text = "Assassination +3";

            character.attributeOneText.gameObject.SetActive(true);
        }
        else if (character.myPresetName == "Rogue")
        {
            character.attributeOneText.text = "Assassination +1";
            character.attributeTwoText.text = "Duelist +1";
            character.attributeThreeText.text = "Corruption +1";

            character.attributeOneText.gameObject.SetActive(true);
            character.attributeTwoText.gameObject.SetActive(true);
            character.attributeThreeText.gameObject.SetActive(true);
        }

        else if (character.myPresetName == "Priest")
        {
            character.attributeOneText.text = "Divinity +2";
            character.attributeTwoText.text = "Shadowcraft +1";

            character.attributeOneText.gameObject.SetActive(true);
            character.attributeTwoText.gameObject.SetActive(true);
        }
        else if (character.myPresetName == "Monk")
        {
            character.attributeOneText.text = "Divinity +1";
            character.attributeTwoText.text = "Duelist +1";
            character.attributeThreeText.text = "Brawler +1";

            character.attributeOneText.gameObject.SetActive(true);
            character.attributeTwoText.gameObject.SetActive(true);
            character.attributeThreeText.gameObject.SetActive(true);
        }

        else if (character.myPresetName == "Wayfarer")
        {
            character.attributeOneText.text = "Ranger +2";
            character.attributeTwoText.text = "Manipulation +1";

            character.attributeOneText.gameObject.SetActive(true);
            character.attributeTwoText.gameObject.SetActive(true);
        }
        else if (character.myPresetName == "Marksman")
        {
            character.attributeOneText.text = "Ranger +2";
            character.attributeTwoText.text = "Assassination +1";

            character.attributeOneText.gameObject.SetActive(true);
            character.attributeTwoText.gameObject.SetActive(true);
        }
        else if (character.myPresetName == "Warlock")
        {
            character.attributeOneText.text = "Corruption +2";
            character.attributeTwoText.text = "Shadowcraft +1";

            character.attributeOneText.gameObject.SetActive(true);
            character.attributeTwoText.gameObject.SetActive(true);
        }
    }
    public void BuildDescriptionText(MenuCharacter character)
    {
        Debug.Log("MainMenuManager.BuildDescriptionText() called...");

        // disable all text first
        character.presetDescriptionText.gameObject.SetActive(false);

        if (character.myPresetName == "Paladin")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Paladin excels at supporting and protect allies, as well as absorbing large amounts of damage. ";
        }

        else if (character.myPresetName == "Knight")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "Equipted with a sword and shield, the Knight excels at melee combat, protecting allies and absorbing immense amounts of damage. ";
        }

        else if (character.myPresetName == "Barbarian")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Barbarian lives for the thrill of battle. Capable of dealing huge damage to large groups of enemies in melee, they become stronger the longer the battle rages on.";
        }

        else if (character.myPresetName == "Spell Blade")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Spell Blade balances ranged magical attacks with brutal melee attacks. Able to adapt to a variety of tactical situations, Spell Blade's are always dangerous.";
        }

        else if (character.myPresetName == "Mage")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Mage destroys its enemies from afar with the power of fire and frost. Mage's excel at keeping their distance from the fray with a variety of tricks.";
        }

        else if (character.myPresetName == "Shadow Blade")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "Striking from the shadows, the Shadow Blade is capable of dealing massive damage to a single target in melee combat, before quickly retreating to safety.";
        }
        else if (character.myPresetName == "Rogue")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "Rogue's love a good scrap. Capable of killing even the mightest foes with a mix of poisons, melee attacks and trickery, Rogue's excel at taking down enemy melee fighters. ";
        }

        else if (character.myPresetName == "Priest")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Priest balances the power of darkness and light to support its allies and defeat its foes. Capable in almost any role, the Priest is an asset to any party. ";
        }
        else if (character.myPresetName == "Monk")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "Monk's can always be found in the thick of battle, supporting allies and cutting down enemies with a variety of melee attacks and fire spells. ";
        }

        else if (character.myPresetName == "Wayfarer")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Wayfarer balances deadly ranged attacks with a variety team support abilities, making them a highly versatile threat.";
        }
        else if (character.myPresetName == "Marksman")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Marksman tracks it foes and waits patiently for the right moment to devastate enemies with a variety of ranged attacks.";

        }
        else if (character.myPresetName == "Warlock")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Warlock revels in death, capable of crippling enemies from afar with a variety of debuffs, poisons and spell attacks.";
        }
    }

}
