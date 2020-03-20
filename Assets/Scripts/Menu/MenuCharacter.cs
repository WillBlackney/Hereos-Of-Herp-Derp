using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Spriter2UnityDX;

public class MenuCharacter : MonoBehaviour
{
    // Variables + Properties
    #region
    [Header("Object References")]
    public GameObject infoPanelParent;
    public UniversalCharacterModel myModel;
    public EntityRenderer myER;


    [Header("Text References")]
    public TextMeshProUGUI presetNameText;
    public TextMeshProUGUI presetDescriptionText;
    public TextMeshProUGUI attributeOneText;
    public TextMeshProUGUI attributeTwoText;
    public TextMeshProUGUI attributeThreeText;


    [Header("Properties")]
    public string myPresetName;
    public MenuAbilityTab tabOne;
    public MenuAbilityTab tabTwo;
    public MenuAbilityTab tabThree;
    public MenuAbilityTab tabFour;

    #endregion

    // Initialization + Set up
    #region
    private void Start()
    {
        // set default view state as random character
        BuildMyViewsFromPresetString("Random");
        myModel.SetIdleAnim();
    }
    public void BuildMyViewsFromPresetString(string presetName)
    {
        // modify text meshs
        SetPresetName(presetName);

        // modify character model
        CharacterModelController.BuildModelFromPresetString(myModel, presetName);

        // modify abilities/passives on panel
        MainMenuManager.Instance.BuildCharacterAbilityTabs(this);

        // modify attribute texts
        MainMenuManager.Instance.BuildAttributeTexts(this);

        // Set Description text
        MainMenuManager.Instance.BuildDescriptionText(this);
    }
    #endregion

    // Mouse + Button Click Events
    #region
    public void OnMouseEnter()
    {
        Debug.Log("MenuCharacter.OnMouseEnter() called...");

        if(myPresetName != "Random")
        {
            myER.Color = MainMenuManager.Instance.highlightColour;
        }        

    }
    public void OnMouseDown()
    {
        Debug.Log("MenuCharacter.OnMouseDown() called...");
        MainMenuManager.Instance.SetSelectedCharacter(this);
    }
    public void OnMouseExit()
    {
        Debug.Log("MenuCharacter.OnMouseExit() called...");

        if (myPresetName != "Random")
        {
            myER.Color = MainMenuManager.Instance.normalColour;
        }            
    }
    public void OnPreviousPresetButtonClicked()
    {
        BuildMyViewsFromPresetString(MainMenuManager.Instance.GetPreviousPresetString(myPresetName));
    }
    public void OnNextPresetButtonClicked()
    {
        BuildMyViewsFromPresetString(MainMenuManager.Instance.GetNextPresetString(myPresetName));
    }
    #endregion

    // View Logic
    #region
    public void EnableInfoPanel()
    {
        infoPanelParent.SetActive(true);
    }
    public void DisableInfoPanel()
    {
        infoPanelParent.SetActive(false);
    }
    #endregion

    // Misc Logic
    #region
    public void SetPresetName(string newName)
    {
        myPresetName = newName;
        presetNameText.text = newName;
    }
    #endregion
}
