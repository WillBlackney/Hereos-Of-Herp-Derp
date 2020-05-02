using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [Header("UI Component References")]    
    public GameObject GameOverScreenParent;
    public GameObject GameOverScreenCanvasParent;
    public TextMeshProUGUI GameOverScreenTitleText;

    [Header("End Turn Button Component References")]
    public Button EndTurnButton;
    public Image EndTurnButtonBGImage;
    public TextMeshProUGUI EndTurnButtonText;
    public Sprite EndTurnButtonDisabledSprite;
    public Sprite EndTurnButtonEnabledSprite;

    [Header("Character Roster Movement References")]
    public Canvas charRosterCanvasComponent;
    public RectTransform characterRosterCentrePosition;
    public RectTransform characterRosterOffScreenPosition;
    public RectTransform characterRosterTransformParent;
    public bool crMovingOnScreen;
    public bool crMovingOffScreen;
    public float characterRosterMoveSpeed;

    [Header("Inventory Movement References")]
    public Canvas inventoryCanvasComponent;
    public RectTransform inventoryCentrePosition;
    public RectTransform inventoryOffScreenPosition;
    public RectTransform inventoryTransformParent;
    public bool inventoryMovingOnScreen;
    public bool inventoryMovingOffScreen;
    public float inventoryMoveSpeed;

    [Header("World Map Movement References")]
    public Canvas worldMapCanvasComponent;
    public RectTransform worldMapCentrePosition;
    public RectTransform worldMapOffScreenPosition;
    public RectTransform worldMapTransformParent;
    public bool worldMapMovingOnScreen;
    public bool worldMapMovingOffScreen;
    public float worldMapMoveSpeed;

    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;       
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
        // refresh char roster button highlight
        EventSystem.current.SetSelectedGameObject(null);

        if (CharacterRoster.Instance.visualParent.activeSelf == true)
        {
            MoveCharacterRosterOffScreen();
            MoveInventoryOffScreen();            
        }

        else
        {
            // Enable parents + canvases
            EnableCharacterRosterView();
            EnableInventoryView();

            // Move views on screen
            MoveCharacterRosterOnScreen();
            MoveInventoryOnScreen();

            // Move world map off screen
            MoveWorldMapOffScreen();
        }
            
    }     
    public void OnWorldMapButtonClicked()
    {
        // refresh world map button highlight
        EventSystem.current.SetSelectedGameObject(null);

        if (WorldManager.Instance.visualParent.activeSelf == true)
        {
            // Move map away and disable
            MoveWorldMapOffScreen();
        }

        else if (WorldManager.Instance.visualParent.activeSelf == false)
        {
            // Move map on screen and enable
            EnableWorldMapView();
            MoveWorldMapOnScreen();

            // Move char roster + inventory off screen and disable
            MoveInventoryOffScreen();
            MoveCharacterRosterOffScreen();
        }
    }
    #endregion

    // Character Roster Movement
    #region
    public Action MoveCharacterRosterOnScreen()
    {
        Debug.Log("UIManager.MoveCharacterRosterToCentrePosition() called...");
        Action action = new Action();
        StartCoroutine(MoveCharacterRosterOnScreenCoroutine(action));
        return action;

    }
    private IEnumerator MoveCharacterRosterOnScreenCoroutine(Action action)
    {
        // reset z axis
        characterRosterTransformParent.anchoredPosition = new Vector3(characterRosterTransformParent.anchoredPosition.x, characterRosterTransformParent.anchoredPosition.y, 0f);

        // set up
        crMovingOffScreen = false;
        crMovingOnScreen = true;
        bool reachedCentrePos = false;

        while (reachedCentrePos == false && crMovingOnScreen)
        {
            Debug.Log("UIManager.MoveCharacterRosterToCentrePositionCoroutine() running 'while' loop...");

            characterRosterTransformParent.anchoredPosition = Vector2.MoveTowards(characterRosterTransformParent.anchoredPosition, characterRosterCentrePosition.anchoredPosition, Time.deltaTime * characterRosterMoveSpeed);

            if (characterRosterTransformParent.anchoredPosition.y == characterRosterCentrePosition.anchoredPosition.y)
            {
                reachedCentrePos = true;
                // reset z axis
                characterRosterTransformParent.anchoredPosition = new Vector3(characterRosterTransformParent.anchoredPosition.x, characterRosterTransformParent.anchoredPosition.y, 0f);                
            }

            yield return new WaitForEndOfFrame();
        }

        crMovingOnScreen = false;
        action.actionResolved = true;
    }
    public Action MoveCharacterRosterOffScreen()
    {
        Debug.Log("UIManager.MoveCharacterRosterToCentrePosition() called...");
        Action action = new Action();
        StartCoroutine(MoveCharacterRosterOffScreenCoroutine(action));
        return action;

    }
    private IEnumerator MoveCharacterRosterOffScreenCoroutine(Action action)
    {
        // reset z axis
        characterRosterTransformParent.anchoredPosition = new Vector3(characterRosterTransformParent.anchoredPosition.x, characterRosterTransformParent.anchoredPosition.y, 0f);

        crMovingOnScreen = false;
        crMovingOffScreen = true;
        
        bool reachedOffScreenPos = false;

        while (reachedOffScreenPos == false && crMovingOffScreen)
        {
            Debug.Log("UIManager.MoveCharacterRosterOffScreenCoroutine() running 'while' loop...");

            characterRosterTransformParent.anchoredPosition = Vector2.MoveTowards(characterRosterTransformParent.anchoredPosition, characterRosterOffScreenPosition.anchoredPosition, Time.deltaTime * characterRosterMoveSpeed);

            if (characterRosterTransformParent.anchoredPosition.y == characterRosterOffScreenPosition.anchoredPosition.y)
            {
                // reset z axis
                characterRosterTransformParent.anchoredPosition = new Vector3(characterRosterTransformParent.anchoredPosition.x, characterRosterTransformParent.anchoredPosition.y, 0f);
                reachedOffScreenPos = true;
                DisableCharacterRosterView();               
            }
            yield return new WaitForEndOfFrame();
        }     
        
        crMovingOffScreen = false;
        action.actionResolved = true;
    }

    #endregion

    // Inventory Movement
    #region
    public Action MoveInventoryOnScreen()
    {
        Debug.Log("UIManager.MoveInventoryOnScreen() called...");
        Action action = new Action();
        StartCoroutine(MoveInventoryOnScreenCoroutine(action));
        return action;

    }
    private IEnumerator MoveInventoryOnScreenCoroutine(Action action)
    {
        // reset z axis
        inventoryTransformParent.anchoredPosition = new Vector3(inventoryTransformParent.anchoredPosition.x, characterRosterTransformParent.anchoredPosition.y, 0f);

        inventoryMovingOffScreen = false;
        inventoryMovingOnScreen = true;
        bool reachedCentrePos = false;

        while (reachedCentrePos == false && inventoryMovingOnScreen)
        {
            Debug.Log("UIManager.MoveInventoryOnScreenCoroutine() running 'while' loop...");

            inventoryTransformParent.anchoredPosition = Vector2.MoveTowards(inventoryTransformParent.anchoredPosition, inventoryCentrePosition.anchoredPosition, Time.deltaTime * inventoryMoveSpeed);

            if (inventoryTransformParent.anchoredPosition.y == inventoryCentrePosition.anchoredPosition.y)
            {
                reachedCentrePos = true;
                // reset z axis
                inventoryTransformParent.anchoredPosition = new Vector3(inventoryTransformParent.anchoredPosition.x, inventoryTransformParent.anchoredPosition.y, 0f);                
            }

            yield return new WaitForEndOfFrame();
        }

        inventoryMovingOnScreen = false;
        action.actionResolved = true;
    }
    public Action MoveInventoryOffScreen()
    {
        Debug.Log("UIManager.MoveInventoryOffScreen() called...");
        Action action = new Action();
        StartCoroutine(MoveInventoryOffScreenCoroutine(action));
        return action;

    }
    private IEnumerator MoveInventoryOffScreenCoroutine(Action action)
    {
        // reset z axis
        inventoryTransformParent.anchoredPosition = new Vector3(inventoryTransformParent.anchoredPosition.x, characterRosterTransformParent.anchoredPosition.y, 0f);

        inventoryMovingOnScreen = false;
        inventoryMovingOffScreen = true;

        bool reachedOffScreenPos = false;

        while (reachedOffScreenPos == false && inventoryMovingOffScreen)
        {
            Debug.Log("UIManager.MoveInventoryOffScreenCoroutine() running 'while' loop...");

            inventoryTransformParent.anchoredPosition = Vector2.MoveTowards(inventoryTransformParent.anchoredPosition, inventoryOffScreenPosition.anchoredPosition, Time.deltaTime * inventoryMoveSpeed);

            if (inventoryTransformParent.anchoredPosition.y == inventoryOffScreenPosition.anchoredPosition.y)
            {
                reachedOffScreenPos = true;
                // reset z axis
                inventoryTransformParent.anchoredPosition = new Vector3(inventoryTransformParent.anchoredPosition.x, characterRosterTransformParent.anchoredPosition.y, 0f);
                DisableInventoryView();                
            }
            yield return new WaitForEndOfFrame();
        }
        
        inventoryMovingOffScreen = false;
        action.actionResolved = true;
    }

    #endregion

    // World Map Movement
    #region
    public Action MoveWorldMapOnScreen()
    {
        Debug.Log("UIManager.MoveWorldMapOnScreen() called...");
        Action action = new Action();
        StartCoroutine(MoveWorldMapOnScreenCoroutine(action));
        return action;

    }
    private IEnumerator MoveWorldMapOnScreenCoroutine(Action action)
    {
        worldMapMovingOffScreen = false;
        worldMapMovingOnScreen = true;
        bool reachedCentrePos = false;

        while (reachedCentrePos == false && worldMapMovingOnScreen)
        {
            Debug.Log("UIManager.MoveWorldMapOnScreenCoroutine() running 'while' loop...");

            worldMapTransformParent.anchoredPosition = Vector2.MoveTowards(worldMapTransformParent.anchoredPosition, worldMapCentrePosition.anchoredPosition, Time.deltaTime * worldMapMoveSpeed);

            if (worldMapTransformParent.anchoredPosition.y == worldMapCentrePosition.anchoredPosition.y)
            {
                reachedCentrePos = true;
            }

            yield return new WaitForEndOfFrame();
        }

        worldMapMovingOnScreen = false;
        action.actionResolved = true;
    }
    public Action MoveWorldMapOffScreen()
    {
        Debug.Log("UIManager.MoveInventoryOffScreen() called...");
        Action action = new Action();
        StartCoroutine(MoveWorldMapOffScreenCoroutine(action));
        return action;

    }
    private IEnumerator MoveWorldMapOffScreenCoroutine(Action action)
    {
        worldMapMovingOnScreen = false;
        worldMapMovingOffScreen = true;

        bool reachedOffScreenPos = false;

        while (reachedOffScreenPos == false && worldMapMovingOffScreen)
        {
            worldMapTransformParent.anchoredPosition = Vector2.MoveTowards(worldMapTransformParent.anchoredPosition, worldMapOffScreenPosition.anchoredPosition, Time.deltaTime * worldMapMoveSpeed);

            if (worldMapTransformParent.anchoredPosition.y == worldMapOffScreenPosition.anchoredPosition.y)
            {
                reachedOffScreenPos = true;
                DisableWorldMapView();
            }
           
            yield return new WaitForEndOfFrame();
        }
        
        worldMapMovingOffScreen = false;
        action.actionResolved = true;
    }

    #endregion

    // Visibility + View Logic
    #region
    public void EnableWorldMapView()
    {
        //worldMapCanvasComponent.enabled = true;
        WorldManager.Instance.canvasParent.SetActive(true);
        WorldManager.Instance.visualParent.SetActive(true);  
        
        if (WorldManager.Instance.canSelectNewEncounter == true)
        {
            WorldManager.Instance.HighlightNextAvailableEncounters();
        }
    }
    public void DisableWorldMapView()
    {
       // worldMapCanvasComponent.enabled = false;
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
        if (EventManager.Instance.combatSceneIsActive)
        {
            InventoryController.Instance.canvasParent.SetActive(false);
        }        
        InventoryController.Instance.visualParent.SetActive(false);
    }
    public void EnableCharacterRosterView()
    {
        Debug.Log("UIManager.EnableCharacterRosterView() called...");
        CharacterRoster.Instance.visualParent.SetActive(true);
        CharacterRoster.Instance.canvasParent.SetActive(true);

        // set character one as default view
        CharacterRoster.Instance.SetDefaultViewState();
    }
    public void DisableCharacterRosterView()
    {
        Debug.Log("UIManager.DisableCharacterRosterView() called...");
        if (EventManager.Instance.combatSceneIsActive)
        {
            CharacterRoster.Instance.canvasParent.SetActive(false);
        }
        CharacterRoster.Instance.visualParent.SetActive(false);
       
        
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
