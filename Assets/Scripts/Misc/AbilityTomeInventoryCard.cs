using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityTomeInventoryCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Properties + Component References
    #region
    [Header("Component References")]    
    public Image bookImage;
    public AbilityInfoSheet abilityInfoSheet;

    [Header("Properties")]
    public InventorySlot myInventorySlot;
    public AbilityDataSO myData;
    #endregion

    // Initialization + Setup
    #region
    public void BuildFromAbilityData(AbilityDataSO data)
    {
        myData = data;

        AbilityInfoSheetController.Instance.BuildSheetFromData(abilityInfoSheet, data, AbilityInfoSheet.PivotDirection.Downwards);
       
        SetBookImage();
    }    
    public void SetBookImage()
    {
        if (myData.abilitySchool == AbilityDataSO.AbilitySchool.None)
        {
            bookImage.sprite = InventoryController.Instance.neutralBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Brawler)
        {
            bookImage.sprite = InventoryController.Instance.brawlerBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Duelist)
        {
            bookImage.sprite = InventoryController.Instance.duelistBookImage; 
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Assassination)
        {
            bookImage.sprite = InventoryController.Instance.assassinationBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Guardian)
        {
            bookImage.sprite = InventoryController.Instance.guardianBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Pyromania)
        {
            bookImage.sprite = InventoryController.Instance.pyromaniaBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Cyromancy)
        {
            bookImage.sprite = InventoryController.Instance.cyromancyBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Ranger)
        {
            bookImage.sprite = InventoryController.Instance.rangerBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Manipulation)
        {
            bookImage.sprite = InventoryController.Instance.manipulationBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Divinity)
        {
            bookImage.sprite = InventoryController.Instance.divinityBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Shadowcraft)
        {
            bookImage.sprite = InventoryController.Instance.shadowcraftBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Corruption)
        {
            bookImage.sprite = InventoryController.Instance.corruptionBookImage;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Naturalism)
        {
            bookImage.sprite = InventoryController.Instance.naturalismBookImage;
        }
    }
    #endregion

    // View Logic
    #region
    public void SetRayCastingState(bool onOrOff)
    {
        Debug.Log("InventoryItemCard.SetRayCastingState() called...");
        bookImage.raycastTarget = onOrOff;
    }
    public void EnableInfoPanelView()
    {
        AbilityInfoSheetController.Instance.EnableSheetView(abilityInfoSheet, true, true);
    }
    public void DisableInfoPanelView()
    {
        AbilityInfoSheetController.Instance.DisableSheetView(abilityInfoSheet);
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
        transform.position = CameraManager.Instance.unityCamera.mainCamera.ScreenToWorldPoint(screenPoint);
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
