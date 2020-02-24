using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Twin Strike");
        mySpellBook.EnemyLearnAbility("Siphon Life");
        mySpellBook.EnemyLearnAbility("Dash");
        myPassiveManager.ModifyLifeSteal(1);
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability siphonLife = mySpellBook.GetAbilityByName("Siphon Life");
        Ability twinStrike = mySpellBook.GetAbilityByName("Twin Strike");
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability dash = mySpellBook.GetAbilityByName("Dash");
       

        ChooseRandomTargetingLogic();

        ActionStart:
       

        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }
        

        if(myCurrentTarget.currentHealth <= 0 || myCurrentTarget == null)
        {
            ChooseRandomTargetingLogic();
        }

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }

        // Siphon Life
        else if(EntityLogic.IsTargetInRange(this, myCurrentTarget, siphonLife.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, siphonLife )
            )
        {
            //SetTargetDefender(GetClosestDefender());
           VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Siphon Life");
            yield return new WaitForSeconds(0.5f);
            Action slAction = AbilityLogic.Instance.PerformSiphonLife(this, myCurrentTarget);
            yield return new WaitUntil(() => slAction.ActionResolved() == true);
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // If unable to siphon life the ideal target, siphon the closest valid target
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestEnemy(this), siphonLife.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, siphonLife)
            )
        {
            SetTargetDefender(EntityLogic.GetClosestEnemy(this));
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Siphon Life");
            yield return new WaitForSeconds(0.5f);
            Action slAction = AbilityLogic.Instance.PerformSiphonLife(this, myCurrentTarget);
            yield return new WaitUntil(() => slAction.ActionResolved() == true);
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Twin Strike
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, twinStrike))
        {
            //SetTargetDefender(GetDefenderWithLowestCurrentHP());
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Twin Strike");
            yield return new WaitForSeconds(0.5f);
            Action twinStrikeAction = AbilityLogic.Instance.PerformTwinStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => twinStrikeAction.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike))
        {
            //SetTargetDefender(GetDefenderWithLowestCurrentHP());
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike");
            yield return new WaitForSeconds(0.5f);
            Action strikeAction = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => strikeAction.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Dash
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, dash) &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(dash, strike, this) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, dash.abilityPrimaryValue) != null
            )
        {
            
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Dash");
            yield return new WaitForSeconds(0.5f);

            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, dash.abilityPrimaryValue);
            Action dashAction = AbilityLogic.Instance.PerformDash(this, destination);
            yield return new WaitUntil(() => dashAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget,currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, strike, this) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this)) != null
            )
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move");
            yield return new WaitForSeconds(0.5f);

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
            EndMyActivation();
        }
    }

    public void ChooseRandomTargetingLogic()
    {
        Debug.Log("ChooseRandomTargetingLogic() called...");
        int randomNumber = Random.Range(0, 2);

        if(randomNumber == 0)
        {
            SetTargetDefender(EntityLogic.GetClosestEnemy(this));
        }

        else
        {
            SetTargetDefender(EntityLogic.GetEnemyWithLowestCurrentHP(this));
        }

        Debug.Log("ChooseRandomTargetingLogic() set target as: " + myCurrentTarget.name);
    }

    public Tile GetBestTeleportLocation(Tile tileEscapingFrom)
    {
        Tile furthestTile = null;
        List <Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();

        float largestDistance = 0;

        foreach(Tile tile in allTiles)
        {            
            if(tile.IsEmpty && tile.IsWalkable)
            {
                float distancefromThisTile = 0;

                distancefromThisTile = Vector2.Distance(tileEscapingFrom.WorldPosition, tile.WorldPosition);
                if (distancefromThisTile > largestDistance)
                {
                    furthestTile = tile;
                    largestDistance = distancefromThisTile;
                    Debug.Log("Furthest tile distance is now: " + largestDistance.ToString());
                }

            }          
        }

        return furthestTile;
    }

    
}
