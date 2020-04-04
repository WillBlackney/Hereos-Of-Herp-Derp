using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ConsumableInShop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Component References")]
    public GameObject visualParent;
    public GameObject infoPanelParent;
    public Image myImage;
    public Image frameImage;
    public TextMeshProUGUI goldCostText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    [Header("Properties")]
    public int goldCost;
    public ConsumableDataSO myData;
    public Color normalColor;
    public Color highlightColor;

    public void BuildFromData(ConsumableDataSO data)
    {
        myData = data;
        myImage.sprite = data.consumableSprite;
        goldCost = Random.Range(5, 11);
        goldCostText.text = goldCost.ToString();
        descriptionText.text = data.consumableDescription;
        nameText.text = data.consumableName;

        EnableSlotView();
    }

    // View Logic
    #region
    public void EnableInfoPanelView()
    {
        infoPanelParent.SetActive(true);
    }
    public void DisableInfoPanelView()
    {
        infoPanelParent.SetActive(false);
    }
    public void DisableSlotView()
    {
        visualParent.SetActive(false);
    }
    public void EnableSlotView()
    {
        visualParent.SetActive(true);
    }
    #endregion

    // Mouse + Click Events
    #region
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ConsumableInShop.OnPointerClick() called...");
        ConsumableManager.Instance.BuyConsumableFromShop(this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ConsumableInShop.OnPointerEnter() called...");
        frameImage.color = highlightColor;
        EnableInfoPanelView();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("ConsumableInShop.OnPointerExit() called...");
        frameImage.color = normalColor;
        DisableInfoPanelView();
    }
    #endregion

    
}
