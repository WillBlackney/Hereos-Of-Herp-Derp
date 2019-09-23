using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWarrior : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();

        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Guard");
        mySpellBook.EnemyLearnAbility("Inspire");

        myPassiveManager.LearnThorns(2);
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability guard = mySpellBook.GetAbilityByName("Guard");
        Ability inspire = mySpellBook.GetAbilityByName("Inspire");

        /* AI LOGIC
        If inspire is ready and best inspire target is in range and inspire ready, inspire
        prioritise archer>assassin>barbarian>warrior>self)
        if best inspire target is not in range, inspire something random within range
        if an ally is not at max hp and in range of barrier and barrier is ready, barrier them        
        if in melee range of closest target and strike ready, strike
        if move ready and not on grass and a grass tile is within movement range, move to grass
        if not in melee range of closest target and both move and strike ready, move towards them 
        (stops Ai from moving onto grass, then off the grass towards an enemy without being able to attack)
        */

        ActionStart:
        
        if (IsAbleToTakeActions() == false)
        {
            EndMyActivation();
        }

        // Inspire best target if they are in range
        else if(IsTargetInRange(GetBestInspireTarget(),inspire.abilityRange) &&
            IsAbilityOffCooldown(inspire.abilityCurrentCooldownTime) &&
            HasEnoughAP(currentAP, inspire.abilityAPCost))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Inspire", false));
            yield return new WaitForSeconds(0.5f);
            AbilityLogic.Instance.PerformInspire(this, GetBestInspireTarget());
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Inspire something else if the best target is not in range
        else if (GetBestInspireTargetInRange(inspire.abilityRange) != null &&
            IsAbilityOffCooldown(inspire.abilityCurrentCooldownTime) &&
            HasEnoughAP(currentAP, inspire.abilityAPCost))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Inspire", false));
            yield return new WaitForSeconds(0.5f);
            AbilityLogic.Instance.PerformInspire(this, GetBestInspireTargetInRange(inspire.abilityRange));

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Guard an ally if they are injured and in range
        else if(GetBestBarrierTargetInRange(guard.abilityRange) != null &&
            IsAbilityOffCooldown(guard.abilityCurrentCooldownTime) &&
            HasEnoughAP(currentAP, guard.abilityAPCost))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Guard", false));
            yield return new WaitForSeconds(0.5f);
            AbilityLogic.Instance.PerformGuard(this, GetBestBarrierTargetInRange(guard.abilityRange));

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (IsTargetInRange(GetClosestDefender(), currentMeleeRange) &&
            HasEnoughAP(currentAP, strike.abilityAPCost) &&
            IsAbilityOffCooldown(strike.abilityCurrentCooldownTime))
        {
            SetTargetDefender(GetClosestDefender());
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike", false));
            yield return new WaitForSeconds(0.5f);

            AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);

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

    public Enemy GetBestInspireTarget()
    {
        Enemy bestTarget = null;
        int pointScore = 0;

        foreach(Enemy enemy in EnemyManager.Instance.allEnemies)
        {
            int myPointScore = 0;

            if (enemy.GetComponent<SkeletonAssassin>())
            {
                myPointScore = 5;
                if(myPointScore > pointScore)
                {
                    pointScore = myPointScore;
                    bestTarget = enemy;
                }
            }

            else if (enemy.GetComponent<SkeletonArcher>())
            {
                myPointScore = 4;
                if (myPointScore > pointScore)
                {
                    pointScore = myPointScore;
                    bestTarget = enemy;
                }
            }

            else if (enemy.GetComponent<SkeletonBarbarian>())
            {
                myPointScore = 3;
                if (myPointScore > pointScore)
                {
                    pointScore = myPointScore;
                    bestTarget = enemy;
                }
            }

            else if (enemy.GetComponent<SkeletonWarrior>())
            {
                myPointScore = 2;
                if (myPointScore > pointScore)
                {
                    pointScore = myPointScore;
                    bestTarget = enemy;
                }
            }
        }

        return bestTarget;
    }

    public Enemy GetBestInspireTargetInRange(int range)
    {
        Enemy bestTarget = null;
        int pointScore = 0;

        foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
        {
            int myPointScore = 0;

            if (enemy.GetComponent<SkeletonAssassin>() && IsTargetInRange(enemy, range))
            {
                myPointScore = 5;
                if (myPointScore > pointScore)
                {
                    pointScore = myPointScore;
                    bestTarget = enemy;
                }
            }

            else if (enemy.GetComponent<SkeletonArcher>() && IsTargetInRange(enemy, range))
            {
                myPointScore = 4;
                if (myPointScore > pointScore)
                {
                    pointScore = myPointScore;
                    bestTarget = enemy;
                }
            }

            else if (enemy.GetComponent<SkeletonBarbarian>() && IsTargetInRange(enemy, range))
            {
                myPointScore = 3;
                if (myPointScore > pointScore)
                {
                    pointScore = myPointScore;
                    bestTarget = enemy;
                }
            }

            else if (enemy.GetComponent<SkeletonWarrior>() && IsTargetInRange(enemy, range))
            {
                myPointScore = 2;
                if (myPointScore > pointScore)
                {
                    pointScore = myPointScore;
                    bestTarget = enemy;
                }
            }
        }

        return bestTarget;
    }

    public Enemy GetBestBarrierTargetInRange(int range)
    {
        Enemy bestTarget = null;
        int lowestHPvalue = 1000;

        foreach(Enemy enemy in EnemyManager.Instance.allEnemies)
        {
            if(enemy.currentHealth < lowestHPvalue &&
                IsTargetInRange(enemy, range) &&
                enemy.currentHealth != enemy.currentMaxHealth)
            {
                bestTarget = enemy;
                lowestHPvalue = enemy.currentHealth;
            }
        }

        return bestTarget;
    }
}
