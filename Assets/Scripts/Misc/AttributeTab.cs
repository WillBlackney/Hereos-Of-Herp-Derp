using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class AttributeTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Properties + Component References
    #region
    [Header("Component References")]
    public Image attributeImage;
    public PassiveInfoSheet passiveInfoSheet;
    public TextMeshProUGUI attributeNameText;
    public TextMeshProUGUI attributeStacksText;

    [Header("Properties")]
    public string attributeName;
    public int attributeStacks;
    #endregion

    // Initialization + Setup
    #region
    public void InitializeSetup(string passiveName, int stacks)
    {
        // Get data
        StatusIconDataSO data = StatusIconLibrary.Instance.GetStatusIconByName(passiveName);        

        // Build main views and properties from status icon data
        attributeImage.sprite = data.statusSprite;
        attributeName = data.statusName;
        attributeNameText.text = data.statusName;

        // Build info panel pop up window
        PassiveInfoSheetController.Instance.BuildSheetFromData(passiveInfoSheet, data, stacks, PassiveInfoSheet.PivotDirection.Upwards);
        TextLogic.SetStatusIconDescriptionText(data.statusName, passiveInfoSheet.descriptionText, stacks);

        // Set stack count + components
        ModifyStacks(stacks);
        if(data.showStackCount == false)
        {
            attributeStacksText.gameObject.SetActive(false);
        }

    }
    #endregion

    // Modify Properties
    #region
    public void ModifyStacks(int stacksGainedOrLost)
    {
        attributeStacks += stacksGainedOrLost;
        attributeStacksText.text = attributeStacks.ToString();
        TextLogic.SetStatusIconDescriptionText(attributeName, passiveInfoSheet.descriptionText, stacksGainedOrLost);
    }
    #endregion

    // View Logic
    #region
    public void EnableInfoPanelView()
    {
        PassiveInfoSheetController.Instance.EnableSheetView(passiveInfoSheet, true, true);
    }
    public void DisableInfoPanelView()
    {
        PassiveInfoSheetController.Instance.DisableSheetView(passiveInfoSheet);
    }
    #endregion

    // Mouse + Input Events
    #region
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
