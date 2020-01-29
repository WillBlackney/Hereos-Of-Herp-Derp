﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBarbarian : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();
        myName = "Skeleton Barbarian";

        CharacterModelController.SetUpAsSkeletonBarbarianPreset(myModel);

        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Charge");
        mySpellBook.EnemyLearnAbility("Whirlwind");

        myPassiveManager.ModifyUndead();
        myPassiveManager.ModifyEnrage(2);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Battle Axe");
    }       
    
    public override IEnumerator StartMyActivationCoroutine()
    {
        // Get ability data
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability charge = mySpellBook.GetAbilityByName("Charge");
        Ability whirlwind = mySpellBook.GetAbilityByName("Whirlwind");

        ActionStart:

        // Pause if game over event has started
        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        // Decide on target
        SetTargetDefender(EntityLogic.GetBestTarget(this, true));

        // below line used later to prevent charging against a this is already in melee with
        List<Tile> tilesInMyMeleeRange = LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, tile);

        // End activation if stunned, out of energy, etc
        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }

        // Try Charge
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, charge.abilityRange + EntityLogic.GetTotalMobility(this)) &&
            EntityLogic.IsAbilityUseable(this, charge) &&
            tilesInMyMeleeRange.Contains(myCurrentTarget.tile) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, charge.abilityRange + EntityLogic.GetTotalMobility(this)) != null
            )
        {            
            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, charge.abilityRange + EntityLogic.GetTotalMobility(this));
            
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Charge");
            yield return new WaitForSeconds(0.5f);

            Action chargeAction = AbilityLogic.Instance.PerformCharge(this, myCurrentTarget, destination);
            yield return new WaitUntil(() => chargeAction.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }        

        // Try Whirlwind
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, whirlwind))
        {            
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Whirlwind");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformWhirlwind(this);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike))
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) == false && 
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, strike, this) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this)) != null
            )
        {            
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move");
            yield return new WaitForSeconds(0.5f);

            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this));
            Action movementAction = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();
    }

    
}
