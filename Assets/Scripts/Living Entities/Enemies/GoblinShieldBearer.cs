using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinShieldBearer : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Goblin Shield Bearer";
        base.SetBaseProperties();

        CharacterModelController.SetUpAsGoblinShieldBearerPreset(myModel);

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Provoke");
        mySpellBook.EnemyLearnAbility("Testudo");

        myPassiveManager.ModifyNimble(1);
        myPassiveManager.ModifyCautious(4);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Sword");
        myOffHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Shield");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability provoke = mySpellBook.GetAbilityByName("Provoke");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability testudo = mySpellBook.GetAbilityByName("Testudo");

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

        // Provoke
        else if (EntityLogic.IsAbilityUseable(this, provoke) &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange))
        {
            Action action = AbilityLogic.Instance.PerformProvoke(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (EntityLogic.IsAbilityUseable(this, strike) &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange))
        {
            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Testudo, if in melee
        else if (EntityLogic.IsAbilityUseable(this, strike) &&
            EntityLogic.GetAllEnemiesWithinRange(this, currentMeleeRange).Count > 0)
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

        // Can't do anything more, end activation
        else
        {
            LivingEntityManager.Instance.EndEntityActivation(this);
        }
    }
}
