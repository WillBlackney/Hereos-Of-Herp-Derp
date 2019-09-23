using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Twin Strike");
        mySpellBook.EnemyLearnAbility("Siphon Life");
        mySpellBook.EnemyLearnAbility("Teleport");
        mySpellBook.EnemyLearnAbility("Fire Ball");

        myPassiveManager.LearnRegeneration(3);

    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability siphonLife = mySpellBook.GetAbilityByName("Siphon Life");
        Ability twinStrike = mySpellBook.GetAbilityByName("Twin Strike");
        Ability teleport = mySpellBook.GetAbilityByName("Teleport");
        Ability fireBall = mySpellBook.GetAbilityByName("Fire Ball");

        ChooseRandomTargetingLogic();

        ActionStart:     
        

        if(myCurrentTarget.currentHealth <= 0 || myCurrentTarget == null)
        {
            ChooseRandomTargetingLogic();
        }

        if (IsAbleToTakeActions() == false)
        {
            EndMyActivation();
        }

        // Siphon Life
        else if(IsAbilityOffCooldown(siphonLife.abilityCurrentCooldownTime) &&
            IsTargetInRange(myCurrentTarget, siphonLife.abilityRange) &&
            HasEnoughAP(currentAP, siphonLife.abilityAPCost)
            )
        {
            //SetTargetDefender(GetClosestDefender());
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Siphon Life", false));
            yield return new WaitForSeconds(0.5f);
            AbilityLogic.Instance.PerformSiphonLife(this, myCurrentTarget);
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // If unable to siphon life the ideal target, siphon the closest valid target
        else if (IsAbilityOffCooldown(siphonLife.abilityCurrentCooldownTime) &&
            IsTargetInRange(GetClosestDefender(), siphonLife.abilityRange) &&
            HasEnoughAP(currentAP, siphonLife.abilityAPCost)
            )
        {
            SetTargetDefender(GetClosestDefender());
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Siphon Life", false));
            yield return new WaitForSeconds(0.5f);
            AbilityLogic.Instance.PerformSiphonLife(this, myCurrentTarget);
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Twin Strike
        else if (IsTargetInRange(myCurrentTarget, currentMeleeRange) &&
            HasEnoughAP(currentAP, twinStrike.abilityAPCost) &&
            IsAbilityOffCooldown(twinStrike.abilityCurrentCooldownTime))
        {
            //SetTargetDefender(GetDefenderWithLowestCurrentHP());
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Twin Strike", false));
            yield return new WaitForSeconds(0.5f);
            AbilityLogic.Instance.PerformTwinStrike(this, myCurrentTarget);
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Fireball
        else if (IsTargetInRange(myCurrentTarget, fireBall.abilityRange) &&            
            HasEnoughAP(currentAP, fireBall.abilityAPCost) &&
            IsAbilityOffCooldown(fireBall.abilityCurrentCooldownTime))
        {
            //SetTargetDefender(GetMostVulnerableDefender());
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Fire Ball", false));
            yield return new WaitForSeconds(0.5f);
            AbilityLogic.Instance.PerformFireBall(this, myCurrentTarget);
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }
               

        // Move
        else if (IsTargetInRange(myCurrentTarget,fireBall.abilityRange) == false &&
            IsAbleToMove() &&
            HasEnoughAP(currentAP, move.abilityAPCost) &&
            IsAbilityOffCooldown(move.abilityCurrentCooldownTime)
            )
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            TileScript destination = AILogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, fireBall.abilityRange, currentMobility);
            AbilityLogic.Instance.PerformMove(this, destination);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        else if (AILogic.IsEngagedInMelee(this) &&
            IsAbilityOffCooldown(teleport.abilityCurrentCooldownTime) &&
            HasEnoughAP(currentAP, teleport.abilityAPCost)
                )
        {            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Teleport", false));
            yield return new WaitForSeconds(0.5f);
            AbilityLogic.Instance.PerformTeleport(this, GetBestTeleportLocation(myCurrentTarget.TileCurrentlyOn));

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();
    }

    public void ChooseRandomTargetingLogic()
    {
        Debug.Log("ChooseRandomTargetingLogic() called...");
        int randomNumber = Random.Range(0, 2);

        if(randomNumber == 0)
        {
            SetTargetDefender(GetClosestDefender());
        }

        else
        {
            SetTargetDefender(GetDefenderWithLowestCurrentHP());
        }

        Debug.Log("ChooseRandomTargetingLogic() set target as: " + myCurrentTarget.name);
    }

    public TileScript GetBestTeleportLocation(TileScript tileEscapingFrom)
    {
        TileScript furthestTile = null;
        List <TileScript> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();

        float largestDistance = 0;

        foreach(TileScript tile in allTiles)
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

                /*
                foreach (Defender defender in DefenderManager.Instance.allDefenders)
                {
                    distancefromThisTile = Vector2.Distance(defender.TileCurrentlyOn.WorldPosition, tile.WorldPosition);

                    if (distancefromThisTile > largestDistance)
                    {
                        furthestTile = tile;
                        largestDistance = distancefromThisTile;
                        Debug.Log("Furthest tile distance is now: " + largestDistance.ToString());
                    }
                }
                */
            }          
        }

        return furthestTile;
    }

    
}
