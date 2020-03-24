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
        mySpellBook.EnemyLearnAbility("Summon Toxic Zombie");
        mySpellBook.EnemyLearnAbility("Toxic Rain");
        mySpellBook.EnemyLearnAbility("Chemical Reaction");
        mySpellBook.EnemyLearnAbility("Drain");

        myPassiveManager.ModifyToxicAura(2);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Staff");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability toxicRain = mySpellBook.GetAbilityByName("Toxic Rain");
        Ability summonToxicZombie = mySpellBook.GetAbilityByName("Summon Toxic Zombie");        
        Ability chemicalReaction = mySpellBook.GetAbilityByName("Chemical Reaction");
        Ability drain = mySpellBook.GetAbilityByName("Drain");
        

        ActionStart:

        SetTargetDefender(EntityLogic.GetBestTarget(this, true));

        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        // if unable to do anything, just end activation
        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            LivingEntityManager.Instance.EndEntityActivation(this);
        }

        // try move to grass/better position if there is one in range of mobility
        else if (EntityLogic.IsAbleToMove(this) &&
            EntityLogic.GetValidGrassTileWithinRange(this, EntityLogic.GetTotalMobility(this)) != null &&
            tile.myTileType != Tile.TileType.Grass &&
            EntityLogic.IsAbilityUseable(this, move)
            )
        {
            Action movementAction = AbilityLogic.Instance.PerformMove(this, EntityLogic.GetValidGrassTileWithinRange(this, EntityLogic.GetTotalMobility(this)));
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Toxic Rain
        else if (EntityLogic.IsAbilityUseable(this, toxicRain))
        {
            Action action = AbilityLogic.Instance.PerformToxicRain(this);
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(0.5f);
            goto ActionStart;
        }

        // Summon toxic zombies
        else if (EntityLogic.IsAbilityUseable(this, summonToxicZombie))
        {
            Action action = AbilityLogic.Instance.PerformSummonToxicZombie(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(0.5f);
            goto ActionStart;
        }

        // Chemical Reaction       
        else if (EntityLogic.GetBestChemicalReactionTarget(this) != null &&
                 EntityLogic.IsTargetInRange(this, EntityLogic.GetBestChemicalReactionTarget(this), chemicalReaction.abilityRange) &&
                 EntityLogic.IsAbilityUseable(this, chemicalReaction, EntityLogic.GetBestChemicalReactionTarget(this))
            )
        {
            SetTargetDefender(EntityLogic.GetBestChemicalReactionTarget(this));

            Action action = AbilityLogic.Instance.PerformChemicalReaction(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }

        // Drain       
        else if (EntityLogic.GetBestDrainTarget(this) != null &&
                 EntityLogic.IsTargetInRange(this, EntityLogic.GetBestDrainTarget(this), chemicalReaction.abilityRange) &&
                 EntityLogic.IsAbilityUseable(this, chemicalReaction, EntityLogic.GetBestDrainTarget(this))
            )
        {
            SetTargetDefender(EntityLogic.GetBestDrainTarget(this));

            Action action = AbilityLogic.Instance.PerformChemicalReaction(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);
            yield return new WaitForSeconds(1f);
            goto ActionStart;

        }

        // Strike
        else if (EntityLogic.IsAbilityUseable(this, strike, myCurrentTarget) &&
            EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange))
        {
            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) == false &&
            EntityLogic.IsAbleToMove(this) &&
            EntityLogic.IsAbilityUseable(this, move) &&
            EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this)) != null
            )
        {
            Tile destination = EntityLogic.GetBestValidMoveLocationBetweenMeAndTarget(this, myCurrentTarget, currentMeleeRange, EntityLogic.GetTotalMobility(this));
            Action movementAction = AbilityLogic.Instance.PerformMove(this, destination);
            yield return new WaitUntil(() => movementAction.ActionResolved() == true);

            // small delay here in order to seperate the two actions a bit.
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }
        // Can't do anything more, end activation
        else
        {
            LivingEntityManager.Instance.EndEntityActivation(this);
        }
    }   

 

    
}
