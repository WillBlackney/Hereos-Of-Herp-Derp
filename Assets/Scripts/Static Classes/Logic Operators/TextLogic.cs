using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class TextLogic 
{
    [Header("RGBA Colour Codes")]
    public static string white = "<color=#FFFFFF>";
    public static string yellow = "<color=#FFF91C>";

    public static string physical = "<color=#BA8400>";
    public static string fire = "<color=#FF6637>";
    public static string frost = "<color=#3687FF>";
    public static string shadow = "<color=#CF01BC>";
    public static string air = "<color=#36EDFF>";
    public static string poison = "<color=#00EC4A>";

    public static string blue = "<color=#00BEFF>";    
    public static string purple = "<color=#CF01BC>";
    public static string darkRed = "<color=#AB0500>";

    public static void SetStatusIconDescriptionText(string statusName, TextMeshProUGUI statusDescriptionText, int statusStacks)
    {
        if (statusName == "Bonus Strength")
        {
            statusDescriptionText.text =
                "Increases the base damage of "+ ReturnColoredText("Melee Attack", yellow)+ " abilities by" +
                ReturnColoredText(statusStacks.ToString(), yellow);
        }
        else if (statusName == "Temporary Bonus Strength")
        {
            statusDescriptionText.text =
                "Increases the base damage of " + ReturnColoredText("Melee Attack", yellow) + " abilities by" +
                ReturnColoredText(statusStacks.ToString(), yellow) +
            ". Expires on activation end";
        }
        else if (statusName == "Bonus Dexterity")
        {
            statusDescriptionText.text =
                "Increases the amount of " + ReturnColoredText("Block", yellow) +
                " granted from abilities and effects by " + ReturnColoredText(statusStacks.ToString(), yellow);
        }
        else if (statusName == "Temporary Bonus Dexterity")
        {
            statusDescriptionText.text =
                "Increases the amount of " + ReturnColoredText("Block", yellow) +
                " granted from abilities and effects by  " + ReturnColoredText(statusStacks.ToString(), yellow)
                + ". Expires on activation end";
        }
        else if (statusName == "Bonus Wisdom")
        {
            statusDescriptionText.text =
                "Increases all " +
                ReturnColoredText("Fire", fire) + ", " +
                ReturnColoredText("Frost", frost) + ", " +
                ReturnColoredText("Poison", poison) + ", " +
                ReturnColoredText("Shadow", shadow) + ", and " +
                ReturnColoredText("Air", air) +
                " damage from abilities by and effects by " + ReturnColoredText(statusStacks.ToString(), yellow);
        }
        else if (statusName == "Temporary Bonus Wisdom")
        {
            statusDescriptionText.text =
            "Increases all " +
                ReturnColoredText("Fire", fire) + ", " +
                ReturnColoredText("Frost", frost) + ", " +
                ReturnColoredText("Poison", poison) + ", " +
                ReturnColoredText("Shadow", shadow) + ", and" +
                ReturnColoredText("Air", air) +
                " damage from abilities by and effects by " + ReturnColoredText(statusStacks.ToString(), yellow)
                + ". Expires on activation end"; 
        }
        else if (statusName == "Bonus Mobility")
        {
            statusDescriptionText.text =
                "Increase the range of movement abilities by " + ReturnColoredText(statusStacks.ToString(), yellow);
        }
        else if (statusName == "Temporary Bonus Mobility")
        {
            statusDescriptionText.text =
                "Increase the range of movement abilities by " + ReturnColoredText(statusStacks.ToString(), yellow)
                + ". Expires on activation end"; 
        }
        else if (statusName == "Bonus Initiative")
        {
            statusDescriptionText.text =
                "Increases your activation order roll by " + ReturnColoredText(statusStacks.ToString(), yellow);
        }
        else if (statusName == "Temporary Bonus Initiative")
        {
            statusDescriptionText.text =
                "Increases your activation order roll by " + ReturnColoredText(statusStacks.ToString(), yellow)
                + ". Expires on activation end"; 
        }
        else if (statusName == "Bonus Stamina")
        {
            statusDescriptionText.text =
                "Increase the amount of "+ ReturnColoredText("Energy", yellow) + 
                " gained on activation start by " + ReturnColoredText(statusStacks.ToString(), yellow);
        }
        else if (statusName == "Temporary Bonus Stamina")
        {
            statusDescriptionText.text =
                "Increase the amount of " + ReturnColoredText("Energy", yellow) +
                " gained on activation start by " + ReturnColoredText(statusStacks.ToString(), yellow)
                + ". Expires on activation end"; 
        }
        else if (statusName == "Temporary Dodge")
        {
            statusDescriptionText.text =
                "Increases your chance to completely avoid Ranged Attacks by " +
                ReturnColoredText(statusStacks.ToString(), yellow)
                + "%. Expires on activation end";
        }
        else if (statusName == "Temporary Parry")
        {
            statusDescriptionText.text =
                "Increases your chance to completely avoid Melee Attacks by " +
                ReturnColoredText(statusStacks.ToString(), yellow)
                + "%. Expires on activation end";
        }
        else if (statusName == "Vulnerable")
        {
            statusDescriptionText.text =
                "This character takes 50% increased damage from all attacks";
        }
        else if (statusName == "Weakened")
        {
            statusDescriptionText.text =
                "This character deals 50% less damage with all attacks";
        }
        else if (statusName == "Burning")
        {
            statusDescriptionText.text =
                "On activation end, take " + ReturnColoredText(statusStacks.ToString(), yellow) + " " +
                ReturnColoredText("Fire", fire) + " damage";
        }
        else if (statusName == "Poisoned")
        {
            statusDescriptionText.text =
                "On activation end, take " + ReturnColoredText(statusStacks.ToString(), yellow) + " " +
                ReturnColoredText("Poison", poison) + " damage";
        }
        else if (statusName == "Fading")
        {
            statusDescriptionText.text =
                "On activation end, lose " + ReturnColoredText(statusStacks.ToString(), yellow) + " health";
        }
        else if (statusName == "Camoflage")
        {
            statusDescriptionText.text =
                "This character cannot be targetted by enemy abilities from further than " + ReturnColoredText("1", yellow)
                + " tile away. Cancelled by using abilities (except Move), or taking damage";
        }
        else if (statusName == "Cautious")
        {
            statusDescriptionText.text =
                "On activation end, if this character has no Block, it gains " + ReturnColoredText(statusStacks.ToString(), yellow)
                + " Block";
        }
        else if (statusName == "Chilled")
        {
            statusDescriptionText.text =
                "This character has -1 " + ReturnColoredText("Mobility", yellow) + " and " +
                ReturnColoredText("Initiative", yellow) + ". Expires on activation end";
        }
        else if (statusName == "Concentration")
        {
            statusDescriptionText.text =
                "This character's Ranged Attack abilities cannot be dodged. +20 Ranged Critical Chance";
        }
        else if (statusName == "Demon")
        {
            statusDescriptionText.text =
                "Increase all "+ ReturnColoredText("Fire", fire) + " damage dealt by " + ReturnColoredText("20%", yellow) + ". " +
                ReturnColoredText("Fire", fire) + " Resistance increased by 20";
        }
        else if (statusName == "Shadow Form")
        {
            statusDescriptionText.text =
                "Increase all " + ReturnColoredText("Shadow", shadow) + " damage dealt by " + ReturnColoredText("20%", yellow) + ". " +
                ReturnColoredText("Shadow", shadow) + " Resistance increased by 20";
        }
        else if (statusName == "Toxicity")
        {
            statusDescriptionText.text =
                "Increase all " + ReturnColoredText("Poison", poison) + " damage dealt by " + ReturnColoredText("20%", yellow) + ". " +
                ReturnColoredText("Poison", poison) + " Resistance increased by 20";
        }
        else if (statusName == "Storm Lord")
        {
            statusDescriptionText.text =
                "Increase all " + ReturnColoredText("Air", air) + " damage dealt by " + ReturnColoredText("20%", yellow) + ". " +
                ReturnColoredText("Air", air) + " Resistance increased by 20";
        }
        else if (statusName == "Frozen Heart")
        {
            statusDescriptionText.text =
                "Increase all " + ReturnColoredText("Frost", frost) + " damage dealt by " + ReturnColoredText("20%", yellow) + ". " +
                ReturnColoredText("Frost", frost) + " Resistance increased by 20";
        }
        else if (statusName == "Disarmed")
        {
            statusDescriptionText.text =
                "This character cannot use " + ReturnColoredText("Melee Attack", yellow) + " abilities. Expires on activation end";
        }
        else if (statusName == "Blind")
        {
            statusDescriptionText.text =
                "This character cannot use " + ReturnColoredText("Ranged Attack", yellow) + " abilities. Expires on activation end";
        }
        else if (statusName == "Silenced")
        {
            statusDescriptionText.text =
                "This character cannot use " + ReturnColoredText("Skill", yellow) + " abilities. Expires on activation end";
        }
        else if (statusName == "Terrified")
        {
            statusDescriptionText.text =
                "This character cannot gain Block";
        }
        else if (statusName == "Stunned")
        {
            statusDescriptionText.text =
                "This character skips its next activation, and cannot take any actions";
        }
        else if (statusName == "Taunted")
        {
            statusDescriptionText.text =
                "This character is forced to target its taunter during its next activation";
        }
        else if (statusName == "Sleep")
        {
            statusDescriptionText.text =
                "This character skips its next activation, and cannot take any actions. Removed if damaged";
        }
        else if (statusName == "Immobilized")
        {
            statusDescriptionText.text =
                "Unable to take movement actions with abilities and effects";
        }
        else if (statusName == "Radiance")
        {
            statusDescriptionText.text =
                "This character's Aura size is increased by " + ReturnColoredText(statusStacks.ToString(), yellow);
        }
        else if (statusName == "Encouraging Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character grants " + ReturnColoredText(statusStacks.ToString(), yellow) + " Energy to allies within it's Aura";
        }
        else if (statusName == "Fiery Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character deals " + ReturnColoredText(statusStacks.ToString(), yellow) +
                " " + ReturnColoredText("Fire", fire) + " damage to enemies within it's Aura";
        }
        else if (statusName == "Toxic Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character applies " + ReturnColoredText(statusStacks.ToString(), yellow) + " " +
                 ReturnColoredText("Poisoned", poison) + " to enemies within it's Aura";
        }
        else if (statusName == "Storm Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character deals " + ReturnColoredText(statusStacks.ToString(), yellow) +
                " " + ReturnColoredText("Air", air) + " damage to a random enemy within it's Aura twice";
        }
        else if (statusName == "Soul Drain Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character steals " + ReturnColoredText(statusStacks.ToString(), yellow) +
                " Strength from enemies within it's Aura";
        }
        else if (statusName == "Guardian Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character grants " + ReturnColoredText(statusStacks.ToString(), yellow) +
                 " Block to allies within it's Aura";
        }
        else if (statusName == "Hateful Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character grants " + ReturnColoredText(statusStacks.ToString(), yellow)
                + " Strength to allies within it's Aura";
        }
        else if (statusName == "Enrage")
        {
            statusDescriptionText.text =
                "Whenever this character loses HP, it gains " + ReturnColoredText(statusStacks.ToString(), yellow)
                + " Strength";
        }
        else if (statusName == "Tenacious")
        {
            statusDescriptionText.text =
                "Whenever this character loses HP, it gains " + ReturnColoredText(statusStacks.ToString(), yellow)
                + " Block";
        }
        else if (statusName == "Opportunist")
        {
            statusDescriptionText.text =
                "This character deals " + ReturnColoredText(statusStacks.ToString(), yellow)
                + "% extra damage with "+ ReturnColoredText("Melee Attack", yellow) + " abilities when back striking";
        }
        else if (statusName == "Hawk Eye")
        {
            statusDescriptionText.text =
                "This character has +" + ReturnColoredText(statusStacks.ToString(), yellow)
                + " range with " + ReturnColoredText("Ranged Attack", yellow) + " abilities";
        }
        else if (statusName == "Temporary Hawk Eye")
        {
            statusDescriptionText.text =
                "This character has +" + ReturnColoredText(statusStacks.ToString(), yellow)
                + " range with " + ReturnColoredText("Ranged Attack", yellow) + " abilities. Expires on activation end";
        }

        else if (statusName == "Ethereal Being")
        {
            statusDescriptionText.text =
                "This character ignores line of sight when targetting";
        }

        else if (statusName == "True Sight")
        {
            statusDescriptionText.text =
                "This character ignores " + ReturnColoredText("Camoflage", yellow) + " when attacking";
        }
        else if (statusName == "Temporary True Sight")
        {
            statusDescriptionText.text =
               "This character ignores " + ReturnColoredText("Camoflage", yellow) + " when attacking. Expires on activation end";
        }
        else if (statusName == "Overwatch")
        {
            statusDescriptionText.text =
                "This character will perform " + ReturnColoredText("Shoot", yellow) + 
                " against the first enemy that moves within it's weapon range";
        }

        else if (statusName == "Fiery Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with " + ReturnColoredText("Melee Attack", yellow) + " abilities into " +
                ReturnColoredText("Fire", fire) + " damage";
        }
        else if (statusName == "Temporary Fiery Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with " + ReturnColoredText("Melee Attack", yellow) + " abilities into " +
                ReturnColoredText("Fire", fire) + " damage. Expires on activation end";
        }
        else if (statusName == "Shadow Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with " + ReturnColoredText("Melee Attack", yellow) + " abilities into " +
                ReturnColoredText("Shadow", shadow) + " damage";
        }
        else if (statusName == "Temporary Shadow Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with " + ReturnColoredText("Melee Attack", yellow) + " abilities into " +
                ReturnColoredText("Shadow", shadow) + " damage. Expires on activation end";
        }
        else if (statusName == "Frost Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with " + ReturnColoredText("Melee Attack", yellow) + " abilities into " +
                ReturnColoredText("Frost", frost) + " damage";
        }
        else if (statusName == "Temporary Frost Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with " + ReturnColoredText("Melee Attack", yellow) + " abilities into " +
                ReturnColoredText("Frost", frost) + " damage. Expires on activation end";
        }
        else if (statusName == "Poison Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with " + ReturnColoredText("Melee Attack", yellow) + " abilities into " +
                ReturnColoredText("Poison", poison) + " damage";
        }
        else if (statusName == "Temporary Poison Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with " + ReturnColoredText("Melee Attack", yellow) + " abilities into " +
                ReturnColoredText("Poison", poison) + " damage. Expires on activation end";
        }
        else if (statusName == "Air Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with " + ReturnColoredText("Melee Attack", yellow) + " abilities into " +
                ReturnColoredText("Air", air) + " damage";
        }
        else if (statusName == "Temporary Air Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with " + ReturnColoredText("Melee Attack", yellow) + " abilities into " +
                ReturnColoredText("Air", air) + " damage. Expires on activation end";
        }
        else if (statusName == "Flux")
        {
            statusDescriptionText.text =
                "This character's first " + ReturnColoredText("Move", yellow) +
                " ability each activation costs " + ReturnColoredText("0", yellow) + " Energy";
        }
        else if (statusName == "Transcendence")
        {
            statusDescriptionText.text =
                "This character is immune to all damage until the end of the current turn cycle";
        }
        else if (statusName == "Immolation")
        {
            statusDescriptionText.text =
                "Whenever this character reduces health with a " + ReturnColoredText("Melee Attack", yellow) +
                " ability, it applies " + ReturnColoredText(statusStacks.ToString(), yellow) + " "
                + ReturnColoredText("Burning", fire);
        }
        else if (statusName == "Poisonous")
        {
            statusDescriptionText.text =
                "Whenever this character reduces health with a " + ReturnColoredText("Melee Attack", yellow) +
                " ability, it applies " + ReturnColoredText(statusStacks.ToString(), yellow) + " "
                + ReturnColoredText("Poisoned", poison);
        }
        else if (statusName == "Growing")
        {
            statusDescriptionText.text =
                "On activation start, this character gains " + ReturnColoredText(statusStacks.ToString(), yellow)
                + " Strength";
        }
        else if (statusName == "Incorruptable")
        {
            statusDescriptionText.text =
                "This character is immune to " + ReturnColoredText("Weakened", yellow) + " and " + ReturnColoredText("Vulnerable", yellow);
        }
        else if (statusName == "Unfallible")
        {
            statusDescriptionText.text =
                "This character is immune to " + ReturnColoredText("Blind", yellow) + ", "
                + ReturnColoredText("Silenced", yellow) + " and "
                + ReturnColoredText("Disarmed", yellow);
        }
        else if (statusName == "Undead")
        {
            statusDescriptionText.text =
                "This character is immune to "+ ReturnColoredText("Poisoned", poison) + 
                " and " + ReturnColoredText("Terrified", yellow);
        }
        else if (statusName == "Unstoppable")
        {
            statusDescriptionText.text =
                "This character is immune to " + ReturnColoredText("Immobilized", yellow) + " and "
                + ReturnColoredText("Knock Back", yellow);
        }
        else if (statusName == "Unwavering")
        {
            statusDescriptionText.text =
                "This character's Block does not expire on activation start";
        }
        else if (statusName == "Infuse")
        {
            statusDescriptionText.text =
                "This character has +" + ReturnColoredText("20", yellow) + " to all resistances";
        }
        else if (statusName == "Testudo")
        {
            statusDescriptionText.text =
                "On activation start, this character gains " + ReturnColoredText("5", yellow) + " Block";
        }
        else if (statusName == "Last Stand")
        {
            statusDescriptionText.text =
                "The first time this character would take lethal damage, it gains 5 Strength, and its health is set at 1";
        }
        else if (statusName == "Rapid Cloaking")
        {
            statusDescriptionText.text =
                "On activation end, this character gains " + ReturnColoredText("Camoflage", yellow);
        }
        else if (statusName == "Life Steal")
        {
            statusDescriptionText.text =
                "Whenever this character reduces health with a " + ReturnColoredText("Melee Attack", yellow) + 
                " ability, it heals that much health";
        }
        else if (statusName == "Masochist")
        {
            statusDescriptionText.text =
                "While this character has 50% or less health, it has +" + ReturnColoredText("20", yellow) + " Critical Chance";
        }
        else if (statusName == "Nimble")
        {
            statusDescriptionText.text =
                "This character's Dodge and Parry chance is increased by " + ReturnColoredText("10", yellow) ;
        }
        else if (statusName == "Perfect Reflexes")
        {
            statusDescriptionText.text =
                "This character's Dodge and Parry chance is increased by " + ReturnColoredText("20", yellow);
        }
        else if (statusName == "Patient Stalker")
        {
            statusDescriptionText.text =
                "This character has +1 Mobility and +20 Critical Strike chance while " + ReturnColoredText("Camoflaged", yellow);
        }
        else if (statusName == "Perfect Aim")
        {
            statusDescriptionText.text =
                "This character's " + ReturnColoredText("Ranged Attack", yellow)+ " abilities cannot be dodged";
        }
        else if (statusName == "Phasing")
        {
            statusDescriptionText.text =
                "The first time this character is melee attacked each turn cycle, it " + ReturnColoredText("Teleports", yellow) +
                " to a random tile within " + ReturnColoredText("2", yellow);
        }
        else if (statusName == "Poison Immunity")
        {
            statusDescriptionText.text =
                "This character cannot be " + ReturnColoredText("Poisoned", poison);
        }
        else if (statusName == "Predator")
        {
            statusDescriptionText.text =
                "This character has " + ReturnColoredText("20", yellow) + " bonus Critical Chance while " + ReturnColoredText("Camoflaged", yellow);
        }
        else if (statusName == "Preparation")
        {
            statusDescriptionText.text =
                "The next ability this character uses costs 0 Energy";
        }
        else if (statusName == "Purity")
        {
            statusDescriptionText.text =
                "This character has +2 " + ReturnColoredText("Strength", yellow) + ", " +
                ReturnColoredText("Wisdom", yellow) + " and " +
                ReturnColoredText("Dexterity", yellow);
        }
        else if (statusName == "Recklessness")
        {
            statusDescriptionText.text =
                "This character has +20 Melee Critical Chance and it's " + ReturnColoredText("Melee Attack", yellow)+ 
                " abilities cannot be parried";
        }
        else if (statusName == "Riposte")
        {
            statusDescriptionText.text =
                "Whenever this character parries an attack, it performs "+ ReturnColoredText("Strike", yellow)+ " against its attacker";
        }
        else if (statusName == "Sacred Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character removes " + ReturnColoredText("Blind", yellow) + ", "
                + ReturnColoredText("Immobilized", yellow) + ", "
                + ReturnColoredText("Disarmed", yellow) + ", "
                + ReturnColoredText("Silenced", yellow) + " and "
                + ReturnColoredText("Terrified", yellow) + 
                " from allies within its Aura";
        }
        else if (statusName == "Shadow Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character applies "+ ReturnColoredText("Weakened", yellow) +
                " to enemies within its Aura";
        }
        else if (statusName == "Sharpened Blade")
        {
            statusDescriptionText.text =
                "This character's next " + ReturnColoredText("Melee Attack", yellow) + " ability is guaranteed to critically strike";
        }
        else if (statusName == "Shatter")
        {
            statusDescriptionText.text =
                "This character has +20 Critical Chance when attacking targets with " + ReturnColoredText("Chilled", frost);
        }
        else if (statusName == "Slippery")
        {
            statusDescriptionText.text =
                "This character is immune to free strikes";
        }
        else if (statusName == "Stealth")
        {
            statusDescriptionText.text =
                "This character is permanently " + ReturnColoredText("Camoflaged", yellow);
        }
        else if (statusName == "Regeneration")
        {
            statusDescriptionText.text =
                "On activation end, this character recovers " + 
                ReturnColoredText(statusStacks.ToString(), yellow) + " health";
        }
        else if (statusName == "Thorns")
        {
            statusDescriptionText.text =
                "Whenever this character is hit with a " + ReturnColoredText("Melee Attack", yellow) + "ability, it deals " +
                ReturnColoredText(statusStacks.ToString(), yellow) + " " + ReturnColoredText("Physical", physical) +
                " damage back to it's attacker";
        }
        else if (statusName == "Time Warp")
        {
            statusDescriptionText.text =
                "This character gains " + ReturnColoredText(statusStacks.ToString(), yellow) + " extra activation";
        }
        else if (statusName == "Volatile")
        {
            statusDescriptionText.text =
                "On death, this character explodes, dealing " + ReturnColoredText(statusStacks.ToString(), yellow) + " " +
                ReturnColoredText("Physical", physical) + " damage to ALL adjacent characters";
        }
        
        else if (statusName == "Growing")
        {
            statusDescriptionText.text =
                "At the start of this characters activation, it gains  " + ReturnColoredText(statusStacks.ToString(), yellow)
                + " Strength";
        }
        else if (statusName == "Virtuoso")
        {
            statusDescriptionText.text =
                "This character's " + ReturnColoredText("Melee Attack", yellow) + " abilities cannot be parried";
        }
        else if (statusName == "Venomous")
        {
            statusDescriptionText.text =
                "Whenever this character applies "+ ReturnColoredText("Poisoned", poison)+ 
                ", it applies " + ReturnColoredText(statusStacks.ToString(), yellow) + " extra";
        }



    }
    public static void SetAbilityDescriptionText(Ability ability)
    {
        // Set up
        LivingEntity entity = ability.myLivingEntity;
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(entity, ability, entity.myMainHandWeapon);
        ItemDataSO weaponUsed = null;
        if(ability.requiresMeleeWeapon || ability.requiresRangedWeapon)
        {
            weaponUsed = entity.myMainHandWeapon;
        }
        int damageValue = CombatLogic.Instance.GetBaseDamageValue(entity, ability.abilityPrimaryValue, ability, damageType, weaponUsed);


        // Set Text
        if (ability.abilityName == "Strike")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Twin Strike")
        {
            // Get off hand weapon data

            string offHandDamageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(entity, ability, entity.myOffHandWeapon);
            int offHandDamageValue = CombatLogic.Instance.GetBaseDamageValue(entity, ability.abilityPrimaryValue, ability, offHandDamageType, entity.myOffHandWeapon);

            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage, then " +
                ReturnColoredText(offHandDamageValue.ToString(), yellow) + " " +
                ReturnColoredText(offHandDamageType, GetColorCodeFromString(offHandDamageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Shoot")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Move")
        {
            ability.descriptionText.text = "Move to a tile within " + ReturnColoredText(EntityLogic.GetTotalMobility(entity).ToString(), yellow);
        }
        else if (ability.abilityName == "Defend")
        {
            ability.descriptionText.text =
                "Gain " + ReturnColoredText(CombatLogic.Instance.CalculateBlockGainedByEffect(ability.abilityPrimaryValue, entity).ToString(), yellow)
                + " " + ReturnColoredText("Block", yellow);
        }
        else if (ability.abilityName == "Inspire")
        {
            ability.descriptionText.text =
                "Increase a target's " + ReturnColoredText("Strength", yellow) + " by " +
                ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow);
        }
        else if (ability.abilityName == "Charge")
        {
            ability.descriptionText.text =
                "Move to a target enemy within " + ReturnColoredText((EntityLogic.GetTotalMobility(entity) + ability.abilityRange).ToString(), yellow) +
                " and apply " +
                ReturnColoredText("Vulnerable", yellow);
        }
        else if (ability.abilityName == "Whirlwind")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to all enemies within " +
                ReturnColoredText(AbilityLogic.Instance.CalculateAbilityRange(ability, entity).ToString(), yellow);
        }
        else if (ability.abilityName == "Fire Ball")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Lightning Bolt")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " + ReturnColoredText("Shocked", air);
        }
        else if (ability.abilityName == "Frost Bolt")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " +
                ReturnColoredText("Immobilized", yellow);
        }
        else if (ability.abilityName == "Telekinesis")
        {
            ability.descriptionText.text =
                "Teleport a target anywhere within " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) +
                " of its current position";
        }
        else if (ability.abilityName == "Dash")
        {
            ability.descriptionText.text =
                "Move to a tile within " + ReturnColoredText((ability.abilityRange + EntityLogic.GetTotalMobility(entity)).ToString(), yellow) +
                " of your current position";
        }
        else if (ability.abilityName == "Preparation")
        {
            ability.descriptionText.text =
                "The next ability you use costs " + ReturnColoredText("0", yellow) +
                " Energy";
        }
        else if (ability.abilityName == "Holy Fire")
        {
            ability.descriptionText.text =
                "Give " + ReturnColoredText(CombatLogic.Instance.CalculateBlockGainedByEffect(ability.abilityPrimaryValue, entity).ToString(), yellow)
                + " " +
                ReturnColoredText("Block", yellow) +
                " to an ally, or deal " +
                ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to an enemy";
        }
        else if (ability.abilityName == "Invigorate")
        {
            ability.descriptionText.text =
                "Give an ally " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) +
                " Energy";
        }
        else if (ability.abilityName == "Chaos Bolt")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target, and apply " +
                ReturnColoredText("Vulnerable", yellow);
        }
        else if (ability.abilityName == "Ambush")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. If this back strikes, gain " +
                ReturnColoredText(ability.abilitySecondaryValue.ToString(), yellow) +
                ReturnColoredText(" Energy", yellow);
        }
        else if (ability.abilityName == "Blade Flurry")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a random enemy within " +
                ReturnColoredText(AbilityLogic.Instance.CalculateAbilityRange(ability, entity).ToString(), yellow) +
                " three times";
        }
        else if (ability.abilityName == "Purity")
        {
            ability.descriptionText.text =
                "While channeled, this character has +2 " + ReturnColoredText("Strength", yellow) + ", " +
                ReturnColoredText("Wisdom", yellow) + " and " +
                ReturnColoredText("Dexterity", yellow);
        }
        else if (ability.abilityName == "Blaze")
        {
            ability.descriptionText.text =
                "While channeled, your " + ReturnColoredText("Melee Attack", yellow) + " abilities deal " + ReturnColoredText("Fire", fire) +
                " damage";
        }
        else if (ability.abilityName == "Testudo")
        {
            ability.descriptionText.text =
                "While channeled, gain " +
                ReturnColoredText(CombatLogic.Instance.CalculateBlockGainedByEffect(3, entity).ToString(), yellow) +
                " " + ReturnColoredText("Block", yellow) + " on activation start";
        }
        else if (ability.abilityName == "Shadow Wreath")
        {
            ability.descriptionText.text =
                "While channeled, your " + ReturnColoredText("Melee Attack", yellow) + " abilities deal " + ReturnColoredText("Shadow", shadow) +
                " damage";
        }
        else if (ability.abilityName == "Creeping Frost")
        {
            ability.descriptionText.text =
                "While channeled, your " + ReturnColoredText("Melee Attack", yellow) + " abilities deal " + ReturnColoredText("Frost", frost) +
                " damage";
        }
        else if (ability.abilityName == "Overload")
        {
            ability.descriptionText.text =
                "While channeled, your " + ReturnColoredText("Melee Attack", yellow) + " abilities deal " + ReturnColoredText("Air", air) +
                " damage";
        }
        else if (ability.abilityName == "Infuse")
        {
            ability.descriptionText.text =
                "While channeled, you have " + ReturnColoredText("20", yellow) +
                " extra resistance to all damage types";
        }
        else if (ability.abilityName == "Concentration")
        {
            ability.descriptionText.text =
                "While channeled, your Ranged Attack abilities have +" + ReturnColoredText("20", yellow) +
                " critical chance and cannot be dodged";
        }
        else if (ability.abilityName == "Rapid Cloaking")
        {
            ability.descriptionText.text =
                "While channeled, gain " + ReturnColoredText("Camoflage", yellow) +
                " on activation end";
        }
        else if (ability.abilityName == "Recklessness")
        {
            ability.descriptionText.text =
                "While channeled, your " + ReturnColoredText("Melee Attack", yellow) + " abilities have +" + ReturnColoredText("20", yellow) +
                " Critical chance and cannot be parried";
        }
        else if (ability.abilityName == "Bless")
        {
            ability.descriptionText.text =
                "Remove " + ReturnColoredText("Weakened", yellow) + ", " +
                ReturnColoredText("Vulnerable", yellow) + " and " +
                ReturnColoredText("Stunned", yellow) + " from an ally";
        }
        else if (ability.abilityName == "Blight")
        {
            ability.descriptionText.text =
                "Apply " + ReturnColoredText((ability.abilityPrimaryValue + entity.myPassiveManager.venomousStacks).ToString(), yellow) +
                "  " +
                ReturnColoredText("Poisoned", poison) + " to a target";
        }
        else if (ability.abilityName == "Blinding Light")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("Blind", yellow) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Blizzard")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("Chilled", frost) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Toxic Eruption")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText((1 + entity.myPassiveManager.venomousStacks).ToString(), yellow) + " " +
                ReturnColoredText("Poisoned", poison) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Meteor")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("1", yellow) + " " +
                ReturnColoredText("Burning", fire) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Thunder Storm")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " +
                ReturnColoredText("Shocked", air) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Rain Of Chaos")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("Weakened", yellow) + " " +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Blink")
        {
            ability.descriptionText.text =
                "Teleport to a location within " + ReturnColoredText(ability.abilityRange.ToString(), yellow);
        }
        else if (ability.abilityName == "Blood Offering")
        {
            ability.descriptionText.text =
                "Lose " + ReturnColoredText(ability.abilitySecondaryValue.ToString(), yellow) +
                " health, then gain " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Energy", yellow);
        }
        else if (ability.abilityName == "Burst Of Knowledge")
        {
            ability.descriptionText.text =
                "Give an ally " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Temporary Wisdom", yellow);
        }
        else if (ability.abilityName == "Primal Rage")
        {
            ability.descriptionText.text =
                "Give an ally " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Temporary Strength", yellow);
        }
        else if (ability.abilityName == "Chain Lightning")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. Jumps to a random adjacent enemy 2 times";
        }
        else if (ability.abilityName == "Challenging Shout")
        {
            ability.descriptionText.text =
                "Force all enemies within " + ReturnColoredText(entity.currentMeleeRange.ToString(), yellow) +
                " to focus their on you during their next activations";
        }
        else if (ability.abilityName == "Cheap Shot")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. If you have " + ReturnColoredText("Camoflage", yellow) +
                ", apply " + ReturnColoredText("Vulnerable", yellow);
        }
        else if (ability.abilityName == "Chemical Reaction")
        {
            ability.descriptionText.text =
                "Double a targets current " + ReturnColoredText("Poisoned", yellow) +
                " amount";
        }
        else if (ability.abilityName == "Chilling Blow")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " + ReturnColoredText("Chilled", frost);
        }
        else if (ability.abilityName == "Combustion")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target for each stack of " + ReturnColoredText("Burning", yellow) + " on them";
        }
        else if (ability.abilityName == "Concealing Clouds")
        {
            ability.descriptionText.text =
                "Give " + ReturnColoredText("Camoflage", yellow) +
                " to all characters in a 3x3 area";

        }
        else if (ability.abilityName == "Consecrate")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to adjacent enemies. Give " + ReturnColoredText(ability.abilitySecondaryValue.ToString(), yellow) +
                " Energy to adjacent allies";
        }       
        else if (ability.abilityName == "Decapitate")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. Target is killed instantly if they have 20% or less health";
        }
        else if (ability.abilityName == "Devastating Blow")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Dimensional Blast")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) +
                " damage of a random damage type to a target";
        }
        else if (ability.abilityName == "Dimensional Hex")
        {
            ability.descriptionText.text =
                "Apply " + ReturnColoredText("Burning", fire) + ", " +
                ReturnColoredText("Poisoned", poison) + ", " +
                ReturnColoredText("Chilled", frost) + " and " +
                ReturnColoredText("Shocked", air) +
                " to a target";
        }
        else if (ability.abilityName == "Disarm")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " + ReturnColoredText("Disarmed", yellow);
        }
        else if (ability.abilityName == "Dragon Breath")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to all characters in a line, up to " + ReturnColoredText(ability.abilitySecondaryValue.ToString(), yellow) +
                " tiles away";
        }
        else if (ability.abilityName == "Drain")
        {
            ability.descriptionText.text =
                "Remove all " + ReturnColoredText("Poisoned", yellow) +
                " from a target. Deal " + ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage equal to amount removed";
        }
        else if (ability.abilityName == "Evasion")
        {
            ability.descriptionText.text =
                "Increase an ally's  " + ReturnColoredText("Parry", yellow) +
                " chance by  " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) +
                " until the end of the current turn cycle";
        }
        else if (ability.abilityName == "Mirage")
        {
            ability.descriptionText.text =
                "Increase an ally's  " + ReturnColoredText("Dodge", yellow) +
                " chance by  " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) +
                " until the end of the current turn cycle";
        }
        else if (ability.abilityName == "Fire Nova")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to adjacent enemies, and apply 1 " + ReturnColoredText("Burning", fire);
        }
        else if (ability.abilityName == "Frost Nova")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to adjacent enemies, and apply " + ReturnColoredText("Chilled", frost);
        }
        else if (ability.abilityName == "Noxious Fumes")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to adjacent enemies, and apply " +
                ReturnColoredText((ability.abilitySecondaryValue + entity.myPassiveManager.venomousStacks).ToString(), yellow) + " " +
                ReturnColoredText("Poisoned", poison);
        }
        else if (ability.abilityName == "Get Down!")
        {
            ability.descriptionText.text =
                "Move to a tile within " +
                ReturnColoredText((ability.abilityPrimaryValue + EntityLogic.GetTotalMobility(entity)).ToString(), yellow) +
                ". At the end of the movement, give " +
                ReturnColoredText(CombatLogic.Instance.CalculateBlockGainedByEffect(ability.abilitySecondaryValue, entity).ToString(), yellow) +
                " " + ReturnColoredText("Block", yellow) + " to adjacent allies";
        }
        else if (ability.abilityName == "Glacial Burst")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a random enemy in your melee range " + ReturnColoredText(ability.abilitySecondaryValue.ToString(), yellow) +
                " times";
        }
        else if (ability.abilityName == "Guard")
        {
            ability.descriptionText.text =
                "Give an ally " +
                ReturnColoredText(CombatLogic.Instance.CalculateBlockGainedByEffect(ability.abilityPrimaryValue, entity).ToString(), yellow) +
                " " + ReturnColoredText("Block", yellow);
        }
        else if (ability.abilityName == "Haste")
        {
            ability.descriptionText.text =
                "Give an ally " +
                ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Mobility", yellow) + " and " +
                ReturnColoredText("Initiative", yellow);
        }
        else if (ability.abilityName == "Head Shot")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Hex")
        {
            ability.descriptionText.text =
                "Apply " + ReturnColoredText("Weakened", yellow) + " and " +
                ReturnColoredText("Vulnerable", yellow) +
                " to a target";
        }
        else if (ability.abilityName == "Icy Focus")
        {
            ability.descriptionText.text =
                "Give an ally " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Wisdom", yellow);
        }
        else if (ability.abilityName == "Impaling Bolt")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " +
                ReturnColoredText(ability.abilitySecondaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Knock Back", yellow);
        }
        else if (ability.abilityName == "Judgement")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " +
                ReturnColoredText("Vulnerable", yellow) + " and " +
                ReturnColoredText("Weakened", yellow);
        }
        else if (ability.abilityName == "Kick To The Balls")
        {
            ability.descriptionText.text =
                "Apply " + ReturnColoredText("Stunned", yellow) + " to a target";
        }
        else if (ability.abilityName == "Nightmare")
        {
            ability.descriptionText.text =
                "Apply " + ReturnColoredText("Sleep", yellow) + " to a target";
        }
        else if (ability.abilityName == "Overwatch")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to the first enemy that moves within " +
                ReturnColoredText("5", yellow);
        }
        else if (ability.abilityName == "Phase Shift")
        {
            ability.descriptionText.text =
                "Switch locations with a target character";
        }
        else if (ability.abilityName == "Phoenix Dive")
        {
            ability.descriptionText.text =
                "Teleport to a location within " + ReturnColoredText(ability.abilityRange.ToString(), yellow) +
                ". On arrival, apply "+ ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Burning", fire) +
                " to adjacent enemies";         

        }
        else if (ability.abilityName == "Provoke")
        {
            ability.descriptionText.text =
                "Force an enemy within " + ReturnColoredText("1", yellow) +
                " to focus its attacks on you during its next activation";
        }
        else if (ability.abilityName == "Rapid Fire")
        {
            ability.descriptionText.text =
                "Spend all your Energy. For each " + ReturnColoredText("10", yellow) +
                " Energy spent, deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Slice And Dice")
        {
            ability.descriptionText.text =
                "Spend all your Energy. For each " + ReturnColoredText("10", yellow) +
                " Energy spent, deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Reactive Armour")
        {
            ability.descriptionText.text =
                "Remove all your " + ReturnColoredText("Block", yellow) +
                ", then deal " + ReturnColoredText(entity.currentBlock.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to all enemies within " + ReturnColoredText(entity.currentMeleeRange.ToString(), yellow);
        }
        else if (ability.abilityName == "Second Wind")
        {
            ability.descriptionText.text =
                "Gain " + ReturnColoredText("Energy", yellow) +
                " equal to your maximum";
        }
        else if (ability.abilityName == "Shadow Step")
        {
            ability.descriptionText.text =
                "Target an enemy within " + ReturnColoredText(ability.abilityRange.ToString(), yellow) +
                " and " + ReturnColoredText("Teleport", yellow) +
                " to their back tile";
        }
        else if (ability.abilityName == "Shank")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Sharpen Blade")
        {
            ability.descriptionText.text =
                "Your next Melee Attack is guaranteed to be a " + ReturnColoredText("Critical", yellow);
        }
        else if (ability.abilityName == "Shield Shatter")
        {
            ability.descriptionText.text =
                "Remove all " + ReturnColoredText("Block", yellow) +
                " from a target, then deal " + ReturnColoredText(damageValue.ToString(), yellow) +
                " " + ReturnColoredText(damageType, GetColorCodeFromString(damageType)) + " damage";
        }
        else if (ability.abilityName == "Shield Slam")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target, then apply 1 " + ReturnColoredText("Knock Back", yellow);
        }
        else if (ability.abilityName == "Shroud")
        {
            ability.descriptionText.text =
                "Give an ally " + ReturnColoredText("Camoflage", yellow);
        }
        else if (ability.abilityName == "Spirit Vision")
        {
            ability.descriptionText.text =
                "Give an ally " + ReturnColoredText("Temporary True Sight", yellow);
        }
        else if (ability.abilityName == "Smash")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " + ReturnColoredText(ability.abilitySecondaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Knock Back", yellow);
        }
        else if (ability.abilityName == "Snipe")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Spirit Surge")
        {
            ability.descriptionText.text =
                "Give an ally " +  ReturnColoredText(ability.abilitySecondaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Strength", yellow) + ", " +
                ReturnColoredText("Wisdom", yellow) + " and " +
                ReturnColoredText("Dexterity", yellow);
        }
        else if (ability.abilityName == "Steady Hands")
        {
            ability.descriptionText.text =
                "Give an ally " + ReturnColoredText("2", yellow) +
                " " + ReturnColoredText("Temporary Hawk Eye", yellow);
        }
        else if (ability.abilityName == "Super Conductor")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) +
                " " + ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to all enemies within " + ReturnColoredText(entity.currentMeleeRange.ToString(), yellow) +
                " and apply " + ReturnColoredText("Shocked", air) +
                ". If any enemies are already " + ReturnColoredText("Shocked", air) +
                " apply " + ReturnColoredText("Stunned", yellow) + " instead";
        }
        else if (ability.abilityName == "Sword And Board")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and gain " + 
                ReturnColoredText(CombatLogic.Instance.CalculateBlockGainedByEffect(ability.abilityPrimaryValue, entity).ToString(), yellow) +
                " " + ReturnColoredText("Block", yellow);
        }
        else if (ability.abilityName == "Tendon Slash")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("Weakened", yellow);
        }
        else if (ability.abilityName == "Toxic Slash")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + 
                ReturnColoredText((ability.abilityPrimaryValue + entity.myPassiveManager.venomousStacks).ToString(), yellow)
                + " "+ ReturnColoredText("Poisoned", poison);
        }
        else if (ability.abilityName == "Thunder Strike")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply "
                + " " + ReturnColoredText("Shocked", air);
        }
        else if (ability.abilityName == "Thaw")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage. If the target is " + ReturnColoredText("Chilled", frost) +
                ", refund the " + ReturnColoredText("Energy", yellow) + " spent";
        }
        else if (ability.abilityName == "Time Warp")
        {
            ability.descriptionText.text =
                "When target character finishes their next activation, they activate once more";
        }
        else if (ability.abilityName == "Transcendence")
        {
            ability.descriptionText.text =
                "Target ally becomes immune to all damage until the end of the current turn cycle";
        }
        else if (ability.abilityName == "Tree Leap")
        {
            ability.descriptionText.text =
               ReturnColoredText("Teleport", yellow) +
               " to a grass tile within " + ReturnColoredText(ability.abilityRange.ToString(), yellow);
        }
        else if (ability.abilityName == "Unbridled Chaos")
        {
            ability.descriptionText.text =
              "Deal " + ReturnColoredText(damageValue.ToString(), yellow) +
              " " + ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
              " damage to a random character (yourself included) within " + 
              ReturnColoredText("3", yellow) +" tiles " +
              ReturnColoredText(ability.abilitySecondaryValue.ToString(), yellow) +
              " times";
        }
        else if (ability.abilityName == "Vanish")
        {
            ability.descriptionText.text =
              "Immediately gain " + ReturnColoredText("Camoflage", yellow);
        }
    }
    public static void SetAbilityDescriptionText(AbilityDataSO ability, TextMeshProUGUI descriptionText)
    {
        // Set up
        string damageValue;
        string damageType;

        // Setup for abilities that use a weapon
        if (ability.requiresMeleeWeapon ||
            ability.requiresRangedWeapon)
        {
            damageValue = (ability.weaponDamagePercentage * 100).ToString() + "%";
            damageType = "weapon";
        }
        // Setup for abilities that DONT use a weapon
        else
        {
            damageValue = ability.primaryValue.ToString();
            damageType = ability.damageType.ToString();
        }   
        

        // Set Text
        if (ability.abilityName == "Strike")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Twin Strike")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " main hand " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage, then " +
                ReturnColoredText(damageValue.ToString(), yellow) + " off hand " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " to a target";
        }
        else if (ability.abilityName == "Shoot")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Move")
        {
            descriptionText.text = "Move to a tile within your " + ReturnColoredText("Mobility", yellow);
        }
        else if (ability.abilityName == "Defend")
        {
            descriptionText.text =
                "Gain " + ReturnColoredText(ability.primaryValue.ToString(), yellow)
                + " " + ReturnColoredText("Block", yellow);
        }
        else if (ability.abilityName == "Inspire")
        {
            descriptionText.text =
                "Increase a target's " + ReturnColoredText("Strength", yellow) + " by " +
                ReturnColoredText(ability.primaryValue.ToString(), yellow);
        }
        else if (ability.abilityName == "Charge")
        {
            descriptionText.text =
                "Move to a target enemy within " + ReturnColoredText("Mobility", yellow) + " + " +
                ReturnColoredText(ability.primaryValue.ToString(), yellow) +
                " and apply " +
                ReturnColoredText("Vulnerable", yellow);
        }
        else if (ability.abilityName == "Whirlwind")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to all enemies within " +
                ReturnColoredText("1", yellow);
        }
        else if (ability.abilityName == "Fire Ball")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Lightning Bolt")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " + ReturnColoredText("Shocked", air);
        }
        else if (ability.abilityName == "Frost Bolt")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " +
                ReturnColoredText("Immobilized", yellow);
        }
        else if (ability.abilityName == "Telekinesis")
        {
            descriptionText.text =
                ReturnColoredText("Teleport", yellow) + " a target anywhere within " + ReturnColoredText(ability.primaryValue.ToString(), yellow) +
                " of its current position";
        }
        else if (ability.abilityName == "Dash")
        {
            descriptionText.text =
                "Move to a tile within " + ReturnColoredText("Mobility", yellow) +
                " + " + ReturnColoredText(ability.primaryValue.ToString(), yellow) + 
                " of your current position";
        }
        else if (ability.abilityName == "Preparation")
        {
            descriptionText.text =
                "The next ability you use costs " + ReturnColoredText("0", yellow) +
                " Energy";
        }
        else if (ability.abilityName == "Holy Fire")
        {
            descriptionText.text =
                "Give " + ReturnColoredText(ability.primaryValue.ToString(), yellow)
                + " " +
                ReturnColoredText("Block", yellow) +
                " to an ally, or deal " +
                ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to an enemy";
        }
        else if (ability.abilityName == "Invigorate")
        {
            descriptionText.text =
                "Give an ally " + ReturnColoredText(ability.primaryValue.ToString(), yellow) +
                " Energy";
        }
        else if (ability.abilityName == "Chaos Bolt")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target, and apply " +
                ReturnColoredText("Vulnerable", yellow);
        }
        else if (ability.abilityName == "Ambush")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. If this back strikes, gain " +
                ReturnColoredText(ability.secondaryValue.ToString(), yellow) +
                ReturnColoredText(" Energy", yellow);
        }
        else if (ability.abilityName == "Blade Flurry")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a random enemy within " +
                ReturnColoredText("1", yellow) +
                " three times";
        }
        else if (ability.abilityName == "Purity")
        {
            descriptionText.text =
                "While channeled, this character has +2 " + ReturnColoredText("Strength", yellow) + ", " +
                ReturnColoredText("Wisdom", yellow) + " and " +
                ReturnColoredText("Dexterity", yellow);
        }
        else if (ability.abilityName == "Blaze")
        {
            descriptionText.text =
                "While channeled, your Melee Attack abilities deal " + ReturnColoredText("Fire", fire) +
                " damage";
        }
        else if (ability.abilityName == "Testudo")
        {
            descriptionText.text =
                "While channeled, gain " +
                ReturnColoredText("3", yellow) +
                " " + ReturnColoredText("Block", yellow) + " on activation start";
        }
        else if (ability.abilityName == "Shadow Wreath")
        {
            descriptionText.text =
                "While channeled, your Melee Attack abilities deal " + ReturnColoredText("Shadow", shadow) +
                " damage";
        }
        else if (ability.abilityName == "Creeping Frost")
        {
            descriptionText.text =
                "While channeled, your Melee Attack abilities deal " + ReturnColoredText("Frost", frost) +
                " damage";
        }
        else if (ability.abilityName == "Overload")
        {
            descriptionText.text =
                "While channeled, your Melee Attack abilities deal " + ReturnColoredText("Air", air) +
                " damage";
        }
        else if (ability.abilityName == "Infuse")
        {
            descriptionText.text =
                "While channeled, you have " + ReturnColoredText("20", yellow) +
                " extra resistance to all damage types";
        }
        else if (ability.abilityName == "Concentration")
        {
            descriptionText.text =
                "While channeled, your Ranged Attack abilities have +" + ReturnColoredText("20", yellow) +
                " Critical chance and cannot be dodged";
        }
        else if (ability.abilityName == "Rapid Cloaking")
        {
            descriptionText.text =
                "While channeled, gain " + ReturnColoredText("Camoflage", yellow) +
                " on activation end";
        }
        else if (ability.abilityName == "Recklessness")
        {
            descriptionText.text =
                "While channeled, your " + ReturnColoredText("Melee Attack", yellow) + " abilities have +" + ReturnColoredText("20", yellow) +
                " Critical chance and cannot be parried";
        }
        else if (ability.abilityName == "Bless")
        {
            descriptionText.text =
                "Remove " + ReturnColoredText("Weakened", yellow) + ", " +
                ReturnColoredText("Vulnerable", yellow) + " and " +
                ReturnColoredText("Stunned", yellow) + " from an ally";
        }
        else if (ability.abilityName == "Blight")
        {
            descriptionText.text =
                "Apply " + ReturnColoredText(ability.primaryValue.ToString(), yellow) +
                "  " +
                ReturnColoredText("Poisoned", poison) + " to a target";
        }
        else if (ability.abilityName == "Blinding Light")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("Blind", yellow) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Blizzard")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("Chilled", frost) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Toxic Eruption")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText(ability.primaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Poisoned", poison) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Meteor")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("1", yellow) + " " +
                ReturnColoredText("Burning", fire) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Thunder Storm")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " +
                ReturnColoredText("Shocked", air) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Rain Of Chaos")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("Weakened", yellow) + " " +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Blink")
        {
            descriptionText.text =
                ReturnColoredText("Teleport", yellow) + 
                " to a location within " + ReturnColoredText(ability.range.ToString(), yellow);
        }
        else if (ability.abilityName == "Blood Offering")
        {
            descriptionText.text =
                "Lose " + ReturnColoredText(ability.secondaryValue.ToString(), yellow) +
                " health, then gain " + ReturnColoredText(ability.primaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Energy", yellow);
        }
        else if (ability.abilityName == "Burst Of Knowledge")
        {
            descriptionText.text =
                "Give an ally " + ReturnColoredText(ability.primaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Temporary Wisdom", yellow);
        }
        else if (ability.abilityName == "Primal Rage")
        {
            descriptionText.text =
                "Give an ally " + ReturnColoredText(ability.primaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Temporary Strength", yellow);
        }
        else if (ability.abilityName == "Chain Lightning")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. Jumps to a random adjacent enemy 2 times";
        }
        else if (ability.abilityName == "Challenging Shout")
        {
            descriptionText.text =
                "Force all enemies within " + ReturnColoredText("1", yellow) +
                " to focus their attacks on you during their next activations";
        }
        else if (ability.abilityName == "Cheap Shot")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. If you have " + ReturnColoredText("Camoflage", yellow) +
                ", apply " + ReturnColoredText("Vulnerable", yellow);
        }
        else if (ability.abilityName == "Chemical Reaction")
        {
            descriptionText.text =
                "Double a targets current " + ReturnColoredText("Poisoned", yellow) +
                " amount";
        }
        else if (ability.abilityName == "Chilling Blow")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " + ReturnColoredText("Chilled", frost);
        }
        else if (ability.abilityName == "Combustion")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target for each stack of " + ReturnColoredText("Burning", fire) + " on them";
        }
        else if (ability.abilityName == "Concealing Clouds")
        {
            descriptionText.text =
                "Give " + ReturnColoredText("Camoflage", yellow) +
                " to all characters in a 3x3 area";

        }
        else if (ability.abilityName == "Consecrate")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to adjacent enemies. Give " + ReturnColoredText(ability.secondaryValue.ToString(), yellow) +
                " Energy to adjacent allies";
        }
        else if (ability.abilityName == "Decapitate")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. Target is killed instantly if they have 20% or less health";
        }
        else if (ability.abilityName == "Devastating Blow")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Dimensional Blast")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) +
                " damage of a random damage type to a target";
        }
        else if (ability.abilityName == "Dimensional Hex")
        {
            descriptionText.text =
                "Apply " + ReturnColoredText("Burning", fire) + ", " +
                ReturnColoredText("Poisoned", poison) + ", " +
                ReturnColoredText("Chilled", frost) + " and " +
                ReturnColoredText("Shocked", air) +
                " to a target";
        }
        else if (ability.abilityName == "Disarm")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " + ReturnColoredText("Disarmed", yellow);
        }
        else if (ability.abilityName == "Dragon Breath")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to all characters in a line, up to " + ReturnColoredText(ability.secondaryValue.ToString(), yellow) +
                " tiles away";
        }
        else if (ability.abilityName == "Drain")
        {
            descriptionText.text =
                "Remove all " + ReturnColoredText("Poisoned", yellow) +
                " from a target. Deal " + ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage equal to amount removed";
        }
        else if (ability.abilityName == "Evasion")
        {
            descriptionText.text =
                "Increase an ally's  " + ReturnColoredText("Parry", yellow) +
                " chance by " + ReturnColoredText(ability.primaryValue.ToString(), yellow) +
                " until the end of the current turn cycle";
        }
        else if (ability.abilityName == "Mirage")
        {
            descriptionText.text =
                "Increase an ally's  " + ReturnColoredText("Dodge", yellow) +
                " chance by  " + ReturnColoredText(ability.primaryValue.ToString(), yellow) +
                " until the end of the current turn cycle";
        }
        else if (ability.abilityName == "Fire Nova")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to adjacent enemies, and apply " +
                ReturnColoredText("1", yellow)+ " " +
                ReturnColoredText("Burning", fire);
        }
        else if (ability.abilityName == "Frost Nova")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to adjacent enemies, and apply " + ReturnColoredText("Chilled", frost);
        }
        else if (ability.abilityName == "Noxious Fumes")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to adjacent enemies, and apply " +
                ReturnColoredText(ability.secondaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Poisoned", poison);
        }
        else if (ability.abilityName == "Get Down!")
        {
            descriptionText.text =
                "Move to a tile within " + ReturnColoredText("Mobility", yellow) +
                " + " + ReturnColoredText(ability.primaryValue.ToString(), yellow) +
                ". At the end of the movement, give " +
                ReturnColoredText(ability.secondaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Block", yellow) + " to adjacent allies";
        }
        else if (ability.abilityName == "Glacial Burst")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a random enemy in your melee range " + ReturnColoredText(ability.secondaryValue.ToString(), yellow) +
                " times";
        }
        else if (ability.abilityName == "Guard")
        {
            descriptionText.text =
                "Give an ally " +
                ReturnColoredText(ability.primaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Block", yellow);
        }
        else if (ability.abilityName == "Haste")
        {
            descriptionText.text =
                "Give an ally " +
                ReturnColoredText(ability.primaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Mobility", yellow) + " and " +
                ReturnColoredText("Initiative", yellow);
        }
        else if (ability.abilityName == "Head Shot")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Hex")
        {
            descriptionText.text =
                "Apply " + ReturnColoredText("Weakened", yellow) + " and " +
                ReturnColoredText("Vulnerable", yellow) +
                " to a target";
        }
        else if (ability.abilityName == "Icy Focus")
        {
            descriptionText.text =
                "Give an ally " + ReturnColoredText(ability.primaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Wisdom", yellow);
        }
        else if (ability.abilityName == "Impaling Bolt")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " +
                ReturnColoredText(ability.secondaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Knock Back", yellow);
        }
        else if (ability.abilityName == "Judgement")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " +
                ReturnColoredText("Vulnerable", yellow) + " and " +
                ReturnColoredText("Weakened", yellow);
        }
        else if (ability.abilityName == "Kick To The Balls")
        {
            descriptionText.text =
                "Apply " + ReturnColoredText("Stunned", yellow) + " to a target";
        }
        else if (ability.abilityName == "Nightmare")
        {
            descriptionText.text =
                "Apply " + ReturnColoredText("Sleep", yellow) + " to a target";
        }
        else if (ability.abilityName == "Overwatch")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to the first enemy that moves within " +
                ReturnColoredText("5", yellow);
        }
        else if (ability.abilityName == "Phase Shift")
        {
            descriptionText.text =
                "Switch locations with a target character";
        }
        else if (ability.abilityName == "Phoenix Dive")
        {
            descriptionText.text =
                "Teleport to a location within " + ReturnColoredText(ability.range.ToString(), yellow) +
                ". On arrival, apply " + ReturnColoredText("1", yellow) + " " +
                ReturnColoredText("Burning", fire) +
                " to adjacent enemies";

        }
        else if (ability.abilityName == "Provoke")
        {
            descriptionText.text =
                "Force an enemy within " + ReturnColoredText("1", yellow) +
                " to focus its attacks on you during its next activation";
        }
        else if (ability.abilityName == "Rapid Fire")
        {
            descriptionText.text =
                "Spend all your Energy. For each " + ReturnColoredText("10", yellow) +
                " Energy spent, deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Slice And Dice")
        {
            descriptionText.text =
                "Spend all your Energy. For each " + ReturnColoredText("10", yellow) +
                " Energy spent, deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Reactive Armour")
        {
            descriptionText.text =
                "Remove all your " + ReturnColoredText("Block", yellow) +
                ", then deal that amount in " + 
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to all enemies within " + ReturnColoredText("1", yellow);
        }
        else if (ability.abilityName == "Second Wind")
        {
            descriptionText.text =
                "Gain " + ReturnColoredText("Energy", yellow) +
                " equal to your maximum";
        }
        else if (ability.abilityName == "Shadow Step")
        {
            descriptionText.text =
                "Target an enemy within " + ReturnColoredText(ability.range.ToString(), yellow) +
                " and " + ReturnColoredText("Teleport", yellow) +
                " to their back tile";
        }
        else if (ability.abilityName == "Shank")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Sharpen Blade")
        {
            descriptionText.text =
                "Your next Melee Attack is guaranteed to be a " + ReturnColoredText("Critical", yellow);
        }
        else if (ability.abilityName == "Shield Shatter")
        {
            descriptionText.text =
                "Remove all " + ReturnColoredText("Block", yellow) +
                " from a target, then deal " + ReturnColoredText(damageValue.ToString(), yellow) +
                " " + ReturnColoredText(damageType, GetColorCodeFromString(damageType)) + " damage";
        }
        else if (ability.abilityName == "Shield Slam")
        {
            descriptionText.text =
                "Deal " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target equal to your current " + ReturnColoredText("Block", yellow) + 
                ", then apply 1 " + ReturnColoredText("Knock Back", yellow);
        }
        else if (ability.abilityName == "Shroud")
        {
            descriptionText.text =
                "Give an ally " + ReturnColoredText("Camoflage", yellow);
        }
        else if (ability.abilityName == "Spirit Vision")
        {
            descriptionText.text =
                "Give an ally " + ReturnColoredText("Temporary True Sight", yellow);
        }
        else if (ability.abilityName == "Smash")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " + ReturnColoredText(ability.secondaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Knock Back", yellow);
        }
        else if (ability.abilityName == "Snipe")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Spirit Surge")
        {
            descriptionText.text =
                "Give an ally " + ReturnColoredText(ability.secondaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Strength", yellow) + ", " +
                ReturnColoredText("Wisdom", yellow) + " and " +
                ReturnColoredText("Dexterity", yellow);
        }
        else if (ability.abilityName == "Steady Hands")
        {
            descriptionText.text =
                "Give an ally " + ReturnColoredText("2", yellow) +
                " " + ReturnColoredText("Temporary Hawk Eye", yellow);
        }
        else if (ability.abilityName == "Super Conductor")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) +
                " " + ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to all enemies within " + ReturnColoredText("1", yellow) +
                " and apply " + ReturnColoredText("Shocked", yellow) +
                ". If any enemies are already " + ReturnColoredText("Shocked", yellow) +
                " apply " + ReturnColoredText("Stunned", yellow) + " instead";
        }
        else if (ability.abilityName == "Sword And Board")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and gain " +
                ReturnColoredText(ability.primaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Block", yellow);
        }
        else if (ability.abilityName == "Tendon Slash")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("Weakened", yellow);
        }
        else if (ability.abilityName == "Toxic Slash")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " +
                ReturnColoredText(ability.primaryValue.ToString(), yellow)
                + " " + ReturnColoredText("Poisoned", poison);
        }
        else if (ability.abilityName == "Thunder Strike")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply "
                + " " + ReturnColoredText("Shocked", air);
        }
        else if (ability.abilityName == "Thaw")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage. If the target is " + ReturnColoredText("Chilled", frost) +
                ", refund the " + ReturnColoredText("Energy", yellow) + " spent";
        }
        else if (ability.abilityName == "Time Warp")
        {
            descriptionText.text =
                "When target character finishes their next activation, they activate once more";
        }
        else if (ability.abilityName == "Transcendence")
        {
            descriptionText.text =
                "Target ally becomes immune to all damage until the end of the current turn cycle";
        }
        else if (ability.abilityName == "Tree Leap")
        {
            descriptionText.text =
               ReturnColoredText("Teleport", yellow) +
               " to a grass tile within " + ReturnColoredText(ability.range.ToString(), yellow);
        }
        else if (ability.abilityName == "Unbridled Chaos")
        {
            descriptionText.text =
              "Deal " + ReturnColoredText(damageValue.ToString(), yellow) +
              " " + ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
              " damage to a random character (yourself included) within " +
              ReturnColoredText("3", yellow) + " tiles " +
              ReturnColoredText(ability.secondaryValue.ToString(), yellow) +
              " times";
        }
        else if (ability.abilityName == "Vanish")
        {
            descriptionText.text =
              "Immediately gain " + ReturnColoredText("Camoflage", yellow);
        }
    }
    public static string ReturnColoredText(string text, string color)
    {
        return (color + text + white);
    }
    public static string GetColorCodeFromString(string damageType)
    {
        string colorCodeReturned = "";

        if(damageType == "Physical")
        {
            colorCodeReturned = physical; 
        }
        else if (damageType == "Fire")
        {
            colorCodeReturned = fire;
        }
        else if (damageType == "Frost")
        {
            colorCodeReturned = frost;
        }
        else if (damageType == "Poison")
        {
            colorCodeReturned = poison;
        }
        else if (damageType == "Shadow")
        {
            colorCodeReturned = shadow;
        }
        else if (damageType == "Air")
        {
            colorCodeReturned = air;
        }
        else
        {
            colorCodeReturned = white;
        }

        return colorCodeReturned;
    }
    public static string GetMouseOverElementString(string elementName)
    {
        string stringReturned = "";

        if(elementName == "Strength")
        {
            stringReturned =
                "Increases the base damage of all " + ReturnColoredText("Melee Attack", yellow) + " abilities";
        }
        else if (elementName == "Wisdom")
        {
            stringReturned =
                "Increases all " +
                ReturnColoredText("Fire", fire) + ", " +
                ReturnColoredText("Frost", frost) + ", " +
                ReturnColoredText("Poison", poison) + ", " +
                ReturnColoredText("Shadow", shadow) + ", and " +
                ReturnColoredText("Air", air) +
                " damage from abilities and effects";
        }
        else if (elementName == "Dexterity")
        {
            stringReturned =
                "Increases the amount of " + ReturnColoredText("Block", yellow) +
                " granted from abilities and effects";
        }
        else if (elementName == "Stamina")
        {
            stringReturned =
                "The amount of " + ReturnColoredText("Energy", yellow) +
                " this character gains at the start of their activation";
        }
        else if (elementName == "Mobility")
        {
            stringReturned =
                "The range in tiles this character can travel with movement abilities";
        }
        else if (elementName == "Initiative")
        {
            stringReturned =
                "The value added to a character's turn order roll";
        }
        else if (elementName == "Critical")
        {
            stringReturned =
                "The percentage chance this character has to deal " + ReturnColoredText("50%", yellow) + " bonus damage with "
                + ReturnColoredText("Melee Attack", yellow) + " and " + ReturnColoredText("Ranged Attack", yellow) + " abilities";
        }
        else if (elementName == "Dodge")
        {
            stringReturned =
                "The percentage chance this character has to completelty avoid damage from "
                + ReturnColoredText("Ranged Attack", yellow) + " abilities";
        }
        else if (elementName == "Parry")
        {
            stringReturned =
                "The percentage chance this character has to completelty avoid damage from "
                + ReturnColoredText("Melee Attack", yellow) + " abilities";
        }
        else if (elementName == "Aura Size")
        {
            stringReturned =
                "The range in tiles this character's aura effect passives and abilities will affect";
        }
        else if (elementName == "Melee Range")
        {
            stringReturned =
                "The range in tiles this character's " + ReturnColoredText("Melee Attack", yellow) +
                " abilities can target";
        }
        else if (elementName == "Max Energy")
        {
            stringReturned =
                "The maximum amount of " + ReturnColoredText("Energy", yellow) +
                " this character can have at any one time";
        }
        else if (elementName == "Fire Resistance")
        {
            stringReturned =
                "Reduces all incoming " + ReturnColoredText("Fire", fire) + " damage dealt to this character";
        }
        else if (elementName == "Poison Resistance")
        {
            stringReturned =
                "Reduces all incoming " + ReturnColoredText("Poison", poison) + " damage dealt to this character";
        }
        else if (elementName == "Frost Resistance")
        {
            stringReturned =
                "Reduces all incoming " + ReturnColoredText("Frost", frost) + " damage dealt to this character";
        }
        else if (elementName == "Shadow Resistance")
        {
            stringReturned =
                "Reduces all incoming " + ReturnColoredText("Shadow", shadow) + " damage dealt to this character";
        }
        else if (elementName == "Air Resistance")
        {
            stringReturned =
                "Reduces all incoming " + ReturnColoredText("Air", air) + " damage dealt to this character";
        }
        else if (elementName == "Physical Resistance")
        {
            stringReturned =
                "Reduces all incoming " + ReturnColoredText("Physical", physical) + " damage dealt to this character";
        }
        else if (elementName == "Health")
        {
            stringReturned =
                "The amount of damage this character can take before it dies";
        }
        else if (elementName == "XP")
        {
            stringReturned =
                "The amount of XP points required to level up. When a character levels up, they gain 1 "
                + ReturnColoredText("Talent Point", yellow) + " and 1 "
                + ReturnColoredText("Ability Point", yellow);
        }        
        else if (elementName == "Ability Points")
        {
            stringReturned =
                "Used to purchase abilities and passives from a talent tree";
        }
        else if (elementName == "Talent Points")
        {
            stringReturned =
                "Used to increase talent pool levels. Placing 1 point in a talent pool unlocks it's talent tree. Additional points placed " +
                " unlock higher tiers within that talent tree";
        }

        return stringReturned;
    }
    public static string GetItemCardEffectDescriptionString(string elementName)
    {
        string stringReturned = "";

        if(elementName == "BonusStrength")
        {

        }
        else if (elementName == "BonusStrength")
        {

        }



        return stringReturned;
    }

}
