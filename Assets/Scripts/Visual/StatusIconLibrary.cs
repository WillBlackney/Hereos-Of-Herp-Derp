using System.Collections.Generic;
using UnityEngine;

public class StatusIconLibrary : MonoBehaviour
{
    [Header("Properties")]
    public List<StatusIconDataSO> allIcons;
    public List<StatusPairingDataSO> allStatusPairingData;    

    // Initialization + Singleton Pattern
    #region
    public static StatusIconLibrary Instance;
   
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

    // Logic
    #region
    public StatusIconDataSO GetStatusIconByName(string name)
    {
        StatusIconDataSO iconReturned = null;

        foreach (StatusIconDataSO icon in allIcons)
        {
            if (icon.statusName == name)
            {
                iconReturned = icon;
                break;
            }
        }

        if(iconReturned == null)
        {
            Debug.Log("StatusIconLibrary.GetStatusIconByName() could not find a status with the name " +
                name + ", returning null...");
        }

        return iconReturned;
    }
    public List <StatusPairingDataSO> GetAllStatusPairingsFromTalentSchool(AbilityDataSO.AbilitySchool school, int maxTierLimit)
    {
        List<StatusPairingDataSO> dataReturned = new List<StatusPairingDataSO>();

        foreach (StatusPairingDataSO ability in allStatusPairingData)
        {
            if (ability.abilitySchool == school && ability.tier <= maxTierLimit)
            {
                dataReturned.Add(ability);
            }
        }

        Debug.Log("StatusIconLibrary.GetAllStatusPairingsFromTalentSchool() found " + dataReturned.Count.ToString() + " " +
            school.ToString() + " abilities at tier " + maxTierLimit.ToString() + " or lower");

        return dataReturned;
    }
    #endregion



}
