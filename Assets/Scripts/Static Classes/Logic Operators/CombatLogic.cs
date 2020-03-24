using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatLogic : MonoBehaviour
{
    // Initialzation + Singleton Pattern
    #region
    public static CombatLogic Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Conditional checks + booleans
    #region
    public bool IsTargetFriendly(LivingEntity caster, LivingEntity target)
    {
        Debug.Log("CombatLogic.IsTargetFriendly() called, comparing " + caster.myName + " and " + target.myName);

        Defender defender = caster.GetComponent<Defender>();
        Enemy enemy = caster.GetComponent<Enemy>();

        if (defender)
        {
            if (target.GetComponent<Defender>())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (enemy)
        {
            if (target.GetComponent<Enemy>())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        else
        {
            Debug.Log("CombatLogic.IsTargetFriendly() could not detect an enemy or defender script attached to the caster game object...");
            return false;
        }

    }    
    public bool IsProtectedByRune(LivingEntity target)
    {
        Debug.Log("CombatLogic.IsProtectedByRune() called for " + target.name);
        // Method checks for a rune AND ALSO removes a rune when attempting to apply a debuff to target

        if (target.myPassiveManager.rune)
        {
            Debug.Log(target.name + " protected by rune, blocking debuff and removing 1 rune.");
            target.myPassiveManager.ModifyRune(-1);
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    // Damage, Damage Type and Resistance Calculators
    #region
    public int GetBaseDamageValue(LivingEntity entity, int baseDamage, Ability abilityUsed, string attackDamageType, ItemDataSO weaponUsed = null)
    {
        Debug.Log("CombatLogic.GetBaseDamageValue() called...");
        int baseDamageValueReturned = 0;

        // Add damage from weapon, if the weapon is being used for the attack
        if(weaponUsed != null)
        {
            baseDamageValueReturned += weaponUsed.baseDamage;
            Debug.Log(weaponUsed.Name + " base damage is: " + weaponUsed.baseDamage.ToString());
        }
        else
        {
            baseDamageValueReturned += baseDamage;
            Debug.Log("Weapon not used, base damage from ability/effect is: " + baseDamageValueReturned.ToString());
        }        

        // Add flat damage bonus from modifiers (strength, etc)
        if(abilityUsed != null &&
           abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            baseDamageValueReturned += EntityLogic.GetTotalStrength(entity);
            Debug.Log("Base damage after strength and related modifiers added: " + baseDamageValueReturned.ToString());
        }

        // Add damage from damage type modifiers that effect spell damage (wisdom, shadowform etc)
        if (attackDamageType == "Fire" || attackDamageType == "Shadow" || attackDamageType == "Air" || attackDamageType == "Frost" || attackDamageType == "Poison")
        {
            baseDamageValueReturned += EntityLogic.GetTotalWisdom(entity);
            Debug.Log("Base damage after wisdom added: " + baseDamageValueReturned.ToString());
        }

        // multiply by ability damage multiplier if ability uses a weapon
        if(abilityUsed != null &&
          (abilityUsed.requiresMeleeWeapon || abilityUsed.requiresRangedWeapon))
        {
            baseDamageValueReturned = (int)(baseDamageValueReturned * abilityUsed.weaponDamagePercentage);
            Debug.Log("Base damage after ability percentage modifer " + baseDamageValueReturned.ToString());
        }

        // return final value
        Debug.Log("Final base damage value of attack returned: " + baseDamageValueReturned.ToString());
        return baseDamageValueReturned;

    }
    public int GetDamageValueAfterResistances(int damageValue, string attackDamageType, LivingEntity target)
    {
        // Debug
        Debug.Log("CombatLogic.GetDamageValueAfterResistances() called...");
        Debug.Log("Damage Type received as argument: " + attackDamageType);

        // Setup
        int damageValueReturned = damageValue;
        int targetResistance = 0;
        float resistanceMultiplier = 0;

        // Get total resistance
        targetResistance = EntityLogic.GetTotalResistance(target, attackDamageType);

        // Debug
        Debug.Log("Target has " + targetResistance + " total " + attackDamageType + " Resistance...");

        // Invert the resistance value from 100. (as in, 80% fire resistance means the attack will deal 20% of it original damage
        int invertedResistanceValue = 100 - targetResistance;
        Debug.Log("Resitance value after inversion: " + invertedResistanceValue.ToString());

        // Convert target resistance to float to multiply against base damage value
        resistanceMultiplier = (float) invertedResistanceValue / 100;
        Debug.Log("Resitance multiplier as float value: " + resistanceMultiplier.ToString());

        // Apply final resistance calculations to the value returned
        damageValueReturned = (int)(damageValueReturned * resistanceMultiplier);

        Debug.Log("Final damage value calculated: " + damageValueReturned.ToString());

        return damageValueReturned;
    }
    private int GetDamageValueAfterNonResistanceModifiers(int damageValue, LivingEntity attacker, LivingEntity target, Ability abilityUsed, string damageType, bool critical)
    {
        Debug.Log("CombatLogic.GetDamageValueAfterNonResistanceModifiers() called...");

        int damageValueReturned = damageValue;
        float damageModifier = 1f;

        // vulnerable
        if (target.myPassiveManager.vulnerable)
        {
            damageModifier += 0.5f;
            Debug.Log("Damage percentage modifier after 'Vulnerable' bonus: " + damageModifier.ToString());
        }

        // weakened
        if (attacker.myPassiveManager.weakened)
        {
            damageModifier -= 0.5f;
            Debug.Log("Damage percentage modifier after 'Weakened' reduction: " + damageModifier.ToString());
        }

        // critical
        if (critical)
        {
            damageModifier += 0.5f;
            Debug.Log("Damage percentage modifier after 'Critical' bonus: " + damageModifier.ToString());
        }

        // dark gift
        if (attacker.myPassiveManager.darkGift)
        {
            damageModifier += 0.5f;
            Debug.Log("Damage percentage modifier after 'Dark Gift' bonus: " + damageModifier.ToString());
        }

        // TO DO: Damage modifiers related to increasing magical damage by percentage should be moved to a new method (make some like CalculateMagicDamageModifiers())

        // Air Damage bonuses
        if (damageType == "Air")
        {
            if (attacker.myPassiveManager.stormLord)
            {
                Debug.Log("Damage has a type of 'Air', and attacker has 'Storm Lord' passive, increasing damage by 30%...");
                damageModifier += 0.3f;
            }
        }

        // Fire Damage bonuses
        if (damageType == "Fire")
        {
            if (attacker.myPassiveManager.demon)
            {
                Debug.Log("Damage has a type of 'Fire', and attacker has 'Demon' passive, increasing damage by 30%...");
                damageModifier += 0.3f;
            }
        }

        // Poison Damage bonuses
        if (damageType == "Poison")
        {
            if (attacker.myPassiveManager.toxicity)
            {
                Debug.Log("Damage has a type of 'Poison', and attacker has 'Toxicity' passive, increasing damage by 30%...");
                damageModifier += 0.3f;
            }
        }

        // Frost Damage bonuses
        if (damageType == "Frost")
        {
            if (attacker.myPassiveManager.frozenHeart)
            {
                Debug.Log("Damage has a type of 'Frost', and attacker has 'Frozen Heart' passive, increasing damage by 30%...");
                damageModifier += 0.3f;
            }
        }

        // Frost Damage bonuses
        if (damageType == "Shadow")
        {
            if (attacker.myPassiveManager.shadowForm)
            {
                Debug.Log("Damage has a type of 'Shadow', and attacker has 'Shadow Form' passive, increasing damage by 30%...");
                damageModifier += 0.3f;
            }

            if (attacker.myPassiveManager.pureHate)
            {
                Debug.Log("Damage has a type of 'Shadow', and attacker has 'Pure Hate' passive, increasing damage by 50%...");
                damageModifier += 0.5f;
            }
        }

        // Back strike bonuses
        if (abilityUsed != null &&
            PositionLogic.Instance.CanAttackerBackStrikeTarget(attacker, target) && 
            abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            // Opportunist
            if (attacker.myPassiveManager.opportunist)
            {
                damageModifier += (float) attacker.myPassiveManager.opportunistStacks / 100;
                Debug.Log("Damage percentage modifier after 'Opportunist' bonus: " + damageModifier.ToString());
            }
        }

        // prevent modifier from going negative
        if (damageModifier < 0)
        {
            Debug.Log("Damage percentage modifier went into negative, setting to 0");
            damageModifier = 0;
        }

        damageValueReturned = (int) (damageValueReturned * damageModifier);
        Debug.Log("Final damage value returned: " + damageValueReturned);

        return damageValueReturned;

    }
    public int GetFinalDamageValueAfterAllCalculations(LivingEntity attacker, LivingEntity target, Ability abilityUsed, string damageType, bool critical, int baseDamage, ItemDataSO weaponUsed = null)
    {
        Debug.Log("CombatLogic.GetFinalDamageValueAfterAllCalculations() called...");
        int finalDamageValueReturned = 0;

        // calculate base damage
        finalDamageValueReturned = GetBaseDamageValue(attacker, baseDamage, abilityUsed, damageType, weaponUsed);
        Debug.Log("CombatLogic.GetFinalDamageValueAfterAllCalculations() finalDamageValueReturned value after base calculations: " + finalDamageValueReturned.ToString());

        // calculate damage after standard modifiers
        finalDamageValueReturned = GetDamageValueAfterNonResistanceModifiers(finalDamageValueReturned, attacker, target, abilityUsed, damageType, critical);
        Debug.Log("CombatLogic.GetFinalDamageValueAfterAllCalculations() finalDamageValueReturned value after non resistance modifier calculations: " + finalDamageValueReturned.ToString());

        // calcualte damage value after resistances
        if(attacker.defender &&
            StateManager.Instance.DoesPlayerAlreadyHaveState("Godly"))
        {
            Debug.Log("CombatLogic.GetFinalDamageValueAfterAllCalculations() detected that attacker is defender and has state 'Godly', ignoring target resistances...");
        }
        else
        {
            finalDamageValueReturned = GetDamageValueAfterResistances(finalDamageValueReturned, damageType, target);
            Debug.Log("CombatLogic.GetFinalDamageValueAfterAllCalculations() finalDamageValueReturned value after resitance type calculations: " + finalDamageValueReturned.ToString());
        }       

        // return final value
        Debug.Log("CombatLogic.GetFinalDamageValueAfterAllCalculations() finalDamageValueReturned final value returned: " + finalDamageValueReturned.ToString());
        return finalDamageValueReturned;

    }
    public string CalculateFinalDamageTypeOfAttack(LivingEntity entity, Ability abilityUsed, ItemDataSO itemUsed = null)
    {
        Debug.Log("CombatLogic.CalculateFinalDamageTypeOfAttack() called...");
        // preferences
        string damageTypeReturned = "None";

        // First, draw damage type from ability
        damageTypeReturned = AbilityLogic.Instance.GetDamageTypeFromAbility(abilityUsed);

        // Second, if ability uses weapon, get damage type from weapon
        if (itemUsed != null && (abilityUsed.myAbilityData.requiresMeleeWeapon || abilityUsed.myAbilityData.requiresRangedWeapon))
        {
            Debug.Log("CombatLogic.CalculateFinalDamageTypeOfAttack() detected that " + abilityUsed.abilityName + " requires a weapon, drawing damage type from weapon...");
            damageTypeReturned = ItemManager.Instance.GetDamageTypeFromWeapon(itemUsed);
            Debug.Log("Damage type from weapon (" + itemUsed.Name + ") is: " + damageTypeReturned);
        }

        // Third, if character has a 'permanent' imbuement, get damage type from that passive
        if (abilityUsed != null && abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            Debug.Log("CombatLogic.CalculateFinalDamageTypeOfAttack() checking for damage type imbuements...");
            if (entity.myPassiveManager.airImbuement)
            {
                Debug.Log(entity.name + " has Air Imbuement!");
                damageTypeReturned = "Air";
            }
            else if (entity.myPassiveManager.fireImbuement)
            {
                Debug.Log(entity.name + " has Fire Imbuement!");
                damageTypeReturned = "Fire";
            }
            else if (entity.myPassiveManager.poisonImbuement)
            {
                Debug.Log(entity.name + " has Poison Imbuement!");
                damageTypeReturned = "Poison";
            }
            else if (entity.myPassiveManager.frostImbuement)
            {
                Debug.Log(entity.name + " has Frost Imbuement!");
                damageTypeReturned = "Frost";
            }
            else if (entity.myPassiveManager.shadowImbuement)
            {
                Debug.Log(entity.name + " has Shadow Imbuement!");
                damageTypeReturned = "Shadow";
            }
        }

        // Fourth, if character has a temporary imbuement, get damage type from that (override permanent imbuement)
        if (abilityUsed != null && abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            Debug.Log("CombatLogic.CalculateFinalDamageTypeOfAttack() checking for TEMPORARY damage type imbuements...");

            if (entity.myPassiveManager.temporaryAirImbuement)
            {
                Debug.Log(entity.name + " has Temporary Air Imbuement!");
                damageTypeReturned = "Air";
            }
            else if (entity.myPassiveManager.temporaryFireImbuement)
            {
                Debug.Log(entity.name + " has Temporary Fire Imbuement!");
                damageTypeReturned = "Fire";
            }
            else if (entity.myPassiveManager.temporaryPoisonImbuement)
            {
                Debug.Log(entity.name + " has Temporary Poison Imbuement!");
                damageTypeReturned = "Poison";
            }
            else if (entity.myPassiveManager.temporaryFrostImbuement)
            {
                Debug.Log(entity.name + " has Temporary Frost Imbuement!");
                damageTypeReturned = "Frost";
            }
            else if (entity.myPassiveManager.temporaryShadowImbuement)
            {
                Debug.Log(entity.name + " has Temporary Shadow Imbuement!");
                damageTypeReturned = "Shadow";
            }
        }

        Debug.Log("CombatLogic.CalculateFinalDamageTypeOfAttack() final damage type returned: " + damageTypeReturned);
        return damageTypeReturned;
    }
    public string GetRandomDamageType()
    {
        // Setup
        string damageTypeReturned = "Unassigned";
        List<string> allDamageTypes = new List<string> { "Air", "Fire", "Poison", "Physical", "Shadow", "Frost" };

        // Calculate random damage type
        damageTypeReturned = allDamageTypes[Random.Range(0, allDamageTypes.Count)];
        Debug.Log("CombatLogic.GetRandomDamageType() randomly generated a damage type of: " + damageTypeReturned);

        // return damage type
        return damageTypeReturned;
    }
    #endregion

    // Calculate Crit, Parry, Dodge, Block Gain
    #region
    private int CalculateCriticalStrikeChance(LivingEntity attacker, LivingEntity target, Ability ability)
    {
        Debug.Log("CombatLogic.CalculateCriticalChance() called...");

        int critChanceReturned = 0;
        critChanceReturned += EntityLogic.GetTotalCriticalChance(attacker, ability);

        // Check for 'Shatter' passive
        if (attacker.myPassiveManager.shatter &&
            target.myPassiveManager.chilled &&
            ability.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            critChanceReturned += 50;
            Debug.Log("Crit chance after 'Shatter' passive bonus" + critChanceReturned.ToString());
        }

        // Cap Crit Chance at 80%
        if (critChanceReturned > 80)
        {
            Debug.Log("Crit chance excedding crit cap of 80%, reducing to 80%...");
            critChanceReturned = 80;
        }

        // Check for sharpen blade
        if (attacker.myPassiveManager.sharpenedBlade && ability.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            Debug.Log(attacker.name + " has 'Sharpened Blade' passive, increasing Crit chance to 100%...");
            critChanceReturned = 100;
        }

        Debug.Log("Final crit chance value returned: " + critChanceReturned.ToString());
        return critChanceReturned;
    }
    private int CalculateParryChance(LivingEntity target, LivingEntity attacker)
    {
        Debug.Log("CombatLogic.CalculateParryChance() called...");
        int parryChanceReturned = 0;

        // Get total parry chance
        bool swordPlay = false;
        parryChanceReturned += EntityLogic.GetTotalParry(target);
        Debug.Log(target.name + " total parry chance: " + parryChanceReturned.ToString() + "%");

        // Check for sword play
        if (target.myPassiveManager.swordPlay &&
            target.timesMeleeAttackedThisTurnCycle == 0)
        {
            Debug.Log(target.myName + " has 'Sword Play' passive, increasing " + target.myName + " parry chance to 100...");
            parryChanceReturned = 100;
            swordPlay = true;
        }

        // Check for recklessness
        if (attacker.myPassiveManager.recklessness)
        {
            Debug.Log(attacker.name + " has 'Recklessness' passive, reducing " + target.name + " parry chance to 0...");
            parryChanceReturned = 0;
        }

        // Check for Marked
        if (target.myPassiveManager.marked)
        {
            Debug.Log(target.myName + " has 'Marked' passive, reducing " + target.myName + " parry chance to 0...");
            parryChanceReturned = 0;
        }

        // Check for masochist
        if (attacker.myPassiveManager.masochist &&
            (attacker.currentMaxHealth / 2) > attacker.currentHealth
            )
        {
            Debug.Log(attacker.name + " has 'Masochist' passive and is below 50% health, reducing " + target.name + " parry chance to 0...");
            parryChanceReturned = 0;
        }

        // Check for virtuoso
        if (attacker.myPassiveManager.virtuoso)
        {
            Debug.Log(attacker.name + " has 'Virtuoso' passive, reducing " + target.name + " parry chance to 0...");
            parryChanceReturned = 0;
        }

        // Cap Parry Chance at 80%
        if (parryChanceReturned > 80 && swordPlay == false)
        {
            Debug.Log(target.name + " has exceeded the parry chance cap, reducing to 80%...");
            parryChanceReturned = 80;
        }


        Debug.Log("Final parry chance calculated for " + target.name +", being attacked by " + attacker.name + ": " + parryChanceReturned.ToString());
        return parryChanceReturned;
    }
    private int CalculateDodgeChance(LivingEntity target, LivingEntity attacker)
    {
        Debug.Log("CombatLogic.CalculateDodgeChance() called...");
        int dodgeChanceReturned = 0;

        // Get Total Dodge
        dodgeChanceReturned += EntityLogic.GetTotalDodge(target);
        Debug.Log(target.name + " base Dodge chance: " + dodgeChanceReturned.ToString());

        // Check for Perfect aim
        if (attacker.myPassiveManager.perfectAim)
        {
            Debug.Log(attacker.name + " has 'Perfect Aim' passive, reducing " + target.name + " dodge chance to 0...");
            dodgeChanceReturned = 0;
        }

        // Check for Concentration Power
        if (attacker.myPassiveManager.concentration)
        {
            Debug.Log(attacker.name + " has 'Concentration' passive, reducing " + target.name + " dodge chance to 0...");
            dodgeChanceReturned = 0;
        }

        // Check for Marked
        if (target.myPassiveManager.marked)
        {
            Debug.Log(target.myName + " has 'Marked' passive, reducing " + target.myName + " dodge chance to 0...");
            dodgeChanceReturned = 0;
        }

        // Cap Dodge Chance at 80%
        if (dodgeChanceReturned > 80)
        {
            Debug.Log(target.name + " has exceeded the dodge chance cap, reducing to 80%...");
            dodgeChanceReturned = 80;
        }

        Debug.Log("Final dodge chance calculated for " + target.name + ", being attacked by " + attacker.name + ": " + dodgeChanceReturned.ToString());
        return dodgeChanceReturned;
    }
    public int CalculateBlockGainedByEffect(int baseBlockGain, LivingEntity caster)
    {
        Debug.Log("CombatLogic.CalculateBlockGainedByEffect() callled for " + caster.name);

        int valueReturned = baseBlockGain;
        Debug.Log("Base block gain value: " + valueReturned);

        valueReturned += EntityLogic.GetTotalDexterity(caster);
        Debug.Log("Block gain value after dexterity added: " + valueReturned);

        Debug.Log("Final block gain value calculated: " + valueReturned);
        return valueReturned;
    }
    #endregion

    // Roll for Crit, Parry and Dodge
    #region
    public bool RollForCritical(LivingEntity attacker, LivingEntity target, Ability ability)
    {
        Debug.Log("CombatLogic.RollForCritical() called...");
        int critChance = CalculateCriticalStrikeChance(attacker, target, ability);

        int roll = Random.Range(1, 101);
        Debug.Log(attacker.gameObject.name + " rolled a " + roll.ToString() + " to crit");

        if (roll <= critChance)
        {
            Debug.Log(attacker.gameObject.name + " successfully rolled a critical");
            return true;
        }
        else
        {
            Debug.Log(attacker.gameObject.name + " failed to roll a critical ");
            return false;
        }
    }
    public bool RollForParry(LivingEntity target, LivingEntity attacker)
    {
        Debug.Log("CombatLogic.RollForParry() called...");
        int parryChance = CalculateParryChance(target, attacker);

        int roll = Random.Range(1, 101);
        Debug.Log(target.gameObject.name + " rolled a " + roll.ToString() + " to parry");

        if (roll <= parryChance)
        {
            Debug.Log(target.gameObject.name + " successfully rolled a parry");
            return true;
        }
        else
        {
            Debug.Log(target.gameObject.name + " failed to roll a parry ");
            return false;
        }
    }
    public bool RollForDodge(LivingEntity target, LivingEntity attacker)
    {
        Debug.Log("CombatLogic.RollForDodge() called...");
        int dodgeChance = CalculateDodgeChance(target, attacker);

        int roll = Random.Range(1, 101);
        Debug.Log(target.gameObject.name + " rolled a " + roll.ToString() + " to dodge");

        if (roll <= dodgeChance)
        {
            Debug.Log(target.gameObject.name + " successfully rolled a dodge");
            return true;
        }
        else
        {
            Debug.Log(target.gameObject.name + " failed to roll a dodge");
            return false;
        }
    }
    #endregion

    // Handle damage + death + attack related events
    public Action HandleDeath(LivingEntity entity)
    {
        Action action = new Action(true);
        StartCoroutine(HandleDeathCoroutine(entity, action));
        return action;

    }
    public IEnumerator HandleDeathCoroutine(LivingEntity entity, Action action)
    {
        Debug.Log("CombatLogic.HandleDeathCoroutine() started for " + entity.myName);
        entity.inDeathProcess = true;

        LevelManager.Instance.SetTileAsUnoccupied(entity.tile);
        LivingEntityManager.Instance.allLivingEntities.Remove(entity);
        entity.DisableWorldSpaceCanvas();
        //entity.myOnActivationEndEffectsFinished = true;
        ActivationManager.Instance.activationOrder.Remove(entity);

        entity.PlayDeathAnimation();
        Action destroyWindowAction = entity.myActivationWindow.FadeOutWindow();
        yield return new WaitUntil(() => destroyWindowAction.ActionResolved() == true);
        yield return new WaitUntil(() => entity.MyDeathAnimationFinished() == true);        


        // Check volatile
        if (entity.myPassiveManager.Volatile)
        {
            // Notification
            VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Volatile");

            // Calculate which characters are hit by the aoe
            List<LivingEntity> targetsInRange = GetAllLivingEntitiesWithinAoeEffect(entity, entity.tile, 1, true, true);

            // Damage all targets hit
            foreach (LivingEntity targetInBlast in targetsInRange)
            {
                if (targetInBlast.inDeathProcess == false)
                {
                    int finalDamageValue = GetFinalDamageValueAfterAllCalculations(entity, targetInBlast, null, "Physical", false, entity.myPassiveManager.volatileStacks);
                    Action volatileExplosion = HandleDamage(finalDamageValue, null, targetInBlast, "Physical");
                    yield return new WaitUntil(() => volatileExplosion.ActionResolved() == true);
                }
            }

            yield return new WaitForSeconds(1);
        }

        // Check unstable
        if (entity.myPassiveManager.unstable)
        {
            // Notification
            VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Unstable");

            // Calculate which characters are hit by the aoe
            List<LivingEntity> targetsInRange = GetAllLivingEntitiesWithinAoeEffect(entity, entity.tile, 1, true, true);

            // Poison all targets hit
            foreach (LivingEntity targetInBlast in targetsInRange)
            {
                if (targetInBlast.inDeathProcess == false)
                {
                    targetInBlast.myPassiveManager.ModifyPoisoned(entity.myPassiveManager.unstableStacks);
                }
            }

            yield return new WaitForSeconds(1);
        }

        // Remove entity from relavent lists
        if (entity.defender)
        {
            DefenderManager.Instance.allDefenders.Remove(entity.defender);
        }
        else if (entity.enemy)
        {
            entity.enemy.currentlyActivated = false;
            EnemyManager.Instance.allEnemies.Remove(entity.enemy);
        }      


        // Depending on the state of the combat, decide which ending or continuation occurs

        // check if the player has lost all characters and thus the game
        if (DefenderManager.Instance.allDefenders.Count == 0 &&
            EventManager.Instance.currentCombatEndEventTriggered == false)
        {
            Debug.Log("CombatLogic.HandleDeath() detected player has lost all defenders...");
            EventManager.Instance.currentCombatEndEventTriggered = true;
            EventManager.Instance.StartNewGameOverDefeatedEvent();
        }

        // check if this was the last enemy in the encounter
        else if (EnemyManager.Instance.allEnemies.Count == 0 &&
            DefenderManager.Instance.allDefenders.Count >= 1 &&
            EventManager.Instance.currentCombatEndEventTriggered == false)
        {
            Debug.Log("CombatLogic.HandleDeath() detected that all enemies have been killed...");

            // Trigger combat victory event depending on current encounter type
            if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.EliteEnemy)
            {
                EventManager.Instance.currentCombatEndEventTriggered = true;
                EventManager.Instance.StartNewEndEliteEncounterEvent();
            }
            else if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.BasicEnemy)
            {
                EventManager.Instance.currentCombatEndEventTriggered = true;
                EventManager.Instance.StartNewEndBasicEncounterEvent();
            }
            else if (EventManager.Instance.currentEncounterType == WorldEncounter.EncounterType.Boss)
            {
                EventManager.Instance.currentCombatEndEventTriggered = true;
                EventManager.Instance.StartNewEndBossEncounterEvent();
            }

        }

        // Destroy character GO
        Debug.Log("Destroying " + entity.myName + " game object");
        Destroy(entity.gameObject);

        // Resolve
        action.actionResolved = true;
        
    }
    public Action HandleDamage(int damageAmount, LivingEntity attacker, LivingEntity victim, string damageType, Ability abilityUsed = null, bool ignoreBlock = false)
    {
        Debug.Log("CombatLogic.NewHandleDamage() called...");
        Action action = new Action(true);
        StartCoroutine(HandleDamageCoroutine(damageAmount, attacker, victim, damageType, action, abilityUsed, ignoreBlock));
        return action;
    }
    private IEnumerator HandleDamageCoroutine(int damageAmount, LivingEntity attacker, LivingEntity victim, string damageType, Action action, Ability abilityUsed = null, bool ignoreBlock = false)
    {
        // Debug setup
        string abilityNameString = "None";
        string attackerName = "No Attacker";

        if(attacker != null)
        {
            attackerName = attacker.myName;
        }
        
        if(abilityUsed != null)
        {
            abilityNameString = abilityUsed.abilityName;
        }

        Debug.Log("CombatLogic.NewHandleDamageCoroutine() started: damageAmount (" + damageAmount.ToString() + "), attacker (" + attackerName +
            "), damageType(" + damageType + "), abilityUsed (" + abilityNameString + "), ignoreBlock (" + ignoreBlock.ToString()
            );

        // Cancel this if character is already in death process
        if (victim.inDeathProcess)
        {
            Debug.Log("CombatLogic.NewHandleDamageCoroutine() detected that " + victim.myName + " is already in death process, exiting damage event...");
            action.actionResolved = true;
            yield break;
        }

        // Establish properties for this damage event
        int totalLifeLost = 0;
        int adjustedDamageValue = damageAmount;
        int startingBlock = victim.currentBlock;
        int blockAfter = victim.currentBlock;
        int healthAfter = victim.currentHealth;

        // play impact VFX
        if (abilityUsed != null &&
            abilityUsed.abilityType != AbilityDataSO.AbilityType.None)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateImpactEffect(victim.transform.position));

            // if melee attack, play melee attack vfx
            if (abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
            {
                StartCoroutine(VisualEffectManager.Instance.CreateMeleeAttackEffect(victim.transform.position));
            }
        }

        // Check for no block
        if (victim.currentBlock == 0)
        {
            healthAfter = victim.currentHealth - adjustedDamageValue;
            blockAfter = 0;
        }

        // Check for block
        else if (victim.currentBlock > 0)
        {
            if(ignoreBlock == false)
            {
                blockAfter = victim.currentBlock;
                Debug.Log("block after = " + blockAfter);
                blockAfter = blockAfter - adjustedDamageValue;
                Debug.Log("block after = " + blockAfter);
                if (blockAfter < 0)
                {
                    healthAfter = victim.currentHealth;
                    healthAfter += blockAfter;
                    blockAfter = 0;
                    Debug.Log("block after = " + blockAfter);
                }
            }            

            // Check if damage event ignores block (poisoned, burning, pierce, etc)
            else if (ignoreBlock)
            {
                blockAfter = victim.currentBlock;
                Debug.Log("block after = " + blockAfter);
                healthAfter = victim.currentHealth - adjustedDamageValue;
            }

        }

        // Check for damage immunity passives
        if (victim.myPassiveManager.barrier || victim.myPassiveManager.transcendence)
        {            
            // Check for transcendence
            if (victim.myPassiveManager.transcendence)
            {
                yield return new WaitForSeconds(0.5f);
                VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "Transcendence!");
                adjustedDamageValue = 0;
                healthAfter = victim.currentHealth;                
            }

            // Check for barrier
            else if (victim.myPassiveManager.barrier && healthAfter < victim.currentHealth)
            {
                yield return new WaitForSeconds(0.5f);
                VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "Barrier!");
                adjustedDamageValue = 0;
                healthAfter = victim.currentHealth;
                victim.myPassiveManager.ModifyBarrier(-1);               
            }

        }           

        // Finished calculating the final damage, health lost and armor lost: p
        totalLifeLost = victim.currentHealth - healthAfter;
        victim.ModifyCurrentHealth(-totalLifeLost);
        victim.SetCurrentBlock(blockAfter);

        // Play VFX depending on whether the victim lost health, block, or was damaged by poison
        if (adjustedDamageValue > 0)
        {
            if (totalLifeLost == 0 && blockAfter < startingBlock)
            {
                // Create Lose Armor Effect
                StartCoroutine(VisualEffectManager.Instance.CreateLoseBlockEffect(victim.transform.position, adjustedDamageValue));
            }
            else if (totalLifeLost > 0)
            {
                // Create Lose hp / damage effect
                StartCoroutine(VisualEffectManager.Instance.CreateDamageEffect(victim.transform.position, totalLifeLost));

                // Play hurt animation
                if (victim.currentHealth > 0 && totalLifeLost > 0)
                {
                    victim.PlayHurtAnimation();
                }
            }
        }

        // Update character data if victim is a defender
        if (victim.defender != null && totalLifeLost > 0)
        {
            victim.defender.myCharacterData.ModifyCurrentHealth(-totalLifeLost);

            // flick bool for scoring
            EventManager.Instance.damageTakenThisEncounter = true;
        }

        // Remove camoflage from victim if damage was taken
        if (victim.myPassiveManager.camoflage)
        {
            yield return new WaitForSeconds(0.5f);
            victim.myPassiveManager.ModifyCamoflage(-1);            
        }

        // Life steal
        if (attacker != null &&
            attacker.myPassiveManager.lifeSteal && totalLifeLost > 0 &&
            abilityUsed != null &&
            abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log(attacker.name + " has 'Life Steal', healing for " + totalLifeLost.ToString() + " damage");
            attacker.ModifyCurrentHealth(totalLifeLost);
        }

        // Poisonous trait
        if (attacker != null &&
            attacker.myPassiveManager.poisonous && totalLifeLost > 0 &&
            abilityUsed != null &&
            (abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack ||
            abilityUsed.abilityType == AbilityDataSO.AbilityType.RangedAttack))
        {
            Debug.Log(attacker.name + " has 'Poisonous', applying " + attacker.myPassiveManager.poisonousStacks.ToString() + " 'Poisoned'");
            yield return new WaitForSeconds(0.5f);
            victim.myPassiveManager.ModifyPoisoned(attacker.myPassiveManager.poisonousStacks, attacker);
        }

        // Immolation trait
        if (attacker != null &&
            attacker.myPassiveManager.immolation && totalLifeLost > 0 &&
            abilityUsed != null &&
            (abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack ||
            abilityUsed.abilityType == AbilityDataSO.AbilityType.RangedAttack))
        {
            Debug.Log(attacker.name + " has 'Immolation', applying " + attacker.myPassiveManager.immolationStacks.ToString() + " 'Burning'");
            yield return new WaitForSeconds(0.5f);
            victim.myPassiveManager.ModifyBurning(attacker.myPassiveManager.immolationStacks, attacker);
        }

        // Remove sleeping
        if (victim.myPassiveManager.sleep && totalLifeLost > 0)
        {
            Debug.Log(victim.name + " took damage and is sleeping, removing sleep");
            yield return new WaitForSeconds(0.5f);
            victim.myPassiveManager.ModifySleep(-victim.myPassiveManager.sleepStacks);
        }

        // Enrage
        if (victim.myPassiveManager.enrage && totalLifeLost > 0)
        {
            Debug.Log(victim.name + " 'Enrage' triggered, gaining " + victim.myPassiveManager.enrage.ToString() + " bonus strength");
            yield return new WaitForSeconds(0.5f);
            victim.myPassiveManager.ModifyBonusStrength(victim.myPassiveManager.enrageStacks);
        }

        // Tenacious
        if (victim.myPassiveManager.tenacious && totalLifeLost > 0)
        {
            Debug.Log(victim.name + " 'Tenacious' triggered, gaining" + (CalculateBlockGainedByEffect(victim.myPassiveManager.tenaciousStacks, victim).ToString() + " block"));
            yield return new WaitForSeconds(0.5f);
            VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "Tenacious!");
            yield return new WaitForSeconds(0.5f);
            victim.ModifyCurrentBlock(CalculateBlockGainedByEffect(victim.myPassiveManager.tenaciousStacks, victim));
        }

        // Thorns
        if (victim.myPassiveManager.thorns && attacker != null)
        {
            if (abilityUsed != null &&
                abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
            {
                yield return new WaitForSeconds(0.5f);
                VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "Thorns");
                Debug.Log(victim.name + " has thorns and was struck by a melee attack, returning damage...");
                int finalThornsDamageValue = GetFinalDamageValueAfterAllCalculations(victim, attacker, null, "Physical", false, victim.myPassiveManager.thornsStacks);
                Action thornsDamage = HandleDamage(finalThornsDamageValue, victim, attacker, "Physical");
                yield return new WaitUntil(() => thornsDamage.ActionResolved() == true);
            }
        }

        // Phasing
        if (victim.currentHealth > 0 &&
            victim.timesMeleeAttackedThisTurnCycle == 0 &&
            victim.myPassiveManager.phasing &&
            abilityUsed != null &&
            abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            Debug.Log(victim.name + "'Phasing' triggered, teleporting....");
            VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "Phasing");
            Action phasingAction = victim.StartPhasingMove();
            yield return new WaitUntil(() => phasingAction.ActionResolved() == true);
        }

        // Increment times attack counter
        if(abilityUsed != null &&
           abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            victim.timesMeleeAttackedThisTurnCycle++;
        }

        // Check if the victim was killed by the damage
        if (victim.currentHealth <= 0 && victim.inDeathProcess == false)
        {
            Debug.Log(victim.name + " has lost enough health to be killed by this damage event...");

            // Check for last stand passive
            if (victim.myPassiveManager.lastStand)
            {
                Debug.Log(victim.name + " has 'Last Stand' passive, preventing death...");

                // VFX Notification
                yield return new WaitForSeconds(0.5f);
                VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "Last Stand!");
                yield return new WaitForSeconds(0.5f);

                // Set victim at 1hp
                victim.ModifyCurrentHealth(1);

                // Remove last stand
                victim.myPassiveManager.ModifyLastStand(-victim.myPassiveManager.lastStandStacks);

                // Gain 5 strength
                victim.myPassiveManager.ModifyBonusStrength(5);
                yield return new WaitForSeconds(0.5f);
            }

            // Check for Blessing of Undeath 
            else if(victim.defender && StateManager.Instance.DoesPlayerAlreadyHaveState("Blessing Of Undeath"))
            {
                Debug.Log(victim.name + " is protected by 'Blessing Of Undeath' state, preventing death...");

                // VFX Notification
                yield return new WaitForSeconds(0.5f);
                VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "Blessing Of Undeath!");
                yield return new WaitForSeconds(0.5f);

                // Set victim at 50% HP
                victim.ModifyCurrentHealth(victim.currentMaxHealth / 2);

                // Reduce blessing of undeath counter, check for removal
                State blessingOfUndeathState = StateManager.Instance.GetActiveStateByName("Blessing Of Undeath");

                blessingOfUndeathState.ModifyCountdown(-1);                
                yield return new WaitForSeconds(0.5f);

            }

            else
            {
                Debug.Log(victim.name + " has no means to prevent death, starting death process...");

                // check for coup de grace passive on attacker
                if (attacker != null &&
                    attacker.myPassiveManager.coupDeGrace)
                {
                    Debug.Log(attacker.myName + " killed " + victim.myName + 
                        " and has 'Coup De Grace passive, gaining max energy...");

                    attacker.ModifyCurrentEnergy(attacker.currentMaxEnergy);
                }

                // the victim was killed, start death process
                Action deathAction = HandleDeath(victim);
                yield return new WaitUntil(() => deathAction.ActionResolved() == true);
            }
            
        }       

        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;
    }
    public Action HandleParry(LivingEntity attacker, LivingEntity target)
    {
        Debug.Log("CombatLogic.HandleParry() called...");
        Action action = new Action(true);
        StartCoroutine(HandleParryCoroutine(attacker, target, action));
        return action;
    }
    private IEnumerator HandleParryCoroutine(LivingEntity attacker, LivingEntity target, Action action)
    {
        target.timesMeleeAttackedThisTurnCycle++;

        if (target.myPassiveManager.riposte)
        {
            Debug.Log(target.name + " parried an attack against " + attacker.name + " AND has riposte, performing riposte attack...");
            VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Riposte!");
            yield return new WaitForSeconds(0.5f);

            // Perform riposte attack
            Action parryAttackAction = AbilityLogic.Instance.PerformRiposteAttack(target, attacker);
            yield return new WaitUntil(() => parryAttackAction.ActionResolved() == true);
        }
        else
        {
            Debug.Log(target.name + " parried an attack against " + attacker.name);
            VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Parry!");
        }

        yield return new WaitForSeconds(0.5f);

        action.actionResolved = true;
    }
    public Action HandleDodge(LivingEntity attacker, LivingEntity target)
    {
        Debug.Log("CombatLogic.HandleDodge() called...");
        Action action = new Action(true);
        StartCoroutine(HandleDodgeCoroutine(attacker, target, action));
        return action;
    }
    private IEnumerator HandleDodgeCoroutine(LivingEntity attacker, LivingEntity target, Action action)
    {
        VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Dodge!");        

        yield return new WaitForSeconds(0.5f);

        action.actionResolved = true;
    }

    // Aoe calculators
    public List<LivingEntity> GetAllLivingEntitiesWithinAoeEffect(LivingEntity caster, Tile aoeCentrePoint, int blastRadius, bool friendlyFire, bool removeCentrePoint)
    {
        Debug.Log("CombatLogic.GetAllLivingEntitiesWithinAoeEffect() called...");

        List<LivingEntity> targetsInAoeEffect = new List<LivingEntity>();
        List<Tile> tilesInBlastRadius = LevelManager.Instance.GetTilesWithinRange(blastRadius, aoeCentrePoint, removeCentrePoint);

        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (tilesInBlastRadius.Contains(entity.tile))
            {
                if (IsTargetFriendly(caster, entity) == false)
                {
                    targetsInAoeEffect.Add(entity);
                }
                else if (IsTargetFriendly(caster, entity) && friendlyFire)
                {
                    targetsInAoeEffect.Add(entity);
                }
            }
        }

        return targetsInAoeEffect;
    }
}
