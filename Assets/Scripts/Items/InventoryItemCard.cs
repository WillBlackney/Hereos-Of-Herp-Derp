using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItemCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Component References")]
    public Image itemImage;
    public GameObject infoItemCardVisualParent;
    public ItemCard myInfoItemCard;

    [Header("Properties")]
    public InventorySlot myInventorySlot;
    public ItemDataSO myItemData;
    public bool equipted;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("InventoryItemCard.OnBeginDrag() called...");

        // prevent player dragging equipted items
        if (!equipted)
        {            
            InventoryController.Instance.itemBeingDragged = gameObject;
            SetRayCastingState(false);
            infoItemCardVisualParent.SetActive(false);
        }
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        // Follow mouse position

        // prevent player dragging equipted items
        if (!equipted)
        {
            var screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            screenPoint.z = 10.0f;
            transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        }
        
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("InventoryItemCard.OnEndDrag() called...");

        // prevent player dragging equipted items
        if (!equipted)
        {
            if (myInventorySlot != null)
            {
                transform.position = myInventorySlot.transform.position;
            }

            SetRayCastingState(true);
            InventoryController.Instance.itemBeingDragged = null;
        }
        
    }

    public void OnMouseEnter()
    {
        // dont display item cards of other items will an item is being dragged
        if (InventoryController.Instance.itemBeingDragged == null)
        {
            infoItemCardVisualParent.SetActive(true);
            //
            ItemCardPanelHover.Instance.OnItemCardMousedOver(myInfoItemCard);
        }
    }
    public void OnMouseExit()
    {
        ItemCardPanelHover.Instance.OnItemCardMouseExit();
        infoItemCardVisualParent.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        /*
        // dont display item cards of other items will an item is being dragged
        if(InventoryController.Instance.itemBeingDragged == null)
        {
            infoItemCardVisualParent.SetActive(true);
            //
            ItemCardPanelHover.Instance.OnItemCardMousedOver(myInfoItemCard);
        }
        */
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        /*
        ItemCardPanelHover.Instance.OnItemCardMouseExit();
        infoItemCardVisualParent.SetActive(false);
        */
    }
    public void SetRayCastingState(bool onOrOff)
    {
        Debug.Log("InventoryItemCard.SetRayCastingState() called...");
        itemImage.raycastTarget = onOrOff;
    }
}
