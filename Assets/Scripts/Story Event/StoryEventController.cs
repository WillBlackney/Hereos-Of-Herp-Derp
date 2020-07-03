using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryEventController : MonoBehaviour
{
    [Header("Propeties")]
    public StoryDataSO currentStoryData;

    [Header("Text Component References")]
    public TextMeshProUGUI storyNameText;
    public TextMeshProUGUI descriptionWindowText;

    [Header("Image Component References")]
    public Image storyImage;

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

        // maybe load choice buttons
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

    }
}
