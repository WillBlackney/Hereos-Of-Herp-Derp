using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLogic : MonoBehaviour
{
    // Singleton model   

    public static AbilityLogic Instance;
    private void Awake()
    {
        Instance = this;
    }

    // Misc Logic
    public void OnAbilityUsed(Ability ability, LivingEntity livingEntity)
    {
        Debug.Log("OnAbilityUsed() called for " + livingEntity.gameObject.name + " using " + ability.abilityName);
        // Set ability on cooldown
        ability.ModifyCurrentCooldown(ability.abilityBaseCooldownTime);

        // Reduce AP by cost of the ability
        // check for preparation here
        if (livingEntity.myPassiveManager.Preparation && ability.abilityName != "Preparation" && ability.abilityName != "Slice And Dice")
        {
            livingEntity.myPassiveManager.Preparation = false;
            livingEntity.myPassiveManager.preparationStacks = 0;
            livingEntity.myStatusManager.RemoveStatusIcon(livingEntity.myStatusManager.GetStatusIconByName("Preparation"));
        }
        else
        {
            livingEntity.ModifyCurrentAP(-ability.abilityAPCost);
        }


        // remove stealth if the ability is not move
        if (ability.abilityName != "Move")
        {
            if (livingEntity.isCamoflaged)
            {
                livingEntity.RemoveCamoflage();
            }
        }

        // TO DO: re-do fleetfooted pasive bonus logic: move ability should be free, not paid for then refunded with AP
        else if (ability.abilityName == "Move")
        {
            // if character has a free move available
            if (livingEntity.moveActionsTakenThisTurn == 0 && livingEntity.myPassiveManager.FleetFooted)
            {
                livingEntity.StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(livingEntity.transform.position, "Fleet Footed", true));
                livingEntity.ModifyCurrentAP(ability.abilityAPCost, false);
            }
            livingEntity.moveActionsTakenThisTurn++;
        }

        LevelManager.Instance.UnhighlightAllTiles();
    }

    // Abilities

    // Free Strike
    public Action PerformFreeStrike(LivingEntity attacker, LivingEntity victim)
    {
        Action action = new Action();
        StartCoroutine(PerformFreeStrikeCoroutine(attacker, victim, action));
        return action;
    }
    public IEnumerator PerformFreeStrikeCoroutine(LivingEntity attacker, LivingEntity victim, Action action)
    {
        StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(attacker.transform.position, "Free Strike", false));
        yield return new WaitForSeconds(0.5f);
        Ability strike = attacker.mySpellBook.GetAbilityByName("Strike");
        attacker.StartCoroutine(attacker.AttackMovement(victim));
        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(strike.abilityPrimaryValue, victim, attacker, strike.abilityDamageType), attacker, victim);        
        yield return new WaitForSeconds(1f);
        action.actionResolved = true;        
    }
    //Strike
    public void PerformStrike(LivingEntity attacker, LivingEntity victim)
    {
        StartCoroutine(PerformStrikeCoroutine(attacker, victim));
    }
    public IEnumerator PerformStrikeCoroutine(LivingEntity attacker, LivingEntity victim)
    {
        Ability strike = attacker.mySpellBook.GetAbilityByName("Strike");
        attacker.StartCoroutine(attacker.AttackMovement(victim));
        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(strike.abilityPrimaryValue, victim, attacker, strike.abilityDamageType), attacker, victim);
        OnAbilityUsed(strike, attacker);
        yield return null;
    }

    // Twin Strike
    public void PerformTwinStrike(LivingEntity attacker, LivingEntity victim)
    {
        StartCoroutine(PerformTwinStrikeCoroutine(attacker, victim));
    }
    public IEnumerator PerformTwinStrikeCoroutine(LivingEntity attacker, LivingEntity victim)
    {
        Ability twinStrike = attacker.mySpellBook.GetAbilityByName("Twin Strike");

        StartCoroutine(attacker.AttackMovement(victim));
        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(twinStrike.abilityPrimaryValue, victim, attacker, twinStrike.abilityDamageType), attacker, victim);
        yield return new WaitForSeconds(0.3f);
        // check to make sure the enemy wasnt killed by the first attack
        if (victim.currentHealth > 0 && victim != null)
        {
            StartCoroutine(attacker.AttackMovement(victim));
            CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(twinStrike.abilityPrimaryValue, victim, attacker, twinStrike.abilityDamageType), attacker,victim);
        }

        OnAbilityUsed(twinStrike, attacker);
    }

    // Fire Ball
    public void PerformFireBall(LivingEntity attacker, LivingEntity victim)
    {
        StartCoroutine(PerformFireBallCoroutine(attacker, victim));
    }
    public IEnumerator PerformFireBallCoroutine(LivingEntity attacker, LivingEntity victim)
    {
        Ability fireball = attacker.mySpellBook.GetAbilityByName("Fire Ball");
        attacker.StartCoroutine(attacker.AttackMovement(victim));
        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(fireball.abilityPrimaryValue, victim, attacker, fireball.abilityDamageType), attacker, victim);
        OnAbilityUsed(fireball, attacker);
        yield return null;
    }

    // Chaos Bolt
    public void PerformChaosBolt(LivingEntity attacker, LivingEntity victim)
    {
        StartCoroutine(PerformChaosBoltCoroutine(attacker, victim));
    }
    public IEnumerator PerformChaosBoltCoroutine(LivingEntity attacker, LivingEntity victim)
    {
        Ability chaosBolt = attacker.mySpellBook.GetAbilityByName("Chaos Bolt");
        attacker.StartCoroutine(attacker.AttackMovement(victim));
        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(chaosBolt.abilityPrimaryValue, victim, attacker, chaosBolt.abilityDamageType), attacker, victim);
        //victim.ApplyKnockDown();
        victim.myPassiveManager.ModifyExposed(chaosBolt.abilitySecondaryValue);
        OnAbilityUsed(chaosBolt, attacker);
        yield return null;
    }

    // Snipe
    public void PerformSnipe(LivingEntity attacker, LivingEntity victim)
    {
        StartCoroutine(PerformSnipeCoroutine(attacker, victim));
    }
    public IEnumerator PerformSnipeCoroutine(LivingEntity attacker, LivingEntity victim)
    {
        Ability snipe = attacker.mySpellBook.GetAbilityByName("Snipe");
        StartCoroutine(attacker.AttackMovement(victim));
        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(snipe.abilityPrimaryValue, victim, attacker, snipe.abilityDamageType), attacker, victim);
        OnAbilityUsed(snipe, attacker);
        yield return null;
    }

    // Inspire
    public void PerformInspire(LivingEntity caster, LivingEntity target)
    {
        StartCoroutine(PerformInspireCoroutine(caster, target));
    }
    public IEnumerator PerformInspireCoroutine(LivingEntity caster, LivingEntity target)
    {
        Ability inspire = caster.mySpellBook.GetAbilityByName("Inspire");
        target.ModifyCurrentStrength(inspire.abilityPrimaryValue);
        OnAbilityUsed(inspire, caster);
        yield return null;
    }

    // Get Down!
    public void PerformGetDown(LivingEntity caster, TileScript destination)
    {
        StartCoroutine(PerformGetDownCoroutine(caster, destination));
    }
    public IEnumerator PerformGetDownCoroutine(LivingEntity caster, TileScript destination)
    {  
        Ability getDown = caster.mySpellBook.GetAbilityByName("Get Down!");
        
        Action action = MovementLogic.Instance.MoveEntity(caster, destination, 6);

        // yield wait until movement complete
        yield return new WaitUntil(() => action.ActionResolved() == true);

        // Give adjacent characters block
        List<TileScript> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, destination);
        foreach (LivingEntity livingEntity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (adjacentTiles.Contains(livingEntity.TileCurrentlyOn) &&
                CombatLogic.Instance.IsTargetFriendly(caster, livingEntity)
                )
            {
                livingEntity.ModifyCurrentBlock(getDown.abilityPrimaryValue);
            }
        }

        OnAbilityUsed(getDown, caster);

        yield return null;

    }

    // Smash
    public void PerformSmash(LivingEntity attacker, LivingEntity victim)
    {
        StartCoroutine(PerformSmashCoroutine(attacker, victim));
    }
    public IEnumerator PerformSmashCoroutine(LivingEntity attacker, LivingEntity victim)
    {
        Debug.Log("Performing Smash...");
        Ability smash = attacker.mySpellBook.GetAbilityByName("Smash");

        // Attack
        StartCoroutine(attacker.AttackMovement(victim));
        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(smash.abilityPrimaryValue, victim, attacker, smash.abilityDamageType), attacker, victim);

        // Knock back.
        MovementLogic.Instance.KnockBackEntity(attacker, victim, smash.abilitySecondaryValue);
        OnAbilityUsed(smash, attacker);
        yield return null;
    }

    // Chain Lightning
    public void PerformChainLightning(LivingEntity attacker, LivingEntity target)
    {
        StartCoroutine(PerformChainLightningCoroutine(attacker, target));
    }
    public IEnumerator PerformChainLightningCoroutine(LivingEntity attacker, LivingEntity target)
    {
        Debug.Log("Performing Chain Lightning...");
        Ability chainLightning = attacker.mySpellBook.GetAbilityByName("Chain Lightning");
        StartCoroutine(attacker.AttackMovement(target));

        LivingEntity currentTarget = target;
        LivingEntity previousTarget = target;

        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(chainLightning.abilityPrimaryValue, target, attacker,chainLightning.abilityDamageType), attacker, target);
        yield return new WaitForSeconds(0.2f);

        for (int lightningJumps = 0; lightningJumps < chainLightning.abilitySecondaryValue; lightningJumps++)
        {
            List<TileScript> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, currentTarget.TileCurrentlyOn);

            foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
            {
                if (adjacentTiles.Contains(enemy.TileCurrentlyOn))
                {
                    previousTarget = currentTarget;
                    currentTarget = enemy;
                }
            }

            if (previousTarget != currentTarget)
            {
                CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(chainLightning.abilityPrimaryValue, currentTarget, attacker, chainLightning.abilityDamageType), attacker, currentTarget);
                yield return new WaitForSeconds(0.2f);
            }

        }

        OnAbilityUsed(chainLightning, attacker);
        yield return null;
    }

    // Primal Blast
    public void PerformPrimalBlast(LivingEntity attacker, LivingEntity target)
    {
        StartCoroutine(PerformPrimalBlastCoroutine(attacker, target));
    }
    public IEnumerator PerformPrimalBlastCoroutine(LivingEntity attacker, LivingEntity target)
    {
        Debug.Log("Performing Primal Blast...");
        Ability primalBlast = attacker.mySpellBook.GetAbilityByName("Primal Blast");
        StartCoroutine(attacker.AttackMovement(target));

        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(primalBlast.abilityPrimaryValue, target, attacker,AbilityDataSO.DamageType.Physical), attacker, target);
        yield return new WaitForSeconds(0.2f);
        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(primalBlast.abilityPrimaryValue, target, attacker, AbilityDataSO.DamageType.Magic), attacker, target);
        yield return new WaitForSeconds(0.2f);

        OnAbilityUsed(primalBlast, attacker);
    }

    // Meteor
    public void PerformMeteor(LivingEntity attacker, TileScript location)
    {
        StartCoroutine(PerformMeteorCoroutine(attacker, location));
    }
    public IEnumerator PerformMeteorCoroutine(LivingEntity attacker, TileScript location)
    {
        Ability meteor = attacker.mySpellBook.GetAbilityByName("Meteor");
        CombatLogic.Instance.CreateAoEAttackEvent(attacker, meteor, location, 1, false, true);
        OnAbilityUsed(meteor, attacker);
        yield return null;
    }

    // Whirlwind
    public void PerformWhirlwind(LivingEntity attacker)
    {
        StartCoroutine(PerformWhirlwindCoroutine(attacker));
    }
    public IEnumerator PerformWhirlwindCoroutine(LivingEntity attacker)
    {
        Ability whirlwind = attacker.mySpellBook.GetAbilityByName("Whirlwind");
        CombatLogic.Instance.CreateAoEAttackEvent(attacker, whirlwind, attacker.TileCurrentlyOn, 1, true, true);
        OnAbilityUsed(whirlwind, attacker);
        yield return null;
    }

    // Guard
    public void PerformGuard(LivingEntity caster, LivingEntity target)
    {
        StartCoroutine(PerformGuardCoroutine(caster, target));
    }
    public IEnumerator PerformGuardCoroutine(LivingEntity caster,LivingEntity target)
    {
        Ability guard = caster.mySpellBook.GetAbilityByName("Guard");        
        target.ModifyCurrentBarrierStacks(guard.abilityPrimaryValue);
        OnAbilityUsed(guard, caster);
        yield return null;
    }

    // Frost bolt
    public void PerformFrostBolt(LivingEntity caster, LivingEntity victim)
    {
        StartCoroutine(PerformFrostBoltCoroutine(caster, victim));
    }
    public IEnumerator PerformFrostBoltCoroutine(LivingEntity caster, LivingEntity victim)
    {
        Ability frostbolt = caster.mySpellBook.GetAbilityByName("Frost Bolt");
        caster.StartCoroutine(caster.AttackMovement(victim));
        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(frostbolt.abilityPrimaryValue, victim, caster, frostbolt.abilityDamageType), caster, victim);
        
        victim.ApplyPinned();
        OnAbilityUsed(frostbolt, caster);
        yield return null;
    }

    // Shoot
    public void PerformShoot(LivingEntity caster, LivingEntity victim)
    {
        StartCoroutine(PerformShootCoroutine(caster, victim));
    }
    public IEnumerator PerformShootCoroutine(LivingEntity caster, LivingEntity victim)
    {
        Ability shoot = caster.mySpellBook.GetAbilityByName("Shoot");
        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(shoot.abilityPrimaryValue, victim, caster, shoot.abilityDamageType), caster, victim);
        OnAbilityUsed(shoot, caster);
        yield return null;
    }

    // Rapid Fire
    public void PerformRapidFire(LivingEntity caster, LivingEntity victim, int shots)
    {
        StartCoroutine(PerformRapidFireCoroutine(caster, victim, shots));
    }
    public IEnumerator PerformRapidFireCoroutine(LivingEntity caster, LivingEntity victim, int shots)
    {
        Ability rapidFIre = caster.mySpellBook.GetAbilityByName("Rapid Fire");

        int shotsFired = 0;

        ShotStart:
        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(rapidFIre.abilityPrimaryValue, victim, caster, rapidFIre.abilityDamageType), caster, victim);
        shotsFired++;
        caster.ModifyCurrentAP(-1);
        yield return new WaitForSeconds(0.5f);
        if (victim != null && shotsFired < shots)
        {
            goto ShotStart;
        }
        OnAbilityUsed(rapidFIre, caster);
    }

    // Poison Dart
    public void PerformPoisonDart(LivingEntity caster, LivingEntity victim)
    {
        StartCoroutine(PerformPoisonDartCoroutine(caster, victim));
    }
    public IEnumerator PerformPoisonDartCoroutine(LivingEntity caster, LivingEntity victim)
    {
        Ability poisonDart = caster.mySpellBook.GetAbilityByName("Poison Dart");
        StartCoroutine(caster.AttackMovement(victim));        
        victim.ModifyPoison(poisonDart.abilitySecondaryValue);
        OnAbilityUsed(poisonDart, caster);
        yield return null;
    }

    // Chemical Reaction
    public void PerformChemicalReaction(LivingEntity caster, LivingEntity victim)
    {
        StartCoroutine(PerformPoisonDartCoroutine(caster, victim));
    }
    public IEnumerator PerformChemicalReactionCoroutine(LivingEntity caster, LivingEntity victim)
    {
        Ability chemicalReaction = caster.mySpellBook.GetAbilityByName("Chemical Reaction");
        StartCoroutine(caster.AttackMovement(victim));
        victim.ModifyPoison(victim.poisonStacks);
        OnAbilityUsed(chemicalReaction, caster);
        yield return null;
    }

    // Slice And Dice
    public void PerformSliceAndDice(LivingEntity caster, LivingEntity victim, int attacks)
    {
        StartCoroutine(PerformSliceAndDiceCoroutine(caster, victim, attacks));
    }
    public IEnumerator PerformSliceAndDiceCoroutine(LivingEntity caster, LivingEntity victim, int attacks)
    {
        int timesAttacked = 0;
        Ability sliceAndDice = caster.mySpellBook.GetAbilityByName("Slice And Dice");
        caster.ModifyCurrentAP(-attacks);

        ShotStart:
        StartCoroutine(caster.AttackMovement(victim));
        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(sliceAndDice.abilityPrimaryValue, victim, caster,sliceAndDice.abilityDamageType),caster,victim);
        yield return new WaitForSeconds(0.3f);
        timesAttacked++;
        if (victim.currentHealth > 0 && victim != null && timesAttacked < attacks)
        {
            goto ShotStart;
        }

        OnAbilityUsed(sliceAndDice, caster);
    }

    // Impaling Bolt
    public void PerformImpalingBolt(LivingEntity caster, LivingEntity victim)
    {
        StartCoroutine(PerformImpalingBoltCoroutine(caster, victim));
    }
    public IEnumerator PerformImpalingBoltCoroutine(LivingEntity caster, LivingEntity victim)
    {
        Ability impalingBolt = caster.mySpellBook.GetAbilityByName("Impaling Bolt");

        // Attack
        StartCoroutine(caster.AttackMovement(victim));
        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(impalingBolt.abilityPrimaryValue, victim, caster, impalingBolt.abilityDamageType), caster, victim);

        // Knockback
        MovementLogic.Instance.KnockBackEntity(caster, victim, impalingBolt.abilitySecondaryValue);
        OnAbilityUsed(impalingBolt, caster);
        yield return null;
    }

    // Impaling Bolt
    public void PerformRockToss(LivingEntity caster, LivingEntity victim)
    {
        StartCoroutine(PerformRockTossCoroutine(caster, victim));
    }
    public IEnumerator PerformRockTossCoroutine(LivingEntity caster, LivingEntity victim)
    {
        Ability rockToss = caster.mySpellBook.GetAbilityByName("Rock Toss");

        // Attack
        StartCoroutine(caster.AttackMovement(victim));
        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(rockToss.abilityPrimaryValue, victim, caster, rockToss.abilityDamageType), caster, victim);

        // Knockback
        MovementLogic.Instance.KnockBackEntity(caster, victim, rockToss.abilitySecondaryValue);
        OnAbilityUsed(rockToss, caster);
        yield return null;
    }

    // Invigorate
    public void PerformInvigorate(LivingEntity caster, LivingEntity target)
    {
        StartCoroutine(PerformInvigorateCoroutine(caster, target));
    }
    public IEnumerator PerformInvigorateCoroutine(LivingEntity caster, LivingEntity target)
    {
        Ability invigorate = caster.mySpellBook.GetAbilityByName("Invigorate");
        target.ModifyCurrentAP(invigorate.abilityPrimaryValue);
        OnAbilityUsed(invigorate, caster);
        yield return null;
    }

    // Lightning Shield
    public void PerformLightningShield(LivingEntity caster, LivingEntity target)
    {
        StartCoroutine(PerformLightningShieldCoroutine(caster, target));
    }
    public IEnumerator PerformLightningShieldCoroutine(LivingEntity caster, LivingEntity target)
    {
        Ability lightningShield = caster.mySpellBook.GetAbilityByName("Lightning Shield");
        target.myPassiveManager.LearnLightningShield(lightningShield.abilityPrimaryValue);
        VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Lightning Shield", false);
        OnAbilityUsed(lightningShield, caster);
        yield return null;
    }

    // Holy Fire
    public void PerformHolyFire(LivingEntity caster, LivingEntity target)
    {
        StartCoroutine(PerformHolyFireCoroutine(caster, target));
    }
    public IEnumerator PerformHolyFireCoroutine(LivingEntity caster, LivingEntity target)
    {
        Ability holyFire = caster.mySpellBook.GetAbilityByName("Holy Fire");

        if(CombatLogic.Instance.IsTargetFriendly(caster, target))
        {
            target.ModifyCurrentBlock(holyFire.abilityPrimaryValue);
        }
        else
        {
            CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(holyFire.abilityPrimaryValue, target, caster, holyFire.abilityDamageType), caster, target);
        }

        OnAbilityUsed(holyFire, caster);
        yield return null;

    }

    // Primal Rage
    public void PerformPrimalRage(LivingEntity caster, LivingEntity target)
    {
        StartCoroutine(PerformPrimalRageCoroutine(caster, target));
    }
    public IEnumerator PerformPrimalRageCoroutine(LivingEntity caster, LivingEntity target)
    {
        Ability primalRage = caster.mySpellBook.GetAbilityByName("Primal Rage");

        target.ModifyCurrentStrength(primalRage.abilityPrimaryValue);
        target.myPassiveManager.ModifyTemporaryStrength(primalRage.abilityPrimaryValue);

        OnAbilityUsed(primalRage, caster);
        yield return null;
    }

    // Phase Shift
    public void PerformPhaseShift(LivingEntity caster, LivingEntity target)
    {
        StartCoroutine(PerformPhaseShiftCoroutine(caster, target));
    }
    public IEnumerator PerformPhaseShiftCoroutine(LivingEntity caster, LivingEntity target)
    {
        Ability phaseShift = caster.mySpellBook.GetAbilityByName("Phase Shift");

        TileScript casterDestination = target.TileCurrentlyOn;

        MovementLogic.Instance.TeleportEntity(caster, target.TileCurrentlyOn);
        MovementLogic.Instance.TeleportEntity(target, casterDestination);

        OnAbilityUsed(phaseShift, caster);
        yield return null;
        
    }

    // Siphon Life
    public void PerformSiphonLife(LivingEntity caster, LivingEntity target)
    {
        StartCoroutine(PerformSiphonLifeCoroutine(caster, target));
    }
    public IEnumerator PerformSiphonLifeCoroutine(LivingEntity caster, LivingEntity target)
    {
        Ability siphonLife = caster.mySpellBook.GetAbilityByName("Siphon Life");

        target.ModifyCurrentStrength(-siphonLife.abilityPrimaryValue);
        caster.ModifyCurrentStrength(siphonLife.abilityPrimaryValue);
        OnAbilityUsed(siphonLife, caster);
        yield return null;
    }

    // Sanctity
    public void PerformSanctity(LivingEntity caster, LivingEntity target)
    {
        StartCoroutine(PerformSanctityCoroutine(caster, target));
    }
    public IEnumerator PerformSanctityCoroutine(LivingEntity caster, LivingEntity target)
    {
        Ability sanctity = caster.mySpellBook.GetAbilityByName("Sanctity");

        if (target.isStunned)
        {
            VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Stun Removed", false);
            yield return new WaitForSeconds(0.2f);
            target.RemoveStunned();
        }
        if (target.isPinned)
        {
            VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Pinned Removed", false);
            yield return new WaitForSeconds(0.2f);
            target.RemovePinned();
        }
        if (target.isPoisoned)
        {
            VisualEffectManager.Instance.CreateStatusEffect(target.transform.position, "Poison Removed", false);
            yield return new WaitForSeconds(0.2f);
            target.ModifyPoison(-target.poisonStacks);
        }
        // remove vulnerable
        // remove weakened

        OnAbilityUsed(sanctity, caster);
    }

    // Bless
    public void PerformBless(LivingEntity caster, LivingEntity target)
    {
        StartCoroutine(PerformBlessCoroutine(caster, target));
    }
    public IEnumerator PerformBlessCoroutine(LivingEntity caster, LivingEntity target)
    {
        Ability bless = caster.mySpellBook.GetAbilityByName("Bless");
        target.myPassiveManager.ModifyRune(bless.abilityPrimaryValue);
        OnAbilityUsed(bless, caster);
        yield return null;
    }

    // Void Bomb
    public void PerformVoidBomb(LivingEntity caster, LivingEntity target)
    {
        StartCoroutine(PerformVoidBombCoroutine(caster, target));
    }
    public IEnumerator PerformVoidBombCoroutine(LivingEntity caster, LivingEntity target)
    {
        Ability voidBomb = caster.mySpellBook.GetAbilityByName("Void Bomb");

        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(voidBomb.abilityPrimaryValue, target, caster, voidBomb.abilityDamageType), caster, target);
        target.ApplyStunned();
        OnAbilityUsed(voidBomb, caster);
        yield return null;

    }

    // Nightmare
    public void PerformNightmare(LivingEntity caster, LivingEntity target)
    {
        StartCoroutine(PerformNightmareCoroutine(caster, target));
    }
    public IEnumerator PerformNightmareCoroutine(LivingEntity caster, LivingEntity target)
    {
        Ability nightmare = caster.mySpellBook.GetAbilityByName("Nightmare");
        target.ModifySleeping(nightmare.abilityPrimaryValue);

        OnAbilityUsed(nightmare, caster);
        yield return null;
    }

    // Move
    public void PerformMove(LivingEntity characterMoved, TileScript destination)
    {
        StartCoroutine(PerformMoveCoroutine(characterMoved, destination));
    }
    public IEnumerator PerformMoveCoroutine(LivingEntity characterMoved, TileScript destination)
    {
        Ability move = characterMoved.mySpellBook.GetAbilityByName("Move");

        MovementLogic.Instance.MoveEntity(characterMoved, destination);

        OnAbilityUsed(move, characterMoved);

        yield return null;
    }

    // Dash
    public void PerformDash(LivingEntity characterMoved, TileScript destination)
    {
        StartCoroutine(PerformDashCoroutine(characterMoved, destination));
    }
    public IEnumerator PerformDashCoroutine(LivingEntity characterMoved, TileScript destination)
    {
        Ability dash = characterMoved.mySpellBook.GetAbilityByName("Dash");

        MovementLogic.Instance.MoveEntity(characterMoved, destination, 6);

        OnAbilityUsed(dash, characterMoved);

        yield return null;
    }

    // Charge
    public void PerformCharge(LivingEntity caster, LivingEntity target, TileScript destination)
    {
        StartCoroutine(PerformChargeCoroutine(caster, target, destination));
    }
    public IEnumerator PerformChargeCoroutine(LivingEntity caster, LivingEntity target, TileScript destination)
    {
        Ability charge = caster.mySpellBook.GetAbilityByName("Charge");

        // Charge movement
        Action action = MovementLogic.Instance.MoveEntity(caster, destination, 6);

        // yield wait until movement complete
        yield return new WaitUntil(() => action.ActionResolved() == true);

        // Charge attack
        caster.StartCoroutine(caster.AttackMovement(target));
        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(charge.abilityPrimaryValue, target, caster, charge.abilityDamageType), caster, target);

        // Apply exposed
        target.myPassiveManager.ModifyExposed(charge.abilitySecondaryValue);

        OnAbilityUsed(charge, caster);        
    }

    // Telekinesis
    public void PerformTelekinesis(LivingEntity caster, LivingEntity target, TileScript destination)
    {
        StartCoroutine(PerformTelekinesisCoroutine(caster, target, destination));
    }
    public IEnumerator PerformTelekinesisCoroutine(LivingEntity caster, LivingEntity target, TileScript destination)
    {
        Ability telekinesis = caster.mySpellBook.GetAbilityByName("Telekinesis");

        MovementLogic.Instance.TeleportEntity(target, destination);

        OnAbilityUsed(telekinesis, caster);

        yield return null;
    }

    // Teleport
    public void PerformTeleport(LivingEntity caster, TileScript destination)
    {
        StartCoroutine(PerformTeleportCoroutine(caster, destination));
    }
    public IEnumerator PerformTeleportCoroutine(LivingEntity caster, TileScript destination)
    {
        Ability teleport = caster.mySpellBook.GetAbilityByName("Teleport");

        MovementLogic.Instance.TeleportEntity(caster, destination);

        OnAbilityUsed(teleport, caster);

        yield return null;
    }

    // Doom
    public void PerformDoom(LivingEntity caster)
    {
        StartCoroutine(PerformDoomCoroutine(caster));
    }
    public IEnumerator PerformDoomCoroutine(LivingEntity caster)
    {
        Ability doom = caster.mySpellBook.GetAbilityByName("Doom");

        foreach(LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if(CombatLogic.Instance.IsTargetFriendly(caster, entity) == false)
            {
                entity.ModifyCurrentEnergy(-1);
            }
        }

        OnAbilityUsed(doom, caster);

        yield return null;
    }

    // Crushing Blow
    public void PerformCrushingBlow(LivingEntity caster, LivingEntity target)
    {
        StartCoroutine(PerformCrushingBlowCoroutine(caster, target));
    }
    public IEnumerator PerformCrushingBlowCoroutine(LivingEntity caster, LivingEntity target)
    {
        Ability crushingBlow = caster.mySpellBook.GetAbilityByName("Crushing Blow");

        caster.StartCoroutine(caster.AttackMovement(target));

        CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(crushingBlow.abilityPrimaryValue, target, caster, crushingBlow.abilityDamageType), caster, target);

        target.ApplyStunned();

        OnAbilityUsed(crushingBlow, caster);

        yield return null;
    }

    // Summon Undead
    public void PerformSummonUndead(LivingEntity caster, LivingEntity target)
    {
        StartCoroutine(PerformSummonUndeadCoroutine(caster, target));
    }
    public IEnumerator PerformSummonUndeadCoroutine(LivingEntity caster, LivingEntity target)
    {
        Ability summonUndead = caster.mySpellBook.GetAbilityByName("Summon Undead");

        List<TileScript> allPossibleSpawnLocations = LevelManager.Instance.GetValidMoveableTilesWithinRange(summonUndead.abilityRange, caster.TileCurrentlyOn);
        List<TileScript> finalList = new List<TileScript>();

        // if target is to the left
        if (target.GridPosition.X <= caster.GridPosition.X)
        {
            foreach (TileScript tile in allPossibleSpawnLocations)
            {
                if (tile.GridPosition.X >= target.GridPosition.X && tile.GridPosition.X <= caster.GridPosition.X)
                {
                    finalList.Add(tile);
                }
            }
        }

        // if target is to the right
        else if (target.GridPosition.X > caster.GridPosition.X)
        {
            foreach (TileScript tile in allPossibleSpawnLocations)
            {
                if (tile.GridPosition.X <= target.GridPosition.X && tile.GridPosition.X >= caster.GridPosition.X)
                {
                    finalList.Add(tile);
                }
            }
        }

        // summon skeletons loop
        for (int skeletonsSummoned = 0; skeletonsSummoned < summonUndead.abilityPrimaryValue; skeletonsSummoned++)
        {
            TileScript spawnLocation = LevelManager.Instance.GetClosestValidTile(finalList, target.TileCurrentlyOn);

            GameObject newSkeletonGO = Instantiate(PrefabHolder.Instance.ZombiePrefab);
            Enemy newSkeleton = newSkeletonGO.GetComponent<Enemy>();

            newSkeleton.InitializeSetup(spawnLocation.GridPosition, spawnLocation);
        }

        OnAbilityUsed(summonUndead, caster);

        yield return null;
    }

    // Healing Light
    public void PerformHealingLight(LivingEntity caster, LivingEntity target)
    {
        StartCoroutine(PerformHealingLightCoroutine(caster, target));
    }
    public IEnumerator PerformHealingLightCoroutine(LivingEntity caster, LivingEntity target)
    {
        Ability healingLight = caster.mySpellBook.GetAbilityByName("Healing Light");

        target.ModifyCurrentHealth(healingLight.abilityPrimaryValue);

        OnAbilityUsed(healingLight, caster);

        yield return null;
    }

}
