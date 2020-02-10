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
                "Increases the base damage of Melee Attacks by" + ReturnColoredText(statusStacks.ToString(), yellow);
        }
        else if (statusName == "Temporary Bonus Strength")
        {
            statusDescriptionText.text =
                "Increases the base damage of Melee Attacks by" + ReturnColoredText(statusStacks.ToString(), yellow) +
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
                "On activation end, take " + ReturnColoredText(statusStacks.ToString(), yellow) + " damage";
        }
        else if (statusName == "Poisoned")
        {
            statusDescriptionText.text =
                "On activation end, take " + ReturnColoredText(statusStacks.ToString(), yellow) + " damage";
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
                + " tile away. Cancelled by moving off a " + ReturnColoredText("Grass", yellow) + " tile";
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
                ReturnColoredText("Initiative", yellow) + ". Expire on activation end";
        }
        else if (statusName == "Concentration")
        {
            statusDescriptionText.text =
                "This character's Ranged Attack abilities cannot be dodged. +20 Ranged Critical Chance";
        }
        else if (statusName == "Demon")
        {
            statusDescriptionText.text =
                "Increase all "+ ReturnColoredText("Fire", fire) + " damage dealt by 20%. +20 " + 
                ReturnColoredText("Fire", fire) + " Resistance";
        }
        else if (statusName == "Shadow Form")
        {
            statusDescriptionText.text =
                "Increase all " + ReturnColoredText("Shadow", shadow) + " damage dealt by 20%. +20 " +
                ReturnColoredText("Shadow", shadow) + " Resistance";
        }
        else if (statusName == "Toxicity")
        {
            statusDescriptionText.text =
                "Increase all " + ReturnColoredText("Poison", poison) + " damage dealt by 20%. +20 " +
                ReturnColoredText("Poison", poison) + " Resistance";
        }
        else if (statusName == "Storm Lord")
        {
            statusDescriptionText.text =
                "Increase all " + ReturnColoredText("Air", air) + " damage dealt by 20%. +20 " +
                ReturnColoredText("Air", air) + " Resistance";
        }
        else if (statusName == "Frozen Heart")
        {
            statusDescriptionText.text =
                "Increase all " + ReturnColoredText("Frost", frost) + " damage dealt by 20%. +20 " +
                ReturnColoredText("Frost", frost) + " Resistance";
        }
        else if (statusName == "Disarmed")
        {
            statusDescriptionText.text =
                "This character cannot use Melee Attacks. Expires on activation end";
        }
        else if (statusName == "Blind")
        {
            statusDescriptionText.text =
                "This character cannot use Ranged Attacks. Expires on activation end";
        }
        else if (statusName == "Silenced")
        {
            statusDescriptionText.text =
                "This character cannot use Skills. Expires on activation end";
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
                "This character skips its next activation, and cannot take any actions. Removed by taking damage";
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
                "On activation end, this character applies " + ReturnColoredText(statusStacks.ToString(), yellow) +
                 " Poisoned to enemies within it's Aura";
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
                + "% extra damage with Melee Attacks when back striking";
        }
        else if (statusName == "Hawk Eye")
        {
            statusDescriptionText.text =
                "This character has +" + ReturnColoredText(statusStacks.ToString(), yellow)
                + " range with Ranged Attack abilities";
        }
        else if (statusName == "Temporary Hawk Eye")
        {
            statusDescriptionText.text =
                "This character has +" + ReturnColoredText(statusStacks.ToString(), yellow)
                + " range with Ranged Attack abilities. Expires on activation end";
        }

        else if (statusName == "Ethereal Being")
        {
            statusDescriptionText.text =
                "This character ignores line of sight when targetting";
        }

        else if (statusName == "True Sight")
        {
            statusDescriptionText.text =
                "This character ignores Camoflage when attacking";
        }
        else if (statusName == "Temporary True Sight")
        {
            statusDescriptionText.text =
                "This character ignores Camoflage when attacking. Expires on activation end";
        }
        else if (statusName == "Overwatch")
        {
            statusDescriptionText.text =
                "This character will perform 'Shoot' against the first enemy that moves within its weapon range";
        }

        else if (statusName == "Fiery Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with Melee Attacks into " +
                ReturnColoredText("Fire", fire) + " damage";
        }
        else if (statusName == "Temporary Fiery Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with Melee Attacks into " +
                ReturnColoredText("Fire", fire) + " damage. Expires on activation end";
        }
        else if (statusName == "Shadow Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with Melee Attacks into " +
                ReturnColoredText("Shadow", shadow) + " damage";
        }
        else if (statusName == "Temporary Shadow Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with Melee Attacks into " +
                ReturnColoredText("Shadow", shadow) + " damage. Expires on activation end";
        }
        else if (statusName == "Frost Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with Melee Attacks into " +
                ReturnColoredText("Frost", frost) + " damage";
        }
        else if (statusName == "Temporary Frost Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with Melee Attacks into " +
                ReturnColoredText("Frost", frost) + " damage. Expires on activation end";
        }
        else if (statusName == "Poison Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with Melee Attacks into " +
                ReturnColoredText("Poison", poison) + " damage";
        }
        else if (statusName == "Temporary Poison Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with Melee Attacks into " +
                ReturnColoredText("Poison", poison) + " damage. Expires on activation end";
        }
        else if (statusName == "Air Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with Melee Attacks into " +
                ReturnColoredText("Air", air) + " damage";
        }
        else if (statusName == "Temporary Air Imbuement")
        {
            statusDescriptionText.text =
                "This character coverts all damage with Melee Attacks into " +
                ReturnColoredText("Air", air) + " damage. Expires on activation end";
        }
        else if (statusName == "Flux")
        {
            statusDescriptionText.text =
                "This character's first 'Move' ability each activation costs 0 Energy";
        }
        else if (statusName == "Transcendence")
        {
            statusDescriptionText.text =
                "This character is immune to all damage until the end of the current turn cycle";
        }
        else if (statusName == "Immolation")
        {
            statusDescriptionText.text =
                "Whenever this character reduces health with a melee attack, it applies " + ReturnColoredText(statusStacks.ToString(), yellow) + " Burning";
        }
        else if (statusName == "Poisonous")
        {
            statusDescriptionText.text =
                "Whenever this character reduces health with a melee attack, it applies " + ReturnColoredText(statusStacks.ToString(), yellow) + " Poisoned";
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
                "This character is immune to Weakened and Vulnerable";
        }
        else if (statusName == "Unfallible")
        {
            statusDescriptionText.text =
                "This character is immune to Blind, Silenced and Disarmed";
        }
        else if (statusName == "Undead")
        {
            statusDescriptionText.text =
                "This character is immune Poisoned and Terrified";
        }
        else if (statusName == "Unstoppable")
        {
            statusDescriptionText.text =
                "This character is immune to Immobilized and Knock Back";
        }
        else if (statusName == "Unwavering")
        {
            statusDescriptionText.text =
                "This character's Block does not expire on activation start";
        }
        else if (statusName == "Infuse")
        {
            statusDescriptionText.text =
                "This character has +20 to all resistances";
        }
        else if (statusName == "Testudo")
        {
            statusDescriptionText.text =
                "On activation start, this character gains " + ReturnColoredText("5", yellow);
        }
        else if (statusName == "Last Stand")
        {
            statusDescriptionText.text =
                "The first time this character would take lethal damage, it gains 5 Strength, and its health is set at 1";
        }
        else if (statusName == "Rapid Cloaking")
        {
            statusDescriptionText.text =
                "On activation end, this character gains Camoflage";
        }
        else if (statusName == "Life Steal")
        {
            statusDescriptionText.text =
                "Whenever this character reduces health with a melee attack, it heals that much health";
        }
        else if (statusName == "Masochist")
        {
            statusDescriptionText.text =
                "While this character has 50% or less health, it has +20 Critical Chance";
        }
        else if (statusName == "Nimble")
        {
            statusDescriptionText.text =
                "This character's Dodge and Parry chance is increased by 10";
        }
        else if (statusName == "Perfect Reflexes")
        {
            statusDescriptionText.text =
                "This character's Dodge and Parry chance is increased by 20";
        }
        else if (statusName == "Patient Stalker")
        {
            statusDescriptionText.text =
                "This character has +1 Mobility and +20 Critical Strike chance while Camoflaged";
        }
        else if (statusName == "Perfect Aim")
        {
            statusDescriptionText.text =
                "This character's Ranged Attacks cannot be dodged";
        }
        else if (statusName == "Phasing")
        {
            statusDescriptionText.text =
                "The first time this character is attacked each turn cycle, it teleports to a random tile within 2";
        }
        else if (statusName == "Poison Immunity")
        {
            statusDescriptionText.text =
                "This character cannot be Poisoned";
        }
        else if (statusName == "Predator")
        {
            statusDescriptionText.text =
                "This character has +20 Critical Chance while Camoflaged";
        }
        else if (statusName == "Preparation")
        {
            statusDescriptionText.text =
                "The next ability this character uses costs 0 Energy";
        }
        else if (statusName == "Purity")
        {
            statusDescriptionText.text =
                "This character has +2 Strength, Wisdom and Dexterity";
        }
        else if (statusName == "Recklessness")
        {
            statusDescriptionText.text =
                "This character has +20 Melee Critical Chance and its Melee Attacks cannot be parried";
        }
        else if (statusName == "Riposte")
        {
            statusDescriptionText.text =
                "Whenever this character parries an attack, it performs 'Strike' against its attacker";
        }
        else if (statusName == "Sacred Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character removes Blind, Immobilized, Disarmed, Silenced and Terrified" +
                " from allies within its Aura";
        }
        else if (statusName == "Shadow Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character applies" +
                " Weakened to enemies within its Aura";
        }
        else if (statusName == "Sharpened Blade")
        {
            statusDescriptionText.text =
                "This character's next Melee Attack is guaranteed to critically strike";
        }
        else if (statusName == "Shatter")
        {
            statusDescriptionText.text =
                "This character has +20 Critical Chance when attacking targets with Chilled";
        }
        else if (statusName == "Slippery")
        {
            statusDescriptionText.text =
                "This character is immune to free strikes";
        }
        else if (statusName == "Stealth")
        {
            statusDescriptionText.text =
                "This character is permanently Camoflaged";
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
                "Whenever this character is hit with a melee attack, it deals " +
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
                "This character's Melee Attacks cannot be parried";
        }
        else if (statusName == "Venomous")
        {
            statusDescriptionText.text =
                "Whenever this character applies Poisoned, it applies " + ReturnColoredText(statusStacks.ToString(), yellow) + " extra";
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
                " damage to a target and apply " + ReturnColoredText("Shocked", yellow);
        }
        else if (ability.abilityName == "Frost Bolt")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " +
                ReturnColoredText("Pinned", yellow);
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
                "While channeled, your Melee Attack abilities deal " + ReturnColoredText("Fire", fire) +
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
                "While channeled, your Melee Attack abilities deal " + ReturnColoredText("Shadow", shadow) +
                " damage";
        }
        else if (ability.abilityName == "Creeping Frost")
        {
            ability.descriptionText.text =
                "While channeled, your Melee Attack abilities deal " + ReturnColoredText("Frost", frost) +
                " damage";
        }
        else if (ability.abilityName == "Overload")
        {
            ability.descriptionText.text =
                "While channeled, your Melee Attack abilities deal " + ReturnColoredText("Air", air) +
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
                "While channeled, your Melee Attack abilities have +" + ReturnColoredText("20", yellow) +
                " critical chance and cannot be parried";
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
                ReturnColoredText("Poisoned", yellow) + " to a target";
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
                " damage and apply " + ReturnColoredText("Chilled", yellow) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Toxic Eruption")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText((1 + entity.myPassiveManager.venomousStacks).ToString(), yellow) + " " +
                ReturnColoredText("Poisoned", yellow) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Meteor")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("1", yellow) + " " +
                ReturnColoredText("Burning", yellow) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Thunder Storm")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " +
                ReturnColoredText("Shocked", yellow) +
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
                " damage to a target and apply " + ReturnColoredText("Chilled", yellow);
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
        else if (ability.abilityName == "Consecration")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to adjacent enemies. Give " + ReturnColoredText(ability.abilitySecondaryValue.ToString(), yellow) +
                " to adjacent allies";
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
        else if (ability.abilityName == "Dimensional Blast")
        {
            ability.descriptionText.text =
                "Apply " + ReturnColoredText("Burning", yellow) + ", " +
                ReturnColoredText("Poisoned", yellow) + ", " +
                ReturnColoredText("Chilled", yellow) + " and " +
                ReturnColoredText("Shocked", yellow) +
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
                " damage to adjacent enemies, and apply 1 " + ReturnColoredText("Burning", yellow);
        }
        else if (ability.abilityName == "Frost Nova")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to adjacent enemies, and apply " + ReturnColoredText("Chilled", yellow);
        }
        else if (ability.abilityName == "Noxious Fumes")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to adjacent enemies, and apply " +
                ReturnColoredText((ability.abilitySecondaryValue + entity.myPassiveManager.venomousStacks).ToString(), yellow) + " " +
                ReturnColoredText("Poisoned", yellow);
        }
        else if (ability.abilityName == "Get Down!")
        {
            ability.descriptionText.text =
                "Move to a tile within " +
                ReturnColoredText((ability.abilityPrimaryValue + EntityLogic.GetTotalMobility(entity)).ToString(), yellow) +
                ". At the end of the movement, give " +
                ReturnColoredText(CombatLogic.Instance.CalculateBlockGainedByEffect(ability.abilityPrimaryValue, entity).ToString(), yellow) +
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
                ". On arrial, apply "+ ReturnColoredText(ability.abilitySecondaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Burning", yellow) +
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
        else if (ability.abilityName == "Sharpened Blade")
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
                "Give an ally " + ReturnColoredText(ability.abilitySecondaryValue.ToString(), yellow) +
                " " + ReturnColoredText("Temporary Hawk Eye", yellow);
        }
        else if (ability.abilityName == "Super Conductor")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) +
                " " + ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to all enemies within " + ReturnColoredText(entity.currentMeleeRange.ToString(), yellow) +
                " and apply " + ReturnColoredText("Shocked", yellow) +
                ". If any enemies are already " + ReturnColoredText("Shocked", yellow) +
                " apply " + ReturnColoredText("Stunned", yellow) + " instead";
        }
        else if (ability.abilityName == "Sword And Board")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and gain" + 
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
                + " "+ ReturnColoredText("Poisoned", yellow);
        }
        else if (ability.abilityName == "Thunder Strike")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply "
                + " " + ReturnColoredText("Shocked", yellow);
        }
        else if (ability.abilityName == "Thaw")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), yellow) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage. If the target is " + ReturnColoredText("Chilled", yellow) +
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

        return colorCodeReturned;
    }

}
