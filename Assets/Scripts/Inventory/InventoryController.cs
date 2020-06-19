using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // Properties + Components
    #region
    [Header("Component References")]
    public GameObject visualParent;
    public GameObject canvasParent;
    public GameObject itemsParent;
    public List<InventorySlot> inventorySlots;

    [Header("Properties")]
    public GameObject itemBeingDragged;
    public int inventoryItemCardSortingOrder;

    [Header("Ability Tome Images")]
    public Sprite neutralBookImage;
    public Sprite brawlerBookImage;
    public Sprite duelistBookImage;
    public Sprite assassinationBookImage;
    public Sprite guardianBookImage;
    public Sprite pyromaniaBookImage;
    public Sprite cyromancyBookImage;
    public Sprite rangerBookImage;
    public Sprite manipulationBookImage;
    public Sprite divinityBookImage;
    public Sprite shadowcraftBookImage;
    public Sprite corruptionBookImage;
    public Sprite naturalismBookImage;
    #endregion

    // Singleton Set up
    #region
    public static InventoryController Instance;
    private void Awake()
    {
        Instance = this;        
    }   

    #endregion

    // Conditional Checks
    #region
    public bool IsTomeDropActionValid(AbilityDataSO data, CharacterData character)
    {
        Debug.Log("InventoryController.IsTomeDropActionValid() called, checking validity of placing " +
            data.abilityName + " tome on " + character.myName + "'s drop slot...");

        bool boolReturned = true;

        // check if character has already learnt the ability
        if (character.DoesCharacterAlreadyKnowAbility(data))
        {
            InvalidActionManager.Instance.ShowNewErrorMessage("Character already knows " + data.abilityName);
            Debug.Log("Tome drop action invalid: " + character.myName + " already knows " + data.abilityName);
            boolReturned = false;
        }

        // check that character meets tier requirments
        else if (!character.DoesCharacterMeetAbilityTierRequirment(data))
        {
            Debug.Log("Tome drop action invalid: " + character.myName + " does not meet ability tier requirmens of "
                + data.abilityName);
            InvalidActionManager.Instance.ShowNewErrorMessage("Character does not meet the talent requirments of " + data.abilityName);
            boolReturned = false;
        }

        // return evaluation
        return boolReturned;
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
            InvalidActionManager.Instance.ShowNewErrorMessage(item.myItemData.Name + " is not a valid item for the " + slot.mySlotType.ToString() + " slot.");
            return false;
        }
    }
    #endregion    

    // Ability Tome Logic
    #region
    public void AddAbilityTomeToInventory(AbilityDataSO abilityData, bool showOverlayCardEffect = false)
    {
        // Create game object
        GameObject newAbilityTomeGO = Instantiate(PrefabHolder.Instance.AbilityTomeInventoryCard, itemsParent.transform);
        AbilityTomeInventoryCard abilityTome = newAbilityTomeGO.GetComponent<AbilityTomeInventoryCard>();

        // Enable char roster sparkle
        UIManager.Instance.SetCharacterRosterButtonParticleViewState(true);

        // play screen overlay VFX
        if (showOverlayCardEffect)
        {
            CardRewardScreenManager.Instance.CreateAbilityCardRewardEffect(abilityData);
        }

        // Place in inventory
        PlaceAbilityTomeOnInventorySlot(abilityTome, GetNextAvailableSlot());

        // Initialize tome
        abilityTome.BuildFromAbilityData(abilityData);
    }
    public void PlaceAbilityTomeOnInventorySlot(AbilityTomeInventoryCard abilityCard, InventorySlot slot)
    {
        Debug.Log("InventoryController.AddItemToInventory() called...");

        abilityCard.transform.position = slot.transform.position;
        abilityCard.transform.SetParent(itemsParent.transform);

        abilityCard.myInventorySlot = slot;
        slot.myAbilityTomeCard = abilityCard;
        slot.occupied = true;

        abilityCard.SetRayCastingState(true);
    }
    public void TryPlaceAbilityTomeOnDropSlot(AbilityTomeInventoryCard tome, AbilityTomeDropSlot dropSlot)
    {
        Debug.Log("InventoryController.TryPlaceAbilityTomeOnDropSlot() called, attempting to teach " +
                 tome.myData.abilityName + " to " + dropSlot.myCharacter.myName);

        if (IsTomeDropActionValid(tome.myData, dropSlot.myCharacter))
        {
            dropSlot.myCharacter.HandleLearnAbility(tome.myData);
            tome.myInventorySlot.occupied = false;
            tome.myInventorySlot.myAbilityTomeCard = null;
            Destroy(tome.gameObject);
        }
    }
    public void BuyAbilityTomeFromShop(AbilityTomeInShop tome)
    {
        Debug.Log("InventoryController.BuyAbilityTomeFromShop() called...");

        if (PlayerDataManager.Instance.currentGold >= tome.goldCost)
        {
            Debug.Log("Buying Ability Tome " + tome.myData.abilityName + " for " + tome.goldCost.ToString());
            PlayerDataManager.Instance.ModifyGold(-tome.goldCost);
            tome.DisableSlotView();
            AddAbilityTomeToInventory(tome.myData, true);
        }
        else
        {
            Debug.Log("Cannot buy ability tome: Not enough gold...");
            InvalidActionManager.Instance.ShowNewErrorMessage("You don't have enough Gold");
        }
    }
    #endregion

    // Add Item Logic
    #region
    public void AddItemToInventory(ItemDataSO itemAdded, bool playCardRewardOverlayEffect = false)
    {
        Debug.Log("InventoryController.AddItemToInventory() called for " + itemAdded.Name);

        // Enable char roster sparkle
        UIManager.Instance.SetCharacterRosterButtonParticleViewState(true);

        // Play screen overlay VFX
        if (playCardRewardOverlayEffect)
        {
            CardRewardScreenManager.Instance.CreateItemCardRewardEffect(itemAdded);
        }

        // Modify player score
        ScoreManager.Instance.itemsCollected++;
        if(itemAdded.itemRarity == ItemDataSO.ItemRarity.Epic)
        {
            ScoreManager.Instance.epicItemsCollected++;
        }

        // Create item
        GameObject newInventoryItem = Instantiate(PrefabHolder.Instance.InventoryItem, itemsParent.transform);
        InventoryItemCard itemCard = newInventoryItem.GetComponent<InventoryItemCard>();

        // Place item in inventory
        PlaceItemOnInventorySlot(itemCard, GetNextAvailableSlot());
        ItemManager.Instance.SetUpInventoryItemCardFromData(itemCard, itemAdded, inventoryItemCardSortingOrder);

    }
    public void CreateAndAddItemDirectlyToCharacter(ItemDataSO itemAdded, CharacterItemSlot weaponSlot)
    {
        // Method used to set characters up with default items. Will be used later again when character presets are set up
        Debug.Log("InventoryController.AddItemToInventory() called for " + itemAdded.Name);

        GameObject newInventoryItem = Instantiate(PrefabHolder.Instance.InventoryItem, itemsParent.transform);
        InventoryItemCard itemCard = newInventoryItem.GetComponent<InventoryItemCard>();
        itemCard.transform.localScale = new Vector3(1, 1, 1);

        ItemManager.Instance.SetUpInventoryItemCardFromData(itemCard, itemAdded, inventoryItemCardSortingOrder);
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
        Debug.Log("InventoryController.PlaceItemOnCharacterSlot() called, placing item " + itemCard.myItemData.Name +
            " on character slot " + characterSlot.mySlotType.ToString());

        itemCard.transform.position = characterSlot.transform.position;
        itemCard.transform.localScale = new Vector3(1, 1, 1);
        if (itemCard.myInventorySlot != null)
        {
            itemCard.myInventorySlot.occupied = false;
            itemCard.myInventorySlot.myItemCard = null;
            itemCard.myInventorySlot = null;
        }
        
        itemCard.equipted = true;       
        itemCard.transform.SetParent(characterSlot.transform);
        characterSlot.myItem = itemCard;
        itemCard.transform.localScale = new Vector3(1, 1, 1);

        // Weapon specific set up
        if (characterSlot.mySlotType == CharacterItemSlot.SlotType.MainHand)
        {
            characterSlot.myCharacterData.mainHandWeapon = itemCard.myItemData;
        }
        else if (characterSlot.mySlotType == CharacterItemSlot.SlotType.OffHand)
        {
            characterSlot.myCharacterData.offHandWeapon = itemCard.myItemData;
        }

        // Check for weapon slot changes, then update weapon related abilities
        if(itemCard.myItemData.itemType == ItemDataSO.ItemType.MeleeOneHand ||
            itemCard.myItemData.itemType == ItemDataSO.ItemType.MeleeTwoHand ||
            itemCard.myItemData.itemType == ItemDataSO.ItemType.RangedTwoHand ||
            itemCard.myItemData.itemType == ItemDataSO.ItemType.Shield)
        {
            UpdateCharacterAbilitiesFromWeapons(characterSlot.myCharacterData);
        }

        CharacterModelController.ApplyItemDataAppearanceToModel(itemCard.myItemData, characterSlot.myCharacterData.myCharacterModel, characterSlot.mySlotType);

        itemCard.SetRayCastingState(true);
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

                    // remove offhand item view from uc model
                    characterSlot.myCharacterData.myCharacterModel.activeOffHandWeapon.gameObject.SetActive(false);
                    characterSlot.myCharacterData.myCharacterModel.activeOffHandWeapon = null;

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
    #endregion

    // Misc Logic
    #region
    public InventorySlot GetNextAvailableSlot()
    {
        Debug.Log("InventoryController.GetNextAvailableSlot() called...");

        InventorySlot slotReturned = null;

        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.occupied == false)
            {
                slotReturned = slot;
                break;
            }
        }

        if (slotReturned == null)
        {
            Debug.Log("InventoryController.GetNextAvailableSlot() could not find an unoccupied slot, returning a null slot...");
        }

        return slotReturned;
    }
    public void UpdateCharacterAbilitiesFromWeapons(CharacterData character)
    {
        Debug.Log("InventoryController.UpdateCharacterAbilitiesFromWeapons() called...");
        List<AbilityDataSO> abilitiesToLearn = new List<AbilityDataSO>();

        ItemDataSO mainHandWeapon = character.mainHandWeapon;
        ItemDataSO offHandWeapon = character.offHandWeapon;

        AbilityDataSO strike = AbilityLibrary.Instance.GetAbilityByName("Strike");
        AbilityDataSO defend = AbilityLibrary.Instance.GetAbilityByName("Defend");
        AbilityDataSO shoot = AbilityLibrary.Instance.GetAbilityByName("Shoot");
        AbilityDataSO twinStrike = AbilityLibrary.Instance.GetAbilityByName("Twin Strike");

        // Unlearn previous weapon related abilities first
        if (character.DoesCharacterAlreadyKnowAbility(strike))
        {
            character.HandleUnlearnAbility(strike);
        }
        if (character.DoesCharacterAlreadyKnowAbility(shoot))
        {
            character.HandleUnlearnAbility(shoot);
        }
        if (character.DoesCharacterAlreadyKnowAbility(defend))
        {
            character.HandleUnlearnAbility(defend);
        }
        if (character.DoesCharacterAlreadyKnowAbility(twinStrike))
        {
            character.HandleUnlearnAbility(twinStrike);
        }


        // Twin Strike
        if (mainHandWeapon.itemType == ItemDataSO.ItemType.MeleeOneHand &&
            offHandWeapon != null && offHandWeapon.itemType == ItemDataSO.ItemType.MeleeOneHand)
        {
            Debug.Log(character.myName + " learning Twin Strike from weapon loadout");
            abilitiesToLearn.Add(twinStrike);
        }

        // Strike and Defend
        else if (mainHandWeapon.itemType == ItemDataSO.ItemType.MeleeOneHand &&
            offHandWeapon != null && offHandWeapon.itemType == ItemDataSO.ItemType.Shield)
        {
            Debug.Log(character.myName + " learning Strike and Defend from weapon loadout");
            abilitiesToLearn.Add(strike);
            abilitiesToLearn.Add(defend);
        }

        // Strike
        else if (mainHandWeapon.itemType == ItemDataSO.ItemType.MeleeOneHand ||
            mainHandWeapon.itemType == ItemDataSO.ItemType.MeleeTwoHand)
        {
            Debug.Log(character.myName + " learning Strike from weapon loadout");
            abilitiesToLearn.Add(strike);
        }        

        // Shoot
        else if(mainHandWeapon.itemType == ItemDataSO.ItemType.RangedTwoHand)
        {
            Debug.Log(character.myName + " learning Shoot from weapon loadout");
            abilitiesToLearn.Add(shoot);
        }

        // Learn abilities
        foreach(AbilityDataSO ability in abilitiesToLearn)
        {
            character.HandleLearnAbility(ability);
        }
    }
    #endregion

}
