using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkeletonNecromancer : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Skeleton Necromancer";
        base.SetBaseProperties();       

        CharacterModelController.SetUpAsSkeletonNecromancerPreset(myModel);

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Summon Undead");
        mySpellBook.EnemyLearnAbility("Blight");

        myPassiveManager.ModifyUndead();
        myPassiveManager.ModifyToxicAura(2);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Staff");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability summonUndead = mySpellBook.GetAbilityByName("Summon Undead");        
        Ability blight = mySpellBook.GetAbilityByName("Blight");
        Ability strike = mySpellBook.GetAbilityByName("Strike");

        ActionStart:

        SetTargetDefender(EntityLogic.GetBestTarget(this, true));

        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        // if unable to do anything, just end activation
        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            EndMyActivation();
        }

        // try move to grass/better position if there is one in range of mobility
        else if (EntityLogic.IsAbleToMove(this) &&
            EntityLogic.GetValidGrassTileWithinRange(this, EntityLogic.GetTotalMobility(this)) != null &&
            tile.myTileType != Tile.TileType.Grass &&
            EntityLogic.IsAbilityUseable(this,move)
            )
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move");
            yield return new WaitForSeconds(0.5f);

            Action movementAction = AbilityLogic.Instance.PerformMove(this, EntityLogic.GetValidGrassTileWithinRange(this, EntityLogic.GetTotalMobility(this)));
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Blight
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, blight.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, blight))
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Blight");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformBlight(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Summon skeletons
        else if (EntityLogic.IsAbilityUseable(this, summonUndead))
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Summon Undead");
            yield return new WaitForSeconds(0.5f);
            Action action = AbilityLogic.Instance.PerformSummonUndead(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(0.5f);
            goto ActionStart;
        }

        // Strike
        else if (EntityLogic.IsAbilityUseable(this, strike) &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange))
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Strike");
            yield return new WaitForSeconds(0.5f);

            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move in range for Blight
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, blight.abilityRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this)) != null &&
            EntityLogic.CanPerformAbilityTwoAfterAbilityOne(move, blight, this)
            )
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Move");
            yield return new WaitForSeconds(0.5f);

            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this));
            Action movementAction = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        EndMyActivation();
    }   

 

    
}
