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
        mySpellBook.EnemyLearnAbility("Blight");

        myPassiveManager.ModifyUndead();
        myPassiveManager.ModifyToxicAura(2);
        
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability summonUndead = mySpellBook.GetAbilityByName("Summon Undead");        
        Ability blight = mySpellBook.GetAbilityByName("Blight");

        ActionStart:
        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        SetTargetDefender(EntityLogic.GetClosestValidEnemy(this));

        // if unable to do anything, just end activation
        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }

        // try move to grass/better position if there is one in range of mobility
        else if (EntityLogic.IsAbleToMove(this) &&
            EntityLogic.GetValidGrassTileWithinRange(this, EntityLogic.GetTotalMobility(this)) != null &&
            tile.myTileType != Tile.TileType.Grass &&
            EntityLogic.IsAbilityUseable(this,move)
            )
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            Action movementAction = AbilityLogic.Instance.PerformMove(this, EntityLogic.GetValidGrassTileWithinRange(this, EntityLogic.GetTotalMobility(this)));
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Blight
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestValidEnemy(this), blight.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, blight) &&
            IsThereAtleastOneZombie()                      
            )
        {
            SetTargetDefender(EntityLogic.GetClosestValidEnemy(this));
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Blight", false));
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformBlight(this, myCurrentTarget);
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
            yield return new WaitForSeconds(0.5f);
            goto ActionStart;
        }

        // Move in range for Blight
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestValidEnemy(this), blight.abilityRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, blight.abilityRange, EntityLogic.GetTotalMobility(this)) != null &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, blight, this)
            )
        {
            SetTargetDefender(EntityLogic.GetClosestValidEnemy(this));

            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, blight.abilityRange, EntityLogic.GetTotalMobility(this));
            Action movementAction = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
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
