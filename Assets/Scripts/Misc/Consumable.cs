using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Consumable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Component References")]
    public GameObject infoPanelParent;
    public Image myImage;
    public Image frameImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI desccriptionText;
    public CanvasGroup infoPanelCg;

    [Header("Properties")]
    public ConsumableDataSO myData;
    public ConsumableTopPanelSlot mySlot;
    public Color normalColor;
    public Color highlightColor;
    public bool fadingIn;
    public float fadeSpeed;

    // Set up + Initialization
    #region
    public void BuilFromConsumableData(ConsumableDataSO data)
    {
        myData = data;
        myImage.sprite = data.consumableSprite;
        nameText.text = data.consumableName;
        desccriptionText.text = data.consumableDescription;

    }
    #endregion

    // Mouse + Click Event Logic
    #region
    public void OnPointerClick(PointerEventData eventData)
    {
        ConsumableManager.Instance.OnConsumableClicked(this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        frameImage.color = highlightColor;
        EnableInfoPanelView();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        frameImage.color = normalColor;
        DisableInfoPanelView();
    }
    #endregion

    // View Logic
    #region
    public void EnableInfoPanelView()
    {
        infoPanelParent.SetActive(true);
        StartCoroutine(FadeInInfoPanel());
    }
    public void DisableInfoPanelView()
    {
        fadingIn = false;
        infoPanelCg.alpha = 0;
        infoPanelParent.SetActive(false);
    }
    public IEnumerator FadeInInfoPanel()
    {
        fadingIn = true;
        infoPanelCg.alpha = 0;

        while (fadingIn && infoPanelCg.alpha < 1)
        {
            infoPanelCg.alpha += 0.1f * fadeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    #endregion
}
