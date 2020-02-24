using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mork : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Mork";
        base.SetBaseProperties();       

        CharacterModelController.SetUpAsMorkPreset(myModel);

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Strike");   
        mySpellBook.EnemyLearnAbility("Smash");
        mySpellBook.EnemyLearnAbility("Whirlwind");

        myPassiveManager.ModifyGrowing(1);
        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Battle Axe");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability smash = mySpellBook.GetAbilityByName("Smash");
        Ability whirlwind = mySpellBook.GetAbilityByName("Smash");


        ActionStart:

        // Pause if game over event has started
        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        // Decide on target
        SetTargetDefender(EntityLogic.GetBestTarget(this, true));

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }

        // Whirlwind
        else if (EntityLogic.GetAllEnemiesWithinRange(this, currentMeleeRange).Count > 1 &&
            EntityLogic.IsAbilityUseable(this, whirlwind))
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Whirlwind");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformWhirlwind(this);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Smash
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, smash)
            )
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Smash");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformSmash(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }

        // Strike
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike))
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
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
