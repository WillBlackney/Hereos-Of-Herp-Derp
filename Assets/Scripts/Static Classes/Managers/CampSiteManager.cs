using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CampSiteManager : MonoBehaviour
{
    // Properties / Variables / References
    #region
    [Header("Component References")]
    public GameObject visualParent;
    public GameObject restButton;
    public GameObject levelUpButton;
    public GameObject continueButton;

    public TextMeshProUGUI restButtonDescriptionText;
    public TextMeshProUGUI trainButtonDescriptionText;

    [Header("Properties")]
    public bool playerHasMadeChoice;
    public bool awaitingLevelUpChoice;
    public bool awaitingHealChoice;
    #endregion

    // Initialization + Singleton Pattern
    #region
    public static CampSiteManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // On Button Click Events
    #region
    public void OnRestButtonClicked()
    {
        Debug.Log("OnRestButtonClicked() called...");        
        UIManager.Instance.EnableCharacterRosterView();
        UIManager.Instance.DisableInventoryView();
        UIManager.Instance.DisableWorldMapView();
       
        awaitingHealChoice = true;
    }        
    public void OnLevelUpButtonClicked()
    {
        Debug.Log("OnLevelUpButtonClicked() called...");
        UIManager.Instance.EnableCharacterRosterView();
        UIManager.Instance.DisableInventoryView();
        UIManager.Instance.DisableWorldMapView();
        
        awaitingLevelUpChoice = true;
    }
    public void OnContinueButtonClicked()
    {
        DisableCampSiteView();
        UIManager.Instance.OnWorldMapButtonClicked();
        UIManager.Instance.EnableWorldMapView();
        // re enable world map + get next viable enocunter hexagon tiles
        WorldMap.Instance.SetWorldMapReadyState();
    }
    #endregion

    // Visibility / View logic
    #region
    public void EnableCampSiteView()
    {
        visualParent.SetActive(true);
        DisableContinueButtonView();
    }
    public void DisableCampSiteView()
    {
        visualParent.SetActive(false);
    }
    public void EnableRestButtonView()
    {
        restButton.SetActive(true);
    }
    public void DisableRestButtonView()
    {
        restButton.SetActive(false);
    }
    public void EnableLevelUpButtonView()
    {
        levelUpButton.SetActive(true);
    }
    public void DisableLevelUpButtonView()
    {
        levelUpButton.SetActive(false);
    }
    public void EnableContinueButtonView()
    {
        continueButton.SetActive(true);
    }
    public void DisableContinueButtonView()
    {
        continueButton.SetActive(false);
    }
    public void DisableAllButtonViews()
    {
        DisableRestButtonView();
        DisableLevelUpButtonView();
    }
    public void EnableAllButtonViews()
    {
        EnableRestButtonView();
        EnableLevelUpButtonView();
    }
    #endregion

    // Misc
    #region
    public void ResetEventProperties()
    {
        playerHasMadeChoice = false;
        awaitingLevelUpChoice = false;
        EnableAllButtonViews();
    }
    #endregion





}
