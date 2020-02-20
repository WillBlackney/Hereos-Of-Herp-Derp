using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityTomeDropSlot : MonoBehaviour, IDropHandler
{
    public CharacterData myCharacter;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("AbilityTomeDropSlot.OnDrop() called...");

        AbilityTomeInventoryCard draggedTome = InventoryController.Instance.itemBeingDragged.GetComponent<AbilityTomeInventoryCard>();

        if (draggedTome)
        {
            InventoryController.Instance.TryPlaceAbilityTomeOnDropSlot(draggedTome, this);
            InventoryController.Instance.itemBeingDragged = null;
        }
        
    }
}
