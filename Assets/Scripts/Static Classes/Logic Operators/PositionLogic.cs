using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLogic : MonoBehaviour
{
    public static PositionLogic Instance;
    private void Awake()
    {
        Instance = this;
    }
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

        // if the target is already on the same X axis, dont flip sprite
        if (LevelManager.Instance.IsDestinationTileToTheLeft(character.tile, tileToFace) == false &&
            LevelManager.Instance.IsDestinationTileToTheRight(character.tile, tileToFace) == false)
        {
            Debug.Log("CalculateWhichDirectionToFace() detected character is already aligned with target, not flipping...");
            return;
        }
        
        // if the target is to the right, flip and face right
        else if (LevelManager.Instance.IsDestinationTileToTheRight(character.tile, tileToFace))
        {
            Debug.Log("CalculateWhichDirectionToFace() facing character towards the right...");
            SetDirection(character,"Right");            
        }

        // if the target is to the left, flip and face left
        else if (LevelManager.Instance.IsDestinationTileToTheRight(character.tile, tileToFace) == false)
        {
            Debug.Log("CalculateWhichDirectionToFace() facing character towards the left...");
            SetDirection(character,"Left");            
        }

    }
    #endregion

    // Get Front + Back Arcs
    #region
    public List<Tile> GetTilesInCharactersMeleeRange(LivingEntity character)
    {
        return LevelManager.Instance.GetTilesWithinRange(character.currentMeleeRange, character.tile);
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
    public Tile GetTargetsBackArcTile(LivingEntity character)
    {
        List<Tile> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, character.tile);
        Tile backArcTile = null;

        if (character.facingRight)
        {
            foreach (Tile tile in adjacentTiles)
            {
                if (tile.GridPosition.X == character.tile.GridPosition.X - 1 &&
                    tile.GridPosition.Y == character.tile.GridPosition.Y)
                {
                    backArcTile = tile;
                    break;
                }
            }
        }
        else
        {
            foreach (Tile tile in adjacentTiles)
            {
                if(tile.GridPosition.X == character.tile.GridPosition.X + 1 &&
                    tile.GridPosition.Y == character.tile.GridPosition.Y)
                {
                    backArcTile = tile;
                    break;
                }
               
            }
        }

        Debug.Log("GetTargetsBackArcTile() returned Tile (" + backArcTile.GridPosition.X.ToString() +", " +
            backArcTile.GridPosition.Y.ToString());

        return backArcTile;
    }
    #endregion

    // Conditional Checks
    #region
    public bool CanAttackerBackStrikeTarget(LivingEntity attacker, LivingEntity target)
    {
        Debug.Log("PositionLogic.CanEntityBackStrikeTarget() called...");
        List<Tile> backArcLocations = GetTargetsBackArcTiles(target);

        if (backArcLocations.Contains(attacker.tile))
        {
            Debug.Log("PositionLogic.CanEntityBackStrikeTarget() returned true...");
            return true;
        }
        else
        {
            Debug.Log("PositionLogic.CanEntityBackStrikeTarget() returned false...");
            return false;
        }
        
    }
    public bool IsLocationNorth(Tile tile, Tile location)
    {
        if (location.GridPosition.Y == tile.GridPosition.Y - 1 &&
            location.GridPosition.X == tile.GridPosition.X)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsLocationSouth(Tile tile, Tile location)
    {
        if (location.GridPosition.Y == tile.GridPosition.Y + 1 &&
            location.GridPosition.X == tile.GridPosition.X)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsLocationEast(Tile tile, Tile location)
    {
        if (location.GridPosition.X == tile.GridPosition.X + 1 &&
            location.GridPosition.Y == tile.GridPosition.Y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsLocationWest(Tile tile, Tile location)
    {
        if (location.GridPosition.X == tile.GridPosition.X - 1 &&
            location.GridPosition.Y == tile.GridPosition.Y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsLocationNorthEast(Tile tile, Tile location)
    {
        if (location.GridPosition.Y == tile.GridPosition.Y - 1 &&
            location.GridPosition.X == tile.GridPosition.X + 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsLocationNorthWest(Tile tile, Tile location)
    {
        if (location.GridPosition.Y == tile.GridPosition.Y - 1 &&
            location.GridPosition.X == tile.GridPosition.X - 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsLocationSouthEast(Tile tile, Tile location)
    {
        if (location.GridPosition.Y == tile.GridPosition.Y + 1 &&
            location.GridPosition.X == tile.GridPosition.X + 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsLocationSouthWest(Tile tile, Tile location)
    {
        if (location.GridPosition.Y == tile.GridPosition.Y + 1 &&
            location.GridPosition.X == tile.GridPosition.X - 1)
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
        Debug.Log("PositionLogic.GetAdjacentNorthernTile() called...");
        List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        Tile tileReturned = null;

        foreach (Tile tile in allTiles)
        {
            if (IsLocationNorth(location, tile))
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentNorthernTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public Tile GetAdjacentSouthernTile(Tile location)
    {
        Debug.Log("PositionLogic.GetAdjacentSouthernTile() called...");
        List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        Tile tileReturned = null;

        foreach (Tile tile in allTiles)
        {
            if (IsLocationSouth(location, tile))
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentSouthernTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public Tile GetAdjacentEasternTile(Tile location)
    {
        Debug.Log("PositionLogic.GetAdjacentEasternTile() called...");
        List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        Tile tileReturned = null;

        foreach (Tile tile in allTiles)
        {
            if (IsLocationEast(location, tile))
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentEasternTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public Tile GetAdjacentWesternTile(Tile location)
    {
        Debug.Log("PositionLogic.GetAdjacentWesternTile() called...");
        List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        Tile tileReturned = null;

        foreach (Tile tile in allTiles)
        {
            if (IsLocationWest(location, tile))
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentWesternTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    #endregion

    // Get adjacent NE, NW, SE, SW tile logic
    #region
    public Tile GetAdjacentNorthEastTile(Tile location)
    {
        Debug.Log("PositionLogic.GetAdjacentNorthEastTile() called...");
        List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        Tile tileReturned = null;

        foreach (Tile tile in allTiles)
        {
            if (IsLocationNorthEast(location, tile))
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentNorthEastTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public Tile GetAdjacentNorthWestTile(Tile location)
    {
        Debug.Log("PositionLogic.GetAdjacentNorthWestTile() called...");
        List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        Tile tileReturned = null;

        foreach (Tile tile in allTiles)
        {
            if (IsLocationNorthWest(location, tile))
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentNorthWestTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public Tile GetAdjacentSouthEastTile(Tile location)
    {
        Debug.Log("PositionLogic.GetAdjacentSouthEastTile() called...");
        List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        Tile tileReturned = null;

        foreach (Tile tile in allTiles)
        {
            if (IsLocationSouthEast(location, tile))
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentSouthEastTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public Tile GetAdjacentSouthWestTile(Tile location)
    {
        Debug.Log("PositionLogic.GetAdjacentSouthWestTile() called...");
        List<Tile> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        Tile tileReturned = null;

        foreach (Tile tile in allTiles)
        {
            if (IsLocationSouthWest(location, tile))
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentSouthWestTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    #endregion

    // Line of Sight Logic
    #region
    public bool IsThereLosFromAtoB(Tile a, Tile b)
    {
        Debug.Log("PositionLogic.IsThereLosFromAtoB() called, checking LoS from tile " +
            a.GridPosition.X.ToString() + ", " + a.GridPosition.Y.ToString() + " to tile " +
            b.GridPosition.X.ToString() + ", " + b.GridPosition.Y.ToString());

        bool hasLoS = true;

        // first ray cast and check from the tile the attacker is on        
        RaycastHit2D[] hits = Physics2D.LinecastAll(a.WorldPosition, b.WorldPosition);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.GetComponent<Tile>())
            {
                if(hit.transform.GetComponent<Tile>().BlocksLoS == true)
                {
                    hasLoS = false;
                    break;
                }                
            }
        }

        return hasLoS;
    }
    #endregion

}
