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

        myPassiveManager.ModifyUndead();
        myPassiveManager.ModifySoulDrainAura(1);
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability doom = mySpellBook.GetAbilityByName("Doom");
        Ability whirlwind = mySpellBook.GetAbilityByName("Whirlwind");
        Ability crushingBlow = mySpellBook.GetAbilityByName("Crushing Blow");

        ActionStart:

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }

        // Doom
        else if (EntityLogic.IsAbilityUseable(this, doom))
        {            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Doom", false));
            yield return new WaitForSeconds(0.5f);
            Action doomAction = AbilityLogic.Instance.PerformDoom(this);
            yield return new WaitUntil(() => doomAction.ActionResolved() == true);
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }

        // Crushing Blow
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestEnemy(this), currentMeleeRange) &&
           EntityLogic.IsAbilityUseable(this, crushingBlow))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Crushing Blow", false));
            yield return new WaitForSeconds(0.6f);

            Action cbAction = AbilityLogic.Instance.PerformCrushingBlow(this, EntityLogic.GetClosestEnemy(this));
            yield return new WaitUntil(() => cbAction.ActionResolved() == true);
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        
        // try to move to a position that we can hit two or more enemies with a whirlwind from
        else if (
            IsAdjacentToTwoOrMoreDefenders() == false && 
            GetClosestValidTileThatHasTwoAdjacentDefenders() != null &&             
            LevelManager.Instance.GetTilesWithinRange(currentMobility, tile).Contains(GetClosestValidTileThatHasTwoAdjacentDefenders()) &&
            EntityLogic.IsAbilityUseable(this, whirlwind) &&
            EntityLogic.IsAbleToMove(this)
            )
        {   
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);
            
            Action movementAction = AbilityLogic.Instance.PerformMove(this, GetClosestValidTileThatHasTwoAdjacentDefenders());
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }      
        

        // Whirlwind
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestEnemy(this), currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, whirlwind))
        {            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Whirlwind", false));
            yield return new WaitForSeconds(1f);
            Action whirlwindAction = AbilityLogic.Instance.PerformWhirlwind(this);
            yield return new WaitUntil(() => whirlwindAction.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Strike
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestEnemy(this), currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike", false));
            yield return new WaitForSeconds(0.5f);

            Action strikeAction = AbilityLogic.Instance.PerformStrike(this, EntityLogic.GetClosestEnemy(this));
            yield return new WaitUntil(() => strikeAction.ActionResolved() == true);
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestEnemy(this), currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move))
        {
            SetTargetDefender(EntityLogic.GetClosestEnemy(this));
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.5f);

            Tile destination = AILogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, currentMobility);
            Action movementAction = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

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
        List<Tile> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, tile);

        foreach (Defender defender in DefenderManager.Instance.allDefenders)
        {
            if (adjacentTiles.Contains(defender.tile))
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
    public Tile GetClosestValidTileThatHasTwoAdjacentDefenders()
    {
        Debug.Log("GetClosestTileThatHasTwoAdjacentDefenders() called...");
        Tile tileReturned = null;
        List<Tile> tilesWithTwoAdjacentDefenders = new List<Tile>();

        foreach (Tile tile in LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary())
        {
            int numberOfAdjacentEnemies = 0;
            List<Tile> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, tile);
            foreach (Defender defender in DefenderManager.Instance.allDefenders)
            {
                if (adjacentTiles.Contains(defender.tile))
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

        tileReturned = LevelManager.Instance.GetClosestValidTile(tilesWithTwoAdjacentDefenders, tile);
        if(tileReturned == null)
        {
            Debug.Log("GetClosestTileThatHasTwoAdjacentDefenders() could not find a valid tile with 2 adjacent defenders, returning null...");
        }
        return tileReturned;
    }
   

    
}
