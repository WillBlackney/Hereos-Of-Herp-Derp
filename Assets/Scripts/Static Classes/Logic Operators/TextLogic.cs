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
    public static void SetAbilityTextProperties(Ability ability)
    {
        // Properties to set
        // Energy Cost
        // Range
        // Cooldown
        // Description

        if (ability.abilityName == "Strike")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Physical", physical) +
                " damage to a target";
        }
        else if (ability.abilityName == "Defend")
        {
            ability.descriptionText.text =
                "Gain " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Block", blue);
        }
        else if (ability.abilityName == "Move")
        {
            ability.descriptionText.text =
                "Move to a tile within " + ReturnColoredText(EntityLogic.GetTotalMobility(ability.myLivingEntity).ToString(), yellow) +
                " of your current position";
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
                "Move to a target enemy within " + ReturnColoredText(ability.abilityRange.ToString(), yellow) +
                " tiles. At the end of the movement, deal " +
                ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Physical", physical) + " damage, and apply " +
                ReturnColoredText(ability.abilitySecondaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Exposed", darkRed);
        }

        else if (ability.abilityName == "Whirlwind")
        {
            string tile = "tile";
            if (ability.myLivingEntity.currentMeleeRange > 1)
            {
                tile = "tiles";
            }

            ability.descriptionText.text =
                "Deal " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Physical", physical) +
                " damage to all enemies within " +
                ReturnColoredText(ability.myLivingEntity.currentMeleeRange.ToString(), yellow) + " " +
                tile;
        }

        else if (ability.abilityName == "Fire Ball")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Magic", purple) +
                " damage to a target";
        }
        else if (ability.abilityName == "Frost Bolt")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Magic", purple) +
                " damage to a target, and apply " +
                ReturnColoredText(ability.abilitySecondaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Pinned", darkRed);
        }
        else if (ability.abilityName == "Telekinesis")
        {
            ability.descriptionText.text =
                "Teleport a target anywhere within " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) +
                " tile of its current position";
        }
        else if (ability.abilityName == "Twin Strike")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Physical", physical) +
                " damage to a target twice";
        }
        else if (ability.abilityName == "Dash")
        {
            ability.descriptionText.text =
                "Move to a tile within " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) +
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
                "Give " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Block", blue) +
                " to an ally, or deal " +
                ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Magic", purple) +
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
                "Deal " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Magic", purple) +
                " damage to a target, and apply " +
                ReturnColoredText(ability.abilitySecondaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Exposed", darkRed);
        }
    }

    public static string ReturnColoredText(string text, string color)
    {
        return (color + text + white);
    }

}
