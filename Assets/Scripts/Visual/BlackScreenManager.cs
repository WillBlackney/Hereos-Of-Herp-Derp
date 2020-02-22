using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackScreenManager : MonoBehaviour
{
    [Header("Component References")]
    public GameObject visualParent;
    public CanvasGroup canvasGroup;
    public Canvas canvas;

    [Header("Properties")]
    public int currentSortingLayer;
    public int aboveEverythingExceptUI;
    public int aboveEverything;
    public int behindEverything;
    public bool fadingOut;
    public bool fadingIn;

    // Initialization + Singleton Pattern
    #region
    public static BlackScreenManager Instance;
    void Awake()
    {
        SetupSingleton();
    }
    public void SetupSingleton()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }       
        
    }
    #endregion

    // Scene Change Listeners + Related
    #region
    private void OnEnable()
    {
        //SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    private void OnDisable()
    {
        //SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        /*
        Debug.Log("Loaded " + scene.name + " in mode " + mode);
        if(scene.name =="Game Scene")
        {
            FadeIn(aboveEverything, 2, 0, false);
        }
        */
        
    }
    #endregion

    // Visibility + View Logic
    #region
    public void SetSortingLayer(int newLayer)
    {
        canvas.sortingOrder = newLayer;
        currentSortingLayer = newLayer;
    }
    public void SetActive(bool onOrOff)
    {
        if (onOrOff == true)
        {
            visualParent.SetActive(true);
        }
        else
        {
            visualParent.SetActive(false);
        }
    }
    #endregion

    // Fade In / Out Logic
    #region
    public Action FadeIn(int sortingLayer, int speed = 2, float alphaTarget = 0, bool setActiveOnComplete = true)
    {
        Action action = new Action();
        StartCoroutine(FadeInCoroutine(sortingLayer, speed, alphaTarget, setActiveOnComplete, action));
        return action;
    }
    public IEnumerator FadeInCoroutine(int sortingLayer, int speed, float alphaTarget, bool setActiveOnComplete, Action action)
    {
        fadingIn = false;
        fadingOut = false;
        fadingIn = true;
        
        SetActive(true);
        SetSortingLayer(sortingLayer);

        Debug.Log("FadeInCoroutine() started...");
        //canvasGroup.alpha = 1;

        while (canvasGroup.alpha > alphaTarget && fadingIn)
        {
            canvasGroup.alpha -= 0.01f * speed;
            if(canvasGroup.alpha == alphaTarget)
            {
                SetSortingLayer(behindEverything);
                SetActive(setActiveOnComplete);
            }
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("FadeInCoroutine() finished...");        
        action.actionResolved = true;

    }
    public Action FadeOut(int sortingLayer, int speed = 2, float alphaTarget = 1, bool setActiveOnComplete = false)
    {
        Action action = new Action();
        StartCoroutine(FadeOutCoroutine(sortingLayer, speed, alphaTarget, setActiveOnComplete, action));
        return action;
    }
    public IEnumerator FadeOutCoroutine(int sortingLayer, int speed, float alphaTarget, bool setActiveOnComplete, Action action)
    {
        fadingIn = false;
        fadingOut = false;
        fadingOut = true;

        SetActive(true);
        SetSortingLayer(sortingLayer);
        //canvasGroup.alpha = 0;

        while (canvasGroup.alpha < alphaTarget && fadingOut == true)
        {
            canvasGroup.alpha += 0.01f * speed;
            if(canvasGroup.alpha == alphaTarget)
            {
                SetActive(setActiveOnComplete);
            }
            yield return new WaitForEndOfFrame();
        }

        /*
        if (EventManager.Instance.gameOverEventStarted == false)
        {
            SetSortingLayer(behindEverything);
            SetActive(false);
        }
        */
        //SetActive(setActiveOnComplete);
        action.actionResolved = true;
    }
    #endregion
}
