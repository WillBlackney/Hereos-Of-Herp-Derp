using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CampSiteButton : MonoBehaviour
{
    [Header("Component References")]
    public GameObject buttonImageParent;
    public GameObject glowOutlineParent;
    public GameObject infoPanelParent;
    public TextMeshProUGUI costText;

    [Header("Properties")]
    public string buttonName;
    private float buttonScale;
    private bool shrinking;
    private bool expanding;
    public int expandSpeed;

    // Initialization + Setup
    #region
    private void Start()
    {
        buttonScale = buttonImageParent.GetComponent<RectTransform>().localScale.x;
    }
    #endregion

    // Mouse + Pointer Events
    #region
    
    public void OnMouseEnter()
    {
        Debug.Log("CampSiteButton.OnMouseEnter() called");
        StartCoroutine(Expand(expandSpeed));
    }
    public void OnMouseExit()
    {
        Debug.Log("CampSiteButton.OnMouseExit() called");
        StartCoroutine(Shrink(expandSpeed));
    }
    public void OnMouseDown()
    {
        Debug.Log("CampSiteButton.OnMouseDown() called");
        CampSiteManager.Instance.OnCampSiteButtonClicked(buttonName);
    }
    #endregion

    // View + VFX logic
    public IEnumerator Expand(int speed)
    {
        shrinking = false;
        expanding = true;

        float finalScale = buttonScale * 1.2f;
        RectTransform transform = buttonImageParent.GetComponent<RectTransform>();

        while (transform.localScale.x < finalScale && expanding == true)
        {
            Vector3 targetScale = new Vector3(transform.localScale.x + (0.1f * speed * Time.deltaTime), transform.localScale.y + (0.1f * speed * Time.deltaTime));
            transform.localScale = targetScale;
            yield return new WaitForEndOfFrame();
        }

        if (transform.localScale.x >= finalScale)
        {
            EnableInfoPanelView();
        }

    }
    public IEnumerator Shrink(int speed)
    {
        expanding = false;
        shrinking = true;
        DisableInfoPanelView();

        RectTransform transform = buttonImageParent.GetComponent<RectTransform>();

        while (transform.localScale.x > buttonScale && shrinking == true)
        {
            Vector3 targetScale = new Vector3(transform.localScale.x - (0.1f * speed * Time.deltaTime), transform.localScale.y - (0.1f * speed * Time.deltaTime));
            transform.localScale = targetScale;
            yield return new WaitForEndOfFrame();
        }
    }
    public void EnableGlowAnimation()
    {
        glowOutlineParent.SetActive(true);
        glowOutlineParent.GetComponent<CanvasGroup>().alpha = 0;
        glowOutlineParent.GetComponent<Animator>().SetTrigger("Glow");
    }
    public void DisableGlowAnimation()
    {
        glowOutlineParent.SetActive(false);
        //glowOutlineParent.GetComponent<CanvasGroup>().alpha = 0;
        //glowOutlineParent.GetComponent<Animator>().SetTrigger("Glow");
    }
    public void EnableInfoPanelView()
    {
        infoPanelParent.SetActive(true);
    }
    public void DisableInfoPanelView()
    {
        infoPanelParent.SetActive(false);
    }


    // Other Logic
    #region
    public void SetActionCostText(int newCost)
    {
        costText.text = newCost.ToString();
    }

    #endregion

    public void SetGlowOutilineViewState(bool onOrOff)
    {
        /*
        if(myGlowOutline != null)
        {
            myGlowOutline.gameObject.SetActive(onOrOff);
        }
        */
        
    }

    
}
