using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CanvasGroup myButtonCG;
    public bool fadingIn;
    public bool fadingOut;
    public IEnumerator FadeIn()
    {
        fadingOut = false;
        fadingIn = true;

        while (myButtonCG.alpha < 1 && fadingIn)
        {
            myButtonCG.alpha += 0.2f;            
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator FadeOut()
    {
        fadingIn = false;
        fadingOut = true;        

        while (myButtonCG.alpha > 0 && fadingOut)
        {
            myButtonCG.alpha -= 0.2f;
            yield return new WaitForEndOfFrame();
        }
    }

    // Mouse + Click Logic
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(FadeIn());
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(FadeOut());
    }
    #endregion
}
