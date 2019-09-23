using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap : Singleton<WorldMap>
{
    public int playerColumn;
    public WorldEncounter playerPosition;
    public List<WorldEncounter> allWorldEncounters;
    public bool canSelectNewEncounter = false;

    public bool generateRandomWorld;

    [Header("Testing + Debugging Properties")]
    public bool OnlySpawnBasicEncounters;

    private void Start()
    {
        CreateHexagons();
        OnNewWorldMapLoaded();
        SetWorldMapReadyState();
        UIManager.Instance.OnWorldMapButtonClicked();
    }
    public void OnNewWorldMapLoaded()
    {
        SetPlayerPosition(0, 1);
    }

    public void CreateHexagons()
    {
        /*
        WorldEncounter[] encounters = FindObjectsOfType<WorldEncounter>();

        foreach(WorldEncounter encounter in encounters)
        {
            encounter.InitializeSetup();
        }
        */

        foreach(WorldEncounter encounter in allWorldEncounters)
        {
            encounter.InitializeSetup();
        }
    }

    public void SetPlayerPosition(WorldEncounter encounter)
    {
        playerPosition = encounter;
        playerColumn = encounter.column;
        encounter.encounterReached = true;
        encounter.SetGraphicMaskColour(encounter.occupiedColour);        
    }   

    public void SetPlayerPosition(int column, int position)
    {
        WorldEncounter newLocation = null;

        foreach(WorldEncounter encounter in allWorldEncounters)
        {
            if(encounter.column == column && encounter.position == position)
            {
                newLocation = encounter;
            }
        }

        if(newLocation == null)
        {
            Debug.Log("WorldMap.SetPlayerPosition() could not find a matching World Encounter location object");
            return;
        }

        playerPosition = newLocation;
        newLocation.encounterReached = true;        
        newLocation.SetGraphicMaskColour(newLocation.occupiedColour);
    }

    public void PopulateEncountersList()
    {
        WorldEncounter[] encountersFound = FindObjectsOfType<WorldEncounter>();
        allWorldEncounters.AddRange(encountersFound);
    }

    public List<WorldEncounter> GetNextViableEncounters(WorldEncounter positionFrom)
    {
        //int currentColumn = positionFrom.column;
        int currentPosition = positionFrom.position;    

        List<WorldEncounter> viableEncounters = new List<WorldEncounter>();

        foreach (WorldEncounter encounter in allWorldEncounters)
        {
            if (
                (encounter.position == currentPosition || encounter.position == currentPosition + 1) &&
                encounter.column == positionFrom.column + 1 &&
                encounter.column <= 6
                )
            {
                viableEncounters.Add(encounter);
            }

            else if (
                (encounter.position == currentPosition || encounter.position == currentPosition - 1) &&
                encounter.column == positionFrom.column + 1 &&
                encounter.column > 6
                )
            {
                viableEncounters.Add(encounter);
            }
        }

        return viableEncounters;

    }

    public void SetWorldMapReadyState()
    {
        canSelectNewEncounter = true;
        HighlightNextAvailableEncounters();
    }

    public void UnHighlightAllHexagons()
    {
        foreach(WorldEncounter encounter in allWorldEncounters)
        {
            encounter.UnHighlightHexagon();
        }
    }

    public void HighlightNextAvailableEncounters()
    {
        List<WorldEncounter> nextEncounters = GetNextViableEncounters(playerPosition);

        foreach (WorldEncounter encounter in nextEncounters)
        {
            encounter.HighlightHexagon();
        }
    }
}
