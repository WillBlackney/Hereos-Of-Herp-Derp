    using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            HandleDamage(damage, attacker, livingEntity,true);
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
            adjustedDamageValue = CalculateDamage(damageAmount, victim, attacker, damageType, attackType, abilityUsed);
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

        if(victim.myPassiveManager.sleeping && healthAfter < victim.currentHealth)
        {
            victim.myPassiveManager.ModifySleeping(-victim.myPassiveManager.sleepingStacks);
            yield return new WaitForSeconds(0.3f);
        }

        // Finished calculating the final damage, health lost and armor lost: p
        totalLifeLost = victim.currentHealth - healthAfter;
        //victim.currentHealth = healthAfter;
        victim.ModifyCurrentHealth(-totalLifeLost);
        victim.SetCurrentBlock(blockAfter);
        //victim.UpdateHealthGUIElements();

        // Play VFX depending on whether the victim lost health, block, or was damaged by poison
        if (adjustedDamageValue > 0)
        {                 
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

        // Check the victim's and attacker's passive traits that are related to taking damage 

        // Life steal
        if (attacker.myPassiveManager.lifeSteal && totalLifeLost > 0)
        {
            attacker.ModifyCurrentHealth(totalLifeLost);
        }

        // Poisonous trait
        if (attacker.myPassiveManager.poisonous && totalLifeLost > 0 && attackType == AbilityDataSO.AttackType.Melee)
        {           
            victim.myPassiveManager.ModifyPoison(attacker.myPassiveManager.poisonousStacks, attacker);
            yield return new WaitForSeconds(0.3f);
        }

        // Remove sleeping
        if (victim.myPassiveManager.sleeping && totalLifeLost > 0)
        {
            Debug.Log("Damage taken, removing sleep");
            victim.myPassiveManager.ModifySleeping(-victim.myPassiveManager.sleepingStacks);
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
        if (victim.myPassiveManager.adaptive && totalLifeLost > 0)
        {
            Debug.Log("Adaptive triggered, gaining block");
            victim.ModifyCurrentBlock(victim.myPassiveManager.adaptiveStacks);
            yield return new WaitForSeconds(0.3f);
        }        

        // Thorns
        if (victim.myPassiveManager.thorns)
        {  
            if(attackType == AbilityDataSO.AttackType.Melee)
            {
                Debug.Log("Victim has thorns and was struck by a melee attack, returning damage...");
                Action thornsDamage = HandleDamage(CalculateDamage(victim.myPassiveManager.thornsStacks, attacker, victim, AbilityDataSO.DamageType.Physical), victim, attacker);                
            }                      
        }

        // Lightning Shield
        if (victim.myPassiveManager.lightningShield && attackType != AbilityDataSO.AttackType.None)
        {
            Debug.Log("Victim has lightning shield and was attacked, returning damage...");
            Action lightningShieldDamage = HandleDamage(CalculateDamage(victim.myPassiveManager.lightningShieldStacks, victim, attacker, AbilityDataSO.DamageType.Magic), attacker, victim);
            yield return new WaitUntil(() => lightningShieldDamage.ActionResolved() == true);
        }

        // Quick Reflexes
        if (victim.timesAttackedThisTurnCycle == 0 &&
            victim.myPassiveManager.quickReflexes && 
            attackType != AbilityDataSO.AttackType.None)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "Quick Reflexes", true, "Blue"));
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
    public int CalculateDamage(int abilityBaseDamage, LivingEntity victim, LivingEntity attacker, AbilityDataSO.DamageType damageType, AbilityDataSO.AttackType attackType = AbilityDataSO.AttackType.None, Ability abilityUsed = null)
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

        // Add ignite damage bonus if fireball used
        if (abilityUsed != null && abilityUsed.abilityName == "Fire Ball" && victim.myPassiveManager.ignite)
        {
            newDamageValue += victim.myPassiveManager.igniteStacks;
        }
        Debug.Log("Damage value after ignite added: " + newDamageValue);

        // multiply/divide the damage value based on factors like vulnerable, knock down, magic vulnerability, etc
        newDamageValue = (int)(newDamageValue * CalculateAndGetDamagePercentageModifier(attacker, victim, damageType, attackType));
        Debug.Log("Damage value after percentage modifers like knockdown added: " + newDamageValue);

        return newDamageValue;
    }
    public float CalculateAndGetDamagePercentageModifier(LivingEntity attacker, LivingEntity victim, AbilityDataSO.DamageType damageType, AbilityDataSO.AttackType attackType = AbilityDataSO.AttackType.None)
    {
        // Get damage type first
        AbilityDataSO.DamageType DamageType = damageType;

        // TO DO in future: this is where we modify the damage type based on character traits 
        // (e.g. if a character has a buff that makes all it damage types magical

        float damageModifier = 1f;
        
        if (victim.myPassiveManager.exposed && attackType != AbilityDataSO.AttackType.None && DamageType != AbilityDataSO.DamageType.None)
        {
            Debug.Log("Victim exposed, increasing damage by 50%...");
            damageModifier += 0.5f;
        }
        if (attacker.myPassiveManager.exhausted)
        {
            damageModifier -= 0.5f;
            Debug.Log("Attacker exhausted, decreasing damage by 50%...");
        }
        if(PositionLogic.Instance.IsWithinTargetsBackArc(attacker, victim) && attackType == AbilityDataSO.AttackType.Melee)
        {
            //damageModifier += 1f;
            //Debug.Log("Attacker striking victims back arc, increasing damage by 100%...");
        }
        
        if (victim.myPassiveManager.magicImmunity && DamageType == AbilityDataSO.DamageType.Magic)
        {
            damageModifier = 0;
            Debug.Log("Victim has Magic immunity, damage reduced to 0%...");
        }
        if (victim.myPassiveManager.physicalImmunity && DamageType == AbilityDataSO.DamageType.Physical)
        {
            damageModifier = 0;
            Debug.Log("Victim has Physical immunity, damage reduced to 0%...");
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


}
