using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGolem : Enemy
{
    public override void SetBaseProperties()
    {
        base.SetBaseProperties();
        mySpellBook.EnemyLearnAbility("Rock Toss");
        mySpellBook.EnemyLearnAbility("Move");
    }

    public override IEnumerator StartMyActivationCoroutine()
    {
        Ability rockToss = mySpellBook.GetAbilityByName("Rock Toss");
        Ability move = mySpellBook.GetAbilityByName("Move");
        

        ActionStart:
        while (EventManager.Instance.gameOverEventStarted)
        {
            yield return null;
        }

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            LivingEntityManager.Instance.EndEntityActivation(this);
        }

        // Rock Toss
        else if (EntityLogic.IsTargetInRange(this, EntityLogic.GetClosestEnemy(this), rockToss.abilityRange) &&
            EntityLogic.IsAbilityUseable(this,rockToss )
            )
        {
            
            SetTargetDefender(EntityLogic.GetClosestEnemy(this));
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
