using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityLibrary : MonoBehaviour
{   
    [Header("Properties")]
    public List<AbilityDataSO> AllAbilities;

    // Initialization + Singleton Pattern
    #region
    public static AbilityLibrary Instance;
    private void Awake()
    {
        Instance = this;        
    }
    #endregion

    public AbilityDataSO GetAbilityByName(string name)
    {
        AbilityDataSO abilityReturned = null;

        foreach(AbilityDataSO ability in AllAbilities)
        {
            if(ability.abilityName == name)
            {
                abilityReturned = ability;
            }
        }

        if (abilityReturned == null)
        {
            Debug.Log("AbilityLibrary.GetAbilityByName() couldn't find an ability called '" + name + "', returning null...");
        }

        return abilityReturned;
    }    

}
