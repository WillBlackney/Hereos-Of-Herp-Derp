using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLogic : MonoBehaviour
{
    // Initialization + Singleton Pattern 
    #region
    public static AbilityLogic Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Misc Logic
    #region
    public void OnAbilityUsedStart(Ability ability, LivingEntity entity)
    {
        Debug.Log("OnAbilityUsedStart() called for " + entity.gameObject.name + " using " + ability.abilityName);

        // Disable tile hover + tile highlights
        if (entity.defender)
        {
            entity.defender.awaitingAnOrder = false;
        }

        TileHover.Instance.SetVisibility(false);
        LevelManager.Instance.UnhighlightAllTiles();
        PathRenderer.Instance.DeactivatePathRenderer();
        InstructionHover.Instance.DisableInstructionHover();

        // temp variables
        int finalCD = ability.abilityBaseCooldownTime;
        int finalEnergyCost = CalculateAbilityEnergyCost(ability, entity);

        // Modify AP
        entity.ModifyCurrentEnergy(-finalEnergyCost);

        // Modify Cooldown
        ability.ModifyCurrentCooldown(finalCD);

        // if character has a free move available from flux
        if (entity.moveActionsTakenThisActivation == 0 &&
            entity.myPassiveManager.flux &&
            ability.abilityName == "Move")
        {
            VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Flux");
        }

        // increment move actions taken if ability used is 'Move
        if (ability.abilityName == "Move")
        {
            entity.moveActionsTakenThisActivation++;
        }

        // increment melee actions taken if ability used is a melee ability
        if (ability.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            entity.meleeAbilityActionsTakenThisActivation++;
        }

        // increment melee actions taken if ability used is a melee ability
        if (ability.abilityType == AbilityDataSO.AbilityType.Skill)
        {
            entity.skillAbilityActionsTakenThisActivation++;
        }

        // increment melee actions taken if ability used is a melee ability
        if (ability.abilityType == AbilityDataSO.AbilityType.RangedAttack)
        {
            entity.rangedAttackAbilityActionsTakenThisActivation++;
        }

        // If ability is power, add to character power list
        if (ability.abilityType == AbilityDataSO.AbilityType.Power)
        {
            AddPowerToEntity(entity, ability);
        }


    }
    public void OnAbilityUsedFinish(Ability ability, LivingEntity entity)
    {
        Debug.Log("OnAbilityUsedFinish() called for " + entity.gameObject.name + " using " + ability.abilityName);

        // remove camoflage
        if (entity.myPassiveManager.camoflage)
        {
            if (ability.abilityName != "Move" &&
                ability.abilityName != "Vanish" &&
                ability.abilityName != "Shroud" &&
                ability.abilityName != "Concealing Clouds" &&
                ability.abilityName != "Shadow Step")
            {
                entity.myPassiveManager.ModifyCamoflage(-1);
            }
        }

        // remove sharpen blades
        if (entity.myPassiveManager.sharpenedBlade && ability.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            entity.myPassiveManager.ModifySharpenedBlade(-entity.myPassiveManager.sharpenedBladeStacks);
        }
    }
    public bool DoesAbilityMeetWeaponRequirements(LivingEntity entity, Ability ability)
    {
        Debug.Log("DoesAbilityMeetWeaponRequirements called for " + entity.name + " using " + ability.abilityName);

        bool boolReturned = false;

        if (ability.requiresMeleeWeapon == false &&
           ability.requiresRangedWeapon == false &&
           ability.requiresShield == false)
        {
            Debug.Log(ability.abilityName + " does not require a melee weapon, ranged weapon or shield...");
            boolReturned = true;
        }
        else if (ability.requiresMeleeWeapon == true &&
            (entity.myMainHandWeapon.itemType == ItemDataSO.ItemType.MeleeOneHand ||
            entity.myMainHandWeapon.itemType == ItemDataSO.ItemType.MeleeTwoHand)
            )
        {
            Debug.Log(ability.abilityName + " requires a melee weapon, and " + entity.name + " has one...");
            boolReturned = true;
        }
        else if (ability.requiresRangedWeapon &&
            entity.myMainHandWeapon.itemType == ItemDataSO.ItemType.RangedTwoHand)
        {
            Debug.Log(ability.abilityName + " requires a ranged weapon, and " + entity.name + " has one...");
            boolReturned = true;
        }
        else if (ability.requiresShield &&
            entity.myOffHandWeapon.itemType == ItemDataSO.ItemType.Shield)
        {
            Debug.Log(ability.abilityName + " requires a shield, and " + entity.name + " has one...");
            boolReturned = true;
        }
        else
        {
            Debug.Log(entity.name + " does not meet the weapon requirments of " + ability.abilityName);
            boolReturned = false;
        }

        return boolReturned;
    }
    public string GetDamageTypeFromAbility(Ability ability)
    {
        Debug.Log("GetDamageTypeFromAbility() called on ability " + ability.abilityName + "...");

        string damageTypeStringReturned = "None";

        if (ability.abilityDamageType == AbilityDataSO.DamageType.Physical)
        {
            damageTypeStringReturned = "Physical";
        }
        else if (ability.abilityDamageType == AbilityDataSO.DamageType.Fire)
        {
            damageTypeStringReturned = "Fire";
        }
        else if (ability.abilityDamageType == AbilityDataSO.DamageType.Frost)
        {
            damageTypeStringReturned = "Frost";
        }
        else if (ability.abilityDamageType == AbilityDataSO.DamageType.Shadow)
        {
            damageTypeStringReturned = "Shadow";
        }
        else if (ability.abilityDamageType == AbilityDataSO.DamageType.Poison)
        {
            damageTypeStringReturned = "Poison";
        }
        else if (ability.abilityDamageType == AbilityDataSO.DamageType.Air)
        {
            damageTypeStringReturned = "Air";
        }

        Debug.Log("GetDamageTypeFromAbility() calculated that " + ability.abilityName + " has a damage type of " + damageTypeStringReturned);
        return damageTypeStringReturned;
    }
    public int CalculateAbilityEnergyCost(Ability ability, LivingEntity entity)
    {
        Debug.Log("AbilityLogic.CalculateAbilityEnergyCost() called for " + entity.myName +
            " using ability " + ability.abilityName);

        // Initialize at base energy cost
        int finalApCost = ability.abilityEnergyCost;

        // Check for 'Well Drilled' State
        if (entity.defender &&
           StateManager.Instance.DoesPlayerAlreadyHaveState("Well Drilled") &&
           (ability.name == "Strike" || ability.name == "Twin Strike" || ability.name == "Move" || ability.name == "Defend" || ability.name == "Shoot")
           )
        {
            if (finalApCost > 5)
            {
                finalApCost -= 5;
            }

            // dont let ability cost less then 5
            if (finalApCost < 5)
            {
                finalApCost = 5;
            }
        }

        // Check for Savage
        if (ability.abilityType == AbilityDataSO.AbilityType.MeleeAttack &&
            entity.myPassiveManager.savage)
        {
            finalApCost -= 5;

            // dont reduce ability cost below 5 energy
            if (finalApCost < 5)
            {
                finalApCost = 5;
            }
        }

        // Check for Knowledgeable
        if (ability.abilityType == AbilityDataSO.AbilityType.Skill &&
            entity.myPassiveManager.knowledgeable)
        {
            finalApCost -= 5;

            // dont reduce ability cost below 5 energy
            if (finalApCost < 5)
            {
                finalApCost = 5;
            }
        }

        // Check for Pragmatic
        if (ability.abilityType == AbilityDataSO.AbilityType.RangedAttack &&
            entity.myPassiveManager.pragmatic)
        {
            finalApCost -= 5;

            // dont reduce ability cost below 5 energy
            if (finalApCost < 5)
            {
                finalApCost = 5;
            }
        }

        // Check for Fury
        if (ability.abilityType == AbilityDataSO.AbilityType.MeleeAttack &&
            entity.meleeAbilityActionsTakenThisActivation == 0 &&
            entity.myPassiveManager.fury)
        {
            finalApCost = 0;
        }

        // Check for Grace
        if (ability.abilityType == AbilityDataSO.AbilityType.Skill &&
            entity.skillAbilityActionsTakenThisActivation == 0 &&
            entity.myPassiveManager.grace)
        {
            finalApCost = 0;
        }

        // Check for Pragmatic
        if (ability.abilityType == AbilityDataSO.AbilityType.RangedAttack &&
            entity.rangedAttackAbilityActionsTakenThisActivation == 0 &&
            entity.myPassiveManager.pragmatic)
        {
            finalApCost = 0;
        }

        // Check for Preparation
        if (entity.myPassiveManager.preparation && ability.abilityName != "Preparation" &&
            ability.abilityName != "Slice And Dice" &&
            ability.abilityName != "Super Conductor" &&
            ability.abilityName != "Rapid Fire")
        {
            entity.myPassiveManager.ModifyPreparation(-entity.myPassiveManager.preparationStacks);
            finalApCost = 0;
        }

        // Check for Flux passive
        if (ability.abilityName == "Move")
        {
            // if character has a free move available
            if (entity.moveActionsTakenThisActivation == 0 && entity.myPassiveManager.flux)
            {
                finalApCost = 0;
            }
        }

        // Check 'spend X energy' abilities
        if (ability.abilityName == "Slice And Dice" ||
            ability.abilityName == "Rapid Fire" ||
             ability.abilityName == "Super Conductor")
        {
            finalApCost = entity.currentEnergy;
        }

        // Check 'Expert Marksman' state and Shoot ability
        if(entity.defender &&
            ability.abilityName == "Shoot" &&
            StateManager.Instance.DoesPlayerAlreadyHaveState("Expert Marksman"))
        {
            finalApCost = 10;
        }

        // Check 'Blood Lust' state and Strike/Twin Strike ability
        if (entity.defender &&
            (ability.abilityName == "Strike" || ability.abilityName == "Twin Strike") &&
            StateManager.Instance.DoesPlayerAlreadyHaveState("Blood Lust"))
        {
            finalApCost = 10;
        }

        Debug.Log("AbilityLogic.CalculateAbilityEnergyCost() returning " + finalApCost.ToString() +
            " as the final energy cost of " + ability.abilityName + " used by " + entity.myName);

        return finalApCost;
    }
    public int CalculateAbilityRange(Ability ability, LivingEntity entity)
    {
        Debug.Log("AbilityLogic.CalculateAbilityRange() called for " +
            entity.myName + " using ability " + ability.abilityName);

        int rangeReturned = 0;

        if (ability.abilityType == AbilityDataSO.AbilityType.RangedAttack)
        {
            rangeReturned = EntityLogic.GetTotalRangeOfRangedAttack(entity, ability);
        }
        else if (ability.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            rangeReturned = entity.currentMeleeRange;
        }
        else if (ability.abilityType == AbilityDataSO.AbilityType.Skill)
        {
            rangeReturned = ability.abilityRange;
        }
        else if (ability.abilityType == AbilityDataSO.AbilityType.Power)
        {
            rangeReturned = 0;
        }
        else
        {
            rangeReturned = 0;
        }

        Debug.Log("AbilityLogic.CalculateAbilityRange() final range of " + ability.abilityName +
            " used by " + entity.myName + " is: " + rangeReturned.ToString());

        return rangeReturned;
    }
    #endregion

    // Powers Logic
    #region
    public void AddPowerToEntity(LivingEntity entity, Ability power)
    {
        Debug.Log("AbilityLogic.AddPowerToEntity() called, adding " + power.abilityName + " to " + entity.name);

        /*
        entity.activePowers.Insert(0,power);
        if(entity.activePowers.Count > entity.currentMaxPowersCount)
        {
            Debug.Log(entity.name + " has excedding its power limit, removing " + power.abilityName + " from active powers list...");
            RemovePowerFromEntity(entity, entity.activePowers.Last());
        }
        */

    }
    public void RemovePowerFromEntity(LivingEntity entity, Ability power)
    {
        Debug.Log("AbilityLogic.RemovePowerFromEntity() called, removing " + power.abilityName + " from " + entity.name);
        // Remove power from active powers list
        entity.activePowers.Remove(power);

        // Disable passive effect of power
        if(power.abilityName == "Overload")
        {
            entity.myPassiveManager.ModifyAirImbuement(-1);
        }
        else if (power.abilityName == "Shadow Wreath")
        {
            entity.myPassiveManager.ModifyShadowImbuement(-1);
        }
        else if (power.abilityName == "Purity")
        {
            entity.myPassiveManager.ModifyPurity(-1);
        }
        else if (power.abilityName == "Infuse")
        {
            entity.myPassiveManager.ModifyInfuse(-1);
        }
        else if (power.abilityName == "Concentration")
        {
            entity.myPassiveManager.ModifyConcentration(-1);
        }
        else if (power.abilityName == "Creeping Frost")
        {
            entity.myPassiveManager.ModifyFrostImbuement(-1);
        }
        else if (power.abilityName == "Blaze")
        {
            entity.myPassiveManager.ModifyFireImbuement(-1);
        }
        else if (power.abilityName == "Testudo")
        {
            entity.myPassiveManager.ModifyTestudo(-1);
        }
        else if (power.abilityName == "Rapid Cloaking")
        {
            entity.myPassiveManager.ModifyRapidCloaking(-1);
        }
        else if (power.abilityName == "Recklessness")
        {
            entity.myPassiveManager.ModifyRecklessness(-1);
        }
    }
    #endregion

    // Neutral Abilities
    #region

    // Strike
    public Action PerformStrike(LivingEntity caster, LivingEntity victim)
    {
        Debug.Log("AbilityLogic.PerformStrike() called. Caster = " + caster.name + ", Target = " + victim.name);
        Action action = new Action(true);
        StartCoroutine(PerformStrikeCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformStrikeCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability strike = caster.mySpellBook.GetAbilityByName("Strike");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, strike);
        bool parry = CombatLogic.Instance.RollForParry(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, strike, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, strike, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);
        
        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Strike");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(strike, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, strike);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(strike, caster);
        action.actionResolved = true;

    }

    // Twin Strike
    public Action PerformTwinStrike(LivingEntity caster, LivingEntity victim)
    {
        Debug.Log("AbilityLogic.PerformStrike() called. Caster = " + caster.name + ", Target = " + victim.name);
        Action action = new Action(true);
        StartCoroutine(PerformTwinStrikeCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformTwinStrikeCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability twinStrike = caster.mySpellBook.GetAbilityByName("Twin Strike");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, twinStrike);
        bool parry = CombatLogic.Instance.RollForParry(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, twinStrike, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, twinStrike, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Twin Strike");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(twinStrike, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, twinStrike);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // SECOND ATTACK
        if(victim.inDeathProcess == false)
        {
            bool critical2 = CombatLogic.Instance.RollForCritical(caster, victim, twinStrike);
            bool parry2 = CombatLogic.Instance.RollForParry(victim, caster);
            string damageType2 = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, twinStrike, caster.myOffHandWeapon);
            int finalDamageValue2 = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, twinStrike, damageType2, critical2, caster.myOffHandWeapon.baseDamage, caster.myOffHandWeapon);

            // Play attack animation
            caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));

            // if the target successfully parried, dont do HandleDamage: do parry stuff instead
            if (parry2)
            {
                Action parryAction = CombatLogic.Instance.HandleParry(caster, victim);
                yield return new WaitUntil(() => parryAction.ActionResolved() == true);
            }

            // if the target did not parry, handle damage event normally
            else
            {
                if (critical2)
                {
                    VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
                }
                Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue2, caster, victim, damageType2, twinStrike);
                yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(twinStrike, caster);
        action.actionResolved = true;

    }

    // Defend
    public Action PerformDefend(LivingEntity caster)
    {
        Debug.Log("AbilityLogic.PerformDefend() called. Caster = " + caster.name);
        Action action = new Action(true);
        StartCoroutine(PerformDefendCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformDefendCoroutine(LivingEntity caster, Action action)
    {
        Ability block = caster.mySpellBook.GetAbilityByName("Defend");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Strike");
            yield return new WaitForSeconds(0.5f);
        }

        caster.PlaySkillAnimation();
        OnAbilityUsedStart(block, caster);
        caster.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(block.abilityPrimaryValue, caster));
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }

    // Move
    public Action PerformMove(LivingEntity caster, Tile destination)
    {
        Debug.Log("AbilityLogic.PerformMove() called. Caster = " + caster.name + 
            ", Target Tile = " + destination.GridPosition.X.ToString() + ", " + destination.GridPosition.Y.ToString());
        Action action = new Action(true);
        StartCoroutine(PerformMoveCoroutine(caster, destination, action));
        return action;
    }
    private IEnumerator PerformMoveCoroutine(LivingEntity caster, Tile destination, Action action)
    {
        Ability move = caster.mySpellBook.GetAbilityByName("Move");
        OnAbilityUsedStart(move, caster);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Move");
            yield return new WaitForSeconds(0.5f);
        }

        Action movementAction = MovementLogic.Instance.MoveEntity(caster, destination);
        yield return new WaitUntil(() => movementAction.ActionResolved() == true);
        action.actionResolved = true;
        caster.PlayIdleAnimation();
    }

    // Shoot
    public Action PerformShoot(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformShootCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformShootCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability shoot = caster.mySpellBook.GetAbilityByName("Shoot");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, shoot);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, shoot, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, shoot, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Shoot");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(shoot, caster);

        // Ranged attack anim
        caster.PlayRangedAttackAnimation();        
        yield return new WaitUntil(() => caster.myRangedAttackFinished == true);

        // Play arrow shot VFX
        Action shootAction = VisualEffectManager.Instance.ShootArrow(caster.tile.WorldPosition, victim.tile.WorldPosition, 9);
        yield return new WaitUntil(() => shootAction.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, shoot);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(shoot, caster);
        action.actionResolved = true;

    }

    // Free Strike
    public Action PerformFreeStrike(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformFreeStrikeCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformFreeStrikeCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Make sure character actually knows strike, has a melee weapon, and the target is not in death process
        if((caster.mySpellBook.GetAbilityByName("Strike") != null || caster.mySpellBook.GetAbilityByName("Twin Strike") != null) &&
           victim.inDeathProcess == false &&
           (caster.myMainHandWeapon.itemType == ItemDataSO.ItemType.MeleeOneHand || caster.myMainHandWeapon.itemType == ItemDataSO.ItemType.MeleeTwoHand)
            )
        {
            // Set up properties
            Ability strike = caster.mySpellBook.GetAbilityByName("Strike");
            Ability twinStrike = caster.mySpellBook.GetAbilityByName("Twin Strike");
            Ability abilityUsed = null;
            if (strike)
            {
                abilityUsed = strike;
            }
            else if(twinStrike)
            {
                abilityUsed = twinStrike;
            }

            
            bool critical = CombatLogic.Instance.RollForCritical(caster, victim, abilityUsed);
            //bool parry = CombatLogic.Instance.RollForParry(victim);
            string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, abilityUsed, caster.myMainHandWeapon);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, abilityUsed, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

            // Play attack animation
            caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));

            // Check critical
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }

            // Deal damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, abilityUsed);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Remove camo
            if (caster.myPassiveManager.camoflage)
            {
                caster.myPassiveManager.ModifyCamoflage(-1);
            }

        }

        // Resolve
        action.actionResolved = true;

    }

    // Parry Attack
    public Action PerformRiposteAttack(LivingEntity caster, LivingEntity victim)
    {
        Debug.Log("AbilityLogic.PerformRiposteAttack() called...");
        Action action = new Action(true);
        StartCoroutine(PerformRiposteAttackCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformRiposteAttackCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Make sure character actually knows strike, has a melee weapon, and the target is not in death process
        if ((caster.mySpellBook.GetAbilityByName("Strike") != null || caster.mySpellBook.GetAbilityByName("Twin Strike") != null) &&
            victim.inDeathProcess == false &&
            (caster.myMainHandWeapon.itemType == ItemDataSO.ItemType.MeleeOneHand || caster.myMainHandWeapon.itemType == ItemDataSO.ItemType.MeleeTwoHand) &&
            caster.myPassiveManager.disarmed == false
            )
        {
            Debug.Log(caster.name + " meets the requirments of a riposte attack, starting riposte process...");
           
            // Get correct attack ability, based on weapon
            Ability attackAbility = null;
            Ability strike = caster.mySpellBook.GetAbilityByName("Strike");
            Ability twinStrike = caster.mySpellBook.GetAbilityByName("Twin Strike");

            if (strike)
            {
                attackAbility = strike;
            }
            else if (twinStrike)
            {
                attackAbility = twinStrike;
            }

            if(attackAbility != null)
            {
                // Start
                bool critical = CombatLogic.Instance.RollForCritical(caster, victim, attackAbility);
                string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, attackAbility, caster.myMainHandWeapon);
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, attackAbility, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

                // Play attack animation
                caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));

                // Check critical
                if (critical)
                {
                    VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
                }

                // Deal damage
                Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, attackAbility);
                yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

                // Remove camo
                if (caster.myPassiveManager.camoflage)
                {
                    caster.myPassiveManager.ModifyCamoflage(-1);
                }
                // Remove Sharpened Blade
                if (caster.myPassiveManager.sharpenedBlade)
                {
                    caster.myPassiveManager.ModifySharpenedBlade(-caster.myPassiveManager.sharpenedBladeStacks);
                }

            }
        }            

        // Resolve
        action.actionResolved = true;

    }

    // Over Watch Shot
    public Action PerformOverwatchShot(LivingEntity caster, LivingEntity victim)
    {
        Debug.Log("AbilityLogic.PerformOverwatchShot() called for " + caster.myName + " against " + victim.myName);

        Action action = new Action(true);
        StartCoroutine(PerformOverwatchShotCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformOverwatchShotCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Make sure character actually knows shoot, has a ranged weapon, and the target is not in death process
        if (caster.mySpellBook.GetAbilityByName("Shoot") != null &&
            victim.inDeathProcess == false &&
            caster.myMainHandWeapon.itemType == ItemDataSO.ItemType.RangedTwoHand
            )
        {
            // Set up properties
            Ability shoot = caster.mySpellBook.GetAbilityByName("Shoot");
            bool critical = CombatLogic.Instance.RollForCritical(caster, victim, shoot);
            bool dodge = CombatLogic.Instance.RollForDodge(victim, caster);
            string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, shoot, caster.myMainHandWeapon);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, shoot, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

            // Remove Overwatch
            caster.myPassiveManager.ModifyOverwatch(-caster.myPassiveManager.overwatchStacks);

            // Ranged attack anim
            caster.PlayRangedAttackAnimation();
            yield return new WaitUntil(() => caster.myRangedAttackFinished == true);

            // Play arrow shot VFX
            Action shootAction = VisualEffectManager.Instance.ShootArrow(caster.tile.WorldPosition, victim.tile.WorldPosition, 9);
            yield return new WaitUntil(() => shootAction.ActionResolved() == true);

            // if the target successfully dodged, dont do HandleDamage: do dodge stuff instead
            if (dodge)
            {
                Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, victim);
                yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
            }

            // if the target did not dodge, handle damage event normally
            else
            {
                if (critical)
                {
                    VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
                }
                Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, shoot);
                yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
            }

            // Remove camo
            if (caster.myPassiveManager.camoflage)
            {
                caster.myPassiveManager.ModifyCamoflage(-1);
            }

        }

        // Resolve
        action.actionResolved = true;

    }
    #endregion

    // Brawler Abilities
    #region

    // Whirlwind
    public Action PerformWhirlwind(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformWhirlwindCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformWhirlwindCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability whirlwind = caster.mySpellBook.GetAbilityByName("Whirlwind");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, whirlwind, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Whirlwind");
            yield return new WaitForSeconds(0.5f);
        }

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInRange = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(caster, caster.tile, caster.currentMeleeRange, false, true);

        // Pay energy cost
        OnAbilityUsedStart(whirlwind, caster);

        // Create whirlwind VFX
        StartCoroutine(VisualEffectManager.Instance.CreateAoeMeleeAttackEffect(caster.transform.position));
        caster.TriggerMeleeAttackAnimation();

        // Resolve hits against targets
        foreach(LivingEntity entity in targetsInRange)
        {
            bool critical = CombatLogic.Instance.RollForCritical(caster, entity, whirlwind);
            bool parry = CombatLogic.Instance.RollForParry(entity, caster);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, whirlwind, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

            // if the target successfully parried, dont do HandleDamage: do parry stuff instead
            if (parry)
            {
                Action parryAction = CombatLogic.Instance.HandleParry(caster, entity);
            }

            // if the target did not parry, handle damage event normally
            else
            {
                if (critical)
                {
                    VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
                }

                Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, whirlwind);
                //yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
            }

        }

        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }

    // Devastating Blow
    public Action PerformDevastatingBlow(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformDevastatingBlowCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformDevastatingBlowCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability devastatingBlow = caster.mySpellBook.GetAbilityByName("Devastating Blow");
        bool critical = CombatLogic.Instance.RollForCritical(caster, target, devastatingBlow);
        bool parry = CombatLogic.Instance.RollForParry(target, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, devastatingBlow, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, target, devastatingBlow, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Devastating Blow");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(devastatingBlow, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(target));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, target);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, target, damageType, devastatingBlow);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(devastatingBlow, caster);
        action.actionResolved = true;
    }

    // Smash
    public Action PerformSmash(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformSmashCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformSmashCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability smash = caster.mySpellBook.GetAbilityByName("Smash");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, smash);
        bool parry = CombatLogic.Instance.RollForParry(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, smash, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, smash, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);
        
        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Smash");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(smash, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, smash);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(smash, caster);
        action.actionResolved = true;

    }

    // Charge
    public Action PerformCharge(LivingEntity caster, LivingEntity target, Tile destination)
    {
        Action action = new Action(true);
        StartCoroutine(PerformChargeCoroutine(caster, target, destination, action));
        return action;
    }
    private IEnumerator PerformChargeCoroutine(LivingEntity caster, LivingEntity victim, Tile destination, Action action)
    {        
        // Set up properties
        Ability charge = caster.mySpellBook.GetAbilityByName("Charge");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Charge");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(charge, caster);

        // Charge movement
        Action moveAction = MovementLogic.Instance.MoveEntity(caster, destination, 6);
        yield return new WaitUntil(() => moveAction.ActionResolved() == true);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));

        // Apply vulnerable
        victim.myPassiveManager.ModifyVulnerable(1);
        caster.PlayIdleAnimation();
        action.actionResolved = true;

        // remove camoflage, etc
        OnAbilityUsedFinish(charge, caster);
        action.actionResolved = true;        

    }

    // Recklessness
    public Action PerformRecklessness(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformRecklessnessCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformRecklessnessCoroutine(LivingEntity caster, Action action)
    {
        // Setup 
        Ability recklessness = caster.mySpellBook.GetAbilityByName("Recklessness");
        OnAbilityUsedStart(recklessness, caster);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Charge");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Gain Recklessness
        caster.myPassiveManager.ModifyRecklessness(1);
        yield return new WaitForSeconds(0.5f);

        // Resolve
        OnAbilityUsedFinish(recklessness, caster);
        action.actionResolved = true;
    }

    // Kick To The Balls
    public Action PerformKickToTheBalls(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformKickToTheBallsCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformKickToTheBallsCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability kickToTheBalls = caster.mySpellBook.GetAbilityByName("Kick To The Balls");
        bool parry = CombatLogic.Instance.RollForParry(victim, caster);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Kick To The Balls");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(kickToTheBalls, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, stun the target
        else
        {
            victim.myPassiveManager.ModifyStunned(1);
            yield return new WaitForSeconds(0.5f);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(kickToTheBalls, caster);
        action.actionResolved = true;

    }

    // Go Berserk
    public Action PerformGoBerserk(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformGoBerserkCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformGoBerserkCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability goBerserk = caster.mySpellBook.GetAbilityByName("Go Berserk");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Go Berserk");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost, + etc
        OnAbilityUsedStart(goBerserk, caster);

        // Apply berserk
        target.myPassiveManager.ModifyBerserk(1);
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(goBerserk, caster);
        action.actionResolved = true;

    }

    // Blade Flurry
    public Action PerformBladeFlurry(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformBladeFlurryCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformBladeFlurryCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability bladeFlurry = caster.mySpellBook.GetAbilityByName("Blade Flurry");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, bladeFlurry, caster.myMainHandWeapon);
        List<LivingEntity> targetsInRange = EntityLogic.GetAllEnemiesWithinRange(caster, caster.currentMeleeRange);
        List<LivingEntity> targetsHit = new List<LivingEntity>();

        // get a random target 3 times
        for (int i = 0; i < bladeFlurry.abilityPrimaryValue; i++)
        {
            targetsHit.Add(targetsInRange[Random.Range(0, targetsInRange.Count)]);
        }

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Blade Flurry");
            yield return new WaitForSeconds(0.5f);
        }


        // Pay energy cost
        OnAbilityUsedStart(bladeFlurry, caster);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsHit)
        {
            if(entity.inDeathProcess == false)
            {
                bool critical = CombatLogic.Instance.RollForCritical(caster, entity, bladeFlurry);
                bool parry = CombatLogic.Instance.RollForParry(entity, caster);
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, bladeFlurry, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

                // Play attack animation
                caster.StartCoroutine(caster.PlayMeleeAttackAnimation(entity));

                // if the target successfully parried, dont do HandleDamage: do parry stuff instead
                if (parry)
                {
                    Action parryAction = CombatLogic.Instance.HandleParry(caster, entity);
                    yield return new WaitUntil(() => parryAction.ActionResolved() == true);
                }

                // if the target did not parry, handle damage event normally
                else
                {
                    if (critical)
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
                    }
                    Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, bladeFlurry);
                    yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
                }

            }
            
        }

        // Remove camo + etc
        OnAbilityUsedFinish(bladeFlurry, caster);

        // Resolve/Complete event
        action.actionResolved = true;

    }

    #endregion

    // Duelist Abilities
    #region

    // Dash
    public Action PerformDash(LivingEntity caster, Tile destination)
    {
        Action action = new Action(true);
        StartCoroutine(PerformDashCoroutine(caster, destination, action));
        return action;
    }
    private IEnumerator PerformDashCoroutine(LivingEntity caster, Tile destination, Action action)
    {
        Ability dash = caster.mySpellBook.GetAbilityByName("Dash");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Dash");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(dash, caster);

        Action dashAction = MovementLogic.Instance.MoveEntity(caster, destination, 6);

        yield return new WaitUntil(() => dashAction.ActionResolved() == true);

        caster.PlayIdleAnimation();
        action.actionResolved = true;

    }

    // Tendon Slash
    public Action PerformTendonSlash(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformTendonSlashCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformTendonSlashCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability tendonSlash = caster.mySpellBook.GetAbilityByName("Tendon Slash");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, tendonSlash);
        bool parry = CombatLogic.Instance.RollForParry(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, tendonSlash, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, tendonSlash, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);


        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Tendon Slash");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(tendonSlash, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, tendonSlash);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Apply Weakened
            if (victim.inDeathProcess == false)
            {
                victim.myPassiveManager.ModifyWeakened(tendonSlash.abilityPrimaryValue);
                yield return new WaitForSeconds(0.5f);
            }
        }               

        // remove camoflage, etc
        OnAbilityUsedFinish(tendonSlash, caster);
        action.actionResolved = true;
        
    }

    // Disarm
    public Action PerformDisarm(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformDisarmCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformDisarmCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability disarm = caster.mySpellBook.GetAbilityByName("Disarm");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, disarm);
        bool parry = CombatLogic.Instance.RollForParry(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, disarm, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, disarm, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Disarm");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(disarm, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, disarm);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Disarm target
            if (victim.inDeathProcess == false)
            {
                victim.myPassiveManager.ModifyDisarmed(1);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(disarm, caster);
        action.actionResolved = true;

    }

    // Shield Shatter
    public Action PerformShieldShatter(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformShieldShatterCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformShieldShatterCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability shieldShatter = caster.mySpellBook.GetAbilityByName("Shield Shatter");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, shieldShatter);
        bool parry = CombatLogic.Instance.RollForParry(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, shieldShatter, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, shieldShatter, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Shield Shatter");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(shieldShatter, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            // Remove all the targets block
            victim.ModifyCurrentBlock(-victim.currentBlock);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, shieldShatter);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(shieldShatter, caster);
        action.actionResolved = true;

    }

    // Evasion
    public Action PerformEvasion(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformEvasionCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformEvasionCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability evasion = caster.mySpellBook.GetAbilityByName("Evasion");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Evasion");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(evasion, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Apply temporary parry
        target.myPassiveManager.ModifyTemporaryParry(evasion.abilityPrimaryValue);
        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(evasion, caster);
        action.actionResolved = true;

    }

    // Decapitate
    public Action PerformDecapitate(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformDecapitateCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformDecapitateCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability decapitate = caster.mySpellBook.GetAbilityByName("Decapitate");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, decapitate);
        bool parry = CombatLogic.Instance.RollForParry(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, decapitate, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, decapitate, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);
        bool instantKill = false;

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Decapitate");
            yield return new WaitForSeconds(0.5f);
        }

        // If target has 30% or less health, they are killed instantly
        if ((victim.currentMaxHealth * 0.3f) >= victim.currentHealth)
        {
            instantKill = true;
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(decapitate, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, decapitate);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            if(!victim.inDeathProcess && instantKill)
            {
                // the victim was insta killed, start death process
                VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "DECAPITATED!");             
                victim.inDeathProcess = true;
                victim.StopAllCoroutines();
                CombatLogic.Instance.HandleDeath(victim);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(decapitate, caster);
        action.actionResolved = true;

    }

    // Second Wind
    public Action PerformSecondWind(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformSecondWindCoroutine(caster, action));
        return action;

    }
    private IEnumerator PerformSecondWindCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability vanish = caster.mySpellBook.GetAbilityByName("Second Wind");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Second Wind");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(vanish, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Gain Max Energy
        caster.ModifyCurrentEnergy(caster.currentMaxEnergy);
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(vanish, caster);
        action.actionResolved = true;

    }

    #endregion

    // Assassination Abilities
    #region

    // Vanish
    public Action PerformVanish(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformVanishCoroutine(caster, action));
        return action;

    }
    private IEnumerator PerformVanishCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability vanish = caster.mySpellBook.GetAbilityByName("Vanish");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Vanish");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(vanish, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Apply temporary parry
        caster.myPassiveManager.ModifyCamoflage(1);
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(vanish, caster);
        action.actionResolved = true;

    }

    // Back Stab
    public Action PerformBackStab(LivingEntity caster, LivingEntity victim)
    {
        Debug.Log("AbilityLogic.PerformBackStab() called. Caster = " + caster.name + ", Target = " + victim.name);
        Action action = new Action(true);
        StartCoroutine(PerformBackStabCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformBackStabCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability backStab = caster.mySpellBook.GetAbilityByName("Back Stab");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, backStab);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Back Stab");
            yield return new WaitForSeconds(0.5f);
        }

        // check for back strike, modify crit
        if (PositionLogic.Instance.CanAttackerBackStrikeTarget(caster, victim))
        {
            critical = true;
        }

        bool parry = CombatLogic.Instance.RollForParry(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, backStab, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, backStab, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        

        // Pay energy cost, + etc
        OnAbilityUsedStart(backStab, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, backStab);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(backStab, caster);
        action.actionResolved = true;

    }


    // Cheap Shot
    public Action PerformCheapShot(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformCheapShotCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformCheapShotCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability cheapShot = caster.mySpellBook.GetAbilityByName("Cheap Shot");
        bool critical = CombatLogic.Instance.RollForCritical(caster, target, cheapShot);
        bool parry = CombatLogic.Instance.RollForParry(target, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, cheapShot, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, target, cheapShot, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Cheap Shot");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(cheapShot, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(target));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, target);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, target, damageType, cheapShot);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Apply vulnerable if camoflaged
            if (caster.myPassiveManager.camoflage || caster.myPassiveManager.stealth)
            {
                target.myPassiveManager.ModifyStunned(1);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(cheapShot, caster);
        action.actionResolved = true;
    }

    // Shank
    public Action PerformShank(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformShankCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformShankCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability shank = caster.mySpellBook.GetAbilityByName("Shank");
        bool critical = CombatLogic.Instance.RollForCritical(caster, target, shank);
        bool parry = CombatLogic.Instance.RollForParry(target, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, shank, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, target, shank, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Shank");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(shank, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(target));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, target);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, target, damageType, shank);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(shank, caster);
        action.actionResolved = true;
    }

    // Rapid Cloaking
    public Action PerformRapidCloaking(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformRapidCloakingCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformRapidCloakingCoroutine(LivingEntity caster, Action action)
    {
        // Setup 
        Ability rapidCloaking = caster.mySpellBook.GetAbilityByName("Rapid Cloaking");
        OnAbilityUsedStart(rapidCloaking, caster);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Rapid Cloaking");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Gain Rapid Cloaking
        caster.myPassiveManager.ModifyRapidCloaking(1);
        yield return new WaitForSeconds(0.5f);

        OnAbilityUsedFinish(rapidCloaking, caster);
        action.actionResolved = true;
    }

    // Shadow Step
    public Action PerformShadowStep(LivingEntity caster, Tile destination)
    {
        Action action = new Action(true);
        StartCoroutine(PerformShadowStepCoroutine(caster, destination, action));
        return action;
    }
    private IEnumerator PerformShadowStepCoroutine(LivingEntity caster, Tile destination, Action action)
    {
        // Setup
        Ability blink = caster.mySpellBook.GetAbilityByName("Shadow Step");
        OnAbilityUsedStart(blink, caster);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Shadow Step");
            yield return new WaitForSeconds(0.5f);
        }

        // teleport
        Action teleportAction = MovementLogic.Instance.TeleportEntity(caster, destination);
        yield return new WaitUntil(() => teleportAction.ActionResolved() == true);

        // Resolve
        OnAbilityUsedFinish(blink, caster);
        action.actionResolved = true;
    }

    // Ambush
    public Action PerformAmbush(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformAmbushCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformAmbushCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability ambush = caster.mySpellBook.GetAbilityByName("Ambush");
        bool critical = CombatLogic.Instance.RollForCritical(caster, target, ambush);
        bool parry = CombatLogic.Instance.RollForParry(target, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, ambush, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, target, ambush, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Ambush");
            yield return new WaitForSeconds(0.5f);
        }


        // Pay energy cost, + etc
        OnAbilityUsedStart(ambush, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(target));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, target);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, target, damageType, ambush);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Gain +20 energy if backstabbing
            if (PositionLogic.Instance.CanAttackerBackStrikeTarget(caster, target))
            {
                caster.ModifyCurrentEnergy(ambush.abilitySecondaryValue);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(ambush, caster);
        action.actionResolved = true;
    }

    // Preparation
    public Action PerformPreparation(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformPreparationCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformPreparationCoroutine(LivingEntity caster, Action action)
    {
        // Set up
        Ability preparation = caster.mySpellBook.GetAbilityByName("Preparation");
        OnAbilityUsedStart(preparation, caster);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Preparation");
            yield return new WaitForSeconds(0.5f);
        }


        // Play animation
        caster.PlaySkillAnimation();

        // Apply preparation
        caster.myPassiveManager.ModifyPreparation(1);
        yield return new WaitForSeconds(0.5f);

        // Resolve
        OnAbilityUsedFinish(preparation, caster);
        action.actionResolved = true;
    }

    // Sharpen Blades
    public Action PerformSharpenBlade(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformSharpenBladeCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformSharpenBladeCoroutine(LivingEntity caster, Action action)
    {
        // Set up
        Ability preparation = caster.mySpellBook.GetAbilityByName("Sharpen Blade");
        OnAbilityUsedStart(preparation, caster);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Sharpen Blade");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Apply Sharpened Blade
        caster.myPassiveManager.ModifySharpenedBlade(1);
        yield return new WaitForSeconds(0.5f);

        // Resolve
        OnAbilityUsedFinish(preparation, caster);
        action.actionResolved = true;
    }

    // Slice And Dice
    public Action PerformSliceAndDice(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformSliceAndDiceCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformSliceAndDiceCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability sliceAndDice = caster.mySpellBook.GetAbilityByName("Slice And Dice");
        int attacksToMake = caster.currentEnergy / 10;
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, sliceAndDice, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Slice And Dice");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(sliceAndDice, caster);

        for (int attacksAlreadyMade = 0; attacksAlreadyMade < attacksToMake; attacksAlreadyMade++)
        {
            // Play attack animation
            caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));

            if (victim.inDeathProcess == false)
            {
                // Set up shot values
                bool critical = CombatLogic.Instance.RollForCritical(caster, victim, sliceAndDice);
                bool parry = CombatLogic.Instance.RollForParry(victim, caster);
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, sliceAndDice, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

                
                // Play arrow shot VFX
                Action shootAction = VisualEffectManager.Instance.ShootArrow(caster.tile.WorldPosition, victim.tile.WorldPosition, 9);
                yield return new WaitUntil(() => shootAction.ActionResolved() == true);

                // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
                if (parry)
                {
                    Action dodgeAction = CombatLogic.Instance.HandleParry(caster, victim);
                    yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
                }

                // if the target did not dodge, handle damage event normally
                else
                {
                    if (critical)
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
                    }
                    Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, sliceAndDice);
                    yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
                }
            }

        }

        // remove camoflage, etc
        OnAbilityUsedFinish(sliceAndDice, caster);
        action.actionResolved = true;

    }

    // Chloroform Bomb
    public Action PerformChloroformBomb(LivingEntity caster, Tile location)
    {
        Action action = new Action(true);
        StartCoroutine(PerformChloroformBombCoroutine(caster, location, action));
        return action;
    }
    private IEnumerator PerformChloroformBombCoroutine(LivingEntity caster, Tile location, Action action)
    {
        // Set up properties
        Ability chloroformBomb = caster.mySpellBook.GetAbilityByName("Chloroform Bomb");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, chloroformBomb);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Chloroform Bomb");
            yield return new WaitForSeconds(0.5f);
        }

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInBlastRadius = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(caster, location, 1, true, false);

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost
        OnAbilityUsedStart(chloroformBomb, caster);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            bool critical = CombatLogic.Instance.RollForCritical(caster, entity, chloroformBomb);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, chloroformBomb, damageType, critical, chloroformBomb.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }

            // Deal Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, chloroformBomb);

            // Apply Silenced
            if (entity.inDeathProcess == false)
            {
                entity.myPassiveManager.ModifySilenced(1);
            }
        }

        // Pay energy cost
        OnAbilityUsedFinish(chloroformBomb, caster);

        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }
    #endregion

    // Guardian Abilities
    #region

    // Guard
    public Action PerformGuard(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformGuardCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformGuardCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Setup
        Ability guard = caster.mySpellBook.GetAbilityByName("Guard");
        OnAbilityUsedStart(guard, caster);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Guard");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Give target block
        target.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(guard.abilityPrimaryValue,caster));
        yield return new WaitForSeconds(0.5f);

        // Finish event
        OnAbilityUsedFinish(guard, caster);
        action.actionResolved = true;
    }

    // Fortify
    public Action PerformFortify(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformFortifyCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformFortifyCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Setup
        Ability fortify = caster.mySpellBook.GetAbilityByName("Fortify");
        OnAbilityUsedStart(fortify, caster);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Fortify");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Give target block
        target.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(fortify.abilityPrimaryValue, caster));
        yield return new WaitForSeconds(0.5f);

        // Remove stunned
        if (target.myPassiveManager.stunned)
        {
            VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Stunned Removed");
            target.myPassiveManager.ModifyStunned(-target.myPassiveManager.stunnedStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Remove immobilized
        if (target.myPassiveManager.immobilized)
        {
            VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Immobilized Removed");
            target.myPassiveManager.ModifyImmobilized(-target.myPassiveManager.immobilizedStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Remove sleep
        if (target.myPassiveManager.sleep)
        {
            VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Sleep Removed");
            target.myPassiveManager.ModifySleep(-target.myPassiveManager.sleepStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Finish event
        OnAbilityUsedFinish(fortify, caster);
        action.actionResolved = true;
    }

    // Stone Form
    public Action PerformStoneForm(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformStoneFormCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformStoneFormCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability stoneForm = caster.mySpellBook.GetAbilityByName("Stone Form");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Stone Form");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost, + etc
        OnAbilityUsedStart(stoneForm, caster);

        // Apply bonus strength
        target.myPassiveManager.ModifyBonusDexterity(stoneForm.abilityPrimaryValue);
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(stoneForm, caster);
        action.actionResolved = true;

    }

    // Provoke
    public Action PerformProvoke(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformProvokeCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformProvokeCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability provoke = caster.mySpellBook.GetAbilityByName("Provoke");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Provoke");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(provoke, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Apply Taunted
        target.myPassiveManager.ModifyTaunted(1, caster);
        yield return new WaitForSeconds(0.5f);   

        // remove camoflage, etc
        OnAbilityUsedFinish(provoke, caster);
        action.actionResolved = true;

    }

    // Challenging Shout
    public Action PerformChallengingShout(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformChallengingShoutCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformChallengingShoutCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability challengingShout = caster.mySpellBook.GetAbilityByName("Challenging Shout");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Challengin Shout");
            yield return new WaitForSeconds(0.5f);
        }

        // Calculate which characters are hit by the global taunt
        List<LivingEntity> targetsInRange = new List<LivingEntity>();
        foreach(LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if(CombatLogic.Instance.IsTargetFriendly(caster, entity) == false)
            {
                targetsInRange.Add(entity);
            }
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost
        OnAbilityUsedStart(challengingShout, caster);

        // Resolve taunts against each enemy
        foreach (LivingEntity entity in targetsInRange)
        {
            entity.myPassiveManager.ModifyTaunted(1, caster);
        }

        // Resolve
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }

    // Sword And Board
    public Action PerformSwordAndBoard(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformSwordAndBoardCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformSwordAndBoardCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability swordAndBoard = caster.mySpellBook.GetAbilityByName("Sword And Board");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, swordAndBoard);
        bool parry = CombatLogic.Instance.RollForParry(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, swordAndBoard, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, swordAndBoard, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Sword And Board");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(swordAndBoard, caster);

        // Gain Block
        caster.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(swordAndBoard.abilityPrimaryValue, caster));

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, swordAndBoard);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(swordAndBoard, caster);
        action.actionResolved = true;

    }

    // Get Down!
    public Action PerformGetDown(LivingEntity caster, Tile destination)
    {
        Action action = new Action(true);
        StartCoroutine(PerformGetDownCoroutine(caster, destination, action));
        return action;
    }
    private IEnumerator PerformGetDownCoroutine(LivingEntity caster, Tile destination, Action action)
    {
    
        Ability getDown = caster.mySpellBook.GetAbilityByName("Get Down!");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Get Down!");
            yield return new WaitForSeconds(0.5f);
        }


        OnAbilityUsedStart(getDown, caster);
        Action moveAction = MovementLogic.Instance.MoveEntity(caster, destination, 4);

        // yield wait until movement complete
        yield return new WaitUntil(() => moveAction.ActionResolved() == true);

        // Give yourself block
        caster.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(getDown.abilitySecondaryValue, caster));

        // Give adjacent characters block
        List<Tile> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, destination);
        foreach (LivingEntity livingEntity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (adjacentTiles.Contains(livingEntity.tile) &&
                CombatLogic.Instance.IsTargetFriendly(caster, livingEntity)
                )
            {
                livingEntity.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(getDown.abilitySecondaryValue, caster));
            }
        }
        caster.PlayIdleAnimation();
        action.actionResolved = true;
    }

    // Testudo
    public Action PerformTestudo(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformTestudoCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformTestudoCoroutine(LivingEntity caster, Action action)
    {
        // Setup 
        Ability testudo = caster.mySpellBook.GetAbilityByName("Testudo");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Testudo");
            yield return new WaitForSeconds(0.5f);
        }


        OnAbilityUsedStart(testudo, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Gain Block
        OnAbilityUsedStart(testudo, caster);
        caster.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(testudo.abilityPrimaryValue, caster));
        yield return new WaitForSeconds(0.5f);

        // Resolve
        OnAbilityUsedFinish(testudo, caster);
        action.actionResolved = true;
    }

    // Shield Slam
    public Action PerformShieldSlam(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformShieldSlamCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformShieldSlamCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability shieldSlam = caster.mySpellBook.GetAbilityByName("Shield Slam");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, shieldSlam);
        bool parry = CombatLogic.Instance.RollForParry(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, shieldSlam, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, shieldSlam, damageType, critical, caster.currentBlock);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Shield Slam");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(shieldSlam, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, shieldSlam);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Knock back.
            if (victim.inDeathProcess == false)
            {
                MovementLogic.Instance.KnockBackEntity(caster, victim, shieldSlam.abilityPrimaryValue);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(shieldSlam, caster);
        action.actionResolved = true;

    }

    // Reactive Armour
    public Action PerformReactiveArmour(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformReactiveArmourCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformReactiveArmourCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability reactiveArmour = caster.mySpellBook.GetAbilityByName("Reactive Armour");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, reactiveArmour, caster.myMainHandWeapon);
        List<LivingEntity> targetsInRange = EntityLogic.GetAllEnemiesWithinRange(caster, caster.currentMeleeRange);
        int baseDamage = caster.currentBlock;

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Reactive Armour");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost
        OnAbilityUsedStart(reactiveArmour, caster);

        // Remove block
        caster.ModifyCurrentBlock(-caster.currentBlock);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsInRange)
        {
            bool critical = CombatLogic.Instance.RollForCritical(caster, entity, reactiveArmour);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, reactiveArmour, damageType, critical, baseDamage);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }

            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, reactiveArmour);
        }        

        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(reactiveArmour, caster);
        action.actionResolved = true;
    }


    #endregion

    // Pyromania Abilities
    #region

    // Fire Ball
    public Action PerformFireBall(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformFireBallCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformFireBallCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        Ability fireball = caster.mySpellBook.GetAbilityByName("Fire Ball");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, fireball);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, fireball);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, fireball, damageType, critical, fireball.abilityPrimaryValue);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Fire Ball");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(fireball, caster);
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));
        yield return new WaitForSeconds(0.15f);

        // Create fireball from prefab and play animation
        Action fireballHit = VisualEffectManager.Instance.ShootFireball(caster.tile.WorldPosition, victim.tile.WorldPosition);

        // wait until fireball has hit the target
        yield return new WaitUntil(() => fireballHit.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, fireball);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            if (!victim.inDeathProcess)
            {
                victim.myPassiveManager.ModifyBurning(fireball.abilitySecondaryValue);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(fireball, caster);
        action.actionResolved = true;
    }

    // Melt
    public Action PerformMelt(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformMeltCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformMeltCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up
        Ability melt = caster.mySpellBook.GetAbilityByName("Melt");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Melt");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(melt, caster);

        // Play Animation
        caster.PlaySkillAnimation();
        yield return new WaitForSeconds(0.15f);

        // Create fire explosion from prefab and play animation
        VisualEffectManager.Instance.ShootFireball(victim.tile.WorldPosition, victim.tile.WorldPosition);

        // Remove all the targets block
        victim.ModifyCurrentBlock(-victim.currentBlock);

        // Apply burning
        victim.myPassiveManager.ModifyBurning(melt.abilityPrimaryValue, caster);

        // remove camoflage, etc
        OnAbilityUsedFinish(melt, caster);
        action.actionResolved = true;
    }

    // Fire Nova
    public Action PerformFireNova(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformFireNovaCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformFireNovaCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability fireNova = caster.mySpellBook.GetAbilityByName("Fire Nova");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, fireNova);
        List<LivingEntity> targetsInRange = EntityLogic.GetAllEnemiesWithinRange(caster, 1);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Fire Nova");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost
        OnAbilityUsedStart(fireNova, caster);

        // Resolve damage against targets
        foreach (LivingEntity entity in targetsInRange)
        {
            bool critical = CombatLogic.Instance.RollForCritical(caster, entity, fireNova);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, fireNova, damageType, critical, fireNova.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            
            // Deal damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, fireNova);

            // Apply burning
            if(entity.inDeathProcess == false)
            {
                entity.myPassiveManager.ModifyBurning(fireNova.abilitySecondaryValue);
            }
        }

        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(fireNova, caster);
        action.actionResolved = true;
    }

    // Phoenix Dive
    public Action PerformPhoenixDive(LivingEntity caster, Tile destination)
    {
        Action action = new Action(true);
        StartCoroutine(PerformPhoenixDiveCoroutine(caster, destination, action));
        return action;
    }
    private IEnumerator PerformPhoenixDiveCoroutine(LivingEntity caster, Tile destination, Action action)
    {
        // Setup
        Ability phoenixDive = caster.mySpellBook.GetAbilityByName("Phoenix Dive");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Phoenix Dive");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(phoenixDive, caster);

        // Teleport to destination
        Action teleportAction = MovementLogic.Instance.TeleportEntity(caster, destination);
        yield return new WaitUntil(() => teleportAction.ActionResolved() == true);

        // Apply 1 burning to adjacent enemies
        List<LivingEntity> targetsInRange = EntityLogic.GetAllEnemiesWithinRange(caster, 1);
        foreach(LivingEntity entity in targetsInRange)
        {
            entity.myPassiveManager.ModifyBurning(phoenixDive.abilityPrimaryValue);
        }

        // Resolve event
        OnAbilityUsedFinish(phoenixDive, caster);
        action.actionResolved = true;

    }

    // Blaze
    public Action PerformBlaze(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformBlazeCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformBlazeCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Setup 
        Ability blaze = caster.mySpellBook.GetAbilityByName("Blaze");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Blaze");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(blaze, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Apply Temp Fire imbuement
        target.myPassiveManager.ModifyTemporaryFireImbuement(1);
        yield return new WaitForSeconds(0.5f);

        OnAbilityUsedFinish(blaze, caster);
        action.actionResolved = true;
    }

    // Meteor
    public Action PerformMeteor(LivingEntity caster, Tile location)
    {
        Action action = new Action(true);
        StartCoroutine(PerformMeteorCoroutine(caster, location, action));
        return action;
    }
    private IEnumerator PerformMeteorCoroutine(LivingEntity caster, Tile location, Action action)
    {
        // Set up properties
        Ability meteor = caster.mySpellBook.GetAbilityByName("Meteor");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, meteor);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Meteor");
            yield return new WaitForSeconds(0.5f);
        }

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInBlastRadius = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(caster, location, 1, true, false);

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost
        OnAbilityUsedStart(meteor, caster);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            bool critical = CombatLogic.Instance.RollForCritical(caster, entity, meteor);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, meteor, damageType, critical, meteor.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }

            // Deal Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, meteor);
            
            // Apply Burning
            if(entity.inDeathProcess == false)
            {
                entity.myPassiveManager.ModifyBurning(meteor.abilitySecondaryValue, caster);
            }
        }

        // Pay energy cost
        OnAbilityUsedFinish(meteor, caster);

        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }

    // Combustion
    public Action PerformCombustion(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformCombustionCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformCombustionCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability combustion = caster.mySpellBook.GetAbilityByName("Combustion");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, combustion);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Combustion");
            yield return new WaitForSeconds(0.5f);
        }

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInBlastRadius = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(caster, victim.tile, 1, true, false);

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost
        OnAbilityUsedStart(combustion, caster);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            bool critical = CombatLogic.Instance.RollForCritical(caster, entity, combustion);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, combustion, damageType, critical, combustion.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }

            // Deal Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, combustion);
        }

        // Pay energy cost
        OnAbilityUsedFinish(combustion, caster);

        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;
    }

    // Dragon Breath
    public Action PerformDragonBreath(LivingEntity caster, Tile location)
    {
        Action action = new Action(true);
        StartCoroutine(PerformDragonBreathCoroutine(caster, location, action));
        return action;
    }
    private IEnumerator PerformDragonBreathCoroutine(LivingEntity caster, Tile location, Action action)
    {
        // Set up properties
        Ability dragonBreath = caster.mySpellBook.GetAbilityByName("Dragon Breath");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, dragonBreath);
        List<Tile> tilesInBreathPath = LevelManager.Instance.GetAllTilesInALine(caster.tile, location, dragonBreath.abilitySecondaryValue, true);
        List<LivingEntity> entitiesInBreathPath = new List<LivingEntity>();

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Dragon Breath");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Calculate which characters are hit by the breath
        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (tilesInBreathPath.Contains(entity.tile))
            {
                entitiesInBreathPath.Add(entity);
            }
        }

        // Pay energy cost
        OnAbilityUsedStart(dragonBreath, caster);

        // Resolve hits against targets
        foreach (LivingEntity entity in entitiesInBreathPath)
        {
            bool critical = CombatLogic.Instance.RollForCritical(caster, entity, dragonBreath);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, dragonBreath, damageType, critical, dragonBreath.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }

            // Deal Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, dragonBreath);
            
        }
        yield return new WaitForSeconds(0.5f);

        // Resolve
        OnAbilityUsedFinish(dragonBreath, caster);        
        action.actionResolved = true;

    }


    #endregion

    // Cyromancy Abilities
    #region

    // Chilling Blow
    public Action PerformChillingBlow(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformChillingBlowCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformChillingBlowCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability chillingBlow = caster.mySpellBook.GetAbilityByName("Chilling Blow");
        bool critical = CombatLogic.Instance.RollForCritical(caster, target, chillingBlow);
        bool parry = CombatLogic.Instance.RollForParry(target, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, chillingBlow, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, target, chillingBlow, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Chilling Blow");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(chillingBlow, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(target));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, target);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, target, damageType, chillingBlow);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Apply Immobilized if chilled
            if (target.inDeathProcess == false && 
                target.myPassiveManager.chilled)
            {
                target.myPassiveManager.ModifyImmobilized(1);
                yield return new WaitForSeconds(0.5f);
            }
        }        

        // remove camoflage, etc
        OnAbilityUsedFinish(chillingBlow, caster);
        action.actionResolved = true;
    }

    // Frost Nova
    public Action PerformFrostNova(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformFrostNovaCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformFrostNovaCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability frostNova = caster.mySpellBook.GetAbilityByName("Frost Nova");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, frostNova);
        List<LivingEntity> targetsInRange = EntityLogic.GetAllEnemiesWithinRange(caster, frostNova.abilitySecondaryValue);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Frost Nova");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost
        OnAbilityUsedStart(frostNova, caster);

        // Resolve damage against targets
        foreach (LivingEntity entity in targetsInRange)
        {
            bool critical = CombatLogic.Instance.RollForCritical(caster, entity, frostNova);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, frostNova, damageType, critical, frostNova.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }

            // Deal damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, frostNova);

            // Apply burning
            if (entity.inDeathProcess == false)
            {
                entity.myPassiveManager.ModifyChilled(1);
            }
        }

        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(frostNova, caster);
        action.actionResolved = true;
    }

    // Global Cooling
    public Action PerformGlobalCooling(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformGlobalCoolingCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformGlobalCoolingCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability globalCooling = caster.mySpellBook.GetAbilityByName("Global Cooling");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, globalCooling);
        List<LivingEntity> entitiesHit = new List<LivingEntity>();

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Global Cooling");
            yield return new WaitForSeconds(0.5f);
        }

        // Get all enemies hit by effect
        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if(!CombatLogic.Instance.IsTargetFriendly(caster, entity))
            {
                entitiesHit.Add(entity);
            }
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost
        OnAbilityUsedStart(globalCooling, caster);

        // Resolve damage against targets
        foreach (LivingEntity entity in entitiesHit)
        {
            bool critical = CombatLogic.Instance.RollForCritical(caster, entity, globalCooling);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, globalCooling, damageType, critical, globalCooling.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }

            // Deal damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, globalCooling);

            // Apply chilled. If already chilled, apply immobilized
            if (entity.inDeathProcess == false)
            {
                if (entity.myPassiveManager.chilled)
                {
                    entity.myPassiveManager.ModifyImmobilized(1);
                }
                else
                {
                    entity.myPassiveManager.ModifyChilled(1);
                }
                
            }
        }

        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(globalCooling, caster);
        action.actionResolved = true;
    }

    // Icy Focus
    public Action PerformIcyFocus(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformIcyFocusCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformIcyFocusCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up
        Ability icyFocus = caster.mySpellBook.GetAbilityByName("Icy Focus");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Icy Focus");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(icyFocus, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Give target wisdom
        target.myPassiveManager.ModifyBonusWisdom(icyFocus.abilityPrimaryValue);
        yield return new WaitForSeconds(0.5f);

        // Resolve Event
        OnAbilityUsedFinish(icyFocus, caster);
        action.actionResolved = true;
        
    }

    // Frost bolt
    public Action PerformFrostBolt(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformFrostBoltCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformFrostBoltCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        Ability frostBolt = caster.mySpellBook.GetAbilityByName("Frost Bolt");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, frostBolt);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, frostBolt);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, frostBolt, damageType, critical, frostBolt.abilityPrimaryValue);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Frost Bolt");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(frostBolt, caster);
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));
        yield return new WaitForSeconds(0.15f);

        // Create frost bolt VFX
        Action frostBoltAction = VisualEffectManager.Instance.ShootFrostBolt(caster.tile.WorldPosition, victim.tile.WorldPosition);
        yield return new WaitUntil(() => frostBoltAction.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }

            // Deal Damage event
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, frostBolt);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Apply Immobilizaed
            if(victim.inDeathProcess == false)
            {
                victim.myPassiveManager.ModifyImmobilized(1);
            }            
        }

        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(frostBolt, caster);
        action.actionResolved = true;
        
    }

    // Blizzard
    public Action PerformBlizzard(LivingEntity caster, Tile location)
    {
        Action action = new Action(true);
        StartCoroutine(PerformBlizzardCoroutine(caster, location, action));
        return action;
    }
    private IEnumerator PerformBlizzardCoroutine(LivingEntity caster, Tile location, Action action)
    {
        // Set up properties
        Ability blizzard = caster.mySpellBook.GetAbilityByName("Blizzard");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, blizzard);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Blizzard");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInBlastRadius = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(caster, location, 1, true, false);

        // Pay energy cost
        OnAbilityUsedStart(blizzard, caster);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            bool critical = CombatLogic.Instance.RollForCritical(caster, entity, blizzard);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, blizzard, damageType, critical, blizzard.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }

            // Deal Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, blizzard);

            // Apply Chilled
            if (entity.inDeathProcess == false)
            {
                entity.myPassiveManager.ModifyChilled(1);
            }
        }

        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }

    // Frost Armour
    public Action PerformFrostArmour(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformFrostArmourCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformFrostArmourCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Setup
        Ability frostArmour = caster.mySpellBook.GetAbilityByName("Frost Armour");
        OnAbilityUsedStart(frostArmour, caster);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Frost Armour");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Give target block
        target.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(frostArmour.abilityPrimaryValue, caster));
        yield return new WaitForSeconds(0.5f);

        // Remove burning
        if (target.myPassiveManager.burning)
        {
            target.myPassiveManager.ModifyBurning(-target.myPassiveManager.burningStacks);
        }

        // Finish event
        OnAbilityUsedFinish(frostArmour, caster);
        action.actionResolved = true;
    }

    // Snow Stasis
    public Action PerformSnowStasis(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformSnowStasisCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformSnowStasisCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability snowStasis = caster.mySpellBook.GetAbilityByName("Snow Stasis");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Snow Stasis");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(snowStasis, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Give bonus energy
        target.myPassiveManager.ModifyBarrier(snowStasis.abilityPrimaryValue);
        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(target.transform.position));
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(snowStasis, caster);
        action.actionResolved = true;

    }


    // Creeping Frost
    public Action PerformCreepingFrost(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformCreepingFrostCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformCreepingFrostCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Setup 
        Ability cf = caster.mySpellBook.GetAbilityByName("Creeping Frost");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Creeping Frost");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(cf, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Apply Temp Frost imbuement
        target.myPassiveManager.ModifyTemporaryFrostImbuement(1);
        yield return new WaitForSeconds(0.5f);

        OnAbilityUsedFinish(cf, caster);
        action.actionResolved = true;
    }


    // Glacial Burst
    public Action PerformGlacialBurst(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformGlacialBurstCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformGlacialBurstCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability glacialBurst = caster.mySpellBook.GetAbilityByName("Glacial Burst");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, glacialBurst);
        List<LivingEntity> targetsInRange = EntityLogic.GetAllEnemiesWithinRange(caster, caster.currentMeleeRange);
        List<LivingEntity> targetsHit = new List<LivingEntity>();

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Glacial Burst");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // get a random target 3 times
        for (int i = 0; i < glacialBurst.abilitySecondaryValue; i++)
        {
            targetsHit.Add(targetsInRange[Random.Range(0, targetsInRange.Count)]);
        }

        // Pay energy cost
        OnAbilityUsedStart(glacialBurst, caster);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsHit)
        {
            if (entity.inDeathProcess == false)
            {
                bool critical = CombatLogic.Instance.RollForCritical(caster, entity, glacialBurst);
                bool dodge = CombatLogic.Instance.RollForDodge(entity, caster);
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, glacialBurst, damageType, critical, glacialBurst.abilityPrimaryValue);

                // Play attack animation
                caster.StartCoroutine(caster.PlayMeleeAttackAnimation(entity));

                // Create frost bolt VFX
                Action frostBoltAction = VisualEffectManager.Instance.ShootFrostBolt(caster.tile.WorldPosition, entity.tile.WorldPosition);
                yield return new WaitUntil(() => frostBoltAction.ActionResolved() == true);

                // if the target successfully dodged, dont do HandleDamage: do dodge stuff instead
                if (dodge)
                {
                    Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, entity);
                    yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
                }

                // if the target did not parry, handle damage event normally
                else
                {
                    if (critical)
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
                    }
                    Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, glacialBurst);
                    yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
                }

            }

        }

        // Remove camo + etc
        OnAbilityUsedFinish(glacialBurst, caster);

        // Resolve/Complete event
        action.actionResolved = true;

    }

    // Thaw
    public Action PerformThaw(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformThawCoroutine(caster, victim, action));
        return action;
    }
    public IEnumerator PerformThawCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        Ability thaw = caster.mySpellBook.GetAbilityByName("Thaw");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, thaw);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, thaw);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, thaw, damageType, critical, thaw.abilityPrimaryValue);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Thaw");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(thaw, caster);
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));
        yield return new WaitForSeconds(0.15f);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, thaw);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Refund energy cost, if target is chilled
            if (victim.myPassiveManager.chilled)
            {
                caster.ModifyCurrentEnergy(20);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(thaw, caster);
        action.actionResolved = true;
    }
    #endregion

    // Ranger
    #region

    // Forest Medicine
    public Action PerformForestMedicine(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformForestMedicineCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformForestMedicineCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Setup
        Ability forestMedicine = caster.mySpellBook.GetAbilityByName("Forest Medicine");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Forest Medicine");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(forestMedicine, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Give target block
        target.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(forestMedicine.abilityPrimaryValue, caster));
        yield return new WaitForSeconds(0.5f);

        // Remove poisoned
        if (target.myPassiveManager.poisoned)
        {
            target.myPassiveManager.ModifyPoisoned(-target.myPassiveManager.poisonedStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Remove burning
        if (target.myPassiveManager.burning)
        {
            target.myPassiveManager.ModifyBurning(-target.myPassiveManager.burningStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Remove shocked
        if (target.myPassiveManager.shocked)
        {
            target.myPassiveManager.ModifyShocked(-target.myPassiveManager.shockedStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Remove Chilled
        if (target.myPassiveManager.chilled)
        {
            target.myPassiveManager.ModifyChilled(-target.myPassiveManager.chilledStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Finish event
        OnAbilityUsedFinish(forestMedicine, caster);
        action.actionResolved = true;
    }

    // Pinning Shot
    public Action PerformPinningShot(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformPinningShotCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformPinningShotCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability pinningShot = caster.mySpellBook.GetAbilityByName("Pinning Shot");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, pinningShot);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, pinningShot, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, pinningShot, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Pinning Shot");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(pinningShot, caster);

        // Ranged attack anim
        caster.PlayRangedAttackAnimation();
        yield return new WaitUntil(() => caster.myRangedAttackFinished == true);

        // Play arrow shot VFX
        Action shootAction = VisualEffectManager.Instance.ShootArrow(caster.tile.WorldPosition, victim.tile.WorldPosition, 9);
        yield return new WaitUntil(() => shootAction.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, pinningShot);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Apply Immobilizaed
            if (victim.inDeathProcess == false)
            {
                victim.myPassiveManager.ModifyImmobilized(1);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(pinningShot, caster);
        action.actionResolved = true;

    }

    // Snipe
    public Action PerformSnipe(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformSnipeCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformSnipeCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability snipe = caster.mySpellBook.GetAbilityByName("Snipe");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, snipe);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, snipe, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, snipe, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Snipe");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(snipe, caster);

        // Ranged attack anim
        caster.PlayRangedAttackAnimation();
        yield return new WaitUntil(() => caster.myRangedAttackFinished == true);

        // Play arrow shot VFX
        Action shootAction = VisualEffectManager.Instance.ShootArrow(caster.tile.WorldPosition, victim.tile.WorldPosition, 9);
        yield return new WaitUntil(() => shootAction.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, snipe);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(snipe, caster);
        action.actionResolved = true;

    }

    // Haste
    public Action PerformHaste(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformHasteCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformHasteCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up
        Ability haste = caster.mySpellBook.GetAbilityByName("Haste");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Haste");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(haste, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Give target mobility 
        target.myPassiveManager.ModifyBonusMobility(haste.abilityPrimaryValue);
        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(target.transform.position));
        yield return new WaitForSeconds(0.5f);

        // Resolve Event
        OnAbilityUsedFinish(haste, caster);
        action.actionResolved = true;

    }

    // Hex
    public Action PerformMarkTarget(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformMarkTargetCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformMarkTargetCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability mark = caster.mySpellBook.GetAbilityByName("Mark Target");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Hex");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(mark, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Apply Weakened
        target.myPassiveManager.ModifyMarked(1);
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(mark, caster);
        action.actionResolved = true;

    }

    // Steady Hands
    public Action PerformSteadyHands(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformSteadyHandsCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformSteadyHandsCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up
        Ability steadyHands = caster.mySpellBook.GetAbilityByName("Steady Hands");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Steady Hands");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(steadyHands, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Give target bonus to ranged attacks
        target.myPassiveManager.ModifyTemporaryHawkEyeBonus(steadyHands.abilityPrimaryValue);
        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(target.transform.position));
        yield return new WaitForSeconds(0.5f);

        // Resolve Event
        OnAbilityUsedFinish(steadyHands, caster);
        action.actionResolved = true;

    }

    // Impaling Bolt
    public Action PerformImpalingBolt(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformImpalingBoltCoroutine(caster, victim, action));
        return action;
    }
    public IEnumerator PerformImpalingBoltCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability impalingBolt = caster.mySpellBook.GetAbilityByName("Impaling Bolt");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, impalingBolt);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, impalingBolt, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, impalingBolt, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Impaling Bolt");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(impalingBolt, caster);

        // Ranged attack anim
        caster.PlayRangedAttackAnimation();
        yield return new WaitUntil(() => caster.myRangedAttackFinished == true);

        // Play arrow shot VFX
        Action shootAction = VisualEffectManager.Instance.ShootArrow(caster.tile.WorldPosition, victim.tile.WorldPosition, 9);
        yield return new WaitUntil(() => shootAction.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }

            // Knockback
            Action knockBackAction = MovementLogic.Instance.KnockBackEntity(caster, victim, impalingBolt.abilitySecondaryValue);
            yield return new WaitUntil(() => knockBackAction.ActionResolved() == true);

            // Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, impalingBolt);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(impalingBolt, caster);
        action.actionResolved = true;
    }    

    // Head Shot
    public Action PerformHeadShot(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformHeadShotCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformHeadShotCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability headShot = caster.mySpellBook.GetAbilityByName("Head Shot");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, headShot);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, headShot, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, headShot, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Head Shot");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(headShot, caster);

        // Ranged attack anim
        caster.PlayRangedAttackAnimation();
        yield return new WaitUntil(() => caster.myRangedAttackFinished == true);

        // Play arrow shot VFX
        Action shootAction = VisualEffectManager.Instance.ShootArrow(caster.tile.WorldPosition, victim.tile.WorldPosition, 9);
        yield return new WaitUntil(() => shootAction.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, headShot);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(headShot, caster);
        action.actionResolved = true;

    }

    // Concentration
    public Action PerformConcentration(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformConcentrationCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformConcentrationCoroutine(LivingEntity caster, Action action)
    {
        // Setup 
        Ability concentration = caster.mySpellBook.GetAbilityByName("Concentration");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Concentration");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(concentration, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Gain Air imbuement
        caster.myPassiveManager.ModifyConcentration(1);
        yield return new WaitForSeconds(0.5f);

        OnAbilityUsedFinish(concentration, caster);
        action.actionResolved = true;
    }

    // Overwatch
    public Action PerformOverwatch(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformOverwatchCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformOverwatchCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability overwatch = caster.mySpellBook.GetAbilityByName("Overwatch");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Overwatch");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost, + etc
        OnAbilityUsedStart(overwatch, caster);

        // Gain Overwatch passive
        caster.myPassiveManager.ModifyOverwatch(1);
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(overwatch, caster);
        action.actionResolved = true;

    }

    // Tree Leap
    public Action PerformTreeLeap(LivingEntity caster, Tile destination)
    {
        Action action = new Action(true);
        StartCoroutine(PerformTreeLeapCoroutine(caster, destination, action));
        return action;
    }
    private IEnumerator PerformTreeLeapCoroutine(LivingEntity caster, Tile destination, Action action)
    {
        // Setup
        Ability treeLeap = caster.mySpellBook.GetAbilityByName("Tree Leap");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Tree Leap");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(treeLeap, caster);

        // Teleport
        Action teleportAction = MovementLogic.Instance.TeleportEntity(caster, destination);
        yield return new WaitUntil(() => teleportAction.ActionResolved() == true);

        // Resolve
        OnAbilityUsedFinish(treeLeap, caster);
        action.actionResolved = true;
    }

    // Rapid Fire
    public Action PerformRapidFire(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformRapidFireCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformRapidFireCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability rapidFire = caster.mySpellBook.GetAbilityByName("Rapid Fire");
        int shotsToFire = caster.currentEnergy / 10;
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, rapidFire, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Rapid Fire");
            yield return new WaitForSeconds(0.5f);
        }

        // Ranged attack anim
        caster.PlayRangedAttackAnimation();
        yield return new WaitUntil(() => caster.myRangedAttackFinished == true);

        // Pay energy cost, + etc
        OnAbilityUsedStart(rapidFire, caster);

        for (int shotsTaken = 0; shotsTaken < shotsToFire; shotsTaken++)
        {
            if (victim.inDeathProcess == false)
            {
                // Set up shot values
                bool critical = CombatLogic.Instance.RollForCritical(caster, victim, rapidFire);
                bool dodge = CombatLogic.Instance.RollForDodge(victim, caster);
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, rapidFire, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

                // Ranged attack anim
                //attacker.PlayRangedAttackAnimation();
                //yield return new WaitUntil(() => attacker.myRangedAttackFinished == true);

                // Play arrow shot VFX
                Action shootAction = VisualEffectManager.Instance.ShootArrow(caster.tile.WorldPosition, victim.tile.WorldPosition, 9);
                yield return new WaitUntil(() => shootAction.ActionResolved() == true);

                // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
                if (dodge)
                {
                    Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, victim);
                    yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
                }

                // if the target did not dodge, handle damage event normally
                else
                {
                    if (critical)
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
                    }
                    Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, rapidFire);
                    yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
                }
            }

        }

        // remove camoflage, etc
        OnAbilityUsedFinish(rapidFire, caster);
        action.actionResolved = true;

    }

    #endregion

    // Corruption
    #region

    // Blight
    public Action PerformBlight(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformBlightCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformBlightCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability blight = caster.mySpellBook.GetAbilityByName("Blight");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Blight");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost, + etc
        OnAbilityUsedStart(blight, caster);

        // Apply Poisoned
        target.myPassiveManager.ModifyPoisoned(blight.abilityPrimaryValue, caster);
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(blight, caster);
        action.actionResolved = true;

    }

    // Blood Offering
    public Action PerformBloodOffering(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformBloodOfferingCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformBloodOfferingCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability bloodOffering = caster.mySpellBook.GetAbilityByName("Blood Offering");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Blood Offering");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(bloodOffering, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Gain Energy
        caster.ModifyCurrentEnergy(bloodOffering.abilityPrimaryValue);
        yield return new WaitForSeconds(1f);

        // Reduce Health
        Action selfDamageAction = CombatLogic.Instance.HandleDamage(bloodOffering.abilitySecondaryValue, caster, caster, "None", null, true);
        yield return new WaitUntil(() => selfDamageAction.ActionResolved() == true);        

        // remove camoflage, etc
        OnAbilityUsedFinish(bloodOffering, caster);
        action.actionResolved = true;

    }

    // Toxic Slash
    public Action PerformToxicSlash(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformToxicSlashCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformToxicSlashCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability toxicSlash = caster.mySpellBook.GetAbilityByName("Toxic Slash");
        bool critical = CombatLogic.Instance.RollForCritical(caster, target, toxicSlash);
        bool parry = CombatLogic.Instance.RollForParry(target, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, toxicSlash, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, target, toxicSlash, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Toxic Slash");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(toxicSlash, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(target));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, target);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, target, damageType, toxicSlash);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Apply poisoned
            if (target.inDeathProcess == false)
            {
                target.myPassiveManager.ModifyPoisoned(toxicSlash.abilityPrimaryValue, caster);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(toxicSlash, caster);
        action.actionResolved = true;
    }

    // Noxious Fumes
    public Action PerformNoxiousFumes(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformNoxiousFumesCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformNoxiousFumesCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability noxiousFumes = caster.mySpellBook.GetAbilityByName("Noxious Fumes");
        List<LivingEntity> targetsInRange = EntityLogic.GetAllEnemiesWithinRange(caster, noxiousFumes.abilitySecondaryValue);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Noxious Fumes");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost
        OnAbilityUsedStart(noxiousFumes, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Resolve damage against targets
        foreach (LivingEntity entity in targetsInRange)
        {
            // Apply poisoned
            entity.myPassiveManager.ModifyPoisoned(noxiousFumes.abilityPrimaryValue, caster);
            yield return new WaitForSeconds(0.5f);

            // Apply Silenced
            entity.myPassiveManager.ModifySilenced(1);
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(noxiousFumes, caster);
        action.actionResolved = true;
    }

    // Toxic Eruption
    public Action PerformToxicEruption(LivingEntity caster, Tile location)
    {
        Action action = new Action(true);
        StartCoroutine(PerformToxicEruptionCoroutine(caster, location, action));
        return action;
    }
    private IEnumerator PerformToxicEruptionCoroutine(LivingEntity caster, Tile location, Action action)
    {
        // Set up properties
        Ability toxicEruption = caster.mySpellBook.GetAbilityByName("Toxic Eruption");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, toxicEruption);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Toxic Eruption");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInBlastRadius = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(caster, location, 1, true, false);

        // Pay energy cost
        OnAbilityUsedStart(toxicEruption, caster);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            bool critical = CombatLogic.Instance.RollForCritical(caster, entity, toxicEruption);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, toxicEruption, damageType, critical, toxicEruption.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }

            // Deal Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, toxicEruption);

            // Apply Poisoned
            if (entity.inDeathProcess == false)
            {
                entity.myPassiveManager.ModifyPoisoned(1, caster);
            }
        }

        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }

    // Toxic Rain
    public Action PerformToxicRain(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformToxicRainCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformToxicRainCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability toxicRain = caster.mySpellBook.GetAbilityByName("Toxic Rain");
        List<LivingEntity> entitiesHit = new List<LivingEntity>();

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Toxic Rain");
            yield return new WaitForSeconds(0.5f);
        }

        // Get all enemies hit by effect
        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (!CombatLogic.Instance.IsTargetFriendly(caster, entity))
            {
                entitiesHit.Add(entity);
            }
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost
        OnAbilityUsedStart(toxicRain, caster);

        // Resolve damage against targets
        foreach (LivingEntity entity in entitiesHit)
        {
            // Apply poisoned
            entity.myPassiveManager.ModifyPoisoned(1, caster);
        }

        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(toxicRain, caster);
        action.actionResolved = true;
    }

    // Chemical Reaction
    public Action PerformChemicalReaction(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformChemicalReactionCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformChemicalReactionCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability blight = caster.mySpellBook.GetAbilityByName("Chemical Reaction");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Chemical Reaction");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost, + etc
        OnAbilityUsedStart(blight, caster);

        // Double targets poison count
        target.myPassiveManager.ModifyPoisoned(target.myPassiveManager.poisonedStacks, caster);
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(blight, caster);
        action.actionResolved = true;

    }

    // Drain
    public Action PerformDrain(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformDrainCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformDrainCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Setup
        Ability drain = caster.mySpellBook.GetAbilityByName("Drain");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, drain);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, drain, damageType, false, victim.myPassiveManager.poisonedStacks);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Drain");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(drain, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Deal Damage
        Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, drain);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

        // Remove targets poison
        if (!victim.inDeathProcess)
        {
            victim.myPassiveManager.ModifyPoisoned(-victim.myPassiveManager.poisonedStacks);
        }
        // Resolve
        OnAbilityUsedFinish(drain, caster);
        action.actionResolved = true;
    }



    #endregion

    // Manipulation
    #region

    // Telekinesis
    public Action PerformTelekinesis(LivingEntity caster, LivingEntity target, Tile destination)
    {
        Action action = new Action(true);
        StartCoroutine(PerformTelekinesisCoroutine(caster, target, destination, action));
        return action;
    }
    private IEnumerator PerformTelekinesisCoroutine(LivingEntity caster, LivingEntity target, Tile destination, Action action)
    {
        Ability telekinesis = caster.mySpellBook.GetAbilityByName("Telekinesis");
        OnAbilityUsedStart(telekinesis, caster);

        if(target != caster)
        {
            // Play animation
            caster.PlaySkillAnimation();
        }

        Action teleportAction = MovementLogic.Instance.TeleportEntity(target, destination);
        yield return new WaitUntil(() => teleportAction.ActionResolved() == true);

        OnAbilityUsedFinish(telekinesis, caster);
        action.actionResolved = true;

        yield return null;
    }

    // Dimensional Blast
    public Action PerformDimensionalBlast(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformDimensionalBlastCoroutine(caster, victim, action));
        return action;
    }
    public IEnumerator PerformDimensionalBlastCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        Ability dimensionalBlast = caster.mySpellBook.GetAbilityByName("Dimensional Blast");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, dimensionalBlast);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, caster);
        string damageType = CombatLogic.Instance.GetRandomDamageType();
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, dimensionalBlast, damageType, critical, dimensionalBlast.abilityPrimaryValue);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Dimensional Blast");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(dimensionalBlast, caster);
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));
        yield return new WaitForSeconds(0.15f);

        // TO DO: should create a project relevant to the damage type generated randomly (e.g., fire creates a fireball, shadow creates a shadow bal, etc)

        // Create fireball from prefab and play animation
        Action fireballHit = VisualEffectManager.Instance.ShootFireball(caster.tile.WorldPosition, victim.tile.WorldPosition);

        // wait until fireball has hit the target
        yield return new WaitUntil(() => fireballHit.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, dimensionalBlast);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(dimensionalBlast, caster);
        action.actionResolved = true;
    }

    // Mirage
    public Action PerformMirage(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformMirageCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformMirageCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability mirage = caster.mySpellBook.GetAbilityByName("Mirage");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Mirage");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost, + etc
        OnAbilityUsedStart(mirage, caster);

        // Apply temporary dodge
        target.myPassiveManager.ModifyTemporaryDodge(mirage.abilityPrimaryValue);
        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(mirage, caster);
        action.actionResolved = true;

    }

    // Phase Shift
    public Action PerformPhaseShift(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformPhaseShiftCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformPhaseShiftCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Setup
        Ability phaseShift = caster.mySpellBook.GetAbilityByName("Phase Shift");
        Tile casterDestination = target.tile;
        Tile targetDestination = caster.tile;

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Phase Shift");
            yield return new WaitForSeconds(0.5f);
        }

        // Start
        OnAbilityUsedStart(phaseShift, caster);

        // Teleport both characters
        Action teleActionOne = MovementLogic.Instance.TeleportEntity(caster, casterDestination, true);
        Action teleActionTwo = MovementLogic.Instance.TeleportEntity(target, targetDestination, true);

        // Wait for teleport events to finish
        yield return new WaitUntil(() => teleActionOne.ActionResolved() == true);
        yield return new WaitUntil(() => teleActionTwo.ActionResolved() == true);

        // Resolve
        OnAbilityUsedFinish(phaseShift, caster);
        action.actionResolved = true;
    }

    // Burst Of Knowledge
    public Action PerformBurstOfKnowledge(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformBurstOfKnowledgeCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformBurstOfKnowledgeCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability burstOfKnowledge = caster.mySpellBook.GetAbilityByName("Burst Of Knowledge");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Burst Of Knowledge");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost, + etc
        OnAbilityUsedStart(burstOfKnowledge, caster);

        // Apply temporary wisdom
        target.myPassiveManager.ModifyTemporaryWisdom(burstOfKnowledge.abilityPrimaryValue);
        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(burstOfKnowledge, caster);
        action.actionResolved = true;

    }

    // Blink
    public Action PerformBlink(LivingEntity caster, Tile destination)
    {
        Action action = new Action(true);
        StartCoroutine(PerformBlinkCoroutine(caster, destination, action));
        return action;
    }
    private IEnumerator PerformBlinkCoroutine(LivingEntity caster, Tile destination, Action action)
    {
        // Setup
        Ability blink = caster.mySpellBook.GetAbilityByName("Blink");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Blink");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(blink, caster);

        // teleport
        Action teleportAction = MovementLogic.Instance.TeleportEntity(caster, destination);
        yield return new WaitUntil(() => teleportAction.ActionResolved() == true);

        // Resolve
        OnAbilityUsedFinish(blink, caster);
        action.actionResolved = true;
    }

    // Dimensional Hex
    public Action PerformDimensionalHex(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformDimensionalHexCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformDimensionalHexCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability dimensionalHex = caster.mySpellBook.GetAbilityByName("Dimensional Hex");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Dimensional Hex");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost, + etc
        OnAbilityUsedStart(dimensionalHex, caster);

        // Apply burning, poisoned, chilled and shocked
        target.myPassiveManager.ModifyBurning(dimensionalHex.abilityPrimaryValue);
        target.myPassiveManager.ModifyPoisoned(dimensionalHex.abilityPrimaryValue, caster);
        target.myPassiveManager.ModifyChilled(dimensionalHex.abilityPrimaryValue);
        target.myPassiveManager.ModifyShocked(dimensionalHex.abilityPrimaryValue);
        StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(dimensionalHex, caster);
        action.actionResolved = true;
    }

    // Infuse
    public Action PerformInfuse(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformInfuseCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformInfuseCoroutine(LivingEntity caster, Action action)
    {
        // Setup 
        Ability infuse = caster.mySpellBook.GetAbilityByName("Infuse");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Infuse");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(infuse, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Gain Infuse
        caster.myPassiveManager.ModifyInfuse(1);
        yield return new WaitForSeconds(0.5f);

        OnAbilityUsedFinish(infuse, caster);
        action.actionResolved = true;
    }

    // Time Warp
    public Action PerformTimeWarp(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformTimeWarpCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformTimeWarpCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability timeWarp = caster.mySpellBook.GetAbilityByName("Time Warp");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Time Warp");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost, + etc
        OnAbilityUsedStart(timeWarp, caster);

        // Apply time warp buff
        target.myPassiveManager.ModifyTimeWarp(1);
        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(target.transform.position));
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(timeWarp, caster);
        action.actionResolved = true;

    }


    #endregion

    // Divinity
    #region

    // Holy Fire
    public Action PerformHolyFire(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformHolyFireCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformHolyFireCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Setup
        Ability holyFire = caster.mySpellBook.GetAbilityByName("Holy Fire");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Holy Fire");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(holyFire, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Create holy fire from prefab and play animation
        Action holyFireHit = VisualEffectManager.Instance.ShootHolyFire(target.tile.WorldPosition);
        yield return new WaitUntil(() => holyFireHit.ActionResolved() == true);

        // Give block if ally
        if (CombatLogic.Instance.IsTargetFriendly(caster, target))
        {
            target.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(holyFire.abilityPrimaryValue, caster));
        }

        // Deal damage if enemy
        else
        {
            // Set up
            bool critical = CombatLogic.Instance.RollForCritical(caster, target, holyFire);
            bool dodge = CombatLogic.Instance.RollForDodge(target, caster);
            string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, holyFire);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, target, holyFire, damageType, critical, holyFire.abilityPrimaryValue);

            
            // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
            if (dodge)
            {
                Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, target);
                yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
            }

            // if the target did not dodge, handle damage event normally
            else
            {
                if (critical)
                {
                    VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
                }
                Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, target, damageType, holyFire);
                yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
            }
        }

        // Resolve
        OnAbilityUsedFinish(holyFire, caster);
        action.actionResolved = true;

    }

    // Blinding Light
    public Action PerformBlindingLight(LivingEntity caster, Tile location)
    {
        Action action = new Action(true);
        StartCoroutine(PerformBlindingLightCoroutine(caster, location, action));
        return action;
    }
    private IEnumerator PerformBlindingLightCoroutine(LivingEntity caster, Tile location, Action action)
    {
        // Set up properties
        Ability blindingLight = caster.mySpellBook.GetAbilityByName("Blinding Light");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, blindingLight);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Blinding Light");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInBlastRadius = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(caster, location, 1, true, false);

        // Pay energy cost
        OnAbilityUsedStart(blindingLight, caster);

        // Resolve damage against targets hit
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            bool critical = CombatLogic.Instance.RollForCritical(caster, entity, blindingLight);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, blindingLight, damageType, critical, blindingLight.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }

            // Deal Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, blindingLight);            
        }

        // Brief delay between damage VFX and Blind VFX
        yield return new WaitForSeconds(0.5f);

        // Blind all targets hit
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            // Apply Blind
            if (entity.inDeathProcess == false)
            {
                entity.myPassiveManager.ModifyBlind(1);
            }
        }
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }

    // Inspire
    public Action PerformInspire(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformInspireCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformInspireCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability inspire = caster.mySpellBook.GetAbilityByName("Inspire");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Inspire");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost, + etc
        OnAbilityUsedStart(inspire, caster);

        // Apply bonus strength
        target.myPassiveManager.ModifyBonusStrength(inspire.abilityPrimaryValue);
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(inspire, caster);
        action.actionResolved = true;

    }

    // Bless
    public Action PerformBless(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformBlessCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformBlessCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability bless = caster.mySpellBook.GetAbilityByName("Bless");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Bless");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost, + etc
        OnAbilityUsedStart(bless, caster);

        // Give target block
        target.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(bless.abilityPrimaryValue, caster));
        yield return new WaitForSeconds(0.5f);

        // Remove Blind
        if (target.myPassiveManager.blind)
        {
            target.myPassiveManager.ModifyBlind(-target.myPassiveManager.blindStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Remove Disarmed
        if (target.myPassiveManager.disarmed)
        {
            target.myPassiveManager.ModifyDisarmed(-target.myPassiveManager.disarmedStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Remove Silenced
        if (target.myPassiveManager.silenced)
        {
            target.myPassiveManager.ModifySilenced(-target.myPassiveManager.silencedStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Resolve
        OnAbilityUsedFinish(bless, caster);
        action.actionResolved = true;

    }

    // Transcendence
    public Action PerformTranscendence(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformTranscendenceCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformTranscendenceCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability transcendence = caster.mySpellBook.GetAbilityByName("Transcendence");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Transcendence");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost, + etc
        OnAbilityUsedStart(transcendence, caster);

        // Apply transcendence
        target.myPassiveManager.ModifyTranscendence(1);
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(transcendence, caster);
        action.actionResolved = true;

    }

    // Consecrate
    public Action PerformConsecrate(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformConsecrateCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformConsecrateCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability consecrate = caster.mySpellBook.GetAbilityByName("Consecrate");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, consecrate);
        List<LivingEntity> targetsInRange = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(caster, caster.tile, caster.currentAuraSize, true, true);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Consecrate");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost
        OnAbilityUsedStart(consecrate, caster);

        // Give block to allies        
        foreach (LivingEntity entity in targetsInRange)
        {
            if (CombatLogic.Instance.IsTargetFriendly(caster, entity))
            {
                // Create holy fire from prefab and play animation
                Action holyFireHit = VisualEffectManager.Instance.ShootHolyFire(entity.tile.WorldPosition);

                // Give target block
                entity.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(consecrate.abilitySecondaryValue, caster));
            }
        }

        // Deal damage to enemies
        foreach (LivingEntity entity in targetsInRange)
        {                       
            // Damage enemies
            if (!CombatLogic.Instance.IsTargetFriendly(caster, entity))
            {
                // Set up
                bool critical = CombatLogic.Instance.RollForCritical(caster, entity, consecrate);
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, consecrate, damageType, critical, consecrate.abilityPrimaryValue);

                // Create holy fire from prefab and play animation
                Action holyFireHit = VisualEffectManager.Instance.ShootHolyFire(entity.tile.WorldPosition);

                if (critical)
                {
                    VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
                }

                // Deal damage
                Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, consecrate);

            }
        }

        yield return new WaitForSeconds(0.5f);

        // Resolve
        OnAbilityUsedFinish(consecrate, caster);
        action.actionResolved = true;
    }

    // Invigorate
    public Action PerformInvigorate(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformInvigorateCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformInvigorateCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability invigorate = caster.mySpellBook.GetAbilityByName("Invigorate");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Invigorate");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(invigorate, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Give bonus energy
        target.ModifyCurrentEnergy(invigorate.abilityPrimaryValue);
        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(target.transform.position));
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(invigorate, caster);
        action.actionResolved = true;

    }

    // Judgement
    public Action PerformJudgement(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformJudgementCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformJudgementCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability judgement = caster.mySpellBook.GetAbilityByName("Judgement");
        bool critical = CombatLogic.Instance.RollForCritical(caster, target, judgement);
        bool parry = CombatLogic.Instance.RollForParry(target, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, judgement, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, target, judgement, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Judgement");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(judgement, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(target));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, target);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, target, damageType, judgement);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Apply Weakened and Vulnerable
            if (!target.inDeathProcess)
            {
                target.myPassiveManager.ModifyWeakened(1);
                yield return new WaitForSeconds(0.5f);
                target.myPassiveManager.ModifyVulnerable(1);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(judgement, caster);
        action.actionResolved = true;
    }

    // Purity
    public Action PerformPurity(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformPurityCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformPurityCoroutine(LivingEntity caster, Action action)
    {
        // Setup 
        Ability purity = caster.mySpellBook.GetAbilityByName("Purity");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Purity");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(purity, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Gain Purity buff
        caster.myPassiveManager.ModifyPurity(1);
        yield return new WaitForSeconds(0.5f);

        OnAbilityUsedFinish(purity, caster);
        action.actionResolved = true;
    }

    #endregion

    // Shadow Craft
    #region

    // Rain Of Chaos
    public Action PerformRainOfChaos(LivingEntity caster, Tile location)
    {
        Action action = new Action(true);
        StartCoroutine(PerformRainOfChaosCoroutine(caster, location, action));
        return action;
    }
    private IEnumerator PerformRainOfChaosCoroutine(LivingEntity caster, Tile location, Action action)
    {
        // Set up properties
        Ability rainOfChaos = caster.mySpellBook.GetAbilityByName("Rain Of Chaos");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, rainOfChaos);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Rain Of Chaos");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInBlastRadius = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(caster, location, 1, true, false);

        // Pay energy cost
        OnAbilityUsedStart(rainOfChaos, caster);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            bool critical = CombatLogic.Instance.RollForCritical(caster, entity, rainOfChaos);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, rainOfChaos, damageType, critical, rainOfChaos.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }

            // Deal Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, rainOfChaos);

            // Apply Weakened
            if (entity.inDeathProcess == false)
            {
                entity.myPassiveManager.ModifyWeakened(1);
                yield return new WaitForSeconds(0.5f);
            }
        }

        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }

    // Shroud
    public Action PerformShroud(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformShroudCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformShroudCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability shroud = caster.mySpellBook.GetAbilityByName("Shroud");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Shroud");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost, + etc
        OnAbilityUsedStart(shroud, caster);

        // Apply camoflage
        target.myPassiveManager.ModifyCamoflage(1);
        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(target.transform.position));
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(shroud, caster);
        action.actionResolved = true;

    }

    // Hex
    public Action PerformHex(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformHexCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformHexCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability hex = caster.mySpellBook.GetAbilityByName("Hex");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Hex");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(hex, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Apply Weakened
        target.myPassiveManager.ModifyWeakened(1);
        yield return new WaitForSeconds(0.5f);

        // Apply Vulnerable
        target.myPassiveManager.ModifyVulnerable(1);
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(hex, caster);
        action.actionResolved = true;

    }

    // Dark Gift
    public Action PerformDarkGift(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformDarkGiftCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformDarkGiftCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability darkGift = caster.mySpellBook.GetAbilityByName("Dark Gift");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Dark Gift");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(darkGift, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Apply Dark Gift
        target.myPassiveManager.ModifyDarkGift(1);
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(darkGift, caster);
        action.actionResolved = true;

    }

    // Pure Hate
    public Action PerformPureHate(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformPureHateCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformPureHateCoroutine(LivingEntity caster,  Action action)
    {
        // Set up properties
        Ability pureHate = caster.mySpellBook.GetAbilityByName("Pure Hate");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Pure Hate");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(pureHate, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Apply Dark Gift
        caster.myPassiveManager.ModifyPureHate(1);
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(pureHate, caster);
        action.actionResolved = true;

    }

    // Chaos Bolt
    public Action PerformChaosBolt(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformChaosBoltCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformChaosBoltCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        Ability chaosBolt = caster.mySpellBook.GetAbilityByName("Chaos Bolt");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, chaosBolt);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, chaosBolt);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, chaosBolt, damageType, critical, chaosBolt.abilityPrimaryValue);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Chaos Bolt");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(chaosBolt, caster);
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));
        yield return new WaitForSeconds(0.15f);

        // Shoot shadow ball
        Action shootAction = VisualEffectManager.Instance.ShootShadowBall(caster.tile.WorldPosition, victim.tile.WorldPosition);
        yield return new WaitUntil(() => shootAction.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, chaosBolt);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            if (!victim.inDeathProcess)
            {
                victim.myPassiveManager.ModifyStunned(1);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(chaosBolt, caster);
        action.actionResolved = true;
    }

    // Nightmare
    public Action PerformNightmare(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformNightmareCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformNightmareCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability nightmare = caster.mySpellBook.GetAbilityByName("Nightmare");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Nightmare");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(nightmare, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Apply Sleep
        target.myPassiveManager.ModifySleep(1);
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(nightmare, caster);
        action.actionResolved = true;

    }

    // Unbridled Chaos
    public Action PerformUnbridledChaos(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformUnbridledChaosCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformUnbridledChaosCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability unbridledChaos = caster.mySpellBook.GetAbilityByName("Unbridled Chaos");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, unbridledChaos);
        List<LivingEntity> targetsInRange = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(caster, caster.tile, caster.currentAuraSize, true, false);
        List<LivingEntity> targetsHit = new List<LivingEntity>();

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Unbridled Chaos");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // get a random target 5 times
        for (int i = 0; i < unbridledChaos.abilitySecondaryValue; i++)
        {
            targetsHit.Add(targetsInRange[Random.Range(0, targetsInRange.Count)]);
        }

        // Pay energy cost
        OnAbilityUsedStart(unbridledChaos, caster);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsHit)
        {
            if (entity.inDeathProcess == false)
            {
                bool critical = CombatLogic.Instance.RollForCritical(caster, entity, unbridledChaos);
                bool dodge = CombatLogic.Instance.RollForDodge(entity, caster);
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, unbridledChaos, damageType, critical, unbridledChaos.abilityPrimaryValue);

                // Play attack animation
                caster.StartCoroutine(caster.PlayMeleeAttackAnimation(entity));

                // Shoot shadow ball
                Action shootAction = VisualEffectManager.Instance.ShootShadowBall(caster.tile.WorldPosition, entity.tile.WorldPosition);
                yield return new WaitUntil(() => shootAction.ActionResolved() == true);

                // if the target successfully dodged, dont do HandleDamage: do dodge stuff instead
                if (dodge)
                {
                    Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, entity);
                    yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
                }

                // if the target did not parry, handle damage event normally
                else
                {
                    if (critical)
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
                    }
                    Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, unbridledChaos);
                    yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
                }

            }

        }

        // Remove camo + etc
        OnAbilityUsedFinish(unbridledChaos, caster);

        // Resolve/Complete event
        action.actionResolved = true;

    }

    // Shadow Wreath
    public Action PerformShadowWreath(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformShadowWreathCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformShadowWreathCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Setup 
        Ability sw = caster.mySpellBook.GetAbilityByName("Shadow Wreath");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Shadow Wreath");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(sw, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Apply Temp Shadow imbuement
        target.myPassiveManager.ModifyTemporaryShadowImbuement(1);
        yield return new WaitForSeconds(0.5f);

        OnAbilityUsedFinish(sw, caster);
        action.actionResolved = true;
    }

    // Shadow Blast
    public Action PerformShadowBlast(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformShadowBlastCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformShadowBlastCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        Ability shadowBlast = caster.mySpellBook.GetAbilityByName("Shadow Blast");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, shadowBlast);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, shadowBlast);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, shadowBlast, damageType, critical, shadowBlast.abilityPrimaryValue);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Shadow Blast");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(shadowBlast, caster);
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));
        yield return new WaitForSeconds(0.15f);

        // Shoot shadow ball
        Action shootAction = VisualEffectManager.Instance.ShootShadowBall(caster.tile.WorldPosition, victim.tile.WorldPosition);
        yield return new WaitUntil(() => shootAction.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, shadowBlast);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Knock back.
            if (victim.inDeathProcess == false)
            {
                MovementLogic.Instance.KnockBackEntity(caster, victim, shadowBlast.abilitySecondaryValue);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(shadowBlast, caster);
        action.actionResolved = true;
    }



    #endregion

    // Naturalist
    #region

    // Thunder Strike
    public Action PerformThunderStrike(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformThunderStrikeCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformThunderStrikeCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability thunderStrike = caster.mySpellBook.GetAbilityByName("Thunder Strike");
        bool critical = CombatLogic.Instance.RollForCritical(caster, target, thunderStrike);
        bool parry = CombatLogic.Instance.RollForParry(target, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, thunderStrike, caster.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, target, thunderStrike, damageType, critical, caster.myMainHandWeapon.baseDamage, caster.myMainHandWeapon);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Thunder Strike");
            yield return new WaitForSeconds(0.5f);
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(thunderStrike, caster);

        // Play attack animation
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(target));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(caster, target);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, target, damageType, thunderStrike);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Apply Stunned if Shocked
            if (target.inDeathProcess == false && target.myPassiveManager.shocked)
            {
                target.myPassiveManager.ModifyStunned(1);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(thunderStrike, caster);
        action.actionResolved = true;
    }

    // Lightning Bolt
    public Action PerformLightningBolt(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action(true);
        StartCoroutine(PerformLightningBoltCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformLightningBoltCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        Ability lightningBolt = caster.mySpellBook.GetAbilityByName("Lightning Bolt");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, lightningBolt);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, lightningBolt);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, lightningBolt, damageType, critical, lightningBolt.abilityPrimaryValue);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Lightning Bolt");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(lightningBolt, caster);
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));
        yield return new WaitForSeconds(0.15f);

        // Create fireball from prefab and play animation
        Action fireballHit = VisualEffectManager.Instance.ShootFireball(caster.tile.WorldPosition, victim.tile.WorldPosition);
        yield return new WaitUntil(() => fireballHit.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, lightningBolt);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Apply Shocked
            if (!victim.inDeathProcess)
            {
                victim.myPassiveManager.ModifyShocked(1);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(lightningBolt, caster);
        action.actionResolved = true;
    }

    // Spirit Surge
    public Action PerformSpiritSurge(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformSpiritSurgeCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformSpiritSurgeCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability spiritSurge = caster.mySpellBook.GetAbilityByName("Spirit Surge");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Spirit Surge");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Start
        OnAbilityUsedStart(spiritSurge, caster);

        List<Tile> tilesInSurgeRange = LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalAuraSize(caster), caster.tile);
        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (tilesInSurgeRange.Contains(entity.tile) &&
                CombatLogic.Instance.IsTargetFriendly(caster, entity))
            {
                StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(entity.transform.position));
                entity.ModifyCurrentEnergy(spiritSurge.abilityPrimaryValue);
            }
        }

        yield return new WaitForSeconds(1f);

        // Resolve
        OnAbilityUsedFinish(spiritSurge, caster);
        action.actionResolved = true;

    }

    // Spirit Vision
    public Action PerformSpiritVision(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformSpiritVisionCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformSpiritVisionCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability spiritVision = caster.mySpellBook.GetAbilityByName("Spirit Vision");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Spirit Vision");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Start
        OnAbilityUsedStart(spiritVision, caster);

        // Give target block
        target.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(spiritVision.abilityPrimaryValue, caster));
        yield return new WaitForSeconds(0.5f);

        // Apply temporary True Sight
        target.myPassiveManager.ModifyTemporaryTrueSight(1);
        yield return new WaitForSeconds(0.5f);

        // Resolve
        OnAbilityUsedFinish(spiritVision, caster);
        action.actionResolved = true;

    }

    // Chain Lightning
    public Action PerformChainLightning(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformChainLightningCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformChainLightningCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        // Setup
        Ability chainLightning = caster.mySpellBook.GetAbilityByName("Chain Lightning");
        bool critical = CombatLogic.Instance.RollForCritical(caster, victim, chainLightning);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, caster);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, chainLightning);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, victim, chainLightning, damageType, critical, chainLightning.abilityPrimaryValue);
        LivingEntity currentTarget = victim;
        LivingEntity previousTarget = victim;

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Chain Lightning");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        OnAbilityUsedStart(chainLightning, caster);

        // Resolve attack against the first target
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }

            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, victim, damageType, chainLightning);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            if (!victim.inDeathProcess)
            {
                victim.myPassiveManager.ModifyShocked(1);
            }

            for (int lightningJumps = 0; lightningJumps < chainLightning.abilitySecondaryValue; lightningJumps++)
            {
                List<Tile> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, currentTarget.tile);

                foreach (LivingEntity enemy in LivingEntityManager.Instance.allLivingEntities)
                {
                    if (adjacentTiles.Contains(enemy.tile) && CombatLogic.Instance.IsTargetFriendly(caster, enemy) == false)
                    {
                        previousTarget = currentTarget;
                        currentTarget = enemy;
                    }
                }

                if (previousTarget != currentTarget)
                {
                    bool critical2 = CombatLogic.Instance.RollForCritical(caster, victim, chainLightning);
                    int finalDamageValue2 = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, currentTarget, chainLightning, damageType, critical2, chainLightning.abilityPrimaryValue);

                    if (critical2)
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
                    }

                    Action abilityAction2 = CombatLogic.Instance.HandleDamage(finalDamageValue2, caster, currentTarget, damageType, chainLightning);
                    yield return new WaitUntil(() => abilityAction2.ActionResolved() == true);

                    if (!currentTarget.inDeathProcess)
                    {
                        currentTarget.myPassiveManager.ModifyShocked(1);
                    }
                }

            }

        }

        // Resolve
        OnAbilityUsedFinish(chainLightning, caster);
        action.actionResolved = true;

    }

    // Thunder Storm
    public Action PerformThunderStorm(LivingEntity caster, Tile location)
    {
        Action action = new Action(true);
        StartCoroutine(PerformThunderStormCoroutine(caster, location, action));
        return action;
    }
    private IEnumerator PerformThunderStormCoroutine(LivingEntity caster, Tile location, Action action)
    {
        // Set up properties
        Ability thunderStorm = caster.mySpellBook.GetAbilityByName("Thunder Storm");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, thunderStorm);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Thunder Storm");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInBlastRadius = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(caster, location, 1, true, false);

        // Pay energy cost
        OnAbilityUsedStart(thunderStorm, caster);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            bool critical = CombatLogic.Instance.RollForCritical(caster, entity, thunderStorm);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, entity, thunderStorm, damageType, critical, thunderStorm.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
            }

            // Deal Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, entity, damageType, thunderStorm);

            // Apply Poisoned
            if (entity.inDeathProcess == false)
            {
                entity.myPassiveManager.ModifyShocked(1);
            }
        }

        yield return new WaitForSeconds(0.5f);

        // Resolve
        OnAbilityUsedFinish(thunderStorm, caster);
        action.actionResolved = true;

    }

    // Primal Rage
    public Action PerformPrimalRage(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformPrimalRageCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformPrimalRageCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability primalRage = caster.mySpellBook.GetAbilityByName("Primal Rage");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Primal Rage");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Pay energy cost, + etc
        OnAbilityUsedStart(primalRage, caster);

        // Apply temporary strength
        target.myPassiveManager.ModifyTemporaryStrength(primalRage.abilityPrimaryValue);
        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(primalRage, caster);
        action.actionResolved = true;

    }

    // Concealing Clouds
    public Action PerformConcealingClouds(LivingEntity caster, Tile location)
    {
        Action action = new Action(true);
        StartCoroutine(PerformConcealingCloudsCoroutine(caster, location, action));
        return action;
    }
    private IEnumerator PerformConcealingCloudsCoroutine(LivingEntity caster, Tile location, Action action)
    {
        // Set up properties
        Ability concealingClouds = caster.mySpellBook.GetAbilityByName("Concealing Clouds");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Concealing Clouds");
            yield return new WaitForSeconds(0.5f);
        }

        // Play animation
        caster.PlaySkillAnimation();

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInBlastRadius = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(caster, location, 1, true, false);

        // Pay energy cost
        OnAbilityUsedStart(concealingClouds, caster);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            // Apply Camoflage
            entity.myPassiveManager.ModifyCamoflage(1);
        }

        yield return new WaitForSeconds(0.5f);

        // Resolve
        OnAbilityUsedFinish(concealingClouds, caster);
        action.actionResolved = true;
    }

    // Super Conductor
    public Action PerformSuperConductor(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformSuperConductorCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformSuperConductorCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability superConductor = caster.mySpellBook.GetAbilityByName("Super Conductor");
        int shotsToFire = caster.currentEnergy / 10;
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(caster, superConductor);

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Super Conductor");
            yield return new WaitForSeconds(0.5f);
        }

        // Ranged attack anim
        caster.PlaySkillAnimation();

        // Pay energy cost, + etc
        OnAbilityUsedStart(superConductor, caster);

        for (int shotsTaken = 0; shotsTaken < shotsToFire; shotsTaken++)
        {
            if (target.inDeathProcess == false)
            {
                // Set up shot values
                bool critical = CombatLogic.Instance.RollForCritical(caster, target, superConductor);
                bool dodge = CombatLogic.Instance.RollForDodge(target, caster);
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(caster, target, superConductor, damageType, critical, superConductor.abilityPrimaryValue);

                // TO DO: create lightning bolt VFX and replace fire ball here
                // Create fireball from prefab and play animation
                Action fireballHit = VisualEffectManager.Instance.ShootFireball(caster.tile.WorldPosition, target.tile.WorldPosition);

                // wait until fireball has hit the target
                yield return new WaitUntil(() => fireballHit.ActionResolved() == true);


                // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
                if (dodge)
                {
                    Action dodgeAction = CombatLogic.Instance.HandleDodge(caster, target);
                    yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
                }

                // if the target did not dodge, handle damage event normally
                else
                {
                    if (critical)
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "CRITICAL!");
                    }
                    Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, caster, target, damageType, superConductor);
                    yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
                }
            }

        }

        // remove camoflage, etc
        OnAbilityUsedFinish(superConductor, caster);
        action.actionResolved = true;

    }

    // Overload
    public Action PerformOverload(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformOverloadCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformOverloadCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Setup 
        Ability overload = caster.mySpellBook.GetAbilityByName("Overload");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Overload");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(overload, caster);

        // Play animation
        caster.PlaySkillAnimation();

        // Apply Temp Air imbuement
        target.myPassiveManager.ModifyTemporaryAirImbuement(1);
        yield return new WaitForSeconds(0.5f);

        OnAbilityUsedFinish(overload, caster);
        action.actionResolved = true;
    }


    #endregion



    // Old Abilities
    #region
        
    

    

    // Doom
    public Action PerformDoom(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformDoomCoroutine(caster, action));
        return action;
    }
    public IEnumerator PerformDoomCoroutine(LivingEntity caster, Action action)
    {
        Ability doom = caster.mySpellBook.GetAbilityByName("Doom");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Doom");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(doom, caster);

        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if(CombatLogic.Instance.IsTargetFriendly(caster, entity) == false)
            {
                entity.ModifyCurrentStamina(-1);
            }
        }

        OnAbilityUsedFinish(doom, caster);
        yield return new WaitForSeconds(1f);
        action.actionResolved = true;
    }

    // Empower Binding
    public Action PerformEmpowerBinding(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformEmpowerBindingCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformEmpowerBindingCoroutine(LivingEntity caster, Action action)
    {
        Ability empowerBinding = caster.mySpellBook.GetAbilityByName("Empower Binding");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Empower Binding");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(empowerBinding, caster);

        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (CombatLogic.Instance.IsTargetFriendly(caster, entity) &&
                entity.myPassiveManager.undead)
            {
                entity.myPassiveManager.ModifyBonusStrength(empowerBinding.abilityPrimaryValue);
            }
        }

        OnAbilityUsedFinish(empowerBinding, caster);
        yield return new WaitForSeconds(1f);
        action.actionResolved = true;
    }

    // Goblin War Cry
    public Action PerformGoblinWarCry(LivingEntity caster)
    {
        Action action = new Action(true);
        StartCoroutine(PerformGoblinWarCryCoroutine(caster, action));
        return action;
    }
    public IEnumerator PerformGoblinWarCryCoroutine(LivingEntity caster, Action action)
    {
        Ability gwc = caster.mySpellBook.GetAbilityByName("Goblin War Cry");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Goblin War Cry");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(gwc, caster);

        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (CombatLogic.Instance.IsTargetFriendly(caster, entity))
            {
                entity.myPassiveManager.ModifyBonusStrength(1);
            }
        }

        OnAbilityUsedFinish(gwc, caster);
        yield return new WaitForSeconds(1f);
        action.actionResolved = true;
    }

    // Crushing Blow
    public Action PerformCrushingBlow(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformCrushingBlowCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformCrushingBlowCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        Ability crushingBlow = caster.mySpellBook.GetAbilityByName("Crushing Blow");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Crushing Blow");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(crushingBlow, caster);
        caster.StartCoroutine(caster.PlayMeleeAttackAnimation(victim));

        victim.myPassiveManager.ModifyStunned(1);
        OnAbilityUsedFinish(crushingBlow, caster);
        yield return null;

        action.actionResolved = true;

    }

    // Summon Undead
    public Action PerformSummonUndead(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformSummonUndeadCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformSummonUndeadCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        Ability summonUndead = caster.mySpellBook.GetAbilityByName("Summon Undead");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Summon Undead");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(summonUndead, caster);
        List<Tile> allPossibleSpawnLocations = LevelManager.Instance.GetValidMoveableTilesWithinRange(summonUndead.abilityRange, caster.tile);
        List<Tile> finalList = new List<Tile>();

        // if target is to the left
        if (target.gridPosition.X <= caster.gridPosition.X)
        {
            foreach (Tile tile in allPossibleSpawnLocations)
            {
                if (tile.GridPosition.X >= target.gridPosition.X && tile.GridPosition.X <= caster.gridPosition.X)
                {
                    finalList.Add(tile);
                }
            }
        }

        // if target is to the right
        else if (target.gridPosition.X > caster.gridPosition.X)
        {
            foreach (Tile tile in allPossibleSpawnLocations)
            {
                if (tile.GridPosition.X <= target.gridPosition.X && tile.GridPosition.X >= caster.gridPosition.X)
                {
                    finalList.Add(tile);
                }
            }
        }

        // summon skeletons loop
        for (int skeletonsSummoned = 0; skeletonsSummoned < summonUndead.abilityPrimaryValue; skeletonsSummoned++)
        {
            Tile spawnLocation = LevelManager.Instance.GetClosestValidTile(finalList, target.tile);

            GameObject newSkeletonGO = Instantiate(PrefabHolder.Instance.ZombiePrefab);
            Enemy newSkeleton = newSkeletonGO.GetComponent<Enemy>();

            newSkeleton.InitializeSetup(spawnLocation.GridPosition, spawnLocation);
        }

        OnAbilityUsedFinish(summonUndead, caster);
        yield return new WaitForSeconds(1f);
        action.actionResolved = true;

    }

    // Summon Skeleton
    public Action PerformSummonSkeleton(LivingEntity caster, Tile spawnLocation)
    {
        Action action = new Action(true);
        StartCoroutine(PerformSummonSkeletonCoroutine(caster, spawnLocation, action));
        return action;
    }
    private IEnumerator PerformSummonSkeletonCoroutine(LivingEntity caster, Tile spawnLocation, Action action)
    {        
        Ability summonUndead = caster.mySpellBook.GetAbilityByName("Summon Skeleton");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Summon Skeleton");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(summonUndead, caster);

        // summon skeleton
        GameObject newSkeletonGO = Instantiate(PrefabHolder.Instance.skeletonSoldierPrefab);
        Enemy newSkeleton = newSkeletonGO.GetComponent<Enemy>();
        newSkeleton.InitializeSetup(spawnLocation.GridPosition, spawnLocation);
        
        // Resolve
        OnAbilityUsedFinish(summonUndead, caster);
        yield return new WaitForSeconds(1f);
        action.actionResolved = true;

    }

    // Summon Toxic Zombie
    public Action PerformSummonToxicZombie(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformSummonToxicZombieCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformSummonToxicZombieCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        Ability summonUndead = caster.mySpellBook.GetAbilityByName("Summon Toxic Zombie");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Summon Toxic Zombie");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(summonUndead, caster);
        List<Tile> allPossibleSpawnLocations = LevelManager.Instance.GetValidMoveableTilesWithinRange(summonUndead.abilityRange, caster.tile);
        List<Tile> finalList = new List<Tile>();

        // if target is to the left
        if (target.gridPosition.X <= caster.gridPosition.X)
        {
            foreach (Tile tile in allPossibleSpawnLocations)
            {
                if (tile.GridPosition.X >= target.gridPosition.X && tile.GridPosition.X <= caster.gridPosition.X)
                {
                    finalList.Add(tile);
                }
            }
        }

        // if target is to the right
        else if (target.gridPosition.X > caster.gridPosition.X)
        {
            foreach (Tile tile in allPossibleSpawnLocations)
            {
                if (tile.GridPosition.X <= target.gridPosition.X && tile.GridPosition.X >= caster.gridPosition.X)
                {
                    finalList.Add(tile);
                }
            }
        }

        // summon skeletons loop
        for (int skeletonsSummoned = 0; skeletonsSummoned < summonUndead.abilityPrimaryValue; skeletonsSummoned++)
        {
            Tile spawnLocation = LevelManager.Instance.GetClosestValidTile(finalList, target.tile);

            if (spawnLocation != null)
            {
                GameObject newSkeletonGO = Instantiate(PrefabHolder.Instance.toxicZombiePrefab);

                Enemy newSkeleton = newSkeletonGO.GetComponent<Enemy>();

                newSkeleton.InitializeSetup(spawnLocation.GridPosition, spawnLocation);
            }

        }

        OnAbilityUsedFinish(summonUndead, caster);
        yield return new WaitForSeconds(1f);
        action.actionResolved = true;

    }

    // Healing Light
    public Action PerformHealingLight(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action(true);
        StartCoroutine(PerformHealingLightCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformHealingLightCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Setup
        Ability healingLight = caster.mySpellBook.GetAbilityByName("Healing Light");

        // Status VFX notification for enemies
        if (caster.enemy)
        {
            VisualEffectManager.Instance.CreateStatusEffect(caster.transform.position, "Healing Light");
            yield return new WaitForSeconds(0.5f);
        }

        OnAbilityUsedStart(healingLight, caster);

        // Give target health
        target.ModifyCurrentHealth(healingLight.abilityPrimaryValue);
        //StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
        yield return new WaitForSeconds(0.5f);

        // Finish event
        OnAbilityUsedFinish(healingLight, caster);
        action.actionResolved = true;

    }
    #endregion

    

}
