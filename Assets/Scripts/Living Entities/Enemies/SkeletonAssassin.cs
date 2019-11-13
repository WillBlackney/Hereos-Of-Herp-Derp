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

        //myPassiveManager.LearnPoisonous(1);
        myPassiveManager.ModifyStealth();
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability dash = mySpellBook.GetAbilityByName("Dash");
        Ability twinStrike = mySpellBook.GetAbilityByName("Twin Strike");

        /* AI LOGIC
           If enemy is in melee range and twin strike ready, twin strike
           if enemy is in melee range and strike ready, strike
           If enemy is not in melee range, and dash ready, dash towards them
           if enemy is not in melee range and move ready, move towards them. 
        */

        ActionStart:
        
        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }

        // Twin Strike
        else if (EntityLogic.IsTargetInRange(this,EntityLogic.GetEnemyWithLowestCurrentHP(this), currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, twinStrike))
        {
            SetTargetDefender(EntityLogic.GetEnemyWithLowestCurrentHP(this));
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Twin Strike", false));
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
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike", false));
            yield return new WaitForSeconds(0.5f);
            
            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Dash
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetEnemyWithLowestCurrentHP(this), currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, dash)
            )
        {
            SetTargetDefender(EntityLogic.GetEnemyWithLowestCurrentHP(this));

            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Dash", false));
            yield return new WaitForSeconds(0.5f);

            Tile destination = AILogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, dash.abilityPrimaryValue);
            Action movementAction = AbilityLogic.Instance.PerformDash(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetEnemyWithLowestCurrentHP(this), currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move)
            )
        {
            SetTargetDefender(EntityLogic.GetEnemyWithLowestCurrentHP(this));
            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            Tile destination = AILogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, currentMobility);
            Action movementAction = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();

    }
}
