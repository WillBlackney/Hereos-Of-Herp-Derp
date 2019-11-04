using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextLogic 
{
    public static string white = "<color=#FFFFFF>";
    public static string brown = "<color=#968300>";
    public static string blue = "<color=#00BEFF>";
    public static string yellow = "<color=#FFF91C>";

    public static void SetStatusIconDescriptionText(StatusIcon icon)
    {
        if(icon.statusName == "Strength")
        {
            icon.statusDescriptionText.text =
                "Increases " + ReturnColoredText("Physical Damage", brown) +
                " from abilities by " + ReturnColoredText(icon.statusStacks.ToString(), yellow);
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
                "Increases " + ReturnColoredText("Magic Damage", blue) +
                " from abilities by " + ReturnColoredText(icon.statusStacks.ToString(), yellow);
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
                + " strength";
        }
        else if (icon.statusName == "Growing")
        {
            icon.statusDescriptionText.text =
                "At the start of this characters activation, it gains  " + ReturnColoredText(icon.statusStacks.ToString(), yellow)
                + " strength";
        }
        else if (icon.statusName == "Growing")
        {
            icon.statusDescriptionText.text =
                "At the start of this characters activation, it gains  " + ReturnColoredText(icon.statusStacks.ToString(), yellow)
                + " strength";
        }
        else if (icon.statusName == "Volatile")
        {
            icon.statusDescriptionText.text =
                "Upon death, this character explodes, dealing " + ReturnColoredText(icon.statusStacks.ToString(), yellow)
                + " damage to ALL adjacent characters";
        }
        else if (icon.statusName == "Camoflage")
        {
            icon.statusDescriptionText.text =
                "This character cannot be targetted by enemy abilities from further then" + ReturnColoredText("1", yellow)
                + " tile away. Cancelled by moving off a " + ReturnColoredText("Grass", yellow) + " tile";
        }
        else if (icon.statusName == "Camoflage")
        {
            icon.statusDescriptionText.text =
                "This character cannot be targetted by enemy abilities from further than" + ReturnColoredText("1", yellow)
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
                + " armor";
        }
        else if (icon.statusName == "Fleet Footed")
        {
            icon.statusDescriptionText.text =
                "This character's first 'Move' ability each turn costs " + ReturnColoredText("0", yellow)+ " AP";
        }
        else if (icon.statusName == "Encouraging Presence")
        {
            icon.statusDescriptionText.text =
                "At the end of this character's activation, it grants " + ReturnColoredText(icon.statusStacks.ToString(), yellow) + " AP to adjacent allies";
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
                ReturnColoredText(icon.statusStacks.ToString(), yellow) +
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
                "The first time this character is damaged each turn, it moves to a random adjacent tile";
        }
        else if (icon.statusName == "Regeneration")
        {
            icon.statusDescriptionText.text =
                "At the end of this character's activation, it heals " + (icon.statusStacks.ToString(), yellow) 
                + " HP";
        }
        else if (icon.statusName == "Adaptive")
        {
            icon.statusDescriptionText.text =
                "Whenever this character loses HP, it gains " + (icon.statusStacks.ToString(), yellow)
                + " block";
        }
        else if (icon.statusName == "Hateful Presence")
        {
            icon.statusDescriptionText.text =
                "At the end of this character's activation, it grants " + (icon.statusStacks.ToString(), yellow)
                + " strength to adjacent allies";
        }
        else if (icon.statusName == "Soul Drain Aura")
        {
            icon.statusDescriptionText.text =
                "At the end of this character's activation, it steals " + (icon.statusStacks.ToString(), yellow)
                + " strength from adjacent enemies";
        }
        else if (icon.statusName == "Lightning Shield")
        {
            icon.statusDescriptionText.text =
                "Whenever this character is hit with an attack, it deals " +
                ReturnColoredText(icon.statusStacks.ToString(), yellow) + " " + ReturnColoredText("Magic Damage", blue) +
                " back to it's attacker. Expires on next activation start";
        }
        else if (icon.statusName == "Thick Of The Fight")
        {
            icon.statusDescriptionText.text =
                "At the start of this character's activation, if there is an enemy within melee range, it gains " +
                ReturnColoredText(icon.statusStacks.ToString(), yellow) + 
                " AP";
        }
        else if (icon.statusName == "Temporary Strength")
        {
            icon.statusDescriptionText.text =
                "At the end of this character's activation, it loses " +
                ReturnColoredText(icon.statusStacks.ToString(), yellow) +
                " strength";
        }
        else if (icon.statusName == "Temporary Initiative")
        {
            icon.statusDescriptionText.text =
                "At the end of this character's activation, it loses " +
                ReturnColoredText(icon.statusStacks.ToString(), yellow) +
                " initiative";
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

    public static string ReturnColoredText(string text, string color)
    {
        return (color + text + white);
    }

}
