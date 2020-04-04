using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryEventManager : MonoBehaviour
{
    [Header("Button References")]
    public StoryEventButton actionButtonOne;
    public StoryEventButton actionButtonTwo;
    public StoryEventButton actionButtonThree;

    [Header("Parent References")]
    public GameObject visualParent;
    public GameObject buttonsVisualParent;
    public GameObject characterPanelParent;

    [Header("Text References")]
    public TextMeshProUGUI eventDescriptionText;
    public TextMeshProUGUI eventNameText;

    [Header("Image + Button References")]
    public Image eventImage;
    public GameObject continueButton;

    [Header("Properties")]
    public List<StoryEventDataSO> allStoryEvents;
    public List<StoryEventDataSO> viableStoryEvents;
    public StoryEventDataSO currentStoryEvent;
    public List<StoryWindowCharacterSlot> allCharacterSlots;

    [Header("Specific Event Properties")]
    public bool awaitingAskForWisdomChoice;
    public bool awaitingAskForHealthChoice;

    [Header("Testing Properties")]
    public bool onlyRunTestStoryEvent;
    public StoryEventDataSO testStory;

    public static StoryEventManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Initialization + Setup
    #region
    private void Start()
    {
        viableStoryEvents.AddRange(allStoryEvents);
    }
    public void SetupStoryWindowCharacter(StoryWindowCharacterSlot characterSlot, CharacterData characterData)
    {
        characterSlot.InitializeSetup(characterData);
    }
    #endregion

    // Story Event Logic
    #region
    public void LoadNewStoryEvent(StoryEventDataSO storyEvent = null)
    {
        if (storyEvent == null)
        {
            if (onlyRunTestStoryEvent)
            {
                storyEvent = testStory;
            }
            else
            {
                storyEvent = GetRandomViableStoryEventData();
            }            
        }

        currentStoryEvent = storyEvent;

        // Set up main window
        EnableEventScreen();
        eventNameText.text = storyEvent.eventName;
        eventDescriptionText.text = storyEvent.eventDescription;
        eventImage.sprite = storyEvent.eventImageOne;

        // Set up buttons;
        if (storyEvent.actionButtonOneActive)
        {
            actionButtonOne.SetUpMyComponents(storyEvent.actionButtonOneName, storyEvent.actionButtonOneDescription);
            actionButtonOne.gameObject.SetActive(true);
        }
        if (storyEvent.actionButtonTwoActive)
        {
            actionButtonTwo.SetUpMyComponents(storyEvent.actionButtonTwoName, storyEvent.actionButtonTwoDescription);
            actionButtonTwo.gameObject.SetActive(true);
        }
        if (storyEvent.actionButtonThreeActive)
        {
            actionButtonThree.SetUpMyComponents(storyEvent.actionButtonThreeName, storyEvent.actionButtonThreeDescription);
            actionButtonThree.gameObject.SetActive(true);
        }
    }
    public void ResetStoryEventWindow()
    {
        ClearAllAwaitingOrders();
        HideAllActionButtons();
        DisableContinueButton();
    }
    public StoryEventDataSO GetRandomViableStoryEventData()
    {
        StoryEventDataSO storyEventReturned = viableStoryEvents[Random.Range(0, viableStoryEvents.Count)];
        viableStoryEvents.Remove(storyEventReturned);
        return storyEventReturned;
    }
    public void ResolveAction(int buttonPressed)
    {
        // Disable Action Buttons to stop player bugging event
        //HideAllActionButtons();

        // Ultimatum At The Bridge
        if (currentStoryEvent.eventName == "Ultimatum At The Bridge")
        {
            ResolveUltimatumAtTheBridgeAction(buttonPressed);
        }

        // Blade Of The Blood God
        else if (currentStoryEvent.eventName == "Blade Of The Blood God")
        {
            ResolveBladeOfTheBloodGodAction(buttonPressed);
        }

        // Vampire Cult
        else if (currentStoryEvent.eventName == "Vampire Cult")
        {
            ResolveVampireCultAction(buttonPressed);
        }

        // War Refugees
        else if (currentStoryEvent.eventName == "War Refugees")
        {
            ResolveWarRefugeesAction(buttonPressed);
        }

        // The Three Witches
        else if (currentStoryEvent.eventName == "The Three Witches")
        {
            if(buttonPressed == 1)
            {
                awaitingAskForWisdomChoice = true;
                EnableCharacterPanelPage();
            }
            else if (buttonPressed == 2)
            {
                awaitingAskForHealthChoice = true;
                EnableCharacterPanelPage();
            }
            else if (buttonPressed == 3)
            {
                PlayerDataManager.Instance.ModifyGold(10);
                HideAllActionButtons();
                EnableContinueButton();
                ClearAllAwaitingOrders();
            }
        }

    }
    public void ClearAllAwaitingOrders()
    {
        awaitingAskForHealthChoice = false;
        awaitingAskForWisdomChoice = false;
    }
    #endregion

    // Resolve Specific Story Events
    #region
    public void ResolveUltimatumAtTheBridgeAction(int buttonPressed)
    {
        // Fight Mork
        if (buttonPressed == 1)
        {
            EventManager.Instance.StartNewBasicEncounterEvent(EnemySpawner.Instance.GetEnemyWaveByName("Mork"));
            HideAllActionButtons();
        }

        // Pay Mork
        else if (buttonPressed == 2)
        {
            PlayerDataManager.Instance.ModifyGold(-PlayerDataManager.Instance.currentGold);
            HideAllActionButtons();
            EnableContinueButton();
        }

        // Walk the long way around
        else if (buttonPressed == 3)
        {
            StateManager.Instance.GainState(StateLibrary.Instance.GetStateByName("Exhausted"));
            HideAllActionButtons();
            EnableContinueButton();
            
        }
    }
    public void ResolveBladeOfTheBloodGodAction(int buttonPressed)
    {
        // Take the Sword
        if (buttonPressed == 1)
        {
            InventoryController.Instance.AddItemToInventory(ItemLibrary.Instance.GetItemByName("Blade Of The Blood God"));
            StateManager.Instance.GainState(StateLibrary.Instance.GetStateByName("Curse Of The Blood God"));
            HideAllActionButtons();
            EnableContinueButton();
        }

        // Pillage the Shrine
        else if (buttonPressed == 2)
        {
            PlayerDataManager.Instance.ModifyGold(15);            
            StateManager.Instance.GainState(StateLibrary.Instance.GetStateByName("Shame"));
            HideAllActionButtons();
            EnableContinueButton();
        }

    }
    public void ResolveVampireCultAction(int buttonPressed)
    {
        // Join The Cult
        if (buttonPressed == 1)
        {
            foreach(CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyMaxHealth(-character.maxHealth / 2);
            }
            StateManager.Instance.GainState(StateLibrary.Instance.GetStateByName("Vampirism"));
            HideAllActionButtons();
            EnableContinueButton();
        }

        // Donate Blood
        else if (buttonPressed == 2)
        {
            PlayerDataManager.Instance.ModifyGold(15);
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                float healthLost = character.maxHealth * 0.3f;
                character.ModifyCurrentHealth((int)-healthLost);
            }
            HideAllActionButtons();
            EnableContinueButton();
        }

    }
    public void ResolveWarRefugeesAction(int buttonPressed)
    {
        // Help The Refugees
        if (buttonPressed == 1)
        {
            // Cant pay 10g if player doesnt actually have 10g
            if(PlayerDataManager.Instance.currentGold >= 10)
            {
                PlayerDataManager.Instance.ModifyGold(-10);
                StateManager.Instance.GainState(StateLibrary.Instance.GetStateByName("Heroism"));
                HideAllActionButtons();
                EnableContinueButton();
            }
           
        }

        // Rob The Refugees
        else if (buttonPressed == 2)
        {
            PlayerDataManager.Instance.ModifyGold(10);
            StateManager.Instance.GainState(StateLibrary.Instance.GetStateByName("Wanted"));
            HideAllActionButtons();
            EnableContinueButton();
        }

    }
    #endregion

    // Button Click + Mouse Events
    #region
    public void OnActionButtonClicked(int buttonNumber)
    {
        ResolveAction(buttonNumber);
    }
    public void OnContinueButtonClicked()
    {
        DisableEventScreen();
        WorldManager.Instance.canSelectNewEncounter = true;
        UIManager.Instance.EnableWorldMapView();

    }
    public void OnCharacterPanelBackButtonClicked()
    {
        ClearAllAwaitingOrders();
        ShowAllActionButtons();
        DisableCharacterPanelPage();
        
    }
    public void HandleCharacterWindowClicked(StoryWindowCharacterSlot character)
    {
        // The Three Witches
        if (currentStoryEvent.eventName == "The Three Witches")
        {
            // heal to full HP
            if (awaitingAskForHealthChoice)
            {
                character.myCharacterData.ModifyCurrentHealth(character.myCharacterData.maxHealth);
            }

            // gain a level
            else if (awaitingAskForWisdomChoice)
            {
                character.myCharacterData.ModifyCurrentXP(100);
            }

            ClearAllAwaitingOrders();
            DisableCharacterPanelPage();
            HideAllActionButtons();
            EnableContinueButton();
            
        }
    }
    #endregion


    // View + Visbility Logic
    #region
    public void HideAllActionButtons()
    {
        // Disable Buttons
        actionButtonOne.gameObject.SetActive(false);
        actionButtonTwo.gameObject.SetActive(false);
        actionButtonThree.gameObject.SetActive(false);
    }
    public void ShowAllActionButtons()
    {
        if (currentStoryEvent.actionButtonOneActive)
        {
            actionButtonOne.gameObject.SetActive(true);
        }
        if (currentStoryEvent.actionButtonTwoActive)
        {
            actionButtonTwo.gameObject.SetActive(true);
        }
        if (currentStoryEvent.actionButtonThreeActive)
        {
            actionButtonThree.gameObject.SetActive(true);
        }
        
    }
    public void EnableEventScreen()
    {
        visualParent.SetActive(true);
    }
    public void DisableEventScreen()
    {
        visualParent.SetActive(false);
    }
    public void EnableContinueButton()
    {
        continueButton.SetActive(true);
    }
    public void DisableContinueButton()
    {
        continueButton.SetActive(false);
    }
    public void EnableCharacterPanelPage()
    {
        characterPanelParent.SetActive(true);
    }
    public void DisableCharacterPanelPage()
    {
        characterPanelParent.SetActive(false);
    }
    #endregion
}
