using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("UI Component References")]
    public Button EndTurnButton;
    public GameObject GameOverCanvas;
    public GameObject CharacterRoster;
    public GameObject worldMap;
    public GameObject rewardScreen;
    public GameObject Inventory;

    // Mouse + Click Events
    #region
    public void OnCharacterPanelBackButtonClicked()
    {
        CharacterRoster.SetActive(false);        
    }
    public void OnCharacterPanelButtonClicked()
    {
        if (CharacterRoster.activeSelf == true)
        {
            DisableCharacterRosterView();
        }

        else
        {
            EnableCharacterRosterView();
            DisableInventoryView();
            DisableWorldMapView();
        }
            
    }  
    public void OnInventoryButtonClicked()
    {
        if (Inventory.activeSelf == true)
        {
            DisableInventoryView();            
        }

        else if (Inventory.activeSelf == false)
        {
            EnableInventoryView();
            DisableCharacterRosterView();
            DisableWorldMapView();
        }
    }
    public void OnWorldMapButtonClicked()
    {
        if(worldMap.activeSelf == true)
        {
            DisableWorldMapView();
        }

        else if (worldMap.activeSelf == false)
        {
            DisableInventoryView();
            DisableCharacterRosterView();
            EnableWorldMapView();            
        }
    }
    #endregion

    // Visibility + View Logic
    #region
    public void EnableWorldMapView()
    {
        worldMap.SetActive(true);
        if (WorldManager.Instance.canSelectNewEncounter == true)
        {
            WorldManager.Instance.HighlightNextAvailableEncounters();
        }
    }
    public void DisableWorldMapView()
    {
        worldMap.SetActive(false);
    }
    public void EnableRewardScreenView()
    {        
        RewardScreen.Instance.EnableRewardScreenView();
    }   
    public void DisableRewardScreenView()
    {        
        RewardScreen.Instance.DisableRewardScreenView();
    }   
    public void EnableInventoryView()
    {
        Inventory.SetActive(true);
    }
    public void DisableInventoryView()
    {
        Inventory.SetActive(false);
    }
    public void EnableCharacterRosterView()
    {
        CharacterRoster.SetActive(true);
    }
    public void DisableCharacterRosterView()
    {
        CharacterRoster.SetActive(false);
        CampSiteManager.Instance.awaitingTrainChoice = false;
    }
    public void DisableEndTurnButton()
    {
        EndTurnButton.gameObject.SetActive(false);
    }
    public void EnableEndTurnButton()
    {
        EndTurnButton.gameObject.SetActive(true);
    }
    public void EnableGameOverCanvas()
    {
        GameOverCanvas.gameObject.SetActive(true);
    }
    #endregion
}
