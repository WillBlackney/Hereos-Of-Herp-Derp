using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class AfflictionOnPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Properties + Component References
    #region
    [Header("Component References")]
    public Image myImageComponent;
    public GameObject infoPanelParent;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    [Header("Properties")]
    public StateDataSO myStateData;
    public State myState;
    #endregion

    // Set up + Initialization
    #region
    public void InitializeSetup(State stateReference)
    {
        myState = stateReference;
        myStateData = stateReference.myStateData;

        nameText.text = stateReference.Name;
        descriptionText.text = stateReference.description;
        myImageComponent.sprite = stateReference.sprite;

        StateManager.Instance.afflicationPanelObjects.Add(this);

    }
    #endregion

    // View Logic
    #region
    public void SetInfoPanelViewState(bool onOrOff)
    {
        infoPanelParent.SetActive(onOrOff);
    }
    #endregion

    // Mouse + Click Event Logic
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("AfflictionOnPanel.OnPointerEnter() called...");
        SetInfoPanelViewState(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("AfflictionOnPanel.OnPointerExit() called...");
        SetInfoPanelViewState(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("AfflictionOnPanel.OnPointerClick() called...");

        if (CampSiteManager.Instance.awaitingBatheChoice)
        {
            CampSiteManager.Instance.PerformBathe(this);
        }
    }
    #endregion

}
