using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    [Header("Ribbon Sprite References")]
    public Sprite commonRibbonSprite;
    public Sprite rareRibbonSprite;
    public Sprite epicRibbonSprite;

    [Header("Frame Sprite References")]
    public Sprite commonFrameSprite;
    public Sprite rareFrameSprite;
    public Sprite epicFrameSprite;

    [Header("Damage Type Sprite References")]
    public Sprite physicalDamageSprite;
    public Sprite fireDamageSprite;
    public Sprite frostDamageSprite;
    public Sprite poisonDamageSprite;
    public Sprite airDamageSprite;
    public Sprite shadowDamageSprite;


    // Get Data
    public string GetDamageTypeFromWeapon(ItemDataSO weapon)
    {
        Debug.Log("ItemManager.GetDamageTypeFromWeapon() called...");
        string damageTypeStringReturned = "None";

        if(weapon.weaponDamageType == ItemDataSO.WeaponDamageType.Physical)
        {
            damageTypeStringReturned = "Physical";
        }
        else if (weapon.weaponDamageType == ItemDataSO.WeaponDamageType.Fire)
        {
            damageTypeStringReturned = "Fire";
        }
        else if (weapon.weaponDamageType == ItemDataSO.WeaponDamageType.Shadow)
        {
            damageTypeStringReturned = "Shadow";
        }
        else if (weapon.weaponDamageType == ItemDataSO.WeaponDamageType.Poison)
        {
            damageTypeStringReturned = "Poison";
        }
        else if (weapon.weaponDamageType == ItemDataSO.WeaponDamageType.Frost)
        {
            damageTypeStringReturned = "Frost";
        }

        Debug.Log("ItemManager.GetDamageTypeFromWeapon() detected that " + weapon.Name + " has a damage type of " + damageTypeStringReturned);
        return damageTypeStringReturned;
    }
    public Sprite GetWeaponDamageTypeImage(ItemDataSO data)
    {
        Debug.Log("GetWeaponDamageTypeImage() called...");

        if (data.weaponDamageType == ItemDataSO.WeaponDamageType.Physical)
        {
            return physicalDamageSprite;
        }

        else if (data.weaponDamageType == ItemDataSO.WeaponDamageType.Fire)
        {
            return fireDamageSprite;
        }

        else if (data.weaponDamageType == ItemDataSO.WeaponDamageType.Frost)
        {
            return frostDamageSprite;
        }

        else if (data.weaponDamageType == ItemDataSO.WeaponDamageType.Poison)
        {
            return poisonDamageSprite;
        }

        else if (data.weaponDamageType == ItemDataSO.WeaponDamageType.Air)
        {
            return airDamageSprite;
        }

        else if (data.weaponDamageType == ItemDataSO.WeaponDamageType.Shadow)
        {
            return shadowDamageSprite;
        }
        else
        {
            Debug.Log("GetWeaponDamageTypeImage() coudln't detect damage type, returning a null sprite...");
            return null;
        }
    }  
    public string ConvertItemTypeEnumToString(ItemDataSO data)
    {
        Debug.Log("ConvertItemTypeEnumToString() called for " + data.Name);

        string stringReturned = "";

        if(data.itemType == ItemDataSO.ItemType.Head)
        {
            stringReturned = "Head";
        }

        else if (data.itemType == ItemDataSO.ItemType.Chest)
        {
            stringReturned = "Chest";
        }

        else if (data.itemType == ItemDataSO.ItemType.Legs)
        {
            stringReturned = "Legs";
        }

        else if (data.itemType == ItemDataSO.ItemType.MeleeOneHand)
        {
            stringReturned = "Melee 1H";
        }

        else if (data.itemType == ItemDataSO.ItemType.MeleeTwoHand)
        {
            stringReturned = "Melee 2H";
        }
        else if (data.itemType == ItemDataSO.ItemType.RangedTwoHand)
        {
            stringReturned = "Ranged 2H";
        }

        Debug.Log("Conversion returned: " + stringReturned);
        return stringReturned;
    }

    // Assign items    
    public void SetUpDefenderWeaponsFromCharacterData(LivingEntity entity)
    {
        Debug.Log("ItemManager.SetUpLivingEntityWeapons() called for " + entity.myName);

        if(entity.defender)
        {
            Debug.Log(entity.myName + " is a 'Defender', checking character data for weapon info...");

            // check for and assign main hand weapon
            if (entity.defender.myCharacterData.mainHandWeapon != null)
            {
                Debug.Log("Assigning " + entity.defender.myCharacterData.mainHandWeapon.Name + " to " + entity.myName + " main hand weapon variable...");
                entity.myMainHandWeapon = entity.defender.myCharacterData.mainHandWeapon;
            }

            // check for and assign off hand weapon
            if (entity.defender.myCharacterData.offHandWeapon != null)
            {
                Debug.Log("Assigning " + entity.defender.myCharacterData.offHandWeapon.Name + " to " + entity.myName + " off hand weapon variable...");
                entity.myOffHandWeapon = entity.defender.myCharacterData.offHandWeapon;
            }
        }

    }
    public void ApplyAllItemEffectsToCharacterData(CharacterData character, ItemDataSO item, bool removing = false)
    {
        Debug.Log("ItemManager.ApplyAllItemEffectsToCharacterData() called for " + item.Name);
        // Check item effect one
        if(item.itemEffectOne != ItemDataSO.ItemEffect.None)
        {
            ApplyItemEffectToCharacterData(character, item.itemEffectOne, item.itemEffectOneValue, removing);
        }

        // Check item effect two
        if (item.itemEffectTwo != ItemDataSO.ItemEffect.None)
        {
            ApplyItemEffectToCharacterData(character, item.itemEffectTwo, item.itemEffectTwoValue, removing);
        }

        // Check item effect three
        if (item.itemEffectThree != ItemDataSO.ItemEffect.None)
        {
            ApplyItemEffectToCharacterData(character, item.itemEffectThree, item.itemEffectThreeValue, removing);
        }
    }
    private void ApplyItemEffectToCharacterData(CharacterData character, ItemDataSO.ItemEffect itemEffect, int itemEffectStacks, bool removing)
    {
        Debug.Log("ItemManager.ApplyItemEffectToCharacterData() called for " + itemEffect.ToString());

        int stacksApplied = itemEffectStacks;

        if (removing)
        {
            stacksApplied = -stacksApplied;
            Debug.Log("Removing " + stacksApplied.ToString() + " " + itemEffect.ToString());
        }
        else
        {
            Debug.Log("Appying " + stacksApplied.ToString() + " " + itemEffect.ToString());
        }

        // Bonus Strength
        if(itemEffect == ItemDataSO.ItemEffect.BonusStrength)
        {
            character.ModifyStrength(stacksApplied);
        }

        // Bonus Wisdom
        else if (itemEffect == ItemDataSO.ItemEffect.BonusWisdom)
        {
            character.ModifyWisdom(stacksApplied);
        }

        // Bonus Dexterity
        else if (itemEffect == ItemDataSO.ItemEffect.BonusDexterity)
        {
            character.ModifyDexterity(stacksApplied);
        }

        // Bonus Mobility
        else if (itemEffect == ItemDataSO.ItemEffect.BonusMobility)
        {
            character.ModifyMobility(stacksApplied);
        }

        // Bonus Initiative
        else if (itemEffect == ItemDataSO.ItemEffect.BonusInitiative)
        {
            character.ModifyInitiative(stacksApplied);
        }

        // Bonus Stamina
        else if (itemEffect == ItemDataSO.ItemEffect.BonusStamina)
        {
            character.ModifyStamina(stacksApplied);
        }

        // Bonus Critical
        else if (itemEffect == ItemDataSO.ItemEffect.BonusCritical)
        {
            character.ModifyCriticalChance(stacksApplied);
        }

        // Bonus Dodge
        else if (itemEffect == ItemDataSO.ItemEffect.BonusDodge)
        {
            character.ModifyDodge(stacksApplied);
        }

        // Bonus Parry
        else if (itemEffect == ItemDataSO.ItemEffect.BonusParry)
        {
            character.ModifyParry(stacksApplied);
        }

        // Max Energy
        else if (itemEffect == ItemDataSO.ItemEffect.BonusMaxEnergy)
        {
            character.ModifyMaxEnergy(stacksApplied);
        }

        // Melee Range
        else if (itemEffect == ItemDataSO.ItemEffect.BonusMeleeRange)
        {
            character.ModifyMeleeRange(stacksApplied);
        }

        // Aura Size
        else if (itemEffect == ItemDataSO.ItemEffect.BonusAuraSize)
        {
            character.ModifyAuraSize(stacksApplied);
        }

        // TO DO: implement bonus spell damage type effects (BonusFireDamage, etc) when character data and items updated

        // Enrage
        else if (itemEffect == ItemDataSO.ItemEffect.Enrage)
        {
            character.ModifyEnrage(stacksApplied);
        }

        // Hawk Eye
        else if (itemEffect == ItemDataSO.ItemEffect.HawkEye)
        {
            character.ModifyHawkEye(stacksApplied);
        }

        // Thorns
        else if (itemEffect == ItemDataSO.ItemEffect.Thorns)
        {
            character.ModifyThorns(stacksApplied);
        }

        // Opportunist
        else if (itemEffect == ItemDataSO.ItemEffect.Opportunist)
        {
            character.ModifyOpportunist(stacksApplied);
        }

        // Bonus Power Limit
        else if (itemEffect == ItemDataSO.ItemEffect.BonusPowerLimit)
        {
            character.ModifyPowerLimit(stacksApplied);
        }

        // Bonus All Resistances
        else if (itemEffect == ItemDataSO.ItemEffect.BonusAllResistances)
        {
            character.ModifyPhysicalResistance(stacksApplied);
            character.ModifyFireResistance(stacksApplied);
            character.ModifyFrostResistance(stacksApplied);
            character.ModifyPoisonResistance(stacksApplied);
            character.ModifyAirResistance(stacksApplied);
            character.ModifyShadowResistance(stacksApplied);
        }

        // Stealth
        else if (itemEffect == ItemDataSO.ItemEffect.Stealth)
        {
            character.ModifyStealth(stacksApplied);
        }

        // True Sight
        else if (itemEffect == ItemDataSO.ItemEffect.TrueSight)
        {
            character.ModifyTrueSight(stacksApplied);
        }

        // Slippery
        else if (itemEffect == ItemDataSO.ItemEffect.Slippery)
        {
            character.ModifySlippery(stacksApplied);
        }

        // Unstoppable
        else if (itemEffect == ItemDataSO.ItemEffect.Unstoppable)
        {
            character.ModifyUnstoppable(stacksApplied);
        }

        // Perfect Aim
        else if (itemEffect == ItemDataSO.ItemEffect.PerfectAim)
        {
            character.ModifyPerfectAim(stacksApplied);
        }

        // Virtuoso
        else if (itemEffect == ItemDataSO.ItemEffect.Virtuoso)
        {
            character.ModifyVirtuoso(stacksApplied);
        }

        // Riposte
        else if (itemEffect == ItemDataSO.ItemEffect.Riposte)
        {
            character.ModifyRiposte(stacksApplied);
        }
    }

    // Bools and conditional checks
    public bool IsItemWeapon(ItemDataSO item)
    {
        Debug.Log("ItemManager.IsItemWeapon() called for: " + item.Name);

        if(item.itemType == ItemDataSO.ItemType.MeleeOneHand ||
            item.itemType == ItemDataSO.ItemType.MeleeTwoHand ||
            item.itemType == ItemDataSO.ItemType.RangedTwoHand)
        {
            Debug.Log(item.Name + " is a weapon");
            return true;
        }
        else
        {
            Debug.Log(item.Name + " is NOT a weapon");
            return false;
        }
    }

    // Set up item cards + etc
    public void SetUpItemCardFromData(ItemCard itemCard, ItemDataSO data)
    {
        Debug.Log("ItemManager.RunSetupFromItemData() called for: " + data.Name);

        // Set scale
        itemCard.originalScale = itemCard.GetComponent<RectTransform>().localScale.x;

        // Store data
        itemCard.myItemDataSO = data;

        // Set properties
        itemCard.myName = data.Name;
        itemCard.myNameText.text = data.Name;
        itemCard.itemImage.sprite = data.sprite;
        //itemCard.myItemRarity = data.itemRarity;
        itemCard.myDescriptionText.text = data.itemDescription;
        itemCard.myItemTypeText.text = ConvertItemTypeEnumToString(data);

        // Weapon specific set up
        if (IsItemWeapon(data))
        {
            // Turn on damage type icon
            itemCard.weaponDamageTypeParent.SetActive(true);

            // Get and set correct damage type sprite
            itemCard.itemDamageTypeImage.sprite = GetWeaponDamageTypeImage(data);

            // Set base damage text
            itemCard.myBaseDamageText.text = data.baseDamage.ToString();
        }

        // Set up ribbon + frame rarity sprites
        if (data.itemRarity == ItemDataSO.ItemRarity.Common)
        {
            itemCard.itemImageFrame.sprite = commonFrameSprite;
            itemCard.itemNameRibbonImage.sprite = commonRibbonSprite;
        }
        else if (data.itemRarity == ItemDataSO.ItemRarity.Rare)
        {
            itemCard.itemImageFrame.sprite = rareFrameSprite;
            itemCard.itemNameRibbonImage.sprite = rareRibbonSprite;
        }
        else if (data.itemRarity == ItemDataSO.ItemRarity.Epic)
        {
            itemCard.itemImageFrame.sprite = epicFrameSprite;
            itemCard.itemNameRibbonImage.sprite = epicRibbonSprite;
        }
    }
    public void SetUpInventoryItemCardFromData(InventoryItemCard itemCard, ItemDataSO data)
    {
        Debug.Log("InventoryController.AddItemToInventory() called for " + data.Name);

        itemCard.myItemData = data;

        itemCard.itemImage.sprite = data.sprite;

        itemCard.myInfoItemCard.RunSetupFromItemData(data);
    }
    
    


}
