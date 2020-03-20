using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonGolem : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Poison Golem";
        base.SetBaseProperties();

        CharacterModelController.SetUpAsPoisonGolemPreset(myModel);

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Noxious Fumes");
        mySpellBook.EnemyLearnAbility("Toxic Slash");

        myPassiveManager.ModifyToxicAura(2);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Staff");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability toxicSlash = mySpellBook.GetAbilityByName("Toxic Slash");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability noxiousFumes = mySpellBook.GetAbilityByName("Noxious Fumes");


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

        // Noxious Fumes
        else if (EntityLogic.GetAllEnemiesWithinRange(this, 1).Count > 1 &&
            EntityLogic.IsAbilityUseable(this, noxiousFumes))
        {
            Action action = AbilityLogic.Instance.PerformNoxiousFumes(this);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Toxic Slash
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, toxicSlash))
        {
            Action action = AbilityLogic.Instance.PerformToxicSlash(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike))
        {
            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (
            EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) == false &&
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
