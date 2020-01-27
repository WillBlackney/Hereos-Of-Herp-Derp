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
        DontDestroyOnLoad(this);
    }
    #endregion

    public AbilityDataSO GetAbilityByName(string _name)
    {
        Debug.Log("AbilityLibrary.GetAbilityByName() called, searching with provided string: " + _name);

        AbilityDataSO abilityReturned = null;

        foreach(AbilityDataSO ability in AllAbilities)
        {
            if(ability.abilityName == _name)
            {
                abilityReturned = ability;
                break;
            }
        }

        if (abilityReturned == null)
        {
            Debug.Log("AbilityLibrary.GetAbilityByName() couldn't find an ability called '" + _name + "', returning null...");
        }

        return abilityReturned;
    }    

}
