using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWarrior : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Skeleton Warrior";
        base.SetBaseProperties();  

        CharacterModelController.SetUpAsSkeletonWarriorPreset(myModel);

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Strike");   
        mySpellBook.EnemyLearnAbility("Inspire");
        mySpellBook.EnemyLearnAbility("Provoke");
        mySpellBook.EnemyLearnAbility("Fortify");

        // myPassiveManager.ModifyUndead();
        myPassiveManager.ModifyThorns(3);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Sword");
        myOffHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Shield");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability inspire = mySpellBook.GetAbilityByName("Inspire");
        Ability fortify = mySpellBook.GetAbilityByName("Fortify");
        Ability provoke = mySpellBook.GetAbilityByName("Provoke");

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

        // Fortify
        else if (EntityLogic.IsAbilityUseable(this, fortify, EntityLogic.GetBestFortifyTarget(this)) &&
            EntityLogic.GetBestFortifyTarget(this) != null)
        {
            Action action = AbilityLogic.Instance.PerformFortify(this, EntityLogic.GetBestFortifyTarget(this));
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Inspire best target if they are in range
        else if(EntityLogic.IsTargetInRange(this, GetBestInspireTarget(),inspire.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, inspire, GetBestInspireTarget()))
        {
            Action action = AbilityLogic.Instance.PerformInspire(this, GetBestInspireTarget());
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Inspire something else if the best target is not in range
        else if (GetBestInspireTargetInRange(inspire.abilityRange) != null &&
            EntityLogic.IsAbilityUseable(this, inspire, GetBestInspireTargetInRange(inspire.abilityRange)))
        {
            Action action = AbilityLogic.Instance.PerformInspire(this, GetBestInspireTargetInRange(inspire.abilityRange));
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Provoke
        else if (EntityLogic.IsAbilityUseable(this, provoke, myCurrentTarget) &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange))
        {
            Action action = AbilityLogic.Instance.PerformProvoke(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike closest target
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike, myCurrentTarget))
        {
            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
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
