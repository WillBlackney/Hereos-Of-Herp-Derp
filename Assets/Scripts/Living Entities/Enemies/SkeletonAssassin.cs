using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkeletonAssassin : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();
        myName = "Skeleton Assassin";

        CharacterModelController.SetUpAsSkeletonAssassinPreset(myModel);

        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Shank");
        mySpellBook.EnemyLearnAbility("Dash");

        myPassiveManager.ModifyUndead();
        myPassiveManager.ModifyStealth(1);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Dagger");
        myOffHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Dagger");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability dash = mySpellBook.GetAbilityByName("Dash");
        Ability shank = mySpellBook.GetAbilityByName("Shank");

        ActionStart:

        SetTargetDefender(EntityLogic.GetBestTarget(this, false, true));

        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }

        // Shank best target
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, shank))
        {
            //SetTargetDefender(EntityLogic.GetBestTarget(this, false, true));
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Shank");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformShank(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike best target
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike)) 
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike");
            yield return new WaitForSeconds(0.5f);
            
            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Shank closest target
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, true), currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, shank))
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Shank");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformShank(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike closest target
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, true), currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike))
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Dash
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, false, true), currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            // dont dash if already in melee (this would trigger a free strike)
            EntityLogic.GetAllEnemiesWithinRange(this, currentMeleeRange).Count == 0 &&
            EntityLogic.IsAbilityUseable(this, dash) &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(dash, strike, this) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, EntityLogic.GetBestTarget(this, false, true), currentMeleeRange, dash.abilityPrimaryValue + EntityLogic.GetTotalMobility(this)) != null
            )
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, false, true));

            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Dash");
            yield return new WaitForSeconds(0.5f);

            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, dash.abilityPrimaryValue + EntityLogic.GetTotalMobility(this));
            Action movementAction = AbilityLogic.Instance.PerformDash(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, false, true), currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this)) != null &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, strike, this)
            )
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, false, true));
            
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move");
            yield return new WaitForSeconds(0.5f);

            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this));
            Action movementAction = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // If, for whatever reason, we cant attack or move towards the weakest enemy, attack the nearest enemy

        // Shank closest target
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, true), currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, shank))
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Cheap Shot");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformCheapShot(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, true), currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike))
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();

    }
}
