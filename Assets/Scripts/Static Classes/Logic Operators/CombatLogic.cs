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

    // AoE + Attack events
    #region
    public void CreateAoEAttackEvent(LivingEntity attacker, Ability abilityUsed, Tile aoeCentrePoint, int aoeSize, bool excludeCentreTile, bool friendlyFire)
    {
        Debug.Log("Starting new AoE damage event...");

        Defender defender = attacker.GetComponent<Defender>();
        Enemy enemy = attacker.GetComponent<Enemy>();

        List<Tile> tilesInAoERadius = LevelManager.Instance.GetTilesWithinRange(aoeSize, aoeCentrePoint, excludeCentreTile);
        List<LivingEntity> livingEntitiesWithinAoeRadius = new List<LivingEntity>();
        List<LivingEntity> finalList = new List<LivingEntity>();

        // Get all living entities within the blast radius
        foreach (LivingEntity livingEntity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (tilesInAoERadius.Contains(livingEntity.tile))
            {
                livingEntitiesWithinAoeRadius.Add(livingEntity);
            }
        }

        // if this ability doesn't not damage friendly targets, filter out friendly targets from the list
        if(friendlyFire == false)
        {
            foreach (LivingEntity livingEntity in livingEntitiesWithinAoeRadius)
            {
                if (defender && livingEntity.GetComponent<Enemy>())
                {
                    finalList.Add(livingEntity);
                }

                else if (enemy && livingEntity.GetComponent<Defender>())
                {
                    finalList.Add(livingEntity);
                }
            }
        }

        // Else if this ability can damage everything, add all living entities within the blast radius to the final list
        else if(friendlyFire == true)
        {
            finalList.AddRange(livingEntitiesWithinAoeRadius);
        }

        // Deal damage to all characters in the final list
        foreach (LivingEntity livingEntity in finalList)
        {
            //livingEntity.HandleDamage(livingEntity.CalculateDamage(abilityUsed.abilityPrimaryValue, livingEntity, attacker), attacker, true);
            Action abilityAction = HandleDamage(abilityUsed.abilityPrimaryValue, attacker, livingEntity, false, abilityUsed.abilityAttackType, abilityUsed.abilityDamageType);            
        }
    }

    // Overload method used for aoe damage events not caused by spells/abilities (e.g. volatile passive)
    public void CreateAoEAttackEvent(LivingEntity attacker, int damage, Tile aoeCentrePoint, int aoeSize, bool excludeCentreTile, bool friendlyFire, AbilityDataSO.DamageType damageType)
    {
        Debug.Log("Starting new AoE damage event...");

        Defender defender = attacker.GetComponent<Defender>();
        Enemy enemy = attacker.GetComponent<Enemy>();

        List<Tile> tilesInAoERadius = LevelManager.Instance.GetTilesWithinRange(aoeSize, aoeCentrePoint, excludeCentreTile);
        List<LivingEntity> livingEntitiesWithinAoeRadius = new List<LivingEntity>();
        List<LivingEntity> finalList = new List<LivingEntity>();

        // Get all living entities within the blast radius
        foreach (LivingEntity livingEntity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (tilesInAoERadius.Contains(livingEntity.tile))
            {
                livingEntitiesWithinAoeRadius.Add(livingEntity);
            }
        }

        // if this ability doesn't not damage friendly targets, filter out friendly targets from the list
        if (friendlyFire == false)
        {
            foreach (LivingEntity livingEntity in livingEntitiesWithinAoeRadius)
            {
                if (defender && livingEntity.GetComponent<Enemy>())
                {
                    finalList.Add(livingEntity);
                }

                else if (enemy && livingEntity.GetComponent<Defender>())
                {
                    finalList.Add(livingEntity);
                }
            }
        }

        // Else if this ability can damage everything, add all living entities within the blast radius to the final list
        else if (friendlyFire == true)
        {
            finalList.AddRange(livingEntitiesWithinAoeRadius);
        }

        // Deal damage to all characters in the final list
        foreach (LivingEntity livingEntity in finalList)
        {
            //livingEntity.HandleDamage(livingEntity.CalculateDamage(damage, livingEntity, attacker), attacker, true);
            HandleDamage(damage, attacker, livingEntity);
        }
    }
    #endregion

    // Damage + damage calculations
    #region
    public Action HandleDamage(int damageAmount, LivingEntity attacker, LivingEntity victim, bool playVFXInstantly = false, AbilityDataSO.AttackType attackType = AbilityDataSO.AttackType.None, AbilityDataSO.DamageType damageType = AbilityDataSO.DamageType.None, Ability abilityUsed = null)
    {
        Action action = new Action();
        StartCoroutine(HandleDamageCoroutine(damageAmount, attacker, victim, action, playVFXInstantly, attackType, damageType, abilityUsed));
        return action;
    }
    public IEnumerator HandleDamageCoroutine(int damageAmount, LivingEntity attacker, LivingEntity victim, Action action, bool playVFXInstantly = false, AbilityDataSO.AttackType attackType = AbilityDataSO.AttackType.None, AbilityDataSO.DamageType damageType = AbilityDataSO.DamageType.None, Ability abilityUsed = null)
    {
        // Establish properties for this damage event
        int totalLifeLost = 0;
        int adjustedDamageValue = damageAmount;
        int blockAfter = victim.currentBlock;
        int healthAfter = victim.currentHealth;
        bool criticalSuccesful = RollForCritical(attacker, abilityUsed);

        // play impact VFX
        if (attackType != AbilityDataSO.AttackType.None)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateImpactEffect(victim.transform.position));

            // if melee attack, play melee attack vfx
            if (attackType == AbilityDataSO.AttackType.Melee)
            {
                StartCoroutine(VisualEffectManager.Instance.CreateMeleeAttackEffect(victim.transform.position));
            }
        }

        if (damageType != AbilityDataSO.DamageType.Poison)
        {
            adjustedDamageValue = CalculateDamage(damageAmount, victim, attacker, damageType, criticalSuccesful, attackType, abilityUsed);
        }           

        if (victim.currentBlock == 0)
        {
            healthAfter = victim.currentHealth - adjustedDamageValue;
            blockAfter = 0;
        }

        else if (victim.currentBlock > 0)
        {
            if(damageType != AbilityDataSO.DamageType.Poison)
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

            else if (damageType == AbilityDataSO.DamageType.Poison)
            {
                blockAfter = victim.currentBlock;
                Debug.Log("block after = " + blockAfter);
                healthAfter = victim.currentHealth - adjustedDamageValue;
            }
        }

        if (victim.myPassiveManager.barrier && healthAfter < victim.currentHealth)
        {
            adjustedDamageValue = 0;
            healthAfter = victim.currentHealth;
            victim.myPassiveManager.ModifyBarrier(-1);
            yield return new WaitForSeconds(0.3f);
        }

        if(victim.myPassiveManager.sleep && healthAfter < victim.currentHealth)
        {
            victim.myPassiveManager.ModifySleep(-victim.myPassiveManager.sleepStacks);
            yield return new WaitForSeconds(0.3f);
        }

        // Finished calculating the final damage, health lost and armor lost: p
        totalLifeLost = victim.currentHealth - healthAfter;
        //victim.currentHealth = healthAfter;
        victim.ModifyCurrentHealth(-totalLifeLost);
        victim.SetCurrentBlock(blockAfter);

        // Play VFX depending on whether the victim lost health, block, or was damaged by poison
        if (adjustedDamageValue > 0)
        {
            if (criticalSuccesful)
            {
                // Create critical status effect text
                StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL", false, "Yellow"));
            }

            if(damageType == AbilityDataSO.DamageType.Poison)
            {
                // Create damaged by poison effect
                StartCoroutine(VisualEffectManager.Instance.CreateDamagedByPoisonEffect(victim.transform.position));
                StartCoroutine(VisualEffectManager.Instance.CreateDamageEffect(victim.transform.position, totalLifeLost, playVFXInstantly));
            }
            else if(totalLifeLost == 0)
            {
                // Create Lose Armor Effect
                StartCoroutine(VisualEffectManager.Instance.CreateLoseBlockEffect(victim.transform.position, adjustedDamageValue));
            }
            else if (totalLifeLost > 0)
            {
                // Create Lose hp / damage effect
                StartCoroutine(VisualEffectManager.Instance.CreateDamageEffect(victim.transform.position, totalLifeLost, playVFXInstantly));
            }
        }

        // Update character data if victim is a defender
        if (victim.defender != null && totalLifeLost > 0)
        {
            victim.defender.myCharacterData.ModifyCurrentHealth(-totalLifeLost);
        }

        // Remove camoflage from victim if damage was taken
        if (victim.myPassiveManager.camoflage)
        {
            victim.myPassiveManager.ModifyCamoflage(-1);
            yield return new WaitForSeconds(0.3f);
        }

        // TO DO: removing stealth from ability use shouldnt be in HandleDamage(). should make a whole new system for 
        // resolving abilities being used and their effects

        // Remove camoflage from attacker if ability used was not 'Move'
        if (abilityUsed != null &&
            abilityUsed.abilityName != "Move")
        {
            attacker.myPassiveManager.ModifyCamoflage(1);
            yield return new WaitForSeconds(0.3f);
        }

        // Check the victim's and attacker's passive traits that are related to taking damage 

        // Life steal
        if (attacker.myPassiveManager.lifeSteal && totalLifeLost > 0)
        {
            attacker.ModifyCurrentHealth(totalLifeLost);
        }

        // Poisonous trait
        if (attacker.myPassiveManager.poisonous && totalLifeLost > 0 && attackType == AbilityDataSO.AttackType.Melee)
        {           
            victim.myPassiveManager.ModifyPoisoned(attacker.myPassiveManager.poisonousStacks, attacker);
            yield return new WaitForSeconds(0.3f);
        }

        // Remove sleeping
        if (victim.myPassiveManager.sleep && totalLifeLost > 0)
        {
            Debug.Log("Damage taken, removing sleep");
            victim.myPassiveManager.ModifySleep(-victim.myPassiveManager.sleepStacks);
            yield return new WaitForSeconds(0.3f);
        }

        // Enrage
        if (victim.myPassiveManager.enrage && totalLifeLost > 0)
        {
            Debug.Log("Enrage triggered, gaining strength");
            victim.ModifyCurrentStrength(victim.myPassiveManager.enrageStacks);
            yield return new WaitForSeconds(0.3f);
        }

        // Adaptive
        if (victim.myPassiveManager.tenacious && totalLifeLost > 0)
        {
            Debug.Log("Adaptive triggered, gaining block");
            victim.ModifyCurrentBlock(victim.myPassiveManager.tenaciousStacks);
            yield return new WaitForSeconds(0.3f);
        }        

        // Thorns
        if (victim.myPassiveManager.thorns)
        {  
            if(attackType == AbilityDataSO.AttackType.Melee)
            {
                Debug.Log("Victim has thorns and was struck by a melee attack, returning damage...");
                Action thornsDamage = HandleDamage(CalculateDamage(victim.myPassiveManager.thornsStacks, attacker, victim, AbilityDataSO.DamageType.Physical, false), victim, attacker);                
            }                      
        }

        // Lightning Shield
        if (victim.myPassiveManager.lightningShield && attackType != AbilityDataSO.AttackType.None)
        {
            Debug.Log("Victim has lightning shield and was attacked, returning damage...");
            Action lightningShieldDamage = HandleDamage(CalculateDamage(victim.myPassiveManager.lightningShieldStacks, victim, attacker, AbilityDataSO.DamageType.Magic, false), attacker, victim);
            yield return new WaitUntil(() => lightningShieldDamage.ActionResolved() == true);
        }

        // Phasing
        if (victim.timesAttackedThisTurnCycle == 0 &&
            victim.myPassiveManager.phasing && 
            attackType != AbilityDataSO.AttackType.None)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "Phasing", true, "Blue"));
            Action reflexAction = victim.StartQuickReflexesMove();
            yield return new WaitUntil(() => reflexAction.ActionResolved() == true);
        }

        victim.timesAttackedThisTurnCycle++;

        // Check if the victim was killed by the damage
        if (victim.currentHealth <= 0 && victim.inDeathProcess == false)
        {
            // the victim was killed, start death process
            victim.inDeathProcess = true;
            victim.StopAllCoroutines();
            StartCoroutine(victim.HandleDeath());
        }

        else if(victim.currentHealth > 0 && totalLifeLost > 0)
        {
            // the victim wasn't killed, play 'hurt' animation
            // victim.myAnimator.enabled = true;
            victim.myAnimator.SetTrigger("Hurt");
        }

        // Send 'actiom resolved' message back up the stack
        action.actionResolved = true;
    }
    public int CalculateDamage(int abilityBaseDamage, LivingEntity victim, LivingEntity attacker, AbilityDataSO.DamageType damageType, bool critical, AbilityDataSO.AttackType attackType = AbilityDataSO.AttackType.None, Ability abilityUsed = null)
    {
        int newDamageValue = 0;

        // Establish base ability damage value
        newDamageValue += abilityBaseDamage;
        Debug.Log("Base damage value: " + newDamageValue);

        // add bonus strength
        if(damageType == AbilityDataSO.DamageType.Physical && attackType != AbilityDataSO.AttackType.None)
        {
            newDamageValue += attacker.currentStrength;
        }        
        Debug.Log("Damage value after strength added: " + newDamageValue);

        // add bonus wisdom
        if (damageType == AbilityDataSO.DamageType.Magic && attackType != AbilityDataSO.AttackType.None)
        {
            newDamageValue += attacker.currentWisdom;
        }
        Debug.Log("Damage value after wisdom added: " + newDamageValue);

        // multiply/divide the damage value based on factors like vulnerable, knock down, magic vulnerability, etc
        newDamageValue = (int)(newDamageValue * CalculateAndGetDamagePercentageModifier(attacker, victim, damageType, critical, attackType));
        Debug.Log("Damage value after percentage modifers like knockdown added: " + newDamageValue);

        return newDamageValue;
    }
    public float CalculateAndGetDamagePercentageModifier(LivingEntity attacker, LivingEntity victim, AbilityDataSO.DamageType damageType, bool critical, AbilityDataSO.AttackType attackType = AbilityDataSO.AttackType.None)
    {
        // Get damage type first
        AbilityDataSO.DamageType DamageType = damageType;

        float damageModifier = 1f;
        
        // exposed
        if (victim.myPassiveManager.vulnerable && attackType != AbilityDataSO.AttackType.None && DamageType != AbilityDataSO.DamageType.None)
        {
            Debug.Log("Victim exposed, increasing damage by 50%...");
            damageModifier += 0.5f;
        }

        // check for critical
        if(attackType != AbilityDataSO.AttackType.None)
        {
            if (RollForCritical(attacker, null))
            {
                damageModifier += 0.5f;
            }
        }

        // exhausted
        if (attacker.myPassiveManager.weakened)
        {
            damageModifier -= 0.5f;
            Debug.Log("Attacker exhausted, decreasing damage by 50%...");
        }

        // back arc
        if(PositionLogic.Instance.CanAttackerBackStrikeTarget(attacker, victim) && attackType == AbilityDataSO.AttackType.Melee)
        {
            //damageModifier += 1f;
            //Debug.Log("Attacker striking victims back arc, increasing damage by 100%...");
        }
        
        // prevent modifier from going negative
        if(damageModifier < 0)
        {
            damageModifier = 0;
        }

        return damageModifier;

    }
    #endregion

    // Conditional checks + booleans
    #region
    public bool IsTargetFriendly(LivingEntity caster, LivingEntity target)
    {
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

    // Damage and Combat Calculators
    public int GetBaseDamageValue(LivingEntity entity, int weaponBaseDamage, Ability abilityUsed, string attackDamageType, ItemDataSO weaponUsed = null)
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
            baseDamageValueReturned += weaponBaseDamage;
        }        

        // Add flat damage bonus from modifiers (strenght, etc)
        if(abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            baseDamageValueReturned += entity.currentStrength;
            Debug.Log("Base damage after strength and related modifiers added: " + baseDamageValueReturned.ToString());
        }

        // Add damage from damage type modifiers that effect spell damage (wisdom, shadowform etc)
        if (attackDamageType == "Fire" || attackDamageType == "Shadow" || attackDamageType == "Air" || attackDamageType == "Frost" || attackDamageType == "Poison")
        {
            baseDamageValueReturned += entity.currentWisdom;
            Debug.Log("Base damage after wisdom added: " + baseDamageValueReturned.ToString());
        }

        // multiply by ability damage multiplier
        baseDamageValueReturned = (int)(baseDamageValueReturned * abilityUsed.weaponDamagePercentage);
        Debug.Log("Base damage ability percentage modifer " + baseDamageValueReturned.ToString());

        // return
        return baseDamageValueReturned;

    }
    public int GetDamageValueAfterResistances(int damageValue, string attackDamageType, LivingEntity target)
    {
        Debug.Log("CombatLogic.GetDamageValueAfterResistances() called...");
        Debug.Log("Damage Type detected: " + attackDamageType);

        int damageValueReturned = damageValue;
        int targetResistance = 0;
        float resistanceMultiplier = 0;

        // get the targets resistance value
        if (attackDamageType == "Physical")
        {
            targetResistance = target.currentPhysicalResistance;
        }
        else if (attackDamageType == "Fire")
        {
            targetResistance = target.currentFireResistance;
        }
        else if (attackDamageType == "Air")
        {
            targetResistance = target.currentAirResistance;
        }
        else if (attackDamageType == "Poison")
        {
            targetResistance = target.currentPoisonResistance;
        }
        else if (attackDamageType == "Shadow")
        {
            targetResistance = target.currentShadowResistance;
        }
        else if (attackDamageType == "Frost")
        {
            targetResistance = target.currentFrostResistance;
        }

        Debug.Log("Target has " + targetResistance + " " + attackDamageType + " Resistance...");

        // invert the resistance value from 100. (as in, 80% fire resistance means the attack will deal 20% of it original damage
        int invertedResistanceValue = 100 - targetResistance;
        Debug.Log("Resitance value after inversion: " + invertedResistanceValue.ToString());

        // convert target resistance to float to multiply against base damage value
        resistanceMultiplier = invertedResistanceValue / 100;
        Debug.Log("Resitance multiplier as float value: " + resistanceMultiplier.ToString());

        // apply final resistance calculations to the value returned
        damageValueReturned = (int)(damageValueReturned * resistanceMultiplier);
        Debug.Log("Final damage value calculated: " + damageValueReturned.ToString());

        return damageValueReturned;
    }
    public int GetDamageValueAfterNonResistanceModifiers(int damageValue, LivingEntity attacker, LivingEntity target, Ability abilityUsed, string damageType, bool critical)
    {
        Debug.Log("CombatLogic.GetDamageValueAfterNonResistanceModifiers() called...");

        int damageValueReturned = damageValue;
        float damageModifier = 1f;

        // vulnerable
        if (target.myPassiveManager.vulnerable)
        {
            damageModifier += 0.5f;
        }

        // weakened
        if (attacker.myPassiveManager.weakened)
        {
            damageModifier -= 0.5f;
        }

        // critical
        if (critical)
        {
            damageModifier += 0.5f;
        }

        // back strike bonuses
        if (PositionLogic.Instance.CanAttackerBackStrikeTarget(attacker, target) && abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            // TO DO: update this when we implement opportunist passive
        }

        // TO DO: percentage bonus modifiers to spell damage should go here (e.g. Shadow Form, Demon, Increase all air damage by X%, etc)

        // prevent modifier from going negative
        if (damageModifier < 0)
        {
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

        // calculate damage after standard modifiers
        finalDamageValueReturned = GetDamageValueAfterNonResistanceModifiers(finalDamageValueReturned, attacker, target, abilityUsed, damageType, critical);

        // calcualte damage value after resistances
        finalDamageValueReturned = GetDamageValueAfterResistances(finalDamageValueReturned, damageType, target);

        // return final value
        return finalDamageValueReturned;

    }
    public string CalculateFinalDamageTypeOfAttack(LivingEntity entity, Ability abilityUsed, ItemDataSO itemUsed = null)
    {
        Debug.Log("CombatLogic.CalculateFinalDamageTypeOfAttack() called...");
        // preferences
        string damageTypeReturned = "None";

        // draw damage type from ability
        damageTypeReturned = AbilityLogic.Instance.GetDamageTypeFromAbility(abilityUsed);

        // if ability uses weapon, get damage type from weapon
        if (itemUsed != null && (abilityUsed.myAbilityData.requiresMeleeWeapon || abilityUsed.myAbilityData.requiresRangedWeapon))
        {
            damageTypeReturned = ItemManager.Instance.GetDamageTypeFromWeapon(itemUsed);
            Debug.Log("Damage type from weapon (" + itemUsed.Name + ") is: " + damageTypeReturned);
        }


        // after all this, try draw from passive traits that effect damage type output
        // override this damage type if the character has a temporary damage type buff

        // to do: once more passive traits about damage typing have been implemented, add the logic above




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
    public int CalculateCriticalStrikeChance(LivingEntity character, Ability ability)
    {
        Debug.Log("CombatLogic.CalculateCriticalChance() called...");
        // TO DO: when more passive traits are added that effect crit chance (ambusher, predator, etc), update this method
        int critChanceReturned = 0;

        // Add base crit chance
        critChanceReturned += character.currentCriticalChance;

        // Cap Crit Chance at 80%
        if(critChanceReturned > 80)
        {
            critChanceReturned = 80;
        }

        // Check for sharpen blade
        if (character.myPassiveManager.sharpenedBlade && ability.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            critChanceReturned = 100;
        }

        return critChanceReturned;
    }
    public int CalculateParryChance(LivingEntity target)
    {
        Debug.Log("CombatLogic.CalculateParryChance() called...");
        int parryChanceReturned = 0;

        // Add base parry chance
        parryChanceReturned += target.currentParryChance;

        // add bonuses from passives
        parryChanceReturned += target.myPassiveManager.temporaryBonusParryStacks;

        // Cap Parry Chance at 80%
        if (parryChanceReturned > 80)
        {
            parryChanceReturned = 80;
        }

        return parryChanceReturned;
    }
    public int CalculateDodgeChance(LivingEntity target)
    {
        Debug.Log("CombatLogic.CalculateDodgeChance() called...");
        int dodgeChanceReturned = 0;

        // Get base dodge chance
        dodgeChanceReturned += target.currentDodgeChance;

        // add bonuses from passives
        dodgeChanceReturned += target.myPassiveManager.temporaryBonusDodgeStacks;

        // Cap Parry Chance at 80%
        if (dodgeChanceReturned > 80)
        {
            dodgeChanceReturned = 80;
        }

        return dodgeChanceReturned;
    }
    public int CalculateBlockGainedByEffect(int baseBlockGain, LivingEntity caster)
    {
        int valueReturned = baseBlockGain;

        valueReturned += caster.currentDexterity;

        return valueReturned;
    }

    // Roll for Crit, Parry and Dodge
    public bool RollForCritical(LivingEntity attacker, Ability ability)
    {
        Debug.Log("CombatLogic.RollForCritical() called...");
        int critChance = CalculateCriticalStrikeChance(attacker, ability);

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
    public bool RollForParry(LivingEntity target)
    {
        Debug.Log("CombatLogic.RollForParry() called...");
        int parryChance = CalculateParryChance(target);

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
    public bool RollForDodge(LivingEntity target)
    {
        Debug.Log("CombatLogic.RollForDodge() called...");
        int dodgeChance = CalculateDodgeChance(target);

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

    // Attack + Ability Specific Events
    public Action NewHandleDamage(int damageAmount, LivingEntity attacker, LivingEntity victim, string damageType, Ability abilityUsed = null, bool ignoreBlock = false)
    {
        Debug.Log("CombatLogic.NewHandleDamage() called...");
        Action action = new Action();
        StartCoroutine(NewHandleDamageCoroutine(damageAmount, attacker, victim, damageType, action, abilityUsed, ignoreBlock));
        return action;
    }
    private IEnumerator NewHandleDamageCoroutine(int damageAmount, LivingEntity attacker, LivingEntity victim, string damageType, Action action, Ability abilityUsed = null, bool ignoreBlock = false)
    {
        // Establish properties for this damage event
        int totalLifeLost = 0;
        int adjustedDamageValue = damageAmount;
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

        // Check for block
        if (victim.currentBlock == 0)
        {
            healthAfter = victim.currentHealth - adjustedDamageValue;
            blockAfter = 0;
        }

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

            else if (ignoreBlock)
            {
                blockAfter = victim.currentBlock;
                Debug.Log("block after = " + blockAfter);
                healthAfter = victim.currentHealth - adjustedDamageValue;
            }

        }

        // Check for barrier
        if (victim.myPassiveManager.barrier && healthAfter < victim.currentHealth)
        {
            adjustedDamageValue = 0;
            healthAfter = victim.currentHealth;
            victim.myPassiveManager.ModifyBarrier(-1);
            yield return new WaitForSeconds(0.3f);
        }        

        // Finished calculating the final damage, health lost and armor lost: p
        totalLifeLost = victim.currentHealth - healthAfter;
        victim.ModifyCurrentHealth(-totalLifeLost);
        victim.SetCurrentBlock(blockAfter);

        // Play VFX depending on whether the victim lost health, block, or was damaged by poison
        if (adjustedDamageValue > 0)
        {
            if (totalLifeLost == 0)
            {
                // Create Lose Armor Effect
                StartCoroutine(VisualEffectManager.Instance.CreateLoseBlockEffect(victim.transform.position, adjustedDamageValue));
            }
            else if (totalLifeLost > 0)
            {
                // Create Lose hp / damage effect
                StartCoroutine(VisualEffectManager.Instance.CreateDamageEffect(victim.transform.position, totalLifeLost));
            }
        }

        // Update character data if victim is a defender
        if (victim.defender != null && totalLifeLost > 0)
        {
            victim.defender.myCharacterData.ModifyCurrentHealth(-totalLifeLost);
        }

        // Remove camoflage from victim if damage was taken
        if (victim.myPassiveManager.camoflage)
        {
            victim.myPassiveManager.ModifyCamoflage(-1);
            yield return new WaitForSeconds(0.3f);
        }

        // Remove Sleeping
        if (victim.myPassiveManager.sleep && healthAfter < victim.currentHealth)
        {
            victim.myPassiveManager.ModifySleep(-victim.myPassiveManager.sleepStacks);
            yield return new WaitForSeconds(0.3f);
        }

        // Life steal
        if (attacker.myPassiveManager.lifeSteal && totalLifeLost > 0 &&
            abilityUsed != null &&
            abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            attacker.ModifyCurrentHealth(totalLifeLost);
        }

        // Poisonous trait
        if (attacker.myPassiveManager.poisonous && totalLifeLost > 0 &&
            abilityUsed != null &&
            (abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack ||
            abilityUsed.abilityType == AbilityDataSO.AbilityType.RangedAttack))
        {
            victim.myPassiveManager.ModifyPoisoned(attacker.myPassiveManager.poisonousStacks, attacker);
            yield return new WaitForSeconds(0.3f);
        }

        // Remove sleeping
        if (victim.myPassiveManager.sleep && totalLifeLost > 0)
        {
            Debug.Log("Damage taken, removing sleep");
            victim.myPassiveManager.ModifySleep(-victim.myPassiveManager.sleepStacks);
            yield return new WaitForSeconds(0.3f);
        }

        // Enrage
        if (victim.myPassiveManager.enrage && totalLifeLost > 0)
        {
            Debug.Log("Enrage triggered, gaining strength");
            victim.ModifyCurrentStrength(victim.myPassiveManager.enrageStacks);
            yield return new WaitForSeconds(0.3f);
        }

        // Tenacious
        if (victim.myPassiveManager.tenacious && totalLifeLost > 0)
        {
            Debug.Log("Tenacious triggered, gaining block");
            victim.ModifyCurrentBlock(victim.myPassiveManager.tenaciousStacks);
            yield return new WaitForSeconds(0.3f);
        }

        // Thorns
        if (victim.myPassiveManager.thorns)
        {
            if (abilityUsed != null &&
                abilityUsed.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
            {
                Debug.Log("Victim has thorns and was struck by a melee attack, returning damage...");
                Action thornsDamage = HandleDamage(CalculateDamage(victim.myPassiveManager.thornsStacks, attacker, victim, AbilityDataSO.DamageType.Physical, false), victim, attacker);
            }
        }

        // Quick Reflexes
        if (victim.timesAttackedThisTurnCycle == 0 &&
            victim.myPassiveManager.phasing &&
            abilityUsed != null &&
            abilityUsed.abilityType != AbilityDataSO.AbilityType.None)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "Phasing", true, "Blue"));
            Action reflexAction = victim.StartQuickReflexesMove();
            yield return new WaitUntil(() => reflexAction.ActionResolved() == true);
        }

        victim.timesAttackedThisTurnCycle++;

        // Check if the victim was killed by the damage
        if (victim.currentHealth <= 0 && victim.inDeathProcess == false)
        {
            // the victim was killed, start death process
            victim.inDeathProcess = true;
            victim.StopAllCoroutines();
            StartCoroutine(victim.HandleDeath());
        }

        // if not dead but hurt, play hurt animation
        else if (victim.currentHealth > 0 && totalLifeLost > 0)
        {
            victim.myAnimator.SetTrigger("Hurt");
        }

        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;
    }

    public Action HandleParry(LivingEntity attacker, LivingEntity target)
    {
        Debug.Log("CombatLogic.HandleParry() called...");
        Action action = new Action();
        StartCoroutine(HandleParryCoroutine(attacker, target, action));
        return action;
    }
    private IEnumerator HandleParryCoroutine(LivingEntity attacker, LivingEntity target, Action action)
    {                
        if (target.myPassiveManager.riposte)
        {
            Debug.Log(target.name + " parried an attack AND has riposte, performing riposte attack...");
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Riposte!", true, "Yellow"));
            // perform riposte attack
        }
        else
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Parry!", true, "Yellow"));
        }

        yield return new WaitForSeconds(0.5f);

        action.actionResolved = true;
    }
    public Action HandleDodge(LivingEntity attacker, LivingEntity target)
    {
        Debug.Log("CombatLogic.HandleParry() called...");
        Action action = new Action();
        StartCoroutine(HandleDodgeCoroutine(attacker, target, action));
        return action;
    }
    private IEnumerator HandleDodgeCoroutine(LivingEntity attacker, LivingEntity target, Action action)
    {
        StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Dodge!", true, "Yellow"));

        yield return new WaitForSeconds(0.5f);

        action.actionResolved = true;
    }

    // Aoe calculators
    public List<LivingEntity> GetAllLivingEntitiesWithinAoeEffect(LivingEntity caster, Tile aoeCentrePoint, int blastRadius, bool friendlyFire, bool removeCentrePoint)
    {
        List<LivingEntity> targetsInAoeEffect = new List<LivingEntity>();
        List<Tile> tilesInBlastRadius = LevelManager.Instance.GetTilesWithinRange(blastRadius, aoeCentrePoint, removeCentrePoint);

        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (tilesInBlastRadius.Contains(entity.tile))
            {
                if (CombatLogic.Instance.IsTargetFriendly(caster, entity) == false)
                {
                    targetsInAoeEffect.Add(entity);
                }
                else if (CombatLogic.Instance.IsTargetFriendly(caster, entity) && friendlyFire)
                {
                    targetsInAoeEffect.Add(entity);
                }
            }
        }

        return targetsInAoeEffect;
    }
}
