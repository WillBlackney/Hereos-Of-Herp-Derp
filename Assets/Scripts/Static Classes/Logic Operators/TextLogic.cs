using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class TextLogic 
{
    [Header("RGBA Colour Codes")]
    public static string white = "<color=#FFFFFF>";
    public static string yellow = "<color=#FFF91C>";
    public static string blueNumber = "<color=#92E0FF>";

    public static string negativeRed = "<color=#FF1D00>";
    public static string positiveGreen = "<color=#16FF00>";
    public static string neutralYellow = "<color=#F8FF00>";

    public static string physical = "<color=#BA8400>";
    public static string fire = "<color=#FF6637>";
    public static string frost = "<color=#3687FF>";
    public static string shadow = "<color=#CF01BC>";
    public static string air = "<color=#36EDFF>";
    public static string poison = "<color=#00EC4A>";

    public static string blue = "<color=#00BEFF>";    
    public static string purple = "<color=#CF01BC>";
    public static string darkRed = "<color=#AB0500>";

    public static string humanRaceDescription
    {
        get { return "Diplomatic and pragmatic, Humans are masters of anything they put their minds too. Rulers of great cities," +
                " they exert their authority with justice and honour over all other races and nations"; }
    }
    public static string undeadRaceDescription
    {
        get { return "A former legion of mindless ghouls and necromancers, the Undead were once feared by all living creatures." +
                " For reasons unknown, the Undead have broken their enslaving magical bindings and regained their free will." +
                " Now they fight to claim their place in the new world."; }
    }
    public static string elfRaceDescription
    {
        get
        {
            return "Mysterious and elusive, the elves hide away in their native forests, worshipping the godess of the moon." +
                " Masters of magic and archery, " +
                "they are highly intelligent, sophisticated and dependable allies.";
        }
    }
    public static string orcRaceDescription
    {
        get { return "From the Badlands Raiders to the peaceful Herp Derpianian natives, the shamanistic yet warlike orcs are capable of both " +
                "altruistic kindness, and brutal savagery. They are your best friends, and your worst enemies."; }
    }

    // Set ability + passive texts
    #region
    public static void SetStatusIconDescriptionText(string statusName, TextMeshProUGUI statusDescriptionText, int statusStacks)
    {
        if (statusName == "Bonus Strength")
        {
            statusDescriptionText.text =
                "Increases the base damage of "+ ReturnColoredText("Melee Attack", yellow)+ " abilities by" +
                ReturnColoredText(statusStacks.ToString(), blueNumber);
        }
        else if (statusName == "Temporary Bonus Strength")
        {
            statusDescriptionText.text =
                "Increases the base damage of " + ReturnColoredText("Melee Attack", yellow) + " abilities by" +
                ReturnColoredText(statusStacks.ToString(), blueNumber) +
            ". Expires on activation end";
        }
        else if (statusName == "Bonus Dexterity")
        {
            statusDescriptionText.text =
                "Increases the amount of " + ReturnColoredText("Block", yellow) +
                " granted from abilities and effects by " + ReturnColoredText(statusStacks.ToString(), blueNumber);
        }
        else if (statusName == "Temporary Bonus Dexterity")
        {
            statusDescriptionText.text =
                "Increases the amount of " + ReturnColoredText("Block", yellow) +
                " granted from abilities and effects by  " + ReturnColoredText(statusStacks.ToString(), blueNumber)
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
                " damage from abilities by and effects by " + ReturnColoredText(statusStacks.ToString(), blueNumber);
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
                " damage from abilities by and effects by " + ReturnColoredText(statusStacks.ToString(), blueNumber)
                + ". Expires on activation end"; 
        }
        else if (statusName == "Bonus Mobility")
        {
            statusDescriptionText.text =
                "Increase the range of movement abilities by " + ReturnColoredText(statusStacks.ToString(), blueNumber);
        }
        else if (statusName == "Temporary Bonus Mobility")
        {
            statusDescriptionText.text =
                "Increase the range of movement abilities by " + ReturnColoredText(statusStacks.ToString(), blueNumber)
                + ". Expires on activation end"; 
        }
        else if (statusName == "Bonus Initiative")
        {
            statusDescriptionText.text =
                "Increases your activation order roll by " + ReturnColoredText(statusStacks.ToString(), blueNumber);
        }
        else if (statusName == "Temporary Bonus Initiative")
        {
            statusDescriptionText.text =
                "Increases your activation order roll by " + ReturnColoredText(statusStacks.ToString(), blueNumber)
                + ". Expires on activation end"; 
        }
        else if (statusName == "Bonus Critical")
        {
            statusDescriptionText.text =
                "Increases your chance to deal " + ReturnColoredText("50%", blueNumber) + " bonus damage with an attack by " +
                ReturnColoredText(statusStacks.ToString() + "%", blueNumber);                
        }
        else if (statusName == "Bonus Dodge")
        {
            statusDescriptionText.text =
                "Increases your chance to completely avoid a " + ReturnColoredText("Ranged Attack", yellow) + " ability by " +
                ReturnColoredText(statusStacks.ToString() + "%", blueNumber);
        }
        else if (statusName == "Bonus Parry")
        {
            statusDescriptionText.text =
                "Increases your chance to completely avoid a " + ReturnColoredText("Melee Attack", yellow) + " ability by " +
                ReturnColoredText(statusStacks.ToString() + "%", blueNumber);
        }
        else if (statusName == "Bonus Max Energy")
        {
            statusDescriptionText.text =
                "Increases the maximum amount of " + ReturnColoredText("Energy", yellow) + 
                " you can have at any time by " +
                ReturnColoredText(statusStacks.ToString(), blueNumber);
        }
        else if (statusName == "Bonus Melee Range")
        {
            statusDescriptionText.text =
                "Increases the range of your " + ReturnColoredText("Melee Attack", yellow) +
                " abilities by " +
                ReturnColoredText(statusStacks.ToString(), blueNumber);
        }
        else if (statusName == "Bonus Power Limit")
        {
            statusDescriptionText.text =
                "Increases the amount of  " + ReturnColoredText("Power", yellow) +
                " abilities you can channel at a time by " +
                ReturnColoredText(statusStacks.ToString(), blueNumber);
        }
        else if (statusName == "Bonus All Resistances")
        {
            statusDescriptionText.text =
                "Reduces incoming damage from all damage types by" +
                ReturnColoredText(statusStacks.ToString(), blueNumber);
        }


        else if (statusName == "Bonus Stamina")
        {
            statusDescriptionText.text =
                "Increase the amount of "+ ReturnColoredText("Energy", yellow) + 
                " gained on activation start by " + ReturnColoredText(statusStacks.ToString(), blueNumber);
        }
        else if (statusName == "Temporary Bonus Stamina")
        {
            statusDescriptionText.text =
                "Increase the amount of " + ReturnColoredText("Energy", yellow) +
                " gained on activation start by " + ReturnColoredText(statusStacks.ToString(), blueNumber)
                + ". Expires on activation end"; 
        }
        else if (statusName == "Temporary Dodge")
        {
            statusDescriptionText.text =
                "Increases your chance to completely avoid "+ ReturnColoredText("Ranged Attack", blueNumber) + " abilities by " +
                ReturnColoredText(statusStacks.ToString() + "%", blueNumber)
                +". Expires on activation end";
        }
        else if (statusName == "Temporary Parry")
        {
            statusDescriptionText.text =
                "Increases your chance to completely avoid " + ReturnColoredText("Melee Attack", blueNumber) + " abilities by " +
                ReturnColoredText(statusStacks.ToString() + "%", blueNumber)
                + ". Expires on activation end";
        }
        else if (statusName == "Vulnerable")
        {
            statusDescriptionText.text =
                "This character takes "+ ReturnColoredText("50%", blueNumber)+ " increased damage from all attacks";
        }
        else if (statusName == "Weakened")
        {
            statusDescriptionText.text =
                "This character deals " + ReturnColoredText("50%", blueNumber) + " less damage with all attacks";
        }
        else if (statusName == "Burning")
        {
            statusDescriptionText.text =
                "On activation end, take " + ReturnColoredText(statusStacks.ToString(), blueNumber) + " " +
                ReturnColoredText("Fire", fire) + " damage";
        }
        else if (statusName == "Poisoned")
        {
            statusDescriptionText.text =
                "On activation end, take " + ReturnColoredText(statusStacks.ToString(), blueNumber) + " " +
                ReturnColoredText("Poison", poison) + " damage";
        }
        else if (statusName == "Fading")
        {
            statusDescriptionText.text =
                "On activation end, lose " + ReturnColoredText(statusStacks.ToString(), blueNumber) + " health";
        }
        else if (statusName == "Barrier")
        {
            statusDescriptionText.text =
                "The next " + ReturnColoredText(statusStacks.ToString(), blueNumber) + " time(s) this character loses health, ignore it";
        }
        else if (statusName == "Camoflage")
        {
            statusDescriptionText.text =
                "This character cannot be targetted by enemy abilities from further than " + ReturnColoredText("1", blueNumber)
                + " tile away. Cancelled by using abilities (except Move), or taking damage";
        }
        else if (statusName == "Cautious")
        {
            statusDescriptionText.text =
                "On activation start, this character gains " + ReturnColoredText(statusStacks.ToString(), blueNumber)
                + " " + ReturnColoredText("Block", yellow);
        }
        else if (statusName == "Chilled")
        {
            statusDescriptionText.text =
                "This character has " + ReturnColoredText("-1", blueNumber) + " " + ReturnColoredText("Mobility", yellow) + " and " +
                ReturnColoredText("Initiative", yellow) + ". Expires on activation end";
        }
        else if (statusName == "Shocked")
        {
            statusDescriptionText.text =
                "This character has " + ReturnColoredText("10", blueNumber) + " less " + ReturnColoredText("Stamina", yellow) +
                ". Expires on activation end";
        }
        else if (statusName == "Concentration")
        {
            statusDescriptionText.text =
                "This character's "+ ReturnColoredText("Ranged Attack", yellow) + " abilities cannot be dodged. +20 Ranged Critical Chance";
        }
        else if (statusName == "Demon")
        {
            statusDescriptionText.text =
                "Increase all "+ ReturnColoredText("Fire", fire) + " damage dealt by " + ReturnColoredText("30%", blueNumber) + ". " +
                ReturnColoredText("Fire", fire) + " Resistance increased by " + ReturnColoredText("30", blueNumber);
        }
        else if (statusName == "Shadow Form")
        {
            statusDescriptionText.text =
                "Increase all " + ReturnColoredText("Shadow", shadow) + " damage dealt by " + ReturnColoredText("30%", blueNumber) + ". " +
                ReturnColoredText("Shadow", shadow) + " Resistance increased by " + ReturnColoredText("30", blueNumber);
        }
        else if (statusName == "Toxicity")
        {
            statusDescriptionText.text =
                "Increase all " + ReturnColoredText("Poison", poison) + " damage dealt by " + ReturnColoredText("30%", blueNumber) + ". " +
                ReturnColoredText("Poison", poison) + " Resistance increased by " + ReturnColoredText("30", blueNumber);
        }
        else if (statusName == "Storm Lord")
        {
            statusDescriptionText.text =
                "Increase all " + ReturnColoredText("Air", air) + " damage dealt by " + ReturnColoredText("30%", blueNumber) + ". " +
                ReturnColoredText("Air", air) + " Resistance increased by " + ReturnColoredText("30", blueNumber);
        }
        else if (statusName == "Frozen Heart")
        {
            statusDescriptionText.text =
                "Increase all " + ReturnColoredText("Frost", frost) + " damage dealt by " + ReturnColoredText("30%", blueNumber) + ". " +
                ReturnColoredText("Frost", frost) + " Resistance increased by " + ReturnColoredText("30", blueNumber);
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
                "This character cannot gain " + ReturnColoredText("Block", yellow);
        }
        else if (statusName == "Stunned")
        {
            statusDescriptionText.text =
                "This character cannot use abilities or take any action";
        }
        else if (statusName == "Taunted")
        {
            statusDescriptionText.text =
                "This character cannot use targetable abilities against characters that are not its taunter";
        }
        else if (statusName == "Sleep")
        {
            statusDescriptionText.text =
                "This character cannot use abilities or take any action. Removed if damaged";
        }
        else if (statusName == "Immobilized")
        {
            statusDescriptionText.text =
                "Unable to take movement actions with abilities and effects";
        }
        else if (statusName == "Radiance")
        {
            statusDescriptionText.text =
                "This character's "+ ReturnColoredText("Aura Size", yellow) + " is increased by " + ReturnColoredText(statusStacks.ToString(), blueNumber);
        }
        else if (statusName == "Encouraging Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character grants " + ReturnColoredText(statusStacks.ToString(), blueNumber) + " "
                + ReturnColoredText("Energy", yellow) + " to allies within it's " + ReturnColoredText("Aura", yellow);
        }
        else if (statusName == "Fiery Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character deals " + ReturnColoredText(statusStacks.ToString(), blueNumber) +
                " " + ReturnColoredText("Fire", fire) + " damage to enemies within it's " + ReturnColoredText("Aura", yellow);
        }
        else if (statusName == "Toxic Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character applies " + ReturnColoredText(statusStacks.ToString(), blueNumber) + " " +
                 ReturnColoredText("Poisoned", poison) + " to enemies within it's " + ReturnColoredText("Aura", yellow);
        }
        else if (statusName == "Storm Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character deals " + ReturnColoredText(statusStacks.ToString(), blueNumber) +
                " " + ReturnColoredText("Air", air) + " damage to a random enemy within it's " + ReturnColoredText("Aura", yellow)+ " twice";
        }
        else if (statusName == "Soul Drain Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character steals " + ReturnColoredText(statusStacks.ToString(), blueNumber) +
                " "+ ReturnColoredText("Strength", yellow) + " from enemies within it's " + ReturnColoredText("Aura", yellow);
        }
        else if (statusName == "Guardian Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character grants " + ReturnColoredText(statusStacks.ToString(), blueNumber) +
                 " " + ReturnColoredText("Block", yellow)+ " to allies within it's " + ReturnColoredText("Aura", yellow);
        }
        else if (statusName == "Hateful Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character grants " + ReturnColoredText(statusStacks.ToString(), blueNumber) +
                " " + ReturnColoredText("Strength", yellow) +" to allies within it's " + ReturnColoredText("Aura", yellow);
        }
        else if (statusName == "Enrage")
        {
            statusDescriptionText.text =
            statusDescriptionText.text =
                "Whenever this character loses health, it gains " + ReturnColoredText(statusStacks.ToString(), blueNumber)
                + " " + ReturnColoredText("Strength", yellow);
        }
        else if (statusName == "Tenacious")
        {
            statusDescriptionText.text =
                "Whenever this character loses health, it gains " + ReturnColoredText(statusStacks.ToString(), blueNumber)
               + " " + ReturnColoredText("Block", yellow);
        }
        else if (statusName == "Opportunist")
        {
            statusDescriptionText.text =
                "This character deals " + ReturnColoredText("50%", blueNumber)
                + " extra damage with "+ ReturnColoredText("Melee Attack", yellow) + " abilities when back striking";
        }
        else if (statusName == "Hawk Eye")
        {
            statusDescriptionText.text =
                "This character has " + ReturnColoredText(statusStacks.ToString(), blueNumber)
                + " bonus range with " + ReturnColoredText("Ranged Attack", yellow) + " abilities";
        }
        else if (statusName == "Temporary Hawk Eye")
        {
            statusDescriptionText.text =
                "This character has " + ReturnColoredText(statusStacks.ToString(), blueNumber)
                + " bonus range with " + ReturnColoredText("Ranged Attack", yellow) + " abilities. Expires on activation end";
        }

        else if (statusName == "Ethereal Being")
        {
            statusDescriptionText.text =
                "This character ignores line of sight when targetting";
        }

        else if (statusName == "True Sight")
        {
            statusDescriptionText.text =
                "This character ignores " + ReturnColoredText("Camoflage", yellow) + " when attacking and targeting";
        }
        else if (statusName == "Temporary True Sight")
        {
            statusDescriptionText.text =
               "This character ignores " + ReturnColoredText("Camoflage", yellow) + " when attacking and targeting. Expires on activation end";
        }
        else if (statusName == "Overwatch")
        {
            statusDescriptionText.text =
                "This character will perform " + ReturnColoredText("Shoot", yellow) + 
                " against the first enemy that moves within it's weapon's range";
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
        else if (statusName == "Sword Play")
        {
            statusDescriptionText.text =
                "This character is guaranteed to " + ReturnColoredText("Parry", yellow) +
                " the first attack made against it each turn cycle";
        }
        else if (statusName == "Flux")
        {
            statusDescriptionText.text =
                "This character's first " + ReturnColoredText("Move", yellow) +
                " ability each activation costs " + ReturnColoredText("0", blueNumber) + ReturnColoredText(" Energy", yellow);
        }
        else if (statusName == "Pierce")
        {
            statusDescriptionText.text =
                "This character ignores " + ReturnColoredText("Block", yellow) +
                " when attacking";
        }
        else if (statusName == "Quick Draw")
        {
            statusDescriptionText.text =
                "This character's first " + ReturnColoredText("Ranged Attack", yellow) +
                " ability each activation costs " + ReturnColoredText("0", blueNumber) + ReturnColoredText(" Energy", yellow);
        }
        else if (statusName == "Fury")
        {
            statusDescriptionText.text =
                "This character's first " + ReturnColoredText("Melee Attack", yellow) +
                " ability each activation costs " + ReturnColoredText("0", blueNumber) + ReturnColoredText(" Energy", yellow);
        }
        else if (statusName == "Grace")
        {
            statusDescriptionText.text =
                "This character's first " + ReturnColoredText("Skill", yellow) +
                " ability each activation costs " + ReturnColoredText("0", blueNumber) + ReturnColoredText(" Energy", yellow);
        }
        else if (statusName == "Knowledgeable")
        {
            statusDescriptionText.text =
                "This character's " + ReturnColoredText("Skill", yellow) +
                " abilities cost " + ReturnColoredText("5", blueNumber) + " less " + ReturnColoredText("Energy", yellow);
        }
        else if (statusName == "Savage")
        {
            statusDescriptionText.text =
                "This character's " + ReturnColoredText("Melee Attack", yellow) +
                " abilities cost " + ReturnColoredText("5", blueNumber) + " less " + ReturnColoredText("Energy", yellow);
        }
        else if (statusName == "Pragmatic")
        {
            statusDescriptionText.text =
                "This character's " + ReturnColoredText("Ranged Attack", yellow) +
                " abilities cost " + ReturnColoredText("5", blueNumber) + " less " + ReturnColoredText("Energy", yellow);
        }
        else if (statusName == "Transcendence")
        {
            statusDescriptionText.text =
                "This character is immune to all damage until the end of the current turn cycle";
        }
        else if (statusName == "Marked")
        {
            statusDescriptionText.text =
                "This character is unable to " + ReturnColoredText("Dodge", yellow) +
                " or " + ReturnColoredText("Parry", yellow) +
                " attacks until the end of the current turn cycle";
        }
        else if (statusName == "Snow Stasis")
        {
            statusDescriptionText.text =
                "Target yourself or an ally. The next time that character takes damage, ignore it.";
        }
        else if (statusName == "Immolation")
        {
            statusDescriptionText.text =
                "Whenever this character reduces health with a " + ReturnColoredText("Melee Attack", yellow) +
                " ability, it applies " + ReturnColoredText(statusStacks.ToString(), blueNumber) + " "
                + ReturnColoredText("Burning", fire);
        }
        else if (statusName == "Poisonous")
        {
            statusDescriptionText.text =
                "Whenever this character reduces health with a " + ReturnColoredText("Melee Attack", yellow) +
                " ability, it applies " + ReturnColoredText(statusStacks.ToString(), blueNumber) + " "
                + ReturnColoredText("Poisoned", poison);
        }
        else if (statusName == "Growing")
        {
            statusDescriptionText.text =
                "On activation start, this character gains " + ReturnColoredText(statusStacks.ToString(), blueNumber)
                + ReturnColoredText(" Strength", yellow);
        }
        else if (statusName == "Fast Learner")
        {
            statusDescriptionText.text =
                "On activation start, this character gains " + ReturnColoredText(statusStacks.ToString(), blueNumber)
                + ReturnColoredText(" Wisdom", yellow);
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
                "This character is immune to " + ReturnColoredText("Stun", yellow) + ", " +
                ReturnColoredText("Sleep", yellow) + " and " +
                ReturnColoredText("Immobilized", yellow);
        }
        else if (statusName == "Unwavering")
        {
            statusDescriptionText.text =
                "This character's " + ReturnColoredText("Block", yellow)+ " does not expire on activation start";
        }
        else if (statusName == "Infuse")
        {
            statusDescriptionText.text =
                "This character has +" + ReturnColoredText("20", yellow) + " to all resistances";
        }
        
        else if (statusName == "Last Stand")
        {
            statusDescriptionText.text =
                "The first time this character would take lethal damage in each combat, it gains "
                + ReturnColoredText("5", blueNumber) + " " +
                 ReturnColoredText("Strength", yellow) + ", and its health is set to " + ReturnColoredText("1", blueNumber);
        }
        else if (statusName == "Coup De Grace")
        {
            statusDescriptionText.text =
                "Whenever this character kills an enemy, it gains maximum " + ReturnColoredText("Energy", yellow);
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
                " ability, it gains health equal to the amount of health lost";
        }
        else if (statusName == "Masochist")
        {
            statusDescriptionText.text =
                "While this character has " + ReturnColoredText("50%", blueNumber) + " or less health, it has " +
                ReturnColoredText("50", blueNumber) + " bonus " + ReturnColoredText("Critical", yellow) + " chance";
        }
        else if (statusName == "Nimble")
        {
            statusDescriptionText.text =
                "This character's " + ReturnColoredText("Dodge", yellow) + 
                " and " + ReturnColoredText("Parry", yellow) +
                " chance is increased by " + ReturnColoredText("10", blueNumber) ;
        }
        else if (statusName == "Perfect Reflexes")
        {
            statusDescriptionText.text =
                "This character's " + ReturnColoredText("Dodge", yellow) +
                " and " + ReturnColoredText("Parry", yellow) +
                " chance is increased by " + ReturnColoredText("30", blueNumber);
        }
        else if (statusName == "Patient Stalker")
        {
            statusDescriptionText.text =
                "This character has " + ReturnColoredText("1", blueNumber) + " bonus " + ReturnColoredText("Mobility", yellow) +
                " and " + ReturnColoredText("20", blueNumber) + " bonus " + ReturnColoredText("Stamina", yellow) + " while " + ReturnColoredText("Camoflaged", yellow);
        }
        else if (statusName == "Perfect Aim")
        {
            statusDescriptionText.text =
                "This character's " + ReturnColoredText("Ranged Attack", yellow)+ " abilities cannot be " + ReturnColoredText("Dodged", yellow);
        }
        else if (statusName == "Phasing")
        {
            statusDescriptionText.text =
                "The first time this character is the victim of a "+ ReturnColoredText("Melee Attack", yellow) + " ability each turn cycle, it " + 
                ReturnColoredText("Teleports", yellow) +
                " to a random tile within " + ReturnColoredText("2", blueNumber);
        }
        else if (statusName == "Poison Immunity")
        {
            statusDescriptionText.text =
                "This character cannot be " + ReturnColoredText("Poisoned", poison);
        }
        else if (statusName == "Predator")
        {
            statusDescriptionText.text =
                "This character has " + ReturnColoredText("50", blueNumber) + " bonus "+ ReturnColoredText("Critical", yellow) +
                " chance while " + ReturnColoredText("Camoflaged", yellow);
        }
        else if (statusName == "Preparation")
        {
            statusDescriptionText.text =
                "The next ability this character uses costs " + ReturnColoredText("0", blueNumber) + " " +
                ReturnColoredText("Energy", yellow);
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
        else if (statusName == "Berserk")
        {
            statusDescriptionText.text =
                "This character has " + ReturnColoredText("100", blueNumber) + " bonus " +
                ReturnColoredText("Critical", yellow) + " chance until the end of its next activation.";
        }
        else if (statusName == "Riposte")
        {
            statusDescriptionText.text =
                "Whenever this character successfully " + ReturnColoredText("Parries", yellow) + ", it performs " +
                ReturnColoredText("Strike", yellow)+ " against its attacker";
        }
        else if (statusName == "Sacred Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character removes " + ReturnColoredText("Blind", yellow) + ", "
                + ReturnColoredText("Disarmed", yellow) + ", "
                + ReturnColoredText("Silenced", yellow) + ", "
                + ReturnColoredText("Weakened", yellow) + " and "
                + ReturnColoredText("Vulnerable", yellow) +
                " from allies within it's " + ReturnColoredText("Aura", yellow);
        }
        else if (statusName == "Shadow Aura")
        {
            statusDescriptionText.text =
                "On activation end, this character applies "+ ReturnColoredText("Weakened", yellow) +
                " to enemies within its " + ReturnColoredText("Aura", yellow);
        }
        else if (statusName == "Sharpened Blade")
        {
            statusDescriptionText.text =
                "This character's next " + ReturnColoredText("Melee Attack", yellow) + " ability is guaranteed to be a " +
                 ReturnColoredText("Critical", yellow);;
        }
        else if (statusName == "Shatter")
        {
            statusDescriptionText.text =
                "This character's " + ReturnColoredText("Melee Attack", yellow) + " abilities have " +
                ReturnColoredText("50", blueNumber) + " bonus " + ReturnColoredText("Critical", yellow) + 
                " chance against targets with " + ReturnColoredText("Chilled", frost);
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
                ReturnColoredText(statusStacks.ToString(), blueNumber) + " health";
        }
        else if (statusName == "Thorns")
        {
            statusDescriptionText.text =
                "Whenever this character is hit with a " + ReturnColoredText("Melee Attack", yellow) + " ability, it deals " +
                ReturnColoredText(statusStacks.ToString(), blueNumber) + " " + ReturnColoredText("Physical", physical) +
                " damage back to it's attacker";
        }
        else if (statusName == "Time Warp")
        {
            statusDescriptionText.text =
                "This character gains " + ReturnColoredText(statusStacks.ToString(), blueNumber) + " extra activation";
        }
        else if (statusName == "Volatile")
        {
            statusDescriptionText.text =
                "On death, this character explodes, dealing " + ReturnColoredText(statusStacks.ToString(), blueNumber) + " " +
                ReturnColoredText("Physical", physical) + " damage to ALL characters within " + ReturnColoredText("1", blueNumber);
        }
        else if (statusName == "Unstable")
        {
            statusDescriptionText.text =
                "On death, this character explodes, applying " + ReturnColoredText(statusStacks.ToString(), blueNumber) + " " +
                ReturnColoredText("Poisoned", poison) + " to all characters within " + ReturnColoredText("1", blueNumber);
        }
        else if (statusName == "Virtuoso")
        {
            statusDescriptionText.text =
                "This character's " + ReturnColoredText("Melee Attack", yellow) + " abilities cannot be " + ReturnColoredText("Parried", yellow);
        }
        else if (statusName == "Venomous")
        {
            statusDescriptionText.text =
                "Whenever this character applies "+ ReturnColoredText("Poisoned", poison) + 
                ", it applies " + ReturnColoredText(statusStacks.ToString(), blueNumber) + " extra";
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
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Twin Strike")
        {
            // Get off hand weapon data

            string offHandDamageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(entity, ability, entity.myOffHandWeapon);
            int offHandDamageValue = CombatLogic.Instance.GetBaseDamageValue(entity, ability.abilityPrimaryValue, ability, offHandDamageType, entity.myOffHandWeapon);

            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage, then " +
                ReturnColoredText(offHandDamageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(offHandDamageType, GetColorCodeFromString(offHandDamageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Shoot")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Move")
        {
            ability.abilityInfoSheet.descriptionText.text =
                ReturnColoredText("Move", yellow) + " to a tile within " + ReturnColoredText(EntityLogic.GetTotalMobility(entity).ToString(), blueNumber)
                + " of your current position";
        }
        else if (ability.abilityName == "Defend")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Gain " + ReturnColoredText(CombatLogic.Instance.CalculateBlockGainedByEffect(ability.abilityPrimaryValue, entity).ToString(), blueNumber)
                + " " + ReturnColoredText("Block", yellow);
        }
        else if (ability.abilityName == "Inspire")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Increase a target's " + ReturnColoredText("Strength", yellow) + " by " +
                ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber);
        }
        else if (ability.abilityName == "Go Berserk")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Increase a target's " + ReturnColoredText("Critical", yellow) + " chance by " +
                ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber) + " until the end of its next activation";
        }

        else if (ability.abilityName == "Stone Form")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Increase a target's " + ReturnColoredText("Dexterity", yellow) + " by " +
                ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber);
        }
        else if (ability.abilityName == "Charge")
        {
            ability.abilityInfoSheet.descriptionText.text =
                ReturnColoredText("Move", yellow) + " to a target enemy within " + ReturnColoredText((EntityLogic.GetTotalMobility(entity) + ability.abilityRange).ToString(), blueNumber) +
                " and apply " +
                ReturnColoredText("Vulnerable", yellow);
        }
        else if (ability.abilityName == "Whirlwind")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to all enemies within " +
                ReturnColoredText(AbilityLogic.Instance.CalculateAbilityRange(ability, entity).ToString(), blueNumber);
        }
        else if (ability.abilityName == "Fire Ball")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " + ReturnColoredText(ability.abilitySecondaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Burning", fire);
        }
        else if (ability.abilityName == "Shadow Blast")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " +
                ReturnColoredText(ability.myAbilityData.secondaryValue.ToString(), blueNumber) + " " +
                ReturnColoredText("Knock Back", yellow);
        }
        else if (ability.abilityName == "Lightning Bolt")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " + ReturnColoredText("Shocked", air);
        }
        else if (ability.abilityName == "Frost Bolt")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " +
                ReturnColoredText("Immobilized", yellow);
        }
        else if (ability.abilityName == "Pinning Shot")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " +
                ReturnColoredText("Immobilized", yellow);
        }
        else if (ability.abilityName == "Telekinesis")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Teleport a target anywhere within " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber) +
                " of its current position";
        }
        else if (ability.abilityName == "Dash")
        {
            ability.abilityInfoSheet.descriptionText.text =
                ReturnColoredText("Move", yellow) + " to a tile within " + ReturnColoredText((ability.abilityRange + EntityLogic.GetTotalMobility(entity)).ToString(), blueNumber) +
                " of your current position";
        }
        else if (ability.abilityName == "Preparation")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "The next ability you use costs " + ReturnColoredText("0", blueNumber) + " "
                + ReturnColoredText("Energy", yellow);
        }
        else if (ability.abilityName == "Holy Fire")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Give " + ReturnColoredText(CombatLogic.Instance.CalculateBlockGainedByEffect(ability.abilityPrimaryValue, entity).ToString(), blueNumber)
                + " " +
                ReturnColoredText("Block", yellow) +
                " to an ally, or deal " +
                ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to an enemy";
        }
        else if (ability.abilityName == "Invigorate")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Give an ally " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber)
                + " " + ReturnColoredText("Energy", yellow);
        }
        else if (ability.abilityName == "Goblin War Cry")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Give all friendly goblins " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Strength", yellow);
        }
        else if (ability.abilityName == "Chaos Bolt")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target, and apply " +
                ReturnColoredText("Stunned", yellow);
        }
        else if (ability.abilityName == "Ambush")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. If this back strikes, gain " +
                ReturnColoredText(ability.abilitySecondaryValue.ToString(), blueNumber) +
                ReturnColoredText(" Energy", yellow);
        }
        else if (ability.abilityName == "Blade Flurry")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a random enemy within " +
                ReturnColoredText(AbilityLogic.Instance.CalculateAbilityRange(ability, entity).ToString(), blueNumber) +
                " three times";
        }
        else if (ability.abilityName == "Purity")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "While channeled, this character has +2 " + ReturnColoredText("Strength", yellow) + ", " +
                ReturnColoredText("Wisdom", yellow) + " and " +
                ReturnColoredText("Dexterity", yellow);
        }
        else if (ability.abilityName == "Blaze")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Target ally converts all damage from " + ReturnColoredText("Melee Attack", yellow) +
                " abilities into " + ReturnColoredText("Fire", fire) +
                " damage during its next activation";
        }
        else if (ability.abilityName == "Testudo")
        {
            ability.abilityInfoSheet.descriptionText.text =
                 "Gain " +
                ReturnColoredText(CombatLogic.Instance.CalculateBlockGainedByEffect(ability.abilityPrimaryValue, entity).ToString(), blueNumber) +
                " " + ReturnColoredText("Block", yellow);
        }
        else if (ability.abilityName == "Shadow Wreath")
        {
                ability.abilityInfoSheet.descriptionText.text =
                "Target ally converts all damage from " + ReturnColoredText("Melee Attack", yellow) +
                " abilities into " + ReturnColoredText("Shadow", shadow) +
                " damage during its next activation";
        }
        else if (ability.abilityName == "Creeping Frost")
        {
                ability.abilityInfoSheet.descriptionText.text =
                "Target ally converts all damage from " + ReturnColoredText("Melee Attack", yellow) +
                " abilities into " + ReturnColoredText("Frost", frost) +
                " damage during its next activation";
        }
        else if (ability.abilityName == "Overload")
        {
                ability.abilityInfoSheet.descriptionText.text =
                "Target ally converts all damage from " + ReturnColoredText("Melee Attack", yellow) +
                " abilities into " + ReturnColoredText("Air", air) +
                " damage during its next activation";
        }
        else if (ability.abilityName == "Infuse")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "While channeled, you have " + ReturnColoredText("20", yellow) +
                " extra resistance to all damage types";
        }
        else if (ability.abilityName == "Concentration")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "While channeled, your Ranged Attack abilities have +" + ReturnColoredText("20", yellow) +
                " critical chance and cannot be dodged";
        }
        else if (ability.abilityName == "Rapid Cloaking")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "While channeled, gain " + ReturnColoredText("Camoflage", yellow) +
                " on activation end";
        }
        else if (ability.abilityName == "Recklessness")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "While channeled, your " + ReturnColoredText("Melee Attack", yellow) + " abilities have +" + ReturnColoredText("20", yellow) +
                " Critical chance and cannot be parried";
        }
        else if (ability.abilityName == "Bless")
        {
            ability.abilityInfoSheet.descriptionText.text =
            "Give an ally " +
               ReturnColoredText(CombatLogic.Instance.CalculateBlockGainedByEffect(ability.abilityPrimaryValue, entity).ToString(), blueNumber) +
               " " + ReturnColoredText("Block", yellow) +
                ". Removes " + ReturnColoredText("Blind", yellow) + ", " +
               ReturnColoredText("Disarmed", yellow) + " and " +
               ReturnColoredText("Silenced", yellow);
        }
        else if (ability.abilityName == "Blight")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Apply " + ReturnColoredText((ability.abilityPrimaryValue + entity.myPassiveManager.venomousStacks).ToString(), blueNumber) +
                "  " +
                ReturnColoredText("Poisoned", poison) + " to a target";
        }
        else if (ability.abilityName == "Blinding Light")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("Blind", yellow) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Blizzard")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("Chilled", frost) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Toxic Eruption")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText((1 + entity.myPassiveManager.venomousStacks).ToString(), blueNumber) + " " +
                ReturnColoredText("Poisoned", poison) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Meteor")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText(ability.abilitySecondaryValue.ToString(), blueNumber) + " " +
                ReturnColoredText("Burning", fire) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Chloroform Bomb")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " +
                ReturnColoredText("Silenced", yellow) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Thunder Storm")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " +
                ReturnColoredText("Shocked", air) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Rain Of Chaos")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("Weakened", yellow) + " " +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Blink")
        {
            ability.abilityInfoSheet.descriptionText.text =
                ReturnColoredText("Teleport", yellow) + " to a location within " + ReturnColoredText(ability.abilityRange.ToString(), blueNumber +
                " of your current position");
        }
        else if (ability.abilityName == "Blood Offering")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Lose " + ReturnColoredText(ability.abilitySecondaryValue.ToString(), blueNumber) +
                " health, then gain " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Energy", yellow);
        }
        else if (ability.abilityName == "Burst Of Knowledge")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Give an ally " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Wisdom", yellow) + " until the end of their next activation";
        }
        else if (ability.abilityName == "Primal Rage")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Give an ally " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Strength", yellow) + " until the end of their next activation";
        }
        else if (ability.abilityName == "Chain Lightning")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " + ReturnColoredText("Shocked", air)+ 
                ". Jumps to a random adjacent enemy up to 2 times";
        }
        else if (ability.abilityName == "Challenging Shout")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Force all enemies within " + ReturnColoredText(entity.currentMeleeRange.ToString(), blueNumber) +
                " to focus their attacks on you during their next activations";
        }
        else if (ability.abilityName == "Cheap Shot")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. If you have " + ReturnColoredText("Camoflage", yellow) +
                ", apply " + ReturnColoredText("Stunned", yellow);
        }
        else if (ability.abilityName == "Back Stab")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. If this back strikes, it is guaranteed to be a " + 
                ReturnColoredText("Critical", yellow);
        }
        else if (ability.abilityName == "Chemical Reaction")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Double a targets current " + ReturnColoredText("Poisoned", poison) +
                " amount";
        }
        else if (ability.abilityName == "Chilling Blow")
        {
            ability.abilityInfoSheet.descriptionText.text =
                 "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                 ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                 " damage to a target. If the target is " + ReturnColoredText("Chilled", frost) +
                 ", apply " + ReturnColoredText("Immobilized", yellow);
        }
        else if (ability.abilityName == "Combustion")
        {
            ability.abilityInfoSheet.descriptionText.text = 
                "Target an enemy. That target and enemies within " +
                ReturnColoredText("1", blueNumber) +
                " of it take " +
                ReturnColoredText("Fire", fire) + " damage equal to the target's current " +
                ReturnColoredText("Burning", fire) + " amount";
        }
        else if (ability.abilityName == "Concealing Clouds")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Give " + ReturnColoredText("Camoflage", yellow) +
                " to all characters in a 3x3 area";

        }
        else if (ability.abilityName == "Consecrate")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to adjacent enemies. Give " + ReturnColoredText(ability.abilitySecondaryValue.ToString(), blueNumber) + " " +
                ReturnColoredText("Block", yellow) + 
                " to adjacent allies";
        }       
        else if (ability.abilityName == "Decapitate")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. Target is killed instantly if they have " + ReturnColoredText("30%", blueNumber) + 
                " or less health";
        }
        else if (ability.abilityName == "Devastating Blow")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Dimensional Blast")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) +
                " damage of a random damage type to a target";
        }
        else if (ability.abilityName == "Dimensional Hex")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Apply " + ReturnColoredText("Burning", fire) + ", " +
                ReturnColoredText("Poisoned", poison) + ", " +
                ReturnColoredText("Chilled", frost) + " and " +
                ReturnColoredText("Shocked", air) +
                " to a target";
        }
        else if (ability.abilityName == "Disarm")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " + ReturnColoredText("Disarmed", yellow);
        }
        else if (ability.abilityName == "Dragon Breath")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("2", blueNumber) +" " + ReturnColoredText("Burning",fire) +
                " to all characters in a line, up to " + ReturnColoredText(ability.abilitySecondaryValue.ToString(), blueNumber) +
                " tiles away";
        }
        else if (ability.abilityName == "Drain")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Remove all " + ReturnColoredText("Poisoned", poison) +
                " from a target. Deal " + ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage equal to the amount removed";
        }
        else if (ability.abilityName == "Evasion")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Increase an ally's  " + ReturnColoredText("Parry", yellow) +
                " chance by  " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber) +
                " until the end of the current turn cycle";
        }
        else if (ability.abilityName == "Mirage")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Increase an ally's  " + ReturnColoredText("Dodge", yellow) +
                " chance by  " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber) +
                " until the end of the current turn cycle";
        }
        else if (ability.abilityName == "Fire Nova")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply "
                + ReturnColoredText(ability.abilitySecondaryValue.ToString(), blueNumber) + " "
                + ReturnColoredText("Burning", fire) + " to adjacent enemies";
        }
        else if (ability.abilityName == "Frost Nova")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to adjacent enemies, and apply " + ReturnColoredText("Chilled", frost);
        }
        else if (ability.abilityName == "Noxious Fumes")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Apply " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber) + " " +
                ReturnColoredText("Poisoned", poison) + " and " + ReturnColoredText("Silenced", yellow) +
                " to all enemies within your " + ReturnColoredText("Aura", yellow);
                
        }
        else if (ability.abilityName == "Get Down!")
        {
            ability.abilityInfoSheet.descriptionText.text =
                ReturnColoredText("Move", yellow) + " to a tile within " +
                ReturnColoredText((ability.abilityPrimaryValue + EntityLogic.GetTotalMobility(entity)).ToString(), blueNumber) +
                ". At the end of the movement, give " +
                ReturnColoredText(CombatLogic.Instance.CalculateBlockGainedByEffect(ability.abilitySecondaryValue, entity).ToString(), blueNumber) +
                " " + ReturnColoredText("Block", yellow) + " to yourself and adjacent allies";
        }
        else if (ability.abilityName == "Glacial Burst")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a random enemy in your melee range " + ReturnColoredText(ability.abilitySecondaryValue.ToString(), blueNumber) +
                " times";
        }
        else if (ability.abilityName == "Global Cooling")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to all enemies and apply  " + ReturnColoredText("Chilled", frost) +
                ". Characters that are already " + ReturnColoredText("Chilled", frost) + " are " +
                ReturnColoredText("Immobilized", yellow) + " instead";
        }
        else if (ability.abilityName == "Toxic Rain")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Apply " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber) + " " +
                ReturnColoredText("Poisoned", poison) +
                " to all enemies";
        }
        else if (ability.abilityName == "Guard")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Give an ally " +
                ReturnColoredText(CombatLogic.Instance.CalculateBlockGainedByEffect(ability.abilityPrimaryValue, entity).ToString(), blueNumber) +
                " " + ReturnColoredText("Block", yellow);
        }
        else if (ability.abilityName == "Haste")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Increase a target's " + ReturnColoredText("Mobility", yellow) + " by " +
                ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber);
        }
        else if (ability.abilityName == "Head Shot")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Hex")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Apply " + ReturnColoredText("Weakened", yellow) + " and " +
                ReturnColoredText("Vulnerable", yellow) +
                " to a target";
        }
        else if (ability.abilityName == "Dark Gift")
        {
            ability.abilityInfoSheet.descriptionText.text =
                 "Increase all damage dealt by a target by " + ReturnColoredText("50%", blueNumber) +
                " until the end of their next activation";
        }
        else if (ability.abilityName == "Pure Hate")
        {
            ability.abilityInfoSheet.descriptionText.text =
                 "Increase all " + ReturnColoredText("Shadow", shadow) + " damage dealt by this character by " + ReturnColoredText("50%", blueNumber) +
                " until the end of it's next activation";
        }

        else if (ability.abilityName == "Mark Target")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Target an enemy. That character is unable to " + ReturnColoredText("Dodge", yellow) + " or " +
                ReturnColoredText("Parry", yellow) +
                " attacks until the end of the current turn cycle";
        }
            
        else if (ability.abilityName == "Icy Focus")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Increase a target's "+ ReturnColoredText("Wisdom", yellow) + " by " +
                ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber);
        }
        else if (ability.abilityName == "Impaling Bolt")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " +
                ReturnColoredText(ability.abilitySecondaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Knock Back", yellow);
        }
        else if (ability.abilityName == "Judgement")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " +
                ReturnColoredText("Vulnerable", yellow) + " and " +
                ReturnColoredText("Weakened", yellow);
        }
        else if (ability.abilityName == "Kick To The Balls")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Apply " + ReturnColoredText("Stunned", yellow) + " to a target";
        }
        else if (ability.abilityName == "Nightmare")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Apply " + ReturnColoredText("Sleep", yellow) + " to a target";
        }
        else if (ability.abilityName == "Overwatch")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to the first enemy that moves within " +
                ReturnColoredText("5", blueNumber);
        }
        else if (ability.abilityName == "Phase Shift")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Switch locations with a target character via " + ReturnColoredText("Teleport", yellow);
        }
        else if (ability.abilityName == "Phoenix Dive")
        {
            ability.abilityInfoSheet.descriptionText.text =
                ReturnColoredText("Teleport", yellow) + " to a tile within " + ReturnColoredText(ability.abilityRange.ToString(), blueNumber) +
                ". On arrival, apply "+ ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber) + " " +
                ReturnColoredText("Burning", fire) +
                " to adjacent enemies";         

        }
        else if (ability.abilityName == "Provoke")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Force an enemy within " + ReturnColoredText("1", blueNumber) +
                " to focus its attacks on you during its next activation";
        }
        else if (ability.abilityName == "Rapid Fire")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Spend all your " + ReturnColoredText("Energy", yellow) + ". For each " + ReturnColoredText("10", blueNumber) + " " +
                ReturnColoredText("Energy", yellow) + " spent, deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Slice And Dice")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Spend all your " + ReturnColoredText("Energy", yellow) + ". For each " + ReturnColoredText("10", blueNumber) +
                ReturnColoredText("Energy", yellow) + " spent, deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Reactive Armour")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Remove all your " + ReturnColoredText("Block", yellow) +
                ", then deal " + ReturnColoredText(entity.currentBlock.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to all enemies within " + ReturnColoredText(entity.currentMeleeRange.ToString(), blueNumber);
        }
        else if (ability.abilityName == "Second Wind")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Gain " + ReturnColoredText("Energy", yellow) +
                " equal to your maximum";
        }
        else if (ability.abilityName == "Shadow Step")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Target an enemy within " + ReturnColoredText(ability.abilityRange.ToString(), blueNumber) +
                " and " + ReturnColoredText("Teleport", yellow) +
                " to their back tile";
        }
        else if (ability.abilityName == "Shank")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Sharpen Blade")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Your next " + ReturnColoredText("Melee Attack", yellow) + 
                " ability is guaranteed to be a " + ReturnColoredText("Critical", yellow);
        }
        else if (ability.abilityName == "Shield Shatter")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Remove all " + ReturnColoredText("Block", yellow) +
                " from a target, then deal " + ReturnColoredText(damageValue.ToString(), blueNumber) +
                " " + ReturnColoredText(damageType, GetColorCodeFromString(damageType)) + " damage";
        }
        else if (ability.abilityName == "Melt")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Remove all " + ReturnColoredText("Block", yellow) +
                " from a target and apply " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Burning", fire);
        }
        else if (ability.abilityName == "Shield Slam")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(entity.currentBlock.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target, then apply " + ReturnColoredText("1", blueNumber) + " " + 
                ReturnColoredText("Knock Back", yellow);
        }
        else if (ability.abilityName == "Shroud")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Give an ally " + ReturnColoredText("Camoflage", yellow);
        }
        else if (ability.abilityName == "Spirit Vision")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Give an ally " +
                ReturnColoredText(CombatLogic.Instance.CalculateBlockGainedByEffect(ability.abilityPrimaryValue, entity).ToString(), blueNumber) +
                " " + ReturnColoredText("Block", yellow) + " and " +
                ReturnColoredText("True Sight", yellow) + " during their next activation";
        }

        else if (ability.abilityName == "Fortify")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Give an ally " +
                ReturnColoredText(CombatLogic.Instance.CalculateBlockGainedByEffect(ability.abilityPrimaryValue, entity).ToString(), blueNumber) +
                " " + ReturnColoredText("Block", yellow) + ". Removes " +
                ReturnColoredText("Stunned", yellow) + ", " +
                ReturnColoredText("Sleep", yellow) + ",  and " +
                ReturnColoredText("Immobilized", yellow);
        }
        else if (ability.abilityName == "Forest Medicine")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Give an ally " +
                ReturnColoredText(CombatLogic.Instance.CalculateBlockGainedByEffect(ability.abilityPrimaryValue, entity).ToString(), blueNumber) +
                " " + ReturnColoredText("Block", yellow) + ". Removes " +
                ReturnColoredText("Poisoned", poison) + ", " +
                ReturnColoredText("Burning", fire) + ", " +
                ReturnColoredText("Shocked", air) +
                ",  and " +
                ReturnColoredText("Chilled", frost);
        }
        else if (ability.abilityName == "Smash")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Snipe")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Spirit Surge")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Give " + ReturnColoredText(ability.abilityPrimaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Energy", yellow) + 
                " to all allies within your " + ReturnColoredText("Aura", yellow);
        }
        else if (ability.abilityName == "Steady Hands")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Give an ally " + ReturnColoredText("2", blueNumber) +
                " " + ReturnColoredText("Temporary Hawk Eye", yellow);
        }
        else if (ability.abilityName == "Super Conductor")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Spend all your " + ReturnColoredText("Energy", yellow) + ". For each " + ReturnColoredText("10", blueNumber) + " " +
                ReturnColoredText("Energy", yellow) + " spent, deal " +
                ReturnColoredText(damageValue.ToString(), blueNumber) +
                " " + ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target ";
        }
        else if (ability.abilityName == "Sword And Board")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and gain " + 
                ReturnColoredText(CombatLogic.Instance.CalculateBlockGainedByEffect(ability.abilityPrimaryValue, entity).ToString(), blueNumber) +
                " " + ReturnColoredText("Block", yellow);
        }
        else if (ability.abilityName == "Tendon Slash")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("Weakened", yellow);
        }
        else if (ability.abilityName == "Toxic Slash")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + 
                ReturnColoredText((ability.abilityPrimaryValue + entity.myPassiveManager.venomousStacks).ToString(), blueNumber)
                + " "+ ReturnColoredText("Poisoned", poison);
        }
        else if (ability.abilityName == "Thunder Strike")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. If the target is " +
                ReturnColoredText("Shocked", air) + ", apply " +
                ReturnColoredText("Stunned", yellow);
        }
        else if (ability.abilityName == "Thaw")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage. If the target is " + ReturnColoredText("Chilled", frost) +
                ", gain " + ReturnColoredText("20", blueNumber)+" " + ReturnColoredText("Energy", yellow);
        }
        else if (ability.abilityName == "Time Warp")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "When target character finishes their next activation, they activate once more";
        }
        else if (ability.abilityName == "Transcendence")
        {
            ability.abilityInfoSheet.descriptionText.text =
                "Target ally becomes immune to all damage until the end of the current turn cycle";
        }
        else if (ability.abilityName == "Tree Leap")
        {
            ability.abilityInfoSheet.descriptionText.text =
               ReturnColoredText("Teleport", yellow) +
               " to a grass tile within " + ReturnColoredText(ability.abilityRange.ToString(), blueNumber);
        }
        else if (ability.abilityName == "Unbridled Chaos")
        {
            ability.abilityInfoSheet.descriptionText.text =
              "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) +
              " " + ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
              " damage to a random character (yourself included) within your " + 
              ReturnColoredText("Aura", yellow) + " up to " +
              ReturnColoredText(ability.abilitySecondaryValue.ToString(), blueNumber) +
              " times";
        }
        else if (ability.abilityName == "Vanish")
        {
            ability.abilityInfoSheet.descriptionText.text =
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
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Twin Strike")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " main hand " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage, then " +
                ReturnColoredText(damageValue.ToString(), blueNumber) + " off hand " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " to a target";
        }
        else if (ability.abilityName == "Shoot")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Move")
        {
            descriptionText.text =
                ReturnColoredText("Move", yellow) + " to a tile within your " + ReturnColoredText("Mobility", yellow);
        }
        else if (ability.abilityName == "Defend")
        {
            descriptionText.text =
                "Gain " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber)
                + " " + ReturnColoredText("Block", yellow);
        }
        else if (ability.abilityName == "Inspire")
        {
            descriptionText.text =
                "Increase a target's " + ReturnColoredText("Strength", yellow) + " by " +
                ReturnColoredText(ability.primaryValue.ToString(), blueNumber);
        }
        else if (ability.abilityName == "Go Berserk")
        {
            descriptionText.text =
                "Increase a target's " + ReturnColoredText("Critical", yellow) + " chance by " +
                ReturnColoredText(ability.primaryValue.ToString(), blueNumber) + " until the end of its next activation";
        }
        else if (ability.abilityName == "Stone Form")
        {
            descriptionText.text =
                "Increase a target's " + ReturnColoredText("Dexterity", yellow) + " by " +
                ReturnColoredText(ability.primaryValue.ToString(), blueNumber);
        }
        else if (ability.abilityName == "Charge")
        {
            descriptionText.text =
                ReturnColoredText("Move", yellow) + " to a target enemy within " + ReturnColoredText("Mobility", yellow) + " + " +
                ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " and apply " +
                ReturnColoredText("Vulnerable", yellow);
        }
        else if (ability.abilityName == "Whirlwind")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to all enemies within your " +
                ReturnColoredText("Melee Range", yellow);
        }
        else if (ability.abilityName == "Fire Ball")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " + ReturnColoredText(ability.secondaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Burning", fire);
        }
        else if (ability.abilityName == "Shadow Blast")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " +
                ReturnColoredText(ability.secondaryValue.ToString(), blueNumber) + " " +
                ReturnColoredText("Knock Back", yellow);
        }
        else if (ability.abilityName == "Lightning Bolt")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " + ReturnColoredText("Shocked", air);
        }
        else if (ability.abilityName == "Frost Bolt")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " +
                ReturnColoredText("Immobilized", yellow);
        }
        else if (ability.abilityName == "Pinning Shot")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " +
                ReturnColoredText("Immobilized", yellow);
        }
        else if (ability.abilityName == "Telekinesis")
        {
            descriptionText.text =
                ReturnColoredText("Teleport", yellow) + " a target anywhere within " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " of its current position";
        }
        else if (ability.abilityName == "Dash")
        {
            descriptionText.text =
                ReturnColoredText("Move", yellow) + " to a tile within " + ReturnColoredText("Mobility", yellow) +
                " + " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " of your current position";
        }
        else if (ability.abilityName == "Preparation")
        {
            descriptionText.text =
                "The next ability you use costs " + ReturnColoredText("0", blueNumber) +
                ReturnColoredText(" Energy", yellow);
        }
        else if (ability.abilityName == "Holy Fire")
        {
            descriptionText.text =
                "Give " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber)
                + " " +
                ReturnColoredText("Block", yellow) +
                " to an ally, or deal " +
                ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to an enemy";
        }
        else if (ability.abilityName == "Invigorate")
        {
            descriptionText.text =
                "Give an ally " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                ReturnColoredText(" Energy", yellow);
        }
        else if (ability.abilityName == "Goblin War Cry")
        {
            descriptionText.text =
                "Give all friendly goblins " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Strength", yellow);
        }
        else if (ability.abilityName == "Chaos Bolt")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target, and apply " +
                ReturnColoredText("Stunned", yellow);
        }
        else if (ability.abilityName == "Ambush")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. If this back strikes, gain " +
                ReturnColoredText(ability.secondaryValue.ToString(), blueNumber) +
                ReturnColoredText(" Energy", yellow);
        }
        else if (ability.abilityName == "Blade Flurry")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a random enemy within your " +
                ReturnColoredText("Melee Range", yellow) + ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " times";
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
                "Target ally converts all damage from " + ReturnColoredText("Melee Attack", yellow) +
                " abilities into " + ReturnColoredText("Fire", fire) +
                " damage during its next activation";
        }
        else if (ability.abilityName == "Testudo")
        {
            descriptionText.text =
                "Gain " +
                ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Block", yellow);
        }
        else if (ability.abilityName == "Shadow Wreath")
        {
            descriptionText.text =
                "Target ally converts all damage from " + ReturnColoredText("Melee Attack", yellow) +
                " abilities into " + ReturnColoredText("Shadow", shadow) +
                " damage during its next activation";
        }
        else if (ability.abilityName == "Creeping Frost")
        {
            descriptionText.text =
                "Target ally converts all damage from " + ReturnColoredText("Melee Attack", yellow) +
                " abilities into " + ReturnColoredText("Frost", frost) +
                " damage during its next activation";
        }
        else if (ability.abilityName == "Overload")
        {
            descriptionText.text =
                "Target ally converts all damage from " + ReturnColoredText("Melee Attack", yellow) +
                " abilities into " + ReturnColoredText("Air", air) +
                " damage during its next activation";
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
                "Give an ally " +
                ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Block", yellow) +
                 ". Removes " + ReturnColoredText("Blind", yellow) + ", " +
                ReturnColoredText("Disarmed", yellow) + " and " +
                ReturnColoredText("Silenced", yellow);
        }
        else if (ability.abilityName == "Blight")
        {
            descriptionText.text =
                "Apply " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                "  " +
                ReturnColoredText("Poisoned", poison) + " to a target";
        }
        else if (ability.abilityName == "Blinding Light")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("Blind", yellow) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Blizzard")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("Chilled", frost) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Toxic Eruption")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber) + " " +
                ReturnColoredText("Poisoned", poison) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Meteor")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText(ability.secondaryValue.ToString(), blueNumber) + " " +
                ReturnColoredText("Burning", fire) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Chloroform Bomb")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " +
                ReturnColoredText("Silenced", yellow) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Thunder Storm")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " +
                ReturnColoredText("Shocked", air) +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Rain Of Chaos")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("Weakened", yellow) + " " +
                " to all characters in a 3x3 area. ";
        }
        else if (ability.abilityName == "Blink")
        {
            descriptionText.text =
                ReturnColoredText("Teleport", yellow) +
                " to a location within " + ReturnColoredText(ability.range.ToString(), blueNumber);
        }
        else if (ability.abilityName == "Blood Offering")
        {
            descriptionText.text =
                "Lose " + ReturnColoredText(ability.secondaryValue.ToString(), blueNumber) +
                " health, then gain " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Energy", yellow);
        }
        else if (ability.abilityName == "Burst Of Knowledge")
        {
            descriptionText.text =
                "Give an ally " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Wisdom", yellow) + " until the end of their next activation";
        }
        else if (ability.abilityName == "Primal Rage")
        {
            descriptionText.text =
                "Give an ally " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Strength", yellow) + " until the end of their next activation"; ;
        }
        else if (ability.abilityName == "Chain Lightning")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " + ReturnColoredText("Shocked", air) +
                ". Jumps to a random adjacent enemy up to " + ReturnColoredText(ability.secondaryValue.ToString(), blueNumber) +" more times";
        }
        else if (ability.abilityName == "Challenging Shout")
        {
            descriptionText.text =
                "Force all enemies within your " + ReturnColoredText("Melee Range", yellow) +
                " to focus their attacks on you during their next activations";
        }
        else if (ability.abilityName == "Cheap Shot")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. If you have " + ReturnColoredText("Camoflage", yellow) +
                ", apply " + ReturnColoredText("Stunned", yellow);
        }
        else if (ability.abilityName == "Back Stab")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. If this back strikes, it is guaranteed to be a " +
                ReturnColoredText("Critical", yellow);
        }
        else if (ability.abilityName == "Chemical Reaction")
        {
            descriptionText.text =
                "Double a targets current " + ReturnColoredText("Poisoned", poison) +
                " amount";
        }
        else if (ability.abilityName == "Chilling Blow")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. If the target is " + ReturnColoredText("Chilled", frost) +
                ", apply " + ReturnColoredText("Immobilized", yellow);
        }
        else if (ability.abilityName == "Combustion")
        {
            descriptionText.text =
                "Target an enemy. That target and enemies within " +
                ReturnColoredText("1", blueNumber) +
                " of it take " +
                ReturnColoredText("Fire", fire) + " damage equal to the target's current " +
                ReturnColoredText("Burning", fire) + " amount";
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
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to adjacent enemies. Give " + ReturnColoredText(ability.secondaryValue.ToString(), blueNumber) + " " +
                ReturnColoredText("Block", yellow) +
                " to adjacent allies";
        }
        else if (ability.abilityName == "Decapitate")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. Target is killed instantly if they have " + 
                ReturnColoredText("30%", blueNumber) + 
                " or less health";
        }
        else if (ability.abilityName == "Devastating Blow")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Dimensional Blast")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) +
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
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " + ReturnColoredText("Disarmed", yellow);
        }
        else if (ability.abilityName == "Dragon Breath")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("2", blueNumber) + " " + ReturnColoredText("Burning", fire) +
                " to all characters in a line, up to " + ReturnColoredText(ability.secondaryValue.ToString(), blueNumber) +
                " tiles away";
        }
        else if (ability.abilityName == "Drain")
        {
            descriptionText.text =
                "Remove all " + ReturnColoredText("Poisoned", poison) +
                " from a target. Deal " + ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage equal to the amount removed";
        }
        else if (ability.abilityName == "Evasion")
        {
            descriptionText.text =
                "Increase an ally's  " + ReturnColoredText("Parry", yellow) +
                " chance by " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " until the end of the current turn cycle";
        }
        else if (ability.abilityName == "Mirage")
        {
            descriptionText.text =
                "Increase an ally's  " + ReturnColoredText("Dodge", yellow) +
                " chance by  " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " until the end of the current turn cycle";
        }
        else if (ability.abilityName == "Fire Nova")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply "
                + ReturnColoredText(ability.secondaryValue.ToString(), blueNumber) + " "
                + ReturnColoredText("Burning", fire) + " to adjacent enemies";
        }
        else if (ability.abilityName == "Frost Nova")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to adjacent enemies, and apply " + ReturnColoredText("Chilled", frost);
        }
        else if (ability.abilityName == "Noxious Fumes")
        {
            descriptionText.text =
                "Apply " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber) + " " +
                ReturnColoredText("Poisoned", poison) + " and " + ReturnColoredText("Silenced", yellow) +
                " to all enemies within your " + ReturnColoredText("Aura", yellow);
        }
        else if (ability.abilityName == "Get Down!")
        {
            descriptionText.text =
                ReturnColoredText("Move", yellow) + " to a tile within " + ReturnColoredText("Mobility", yellow) +
                " + " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                ". At the end of the movement, give " +
                ReturnColoredText(ability.secondaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Block", yellow) + " to yourself and adjacent allies";
        }
        else if (ability.abilityName == "Glacial Burst")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a random enemy in your " + ReturnColoredText("Melee Range", yellow) + " up to "
                + ReturnColoredText(ability.secondaryValue.ToString(), blueNumber) +
                " times";
        }
        else if (ability.abilityName == "Global Cooling")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to all enemies and apply  " + ReturnColoredText("Chilled", frost) +
                ". Characters that are already " + ReturnColoredText("Chilled", frost) + " are " +
                ReturnColoredText("Immobilized", yellow) + " instead";
        }
        else if (ability.abilityName == "Toxic Rain")
        {
           descriptionText.text =
                "Apply " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber) + " " +
                ReturnColoredText("Poisoned", poison) +
                " to all enemies";
        }
        else if (ability.abilityName == "Guard")
        {
            descriptionText.text =
                "Give an ally " +
                ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Block", yellow);
        }
        else if (ability.abilityName == "Haste")
        {
            descriptionText.text =
                "Increase a target's " + ReturnColoredText("Mobility", yellow) + " by " +
                ReturnColoredText(ability.primaryValue.ToString(), blueNumber);
        }
        else if (ability.abilityName == "Head Shot")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
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
        else if (ability.abilityName == "Dark Gift")
        {
            descriptionText.text =
                "Increase all damage dealt by a target by " + ReturnColoredText("50%", blueNumber) +
                " until the end of their next activation";
        }

        else if (ability.abilityName == "Pure Hate")
        {
            descriptionText.text =
                 "Increase all " + ReturnColoredText("Shadow", shadow) + " damage dealt by this character by " + ReturnColoredText("50%", blueNumber) +
                " until the end of their next activation";
        }
        else if (ability.abilityName == "Mark Target")
        {
            descriptionText.text =
                "Target an enemy. That character is unable to " + ReturnColoredText("Dodge", yellow) + " or " +
                ReturnColoredText("Parry", yellow) +
                " attacks until the end of the current turn cycle";
        }
        else if (ability.abilityName == "Icy Focus")
        {
            descriptionText.text =
                "Increase a target's " + ReturnColoredText("Wisdom", yellow) + " by " +
                ReturnColoredText(ability.primaryValue.ToString(), blueNumber);
        }
        else if (ability.abilityName == "Impaling Bolt")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target and apply " +
                ReturnColoredText(ability.secondaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Knock Back", yellow);
        }
        else if (ability.abilityName == "Judgement")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
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
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to the first enemy that moves within " +
                ReturnColoredText("5", blueNumber);
        }
        else if (ability.abilityName == "Phase Shift")
        {
            descriptionText.text =
                "Switch locations with a target character via " + ReturnColoredText("Teleport", yellow);
        }
        else if (ability.abilityName == "Phoenix Dive")
        {
            descriptionText.text =
                ReturnColoredText("Teleport", yellow) + " to a tile within " + ReturnColoredText(ability.range.ToString(), blueNumber) +
                ". On arrival, apply " + ReturnColoredText("1", blueNumber) + " " +
                ReturnColoredText("Burning", fire) +
                " to adjacent enemies";

        }
        else if (ability.abilityName == "Provoke")
        {
            descriptionText.text =
                "Force an enemy within " + ReturnColoredText("Melee Range", yellow) +
                " to focus its attacks on you during its next activation";
        }
        else if (ability.abilityName == "Rapid Fire")
        {
            descriptionText.text =
                "Spend all your " + ReturnColoredText("Energy", yellow) + ". For each " + ReturnColoredText("10", blueNumber) +
                ReturnColoredText(" Energy", yellow) +" spent, deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Slice And Dice")
        {
            descriptionText.text =
                "Spend all your " + ReturnColoredText("Energy", yellow) + ". For each " + ReturnColoredText("10", blueNumber) +
                ReturnColoredText(" Energy", yellow) + " spent, deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Reactive Armour")
        {
            descriptionText.text =
                "Remove all your " + ReturnColoredText("Block", yellow) +
                ", then deal that amount in " + 
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to all enemies within your " + ReturnColoredText("Melee Range", yellow);
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
                "Target an enemy within " + ReturnColoredText(ability.range.ToString(), blueNumber) +
                " and " + ReturnColoredText("Teleport", yellow) +
                " to their back tile";
        }
        else if (ability.abilityName == "Shank")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Sharpen Blade")
        {
            descriptionText.text =
                "Your next " + ReturnColoredText("Melee Attack", yellow) + " ability is guaranteed to be a " + ReturnColoredText("Critical", yellow);
        }
        else if (ability.abilityName == "Shield Shatter")
        {
            descriptionText.text =
                "Remove all " + ReturnColoredText("Block", yellow) +
                " from a target, then deal " + ReturnColoredText(damageValue.ToString(), blueNumber) +
                " " + ReturnColoredText(damageType, GetColorCodeFromString(damageType)) + " damage";
        }

        else if (ability.abilityName == "Melt")
        {
            descriptionText.text =
                "Remove all " + ReturnColoredText("Block", yellow) +
                " from a target and apply " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Burning", fire);
        }
        else if (ability.abilityName == "Shield Slam")
        {
            descriptionText.text =
                "Deal " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target equal to your current " + ReturnColoredText("Block", yellow) + 
                ", then apply " + ReturnColoredText("1", blueNumber) + ReturnColoredText(" Knock Back", yellow);
        }
        else if (ability.abilityName == "Shroud")
        {
            descriptionText.text =
                "Give an ally " + ReturnColoredText("Camoflage", yellow);
        }
        else if (ability.abilityName == "Spirit Vision")
        {
            descriptionText.text =
                "Give an ally " +
                ReturnColoredText(ability.primaryValue.ToString(), yellow) + 
                " " + ReturnColoredText("Block", yellow) + " and " +
                ReturnColoredText("True Sight", yellow) + " during their next activation";
        }
        else if (ability.abilityName == "Fortify")
        {
            descriptionText.text =
                "Give an ally " +
                ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Block", yellow) + ". Removes " +
                ReturnColoredText("Stunned", yellow) + ", " +
                ReturnColoredText("Sleep", yellow) + ",  and " +
                ReturnColoredText("Immobilized", yellow);
        }
        else if (ability.abilityName == "Forest Medicine")
        {
            descriptionText.text =
                "Give an ally " +
                ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Block", yellow) + ". Removes " +
                ReturnColoredText("Poisoned", poison) + ", " +
                ReturnColoredText("Burning", fire) + ", " +
                ReturnColoredText("Shocked", air) +
                ",  and " +
                ReturnColoredText("Chilled", frost);
        }
        else if (ability.abilityName == "Smash")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Snipe")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Spirit Surge")
        {
            descriptionText.text =
                "Give " + ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Energy", yellow) +
                " to all allies within your " + ReturnColoredText("Aura", yellow);
        }
        else if (ability.abilityName == "Steady Hands")
        {
            descriptionText.text =
                "Give an ally " + ReturnColoredText("2", blueNumber) +
                " " + ReturnColoredText("Temporary Hawk Eye", yellow);
        }
        else if (ability.abilityName == "Super Conductor")
        {
            descriptionText.text =
               "Spend all your " + ReturnColoredText("Energy", yellow) + ". For each " + ReturnColoredText("10", blueNumber) +
                ReturnColoredText(" Energy", yellow) +
                " spent, deal "
                 + ReturnColoredText(damageValue.ToString(), yellow) +
                " " + ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target";
        }
        else if (ability.abilityName == "Sword And Board")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and gain " +
                ReturnColoredText(ability.primaryValue.ToString(), blueNumber) +
                " " + ReturnColoredText("Block", yellow);
        }
        else if (ability.abilityName == "Tendon Slash")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " + ReturnColoredText("Weakened", yellow);
        }
        else if (ability.abilityName == "Toxic Slash")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage and apply " +
                ReturnColoredText(ability.primaryValue.ToString(), blueNumber)
                + " " + ReturnColoredText("Poisoned", poison);
        }
        else if (ability.abilityName == "Thunder Strike")
        {
            descriptionText.text =
                "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage to a target. If the target is " +
                ReturnColoredText("Shocked", air) + ", apply " +
                ReturnColoredText("Stunned", yellow);
        }
        else if (ability.abilityName == "Thaw")
        {
            descriptionText.text =
               "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) + " " +
                ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
                " damage. If the target is " + ReturnColoredText("Chilled", frost) +
                ", gain " + ReturnColoredText("20", blueNumber)
                + ReturnColoredText(" Energy", yellow);
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
        else if (ability.abilityName == "Snow Stasis")
        {
            descriptionText.text =
                "Target yourself or an ally. The next time that character takes damage, ignore it.";
        }
        else if (ability.abilityName == "Tree Leap")
        {
            descriptionText.text =
               ReturnColoredText("Teleport", yellow) +
               " to a grass tile within " + ReturnColoredText(ability.range.ToString(), blueNumber);
        }
        else if (ability.abilityName == "Unbridled Chaos")
        {
            descriptionText.text =
              "Deal " + ReturnColoredText(damageValue.ToString(), blueNumber) +
              " " + ReturnColoredText(damageType, GetColorCodeFromString(damageType)) +
              " damage to a random character (yourself included) within " +
              ReturnColoredText("3", blueNumber) + " tiles up to " +
              ReturnColoredText(ability.secondaryValue.ToString(), blueNumber) +
              " times";
        }
        else if (ability.abilityName == "Vanish")
        {
            descriptionText.text =
              "Immediately gain " + ReturnColoredText("Camoflage", yellow);
        }
    }
    #endregion

    // Get strings, colours and text
    #region
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
   
    public static string GetKingsBlessingChoiceText(string choiceName)
    {
        string stringReturned = "";
        if(choiceName == "Gain 10 Gold")
        {
            stringReturned = ReturnColoredText("Gain 10 Gold", yellow);
        }
        else if(choiceName == "Gain A Random Epic Item. Gain A Random Affliction")
        {
            stringReturned = ReturnColoredText("Gain A Random Epic Item. ", yellow)
                + ReturnColoredText("Gain A Random Affliction.", fire);
        }
        else if (choiceName == "Gain 3 Random Common Items")
        {
            stringReturned = ReturnColoredText("Gain 3 Random Common Items", yellow);
        }
        else if (choiceName == "Gain 3 Random Tier 1 Spell Books")
        {
            stringReturned = ReturnColoredText("Gain 3 Random Tier 1 Spell Books", yellow);
        }
        else if (choiceName == "Enemies In The Next Two Combats Have 50% Health")
        {
            stringReturned = ReturnColoredText("Enemies In The Next Two Combats Have 50% Health", yellow);
        }
        else if (choiceName == "Gain 3 Random Consumables")
        {
            stringReturned = ReturnColoredText("Gain 3 Random Consumables", yellow);
        }
        else if (choiceName == "All Characters Gain 80 XP")
        {
            stringReturned = ReturnColoredText("All Characters Gain 80 XP", yellow);
        }
        else if (choiceName == "Gain A Random Rare State. Gain A Random Affliction")
        {
            stringReturned = ReturnColoredText("Gain A Random Rare State. ", yellow) +
                ReturnColoredText("Gain A Random Affliction.", fire);
        }
        else if (choiceName == "Gain 3 Random Rare Items. Gain A Random Affliction")
        {
            stringReturned = ReturnColoredText("Gain 3 Random Rare Items. ", yellow) +
                ReturnColoredText("Gain A Random Affliction.", fire);
        }
        else if (choiceName == "Gain A Random Rare Weapon")
        {
            stringReturned = ReturnColoredText("Gain A Random Rare Weapon.", yellow);
        }
        else if (choiceName == "Gain A Random Common State")
        {
            stringReturned = ReturnColoredText("Gain A Random Common State.", yellow);
        }
        else if (choiceName == "All Characters Gain 20 Max Health")
        {
            stringReturned = ReturnColoredText("All Characters Gain 20 Max Health.", yellow);
        }

        return stringReturned;
    }
    #endregion

}
