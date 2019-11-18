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
    public GameObject triageButton;
    public GameObject trainButton;
    public GameObject continueButton;
    public TextMeshProUGUI actionPointsText;

    public TextMeshProUGUI restButtonDescriptionText;
    public TextMeshProUGUI trainButtonDescriptionText;

    [Header("Properties")]
    public List<CampSiteCharacter> allCharacterSlots;
    public int maxActionPoints;
    public int currentActionPoints;
    public int triagePointCost;
    public int trainPointCost;
    public int feastPointCost;
    public int restPointCost;
    public int prayPointCost;
    public bool playerHasMadeChoice;
    public bool awaitingTrainChoice;
    public bool awaitingTriageChoice;
    public bool awaitingPrayChoice;
    #endregion

    // Initialization + Singleton Pattern
    #region
    public static CampSiteManager Instance;
    private void Awake()
    {
        Instance = this;

        maxActionPoints = 3;
        triagePointCost = 1;
        trainPointCost = 2;
        feastPointCost = 1;
        restPointCost = 2;
        prayPointCost = 3;
    }

    public void SetupCampSiteCharacter(CampSiteCharacter characterSlot, CharacterData characterData)
    {
        characterSlot.InitializeSetup(characterData);
    }
    #endregion

    // On Button Click Events
    #region
    public void OnTriageButtonClicked()
    {
        if (HasEnoughCampSitePoints(triagePointCost))
        {
            Debug.Log("OnRestButtonClicked() called...");
            //UIManager.Instance.EnableCharacterRosterView();
            UIManager.Instance.DisableInventoryView();
            UIManager.Instance.DisableWorldMapView();

            awaitingTriageChoice = true;
            // To do: make triage button and character buttons flash to indicate a choice is being awaited
        }

    }        
    public void OnTrainButtonClicked()
    {
        if (HasEnoughCampSitePoints(trainPointCost))
        {
            Debug.Log("OnLevelUpButtonClicked() called...");
            //UIManager.Instance.EnableCharacterRosterView();
            UIManager.Instance.DisableInventoryView();
            UIManager.Instance.DisableWorldMapView();

            awaitingTrainChoice = true;
        }           
    }
    public void OnPrayButtonClicked()
    {
        if (HasEnoughCampSitePoints(prayPointCost))
        {
            Debug.Log("OnRestButtonClicked() called...");
            //UIManager.Instance.EnableCharacterRosterView();
            UIManager.Instance.DisableInventoryView();
            UIManager.Instance.DisableWorldMapView();

            awaitingPrayChoice = true;
            // To do: make triage button and character buttons flash to indicate a choice is being awaited
        }

    }
    public void OnFeastButtonClicked()
    {
        if (HasEnoughCampSitePoints(feastPointCost))
        {
            PerformFeast();
        }
    }
    public void OnRestButtonClicked()
    {
        if (HasEnoughCampSitePoints(restPointCost))
        {
            PerformRest();
        }

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
        //DisableContinueButtonView();
    }
    public void DisableCampSiteView()
    {
        visualParent.SetActive(false);
    }
    public void EnableRestButtonView()
    {
        triageButton.SetActive(true);
    }
    public void DisableRestButtonView()
    {
        triageButton.SetActive(false);
    }
    public void EnableLevelUpButtonView()
    {
        trainButton.SetActive(true);
    }
    public void DisableLevelUpButtonView()
    {
        trainButton.SetActive(false);
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
        //EnableRestButtonView();
        //EnableLevelUpButtonView();
    }
    #endregion

    // Misc
    #region
    public void ResetEventProperties()
    {
        ModifyCurrentCampSitePoints(maxActionPoints - currentActionPoints);
        EnableAllButtonViews();
    }
    public bool HasEnoughCampSitePoints(int actionCost)
    {
        if(currentActionPoints >= actionCost)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    // Trigger Camp Site Events
    public void ModifyCurrentCampSitePoints(int pointsGainedOrLost)
    {
        currentActionPoints += pointsGainedOrLost;
        actionPointsText.text = currentActionPoints.ToString();
    }
    public void PerformTriage(CampSiteCharacter characterClicked)
    {
        // do nice visual stuff

        // heal 50%
        characterClicked.myCharacterData.ModifyCurrentHealth(characterClicked.myCharacterData.MaxHealth / 2);
        ModifyCurrentCampSitePoints(-triagePointCost);
        awaitingTriageChoice = false;
    }
    public void PerformTrain(CampSiteCharacter characterClicked)
    {
        // do nice visual stuff

        if (ArtifactManager.Instance.HasArtifact("Kettle Bell"))
        {
            characterClicked.myCharacterData.ModifyCurrentLevel(2);
            characterClicked.myCharacterData.ModifyTalentPoints(2);

        }
        else
        {
            characterClicked.myCharacterData.ModifyCurrentLevel(1);
            characterClicked.myCharacterData.ModifyTalentPoints(1);
        }
        ModifyCurrentCampSitePoints(-trainPointCost);
        awaitingTrainChoice = false;
    }
    public void PerformPray(CampSiteCharacter characterClicked)
    {
        // do nice visual stuff

        // heal 50%
        characterClicked.myCharacterData.ModifyCurrentHealth(characterClicked.myCharacterData.MaxHealth / 2);
        ModifyCurrentCampSitePoints(-prayPointCost);
        awaitingPrayChoice = false;
    }
    public void PerformFeast()
    {
        StateManager.Instance.GainState(StateLibrary.Instance.GetStateByName("Well Fed"));
        ModifyCurrentCampSitePoints(-feastPointCost);        
    }
    public void PerformRest()
    {
        StateManager.Instance.GainState(StateLibrary.Instance.GetStateByName("Well Rested"));
        ModifyCurrentCampSitePoints(-restPointCost);
    }





}
