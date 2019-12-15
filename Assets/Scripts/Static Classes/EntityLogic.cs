using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class EntityLogic
{
    // Get Living Entity Methods
    #region
    public static LivingEntity GetClosestEnemy(LivingEntity entity)
    {
        // TO DO: this method determines which defender is closest by drawing a straight line. It should instead calculate the closest by drawing a path to each with Astar

        LivingEntity closestTarget = null;
        float minimumDistance = Mathf.Infinity;

        // Declare new temp list for storing valid entities
        List<LivingEntity> entities = new List<LivingEntity>();

        // Add all active entities that are NOT friendly to the temp list
        foreach (LivingEntity entityy in LivingEntityManager.Instance.allLivingEntities)
        {
            if(!CombatLogic.Instance.IsTargetFriendly(entity, entityy))
            {
                entities.Add(entityy);
            }
            
        }

        // Iterate throught the temp list to find the closest defender to this enemy
        foreach (LivingEntity entityyy in entities)
        {
            float distancefromDefender = Vector2.Distance(entityyy.gameObject.transform.position, entity.transform.position);
            if (distancefromDefender < minimumDistance)
            {
                closestTarget = entityyy;
                minimumDistance = distancefromDefender;
            }
        }
        return closestTarget;
    }
    public static LivingEntity GetClosestAlly(LivingEntity entity)
    {
        LivingEntity closestAlly = null;
        float minimumDistance = Mathf.Infinity;

        foreach (LivingEntity entityy in LivingEntityManager.Instance.allLivingEntities)
        {
            if(CombatLogic.Instance.IsTargetFriendly(entityy, entity))
            {
                float distancefromThisCharacter = Vector2.Distance(entityy.gameObject.transform.position, entity.transform.position);
                if (distancefromThisCharacter < minimumDistance && entityy != entity)
                {
                    closestAlly = entityy;
                    minimumDistance = distancefromThisCharacter;
                }
            }
            
        }


        if (closestAlly == null)
        {
            closestAlly = entity;
        }

        return closestAlly;
    }
    public static LivingEntity GetMostVulnerableEnemy(LivingEntity entity)
    {
        LivingEntity bestTarget = null;
        int pointScore = 0;

        foreach (LivingEntity entityy in LivingEntityManager.Instance.allLivingEntities)
        {
            if(!CombatLogic.Instance.IsTargetFriendly(entity, entityy))
            {
                int myPointScore = 1;

                if (entityy.myPassiveManager.exposed)
                {
                    myPointScore += 1;
                }

                if (myPointScore > pointScore)
                {
                    pointScore = myPointScore;
                    bestTarget = entityy;
                }
            }
            
        }

        return bestTarget;
    }
    public static LivingEntity GetEnemyWithLowestCurrentHP(LivingEntity entity)
    {
        LivingEntity bestTarget = null;
        int lowestHP = 1000;

        // Declare new temp list for storing defender 
        List<LivingEntity> potentialEnemies = new List<LivingEntity>();

        // Add all active defenders to the temp list
        foreach (LivingEntity entityy in LivingEntityManager.Instance.allLivingEntities)
        {
            if(!CombatLogic.Instance.IsTargetFriendly(entity, entityy))
            {
                potentialEnemies.Add(entityy);
            }            
        }

        foreach (LivingEntity entityy in potentialEnemies)
        {
            if (entityy.currentHealth < lowestHP)
            {
                bestTarget = entityy;
                lowestHP = entityy.currentHealth;
            }
        }

        if (bestTarget == null)
        {
            Debug.Log("GetDefenderWithLowestCurrentHP() returning null !!...");
        }

        return bestTarget;
    }
    #endregion

    // Conditional Checks + Booleans
    #region
    public static bool IsAbleToMove(LivingEntity entity)
    {
        if(entity.myPassiveManager.pinned ||
            entity.currentMobility <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
    public static bool HasEnoughAP(LivingEntity entity, Ability ability)
    {
        if (entity.currentAP >= ability.abilityAPCost)
        {
            return true;
        }

        else if (entity.myPassiveManager.preparation)
        {
            return true;
        }

        else if (entity.myPassiveManager.fleetFooted &&
                entity.moveActionsTakenThisActivation == 0 &&
                ability.abilityName == "Move")
        {
            return true;
        }

        else
        {
            Debug.Log("Action failed: Not enough AP");
            return false;
        }
    }
    public static bool IsAbleToTakeActions(LivingEntity entity)
    {
        if (entity.myPassiveManager.stunned ||
            entity.myPassiveManager.sleeping)
        {
            Debug.Log("Action failed. Unable to take actions while stunned or sleeping");
            return false;
        }
        
        else
        {
            return true;
        }
    }
    public static bool IsTargetInRange(LivingEntity caster, LivingEntity target, int range)
    {
        List<Tile> tilesWithinMyRange = LevelManager.Instance.GetTilesWithinRange(range, caster.tile, false);

        if (target == null)
        {
            Debug.Log("IsTargetInRange() target value is null...");
            return false;
        }

        Tile targetsTile = target.tile;

        if (tilesWithinMyRange.Contains(targetsTile) && IsTargetVisible(caster, target))
        {
            Debug.Log("Target enemy is range");
            return true;
        }
        else
        {
            Debug.Log("Target enemy is NOT range");
            return false;
        }
    }
    public static bool IsTargetVisible(LivingEntity caster, LivingEntity target)
    {
        List<Tile> tilesWithinStealthSight = LevelManager.Instance.GetTilesWithinRange(1, caster.tile);

        if(target == null)
        {
            Debug.Log("IsTargetVisible() target recieved is null, returning false...");
            return false;
        }

        if (tilesWithinStealthSight.Contains(target.tile) == false &&
            (target.myPassiveManager.camoflage || target.myPassiveManager.stealth) &&
            (CombatLogic.Instance.IsTargetFriendly(caster, target) == false) &&
             caster.myPassiveManager.trueSight == false
            )
        {
            Debug.Log("Invalid target: Target is in stealth/camoflague and more than 1 tile away...");
            return false;
        }

        else
        {
            return true;
        }
    }
    public static bool IsAbilityOffCooldown(Ability ability)
    {
        if (ability.abilityCurrentCooldownTime == 0)
        {
            return true;
        }
        else
        {
            Debug.Log("Cannot use ability: Ability is on cooldown");
            return false;
        }
    }
    public static bool IsAbilityUseable(LivingEntity entity, Ability ability)
    {
        if(HasEnoughAP(entity, ability) &&
            IsAbilityOffCooldown(ability))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    // Get Tiles + Pathfinding Logic
    #region
    public static Tile GetValidGrassTileWithinRange(LivingEntity entityFrom, int range)
    {
        Tile closestGrassTile = null;
        List<Tile> adjacentTiles = LevelManager.Instance.GetTilesWithinMovementRange(range, entityFrom.tile);

        foreach (Tile tile in adjacentTiles)
        {
            if (tile.myTileType == Tile.TileType.Grass && tile.IsEmpty && tile.IsWalkable)
            {
                closestGrassTile = tile;
            }
        }

        return closestGrassTile;
    }
    public static Tile GetFurthestTileFromTargetWithinRange(LivingEntity originCharacter, LivingEntity target, int range)
    {
        // method used to enemy AI move away from a defender logically to safety
        List<Tile> tilesWithinRangeOfOriginCharacter = LevelManager.Instance.GetValidMoveableTilesWithinRange(range, originCharacter.tile);
        return LevelManager.Instance.GetFurthestTileFromTargetFromList(tilesWithinRangeOfOriginCharacter, target.tile);
    }
    public static Tile GetBestValidMoveLocationBetweenMeAndTarget(LivingEntity characterActing, LivingEntity target, int rangeFromTarget, int movePoints)
    {
        // Debugging checks
        if(characterActing == null)
        {
            Debug.Log("GetBestValidMoveLocationBetweenMeAndTarget() was given a null value for 'characterActing' argument");
        }
        if (target == null)
        {
            Debug.Log("GetBestValidMoveLocationBetweenMeAndTarget() was given a null value for 'target' argument");
        }

        Tile tileReturned = null;
        Tile tileClosestToTarget = LevelManager.Instance.GetClosestValidTile(LevelManager.Instance.GetTilesWithinRange(rangeFromTarget, target.tile), characterActing.tile);
        
        Stack<Node> pathFromMeToIdealTile = AStar.GetPath(characterActing.tile.GridPosition, tileClosestToTarget.GridPosition);

        Debug.Log("GetBestValidMoveLocationBetweenMeAndTarget() generated a path with " +
            pathFromMeToIdealTile.Count.ToString() + " tiles on it"
            );

        if (pathFromMeToIdealTile.Count > 2)
        {
            tileReturned = pathFromMeToIdealTile.ElementAt(movePoints - 1).TileRef;
        }
        else if (pathFromMeToIdealTile.Count == 2)
        {
            tileReturned = pathFromMeToIdealTile.ElementAt(0).TileRef;
        }
        else
        {
            if (pathFromMeToIdealTile.Count == 1)
            {
                if (pathFromMeToIdealTile.ElementAt(0) != null &&
                pathFromMeToIdealTile.ElementAt(0).TileRef != null)
                {
                    tileReturned = pathFromMeToIdealTile.ElementAt(0).TileRef;
                }
            }

        }

        if (tileReturned == null)
        {
            Debug.Log("GetBestValidMoveLocationBetweenMeAndTarget() could not draw a valid path from" + characterActing.name +
                " to " + target.name + ", returning a null Tile destination...");
        }

        return tileReturned;

    }

    #endregion
}
