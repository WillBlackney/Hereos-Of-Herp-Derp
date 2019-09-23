using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("All UI Elements")]
    public Button EndTurnButton;
    public GameObject GameOverCanvas;
    public GameObject CharacterRoster;
    public GameObject worldMap;
    public GameObject RewardScreen;
    public GameObject Inventory;

    // Buttons
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

    // On Button click events

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
            if(WorldMap.Instance.canSelectNewEncounter == true)
            {
                WorldMap.Instance.HighlightNextAvailableEncounters();
            }
            
            
        }
    }

    // Enable / Disable views

    public void EnableWorldMapView()
    {
        worldMap.SetActive(true);
    }

    public void DisableWorldMapView()
    {
        worldMap.SetActive(false);
    }

    public void EnableRewardScreenView()
    {
        RewardScreen.SetActive(true);
    }

    public void DisableRewardScreenView()
    {
        RewardScreen.SetActive(false);
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
        CampSiteManager.Instance.awaitingLevelUpChoice = false;
    }
}
