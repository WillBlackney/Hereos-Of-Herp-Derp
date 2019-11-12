using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class Enemy : LivingEntity
{
    [Header("Enemy Components")]
    public CharacterInfoPanel myInfoPanel;
    [Header("Enemy Properties")]
    public string myName;

    // Initialization + Setup
    #region
    public override void InitializeSetup(Point startingGridPosition, Tile startingTile)
    {              
        EnemyManager.Instance.allEnemies.Add(this);        
        base.InitializeSetup(startingGridPosition, startingTile);
        myInfoPanel.InitializeSetup(this);
        
    }
    #endregion

    // Activation + Related
    #region
    public virtual void StartMyActivation()
    {       
        StartCoroutine(StartMyActivationCoroutine());
    }
    public virtual IEnumerator StartMyActivationCoroutine()
    {
        yield return null;
    }
    public Action EndMyActivation()
    {
        Action action = new Action();
        StartCoroutine(EndMyActivationCoroutine(action));
        return action;

    }
    public IEnumerator EndMyActivationCoroutine(Action action)
    {
        Action endActivation = ActivationManager.Instance.EndEntityActivation(this);
        yield return new WaitUntil(() => endActivation.ActionResolved() == true);
        action.actionResolved = true;
        ActivationManager.Instance.ActivateNextEntity();
    }

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
    #endregion

    // AI Targeting Logic
    #region
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
    #endregion

    // AI Pathfinding Logic    
    #region
    public void GeneratePathToClosestTileWithinRangeOfTarget(LivingEntity target, int rangeFromTarget)
    {
        Tile tile = GetClosestValidTile(LevelManager.Instance.GetTilesWithinRange(rangeFromTarget,target.tile));
        path = AStar.GetPath(gridPosition, tile.GridPosition);
        SetPath(path);
        //

    }       
    public Tile GetClosestValidTile(List<Tile> tiles)
    {
        Tile closestTile = null;
        float minimumDistance = Mathf.Infinity;  
        
        foreach (Tile tile in tiles)
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
            gridPosition = path.Peek().GridPosition;
            destination = path.Peek().WorldPosition;
        }
    }
    #endregion

    // Mouse + Click Events
    #region
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
    #endregion

}
