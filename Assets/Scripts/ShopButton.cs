using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Component References")]
    public GameObject buttonImageParent;
    public RectTransform buttonImageParentTransform;

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
        buttonScale = buttonImageParentTransform.localScale.x;
    }
    #endregion

    // Mouse + Pointer Click Events
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ShopButton.OnPointerEnter() called");
        StartCoroutine(Expand(expandSpeed));
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("ShopButton.OnPointerExit() called");
        StartCoroutine(Shrink(expandSpeed));
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ShopButton.OnPointerClick() called");

        if(buttonName == "Armoury")
        {
            ShopScreenManager.Instance.EnableArmouryScreenView();
            StartCoroutine(Shrink(expandSpeed));
        }
        else if (buttonName == "Potion Lab")
        {
            ShopScreenManager.Instance.EnablePotionLabScreenView();
            StartCoroutine(Shrink(expandSpeed));
        }
        else if (buttonName == "Library")
        {
            ShopScreenManager.Instance.EnableLibraryScreenView();
            StartCoroutine(Shrink(expandSpeed));
        }
    }
    #endregion

    // View + VFX logic
    #region
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

    }
    public IEnumerator Shrink(int speed)
    {
        expanding = false;
        shrinking = true;

        RectTransform transform = buttonImageParent.GetComponent<RectTransform>();

        while (transform.localScale.x > buttonScale && shrinking == true)
        {
            Vector3 targetScale = new Vector3(transform.localScale.x - (0.1f * speed * Time.deltaTime), transform.localScale.y - (0.1f * speed * Time.deltaTime));
            transform.localScale = targetScale;
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
