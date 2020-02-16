using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public UniversalCharacterModel modelOne;
    public UniversalCharacterModel modelTwo;
    public UniversalCharacterModel modelThree;
    public UniversalCharacterModel modelFour;
    public UniversalCharacterModel kingModel;

    [Header("Properties")]
    private List<BlessingChoice> allChoiceData;
    public List<BlessingChoiceButton> allChoiceButtons;
    public BlessingChoiceButton choiceButtonOne;
    public BlessingChoiceButton choiceButtonTwo;
    public BlessingChoiceButton choiceButtonThree;
    public BlessingChoiceButton choiceButtonFour;
    public GameObject continueButton;
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
    }
    #endregion

    // View Logic
    #region
    public void EnableView()
    {
        visualParent.SetActive(true);
    }
    public void DisableView()
    {
        visualParent.SetActive(false);
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
    #endregion

    // Events
    #region
    public void OnContinueButtonClicked()
    {
        DisableView();
        UIManager.Instance.OnWorldMapButtonClicked();
    }
    public void StartChoiceProcess(string choiceName)
    {
        Debug.Log("KingsBlessingManager.StartChoiceProcess() called for: " + choiceName);

        DisableAllChoiceButtonViews();
        EnableContinuteButtonView();

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
        else if (choiceName == "Choose A Rare Item")
        {

        }
        else if (choiceName == "Gain 3 Random Common Items")
        {
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        }
        else if (choiceName == "Gain 2 Random Spell Books")
        {
            //
        }
        else if (choiceName == "Enemies In The Next Two Combats Have 50% Health")
        {
            StateManager.Instance.GainState(StateLibrary.Instance.GetStateByName("King's Decree"));
        }
        else if (choiceName == "All Characters Gain 75 XP")
        {
            CharacterRoster.Instance.RewardAllCharactersXP(75);
        }
        else if (choiceName == "Gain 2 Random Consumables")
        {
            ConsumableManager.Instance.StartGainConsumableProcess(ConsumableLibrary.Instance.GetRandomConsumable());
            ConsumableManager.Instance.StartGainConsumableProcess(ConsumableLibrary.Instance.GetRandomConsumable());
        }
        else if (choiceName == "Gain A Random State. Gain A Random Afflication")
        {
            StateManager.Instance.GainState(StateLibrary.Instance.GetRandomStateReward());
            StateManager.Instance.GainState(StateLibrary.Instance.GetRandomAfflication());
        }
        else if (choiceName == "Gain A Random Epic Item. Gain A Random Afflication")
        {
            // TO DO: GAIN EPIC ITEM
            StateManager.Instance.GainState(StateLibrary.Instance.GetRandomAfflication());
        }
        else if (choiceName == "Gain 3 Random Rare Items. Gain A Random Afflication")
        {
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomRareItem());
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomRareItem());
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetRandomRareItem());
        }

    }
    #endregion

    // Create Choice Buttons + Get Data
    #region
    public void PopulateChoiceDataList()
    {
        Debug.Log("KingsBlessingManager.PopulateChoiceDataList() called...");

        allChoiceData = new List<BlessingChoice>();

        //allChoiceData.Add(new BlessingChoice("Gain 10 Gold"));
        //allChoiceData.Add(new BlessingChoice("Choose A Rare Item"));
       // allChoiceData.Add(new BlessingChoice("Gain 3 Random Common Items"));
        //allChoiceData.Add(new BlessingChoice("Gain 2 Random Spell Books"));
        allChoiceData.Add(new BlessingChoice("Enemies In The Next Two Combats Have 50% Health"));
        allChoiceData.Add(new BlessingChoice("All Characters Gain 75 XP"));
        allChoiceData.Add(new BlessingChoice("Gain 2 Random Consumables"));
        allChoiceData.Add(new BlessingChoice("Gain A Random State. Gain A Random Afflication"));
        //allChoiceData.Add(new BlessingChoice("Gain A Random Epic Item. Gain A Random Afflication"));
       // allChoiceData.Add(new BlessingChoice("Gain 3 Random Rare Items. Gain A Random Afflication"));
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
