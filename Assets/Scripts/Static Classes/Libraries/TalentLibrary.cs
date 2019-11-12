using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentLibrary : Singleton<TalentLibrary>
{
    [Header("Properties")]
    public List<TalentDataSO> allTalents;
    public TalentDataSO GetTalentDataByName(string talentName)
    {
        TalentDataSO talentReturned = null;

        foreach (TalentDataSO talent in allTalents)
        {
            if (talent.Name == talentName)
            {
                talentReturned = talent;
                break;
            }
        }

        return talentReturned;
    }
}
