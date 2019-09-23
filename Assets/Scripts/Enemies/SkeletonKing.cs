using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKing : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();

        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Doom");
        mySpellBook.EnemyLearnAbility("Whirlwind");
        mySpellBook.EnemyLearnAbility("Crushing Blow");

        myPassiveManager.LearnSoulDrainAura(1);
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability doom = mySpellBook.GetAbilityByName("Doom");
        Ability whirlwind = mySpellBook.GetAbilityByName("Whirlwind");
        Ability crushingBlow = mySpellBook.GetAbilityByName("Crushing Blow");

        ActionStart:

        if (IsAbleToTakeActions() == false)
        {
            EndMyActivation();
        }

        // Doom
        else if (IsAbilityOffCooldown(doom.abilityCurrentCooldownTime) &&
            HasEnoughAP(currentAP, doom.abilityAPCost)
            )
        {            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Doom", false));
            yield return new WaitForSeconds(0.5f);
            AbilityLogic.Instance.PerformDoom(this);
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }

        // Crushing Blow
        else if (IsTargetInRange(GetClosestDefender(), currentMeleeRange) &&
           HasEnoughAP(currentAP, crushingBlow.abilityAPCost) &&
           IsAbilityOffCooldown(crushingBlow.abilityCurrentCooldownTime))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Crushing Blow", false));
            yield return new WaitForSeconds(0.6f);

            AbilityLogic.Instance.PerformCrushingBlow(this, GetClosestDefender());            
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        
        // try to move to a position that we can hit two or more enemies with a whirlwind from
        else if (
            IsAdjacentToTwoOrMoreDefenders() == false && 
            GetClosestValidTileThatHasTwoAdjacentDefenders() != null &&             
            LevelManager.Instance.GetTilesWithinRange(currentMobility, TileCurrentlyOn).Contains(GetClosestValidTileThatHasTwoAdjacentDefenders()) &&
            HasEnoughAP(currentAP, move.abilityAPCost) &&            
            IsAbilityOffCooldown(whirlwind.abilityCurrentCooldownTime)
            )
        {   
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);
            
            AbilityLogic.Instance.PerformMove(this, GetClosestValidTileThatHasTwoAdjacentDefenders());

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }      
        

        // Whirlwind
        else if (IsTargetInRange(GetClosestDefender(), currentMeleeRange) &&
            HasEnoughAP(currentAP, whirlwind.abilityAPCost) &&
            IsAbilityOffCooldown(whirlwind.abilityCurrentCooldownTime))
        {            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Whirlwind", false));
            yield return new WaitForSeconds(1f);
            AbilityLogic.Instance.PerformWhirlwind(this);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (IsTargetInRange(GetClosestDefender(), currentMeleeRange) && HasEnoughAP(currentAP, strike.abilityAPCost))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike", false));
            yield return new WaitForSeconds(0.5f);

            AbilityLogic.Instance.PerformStrike(this, GetClosestDefender());
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (IsTargetInRange(GetClosestDefender(), currentMeleeRange) == false && IsAbleToMove() && HasEnoughAP(currentAP, move.abilityAPCost))
        {
            SetTargetDefender(GetClosestDefender());
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            TileScript destination = AILogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, currentMobility);
            AbilityLogic.Instance.PerformMove(this, destination);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();
    }

    public bool IsAdjacentToTwoOrMoreDefenders()
    {
        Debug.Log("IsAdjacentToTwoOrMoreDefenders() called...");

        int adjacentDefenders = 0;
        List<TileScript> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, TileCurrentlyOn);

        foreach (Defender defender in DefenderManager.Instance.allDefenders)
        {
            if (adjacentTiles.Contains(defender.TileCurrentlyOn))
            {
                adjacentDefenders++;
            }
        }

        Debug.Log("Skeleton King is adjacent to " + adjacentDefenders.ToString() + " defenders");

        if (adjacentDefenders >= 2)
        {
            Debug.Log("IsAdjacentToTwoOrMoreDefenders() returned true...");
            return true;
        }
        else
        {
            Debug.Log("IsAdjacentToTwoOrMoreDefenders() returned false...");
            return false;
        }

    }
    public TileScript GetClosestValidTileThatHasTwoAdjacentDefenders()
    {
        Debug.Log("GetClosestTileThatHasTwoAdjacentDefenders() called...");
        TileScript tileReturned = null;
        List<TileScript> tilesWithTwoAdjacentDefenders = new List<TileScript>();

        foreach (TileScript tile in LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary())
        {
            int numberOfAdjacentEnemies = 0;
            List<TileScript> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, tile);
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                if (adjacentTiles.Contains(defender.TileCurrentlyOn))
                {
                    numberOfAdjacentEnemies++;
                }
            }

            if (numberOfAdjacentEnemies >= 2)
            {                
                tilesWithTwoAdjacentDefenders.Add(tile);
            }
        }
        Debug.Log("Tiles found that have atleast 2 adjacent defenders: " + tilesWithTwoAdjacentDefenders.Count.ToString());

        tileReturned = LevelManager.Instance.GetClosestValidTile(tilesWithTwoAdjacentDefenders, TileCurrentlyOn);
        if(tileReturned == null)
        {
            Debug.Log("GetClosestTileThatHasTwoAdjacentDefenders() could not find a valid tile with 2 adjacent defenders, returning null...");
        }
        return tileReturned;
    }
   

    
}
