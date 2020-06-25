using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
public class MainMenuManager : MonoBehaviour
{
    [Header("Component References")]
    public GameObject HeroSelectionScreen;
    public GameObject loadPresetWindowVisualParent;
    public RectTransform loadPresetWindowButtonParent;
    public GameObject arrowParent;
    public GameObject allElementsParent;
    public GameObject northPos;
    public GameObject centrePos;
    public CanvasGroup textElementsParentCG;
    public List<MenuButton> allMenuButtons;

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

    [Header("Load Preset Data Button References")]
    public GameObject loadPresetDataButtonPrefab;
    public List<LoadPresetButton> activeLoadPresetButtons;
    
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
        DisableAllMenuButtons();

        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 4, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

        goToTeamBuilderScreenEventTriggered = false;
        HeroSelectionScreen.SetActive(true);
        arrowParent.SetActive(true);
        PlayIdleAnimOnAllMenuCharacters();

        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 4, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);
    }
    public void OnQuitGameButtonClicked()
    {
        Application.Quit();
    }
    public void PlayIdleAnimOnAllMenuCharacters()
    {
        characterOne.myModel.SetIdleAnim();
        characterTwo.myModel.SetIdleAnim();
        characterThree.myModel.SetIdleAnim();
        characterFour.myModel.SetIdleAnim();
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
        EnableAllMenuButtons();

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
    private IEnumerator OnStartGameButtonClickedCoroutine()
    {
        Debug.Log("Start Game Button Clicked...");

        // Disable arrow
        arrowParent.SetActive(false);

        // Start screen fade transistion
        Action fadeAction = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 4, 1, true);
        yield return new WaitUntil(() => fadeAction.ActionResolved() == true);

        // Set Properties
        List<MenuCharacter> allCharacters = new List<MenuCharacter> { characterOne, characterTwo, characterThree, characterFour };
        List<CharacterPresetData> chosenClasses = new List<CharacterPresetData>();

        // add chosen menu characters names as strings to list
        foreach(MenuCharacter character in allCharacters)
        {
            // randomize presets for characters marked as random
            if (character.myPreset.characterName == "Random")
            {
                // Get random class
                character.myPreset = CharacterPresetLibrary.Instance.GetRandomOriginCharacter();

                // re roll class if player has it already, prevent duplicate character when randomizing team
                while (chosenClasses.Contains(character.myPreset))
                {
                    Debug.Log("MainMenuManager detected that player already has " + character.myPreset.characterName +
                        " in their team, rerolling for random character again...");
                    character.myPreset = CharacterPresetLibrary.Instance.GetRandomOriginCharacter();
                }
            }
            chosenClasses.Add(character.myPreset);

        }

        // store selected preset data between scene change
        foreach (CharacterPresetData character in chosenClasses)
        {
            SceneChangeDataStorage.Instance.chosenCharacters.Add(character);
        }

        // Enable loading screen
        SceneController.Instance.loadScreenVisualParent.SetActive(true);

        // Fade Screen back in
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 4, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);

        // Ready, load the game scene
        SceneController.Instance.LoadGameSceneAsync();
    }
    #endregion

    // Misc Logic
    #region
    public void EnableAllMenuButtons()
    {
        foreach(MenuButton button in allMenuButtons)
        {
            button.myButtonComponent.interactable = true;
        }
    }
    public void DisableAllMenuButtons()
    {
        foreach (MenuButton button in allMenuButtons)
        {
            button.myButtonComponent.interactable = false;
        }
    }
    public void MoveArrowTowardsSelectedCharacter()
    {
        if (selectedMenuCharacter != null)
        {
            // calculate ideal pos with offest for arrow
            Vector2 idealPos = new Vector2(selectedMenuCharacter.myModel.transform.position.x, arrowParent.transform.position.y);

            // move arrow gradually
            arrowParent.transform.position = Vector2.MoveTowards(arrowParent.transform.position, idealPos, 20 * Time.deltaTime);
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
            character.tabOne.SetUpAbilityTabAsAbility("Charge");
            character.tabTwo.SetUpAbilityTabAsAbility("Invigorate");
            character.tabThree.SetUpAbilityTabAsAbility("Inspire");
            character.tabFour.SetUpAbilityTabAsPassive("Encouraging Aura", 10);
        }

        else if (character.myPresetName == "Knight")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Dash");
            character.tabTwo.SetUpAbilityTabAsAbility("Shield Slam");
            character.tabThree.SetUpAbilityTabAsAbility("Testudo");
            character.tabFour.SetUpAbilityTabAsPassive("Cautious", 4);
        }

        else if (character.myPresetName == "Berserker")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Smash");
            character.tabTwo.SetUpAbilityTabAsAbility("Blood Offering");
            character.tabThree.SetUpAbilityTabAsAbility("Charge");
            character.tabFour.SetUpAbilityTabAsPassive("Tenacious", 2);
        }

        else if (character.myPresetName == "Battle Mage")
        {
            character.tabThree.SetUpAbilityTabAsAbility("Whirlwind");            
            character.tabTwo.SetUpAbilityTabAsAbility("Fire Nova");
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
            character.tabFour.SetUpAbilityTabAsPassive("Predator", 1);
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
            character.tabThree.SetUpAbilityTabAsAbility("Transcendence");
            character.tabFour.SetUpAbilityTabAsPassive("Encouraging Aura", 10);
        }
        else if (character.myPresetName == "Monk")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Consecrate");
            character.tabTwo.SetUpAbilityTabAsAbility("Whirlwind");
            character.tabThree.SetUpAbilityTabAsAbility("Dash");
            character.tabFour.SetUpAbilityTabAsPassive("Riposte", 1);
        }

        else if (character.myPresetName == "Wayfarer")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Spirit Surge");
            character.tabTwo.SetUpAbilityTabAsAbility("Haste");
            character.tabThree.SetUpAbilityTabAsAbility("Telekinesis");
            character.tabFour.SetUpAbilityTabAsPassive("Quick Draw", 1);
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
            character.tabTwo.SetUpAbilityTabAsAbility("Chaos Bolt");
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
        else if (character.myPresetName == "Illusionist")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Phase Shift");
            character.tabTwo.SetUpAbilityTabAsAbility("Shadow Blast");
            character.tabThree.SetUpAbilityTabAsAbility("Dimensional Blast");
            character.tabFour.SetUpAbilityTabAsPassive("Flux", 1);
        }
        else if (character.myPresetName == "Frost Knight")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Frost Bolt");
            character.tabTwo.SetUpAbilityTabAsAbility("Chilling Blow");
            character.tabThree.SetUpAbilityTabAsAbility("Frost Nova");
            character.tabFour.SetUpAbilityTabAsPassive("Shatter", 1);
        }
        else if (character.myPresetName == "Shaman")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Chain Lightning");
            character.tabTwo.SetUpAbilityTabAsAbility("Thunder Strike");
            character.tabThree.SetUpAbilityTabAsAbility("Spirit Surge");
            character.tabFour.SetUpAbilityTabAsPassive("Flux", 1);
        }
        else if (character.myPresetName == "Death Knight")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Charge");
            character.tabTwo.SetUpAbilityTabAsAbility("Smash");
            character.tabThree.SetUpAbilityTabAsAbility("Dark Gift");
            character.tabFour.SetUpAbilityTabAsPassive("Shadow Aura", 1);
        }
        else if (character.myPresetName == "Bulwark")
        {
            character.tabOne.SetUpAbilityTabAsAbility("Provoke");
            character.tabTwo.SetUpAbilityTabAsAbility("Get Down!");
            character.tabThree.SetUpAbilityTabAsAbility("Fortify");
            character.tabFour.SetUpAbilityTabAsPassive("Guardian Aura", 3);
        }
        else if (character.myPresetName == "Random")
        {
            character.tabOne.gameObject.SetActive(false);
            character.tabTwo.gameObject.SetActive(false);
            character.tabThree.gameObject.SetActive(false);
            character.tabFour.gameObject.SetActive(false);
        }        
    }
    public void BuildCharacterAbilityTabsFromPresetData(MenuCharacter character, CharacterPresetData data)
    {
        Debug.Log("MainMenuManager.BuildCharacterAbilityTabs() called...");

        // Place all menu tabs in a list
        List<MenuAbilityTab> allTabs = new List<MenuAbilityTab>
        {
            character.tabOne,
            character.tabTwo,
            character.tabThree,
            character.tabFour
        };

        // used for placing elements sequentially
        int nextFreeIndex = 0;

        // Disable all tab views
        character.tabOne.gameObject.SetActive(false);
        character.tabTwo.gameObject.SetActive(false);
        character.tabThree.gameObject.SetActive(false);
        character.tabFour.gameObject.SetActive(false);

        // Build abilities first
        foreach(AbilityDataSO abilityData in data.knownAbilities)
        {
            // enable tab
            allTabs[nextFreeIndex].gameObject.SetActive(true);

            // build in next available slot
            allTabs[nextFreeIndex].SetUpAbilityTabAsAbility(abilityData);

            // increment next slot index 
            nextFreeIndex++;
        }

        // Build passives first
        foreach (StatusPairing passiveData in data.knownPassives)
        {
            // enable tab
            allTabs[nextFreeIndex].gameObject.SetActive(true);

            // build in next available slot
            allTabs[nextFreeIndex].SetUpAbilityTabAsPassive(passiveData.statusData,passiveData.statusStacks);

            // increment next slot index 
            nextFreeIndex++;
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
            character.attributeTwoText.text = "Brawler +1";

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

        else if (character.myPresetName == "Berserker")
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
            character.attributeOneText.text = "Divinity +3";

            character.attributeOneText.gameObject.SetActive(true);
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
            character.attributeOneText.text = "Ranger +1";
            character.attributeTwoText.text = "Manipulation +1";
            character.attributeThreeText.text = "Naturalism +1";

            character.attributeOneText.gameObject.SetActive(true);
            character.attributeTwoText.gameObject.SetActive(true);
            character.attributeThreeText.gameObject.SetActive(true);
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
        else if (character.myPresetName == "Illusionist")
        {
            character.attributeOneText.text = "Manipulation +2";
            character.attributeTwoText.text = "Shadowcraft +1";

            character.attributeOneText.gameObject.SetActive(true);
            character.attributeTwoText.gameObject.SetActive(true);
        }
        else if (character.myPresetName == "Shaman")
        {
            character.attributeOneText.text = "Naturalism +2";
            character.attributeTwoText.text = "Manipulation +1";

            character.attributeOneText.gameObject.SetActive(true);
            character.attributeTwoText.gameObject.SetActive(true);
        }
        else if (character.myPresetName == "Frost Knight")
        {
            character.attributeOneText.text = "Cyromancy +3";

            character.attributeOneText.gameObject.SetActive(true);
        }
        else if (character.myPresetName == "Death Knight")
        {
            character.attributeOneText.text = "Brawler +2";
            character.attributeTwoText.text = "Shadowcraft +1";

            character.attributeOneText.gameObject.SetActive(true);
            character.attributeTwoText.gameObject.SetActive(true);
        }
        else if (character.myPresetName == "Bulwark")
        {
            character.attributeOneText.text = "Guardian +3";

            character.attributeOneText.gameObject.SetActive(true);
        }

        else if (character.myPresetName == "Alchemist")
        {
            character.attributeOneText.text = "Corruption +3";

            character.attributeOneText.gameObject.SetActive(true);
        }
    }
    public void BuildTalentTextsFromCharacterPresetData(MenuCharacter character, CharacterPresetData data)
    {
        Debug.Log("MainMenuManager.BuildAttributeTextsFromCharacterPresetData() called...");

        // disable all text first
        character.attributeOneText.gameObject.SetActive(false);
        character.attributeTwoText.gameObject.SetActive(false);
        character.attributeThreeText.gameObject.SetActive(false);

        List<TextMeshProUGUI> textViews = new List<TextMeshProUGUI>()
        {
            character.attributeOneText,
            character.attributeTwoText,
            character.attributeThreeText
        };

        int nextFreeTextIndex = 0;

        foreach(TalentPairing tp in data.knownTalents)
        {
            textViews[nextFreeTextIndex].gameObject.SetActive(true);
            textViews[nextFreeTextIndex].text = tp.talentType.ToString() + " +" + tp.talentStacks.ToString();
            nextFreeTextIndex++;
        }
    }
    public void BuildBackgroundTextsFromCharacterPresetData(MenuCharacter character, CharacterPresetData data)
    {
        Debug.Log("MainMenuManager.BuildBackgroundTextsFromCharacterPresetData() called...");        

        // disable all text first
        character.backgroundTextOne.gameObject.SetActive(false);
        character.backgroundTextTwo.gameObject.SetActive(false);

        List<TextMeshProUGUI> textViews = new List<TextMeshProUGUI>()
        {
            character.backgroundTextOne,
            character.backgroundTextTwo
        };

        int nextFreeTextIndex = 0;

        foreach (CharacterData.Background bg in data.backgrounds)
        {
            textViews[nextFreeTextIndex].gameObject.SetActive(true);
            textViews[nextFreeTextIndex].text = bg.ToString();
            nextFreeTextIndex++;
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
            character.presetDescriptionText.text = "The Paladin excels at supporting allies in the melee, as well as dealing large amounts of damage with two handed weapons. ";
        }

        else if (character.myPresetName == "Knight")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "Equipted with a sword and shield, the Knight excels at absorbing damage and generating large amounts of Block. Knight's are capable of inflicting massive damage with their shield based attacks.";
        }

        else if (character.myPresetName == "Berserker")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Berserker lives for the thrill of battle. Capable of dealing huge damage to large groups of enemies in melee, Berserker's become stronger the more they take damage.";
        }

        else if (character.myPresetName == "Battle Mage")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Battle Mage balances ranged magical attacks with brutal melee attacks. Able to adapt to a variety of tactical situations, Battle Mage's are always a valueable asset.";
        }

        else if (character.myPresetName == "Arcanist")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Arcanist destroys its enemies from afar with the power of fire and frost magic. Arcanist's excel at keeping their distance from the fray with a variety of tricks.";
        }

        else if (character.myPresetName == "Shadow Blade")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "Striking from the shadows, the Shadow Blade is capable of dealing massive damage to a single target in melee combat, before quickly retreating to the safety of the shadows.";
        }
        else if (character.myPresetName == "Rogue")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "Rogue's love a good scrap. Capable of killing even the mightest foes with a mix of poisons, melee attacks and trickery, Rogue's excel at taking down the biggest bullies on the battlefield. ";
        }

        else if (character.myPresetName == "Priest")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Priest uses holy magic to support and protect allies from harm, before melting foes with divine vengeance";
        }
        else if (character.myPresetName == "Monk")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "Monk's can always be found in the thick of battle, supporting allies and cutting down enemies with a variety of melee attacks and fire magic. ";
        }

        else if (character.myPresetName == "Wayfarer")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Wayfarer balances deadly ranged attacks with a variety team support abilities, making them a highly versatile threat.";
        }
        else if (character.myPresetName == "Marksman")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Marksman tracks it foes and waits patiently for the right moment to devastate enemies with a variety of ranged attacks and disabling abilities.";

        }
        else if (character.myPresetName == "Warlock")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Warlock revels in death, capable of crippling enemies from afar with a debuffs, poisons and shadow magic.";
        }

        else if (character.myPresetName == "Alchemist")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "The Alchemist uses a variety of poisons and potions to bring slow, agaonizing death to it's foes";
        }

        else if (character.myPresetName == "Death Knight")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "Death Knight's use dark magic to bolster themselves and allies, before devastating foes with melee and magic attacks";
        }
        else if (character.myPresetName == "Frost Knight")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "Frost Knight's excel at empowering themselves with a variety of frost magic spells. After withering foes down with the cold, Frost Knight crush their enemies in melee";
        }

        else if (character.myPresetName == "Shaman")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "Mysterious and spiritualistic, Shaman's call upon the elements of wind and lightning to invigorate their allies and devastate enemies with electrical attacks.";
        }

        else if (character.myPresetName == "Illusionist")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "Masters of trickery, Illusionist's excel at manipulating the battlefield, controlling enemies and creating chaos.";
        }

        else if (character.myPresetName == "Bulwark")
        {
            character.presetDescriptionText.gameObject.SetActive(true);
            character.presetDescriptionText.text = "Noble and fearless, Bulwark's go to great lengths to protect their allies by throwing themselves into the fray, and goading foes to attack them. ";
        }
    }
    public void BuildDescriptionTextCharacterPresetData(MenuCharacter character, CharacterPresetData data)
    {
        Debug.Log("MainMenuManager.BuildDescriptionText() called...");

        // enable text view
        character.presetDescriptionText.gameObject.SetActive(true);

        // set text
        character.presetDescriptionText.text = data.characterDescription;
        
    }
    public void BuildRaceTabFromCharacterPresetData(MenuCharacter character, UniversalCharacterModel.ModelRace race)
    {
        Debug.Log("MainMenuManager.BuildRaceTabFromCharacterPresetData() called...");

        // set current race text
        character.currentRaceText.text = race.ToString();

        // set race description
        if(race == UniversalCharacterModel.ModelRace.Elf)
        {
            character.raceDescriptionText.text = TextLogic.elfRaceDescription;
            character.raceDescriptionText.gameObject.SetActive(true);
        }
        else if (race == UniversalCharacterModel.ModelRace.Human)
        {
            character.raceDescriptionText.text = TextLogic.humanRaceDescription;
            character.raceDescriptionText.gameObject.SetActive(true);
        }
        else if (race == UniversalCharacterModel.ModelRace.Undead)
        {
            character.raceDescriptionText.text = TextLogic.undeadRaceDescription;
            character.raceDescriptionText.gameObject.SetActive(true);
        }
        else if (race == UniversalCharacterModel.ModelRace.Orc)
        {
            character.raceDescriptionText.text = TextLogic.orcRaceDescription;
            character.raceDescriptionText.gameObject.SetActive(true);
        }
        else
        {
            character.raceDescriptionText.text = "";
            character.raceDescriptionText.gameObject.SetActive(false);
            character.currentRaceText.text = "Race";
        }




    }
    #endregion

    // Load Custom Preset Screen Logic
    public void EnableLoadPresetWindow()
    {
        Debug.Log("MainMenuManager.EnableLoadPresetWindow() called...");

        loadPresetWindowVisualParent.SetActive(true);
    }
    public void DisableLoadPresetWindow()
    {
        Debug.Log("MainMenuManager.DisableLoadPresetWindow() called...");

        loadPresetWindowVisualParent.SetActive(false);
    }
    public void PopulateLoadPresetWindow()
    {
        Debug.Log("MainMenuManager.PopulateLoadPresetWindow() called...");

        foreach(CharacterPresetData data in PersistencyManager.Instance.LoadAllCharacterPresetDataFromPersistency())
        {
            CreatePresetButton(data);
        }
    }
    private void DestroyAllLoadPresetButtons()
    {
        Debug.Log("MainMenuManager.DestroyAllLoadPresetButtons() called...");

        foreach(LoadPresetButton lpb in activeLoadPresetButtons)
        {
            Destroy(lpb.gameObject);
        }

        activeLoadPresetButtons.Clear();
    }
    private void CreatePresetButton(CharacterPresetData data)
    {
        Debug.Log("MainMenuManager.CreatePresetButton() called for: " + data.characterName);

        LoadPresetButton newButton = Instantiate(loadPresetDataButtonPrefab, loadPresetWindowButtonParent).GetComponent<LoadPresetButton>();
        newButton.InitializeSetup(data);
        activeLoadPresetButtons.Add(newButton);
    }
    public void OnLoadPresetButtonClicked(LoadPresetButton button)
    {
        // build into char maker screen or team maker screen?
        if(CharacterMakerController.Instance.mainVisualParent.activeSelf == true)
        {
            // build into char maker screen character
            CharacterMakerController.Instance.BuildAllTabsAndViewsFromCharacterPresetData(button.presetData);
        }
        else
        {
            // Build into active menu character
            selectedMenuCharacter.BuildMyViewsFromCharacterPresetData(button.presetData);
        }        

        // Clear active preset buttons
        DestroyAllLoadPresetButtons();

        // Hide preset selection window
        DisableLoadPresetWindow();
    }
    public void OnLoadPresetWindowCancelButtonClicked()
    {
        DestroyAllLoadPresetButtons();
        DisableLoadPresetWindow();
    }

}
