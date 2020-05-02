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
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    #endregion

    // Ability Data Search Methods
    #region
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
    public AbilityDataSO GetRandomValidAbilityTomeAbility()
    {
        Debug.Log("AbilityLibrary.GetRandomValidAbilityTomeAbility() called...");

        List<AbilityDataSO> validAbilities = new List<AbilityDataSO>();
        AbilityDataSO dataReturned = null;

        foreach(AbilityDataSO data in AllAbilities)
        {
            if(data.abilitySchool != AbilityDataSO.AbilitySchool.None)
            {
                validAbilities.Add(data);
            }
        }

        dataReturned = validAbilities[Random.Range(0, validAbilities.Count)];
        Debug.Log("AbilityLibrary.GetRandomValidAbilityTomeAbility() returning " + dataReturned.abilityName);
        return dataReturned;
    }
    public AbilityDataSO GetRandomValidTierOneAbilityTomeAbility()
    {
        Debug.Log("AbilityLibrary.GetRandomValidTierOneAbilityTomeAbility() called...");

        List<AbilityDataSO> validAbilities = new List<AbilityDataSO>();
        AbilityDataSO dataReturned = null;

        foreach (AbilityDataSO data in AllAbilities)
        {
            if (data.abilitySchool != AbilityDataSO.AbilitySchool.None &&
                data.tier == 1)
            {
                validAbilities.Add(data);
            }
        }

        dataReturned = validAbilities[Random.Range(0, validAbilities.Count)];
        Debug.Log("AbilityLibrary.GetRandomValidAbilityTomeAbility() returning " + dataReturned.abilityName);
        return dataReturned;
    }
    #endregion

}
