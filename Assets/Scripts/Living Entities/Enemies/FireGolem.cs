using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGolem : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Fire Golem";
        base.SetBaseProperties();

        CharacterModelController.SetUpAsFireGolemPreset(myModel);

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Fire Ball");
        mySpellBook.EnemyLearnAbility("Melt");
        mySpellBook.EnemyLearnAbility("Combustion");

        myPassiveManager.ModifyFieryAura(3);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Staff");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability fireBall = mySpellBook.GetAbilityByName("Fire Ball");        
        Ability melt = mySpellBook.GetAbilityByName("Melt");
        Ability combustion = mySpellBook.GetAbilityByName("Combustion");


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

        // Melt best target
        else if (EntityLogic.GetBestMeltTarget(this) != null &&
            EntityLogic.IsTargetInRange(this, EntityLogic.GetBestMeltTarget(this), melt.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, melt, EntityLogic.GetBestMeltTarget(this)))
        {
            Action action = AbilityLogic.Instance.PerformMelt(this, EntityLogic.GetBestMeltTarget(this));
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Else, Melt current target if in range, and if it has block
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, melt.abilityRange) &&
            myCurrentTarget.currentBlock > 0 &&
            EntityLogic.IsAbilityUseable(this, melt, myCurrentTarget))
        {
            Action action = AbilityLogic.Instance.PerformMelt(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Combustion best target
        else if (EntityLogic.GetBestCombustionTarget(this) != null &&
            EntityLogic.IsTargetInRange(this, EntityLogic.GetBestCombustionTarget(this), melt.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, melt, EntityLogic.GetBestCombustionTarget(this)) &&
            EntityLogic.GetBestCombustionTarget(this).myPassiveManager.burningStacks > 5 &&
            EntityLogic.GetAllEnemiesWithinRange(EntityLogic.GetBestCombustionTarget(this), 1).Count > 1
            )
        {
            Action action = AbilityLogic.Instance.PerformCombustion(this, EntityLogic.GetBestMeltTarget(this));
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Combustion current target if burning enough
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, melt.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, melt, myCurrentTarget) &&
            myCurrentTarget.myPassiveManager.burningStacks > 5 &&
            EntityLogic.GetAllEnemiesWithinRange(myCurrentTarget, 1).Count > 1
            )
        {
            Action action = AbilityLogic.Instance.PerformCombustion(this, EntityLogic.GetBestMeltTarget(this));
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Fire ball 
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, fireBall.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, fireBall, myCurrentTarget))
        {
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
