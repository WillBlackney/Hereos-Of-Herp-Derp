using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CampSiteManager : MonoBehaviour
{
    public static CampSiteManager Instance;

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

    private void Awake()
    {
        Instance = this;
    }

    public void OnRestButtonClicked()
    {
        Debug.Log("OnRestButtonClicked() called...");

        if(playerHasMadeChoice == false)
        {
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                // heal 50% of max hp
                if (ArtifactManager.Instance.HasArtifact("Comfy Pillow"))
                {
                    character.ModifyCurrentHealth(character.MaxHealth / 2);
                }

                // heal 30% of max hp
                else
                {
                    float hpRecovered = character.MaxHealth * 0.3f;
                    character.ModifyCurrentHealth((int) hpRecovered);
                }
                
            }
        }

        DisableAllButtonViews();
        EnableContinueButtonView();
        playerHasMadeChoice = true;
        awaitingLevelUpChoice = false;
    }    

    
    public void OnLevelUpButtonClicked()
    {
        Debug.Log("OnLevelUpButtonClicked() called...");
        UIManager.Instance.EnableCharacterRosterView();
        UIManager.Instance.DisableInventoryView();
        UIManager.Instance.DisableWorldMapView();
        // Disable world map and inventory view just incase

        //CharacterRoster.Instance.CharacterRosterVisualParent.SetActive(true);
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

    public void ResetEventProperties()
    {
        playerHasMadeChoice = false;
        awaitingLevelUpChoice = false;
        EnableAllButtonViews();
    }

    

    

}
