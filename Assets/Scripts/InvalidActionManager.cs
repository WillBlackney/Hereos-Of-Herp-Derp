using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InvalidActionManager : MonoBehaviour
{
    [Header("Component References")]
    public GameObject visualParent;
    public GameObject canvasParent;
    public TextMeshProUGUI reasonText;
    public CanvasGroup cg;

    [Header("Properties")]
    public bool activeMessage;
    public float fadeInSpeed;
    public float fadeOutSpeed;

    // Singleton Pattern
    #region
    public static InvalidActionManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public void ShowNewErrorMessage(string reason)
    {
        StopAllCoroutines();
        ResetMessageViews();
        StartCoroutine(ShowNewErrorMessageCoroutine(reason));
    }
    private IEnumerator ShowNewErrorMessageCoroutine(string reason)
    {
        //ResetMessageViews();
        EnableVisualParent();
        SetReasonText(reason);
        activeMessage = true;

        while (activeMessage)
        {
            // fade in view
            while(cg.alpha < 1)
            {
                cg.alpha += 1 * fadeInSpeed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            // pause, let player view message
            yield return new WaitForSeconds(3);

            // fade out view
            while (cg.alpha > 0)
            {
                cg.alpha -= 1 * fadeOutSpeed * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            activeMessage = false;
        }       

    }

    public void EnableVisualParent()
    {
        canvasParent.SetActive(true);
        visualParent.SetActive(true);
    }
    public void DisableVisualParent()
    {
        canvasParent.SetActive(false);
        visualParent.SetActive(false);
    }
    public void SetReasonText(string reason)
    {
        reasonText.text = reason;
    }
    public void ResetMessageViews()
    {
        activeMessage = false;
        DisableVisualParent();
        cg.alpha = 0;        
    }

}
