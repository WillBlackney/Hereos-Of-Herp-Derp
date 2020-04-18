using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterItemSlot : MonoBehaviour, IDropHandler
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

        InventoryItemCard draggedItem = InventoryController.Instance.itemBeingDragged.GetComponent<InventoryItemCard>();

        if (draggedItem && InventoryController.Instance.itemBeingDragged)
        {
            InventoryController.Instance.TryPlaceItemOnCharacterSlot(draggedItem, this);
            InventoryController.Instance.itemBeingDragged = null;
        }
    
    }
    
}
