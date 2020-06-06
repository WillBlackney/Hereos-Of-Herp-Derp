using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [Header("Origin Tab References")]
    public TextMeshProUGUI characterRaceText;
    public TextMeshProUGUI characterBackgroundText;
    public TextMeshProUGUI characterRacialBackgroundText;

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
    public void OnSaveCharacterButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnSaveCharacterButtonClicked() called...");
        StartCharacterSaveProcess();
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
        else if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Orc)
        {
            CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.orcHeads));
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
        else if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Orc)
        {
            CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetPreviousElementInList(characterModel.orcHeads));
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
        else if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Orc)
        {
            CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.orcFaces));
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
        else if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Orc)
        {
            CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetPreviousElementInList(characterModel.orcFaces));
        }
    }
    public void OnNextHeadWearButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnNextHeadWearButtonClicked() called...");
        CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.allHeadWear));
    }
    public void OnPreviousHeadWearButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnPreviousHeadWearButtonClicked() called...");
        CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetPreviousElementInList(characterModel.allHeadWear));
    }
    public void OnNextChestWearButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnNextChestWearButtonClicked() called...");
        CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.allChestWear));
        CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.allRightArmWear));
        CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.allLeftArmWear));
        CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.allRightHandWear));
        CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.allLeftHandWear));
    }
    public void OnPreviousChestWearButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnPreviousHeadWearButtonClicked() called...");
        CharacterModelController.EnableAndSetElementOnModel(characterModel,
               CharacterModelController.GetNextElementInList(characterModel.allChestWear));
        CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.allRightArmWear));
        CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.allLeftArmWear));
        CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.allRightHandWear));
        CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.allLeftHandWear));
    }
    public void OnNextLegWearButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnNextLegWearButtonClicked() called...");
        CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.allLeftLegWear));
        CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.allRightLegWear));
    }
    public void OnPreviousLegWearButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnPreviousLegWearButtonClicked() called...");
        CharacterModelController.EnableAndSetElementOnModel(characterModel,
               CharacterModelController.GetNextElementInList(characterModel.allLeftLegWear));
        CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.allRightLegWear));
    }

    // Origin Page
    public void OnNextRaceButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnNextRaceButtonClicked() called...");

        if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Human)
        {            
            CharacterModelController.SetBaseOrcView(characterModel);
            characterRaceText.text = "Orc";
        }
        else if(characterModel.myModelRace == UniversalCharacterModel.ModelRace.Orc)
        {
            CharacterModelController.SetBaseHumanView(characterModel);
            characterRaceText.text = "Human";
        }
    }
    public void OnPreviousRaceButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnPreviousRaceButtonClicked() called...");

        if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Orc)
        {
            CharacterModelController.SetBaseHumanView(characterModel);
            characterRaceText.text = "Human";
        }
        else if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Human)
        {
            CharacterModelController.SetBaseOrcView(characterModel);
            characterRaceText.text = "Orc";
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
        characterRaceText.text = "Human";
    }
    #endregion

    // Conditional Checks + Bools
    #region
    public static bool IsCharacterSaveActionValid()
    {
        return true;
    }
    #endregion

    // Loading + Save Character Preset Data Logic
    #region
    public void StartCharacterSaveProcess()
    {
        Debug.Log("CharacterMakerController.StartCharacterSaveProcess() called...");

        if (IsCharacterSaveActionValid())
        {
            // Save action is valid, start save process
            CharacterPresetData newData = new CharacterPresetData();

            // Set up model data
            BuildPresetFileModelDataFromUcmModelData(newData, characterModel);
        }
    }
    public void BuildPresetFileModelDataFromUcmModelData(CharacterPresetData charData, UniversalCharacterModel model)
    {
        // Get all active model elements
        List<UniversalCharacterModelElement> allActiveModelElements = new List<UniversalCharacterModelElement>
        {
            // Body Parts
            model.activeHead,
            model.activeFace,
            model.activeLeftLeg ,
            model.activeRightLeg ,
            model.activeRightHand,
            model.activeRightArm,
            model.activeLeftHand,
            model.activeLeftArm,
            model.activeChest,

            // Clothing 
            model.activeHeadWear,
            model.activeChestWear,
            model.activeRightLegWear,
            model.activeLeftLegWear,
            model.activeRightArmWear,
            model.activeRightHandWear,
            model.activeLeftArmWear,
            model.activeLeftHandWear,

            // Weapons
            model.activeMainHandWeapon,
            model.activeOffHandWeapon,
        };

        // Add names of each element to preset data list
        foreach(UniversalCharacterModelElement ele in allActiveModelElements)
        {
            charData.activeModelElements.Add(ele.gameObject.name);
        }
    }
    #endregion

}
