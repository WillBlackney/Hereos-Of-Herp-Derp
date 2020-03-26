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
    public GameObject continueButton;
    public TextMeshProUGUI actionPointsText;

    public TextMeshProUGUI restButtonDescriptionText;
    public TextMeshProUGUI trainButtonDescriptionText;

    [Header("Button Script References")]
    public CampSiteButton triageButton;
    public CampSiteButton trainButton;
    public CampSiteButton prayButton;
    public CampSiteButton batheButton;

    [Header("Properties")]
    public List<CampSiteCharacter> allCharacterSlots;
    public int maxActionPoints;
    public int currentActionPoints;
    public int triagePointCost;
    public int trainPointCost;
    public int feastPointCost;
    public int restPointCost;
    public int prayPointCost;
    public int bathePointCost;
    public bool playerHasMadeChoice;
    public bool awaitingTrainChoice;
    public bool awaitingTriageChoice;
    public bool awaitingPrayChoice;
    public bool awaitingBatheChoice;
    #endregion

    // Initialization + Singleton Pattern
    #region
    public static CampSiteManager Instance;
    private void Awake()
    {
        Instance = this;

        maxActionPoints = 3;

        triagePointCost = 1;
        trainPointCost = 1;
        feastPointCost = 1;
        restPointCost = 2;
        prayPointCost = 2;
        bathePointCost = 2;
    }
    public void SetupCampSiteCharacter(CampSiteCharacter characterSlot, CharacterData characterData)
    {
        characterSlot.InitializeSetup(characterData);
    }
    #endregion

    // On Button Click Events
    #region
    public void OnCampSiteButtonClicked(string buttonName)
    {
        if (buttonName == "Continue")
        {
            OnContinueButtonClicked();
        }
        else if (AlreadyAwaitingAnOrder())
        {
            return;
        }
        else if (buttonName == "Triage")
        {
            OnTriageButtonClicked();
        }
        else if (buttonName == "Train")
        {
            OnTrainButtonClicked();
        }
        else if (buttonName == "Pray")
        {
            OnPrayButtonClicked();
        }        
        else if (buttonName == "Bathe")
        {
            OnBatheButtonClicked();
        }
        
    }
    public void OnTriageButtonClicked()
    {
        Debug.Log("CampSiteManager.OnTriageButtonClicked() called...");
        ClearAllOrders();

        if (HasEnoughCampSitePoints(triagePointCost))
        {
            triageButton.EnableGlowAnimation();
            awaitingTriageChoice = true;
        }

    }        
    public void OnTrainButtonClicked()
    {
        Debug.Log("CampSiteManager.OnTrainButtonClicked() called...");

        ClearAllOrders();
        if (HasEnoughCampSitePoints(trainPointCost))
        {
            trainButton.EnableGlowAnimation();
            awaitingTrainChoice = true;
        }           
    }
    public void OnPrayButtonClicked()
    {
        Debug.Log("CampSiteManager.OnPrayButtonClicked() called...");

        ClearAllOrders();
        if (HasEnoughCampSitePoints(prayPointCost))
        {
            bool atLeastOneCharacterIsDead = false;
            foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
            {
                if (character.currentHealth == 0)
                {
                    atLeastOneCharacterIsDead = true;
                    character.myCampSiteCharacter.SetGlowOutilineViewState(true);
                }
            }

            if (atLeastOneCharacterIsDead)
            {
                prayButton.SetGlowOutilineViewState(true);
                awaitingPrayChoice = true;
            }
            
        }

    }
    public void OnFeastButtonClicked()
    {
        Debug.Log("CampSiteManager.OnFeastButtonClicked() called...");

        ClearAllOrders();
        if (HasEnoughCampSitePoints(feastPointCost))
        {
            PerformFeast();
        }
    }
    public void OnRestButtonClicked()
    {
        Debug.Log("CampSiteManager.OnRestButtonClicked() called...");

        ClearAllOrders();
        if (HasEnoughCampSitePoints(restPointCost))
        {
            PerformRest();
        }

    }
    public void OnBatheButtonClicked()
    {
        Debug.Log("CampSiteManager.OnBatheButtonClicked() called...");

        ClearAllOrders();

        if (HasEnoughCampSitePoints(bathePointCost) && StateManager.Instance.HasAtleastOneAfflicationState())
        {
            Debug.Log("'Bathe' action requirments met, awaiting bathe choice...");
            awaitingBatheChoice = true;

            StateManager.Instance.ClearAfflicationsPanel();
            StateManager.Instance.SetAfflicationPanelViewState(true);
            StateManager.Instance.PopulateAfflicationsPanel();
            // TO DO: Enable afflications panel view here

        }

    }
    public void OnContinueButtonClicked()
    {
        Debug.Log("CampSiteManager.OnContinueButtonClicked() called...");

        DisableCampSiteView();
        UIManager.Instance.OnWorldMapButtonClicked();
        UIManager.Instance.EnableWorldMapView();
        // re enable world map + get next viable enocunter hexagon tiles
        WorldManager.Instance.SetWorldMapReadyState();
    }
    #endregion

    // Visibility / View logic
    #region
    public void EnableCampSiteView()
    {
        visualParent.SetActive(true);
    }
    public void DisableCampSiteView()
    {
        visualParent.SetActive(false);
    }
    public void SetCharacterPanelGlowOutlineViewStates(bool onOrOff)
    {
        foreach(CampSiteCharacter character in allCharacterSlots)
        {
            //character.SetGlowOutilineViewState(onOrOff);
        }
    }
    public void DisableAllButtonGlows()
    {
        triageButton.DisableGlowAnimation();
        trainButton.DisableGlowAnimation();
        prayButton.DisableGlowAnimation();
        batheButton.DisableGlowAnimation();
    }
    
    
    #endregion

    // Misc Logic
    #region
    public void ResetEventProperties()
    {
        ModifyCurrentCampSitePoints(maxActionPoints - currentActionPoints);        
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
    public void ClearAllOrders()
    {
        awaitingTrainChoice = false;
        awaitingTriageChoice = false;
        awaitingPrayChoice = false;
        awaitingBatheChoice = false;
        SetCharacterPanelGlowOutlineViewStates(false);
        DisableAllButtonGlows();

    }
    public bool AlreadyAwaitingAnOrder()
    {
        if(awaitingBatheChoice ||
            awaitingPrayChoice ||
            awaitingTrainChoice ||
            awaitingTriageChoice)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ModifyCurrentCampSitePoints(int pointsGainedOrLost)
    {
        Debug.Log("CampSiteManager.ModifyCurrentCampSitePoints() called, modifying camp site points by " + pointsGainedOrLost.ToString());

        currentActionPoints += pointsGainedOrLost;
        actionPointsText.text = currentActionPoints.ToString();
    }
    #endregion

    // Trigger Camp Site Events
    #region
    public void PerformTriage(CampSiteCharacter characterClicked)
    {
        Debug.Log("CampSiteManager.PerformTriage() called...");

        // heal 50%
        characterClicked.myCharacterData.ModifyCurrentHealth(characterClicked.myCharacterData.maxHealth / 2);
        ModifyCurrentCampSitePoints(-triagePointCost);
        awaitingTriageChoice = false;
        SetCharacterPanelGlowOutlineViewStates(false);
        ClearAllOrders();
    }
    public void PerformTrain(CampSiteCharacter characterClicked)
    {
        Debug.Log("CampSiteManager.PerformTrain() called...");

        // Grant +50 XP
        characterClicked.myCharacterData.ModifyCurrentXP(50);
        
        ModifyCurrentCampSitePoints(-trainPointCost);
        awaitingTrainChoice = false;
        trainButton.SetGlowOutilineViewState(false);
        SetCharacterPanelGlowOutlineViewStates(false);
        ClearAllOrders();
    }
    public void PerformPray(CampSiteCharacter characterClicked)
    {
        Debug.Log("CampSiteManager.PerformPray() called...");

        // heal 50%
        characterClicked.myCharacterData.ModifyCurrentHealth(characterClicked.myCharacterData.maxHealth / 2);
        ModifyCurrentCampSitePoints(-prayPointCost);
        awaitingPrayChoice = false;
        prayButton.SetGlowOutilineViewState(false);
        SetCharacterPanelGlowOutlineViewStates(false);
        ClearAllOrders();
    }
    public void PerformFeast()
    {
        Debug.Log("CampSiteManager.PerformFeast() called...");        

        StateManager.Instance.GainState(StateLibrary.Instance.GetStateByName("Well Fed"));
        ModifyCurrentCampSitePoints(-feastPointCost);
        ClearAllOrders();
    }
    public void PerformRest()
    {
        Debug.Log("CampSiteManager.PerformRest() called...");

        StateManager.Instance.GainState(StateLibrary.Instance.GetStateByName("Well Rested"));
        ModifyCurrentCampSitePoints(-restPointCost);
        ClearAllOrders();
    }
    public void PerformBathe(AfflictionOnPanel chosenState)
    {
        Debug.Log("CampSiteManager.PerformBathe() called...");

        chosenState.myState.PlayExpireVfxAndDestroy();
        StateManager.Instance.SetAfflicationPanelViewState(false);
        StateManager.Instance.ClearAfflicationsPanel();

        ClearAllOrders();
    }
    #endregion





}
