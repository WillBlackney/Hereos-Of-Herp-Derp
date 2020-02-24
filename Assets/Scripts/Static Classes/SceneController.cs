using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneController : MonoBehaviour
{
    // Properties + Components
    #region
    [Header("Component References")]
    public Slider loadingScreenSlider;
    public TextMeshProUGUI loadingValueText;
    public GameObject loadScreenVisualParent;
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
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
    }
    private IEnumerator LoadSceneAsyncCoroutine(string sceneName)
    {
        // Set up
        loadingScreenSlider.value = 0;
        bool fadeOutStarted = false;    
        float startTime = Time.time;

        // Start async scene load
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Update load progress bar + text
        while (operation.isDone == false)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingScreenSlider.value = progress;
            loadingValueText.text = ((int)(progress * 100)).ToString() + "%";

            if(operation.progress >= 0.9f && fadeOutStarted == false)
            {
                fadeOutStarted = true;

                // brief wait before fade out
                //yield return new WaitForSeconds(1);

                // Start screen fade transistion
                Action fadeOut = BlackScreenManager.Instance.FadeOut(BlackScreenManager.Instance.aboveEverything, 2, 1, true);
                yield return new WaitUntil(() => fadeOut.ActionResolved() == true);                

                // disable load screen view
                loadScreenVisualParent.SetActive(false);

            }
            yield return null;
        }

        // Start fade in
        if(sceneName == "Game Scene")
        {
            Action fadeIn = BlackScreenManager.Instance.FadeIn(BlackScreenManager.Instance.aboveEverything, 2, 0, false);
            yield return new WaitUntil(() => fadeIn.ActionResolved() == true);
        }

        else if(sceneName == "Menu Scene")
        {
            MainMenuManager.Instance.PlayStartSequence();
        }
        

        Debug.Log("Scene load duration: " + (Time.time - startTime).ToString() + " seconds");
        
    }

    #endregion

    public void PlayStartSequence()
    {
        StartCoroutine(PlayStartSequenceCoroutine());
    }
    public IEnumerator PlayStartSequenceCoroutine()
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
            MainMenuManager.Instance.textElementsParentCG.alpha += 0.05f;
            yield return new WaitForEndOfFrame();
        }

    }
}
