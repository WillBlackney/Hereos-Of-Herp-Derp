using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCardInfoTab : MonoBehaviour
{
    [Header("View Component References")]
    public GameObject masterParent;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image image;

    [Header("Layout Component References")]
    public RectTransform nameRect;
    public RectTransform descriptionRect;
    public RectTransform frameRect;
    public RectTransform parentRect;

    public void EnableView()
    {
        masterParent.SetActive(true);
    }
    public void DisableView()
    {
        masterParent.SetActive(false);
    }
    public void RefreshLayoutGroups()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(descriptionRect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(nameRect);        
        LayoutRebuilder.ForceRebuildLayoutImmediate(frameRect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
    }

}
