using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class CharacterMakerController : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Parent Component References")]
    public GameObject mainVisualParent;
    public GameObject panelMasterParent;
    public GameObject originPanelParent;
    public GameObject appearancePanelParent;
    public GameObject presetPanelParent;
    public GameObject editAbilitiesParent;
    public GameObject editTalentsParent;
    public List<MenuAbilityTab> editAbilityButtons;

    [Header("Origin Tab Component References")]
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI characterRaceText;
    public TextMeshProUGUI currentBackgroundOneText;
    public TextMeshProUGUI currentBackgroundTwoText;   

    [Header("Preset Tab Component References")]    
    public TextMeshProUGUI currentClassPresetText;
    public TextMeshProUGUI currentWeaponPresetText;
    public List<TextMeshProUGUI> allTalentTextTabs;
    public List<TalentChangeButton> allTalentChangeButtons;
    public List<MenuAbilityTab> allActiveAbilityTabs;
    public int totalSpentTalentPoints;

    [Header("Temp Ability Data Properties")]
    public List<AbilityDataSO> tempAbilities;
    public List<StatusIconDataSO> tempPassives;

    [Header("Model Component References")]
    public UniversalCharacterModel characterModel;

    [Header("Character Data Properties")]
    private List<StatusPairing> allPassiveTabs = new List<StatusPairing>();
    private WeaponPresetDataSO currentWeaponPreset;
    private ClassPresetDataSO currentClassPreset;
    private CharacterData.Background currentBackgroundOne;
    private CharacterData.Background currentBackgroundTwo;
    private List<TalentPairing> allTalentPairings = new List<TalentPairing>();

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
    #region
    public void OnCharacterMakerMainMenuButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnCharacterMakerButtonClicked() called...");
        SetMainWindowViewState(true);
        SetCharacterModelDefaultStartingState();
        SetCharacterBackgroundDefaultState();
        BuildCharacterFromClassPresetData(CharacterPresetLibrary.Instance.allClassPresets[0]);
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
    public void OnLoadCharacterButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnLoadCharacterButtonClicked() called...");
        MainMenuManager.Instance.EnableLoadPresetWindow();
        MainMenuManager.Instance.PopulateLoadPresetWindow();
    }
    public void OnBackToMainMenuButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnCharacterMakerButtonClicked() called...");
        SetMainWindowViewState(false);
    }
    #endregion

    // Appearance Page
    #region
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
        else if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Undead)
        {
            CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.undeadHeads));
        }
        else if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Elf)
        {
            CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.elfHeads));
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
        else if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Undead)
        {
            CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetPreviousElementInList(characterModel.undeadHeads));
        }
        else if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Elf)
        {
            CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetPreviousElementInList(characterModel.elfHeads));
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
        else if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Undead)
        {
            CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.undeadFaces));
        }
        else if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Elf)
        {
            CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetNextElementInList(characterModel.elfFaces));
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
        else if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Undead)
        {
            CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetPreviousElementInList(characterModel.undeadFaces));
        }
        else if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Elf)
        {
            CharacterModelController.EnableAndSetElementOnModel(characterModel,
                CharacterModelController.GetPreviousElementInList(characterModel.elfFaces));
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
    #endregion

    // Origin Page
    #region
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
            CharacterModelController.SetBaseUndeadView(characterModel);
            characterRaceText.text = "Undead";
        }
        else if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Undead)
        {
            CharacterModelController.SetBaseElfView(characterModel);
            characterRaceText.text = "Elf";
        }
        else if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Elf)
        {
            CharacterModelController.SetBaseHumanView(characterModel);
            characterRaceText.text = "Human";
        }

        BuildWeaponTabFromWeaponPresetData(currentWeaponPreset);

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
            CharacterModelController.SetBaseUndeadView(characterModel);
            characterRaceText.text = "Undead";
        }
        else if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Undead)
        {
            CharacterModelController.SetBaseElfView(characterModel);
            characterRaceText.text = "Elf";
        }
        else if (characterModel.myModelRace == UniversalCharacterModel.ModelRace.Elf)
        {
            CharacterModelController.SetBaseOrcView(characterModel);
            characterRaceText.text = "Orc";
        }

        BuildWeaponTabFromWeaponPresetData(currentWeaponPreset);
    }
    public void OnNextBackgroundOneButtonClicked()
    {
        SetCharacterBackgroundOne(GetNextBackground(currentBackgroundOne));
        if(currentBackgroundOne == currentBackgroundTwo)
        {
            SetCharacterBackgroundOne(GetNextBackground(currentBackgroundOne));
        }
    }
    public void OnPreviousBackgroundOneButtonClicked()
    {
        SetCharacterBackgroundOne(GetPreviousBackground(currentBackgroundOne));
        if (currentBackgroundOne == currentBackgroundTwo)
        {
            SetCharacterBackgroundOne(GetPreviousBackground(currentBackgroundOne));
        }
    }
    public void OnNextBackgroundTwoButtonClicked()
    {
        SetCharacterBackgroundTwo(GetNextBackground(currentBackgroundTwo));
        if (currentBackgroundTwo == currentBackgroundOne)
        {
            SetCharacterBackgroundTwo(GetNextBackground(currentBackgroundTwo));
        }
    }
    public void OnPreviousBackgroundTwoButtonClicked()
    {
        SetCharacterBackgroundTwo(GetPreviousBackground(currentBackgroundTwo));
        if (currentBackgroundTwo == currentBackgroundOne)
        {
            SetCharacterBackgroundTwo(GetPreviousBackground(currentBackgroundTwo));
        }
    }
    #endregion

    // Preset Page
    #region
    public void OnNextClassPresetButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnNextClassPresetButtonClicked() called...");
        BuildCharacterFromClassPresetData(GetNextClassPreset());
    }
    public void OnPreviousClassPresetButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnPreviousClassPresetButtonClicked() called...");
        BuildCharacterFromClassPresetData(GetPreviousClassPreset());
    }
    public void OnNextWeaponPresetButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnNextWeaponPresetButtonClicked() called...");
        BuildWeaponTabFromWeaponPresetData(GetNextWeaponPreset());
    }
    public void OnPreviousWeaponPresetButtonClicked()
    {
        Debug.Log("CharacterMakerController.OnPreviousWeaponPresetButtonClicked() called...");
        BuildWeaponTabFromWeaponPresetData(GetPreviousWeaponPreset());
    }
    public void OnEditAbilitiesButtonClicked()
    {
        ClearAllDataFromInactiveAbilityTabs();

        presetPanelParent.SetActive(false);
        editAbilitiesParent.SetActive(true);

        PopulateEditAbilityButtons();
    }
    public void OnEditAbilitiesBackButtonClicked()
    {
        BuildAllAbilityTabsFromTempAbilityList();
        ClearTempAbilityAndPassiveLists();
        presetPanelParent.SetActive(true);
        editAbilitiesParent.SetActive(false);
    }
    public void OnEditTalentsButtonClicked()
    {
        SetPresetPanelViewState(false);
        editTalentsParent.SetActive(true);
        BuildAllTalentChangeButtons();
    }
    public void OnEditTalentsBackButtonClicked()
    {
        SetPresetPanelViewState(true);
        editTalentsParent.SetActive(false);
        BuildTalentPairingsFromTalentChangeButtons();
    }
    public void OnTalentPointPlusButtonClicked(TalentChangeButton tButton)
    {
        Debug.Log("CharacterMakerController.OnTalentPointPlusButtonClicked() called");

        if (totalSpentTalentPoints >= 3 == false)
        {
            totalSpentTalentPoints++;
            tButton.SetTalentTierCount(tButton.talentTierCount + 1);
        }
    }
    public void OnTalentPointMinusButtonClicked(TalentChangeButton tButton)
    {
        Debug.Log("CharacterMakerController.OnTalentPointMinusButtonClicked() called");

        if (tButton.talentTierCount <= 0 == false)
        {
            totalSpentTalentPoints--;
            tButton.SetTalentTierCount(tButton.talentTierCount - 1);
        }
    }
    #endregion
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
    public void DisableAllActiveAbilityTabs()
    {
        foreach(MenuAbilityTab tab in allActiveAbilityTabs)
        {
            tab.gameObject.SetActive(false);
        }
    }
    private void ClearAllDataFromInactiveAbilityTabs()
    {
        Debug.Log("CharacterMakerController.ClearAllDataFromActiveAbilityTabs() called...");

        foreach (MenuAbilityTab tab in allActiveAbilityTabs)
        {
            if (!tab.gameObject.activeSelf)
            {
                tab.myAbilityData = null;
                tab.myPassiveData = null;
            }            
        }        
    }
    private void ClearAllPassiveTabs()
    {
        Debug.Log("CharacterMakerController.ClearAllPassiveTabs() called...");
        allPassiveTabs.Clear();
    }
    public void DisableAllTalentTextTabs()
    {
        foreach (TextMeshProUGUI tab in allTalentTextTabs)
        {
            tab.gameObject.SetActive(false);
        }
    }
    #endregion

    // Set Default + Start State Logic
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
        CharacterModelController.CompletelyDisableAllViews(characterModel);
        CharacterModelController.SetBaseHumanView(characterModel);
        characterRaceText.text = "Human";
    }
    private void SetCharacterBackgroundDefaultState()
    {
        SetCharacterBackgroundOne(CharacterData.Background.Unknown);
        SetCharacterBackgroundTwo(CharacterData.Background.Unknown);
    }
    #endregion

    // Conditional Checks + Bools
    #region
    public bool IsCharacterSaveActionValid()
    {
        return IsCharacterNameValid();
    }
    public bool IsCharacterNameValid()
    {
        string charName = characterNameText.text;
        Debug.Log("CharacterMakerController.IsCharacterNameValid() called, checking validity of name: " + charName);
        
        bool passedMaxLengthCheck = false;
        bool passedMinLengthCheck = false;
        
        if(charName.Length >= 2)
        {
            passedMinLengthCheck = true;
        }
        else
        {
            Debug.Log("Returning false: '" + charName + "' is less then 2 characters");
            return false;
        }
        if(charName.Length <= 25)
        {
            passedMaxLengthCheck = true;            
        }
        else
        {
            Debug.Log("Returning false: '" + charName + "' is more then 25 characters");
            return false;
        }

        Debug.Log("CharacterMakerController.IsCharacterNameValid() returning true, " + charName +
            " is a valid save name");
        return true;
    }
    #endregion

    // Save Character Preset Data Logic
    #region
    public void StartCharacterSaveProcess()
    {
        Debug.Log("CharacterMakerController.StartCharacterSaveProcess() called...");

        if (IsCharacterSaveActionValid())
        {
            // Save action is valid, start save process
            CharacterPresetData newData = new CharacterPresetData();

            // Set Origin Data
            SaveOriginDataToCharacterPresetFile(newData);            

            // Set up model data
            SaveModelDataToCharacterPresetFile(newData, characterModel);

            // Set up combat data
            SaveCombatDataToCharacterPresetFile(newData);

            // Set up weapon data
            SaveWeaponDataToCharacterPresetFile(newData);

            // Print info (for testing, remove later)
            CharacterPresetLibrary.Instance.PrintPresetData(newData);

            // Add new data to persistency
            CharacterPresetLibrary.Instance.AddCharacterPresetToPlayerMadeList(newData);
        }
    }
    private void SaveModelDataToCharacterPresetFile(CharacterPresetData charData, UniversalCharacterModel model)
    {
        Debug.Log("CharacterMakerController.SaveModelDataToCharacterPresetFile() called...");

        // Get all active model elements
        List<UniversalCharacterModelElement> allActiveModelElements = new List<UniversalCharacterModelElement>
        {
            // Body Parts
            model.activeHead,
            model.activeFace,
            model.activeLeftLeg ,
            model.activeRightLeg,
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
            if(ele != null)
            {
                charData.activeModelElements.Add(new ModelElementData(ele));
            }
            else if(ele == null)
            {
                Debug.Log("CharacterMakerController.SaveModelDataToCharacterPresetFile() detected a " +
                    "UCM element with a null gameObject parent, skipping...");
            }            
        }
    }
    private void SaveCombatDataToCharacterPresetFile(CharacterPresetData charData)
    {
        Debug.Log("CharacterMakerController.SaveCombatDataToCharacterPresetFile() called...");

        ClearAllPassiveTabs();

        // Add abilities
        foreach (MenuAbilityTab ability in allActiveAbilityTabs)
        {
            if (ability.myAbilityData != null)
            {
                charData.knownAbilities.Add(ability.myAbilityData);
            }
            else if (ability.myPassiveData != null)
            {
                allPassiveTabs.Add(new StatusPairing(ability.myPassiveData, ability.passiveStacks));
            }
        }

        // Add passives
        foreach (StatusPairing passive in allPassiveTabs)
        {
            charData.knownPassives.Add(passive);
        }

        // Add talent data
        foreach(TalentPairing talentPairing in allTalentPairings)
        {
            charData.knownTalents.Add(talentPairing);
        }
    }
    private void SaveOriginDataToCharacterPresetFile(CharacterPresetData charData)
    {
        Debug.Log("CharacterMakerController.SaveOriginDataToCharacterPresetFile() called...");

        // Set name
        charData.characterName = characterNameText.text;

        // Set backgrounds
        charData.backgrounds.Add(currentBackgroundOne);
        charData.backgrounds.Add(currentBackgroundTwo);
    }
    private void SaveWeaponDataToCharacterPresetFile(CharacterPresetData charData)
    {
        Debug.Log("CharacterMakerController.SaveWeaponDataToCharacterPresetFile() called...");

        // Save weapon data set
        charData.weaponSetData = currentWeaponPreset;

        // Main hand weapon
        if (currentWeaponPreset.mainHandWeapon)
        {
            charData.mhWeapon = currentWeaponPreset.mainHandWeapon;
        }

        // Off hand weapon
        if (currentWeaponPreset.offHandWeapon)
        {
            charData.ohWeapon = currentWeaponPreset.offHandWeapon;
        }

    }
    #endregion

    // Core Logic
    #region
    public void BuildCharacterFromClassPresetData(ClassPresetDataSO data)
    {
        Debug.Log("CharacterMakerController.BuildCharacterFromClassPresetData() called, building from " + data.classPresetName);
        // Flush old data and views
        DisableAllActiveAbilityTabs();
        DisableAllTalentTextTabs();
        ClearAllTalentPairings();
        ResetAllTalentChangeButtons();

        // Build new data + views
        currentClassPreset = data;
        currentClassPresetText.text = data.classPresetName;
        BuildAllAbilityTabsFromClassPresetData(data);
        BuildAllTalentTextTabsFromClassPresetData(data);
        BuildWeaponTabFromWeaponPresetData(data.weaponPreset);

    }
    private void BuildAllAbilityTabsFromClassPresetData(ClassPresetDataSO data)
    {
        Debug.Log("CharacterMakerController.BuildAbilityTabsFromClassPresetData() called, building from " + data.classPresetName);

        // Build abilities first
        foreach(AbilityDataSO abilityData in data.abilities)
        {
            BuildAbilityTabFromAbilityData(abilityData);
        }

        // Build passives second
        foreach (StatusPairing passiveData in data.passives)
        {
            BuildAbilityTabFromPassiveData(passiveData.statusData, passiveData.statusStacks);
        }
    }
    private void BuildAllAbilityTabsFromTempAbilityList()
    {
        Debug.Log("CharacterMakerController.BuildAllAbilityTabsFromTempAbilityList() called...");

        // Build abilities first
        foreach (AbilityDataSO abilityData in tempAbilities)
        {
            BuildAbilityTabFromAbilityData(abilityData);
        }

        // Build passives second
        foreach (StatusIconDataSO passiveData in tempPassives)
        {
            StatusPairingDataSO sp = StatusIconLibrary.Instance.GetStatusPairingByName(passiveData.statusName);
            BuildAbilityTabFromPassiveData(sp.statusData, sp.stacks);
        }
    }
    private void BuildAllAbilityTabsFromCharacterPresetData(CharacterPresetData data)
    {
        Debug.Log("CharacterMakerController.BuildAllAbilityTabsFromCharacterPresetData() called, building from " + data.characterName);

        // Build abilities first
        foreach (AbilityDataSO abilityData in data.knownAbilities)
        {
            BuildAbilityTabFromAbilityData(abilityData);
        }

        // Build passives second
        foreach (StatusPairing passiveData in data.knownPassives)
        {
            BuildAbilityTabFromPassiveData(passiveData.statusData, passiveData.statusStacks);
        }
    }
    private void BuildTalentTextTabFromTalentPairing(TalentPairing talentPair)
    {
        Debug.Log("CharacterMakerController.BuildTalentTextTabFromTalentPairing() called, building for " +
            talentPair.talentType.ToString() + " (" + talentPair.talentStacks.ToString() + ")");

        // Get next available text tab
        TextMeshProUGUI textTab = GetNextAvailableTalentTextTab();

        // Enable view
        textTab.gameObject.SetActive(true);

        // Set text
        textTab.text = talentPair.talentType.ToString() + " +" + talentPair.talentStacks.ToString();        
    }
    private void BuildWeaponTabFromWeaponPresetData(WeaponPresetDataSO data)
    {
        Debug.Log("CharacterMakerController.BuildWeaponTabFromWeaponPresetData() called...");

        // Cancel build if character preset does not have a weapon preset
        if (!data)
        {
            Debug.Log("CharacterMakerController.BuildWeaponTabFromWeaponPresetData() given null data, cancelling weapon build...");
            return;
        }

        // Set text panel
        currentWeaponPresetText.text = data.weaponPresetName;

        // cache data
        currentWeaponPreset = data;

        // disable model weapon views and cache refs
        if (characterModel.activeMainHandWeapon)
        {
            characterModel.activeMainHandWeapon.gameObject.SetActive(false);
            characterModel.activeMainHandWeapon = null;
        }
        if (characterModel.activeOffHandWeapon)
        {
            characterModel.activeOffHandWeapon.gameObject.SetActive(false);
            characterModel.activeOffHandWeapon = null;
        }

        // set MH weapon model view
        foreach (UniversalCharacterModelElement ucme in characterModel.allMainHandWeapons)
        {
            if (ucme.weaponsWithMyView.Contains(data.mainHandWeapon))
            {
                CharacterModelController.EnableAndSetElementOnModel(characterModel, ucme);
                break;
            }
        }
         
        // Set OH weapon model view
        if(data.offHandWeapon != null)
        {
            foreach (UniversalCharacterModelElement ucme in characterModel.allOffHandWeapons)
            {
                if (ucme.weaponsWithMyView.Contains(data.offHandWeapon))
                {
                    CharacterModelController.EnableAndSetElementOnModel(characterModel, ucme);
                    break;
                }
            }
        }

    }    
    private void BuildAbilityTabFromAbilityData(AbilityDataSO data)
    {
        Debug.Log("CharacterMakerController.BuildAbilityTabFromAbilityData() called, building from: " + data.abilityName);

        // Get slot
        MenuAbilityTab nextSlot = GetNextAvailableMenuTabSlot();

        // Enable view
        nextSlot.gameObject.SetActive(true);

        // Build views and data
        nextSlot.SetUpAbilityTabAsAbility(data);
    }
    private void BuildAbilityTabFromPassiveData(StatusIconDataSO data, int stacks)
    {
        Debug.Log("CharacterMakerController.BuildAbilityTabFromAbilityData() called, building from: " + data.statusName);

        // Get slot
        MenuAbilityTab nextSlot = GetNextAvailableMenuTabSlot();

        // Enable view
        nextSlot.gameObject.SetActive(true);

        // Build views and data
        nextSlot.SetUpAbilityTabAsPassive(data, stacks);
    }
    private void SetCharacterBackgroundOne(CharacterData.Background background)
    {
        // cache BG ref
        currentBackgroundOne = background;

        // set text
        currentBackgroundOneText.text = background.ToString();
    }
    private void SetCharacterBackgroundTwo(CharacterData.Background background)
    {
        // cache BG ref
        currentBackgroundTwo = background;

        // set text
        currentBackgroundTwoText.text = background.ToString();
    }    
    private void BuildAllTalentTextTabsFromClassPresetData(ClassPresetDataSO data)
    {
        Debug.Log("CharacterMakerController.BuildAbilityTabsFromClassPresetData() called, building from " + data.classPresetName);

        foreach(TalentPairing talentPair in data.talents)
        {
            AddTalentPairingToPersistentList(new TalentPairing(talentPair.talentType, talentPair.talentStacks));
            BuildTalentTextTabFromTalentPairing(talentPair);
        }

    }
    private void BuildAllTalentTextTabsFromCharacterPresetData(CharacterPresetData data)
    {
        Debug.Log("CharacterMakerController.BuildAllTalentTextTabsFromCharacterPresetData() called, building from " + data.characterName);

        foreach (TalentPairing talentPair in data.knownTalents)
        {
            AddTalentPairingToPersistentList(new TalentPairing(talentPair.talentType, talentPair.talentStacks));
            BuildTalentTextTabFromTalentPairing(talentPair);
        }

    }
    private void BuildAllTalentChangeButtons()
    {
        Debug.Log("CharacterMakerController.BuildAllTalentChangeButtons() called");

        totalSpentTalentPoints = 0;

        foreach(TalentPairing tPairing in allTalentPairings)
        {
            foreach(TalentChangeButton tButton in allTalentChangeButtons)
            {
                if(tPairing.talentType == tButton.talentSchool)
                {
                    tButton.SetUpFromTalentPairing(tPairing);
                    totalSpentTalentPoints += tPairing.talentStacks;
                    break;
                }
            }
        }
    }     
    private void BuildTalentPairingsFromTalentChangeButtons()
    {
        // Remove previous talent pairing data, disable text views
        DisableAllTalentTextTabs();
        ClearAllTalentPairings();

        // Rebuild talent parents
        foreach (TalentChangeButton tcButton in allTalentChangeButtons)
        {
            if(tcButton.talentTierCount > 0)
            {
                TalentPairing tcp = new TalentPairing(tcButton.talentSchool, tcButton.talentTierCount);
                AddTalentPairingToPersistentList(tcp);
                BuildTalentTextTabFromTalentPairing(tcp);
            }            
        }

        // Check if any abilities/passive selections no longer meet their talent requirements
        UpdateAbilitiesAfterTalentChange();
    }
    private void UpdateAbilitiesAfterTalentChange()
    {
        Debug.Log("CharacterMakerController.UpdateAbilitiesAfterTalentChange() called...");

        foreach(MenuAbilityTab tab in allActiveAbilityTabs)
        {
            bool removeTab = true;

            foreach(TalentPairing tp in allTalentPairings)
            {
                // Abilities
                if (tab.myAbilityData != null &&
                    tab.myAbilityData.abilitySchool == tp.talentType && 
                    tab.myAbilityData.tier <= tp.talentStacks
                    )
                {
                    removeTab = false;
                }

                // Passives
                else if (tab.myPassiveData != null)
                {
                    StatusPairingDataSO status = StatusIconLibrary.Instance.GetStatusPairingByName(tab.myPassiveData.statusName);
                    if(status.abilitySchool == tp.talentType &&
                    status.tier <= tp.talentStacks)
                    {
                        removeTab = false;
                    }
                    
                }
            }

            // Talent + tier requirements of passive/ability no longer met, remove it
            if (removeTab)
            {
                tab.gameObject.SetActive(false);
                tab.myPassiveData = null;
                tab.myAbilityData = null;
            }
        }
    }
    private void ClearAllTalentPairings()
    {
        allTalentPairings.Clear();
    }
    private void ResetAllTalentChangeButtons()
    {
        Debug.Log("CharacterMakerController.ResetAllTalentChangeButton() called...");

        foreach (TalentChangeButton tcb in allTalentChangeButtons)
        {
            tcb.SetTalentTierCount(0);
        }
    }
    private void AddTalentPairingToPersistentList(TalentPairing talentPairing)
    {
        allTalentPairings.Add(talentPairing);
    }

    // Edit Ability Button Logic
    #region
    public void PopulateEditAbilityButtons()
    {
        Debug.Log("CharacterMakerController.PopulateEditAbilityButtons() called...");

        DisableAllEditAbilityButtons();
        DisableAllActiveAbilityTabs();
        ClearTempAbilityAndPassiveLists();
        BuildTempAbilityAndPassiveLists();

        List<AbilityDataSO> validAbilities = new List<AbilityDataSO>();
        List<StatusPairingDataSO> validPassives = new List<StatusPairingDataSO>();

        foreach(TalentPairing tp in allTalentPairings)
        {
            validAbilities.AddRange(AbilityLibrary.Instance.GetAllAbilitiesFromTalentSchool(tp.talentType, tp.talentStacks));
            validPassives.AddRange(StatusIconLibrary.Instance.GetAllStatusPairingsFromTalentSchool(tp.talentType, tp.talentStacks));
        }

        // set up abilities first
        foreach(AbilityDataSO ability in validAbilities)
        {
            BuildEditAbilityTabFromAbilityData(ability, GetNextAvailableEditAbilityButton());
        }

        // set up passives second
        foreach (StatusPairingDataSO passive in validPassives)
        {
            BuildEditAbilityTabFromStatusPairingData(passive, GetNextAvailableEditAbilityButton());
        }

    }
    private void BuildTempAbilityAndPassiveLists()
    {
        Debug.Log("CharacterMakerController.BuildTempAbilityAndPassiveLists() called...");

        foreach(MenuAbilityTab tab in allActiveAbilityTabs)
        {
            if (tab.myAbilityData != null)
            {
                tempAbilities.Add(tab.myAbilityData);
                Debug.Log("BuildTempAbilityAndPassiveLists() adding ability " + tab.myAbilityData.abilityName + " to temp ability list...");
            }
            else if (tab.myPassiveData != null)
            {
                tempPassives.Add(tab.myPassiveData);
                Debug.Log("BuildTempAbilityAndPassiveLists() adding ability " + tab.myPassiveData.statusName + " to temp passive list...");
            }
        }
    }
    public void ClearTempAbilityAndPassiveLists()
    {
        tempAbilities.Clear();
        tempPassives.Clear();
    }
    public MenuAbilityTab GetNextAvailableEditAbilityButton()
    {
        Debug.Log("CharacterMakerController.GetNextAvailableEditAbilityButton() called...");

        MenuAbilityTab tabReturned = null;
        foreach(MenuAbilityTab tab in editAbilityButtons)
        {
            if(tab.gameObject.activeSelf == false)
            {
                tabReturned = tab;
                break;
            }
        }

        return tabReturned;
    }
    public void ClearAllEditAbilityButtons()
    {
        Debug.Log("CharacterMakerController.ClearAllEditAbilityButtons() called...");
    }
    public void DisableAllEditAbilityButtons()
    {
        Debug.Log("CharacterMakerController.DisableAllEditAbilityButtons() called...");

        foreach(MenuAbilityTab eButton in editAbilityButtons)
        {
            eButton.gameObject.SetActive(false);
            eButton.myPassiveData = null;
            eButton.myAbilityData = null;
            eButton.DisableGlowOutline();
        }
    }
    private void BuildEditAbilityTabFromAbilityData(AbilityDataSO data, MenuAbilityTab tab)
    {
        tab.gameObject.SetActive(true);

        tab.SetUpAbilityTabAsAbility(data);

        if (HasCharacterAlreadyUnlockedAbilityOrPassiveFromMenuAbilityTab(tab))
        {
            tab.EnableGlowOutline();
        }
    }
    private void BuildEditAbilityTabFromStatusPairingData(StatusPairingDataSO data, MenuAbilityTab tab)
    {
        tab.gameObject.SetActive(true);

        tab.SetUpAbilityTabAsPassive(data.statusData, data.stacks);

        if (HasCharacterAlreadyUnlockedAbilityOrPassiveFromMenuAbilityTab(tab))
        {
            tab.EnableGlowOutline();
        }
    }
    public void HandleEditAbilityTabClicked(MenuAbilityTab tab)
    {
        Debug.Log("CharacterMakerController.HandleEditAbilityTabClicked() called...");

        // try learn new ability/passive
        if (!HasCharacterAlreadyUnlockedAbilityOrPassiveFromMenuAbilityTab(tab) &&
            !HasCharacterLearntMaximumAbilitiesAndPassives())
        {
            if(tab.myAbilityData != null)
            {
                tempAbilities.Add(tab.myAbilityData);
            }
            else if(tab.myPassiveData != null)
            {
                tempPassives.Add(tab.myPassiveData);
            }

            tab.EnableGlowOutline();
        }

        // try UNLEARN new ability/passive
        else if (HasCharacterAlreadyUnlockedAbilityOrPassiveFromMenuAbilityTab(tab) ||
            HasCharacterLearntMaximumAbilitiesAndPassives())
        {
            if (tab.myAbilityData != null)
            {
                tempAbilities.Remove(tab.myAbilityData);
            }
            if (tab.myPassiveData != null)
            {
                tempPassives.Remove(tab.myPassiveData);
            }

            tab.DisableGlowOutline();
        }

    }
    public bool HasCharacterAlreadyUnlockedAbilityOrPassiveFromMenuAbilityTab(MenuAbilityTab editTab)
    {
        Debug.Log("CharacterMakerController.HasCharacterAlreadyUnlockedMenuAbilityTab() called...");

        bool boolReturned = false;

        foreach(AbilityDataSO ability in tempAbilities)
        {
            if(ability == editTab.myAbilityData)
            {
                boolReturned = true;
            }
        }

        foreach(StatusIconDataSO passive in tempPassives)
        {
            if(passive == editTab.myPassiveData)
            {
                boolReturned = true;
            }
        }       

        /*
        // is status already on the active ability panel?
        foreach (MenuAbilityTab ability in allActiveAbilityTabs)
        {
            if (ability.myPassiveData != null &&
                ability.myPassiveData == editTab.myPassiveData)
            {
                boolReturned = true;
            }
            else if (ability.myAbilityData != null &&
                ability.myAbilityData == editTab.myAbilityData)
            {
                boolReturned = true;
            }
        }
        */

        return boolReturned;
    }
    public bool HasCharacterLearntMaximumAbilitiesAndPassives()
    {
        Debug.Log("CharacterMakerController.HasCharacterLearntMaximumAbilitiesAndPassives() called...");
        bool boolReturned = false;
        int currentLearntAbilityCount = 0;

        currentLearntAbilityCount += tempAbilities.Count;
        currentLearntAbilityCount += tempPassives.Count;

        if(currentLearntAbilityCount >= 4)
        {
            Debug.Log("CharacterMakerController.HasCharacterLearntMaximumAbilitiesAndPassives() detected character already knows 4 abilities");

            boolReturned = true;
        }
        else
        {
            Debug.Log("CharacterMakerController.HasCharacterLearntMaximumAbilitiesAndPassives() detected character knows less than 4 abilities");
            boolReturned = false;
        }

        return boolReturned;
    }
    #endregion
    #endregion

    // Get Next + Previous Data
    #region
    private MenuAbilityTab GetNextAvailableMenuTabSlot()
    {
        Debug.Log("CharacterMakerController.GetNextAvailbleMenuTabSlot() called...");

        MenuAbilityTab tabReturned = null;

        foreach (MenuAbilityTab tab in allActiveAbilityTabs)
        {
            if (tab.gameObject.activeSelf == false)
            {
                tabReturned = tab;
                break;
            }
        }
        if (tabReturned == null)
        {
            Debug.Log("CharacterMakerController.GetNextAvailbleMenuTabSlot() could not find an availble menu tab, returning null...");
        }
        return tabReturned;

    }
    private TextMeshProUGUI GetNextAvailableTalentTextTab()
    {
        Debug.Log("CharacterMakerController.GetNextAvailableTalentTextSlot() called...");

        TextMeshProUGUI tabReturned = null;

        foreach (TextMeshProUGUI tab in allTalentTextTabs)
        {
            if (tab.gameObject.activeSelf == false)
            {
                tabReturned = tab;
                break;
            }
        }
        if (tabReturned == null)
        {
            Debug.Log("CharacterMakerController.TextMeshProUGUI() could not find an availble menu tab, returning null...");
        }
        return tabReturned;

    }
    private ClassPresetDataSO GetNextClassPreset()
    {
        Debug.Log("CharacterMakerController.GetNextClassPreset() called...");

        int currentIndex = CharacterPresetLibrary.Instance.allClassPresets.IndexOf(currentClassPreset);
        int nextIndex = 0;

        if (currentClassPreset == CharacterPresetLibrary.Instance.allClassPresets.Last())
        {
            nextIndex = 0;
        }
        else
        {
            nextIndex = currentIndex + 1;
        }

        return CharacterPresetLibrary.Instance.allClassPresets[nextIndex];
    }
    private ClassPresetDataSO GetPreviousClassPreset()
    {
        Debug.Log("CharacterMakerController.GetPreviousClassPreset() called...");

        int currentIndex = CharacterPresetLibrary.Instance.allClassPresets.IndexOf(currentClassPreset);
        int previousIndex = 0;

        if (currentClassPreset == CharacterPresetLibrary.Instance.allClassPresets.First())
        {
            previousIndex = CharacterPresetLibrary.Instance.allClassPresets.Count - 1;
        }
        else
        {
            previousIndex = currentIndex - 1;
        }

        return CharacterPresetLibrary.Instance.allClassPresets[previousIndex];
    }
    private WeaponPresetDataSO GetNextWeaponPreset()
    {
        Debug.Log("CharacterMakerController.GetNextWeaponPreset() called...");

        int currentIndex = CharacterPresetLibrary.Instance.allWeaponPresets.IndexOf(currentWeaponPreset);
        int nextIndex = 0;

        if (currentWeaponPreset == CharacterPresetLibrary.Instance.allWeaponPresets.Last())
        {
            nextIndex = 0;
        }
        else
        {
            nextIndex = currentIndex + 1;
        }

        return CharacterPresetLibrary.Instance.allWeaponPresets[nextIndex];
    }
    private WeaponPresetDataSO GetPreviousWeaponPreset()
    {
        Debug.Log("CharacterMakerController.GetPreviousWeaponPreset() called...");

        int currentIndex = CharacterPresetLibrary.Instance.allWeaponPresets.IndexOf(currentWeaponPreset);
        int previousIndex = 0;

        if (currentWeaponPreset == CharacterPresetLibrary.Instance.allWeaponPresets.First())
        {
            previousIndex = CharacterPresetLibrary.Instance.allWeaponPresets.Count - 1;
        }
        else
        {
            previousIndex = currentIndex - 1;
        }

        return CharacterPresetLibrary.Instance.allWeaponPresets[previousIndex];
    }
    private CharacterData.Background GetNextBackground(CharacterData.Background currentBackground)
    {
        Debug.Log("CharacterMakerController.GetNextBackground() called...");

        CharacterData.Background bgReturned = CharacterData.Background.None;

        int currentEnumIndex = (int)currentBackground;
        int enumCount = Enum.GetNames(typeof(CharacterData.Background)).Length;
        Debug.Log("CharacterMakerController.GetNextBackground() found " + enumCount.ToString() + " elements in the background enum");
        int nextIndex = 0;

        if (currentEnumIndex == enumCount - 1)
        {
            nextIndex = 0;
        }
        else
        {
            nextIndex = currentEnumIndex + 1;
        }

        bgReturned = (CharacterData.Background)nextIndex;

        return bgReturned;
    }
    private CharacterData.Background GetPreviousBackground(CharacterData.Background currentBackground)
    {
        Debug.Log("CharacterMakerController.GetPreviousBackground() called...");

        CharacterData.Background bgReturned = CharacterData.Background.None;

        int currentEnumIndex = (int)currentBackground;
        int enumCount = Enum.GetNames(typeof(CharacterData.Background)).Length;
        Debug.Log("CharacterMakerController.GetPreviousBackground() found " + enumCount.ToString() + " elements in the background enum");
        int previousIndex = 0;

        if (currentEnumIndex == 0)
        {
            previousIndex = enumCount - 1;
        }
        else
        {
            previousIndex = currentEnumIndex - 1;
        }

        bgReturned = (CharacterData.Background)previousIndex;

        return bgReturned;
    }
    #endregion

    // Load Preset Data Logic
    #region
    public void BuildAllTabsAndViewsFromCharacterPresetData(CharacterPresetData data)
    {
        // Flush old data and views
        DisableAllActiveAbilityTabs();
        DisableAllTalentTextTabs();
        ClearAllTalentPairings();

        // Load name
        characterNameText.text = data.characterName;

        // Load backgrounds
        SetCharacterBackgroundOne(data.backgrounds[0]);
        SetCharacterBackgroundTwo(data.backgrounds[1]);

        // Load Model Data
        CharacterModelController.BuildModelFromCharacterPresetData(characterModel, data);

        // Build ability + passive tabs
        BuildAllAbilityTabsFromCharacterPresetData(data);

        // Build Talent views
        BuildAllTalentTextTabsFromCharacterPresetData(data);

        // Load Weapon data
        BuildWeaponTabFromWeaponPresetData(data.weaponSetData);        
    }
    #endregion

}
