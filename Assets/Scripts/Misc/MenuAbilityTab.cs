using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuAbilityTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AbilityInfoSheet abilityInfoSheet;
    public Image abilityImage;

    [Header("Passive Info Panel References")]
    public GameObject passiveInfoPanelParent;
    public TextMeshProUGUI passiveNameText;
    public TextMeshProUGUI passiveDescriptionText;
    public Image passiveImage;
    public Image passiveInfoPanelImage;

    [Header("Rect Transform Panel Components")]
    public RectTransform statusPanelParentRect;
    public RectTransform statusPanelDescriptionRect;
    public RectTransform statusPanelNameRect;


    [Header("Properties")]
    public bool isAbility;
    public bool isPassive;

    void Start()
    {
        statusPanelParentRect = passiveInfoPanelParent.GetComponent<RectTransform>();
        statusPanelDescriptionRect = passiveDescriptionText.GetComponent<RectTransform>();
        statusPanelNameRect = passiveNameText.GetComponent<RectTransform>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isAbility)
        {
            AbilityInfoSheetController.Instance.EnableSheetView(abilityInfoSheet, true, true);
        }
        else if (isPassive)
        {
            passiveInfoPanelParent.SetActive(true);
            RefreshLayoutGroups();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        AbilityInfoSheetController.Instance.DisableSheetView(abilityInfoSheet);
        passiveInfoPanelParent.SetActive(false);
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

        passiveNameText.text = data.statusName;
        TextLogic.SetStatusIconDescriptionText(data.statusName, passiveDescriptionText, stacks);
        passiveImage.sprite = data.statusSprite;
        passiveInfoPanelImage.sprite = data.statusSprite;

        // Refresh layout
        RefreshLayoutGroups();

    }
    public void RefreshLayoutGroups()
    {        
        LayoutRebuilder.ForceRebuildLayoutImmediate(statusPanelDescriptionRect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(statusPanelNameRect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(statusPanelParentRect);
    }
}
