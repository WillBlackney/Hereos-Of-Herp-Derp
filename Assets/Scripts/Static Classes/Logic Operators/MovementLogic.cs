using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MovementLogic : Singleton<MovementLogic>
{
    // Path Generation + AStar Logic
    public bool movementPaused;
    #region
    public Stack<Node> GeneratePath(Point start, Point end)
    {
        return AStar.GetPath(start, end);
    }
    public void SetPath(LivingEntity characterMoved, Stack<Node> path)
    {
        characterMoved.path = path;
        characterMoved.gridPosition = path.Peek().GridPosition;
        characterMoved.destination = path.Peek().WorldPosition;
    }
    #endregion

    // Conditional Checks 
    #region
    public bool IsLocationMoveable(Tile destination, LivingEntity characterMoved, int range)
    {
        //List<Tile> validTilesWithinMovementRange = LevelManager.Instance.GetValidMoveableTilesWithinRange(range, LevelManager.Instance.Tiles[characterMoved.gridPosition]);
        List<Tile> validTilesWithinMovementRange = LevelManager.Instance.GetValidMoveableTilesWithinRange(range, characterMoved.tile);

        if (validTilesWithinMovementRange.Contains(destination) &&
            destination.IsEmpty &&
            destination.IsWalkable
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    // Move Teleport + Knock Back Logic
    #region

    // Movement 
    public Action MoveEntity(LivingEntity characterMoved, Tile destination, float speed = 3, bool freeStrikeImmune = false)
    {
        Action action = new Action();
        StartCoroutine(MoveEntityCoroutine(characterMoved, destination, action, speed, freeStrikeImmune));
        return action;
    }
    public IEnumerator MoveEntityCoroutine(LivingEntity characterMoved, Tile destination, Action action, float speed = 3, bool freeStrikeImmune = false)
    {
        // Set properties
        float originalSpeed = characterMoved.speed;
        float speedOfThisMovement = speed;
        bool hasCompletedMovement = false;
        bool freeStrikesOnThisTileResolved = false;

        // Set path + destination
        SetPath(characterMoved, GeneratePath(characterMoved.gridPosition, destination.GridPosition));

        // Play movement animation
        characterMoved.myAnimator.SetTrigger("Move");

        // flip the sprite's x axis depending on the direction of movement
        PositionLogic.Instance.CalculateWhichDirectionToFace(characterMoved, destination);  

        // Commence movement
        while (hasCompletedMovement == false)
        {
            // Check for free strikes first
            if (freeStrikesOnThisTileResolved == false && characterMoved.path.Count > 0)
            {
                Debug.Log("Checking for free strikes...");
                Tile nextTile = LevelManager.Instance.GetTileFromPointReference(characterMoved.path.Peek().GridPosition);                
                Action freeStrikeCheck = ResolveFreeStrikes(characterMoved, characterMoved.tile, nextTile);
                yield return new WaitUntil(() => freeStrikeCheck.ActionResolved() == true);
                freeStrikesOnThisTileResolved = true;
            }
            
            Debug.Log("Moving to next tile on path...");
            characterMoved.transform.position = Vector2.MoveTowards(characterMoved.transform.position, characterMoved.destination, speedOfThisMovement * Time.deltaTime);

            if (characterMoved.transform.position == characterMoved.destination)
            {
                // if we have reached the next tile in our path
                if (characterMoved.path != null && characterMoved.path.Count > 0)
                {
                    Debug.Log("Next tile on path reached...");
                    Tile previousTile = characterMoved.tile;
                    characterMoved.gridPosition = characterMoved.path.Peek().GridPosition;
                    // Free up the tile we were standing on before we moved
                    LevelManager.Instance.SetTileAsUnoccupied(characterMoved.tile);
                    // Set our current tile to the tile we ended up at the end of the move
                    characterMoved.tile = LevelManager.Instance.GetTileFromPointReference(characterMoved.gridPosition);
                    // Set our current tile to be occupied, so other characters cant stack ontop of it.
                    LevelManager.Instance.SetTileAsOccupied(characterMoved.tile);
                    Action moveToNewLocation = OnLocationMovedTo(characterMoved, characterMoved.tile, previousTile, freeStrikeImmune);
                    yield return new WaitUntil(() => moveToNewLocation.ActionResolved() == true);
                    characterMoved.destination = characterMoved.path.Pop().WorldPosition;
                    freeStrikesOnThisTileResolved = false;
                }

                // if we have reached the final destination
                else if (characterMoved.path != null && characterMoved.path.Count == 0)
                {
                    Debug.Log("Last tile on path reached...");
                    Tile previousTile = characterMoved.tile;
                    // Free up the tile we were standing on before we moved
                    LevelManager.Instance.SetTileAsUnoccupied(characterMoved.tile);
                    // Set our current tile to the tile we ended up at the end of the move
                    characterMoved.tile = LevelManager.Instance.GetTileFromPointReference(characterMoved.gridPosition);
                    // Set our current tile to be occupied, so other characters cant stack ontop of it.
                    LevelManager.Instance.SetTileAsOccupied(characterMoved.tile);
                    // Prevent character from being able to move again
                    //hasMovedThisTurn = true;
                    Action moveToNewLocation = OnLocationMovedTo(characterMoved, characterMoved.tile, previousTile, freeStrikeImmune);
                    yield return new WaitUntil(() => moveToNewLocation.ActionResolved() == true);
                    Debug.Log("Final point reached, movement finished");
                    characterMoved.myAnimator.SetTrigger("Idle");
                    freeStrikesOnThisTileResolved = false;
                    hasCompletedMovement = true;
                    action.actionResolved = true;
                }
            }

            
            // Reset speed
            characterMoved.speed = originalSpeed;
            yield return null;
        }
        

    }

    // Teleportation
    public Action TeleportEntity(LivingEntity target, Tile destination, bool switchingPosWithAnotherEntity = false)
    {
        Action action = new Action();
        StartCoroutine(TeleportEntityCoroutine(target, destination,action, switchingPosWithAnotherEntity));
        return action;
    }
    public IEnumerator TeleportEntityCoroutine(LivingEntity target, Tile destination, Action action, bool switchingPosWithAnotherEntity = false)
    {
        if (!switchingPosWithAnotherEntity)
        {
            LevelManager.Instance.SetTileAsUnoccupied(target.tile);
        }

        // Make target vanish
        target.myModelParent.SetActive(false);
        target.myWorldSpaceCanvasParent.SetActive(false);
        StartCoroutine(VisualEffectManager.Instance.CreateTeleportEffect(target.transform.position));
        yield return new WaitForSeconds(0.5f);

        // Move to new location
        target.gridPosition = destination.GridPosition;
        target.tile = destination;
        target.transform.position = destination.WorldPosition;

        // Make target reappear
        StartCoroutine(VisualEffectManager.Instance.CreateTeleportEffect(target.transform.position));
        target.myModelParent.SetActive(true);
        target.myWorldSpaceCanvasParent.SetActive(true);

        // Set new tile location, resolve event
        LevelManager.Instance.SetTileAsOccupied(destination);
        OnNewTileSet(target);
        action.actionResolved = true;
        yield return null;
    }

    // Knock Back
    public Action KnockBackEntity(LivingEntity attacker, LivingEntity target, int pushBackDistance)
    {
        Action action = new Action();
        

        Debug.Log("CreateKnockBackEvent() called, starting new knockback event...");
        // First, deal the initial bolt damage
        // HandleDamage(CalculateDamage(damageAmount, target, attacker), attacker);

        Tile TileCurrentlyOn = attacker.tile;
        string direction = "unassigned";
        Tile targetTile = target.tile;
        Tile finalDestination = targetTile;
        List<Tile> tilesOnPath = new List<Tile>();
        List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        LivingEntity characterKnockedInto = null;

        // Second, calculate which direction the target will be moved towards when shot

        // South
        if (TileCurrentlyOn.GridPosition.X == targetTile.GridPosition.X &&
            TileCurrentlyOn.GridPosition.Y < targetTile.GridPosition.Y)
        {
            direction = "South";
            Debug.Log("Knockback target is south...");
            // Find all tiles that the target will move over during the knock back, then add them to a list
            foreach (Tile tile in allTiles)
            {
                if (
                    tile.GridPosition.X == targetTile.GridPosition.X &&
                    tile.GridPosition.Y > targetTile.GridPosition.Y &&
                    tile.GridPosition.Y <= targetTile.GridPosition.Y + pushBackDistance
                    )
                {
                    tilesOnPath.Add(tile);
                    Debug.Log("Tile " + tile.GridPosition.X.ToString() + ", " + tile.GridPosition.Y.ToString());
                }
            }

            // Order the list of tiles, so that the tiles closest to the target are at the start of the list
            List<Tile> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.Y).ToList();

            // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
            // This is determined looking at the next tile, and checking if it contains an enemy on it already.
            foreach (Tile tile in SortedList)
            {
                if (tile.IsEmpty && tile.IsWalkable)
                {
                    finalDestination = tile;
                }
                else
                {
                    foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                    {
                        if (enemy.tile == tile && enemy != target)
                        {
                            characterKnockedInto = enemy;
                            break;
                        }
                    }
                }
            }
        }

        // North
        else if (TileCurrentlyOn.GridPosition.X == targetTile.GridPosition.X &&
                 TileCurrentlyOn.GridPosition.Y > targetTile.GridPosition.Y)
        {
            direction = "North";

            foreach (Tile tile in allTiles)
            {
                if (
                    tile.GridPosition.X == targetTile.GridPosition.X &&
                    tile.GridPosition.Y < targetTile.GridPosition.Y &&
                    tile.GridPosition.Y >= targetTile.GridPosition.Y - pushBackDistance
                    )
                {
                    tilesOnPath.Add(tile);
                    Debug.Log("Tile " + tile.GridPosition.X.ToString() + ", " + tile.GridPosition.Y.ToString());
                }
            }

            List<Tile> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.Y).ToList();
            SortedList.Reverse();

            foreach (Tile tile in SortedList)
            {
                if (tile.IsEmpty && tile.IsWalkable)
                {
                    finalDestination = tile;
                }
                else
                {
                    foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                    {
                        if (enemy.tile == tile && enemy != target)
                        {
                            characterKnockedInto = enemy;
                            break;
                        }
                    }
                }
            }


            LevelManager.Instance.SetTileAsUnoccupied(target.tile);
            target.gridPosition = finalDestination.GridPosition;
            target.tile = finalDestination;
            target.transform.position = finalDestination.WorldPosition;
            LevelManager.Instance.SetTileAsOccupied(finalDestination);
        }

        // East
        else if (TileCurrentlyOn.GridPosition.X < targetTile.GridPosition.X &&
                 TileCurrentlyOn.GridPosition.Y == targetTile.GridPosition.Y)
        {
            direction = "East";
            // Find all tiles that the target will move over during the knock back, then add them to a list
            foreach (Tile tile in allTiles)
            {
                if (
                    tile.GridPosition.Y == targetTile.GridPosition.Y &&
                    tile.GridPosition.X > targetTile.GridPosition.X &&
                    tile.GridPosition.X <= targetTile.GridPosition.X + pushBackDistance
                    )
                {
                    tilesOnPath.Add(tile);
                    Debug.Log("Tile " + tile.GridPosition.X.ToString() + ", " + tile.GridPosition.Y.ToString());
                }
            }

            // Order the list of tiles, so that the tiles closest to the target are at the start of the list
            List<Tile> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

            // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
            // This is determined looking at the next tile, and checking if it contains an enemy on it already.
            foreach (Tile tile in SortedList)
            {
                if (tile.IsEmpty && tile.IsWalkable)
                {
                    finalDestination = tile;
                }
                else
                {
                    foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                    {
                        if (enemy.tile == tile && enemy != target)
                        {
                            characterKnockedInto = enemy;
                            break;
                        }
                    }
                }
            }
        }

        // West
        else if (TileCurrentlyOn.GridPosition.X > targetTile.GridPosition.X &&
                 TileCurrentlyOn.GridPosition.Y == targetTile.GridPosition.Y)
        {
            direction = "West";
            // Find all tiles that the target will move over during the knock back, then add them to a list
            foreach (Tile tile in allTiles)
            {
                if (
                    tile.GridPosition.Y == targetTile.GridPosition.Y &&
                    tile.GridPosition.X < targetTile.GridPosition.X &&
                    tile.GridPosition.X >= targetTile.GridPosition.X - pushBackDistance
                    )
                {
                    tilesOnPath.Insert(0, tile);
                    Debug.Log("Tile " + tile.GridPosition.X.ToString() + ", " + tile.GridPosition.Y.ToString());
                }
            }

            // Order the list of tiles, so that the tiles closest to the target are at the start of the list
            //List<TileScript> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();
            //tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

            // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
            // This is determined looking at the next tile, and checking if it contains an enemy on it already.
            foreach (Tile tile in tilesOnPath)
            {
                if (tile.IsEmpty && tile.IsWalkable)
                {
                    finalDestination = tile;
                }
                else
                {
                    foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                    {
                        if (enemy.tile == tile && enemy != target)
                        {
                            characterKnockedInto = enemy;
                            break;
                        }
                    }
                }
            }
        }

        // South West
        else if (TileCurrentlyOn.GridPosition.X > targetTile.GridPosition.X &&
                 TileCurrentlyOn.GridPosition.Y < targetTile.GridPosition.Y)
        {
            direction = "South West";
            List<Tile> tempList = new List<Tile>();
            int xPos = 1;
            int yPos = 1;
            // Find all tiles that the target will move over during the knock back, then add them to a list

            foreach (Tile tile in allTiles)
            {
                if (tile.GridPosition.Y > targetTile.GridPosition.Y && tile.GridPosition.Y <= targetTile.GridPosition.Y + pushBackDistance &&
                    tile.GridPosition.X < targetTile.GridPosition.X && tile.GridPosition.X >= targetTile.GridPosition.X - pushBackDistance)
                {
                    tempList.Add(tile);
                }
            }

            Loop:
            foreach (Tile tile in tempList)
            {
                if (tile.GridPosition.X == targetTile.GridPosition.X - xPos)
                {
                    if (tile.GridPosition.Y == targetTile.GridPosition.Y + yPos)
                    {
                        tilesOnPath.Add(tile);
                        xPos++;
                        yPos++;
                        goto Loop;
                    }
                }
            }

            // Order the list of tiles, so that the tiles closest to the target are at the start of the list
            List<Tile> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

            // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
            // This is determined looking at the next tile, and checking if it contains an enemy on it already.
            foreach (Tile tile in tilesOnPath)
            {
                if (tile.IsEmpty && tile.IsWalkable)
                {
                    finalDestination = tile;
                }
                else
                {
                    foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                    {
                        if (enemy.tile == tile && enemy != target)
                        {
                            characterKnockedInto = enemy;
                            break;
                        }
                    }
                }
            }
        }

        // South East
        else if (TileCurrentlyOn.GridPosition.X < targetTile.GridPosition.X &&
                 TileCurrentlyOn.GridPosition.Y < targetTile.GridPosition.Y)
        {
            direction = "South East";
            List<Tile> tempList = new List<Tile>();
            int xPos = 1;
            int yPos = 1;
            // Find all tiles that the target will move over during the knock back, then add them to a list

            foreach (Tile tile in allTiles)
            {
                if (tile.GridPosition.Y > targetTile.GridPosition.Y && tile.GridPosition.Y <= targetTile.GridPosition.Y + pushBackDistance &&
                    tile.GridPosition.X > targetTile.GridPosition.X && tile.GridPosition.X <= targetTile.GridPosition.X + pushBackDistance)
                {
                    tempList.Add(tile);
                }
            }

            Loop:
            foreach (Tile tile in tempList)
            {
                if (tile.GridPosition.X == targetTile.GridPosition.X + xPos)
                {
                    if (tile.GridPosition.Y == targetTile.GridPosition.Y + yPos)
                    {
                        tilesOnPath.Add(tile);
                        xPos++;
                        yPos++;
                        goto Loop;
                    }
                }
            }

            // Order the list of tiles, so that the tiles closest to the target are at the start of the list
            List<Tile> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

            // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
            // This is determined looking at the next tile, and checking if it contains an enemy on it already.
            foreach (Tile tile in tilesOnPath)
            {
                if (tile.IsEmpty && tile.IsWalkable)
                {
                    finalDestination = tile;
                }
                else
                {
                    foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                    {
                        if (enemy.tile == tile && enemy != target)
                        {
                            characterKnockedInto = enemy;
                            break;
                        }
                    }
                }
            }
        }

        // North West
        else if (TileCurrentlyOn.GridPosition.X > targetTile.GridPosition.X &&
                 TileCurrentlyOn.GridPosition.Y > targetTile.GridPosition.Y)
        {
            direction = "North West";
            List<Tile> tempList = new List<Tile>();
            int xPos = 1;
            int yPos = 1;
            // Find all tiles that the target will move over during the knock back, then add them to a list

            foreach (Tile tile in allTiles)
            {
                if (tile.GridPosition.Y < targetTile.GridPosition.Y && tile.GridPosition.Y >= targetTile.GridPosition.Y - pushBackDistance &&
                    tile.GridPosition.X < targetTile.GridPosition.X && tile.GridPosition.X >= targetTile.GridPosition.X - pushBackDistance)
                {
                    tempList.Add(tile);
                }
            }

            Loop:
            foreach (Tile tile in tempList)
            {
                if (tile.GridPosition.X == targetTile.GridPosition.X - xPos)
                {
                    if (tile.GridPosition.Y == targetTile.GridPosition.Y - yPos)
                    {
                        tilesOnPath.Add(tile);
                        xPos++;
                        yPos++;
                        goto Loop;
                    }
                }
            }

            // Order the list of tiles, so that the tiles closest to the target are at the start of the list
            List<Tile> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

            // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
            // This is determined looking at the next tile, and checking if it contains an enemy on it already.
            foreach (Tile tile in tilesOnPath)
            {
                if (tile.IsEmpty && tile.IsWalkable)
                {
                    finalDestination = tile;
                }
                else
                {
                    foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                    {
                        if (enemy.tile == tile && enemy != target)
                        {
                            characterKnockedInto = enemy;
                            break;
                        }
                    }
                }
            }
        }

        // North East
        else if (TileCurrentlyOn.GridPosition.X < targetTile.GridPosition.X &&
                 TileCurrentlyOn.GridPosition.Y > targetTile.GridPosition.Y)
        {
            direction = "North East";
            List<Tile> tempList = new List<Tile>();
            int xPos = 1;
            int yPos = 1;
            // Find all tiles that the target will move over during the knock back, then add them to a list

            foreach (Tile tile in allTiles)
            {
                if (tile.GridPosition.Y < targetTile.GridPosition.Y && tile.GridPosition.Y >= targetTile.GridPosition.Y - pushBackDistance &&
                    tile.GridPosition.X > targetTile.GridPosition.X && tile.GridPosition.X <= targetTile.GridPosition.X + pushBackDistance)
                {
                    tempList.Add(tile);
                }
            }

            Loop:
            foreach (Tile tile in tempList)
            {
                if (tile.GridPosition.X == targetTile.GridPosition.X + xPos)
                {
                    if (tile.GridPosition.Y == targetTile.GridPosition.Y - yPos)
                    {
                        tilesOnPath.Add(tile);
                        xPos++;
                        yPos++;
                        goto Loop;
                    }
                }
            }

            // Order the list of tiles, so that the tiles closest to the target are at the start of the list
            List<Tile> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

            // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
            // This is determined looking at the next tile, and checking if it contains an enemy on it already.
            foreach (Tile tile in tilesOnPath)
            {
                if (tile.IsEmpty && tile.IsWalkable)
                {
                    finalDestination = tile;
                }
                else
                {
                    foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                    {
                        if (enemy.tile == tile && enemy != target)
                        {
                            characterKnockedInto = enemy;
                            break;
                        }
                    }
                }
            }
        }

        LevelManager.Instance.SetTileAsUnoccupied(target.tile);
        target.gridPosition = finalDestination.GridPosition;
        target.tile = finalDestination;
        StartCoroutine(KnockBackEntityCoroutine(target, finalDestination.WorldPosition, action));
        LevelManager.Instance.SetTileAsOccupied(finalDestination);


        Debug.Log("Tiles on path: " + tilesOnPath.Count.ToString());
        Debug.Log("Target is " + direction + " of the the attacker");
        return action;

    }
    public IEnumerator KnockBackEntityCoroutine(LivingEntity entityMoved, Vector3 destination, Action action)
    {
        Debug.Log("KnockBackMove() called by CombatLogic.cs....");
        bool movementCompleted = false;

        while (movementCompleted == false)
        {
            Debug.Log("KnockBackMove() moving entity...");
            entityMoved.transform.position = Vector2.MoveTowards(entityMoved.transform.position, destination, 8f * Time.deltaTime);
            if (entityMoved.transform.position == destination)
            {
                movementCompleted = true;
            }
            yield return new WaitForEndOfFrame();
        }

        OnNewTileSet(entityMoved);
        action.actionResolved = true;
    }
    #endregion

    // New Location Set Logic
    #region
    public Action OnLocationMovedTo(LivingEntity character, Tile newLocation, Tile previousLocation, bool freeStrikeImmune = false)
    {
        Debug.Log("OnLocationMovedToCalled() called....");
        Action action = new Action();
        StartCoroutine(OnLocationMovedToCoroutine(character, newLocation, previousLocation,action, freeStrikeImmune));
        return action;
    }
    public IEnumerator OnLocationMovedToCoroutine(LivingEntity character, Tile newLocation, Tile previousLocation, Action action, bool freeStrikeImmune = false)
    {
        Debug.Log("OnLocationMovedToCalledCoroutine() called....");        

        OnNewTileSet(character);       
        action.actionResolved = true;
        yield return null;
        
    }
    public void OnNewTileSet(LivingEntity character)
    {
        /*
        // Check grass tile / camo application or removal
        if (character.tile.myTileType == Tile.TileType.Grass &&
            character.myPassiveManager.camoflage == false)
        {
            character.myPassiveManager.ModifyCamoflage(1);
        }
        else if (character.tile.myTileType != Tile.TileType.Grass &&
            character.myPassiveManager.camoflage)
        {
            character.myPassiveManager.ModifyCamoflage(-character.myPassiveManager.camoflageStacks);
        }
        */
    }
    public Action ResolveFreeStrikes(LivingEntity characterMoved, Tile previousLocation, Tile newLocation)
    {
        Debug.Log("ResolveFreeStrikes() called....");
        Action action = new Action();
        StartCoroutine(ResolveFreeStrikesCoroutine(characterMoved, action, previousLocation, newLocation));
        return action;
    }
    public IEnumerator ResolveFreeStrikesCoroutine(LivingEntity characterMoved, Action action, Tile previousLocation, Tile newLocation)
    {
        if((characterMoved.defender && ArtifactManager.Instance.HasArtifact("Goblin Mask")) == false)
        {
            Debug.Log("ResolveFreeStrikesCoroutine() called....");
            List<LivingEntity> unfriendlyEntities = new List<LivingEntity>();

            foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
            {
                if (CombatLogic.Instance.IsTargetFriendly(characterMoved, entity) == false)
                {
                    unfriendlyEntities.Add(entity);
                }
            }

            foreach (LivingEntity entity in unfriendlyEntities)
            {
                if (PositionLogic.Instance.GetTilesInCharactersMeleeRange(entity).Contains(previousLocation) &&
                    PositionLogic.Instance.GetTilesInCharactersMeleeRange(entity).Contains(newLocation) == false &&
                    characterMoved.inDeathProcess == false)
                {
                    Debug.Log("ResolveFreeStrikesCoroutine() detected that " + characterMoved.name + " triggered a free strike from " + entity.name);
                    movementPaused = true;
                    //characterMoved.myAnimator.enabled = false;
                    characterMoved.myAnimator.SetTrigger("Idle");
                    Action freeStrikeAction = AbilityLogic.Instance.PerformFreeStrike(entity, characterMoved);
                    yield return new WaitUntil(() => freeStrikeAction.ActionResolved() == true);                    
                      
                    // Resume movement
                    //characterMoved.myAnimator.enabled = true;
                    characterMoved.myAnimator.SetTrigger("Move");
                    movementPaused = false;
                }
            }
        }        

        action.actionResolved = true;
    }

    #endregion


}
