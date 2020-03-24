using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkElfRanger : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Dark Elf Ranger";
        base.SetBaseProperties();

        CharacterModelController.SetUpAsDarkElfRangerPreset(myModel);

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Shoot");
        mySpellBook.EnemyLearnAbility("Impaling Bolt");
        mySpellBook.EnemyLearnAbility("Pinning Shot");
        mySpellBook.EnemyLearnAbility("Overwatch");
        mySpellBook.EnemyLearnAbility("Snipe");

        myPassiveManager.ModifyStealth(1);
        myPassiveManager.ModifyFlux(1);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Bow");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability shoot = mySpellBook.GetAbilityByName("Shoot");
        Ability snipe = mySpellBook.GetAbilityByName("Snipe");        
        Ability impalingBolt = mySpellBook.GetAbilityByName("Impaling Bolt");
        Ability pinningShot = mySpellBook.GetAbilityByName("Pinning Shot");
        Ability overwatch = mySpellBook.GetAbilityByName("Overwatch");

    ActionStart:

        SetTargetDefender(EntityLogic.GetBestTarget(this, false, false, true));

        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            LivingEntityManager.Instance.EndEntityActivation(this);
        }

        // Impaling Bolt        
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, true), 1) &&
            EntityLogic.IsAbilityUseable(this, impalingBolt, EntityLogic.GetBestTarget(this, true))
            )
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));

            Action action = AbilityLogic.Instance.PerformImpalingBolt(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }

        // Pinning Shot       
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, true), pinningShot.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, pinningShot, EntityLogic.GetBestTarget(this, true))
            )
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));

            Action action = AbilityLogic.Instance.PerformPinningShot(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }

        // Overwatch      
        else if (EntityLogic.IsAbilityUseable(this, overwatch) &&
            myPassiveManager.overwatch == false
            )
        {
            Action action = AbilityLogic.Instance.PerformOverwatch(this);
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }

        // Snipe 
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, snipe.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, snipe, myCurrentTarget))
        {
            Action action = AbilityLogic.Instance.PerformSnipe(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Shoot most vulnerable target
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, shoot.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, shoot, myCurrentTarget))
        {
            Action action = AbilityLogic.Instance.PerformShoot(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Shoot the target with lowest current HP
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, false, true), shoot.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, shoot, EntityLogic.GetBestTarget(this, false, true)))
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, false, true));
            Action action = AbilityLogic.Instance.PerformShoot(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Shoot the closest target if the most vulnerable and lowest HP target cant be targetted
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, true), shoot.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, shoot, EntityLogic.GetBestTarget(this, true)))
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));
            Debug.Log("Skeleton Archer using shoot against closest target: " + myCurrentTarget.myName);

            Action action = AbilityLogic.Instance.PerformShoot(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Is any target in range and valid?
        // Try move into range if nothing is in range
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestTarget(this, true), shoot.abilityRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, shoot, this) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, EntityLogic.GetBestTarget(this, true), currentMeleeRange, EntityLogic.GetTotalMobility(this)) != null &&
            EntityLogic.IsAbilityUseable(this, move)
            )
        {
            SetTargetDefender(EntityLogic.GetBestTarget(this, true));
            Debug.Log("Skeleton Archer moving towards: " + myCurrentTarget.myName);

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
