    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonGolem : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Poison Golem";
        base.SetBaseProperties();

        CharacterModelController.SetUpAsPoisonGolemPreset(myModel);

        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Strike");
        mySpellBook.EnemyLearnAbility("Noxious Fumes");
        mySpellBook.EnemyLearnAbility("Toxic Slash");
        mySpellBook.EnemyLearnAbility("Chemical Reaction");

        myPassiveManager.ModifyToxicAura(2);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Staff");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability toxicSlash = mySpellBook.GetAbilityByName("Toxic Slash");       
        Ability chemicalReaction = mySpellBook.GetAbilityByName("Chemical Reaction");
        Ability noxiousFumes = mySpellBook.GetAbilityByName("Noxious Fumes");


    ActionStart:

        SetTargetDefender(EntityLogic.GetBestTarget(this, true));

        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            LivingEntityManager.Instance.EndEntityActivation(this);
        }

        // Noxious Fumes
        else if (EntityLogic.GetAllEnemiesWithinRange(this, 1).Count > 0 &&
            EntityLogic.IsAbilityUseable(this, noxiousFumes, myCurrentTarget))
        {
            Action action = AbilityLogic.Instance.PerformNoxiousFumes(this);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Toxic Slash
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, toxicSlash, myCurrentTarget))
        {
            Action action = AbilityLogic.Instance.PerformToxicSlash(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            yield return new WaitForSeconds(1f);
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

        // Strike
        else if (EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) &&
            EntityLogic.IsAbilityUseable(this, strike, myCurrentTarget))
        {
            Action action = AbilityLogic.Instance.PerformStrike(this, myCurrentTarget);
            yield return new WaitUntil(() => action.ActionResolved() == true);

            // brief delay between actions
            yield return new WaitForSeconds(1f);
            goto ActionStart;
        }

        // Move
        else if (
            EntityLogic.IsTargetInRange(this, myCurrentTarget, currentMeleeRange) == false &&
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
