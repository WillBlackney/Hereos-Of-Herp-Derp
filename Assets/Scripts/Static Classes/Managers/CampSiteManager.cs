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
        ClearAllOrders();

        if (HasEnoughCampSitePoints(triagePointCost))
        {
            triageButton.SetGlowOutilineViewState(true);
            SetCharacterPanelGlowOutlineViewStates(true);
            awaitingTriageChoice = true;
        }

    }        
    public void OnTrainButtonClicked()
    {
        ClearAllOrders();
        if (HasEnoughCampSitePoints(trainPointCost))
        {
            trainButton.SetGlowOutilineViewState(true);
            SetCharacterPanelGlowOutlineViewStates(true);
            awaitingTrainChoice = true;
        }           
    }
    public void OnPrayButtonClicked()
    {
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
        ClearAllOrders();
        if (HasEnoughCampSitePoints(feastPointCost))
        {
            PerformFeast();
        }
    }
    public void OnRestButtonClicked()
    {
        ClearAllOrders();
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
        WorldManager.Instance.SetWorldMapReadyState();
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
    public void SetCharacterPanelGlowOutlineViewStates(bool onOrOff)
    {
        foreach(CampSiteCharacter character in allCharacterSlots)
        {
            character.SetGlowOutilineViewState(onOrOff);
        }
    }
    public void DisableAllButtonGlows()
    {
        triageButton.SetGlowOutilineViewState(false);
        trainButton.SetGlowOutilineViewState(false);
        prayButton.SetGlowOutilineViewState(false);
    }
    public void ClearAllOrders()
    {
        awaitingTrainChoice = false;
        awaitingTriageChoice = false;
        awaitingPrayChoice = false;
        SetCharacterPanelGlowOutlineViewStates(false);
        DisableAllButtonGlows();

    }
    
    #endregion

    // Misc
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
        characterClicked.myCharacterData.ModifyCurrentHealth(characterClicked.myCharacterData.maxHealth / 2);
        ModifyCurrentCampSitePoints(-triagePointCost);
        awaitingTriageChoice = false;
        triageButton.SetGlowOutilineViewState(false);
        SetCharacterPanelGlowOutlineViewStates(false);
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
        trainButton.SetGlowOutilineViewState(false);
        SetCharacterPanelGlowOutlineViewStates(false);
    }
    public void PerformPray(CampSiteCharacter characterClicked)
    {
        // do nice visual stuff

        // heal 50%
        characterClicked.myCharacterData.ModifyCurrentHealth(characterClicked.myCharacterData.maxHealth / 2);
        ModifyCurrentCampSitePoints(-prayPointCost);
        awaitingPrayChoice = false;
        prayButton.SetGlowOutilineViewState(false);
        SetCharacterPanelGlowOutlineViewStates(false);
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
