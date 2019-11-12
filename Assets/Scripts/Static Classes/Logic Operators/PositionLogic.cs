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
            }

            else
            {
                character.mySpriteRenderer.flipX = true;
            }
        }

        else if (character.spriteImportedFacingRight == false)
        {
            if (faceRight == true)
            {
                character.mySpriteRenderer.flipX = true;
            }

            else
            {
                character.mySpriteRenderer.flipX = false;
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



}
