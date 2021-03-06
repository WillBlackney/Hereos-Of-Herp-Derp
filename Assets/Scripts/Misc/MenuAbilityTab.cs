﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuAbilityTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum Location { None, EditAbilityScreen};
    // Properties + Component References
    #region
    [Header("Sheet Component References")]
    public AbilityInfoSheet abilityInfoSheet;
    public PassiveInfoSheet passiveInfoSheet;

    [Header("Image Component References")]
    public Image abilityImage;
    public Image passiveImage;

    [Header("Misc Component References")]
    public GameObject purchasedOverlayParent;
    public GameObject mouseOverOverlayParent;

    [Header("Properties")]
    public Location location;
    public AbilityDataSO myAbilityData;
    public StatusIconDataSO myPassiveData;
    public bool isAbility;
    public bool isPassive;
    public int passiveStacks;
    #endregion

    // Mouse + Pointer Events
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isAbility)
        {
            AbilityInfoSheetController.Instance.EnableSheetView(abilityInfoSheet, true, true);
        }
        else if (isPassive)
        {
            PassiveInfoSheetController.Instance.EnableSheetView(passiveInfoSheet, true, true);
        }

        if (mouseOverOverlayParent != null)
        {
            EnableMouseOverOutline();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        AbilityInfoSheetController.Instance.DisableSheetView(abilityInfoSheet);
        PassiveInfoSheetController.Instance.DisableSheetView(passiveInfoSheet);

        if (mouseOverOverlayParent != null)
        {
            DisableMouseOverOutline();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("MenuAbilityTab.OnPointerClick() called...");
        if(location == Location.EditAbilityScreen)
        {
            CharacterMakerController.Instance.HandleEditAbilityTabClicked(this);
        }
    }
    #endregion

    // Build Ability Tab
    #region
    public void SetUpAbilityTabAsAbility(string abilityName)
    {
        isAbility = true;
        isPassive = false;
        AbilityDataSO data = AbilityLibrary.Instance.GetAbilityByName(abilityName);
        myAbilityData = data;
        myPassiveData = null;
        abilityImage.sprite = data.sprite;
        AbilityInfoSheetController.Instance.BuildSheetFromData(abilityInfoSheet, data, AbilityInfoSheet.PivotDirection.Upwards);

    }
    public void SetUpAbilityTabAsAbility(AbilityDataSO data)
    {
        isAbility = true;
        isPassive = false;
        myAbilityData = data;
        myPassiveData = null;
        abilityImage.sprite = data.sprite;
        AbilityInfoSheetController.Instance.BuildSheetFromData(abilityInfoSheet, data, AbilityInfoSheet.PivotDirection.Upwards);

    }
    public void SetUpAbilityTabAsPassive(string passiveName, int stacks)
    {
        isPassive = true;
        isAbility = false;
        StatusIconDataSO data = StatusIconLibrary.Instance.GetStatusIconByName(passiveName);
        myAbilityData = null;
        myPassiveData = data;
        passiveStacks = stacks;

        passiveImage.sprite = data.statusSprite;
        PassiveInfoSheetController.Instance.BuildSheetFromData(passiveInfoSheet, data, stacks, PassiveInfoSheet.PivotDirection.Upwards);
    }
    public void SetUpAbilityTabAsPassive(StatusIconDataSO data, int stacks)
    {
        isPassive = true;
        isAbility = false;
        myAbilityData = null;
        myPassiveData = data;
        passiveStacks = stacks;

        passiveImage.sprite = data.statusSprite;
        PassiveInfoSheetController.Instance.BuildSheetFromData(passiveInfoSheet, data, stacks, PassiveInfoSheet.PivotDirection.Upwards);
    }

    // View Logic
    #region
    public void EnableGlowOutline()
    {
        purchasedOverlayParent.SetActive(true);
    }
    public void DisableGlowOutline()
    {
        purchasedOverlayParent.SetActive(false);
    }
    public void EnableMouseOverOutline()
    {
        mouseOverOverlayParent.SetActive(true);
    }
    public void DisableMouseOverOutline()
    {
        mouseOverOverlayParent.SetActive(false);
    }
    #endregion


    #endregion

}
