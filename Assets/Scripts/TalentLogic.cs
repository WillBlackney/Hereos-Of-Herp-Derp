using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentLogic : Singleton<TalentLogic>
{    
    public bool DoesCharacterHaveTalent(LivingEntity entity, string talentName)
    {
        if(talentName == "Improved Invigorate")
        {
            if (entity.defender &&
                entity.defender.myCharacterData.KnowsImprovedInvigorate)
            {
                return true;
            }
        }

        else
        {
            return false;
        }

        return false;
    }

}
