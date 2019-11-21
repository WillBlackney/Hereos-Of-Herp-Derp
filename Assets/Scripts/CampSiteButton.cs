using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampSiteButton : MonoBehaviour
{
    public Image myGlowOutline;
    public CanvasGroup myGlowCG;

    public void SetGlowOutilineViewState(bool onOrOff)
    {
        myGlowOutline.gameObject.SetActive(onOrOff);
    }

}
