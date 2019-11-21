using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryEventManager : Singleton<StoryEventManager>
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


    // Initialization + Setup
    private void Start()
    {
        viableStoryEvents.AddRange(allStoryEvents);
    }
    public void SetupStoryWindowCharacter(StoryWindowCharacterSlot characterSlot, CharacterData characterData)
    {
        characterSlot.InitializeSetup(characterData);
    }

    // Story Event Logic
    #region
    public void LoadNewStoryEvent(StoryEventDataSO storyEvent = null)
    {
        if(storyEvent == null)
        {
            storyEvent = GetRandomViableStoryEventData();
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
    public StoryEventDataSO GetRandomStoryEventData()
    {
        return allStoryEvents[Random.Range(0, allStoryEvents.Count)];
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
        HideAllActionButtons();

        // Ultimatum At The Bridge
        if (currentStoryEvent.eventName == "Ultimatum At The Bridge")
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
                EnableContinueButton();
            }

            // Walk the long way around
            else if (buttonPressed == 3)
            {
                EnableContinueButton();
                StateManager.Instance.GainState(StateLibrary.Instance.GetStateByName("Over Marched"));
            }
        }

        // The Three Witches
        else if (currentStoryEvent.eventName == "The Three Witches")
        {
            // Ask For Wisdom
            if (buttonPressed == 1)
            {
                EnableCharacterPanelPage();
                awaitingAskForWisdomChoice = true;
            }

            // Ask For Health
            else if (buttonPressed == 2)
            {
                // enable character panel view, awaiting choice
                EnableCharacterPanelPage();
                awaitingAskForHealthChoice = true;
            }

            // Ask For Riches
            else if (buttonPressed == 3)
            {
                PlayerDataManager.Instance.ModifyGold(100);
                EnableContinueButton();
            }
        }
    }
    public void ClearAllAwaitingOrders()
    {
        awaitingAskForHealthChoice = false;
        awaitingAskForWisdomChoice = false;
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
        WorldMap.Instance.canSelectNewEncounter = true;
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
                character.myCharacterData.ModifyCurrentHealth(character.myCharacterData.MaxHealth);
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
