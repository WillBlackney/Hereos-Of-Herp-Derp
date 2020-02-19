using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityTomeInventoryCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public AbilityDataSO myData;
    public GameObject infoPanelVisualParent;
    public InventorySlot myInventorySlot;
    public Image bgColorImage;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI energyCostText;

    public void BuildFromAbilityData(AbilityDataSO data)
    {
        myData = data;

        // build text and images assets
        nameText.text = data.abilityName;
        TextLogic.SetAbilityDescriptionText(data, descriptionText);
        cooldownText.text = data.baseCooldownTime.ToString();
        rangeText.text = data.range.ToString();
        energyCostText.text = data.energyCost.ToString();
    }
    public void SetRayCastingState(bool onOrOff)
    {
        Debug.Log("InventoryItemCard.SetRayCastingState() called...");
        bgColorImage.raycastTarget = onOrOff;
    }
    public void EnableInfoPanelView()
    {
        infoPanelVisualParent.SetActive(true);
    }
    public void DisableInfoPanelView()
    {
        infoPanelVisualParent.SetActive(false);
    }


    // Drag + Mouse Event Logic
    #region
    public void OnBeginDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
    public void OnDrag(PointerEventData eventData)
    {
       // throw new System.NotImplementedException();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
       // throw new System.NotImplementedException();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        EnableInfoPanelView();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        DisableInfoPanelView();
    }
    #endregion
}
