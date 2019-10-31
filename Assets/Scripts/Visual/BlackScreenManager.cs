using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreenManager : Singleton<BlackScreenManager>
{

    [Header("Component References")]
    public GameObject visualParent;
    public CanvasGroup canvasGroup;
    public Canvas canvas;

    [Header("Properties")]
    public int currentSortingLayer;
    public int aboveEverything;
    public int behindEverything;


    // Property Modifiers

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

    // Fade effects

    public Action FadeIn(int speed = 2, float alphaTarget = 0, bool setActiveOnComplete = true)
    {
        Action action = new Action();
        StartCoroutine(FadeInCoroutine(speed, alphaTarget, setActiveOnComplete, action));
        return action;
    }
    public IEnumerator FadeInCoroutine(int speed, float alphaTarget, bool setActiveOnComplete, Action action)
    {
        SetActive(true);
        SetSortingLayer(aboveEverything);

        Debug.Log("FadeInCoroutine() started...");
        //canvasGroup.alpha = 1;

        while (canvasGroup.alpha > alphaTarget)
        {
            canvasGroup.alpha -= 0.01f * speed;
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("FadeInCoroutine() finished...");
        SetSortingLayer(behindEverything);
        SetActive(setActiveOnComplete);
        action.actionResolved = true;

    }

    public Action FadeOut(int speed = 2, float alphaTarget = 1, bool setActiveOnComplete = false)
    {
        Action action = new Action();
        StartCoroutine(FadeOutCoroutine(speed, alphaTarget, setActiveOnComplete, action));
        return action;
    }
    public IEnumerator FadeOutCoroutine(int speed, float alphaTarget, bool setActiveOnComplete, Action action)
    {
        SetActive(true);
        SetSortingLayer(aboveEverything);
        //canvasGroup.alpha = 0;

        while (canvasGroup.alpha < alphaTarget)
        {
            canvasGroup.alpha += 0.01f * speed;
            yield return new WaitForEndOfFrame();
        }

        /*
        if (EventManager.Instance.gameOverEventStarted == false)
        {
            SetSortingLayer(behindEverything);
            SetActive(false);
        }
        */
        SetActive(setActiveOnComplete);
        action.actionResolved = true;
    }

}
