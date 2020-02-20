using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttributeTab : MonoBehaviour
{
    [Header("Component References")]
    public Image attributeImage;
    public TextMeshProUGUI attributeNameText;
    public TextMeshProUGUI attributeStacksText;

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

        ModifyStacks(stacks);

    }

    public void ModifyStacks(int stacksGainedOrLost)
    {
        attributeStacks += stacksGainedOrLost;
        attributeStacksText.text = attributeStacks.ToString();
    }
}
