using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkeletonAssassin : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();

        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Twin Strike");
        mySpellBook.EnemyLearnAbility("Dash");

        myPassiveManager.LearnPoisonous(1);
        myPassiveManager.LearnStealth();
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability dash = mySpellBook.GetAbilityByName("Dash");
        Ability twinStrike = mySpellBook.GetAbilityByName("Twin Strike");

        /* AI LOGIC
           If enemy is in melee range and twin strike ready, twin strike
           if enemy is in melee range and strike ready, strike
           If enemy is not in melee range, and dash ready, dash towards them
           if enemy is not in melee range and move ready, move towards them. 
        */

        ActionStart:
        
        if (IsAbleToTakeActions() == false)
        {
            EndMyActivation();
        }

        // Twin Strike
        else if (IsTargetInRange(GetDefenderWithLowestCurrentHP(), currentMeleeRange) &&
            HasEnoughAP(currentAP, twinStrike.abilityAPCost) &&
            IsAbilityOffCooldown(twinStrike.abilityCurrentCooldownTime))
        {
            SetTargetDefender(GetDefenderWithLowestCurrentHP());
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Twin Strike", false));
            yield return new WaitForSeconds(0.5f);

            AbilityLogic.Instance.PerformTwinStrike(this, myCurrentTarget);
            
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (IsTargetInRange(GetDefenderWithLowestCurrentHP(), currentMeleeRange) &&
            HasEnoughAP(currentAP, strike.abilityAPCost) &&
            IsAbilityOffCooldown(strike.abilityCurrentCooldownTime))
        {
            SetTargetDefender(GetDefenderWithLowestCurrentHP());
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike", false));
            yield return new WaitForSeconds(0.5f);
            
            AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Dash
        else if (IsTargetInRange(GetDefenderWithLowestCurrentHP(), currentMeleeRange) == false &&
            IsAbleToMove() &&
            HasEnoughAP(currentAP, dash.abilityAPCost) &&
            IsAbilityOffCooldown(dash.abilityCurrentCooldownTime)
            )
        {
            SetTargetDefender(GetDefenderWithLowestCurrentHP());

            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Dash", false));
            yield return new WaitForSeconds(0.5f);

            TileScript destination = AILogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, dash.abilityPrimaryValue);
            AbilityLogic.Instance.PerformDash(this, destination);
            
            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (IsTargetInRange(GetDefenderWithLowestCurrentHP(), currentMeleeRange) == false &&
            IsAbleToMove() && 
            HasEnoughAP(currentAP, move.abilityAPCost) &&
            IsAbilityOffCooldown(move.abilityCurrentCooldownTime)
            )
        {
            SetTargetDefender(GetDefenderWithLowestCurrentHP());
            
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
