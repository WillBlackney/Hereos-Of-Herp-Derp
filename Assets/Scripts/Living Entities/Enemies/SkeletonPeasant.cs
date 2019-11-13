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

        SetTargetDefender(EntityLogic.GetClosestEnemy(this));

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }       

        // Strike
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike", false));
            yield return new WaitForSeconds(0.5f);
           // StartCoroutine(AttackTarget(myCurrentTarget, strike.abilityPrimaryValue));
            ModifyCurrentAP(-strike.abilityAPCost);
            //yield return new WaitUntil(() => AttackFinished() == true);

            // brief delay between actions
            yield return new WaitForSeconds(0.5f);
            goto ActionStart;
        }

        // Move
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this,move))
        {
            SetTargetDefender(EntityLogic.GetClosestEnemy(this));
            //GeneratePathToClosestTileWithinRangeOfTarget(myCurrentTarget, currentMeleeRange);
            //SetPath(path);
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);
            //StartCoroutine(Move());
            ModifyCurrentAP(-move.abilityAPCost);
           // yield return new WaitUntil(() => MovementFinished() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(0.5f);
            goto ActionStart;
        }

        EndMyActivation();                                                                                                                                                                    
    }
}
