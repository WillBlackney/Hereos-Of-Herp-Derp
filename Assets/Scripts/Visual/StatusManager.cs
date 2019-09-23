using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public GameObject statusIconPrefab;
    List<StatusIcon> myStatusIcons = new List<StatusIcon>();
    public void StartAddStatusProcess(StatusIcon iconData, int stacksGainedOrLost)
    {
        Debug.Log("StartAddStatusProcess() called");

        if (myStatusIcons.Count > 0)
        {
            StatusIcon si = null;
            int stacks = 0;
            bool iconUpdated = false;

            foreach (StatusIcon icon in myStatusIcons)
            {
                if (iconData.statusName == icon.statusName)
                {
                    // Icon already exists
                    UpdateStatusIcon(icon, stacksGainedOrLost);
                    iconUpdated = true;
                    break;
                }

                else
                {
                    si = iconData;
                    stacks = stacksGainedOrLost;                    
                }
            }

            if(iconUpdated == false)
            {
                AddNewStatusIcon(si, stacks);
            }
            
        }
        else
        {
            AddNewStatusIcon(iconData, stacksGainedOrLost);
        }
        
        
    }

    public void AddNewStatusIcon(StatusIcon iconData, int stacksGained)
    {
        Debug.Log("AddNewStatusProcess() called");
        GameObject newIconGO = Instantiate(statusIconPrefab, gameObject.transform);
        StatusIcon newStatus = newIconGO.GetComponent<StatusIcon>();
        newStatus.SetUpProperties(StatusIconLibrary.Instance.GetStatusIconByName(iconData.statusName));
        newStatus.ModifyStatusIconStacks(stacksGained);
        myStatusIcons.Add(newStatus);

    }

    public void RemoveStatusIcon(StatusIcon iconToRemove)
    {
        myStatusIcons.Remove(iconToRemove);
        Destroy(iconToRemove.gameObject);
    }

    public void UpdateStatusIcon(StatusIcon iconToUpdate, int stacksGainedOrLost)
    {
        Debug.Log("UpdateStatusProcess() called");

        iconToUpdate.ModifyStatusIconStacks(stacksGainedOrLost);
        if(iconToUpdate.statusStacks == 0)
        {
            RemoveStatusIcon(iconToUpdate);
        }
       
    }

    public StatusIcon GetStatusIconByName(string iconName)
    {
        StatusIcon iconReturned = null; 

        foreach(StatusIcon icon in myStatusIcons)
        {
            if(iconName == icon.statusName)
            {
                iconReturned = icon;
            }
        }

        return iconReturned;
    }
}
