using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBladeMaster : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Demon Blade Master";
        base.SetBaseProperties();

        CharacterModelController.SetUpAsDemonBladeMasterPreset(myModel);

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Twin Strike");
        mySpellBook.EnemyLearnAbility("Dash");
        mySpellBook.EnemyLearnAbility("Evasion");
        mySpellBook.EnemyLearnAbility("Tendon Slash");
        mySpellBook.EnemyLearnAbility("Disarm");

        myPassiveManager.ModifyRiposte(1);
        myPassiveManager.ModifySwordPlay(1);        

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Sword");
        myOffHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Sword");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        // Get ability data
        Ability twinSTrike = mySpellBook.GetAbilityByName("Twin Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability dash = mySpellBook.GetAbilityByName("Dash");
        Ability evasion = mySpellBook.GetAbilityByName("Evasion");
        Ability tendonSlash = mySpellBook.GetAbilityByName("Tendon Slash");
        Ability disarm = mySpellBook.GetAbilityByName("Disarm");

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

        // Evasion
        else if (EntityLogic.GetAllEnemiesWithinRange(this, currentMeleeRange).Count > 1 &&
            EntityLogic.IsAbilityUseable(this, evasion, myCurrentTarget))
        {
            Action action = AbilityLogic.Instance.PerformEvasion(this, this);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Disarm
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, disarm, myCurrentTarget))
        {
            Action action = AbilityLogic.Instance.PerformDisarm(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Tendon Slash
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, tendonSlash, myCurrentTarget))
        {
            Action action = AbilityLogic.Instance.PerformTendonSlash(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }
        
        // Twin Strike
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, twinSTrike, myCurrentTarget))
        {
            Action action = AbilityLogic.Instance.PerformTwinStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, twinSTrike, this) &&
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
