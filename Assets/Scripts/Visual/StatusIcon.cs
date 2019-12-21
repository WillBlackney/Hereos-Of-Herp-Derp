using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StatusIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Properties")]
    public StatusIcon myIconData;
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
    public void InitializeSetup(StatusIcon iconData)
    {
        myIconData = iconData;
        statusSprite = iconData.statusSprite;
        iconImage.sprite = statusSprite;

        statusName = iconData.statusName;        
        statusDescription = iconData.statusDescription;
        statusStacks = iconData.statusStacks;
        statusStacksText.text = statusStacks.ToString();

        // info panel set up
        statusNameText.text = TextLogic.ReturnColoredText(statusName, TextLogic.yellow);
        statusDescriptionText.text = statusDescription;
        infoPanelIconImage.sprite = statusSprite;

        TextLogic.SetStatusIconDescriptionText(this);        
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
        TextLogic.SetStatusIconDescriptionText(this);
    }
    #endregion

    // Mouse + Click Events
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Status Icon mouse over detected...");
        //SpellInfoBox.Instance.ShowInfoBox(statusName, 0, 0, 0, statusDescription);
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
            // quickly turning on and off prevents content size fitter from strecthing incorrectly
            /*
            infoPanelParent.SetActive(false);
            infoPanelParent.SetActive(true);
            panelCG.alpha = 1;
            panelCG.alpha = 0;
            panelCG.alpha = 1;
            infoPanelParent.SetActive(false);
            infoPanelParent.SetActive(true);
            */
            panelCG.alpha = 1;

        }
        else
        {
            //infoPanelParent.SetActive(false);
            panelCG.alpha = 0.001f;
        }
        
    }
    #endregion
}
