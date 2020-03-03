using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Properties + Components
    #region
    [Header("Prefab References")]
    [SerializeField] private GameObject[] tilePrefabs;    
    [SerializeField] private List <TextAsset> mapTextFiles;
    public GameObject combatBGPrefab;

    [Header("Component References")]
    [SerializeField] private Transform tileParent;

    [Header("Properties")]
    public Tile selectedTile;    
    private Point mapSize;
    public GameObject currentLevelBG;
    public Tile mousedOverTile;
    public Dictionary<Point, Tile> Tiles { get; set; }
    public List<Tile> HighlightedTiles = new List<Tile>();
    #endregion

    // Singleton Setup
    #region
    public static LevelManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Initialzation + Level Creation
    #region
    public void CreateLevel()
    {
        // Clear previous level tiles and declare new list
        Tiles = new Dictionary<Point, Tile>();

        // Read level data from text file
        string[] mapData = ReadMapTextAssetData();

        // Set properties for level construction
        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);
        int mapX = mapData[0].ToCharArray().Length;
        int mapY = mapData.Length;
        Vector3 worldStart = new Vector3(0, 0, 0);

        // Create all tiles
        for (int y = 0; y < mapY; y++) // the y positions
        {
            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapX; x++) // the x positions
            {
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }

        // Turn on level background
        ToggleLevelBackgroundView(true);

        // Move camera to focus on the centre tile
        FindObjectOfType<CameraMovement>().cinemachineCamera.transform.position = new Vector3(GetWorldCentreTile().WorldPosition.x, GetWorldCentreTile().WorldPosition.y + 0.5f, -10);

    }
    public void CreateLevelBackground()
    {
        GameObject newLevelBG = Instantiate(PrefabHolder.Instance.LevelBG);
        currentLevelBG = newLevelBG;        
        newLevelBG.transform.position = new Vector3(0, -4, 0.001F);
    }
    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);

        Tile newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<Tile>();

        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), tileParent);

    }
    private string[] ReadMapTextAssetData()
    {
        TextAsset bindData = mapTextFiles[UnityEngine.Random.Range(0, mapTextFiles.Count)];
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);
        return data.Split('-');
    }
    #endregion

    // Visibility + View Logic
    #region
    public void ToggleLevelBackgroundView(bool onOrOff)
    {
        if (onOrOff == true)
        {
            currentLevelBG.SetActive(true);
        }
        else
        {
            currentLevelBG.SetActive(false);
        }
    }
    public void HighlightTiles(List<Tile> tilesToHighlight)
    {
        Debug.Log("Highlighting tiles");
        foreach (Tile tile in tilesToHighlight)
        {
            HighlightTile(tile);
        }
    }
    public void HighlightTile(Tile tile)
    {
        HighlightedTiles.Add(tile);
        tile.myAnimator.SetBool("Highlight", true);
    }        
    public void UnhighlightAllTiles()
    {
        foreach (Tile tile in HighlightedTiles)
        {
            tile.myAnimator.SetBool("Highlight", false);
        }

        HighlightedTiles.Clear();
    }
    #endregion

    // Conditional Logic + Booleans
    #region
    public bool InBounds(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X < mapSize.X && position.Y < mapSize.Y;
    }
    public bool IsTileWithinMovementRange(Tile targetTile, Tile tileFrom, int mobility)
    {
        Stack<Node> path = AStar.GetPath(tileFrom.GridPosition, targetTile.GridPosition);
        if (path.Count <= mobility)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsTileYWithinRangeOfTileX(Tile tileX, Tile tileY, int range)
    {
        bool boolReturned = false;
        List<Tile> tilesWithinRange = GetTilesWithinRange(range, tileX);

        if (tilesWithinRange.Contains(tileY))
        {
            boolReturned = true;
        }
        return boolReturned;
    }
    public bool IsDestinationTileToTheRight(Tile tileFrom, Tile destination)
    {
        if (tileFrom.GridPosition.X < destination.GridPosition.X)
        {
            return true;
        }
        else if (tileFrom.GridPosition.X > destination.GridPosition.X)
        {
            return false;
        }

        else
        {
            return false;
        }
    }
    public bool IsDestinationTileToTheLeft(Tile tileFrom, Tile destination)
    {
        if (tileFrom.GridPosition.X > destination.GridPosition.X)
        {
            return true;
        }
        else if (tileFrom.GridPosition.X < destination.GridPosition.X)
        {
            return false;
        }

        else
        {
            return false;
        }
    }
    #endregion

    // Get Tile + Level Data
    #region
    public List<Tile> GetAllTilesFromCurrentLevelDictionary()
    {
        List<Tile> listReturned = new List<Tile>();

        foreach (KeyValuePair<Point, Tile> kvp in Tiles)
        {
            listReturned.Add(kvp.Value);
        }

        Debug.Log("GetAllTilesFromCurrentLevelDictionary() found " + listReturned.Count.ToString() +
            " tiles");

        return listReturned;
    }
    public List<Tile> GetTilesWithinRange(int range, Tile tileFrom, bool removeTileFrom = true, bool ignoreLos = true)
    {
        // iterate through every tile, and add those within range to the temp list        
        List<Tile> allTiles = GetAllTilesFromCurrentLevelDictionary();
        List<Tile> allTilesWithinXPosRange = new List<Tile>();
        List<Tile> allTilesWithinRange = new List<Tile>();
        List<Tile> allTilesWithinRangeAndLos = new List<Tile>();
        List<Tile> finalList = new List<Tile>();

        // first, filter in all tiles with an X grid position within movement range
        foreach (Tile tile in allTiles)
        {
            int myXPos = tile.GridPosition.X;

            if (
                (myXPos >= tileFrom.GridPosition.X && (myXPos <= tileFrom.GridPosition.X + range)) ||
                (myXPos <= tileFrom.GridPosition.X && (myXPos >= tileFrom.GridPosition.X - range))
                )
            {
                allTilesWithinXPosRange.Add(tile);
            }

        }

        // second, filter out all tiles outside of Y range, then add the remainding tiles to the final list.
        foreach (Tile Xtile in allTilesWithinXPosRange)
        {
            int myYPos = Xtile.GridPosition.Y;

            if (
                (myYPos >= tileFrom.GridPosition.Y && myYPos <= tileFrom.GridPosition.Y + range) ||
                (myYPos <= tileFrom.GridPosition.Y && (myYPos >= tileFrom.GridPosition.Y - range))
                )
            {
                allTilesWithinRange.Add(Xtile);
            }
        }

        // third, remove the 'fromTile' from the list
        if(removeTileFrom == true)
        {
            allTilesWithinRange.Remove(tileFrom);
        }

        // check for LoS, if required
        if(ignoreLos == true)
        {
            finalList.AddRange(allTilesWithinRange);
        }

        else
        {
            foreach(Tile tile in allTilesWithinRange)
            {
                if(PositionLogic.Instance.IsThereLosFromAtoB(tileFrom, tile))
                {
                    allTilesWithinRangeAndLos.Add(tile);
                }
            }

            finalList.AddRange(allTilesWithinRangeAndLos);
        }

        return finalList;
        
    }
    public List<Tile> GetValidMoveableTilesWithinRange(int range, Tile tileFrom, bool ignorePathing = false)
    {
        // iterate through every tile, and add those within range to the temp list
        List<Tile> allTiles = GetAllTilesFromCurrentLevelDictionary();
        List<Tile> allTilesWithinXPosRange = new List<Tile>();
        List<Tile> allTilesWithinRange = new List<Tile>();
        List<Tile> allTilesWithinMobilityRange = new List<Tile>();

        // first, filter in all tiles with an X grid position within movement range
        foreach (Tile tile in allTiles)
        {
            int myXPos = tile.GridPosition.X;

            if (
                (myXPos >= tileFrom.GridPosition.X && (myXPos <= tileFrom.GridPosition.X + range)) ||
                (myXPos <= tileFrom.GridPosition.X && (myXPos >= tileFrom.GridPosition.X - range))
                )
            {
                //only add tiles to the list if they are walkable and unoccupied
                if(tile.IsEmpty == true && tile.IsWalkable == true)
                {
                    allTilesWithinXPosRange.Add(tile);
                }                
            }
        }

        // second, filter out all tiles outside of Y range, then add the remainding tiles to the final list.
        foreach (Tile Xtile in allTilesWithinXPosRange)
        {
            int myYPos = Xtile.GridPosition.Y;

            if (
                (myYPos >= tileFrom.GridPosition.Y && myYPos <= tileFrom.GridPosition.Y + range) ||
                (myYPos <= tileFrom.GridPosition.Y && (myYPos >= tileFrom.GridPosition.Y - range))
                )
            {
                allTilesWithinRange.Add(Xtile);
            }
        }

        // third, remove the 'fromTile' from the list
        allTilesWithinRange.Remove(tileFrom);

        // fourth, draw a path to each tile in the list, filtering the ones within mobility range
        if(ignorePathing == false)
        {
            foreach (Tile tile in allTilesWithinRange)
            {
                Stack<Node> path = AStar.GetPath(tileFrom.GridPosition, tile.GridPosition);
                if (path.Count <= range)
                {
                    allTilesWithinMobilityRange.Add(tile);
                }
            }

            return allTilesWithinMobilityRange;
        }

        else
        {
            return allTilesWithinRange;
        }
    }   
    public List<Tile> GetEnemySpawnTiles()
    {
        List<Tile> allTiles = GetAllTilesFromCurrentLevelDictionary();
        List<Tile> enemySpawnTiles = new List<Tile>();

        foreach (Tile tile in allTiles)
        {
            if (
                (tile.GridPosition.X == 12) 
                && (tile.GridPosition.Y == 0 || tile.GridPosition.Y == 1 || tile.GridPosition.Y == 2 || tile.GridPosition.Y == 3 || tile.GridPosition.Y == 4 || tile.GridPosition.Y == 5 || tile.GridPosition.Y == 6)
                )
            {
                enemySpawnTiles.Add(tile);
            }
        }

        return enemySpawnTiles;
    }
    public List<Tile> GetDefenderSpawnTiles()
    {
        List<Tile> allTiles = GetAllTilesFromCurrentLevelDictionary();
        List<Tile> defenderSpawnTiles = new List<Tile>();

        foreach (Tile tile in allTiles)
        {
            if (
                (tile.GridPosition.X == 0) &&
                (tile.GridPosition.Y == 1 || tile.GridPosition.Y == 2 || tile.GridPosition.Y == 3 || tile.GridPosition.Y == 0)
                )
            {
                defenderSpawnTiles.Add(tile);
            }
        }

        return defenderSpawnTiles;

    }   
    public List<Tile> GetAllCurrentDefenderLocations()
    {
        List<Tile> defenderLocations = new List<Tile>();

        foreach(Defender defender in DefenderManager.Instance.allDefenders)
        {
            defenderLocations.Add(defender.tile);
        }

        return defenderLocations;
    }
    public List<Tile> GetAllCurrentEnemyLocations()
    {
        List<Tile> enemyLocations = new List<Tile>();

        foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
        {
            enemyLocations.Add(enemy.tile);
        }

        return enemyLocations;
    }
    public List<Tile> GetAllCurrentLivingEntitiesLocations()
    {
        List<Tile> entityLocations = new List<Tile>();

        entityLocations.AddRange(GetAllCurrentDefenderLocations());
        entityLocations.AddRange(GetAllCurrentEnemyLocations());

        return entityLocations;
    }
    public Tile GetClosestValidTile(List <Tile> tiles, Tile tileFrom)
    {
        Tile closestTile = null;
        float minimumDistance = Mathf.Infinity;

        // Iterate throught the temp list to find the closest defender to this enemy
        foreach (Tile tile in tiles)
        {
            float distancefromTileFrom = Vector2.Distance(tile.gameObject.transform.position, tileFrom.WorldPosition);
            if (distancefromTileFrom < minimumDistance && tile.IsEmpty && tile.IsWalkable)
            {
                closestTile = tile;
                minimumDistance = distancefromTileFrom;
            }
        }

        if(closestTile == null)
        {
            Debug.Log("GetClosestValidTile() could not find any valid tiles from the supplied list, return NULL...");
        }
        return closestTile;
    }
    public Tile GetFurthestTileFromTargetFromList(List <Tile> tiles, Tile tileFrom)
    {
        Tile furthestTile = null;
        float minimumDistance = 0;

        // Iterate throught the temp list to find the closest defender to this enemy
        foreach (Tile tile in tiles)
        {
            float distancefromTileFrom = Vector2.Distance(tile.WorldPosition, tileFrom.WorldPosition);
            if (distancefromTileFrom > minimumDistance && tile.IsEmpty && tile.IsWalkable)
            {
                furthestTile = tile;
                minimumDistance = distancefromTileFrom;
            }
        }

        if (furthestTile == null)
        {
            Debug.Log("GetClosestValidTile() could not find any valid tiles from the supplied list, return NULL...");
        }
        return furthestTile;
    }
    public Tile GetWorldCentreTile()
    {
        List<Tile> allTiles = GetAllTilesFromCurrentLevelDictionary();
        Tile centreTile = null;
        int maxXGridPos = 0;
        int maxYGridPos = 0;

        int centreTileX = 0;
        int centreTileY = 0;

        // Calculate the furthest x position on the map
        foreach (Tile tile in allTiles)
        {
            if (tile.GridPosition.X > maxXGridPos)
            {
                maxXGridPos = tile.GridPosition.X;
            }
        }

        // Calculate the furthest y position on the map
        foreach (Tile tile in allTiles)
        {
            if (tile.GridPosition.Y > maxYGridPos)
            {
                maxYGridPos = tile.GridPosition.Y;
            }
        }

        centreTileX = maxXGridPos / 2;
        centreTileY = maxYGridPos / 2;

        foreach (Tile tile in allTiles)
        {
            if(tile.GridPosition.X == centreTileX &&
               tile.GridPosition.Y == centreTileY)
            {
                centreTile = tile;
            }
        }

        return centreTile;
    }    
    public Tile GetRandomValidMoveableTile(List<Tile> tiles)
    {
        List<Tile> validTiles = new List<Tile>();

        foreach(Tile tile in tiles)
        {
            if(tile.IsEmpty == true &&
                tile.IsWalkable == true)
            {
                validTiles.Add(tile);
            }
        }

        Debug.Log("Valid tiles found: " + validTiles.Count);

        int randomIndex = UnityEngine.Random.Range(0, validTiles.Count);
        return validTiles[randomIndex];
    }
    public Tile GetTileFromPointReference(Point point)
    {
        Tile tileReturned = null;
        foreach(Tile tile in GetAllTilesFromCurrentLevelDictionary())
        {
            if(tile.GridPosition == point)
            {
                tileReturned = tile;
                break;
            }
        }
        return tileReturned;
    }   
    public void SetTileAsOccupied(Tile tile)
    {
        Debug.Log("Tile " + tile.GridPosition.X + ", " + tile.GridPosition.Y + " is now occupied");
        tile.IsEmpty = false;
    }
    public void SetTileAsUnoccupied(Tile tile)
    {
        Debug.Log("Tile " + tile.GridPosition.X + ", " + tile.GridPosition.Y + " is now occupied");
        tile.IsEmpty = true;
    }
    public List<Tile> GetAllTilesInALine(Tile startTile, Tile adjacentDirectionTile, int distance, bool removeStartTile)
    {
        Debug.Log("LevelManager. GetAllTilesInALine() called...");
        Debug.Log("Calculating a line from tile " + startTile.GridPosition.X.ToString() + ", " + startTile.GridPosition.Y.ToString() +
            " towards tile " + adjacentDirectionTile.GridPosition.X.ToString() + ", " + adjacentDirectionTile.GridPosition.Y.ToString() +
            " with a distance of " + distance.ToString());
        List<Tile> tilesReturned = new List<Tile>();
        Tile currentLoopTile = startTile;

        // Calculate direction first

        // Check north
        if (PositionLogic.Instance.IsLocationNorth(startTile, adjacentDirectionTile))
        {
            for (int i = 0; i < distance; i++)
            {
                Tile nextTile = PositionLogic.Instance.GetAdjacentNorthernTile(currentLoopTile);
                if (nextTile != null)
                {
                    tilesReturned.Add(nextTile);
                    currentLoopTile = nextTile;
                }
            }
        }
        // Check south
        else if (PositionLogic.Instance.IsLocationSouth(startTile, adjacentDirectionTile))
        {
            for (int i = 0; i < distance; i++)
            {
                Tile nextTile = PositionLogic.Instance.GetAdjacentSouthernTile(currentLoopTile);
                if (nextTile != null)
                {
                    tilesReturned.Add(nextTile);
                    currentLoopTile = nextTile;
                }
            }
        }
        // Check east
        else if (PositionLogic.Instance.IsLocationEast(startTile, adjacentDirectionTile))
        {
            for (int i = 0; i < distance; i++)
            {
                Tile nextTile = PositionLogic.Instance.GetAdjacentEasternTile(currentLoopTile);
                if (nextTile != null)
                {
                    tilesReturned.Add(nextTile);
                    currentLoopTile = nextTile;
                }
            }
        }
        // Check west
        else if (PositionLogic.Instance.IsLocationWest(startTile, adjacentDirectionTile))
        {
            for (int i = 0; i < distance; i++)
            {
                Tile nextTile = PositionLogic.Instance.GetAdjacentWesternTile(currentLoopTile);
                if (nextTile != null)
                {
                    tilesReturned.Add(nextTile);
                    currentLoopTile = nextTile;
                }
            }
        }
        // Check north east
        else if (PositionLogic.Instance.IsLocationNorthEast(startTile, adjacentDirectionTile))
        {
            for (int i = 0; i < distance; i++)
            {
                Tile nextTile = PositionLogic.Instance.GetAdjacentNorthEastTile(currentLoopTile);
                if (nextTile != null)
                {
                    tilesReturned.Add(nextTile);
                    currentLoopTile = nextTile;
                }
            }
        }
        // Check north west
        else if (PositionLogic.Instance.IsLocationNorthWest(startTile, adjacentDirectionTile))
        {
            for (int i = 0; i < distance; i++)
            {
                Tile nextTile = PositionLogic.Instance.GetAdjacentNorthWestTile(currentLoopTile);
                if (nextTile != null)
                {
                    tilesReturned.Add(nextTile);
                    currentLoopTile = nextTile;
                }
            }
        }
        // Check south east
        else if (PositionLogic.Instance.IsLocationSouthEast(startTile, adjacentDirectionTile))
        {
            for (int i = 0; i < distance; i++)
            {
                Tile nextTile = PositionLogic.Instance.GetAdjacentSouthEastTile(currentLoopTile);
                if (nextTile != null)
                {
                    tilesReturned.Add(nextTile);
                    currentLoopTile = nextTile;
                }
            }
        }
        // Check south west
        else if (PositionLogic.Instance.IsLocationSouthWest(startTile, adjacentDirectionTile))
        {
            for (int i = 0; i < distance; i++)
            {
                Tile nextTile = PositionLogic.Instance.GetAdjacentSouthWestTile(currentLoopTile);
                if (nextTile != null)
                {
                    tilesReturned.Add(nextTile);
                    currentLoopTile = nextTile;
                }
            }
        }

        if (removeStartTile)
        {
            tilesReturned.Remove(startTile);
        }

        Debug.Log("Found " + tilesReturned.Count + " tiles in a line");
        return tilesReturned;
    }
    #endregion

    // Misc Logic
    #region
    public void DestroyCurrentLevel()
    {
        
        Debug.Log("Destroying all tiles...");

        List<Tile> tilesToDestroy = new List<Tile>();

        if (Tiles != null && Tiles.Count > 0)
        {
            tilesToDestroy.AddRange(GetAllTilesFromCurrentLevelDictionary());
            Tiles.Clear();
        }

        foreach(Tile tile in tilesToDestroy)
        {            
            Destroy(tile.gameObject);
            Destroy(tile);
        }

        AStar.ClearNodes();
        // Destroy(currentLevelBG);
        

    }
    public float TileSize
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }
    #endregion




}
