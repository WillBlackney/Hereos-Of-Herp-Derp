using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinStabby : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Goblin Stabby";
        base.SetBaseProperties();        

        CharacterModelController.SetUpAsGoblinStabbyPreset(myModel);

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Tendon Slash");

        myPassiveManager.ModifyNimble(1);
        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Sword");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability tendonSlash = mySpellBook.GetAbilityByName("Tendon Slash");
        Ability move = mySpellBook.GetAbilityByName("Move");

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

        // Tendon Slash
        else if (EntityLogic.IsAbilityUseable(this, tendonSlash) &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            myCurrentTarget.myPassiveManager.weakened == false)
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Tendon Slash");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformTendonSlash(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (EntityLogic.IsAbilityUseable(this, strike) &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange))
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
            //EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, strike, this) &&
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
