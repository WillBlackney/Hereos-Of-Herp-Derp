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

        myPassiveManager.ModifyUndead();
        myPassiveManager.ModifyThorns(2);

    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability guard = mySpellBook.GetAbilityByName("Guard");
        Ability inspire = mySpellBook.GetAbilityByName("Inspire");

        ActionStart:

        SetTargetDefender(EntityLogic.GetClosestEnemy(this));

        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }

        // Inspire best target if they are in range
        else if(EntityLogic.IsTargetInRange(this, GetBestInspireTarget(),inspire.abilityRange) &&
            EntityLogic.IsAbilityUseable(this,inspire))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Inspire", false));
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformInspire(this, GetBestInspireTarget());
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Inspire something else if the best target is not in range
        else if (GetBestInspireTargetInRange(inspire.abilityRange) != null &&
            EntityLogic.IsAbilityUseable(this,inspire))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Inspire", false));
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformInspire(this, GetBestInspireTargetInRange(inspire.abilityRange));
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Guard an ally if they are injured and in range
        else if(GetBestBarrierTargetInRange(guard.abilityRange) != null &&
            EntityLogic.IsAbilityUseable(this, guard))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Guard", false));
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformGuard(this, GetBestBarrierTargetInRange(guard.abilityRange));
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestEnemy(this), currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike))
        {
            SetTargetDefender(EntityLogic.GetClosestEnemy(this));
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike", false));
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestEnemy(this), currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this)) != null
            )
        {
            SetTargetDefender(EntityLogic.GetClosestEnemy(this));

            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this));
            Action movementAction = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

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

            if (enemy.GetComponent<SkeletonAssassin>() && EntityLogic.IsTargetInRange(this, enemy, range))
            {
                myPointScore = 5;
                if (myPointScore > pointScore)
                {
                    pointScore = myPointScore;
                    bestTarget = enemy;
                }
            }

            else if (enemy.GetComponent<SkeletonArcher>() && EntityLogic.IsTargetInRange(this, enemy, range))
            {
                myPointScore = 4;
                if (myPointScore > pointScore)
                {
                    pointScore = myPointScore;
                    bestTarget = enemy;
                }
            }

            else if (enemy.GetComponent<SkeletonBarbarian>() && EntityLogic.IsTargetInRange(this, enemy, range))
            {
                myPointScore = 3;
                if (myPointScore > pointScore)
                {
                    pointScore = myPointScore;
                    bestTarget = enemy;
                }
            }

            else if (enemy.GetComponent<SkeletonWarrior>() && EntityLogic.IsTargetInRange(this, enemy, range))
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
                EntityLogic.IsTargetInRange(this, enemy, range) &&
                enemy.currentHealth != enemy.currentMaxHealth)
            {
                bestTarget = enemy;
                lowestHPvalue = enemy.currentHealth;
            }
        }

        return bestTarget;
    }
}
