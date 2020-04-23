using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EnemyPanelAbilityTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Component References")]
    public Image abilityImage;
    public AbilityInfoSheet abilityInfoSheet;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AbilityInfoSheetController.Instance.EnableSheetView(abilityInfoSheet, true, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AbilityInfoSheetController.Instance.DisableSheetView(abilityInfoSheet);
    }
}
