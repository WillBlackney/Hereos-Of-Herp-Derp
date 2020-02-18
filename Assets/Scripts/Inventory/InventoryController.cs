using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("Component References")]
    public GameObject itemsParent;
    public List<InventorySlot> inventorySlots;

    [Header("Properties")]
    public GameObject itemBeingDragged;

    public static InventoryController Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void AddItemToInventory(ItemDataSO itemAdded)
    {
        Debug.Log("InventoryController.AddItemToInventory() called for " + itemAdded.Name);

        GameObject newInventoryItem = Instantiate(PrefabHolder.Instance.InventoryItem, itemsParent.transform);
        InventoryItemCard itemCard = newInventoryItem.GetComponent<InventoryItemCard>();
        PlaceItemOnInventorySlot(itemCard, GetNextAvailableSlot());
        ItemManager.Instance.SetUpInventoryItemCardFromData(itemCard, itemAdded);

    }
    public void CreateAndAddItemDirectlyToCharacter(ItemDataSO itemAdded, CharacterItemSlot weaponSlot)
    {
        // Method used to set characters up with default items. Will be used later again when character presets are set up
        Debug.Log("InventoryController.AddItemToInventory() called for " + itemAdded.Name);

        GameObject newInventoryItem = Instantiate(PrefabHolder.Instance.InventoryItem, itemsParent.transform);
        InventoryItemCard itemCard = newInventoryItem.GetComponent<InventoryItemCard>();

        ItemManager.Instance.SetUpInventoryItemCardFromData(itemCard, itemAdded);
        PlaceItemOnCharacterSlot(itemCard, weaponSlot);

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
        if(itemCard.myInventorySlot != null)
        {
            itemCard.myInventorySlot.occupied = false;
            itemCard.myInventorySlot.myItemCard = null;
            itemCard.myInventorySlot = null;
        }
        
        itemCard.equipted = true;       
        itemCard.transform.SetParent(characterSlot.transform);
        characterSlot.myItem = itemCard;  
        
        // Weapon specific set up
        if(characterSlot.mySlotType == CharacterItemSlot.SlotType.MainHand)
        {
            characterSlot.myCharacterData.mainHandWeapon = itemCard.myItemData;
        }
        else if (characterSlot.mySlotType == CharacterItemSlot.SlotType.OffHand)
        {
            characterSlot.myCharacterData.offHandWeapon = itemCard.myItemData;
        }

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
            // check if player is adding a 1h to the off hand slot while a 2h weapon is equipted, if so, prevent this
            if((characterSlot.myCharacterData.mainHandSlot.myItem.myItemData.itemType == ItemDataSO.ItemType.MeleeTwoHand ||
                characterSlot.myCharacterData.mainHandSlot.myItem.myItemData.itemType == ItemDataSO.ItemType.MeleeTwoHand) && 
                characterSlot.mySlotType == CharacterItemSlot.SlotType.OffHand
                )
            {
                Debug.Log("InventoryController.PlaceItemOnCharacterSlot() detected player attempting to place item in off hand slot while 2h weapon equipted," +
                    " cancelling item placement");
                return;
            }

            // is the slot empty? If so, just add new item
            if(characterSlot.myItem == null)
            {
                Debug.Log("Slot is not occupied");
                // Apply item to character slot
                PlaceItemOnCharacterSlot(itemCard, characterSlot);

                // Check if weapon being added is a 2h weapon. If it is, also remove the off hand weapon (cant duel wield 2h weapons)
                if (
                    (itemCard.myItemData.itemType == ItemDataSO.ItemType.MeleeTwoHand ||
                    itemCard.myItemData.itemType == ItemDataSO.ItemType.RangedTwoHand) &&
                    characterSlot.myCharacterData.offHandSlot.myItem != null
                    )
                {
                    PlaceItemOnInventorySlot(characterSlot.myCharacterData.offHandSlot.myItem, GetNextAvailableSlot());

                    // Remove old item effects from character
                    ItemManager.Instance.ApplyAllItemEffectsToCharacterData(characterSlot.myCharacterData, characterSlot.myCharacterData.offHandSlot.myItem.myItemData, true);

                    // null off hand slot
                    characterSlot.myCharacterData.offHandSlot.myItem = null;
                }

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

                // Check if weapon being added is a 2h weapon. If it is, also remove the off hand weapon (cant duel wield 2h weapons)
                if(
                    (itemCard.myItemData.itemType == ItemDataSO.ItemType.MeleeTwoHand || 
                    itemCard.myItemData.itemType == ItemDataSO.ItemType.RangedTwoHand) &&
                    characterSlot.myCharacterData.offHandSlot.myItem != null
                    )
                {
                    PlaceItemOnInventorySlot(characterSlot.myCharacterData.offHandSlot.myItem, GetNextAvailableSlot());

                    // Remove old item effects from character
                    ItemManager.Instance.ApplyAllItemEffectsToCharacterData(characterSlot.myCharacterData, characterSlot.myCharacterData.offHandSlot.myItem.myItemData, true);

                    // null off hand slot
                    characterSlot.myCharacterData.offHandSlot.myItem = null;
                }

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
