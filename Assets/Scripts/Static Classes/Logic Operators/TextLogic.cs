using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextLogic 
{
    [Header("RGBA Colour Codes")]
    public static string white = "<color=#FFFFFF>";
    public static string brown = "<color=#968300>";
    public static string blue = "<color=#00BEFF>";
    public static string yellow = "<color=#FFF91C>";
    public static string purple = "<color=#CF01BC>";
    public static string darkRed = "<color=#AB0500>";

    public static void SetStatusIconDescriptionText(StatusIcon icon)
    {
        if(icon.statusName == "Strength")
        {
            icon.statusDescriptionText.text =
                "Increases " + ReturnColoredText("Physical", brown) +
                " damage from abilities by " + ReturnColoredText(icon.statusStacks.ToString(), yellow);
        }
        else if (icon.statusName == "Dexterity")
        {
            icon.statusDescriptionText.text =
                "Increases the amount of " + ReturnColoredText("Block", yellow) +
                " gained from abilities and effects by " + ReturnColoredText(icon.statusStacks.ToString(), yellow);
        }
        else if (icon.statusName == "Wisdom")
        {
            icon.statusDescriptionText.text =
                "Increases " + ReturnColoredText("Magic", blue) +
                " damage from abilities by " + ReturnColoredText(icon.statusStacks.ToString(), yellow);
        }
        else if (icon.statusName == "Stunned")
        {
            icon.statusDescriptionText.text =
                "This character skips its next activation, and cannot take any actions";
        }
        else if (icon.statusName == "Pinned")
        {
            icon.statusDescriptionText.text =
                "Unable to take movement actions with abilities and effects";
        }
        else if (icon.statusName == "Barrier")
        {
            icon.statusDescriptionText.text =
                "The next " + ReturnColoredText(icon.statusStacks.ToString(), yellow) + 
                "time(s) this character would lose HP, ignore it";
        }
        else if (icon.statusName == "Enrage")
        {
            icon.statusDescriptionText.text =
                "Whenever this character loses HP, it gains " + ReturnColoredText(icon.statusStacks.ToString(), yellow)
                + " Strength";
        }
        else if (icon.statusName == "Growing")
        {
            icon.statusDescriptionText.text =
                "At the start of this characters activation, it gains  " + ReturnColoredText(icon.statusStacks.ToString(), yellow)
                + " Strength";
        }        
        else if (icon.statusName == "Volatile")
        {
            icon.statusDescriptionText.text =
                "On death, this character explodes, dealing " + ReturnColoredText(icon.statusStacks.ToString(), yellow) + " " +
                ReturnColoredText("Physical", brown) + " damage to ALL adjacent characters";
        }
        else if (icon.statusName == "Camoflage")
        {
            icon.statusDescriptionText.text =
                "This character cannot be targetted by enemy abilities from further than " + ReturnColoredText("1", yellow)
                + " tile away. Cancelled by moving off a " + ReturnColoredText("Grass", yellow) + " tile";
        }
        
        else if (icon.statusName == "Poison")
        {
            icon.statusDescriptionText.text =
                "At the end of this character's activation, it loses " + ReturnColoredText(icon.statusStacks.ToString(), yellow)
                + " health";
        }
        else if (icon.statusName == "Cautious")
        {
            icon.statusDescriptionText.text =
                "At the end of this character's activation, it gains " + ReturnColoredText(icon.statusStacks.ToString(), yellow)
                + " Block";
        }
        else if (icon.statusName == "Fleet Footed")
        {
            icon.statusDescriptionText.text =
                "This character's first 'Move' ability each turn costs " + ReturnColoredText("0", yellow)+ " AP";
        }
        else if (icon.statusName == "Encouraging Presence")
        {
            icon.statusDescriptionText.text =
                "At the end of this character's activation, it grants " + ReturnColoredText(icon.statusStacks.ToString(), yellow) + " Energy to adjacent allies";
        }
        else if (icon.statusName == "Poisonous")
        {
            icon.statusDescriptionText.text =
                "Whenever this character reduces health with a melee attack, it applies " + ReturnColoredText(icon.statusStacks.ToString(), yellow) + " poison";
        }
        else if (icon.statusName == "Preparation")
        {
            icon.statusDescriptionText.text =
                "This character's next ability costs " + ReturnColoredText("0", yellow) + " AP";
        }
        else if (icon.statusName == "Stealth")
        {
            icon.statusDescriptionText.text =
                "This character cannot be targetted by enemy abilities from further than " + ReturnColoredText("1", yellow)
                + " tile away";
        }
        else if (icon.statusName == "Thorns")
        {
            icon.statusDescriptionText.text =
                "Whenever this character is hit with a melee attack, it deals " +
                ReturnColoredText(icon.statusStacks.ToString(), yellow) + " " + ReturnColoredText("Physical", brown) +
                " damage back to it's attacker";                
        }
        else if (icon.statusName == "Unhygienic")
        {
            icon.statusDescriptionText.text =
                "At the end of this character's activation, it applies " +
                ReturnColoredText(icon.statusStacks.ToString(), yellow) +
                " poison to all adjacent enemies";
        }
        else if (icon.statusName == "Quick Reflexes")
        {
            icon.statusDescriptionText.text =
                "The first time this character is attacked each turn cycle, it teleports to a random adjacent tile";
        }
        else if (icon.statusName == "Regeneration")
        {
            icon.statusDescriptionText.text =
                "At the end of this character's activation, it heals " + ReturnColoredText(icon.statusStacks.ToString(), yellow) 
                + " HP";
        }
        else if (icon.statusName == "Adaptive")
        {
            icon.statusDescriptionText.text =
                "Whenever this character loses HP, it gains " + ReturnColoredText(icon.statusStacks.ToString(), yellow)
                + " Block";
        }
        else if (icon.statusName == "Hateful Presence")
        {
            icon.statusDescriptionText.text =
                "At the end of this character's activation, it grants " + ReturnColoredText(icon.statusStacks.ToString(), yellow)
                + " Strength to adjacent allies";
        }
        else if (icon.statusName == "Soul Drain Aura")
        {
            icon.statusDescriptionText.text =
                "At the end of this character's activation, it steals " + ReturnColoredText(icon.statusStacks.ToString(), yellow)
                + "Sstrength from adjacent enemies";
        }
        else if (icon.statusName == "Lightning Shield")
        {
            icon.statusDescriptionText.text =
                "Whenever this character is hit with an attack, it deals " +
                ReturnColoredText(icon.statusStacks.ToString(), yellow) + " " + ReturnColoredText("Magic", blue) +
                " damage back to it's attacker. Expires on next activation start";
        }
        else if (icon.statusName == "Thick Of The Fight")
        {
            icon.statusDescriptionText.text =
                "At the start of this character's activation, if there is an enemy within melee range, it gains " +
                ReturnColoredText(icon.statusStacks.ToString(), yellow) + 
                " Energy";
        }
        else if (icon.statusName == "Temporary Strength")
        {
            icon.statusDescriptionText.text =
                "At the end of this character's activation, it loses " +
                ReturnColoredText(icon.statusStacks.ToString(), yellow) +
                " Strength";
        }
        else if (icon.statusName == "Temporary Initiative")
        {
            icon.statusDescriptionText.text =
                "At the end of this character's activation, it loses " +
                ReturnColoredText(icon.statusStacks.ToString(), yellow) +
                " Initiative";
        }
        else if (icon.statusName == "Rune")
        {
            icon.statusDescriptionText.text =
                "The next " +
                ReturnColoredText(icon.statusStacks.ToString(), yellow) +
                " time(s) this character would suffer a negative status effect, ignore it";
        }
        else if (icon.statusName == "Exposed")
        {
            icon.statusDescriptionText.text =
                "This character takes 50% increased damage from all attacks for " +
                ReturnColoredText(icon.statusStacks.ToString(), yellow) +
                " turn(s)";
        }
        else if (icon.statusName == "Exhausted")
        {
            icon.statusDescriptionText.text =
                "This character deals 50% less damage with all attacks for " +
                ReturnColoredText(icon.statusStacks.ToString(), yellow) +
                " turn(s)";
        }

    }
    public static void SetAbilityDescriptionText(Ability ability)
    {
        if (ability.abilityName == "Strike")
        {
            ability.descriptionText.text =
                "Deal " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Physical", brown) +
                " damage to a target";
        }
        else if (ability.abilityName == "Block")
        {
            ability.descriptionText.text =
                "Gain " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), yellow) + " " +
                ReturnColoredText("Block", blue);
        }
        else if (ability.abilityName == "Move")
        {
            ability.descriptionText.text =
                "Move to a tile within " + ReturnColoredText(ability.myLivingEntity.currentMobility.ToString(), yellow) +
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
                ReturnColoredText("Physical", brown) + " damage, and apply " +
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
                ReturnColoredText("Physical", brown) +
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
                ReturnColoredText("Physical", brown) +
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
