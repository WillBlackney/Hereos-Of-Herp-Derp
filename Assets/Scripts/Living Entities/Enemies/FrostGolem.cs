﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostGolem : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Frost Golem";
        base.SetBaseProperties();

        CharacterModelController.SetUpAsFrostGolemPreset(myModel);

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Frost Nova");
        mySpellBook.EnemyLearnAbility("Chilling Blow");
        mySpellBook.EnemyLearnAbility("Thaw");

        myPassiveManager.ModifyShatter(1);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Staff");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability chillingBlow = mySpellBook.GetAbilityByName("Chilling Blow");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability frostNova = mySpellBook.GetAbilityByName("Frost Nova");
        Ability thaw = mySpellBook.GetAbilityByName("Thaw");


    ActionStart:

        SetTargetDefender(EntityLogic.GetBestTarget(this, true));

        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            LivingEntityManager.Instance.EndEntityActivation(this);
        }

        // Frost Nova
        else if (EntityLogic.GetAllEnemiesWithinRange(this, 1).Count > 0 &&
            EntityLogic.IsAbilityUseable(this, frostNova))
        {
            Action action = AbilityLogic.Instance.PerformFrostNova(this);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Thaw
        else if (myCurrentTarget.myPassiveManager.chilled &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, thaw.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, thaw, myCurrentTarget))
        {
            Action action = AbilityLogic.Instance.PerformChillingBlow(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Chilling Blow 
        else if (myCurrentTarget.myPassiveManager.chilled &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, chillingBlow, myCurrentTarget))
        {
            Action action = AbilityLogic.Instance.PerformChillingBlow(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike, myCurrentTarget))
        {
            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (
            EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this)) != null
            )
        {
            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this));
            Action movementAction = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Can't do anything more, end activation
        else
        {
            LivingEntityManager.Instance.EndEntityActivation(this);
        }
    }
}
