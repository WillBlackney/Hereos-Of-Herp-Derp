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
        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Summon Undead");        
        mySpellBook.EnemyLearnAbility("Chaos Bolt");

        myPassiveManager.ModifyUnhygienic(2);
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability summonUndead = mySpellBook.GetAbilityByName("Summon Undead");        
        Ability chaosBolt = mySpellBook.GetAbilityByName("Chaos Bolt");

        ActionStart:

        SetTargetDefender(EntityLogic.GetClosestEnemy(this));

        // if unable to do anything, just end activation
        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }

        // try move to grass/better position if there is one in range of mobility
        else if (EntityLogic.IsAbleToMove(this) &&
            EntityLogic.GetValidGrassTileWithinRange(this, currentMobility) != null &&
            tile.myTileType != Tile.TileType.Grass &&
            EntityLogic.IsAbilityUseable(this,move)
            )
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            Action movementAction = AbilityLogic.Instance.PerformMove(this, EntityLogic.GetValidGrassTileWithinRange(this,currentMobility));
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Chaos Bolt
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestEnemy(this), chaosBolt.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, chaosBolt) &&
            IsThereAtleastOneZombie()                      
            )
        {
            SetTargetDefender(EntityLogic.GetClosestEnemy(this));
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Chaos Bolt", false));
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformChaosBolt(this, EntityLogic.GetClosestEnemy(this));
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Summon skeletons
        else if (EntityLogic.IsAbilityUseable(this, summonUndead))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Summon Undead", false));
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformSummonUndead(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);
            // update panel arrow pos
            ActivationManager.Instance.MoveArrowTowardsTargetPanelPos(ActivationManager.Instance.entityActivated.myActivationWindow);
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
