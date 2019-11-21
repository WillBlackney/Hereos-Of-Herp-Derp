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
    public void OnAbilityUsed(Ability ability, LivingEntity livingEntity)
    {
        Debug.Log("OnAbilityUsed() called for " + livingEntity.gameObject.name + " using " + ability.abilityName);

        // Disable tile hover + tile highlights
        TileHover.Instance.SetVisibility(false);
        LevelManager.Instance.UnhighlightAllTiles();

        // temp variables
        int finalApCost = ability.abilityAPCost;
        int finalCD = ability.abilityBaseCooldownTime;
       
        // Reduce AP by cost of the ability
        // check for preparation here
        if (livingEntity.myPassiveManager.preparation && ability.abilityName != "Preparation" && ability.abilityName != "Slice And Dice")
        {
            livingEntity.myPassiveManager.preparation = false;
            livingEntity.myPassiveManager.preparationStacks = 0;
            livingEntity.myStatusManager.RemoveStatusIcon(livingEntity.myStatusManager.GetStatusIconByName("Preparation"));
            finalApCost = 0;
        }        


        // TO DO: re-do fleetfooted pasive bonus logic: move ability should be free, not paid for then refunded with AP
        if(ability.abilityName == "Move")
        {
            // if character has a free move available
            if (livingEntity.moveActionsTakenThisTurn == 0 && livingEntity.myPassiveManager.fleetFooted)
            {
                livingEntity.StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(livingEntity.transform.position, "Fleet Footed", true, "Blue"));
                finalApCost = 0;
            }
            livingEntity.moveActionsTakenThisTurn++;
        }

        else if (ability.abilityName == "Slice And Dice")
        {
            finalApCost = livingEntity.currentAP;
        }

        // Modify AP
        livingEntity.ModifyCurrentAP(-finalApCost);
        // Modify Cooldown
        ability.ModifyCurrentCooldown(finalCD);

        
    }
    #endregion

    // Specific Ability Logic
    #region

    // Free Strike
    public Action PerformFreeStrike(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformFreeStrikeCoroutine(attacker, victim, action));
        return action;
    }
    public IEnumerator PerformFreeStrikeCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "Free Strike", false, "Blue"));
        yield return new WaitForSeconds(0.5f);
        // to do: this should
        // AbilityLogic.Instance.PerformFreeStrike
        // wait until this is resolved
        // continue
        Ability strike = attacker.mySpellBook.GetAbilityByName("Strike");
        attacker.StartCoroutine(attacker.AttackMovement(victim));
        Action abilityAction = CombatLogic.Instance.HandleDamage(strike.abilityPrimaryValue, attacker, victim, false, strike.abilityAttackType, strike.abilityDamageType);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        yield return new WaitForSeconds(1f);
        action.actionResolved = true;        
    }

    // Block
    public Action PerformBlock(LivingEntity caster)
    {
        Action action = new Action();
        StartCoroutine(PerformBlockCoroutine(caster, action));
        return action;
    }
    public IEnumerator PerformBlockCoroutine(LivingEntity caster,  Action action)
    {
        Ability block = caster.mySpellBook.GetAbilityByName("Block");
        OnAbilityUsed(block, caster);
        caster.ModifyCurrentBlock(block.abilityPrimaryValue);
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }

    // Preparation
    public Action PerformPreparation(LivingEntity caster)
    {
        Action action = new Action();
        StartCoroutine(PerformPreparationCoroutine(caster, action));
        return action;
    }
    public IEnumerator PerformPreparationCoroutine(LivingEntity caster, Action action)
    {
        Debug.Log("Preparation button clicked");

        Ability preparation = caster.mySpellBook.GetAbilityByName("Preparation");
        OnAbilityUsed(preparation, caster);

        // check for 'Improved Preparation' talent
        if (caster.defender != null &&
            caster.defender.myCharacterData.KnowsImprovedPreparation)
        {
            foreach (Ability ability in caster.mySpellBook.myActiveAbilities)
            {
                if (ability != preparation)
                {
                    ability.ModifyCurrentCooldown(-1);
                }
            }
        }

        caster.myPassiveManager.ModifyPreparation(1);
        yield return new WaitForSeconds(0.5f);

        action.actionResolved = true;
    }

    //Strike
    public Action PerformStrike(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformStrikeCoroutine(attacker, victim, action));
        return action;
    }
    public IEnumerator PerformStrikeCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability strike = attacker.mySpellBook.GetAbilityByName("Strike");
        OnAbilityUsed(strike, attacker);
        attacker.StartCoroutine(attacker.AttackMovement(victim));        
        Action abilityAction = CombatLogic.Instance.HandleDamage(strike.abilityPrimaryValue, attacker, victim, false, strike.abilityAttackType, strike.abilityDamageType);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
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
        OnAbilityUsed(twinStrike, attacker);
        StartCoroutine(attacker.AttackMovement(victim));
        Action abilityAction = CombatLogic.Instance.HandleDamage(twinStrike.abilityPrimaryValue, attacker, victim, false, twinStrike.abilityAttackType, twinStrike.abilityDamageType);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        yield return new WaitForSeconds(0.3f);
        // check to make sure the enemy wasnt killed by the first attack
        if (victim.currentHealth > 0 && victim != null)
        {
            StartCoroutine(attacker.AttackMovement(victim));
            Action abilityAction2 = CombatLogic.Instance.HandleDamage(twinStrike.abilityPrimaryValue, attacker, victim, false, twinStrike.abilityAttackType, twinStrike.abilityDamageType);
            yield return new WaitUntil(() => abilityAction2.ActionResolved() == true);
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
        OnAbilityUsed(morkSmash, attacker);
        StartCoroutine(attacker.AttackMovement(victim));
        Action abilityAction = CombatLogic.Instance.HandleDamage(morkSmash.abilityPrimaryValue, attacker, victim, false, morkSmash.abilityAttackType, morkSmash.abilityDamageType);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

        // Knock back.
        Action knockBackAction = MovementLogic.Instance.KnockBackEntity(attacker, victim, morkSmash.abilitySecondaryValue);
        yield return new WaitUntil(() => knockBackAction.ActionResolved() == true);        
        action.actionResolved = true;
    }
    // Fire Ball
    public Action PerformFireBall(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformFireBallCoroutine(attacker, victim, action));
        return action;
    }
    public IEnumerator PerformFireBallCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability fireball = attacker.mySpellBook.GetAbilityByName("Fire Ball");
        OnAbilityUsed(fireball, attacker);
        attacker.StartCoroutine(attacker.AttackMovement(victim));
        Action abilityAction = CombatLogic.Instance.HandleDamage(fireball.abilityPrimaryValue, attacker, victim, false, fireball.abilityAttackType, fireball.abilityDamageType, fireball);
        // check for improved fireball talent
        if(attacker.defender && attacker.defender.myCharacterData.KnowsImprovedFireBall)
        {
            victim.myPassiveManager.ModifyIgnite(1);
        }
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

        action.actionResolved = true;
        
    }

    // Chaos Bolt
    public Action PerformChaosBolt(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformChaosBoltCoroutine(attacker, victim, action));
        return action;
    }
    public IEnumerator PerformChaosBoltCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability chaosBolt = attacker.mySpellBook.GetAbilityByName("Chaos Bolt");
        OnAbilityUsed(chaosBolt, attacker);
        attacker.StartCoroutine(attacker.AttackMovement(victim));
        Action abilityAction = CombatLogic.Instance.HandleDamage(chaosBolt.abilityPrimaryValue, attacker, victim, false, chaosBolt.abilityAttackType, chaosBolt.abilityDamageType);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        victim.myPassiveManager.ModifyExposed(chaosBolt.abilitySecondaryValue);
        action.actionResolved = true;
    }

    // Snipe
    public Action PerformSnipe(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformSnipeCoroutine(attacker, victim, action));
        return action;
    }
    public IEnumerator PerformSnipeCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability snipe = attacker.mySpellBook.GetAbilityByName("Snipe");
        OnAbilityUsed(snipe, attacker);
        StartCoroutine(attacker.AttackMovement(victim));
        Action abilityAction = CombatLogic.Instance.HandleDamage(snipe.abilityPrimaryValue, attacker, victim, false, snipe.abilityAttackType, snipe.abilityDamageType);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        action.actionResolved = true;
    }

    // Inspire
    public Action PerformInspire(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformInspireCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformInspireCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        Ability inspire = caster.mySpellBook.GetAbilityByName("Inspire");
        OnAbilityUsed(inspire, caster);
        target.ModifyCurrentStrength(inspire.abilityPrimaryValue);
        if(caster.defender && caster.defender.myCharacterData.KnowsImprovedInspire)
        {
            target.ModifyCurrentDexterity(inspire.abilityPrimaryValue);
            target.ModifyCurrentWisdom(inspire.abilityPrimaryValue);
        }
        action.actionResolved = true;
        yield return null;
    }

    // Get Down!
    public Action PerformGetDown(LivingEntity caster, Tile destination)
    {
        Action action = new Action();
        StartCoroutine(PerformGetDownCoroutine(caster, destination, action));
        return action;
    }
    public IEnumerator PerformGetDownCoroutine(LivingEntity caster, Tile destination, Action action)
    {  
        Ability getDown = caster.mySpellBook.GetAbilityByName("Get Down!");
        OnAbilityUsed(getDown, caster);
        Action moveAction = MovementLogic.Instance.MoveEntity(caster, destination, 5);

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
                livingEntity.ModifyCurrentBlock(getDown.abilityPrimaryValue);
            }
        }

        action.actionResolved = true;
    }

    // Smash
    public Action PerformSmash(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformSmashCoroutine(attacker, victim, action));
        return action;
    }
    public IEnumerator PerformSmashCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Debug.Log("Performing Smash...");
        Ability smash = attacker.mySpellBook.GetAbilityByName("Smash");
        OnAbilityUsed(smash, attacker);
        // Attack
        StartCoroutine(attacker.AttackMovement(victim));
        Action abilityAction = CombatLogic.Instance.HandleDamage(smash.abilityPrimaryValue, attacker, victim, false, smash.abilityAttackType, smash.abilityDamageType);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

        // Knock back.
        MovementLogic.Instance.KnockBackEntity(attacker, victim, smash.abilitySecondaryValue);
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;
        
    }

    // Chain Lightning
    public Action PerformChainLightning(LivingEntity attacker, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformChainLightningCoroutine(attacker, target, action));
        return action;
    }
    public IEnumerator PerformChainLightningCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Debug.Log("Performing Chain Lightning...");
        Ability chainLightning = attacker.mySpellBook.GetAbilityByName("Chain Lightning");
        OnAbilityUsed(chainLightning, attacker);
        StartCoroutine(attacker.AttackMovement(victim));

        LivingEntity currentTarget = victim;
        LivingEntity previousTarget = victim;

        Action abilityAction = CombatLogic.Instance.HandleDamage(chainLightning.abilityPrimaryValue, attacker, victim, false, chainLightning.abilityAttackType, chainLightning.abilityDamageType);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        yield return new WaitForSeconds(0.2f);

        for (int lightningJumps = 0; lightningJumps < chainLightning.abilitySecondaryValue; lightningJumps++)
        {
            List<Tile> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, currentTarget.tile);

            foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
            {
                if (adjacentTiles.Contains(enemy.tile))
                {
                    previousTarget = currentTarget;
                    currentTarget = enemy;
                }
            }

            if (previousTarget != currentTarget)
            {
                Action abilityAction2 = CombatLogic.Instance.HandleDamage(chainLightning.abilityPrimaryValue, attacker, victim, false, chainLightning.abilityAttackType, chainLightning.abilityDamageType);
                yield return new WaitUntil(() => abilityAction2.ActionResolved() == true);
                yield return new WaitForSeconds(0.2f);
            }

        }

        action.actionResolved = true;

    }

    // Primal Blast
    public Action PerformPrimalBlast(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformPrimalBlastCoroutine(attacker, victim, action));
        return action;
    }
    public IEnumerator PerformPrimalBlastCoroutine(LivingEntity attacker, LivingEntity target, Action action)
    {
        Debug.Log("Performing Primal Blast...");
        Ability strike = attacker.mySpellBook.GetAbilityByName("Primal Blast");
        OnAbilityUsed(strike, attacker);
        StartCoroutine(attacker.AttackMovement(target));

        Action abilityAction = CombatLogic.Instance.HandleDamage(strike.abilityPrimaryValue, attacker, target, false, strike.abilityAttackType, AbilityDataSO.DamageType.Physical);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        yield return new WaitForSeconds(0.2f);

        Action abilityAction2 = CombatLogic.Instance.HandleDamage(strike.abilityPrimaryValue, attacker, target, false, strike.abilityAttackType, AbilityDataSO.DamageType.Magic);
        yield return new WaitUntil(() => abilityAction2.ActionResolved() == true);

        action.actionResolved = true;        
    }

    // Meteor
    public Action PerformMeteor(LivingEntity attacker, Tile location)
    {
        Action action = new Action();
        StartCoroutine(PerformMeteorCoroutine(attacker, location, action));
        return action;
    }
    public IEnumerator PerformMeteorCoroutine(LivingEntity attacker, Tile location, Action action)
    {
        Ability meteor = attacker.mySpellBook.GetAbilityByName("Meteor");
        OnAbilityUsed(meteor, attacker);
        CombatLogic.Instance.CreateAoEAttackEvent(attacker, meteor, location, 1, false, true);
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;        
    }

    // Whirlwind
    public Action PerformWhirlwind(LivingEntity attacker)
    {
        Action action = new Action();
        StartCoroutine(PerformWhirlwindCoroutine(attacker, action));
        return action;
    }
    public IEnumerator PerformWhirlwindCoroutine(LivingEntity attacker, Action action)
    {
        Ability whirlwind = attacker.mySpellBook.GetAbilityByName("Whirlwind");
        OnAbilityUsed(whirlwind, attacker);
        CombatLogic.Instance.CreateAoEAttackEvent(attacker, whirlwind, attacker.tile, 1, true, false);

        // Improved Whirlwind talent
        if(attacker.defender != null)
        {
            if (attacker.defender.myCharacterData.KnowsImprovedWhirlwind)
            {
                List<Tile> tilesInWhirlwindRange = LevelManager.Instance.GetTilesWithinRange(1, attacker.tile);
                foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
                {
                    if (CombatLogic.Instance.IsTargetFriendly(attacker, entity) == false &&
                        tilesInWhirlwindRange.Contains(entity.tile))
                    {
                        entity.myPassiveManager.ModifyExhausted(1);
                        entity.myPassiveManager.ModifyExposed(1);
                    }
                }
            }
            
        }
        yield return new WaitForSeconds(1f);
        action.actionResolved = true;
        
    }

    // Frost Nova
    public Action PerformFrostNova(LivingEntity attacker)
    {
        Action action = new Action();
        StartCoroutine(PerformFrostNovaCoroutine(attacker, action));
        return action;
    }
    public IEnumerator PerformFrostNovaCoroutine(LivingEntity attacker, Action action)
    {
        Ability frostNova = attacker.mySpellBook.GetAbilityByName("Frost Nova");
        OnAbilityUsed(frostNova, attacker);

        // damage enmies within 1
        CombatLogic.Instance.CreateAoEAttackEvent(attacker, frostNova, attacker.tile, 1, true, false);

        // pin enemies within 1
        List<Tile> tilesInNovaRange = LevelManager.Instance.GetTilesWithinRange(1, attacker.tile);
        foreach(LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if(!CombatLogic.Instance.IsTargetFriendly(attacker, entity) && tilesInNovaRange.Contains(entity.tile))
            {
                entity.myPassiveManager.ModifyPinned(1, attacker);
            }
        }
        
        yield return new WaitForSeconds(1f);
        action.actionResolved = true;

    }

    // Blood Lust
    public Action PerformBloodLust(LivingEntity attacker)
    {
        Action action = new Action();
        StartCoroutine(PerformBloodLustCoroutine(attacker, action));
        return action;
    }
    public IEnumerator PerformBloodLustCoroutine(LivingEntity attacker, Action action)
    {
        Ability bloodLust = attacker.mySpellBook.GetAbilityByName("Blood Lust");
        OnAbilityUsed(bloodLust, attacker);
        Action selfDamageAction = CombatLogic.Instance.HandleDamage(bloodLust.abilitySecondaryValue, attacker, attacker, false, AbilityDataSO.AttackType.None, AbilityDataSO.DamageType.None);
        yield return new WaitUntil(() => selfDamageAction.ActionResolved() == true);
        yield return new WaitForSeconds(0.5f);
        attacker.ModifyCurrentAP(bloodLust.abilityPrimaryValue);
        action.actionResolved = true;        
    }

    // Guard
    public Action PerformGuard(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformGuardCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformGuardCoroutine(LivingEntity caster,LivingEntity target, Action action)
    {
        Ability guard = caster.mySpellBook.GetAbilityByName("Guard");
        OnAbilityUsed(guard, caster);
        target.myPassiveManager.ModifyBarrier(guard.abilityPrimaryValue);
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;
        
    }

    // Frost bolt
    public Action PerformFrostBolt(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformFrostBoltCoroutine(caster, victim, action));
        return action;
    }
    public IEnumerator PerformFrostBoltCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability frostbolt = attacker.mySpellBook.GetAbilityByName("Frost Bolt");
        OnAbilityUsed(frostbolt, attacker);
        attacker.StartCoroutine(attacker.AttackMovement(victim));
        Action abilityAction = CombatLogic.Instance.HandleDamage(frostbolt.abilityPrimaryValue, attacker, victim, false, frostbolt.abilityAttackType, frostbolt.abilityDamageType);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        victim.myPassiveManager.ModifyPinned(1, attacker);
        action.actionResolved = true;
    }

    // Shoot
    public Action PerformShoot(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformShootCoroutine(attacker, victim, action));
        return action;
    }
    public IEnumerator PerformShootCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        Ability shoot = attacker.mySpellBook.GetAbilityByName("Shoot");
        OnAbilityUsed(shoot, attacker);
        Action abilityAction = CombatLogic.Instance.HandleDamage(shoot.abilityPrimaryValue, attacker, victim, false, shoot.abilityAttackType, shoot.abilityDamageType);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        action.actionResolved = true;
      
    }

    // Rapid Fire
    public Action PerformRapidFire(LivingEntity attacker, LivingEntity victim, int shots)
    {
        Action action = new Action();
        StartCoroutine(PerformRapidFireCoroutine(attacker, victim, shots, action));
        return action;
    }
    public IEnumerator PerformRapidFireCoroutine(LivingEntity attacker, LivingEntity victim, int shots, Action action)
    {
        Ability rapidFire = attacker.mySpellBook.GetAbilityByName("Rapid Fire");
        OnAbilityUsed(rapidFire, attacker);

        int shotsFired = 0;

        ShotStart:
        Action abilityAction = CombatLogic.Instance.HandleDamage(rapidFire.abilityPrimaryValue, attacker, victim, false, rapidFire.abilityAttackType, rapidFire.abilityDamageType);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        shotsFired++;
        attacker.ModifyCurrentAP(-1);
        yield return new WaitForSeconds(0.5f);
        if (victim != null && shotsFired < shots)
        {
            goto ShotStart;
        }

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
        OnAbilityUsed(poisonDart, caster);
        StartCoroutine(caster.AttackMovement(victim));        
        victim.myPassiveManager.ModifyPoison(poisonDart.abilitySecondaryValue, caster);
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;
        
    }

    // Chemical Reaction
    public Action PerformChemicalReaction(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformChemicalReactionCoroutine(caster, victim, action));
        return action;
    }
    public IEnumerator PerformChemicalReactionCoroutine(LivingEntity caster, LivingEntity victim, Action action)
    {
        Ability chemicalReaction = caster.mySpellBook.GetAbilityByName("Chemical Reaction");
        OnAbilityUsed(chemicalReaction, caster);
        StartCoroutine(caster.AttackMovement(victim));
        victim.myPassiveManager.ModifyPoison(victim.myPassiveManager.poisonStacks, caster);
        
        yield return null;
    }

    // Slice And Dice
    public Action PerformSliceAndDice(LivingEntity caster, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformSliceAndDiceCoroutine(caster, victim, action));
        return action;
    }
    public IEnumerator PerformSliceAndDiceCoroutine(LivingEntity attacker, LivingEntity victim,  Action action)
    {
        int totalAttacks = attacker.currentAP;
        int timesAttacked = 0;
        Ability sliceAndDice = attacker.mySpellBook.GetAbilityByName("Slice And Dice");
        OnAbilityUsed(sliceAndDice, attacker);        

        AttackLoopStart:
        StartCoroutine(attacker.AttackMovement(victim));
        Action abilityAction = CombatLogic.Instance.HandleDamage(sliceAndDice.abilityPrimaryValue, attacker, victim, false, sliceAndDice.abilityAttackType, sliceAndDice.abilityDamageType);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        yield return new WaitForSeconds(0.3f);
        timesAttacked++;
        if (victim.inDeathProcess == false && victim != null && timesAttacked < totalAttacks)
        {
            goto AttackLoopStart;
        }
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
        Ability impalingBolt = attacker.mySpellBook.GetAbilityByName("Impaling Bolt");
        OnAbilityUsed(impalingBolt, attacker);

        // Attack
        StartCoroutine(attacker.AttackMovement(victim));
        Action abilityAction = CombatLogic.Instance.HandleDamage(impalingBolt.abilityPrimaryValue, attacker, victim, false, impalingBolt.abilityAttackType, impalingBolt.abilityDamageType);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

        // Knockback
        MovementLogic.Instance.KnockBackEntity(attacker, victim, impalingBolt.abilitySecondaryValue);
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
        OnAbilityUsed(rockToss, attacker);

        // Attack
        StartCoroutine(attacker.AttackMovement(victim));
        Action abilityAction = CombatLogic.Instance.HandleDamage(rockToss.abilityPrimaryValue, attacker, victim, false, rockToss.abilityAttackType, rockToss.abilityDamageType);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        // Knockback
        MovementLogic.Instance.KnockBackEntity(attacker, victim, rockToss.abilitySecondaryValue);
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;
        
    }

    // Invigorate
    public Action PerformInvigorate(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformInvigorateCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformInvigorateCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        Ability invigorate = caster.mySpellBook.GetAbilityByName("Invigorate");
        OnAbilityUsed(invigorate, caster);
        target.ModifyCurrentAP(invigorate.abilityPrimaryValue);
        if (TalentLogic.Instance.DoesCharacterHaveTalent(caster, "Improved Invigorate"))
        {
            target.myPassiveManager.ModifyTemporaryMobility(1);
        }
        ParticleManager.Instance.CreateParticleEffect(target.tile.WorldPosition, ParticleManager.Instance.basicParticlePrefab);
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
        OnAbilityUsed(lightningShield, caster);
        target.myPassiveManager.ModifyLightningShield(lightningShield.abilityPrimaryValue);
        VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Lightning Shield", false, "Blue");
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;
        
    }

    // Holy Fire
    public Action PerformHolyFire(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformHolyFireCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformHolyFireCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        Ability holyFire = caster.mySpellBook.GetAbilityByName("Holy Fire");
        OnAbilityUsed(holyFire, caster);

        if (CombatLogic.Instance.IsTargetFriendly(caster, target))
        {
            target.ModifyCurrentBlock(holyFire.abilityPrimaryValue);
        }
        else
        {
            Action abilityAction = CombatLogic.Instance.HandleDamage(holyFire.abilityPrimaryValue, caster, target, false, holyFire.abilityAttackType, holyFire.abilityDamageType);
            yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        }

        action.actionResolved = true;

    }

    // Primal Rage
    public Action PerformPrimalRage(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformPrimalRageCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformPrimalRageCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        Ability primalRage = caster.mySpellBook.GetAbilityByName("Primal Rage");
        OnAbilityUsed(primalRage, caster);
        target.ModifyCurrentStrength(primalRage.abilityPrimaryValue);
        target.myPassiveManager.ModifyTemporaryStrength(primalRage.abilityPrimaryValue);
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;
    }

    // Phase Shift
    public Action PerformPhaseShift(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformPhaseShiftCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformPhaseShiftCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        Ability phaseShift = caster.mySpellBook.GetAbilityByName("Phase Shift");
        OnAbilityUsed(phaseShift, caster);

        Tile casterDestination = target.tile;
        Tile targetDestination = caster.tile;

        MovementLogic.Instance.TeleportEntity(caster, casterDestination, true);
        MovementLogic.Instance.TeleportEntity(target, targetDestination, true);

        action.actionResolved = true;
        
        yield return null;        
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
        OnAbilityUsed(siphonLife, caster);
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
        OnAbilityUsed(sanctity, caster);

        if (target.myPassiveManager.stunned)
        {
            VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Stun Removed", false, "Blue");
            yield return new WaitForSeconds(0.2f);
            target.myPassiveManager.ModifyStunned(-target.myPassiveManager.stunnedStacks);
        }
        if (target.myPassiveManager.pinned)
        {
            VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Pinned Removed", false, "Blue");
            yield return new WaitForSeconds(0.2f);
            target.myPassiveManager.ModifyPinned(-target.myPassiveManager.pinnedStacks);
        }
        if (target.myPassiveManager.poison)
        {
            VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Poison Removed", false, "Blue");
            yield return new WaitForSeconds(0.2f);
            target.myPassiveManager.ModifyPoison(-target.myPassiveManager.poisonStacks);
        }
        // remove vulnerable
        // remove weakened

        action.actionResolved = true;
        
    }

    // Bless
    public Action PerformBless(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformBlessCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformBlessCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        Ability bless = caster.mySpellBook.GetAbilityByName("Bless");
        OnAbilityUsed(bless, caster);
        target.myPassiveManager.ModifyRune(bless.abilityPrimaryValue);
        yield return new WaitForSeconds(0.5f);
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
        OnAbilityUsed(voidBomb, attacker);
        Action abilityAction = CombatLogic.Instance.HandleDamage(voidBomb.abilityPrimaryValue, attacker, victim, false, voidBomb.abilityAttackType, voidBomb.abilityDamageType);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        victim.myPassiveManager.ModifyStunned(1);
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;
        

    }

    // Nightmare
    public Action PerformNightmare(LivingEntity caster, LivingEntity target)
    {
        Action action = new Action();
        StartCoroutine(PerformNightmareCoroutine(caster, target, action));
        return action;
    }
    public IEnumerator PerformNightmareCoroutine(LivingEntity caster, LivingEntity target, Action action)
    {
        Ability nightmare = caster.mySpellBook.GetAbilityByName("Nightmare");
        OnAbilityUsed(nightmare, caster);
        target.myPassiveManager.ModifySleeping(nightmare.abilityPrimaryValue);
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;        
    }

    // Move
    public Action PerformMove(LivingEntity characterMoved, Tile destination)
    {
        Action action = new Action();
        StartCoroutine(PerformMoveCoroutine(characterMoved, destination, action));
        return action;
    }
    public IEnumerator PerformMoveCoroutine(LivingEntity characterMoved, Tile destination, Action action)
    {
        Ability move = characterMoved.mySpellBook.GetAbilityByName("Move");
        OnAbilityUsed(move, characterMoved);
        Action movementAction = MovementLogic.Instance.MoveEntity(characterMoved, destination);

        yield return new WaitUntil(() => movementAction.ActionResolved() == true);
        action.actionResolved = true;  
    }

    // Dash
    public Action PerformDash(LivingEntity characterMoved, Tile destination)
    {
        Action action = new Action();
        StartCoroutine(PerformDashCoroutine(characterMoved, destination, action));
        return action;
    }
    public IEnumerator PerformDashCoroutine(LivingEntity characterMoved, Tile destination, Action action)
    {
        Ability dash = characterMoved.mySpellBook.GetAbilityByName("Dash");

        OnAbilityUsed(dash, characterMoved);

        Action dashAction = MovementLogic.Instance.MoveEntity(characterMoved, destination, 5);
        
        yield return new WaitUntil(() => dashAction.ActionResolved() == true);

        if (characterMoved.defender)
        {
            if (characterMoved.defender.myCharacterData.KnowsImprovedDash)
            {
                characterMoved.myPassiveManager.ModifyTemporaryStrength(2);
            }
        }

        action.actionResolved = true;

    }

    // Charge
    public Action PerformCharge(LivingEntity caster, LivingEntity target, Tile destination)
    {
        Action action = new Action();
        StartCoroutine(PerformChargeCoroutine(caster, target, destination, action));
        return action;
    }
    public IEnumerator PerformChargeCoroutine(LivingEntity attacker, LivingEntity victim, Tile destination, Action action)
    {
        Ability charge = attacker.mySpellBook.GetAbilityByName("Charge");
        OnAbilityUsed(charge, attacker);
        // Charge movement
        Action moveAction = MovementLogic.Instance.MoveEntity(attacker, destination, 5);

        // yield wait until movement complete
        yield return new WaitUntil(() => moveAction.ActionResolved() == true);

        // Charge attack
        attacker.StartCoroutine(attacker.AttackMovement(victim));
        Action abilityAction = CombatLogic.Instance.HandleDamage(charge.abilityPrimaryValue, attacker, victim, false, charge.abilityAttackType, charge.abilityDamageType);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);
        // Apply exposed
        victim.myPassiveManager.ModifyExposed(charge.abilitySecondaryValue);

        action.actionResolved = true;
               
    }

    // Telekinesis
    public Action PerformTelekinesis(LivingEntity caster, LivingEntity target, Tile destination)
    {
        Action action = new Action();
        StartCoroutine(PerformTelekinesisCoroutine(caster, target, destination, action));
        return action;
    }
    public IEnumerator PerformTelekinesisCoroutine(LivingEntity caster, LivingEntity target, Tile destination, Action action)
    {
        Ability telekinesis = caster.mySpellBook.GetAbilityByName("Telekinesis");
        OnAbilityUsed(telekinesis, caster);

        MovementLogic.Instance.TeleportEntity(target, destination);

        action.actionResolved = true;

        yield return null;
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
        OnAbilityUsed(teleport, caster);

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
        OnAbilityUsed(doom, caster);
        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if(CombatLogic.Instance.IsTargetFriendly(caster, entity) == false)
            {
                entity.ModifyCurrentEnergy(-1);
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
        OnAbilityUsed(crushingBlow, attacker);
        attacker.StartCoroutine(attacker.AttackMovement(victim));

        Action abilityAction = CombatLogic.Instance.HandleDamage(crushingBlow.abilityPrimaryValue, attacker, victim, false, crushingBlow.abilityAttackType, crushingBlow.abilityDamageType);
        yield return new WaitUntil(() => abilityAction.ActionResolved() == true);

        victim.myPassiveManager.ModifyStunned(1);

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
        OnAbilityUsed(summonUndead, caster);
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
        OnAbilityUsed(healingLight, caster);
        target.ModifyCurrentHealth(healingLight.abilityPrimaryValue);
        yield return new WaitForSeconds(0.5f);
        action.actionResolved = true;

    }
    #endregion

}
