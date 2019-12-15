using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
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
        Vector3 worldStart = new Vector3(0,0,0);

        // Create all tiles
        for (int y = 0; y < mapY; y++) // the y positions
        {
            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapX; x++) // the x positions
            {
                PlaceTile(newTiles[x].ToString(), x,y, worldStart);
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
            //tile.ColorTile(tile.highlightedColor);
            HighlightTile(tile);
        }
    }
    public void HighlightTile(Tile tile)
    {
        HighlightedTiles.Add(tile);
        //tile.ColorTile(tile.highlightedColor);
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

        return listReturned;
    }
    public List<Tile> GetTilesWithinRange(int range, Tile tileFrom, bool removeTileFrom = true)
    {
        // iterate through every tile, and add those within range to the temp list        
        List<Tile> allTiles = GetAllTilesFromCurrentLevelDictionary();
        List<Tile> allTilesWithinXPosRange = new List<Tile>();
        List<Tile> allTilesWithinRange = new List<Tile>();

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
        
        
        //Debug.Log("Tiles within movement range: " + allTilesWithinRange.Count);
        return allTilesWithinRange;
    }
    public List<Tile> GetValidMoveableTilesWithinRange(int range, Tile tileFrom)
    {
        // iterate through every tile, and add those within range to the temp list        
        Tile[] allTiles = FindObjectsOfType<Tile>();
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
        foreach (Tile tile in allTilesWithinRange)
        {
            Stack<Node> path = AStar.GetPath(tileFrom.GridPosition, tile.GridPosition);
            if (path.Count <= range)
            {
                allTilesWithinMobilityRange.Add(tile);
            }
        }

        //Debug.Log("Tiles within range: " + allTilesWithinRange.Count);
        return allTilesWithinMobilityRange;
    }
    public List<Tile> GetTilesWithinMovementRange(int range, Tile tileFrom)
    {
        // iterate through every tile, and add those within range to the temp list        
        Tile[] allTiles = FindObjectsOfType<Tile>();
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
                if (tile.IsEmpty == true && tile.IsWalkable == true)
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
        foreach(Tile tile in allTilesWithinRange)
        {
            Stack<Node> path = AStar.GetPath(tileFrom.GridPosition, tile.GridPosition);
            if(path.Count <= range)
            {
                allTilesWithinMobilityRange.Add(tile);
            }
        }

        //Debug.Log("Tiles within range: " + allTilesWithinRange.Count);
        return allTilesWithinMobilityRange;
    }    
    public List<Tile> GetEnemySpawnTiles()
    {
        List<Tile> allTiles = GetAllTilesFromCurrentLevelDictionary();
        List<Tile> enemySpawnTiles = new List<Tile>();

        foreach (Tile tile in allTiles)
        {
            if (
                (tile.GridPosition.X == 6) 
                && (tile.GridPosition.Y == 1 || tile.GridPosition.Y == 2 || tile.GridPosition.Y == 3 || tile.GridPosition.Y == 4)
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

        // Declare new temp list for storing defender 
        //List<Defender> defenders = new List<Defender>();

        // Add all active defenders to the temp list


        // Iterate throught the temp list to find the closest defender to this enemy
        foreach (Tile tile in tiles)
        {
            float distancefromTileFrom = Vector2.Distance(tile.gameObject.transform.position, transform.position);
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
        return Tiles[point];
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

        // Destroy(currentLevelBG);
        
    }
    public float TileSize
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }
    #endregion




}
