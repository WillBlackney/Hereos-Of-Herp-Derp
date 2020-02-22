﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Component References")]    
    public GameObject GameOverScreenParent;
    public TextMeshProUGUI GameOverScreenTitleText;
    public GameObject CharacterRoster;
    public GameObject worldMap;
    public GameObject rewardScreen;
    public GameObject Inventory;
    //public GameObject

    [Header("End Turn Button Component References")]
    public Button EndTurnButton;
    public Image EndTurnButtonBGImage;
    public TextMeshProUGUI EndTurnButtonText;
    public Sprite EndTurnButtonDisabledSprite;
    public Sprite EndTurnButtonEnabledSprite;

    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Mouse + Click Events
    #region
    public void OnCharacterPanelBackButtonClicked()
    {
        CharacterRoster.SetActive(false);        
    }
    public void OnCharacterPanelButtonClicked()
    {
        KingsBlessingManager.Instance.DisableView();

        if (CharacterRoster.activeSelf == true)
        {
            DisableInventoryView();
            DisableCharacterRosterView();
            if(KingsBlessingManager.Instance.eventCompleted == false)
            {
                KingsBlessingManager.Instance.EnableView();
            }
        }

        else
        {
            EnableCharacterRosterView();
            EnableInventoryView();
            //DisableInventoryView();
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
        KingsBlessingManager.Instance.DisableView();

        if(worldMap.activeSelf == true)
        {
            DisableWorldMapView();
            if (KingsBlessingManager.Instance.eventCompleted == false)
            {
                KingsBlessingManager.Instance.EnableView();
            }
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
    public void DisableEndTurnButtonInteractions()
    {
        EndTurnButton.interactable = false;
        SetEndTurnButtonSprite(EndTurnButtonDisabledSprite);
    }
    public void EnableEndTurnButtonInteractions()
    {        
        EndTurnButton.interactable = true;
        SetEndTurnButtonSprite(EndTurnButtonEnabledSprite);
    }
    public void DisableEndTurnButtonView()
    {
        EndTurnButton.gameObject.SetActive(false);
    }
    public void EnableEndTurnButtonView()
    {
        EndTurnButton.gameObject.SetActive(true);
    }
    public void SetEndTurnButtonSprite(Sprite newSprite)
    {
        EndTurnButtonBGImage.sprite = newSprite;
    }
    public void SetEndTurnButtonText(string newText)
    {
        EndTurnButtonText.text = newText;
    }    
    public Action FadeInGameOverScreen()
    {
        Action action = new Action();
        StartCoroutine(FadeInGameOverScreenCoroutine(action));
        return action;
    }
    public IEnumerator FadeInGameOverScreenCoroutine(Action action)
    {
        GameOverScreenParent.SetActive(true);
        CanvasGroup gameOverScreenCG = GameOverScreenParent.GetComponent<CanvasGroup>();

        gameOverScreenCG.alpha = 0;
        while(gameOverScreenCG.alpha < 1)
        {
            gameOverScreenCG.alpha += 0.1f;            
            yield return new WaitForEndOfFrame();
        }

        action.actionResolved = true;
    }
    public void OnGameOverScreenMainMenuButtonClicked()
    {
        StartCoroutine(OnGameOverScreenMainMenuButtonClickedCoroutine());
    }
    public IEnumerator OnGameOverScreenMainMenuButtonClickedCoroutine()
    {        
        // Start screen fade transistion
        Action fadeAction = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 2, 1, true);
        yield return new WaitUntil(() => fadeAction.ActionResolved() == true);

        // Enable loading screen
        SceneController.Instance.loadScreenVisualParent.SetActive(true);

        // Fade Screen back in
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 4, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);

        // Ready, load the game scene
        SceneController.Instance.LoadSceneAsync("Menu Scene");
    }
    #endregion
}
