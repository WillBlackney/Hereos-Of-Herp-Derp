using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBarbarian : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();

        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Charge");
        mySpellBook.EnemyLearnAbility("Whirlwind");

        myPassiveManager.ModifyUndead();
        myPassiveManager.ModifyEnrage(1);
    }       
    
    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability charge = mySpellBook.GetAbilityByName("Charge");
        Ability whirlwind = mySpellBook.GetAbilityByName("Whirlwind");

        ActionStart:

        SetTargetDefender(EntityLogic.GetClosestEnemy(this));
        // below line used later to prevent charging this is already in melee with
        List<Tile> tilesInMyMeleeRange = LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, tile);

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }

        // Charge
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, charge.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, charge) &&
            tilesInMyMeleeRange.Contains(myCurrentTarget.tile) == false &&
            EntityLogic.IsAbleToMove(this)
            )
        {            
            Tile destination = AILogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, charge.abilityRange);
            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Charge", false));
            yield return new WaitForSeconds(0.5f);
            Action chargeAction = AbilityLogic.Instance.PerformCharge(this, myCurrentTarget, destination);
            yield return new WaitUntil(() => chargeAction.ActionResolved() == true);
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }        

        // Whirlwind
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, whirlwind))
        {            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Whirlwind", false));
            yield return new WaitForSeconds(1f);
            Action action = AbilityLogic.Instance.PerformWhirlwind(this);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike", false));
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
            EntityLogic.IsAbilityUseable(this, move))
        {            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            Tile destination = AILogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, currentMobility);
            Action movementAction = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();
    }

    
}
