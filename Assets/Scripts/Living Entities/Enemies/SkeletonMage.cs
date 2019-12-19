using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMage : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();
        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Fire Ball");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Frost Bolt");

        myPassiveManager.ModifyUndead();
        myPassiveManager.ModifyQuickReflexes(1);
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability fireBall = mySpellBook.GetAbilityByName("Fire Ball");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability frostBolt = mySpellBook.GetAbilityByName("Frost Bolt");

        ActionStart:

        SetTargetDefender(EntityLogic.GetClosestEnemy(this));

        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }

        // Frost Bolt
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestEnemy(this), frostBolt.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, frostBolt)
            )
        {            
            SetTargetDefender(EntityLogic.GetClosestEnemy(this));
            // VFX notification
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Frost Bolt", false));
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformFrostBolt(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }


        // Fireball the most vulnerable target
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetMostVulnerableEnemy(this), fireBall.abilityRange) &&            
            EntityLogic.IsAbilityUseable(this, fireBall))
        {
            SetTargetDefender(EntityLogic.GetMostVulnerableEnemy(this));
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Fire Ball", false));
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformFireBall(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }

        // Fireball the target with lowest current HP
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetEnemyWithLowestCurrentHP(this), fireBall.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, fireBall))
        {
            SetTargetDefender(EntityLogic.GetEnemyWithLowestCurrentHP(this));
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Fire Ball", false));
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformFireBall(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Fireball the closest target if the most vulnerable and the weakest cant be targetted
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestEnemy(this), fireBall.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, fireBall))
        {
            SetTargetDefender(EntityLogic.GetClosestEnemy(this));
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Fire Ball", false));
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformFireBall(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }
        
        // Move
        else if (
            EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestEnemy(this), fireBall.abilityRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, fireBall, this) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, EntityLogic.GetClosestEnemy(this), fireBall.abilityRange, currentMobility) != null
            )
        {
            SetTargetDefender(EntityLogic.GetClosestEnemy(this));
            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, fireBall.abilityRange, currentMobility);
            Action movementAction = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();
    }
}
