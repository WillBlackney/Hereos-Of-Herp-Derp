using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkeletonAssassin : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();

        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Twin Strike");
        mySpellBook.EnemyLearnAbility("Dash");

        myPassiveManager.ModifyUndead();
        myPassiveManager.ModifyStealth(1);
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability dash = mySpellBook.GetAbilityByName("Dash");
        Ability twinStrike = mySpellBook.GetAbilityByName("Twin Strike");

        ActionStart:

        SetTargetDefender(EntityLogic.GetEnemyWithLowestCurrentHP(this));

        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }

        // Twin Strike
        else if (EntityLogic.IsTargetInRange(this,EntityLogic.GetEnemyWithLowestCurrentHP(this), currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, twinStrike))
        {
            SetTargetDefender(EntityLogic.GetEnemyWithLowestCurrentHP(this));
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Twin Strike");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformTwinStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetEnemyWithLowestCurrentHP(this), currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike)) 
        {
            SetTargetDefender(EntityLogic.GetEnemyWithLowestCurrentHP(this));
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike");
            yield return new WaitForSeconds(0.5f);
            
            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Dash
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetEnemyWithLowestCurrentHP(this), currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, dash) &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(dash, strike, this) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, EntityLogic.GetEnemyWithLowestCurrentHP(this), currentMeleeRange, dash.abilityPrimaryValue) != null
            )
        {
            SetTargetDefender(EntityLogic.GetEnemyWithLowestCurrentHP(this));

            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Dash");
            yield return new WaitForSeconds(0.5f);

            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, dash.abilityPrimaryValue);
            Action movementAction = AbilityLogic.Instance.PerformDash(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetEnemyWithLowestCurrentHP(this), currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this)) != null &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, strike, this)
            )
        {
            SetTargetDefender(EntityLogic.GetEnemyWithLowestCurrentHP(this));
            
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

        // Twin Strike
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestValidEnemy(this), currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, twinStrike))
        {
            SetTargetDefender(EntityLogic.GetClosestValidEnemy(this));
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Twin Strike");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformTwinStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestValidEnemy(this), currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike))
        {
            SetTargetDefender(EntityLogic.GetClosestValidEnemy(this));
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
