using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghoul : Enemy
{

    public override void SetBaseProperties()
    {
        base.SetBaseProperties();

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Acid Spit");
        mySpellBook.EnemyLearnAbility("Guard");

        myPassiveManager.ModifyUndead();
        myPassiveManager.ModifyToxicAura(2);

    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability acidSpit = mySpellBook.GetAbilityByName("Acid Spit");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability guard = mySpellBook.GetAbilityByName("Guard");

        ActionStart:

        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        SetTargetDefender(EntityLogic.GetClosestValidEnemy(this));

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();

        }

        // Acid Spit
        else if (EntityLogic.IsAbilityUseable(this, acidSpit) &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, acidSpit.abilityRange))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Acid Spit", false));
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformAcidSpit(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Guard an ally if they are injured and in range
        else if (GetBestBarrierTargetInRange(guard.abilityRange) != null &&
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
        else if (EntityLogic.IsAbilityUseable(this, strike) &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike", false));
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, strike, this) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this)) != null &&
            EntityLogic.IsAbilityUseable(this, move))
        {
            SetTargetDefender(EntityLogic.GetClosestValidEnemy(this));

            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this));           

            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            Action movementAction = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();
    }
    public Enemy GetBestBarrierTargetInRange(int range)
    {
        Enemy bestTarget = null;
        int lowestHPvalue = 1000;

        foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
        {
            if (enemy.currentHealth < lowestHPvalue &&
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
