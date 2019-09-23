using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPriest : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();

        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Invigorate");
        mySpellBook.EnemyLearnAbility("Healing Light");
        myPassiveManager.LearnEncouragingPresence(1);

    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability invigorate = mySpellBook.GetAbilityByName("Invigorate");
        Ability healingLight = mySpellBook.GetAbilityByName("Healing Light");


        /* AI LOGIC
        if invigorate ready, invigorate
        Find the closest wounded ally
        if heal ready and wounded ally in range, heal. 
        If closest wounded ally is not in range and move ready, move towards them. 
        Check if all allies are at full hp
        GetClosestAlly(dont include self)
        If all allies are at full health, and move ready, move towards ally
        will try to move adjacent to an ally to give the protector bonus. 
        If no allies are in range to move next to, it will try to move into a grass tile. 
        If no grass tile is within range, it will stand still. 
        Only attacks enemies with strike if all allies are dead, and will target the closest enemy.
        */

        ActionStart:

        if (IsAbleToTakeActions() == false)
        {
            EndMyActivation();
        }
        
        // Invigorate
        else if (IsTargetInRange(GetBestInvigorateTarget(invigorate.abilityRange), invigorate.abilityRange) &&
            IsAbilityOffCooldown(invigorate.abilityCurrentCooldownTime) &&
            HasEnoughAP(currentAP, invigorate.abilityAPCost))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Invigorate", false));
            yield return new WaitForSeconds(0.5f);

            AbilityLogic.Instance.PerformInvigorate(this, GetBestInvigorateTarget(invigorate.abilityRange));            
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Healing Light
        else if (IsTargetInRange(GetBestHealingLightTarget(), healingLight.abilityRange) &&
            GetBestHealingLightTarget().currentHealth < GetBestHealingLightTarget().currentMaxHealth &&
            IsAbilityOffCooldown(healingLight.abilityCurrentCooldownTime) &&
            HasEnoughAP(currentAP, healingLight.abilityAPCost))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Healing Light", false));
            yield return new WaitForSeconds(0.5f);

            AbilityLogic.Instance.PerformHealingLight(this, GetBestHealingLightTarget());
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }        

        // Move towards an ally to give Encouraging presence bonus
        else if (GetClosestFriendlyTarget() != this &&
            IsTargetInRange(GetClosestFriendlyTarget(), currentMobility) == false &&
            IsAbleToMove() &&
            HasEnoughAP(currentAP, move.abilityAPCost) &&
            IsAbilityOffCooldown(move.abilityCurrentCooldownTime)
            )
        {            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);
            TileScript destination = AILogic.GetBestValidMoveLocationBetweenMeAndTarget(this, GetClosestFriendlyTarget(), 1, currentMobility);
            AbilityLogic.Instance.PerformMove(this, destination);            

            // small delay here in order to seperate the two actions a bit.
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

        EndMyActivation();

    }

    public Enemy GetBestInvigorateTarget(int range)
    {
        Enemy bestTarget = null;

        foreach(Enemy enemy in EnemyManager.Instance.allEnemies)
        {
            if(enemy.currentAP < enemy.currentMaxAP &&
                IsTargetInRange(enemy, range) &&
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
