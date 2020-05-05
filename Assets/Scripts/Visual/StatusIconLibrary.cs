using System.Collections.Generic;
using UnityEngine;

public class StatusIconLibrary : MonoBehaviour
{
    [Header("Properties")]
    public List<StatusIconDataSO> allIcons;

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
    #endregion

    
    
}
