using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher : Enemy
{ 

    public override void SetBaseProperties()
    {
        base.SetBaseProperties();
        mySpellBook.EnemyLearnAbility("Snipe");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Impaling Bolt");
        myPassiveManager.ModifyFleetFooted(1);
    }       

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability snipe = mySpellBook.GetAbilityByName("Snipe");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability impalingBolt = mySpellBook.GetAbilityByName("Impaling Bolt");

        ActionStart:

        if (IsAbleToTakeActions() == false)
        {
            EndMyActivation();
        }        
        
        // Impaling Bolt        
        else if(IsTargetInRange(EntityLogic.GetClosestEnemy(this), impalingBolt.abilityRange) && 
            HasEnoughAP(currentAP, impalingBolt.abilityAPCost) && 
            IsAbilityOffCooldown(impalingBolt.abilityCurrentCooldownTime)
            )           
        {
            Debug.Log("Skeleton Archer using Impaling Bolt...");
            SetTargetDefender(EntityLogic.GetClosestEnemy(this));
            // VFX notification
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Impaling Bolt", false));
            yield return new WaitForSeconds(0.5f);

            Action action= AbilityLogic.Instance.PerformImpalingBolt(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;
            
        }
        

        // Snipe the most vulnerable target
        else if (IsTargetInRange(GetMostVulnerableDefender(), snipe.abilityRange) &&
            //GetMostVulnerableDefender().isKnockedDown &&
            HasEnoughAP(currentAP, snipe.abilityAPCost) &&
            IsAbilityOffCooldown(snipe.abilityCurrentCooldownTime))
        {
            SetTargetDefender(GetMostVulnerableDefender());
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Snipe", false));
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformSnipe(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Snipe the target with lowest current HP
        else if (IsTargetInRange(GetDefenderWithLowestCurrentHP(), snipe.abilityRange) &&
            HasEnoughAP(currentAP, snipe.abilityAPCost) &&
            IsAbilityOffCooldown(snipe.abilityCurrentCooldownTime))
        {
            SetTargetDefender(GetDefenderWithLowestCurrentHP());
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Snipe", false));
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformSnipe(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Snipe the closest target if the most vulnerable and the weakest cant be targetted
        else if (IsTargetInRange(EntityLogic.GetClosestEnemy(this), snipe.abilityRange) &&
            HasEnoughAP(currentAP, snipe.abilityAPCost) &&
            IsAbilityOffCooldown(snipe.abilityCurrentCooldownTime))
        {
            SetTargetDefender(EntityLogic.GetClosestEnemy(this));
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Snipe", false));
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformSnipe(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }


        // Take a free move: try to move towards a grass tile first
        else if(myPassiveManager.fleetFooted &&
            moveActionsTakenThisTurn == 0 &&
            IsAbleToMove() &&
            EntityLogic.GetValidGrassTileWithinRange(this, currentMobility) != null &&
            tile.myTileType != Tile.TileType.Grass            
            )
        {
            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            Action movementAction = AbilityLogic.Instance.PerformMove(this, EntityLogic.GetValidGrassTileWithinRange(this,currentMobility));
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Take a free move: try to move furthest away from the closest defender if there is no nearby grass tile
        else if (myPassiveManager.fleetFooted &&
           moveActionsTakenThisTurn == 0 &&
           IsAbleToMove() &&
           EntityLogic.GetFurthestTileFromTargetWithinRange(this, EntityLogic.GetClosestEnemy(this), currentMobility) != null &&
           EntityLogic.GetFurthestTileFromTargetWithinRange(this, EntityLogic.GetClosestEnemy(this), currentMobility) != tile
           )
        {            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            Action movementAction = AbilityLogic.Instance.PerformMove(this, EntityLogic.GetFurthestTileFromTargetWithinRange(this, EntityLogic.GetClosestEnemy(this), currentMobility));
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            yield return new WaitForSeconds(1f);

            goto ActionStart;
        }        

        EndMyActivation();
    }
    

}
