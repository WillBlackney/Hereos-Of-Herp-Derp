using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLogic : Singleton<PositionLogic>
{
    // Calculate Direction + Visual
    #region
    public void FlipCharacterSprite(LivingEntity character, bool faceRight)
    {
        if(character.spriteImportedFacingRight == true)
        {
            if (faceRight == true)
            {
                character.mySpriteRenderer.flipX = false;
                if (character.myModelParent != null)
                {
                    character.myModelParent.transform.localScale = new Vector3(1, 1, 1);
                }
            }

            else
            {
                character.mySpriteRenderer.flipX = true;
                if (character.myModelParent != null)
                {
                    character.myModelParent.transform.localScale = new Vector3(-1, 1, 1);
                }
            }
        }

        else if (character.spriteImportedFacingRight == false)
        {
            if (faceRight == true)
            {
                character.mySpriteRenderer.flipX = true;
                if (character.myModelParent != null)
                {
                    character.myModelParent.transform.localScale = new Vector3(-1, 1, 1);
                }
            }

            else
            {
                character.mySpriteRenderer.flipX = false;
                if (character.myModelParent != null)
                {
                    character.myModelParent.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }

    }
    public void SetDirection(LivingEntity character, string leftOrRight)
    {
        if (leftOrRight == "Left")
        {
            FlipCharacterSprite(character, false);
            //character.FlipMySprite(false);
            character.facingRight = false;
        }
        else if (leftOrRight == "Right")
        {
            FlipCharacterSprite(character, true);
            //character.FlipMySprite(true);
            character.facingRight = true;
        }
    }
    public void CalculateWhichDirectionToFace(LivingEntity character, Tile tileToFace)
    {
        // flip the sprite's x axis depending on the direction of movement
        if (LevelManager.Instance.IsDestinationTileToTheRight(character.tile, tileToFace))
        {
            Debug.Log("CalculateWhichDirectionToFace() facing character towards the right...");
            SetDirection(character,"Right");            
        }

        else if (LevelManager.Instance.IsDestinationTileToTheRight(character.tile, tileToFace) == false)
        {
            Debug.Log("CalculateWhichDirectionToFace() facing character towards the left...");
            SetDirection(character,"Left");            
        }

    }
    #endregion

    // Get Front + Back Arcs
    #region
    public List<Tile> GetTargetsFrontArcTiles(LivingEntity character)
    {
        List<Tile> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, character.tile);
        List<Tile> frontArcTiles = new List<Tile>();

        if (character.facingRight)
        {
            foreach(Tile tile in adjacentTiles)
            {
                if (
                    (tile.GridPosition.X == character.tile.GridPosition.X + 1 || 
                    tile.GridPosition.X == character.tile.GridPosition.X) &&
                    ((tile.GridPosition.Y == character.tile.GridPosition.Y) || 
                    (tile.GridPosition.Y == character.tile.GridPosition.Y +1) || 
                    (tile.GridPosition.Y == character.tile.GridPosition.Y - 1)
                    ))                    
                {
                    frontArcTiles.Add(tile);
                }
            }
        }
        else
        {
            foreach (Tile tile in adjacentTiles)
            {
                if (
                    (tile.GridPosition.X == character.tile.GridPosition.X - 1 ||
                    tile.GridPosition.X == character.tile.GridPosition.X) &&
                    ((tile.GridPosition.Y == character.tile.GridPosition.Y) ||
                    (tile.GridPosition.Y == character.tile.GridPosition.Y + 1) ||
                    (tile.GridPosition.Y == character.tile.GridPosition.Y - 1)
                    ))
                {
                    frontArcTiles.Add(tile);
                }
            }
        }

        Debug.Log("GetTargetsFrontArcTiles() returned " + frontArcTiles.Count.ToString() + " tiles");
        return frontArcTiles;
    }
    public List<Tile> GetTargetsBackArcTiles(LivingEntity character)
    {
        List<Tile> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, character.tile);
        List<Tile> backArcTiles = new List<Tile>();

        if (character.facingRight)
        {
            foreach (Tile tile in adjacentTiles)
            {
                if (tile.GridPosition.X == character.tile.GridPosition.X - 1 &&
                    ((tile.GridPosition.Y == character.tile.GridPosition.Y) ||
                    (tile.GridPosition.Y == character.tile.GridPosition.Y + 1) ||
                    (tile.GridPosition.Y == character.tile.GridPosition.Y - 1)
                    ))
                {
                    backArcTiles.Add(tile);
                }
            }
        }
        else
        {
            foreach (Tile tile in adjacentTiles)
            {
                if (tile.GridPosition.X == character.tile.GridPosition.X + 1 &&
                    ((tile.GridPosition.Y == character.tile.GridPosition.Y) ||
                    (tile.GridPosition.Y == character.tile.GridPosition.Y + 1) ||
                    (tile.GridPosition.Y == character.tile.GridPosition.Y - 1)
                    ))
                {
                    backArcTiles.Add(tile);
                }
            }
        }

        Debug.Log("GetTargetsBackArcTiles() returned " + backArcTiles.Count.ToString() + " tiles");
        return backArcTiles;
    }
    #endregion

    // Conditional Checks
    #region
    public bool IsWithinTargetsBackArc(LivingEntity attacker, LivingEntity target)
    {     
        List<Tile> backArcLocations = GetTargetsBackArcTiles(target);

        if (backArcLocations.Contains(attacker.tile))
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
    #endregion

    // Get adjacent NSEW tile logic
    #region
    public Tile GetAdjacentNorthernTile(Tile location)
    {
        List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        Tile tileReturned = null;

        foreach (Tile tile in allTiles)
        {
            if (tile.GridPosition.Y == location.GridPosition.Y - 1 &&
                tile.GridPosition.X == location.GridPosition.X)
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public Tile GetAdjacentSouthernTile(Tile location)
    {
        List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        Tile tileReturned = null;

        foreach (Tile tile in allTiles)
        {
            if (tile.GridPosition.Y == location.GridPosition.Y + 1 &&
                tile.GridPosition.X == location.GridPosition.X)
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public Tile GetAdjacentEasternTile(Tile location)
    {
        List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        Tile tileReturned = null;

        foreach (Tile tile in allTiles)
        {
            if (tile.GridPosition.X == location.GridPosition.X + 1 &&
                tile.GridPosition.Y == location.GridPosition.Y)
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public Tile GetAdjacentWesternTile(Tile location)
    {
        List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        Tile tileReturned = null;

        foreach (Tile tile in allTiles)
        {
            if (tile.GridPosition.X == location.GridPosition.X - 1 &&
                tile.GridPosition.Y == location.GridPosition.Y)
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    #endregion

    // Get adjacent NE, NW, SE, SW tile logic
    #region
    public Tile GetAdjacentNorthEastTile(Tile location)
    {
        List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        Tile tileReturned = null;

        foreach (Tile tile in allTiles)
        {
            if (tile.GridPosition.Y == location.GridPosition.Y - 1 &&
                tile.GridPosition.X == location.GridPosition.X + 1)
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public Tile GetAdjacentNorthWestTile(Tile location)
    {
        List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        Tile tileReturned = null;

        foreach (Tile tile in allTiles)
        {
            if (tile.GridPosition.Y == location.GridPosition.Y - 1 &&
                tile.GridPosition.X == location.GridPosition.X - 1)
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public Tile GetAdjacentSouthEastTile(Tile location)
    {
        List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        Tile tileReturned = null;

        foreach (Tile tile in allTiles)
        {
            if (tile.GridPosition.Y == location.GridPosition.Y + 1 &&
                tile.GridPosition.X == location.GridPosition.X + 1)
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public Tile GetAdjacentSouthWestTile(Tile location)
    {
        List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        Tile tileReturned = null;

        foreach (Tile tile in allTiles)
        {
            if (tile.GridPosition.Y == location.GridPosition.Y + 1 &&
                tile.GridPosition.X == location.GridPosition.X - 1)
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    #endregion

}
