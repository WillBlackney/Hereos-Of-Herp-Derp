using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkeletonNecromancer : Enemy
{
    public GameObject zombiePrefab;
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();
        
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Summon Undead");        
        mySpellBook.EnemyLearnAbility("Chaos Bolt");

        myPassiveManager.LearnUnhygienic(2);
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability summonUndead = mySpellBook.GetAbilityByName("Summon Undead");        
        Ability chaosBolt = mySpellBook.GetAbilityByName("Chaos Bolt");

        ActionStart:

        SetTargetDefender(GetClosestDefender());

        // if unable to do anything, just end activation
        if (IsAbleToTakeActions() == false)
        {
            EndMyActivation();
        }

        // try move to grass/better position if there is one in range of mobility
        else if (IsAbleToMove() &&
            GetValidGrassTileWithinRange(currentMobility) != null &&
            TileCurrentlyOn.myTileType != TileScript.TileType.Grass &&
            HasEnoughAP(currentAP, move.abilityAPCost)
            )
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            AbilityLogic.Instance.PerformMove(this, GetValidGrassTileWithinRange(currentMobility));
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Chaos Bolt
        else if (IsTargetInRange(GetClosestDefender(), chaosBolt.abilityRange) &&
            HasEnoughAP(currentAP, chaosBolt.abilityAPCost) &&
            IsAbilityOffCooldown(chaosBolt.abilityCurrentCooldownTime) &&
            IsThereAtleastOneZombie()                      
            )
        {
            SetTargetDefender(GetClosestDefender());
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Chaos Bolt", false));
            yield return new WaitForSeconds(0.5f);
            AbilityLogic.Instance.PerformChaosBolt(this, GetClosestDefender());

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Summon skeletons
                else if (IsAbilityOffCooldown(summonUndead.abilityCurrentCooldownTime) &&
            HasEnoughAP(currentAP, summonUndead.abilityAPCost))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Summon Undead", false));
            yield return new WaitForSeconds(0.5f);
            AbilityLogic.Instance.PerformSummonUndead(this, myCurrentTarget);            
            yield return new WaitForSeconds(0.5f);
            goto ActionStart;
        }        

        EndMyActivation();
    }

      

    public bool IsThereAtleastOneZombie()
    {
        Enemy zombie = null;

        foreach(Enemy enemy in EnemyManager.Instance.allEnemies)
        {
            if(enemy.myName == "Volatile Zombie")
            {
                zombie = enemy;
                break;
            }
        }

        if(zombie == null)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    
}
