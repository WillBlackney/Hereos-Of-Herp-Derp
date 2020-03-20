using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPriest : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Skeleton Priest";
        base.SetBaseProperties();       

        CharacterModelController.SetUpAsSkeletonPriestPreset(myModel);

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Strike");       
        mySpellBook.EnemyLearnAbility("Shadow Blast");
        mySpellBook.EnemyLearnAbility("Healing Light");

        //myPassiveManager.ModifyUndead();
        myPassiveManager.ModifyEncouragingAura(10);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Staff");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability healingLight = mySpellBook.GetAbilityByName("Healing Light");
        Ability shadowBlast = mySpellBook.GetAbilityByName("Shadow Blast");

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
        
        // Healing Light
        else if (EntityLogic.IsTargetInRange(this, GetBestHealingLightTarget(), healingLight.abilityRange) &&
            GetBestHealingLightTarget().currentHealth < GetBestHealingLightTarget().currentMaxHealth &&
            EntityLogic.IsAbilityUseable(this, healingLight))
        {
            Action action = AbilityLogic.Instance.PerformHealingLight(this, GetBestHealingLightTarget());
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Shadow Blast the closest enemy
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, shadowBlast.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, shadowBlast))
        {
            Action action = AbilityLogic.Instance.PerformShadowBlast(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike))
        {
            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, shadowBlast.abilityRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, shadowBlast, this) &&
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

    public Enemy GetBestInvigorateTarget(int range)
    {
        Enemy bestTarget = null;

        foreach(Enemy enemy in EnemyManager.Instance.allEnemies)
        {
            if(enemy.currentEnergy < enemy.currentMaxEnergy &&
                EntityLogic.IsTargetInRange(this, enemy, range) &&
                enemy != this)
            {
                bestTarget = enemy;
            }
        }

        if(bestTarget == null)
        {
            Debug.Log("GetBestInvigorateTarget() is null, returning caster as best target");
            bestTarget = this;
        }
        Debug.Log("GetBestInvigorateTarget() return character: " + bestTarget.name);
        return bestTarget;
    }       

    public Enemy GetBestHealingLightTarget()
    {
        Enemy bestTarget = null;
        int lowestHPvalue = 1000;

        foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
        {
            if (enemy.currentHealth < lowestHPvalue &&
                enemy.currentHealth < enemy.currentMaxHealth)
            {
                bestTarget = enemy;
                lowestHPvalue = enemy.currentHealth;
            }
        }

        if(bestTarget == null)
        {
            Debug.Log("GetBestHealingLightTarget() is null, returning caster as best target");
            bestTarget = this;
        }

        return bestTarget;
    }
}
