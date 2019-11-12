using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderManager : Singleton<DefenderManager>
{
    [Header("Component References")]
    public GameObject defendersParent;

    [Header("Properties")]
    public Defender selectedDefender;
    public List<Defender> allDefenders = new List<Defender>();
    public List<Tile> spawnLocations = new List<Tile>();

    // Defender Selection
    #region
    public void SetSelectedDefender(Defender defender)
    {
        // if we have already have a defender selected when we click on another defender, unselect that defender, then select the new defender
        if(selectedDefender != defender && selectedDefender != null)
        {
            Debug.Log("Clearing selected defender");
            selectedDefender.UnselectDefender();
            LevelManager.Instance.UnhighlightAllTiles();
        }

        selectedDefender = defender;
        CameraManager.Instance.SetCameraLookAtTarget(selectedDefender.gameObject);
        Debug.Log("Selected defender: " + selectedDefender.gameObject.name);
    }
    public void ClearSelectedDefender()
    {
        if(selectedDefender != null)
        {
            selectedDefender.UnselectDefender();
            selectedDefender = null;
        }
        CameraManager.Instance.ClearCameraLookAtTarget();
        LevelManager.Instance.UnhighlightAllTiles();
    }
    #endregion

    // Misc Logic
    #region
    public void DestroyAllDefenders()
    {
        List<Defender> allDefs = new List<Defender>();
        allDefs.AddRange(allDefenders);

        foreach(Defender defender in allDefenders)
        {
            LivingEntityManager.Instance.allLivingEntities.Remove(defender);
        }       

        foreach(Defender defender in allDefs)
        {
            if (allDefenders.Contains(defender))
            {
                allDefenders.Remove(defender);
                Destroy(defender.gameObject);
            }
        }

        allDefenders.Clear();
    }
    #endregion





}
