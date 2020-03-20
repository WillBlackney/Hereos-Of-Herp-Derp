using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonLord : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();

        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Summon Skeleton");
        mySpellBook.EnemyLearnAbility("Empower Binding");
        myPassiveManager.ModifySoulLink();    

    }
    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability summonSkeleton = mySpellBook.GetAbilityByName("Summon Skeleton");
        Ability empowerBinding = mySpellBook.GetAbilityByName("Empower Binding");

        ActionStart:
        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        SetTargetDefender(EntityLogic.GetClosestEnemy(this));

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            LivingEntityManager.Instance.EndEntityActivation(this);
        }

        // Summon skeleton
        else if (EntityLogic.IsAbilityUseable(this, summonSkeleton))
        {
            
            Action action = AbilityLogic.Instance.PerformSummonSkeleton(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(0.5f);
            goto ActionStart;
        }

        // Empower Binding
        else if (EntityLogic.IsAbilityUseable(this, empowerBinding))
        {
            Action action = AbilityLogic.Instance.PerformEmpowerBinding(this);
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(0.5f);
            goto ActionStart;
        }

        LivingEntityManager.Instance.EndEntityActivation(this);
        yield return null;

    }
}
