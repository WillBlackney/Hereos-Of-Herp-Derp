using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingPathRenderer : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Component References")]
    public LineRenderer lineRenderer;
    public GameObject lineRendererParent;

    [Header("Properties")]
    public bool active;
    #endregion

    // Singleton Pattern
    #region
    public static TargetingPathRenderer Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            lineRenderer.positionCount = 2;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    #endregion

    // Pathing Logic
    #region
    public void DrawPath()
    {
        lineRenderer.positionCount = 2;
        Tile startPoint = DefenderManager.Instance.selectedDefender.tile;
        Tile endPoint = LevelManager.Instance.mousedOverTile;

        if(startPoint && endPoint)
        {            
            lineRenderer.SetPosition(0, startPoint.WorldPosition);
            lineRenderer.SetPosition(1, endPoint.WorldPosition);
        }
       
    }
    #endregion

    // View Logic
    #region
    public void ActivatePathRenderer()
    {
        lineRenderer.positionCount = 0;
        SetLineRendererViewState(true);
        SetReadyState(true);
    }
    public void DeactivatePathRenderer()
    {
        SetLineRendererViewState(false);
        SetReadyState(false);
    }
    public void SetLineRendererViewState(bool onOrOff)
    {
        lineRendererParent.SetActive(onOrOff);
    }
    public void SetReadyState(bool onOrOff)
    {
        active = onOrOff;
    }
    #endregion
}
