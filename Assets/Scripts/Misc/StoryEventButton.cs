using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StoryEventButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Component References")]
    public TextMeshProUGUI actionNameText;
    public TextMeshProUGUI actionDescriptionText;
    public RectTransform myRectTransform;

    [Header("Properties")]
    public bool shrinking;
    public bool expanding;
    public float originalScale;    


    // Initialization + Setup
    #region
    public void SetUpMyComponents(string name, string description)
    {
        actionNameText.text = name;
        actionDescriptionText.text = description;

        // reset button scale
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
        RectTransform transform = GetComponent<RectTransform>();

        while (transform.localScale.x < finalScale && expanding == true)
        {
            Vector3 targetScale = new Vector3(transform.localScale.x + (1 * speed * Time.deltaTime), transform.localScale.y + (1 * speed * Time.deltaTime));
            transform.localScale = targetScale;
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator Shrink(int speed)
    {
        expanding = false;
        shrinking = true;

        RectTransform transform = GetComponent<RectTransform>();

        while (transform.localScale.x > originalScale && shrinking == true)
        {
            Vector3 targetScale = new Vector3(transform.localScale.x - (1 * speed * Time.deltaTime), transform.localScale.y - (1 * speed * Time.deltaTime));
            transform.localScale = targetScale;
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
