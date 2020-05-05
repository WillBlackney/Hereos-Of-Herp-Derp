using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class StoryEventButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Properties + Component References
    #region
    [Header("Component References")]
    public TextMeshProUGUI actionNameText;
    public TextMeshProUGUI actionDescriptionText;
    public RectTransform myRectTransform;

    [Header("Properties")]
    public bool shrinking;
    public bool expanding;
    public float originalScale;
    #endregion

    // Initialization + Setup
    #region
    public void SetUpMyComponents(string name, string description)
    {
        // Set Text
        actionNameText.text = name;
        actionDescriptionText.text = description;

        // Reset button scale
        myRectTransform.localScale = new Vector2(originalScale, originalScale);
    }
    #endregion

    // Mouse + Input related events
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(Expand(1));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(Shrink(1));
    }
    #endregion
       
    // Visibility + View Logic
    #region
    public IEnumerator Expand(int speed)
    {
        shrinking = false;
        expanding = true;

        float finalScale = originalScale * 1.2f;

        while (myRectTransform.localScale.x < finalScale && expanding == true)
        {
            Vector3 targetScale = new Vector3(myRectTransform.localScale.x + (1 * speed * Time.deltaTime), myRectTransform.localScale.y + (1 * speed * Time.deltaTime));
            myRectTransform.localScale = targetScale;
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator Shrink(int speed)
    {
        expanding = false;
        shrinking = true;

        while (myRectTransform.localScale.x > originalScale && shrinking == true)
        {
            Vector3 targetScale = new Vector3(myRectTransform.localScale.x - (1 * speed * Time.deltaTime), myRectTransform.localScale.y - (1 * speed * Time.deltaTime));
            myRectTransform.localScale = targetScale;
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
