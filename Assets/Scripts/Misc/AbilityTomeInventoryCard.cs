using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityTomeInventoryCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("General Properties + Components")]
    public Image bgColorImage;
    public AbilityDataSO myData;
    public InventorySlot myInventorySlot;    

    [Header("Info Panel Component References")]
    public GameObject infoPanelVisualParent;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI energyCostText;


    // Initialization + Setup
    #region
    public void BuildFromAbilityData(AbilityDataSO data)
    {
        myData = data;

        // build text and images assets
        nameText.text = data.abilityName;
        TextLogic.SetAbilityDescriptionText(data, descriptionText);
        cooldownText.text = data.baseCooldownTime.ToString();
        rangeText.text = data.range.ToString();
        energyCostText.text = data.energyCost.ToString();

        SetBackgroundColor();
    }
    public void SetBackgroundColor()
    {
        if (myData.abilitySchool == AbilityDataSO.AbilitySchool.None)
        {
            bgColorImage.color = InventoryController.Instance.neutralColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Brawler)
        {
            bgColorImage.color = InventoryController.Instance.brawlerColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Duelist)
        {
            bgColorImage.color = InventoryController.Instance.duelistColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Assassination)
        {
            bgColorImage.color = InventoryController.Instance.assassinationColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Guardian)
        {
            bgColorImage.color = InventoryController.Instance.guardianColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Pyromania)
        {
            bgColorImage.color = InventoryController.Instance.pyromaniaColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Cyromancy)
        {
            bgColorImage.color = InventoryController.Instance.cyromancyColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Ranger)
        {
            bgColorImage.color = InventoryController.Instance.rangerColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Manipulation)
        {
            bgColorImage.color = InventoryController.Instance.manipulationColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Divinity)
        {
            bgColorImage.color = InventoryController.Instance.divinityColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Shadowcraft)
        {
            bgColorImage.color = InventoryController.Instance.shadowcraftColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Corruption)
        {
            bgColorImage.color = InventoryController.Instance.corruptionColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Naturalism)
        {
            bgColorImage.color = InventoryController.Instance.naturalismColor;
        }
    }
    #endregion

    // View Logic
    #region
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
    #endregion

    // Drag + Mouse Event Logic
    #region
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("AbilityTomeInventoryCard.OnBeginDrag() called...");

        InventoryController.Instance.itemBeingDragged = gameObject;
        SetRayCastingState(false);
        DisableInfoPanelView();
    }
    public void OnDrag(PointerEventData eventData)
    {
        var screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        screenPoint.z = 10.0f;
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (myInventorySlot != null)
        {
            transform.position = myInventorySlot.transform.position;
        }

        SetRayCastingState(true);
        InventoryController.Instance.itemBeingDragged = null;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // dont display item cards of other items will an item is being dragged
        if (InventoryController.Instance.itemBeingDragged == null)
        {
            EnableInfoPanelView();
        }
            
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        DisableInfoPanelView();
    }
    #endregion
}
