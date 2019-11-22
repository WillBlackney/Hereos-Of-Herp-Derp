using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : Singleton<WorldManager>
{
    [Header("Properties")]
    public World currentWorld;
    public int playerColumnPosition;
    public WorldEncounter playerEncounterPosition;
    public bool canSelectNewEncounter;

    [Header("Component References")]
    public List<GameObject> worldMapPrefabs;    
    public GameObject worldMapParent;

    

    // Initialization + Setup
    #region
    private void Start()
    {
        OnNewGameStarted();
    }   
    public void OnNewGameStarted()
    {
        CreateNewWorld();
        SetPlayerPosition(0);
        SetWorldMapReadyState();
        UIManager.Instance.OnWorldMapButtonClicked();
    }
    public void CreateNewWorld()
    {
        GameObject newWorld = Instantiate(GetRandomWorldPrefab(), worldMapParent.transform);
        currentWorld = newWorld.GetComponent<World>();
        currentWorld.InitializeSetup();
    }
    #endregion

    // Get Encounters + World Data
    #region
    public GameObject GetRandomWorldPrefab()
    {
        return worldMapPrefabs[Random.Range(0, worldMapPrefabs.Count)];
    }
    public List<WorldEncounter> GetNextViableEncounters(WorldEncounter playerPosition = null)
    {
        List<WorldEncounter> encountersReturned = new List<WorldEncounter>();

        // if we are at the very start of the map
        if (playerPosition == null)
        {
            foreach (WorldEncounter encounter in currentWorld.allEncounters)
            {
                if (encounter.column == 1)
                {
                    encountersReturned.Add(encounter);
                }
            }
        }
        else if (playerPosition != null)
        {
            foreach (WorldEncounter encounter in playerPosition.connectingEncounters)
            {
                encountersReturned.Add(encounter);
            }
        }

        return encountersReturned;
    }
    #endregion

    // Travelling + World Navigation Logic
    #region
    public void SetPlayerPosition(int newColumnPosition)
    {
        playerColumnPosition = newColumnPosition;
    }
    public void SetPlayerPosition(WorldEncounter encounter)
    {
        playerEncounterPosition = encounter;
        playerColumnPosition = encounter.column;
        encounter.encounterReached = true;
        encounter.SetCircleBackgroundViewState(true);
    }
    public void SetWorldMapReadyState()
    {
        canSelectNewEncounter = true;
        HighlightNextAvailableEncounters();
    }
    #endregion

    // Visibility + View Logic
    #region
    public void HighlightNextAvailableEncounters()
    {
        List<WorldEncounter> nextEncounters = GetNextViableEncounters(playerEncounterPosition);

        foreach (WorldEncounter encounter in nextEncounters)
        {
            encounter.PlayBreatheAnimation();
        }
    }
    public void UnhighlightAllHexagons()
    {
        foreach (WorldEncounter encounter in currentWorld.allEncounters)
        {
            encounter.PlayIdleAnimation();
        }
    }
    #endregion

    // Misc Logic
    #region
    public void DestroyCurrentWorld()
    {
        if (currentWorld != null)
        {
            Destroy(currentWorld.gameObject);
        }
    }
    #endregion
}
