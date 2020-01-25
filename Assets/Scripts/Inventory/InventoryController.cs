using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
    [Header("Component References")]
    public GameObject itemsParent;
    public List<InventorySlot> inventorySlots;

    [Header("Properties")]
    public GameObject itemBeingDragged;

    // for testing
    private void Start()
    {
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
        AddItemToInventory(ItemLibrary.Instance.GetRandomCommonItem());
    }

    public void AddItemToInventory(ItemDataSO itemAdded)
    {
        Debug.Log("InventoryController.AddItemToInventory() called for " + itemAdded.Name);

        GameObject newInventoryItem = Instantiate(PrefabHolder.Instance.InventoryItem, itemsParent.transform);
        InventoryItemCard itemCard = newInventoryItem.GetComponent<InventoryItemCard>();
        PlaceItemOnInventorySlot(itemCard, GetNextAvailableSlot());
        ItemManager.Instance.SetUpInventoryItemCardFromData(itemCard, itemAdded);

    }
    public void PlaceItemOnInventorySlot(InventoryItemCard item, InventorySlot slot)
    {
        Debug.Log("InventoryController.AddItemToInventory() called...");

        item.transform.position = slot.transform.position;
        item.transform.SetParent(itemsParent.transform);

        item.equipted = false;
        item.myInventorySlot = slot;
        slot.myItemCard = item;
        slot.occupied = true;

        item.SetRayCastingState(true);
    }
    public void PlaceItemOnCharacterSlot(InventoryItemCard itemCard, CharacterItemSlot characterSlot)
    {
        itemCard.transform.position = characterSlot.transform.position;
        itemCard.myInventorySlot.occupied = false;
        itemCard.equipted = true;
        itemCard.myInventorySlot.myItemCard = null;
        itemCard.myInventorySlot = null;
        itemCard.transform.SetParent(characterSlot.transform);
        characterSlot.myItem = itemCard;        

        itemCard.SetRayCastingState(true);
    }
    public InventorySlot GetNextAvailableSlot()
    {
        Debug.Log("InventoryController.GetNextAvailableSlot() called...");

        InventorySlot slotReturned = null;

        foreach(InventorySlot slot in inventorySlots)
        {
            if(slot.occupied == false)
            {
                slotReturned = slot;
                break;
            }
        }

        if(slotReturned == null)
        {
            Debug.Log("InventoryController.GetNextAvailableSlot() could not find an unoccupied slot, returning a null slot...");
        }

        return slotReturned;
    }
    public void TryPlaceItemOnCharacterSlot(InventoryItemCard itemCard, CharacterItemSlot characterSlot)
    {
        Debug.Log("InventoryController.PlaceItemOnCharacterSlot() called, attempting to place " + 
                  itemCard.myItemData.Name + " on the " + characterSlot.mySlotType.ToString() + " slot...");

        // Prevent player from mismatching items to slots
        if (IsSlotValidForItem(itemCard, characterSlot))
        {
            // is the slot empty? If so, just add new item
            if(characterSlot.myItem == null)
            {
                Debug.Log("Slot is not occupied");
                // Apply item to character slot
                PlaceItemOnCharacterSlot(itemCard, characterSlot);

                // Apply item effects
                ItemManager.Instance.ApplyAllItemEffectsToCharacterData(characterSlot.myCharacterData, itemCard.myItemData);
            }

            // does the slot already have an item in it? (if so, replace it)
            else if(characterSlot.myItem != null)
            {
                Debug.Log("Slot is occupied, sending current slotted item back to inventory");

                // Send old item back to inventory
                PlaceItemOnInventorySlot(characterSlot.myItem, GetNextAvailableSlot());

                // Remove old item effects from character
                ItemManager.Instance.ApplyAllItemEffectsToCharacterData(characterSlot.myCharacterData, characterSlot.myItem.myItemData, true);

                // Apply new item to character slot
                PlaceItemOnCharacterSlot(itemCard, characterSlot);

                // Apply item effects
                ItemManager.Instance.ApplyAllItemEffectsToCharacterData(characterSlot.myCharacterData, itemCard.myItemData);
            }
            
        }      
        
    }
    public bool IsSlotValidForItem(InventoryItemCard item, CharacterItemSlot slot)
    {
        Debug.Log("InventoryController.IsSlotValidForItem() called for: " + item.myItemData.Name);

        // Head
        if (item.myItemData.itemType == ItemDataSO.ItemType.Head && slot.mySlotType == CharacterItemSlot.SlotType.Head)
        {
            Debug.Log(item.myItemData.Name + " is a valid Head slot item, returning true...");
            return true;
        }

        // Chest
        else if (item.myItemData.itemType == ItemDataSO.ItemType.Chest && slot.mySlotType == CharacterItemSlot.SlotType.Chest)
        {
            Debug.Log(item.myItemData.Name + " is a valid Chest slot item, returning true...");
            return true;
        }

        // Legs
        else if (item.myItemData.itemType == ItemDataSO.ItemType.Legs && slot.mySlotType == CharacterItemSlot.SlotType.Legs)
        {
            Debug.Log(item.myItemData.Name + " is a valid Legs slot item, returning true...");
            return true;
        }

        // Main Hand        
        else if (
            (item.myItemData.itemType == ItemDataSO.ItemType.MeleeOneHand && slot.mySlotType == CharacterItemSlot.SlotType.MainHand) ||
            (item.myItemData.itemType == ItemDataSO.ItemType.MeleeTwoHand && slot.mySlotType == CharacterItemSlot.SlotType.MainHand) ||
            (item.myItemData.itemType == ItemDataSO.ItemType.RangedTwoHand && slot.mySlotType == CharacterItemSlot.SlotType.MainHand)
            )
        {
            // TO DO!: after character data updated to hold info about items, make code here that checks the character data for a 
            // main hand weapon. If they dont have a main hand weapon, they cannot put a weapon in the offhand slot

            Debug.Log(item.myItemData.Name + " is a valid Main Hand slot item, returning true...");
            return true;
        }

        // Off Hand
        else if (
            (item.myItemData.itemType == ItemDataSO.ItemType.MeleeOneHand || 
             item.myItemData.itemType == ItemDataSO.ItemType.Offhand ||
             item.myItemData.itemType == ItemDataSO.ItemType.Shield) && 
             slot.mySlotType == CharacterItemSlot.SlotType.OffHand)
        {
            // TO DO!: after character data updated to hold info about items, make code here that checks the character data for a 
            // main hand weapon. If they dont have a main hand weapon, they cannot put a weapon in the offhand slot

            Debug.Log(item.myItemData.Name + " is a valid Off Hand slot item, returning true...");
            return true;
        }
        else
        {
            Debug.Log(item.myItemData.Name + " is NOT valid in the " + slot.mySlotType.ToString() + " slot, returning false...");
            return false;
        }
    }
}
