using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class SceneController : MonoBehaviour
{
    // Properties + Components
    #region
    [Header("Component References")]
    public Slider loadingScreenSlider;
    public TextMeshProUGUI loadingValueText;
    public GameObject loadScreenVisualParent;
    public GameObject continueButton;
    public CanvasGroup continueButtonCG;
    public CanvasGroup loadingBarCG;

    [Header("Properties")]
    public bool playerPressedContinue;
    public bool currentLoadFinished;
    #endregion

    // Singleton Set up
    #region
    public static SceneController Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayStartSequence();
    }
    #endregion

    // Load Scene Logic
    #region
    public void LoadMenuSceneAsync(bool displayTips)
    {
        StartCoroutine(LoadMenuSceneAsyncCoroutine(displayTips));
    }
    private IEnumerator LoadMenuSceneAsyncCoroutine(bool displayTips)
    {
        // Set up + reset values from a previous scene load
        playerPressedContinue = false;
        loadingScreenSlider.value = 0;
        loadingBarCG.alpha = 1;
        continueButton.SetActive(false);

        // create CG component dynamically
        //continueButtonCG = continueButton.AddComponent<CanvasGroup>();
        //continueButtonCG.alpha = 0;

        bool fadeOutStarted = false;    
        float startTime = Time.time;

        // start showing tips
        if (displayTips)
        {
            TipsManager.Instance.DisplayTips();
        }

        // Start async scene load
        AsyncOperation operation = SceneManager.LoadSceneAsync("Menu Scene");

        // Update load progress bar + text
        while (operation.isDone == false && currentLoadFinished == false)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingScreenSlider.value = progress;
            loadingValueText.text = ((int)(progress * 100)).ToString() + "%";    

            // Menu scene transition sequence
            if (operation.progress >= 0.9f &&  fadeOutStarted == false)
            {
                fadeOutStarted = true;
                Debug.Log("Menu Scene load duration: " + (Time.time - startTime).ToString() + " seconds");

                // Start screen fade transistion
                Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 2, 1, true);
                yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

                // Disable tips
                TipsManager.Instance.displayTips = false;
                TipsManager.Instance.tipTextCG.alpha = 0;
                
                // disable load screen view
                loadScreenVisualParent.SetActive(false);

                // start menu scene visual sequence
                PlayStartSequence();
                currentLoadFinished = true;
            }
            
            yield return null;
        }     
        
        
    }

    public void LoadGameSceneAsync()
    {
        StartCoroutine(LoadGameSceneAsyncCoroutine());
    }
    private IEnumerator LoadGameSceneAsyncCoroutine()
    {
        // Set up + reset values from a previous scene load
        currentLoadFinished = false;
        playerPressedContinue = false;
        loadingScreenSlider.value = 0;
        loadingBarCG.alpha = 1;
        continueButton.SetActive(false);

        // create CG component dynamically
        continueButtonCG = continueButton.AddComponent<CanvasGroup>();
        continueButtonCG.alpha = 0;

        // start the loading time clock
        float startTime = Time.time;

        // enable tips
        TipsManager.Instance.DisplayTips();        

        // Start async scene load
        AsyncOperation operation = SceneManager.LoadSceneAsync("Game Scene");

        // Update load progress bar + text        
        while (operation.isDone == false)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingScreenSlider.value = progress;
            loadingValueText.text = ((int)(progress * 100)).ToString() + "%";
            yield return null;
        }          
        

        // print scene load time
        Debug.Log("Game Scene load duration: " + (Time.time - startTime).ToString() + " seconds");

        // fade out loading bar
        Action hideLoadingBar = FadeOutLoadingBar();
        yield return new WaitUntil(() => hideLoadingBar.actionResolved == true);

        // fade in continue button
        Action showContinueButton = FadeInContinueButton();
        yield return new WaitUntil(() => showContinueButton.actionResolved == true);

        // wait until player presses the 'continue button' before revealing the game scene
        yield return new WaitUntil(() => playerPressedContinue == true);
        playerPressedContinue = false;

        // Start screen fade transistion
        Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 2, 1, true);
        yield return new WaitUntil(() => fadeOut.ActionResolved() == true);

        // Disable tips
        TipsManager.Instance.displayTips = false;
        TipsManager.Instance.tipTextCG.alpha = 0;

        // disable load screen view
        loadScreenVisualParent.SetActive(false);

        // Fade in game scene
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 2, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);
        currentLoadFinished = true;

    }
    #endregion

    public void PlayStartSequence()
    {
        StartCoroutine(PlayStartSequenceCoroutine());
    }
    private IEnumerator PlayStartSequenceCoroutine()
    {
        Debug.Log("MainMenuManager.PlayStartSequenceCoroutine() started...");
        // Set up
        BlackScreenManager.Instance.canvasGroup.alpha = 0;
        BlackScreenManager.Instance.canvasGroup.alpha = 1;
        MainMenuManager.Instance.textElementsParentCG.alpha = 0;
        MainMenuManager.Instance.allElementsParent.transform.position = MainMenuManager.Instance.northPos.transform.position;

        // Start fade in
        Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 2, 0, false);
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);
        yield return new WaitForSeconds(1);

        // move screen to normal position
        while (MainMenuManager.Instance.allElementsParent.transform.position != MainMenuManager.Instance.centrePos.transform.position)
        {
            MainMenuManager.Instance.allElementsParent.transform.position = Vector3.MoveTowards(MainMenuManager.Instance.allElementsParent.transform.position,
                MainMenuManager.Instance.centrePos.transform.position, 5 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        // brief wait
        yield return new WaitForSeconds(0.5f);

        // fade in text elements and UI
        while (MainMenuManager.Instance.textElementsParentCG.alpha < 1)
        {
            //Debug.Log("fading in text and ui");
            MainMenuManager.Instance.textElementsParentCG.alpha += 0.80f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

    }
    public void OnContinueButtonClicked()
    {
        playerPressedContinue = true;
    }
    public Action FadeInContinueButton()
    {
        Debug.Log("SceneController.FadeInContinueButton() called...");
        Action action = new Action();
        StartCoroutine(FadeInContinueButtonCoroutine(action));
        return action;
    }
    public IEnumerator FadeInContinueButtonCoroutine(Action action)
    {
        continueButton.SetActive(true);

        while (continueButtonCG.alpha < 1)
        {
            continueButtonCG.alpha += 2f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // canvas group prevents button component from working
        // therefore, the component needs to be destroyed
        Destroy(continueButtonCG);
        action.actionResolved = true;
    }
    public Action FadeOutLoadingBar()
    {
        Debug.Log("SceneController.FadeOutLoadingBar() called...");
        Action action = new Action();
        StartCoroutine(FadeOutLoadingBarCoroutine(action));
        return action;
    }
    public IEnumerator FadeOutLoadingBarCoroutine(Action action)
    {
        while (loadingBarCG.alpha > 0)
        {
            loadingBarCG.alpha -= 2f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        action.actionResolved = true;
    }
}
