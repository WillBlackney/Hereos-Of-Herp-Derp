using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [Header("Properties")]
    public World currentWorld;
    public int currentAct;
    public int playerColumnPosition;
    public WorldEncounter playerEncounterPosition;
    public bool canSelectNewEncounter;

    [Header("Component References")]
    public List<GameObject> worldMapPrefabs;    
    public GameObject visualParent;
    public GameObject canvasParent;
    public RectTransform scrollViewParent;

    [Header("Testing Properties")]
    public bool onlySpawnBasics;
    public bool onlySpawnElites;
    public bool onlySpawnMysterys;
    public bool onlySpawnShops;
    public bool onlySpawnTreasures;
    public bool onlySpawnCampSites;

    [Header("Encounter Type Images")]
    public Sprite basicEncounterImage;
    public Sprite eliteEncounterImage;
    public Sprite campSiteEncounterImage;
    public Sprite shopEncounterImage;
    public Sprite treasureEncounterImage;
    public Sprite mysteryEncounterImage;
    public Sprite bossEncounterImage;


    // Initialization + Setup
    #region
    public static WorldManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        OnNewGameStarted();
    }   
    public void OnNewGameStarted()
    {
        OnActOneStarted();
    }
    public void OnActOneStarted()
    {
        currentAct = 1;
        CreateNewWorld();
        SetPlayerAtHomePosition();
        SetWorldMapReadyState();
    }
    public void OnActTwoStarted()
    {
        currentAct = 2;
        DestroyCurrentWorld();
        CreateNewWorld();
        SetPlayerAtHomePosition();
        SetWorldMapReadyState();
    }
    public void CreateNewWorld()
    {
        GameObject newWorld = Instantiate(GetRandomWorldPrefab(), scrollViewParent);
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
        
        else if(playerPosition != null)
        {
            foreach (WorldEncounter encounter in currentWorld.allEncounters)
            {
                // check for viable column
                if (encounter.column == playerEncounterPosition.column + 1)
                {
                    // check for viable position
                    if(encounter.position == playerEncounterPosition.position ||
                        encounter.position == playerEncounterPosition.position + 1 ||
                        encounter.position == playerEncounterPosition.position - 1)
                    {
                        encountersReturned.Add(encounter);
                    }
                    
                }
            }
        }

        return encountersReturned;
    }
    #endregion

    // Travelling + World Navigation Logic
    #region
    public void SetPlayerAtHomePosition()
    {
        playerColumnPosition = 0;
        playerEncounterPosition = null;
    }
    public void SetPlayerPosition(int newColumnPosition)
    {
        playerColumnPosition = newColumnPosition;
    }
    public void SetPlayerPosition(WorldEncounter encounter)
    {
        playerEncounterPosition = encounter;
        playerColumnPosition = encounter.column;
        encounter.encounterReached = true;
        encounter.SetRedXOverlayViewState(true);
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
    public void IdleAllEncounters()
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
            currentWorld = null;
        }
    }
    #endregion
}
