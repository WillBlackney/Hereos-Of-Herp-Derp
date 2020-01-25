using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterItemSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler
{
    public enum SlotType { None, Head, Legs, Chest, MainHand, OffHand};

    [Header("Properties")]
    public SlotType mySlotType;
    public CharacterData myCharacterData;
    public InventoryItemCard myItem;
    public bool occupied;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("CharacterItemSlot.OnDrop() called...");
        InventoryController.Instance.TryPlaceItemOnCharacterSlot
            (InventoryController.Instance.itemBeingDragged.GetComponent<InventoryItemCard>(), this);
        InventoryController.Instance.itemBeingDragged = null;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("CharacterItemSlot.OnPointerEnter() called...");
    }
}
