using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileSpriteManager : Singleton<TileSpriteManager>
{
    [Header("Component References")]
    public GameObject edgePrefabGO;

    [Header("Tile Edge Sprites")]
    public Sprite northEdge;
    public Sprite southEdge;
    public Sprite westEdge;
    public Sprite eastEdge;
    public Sprite northEastEdge;
    public Sprite northWestEdge;
    public Sprite southEastEdge;
    public Sprite southWestEdge;

    // Edge + position checks
    #region
    public bool NorthEdge(Tile tile)
    {
        if (PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SouthEdge(Tile tile)
    {
        if (PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool EastEdge(Tile tile)
    {
        if (PositionLogic.Instance.GetAdjacentEasternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool WestEdge(Tile tile)
    {
        if (PositionLogic.Instance.GetAdjacentWesternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool NorthEastEdge(Tile tile)
    {
        if (PositionLogic.Instance.GetAdjacentNorthEastTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool NorthWestEdge(Tile tile)
    {
        if (PositionLogic.Instance.GetAdjacentNorthWestTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SouthWestEdge(Tile tile)
    {
        if (PositionLogic.Instance.GetAdjacentSouthWestTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SouthEastEdge(Tile tile)
    {
        if (PositionLogic.Instance.GetAdjacentSouthEastTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    // Get + Create edge sprites
    #region
    public void DetermineAndSetEdgeSprites(Tile tile)
    {
        if(tile.myTileType != Tile.TileType.Dirt)
        {
            return;
        }
        // Set edge sprites
        if (NorthEdge(tile))
        {
            CreateSpriteOverTile(tile, northEdge,2);
        }
        if (SouthEdge(tile))
        {
            CreateSpriteOverTile(tile, southEdge, 2);
        }
        if (WestEdge(tile))
        {
            CreateSpriteOverTile(tile, westEdge, 2);
        }
        if (EastEdge(tile))
        {
            CreateSpriteOverTile(tile, eastEdge, 2);
        }
        if (NorthEastEdge(tile))
        {
            CreateSpriteOverTile(tile, northEastEdge, 1);
        }
        if (SouthEastEdge(tile))
        {
            CreateSpriteOverTile(tile, southEastEdge, 1);
        }
        if (NorthWestEdge(tile))
        {
            CreateSpriteOverTile(tile, northWestEdge, 1);
        }
        if (SouthWestEdge(tile))
        {
            CreateSpriteOverTile(tile, southWestEdge, 1);
        }
    }
    public void CreateSpriteOverTile(Tile location, Sprite edgeSprite, int sortingOrder = 1)
    {
        GameObject newEdgeSprite = Instantiate(edgePrefabGO);
        newEdgeSprite.transform.position = location.transform.position;
        newEdgeSprite.GetComponent<SpriteRenderer>().sprite = edgeSprite;
        newEdgeSprite.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
    }
    
    #endregion

    
   
   
    
   


}
