using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonHellGuard : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Demon Hell Guard";
        base.SetBaseProperties();

        CharacterModelController.SetUpAsDemonHellGuardPreset(myModel);

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Testudo");
        mySpellBook.EnemyLearnAbility("Fortify");
        mySpellBook.EnemyLearnAbility("Shield Slam");
        mySpellBook.EnemyLearnAbility("Provoke");

        myPassiveManager.ModifyUnwavering(1);
        myPassiveManager.ModifyCautious(5);        

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Sword");
        myOffHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Shield");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        // Get ability data
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability fortify = mySpellBook.GetAbilityByName("Fortify");
        Ability testudo = mySpellBook.GetAbilityByName("Testudo");
        Ability shieldSlam = mySpellBook.GetAbilityByName("Shield Slam");
        Ability provoke = mySpellBook.GetAbilityByName("Provoke");

        ActionStart:

        // Pause if game over event has started
        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        // Decide on target
        SetTargetDefender(EntityLogic.GetBestTarget(this, true));

        // End activation if stunned, out of energy, etc
        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            LivingEntityManager.Instance.EndEntityActivation(this);
        }

        // Fortify
        else if (EntityLogic.IsAbilityUseable(this, fortify, EntityLogic.GetBestFortifyTarget(this)) &&
            EntityLogic.GetBestFortifyTarget(this) != null)
        {
            Action action = AbilityLogic.Instance.PerformFortify(this, EntityLogic.GetBestFortifyTarget(this));
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Testudo
        else if (EntityLogic.IsAbilityUseable(this, testudo))
        {
            Action action = AbilityLogic.Instance.PerformTestudo(this);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Shield Slam
        else if (EntityLogic.IsAbilityUseable(this, shieldSlam, myCurrentTarget) &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            currentBlock >= 10)
        {
            Action action = AbilityLogic.Instance.PerformShieldSlam(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Provoke
        else if (EntityLogic.IsAbilityUseable(this, provoke, myCurrentTarget) &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange))
        {
            Action action = AbilityLogic.Instance.PerformProvoke(this, myCurrentTarget);
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
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, strike, this) &&
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
