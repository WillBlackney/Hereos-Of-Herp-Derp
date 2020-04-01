using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMage : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Skeleton Mage";
        base.SetBaseProperties();        

        CharacterModelController.SetUpAsSkeletonMagePreset(myModel);

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Fire Ball");        
        mySpellBook.EnemyLearnAbility("Frost Bolt");
        mySpellBook.EnemyLearnAbility("Icy Focus");

        // myPassiveManager.ModifyUndead();
        myPassiveManager.ModifyFlux(1);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Staff");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability fireBall = mySpellBook.GetAbilityByName("Fire Ball");        
        Ability frostBolt = mySpellBook.GetAbilityByName("Frost Bolt");
        Ability icyFocus = mySpellBook.GetAbilityByName("Icy Focus");

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


        // Icy Focus
        else if (EntityLogic.IsTargetInRange(this, this, icyFocus.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, icyFocus, myCurrentTarget)
            )
        {
            Action action = AbilityLogic.Instance.PerformIcyFocus(this, this);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Frost Bolt
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, frostBolt.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, frostBolt, myCurrentTarget) &&
            myCurrentTarget.myPassiveManager.immobilized == false
            )
        { 
            Action action = AbilityLogic.Instance.PerformFrostBolt(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }


        // Fireball the most vulnerable target
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, false, false, true), fireBall.abilityRange) &&            
            EntityLogic.IsAbilityUseable(this, fireBall, EntityLogic.GetBestTarget(this, false, false, true)))
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, false, false, true));

            Action action = AbilityLogic.Instance.PerformFireBall(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }

        // Fireball the target with lowest current HP
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, false, true), fireBall.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, fireBall, EntityLogic.GetBestTarget(this, false, true)))
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, false, true));

            Action action = AbilityLogic.Instance.PerformFireBall(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Fireball the closest target if the most vulnerable and the weakest cant be targetted
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, fireBall.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, fireBall, myCurrentTarget))
        {
            Action action = AbilityLogic.Instance.PerformFireBall(this, myCurrentTarget);
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
            EntityLogic.IsTargetInRange(this, myCurrentTarget, fireBall.abilityRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
            //EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, fireBall, this) &&
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
