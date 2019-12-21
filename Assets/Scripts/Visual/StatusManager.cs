using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatusManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Component + Prefab References")]
    public GameObject statusIconPrefab;
    public LivingEntity myLivingEntity;
    public CanvasGroup myCG;
    public List<StatusIcon> myStatusIcons;

    [Header("Properties")]
    public bool fadingIn;
    public bool fadingOut;

    // Add, Update, and Remove Status Icon Logic
    #region
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
        newStatus.InitializeSetup(StatusIconLibrary.Instance.GetStatusIconByName(iconData.statusName));
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
    #endregion

    // Misc Logic + View Logic
    #region
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
    public void SetPanelViewState(bool onOrOff)
    {
        if(onOrOff == true)
        {
            gameObject.SetActive(true);
        }        
        SetPanelViewStateCoroutine(onOrOff);
    }
    public void SetPanelViewStateCoroutine(bool onOrOff)
    {
        if(gameObject.activeSelf == true)
        {
            if (onOrOff == true)
            {
                FadeIn();
            }
            else
            {
                FadeOut();
            }
        }
        
    }
    public Action FadeIn()
    {
        Action action = new Action();
        StartCoroutine(FadeInCoroutine(action));
        return action;
    }
    public IEnumerator FadeInCoroutine(Action action)
    {
        fadingOut = false;
        fadingIn = true;        

        while(fadingIn && myCG.alpha < 1 && myLivingEntity.inDeathProcess == false)
        {
            myCG.alpha += 0.1f;
            if(myCG.alpha == 1)
            {
                fadingIn = false;
            }
            yield return new WaitForEndOfFrame();
        }
        action.actionResolved = true;
    }
    public Action FadeOut()
    {
        Action action = new Action();
        StartCoroutine(FadeOutCoroutine(action));
        return action;
    }
    public IEnumerator FadeOutCoroutine(Action action)
    {
        fadingIn = false;
        fadingOut = true;

        while (fadingOut && myCG.alpha > 0 && myLivingEntity.inDeathProcess == false)
        {
            myCG.alpha -= 0.1f;
            if(myCG.alpha == 0)
            {
                fadingOut = false;
                gameObject.SetActive(false);
            }
            yield return new WaitForEndOfFrame();
        }
        action.actionResolved = true;
    }
    private void Start()
    {
        myLivingEntity = GetComponentInParent<LivingEntity>();
        SetPanelViewState(false);
    }
    #endregion

    // Mouse + Input Events
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Status Manager mouse over detected");
        myLivingEntity.mouseIsOverStatusIconPanel = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        myLivingEntity.mouseIsOverStatusIconPanel = false;
        if (myLivingEntity.mouseIsOverCharacter == false)
        {
            SetPanelViewState(false);
            DisableAllMyIcons();
        }        
    }
    public void DisableAllMyIcons()
    {
        foreach(StatusIcon icon in myStatusIcons)
        {
            icon.SetInfoPanelVisibility(false);
        }
    }

    #endregion

}
