using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrimReaper : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();

        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Doom");
        mySpellBook.EnemyLearnAbility("Whirlwind");
        mySpellBook.EnemyLearnAbility("Crushing Blow");

        //myPassiveManager.LearnSoulDrainAura(1);
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability doom = mySpellBook.GetAbilityByName("Doom");
        Ability whirlwind = mySpellBook.GetAbilityByName("Whirlwind");
        Ability crushingBlow = mySpellBook.GetAbilityByName("Crushing Blow");

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }

        yield return null;

        EndMyActivation();
    }

    public bool IsAdjacentToTwoOrMoreDefenders()
    {
        int adjacentDefenders = 0;
        List<Tile> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, tile);

        foreach(Defender defender in DefenderManager.Instance.allDefenders)
        {
            if (adjacentTiles.Contains(defender.tile))
            {
                adjacentDefenders++;
            }
        }

        if(adjacentDefenders >= 2)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public Tile GetClosestTileThatHasTwoAdjacentDefenders()
    {
        Tile tileReturned = null;
        List<Tile> tilesWithTwoAdjacentDefenders = new List<Tile>();

        foreach(Tile tile in LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary())
        {
            int numberOfAdjacentEnemies = 0;
            List<Tile> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, tile);
            foreach(Defender defender in DefenderManager.Instance.allDefenders)
            {
                if (adjacentTiles.Contains(defender.tile))
                {
                    numberOfAdjacentEnemies++;
                }
            }

            if(numberOfAdjacentEnemies >= 2)
            {
                tilesWithTwoAdjacentDefenders.Add(tile);
            }
        }

        tileReturned = LevelManager.Instance.GetClosestValidTile(tilesWithTwoAdjacentDefenders, tile);
        return tileReturned;
    }
    public void PerformDoom()
    {
        foreach (Defender defender in DefenderManager.Instance.allDefenders)
        {
            defender.ModifyCurrentEnergy(-1);
        }
    }

    public void PerformCrushingBlow(Defender target)
    {
        Ability crushingBlow = mySpellBook.GetAbilityByName("Crushing Blow");
        
       //StartCoroutine(AttackTarget(target, crushingBlow.abilityPrimaryValue, false));
        StartCoroutine(AttackMovement(target));
        //target.ApplyStunned();
    }
}
