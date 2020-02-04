using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampSiteButton : MonoBehaviour
{
    public Image myGlowOutline;

    public void SetGlowOutilineViewState(bool onOrOff)
    {
        myGlowOutline.gameObject.SetActive(onOrOff);
    }

}
