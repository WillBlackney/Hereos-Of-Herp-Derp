using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
    [Header("Component References")]
    public LineRenderer lineRenderer;
    public GameObject lineRendererParent;

    [Header("Properties")]
    public List<Tile> currentPath;
    public bool active;

    public static PathRenderer Instance;
    private void Awake()
    {
        Instance = this;
    }   

    public void DrawPath()
    {
        Tile endPoint = LevelManager.Instance.mousedOverTile;
        bool finishedDrawing = false;

        while (endPoint == LevelManager.Instance.mousedOverTile &&
                finishedDrawing == false)
        {
            // Clear previous path
            ClearCurrentPath();

            // Disable free strike indicators
            DisableAllFreeStrikeIndicators();

            // Calculate a path with AStar
            Stack<Node> path = AStar.GetPath(DefenderManager.Instance.selectedDefender.tile.GridPosition, LevelManager.Instance.mousedOverTile.GridPosition);

            // Convert node stack into list of tiles
            foreach (Node node in path)
            {
                Tile tile = LevelManager.Instance.GetTileFromPointReference(node.GridPosition);
                currentPath.Add(tile);
            }

            // Insert defenders current tile as index 0
            currentPath.Insert(0, DefenderManager.Instance.selectedDefender.tile);

            // Set line renderer vertex points
            lineRenderer.positionCount = currentPath.Count;
            for (int index = 0; index < currentPath.Count; index++)
            {
                lineRenderer.SetPosition(index, currentPath[index].WorldPosition);
            }

            // Start checking for free strikes
            for (int index = 0; index < currentPath.Count; index++)
            {
                // check to make sure the tile being examined is not the last index in the list
                if (currentPath.Count > 0 && index != currentPath.Count - 1)
                {
                    Tile currentTile = currentPath[index];
                    foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                    {
                        // only check enemies that actually have a melee weapon
                        if(enemy.myMainHandWeapon.itemType == ItemDataSO.ItemType.MeleeOneHand || enemy.myMainHandWeapon.itemType == ItemDataSO.ItemType.MeleeTwoHand)
                        {
                            // Dont both checking free strikes if selected defender is immune to free strikes, or enemy doesnt have a melee weapon
                            if (DefenderManager.Instance.selectedDefender == null || DefenderManager.Instance.selectedDefender.myPassiveManager.slippery == false)
                            {
                                List<Tile> enemyMeleeRange = LevelManager.Instance.GetTilesWithinRange(enemy.currentMeleeRange, enemy.tile, true, false);
                                // is current tile index in melee range of the enemy?
                                if (enemyMeleeRange.Contains(currentTile))
                                {
                                    // it is. is the next position on the path not within the enemies melee range?
                                    if (enemyMeleeRange.Contains(currentPath[index + 1]) == false)
                                    {
                                        // moving from the current index will trigger a free strike
                                        Debug.Log("PathRenderer.DrawPath() FREE STRIKE DETECTED");
                                        enemy.SetFreeStrikeIndicatorViewState(true);
                                    }
                                }
                            }
                        }     

                    }
                }

            }

            finishedDrawing = true;
        }
        
    }

    public void ActivatePathRenderer()
    {
        SetLineRendererViewState(true);
        SetReadyState(true);
    }
    public void DeactivatePathRenderer()
    {
        SetLineRendererViewState(false);
        SetReadyState(false);
        DisableAllFreeStrikeIndicators();
        ClearCurrentPath();
    }

    public void ClearCurrentPath()
    {
        currentPath.Clear();
        lineRenderer.positionCount = 0;
    }

    public void SetLineRendererViewState(bool onOrOff)
    {
        lineRendererParent.SetActive(onOrOff);
    }

    public void SetReadyState(bool onOrOff)
    {
        active = onOrOff;
    }

    public void DisableAllFreeStrikeIndicators()
    {
        foreach(Enemy enemy in EnemyManager.Instance.allEnemies)
        {
            enemy.SetFreeStrikeIndicatorViewState(false);
        }
    }
}
