using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPeasant : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();
        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Move");
    }


    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");

        ActionStart:

        SetTargetDefender(GetClosestDefender());

        if (IsAbleToTakeActions() == false)
        {
            EndMyActivation();
        }       

        // Strike
        else if (IsTargetInRange(myCurrentTarget, currentMeleeRange) && HasEnoughAP(currentAP, strike.abilityAPCost))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike", false));
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(AttackTarget(myCurrentTarget, strike.abilityPrimaryValue));
            ModifyCurrentAP(-strike.abilityAPCost);
            yield return new WaitUntil(() => AttackFinished() == true);

            // brief delay between actions
            yield return new WaitForSeconds(0.5f);
            goto ActionStart;
        }

        // Move
        else if (IsTargetInRange(myCurrentTarget, currentMeleeRange) == false && IsAbleToMove() && HasEnoughAP(currentAP, move.abilityAPCost))
        {
            SetTargetDefender(GetClosestDefender());
            GeneratePathToClosestTileWithinRangeOfTarget(myCurrentTarget, currentMeleeRange);
            SetPath(path);
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Move());
            ModifyCurrentAP(-move.abilityAPCost);
            yield return new WaitUntil(() => MovementFinished() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(0.5f);
            goto ActionStart;
        }

        EndMyActivation();                                                                                                                                                                    
    }
}
