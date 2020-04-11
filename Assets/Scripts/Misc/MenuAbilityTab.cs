using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuAbilityTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Sheet Component References")]
    public AbilityInfoSheet abilityInfoSheet;
    public PassiveInfoSheet passiveInfoSheet;

    [Header("Image Component References")]
    public Image abilityImage;
    public Image passiveImage;

    [Header("Properties")]
    public bool isAbility;
    public bool isPassive;

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
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        AbilityInfoSheetController.Instance.DisableSheetView(abilityInfoSheet);
        PassiveInfoSheetController.Instance.DisableSheetView(passiveInfoSheet);
    }

    public void SetUpAbilityTabAsAbility(string abilityName)
    {
        isAbility = true;
        isPassive = false;
        AbilityDataSO data = AbilityLibrary.Instance.GetAbilityByName(abilityName);
        abilityImage.sprite = data.sprite;
        AbilityInfoSheetController.Instance.BuildSheetFromData(abilityInfoSheet, data, AbilityInfoSheet.PivotDirection.Upwards);

    }
    public void SetUpAbilityTabAsPassive(string passiveName, int stacks)
    {
        isPassive = true;
        isAbility = false;
        StatusIconDataSO data = StatusIconLibrary.Instance.GetStatusIconByName(passiveName);

        passiveImage.sprite = data.statusSprite;
        PassiveInfoSheetController.Instance.BuildSheetFromData(passiveInfoSheet, data, stacks, PassiveInfoSheet.PivotDirection.Upwards);

        // Refresh layout
        //RefreshLayoutGroups();

    }
    
}
