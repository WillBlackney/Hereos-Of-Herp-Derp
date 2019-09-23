using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StatusIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string statusName;
    public string statusDescription;
    public int statusStacks;

    public TextMeshProUGUI statusStacksText;
    public Sprite statusImage;

    public void SetUpProperties(StatusIcon iconData)
    {
        statusImage = iconData.statusImage;
        GetComponent<Image>().sprite = statusImage;

        statusName = iconData.statusName;        
        statusDescription = iconData.statusDescription;
        statusStacks = iconData.statusStacks;
        statusStacksText.text = statusStacks.ToString();
    }

    public void ModifyStatusIconStacks(int stacksGainedOrLost)
    {
        statusStacks += stacksGainedOrLost;
        statusStacksText.text = statusStacks.ToString();
        if(statusStacks == 0)
        {
            statusStacksText.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Status Icon mouse over detected...");
        SpellInfoBox.Instance.ShowInfoBox(statusName, 0, 0, 0, statusDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SpellInfoBox.Instance.HideInfoBox();
    }
}
