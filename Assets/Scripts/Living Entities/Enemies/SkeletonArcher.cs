using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher : Enemy
{ 

    public override void SetBaseProperties()
    {
        base.SetBaseProperties();
        mySpellBook.EnemyLearnAbility("Shoot");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Impaling Bolt");
        myPassiveManager.ModifyUndead();
        myPassiveManager.ModifyFleetFooted(1);
    }       

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability shoot = mySpellBook.GetAbilityByName("Shoot");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability impalingBolt = mySpellBook.GetAbilityByName("Impaling Bolt");

        ActionStart:
        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }

        // Impaling Bolt        
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestValidEnemy(this), impalingBolt.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, impalingBolt)
            )
        {
            Debug.Log("Skeleton Archer using Impaling Bolt...");
            SetTargetDefender(EntityLogic.GetClosestValidEnemy(this));
            // VFX notification
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Impaling Bolt", false));
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformImpalingBolt(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }

        // shoot target if in range
        // move into range if not in range
        // if already in range, try move to a grass tile if the grass tile is in range of enemy still
        // Snipe the most vulnerable target
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetMostVulnerableEnemy(this), shoot.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, shoot))
        {
            SetTargetDefender(EntityLogic.GetMostVulnerableEnemy(this));
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Shoot", false));
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformShoot(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Shoot the target with lowest current HP
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetEnemyWithLowestCurrentHP(this), shoot.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, shoot))
        {
            SetTargetDefender(EntityLogic.GetEnemyWithLowestCurrentHP(this));
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Shoot", false));
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformShoot(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Snipe the closest target if the most vulnerable and the weakest cant be targetted
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestValidEnemy(this), shoot.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, shoot))
        {
            SetTargetDefender(EntityLogic.GetClosestValidEnemy(this));
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Shoot", false));
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformShoot(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Is any target in range and valid?

        // Try move into range
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestValidEnemy(this), shoot.abilityRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, shoot, this) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, EntityLogic.GetClosestValidEnemy(this), shoot.abilityRange, currentMobility) != null &&
            EntityLogic.IsAbilityUseable(this, move)
            )
        {
            SetTargetDefender(EntityLogic.GetClosestValidEnemy(this));            

            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, shoot.abilityRange, currentMobility);
            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            Action movementAction = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // If we have no AP but can still make a free move, try to move towards a grass tile first
        else if (myPassiveManager.fleetFooted &&
            moveActionsTakenThisActivation == 0 &&
            currentEnergy == 0 &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.GetValidGrassTileWithinRange(this, currentMobility) != null &&
            tile.myTileType != Tile.TileType.Grass
            )
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            Action movementAction = AbilityLogic.Instance.PerformMove(this, EntityLogic.GetValidGrassTileWithinRange(this, currentMobility));
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();
    }
    

}
