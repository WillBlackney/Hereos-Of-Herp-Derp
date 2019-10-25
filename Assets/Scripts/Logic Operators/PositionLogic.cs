﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLogic : Singleton<PositionLogic>
{
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

    public void CalculateWhichDirectionToFace(LivingEntity character, TileScript tileToFace)
    {
        // flip the sprite's x axis depending on the direction of movement
        if (LevelManager.Instance.IsDestinationTileToTheRight(character.TileCurrentlyOn, tileToFace))
        {
            Debug.Log("CalculateWhichDirectionToFace() facing character towards the right...");
            SetDirection(character,"Right");            
        }

        else if (LevelManager.Instance.IsDestinationTileToTheRight(character.TileCurrentlyOn, tileToFace) == false)
        {
            Debug.Log("CalculateWhichDirectionToFace() facing character towards the left...");
            SetDirection(character,"Left");            
        }

    }

    public List<TileScript> GetTargetsFrontArcTiles(LivingEntity character)
    {
        List<TileScript> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, character.TileCurrentlyOn);
        List<TileScript> frontArcTiles = new List<TileScript>();

        if (character.facingRight)
        {
            foreach(TileScript tile in adjacentTiles)
            {
                if (
                    (tile.GridPosition.X == character.TileCurrentlyOn.GridPosition.X + 1 || 
                    tile.GridPosition.X == character.TileCurrentlyOn.GridPosition.X) &&
                    ((tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y) || 
                    (tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y +1) || 
                    (tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y - 1)
                    ))                    
                {
                    frontArcTiles.Add(tile);
                }
            }
        }
        else
        {
            foreach (TileScript tile in adjacentTiles)
            {
                if (
                    (tile.GridPosition.X == character.TileCurrentlyOn.GridPosition.X - 1 ||
                    tile.GridPosition.X == character.TileCurrentlyOn.GridPosition.X) &&
                    ((tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y) ||
                    (tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y + 1) ||
                    (tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y - 1)
                    ))
                {
                    frontArcTiles.Add(tile);
                }
            }
        }

        Debug.Log("GetTargetsFrontArcTiles() returned " + frontArcTiles.Count.ToString() + " tiles");
        return frontArcTiles;
    }

    public List<TileScript> GetTargetsBackArcTiles(LivingEntity character)
    {
        List<TileScript> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, character.TileCurrentlyOn);
        List<TileScript> backArcTiles = new List<TileScript>();

        if (character.facingRight)
        {
            foreach (TileScript tile in adjacentTiles)
            {
                if (tile.GridPosition.X == character.TileCurrentlyOn.GridPosition.X - 1 &&
                    ((tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y) ||
                    (tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y + 1) ||
                    (tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y - 1)
                    ))
                {
                    backArcTiles.Add(tile);
                }
            }
        }
        else
        {
            foreach (TileScript tile in adjacentTiles)
            {
                if (tile.GridPosition.X == character.TileCurrentlyOn.GridPosition.X + 1 &&
                    ((tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y) ||
                    (tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y + 1) ||
                    (tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y - 1)
                    ))
                {
                    backArcTiles.Add(tile);
                }
            }
        }

        Debug.Log("GetTargetsBackArcTiles() returned " + backArcTiles.Count.ToString() + " tiles");
        return backArcTiles;
    }

    
    public bool IsWithinTargetsBackArc(LivingEntity attacker, LivingEntity target)
    {     
        List<TileScript> backArcLocations = GetTargetsBackArcTiles(target);

        if (backArcLocations.Contains(attacker.TileCurrentlyOn))
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
    

   
}
