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
    public AbilityDataSO GetRandomAbility()
    {
        Debug.Log("AbilityLibrary.GetRandomAbility() called...");

        return AllAbilities[Random.Range(0, AllAbilities.Count)];
    }
    public AbilityDataSO GetRandomValidAbilityTomeAbility()
    {
        List<AbilityDataSO> validAbilities = new List<AbilityDataSO>();

        foreach(AbilityDataSO data in AllAbilities)
        {
            if(data.abilityName != "Strike" ||
                data.abilityName != "Defend" ||
                data.abilityName != "Move" ||
                data.abilityName != "Shoot" ||
                data.abilityName != "Twin Strike")
            {
                validAbilities.Add(data);
            }
        }

        return validAbilities[Random.Range(0, validAbilities.Count)];
    }

}
