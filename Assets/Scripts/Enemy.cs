using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class Enemy : LivingEntity
{
    [Header("Enemy Unique Components + Properties")]
    public CharacterInfoPanel myInfoPanel;
    public string myName;
    
    // All Methods and Logic

    // Initialization

    public override void InitializeSetup(Point startingGridPosition, TileScript startingTile)
    {              
        EnemyManager.Instance.allEnemies.Add(this);        
        base.InitializeSetup(startingGridPosition, startingTile);
        myInfoPanel.InitializeSetup(this);
        
    }

    public override void SetBaseProperties()
    {
        base.SetBaseProperties();            
    }

    // Enumerators and Coroutines
    public virtual void StartMyActivation()
    {        
        StartCoroutine(OnTurnStart());        
        StartCoroutine(StartMyActivationCoroutine());
    }

    public virtual IEnumerator StartMyActivationCoroutine()
    {
        //currentlyActivated = false;
        yield return null;
    }

    public virtual IEnumerator Move(int movePoints = 1, float movSpeed = 3)
    {
        myAnimator.SetTrigger("Move");
        float originalSpeed = speed;
        float speedOfThisMovement = movSpeed;
        int movePointsLeftOnThisMoveAction = movePoints + 1;
        bool hasCompletedMovement = false;

        speed = speedOfThisMovement;

        // flip the sprite's x axis depending on the direction of movement
        //CalculateWhichDirectionToFace(TileCurrentlyOn, LevelManager.Instance.GetTileFromPointReference(path.Peek().GridPosition));
        Debug.Log("Tiles on this movement path: " + path.Count);
        while (hasCompletedMovement == false)
        {          
           
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (transform.position == destination && movePointsLeftOnThisMoveAction > 0)
            {
                Debug.Log("Next tile on path reached...");
                
                movePointsLeftOnThisMoveAction--;
                

                if (path != null && path.Count > 0 && movePointsLeftOnThisMoveAction > 0)
                {
                    GridPosition = path.Peek().GridPosition;
                    // Free up the tile we were standing on before we moved
                    LevelManager.Instance.SetTileAsUnoccupied(TileCurrentlyOn);
                    // Set our current tile to the tile we ended up at the end of the move
                    TileCurrentlyOn = LevelManager.Instance.GetTileFromPointReference(GridPosition);
                    // Set our current tile to be occupied, so other characters cant stack ontop of it.
                    LevelManager.Instance.SetTileAsOccupied(TileCurrentlyOn);
                    destination = path.Pop().WorldPosition;
                    Debug.Log("Remaining tiles on this movement path: " + path.Count);
                }

                // if we have reached the final destination
                else if ((path != null && path.Count == 0) ||
                    (path != null && movePointsLeftOnThisMoveAction == 0)
                    )
                {
                    // Free up the tile we were standing on before we moved
                    LevelManager.Instance.SetTileAsUnoccupied(TileCurrentlyOn);
                    // Set our current tile to the tile we ended up at the end of the move
                    TileCurrentlyOn = LevelManager.Instance.GetTileFromPointReference(GridPosition);
                    // Set our current tile to be occupied, so other characters cant stack ontop of it.
                    LevelManager.Instance.SetTileAsOccupied(TileCurrentlyOn);
                    
                    Debug.Log("Final point reached, movement finished");
                    hasCompletedMovement = true;
                    movementFinished = true;
                    myAnimator.SetTrigger("Idle");
                    speed = originalSpeed;
                }
            }

            yield return null;
        }
    }

    public virtual IEnumerator AttackTarget(LivingEntity target, int damageAmount, bool playAnimation = true)
    {
        // start attack animation code here
        //CalculateWhichDirectionToFace(TileCurrentlyOn, target.TileCurrentlyOn);
        if(playAnimation == true)
        {
            StartAttackAnimation();
            yield return new WaitUntil(() => AttackAnimationMomentOfAttackReached() == true);
            //CombatLogic.Instance.HandleDamage(CombatLogic.Instance.CalculateDamage(damageAmount, target, this), this);
            yield return new WaitUntil(() => AttackAnimationFinished() == true);
        }
        else
        {
           // target.HandleDamage(CalculateDamage(damageAmount, target, this), this);
        }

        attackFinished = true;
    }    

    // Defenders and Targeting

    public void SetTargetDefender(Defender target)
    {
        myCurrentTarget = target;
    }

    public Defender GetClosestDefender()
    {
        // TO DO: this method determines which defender is closest by drawing a straight line. It should instead calculate the closest by drawing a path to each with Astar

        Defender closestTarget = null;
        float minimumDistance = Mathf.Infinity;

        // Declare new temp list for storing defender 
        List<Defender> defenders = new List<Defender>();

        // Add all active defenders to the temp list
        foreach (Defender defender in DefenderManager.Instance.allDefenders)
        {
            defenders.Add(defender);
        }
               
        // Iterate throught the temp list to find the closest defender to this enemy
        foreach (Defender defender in defenders)
        {
            float distancefromDefender = Vector2.Distance(defender.gameObject.transform.position, transform.position);
            if (distancefromDefender < minimumDistance)
            {
                closestTarget = defender;
                minimumDistance = distancefromDefender;
            }
        }
        return closestTarget;
    }       

    public Defender GetMostVulnerableDefender()
    {
        Defender bestTarget = null;
        int pointScore = 0;

        foreach(Defender defender in DefenderManager.Instance.allDefenders)
        {
            int myPointScore = 1;

            if (defender.isKnockedDown)
            {
                myPointScore++;
            }

            if(myPointScore > pointScore)
            {
                pointScore = myPointScore;
                bestTarget = defender;
            }
        }

        return bestTarget;
    }

    public Defender GetDefenderWithLowestCurrentHP()
    {
        Defender bestTarget = null;
        int lowestHP = 1000;

        // Declare new temp list for storing defender 
        List<Defender> defenders = new List<Defender>();

        // Add all active defenders to the temp list
        foreach (Defender defender in DefenderManager.Instance.allDefenders)
        {
            defenders.Add(defender);
        }

        foreach (Defender defender in defenders)
        {            
            if (defender.currentHealth < lowestHP)
            {
                bestTarget = defender;
                lowestHP = defender.currentHealth;                
            }            
        }

        if(bestTarget == null)
        {
            Debug.Log("GetDefenderWithLowestCurrentHP() returning null !!...");
        }

        return bestTarget;
    }

    // State + Animation related booleans

    public bool currentlyActivated = false;
    public bool ActivationFinished()
    {
        if (currentlyActivated == false)
        {
            return true;
        }

        else
        {
            return false;
        }
    }    

    public bool attackFinished = false;
    public bool AttackFinished()
    {
        if (attackFinished == true)
        {
            attackFinished = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool attackAnimationFinished = false;
    public bool AttackAnimationFinished()
    {
        if (attackAnimationFinished == true)
        {
            attackAnimationFinished = false;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SetAttackAnimationFinished()
    {
        attackAnimationFinished = true;
    }

    public bool attackAnimationMomentOfAttackReached = false;
    public bool AttackAnimationMomentOfAttackReached()
    {
        if(attackAnimationMomentOfAttackReached == true)
        {
            attackAnimationMomentOfAttackReached = false;
            return true;
        }
        else
        {
            return false;
        }
    }
    // this method should only ever be called by the animator .
    public void SetAttackAnimationMomentOfAttackReached()
    {
        attackAnimationMomentOfAttackReached = true;
    }

    public bool movementFinished = false;
    public bool MovementFinished()
    {
        if (movementFinished == true)
        {
            Debug.Log("MovementFinished() returning true...");
            movementFinished = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Pathfinding and Tiles

    public void GeneratePathToMeleeRangeOfTarget(Defender target)
    {
        TileScript tile = GetClosestValidTile(GetAdjacentTilesOfTarget(target));
        path = AStar.GetPath(GridPosition, tile.GridPosition);
    }

    public void GeneratePathToClosestTileWithinRangeOfTarget(LivingEntity target, int rangeFromTarget)
    {
        TileScript tile = GetClosestValidTile(LevelManager.Instance.GetTilesWithinRange(rangeFromTarget,target.TileCurrentlyOn));
        path = AStar.GetPath(GridPosition, tile.GridPosition);
        SetPath(path);
        //

    }

    public TileScript GetBestValidMoveLocationBetweenMeAndTarget(LivingEntity target, int rangeFromTarget, int movePoints)
    {
        TileScript tile = GetClosestValidTile(LevelManager.Instance.GetTilesWithinRange(rangeFromTarget, target.TileCurrentlyOn));
        Stack<Node> pathFromMeToIdealTile = AStar.GetPath(TileCurrentlyOn.GridPosition, tile.GridPosition);

        Debug.Log("GetBestValidMoveLocationBetweenMeAndTarget() generated a path with " +
            pathFromMeToIdealTile.Count.ToString() + " tiles on it"
            );

        if(pathFromMeToIdealTile.Count > 2)
        {
            return pathFromMeToIdealTile.ElementAt(movePoints - 1).TileRef;
        }
        else if (pathFromMeToIdealTile.Count == 2)
        {
            return pathFromMeToIdealTile.ElementAt(0).TileRef;
        }
        else
        {
            return pathFromMeToIdealTile.ElementAt(0).TileRef;
        }        
        
    }

    public List<TileScript> GetAdjacentTilesOfTarget(Defender target)
    {      
        TileScript[] allTiles = FindObjectsOfType<TileScript>();
        List<TileScript> adjacentTiles = new List<TileScript>();

        // iterate through all tiles in the world and find the tiles adjacent to the target
        foreach(TileScript tile in allTiles)
        {
            if(                 
                // east tile
                (tile.GridPosition.X == target.GridPosition.X + 1 && tile.GridPosition.Y == target.GridPosition.Y) ||
                // west tile
                (tile.GridPosition.X == target.GridPosition.X - 1 && tile.GridPosition.Y == target.GridPosition.Y) ||
                // south tile
                (tile.GridPosition.X == target.GridPosition.X && tile.GridPosition.Y == target.GridPosition.Y - 1) ||
                // north tile
                (tile.GridPosition.X == target.GridPosition.X && tile.GridPosition.Y == target.GridPosition.Y + 1) ||
                // north east tile
                (tile.GridPosition.X == target.GridPosition.X + 1 && tile.GridPosition.Y == target.GridPosition.Y + 1) ||
                // south east tile
                (tile.GridPosition.X == target.GridPosition.X + 1 && tile.GridPosition.Y == target.GridPosition.Y - 1) ||
                // south west tile
                (tile.GridPosition.X == target.GridPosition.X - 1 && tile.GridPosition.Y == target.GridPosition.Y - 1) ||
                // north west tile
                (tile.GridPosition.X == target.GridPosition.X - 1 && tile.GridPosition.Y == target.GridPosition.Y + 1)

                )
            {
                adjacentTiles.Add(tile);                
            }
        }
        
        return adjacentTiles;        
    }

    public TileScript GetClosestValidTile(List<TileScript> tiles)
    {
        TileScript closestTile = null;
        float minimumDistance = Mathf.Infinity;  
        
        foreach (TileScript tile in tiles)
        {
            if(tile.IsEmpty && tile.IsWalkable)
            {
                float distanceFromEnemy = Vector2.Distance(tile.gameObject.transform.position, transform.position);
                if (distanceFromEnemy < minimumDistance)
                {
                    closestTile = tile;
                    minimumDistance = distanceFromEnemy;
                }
            }
            
        }
        
        return closestTile;
    }

    public virtual void SetPath(Stack<Node> newPath)
    {
        if(newPath != null)
        {
            this.path = newPath;
            GridPosition = path.Peek().GridPosition;
            destination = path.Peek().WorldPosition;
        }
    }
    
    // Events and activation

    public override IEnumerator OnTurnStart()
    {
        StartCoroutine(base.OnTurnStart());
        currentlyActivated = true;
        yield return null;
    }

    public void EndMyActivation()
    {
        currentlyActivated = false;
    }

    // Input and Player interaction

    public void OnMouseDown()
    {
        Debug.Log("Enemy click detected");
        EnemyManager.Instance.SelectEnemy(this);
    }

    public void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1))
        {
            myInfoPanel.EnablePanelView();
        }
    }

    // Visual + UI 
    public void StartAttackAnimation()
    {
        myAnimator.SetTrigger("Attack");

    }
   
}
