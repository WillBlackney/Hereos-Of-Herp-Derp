using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMakerController : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Parent References")]
    public GameObject mainVisualParent;
    public GameObject panelMasterParent;
    public GameObject originPanelParent;
    public GameObject appearancePanelParent;
    public GameObject presetPanelParent;

    [Header("UCM References")]
    public UniversalCharacterModel characterModel;
    #endregion

    // Singleton Pattern
    #region
    public static CharacterMakerController Instance;
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

    // On Button Click Events
    #region

    // Main Buttons
    public void OnCharacterMakerMainMenuButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnCharacterMakerButtonClicked() called...");
        SetMainWindowViewState(true);
        SetCharacterModelDefaultStartingState();
    }
    public void OnOriginButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnOriginButtonClicked() called...");
        DisabelAllPanelViews();
        SetOriginPanelViewState(true);
    }
    public void OnAppearanceButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnAppearanceButtonClicked() called...");
        DisabelAllPanelViews();
        SetAppearancePanelViewState(true);
    }
    public void OnPresetButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnPresetButtonClicked() called...");
        DisabelAllPanelViews();
        SetPresetPanelViewState(true);
    }

    // Appearance Page
    public void OnNextHeadButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnNextHeadButtonClicked() called...");

        if(characterModel.myModelRace == UniversalCharacterModel.ModelRace.Human)
        {
            CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.humanHeads));
        }
    }
    public void OnPreviousHeadButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnPreviousHeadButtonClicked() called...");

        if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Human)
        {
            CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetPreviousElementInList(characterModel.humanHeads));
        }
    }
    public void OnNextFaceButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnNextFaceButtonClicked() called...");

        if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Human)
        {
            CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.humanFaces));
        }
    }
    public void OnPreviousFaceButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnPreviousFaceButtonClicked() called...");

        if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Human)
        {
            CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetPreviousElementInList(characterModel.humanFaces));
        }
    }
    #endregion

    // Enable + Disable Views
    #region
    public void SetMainWindowViewState(bool onOrOff)
    {
        Debug.Log("CharacterMakerController.SetMainWindowViewState() called, set state: " + onOrOff.ToString());
        mainVisualParent.SetActive(onOrOff);
    }
    public void SetPanelMasterViewState(bool onOrOff)
    {
        Debug.Log("CharacterMakerController.SetPanelMasterViewState() called, set state: " + onOrOff.ToString());
        panelMasterParent.SetActive(onOrOff);
    }
    public void SetOriginPanelViewState(bool onOrOff)
    {
        Debug.Log("CharacterMakerController.SetOriginPanelViewState() called, set state: " + onOrOff.ToString());
        originPanelParent.SetActive(onOrOff);
    }
    public void SetAppearancePanelViewState(bool onOrOff)
    {
        Debug.Log("CharacterMakerController.SetAppearancePanelViewState() called, set state: " + onOrOff.ToString());
        appearancePanelParent.SetActive(onOrOff);
    }
    public void SetPresetPanelViewState(bool onOrOff)
    {
        Debug.Log("CharacterMakerController.SetPresetPanelViewState() called, set state: " + onOrOff.ToString());
        presetPanelParent.SetActive(onOrOff);
    }
    public void DisabelAllPanelViews()
    {
        Debug.Log("CharacterMakerController.DisabelAllPanelViews() called...");
        SetOriginPanelViewState(false);
        SetAppearancePanelViewState(false);
        SetPresetPanelViewState(false);
    }
    #endregion

    // Character Model Logic
    #region
    private void SetCharacterModelDefaultStartingState()
    {
        Debug.Log("CharacterMakerController.SetCharacterModelDefaultStartingState() called...");
        SetCharacterModelIdleAnim();
        SetCharacterModelDefaultView();
    }
    private void SetCharacterModelIdleAnim()
    {
        Debug.Log("CharacterMakerController.SetCharacterModelIdleAnim() called...");
        characterModel.SetIdleAnim();
    }
    private void SetCharacterModelDefaultView()
    {
        Debug.Log("CharacterMakerController.SetCharacterModelDefaultView() called...");
        CharacterModelController.SetBaseHumanView(characterModel);
    }
    #endregion
}
