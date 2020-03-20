using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkeletonAssassin : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Skeleton Assassin";
        base.SetBaseProperties();        

        CharacterModelController.SetUpAsSkeletonAssassinPreset(myModel);

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Twin Strike");        
        mySpellBook.EnemyLearnAbility("Shank");
        mySpellBook.EnemyLearnAbility("Dash");

        myPassiveManager.ModifyStealth(1);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Dagger");
        myOffHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Dagger");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability twinStrike = mySpellBook.GetAbilityByName("Twin Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability dash = mySpellBook.GetAbilityByName("Dash");
        Ability shank = mySpellBook.GetAbilityByName("Shank");

        ActionStart:

        SetTargetDefender(EntityLogic.GetBestTarget(this, false, true));

        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            LivingEntityManager.Instance.EndEntityActivation(this);
        }

        // Shank best target
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, shank))
        {
            Action action = AbilityLogic.Instance.PerformShank(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Twin Strike best target
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, twinStrike)) 
        {            
            Action action = AbilityLogic.Instance.PerformTwinStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Shank closest target
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, true), currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, shank))
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));
            
            Action action = AbilityLogic.Instance.PerformShank(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike closest target
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, true), currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, twinStrike))
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));
            
            Action action = AbilityLogic.Instance.PerformTwinStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Dash
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, false, true), currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            // dont dash if already in melee (this would trigger a free strike)
            EntityLogic.GetAllEnemiesWithinRange(this, currentMeleeRange).Count == 0 &&
            EntityLogic.IsAbilityUseable(this, dash) &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(dash, twinStrike, this) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, EntityLogic.GetBestTarget(this, false, true), currentMeleeRange, dash.abilityPrimaryValue + EntityLogic.GetTotalMobility(this)) != null
            )
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, false, true));

            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, dash.abilityPrimaryValue + EntityLogic.GetTotalMobility(this));
            Action movementAction = AbilityLogic.Instance.PerformDash(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, false, true), currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this)) != null &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, twinStrike, this)
            )
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, false, true));
            
            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this));
            Action movementAction = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // If, for whatever reason, we cant attack or move towards the weakest enemy, attack the nearest enemy

        // Shank closest target
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, true), currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, shank))
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));
           
            Action action = AbilityLogic.Instance.PerformShank(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Twin Strike
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, true), currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, twinStrike))
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));
            
            Action action = AbilityLogic.Instance.PerformTwinStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

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
