using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StatusIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Properties + Component References
    #region
    [Header("Properties")]
    public StatusIconDataSO myIconData;
    public string statusName;
    public int statusStacks;
    public Sprite statusSprite;    

    [Header("Component References")]
    public TextMeshProUGUI statusStacksText;
    public Image iconImage;
    #endregion

    // Initialization + Setup
    #region
    public void InitializeSetup(StatusIconDataSO iconData)
    {
        myIconData = iconData;
        statusSprite = iconData.statusSprite;
        iconImage.sprite = statusSprite;

        statusName = iconData.statusName;        
        if (iconData.showStackCount)
        {
            statusStacksText.gameObject.SetActive(true);
        }

        statusStacksText.text = statusStacks.ToString();
        
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
    }
    #endregion

    // Mouse + Click Events
    #region
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Status Icon mouse over detected...");
        InfoPanelHover.Instance.HandleIconMousedEnter(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanelHover.Instance.HandleIconMouseExit(this);
    }
    #endregion

    // Visibility + View Logic
    #region
   
    #endregion
}
