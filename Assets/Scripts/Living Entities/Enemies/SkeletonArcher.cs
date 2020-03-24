using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher : Enemy
{ 

    public override void SetBaseProperties()
    {
        myName = "Skeleton Archer";
        base.SetBaseProperties();       

        CharacterModelController.SetUpAsSkeletonArcherPreset(myModel);
     
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Shoot");
        mySpellBook.EnemyLearnAbility("Impaling Bolt");
        mySpellBook.EnemyLearnAbility("Snipe");
       
        myPassiveManager.ModifyTrueSight(1);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Bow");
    }       

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability shoot = mySpellBook.GetAbilityByName("Shoot");
        Ability snipe = mySpellBook.GetAbilityByName("Snipe");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability impalingBolt = mySpellBook.GetAbilityByName("Impaling Bolt");

        ActionStart:

        SetTargetDefender(EntityLogic.GetBestTarget(this, false, false, true));

        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            LivingEntityManager.Instance.EndEntityActivation(this);
        }

        // Impaling Bolt        
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, true), impalingBolt.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, impalingBolt, EntityLogic.GetBestTarget(this, true))
            )
        {
            Debug.Log("Skeleton Archer using Impaling Bolt against " + myCurrentTarget.myName);
            
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));
            
            Action action = AbilityLogic.Instance.PerformImpalingBolt(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }

        // Snipe most vulnerable target
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, snipe.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, snipe, myCurrentTarget))
        {
            Debug.Log("Skeleton Archer using snipe against most vulnerable target: " + myCurrentTarget.myName);

            Action action = AbilityLogic.Instance.PerformSnipe(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Shoot most vulnerable target
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, shoot.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, shoot, myCurrentTarget))
        {
            Debug.Log("Skeleton Archer using shoot against most vulnerable target: " + myCurrentTarget.myName);

            Action action = AbilityLogic.Instance.PerformShoot(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Shoot the target with lowest current HP
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, false, true), shoot.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, shoot, EntityLogic.GetBestTarget(this, false, true)))
        {
            
            SetTargetDefender(EntityLogic.GetBestTarget(this, false, true));
            Debug.Log("Skeleton Archer using shoot against most lowest HP target: " + myCurrentTarget.myName);

            Action action = AbilityLogic.Instance.PerformShoot(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Shoot the closest target if the most vulnerable and lowest HP target cant be targetted
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, true), shoot.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, shoot, EntityLogic.GetBestTarget(this, true)))
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));
            Debug.Log("Skeleton Archer using shoot against closest target: " + myCurrentTarget.myName);

            Action action = AbilityLogic.Instance.PerformShoot(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Is any target in range and valid?

        // Try move into range if nothing is in range
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, true), shoot.abilityRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, shoot, this) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, EntityLogic.GetBestTarget(this, true), currentMeleeRange, EntityLogic.GetTotalMobility(this)) != null &&
            EntityLogic.IsAbilityUseable(this, move)
            )
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));
            Debug.Log("Skeleton Archer moving towards: " + myCurrentTarget.myName);

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
