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

        myPassiveManager.LearnEnrage(1);
    }       
    
    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability charge = mySpellBook.GetAbilityByName("Charge");
        Ability whirlwind = mySpellBook.GetAbilityByName("Whirlwind");

        ActionStart:

        SetTargetDefender(GetClosestDefender());
        // below line used later to prevent charging this is already in melee with
        List<TileScript> tilesInMyMeleeRange = LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, TileCurrentlyOn);

        if (IsAbleToTakeActions() == false)
        {
            EndMyActivation();
        }

        // Charge
        else if (IsAbilityOffCooldown(charge.abilityCurrentCooldownTime) &&
            IsTargetInRange(myCurrentTarget, charge.abilityRange) &&
            HasEnoughAP(currentAP, charge.abilityAPCost) &&
            tilesInMyMeleeRange.Contains(myCurrentTarget.TileCurrentlyOn) == false &&
            IsAbleToMove()
            )
        {            
            TileScript destination = AILogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, charge.abilityRange);
            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Charge", false));
            yield return new WaitForSeconds(0.5f);
            AbilityLogic.Instance.PerformCharge(this, myCurrentTarget, destination);            
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }        

        // Whirlwind
        else if (IsTargetInRange(myCurrentTarget, currentMeleeRange) &&
            HasEnoughAP(currentAP, whirlwind.abilityAPCost) &&
            IsAbilityOffCooldown(whirlwind.abilityCurrentCooldownTime))
        {            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Whirlwind", false));
            yield return new WaitForSeconds(1f);
            AbilityLogic.Instance.PerformWhirlwind(this);
            
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (IsTargetInRange(myCurrentTarget, currentMeleeRange) && HasEnoughAP(currentAP, strike.abilityAPCost))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike", false));
            yield return new WaitForSeconds(0.5f);

            AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (IsTargetInRange(myCurrentTarget, currentMeleeRange) == false && IsAbleToMove() && HasEnoughAP(currentAP, move.abilityAPCost))
        {            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            TileScript destination = AILogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, currentMobility);
            AbilityLogic.Instance.PerformMove(this, destination);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();
    }

    
}
