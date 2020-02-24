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

        myPassiveManager.ModifyUndead();
        myPassiveManager.ModifyPhasing(1);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Staff");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability fireBall = mySpellBook.GetAbilityByName("Fire Ball");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability frostBolt = mySpellBook.GetAbilityByName("Frost Bolt");

        ActionStart:

        SetTargetDefender(EntityLogic.GetBestTarget(this, true));

        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }

        // Frost Bolt
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, frostBolt.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, frostBolt) &&
            myCurrentTarget.myPassiveManager.immobilized == false
            )
        { 
            // VFX notification
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Frost Bolt");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformFrostBolt(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }


        // Fireball the most vulnerable target
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, false, false, true), fireBall.abilityRange) &&            
            EntityLogic.IsAbilityUseable(this, fireBall))
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, false, false, true));

            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Fire Ball");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformFireBall(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }

        // Fireball the target with lowest current HP
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, false, true), fireBall.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, fireBall))
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, false, true));

            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Fire Ball");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformFireBall(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Fireball the closest target if the most vulnerable and the weakest cant be targetted
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, fireBall.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, fireBall))
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Fire Ball");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformFireBall(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }
        
        // Move
        else if (
            EntityLogic.IsTargetInRange(this, myCurrentTarget, fireBall.abilityRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, fireBall, this) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this)) != null
            )
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move");
            yield return new WaitForSeconds(0.5f);

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
            EndMyActivation();
        }
    }
}
