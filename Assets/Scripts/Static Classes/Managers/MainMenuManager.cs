using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Component References")]
    public GameObject HeroSelectionScreen;
    public GameObject arrowParent;
    public GameObject allElementsParent;
    public GameObject northPos;
    public GameObject centrePos;
    public CanvasGroup textElementsParentCG;

    [Header("Properties")]
    public List<string> characterClassNames;
    public bool startGameEventTriggered;
    public bool goToTeamBuilderScreenEventTriggered;
    public bool returnToMainMenuEventTriggered;
    public MenuCharacter selectedMenuCharacter;

    [Header("Color Stuff")]
    public Color normalColour;
    public Color highlightColour;

    [Header("Menu Character References")]
    public MenuCharacter characterOne;
    public MenuCharacter characterTwo;
    public MenuCharacter characterThree;
    public MenuCharacter characterFour;

    // Singleton Set up + Start + Update
    #region
    public static MainMenuManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SetSelectedCharacter(characterOne);
    }
    private void Update()
    {
        MoveArrowTowardsSelectedCharacter();       
    }
    #endregion 

    // Mouse + Button + Click Events
    #region
    public void OnNewGameButtonClicked()
    {
        if(goToTeamBuilderScreenEventTriggered == false)
        {
            goToTeamBuilderScreenEventTriggered = true;
            StartCoroutine(OnNewGameButtonClickedCoroutine());
        }
        
    }
    public IEnumerator OnNewGameButtonClickedCoroutine()
    {
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 4, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

        goToTeamBuilderScreenEventTriggered = false;
        HeroSelectionScreen.SetActive(true);
        arrowParent.SetActive(true);

        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 4, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);
    }
    public void OnQuitGameButtonClicked()
    {
        Application.Quit();
    }
    public void OnBackToMainMenuButtonCicked()
    {
        if(returnToMainMenuEventTriggered == false)
        {
            returnToMainMenuEventTriggered = true;
            StartCoroutine(OnBackToMainMenuButtonCickedCoroutine());
        }
        
    }
    public IEnumerator OnBackToMainMenuButtonCickedCoroutine()
    {
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 4, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

        HeroSelectionScreen.SetActive(false);
        arrowParent.SetActive(false);
        returnToMainMenuEventTriggered = false;

        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 4, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);
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

        // Disable arrow
        arrowParent.SetActive(false);

        // Start screen fade transistion
        Action fadeAction = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 4, 1, true);
        yield return new WaitUntil(() => fadeAction.ActionResolved() == true);

        // Set Properties
        List<MenuCharacter> allCharacters = new List<MenuCharacter> { characterOne, characterTwo, characterThree, characterFour };
        List<string> chosenClasses = new List<string>();

        // add chosen menu characters names as strings to list
        foreach(MenuCharacter character in allCharacters)
        {
            // randomize presets for characters marked as random
            if(character.myPresetName == "Random")
            {
                character.myPresetName = GetRandomClassString();
            }
            chosenClasses.Add(character.myPresetName);
        }

        // store selected preset data between scene change
        foreach (string characterName in chosenClasses)
        {
            SceneChangeDataStorage.Instance.chosenCharacters.Add(characterName);
        }

        // Enable loading screen
        SceneController.Instance.loadScreenVisualParent.SetActive(true);

        // Fade Screen back in
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 4, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);

        // Ready, load the game scene
        SceneController.Instance.LoadSceneAsync("Game Scene");
    }
    #endregion

    // Misc Logic
    #region
    public void MoveArrowTowardsSelectedCharacter()
    {
        if (selectedMenuCharacter != null)
        {
            arrowParent.transform.position = Vector2.MoveTowards(arrowParent.transform.position, selectedMenuCharacter.transform.position, 20 * Time.deltaTime);
        }
    }
    public string GetRandomClassString()
    {
        List<string> classes = new List<string>();
        classes.AddRange(characterClassNames);
        classes.Remove("Random");

        int randomIndex = Random.Range(0, classes.Count);
        return classes[randomIndex];
    }
    public void PlayStartSequence()
    {
        StartCoroutine(PlayStartSequenceCoroutine());
    }
    public IEnumerator PlayStartSequenceCoroutine()
    {
        Debug.Log("MainMenuManager.PlayStartSequenceCoroutine() started...");
        // Set up
        BlackScreenManager.Instance.canvasGroup.alpha = 0;
        BlackScreenManager.Instance.canvasGroup.alpha = 1;
        textElementsParentCG.alpha = 0;
        allElementsParent.transform.position = northPos.transform.position;

        // Start fade in
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 1, 0, false);      
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);
        yield return new WaitForSeconds(1);

        // move screen to normal position
        while(allElementsParent.transform.position != centrePos.transform.position)
        {
            allElementsParent.transform.position = Vector3.MoveTowards(allElementsParent.transform.position, centrePos.transform.position, 5 * Time.deltaTime);           
            yield return new WaitForEndOfFrame();
        }

        // brief wait
        yield return new WaitForSeconds(0.5f);

        // fade in text elements and UI
        while(textElementsParentCG.alpha < 1)
        {
            //Debug.Log("fading in text and ui");
            textElementsParentCG.alpha += 0.05f;
            yield return new WaitForEndOfFrame();
        }
        
    }
    #endregion

    // Main Menu Character Logic
    #region
    public void SetSelectedCharacter(MenuCharacter character)
    {
        characterOne.DisableInfoPanel();
        characterTwo.DisableInfoPanel();
        characterThree.DisableInfoPanel();
        characterFour.DisableInfoPanel();

        character.EnableInfoPanel();
        selectedMenuCharacter = character;
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

        character.tabOne.gameObject.SetActive(true);
        character.tabTwo.gameObject.SetActive(true);
        character.tabThree.gameObject.SetActive(true);
        character.tabFour.gameObject.SetActive(true);

        if (character.myPresetName == "Paladin")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Guard");
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
            character.tabOne.SetUpAbilityTabAsAbility("Devastating Blow");
            character.tabTwo.SetUpAbilityTabAsAbility("Blood Offering");
            character.tabThree.SetUpAbilityTabAsAbility("Charge");
            character.tabFour.SetUpAbilityTabAsPassive("Tenacious", 2);
        }

        else if (character.myPresetName == "Battle Mage")
        {
            character.tabThree.SetUpAbilityTabAsAbility("Devastating Blow");            
            character.tabTwo.SetUpAbilityTabAsAbility("Whirlwind");
            character.tabOne.SetUpAbilityTabAsAbility("Phoenix Dive");
            character.tabFour.SetUpAbilityTabAsPassive("Fiery Aura", 3);
        }

        else if (character.myPresetName == "Arcanist")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Fire Ball");
            character.tabTwo.SetUpAbilityTabAsAbility("Telekinesis");
            character.tabThree.SetUpAbilityTabAsAbility("Icy Focus");
            character.tabFour.SetUpAbilityTabAsPassive("Flux", 1);
        }

        else if (character.myPresetName == "Shadow Blade")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Shadow Step");
            character.tabTwo.SetUpAbilityTabAsAbility("Vanish");
            character.tabThree.SetUpAbilityTabAsAbility("Cheap Shot");
            character.tabFour.SetUpAbilityTabAsPassive("Opportunist", 50);
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
            character.tabOne.SetUpAbilityTabAsAbility("Head Shot");
            character.tabTwo.SetUpAbilityTabAsAbility("Haste");
            character.tabThree.SetUpAbilityTabAsAbility("Telekinesis");
            character.tabFour.SetUpAbilityTabAsPassive("Flux", 1);
        }
        else if (character.myPresetName == "Marksman")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Vanish");
            character.tabTwo.SetUpAbilityTabAsAbility("Impaling Bolt");
            character.tabThree.SetUpAbilityTabAsAbility("Head Shot");
            character.tabFour.SetUpAbilityTabAsPassive("Predator", 1);
        }
        else if (character.myPresetName == "Warlock")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Shadow Blast");
            character.tabTwo.SetUpAbilityTabAsAbility("Nightmare");
            character.tabThree.SetUpAbilityTabAsAbility("Hex");
            character.tabFour.SetUpAbilityTabAsPassive("Flux", 1);
        }
        else if (character.myPresetName == "Alchemist")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Blight");
            character.tabTwo.SetUpAbilityTabAsAbility("Chemical Reaction");
            character.tabThree.SetUpAbilityTabAsAbility("Drain");
            character.tabFour.SetUpAbilityTabAsPassive("Venomous", 1);
        }
        else if (character.myPresetName == "Random")
        {
            character.tabOne.gameObject.SetActive(false);
            character.tabTwo.gameObject.SetActive(false);
            character.tabThree.gameObject.SetActive(false);
            character.tabFour.gameObject.SetActive(false);
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

        else if (character.myPresetName == "Battle Mage")
        {
            character.attributeOneText.text = "Pyromania +2";
            character.attributeTwoText.text = "Brawler +1";

            character.attributeOneText.gameObject.SetActive(true);
            character.attributeTwoText.gameObject.SetActive(true);
        }

        else if (character.myPresetName == "Arcanist")
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
            character.attributeOneText.text = "Shadowcraft +2";
            character.attributeTwoText.text = "Manipulation +1";

            character.attributeOneText.gameObject.SetActive(true);
            character.attributeTwoText.gameObject.SetActive(true);
        }

        else if (character.myPresetName == "Alchemist")
        {
            character.attributeOneText.text = "Corruption +3";

            character.attributeOneText.gameObject.SetActive(true);
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

        else if (character.myPresetName == "Battle Mage")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Battle Mage balances ranged magical attacks with brutal melee attacks. Able to adapt to a variety of tactical situations, Battle Mage's are always dangerous.";
        }

        else if (character.myPresetName == "Arcanist")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Arcanist destroys its enemies from afar with the power of fire and frost. Arcanist's excel at keeping their distance from the fray with a variety of tricks.";
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

        else if (character.myPresetName == "Alchemist")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Alchemist uses a variety of poisons and potions to bring slow, agaonizing death to it's foes";
        }
    }
    #endregion

}
