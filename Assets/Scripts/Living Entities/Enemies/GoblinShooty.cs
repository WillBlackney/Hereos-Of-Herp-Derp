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

        myPassiveManager.ModifyNimble(1);
        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Bow");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability shoot = mySpellBook.GetAbilityByName("Shoot");
        Ability move = mySpellBook.GetAbilityByName("Move");

        ActionStart:

        SetTargetDefender(EntityLogic.GetBestTarget(this, true));

        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        SetTargetDefender(EntityLogic.GetClosestValidEnemy(this));

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();

        }

        // Shoot
        else if (EntityLogic.IsAbilityUseable(this, shoot) &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, shoot.abilityRange))
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Shoot");
            yield return new WaitForSeconds(0.5f);

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
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, shoot.abilityRange, EntityLogic.GetTotalMobility(this)) != null
            )
        {
            SetTargetDefender(EntityLogic.GetClosestValidEnemy(this));

            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move");
            yield return new WaitForSeconds(0.5f);

            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, shoot.abilityRange, EntityLogic.GetTotalMobility(this));           
            

            Action movementAction = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();
    }
}
