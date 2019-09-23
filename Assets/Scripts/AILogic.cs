using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class AILogic 
{
    public static TileScript GetBestValidMoveLocationBetweenMeAndTarget(LivingEntity characterActing, LivingEntity target, int rangeFromTarget, int movePoints)
    {
        TileScript tile = LevelManager.Instance.GetClosestValidTile(LevelManager.Instance.GetTilesWithinRange(rangeFromTarget, target.TileCurrentlyOn), characterActing.TileCurrentlyOn);
        Stack<Node> pathFromMeToIdealTile = AStar.GetPath(characterActing.TileCurrentlyOn.GridPosition, tile.GridPosition);

        Debug.Log("GetBestValidMoveLocationBetweenMeAndTarget() generated a path with " +
            pathFromMeToIdealTile.Count.ToString() + " tiles on it"
            );

        if (pathFromMeToIdealTile.Count > 2)
        {
            return pathFromMeToIdealTile.ElementAt(movePoints - 1).TileRef;
        }
        else if (pathFromMeToIdealTile.Count == 2)
        {
            return pathFromMeToIdealTile.ElementAt(0).TileRef;
        }
        else
        {
            return pathFromMeToIdealTile.ElementAt(0).TileRef;
        }

    }

    public static bool IsEngagedInMelee(LivingEntity enemyConsidered)
    {
        List<TileScript> tilesInMyMeleeRange = LevelManager.Instance.GetTilesWithinRange(1, enemyConsidered.TileCurrentlyOn);
        bool inMelee = false;

        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (tilesInMyMeleeRange.Contains(entity.TileCurrentlyOn) && CombatLogic.Instance.IsTargetFriendly(enemyConsidered, entity) == false)
            {
                inMelee = true;
            }
        }

        return inMelee;
    }


}
