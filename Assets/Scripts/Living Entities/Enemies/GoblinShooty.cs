using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinShooty : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Goblin Shooty";
        base.SetBaseProperties();        

        CharacterModelController.SetUpAsGoblinShootyPreset(myModel);

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Shoot");
        mySpellBook.EnemyLearnAbility("Pinning Shot");

        myPassiveManager.ModifyNimble(1);
        myPassiveManager.ModifyQuickDraw(1);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Bow");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability shoot = mySpellBook.GetAbilityByName("Shoot");
        Ability pinningShot = mySpellBook.GetAbilityByName("Pinning Shot");

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

        // Pinning Shot
        else if (EntityLogic.IsAbilityUseable(this, pinningShot, myCurrentTarget) &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, pinningShot.abilityRange) &&
            myCurrentTarget.myPassiveManager.immobilized)
        {
            Action action = AbilityLogic.Instance.PerformPinningShot(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Shoot
        else if (EntityLogic.IsAbilityUseable(this, shoot, myCurrentTarget) &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, shoot.abilityRange))
        {
            Action action = AbilityLogic.Instance.PerformShoot(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, shoot.abilityRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, shoot, this) &&
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

        // Can't do anything more, end activation
        else
        {
            LivingEntityManager.Instance.EndEntityActivation(this);
        }
    }
}
