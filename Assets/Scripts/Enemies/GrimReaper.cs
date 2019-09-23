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

        ActionStart:

        if (IsAbleToTakeActions() == false)
        {
            EndMyActivation();
        }

        // Doom
        else if (IsAbilityOffCooldown(doom.abilityCurrentCooldownTime) &&            
            HasEnoughAP(currentAP, doom.abilityAPCost)             
            )
        {
            // Doom           
            
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Doom", false));
            yield return new WaitForSeconds(0.6f);
            PerformDoom();            
            OnAbilityUsed(doom, this);
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }

        // Crushing Blow
        else if (IsTargetInRange(GetClosestDefender(), currentMeleeRange) &&
           HasEnoughAP(currentAP, crushingBlow.abilityAPCost) &&
           IsAbilityOffCooldown(crushingBlow.abilityCurrentCooldownTime))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Crushing Blow", false));
            yield return new WaitForSeconds(0.6f);
            PerformCrushingBlow(GetClosestDefender());
            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }

        // try to move to a position that we can hit two or more enemies with a whirlwind from
        else if(GetClosestTileThatHasTwoAdjacentDefenders() != null &&
            LevelManager.Instance.IsTileWithinMovementRange(GetClosestTileThatHasTwoAdjacentDefenders(),TileCurrentlyOn,  currentMobility) &&
            IsAdjacentToTwoOrMoreDefenders() == false &&
            HasEnoughAP(currentAP, move.abilityAPCost) &&
            IsAbilityOffCooldown(move.abilityCurrentCooldownTime) &&
            IsAbilityOffCooldown(whirlwind.abilityCurrentCooldownTime)
            )
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            SetPath(AStar.GetPath(TileCurrentlyOn.GridPosition, GetClosestTileThatHasTwoAdjacentDefenders().GridPosition));            
            yield return new WaitForSeconds(0.6f);
            StartCoroutine(Move(currentMobility));
            yield return new WaitUntil(() => MovementFinished() == true);
            OnAbilityUsed(move, this);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Whirlwind
        else if (IsTargetInRange(GetClosestDefender(), currentMeleeRange) &&
            HasEnoughAP(currentAP, whirlwind.abilityAPCost) &&
            IsAbilityOffCooldown(whirlwind.abilityCurrentCooldownTime))
        {
            //PerformWhirlwind();
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Whirlwind", false));
            yield return new WaitForSeconds(1f);
            CombatLogic.Instance.CreateAoEAttackEvent(this, whirlwind, TileCurrentlyOn, currentMeleeRange, true, false);
            OnAbilityUsed(whirlwind, this);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        else if (IsTargetInRange(GetClosestDefender(), currentMeleeRange) && HasEnoughAP(currentAP, strike.abilityAPCost))
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike", false));
            yield return new WaitForSeconds(0.6f);
            StartCoroutine(AttackTarget(GetClosestDefender(), strike.abilityPrimaryValue, false));
            StartCoroutine(AttackMovement(GetClosestDefender()));
            OnAbilityUsed(strike, this);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (IsTargetInRange(myCurrentTarget, currentMeleeRange) == false && IsAbleToMove() && HasEnoughAP(currentAP, move.abilityAPCost))
        {
            SetTargetDefender(GetClosestDefender());
            GeneratePathToClosestTileWithinRangeOfTarget(myCurrentTarget, currentMeleeRange);
            SetPath(path);
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move", false));
            yield return new WaitForSeconds(0.6f);
            StartCoroutine(Move(currentMobility));
            yield return new WaitUntil(() => MovementFinished() == true);
            OnAbilityUsed(move, this);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();
    }

    public bool IsAdjacentToTwoOrMoreDefenders()
    {
        int adjacentDefenders = 0;
        List<TileScript> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, TileCurrentlyOn);

        foreach(Defender defender in DefenderManager.Instance.allDefenders)
        {
            if (adjacentTiles.Contains(defender.TileCurrentlyOn))
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
    public TileScript GetClosestTileThatHasTwoAdjacentDefenders()
    {
        TileScript tileReturned = null;
        List<TileScript> tilesWithTwoAdjacentDefenders = new List<TileScript>();

        foreach(TileScript tile in LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary())
        {
            int numberOfAdjacentEnemies = 0;
            List<TileScript> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, tile);
            foreach(Defender defender in DefenderManager.Instance.allDefenders)
            {
                if (adjacentTiles.Contains(defender.TileCurrentlyOn))
                {
                    numberOfAdjacentEnemies++;
                }
            }

            if(numberOfAdjacentEnemies >= 2)
            {
                tilesWithTwoAdjacentDefenders.Add(tile);
            }
        }

        tileReturned = LevelManager.Instance.GetClosestValidTile(tilesWithTwoAdjacentDefenders, TileCurrentlyOn);
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
        
        StartCoroutine(AttackTarget(target, crushingBlow.abilityPrimaryValue, false));
        StartCoroutine(AttackMovement(target));
        target.ApplyStunned();
    }
}
