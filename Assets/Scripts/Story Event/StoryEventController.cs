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
    [Header("Properties")]
    public StoryDataSO currentStoryData;
    public StoryDataSO testingStoryData;

    [Header("Parent + Transform Components")]
    public GameObject choiceButtonsParent;

    [Header("Text Component References")]
    public TextMeshProUGUI storyNameText;
    public TextMeshProUGUI descriptionWindowText;

    [Header("Image Component References")]
    public Image storyImage;

    [Header("Prefab References")]
    public GameObject choiceButtonPrefab;
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
        SetDescriptionText(storyData.storyInitialDescription);

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
    public void SetDescriptionText(string newText)
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

    // Set Image Logic
    #region
    public void SetStoryImage(Sprite newImage)
    {
        storyImage.sprite = newImage;
    }
    #endregion

    // Load + Build choice buttons logic
    #region
    public void BuildAllChoiceButtonsFromStoryPage(List<StoryChoiceDataSO> choicesList)
    {
        Debug.Log("StoryEventController.BuildAllChoiceButtonsFromStoryPage() called...");

        foreach(StoryChoiceDataSO data in choicesList)
        {
            // Does the player meet the requirements to enable this choice?
            if (AreChoiceButtonRequirementsMet(data))
            {
                // They do, build the choice button
                BuildChoiceButtonFromChoiceData(data);
            }
           
        }
    }
    public void BuildChoiceButtonFromChoiceData(StoryChoiceDataSO data)
    {
        Debug.Log("StoryEventController.BuildChoiceButtonFromChoiceData() called for choice: " + data.description);

        // Create game object, parent to the vertical fitter
        StoryChoiceButton newButton = Instantiate(choiceButtonPrefab, choiceButtonsParent.transform).GetComponent<StoryChoiceButton>();

        // Cache SO data
        newButton.myChoiceData = data;

        // Set button main label text
        newButton.descriptionText.text = data.description;

        // set requirment text
        AutoSetChoiceButtonRequirementText(newButton, data);

        // set consequence texts
        AutoSetChoiceButtonSuccessConsequencesText(newButton, data);
        AutoSetChoiceButtonFailureConsequencesText(newButton, data);

        // calculate success chance, set related text values
        newButton.successChanceText.text = CalculateFinalSuccessChanceOfChoiceData(newButton.myChoiceData).ToString();

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

                foreach(CharacterData cd in CharacterRoster.Instance.allCharacterDataObjects)
                {
                    if (cd.backgrounds.Contains(cr.backgroundRequirement))
                    {
                        passedBgCheck = true;
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
        Debug.Log("StoryEventController.CalculateFinalSuccessChanceOfChoiceData() called...");

        // initialize return value at the choice's base success chance
        int chanceReturned = choice.baseSuccessChance;

        foreach(SuccessChanceModifier scm in choice.successChanceModifiers)
        {
            chanceReturned += CalculateFinalValueOfSuccessChanceModifier(scm);
        }


        return chanceReturned;
    }
    public int CalculateFinalValueOfSuccessChanceModifier(SuccessChanceModifier element)
    {
        Debug.Log("StoryEventController.CalculateFinalValueOfSuccessChanceModifier() called...");

        int valueReturned = 0;

        if (element.chanceTypeModifier == SuccessChanceModifier.ChanceModifierType.HasBackground)
        {
            if (CharacterRoster.Instance)
            {
                foreach (CharacterData cd in CharacterRoster.Instance.allCharacterDataObjects)
                {
                    if (cd.backgrounds.Contains(element.backgroundRequirement))
                    {
                        valueReturned += element.chancePercentageModifier;
                    }
                }
            }
           
        }
        else if (element.chanceTypeModifier == SuccessChanceModifier.ChanceModifierType.HasRace)
        {
            if (CharacterRoster.Instance)
            {
                foreach (CharacterData cd in CharacterRoster.Instance.allCharacterDataObjects)
                {
                    if (cd.myRace == element.raceRequirement)
                    {
                        valueReturned += element.chancePercentageModifier;
                    }
                }
            }              
        }
        else if (element.chanceTypeModifier == SuccessChanceModifier.ChanceModifierType.HasState)
        {
            if (StateManager.Instance && 
                 StateManager.Instance.DoesPlayerAlreadyHaveState(element.stateRequirement.stateName))
            {
                valueReturned += element.chancePercentageModifier;
            }
        }

        return valueReturned;
    }
    #endregion


}
