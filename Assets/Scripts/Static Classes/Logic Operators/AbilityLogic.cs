using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        TileHover.Instance.SetVisibility(false);
        LevelManager.Instance.UnhighlightAllTiles();
        PathRenderer.Instance.DeactivatePathRenderer();

        // temp variables
        int finalApCost = ability.abilityEnergyCost;
        int finalCD = ability.abilityBaseCooldownTime;
       
        // Reduce AP by cost of the ability
        // check for preparation here
        if (entity.myPassiveManager.preparation && ability.abilityName != "Preparation" && 
            ability.abilityName != "Slice And Dice" &&
            ability.abilityName != "Rapid Fire")
        {
            entity.myPassiveManager.ModifyPreparation(-entity.myPassiveManager.preparationStacks);
            finalApCost = 0;
        }        

        // Check for Flux passive
        if(ability.abilityName == "Move")
        {
            // if character has a free move available
            if (entity.moveActionsTakenThisActivation == 0 && entity.myPassiveManager.flux)
            {
                VisualEffectManager.Instance.CreateStatusEffect(entity.transform.position, "Flux");
                finalApCost = 0;
            }
            entity.moveActionsTakenThisActivation++;
        }

        else if (ability.abilityName == "Slice And Dice" || ability.abilityName == "Rapid Fire")
        {
            finalApCost = entity.currentEnergy;
        }

        // Modify AP
        entity.ModifyCurrentEnergy(-finalApCost);
        // Modify Cooldown
        ability.ModifyCurrentCooldown(finalCD);


        if(ability.abilityType == AbilityDataSO.AbilityType.Power)
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

        if(ability.requiresMeleeWeapon == false &&
           ability.requiresRangedWeapon == false &&
           ability.requiresShield == false)
        {
            Debug.Log(ability.abilityName + " does not require a melee weapon, ranged weapon or shield...");
            boolReturned = true;
        }
        else if(ability.requiresMeleeWeapon == true &&
            (entity.myMainHandWeapon.itemType == ItemDataSO.ItemType.MeleeOneHand || 
            entity.myMainHandWeapon.itemType == ItemDataSO.ItemType.MeleeTwoHand)
            )
        {
            Debug.Log(ability.abilityName + " requires a melee weapon, and " + entity.name + " has one...");
            boolReturned = true;
        }
        else if(ability.requiresRangedWeapon &&
            entity.myMainHandWeapon.itemType == ItemDataSO.ItemType.RangedTwoHand)
        {
            Debug.Log(ability.abilityName + " requires a ranged weapon, and " + entity.name + " has one...");
            boolReturned = true;
        }
        else if(ability.requiresShield &&
            entity.myOffHandWeapon.itemType == ItemDataSO.ItemType.Shield)
        {
            Debug.Log(ability.abilityName + " requires a shield, and " + entity.name + " has one...");
            boolReturned = true;
        }
        else
        {
            Debug.Log(entity.name +" does not meet the weapon requirments of " + ability.abilityName);
            boolReturned = false;
        }        

        return boolReturned;
    }
    public string GetDamageTypeFromAbility(Ability ability)
    {
        Debug.Log("GetDamageTypeFromAbility() called on ability " + ability.abilityName +"...");

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
    #endregion

    // Powers Logic
    #region
    public void AddPowerToEntity(LivingEntity entity, Ability power)
    {
        Debug.Log("AbilityLogic.AddPowerToEntity() called, adding " + power.abilityName + " to " + entity.name);

        entity.activePowers.Insert(0,power);
        if(entity.activePowers.Count > entity.currentMaxPowersCount)
        {
            Debug.Log(entity.name + " has excedding its power limit, removing " + power.abilityName + " from active powers list...");
            RemovePowerFromEntity(entity, entity.activePowers.Last());
        }

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
    public Action PerformStrike(LivingEntity attacker, LivingEntity victim)
    {
        Debug.Log("AbilityLogic.PerformStrike() called. Caster = " + attacker.name + ", Target = " + victim.name);
        Action action = new Action();
        StartCoroutine(PerformStrikeCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformStrikeCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability strike = attacker.mySpellBook.GetAbilityByName("Strike");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, strike);
        bool parry = CombatLogic.Instance.RollForParry(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, strike, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, strike, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(strike, attacker);

        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, strike);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(strike, attacker);
        action.actionResolved = true;

    }

    // Defend
    public Action PerformDefend(LivingEntity caster)
    {
        Debug.Log("AbilityLogic.PerformDefend() called. Caster = " + caster.name);
        Action action = new Action();
        StartCoroutine(PerformDefendCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformDefendCoroutine(LivingEntity caster, Action action)
    {
        Ability block = caster.mySpellBook.GetAbilityByName("Defend");
        OnAbilityUsedStart(block, caster);
        caster.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(block.abilityPrimaryValue, caster));
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }

    // Move
    public Action PerformMove(LivingEntity characterMoved, Tile destination)
    {
        Debug.Log("AbilityLogic.PerformMove() called. Caster = " + characterMoved.name + 
            ", Target Tile = " + destination.GridPosition.X.ToString() + ", " + destination.GridPosition.Y.ToString());
        Action action = new Action();
        StartCoroutine(PerformMoveCoroutine(characterMoved, destination, action));
        return action;
    }
    private IEnumerator PerformMoveCoroutine(LivingEntity characterMoved, Tile destination, Action action)
    {
        Ability move = characterMoved.mySpellBook.GetAbilityByName("Move");
        OnAbilityUsedStart(move, characterMoved);
        Action movementAction = MovementLogic.Instance.MoveEntity(characterMoved, destination);
        yield return new WaitUntil(() => movementAction.ActionResolved() == true);
        action.actionResolved = true;
        characterMoved.myAnimator.SetTrigger("Idle");
    }

    // Shoot
    public Action PerformShoot(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformShootCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformShootCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability shoot = attacker.mySpellBook.GetAbilityByName("Shoot");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, shoot);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, shoot, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, shoot, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(shoot, attacker);

        // Ranged attack anim
        //attacker.PlayRangedAttackAnimation();
        //yield return new WaitUntil(() => attacker.myRangedAttackFinished == true);

        // Play arrow shot VFX
        Action shootAction = VisualEffectManager.Instance.ShootArrow(attacker.tile.WorldPosition, victim.tile.WorldPosition, 9);
        yield return new WaitUntil(() => shootAction.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(attacker, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, shoot);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(shoot, attacker);
        action.actionResolved = true;

    }

    // Free Strike
    public Action PerformFreeStrike(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformFreeStrikeCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformFreeStrikeCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Make sure character actually knows strike, has a melee weapon, and the target is not in death process
        if(attacker.mySpellBook.GetAbilityByName("Strike") != null &&
           victim.inDeathProcess == false &&
           (attacker.myMainHandWeapon.itemType == ItemDataSO.ItemType.MeleeOneHand || attacker.myMainHandWeapon.itemType == ItemDataSO.ItemType.MeleeTwoHand)
            )
        {
            // Set up properties
            Ability strike = attacker.mySpellBook.GetAbilityByName("Strike");
            bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, strike);
            //bool parry = CombatLogic.Instance.RollForParry(victim);
            string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, strike, attacker.myMainHandWeapon);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, strike, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

            // Play attack animation
            attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));

            // Check critical
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }

            // Deal damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, strike);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Remove camo
            if (attacker.myPassiveManager.camoflage)
            {
                attacker.myPassiveManager.ModifyCamoflage(-1);
            }

        }

        // Resolve
        action.actionResolved = true;

    }

    // Parry Attack
    public Action PerformRiposteAttack(LivingEntity attacker, LivingEntity victim)
    {
        Debug.Log("AbilityLogic.PerformRiposteAttack() called...");
        Action action = new Action();
        StartCoroutine(PerformRiposteAttackCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformRiposteAttackCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Make sure character actually knows strike, has a melee weapon, and the target is not in death process
        if (true
           // attacker.mySpellBook.GetAbilityByName("Strike") != null &&
          // victim.inDeathProcess == false &&
           //(attacker.myMainHandWeapon.itemType == ItemDataSO.ItemType.MeleeOneHand || attacker.myMainHandWeapon.itemType == ItemDataSO.ItemType.MeleeTwoHand)
            )
        {
            Debug.Log(attacker.name + " meets the requirments of a riposte attack, starting riposte process...");
            // Set up properties
            Ability strike = attacker.mySpellBook.GetAbilityByName("Strike");
            bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, strike);
            string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, strike, attacker.myMainHandWeapon);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, strike, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

            // Play attack animation
            attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));

            // Check critical
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }

            // Deal damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, strike);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Remove camo
            if (attacker.myPassiveManager.camoflage)
            {
                attacker.myPassiveManager.ModifyCamoflage(-1);
            }
            // Remove Sharpened Blade
            if (attacker.myPassiveManager.sharpenedBlade)
            {
                attacker.myPassiveManager.ModifySharpenedBlade(-attacker.myPassiveManager.sharpenedBladeStacks);
            }

        }

        // Resolve
        action.actionResolved = true;

    }

    // Over Watch Shot
    public Action PerformOverwatchShot(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformOverwatchShotCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformOverwatchShotCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Make sure character actually knows shoot, has a ranged weapon, and the target is not in death process
        if (attacker.mySpellBook.GetAbilityByName("Shoot") != null &&
            victim.inDeathProcess == false &&
            attacker.myMainHandWeapon.itemType == ItemDataSO.ItemType.RangedTwoHand
            )
        {
            // Set up properties
            Ability shoot = attacker.mySpellBook.GetAbilityByName("Shoot");
            bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, shoot);
            bool dodge = CombatLogic.Instance.RollForDodge(victim, attacker);
            string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, shoot, attacker.myMainHandWeapon);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, shoot, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

            // Remove Overwatch
            attacker.myPassiveManager.ModifyOverwatch(-attacker.myPassiveManager.overwatchStacks);

            // Play attack animation
            attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));

            // Play arrow shot VFX
            Action shootAction = VisualEffectManager.Instance.ShootArrow(attacker.tile.WorldPosition, victim.tile.WorldPosition, 9);
            yield return new WaitUntil(() => shootAction.ActionResolved() == true);

            // if the target successfully parried, dont do HandleDamage: do parry stuff instead
            if (dodge)
            {
                Action dodgeAction = CombatLogic.Instance.HandleDodge(attacker, victim);
                yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
            }

            // if the target did not dodge, handle damage event normally
            else
            {
                if (critical)
                {
                    VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
                }
                Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, shoot);
                yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
            }

            // Remove camo
            if (attacker.myPassiveManager.camoflage)
            {
                attacker.myPassiveManager.ModifyCamoflage(-1);
            }

        }

        // Resolve
        action.actionResolved = true;

    }
    #endregion

    // Brawler Abilities
    #region

    // Whirlwind
    public Action PerformWhirlwind(LivingEntity attacker)
    {
        Action action = new Action();
        StartCoroutine(PerformWhirlwindCoroutine(attacker, action));
        return action;
    }
    private IEnumerator PerformWhirlwindCoroutine(LivingEntity attacker, Action action)
    {
        // Set up properties
        Ability whirlwind = attacker.mySpellBook.GetAbilityByName("Whirlwind");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, whirlwind, attacker.myMainHandWeapon);

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInRange = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(attacker, attacker.tile, 1, false, true);

        // Pay energy cost
        OnAbilityUsedStart(whirlwind, attacker);

        // Create whirlwind VFX
        StartCoroutine(VisualEffectManager.Instance.CreateAoeMeleeAttackEffect(attacker.transform.position));
        attacker.myAnimator.SetTrigger("Melee Attack");

        // Resolve hits against targets
        foreach(LivingEntity entity in targetsInRange)
        {
            bool critical = CombatLogic.Instance.RollForCritical(attacker, entity, whirlwind);
            bool parry = CombatLogic.Instance.RollForParry(entity, attacker);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, entity, whirlwind, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

            // if the target successfully parried, dont do HandleDamage: do parry stuff instead
            if (parry)
            {
                Action parryAction = CombatLogic.Instance.HandleParry(attacker, entity);
            }

            // if the target did not parry, handle damage event normally
            else
            {
                if (critical)
                {
                    VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
                }

                Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, entity, damageType, whirlwind);
                //yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
            }

        }

        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }

    // Devastating Blow
    public Action PerformDevastatingBlow(LivingEntity attacker, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformDevastatingBlowCoroutine(attacker, target, action));
        return action;
    }
    private IEnumerator PerformDevastatingBlowCoroutine(LivingEntity attacker, LivingEntity target, Action action)
    {
        // Set up properties
        Ability devastatingBlow = attacker.mySpellBook.GetAbilityByName("Devastating Blow");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, target, devastatingBlow);
        bool parry = CombatLogic.Instance.RollForParry(target, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, devastatingBlow, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, target, devastatingBlow, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(devastatingBlow, attacker);

        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(target));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, target);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, target, damageType, devastatingBlow);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(devastatingBlow, attacker);
        action.actionResolved = true;
    }

    // Smash
    public Action PerformSmash(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformSmashCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformSmashCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability smash = attacker.mySpellBook.GetAbilityByName("Smash");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, smash);
        bool parry = CombatLogic.Instance.RollForParry(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, smash, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, smash, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(smash, attacker);

        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, smash);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Knock back.
            if(victim.inDeathProcess == false)
            {
                MovementLogic.Instance.KnockBackEntity(attacker, victim, smash.abilitySecondaryValue);
                yield return new WaitForSeconds(0.5f);
            }
           
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(smash, attacker);
        action.actionResolved = true;

    }

    // Charge
    public Action PerformCharge(LivingEntity caster, LivingEntity target, Tile destination)
    {
        Action action = new Action();
        StartCoroutine(PerformChargeCoroutine(caster, target, destination, action));
        return action;
    }
    private IEnumerator PerformChargeCoroutine(LivingEntity attacker, LivingEntity victim, Tile destination, Action action)
    {        
        // Set up properties
        Ability charge = attacker.mySpellBook.GetAbilityByName("Charge");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, charge);
        bool parry = CombatLogic.Instance.RollForParry(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, charge, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, charge, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(charge, attacker);

        // Charge movement
        Action moveAction = MovementLogic.Instance.MoveEntity(attacker, destination, 4);
        yield return new WaitUntil(() => moveAction.ActionResolved() == true);


        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, charge);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Apply vulnerable
            victim.myPassiveManager.ModifyVulnerable(charge.abilitySecondaryValue);
            attacker.myAnimator.SetTrigger("Idle");
            action.actionResolved = true;
        }        

        // remove camoflage, etc
        OnAbilityUsedFinish(charge, attacker);
        action.actionResolved = true;        

    }

    // Recklessness
    public Action PerformRecklessness(LivingEntity caster)
    {
        Action action = new Action();
        StartCoroutine(PerformRecklessnessCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformRecklessnessCoroutine(LivingEntity caster, Action action)
    {
        // Setup 
        Ability recklessness = caster.mySpellBook.GetAbilityByName("Recklessness");
        OnAbilityUsedStart(recklessness, caster);

        // Gain Recklessness
        caster.myPassiveManager.ModifyRecklessness(1);
        yield return new WaitForSeconds(0.5f);

        // Resolve
        OnAbilityUsedFinish(recklessness, caster);
        action.actionResolved = true;
    }

    // Kick To The Balls
    public Action PerformKickToTheBalls(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformKickToTheBallsCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformKickToTheBallsCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability kickToTheBalls = attacker.mySpellBook.GetAbilityByName("Kick To The Balls");
        bool parry = CombatLogic.Instance.RollForParry(victim, attacker);

        // Pay energy cost, + etc
        OnAbilityUsedStart(kickToTheBalls, attacker);

        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, stun the target
        else
        {
            victim.myPassiveManager.ModifyStunned(1);
            yield return new WaitForSeconds(0.5f);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(kickToTheBalls, attacker);
        action.actionResolved = true;

    }

    // Blade Flurry
    public Action PerformBladeFlurry(LivingEntity attacker)
    {
        Action action = new Action();
        StartCoroutine(PerformBladeFlurryCoroutine(attacker, action));
        return action;
    }
    private IEnumerator PerformBladeFlurryCoroutine(LivingEntity attacker, Action action)
    {
        // Set up properties
        Ability bladeFlurry = attacker.mySpellBook.GetAbilityByName("Blade Flurry");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, bladeFlurry, attacker.myMainHandWeapon);
        List<LivingEntity> targetsInRange = EntityLogic.GetAllEnemiesWithinRange(attacker, attacker.currentMeleeRange);
        List<LivingEntity> targetsHit = new List<LivingEntity>();

        // get a random target 3 times
        for (int i = 0; i < bladeFlurry.abilityPrimaryValue; i++)
        {
            targetsHit.Add(targetsInRange[Random.Range(0, targetsInRange.Count)]);
        }

        // Pay energy cost
        OnAbilityUsedStart(bladeFlurry, attacker);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsHit)
        {
            if(entity.inDeathProcess == false)
            {
                bool critical = CombatLogic.Instance.RollForCritical(attacker, entity, bladeFlurry);
                bool parry = CombatLogic.Instance.RollForParry(entity, attacker);
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, entity, bladeFlurry, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

                // Play attack animation
                attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(entity));

                // if the target successfully parried, dont do HandleDamage: do parry stuff instead
                if (parry)
                {
                    Action parryAction = CombatLogic.Instance.HandleParry(attacker, entity);
                    yield return new WaitUntil(() => parryAction.ActionResolved() == true);
                }

                // if the target did not parry, handle damage event normally
                else
                {
                    if (critical)
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
                    }
                    Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, entity, damageType, bladeFlurry);
                    yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
                }

            }
            
        }

        // Remove camo + etc
        OnAbilityUsedFinish(bladeFlurry, attacker);

        // Resolve/Complete event
        action.actionResolved = true;

    }

    #endregion

    // Duelist Abilities
    #region

    // Dash
    public Action PerformDash(LivingEntity characterMoved, Tile destination)
    {
        Action action = new Action();
        StartCoroutine(PerformDashCoroutine(characterMoved, destination, action));
        return action;
    }
    private IEnumerator PerformDashCoroutine(LivingEntity characterMoved, Tile destination, Action action)
    {
        Ability dash = characterMoved.mySpellBook.GetAbilityByName("Dash");

        OnAbilityUsedStart(dash, characterMoved);

        Action dashAction = MovementLogic.Instance.MoveEntity(characterMoved, destination, 4);

        yield return new WaitUntil(() => dashAction.ActionResolved() == true);

        characterMoved.myAnimator.SetTrigger("Idle");
        action.actionResolved = true;

    }

    // Tendon Slash
    public Action PerformTendonSlash(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformTendonSlashCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformTendonSlashCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability tendonSlash = attacker.mySpellBook.GetAbilityByName("Tendon Slash");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, tendonSlash);
        bool parry = CombatLogic.Instance.RollForParry(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, tendonSlash, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, tendonSlash, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(tendonSlash, attacker);

        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, tendonSlash);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Apply Weakened
            if (victim.inDeathProcess == false)
            {
                victim.myPassiveManager.ModifyWeakened(tendonSlash.abilityPrimaryValue);
                yield return new WaitForSeconds(0.5f);
            }
        }               

        // remove camoflage, etc
        OnAbilityUsedFinish(tendonSlash, attacker);
        action.actionResolved = true;
        
    }

    // Disarm
    public Action PerformDisarm(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformDisarmCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformDisarmCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability disarm = attacker.mySpellBook.GetAbilityByName("Disarm");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, disarm);
        bool parry = CombatLogic.Instance.RollForParry(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, disarm, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, disarm, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(disarm, attacker);

        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, disarm);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Disarm target
            if (victim.inDeathProcess == false)
            {
                victim.myPassiveManager.ModifyDisarmed(1);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(disarm, attacker);
        action.actionResolved = true;

    }

    // Shield Shatter
    public Action PerformShieldShatter(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformShieldShatterCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformShieldShatterCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability shieldShatter = attacker.mySpellBook.GetAbilityByName("Shield Shatter");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, shieldShatter);
        bool parry = CombatLogic.Instance.RollForParry(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, shieldShatter, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, shieldShatter, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(shieldShatter, attacker);

        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            // Remove all the targets block
            victim.ModifyCurrentBlock(-victim.currentBlock);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, shieldShatter);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(shieldShatter, attacker);
        action.actionResolved = true;

    }

    // Evasion
    public Action PerformEvasion(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformEvasionCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformEvasionCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability evasion = caster.mySpellBook.GetAbilityByName("Evasion");

        // Pay energy cost, + etc
        OnAbilityUsedStart(evasion, caster);

        // Apply temporary parry
        target.myPassiveManager.ModifyTemporaryParry(evasion.abilityPrimaryValue);
        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(evasion, caster);
        action.actionResolved = true;

    }

    // Decapitate
    public Action PerformDecapitate(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformDecapitateCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformDecapitateCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability decapitate = attacker.mySpellBook.GetAbilityByName("Decapitate");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, decapitate);
        bool parry = CombatLogic.Instance.RollForParry(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, decapitate, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, decapitate, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);
        bool instantKill = false;

        // If target has 20% or less health, they are killed instantly
        if((victim.currentMaxHealth * 0.2f) >= victim.currentHealth)
        {
            instantKill = true;
        }

        // Pay energy cost, + etc
        OnAbilityUsedStart(decapitate, attacker);

        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, decapitate);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            if(!victim.inDeathProcess && instantKill)
            {
                // the victim was insta killed, start death process
                VisualEffectManager.Instance.CreateStatusEffect(victim.transform.position, "DECAPITATED!");             
                victim.inDeathProcess = true;
                victim.StopAllCoroutines();
                StartCoroutine(victim.HandleDeath());
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(decapitate, attacker);
        action.actionResolved = true;

    }

    // Second Wind
    public Action PerformSecondWind(LivingEntity caster)
    {
        Action action = new Action();
        StartCoroutine(PerformSecondWindCoroutine(caster, action));
        return action;

    }
    private IEnumerator PerformSecondWindCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability vanish = caster.mySpellBook.GetAbilityByName("Second Wind");

        // Pay energy cost, + etc
        OnAbilityUsedStart(vanish, caster);

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
        Action action = new Action();
        StartCoroutine(PerformVanishCoroutine(caster, action));
        return action;

    }
    private IEnumerator PerformVanishCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability vanish = caster.mySpellBook.GetAbilityByName("Vanish");

        // Pay energy cost, + etc
        OnAbilityUsedStart(vanish, caster);

        // Apply temporary parry
        caster.myPassiveManager.ModifyCamoflage(1);
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(vanish, caster);
        action.actionResolved = true;

    }

    // Cheap Shot
    public Action PerformCheapShot(LivingEntity attacker, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformCheapShotCoroutine(attacker, target, action));
        return action;
    }
    private IEnumerator PerformCheapShotCoroutine(LivingEntity attacker, LivingEntity target, Action action)
    {
        // Set up properties
        Ability cheapShot = attacker.mySpellBook.GetAbilityByName("Cheap Shot");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, target, cheapShot);
        bool parry = CombatLogic.Instance.RollForParry(target, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, cheapShot, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, target, cheapShot, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(cheapShot, attacker);

        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(target));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, target);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, target, damageType, cheapShot);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Apply vulnerable if backstabbing
            if (PositionLogic.Instance.CanAttackerBackStrikeTarget(attacker, target))
            {
                target.myPassiveManager.ModifyVulnerable(1);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(cheapShot, attacker);
        action.actionResolved = true;
    }

    // Shank
    public Action PerformShank(LivingEntity attacker, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformShankCoroutine(attacker, target, action));
        return action;
    }
    private IEnumerator PerformShankCoroutine(LivingEntity attacker, LivingEntity target, Action action)
    {
        // Set up properties
        Ability shank = attacker.mySpellBook.GetAbilityByName("Shank");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, target, shank);
        bool parry = CombatLogic.Instance.RollForParry(target, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, shank, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, target, shank, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(shank, attacker);

        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(target));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, target);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, target, damageType, shank);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(shank, attacker);
        action.actionResolved = true;
    }

    // Rapid Cloaking
    public Action PerformRapidCloaking(LivingEntity caster)
    {
        Action action = new Action();
        StartCoroutine(PerformRapidCloakingCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformRapidCloakingCoroutine(LivingEntity caster, Action action)
    {
        // Setup 
        Ability rapidCloaking = caster.mySpellBook.GetAbilityByName("Rapid Cloaking");
        OnAbilityUsedStart(rapidCloaking, caster);

        // Gain Rapid Cloaking
        caster.myPassiveManager.ModifyRapidCloaking(1);
        yield return new WaitForSeconds(0.5f);

        OnAbilityUsedFinish(rapidCloaking, caster);
        action.actionResolved = true;
    }

    // Shadow Step
    public Action PerformShadowStep(LivingEntity caster, Tile destination)
    {
        Action action = new Action();
        StartCoroutine(PerformShadowStepCoroutine(caster, destination, action));
        return action;
    }
    private IEnumerator PerformShadowStepCoroutine(LivingEntity caster, Tile destination, Action action)
    {
        // Setup
        Ability blink = caster.mySpellBook.GetAbilityByName("Shadow Step");
        OnAbilityUsedStart(blink, caster);

        // teleport
        Action teleportAction = MovementLogic.Instance.TeleportEntity(caster, destination);
        yield return new WaitUntil(() => teleportAction.ActionResolved() == true);

        // Resolve
        OnAbilityUsedFinish(blink, caster);
        action.actionResolved = true;
    }

    // Ambush
    public Action PerformAmbush(LivingEntity attacker, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformAmbushCoroutine(attacker, target, action));
        return action;
    }
    private IEnumerator PerformAmbushCoroutine(LivingEntity attacker, LivingEntity target, Action action)
    {
        // Set up properties
        Ability ambush = attacker.mySpellBook.GetAbilityByName("Ambush");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, target, ambush);
        bool parry = CombatLogic.Instance.RollForParry(target, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, ambush, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, target, ambush, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(ambush, attacker);

        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(target));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, target);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, target, damageType, ambush);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Gain +20 energy if backstabbing
            if (PositionLogic.Instance.CanAttackerBackStrikeTarget(attacker, target))
            {
                attacker.ModifyCurrentEnergy(ambush.abilitySecondaryValue);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(ambush, attacker);
        action.actionResolved = true;
    }

    // Preparation
    public Action PerformPreparation(LivingEntity caster)
    {
        Action action = new Action();
        StartCoroutine(PerformPreparationCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformPreparationCoroutine(LivingEntity caster, Action action)
    {
        // Set up
        Ability preparation = caster.mySpellBook.GetAbilityByName("Preparation");
        OnAbilityUsedStart(preparation, caster);

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
        Action action = new Action();
        StartCoroutine(PerformSharpenBladeCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformSharpenBladeCoroutine(LivingEntity caster, Action action)
    {
        // Set up
        Ability preparation = caster.mySpellBook.GetAbilityByName("Sharpen Blade");
        OnAbilityUsedStart(preparation, caster);

        // Apply Sharpened Blade
        caster.myPassiveManager.ModifySharpenedBlade(1);
        yield return new WaitForSeconds(0.5f);

        // Resolve
        OnAbilityUsedFinish(preparation, caster);
        action.actionResolved = true;
    }

    // Slice And Dice
    public Action PerformSliceAndDice(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformSliceAndDiceCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformSliceAndDiceCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability sliceAndDice = attacker.mySpellBook.GetAbilityByName("Slice And Dice");
        int attacksToMake = attacker.currentEnergy / 10;
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, sliceAndDice, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(sliceAndDice, attacker);

        for (int attacksAlreadyMade = 0; attacksAlreadyMade < attacksToMake; attacksAlreadyMade++)
        {
            // Play attack animation
            attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));

            if (victim.inDeathProcess == false)
            {
                // Set up shot values
                bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, sliceAndDice);
                bool parry = CombatLogic.Instance.RollForParry(victim, attacker);
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, sliceAndDice, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

                
                // Play arrow shot VFX
                Action shootAction = VisualEffectManager.Instance.ShootArrow(attacker.tile.WorldPosition, victim.tile.WorldPosition, 9);
                yield return new WaitUntil(() => shootAction.ActionResolved() == true);

                // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
                if (parry)
                {
                    Action dodgeAction = CombatLogic.Instance.HandleParry(attacker, victim);
                    yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
                }

                // if the target did not dodge, handle damage event normally
                else
                {
                    if (critical)
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
                    }
                    Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, sliceAndDice);
                    yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
                }
            }

        }

        // remove camoflage, etc
        OnAbilityUsedFinish(sliceAndDice, attacker);
        action.actionResolved = true;

    }

    #endregion

    // Guardian Abilities
    #region

    // Guard
    public Action PerformGuard(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformGuardCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformGuardCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Setup
        Ability guard = caster.mySpellBook.GetAbilityByName("Guard");
        OnAbilityUsedStart(guard, caster);

        // Give target block
        target.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(guard.abilityPrimaryValue,caster));
        yield return new WaitForSeconds(0.5f);

        // Finish event
        OnAbilityUsedFinish(guard, caster);
        action.actionResolved = true;
    }

    // Provoke
    public Action PerformProvoke(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformProvokeCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformProvokeCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability provoke = caster.mySpellBook.GetAbilityByName("Provoke");

        // Pay energy cost, + etc
        OnAbilityUsedStart(provoke, caster);

        // Apply Taunted
        target.myPassiveManager.ModifyTaunted(1, caster);
        yield return new WaitForSeconds(0.5f);   

        // remove camoflage, etc
        OnAbilityUsedFinish(provoke, caster);
        action.actionResolved = true;

    }

    // Challenging Shout
    public Action PerformChallengingShout(LivingEntity attacker)
    {
        Action action = new Action();
        StartCoroutine(PerformChallengingShoutCoroutine(attacker, action));
        return action;
    }
    private IEnumerator PerformChallengingShoutCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability challengingShout = caster.mySpellBook.GetAbilityByName("Challenging Shout");

        // Calculate which characters are hit by the aoe taunt
        List<LivingEntity> targetsInRange = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(caster, caster.tile, 1, false, true);

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
    public Action PerformSwordAndBoard(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformSwordAndBoardCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformSwordAndBoardCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability swordAndBoard = attacker.mySpellBook.GetAbilityByName("Sword And Board");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, swordAndBoard);
        bool parry = CombatLogic.Instance.RollForParry(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, swordAndBoard, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, swordAndBoard, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(swordAndBoard, attacker);

        // Gain Block
        attacker.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(swordAndBoard.abilityPrimaryValue, attacker));

        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, swordAndBoard);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(swordAndBoard, attacker);
        action.actionResolved = true;

    }

    // Get Down!
    public Action PerformGetDown(LivingEntity caster, Tile destination)
    {
        Action action = new Action();
        StartCoroutine(PerformGetDownCoroutine(caster, destination, action));
        return action;
    }
    private IEnumerator PerformGetDownCoroutine(LivingEntity caster, Tile destination, Action action)
    {
        Ability getDown = caster.mySpellBook.GetAbilityByName("Get Down!");
        OnAbilityUsedStart(getDown, caster);
        Action moveAction = MovementLogic.Instance.MoveEntity(caster, destination, 4);

        // yield wait until movement complete
        yield return new WaitUntil(() => moveAction.ActionResolved() == true);

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
        caster.myAnimator.SetTrigger("Idle");
        action.actionResolved = true;
    }

    // Testudo
    public Action PerformTestudo(LivingEntity caster)
    {
        Action action = new Action();
        StartCoroutine(PerformTestudoCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformTestudoCoroutine(LivingEntity caster, Action action)
    {
        // Setup 
        Ability testudo = caster.mySpellBook.GetAbilityByName("Testudo");
        OnAbilityUsedStart(testudo, caster);

        // Gain Air imbuement
        caster.myPassiveManager.ModifyTestudo(1);
        yield return new WaitForSeconds(0.5f);

        OnAbilityUsedFinish(testudo, caster);
        action.actionResolved = true;
    }

    // Shield Slam
    public Action PerformShieldSlam(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformShieldSlamCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformShieldSlamCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability shieldSlam = attacker.mySpellBook.GetAbilityByName("Shield Slam");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, shieldSlam);
        bool parry = CombatLogic.Instance.RollForParry(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, shieldSlam, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, shieldSlam, damageType, critical, attacker.currentBlock);

        // Pay energy cost, + etc
        OnAbilityUsedStart(shieldSlam, attacker);

        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, victim);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, shieldSlam);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Knock back.
            if (victim.inDeathProcess == false)
            {
                MovementLogic.Instance.KnockBackEntity(attacker, victim, shieldSlam.abilityPrimaryValue);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(shieldSlam, attacker);
        action.actionResolved = true;

    }

    // Reactive Armour
    public Action PerformReactiveArmour(LivingEntity attacker)
    {
        Action action = new Action();
        StartCoroutine(PerformReactiveArmourCoroutine(attacker, action));
        return action;
    }
    private IEnumerator PerformReactiveArmourCoroutine(LivingEntity attacker, Action action)
    {
        // Set up properties
        Ability reactiveArmour = attacker.mySpellBook.GetAbilityByName("Reactive Armour");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, reactiveArmour, attacker.myMainHandWeapon);
        List<LivingEntity> targetsInRange = EntityLogic.GetAllEnemiesWithinRange(attacker, attacker.currentMeleeRange);
        int baseDamage = attacker.currentBlock;

        // Pay energy cost
        OnAbilityUsedStart(reactiveArmour, attacker);

        // Remove block
        attacker.ModifyCurrentBlock(-attacker.currentBlock);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsInRange)
        {
            bool critical = CombatLogic.Instance.RollForCritical(attacker, entity, reactiveArmour);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, entity, reactiveArmour, damageType, critical, baseDamage);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }

            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, entity, damageType, reactiveArmour);
        }        

        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(reactiveArmour, attacker);
        action.actionResolved = true;
    }


    #endregion

    // Pyromania Abilities
    #region

    // Fire Ball
    public Action PerformFireBall(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformFireBallCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformFireBallCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability fireball = attacker.mySpellBook.GetAbilityByName("Fire Ball");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, fireball);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, fireball);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, fireball, damageType, critical, fireball.abilityPrimaryValue);

        OnAbilityUsedStart(fireball, attacker);
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));
        yield return new WaitForSeconds(0.15f);

        // Create fireball from prefab and play animation
        Action fireballHit = VisualEffectManager.Instance.ShootFireball(attacker.tile.WorldPosition, victim.tile.WorldPosition);

        // wait until fireball has hit the target
        yield return new WaitUntil(() => fireballHit.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(attacker, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, fireball);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(fireball, attacker);
        action.actionResolved = true;
    }

    // Fire Nova
    public Action PerformFireNova(LivingEntity attacker)
    {
        Action action = new Action();
        StartCoroutine(PerformFireNovaCoroutine(attacker, action));
        return action;
    }
    private IEnumerator PerformFireNovaCoroutine(LivingEntity attacker, Action action)
    {
        // Set up properties
        Ability fireNova = attacker.mySpellBook.GetAbilityByName("Fire Nova");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, fireNova);
        List<LivingEntity> targetsInRange = EntityLogic.GetAllEnemiesWithinRange(attacker, fireNova.abilitySecondaryValue);

        // Pay energy cost
        OnAbilityUsedStart(fireNova, attacker);

        // Resolve damage against targets
        foreach (LivingEntity entity in targetsInRange)
        {
            bool critical = CombatLogic.Instance.RollForCritical(attacker, entity, fireNova);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, entity, fireNova, damageType, critical, fireNova.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            
            // Deal damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, entity, damageType, fireNova);

            // Apply burning
            if(entity.inDeathProcess == false)
            {
                entity.myPassiveManager.ModifyBurning(1);
            }
        }

        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(fireNova, attacker);
        action.actionResolved = true;
    }

    // Phoenix Dive
    public Action PerformPhoenixDive(LivingEntity caster, Tile destination)
    {
        Action action = new Action();
        StartCoroutine(PerformPhoenixDiveCoroutine(caster, destination, action));
        return action;
    }
    private IEnumerator PerformPhoenixDiveCoroutine(LivingEntity caster, Tile destination, Action action)
    {
        // Setup
        Ability phoenixDive = caster.mySpellBook.GetAbilityByName("Phoenix Dive");
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
    public Action PerformBlaze(LivingEntity caster)
    {
        Action action = new Action();
        StartCoroutine(PerformBlazeCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformBlazeCoroutine(LivingEntity caster, Action action)
    {
        // Setup 
        Ability blaze = caster.mySpellBook.GetAbilityByName("Blaze");
        OnAbilityUsedStart(blaze, caster);

        // Gain Fire imbuement
        caster.myPassiveManager.ModifyFireImbuement(1);
        yield return new WaitForSeconds(0.5f);

        OnAbilityUsedFinish(blaze, caster);
        action.actionResolved = true;
    }

    // Meteor
    public Action PerformMeteor(LivingEntity attacker, Tile location)
    {
        Action action = new Action();
        StartCoroutine(PerformMeteorCoroutine(attacker, location, action));
        return action;
    }
    private IEnumerator PerformMeteorCoroutine(LivingEntity attacker, Tile location, Action action)
    {
        // Set up properties
        Ability meteor = attacker.mySpellBook.GetAbilityByName("Meteor");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, meteor);

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInBlastRadius = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(attacker, location, 1, true, false);

        // Pay energy cost
        OnAbilityUsedStart(meteor, attacker);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            bool critical = CombatLogic.Instance.RollForCritical(attacker, entity, meteor);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, entity, meteor, damageType, critical, meteor.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }

            // Deal Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, entity, damageType, meteor);
            
            // Apply Burning
            if(entity.inDeathProcess == false)
            {
                entity.myPassiveManager.ModifyBurning(1, attacker);
            }
        }

        // Pay energy cost
        OnAbilityUsedFinish(meteor, attacker);

        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }

    // Combustion
    public Action PerformCombustion(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformCombustionCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformCombustionCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Setup
        Ability combustion = attacker.mySpellBook.GetAbilityByName("Combustion");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, combustion);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, combustion, damageType, false, victim.myPassiveManager.burningStacks * combustion.abilityPrimaryValue);
        OnAbilityUsedStart(combustion, attacker);

        // Deal Damage
        Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, combustion);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

        // Remove targets burning
        if (!victim.inDeathProcess)
        {
            victim.myPassiveManager.ModifyBurning(-victim.myPassiveManager.burningStacks);
        }
        // Resolve
        OnAbilityUsedFinish(combustion, attacker);
        action.actionResolved = true;
    }

    // Dragon Breath
    public Action PerformDragonBreath(LivingEntity attacker, Tile location)
    {
        Action action = new Action();
        StartCoroutine(PerformDragonBreathCoroutine(attacker, location, action));
        return action;
    }
    private IEnumerator PerformDragonBreathCoroutine(LivingEntity attacker, Tile location, Action action)
    {
        // Set up properties
        Ability dragonBreath = attacker.mySpellBook.GetAbilityByName("Dragon Breath");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, dragonBreath);
        List<Tile> tilesInBreathPath = LevelManager.Instance.GetAllTilesInALine(attacker.tile, location, dragonBreath.abilitySecondaryValue, true);
        List<LivingEntity> entitiesInBreathPath = new List<LivingEntity>();

        // Calculate which characters are hit by the breath
        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (tilesInBreathPath.Contains(entity.tile))
            {
                entitiesInBreathPath.Add(entity);
            }
        }

        // Pay energy cost
        OnAbilityUsedStart(dragonBreath, attacker);

        // Resolve hits against targets
        foreach (LivingEntity entity in entitiesInBreathPath)
        {
            bool critical = CombatLogic.Instance.RollForCritical(attacker, entity, dragonBreath);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, entity, dragonBreath, damageType, critical, dragonBreath.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }

            // Deal Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, entity, damageType, dragonBreath);
            
        }
        yield return new WaitForSeconds(0.5f);

        // Resolve
        OnAbilityUsedFinish(dragonBreath, attacker);        
        action.actionResolved = true;

    }


    #endregion

    // Cyromancy Abilities
    #region

    // Chilling Blow
    public Action PerformChillingBlow(LivingEntity attacker, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformChillingBlowCoroutine(attacker, target, action));
        return action;
    }
    private IEnumerator PerformChillingBlowCoroutine(LivingEntity attacker, LivingEntity target, Action action)
    {
        // Set up properties
        Ability chillingBlow = attacker.mySpellBook.GetAbilityByName("Chilling Blow");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, target, chillingBlow);
        bool parry = CombatLogic.Instance.RollForParry(target, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, chillingBlow, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, target, chillingBlow, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(chillingBlow, attacker);

        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(target));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, target);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, target, damageType, chillingBlow);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Apply Chilled
            if (target.inDeathProcess == false)
            {
                target.myPassiveManager.ModifyChilled(1);
                yield return new WaitForSeconds(0.5f);
            }
        }        

        // remove camoflage, etc
        OnAbilityUsedFinish(chillingBlow, attacker);
        action.actionResolved = true;
    }

    // Frost Nova
    public Action PerformFrostNova(LivingEntity attacker)
    {
        Action action = new Action();
        StartCoroutine(PerformFrostNovaCoroutine(attacker, action));
        return action;
    }
    private IEnumerator PerformFrostNovaCoroutine(LivingEntity attacker, Action action)
    {
        // Set up properties
        Ability frostNova = attacker.mySpellBook.GetAbilityByName("Frost Nova");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, frostNova);
        List<LivingEntity> targetsInRange = EntityLogic.GetAllEnemiesWithinRange(attacker, frostNova.abilitySecondaryValue);

        // Pay energy cost
        OnAbilityUsedStart(frostNova, attacker);

        // Resolve damage against targets
        foreach (LivingEntity entity in targetsInRange)
        {
            bool critical = CombatLogic.Instance.RollForCritical(attacker, entity, frostNova);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, entity, frostNova, damageType, critical, frostNova.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }

            // Deal damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, entity, damageType, frostNova);

            // Apply burning
            if (entity.inDeathProcess == false)
            {
                entity.myPassiveManager.ModifyChilled(1);
            }
        }

        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(frostNova, attacker);
        action.actionResolved = true;
    }

    // Icy Focus
    public Action PerformIcyFocus(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformIcyFocusCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformIcyFocusCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up
        Ability icyFocus = caster.mySpellBook.GetAbilityByName("Icy Focus");
        OnAbilityUsedStart(icyFocus, caster);

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
        Action action = new Action();
        StartCoroutine(PerformFrostBoltCoroutine(caster, victim, action));
        return action;
    }
    private IEnumerator PerformFrostBoltCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability frostBolt = attacker.mySpellBook.GetAbilityByName("Frost Bolt");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, frostBolt);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, frostBolt);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, frostBolt, damageType, critical, frostBolt.abilityPrimaryValue);

        OnAbilityUsedStart(frostBolt, attacker);
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));
        yield return new WaitForSeconds(0.15f);

        // Create frost bolt VFX
        Action frostBoltAction = VisualEffectManager.Instance.ShootFrostBolt(attacker.tile.WorldPosition, victim.tile.WorldPosition);
        yield return new WaitUntil(() => frostBoltAction.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(attacker, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }

            // Deal Damage event
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, frostBolt);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Apply Immobilizaed
            if(victim.inDeathProcess == false)
            {
                victim.myPassiveManager.ModifyImmobilized(1);
            }            
        }

        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(frostBolt, attacker);
        action.actionResolved = true;
        
    }

    // Blizzard
    public Action PerformBlizzard(LivingEntity attacker, Tile location)
    {
        Action action = new Action();
        StartCoroutine(PerformBlizzardCoroutine(attacker, location, action));
        return action;
    }
    private IEnumerator PerformBlizzardCoroutine(LivingEntity attacker, Tile location, Action action)
    {
        // Set up properties
        Ability blizzard = attacker.mySpellBook.GetAbilityByName("Blizzard");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, blizzard);

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInBlastRadius = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(attacker, location, 1, true, false);

        // Pay energy cost
        OnAbilityUsedStart(blizzard, attacker);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            bool critical = CombatLogic.Instance.RollForCritical(attacker, entity, blizzard);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, entity, blizzard, damageType, critical, blizzard.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }

            // Deal Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, entity, damageType, blizzard);

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
        Action action = new Action();
        StartCoroutine(PerformFrostArmourCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformFrostArmourCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Setup
        Ability frostArmour = caster.mySpellBook.GetAbilityByName("Frost Armour");
        OnAbilityUsedStart(frostArmour, caster);

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

    // Creeping Frost
    public Action PerformCreepingFrost(LivingEntity caster)
    {
        Action action = new Action();
        StartCoroutine(PerformCreepingFrostCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformCreepingFrostCoroutine(LivingEntity caster, Action action)
    {
        // Setup 
        Ability creepingFrost = caster.mySpellBook.GetAbilityByName("Creeping Frost");
        OnAbilityUsedStart(creepingFrost, caster);

        // Gain Frost imbuement
        caster.myPassiveManager.ModifyFrostImbuement(1);
        yield return new WaitForSeconds(0.5f);

        OnAbilityUsedFinish(creepingFrost, caster);
        action.actionResolved = true;
    }


    // Glacial Burst
    public Action PerformGlacialBurst(LivingEntity attacker)
    {
        Action action = new Action();
        StartCoroutine(PerformGlacialBurstCoroutine(attacker, action));
        return action;
    }
    private IEnumerator PerformGlacialBurstCoroutine(LivingEntity attacker, Action action)
    {
        // Set up properties
        Ability glacialBurst = attacker.mySpellBook.GetAbilityByName("Glacial Burst");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, glacialBurst);
        List<LivingEntity> targetsInRange = EntityLogic.GetAllEnemiesWithinRange(attacker, attacker.currentMeleeRange);
        List<LivingEntity> targetsHit = new List<LivingEntity>();

        // get a random target 3 times
        for (int i = 0; i < glacialBurst.abilitySecondaryValue; i++)
        {
            targetsHit.Add(targetsInRange[Random.Range(0, targetsInRange.Count)]);
        }

        // Pay energy cost
        OnAbilityUsedStart(glacialBurst, attacker);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsHit)
        {
            if (entity.inDeathProcess == false)
            {
                bool critical = CombatLogic.Instance.RollForCritical(attacker, entity, glacialBurst);
                bool dodge = CombatLogic.Instance.RollForDodge(entity, attacker);
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, entity, glacialBurst, damageType, critical, glacialBurst.abilityPrimaryValue);

                // Play attack animation
                attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(entity));

                // Create frost bolt VFX
                Action frostBoltAction = VisualEffectManager.Instance.ShootFrostBolt(attacker.tile.WorldPosition, entity.tile.WorldPosition);
                yield return new WaitUntil(() => frostBoltAction.ActionResolved() == true);

                // if the target successfully dodged, dont do HandleDamage: do dodge stuff instead
                if (dodge)
                {
                    Action dodgeAction = CombatLogic.Instance.HandleDodge(attacker, entity);
                    yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
                }

                // if the target did not parry, handle damage event normally
                else
                {
                    if (critical)
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
                    }
                    Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, entity, damageType, glacialBurst);
                    yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
                }

            }

        }

        // Remove camo + etc
        OnAbilityUsedFinish(glacialBurst, attacker);

        // Resolve/Complete event
        action.actionResolved = true;

    }

    // Thaw
    public Action PerformThaw(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformThawCoroutine(attacker, victim, action));
        return action;
    }
    public IEnumerator PerformThawCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability thaw = attacker.mySpellBook.GetAbilityByName("Thaw");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, thaw);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, thaw);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, thaw, damageType, critical, thaw.abilityPrimaryValue);

        OnAbilityUsedStart(thaw, attacker);
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));
        yield return new WaitForSeconds(0.15f);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(attacker, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, thaw);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Refund energy cost, if target is chilled
            if (victim.myPassiveManager.chilled)
            {
                attacker.ModifyCurrentEnergy(thaw.abilityEnergyCost);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(thaw, attacker);
        action.actionResolved = true;
    }
    #endregion

    // Ranger
    #region

    // Forest Medicine
    public Action PerformForestMedicine(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformForestMedicineCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformForestMedicineCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Setup
        Ability forestMedicine = caster.mySpellBook.GetAbilityByName("Forest Medicine");
        OnAbilityUsedStart(forestMedicine, caster);

        // Give target block
        target.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(forestMedicine.abilityPrimaryValue, caster));
        yield return new WaitForSeconds(0.5f);

        // Remove poisoned
        if (target.myPassiveManager.poisoned)
        {
            target.myPassiveManager.ModifyPoisoned(-target.myPassiveManager.poisonedStacks);
        }

        // Finish event
        OnAbilityUsedFinish(forestMedicine, caster);
        action.actionResolved = true;
    }

    // Snipe
    public Action PerformSnipe(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformSnipeCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformSnipeCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability snipe = attacker.mySpellBook.GetAbilityByName("Snipe");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, snipe);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, snipe, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, snipe, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(snipe, attacker);

        // Ranged attack anim
       // attacker.PlayRangedAttackAnimation();
        //yield return new WaitUntil(() => attacker.myRangedAttackFinished == true);

        // Play arrow shot VFX
        Action shootAction = VisualEffectManager.Instance.ShootArrow(attacker.tile.WorldPosition, victim.tile.WorldPosition, 9);
        yield return new WaitUntil(() => shootAction.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(attacker, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, snipe);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(snipe, attacker);
        action.actionResolved = true;

    }

    // Haste
    public Action PerformHaste(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformHasteCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformHasteCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up
        Ability haste = caster.mySpellBook.GetAbilityByName("Haste");
        OnAbilityUsedStart(haste, caster);

        // Give target mobility and mobility
        target.myPassiveManager.ModifyBonusMobility(haste.abilityPrimaryValue);
        target.myPassiveManager.ModifyBonusInitiative(haste.abilityPrimaryValue);
        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(target.transform.position));
        yield return new WaitForSeconds(0.5f);

        // Resolve Event
        OnAbilityUsedFinish(haste, caster);
        action.actionResolved = true;

    }

    // Steady Hands
    public Action PerformSteadyHands(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformSteadyHandsCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformSteadyHandsCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up
        Ability steadyHands = caster.mySpellBook.GetAbilityByName("Steady Hands");
        OnAbilityUsedStart(steadyHands, caster);

        // Give target bonus to ranged attacks
        target.myPassiveManager.ModifyTemporaryHawkEyeBonus(steadyHands.abilityPrimaryValue);
        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
        yield return new WaitForSeconds(0.5f);

        // Resolve Event
        OnAbilityUsedFinish(steadyHands, caster);
        action.actionResolved = true;

    }

    // Impaling Bolt
    public Action PerformImpalingBolt(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformImpalingBoltCoroutine(caster, victim, action));
        return action;
    }
    public IEnumerator PerformImpalingBoltCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability impalingBolt = attacker.mySpellBook.GetAbilityByName("Impaling Bolt");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, impalingBolt);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, impalingBolt, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, impalingBolt, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(impalingBolt, attacker);

        // Ranged attack anim
        //attacker.PlayRangedAttackAnimation();
        //yield return new WaitUntil(() => attacker.myRangedAttackFinished == true);

        // Play arrow shot VFX
        Action shootAction = VisualEffectManager.Instance.ShootArrow(attacker.tile.WorldPosition, victim.tile.WorldPosition, 9);
        yield return new WaitUntil(() => shootAction.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(attacker, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }

            // Knockback
            Action knockBackAction = MovementLogic.Instance.KnockBackEntity(attacker, victim, impalingBolt.abilitySecondaryValue);
            yield return new WaitUntil(() => knockBackAction.ActionResolved() == true);

            // Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, impalingBolt);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(impalingBolt, attacker);
        action.actionResolved = true;
    }    

    // Head Shot
    public Action PerformHeadShot(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformHeadShotCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformHeadShotCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability headShot = attacker.mySpellBook.GetAbilityByName("Head Shot");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, headShot);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, headShot, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, headShot, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(headShot, attacker);

        // Ranged attack anim
        //attacker.PlayRangedAttackAnimation();
        //yield return new WaitUntil(() => attacker.myRangedAttackFinished == true);

        // Play arrow shot VFX
        Action shootAction = VisualEffectManager.Instance.ShootArrow(attacker.tile.WorldPosition, victim.tile.WorldPosition, 9);
        yield return new WaitUntil(() => shootAction.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(attacker, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, headShot);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(headShot, attacker);
        action.actionResolved = true;

    }

    // Concentration
    public Action PerformConcentration(LivingEntity caster)
    {
        Action action = new Action();
        StartCoroutine(PerformConcentrationCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformConcentrationCoroutine(LivingEntity caster, Action action)
    {
        // Setup 
        Ability concentration = caster.mySpellBook.GetAbilityByName("Concentration");
        OnAbilityUsedStart(concentration, caster);

        // Gain Air imbuement
        caster.myPassiveManager.ModifyConcentration(1);
        yield return new WaitForSeconds(0.5f);

        OnAbilityUsedFinish(concentration, caster);
        action.actionResolved = true;
    }

    // Overwatch
    public Action PerformOverwatch(LivingEntity caster)
    {
        Action action = new Action();
        StartCoroutine(PerformOverwatchCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformOverwatchCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability overwatch = caster.mySpellBook.GetAbilityByName("Overwatch");

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
        Action action = new Action();
        StartCoroutine(PerformTreeLeapCoroutine(caster, destination, action));
        return action;
    }
    private IEnumerator PerformTreeLeapCoroutine(LivingEntity caster, Tile destination, Action action)
    {
        // Setup
        Ability treeLeap = caster.mySpellBook.GetAbilityByName("Tree Leap");
        OnAbilityUsedStart(treeLeap, caster);

        // Teleport
        Action teleportAction = MovementLogic.Instance.TeleportEntity(caster, destination);
        yield return new WaitUntil(() => teleportAction.ActionResolved() == true);

        // Resolve
        OnAbilityUsedFinish(treeLeap, caster);
        action.actionResolved = true;
    }

    // Rapid Fire
    public Action PerformRapidFire(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformRapidFireCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformRapidFireCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Set up properties
        Ability rapidFire = attacker.mySpellBook.GetAbilityByName("Rapid Fire");
        int shotsToFire = attacker.currentEnergy / 10;
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, rapidFire, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(rapidFire, attacker);

        for (int shotsTaken = 0; shotsTaken < shotsToFire; shotsTaken++)
        {
            if (victim.inDeathProcess == false)
            {
                // Set up shot values
                bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, rapidFire);
                bool dodge = CombatLogic.Instance.RollForDodge(victim, attacker);
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, rapidFire, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

                // Ranged attack anim
                //attacker.PlayRangedAttackAnimation();
                //yield return new WaitUntil(() => attacker.myRangedAttackFinished == true);

                // Play arrow shot VFX
                Action shootAction = VisualEffectManager.Instance.ShootArrow(attacker.tile.WorldPosition, victim.tile.WorldPosition, 9);
                yield return new WaitUntil(() => shootAction.ActionResolved() == true);

                // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
                if (dodge)
                {
                    Action dodgeAction = CombatLogic.Instance.HandleDodge(attacker, victim);
                    yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
                }

                // if the target did not dodge, handle damage event normally
                else
                {
                    if (critical)
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
                    }
                    Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, rapidFire);
                    yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
                }
            }

        }

        // remove camoflage, etc
        OnAbilityUsedFinish(rapidFire, attacker);
        action.actionResolved = true;

    }

    #endregion

    // Corruption
    #region

    // Blight
    public Action PerformBlight(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformBlightCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformBlightCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability blight = caster.mySpellBook.GetAbilityByName("Blight");

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
        Action action = new Action();
        StartCoroutine(PerformBloodOfferingCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformBloodOfferingCoroutine(LivingEntity caster, Action action)
    {
        // Set up properties
        Ability bloodOffering = caster.mySpellBook.GetAbilityByName("Blood Offering");

        // Pay energy cost, + etc
        OnAbilityUsedStart(bloodOffering, caster);

        // Gain Energy
        caster.ModifyCurrentEnergy(bloodOffering.abilityPrimaryValue);
        yield return new WaitForSeconds(0.5f);

        // Reduce Health
        Action selfDamageAction = CombatLogic.Instance.HandleDamage(bloodOffering.abilitySecondaryValue, caster, caster, "None", null, true);
        yield return new WaitUntil(() => selfDamageAction.ActionResolved() == true);        

        // remove camoflage, etc
        OnAbilityUsedFinish(bloodOffering, caster);
        action.actionResolved = true;

    }

    // Toxic Slash
    public Action PerformToxicSlash(LivingEntity attacker, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformToxicSlashCoroutine(attacker, target, action));
        return action;
    }
    private IEnumerator PerformToxicSlashCoroutine(LivingEntity attacker, LivingEntity target, Action action)
    {
        // Set up properties
        Ability toxicSlash = attacker.mySpellBook.GetAbilityByName("Toxic Slash");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, target, toxicSlash);
        bool parry = CombatLogic.Instance.RollForParry(target, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, toxicSlash, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, target, toxicSlash, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(toxicSlash, attacker);

        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(target));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, target);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, target, damageType, toxicSlash);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Apply poisoned
            if (target.inDeathProcess == false)
            {
                target.myPassiveManager.ModifyPoisoned(toxicSlash.abilityPrimaryValue, attacker);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(toxicSlash, attacker);
        action.actionResolved = true;
    }

    // Noxious Fumes
    public Action PerformNoxiousFumes(LivingEntity attacker)
    {
        Action action = new Action();
        StartCoroutine(PerformNoxiousFumesCoroutine(attacker, action));
        return action;
    }
    private IEnumerator PerformNoxiousFumesCoroutine(LivingEntity attacker, Action action)
    {
        // Set up properties
        Ability noxiousFumes = attacker.mySpellBook.GetAbilityByName("Noxious Fumes");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, noxiousFumes);
        List<LivingEntity> targetsInRange = EntityLogic.GetAllEnemiesWithinRange(attacker, noxiousFumes.abilitySecondaryValue);

        // Pay energy cost
        OnAbilityUsedStart(noxiousFumes, attacker);

        // Resolve damage against targets
        foreach (LivingEntity entity in targetsInRange)
        {
            bool critical = CombatLogic.Instance.RollForCritical(attacker, entity, noxiousFumes);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, entity, noxiousFumes, damageType, critical, noxiousFumes.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }

            // Deal damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, entity, damageType, noxiousFumes);

            // Apply poisoned
            if (entity.inDeathProcess == false)
            {
                entity.myPassiveManager.ModifyPoisoned(1, attacker);
            }
        }

        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(noxiousFumes, attacker);
        action.actionResolved = true;
    }

    // Toxic Eruption
    public Action PerformToxicEruption(LivingEntity attacker, Tile location)
    {
        Action action = new Action();
        StartCoroutine(PerformToxicEruptionCoroutine(attacker, location, action));
        return action;
    }
    private IEnumerator PerformToxicEruptionCoroutine(LivingEntity attacker, Tile location, Action action)
    {
        // Set up properties
        Ability toxicEruption = attacker.mySpellBook.GetAbilityByName("Toxic Eruption");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, toxicEruption);

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInBlastRadius = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(attacker, location, 1, true, false);

        // Pay energy cost
        OnAbilityUsedStart(toxicEruption, attacker);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            bool critical = CombatLogic.Instance.RollForCritical(attacker, entity, toxicEruption);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, entity, toxicEruption, damageType, critical, toxicEruption.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }

            // Deal Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, entity, damageType, toxicEruption);

            // Apply Poisoned
            if (entity.inDeathProcess == false)
            {
                entity.myPassiveManager.ModifyPoisoned(1, attacker);
            }
        }

        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }

    // Chemical Reaction
    public Action PerformChemicalReaction(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformChemicalReactionCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformChemicalReactionCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability blight = caster.mySpellBook.GetAbilityByName("Chemical Reaction");

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
    public Action PerformDrain(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformDrainCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformDrainCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Setup
        Ability drain = attacker.mySpellBook.GetAbilityByName("Drain");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, drain);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, drain, damageType, false, victim.myPassiveManager.poisonedStacks * 2);        
        OnAbilityUsedStart(drain, attacker);

        // Deal Damage
        Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, drain);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

        // Remove targets poison
        if (!victim.inDeathProcess)
        {
            victim.myPassiveManager.ModifyPoisoned(-victim.myPassiveManager.poisonedStacks);
        }
        // Resolve
        OnAbilityUsedFinish(drain, attacker);
        action.actionResolved = true;
    }



    #endregion

    // Manipulation
    #region

    // Telekinesis
    public Action PerformTelekinesis(LivingEntity caster, LivingEntity target, Tile destination)
    {
        Action action = new Action();
        StartCoroutine(PerformTelekinesisCoroutine(caster, target, destination, action));
        return action;
    }
    private IEnumerator PerformTelekinesisCoroutine(LivingEntity caster, LivingEntity target, Tile destination, Action action)
    {
        Ability telekinesis = caster.mySpellBook.GetAbilityByName("Telekinesis");
        OnAbilityUsedStart(telekinesis, caster);

        Action teleportAction = MovementLogic.Instance.TeleportEntity(target, destination);
        yield return new WaitUntil(() => teleportAction.ActionResolved() == true);

        OnAbilityUsedFinish(telekinesis, caster);
        action.actionResolved = true;

        yield return null;
    }

    // Dimensional Blast
    public Action PerformDimensionalBlast(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformDimensionalBlastCoroutine(attacker, victim, action));
        return action;
    }
    public IEnumerator PerformDimensionalBlastCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability dimensionalBlast = attacker.mySpellBook.GetAbilityByName("Dimensional Blast");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, dimensionalBlast);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, attacker);
        string damageType = CombatLogic.Instance.GetRandomDamageType();
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, dimensionalBlast, damageType, critical, dimensionalBlast.abilityPrimaryValue);

        OnAbilityUsedStart(dimensionalBlast, attacker);
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));
        yield return new WaitForSeconds(0.15f);

        // TO DO: should create a project relevant to the damage type generated randomly (e.g., fire creates a fireball, shadow creates a shadow bal, etc)

        // Create fireball from prefab and play animation
        Action fireballHit = VisualEffectManager.Instance.ShootFireball(attacker.tile.WorldPosition, victim.tile.WorldPosition);

        // wait until fireball has hit the target
        yield return new WaitUntil(() => fireballHit.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(attacker, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, dimensionalBlast);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(dimensionalBlast, attacker);
        action.actionResolved = true;
    }

    // Evasion
    public Action PerformMirage(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformMirageCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformMirageCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability mirage = caster.mySpellBook.GetAbilityByName("Mirage");

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
        Action action = new Action();
        StartCoroutine(PerformPhaseShiftCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformPhaseShiftCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Setup
        Ability phaseShift = caster.mySpellBook.GetAbilityByName("Phase Shift");
        Tile casterDestination = target.tile;
        Tile targetDestination = caster.tile;

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
        Action action = new Action();
        StartCoroutine(PerformBurstOfKnowledgeCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformBurstOfKnowledgeCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability burstOfKnowledge = caster.mySpellBook.GetAbilityByName("Burst Of Knowledge");

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
        Action action = new Action();
        StartCoroutine(PerformBlinkCoroutine(caster, destination, action));
        return action;
    }
    private IEnumerator PerformBlinkCoroutine(LivingEntity caster, Tile destination, Action action)
    {
        // Setup
        Ability blink = caster.mySpellBook.GetAbilityByName("Blink");
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
        Action action = new Action();
        StartCoroutine(PerformDimensionalHexCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformDimensionalHexCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability dimensionalHex = caster.mySpellBook.GetAbilityByName("Dimensional Hex");

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
        Action action = new Action();
        StartCoroutine(PerformInfuseCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformInfuseCoroutine(LivingEntity caster, Action action)
    {
        // Setup 
        Ability infuse = caster.mySpellBook.GetAbilityByName("Infuse");
        OnAbilityUsedStart(infuse, caster);

        // Gain Air imbuement
        caster.myPassiveManager.ModifyInfuse(1);
        yield return new WaitForSeconds(0.5f);

        OnAbilityUsedFinish(infuse, caster);
        action.actionResolved = true;
    }

    // Time Warp
    public Action PerformTimeWarp(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformTimeWarpCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformTimeWarpCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability timeWarp = caster.mySpellBook.GetAbilityByName("Time Warp");

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
        Action action = new Action();
        StartCoroutine(PerformHolyFireCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformHolyFireCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Setup
        Ability holyFire = caster.mySpellBook.GetAbilityByName("Holy Fire");
        OnAbilityUsedStart(holyFire, caster);

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
    public Action PerformBlindingLight(LivingEntity attacker, Tile location)
    {
        Action action = new Action();
        StartCoroutine(PerformBlindingLightCoroutine(attacker, location, action));
        return action;
    }
    private IEnumerator PerformBlindingLightCoroutine(LivingEntity attacker, Tile location, Action action)
    {
        // Set up properties
        Ability blindingLight = attacker.mySpellBook.GetAbilityByName("Blinding Light");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, blindingLight);

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInBlastRadius = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(attacker, location, 1, true, false);

        // Pay energy cost
        OnAbilityUsedStart(blindingLight, attacker);

        // Resolve damage against targets hit
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            bool critical = CombatLogic.Instance.RollForCritical(attacker, entity, blindingLight);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, entity, blindingLight, damageType, critical, blindingLight.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }

            // Deal Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, entity, damageType, blindingLight);            
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
        Action action = new Action();
        StartCoroutine(PerformInspireCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformInspireCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability inspire = caster.mySpellBook.GetAbilityByName("Inspire");

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
        Action action = new Action();
        StartCoroutine(PerformBlessCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformBlessCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability bless = caster.mySpellBook.GetAbilityByName("Bless");

        // Pay energy cost, + etc
        OnAbilityUsedStart(bless, caster);

        // Remove Weakened
        if (target.myPassiveManager.weakened)
        {
            target.myPassiveManager.ModifyWeakened(-target.myPassiveManager.weakenedStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Remove Vulnerable
        if (target.myPassiveManager.vulnerable)
        {
            target.myPassiveManager.ModifyVulnerable(-target.myPassiveManager.vulnerableStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Remove Stunned
        if (target.myPassiveManager.stunned)
        {
            target.myPassiveManager.ModifyStunned(-target.myPassiveManager.stunnedStacks);
            yield return new WaitForSeconds(0.5f);
        }

        // Resolve
        OnAbilityUsedFinish(bless, caster);
        action.actionResolved = true;

    }

    // Transcendence
    public Action PerformTranscendence(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformTranscendenceCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformTranscendenceCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability transcendence = caster.mySpellBook.GetAbilityByName("Transcendence");

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
    public Action PerformConsecrate(LivingEntity attacker)
    {
        Action action = new Action();
        StartCoroutine(PerformConsecrateCoroutine(attacker, action));
        return action;
    }
    private IEnumerator PerformConsecrateCoroutine(LivingEntity attacker, Action action)
    {
        // Set up properties
        Ability consecrate = attacker.mySpellBook.GetAbilityByName("Consecrate");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, consecrate);
        List<LivingEntity> targetsInRange = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(attacker, attacker.tile, attacker.currentMeleeRange, true, true);

        // Pay energy cost
        OnAbilityUsedStart(consecrate, attacker);

        // Resolve effects against all entities in range
        foreach (LivingEntity entity in targetsInRange)
        {
            // Create holy fire from prefab and play animation
            Action holyFireHit = VisualEffectManager.Instance.ShootHolyFire(entity.tile.WorldPosition);

            // Give energy to allies
            if (CombatLogic.Instance.IsTargetFriendly(attacker, entity))
            {
                entity.ModifyCurrentEnergy(consecrate.abilitySecondaryValue);
            }

            // Damage enemies
            else if (!CombatLogic.Instance.IsTargetFriendly(attacker, entity))
            {
                bool critical = CombatLogic.Instance.RollForCritical(attacker, entity, consecrate);
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, entity, consecrate, damageType, critical, consecrate.abilityPrimaryValue);

                if (critical)
                {
                    VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
                }

                // Deal damage
                Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, entity, damageType, consecrate);

            }

        }

        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(consecrate, attacker);
        action.actionResolved = true;
    }

    // Invigorate
    public Action PerformInvigorate(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformInvigorateCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformInvigorateCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability invigorate = caster.mySpellBook.GetAbilityByName("Invigorate");

        // Pay energy cost, + etc
        OnAbilityUsedStart(invigorate, caster);

        // Give bonus energy
        target.ModifyCurrentEnergy(invigorate.abilityPrimaryValue);
        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(target.transform.position));
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(invigorate, caster);
        action.actionResolved = true;

    }

    // Judgement
    public Action PerformJudgement(LivingEntity attacker, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformJudgementCoroutine(attacker, target, action));
        return action;
    }
    private IEnumerator PerformJudgementCoroutine(LivingEntity attacker, LivingEntity target, Action action)
    {
        // Set up properties
        Ability judgement = attacker.mySpellBook.GetAbilityByName("Judgement");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, target, judgement);
        bool parry = CombatLogic.Instance.RollForParry(target, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, judgement, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, target, judgement, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(judgement, attacker);

        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(target));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, target);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, target, damageType, judgement);
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
        OnAbilityUsedFinish(judgement, attacker);
        action.actionResolved = true;
    }

    // Purity
    public Action PerformPurity(LivingEntity caster)
    {
        Action action = new Action();
        StartCoroutine(PerformPurityCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformPurityCoroutine(LivingEntity caster, Action action)
    {
        // Setup 
        Ability purity = caster.mySpellBook.GetAbilityByName("Purity");
        OnAbilityUsedStart(purity, caster);

        // Gain Air imbuement
        caster.myPassiveManager.ModifyPurity(1);
        yield return new WaitForSeconds(0.5f);

        OnAbilityUsedFinish(purity, caster);
        action.actionResolved = true;
    }

    #endregion

    // Shadow Craft
    #region

    // Rain Of Chaos
    public Action PerformRainOfChaos(LivingEntity attacker, Tile location)
    {
        Action action = new Action();
        StartCoroutine(PerformRainOfChaosCoroutine(attacker, location, action));
        return action;
    }
    private IEnumerator PerformRainOfChaosCoroutine(LivingEntity attacker, Tile location, Action action)
    {
        // Set up properties
        Ability rainOfChaos = attacker.mySpellBook.GetAbilityByName("Rain Of Chaos");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, rainOfChaos);

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInBlastRadius = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(attacker, location, 1, true, false);

        // Pay energy cost
        OnAbilityUsedStart(rainOfChaos, attacker);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            bool critical = CombatLogic.Instance.RollForCritical(attacker, entity, rainOfChaos);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, entity, rainOfChaos, damageType, critical, rainOfChaos.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }

            // Deal Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, entity, damageType, rainOfChaos);

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
        Action action = new Action();
        StartCoroutine(PerformShroudCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformShroudCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability shroud = caster.mySpellBook.GetAbilityByName("Shroud");

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
        Action action = new Action();
        StartCoroutine(PerformHexCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformHexCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability hex = caster.mySpellBook.GetAbilityByName("Hex");

        // Pay energy cost, + etc
        OnAbilityUsedStart(hex, caster);

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

    // Chaos Bolt
    public Action PerformChaosBolt(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformChaosBoltCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformChaosBoltCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability chaosBolt = attacker.mySpellBook.GetAbilityByName("Chaos Bolt");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, chaosBolt);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, chaosBolt);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, chaosBolt, damageType, critical, chaosBolt.abilityPrimaryValue);

        OnAbilityUsedStart(chaosBolt, attacker);
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));
        yield return new WaitForSeconds(0.15f);

        // Shoot shadow ball
        Action shootAction = VisualEffectManager.Instance.ShootShadowBall(attacker.tile.WorldPosition, victim.tile.WorldPosition);
        yield return new WaitUntil(() => shootAction.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(attacker, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, chaosBolt);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            if (!victim.inDeathProcess)
            {
                victim.myPassiveManager.ModifyVulnerable(1);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(chaosBolt, attacker);
        action.actionResolved = true;
    }

    // Nightmare
    public Action PerformNightmare(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformNightmareCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformNightmareCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability nightmare = caster.mySpellBook.GetAbilityByName("Nightmare");

        // Pay energy cost, + etc
        OnAbilityUsedStart(nightmare, caster);

        // Apply Sleep
        target.myPassiveManager.ModifySleep(1);
        yield return new WaitForSeconds(0.5f);

        // remove camoflage, etc
        OnAbilityUsedFinish(nightmare, caster);
        action.actionResolved = true;

    }

    // Unbridled Chaos
    public Action PerformUnbridledChaos(LivingEntity attacker)
    {
        Action action = new Action();
        StartCoroutine(PerformUnbridledChaosCoroutine(attacker, action));
        return action;
    }
    private IEnumerator PerformUnbridledChaosCoroutine(LivingEntity attacker, Action action)
    {
        // Set up properties
        Ability unbridledChaos = attacker.mySpellBook.GetAbilityByName("Unbridled Chaos");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, unbridledChaos);
        List<LivingEntity> targetsInRange = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(attacker, attacker.tile, 3, true, false);
        List<LivingEntity> targetsHit = new List<LivingEntity>();

        // get a random target 6 times
        for (int i = 0; i < unbridledChaos.abilitySecondaryValue; i++)
        {
            targetsHit.Add(targetsInRange[Random.Range(0, targetsInRange.Count)]);
        }

        // Pay energy cost
        OnAbilityUsedStart(unbridledChaos, attacker);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsHit)
        {
            if (entity.inDeathProcess == false)
            {
                bool critical = CombatLogic.Instance.RollForCritical(attacker, entity, unbridledChaos);
                bool dodge = CombatLogic.Instance.RollForDodge(entity, attacker);
                int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, entity, unbridledChaos, damageType, critical, unbridledChaos.abilityPrimaryValue);

                // Play attack animation
                attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(entity));

                // Shoot shadow ball
                Action shootAction = VisualEffectManager.Instance.ShootShadowBall(attacker.tile.WorldPosition, entity.tile.WorldPosition);
                yield return new WaitUntil(() => shootAction.ActionResolved() == true);

                // if the target successfully dodged, dont do HandleDamage: do dodge stuff instead
                if (dodge)
                {
                    Action dodgeAction = CombatLogic.Instance.HandleDodge(attacker, entity);
                    yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
                }

                // if the target did not parry, handle damage event normally
                else
                {
                    if (critical)
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
                    }
                    Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, entity, damageType, unbridledChaos);
                    yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
                }

            }

        }

        // Remove camo + etc
        OnAbilityUsedFinish(unbridledChaos, attacker);

        // Resolve/Complete event
        action.actionResolved = true;

    }

    // Shadow Wreath
    public Action PerformShadowWreath(LivingEntity caster)
    {
        Action action = new Action();
        StartCoroutine(PerformShadowWreathCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformShadowWreathCoroutine(LivingEntity caster, Action action)
    {
        // Setup 
        Ability shadowWreath = caster.mySpellBook.GetAbilityByName("Shadow Wreath");
        OnAbilityUsedStart(shadowWreath, caster);

        // Gain Air imbuement
        caster.myPassiveManager.ModifyShadowImbuement(1);
        yield return new WaitForSeconds(0.5f);

        OnAbilityUsedFinish(shadowWreath, caster);
        action.actionResolved = true;
    }

    // Shadow Blast
    public Action PerformShadowBlast(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformShadowBlastCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformShadowBlastCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability shadowBlast = attacker.mySpellBook.GetAbilityByName("Shadow Blast");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, shadowBlast);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, shadowBlast);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, shadowBlast, damageType, critical, shadowBlast.abilityPrimaryValue);

        OnAbilityUsedStart(shadowBlast, attacker);
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));
        yield return new WaitForSeconds(0.15f);

        // Shoot shadow ball
        Action shootAction = VisualEffectManager.Instance.ShootShadowBall(attacker.tile.WorldPosition, victim.tile.WorldPosition);
        yield return new WaitUntil(() => shootAction.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(attacker, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, shadowBlast);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Knock back.
            if (victim.inDeathProcess == false)
            {
                MovementLogic.Instance.KnockBackEntity(attacker, victim, shadowBlast.abilitySecondaryValue);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(shadowBlast, attacker);
        action.actionResolved = true;
    }



    #endregion

    // Naturalist
    #region

    // Thunder Strike
    public Action PerformThunderStrike(LivingEntity attacker, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformThunderStrikeCoroutine(attacker, target, action));
        return action;
    }
    private IEnumerator PerformThunderStrikeCoroutine(LivingEntity attacker, LivingEntity target, Action action)
    {
        // Set up properties
        Ability thunderStrike = attacker.mySpellBook.GetAbilityByName("Thunder Strike");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, target, thunderStrike);
        bool parry = CombatLogic.Instance.RollForParry(target, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, thunderStrike, attacker.myMainHandWeapon);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, target, thunderStrike, damageType, critical, attacker.myMainHandWeapon.baseDamage, attacker.myMainHandWeapon);

        // Pay energy cost, + etc
        OnAbilityUsedStart(thunderStrike, attacker);

        // Play attack animation
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(target));

        // if the target successfully parried, dont do HandleDamage: do parry stuff instead
        if (parry)
        {
            Action parryAction = CombatLogic.Instance.HandleParry(attacker, target);
            yield return new WaitUntil(() => parryAction.ActionResolved() == true);
        }

        // if the target did not parry, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, target, damageType, thunderStrike);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Apply Shocked
            if (target.inDeathProcess == false)
            {
                target.myPassiveManager.ModifyShocked(1);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(thunderStrike, attacker);
        action.actionResolved = true;
    }

    // Lightning Bolt
    public Action PerformLightningBolt(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformLightningBoltCoroutine(attacker, victim, action));
        return action;
    }
    private IEnumerator PerformLightningBoltCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability lightningBolt = attacker.mySpellBook.GetAbilityByName("Lightning Bolt");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, lightningBolt);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, lightningBolt);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, lightningBolt, damageType, critical, lightningBolt.abilityPrimaryValue);

        OnAbilityUsedStart(lightningBolt, attacker);
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));
        yield return new WaitForSeconds(0.15f);

        // Create fireball from prefab and play animation
        Action fireballHit = VisualEffectManager.Instance.ShootFireball(attacker.tile.WorldPosition, victim.tile.WorldPosition);
        yield return new WaitUntil(() => fireballHit.ActionResolved() == true);

        // if the target successfully dodged dont do HandleDamage: do dodge stuff instead
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(attacker, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, lightningBolt);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            // Apply Shocked
            if (!victim.inDeathProcess)
            {
                victim.myPassiveManager.ModifyShocked(1);
            }
        }

        // remove camoflage, etc
        OnAbilityUsedFinish(lightningBolt, attacker);
        action.actionResolved = true;
    }

    // Spirit Surge
    public Action PerformSpiritSurge(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformSpiritSurgeCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformSpiritSurgeCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability spiritSurge = caster.mySpellBook.GetAbilityByName("Spirit Surge");

        // Start
        OnAbilityUsedStart(spiritSurge, caster);

        // Apply bonus strength
        target.myPassiveManager.ModifyBonusStrength(spiritSurge.abilityPrimaryValue);
        yield return new WaitForSeconds(0.3f);

        // Apply bonus wisdom
        target.myPassiveManager.ModifyBonusWisdom(spiritSurge.abilityPrimaryValue);
        yield return new WaitForSeconds(0.3f);

        // Apply bonus dexterity
        target.myPassiveManager.ModifyBonusDexterity(spiritSurge.abilityPrimaryValue);
        yield return new WaitForSeconds(0.3f);

        // Resolve
        OnAbilityUsedFinish(spiritSurge, caster);
        action.actionResolved = true;

    }

    // Spirit Vision
    public Action PerformSpiritVision(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformSpiritVisionCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformSpiritVisionCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability spiritSurge = caster.mySpellBook.GetAbilityByName("Spirit Vision");

        // Start
        OnAbilityUsedStart(spiritSurge, caster);

        // Apply temporary True Sight
        target.myPassiveManager.ModifyTemporaryTrueSight(1);
        yield return new WaitForSeconds(0.3f);

        // Resolve
        OnAbilityUsedFinish(spiritSurge, caster);
        action.actionResolved = true;

    }

    // Chain Lightning
    public Action PerformChainLightning(LivingEntity attacker, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformChainLightningCoroutine(attacker, target, action));
        return action;
    }
    private IEnumerator PerformChainLightningCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        // Setup
        Ability chainLightning = attacker.mySpellBook.GetAbilityByName("Chain Lightning");
        bool critical = CombatLogic.Instance.RollForCritical(attacker, victim, chainLightning);
        bool dodge = CombatLogic.Instance.RollForDodge(victim, attacker);
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, chainLightning);
        int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, victim, chainLightning, damageType, critical, chainLightning.abilityPrimaryValue);
        LivingEntity currentTarget = victim;
        LivingEntity previousTarget = victim;

        OnAbilityUsedStart(chainLightning, attacker);

        // Resolve attack against the first target
        if (dodge)
        {
            Action dodgeAction = CombatLogic.Instance.HandleDodge(attacker, victim);
            yield return new WaitUntil(() => dodgeAction.ActionResolved() == true);
        }

        // if the target did not dodge, handle damage event normally
        else
        {
            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }

            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, victim, damageType, chainLightning);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

            for (int lightningJumps = 0; lightningJumps < chainLightning.abilitySecondaryValue; lightningJumps++)
            {
                List<Tile> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, currentTarget.tile);

                foreach (LivingEntity enemy in LivingEntityManager.Instance.allLivingEntities)
                {
                    if (adjacentTiles.Contains(enemy.tile) && CombatLogic.Instance.IsTargetFriendly(attacker, enemy) == false)
                    {
                        previousTarget = currentTarget;
                        currentTarget = enemy;
                    }
                }

                if (previousTarget != currentTarget)
                {
                    bool critical2 = CombatLogic.Instance.RollForCritical(attacker, victim, chainLightning);
                    int finalDamageValue2 = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, currentTarget, chainLightning, damageType, critical2, chainLightning.abilityPrimaryValue);

                    if (critical2)
                    {
                        VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
                    }

                    Action abilityAction2 = CombatLogic.Instance.HandleDamage(finalDamageValue2, attacker, currentTarget, damageType, chainLightning);
                    yield return new WaitUntil(() => abilityAction2.ActionResolved() == true);
                    yield return new WaitForSeconds(0.2f);
                }

            }

        }

        // Resolve
        OnAbilityUsedFinish(chainLightning, attacker);
        action.actionResolved = true;

    }

    // Thunder Storm
    public Action PerformThunderStorm(LivingEntity attacker, Tile location)
    {
        Action action = new Action();
        StartCoroutine(PerformThunderStormCoroutine(attacker, location, action));
        return action;
    }
    private IEnumerator PerformThunderStormCoroutine(LivingEntity attacker, Tile location, Action action)
    {
        // Set up properties
        Ability thunderStorm = attacker.mySpellBook.GetAbilityByName("Thunder Storm");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, thunderStorm);

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInBlastRadius = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(attacker, location, 1, true, false);

        // Pay energy cost
        OnAbilityUsedStart(thunderStorm, attacker);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            bool critical = CombatLogic.Instance.RollForCritical(attacker, entity, thunderStorm);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, entity, thunderStorm, damageType, critical, thunderStorm.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }

            // Deal Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, entity, damageType, thunderStorm);

            // Apply Poisoned
            if (entity.inDeathProcess == false)
            {
                entity.myPassiveManager.ModifyShocked(1);
            }
        }

        yield return new WaitForSeconds(0.5f);

        // Resolve
        OnAbilityUsedFinish(thunderStorm, attacker);
        action.actionResolved = true;

    }

    // Primal Rage
    public Action PerformPrimalRage(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformPrimalRageCoroutine(caster, target, action));
        return action;
    }
    private IEnumerator PerformPrimalRageCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        // Set up properties
        Ability primalRage = caster.mySpellBook.GetAbilityByName("Primal Rage");

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
    public Action PerformConcealingClouds(LivingEntity attacker, Tile location)
    {
        Action action = new Action();
        StartCoroutine(PerformConcealingCloudsCoroutine(attacker, location, action));
        return action;
    }
    private IEnumerator PerformConcealingCloudsCoroutine(LivingEntity attacker, Tile location, Action action)
    {
        // Set up properties
        Ability concealingClouds = attacker.mySpellBook.GetAbilityByName("Concealing Clouds");

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInBlastRadius = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(attacker, location, 1, true, false);

        // Pay energy cost
        OnAbilityUsedStart(concealingClouds, attacker);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            // Apply Camoflage
            entity.myPassiveManager.ModifyCamoflage(1);
        }

        yield return new WaitForSeconds(0.5f);

        // Resolve
        OnAbilityUsedFinish(concealingClouds, attacker);
        action.actionResolved = true;
    }

    // Super Conductor
    public Action PerformSuperConductor(LivingEntity attacker)
    {
        Action action = new Action();
        StartCoroutine(PerformSuperConductorCoroutine(attacker, action));
        return action;
    }
    private IEnumerator PerformSuperConductorCoroutine(LivingEntity attacker, Action action)
    {
        // Set up properties
        Ability superConductor = attacker.mySpellBook.GetAbilityByName("Super Conductor");
        string damageType = CombatLogic.Instance.CalculateFinalDamageTypeOfAttack(attacker, superConductor);

        // Calculate which characters are hit by the aoe
        List<LivingEntity> targetsInBlastRadius = CombatLogic.Instance.GetAllLivingEntitiesWithinAoeEffect(attacker, attacker.tile, 1, true, true);

        // Pay energy cost
        OnAbilityUsedStart(superConductor, attacker);

        // Resolve hits against targets
        foreach (LivingEntity entity in targetsInBlastRadius)
        {
            bool critical = CombatLogic.Instance.RollForCritical(attacker, entity, superConductor);
            int finalDamageValue = CombatLogic.Instance.GetFinalDamageValueAfterAllCalculations(attacker, entity, superConductor, damageType, critical, superConductor.abilityPrimaryValue);

            if (critical)
            {
                VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "CRITICAL!");
            }

            // Deal Damage
            Action abilityAction = CombatLogic.Instance.HandleDamage(finalDamageValue, attacker, entity, damageType, superConductor);

            // Apply Shocked or Stun
            if (entity.inDeathProcess == false)
            {
                // Apply Stun
                if (entity.myPassiveManager.shocked)
                {
                    entity.myPassiveManager.ModifyStunned(1);
                }

                // Apply Shocked
                else
                {
                    entity.myPassiveManager.ModifyShocked(1);
                }
                
            }
        }

        yield return new WaitForSeconds(0.5f);

        // Resolve
        OnAbilityUsedFinish(superConductor, attacker);
        action.actionResolved = true;
    }

    // Over Load
    public Action PerformOverload(LivingEntity caster)
    {
        Action action = new Action();
        StartCoroutine(PerformOverloadCoroutine(caster, action));
        return action;
    }
    private IEnumerator PerformOverloadCoroutine(LivingEntity caster, Action action)
    {
        // Setup 
        Ability overload = caster.mySpellBook.GetAbilityByName("Overload");
        OnAbilityUsedStart(overload, caster);

        // Gain Air imbuement
        caster.myPassiveManager.ModifyAirImbuement(1);
        yield return new WaitForSeconds(0.5f);

        OnAbilityUsedFinish(overload, caster);
        action.actionResolved = true;
    }


    #endregion



    // Old Abilities
    #region
    
   


    

    //Rend
    public Action PerformRend(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformRendCoroutine(attacker, victim, action));
        return action;
    }
    public IEnumerator PerformRendCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability rend = attacker.mySpellBook.GetAbilityByName("Rend");
        OnAbilityUsedStart(rend, attacker);
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));
        //Action abilityAction = CombatLogic.Instance.HandleDamage(rend.abilityPrimaryValue, attacker, victim, false, rend.abilityAttackType, rend.abilityDamageType);
       // yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        victim.myPassiveManager.ModifyWeakened(rend.abilitySecondaryValue);
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }

   
    // Twin Strike
    public Action PerformTwinStrike(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformTwinStrikeCoroutine(attacker, victim, action));
        return action;
    }
    public IEnumerator PerformTwinStrikeCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability twinStrike = attacker.mySpellBook.GetAbilityByName("Twin Strike");
        OnAbilityUsedStart(twinStrike, attacker);
        StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));
        //Action abilityAction = CombatLogic.Instance.HandleDamage(twinStrike.abilityPrimaryValue, attacker, victim, false, twinStrike.abilityAttackType, twinStrike.abilityDamageType);
        //yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        yield return new WaitForSeconds(0.3f);

        // check to make sure the target is still valid for the second attack
        if (victim.inDeathProcess == false && 
            EntityLogic.IsTargetInRange(attacker, victim, attacker.currentMeleeRange))
        {
            StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));
            //Action abilityAction2 = CombatLogic.Instance.HandleDamage(twinStrike.abilityPrimaryValue, attacker, victim, false, twinStrike.abilityAttackType, twinStrike.abilityDamageType);
            //yield return new WaitUntil(() => abilityAction2.ActionResolved() == true);
        }

        action.actionResolved = true;
    }

    // Mork Smash
    public Action PerformMorkSmash(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformMorkSmashCoroutine(attacker, victim, action));
        return action;
    }
    public IEnumerator PerformMorkSmashCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability morkSmash = attacker.mySpellBook.GetAbilityByName("Mork Smash!");
        OnAbilityUsedStart(morkSmash, attacker);
        StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));
        //Action abilityAction = CombatLogic.Instance.HandleDamage(morkSmash.abilityPrimaryValue, attacker, victim, false, morkSmash.abilityAttackType, morkSmash.abilityDamageType);
        //yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

        // Knock back.
        Action knockBackAction = MovementLogic.Instance.KnockBackEntity(attacker, victim, morkSmash.abilitySecondaryValue);
        yield return new WaitUntil(() => knockBackAction.ActionResolved() == true);        
        action.actionResolved = true;
    }
   
    

  

    

    

   



    

    


   

    

    

    

    // Poison Dart
    public Action PerformPoisonDart(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformPoisonDartCoroutine(caster, victim, action));
        return action;
    }
    public IEnumerator PerformPoisonDartCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        Ability poisonDart = caster.mySpellBook.GetAbilityByName("Poison Dart");
        OnAbilityUsedStart(poisonDart, caster);
        StartCoroutine(caster.PlayMeleeAttackAnimation(victim));        
        victim.myPassiveManager.ModifyPoisoned(poisonDart.abilitySecondaryValue, caster);
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;
        
    }

    

    // Acid Spit
    public Action PerformAcidSpit(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformAcidSpitCoroutine(caster, victim, action));
        return action;
    }
    public IEnumerator PerformAcidSpitCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        Ability acidSpit = caster.mySpellBook.GetAbilityByName("Acid Spit");
        OnAbilityUsedStart(acidSpit, caster);
        StartCoroutine(caster.PlayMeleeAttackAnimation(victim));
        //Action abilityAction = CombatLogic.Instance.HandleDamage(acidSpit.abilityPrimaryValue, caster, victim, false, acidSpit.abilityAttackType, acidSpit.abilityDamageType, acidSpit);        
        //yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        victim.myPassiveManager.ModifyPoisoned(acidSpit.abilitySecondaryValue, caster);
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }

    
    

    

    // Rock Toss
    public Action PerformRockToss(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformRockTossCoroutine(caster, victim, action));
        return action;
    }
    public IEnumerator PerformRockTossCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability rockToss = attacker.mySpellBook.GetAbilityByName("Rock Toss");
        OnAbilityUsedStart(rockToss, attacker);

        // Attack
        StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));
        //Action abilityAction = CombatLogic.Instance.HandleDamage(rockToss.abilityPrimaryValue, attacker, victim, false, rockToss.abilityAttackType, rockToss.abilityDamageType);
        //yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        // Knockback
        MovementLogic.Instance.KnockBackEntity(attacker, victim, rockToss.abilitySecondaryValue);
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;
        
    }

   

    // Lightning Shield
    public Action PerformLightningShield(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformLightningShieldCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformLightningShieldCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        Ability lightningShield = caster.mySpellBook.GetAbilityByName("Lightning Shield");
        OnAbilityUsedStart(lightningShield, caster);
        target.myPassiveManager.ModifyLightningShield(lightningShield.abilityPrimaryValue);
        VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Lightning Shield");
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;
        
    }

    

    

    

    // Siphon Life
    public Action PerformSiphonLife(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformSiphonLifeCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformSiphonLifeCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        Ability siphonLife = caster.mySpellBook.GetAbilityByName("Siphon Life");
        OnAbilityUsedStart(siphonLife, caster);
        target.ModifyCurrentStrength(-siphonLife.abilityPrimaryValue);
        caster.ModifyCurrentStrength(siphonLife.abilityPrimaryValue);
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;
        
    }

    // Sanctity
    public Action PerformSanctity(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformSanctityCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformSanctityCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        Ability sanctity = caster.mySpellBook.GetAbilityByName("Sanctity");
        OnAbilityUsedStart(sanctity, caster);

        if (target.myPassiveManager.stunned)
        {
            VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Stun Removed");
            yield return new WaitForSeconds(0.2f);
            target.myPassiveManager.ModifyStunned(-target.myPassiveManager.stunnedStacks);
        }
        if (target.myPassiveManager.immobilized)
        {
            VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Immobilized Removed");
            yield return new WaitForSeconds(0.2f);
            target.myPassiveManager.ModifyImmobilized(-target.myPassiveManager.immobilizedStacks);
        }
        if (target.myPassiveManager.poisoned)
        {
            VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Poison Removed");
            yield return new WaitForSeconds(0.2f);
            target.myPassiveManager.ModifyPoisoned(-target.myPassiveManager.poisonedStacks);
        }
        // remove vulnerable
        // remove weakened

        action.actionResolved = true;
        
    }


    // Void Bomb
    public Action PerformVoidBomb(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformVoidBombCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformVoidBombCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability voidBomb = attacker.mySpellBook.GetAbilityByName("Void Bomb");
        OnAbilityUsedStart(voidBomb, attacker);
        //Action abilityAction = CombatLogic.Instance.HandleDamage(voidBomb.abilityPrimaryValue, attacker, victim, false, voidBomb.abilityAttackType, voidBomb.abilityDamageType);
       // yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        victim.myPassiveManager.ModifyStunned(1);
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;
        

    }

     

    

   

    // Teleport
    public Action PerformTeleport(LivingEntity caster, Tile destination)
    {
        Action action = new Action();
        StartCoroutine(PerformTeleportCoroutine(caster, destination, action));
        return action;
    }
    public IEnumerator PerformTeleportCoroutine(LivingEntity caster, Tile destination, Action action)
    {
        Ability teleport = caster.mySpellBook.GetAbilityByName("Teleport");
        OnAbilityUsedStart(teleport, caster);

        MovementLogic.Instance.TeleportEntity(caster, destination);

        action.actionResolved = true;
        

        yield return null;
    }

    // Doom
    public Action PerformDoom(LivingEntity caster)
    {
        Action action = new Action();
        StartCoroutine(PerformDoomCoroutine(caster, action));
        return action;
    }
    public IEnumerator PerformDoomCoroutine(LivingEntity caster, Action action)
    {
        Ability doom = caster.mySpellBook.GetAbilityByName("Doom");
        OnAbilityUsedStart(doom, caster);
        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if(CombatLogic.Instance.IsTargetFriendly(caster, entity) == false)
            {
                entity.ModifyCurrentStamina(-1);
            }
        }
        yield return new WaitForSeconds(1f);
        action.actionResolved = true;
    }

    // Empower Binding
    public Action PerformEmpowerBinding(LivingEntity caster)
    {
        Action action = new Action();
        StartCoroutine(PerformEmpowerBindingCoroutine(caster, action));
        return action;
    }
    public IEnumerator PerformEmpowerBindingCoroutine(LivingEntity caster, Action action)
    {
        Ability empowerBinding = caster.mySpellBook.GetAbilityByName("Empower Binding");
        OnAbilityUsedStart(empowerBinding, caster);
        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (CombatLogic.Instance.IsTargetFriendly(caster, entity) &&
                entity.myPassiveManager.undead)
            {
                entity.ModifyCurrentStrength(1);
            }
        }
        yield return new WaitForSeconds(1f);
        action.actionResolved = true;
    }

    // Crushing Blow
    public Action PerformCrushingBlow(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformCrushingBlowCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformCrushingBlowCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability crushingBlow = attacker.mySpellBook.GetAbilityByName("Crushing Blow");
        OnAbilityUsedStart(crushingBlow, attacker);
        attacker.StartCoroutine(attacker.PlayMeleeAttackAnimation(victim));

       // Action abilityAction = CombatLogic.Instance.HandleDamage(crushingBlow.abilityPrimaryValue, attacker, victim, false, crushingBlow.abilityAttackType, crushingBlow.abilityDamageType);
        //yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

        victim.myPassiveManager.ModifyStunned(1);
        yield return null;

        action.actionResolved = true;

    }

    // Summon Undead
    public Action PerformSummonUndead(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformSummonUndeadCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformSummonUndeadCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        Ability summonUndead = caster.mySpellBook.GetAbilityByName("Summon Undead");
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

        yield return new WaitForSeconds(1f);
        action.actionResolved = true;

    }

    // Summon Undead
    public Action PerformSummonSkeleton(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformSummonSkeletonCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformSummonSkeletonCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        Ability summonUndead = caster.mySpellBook.GetAbilityByName("Summon Skeleton");
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

            // GameObject newSkeletonGO = Instantiate(PrefabHolder.Instance.skeletonPrefabs[Random.Range(0,PrefabHolder.Instance.skeletonPrefabs.Count)]);
            GameObject newSkeletonGO = Instantiate(PrefabHolder.Instance.SkeletonPeasantPrefab);
 
            Enemy newSkeleton = newSkeletonGO.GetComponent<Enemy>();

            newSkeleton.InitializeSetup(spawnLocation.GridPosition, spawnLocation);
        }

        yield return new WaitForSeconds(1f);
        action.actionResolved = true;

    }

    // Healing Light
    public Action PerformHealingLight(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformHealingLightCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformHealingLightCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        Ability healingLight = caster.mySpellBook.GetAbilityByName("Healing Light");
        OnAbilityUsedStart(healingLight, caster);
        target.ModifyCurrentHealth(healingLight.abilityPrimaryValue);
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }
    #endregion

    

}
