using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CampSiteButton : MonoBehaviour
{
    public Image myGlowOutline;
    public string buttonName;

    public void OnMouseEnter()
    {
        Debug.Log("OnMouseEnter called");
    }
    public void OnMouseExit()
    {
        Debug.Log("OnMouseExit called");
    }
    public void OnMouseDown()
    {
        Debug.Log("CampSiteButton.OnMouseDown() called");
        CampSiteManager.Instance.OnCampSiteButtonClicked(buttonName);
    }

    public void SetGlowOutilineViewState(bool onOrOff)
    {
        if(myGlowOutline != null)
        {
            myGlowOutline.gameObject.SetActive(onOrOff);
        }
        
    }

}
