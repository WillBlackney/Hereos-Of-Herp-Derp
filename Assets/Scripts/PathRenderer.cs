using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRenderer : Singleton<PathRenderer>
{
    [Header("Component References")]
    public LineRenderer lineRenderer;
    public GameObject lineRendererParent;

    [Header("Properties")]
    public List<Tile> currentPath;
    public bool active;

    public void DrawPath()
    {
        // Clear previous path
        ClearCurrentPath();

        // Calculate a path with AStar
        Stack<Node> path = AStar.GetPath(DefenderManager.Instance.selectedDefender.tile.GridPosition, LevelManager.Instance.mousedOverTile.GridPosition);

        // Convert node stack into list of tiles
        foreach(Node node in path)
        {
            Tile tile = LevelManager.Instance.GetTileFromPointReference(node.GridPosition);
            currentPath.Add(tile);
        }

        currentPath.Insert(0, DefenderManager.Instance.selectedDefender.tile);

        // Set line renderer vertex points
        lineRenderer.positionCount = currentPath.Count;
        //lineRenderer.SetPosition(0, DefenderManager.Instance.selectedDefender.tile.WorldPosition);

        for (int index = 0; index < currentPath.Count; index++)
        {
            lineRenderer.SetPosition(index, currentPath[index].WorldPosition);
        }
        

        // Enable line renderer view
        SetLineRendererViewState(true);
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
}
