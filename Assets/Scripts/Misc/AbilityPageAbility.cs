using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityPageAbility : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Properties + General Components")]
    public Image abilityImage;
    public bool onAbilityBar;
    public AbilityDataSO myData;
    public AbilitySlot myCurrentSlot;
    public CharacterData myCharacter;
    public AbilityInfoSheet abilityInfoSheet;

    // Setup
    #region
    public void InitializeSetup(AbilityDataSO data)
    {
        abilityImage.sprite = data.sprite;
        myData = data;
        BuildInfoPanelFromData(myData);
    }
    public void BuildInfoPanelFromData(AbilityDataSO data)
    {
        // build text and images assets
        AbilityInfoSheetController.Instance.BuildSheetFromData(abilityInfoSheet, data, AbilityInfoSheet.PivotDirection.Upwards);
    }
    #endregion

    // View Logic
    #region
    public void EnableInfoPanelView()
    {
        AbilityInfoSheetController.Instance.EnableSheetView(abilityInfoSheet, true, true);
    }
    public void DisableInfoPanelView()
    {
        AbilityInfoSheetController.Instance.DisableSheetView(abilityInfoSheet);
    }
    #endregion

    // Mouse + Click Events
    #region
    public void OnPointerClick(PointerEventData eventData)
    {
        if (onAbilityBar)
        {
            myCharacter.RemoveAbilityFromActiveAbilityBar(myData);
        }

        else
        {
            // Make sure there is actually slots availble first
            AbilitySlot abilityBarSlot = myCharacter.GetNextAvailableAbilityBarSlot();

            // create ability on active bar
            if (abilityBarSlot != null && !myCharacter.IsAbilityAlreadyOnActiveBar(myData))
            {
                myCharacter.activeAbilities.Add(myCharacter.CreateNewAbilityPageAbilityTab(myData, abilityBarSlot));
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        EnableInfoPanelView();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        DisableInfoPanelView();
    }
    #endregion
}
