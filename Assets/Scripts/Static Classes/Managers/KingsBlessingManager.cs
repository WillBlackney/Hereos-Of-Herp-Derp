using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlessingChoice
{
    public string choiceName;

    public BlessingChoice(string _choiceName)
    {
        choiceName = _choiceName;
    }
}

public class KingsBlessingManager : MonoBehaviour
{
    // Component References + Properties
    #region
    [Header("Component References")]
    public GameObject visualParent;
    public GameObject canvasParent;
    public GameObject continueButton;
    public CanvasGroup speechBubbleCG;
    public TextMeshProUGUI speechBubbleText;
    public UniversalCharacterModel modelOne;
    public UniversalCharacterModel modelTwo;
    public UniversalCharacterModel modelThree;
    public UniversalCharacterModel modelFour;
    public UniversalCharacterModel kingModel;

    [Header("Properties")]
    private List<BlessingChoice> allChoiceData;
    public List<BlessingChoiceButton> allChoiceButtons;
    public bool eventCompleted;
    public BlessingChoiceButton choiceButtonOne;
    public BlessingChoiceButton choiceButtonTwo;
    public BlessingChoiceButton choiceButtonThree;
    public BlessingChoiceButton choiceButtonFour;
    
    #endregion

    // Singleton Setup + Awake / Start
    #region
    public static KingsBlessingManager Instance;
    private void Awake()
    {        
        Instance = this;        
    }
    private void Start()
    {
        OnNewGameLoaded();

        // REMOVE THIS LATER! FOR TESTING
        for(int i = 0; i< 15; i++)
        {
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem(), false);
        }
    }
    public void OnNewGameLoaded()
    {
        Debug.Log("KingsBlessingManager.OnNewGameLoaded() called...");

        // Disable player's ability to click on encounter buttons and start new encounters
        WorldManager.Instance.canSelectNewEncounter = false;

        // Set up screen
        EnableView();
        PopulateChoiceDataList();
        CreateChoiceButtons();
        CharacterModelController.BuildModelFromPresetString(kingModel, "King");
        PlayIdleAnimOnAllModels();
        SetSpeechBubbleText("Choose my blessing...");
        FadeInSpeechBubble(1);
    }
    #endregion

    // View Logic
    #region
    public void EnableView()
    {
        //canvasParent.SetActive(true);
        visualParent.SetActive(true);
        modelOne.SetIdleAnim();
        modelTwo.SetIdleAnim();
        modelThree.SetIdleAnim();
        modelFour.SetIdleAnim();
        kingModel.SetIdleAnim();
    }
    public void DisableView()
    {
       // canvasParent.SetActive(false);
        visualParent.SetActive(false);
    }
    public void SetSpeechBubbleText(string text)
    {
        speechBubbleText.text = TextLogic.ReturnColoredText(text, TextLogic.yellow);
    }
    public void FadeInSpeechBubble(float speed)
    {
        StartCoroutine(FadeInSpeechBubbleCoroutine(speed));
    }
    public IEnumerator FadeInSpeechBubbleCoroutine(float speed)
    {
        speechBubbleCG.alpha = 0;
        yield return new WaitForSeconds(1);

        while(speechBubbleCG.alpha < 1)
        {
            speechBubbleCG.alpha += speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    public void FadeOutSpeechBubble(float speed)
    {
        StartCoroutine(FadeOutSpeechBubbleCoroutine(speed));
    }
    public IEnumerator FadeOutSpeechBubbleCoroutine(float speed)
    {
        speechBubbleCG.alpha = 1;
        yield return new WaitForSeconds(2);

        while (speechBubbleCG.alpha > 0)
        {
            speechBubbleCG.alpha -= speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    public void EnableContinuteButtonView()
    {
        continueButton.SetActive(true);
    }
    public void DisableAllChoiceButtonViews()
    {
        choiceButtonOne.gameObject.SetActive(false);
        choiceButtonTwo.gameObject.SetActive(false);
        choiceButtonThree.gameObject.SetActive(false);
        choiceButtonFour.gameObject.SetActive(false);
    }

    public void PlayIdleAnimOnAllModels()
    {
        modelOne.SetIdleAnim();
        modelTwo.SetIdleAnim();
        modelThree.SetIdleAnim();
        modelFour.SetIdleAnim();
        kingModel.SetIdleAnim();
    }
    #endregion

    // Events
    #region
    public void OnContinueButtonClicked()
    {
        eventCompleted = true;
        //DisableView();
        UIManager.Instance.OnWorldMapButtonClicked();
    }
    public void StartChoiceProcess(string choiceName)
    {
        Debug.Log("KingsBlessingManager.StartChoiceProcess() called for: " + choiceName);

        DisableAllChoiceButtonViews();
        EnableContinuteButtonView();
        SetSpeechBubbleText("A Wise Choice...");
        FadeOutSpeechBubble(1);

        // re enable world map + get next viable enocunter hexagon tiles
        WorldManager.Instance.SetWorldMapReadyState();

        ApplyChoice(choiceName);
    }
    public void ApplyChoice(string choiceName)
    {
        Debug.Log("KingsBlessingManager.ApplyChoice() called, applying: " + choiceName);

        if (choiceName == "Gain 10 Gold")
        {
            PlayerDataManager.Instance.ModifyGold(10);
        }        
        else if (choiceName == "Gain 3 Random Common Items")
        {
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem(), true);
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem(), true);
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem(), true);
        }
        else if (choiceName == "Gain 3 Random Tier 1 Spell Books")
        {
            InventoryController.Instance.AddAbilityTomeToInventory(AbilityLibrary.Instance.GetRandomValidTierOneAbilityTomeAbility(), true);
            InventoryController.Instance.AddAbilityTomeToInventory(AbilityLibrary.Instance.GetRandomValidTierOneAbilityTomeAbility(), true);
            InventoryController.Instance.AddAbilityTomeToInventory(AbilityLibrary.Instance.GetRandomValidTierOneAbilityTomeAbility(), true);

        }
        else if (choiceName == "Enemies In The Next Two Combats Have 50% Health")
        {
            StateManager.Instance.GainState(StateLibrary.Instance.GetStateByName("King's Decree"), true);
        }
        else if (choiceName == "All Characters Gain 80 XP")
        {
            CharacterRoster.Instance.RewardAllCharactersXP(80);
        }
        else if (choiceName == "Gain 3 Random Consumables")
        {
            ConsumableManager.Instance.StartGainConsumableProcess(ConsumableLibrary.Instance.GetRandomConsumable());
            ConsumableManager.Instance.StartGainConsumableProcess(ConsumableLibrary.Instance.GetRandomConsumable());
            ConsumableManager.Instance.StartGainConsumableProcess(ConsumableLibrary.Instance.GetRandomConsumable());
        }
        else if (choiceName == "Gain A Random Rare State. Gain A Random Affliction")
        {
            StateManager.Instance.GainState(StateLibrary.Instance.GetRandomRareState(), true);
            StateManager.Instance.GainState(StateLibrary.Instance.GetRandomAffliction(), true);
        }
        else if (choiceName == "Gain A Random Epic Item. Gain A Random Affliction")
        {
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomEpicItem(), true);
            StateManager.Instance.GainState(StateLibrary.Instance.GetRandomAffliction(), true);
        }
        else if (choiceName == "Gain 3 Random Rare Items. Gain A Random Affliction")
        {
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomRareItem(), true);
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomRareItem(), true);
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomRareItem(), true);

            StateManager.Instance.GainState(StateLibrary.Instance.GetRandomAffliction(), true);
        }

        else if (choiceName == "All Characters Gain 20 Max Health")
        {
            foreach(CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyMaxHealth(20);
                character.ModifyCurrentHealth(20);
            }
        }
        else if (choiceName == "Gain A Random Rare Weapon")
        {
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomRareWeaponItem(), true);
        }
        else if (choiceName == "Gain A Random Common State")
        {
            StateManager.Instance.GainState(StateLibrary.Instance.GetRandomCommonState(), true);
        }

    }
    #endregion

    // Create Choice Buttons + Get Data
    #region
    public void PopulateChoiceDataList()
    {
        Debug.Log("KingsBlessingManager.PopulateChoiceDataList() called...");

        allChoiceData = new List<BlessingChoice>();

        allChoiceData.Add(new BlessingChoice("Gain 10 Gold"));
        allChoiceData.Add(new BlessingChoice("Gain 3 Random Common Items"));
        allChoiceData.Add(new BlessingChoice("Gain 3 Random Tier 1 Spell Books"));
        allChoiceData.Add(new BlessingChoice("Enemies In The Next Two Combats Have 50% Health"));
        allChoiceData.Add(new BlessingChoice("All Characters Gain 80 XP"));
        allChoiceData.Add(new BlessingChoice("Gain 3 Random Consumables"));
        allChoiceData.Add(new BlessingChoice("Gain A Random Rare State. Gain A Random Affliction"));
        allChoiceData.Add(new BlessingChoice("Gain 3 Random Rare Items. Gain A Random Affliction"));
        allChoiceData.Add(new BlessingChoice("Gain A Random Epic Item. Gain A Random Affliction")); 
        allChoiceData.Add(new BlessingChoice("Gain A Random Rare Weapon"));
        allChoiceData.Add(new BlessingChoice("All Characters Gain 20 Max Health"));
        allChoiceData.Add(new BlessingChoice("Gain A Random Common State"));
    }   
    public void CreateChoiceButtons()
    {
        Debug.Log("KingsBlessingManager.CreateChoiceButtons() called...");

        // Set up + Get 4 random choices
        BlessingChoice dataOne = GetRandomChoice();
        BlessingChoice dataTwo = GetRandomChoice();
        BlessingChoice dataThree = GetRandomChoice();
        BlessingChoice dataFour = GetRandomChoice();

        // Re-roll duplicate choices
        while (dataTwo.choiceName == dataOne.choiceName)
        {
            dataTwo = GetRandomChoice();
        }
        
        while (dataThree.choiceName == dataOne.choiceName ||
            dataThree.choiceName == dataTwo.choiceName)
        {
            dataThree = GetRandomChoice();
        }
        
        while (dataFour.choiceName == dataOne.choiceName ||
            dataFour.choiceName == dataTwo.choiceName ||
            dataFour.choiceName == dataThree.choiceName)
        {
            dataFour = GetRandomChoice();
        }

        // Build all buttons from data
        choiceButtonOne.BuildButtonFromData(dataOne);
        choiceButtonTwo.BuildButtonFromData(dataTwo);
        choiceButtonThree.BuildButtonFromData(dataThree);
        choiceButtonFour.BuildButtonFromData(dataFour);

    }
    public BlessingChoice GetRandomChoice()
    {
        Debug.Log("KingsBlessingManager.GetRandomChoice() called...");

        return allChoiceData[Random.Range(0, allChoiceData.Count)];
    }
    #endregion
   
    
   
}
