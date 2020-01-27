using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Spriter2UnityDX;

public class MenuCharacter : MonoBehaviour
{
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



    // Mouse + Button Click Events
    public void OnMouseEnter()
    {
        Debug.Log("MenuCharacter.OnMouseEnter() called...");
        myER.Color = MainMenuManager.Instance.highlightColour;

    }
    public void OnMouseDown()
    {
        Debug.Log("MenuCharacter.OnMouseDown() called...");
        MainMenuManager.Instance.SetSelectedCharacter(this);
    }
    public void OnMouseExit()
    {
        Debug.Log("MenuCharacter.OnMouseExit() called...");
        myER.Color = MainMenuManager.Instance.normalColour;
    }
    public void OnPreviousPresetButtonClicked()
    {
        BuildMyViewsFromPresetString(MainMenuManager.Instance.GetPreviousPresetString(myPresetName));
    }
    public void OnNextPresetButtonClicked()
    {
        BuildMyViewsFromPresetString(MainMenuManager.Instance.GetNextPresetString(myPresetName));
    }


    public void EnableInfoPanel()
    {
        infoPanelParent.SetActive(true);
    }
    public void DisableInfoPanel()
    {
        infoPanelParent.SetActive(false);
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
    public void SetPresetName(string newName)
    {
        myPresetName = newName;
        presetNameText.text = newName;
    }
}
