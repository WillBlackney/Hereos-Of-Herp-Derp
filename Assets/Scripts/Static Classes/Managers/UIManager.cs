using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Component References")]    
    public GameObject GameOverScreenParent;
    public GameObject GameOverScreenCanvasParent;
    public TextMeshProUGUI GameOverScreenTitleText;
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
    private void Start()
    {
        // for some reason, game build always open inventory on game start
        // these lines prevents this weird bug
        DisableInventoryView();
        DisableInventoryView();
    }

    // Mouse + Click Events
    #region
    public void OnCharacterPanelBackButtonClicked()
    {
        CharacterRoster.Instance.canvasParent.SetActive(false);
        CharacterRoster.Instance.visualParent.SetActive(false);
    }
    public void OnCharacterPanelButtonClicked()
    {
        KingsBlessingManager.Instance.DisableView();

        if (CharacterRoster.Instance.visualParent.activeSelf == true)
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
            DisableWorldMapView();
        }
            
    }  
   
    public void OnWorldMapButtonClicked()
    {
        KingsBlessingManager.Instance.DisableView();

        if(WorldManager.Instance.visualParent.activeSelf == true)
        {
            DisableWorldMapView();
            if (KingsBlessingManager.Instance.eventCompleted == false)
            {
                KingsBlessingManager.Instance.EnableView();
            }
        }

        else if (WorldManager.Instance.visualParent.activeSelf == false)
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
        WorldManager.Instance.canvasParent.SetActive(true);
        WorldManager.Instance.visualParent.SetActive(true);        
        if (WorldManager.Instance.canSelectNewEncounter == true)
        {
            WorldManager.Instance.HighlightNextAvailableEncounters();
        }
    }
    public void DisableWorldMapView()
    {
        WorldManager.Instance.visualParent.SetActive(false);
        WorldManager.Instance.canvasParent.SetActive(false);
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
        InventoryController.Instance.canvasParent.SetActive(true);
        InventoryController.Instance.visualParent.SetActive(true);
    }
    public void DisableInventoryView()
    {
        InventoryController.Instance.canvasParent.SetActive(false);
        InventoryController.Instance.visualParent.SetActive(false);
    }
    public void EnableCharacterRosterView()
    {
        CharacterRoster.Instance.visualParent.SetActive(true);
        CharacterRoster.Instance.canvasParent.SetActive(true);
        // set character one as default view
        CharacterRoster.Instance.SetDefaultViewState();
    }
    public void DisableCharacterRosterView()
    {
        CharacterRoster.Instance.visualParent.SetActive(false);
        CharacterRoster.Instance.canvasParent.SetActive(false);
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
        GameOverScreenCanvasParent.SetActive(true);
        GameOverScreenParent.SetActive(true);
        CanvasGroup gameOverScreenCG = GameOverScreenParent.GetComponent<CanvasGroup>();

        gameOverScreenCG.alpha = 0;
        while(gameOverScreenCG.alpha < 1)
        {
            gameOverScreenCG.alpha += 0.1f * 10 * Time.deltaTime;            
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
        SceneController.Instance.continueButton.SetActive(false);
        SceneController.Instance.LoadMenuSceneAsync(true);
    }

    public void DisableUnneededCanvasesOnCombatStart()
    {
        DisableCharacterRosterView();
        DisableInventoryView();
        DisableWorldMapView();
    }
    #endregion
}
