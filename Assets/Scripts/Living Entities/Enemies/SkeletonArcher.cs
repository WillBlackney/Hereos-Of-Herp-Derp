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

        myPassiveManager.ModifyUndead();        
        myPassiveManager.ModifyTrueSight(1);
        //myPassiveManager.ModifyFlux(1);

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
            EndMyActivation();
        }

        // Impaling Bolt        
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, true), impalingBolt.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, impalingBolt)
            )
        {
            Debug.Log("Skeleton Archer using Impaling Bolt against " + myCurrentTarget.myName);
            
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));

            // VFX notification
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Impaling Bolt");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformImpalingBolt(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }

        // Snipe most vulnerable target
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, snipe.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, snipe))
        {
            Debug.Log("Skeleton Archer using snipe against most vulnerable target: " + myCurrentTarget.myName);

            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Snipe");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformSnipe(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Shoot most vulnerable target
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, shoot.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, shoot))
        {
            Debug.Log("Skeleton Archer using shoot against most vulnerable target: " + myCurrentTarget.myName);

            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Shoot");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformShoot(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Shoot the target with lowest current HP
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, false, true), shoot.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, shoot))
        {
            
            SetTargetDefender(EntityLogic.GetBestTarget(this, false, true));
            Debug.Log("Skeleton Archer using shoot against most lowest HP target: " + myCurrentTarget.myName);

            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Shoot");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformShoot(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Shoot the closest target if the most vulnerable and lowest HP target cant be targetted
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, true), shoot.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, shoot))
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));
            Debug.Log("Skeleton Archer using shoot against closest target: " + myCurrentTarget.myName);

            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Shoot");
            yield return new WaitForSeconds(0.5f);

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
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, EntityLogic.GetBestTarget(this, true), shoot.abilityRange, EntityLogic.GetTotalMobility(this)) != null &&
            EntityLogic.IsAbilityUseable(this, move)
            )
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));
            Debug.Log("Skeleton Archer moving towards: " + myCurrentTarget.myName);

            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, shoot.abilityRange, EntityLogic.GetTotalMobility(this));
            
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move");
            yield return new WaitForSeconds(0.5f);

            Action movementAction = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();
    }
    

}
