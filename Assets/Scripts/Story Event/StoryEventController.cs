using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class StoryEventController : MonoBehaviour
{
    // Propeties + Component References
    #region
    public enum ContinueButtonEvent { None, OpenWorldMap, TriggerCombatEvent };

    [Header("Properties")]
    public StoryDataSO currentStoryData;
    public StoryDataSO testingStoryData;
    public List<StoryChoiceButton> activeChoiceButtons;

    [Header("Parent + Transform Components")]
    public GameObject choiceButtonsParent;
    public GameObject continueButtonParent;

    [Header("Text Component References")]
    public TextMeshProUGUI storyNameText;
    public TextMeshProUGUI descriptionWindowText;

    [Header("Image Component References")]
    public Image storyImage;

    [Header("Prefab References")]
    public GameObject choiceButtonPrefab;

    [Header("Continue Button Pressed Properties")]    
    public ContinueButtonEvent eventFiredOnContinueButtonClicked;
    public EnemyWaveSO combatEventAwaitingStart;
    #endregion

    // Singleton Pattern
    #region
    public static StoryEventController Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        ResetAndFlushAllPropertiesAndViews();
        // REMOVE IN FUTURE: FOR TESTING ONLY
        BuildFromStoryEventData(testingStoryData);
    }
    #endregion

    // General Logic
    #region
    public void BuildFromStoryEventData(StoryDataSO storyData)
    {
        Debug.Log("StoryEventController.BuildFromStoryEventData() called, building event: " + storyData.storyName);

        // cache data
        currentStoryData = storyData;

        // set description text
        SetEventDescriptionText(storyData.storyInitialDescription);

        // set name text
        SetStoryNameText(storyData.storyName);

        // set event image
        SetStoryImage(storyData.storyInitialSprite);

        // Load first page buttons
        BuildAllChoiceButtonsFromStoryPage(storyData.pageOneChoices);
    }
    #endregion

    // Set Text Values Logic
    #region
    public void SetEventDescriptionText(string newText)
    {
        descriptionWindowText.text = newText;
    }
    public void SetStoryNameText(string newText)
    {
        storyNameText.text = newText;
    }
    public string ConvertChoiceConsequenceToString(ChoiceConsequence choiceConsequence)
    {
        Debug.Log("StoryEventController.ConvertChoiceConsequenceToString() called...");

        string stringReturned = "";

        if (choiceConsequence.consequenceType == ChoiceConsequence.ConsequenceType.EventEnds)
        {
            stringReturned = "Event Ends";
        }
        else if (choiceConsequence.consequenceType == ChoiceConsequence.ConsequenceType.AllCharactersGainXP)
        {
            stringReturned = "All characters gain " + choiceConsequence.xpGainAmount.ToString() + " XP";
        }
        else if (choiceConsequence.consequenceType == ChoiceConsequence.ConsequenceType.GainGold)
        {
            stringReturned = "Gain " + choiceConsequence.goldGainAmount.ToString() + "g";
        }
        else if (choiceConsequence.consequenceType == ChoiceConsequence.ConsequenceType.GainSpecificItem)
        {
            stringReturned = "Gain item: " + choiceConsequence.specificItemGained.Name;
        }
        else if (choiceConsequence.consequenceType == ChoiceConsequence.ConsequenceType.TriggerCombatEvent)
        {
            stringReturned = "Triggers combat event: " + choiceConsequence.combatEvent.waveName;
        }
        else
        {
            Debug.Log("StoryEventController.ConvertChoiceRequirementToString() detected that a string conversion for" +
                " the enum value " + choiceConsequence.ToString() + " has not been defined, returning an empty string...");
        }

        return stringReturned;
    }
    public string ConvertChoiceRequirementToString(ChoiceRequirment choiceRequirement)
    {
        Debug.Log("StoryEventController.ConvertChoiceRequirementToString() called...");

        string stringReturned = "";

        if (choiceRequirement.requirementType == ChoiceRequirment.RequirementType.None)
        {
            stringReturned = "None";
        }
        else if (choiceRequirement.requirementType == ChoiceRequirment.RequirementType.HasBackground)
        {
            stringReturned = choiceRequirement.backgroundRequirement.ToString() + " in party";
        }
        else if (choiceRequirement.requirementType == ChoiceRequirment.RequirementType.HasRace)
        {
            stringReturned = choiceRequirement.raceRequirement.ToString() + " in party";
        }
        else if (choiceRequirement.requirementType == ChoiceRequirment.RequirementType.HasEnoughGold)
        {
            stringReturned = choiceRequirement.goldAmountRequired.ToString() + "g";
        }
        else
        {
            Debug.Log("StoryEventController.ConvertChoiceRequirementToString() detected that a string conversion for" +
                " the enum value " + choiceRequirement.ToString() + " has not been defined, returning an empty string...");
        }

        return stringReturned;
    }
    public void AutoSetChoiceButtonRequirementText(StoryChoiceButton button, StoryChoiceDataSO data)
    {
        Debug.Log("StoryEventController.AutoSetChoiceButtonRequirementText() called for button: '" +
            data.description +"'");

        string requirementString = "";

        // if there are no requirements in choice req list, just set text to 'None'
        if (data.choiceRequirements.Count == 0)
        {
            requirementString = "None";
        }
        else
        {
            foreach (ChoiceRequirment cr in data.choiceRequirements)
            {
                // Add the choice requirement to the final string
                requirementString += ConvertChoiceRequirementToString(cr);

                // Seperate each requirement with a comma, if it is not the last req in list
                if (cr != data.choiceRequirements.Last())
                {
                    requirementString += ", ";
                }
            }
        }        

        // Apply generated string to description text component
        button.requirementsText.text = requirementString;
    }
    public void AutoSetChoiceButtonSuccessConsequencesText(StoryChoiceButton button, StoryChoiceDataSO data)
    {
        Debug.Log("StoryEventController.AutoSetChoiceButtonSuccessConsequencesText() called for button: '" +
            data.description + "'");

        string consequencesString = "";

        foreach (ChoiceConsequence cc in data.onSuccessConsequences)
        {
            // Add the choice requirement to the final string
            consequencesString += ConvertChoiceConsequenceToString(cc);

            // Seperate each requirement with a comma, if it is not the last req in list
            if (cc != data.onSuccessConsequences.Last())
            {
                consequencesString += ", ";
            }
        }

        // Apply generated string to description text component
        button.successConsequenceText.text = consequencesString;
    }
    public void AutoSetChoiceButtonFailureConsequencesText(StoryChoiceButton button, StoryChoiceDataSO data)
    {
        Debug.Log("StoryEventController.AutoSetChoiceButtonFailureConsequencesText() called for button: '" +
           data.description + "'");

        string consequencesString = "";

        // Disable the on failure text elements if they are not needed
        if (data.onFailureConsequences.Count == 0)
        {
            button.onFailureTextRowParent.SetActive(false);
        }        
        else
        {
            foreach (ChoiceConsequence cc in data.onFailureConsequences)
            {
                // Add the choice requirement to the final string
                consequencesString += ConvertChoiceConsequenceToString(cc);

                // Seperate each requirement with a comma, if it is not the last req in list
                if (cc != data.onFailureConsequences.Last())
                {
                    consequencesString += ", ";
                }
            }
        }       

        // Apply generated string to description text component
        button.failureConsequenceText.text = consequencesString;
    }
    #endregion

    // View + GUI Logic
    #region
    public void ResetAndFlushAllPropertiesAndViews()
    {
        Debug.Log("StoryEventController.ResetAndFlushAllPropertiesAndViews()");

        DestroyAllChoiceButtons();
        SetContinueButtonVisibility(false);
        SetEventDescriptionText("");
        SetStoryNameText("");
        eventFiredOnContinueButtonClicked = ContinueButtonEvent.None;
        combatEventAwaitingStart = null;
    }
    public void DestroyAllChoiceButtons()
    {
        Debug.Log("StoryEventController.DestroyAllChoiceButtons() called...");

        // Destroy all button gameobjects
        foreach(StoryChoiceButton button in activeChoiceButtons)
        {
            Destroy(button.gameObject);
        }

        // flush persistent button data list
        activeChoiceButtons.Clear();

    }
    public void SetContinueButtonVisibility(bool onOrOff)
    {
        continueButtonParent.SetActive(onOrOff);
    }
    #endregion

    // Set Image Logic
    #region
    public void SetStoryImage(Sprite newImage)
    {
        storyImage.sprite = newImage;
    }
    #endregion

    // Choice buttons logic
    #region
    public void BuildAllChoiceButtonsFromStoryPage(List<StoryChoiceDataSO> choicesList)
    {
        Debug.Log("StoryEventController.BuildAllChoiceButtonsFromStoryPage() called...");

        foreach(StoryChoiceDataSO data in choicesList)
        {
            // Build a new choice button
            StoryChoiceButton newButton = BuildChoiceButtonFromChoiceData(data);

            // Does the player meet the requirements to enable this choice?
            if (!AreChoiceButtonRequirementsMet(data))
            {
                // They don't, lock the choice button
                LockChoiceButton(newButton);
            }           
        }
    }
    public StoryChoiceButton BuildChoiceButtonFromChoiceData(StoryChoiceDataSO data)
    {
        Debug.Log("StoryEventController.BuildChoiceButtonFromChoiceData() called for choice: " + data.description);

        // Create game object, parent to the vertical fitter
        StoryChoiceButton newButton = Instantiate(choiceButtonPrefab, choiceButtonsParent.transform).GetComponent<StoryChoiceButton>();

        // Cache data
        newButton.myChoiceData = data;
        activeChoiceButtons.Add(newButton);

        // Set button main label text
        newButton.descriptionText.text = data.description;

        // set requirment text
        AutoSetChoiceButtonRequirementText(newButton, data);

        // set consequence texts
        AutoSetChoiceButtonSuccessConsequencesText(newButton, data);
        AutoSetChoiceButtonFailureConsequencesText(newButton, data);

        // calculate success chance, set related text values
        newButton.successChanceText.text = CalculateFinalSuccessChanceOfChoiceData(newButton.myChoiceData).ToString() + "%";

        // return the button just made
        return newButton;

    }
    public bool AreChoiceButtonRequirementsMet(StoryChoiceDataSO data)
    {
        Debug.Log("StoryEventController.AreChoiceButtonRequirementsMet() checking button: " + data.description);

        // return true;
        // TODO: uncomment in future, when we want this to work properly in the actual game
        
        bool boolReturned = true;
        List<bool> requirementChecks = new List<bool>();

        // Evaluate each requirement condition individually
        foreach(ChoiceRequirment cr in data.choiceRequirements)
        {
            // Check for none: guaranteed pass
            if(cr.requirementType == ChoiceRequirment.RequirementType.None)
            {
                Debug.Log("Choice Requirement Met: Choice Button does not have any requirements");
                requirementChecks.Add(true);
            }

            // Check Gold
            else if (cr.requirementType == ChoiceRequirment.RequirementType.HasEnoughGold)
            { 
                if(PlayerDataManager.Instance.currentGold >= cr.goldAmountRequired)
                {
                    Debug.Log("Choice Requirement Met: Choice Button has a gold requirement of " + cr.goldAmountRequired + 
                        ", player currently has " + PlayerDataManager.Instance.currentGold);
                    requirementChecks.Add(true);
                }
                else
                {
                    Debug.Log("Choice Requirement NOT Met: Choice Button has a gold requirement of " + cr.goldAmountRequired +
                        ", player currently has " + PlayerDataManager.Instance.currentGold);
                    requirementChecks.Add(false);
                }
               
            }

            // Check backgrounds
            else if (cr.requirementType == ChoiceRequirment.RequirementType.HasBackground)
            {
                bool passedBgCheck = false;

                if (CharacterRoster.Instance)
                {
                    foreach (CharacterData cd in CharacterRoster.Instance.allCharacterDataObjects)
                    {
                        if (cd.backgrounds.Contains(cr.backgroundRequirement))
                        {
                            passedBgCheck = true;
                        }
                    }
                }                

                if (passedBgCheck)
                {
                    Debug.Log("Choice Requirement Met: Player has a character in party with the required background of "
                        + cr.backgroundRequirement);

                    requirementChecks.Add(true);
                }
                else
                {
                    Debug.Log("Choice Requirement NOT Met: Player does not have a character in party with the required background of "
                        + cr.backgroundRequirement);
                    requirementChecks.Add(false);
                }
            }

            // Check racials
            else if (cr.requirementType == ChoiceRequirment.RequirementType.HasRace)
            {
                bool passedRacialCheck = false;

                foreach (CharacterData cd in CharacterRoster.Instance.allCharacterDataObjects)
                {
                    if (cd.myRace == cr.raceRequirement)
                    {
                        passedRacialCheck = true;
                    }
                }

                if (passedRacialCheck)
                {
                    Debug.Log("Choice Requirement Met: Player has a character in party with the required race of "
                        + cr.raceRequirement);
                    requirementChecks.Add(true);
                }
                else
                {
                    Debug.Log("Choice Requirement NOT Met: Player does not have a character in party with the required race of "
                        + cr.raceRequirement);
                    requirementChecks.Add(false);
                }
            }

            // if we get here, it means the code to check the enum related to
            // the requirement has not been written...
            else
            {
                Debug.Log("StoryEventController.AreChoiceButtonRequirementsMet() detected that the requirement type enum value '" + cr.requirementType.ToString() +
                    "' is not recognized and cannot be evaluated, returning false by default..."
                        + cr.raceRequirement);
                requirementChecks.Add(false);
            }
        }

        // did we fail any of the requirment checks?
        if (requirementChecks.Contains(false))
        {
            // we did, return false
            boolReturned = false;
        }

        return boolReturned;
        
    }
    public int CalculateFinalSuccessChanceOfChoiceData(StoryChoiceDataSO choice)
    {
        Debug.Log("StoryEventController.CalculateFinalSuccessChanceOfChoiceData() called, " +
            "base success chance = " + choice.baseSuccessChance.ToString()+ "%");

        // initialize return value at the choice's base success chance
        int chanceReturned = choice.baseSuccessChance;

        foreach(SuccessChanceModifier scm in choice.successChanceModifiers)
        {
            chanceReturned += CalculateFinalValueOfSuccessChanceModifier(scm);
        }

        // Prevent success chance exceding 100%
        if(chanceReturned > 100)
        {
            chanceReturned = 100;
        }
        // Prevent success chance dropping below 0%
        else if (chanceReturned < 0)
        {
            chanceReturned = 0;
        }

        Debug.Log("StoryEventController.CalculateFinalSuccessChanceOfChoiceData() final success chance calculated = " +
            chanceReturned.ToString() + "%");

        return chanceReturned;
    }
    public int CalculateFinalValueOfSuccessChanceModifier(SuccessChanceModifier element)
    {
        Debug.Log("StoryEventController.CalculateFinalValueOfSuccessChanceModifier() called...");

        int valueReturned = 0;

        // Check background modifier
        if (element.chanceTypeModifier == SuccessChanceModifier.ChanceModifierType.HasBackground)
        {
            foreach (CharacterData cd in CharacterRoster.Instance.allCharacterDataObjects)
            {
                if (cd.backgrounds.Contains(element.backgroundRequirement))
                {
                    valueReturned += element.chancePercentageModifier;
                }
            }

        }

        // Check racial modifier
        else if (element.chanceTypeModifier == SuccessChanceModifier.ChanceModifierType.HasRace)
        {
            foreach (CharacterData cd in CharacterRoster.Instance.allCharacterDataObjects)
            {
                if (cd.myRace == element.raceRequirement)
                {
                    valueReturned += element.chancePercentageModifier;
                }
            }
        }

        // Check state modifier
        else if (element.chanceTypeModifier == SuccessChanceModifier.ChanceModifierType.HasState)
        {
            if (StateManager.Instance.DoesPlayerAlreadyHaveState(element.stateRequirement.stateName))
            {
                valueReturned += element.chancePercentageModifier;
            }
        }

        return valueReturned;
    }
    public void LockChoiceButton(StoryChoiceButton button)
    {
        button.SetPanelColour(button.disabledColour);
        button.locked = true;
    }    
    public int GenerateRandomRollResult(int lowerLimit = 1, int upperLimit = 101)
    {
        Debug.Log("StoryEventController.GenerateRandomRollResult() called, generating random number between "
            + lowerLimit.ToString() + " and " + (upperLimit -1).ToString());

        int rollResult = Random.Range(lowerLimit, upperLimit);
        Debug.Log("GenerateRandomRollResult() rolled a " + rollResult.ToString());
        return rollResult;
    }
    public bool DidChoicePassSuccessRoll(StoryChoiceButton choiceButton)
    {
        Debug.Log("StoryEventController.DidChoicePassSuccessRoll() called...");

        bool boolReturned = false;

        // Calculate the roll required to pass the success check
        int rollResultRequired = CalculateFinalSuccessChanceOfChoiceData(choiceButton.myChoiceData);

        // Roll for success
        int rollResultActual = GenerateRandomRollResult();

        if(rollResultActual <= rollResultRequired)
        {
            boolReturned = true;
        }

        return boolReturned;
    }
    #endregion

    // Mouse + Input Logic
    #region
    public void OnChoiceButtonClicked(StoryChoiceButton buttonClicked)
    {
        Debug.Log("StoryEventController.OnChoiceButtonClicked() called for button: " + buttonClicked.myChoiceData.description);

        StartResolveChoiceProcess(buttonClicked);
    }
    public void OnContinueButtonClicked()
    {
        Debug.Log("StoryEventController.OnContinueButtonClicked() called...");
    }
    #endregion

    // Resolve Events + Choices + Consequences
    #region
    public void StartResolveChoiceProcess(StoryChoiceButton buttonClicked)
    {
        Debug.Log("StoryEventController.StartResolveChoiceProcess() called...");

        // Did we pass the success chance roll?
        if (DidChoicePassSuccessRoll(buttonClicked))
        {
            // we did, start handle pass result process
            StartResolveChoiceSuccessfulProcess(buttonClicked);
        }
        else
        {
            // we failed to roll high enough, start handle fail result process
            StartResolveChoiceFailureProcess(buttonClicked);
        }
    }
    public void StartResolveChoiceSuccessfulProcess(StoryChoiceButton choiceButton)
    {
        Debug.Log("StoryEventController.StartResolveChoiceSuccessfulProcess() called...");

        // Resolve player relevant effects (xp rewards, gain gold, etc)
        foreach(ChoiceConsequence consequence in choiceButton.myChoiceData.onSuccessConsequences)
        {
            ResolveChoiceConsequence(consequence);
        }

        // Resolve all GUI updates and events
        foreach(ChoiceResolvedGuiEvent guiEvent in choiceButton.myChoiceData.onSuccessGuiEvents)
        {
            ResolveChoiceGuiEvent(guiEvent);
        }

    }
    public void StartResolveChoiceFailureProcess(StoryChoiceButton choiceButton)
    {
        Debug.Log("StoryEventController.StartResolveChoiceSuccessfulProcess() called...");

        // Resolve player relevant effects (xp rewards, gain gold, etc)
        foreach (ChoiceConsequence consequence in choiceButton.myChoiceData.onFailureConsequences)
        {
            ResolveChoiceConsequence(consequence);
        }

        // Resolve all GUI updates and events
        foreach (ChoiceResolvedGuiEvent guiEvent in choiceButton.myChoiceData.onFailureGuiEvents)
        {
            ResolveChoiceGuiEvent(guiEvent);
        }

    }
    public void ResolveChoiceConsequence(ChoiceConsequence consequence)
    {
        Debug.Log("StoryEventController.ResolveChoiceConsequence() called, resolving effect of consequence: "
            + consequence.consequenceType.ToString());
        if (consequence.consequenceType == ChoiceConsequence.ConsequenceType.EventEnds)
        {
            // 
        }
        else if (consequence.consequenceType == ChoiceConsequence.ConsequenceType.AllCharactersGainXP)
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                character.ModifyCurrentXP(consequence.xpGainAmount);
            }
        }
        else if (consequence.consequenceType == ChoiceConsequence.ConsequenceType.GainGold)
        {
            PlayerDataManager.Instance.ModifyGold(consequence.goldGainAmount);
        }
        else if (consequence.consequenceType == ChoiceConsequence.ConsequenceType.GainSpecificItem)
        {
            InventoryController.Instance.AddItemToInventory(consequence.specificItemGained, true);
        }
        else if (consequence.consequenceType == ChoiceConsequence.ConsequenceType.TriggerCombatEvent)
        {
            SetAwaitingCombatEventState(consequence.combatEvent);
        }
    }
    public void ResolveChoiceGuiEvent(ChoiceResolvedGuiEvent guiEvent)
    {
        Debug.Log("StoryEventController.ResolveChoiceGuiEvent() called, resolving GUI event: " + guiEvent.guiEvent.ToString());

        if (guiEvent.guiEvent == ChoiceResolvedGuiEvent.GuiEvent.UpdateEventDescription)
        {
            SetEventDescriptionText(guiEvent.newEventDescription);
        }
        else if (guiEvent.guiEvent == ChoiceResolvedGuiEvent.GuiEvent.DestroyAllChoiceButtons)
        {
            DestroyAllChoiceButtons();
        }
        else if (guiEvent.guiEvent == ChoiceResolvedGuiEvent.GuiEvent.EnableContinueButton)
        {
            SetContinueButtonVisibility(true);
        }
    }
    public void SetAwaitingCombatEventState(EnemyWaveSO combatAwaited)
    {
        Debug.Log("StoryEventController.SetAwaitingCombatEventState() called, on continute button clicked, combat event "
            + combatAwaited.waveName + " will be triggered");

        combatEventAwaitingStart = combatAwaited;
        eventFiredOnContinueButtonClicked = ContinueButtonEvent.TriggerCombatEvent;
    }
    #endregion




    /*
     * On success/failure events ideas
     * - send new description to description text
     * - send new sprite to image holder
     * - load more choice buttons / move to next choice page
     * - clear all choice buttons
     * - enable continue button
     * - set continute button event to trigger (starts new combat, continute on with journey / load world map, etc)
     * 
     * 
     */

}
