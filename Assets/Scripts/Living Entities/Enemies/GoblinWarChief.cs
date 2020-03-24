using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinWarChief : Enemy
{
    public override void SetBaseProperties()
    {
        myName = "Goblin War Chief";
        base.SetBaseProperties();

        CharacterModelController.SetUpAsGoblinWarChiefPreset(myModel);

        mySpellBook.EnemyLearnAbility("Invigorate");
        mySpellBook.EnemyLearnAbility("Goblin War Cry");
        mySpellBook.EnemyLearnAbility("Move");
        mySpellBook.EnemyLearnAbility("Strike");

        myPassiveManager.ModifyNimble(1);
        myPassiveManager.ModifyFury(1);

        myMainHandWeapon = ItemLibrary.Instance.GetItemByName("Simple Battle Axe");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability move = mySpellBook.GetAbilityByName("Move");
        Ability strike = mySpellBook.GetAbilityByName("Strike");
        Ability goblinWarCry = mySpellBook.GetAbilityByName("Goblin War Cry");
        Ability invigorate = mySpellBook.GetAbilityByName("Invigorate");

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
        
        // Goblin War Cry
        else if (EntityLogic.IsAbilityUseable(this, goblinWarCry))
        {
            Action action = AbilityLogic.Instance.PerformGoblinWarCry(this);
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

        // Invigorate 
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetBestInvigorateTarget(this), invigorate.abilityRange) &&
            EntityLogic.IsAbilityUseable(this, invigorate, EntityLogic.GetBestInvigorateTarget(this)))
        {
            Action action = AbilityLogic.Instance.PerformInvigorate(this, EntityLogic.GetBestInvigorateTarget(this));
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
