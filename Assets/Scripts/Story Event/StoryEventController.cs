using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class StoryEventController : MonoBehaviour
{
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

    // Logic
    #region
    public void BuildFromStoryEventData(StoryDataSO storyData)
    {
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
    #endregion

    // Set Image Logic
    #region
    public void SetStoryImage(Sprite newImage)
    {
        storyImage.sprite = newImage;
    }
    #endregion

    // Load choice buttons logic
    public void BuildAllChoiceButtonsFromStoryPage(List<StoryChoiceDataSO> choicesList)
    {
        foreach(StoryChoiceDataSO data in choicesList)
        {
            BuildChoiceButtonFromChoiceData(data);
        }
    }
    public void BuildChoiceButtonFromChoiceData(StoryChoiceDataSO data)
    {
        // Create game object, parent to the vertical fitter
        StoryChoiceButton newButton = Instantiate(choiceButtonPrefab, choiceButtonsParent.transform).GetComponent<StoryChoiceButton>();

        // Cache SO data
        newButton.myChoiceData = data;

        // Set button main label text
        newButton.descriptionText.text = data.description;

        // set requirment text
        AutoSetChoiceButtonRequirementText(newButton, data);

        // calculate success chance, set related text values

        
    }


    // Requirement Text Logic
    #region
    public void AutoSetChoiceButtonRequirementText(StoryChoiceButton button, StoryChoiceDataSO data)
    {
        string requirementString = "";

        foreach(ChoiceRequirment cr in data.choiceRequirements)
        {
            // Add the choice requirement to the final string
            requirementString += ConvertChoiceRequirementToString(cr);

            // Seperate each requirement with a comma, if it is not the last req in list
            if(cr != data.choiceRequirements.Last())
            {
                requirementString += ", ";
            }
        }

        // Apply generated string to description text component
        button.requirementsText.text = requirementString;
    }
    public string ConvertChoiceRequirementToString(ChoiceRequirment choiceRequirement)
    {
        string stringReturned = "";

        if(choiceRequirement.requirementType == ChoiceRequirment.RequirementType.HasBackground)
        {
            stringReturned = choiceRequirement.backgroundRequirement.ToString() + " party member";
        }
        else if (choiceRequirement.requirementType == ChoiceRequirment.RequirementType.HasRace)
        {
            stringReturned = choiceRequirement.raceRequirement.ToString() + " party member";
        }
        else if (choiceRequirement.requirementType == ChoiceRequirment.RequirementType.HasEnoughGold)
        {
            stringReturned = choiceRequirement.goldAmountRequired.ToString() +"g";
        }

        return stringReturned;
    }
    #endregion
}
