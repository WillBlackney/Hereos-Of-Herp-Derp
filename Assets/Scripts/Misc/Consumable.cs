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
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI desccriptionText;

    [Header("Properties")]
    public ConsumableDataSO myData;
    public ConsumableTopPanelSlot mySlot;

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
        EnableInfoPanelView();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        DisableInfoPanelView();
    }
    #endregion

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

    #endregion
}
