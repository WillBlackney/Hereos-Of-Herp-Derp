using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirGolem : Enemy
{

    public override void SetBaseProperties()
    {
        myName = "Air Golem";
        base.SetBaseProperties();

        CharacterModelController.SetUpAsAirGolemPreset(myModel);

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Lightning Bolt");
        mySpellBook.EnemyLearnAbility("Thunder Strike");
        mySpellBook.EnemyLearnAbility("Chain Lightning");

        myPassiveManager.ModifyStormAura(3);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Staff");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability thunderStrike = mySpellBook.GetAbilityByName("Thunder Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability lightningBolt = mySpellBook.GetAbilityByName("Lightning Bolt");
        Ability chainLightning = mySpellBook.GetAbilityByName("Chain Lightning");

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

        // Thunder Strike
        else if (myCurrentTarget.myPassiveManager.shocked &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, thunderStrike, myCurrentTarget))
        {
            Action action = AbilityLogic.Instance.PerformThunderStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Chain Lightning
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, chainLightning.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, chainLightning, myCurrentTarget) &&
            EntityLogic.GetAllEnemiesWithinRange(myCurrentTarget, 1).Count > 0)
        {
            Action action = AbilityLogic.Instance.PerformChainLightning(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Lightning Bolt 
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, lightningBolt.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, lightningBolt, myCurrentTarget))
        {
            Action action = AbilityLogic.Instance.PerformLightningBolt(this, myCurrentTarget);
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
        else if (
            EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, thunderStrike, this) &&
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
