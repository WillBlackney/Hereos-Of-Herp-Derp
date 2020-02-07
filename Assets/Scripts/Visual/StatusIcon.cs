using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StatusIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Properties")]
    public StatusIconDataSO myIconData;
    public string statusName;
    public string statusDescription;
    public int statusStacks;
    public Sprite statusSprite;

    [Header("Text Components")]
    public TextMeshProUGUI statusStacksText;
    public TextMeshProUGUI statusNameText;
    public TextMeshProUGUI statusDescriptionText;

    [Header("Image Components")]
    public Image iconImage;
    public Image infoPanelIconImage;
    public GameObject infoPanelParent;
    public CanvasGroup panelCG;

    // Initialization + Setup
    #region
    public void InitializeSetup(StatusIconDataSO iconData)
    {
        myIconData = iconData;
        statusSprite = iconData.statusSprite;
        iconImage.sprite = statusSprite;

        statusName = iconData.statusName;        
        statusDescription = iconData.statusDescription;
        if (iconData.showStackCount)
        {
            statusStacksText.gameObject.SetActive(true);

        }
        statusStacksText.text = statusStacks.ToString();
        
        // info panel set up
        statusNameText.text = TextLogic.ReturnColoredText(statusName, TextLogic.yellow);
        statusDescriptionText.text = statusDescription;
        infoPanelIconImage.sprite = statusSprite;

        TextLogic.SetStatusIconDescriptionText(statusName, statusDescriptionText, statusStacks);
    }
    #endregion

    // Logic
    #region
    public void ModifyStatusIconStacks(int stacksGainedOrLost)
    {
        statusStacks += stacksGainedOrLost;
        statusStacksText.text = statusStacks.ToString();
        if(statusStacks == 0)
        {
            statusStacksText.gameObject.SetActive(false);
        }

        // Make sure status info panel text is updated when stacks data changes
        //TextLogic.SetStatusIconDescriptionText(this);
    }
    #endregion

    // Mouse + Click Events
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Status Icon mouse over detected...");
        TextLogic.SetStatusIconDescriptionText(statusName, statusDescriptionText, statusStacks);
        SetInfoPanelVisibility(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        SetInfoPanelVisibility(false);
    }
    #endregion

    // Visibility + View Logic
    #region
    public void SetInfoPanelVisibility(bool onOroff)
    {
        if(onOroff == true)
        {
            panelCG.alpha = 1;

        }
        else
        {
            panelCG.alpha = 0.001f;
        }
        
    }
    #endregion
}
