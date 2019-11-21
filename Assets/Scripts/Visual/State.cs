using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class State : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Component References")]
    public Image myImageComponent;
    public GameObject infoPanelParent;
    public TextMeshProUGUI durationText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    [Header("Properties")]
    public StateDataSO myStateData;
    public Sprite sprite;
    public string Name;
    public string description;
    public StateDataSO.ExpirationCondition expirationCondition;
    public bool affliction;
    public int duration;
    public int currentDuration;
    public void InitializeSetup(StateDataSO stateData)
    {
        myStateData = stateData;
        sprite = stateData.sprite;
        myImageComponent.sprite = sprite;
        Name = stateData.Name;
        description = stateData.description;
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
    }
    public void SetInfoPanelViewState(bool onOrOff)
    {
        infoPanelParent.SetActive(onOrOff);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetInfoPanelViewState(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        SetInfoPanelViewState(false);
    }
    public void ModifyCountdown(int timeGainedOrLost)
    {
        currentDuration += timeGainedOrLost;
        durationText.text = currentDuration.ToString();
    }
    public Action PlayExpireVfxAndDestroy()
    {
        Debug.Log("PlayExpireVfxAndDestroy().called");
        Action action = new Action();
        StartCoroutine(PlayExpireVfxAndDestroyCoroutine(action));
        return action;
    }
    public IEnumerator PlayExpireVfxAndDestroyCoroutine(Action action)
    {
        Debug.Log("State '" + Name + "' expiration condition met, destroying... "); 
        // TO DO: play some cool expiration VFX, like it burning up or fading out
        yield return new WaitForSeconds(0.5f);
        StateManager.Instance.activeStates.Remove(this);
        action.actionResolved = true;
        Destroy(gameObject);
    }

}
