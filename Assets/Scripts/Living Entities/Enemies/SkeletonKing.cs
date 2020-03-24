using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKing : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Skeleton King";
        base.SetBaseProperties();

        CharacterModelController.SetUpAsSkeletonKingPreset(myModel);

        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Summon Skeleton");
        mySpellBook.EnemyLearnAbility("Empower Binding");

        myPassiveManager.ModifyFading(5);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Sword");

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
                else if (EntityLogic.IsAbilityUseable(this, summonSkeleton) &&
                EntityLogic.GetBestSummonEnemyLocation(this, myCurrentTarget, summonSkeleton.abilityRange) != null)
        {            
            Action action = AbilityLogic.Instance.PerformSummonSkeleton(this, EntityLogic.GetBestSummonEnemyLocation(this, myCurrentTarget, summonSkeleton.abilityRange));
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

        //  Strike
        else if (EntityLogic.IsAbilityUseable(this, strike, myCurrentTarget) &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange))
        {
            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) == false &&
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
        LivingEntityManager.Instance.EndEntityActivation(this);
        yield return null;

    }
}
