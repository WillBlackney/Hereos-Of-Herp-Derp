using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class State : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Properties + Component References
    #region
    [Header("Component References")]
    public Image myImageComponent;
    public GameObject infoPanelParent;
    public TextMeshProUGUI durationText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    [Header("Rarity Parent References")]
    public GameObject commonParent;
    public GameObject rareParent;
    public GameObject bossParent;
    public GameObject eventParent;
    public GameObject afflictionParent;

    [Header("Properties")]
    public StateDataSO myStateData;
    public Sprite sprite;
    public string Name;
    public string description;
    public StateDataSO.ExpirationCondition expirationCondition;
    public bool affliction;
    public int duration;
    public int currentDuration;
    #endregion

    // Initialization + Setup
    #region
    public void InitializeSetup(StateDataSO stateData)
    {
        myStateData = stateData;
        sprite = stateData.stateSprite;
        myImageComponent.sprite = sprite;
        Name = stateData.stateName;
        description = stateData.stateDescription;
        expirationCondition = stateData.expirationCondition;
        affliction = stateData.affliction;
        duration = stateData.duration;
        currentDuration = duration;

        // Set up text components
        
        // Only enable duration text if the state has a timer 
        if(currentDuration > 0 && stateData.expirationCondition == StateDataSO.ExpirationCondition.Timer)
        {
            durationText.text = currentDuration.ToString();
            durationText.gameObject.SetActive(true);
        }
        descriptionText.text = description;
        nameText.text = Name;

        // set rarity gem / afflication gem
        if (stateData.affliction)
        {
            afflictionParent.SetActive(true);
        }
        else if (stateData.rarity == StateDataSO.Rarity.Common)
        {
            commonParent.SetActive(true);
        }
        else if (stateData.rarity == StateDataSO.Rarity.Rare)
        {
            rareParent.SetActive(true);
        }
        else if (stateData.rarity == StateDataSO.Rarity.Boss)
        {
            bossParent.SetActive(true);
        }
        else if (stateData.eventReward)
        {
            eventParent.SetActive(true);
        }
    }
    #endregion

    // View Logic
    #region
    public void SetInfoPanelViewState(bool onOrOff)
    {
        infoPanelParent.SetActive(onOrOff);
    }  
    public Action PlayExpireVfxAndDestroy(bool createScreenCardOverlayEffect = false)
    {
        Debug.Log("PlayExpireVfxAndDestroy().called");
        Action action = new Action();
        StartCoroutine(PlayExpireVfxAndDestroyCoroutine(action, createScreenCardOverlayEffect));
        return action;
    }
    private IEnumerator PlayExpireVfxAndDestroyCoroutine(Action action, bool createScreenCardOverlayEffect)
    {
        Debug.Log("State '" + Name + "' expiration condition met, destroying... ");
        if (createScreenCardOverlayEffect)
        {
            CardRewardScreenManager.Instance.CreateAfflictionCardRemovedEffect(myStateData);
        }       

        yield return new WaitForSeconds(0.5f);
        StateManager.Instance.activeStates.Remove(this);
        action.actionResolved = true;
        Destroy(gameObject);
    }
    #endregion

    // Mouse + Click Events
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetInfoPanelViewState(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        SetInfoPanelViewState(false);
    }
    #endregion

    // Misc Logic
    #region
    public void ModifyCountdown(int timeGainedOrLost)
    {
        Debug.Log("State.ModifyCountdown() called for " + Name);

        currentDuration += timeGainedOrLost;
        durationText.text = currentDuration.ToString();
        if (currentDuration <= 0)
        {
            PlayExpireVfxAndDestroy();
        }
    }
    #endregion
}
