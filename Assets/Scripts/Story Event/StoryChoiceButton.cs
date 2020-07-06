using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryChoiceButton : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Properties")]
    public StoryChoiceDataSO myChoiceData;
    public bool locked;

    [Header("Parent Component References")]
    public GameObject onFailureTextRowParent;

    [Header("Text Component References")]
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI requirementsText;
    public TextMeshProUGUI successConsequenceText;
    public TextMeshProUGUI failureConsequenceText;
    public TextMeshProUGUI successChanceText;

    [Header("Image Component References")]
    public Image panelBgImage;

    [Header("Colour Properties")]
    public Color normalColour;
    public Color highlightColour;
    public Color disabledColour;
    #endregion

    // View Logic
    #region
    public void SetPanelColour(Color newColor)
    {
        panelBgImage.color = newColor;
    }
    #endregion

    // Mouse + Input Events
    #region
    public void OnMouseEnter()
    {
        if(!locked)
        SetPanelColour(highlightColour);
    }
    public void OnMouseExit()
    {
        if(!locked)
        SetPanelColour(normalColour);
    }
    public void OnMouseDown()
    {
        Debug.Log("StoryChoiceButton.OnMouseDown() called...");

        StoryEventController.Instance.OnChoiceButtonClicked(this);
    }
    #endregion
}
