using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class AttributeTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Component References")]
    public Image attributeImage;
    public TextMeshProUGUI attributeNameText;
    public TextMeshProUGUI attributeStacksText;

    [Header("Rect Transform Component References")]
    public RectTransform panelBgRect;
    public RectTransform frameRect;
    public RectTransform statusNameRect;
    public RectTransform descriptionRect;

    [Header("Info Panel Component References")]
    public GameObject infoPanelParent;
    public Image attributeInfoPanelImage;
    public TextMeshProUGUI attributeInfoPanelNameText;
    public TextMeshProUGUI attributeInfoPanelDescriptionText;

    [Header("Properties")]
    public string attributeName;
    public int attributeStacks;

    public void InitializeSetup(string passiveName, int stacks)
    {
        StatusIconDataSO data = StatusIconLibrary.Instance.GetStatusIconByName(passiveName);

        // build properties and components from status icon data
        attributeImage.sprite = data.statusSprite;
        attributeName = data.statusName;
        attributeNameText.text = data.statusName;

        // build info panel
        attributeInfoPanelImage.sprite = data.statusSprite;
        attributeInfoPanelNameText.text = data.statusName;

        TextLogic.SetStatusIconDescriptionText(data.statusName, attributeInfoPanelDescriptionText, stacks);

        ModifyStacks(stacks);
        if(data.showStackCount == false)
        {
            attributeStacksText.gameObject.SetActive(false);
        }

    }
    public void ModifyStacks(int stacksGainedOrLost)
    {
        attributeStacks += stacksGainedOrLost;
        attributeStacksText.text = attributeStacks.ToString();
        TextLogic.SetStatusIconDescriptionText(attributeName, attributeInfoPanelDescriptionText, stacksGainedOrLost);
    }

    public void EnableInfoPanelView()
    {
        infoPanelParent.SetActive(true);
        RebuildAllLayourGroups();
    }
    public void DisableInfoPanelView()
    {
        infoPanelParent.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EnableInfoPanelView();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DisableInfoPanelView();
    }

    public void RebuildAllLayourGroups()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(panelBgRect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(frameRect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(statusNameRect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(descriptionRect);
    }
}
