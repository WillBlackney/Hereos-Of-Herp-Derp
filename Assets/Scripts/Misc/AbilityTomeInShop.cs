using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityTomeInShop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Component References")]
    public GameObject visualParent;
    public TextMeshProUGUI goldCostText;
    public Image bgColorImage;

    [Header("Info Panel Component References")]
    public GameObject infoPanelVisualParent;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI energyCostText;

    [Header("Properties")]
    public int goldCost;
    public AbilityDataSO myData;


    // Initialization + Setup
    #region
    public void BuildFromData(AbilityDataSO data)
    {
        myData = data;

        // Build text and images assets
        nameText.text = data.abilityName;
        TextLogic.SetAbilityDescriptionText(data, descriptionText);
        cooldownText.text = data.baseCooldownTime.ToString();
        rangeText.text = data.range.ToString();
        energyCostText.text = data.energyCost.ToString();

        // Randomize and set gold cost
        if(data.tier == 1)
        {
            goldCost = Random.Range(4, 7);
        }
        else if (data.tier == 2)
        {
            goldCost = Random.Range(7,10);
        }
        else if (data.tier == 3)
        {
            goldCost = Random.Range(10, 13);
        }
        goldCostText.text = goldCost.ToString();

        // Enable Views
        EnableSlotView();
        SetBackgroundColor();

    }
    public void SetBackgroundColor()
    {
        if (myData.abilitySchool == AbilityDataSO.AbilitySchool.None)
        {
            bgColorImage.color = InventoryController.Instance.neutralColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Brawler)
        {
            bgColorImage.color = InventoryController.Instance.brawlerColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Duelist)
        {
            bgColorImage.color = InventoryController.Instance.duelistColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Assassination)
        {
            bgColorImage.color = InventoryController.Instance.assassinationColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Guardian)
        {
            bgColorImage.color = InventoryController.Instance.guardianColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Pyromania)
        {
            bgColorImage.color = InventoryController.Instance.pyromaniaColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Cyromancy)
        {
            bgColorImage.color = InventoryController.Instance.cyromancyColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Ranger)
        {
            bgColorImage.color = InventoryController.Instance.rangerColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Manipulation)
        {
            bgColorImage.color = InventoryController.Instance.manipulationColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Divinity)
        {
            bgColorImage.color = InventoryController.Instance.divinityColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Shadowcraft)
        {
            bgColorImage.color = InventoryController.Instance.shadowcraftColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Corruption)
        {
            bgColorImage.color = InventoryController.Instance.corruptionColor;
        }
        else if (myData.abilitySchool == AbilityDataSO.AbilitySchool.Naturalism)
        {
            bgColorImage.color = InventoryController.Instance.naturalismColor;
        }
    }
    #endregion

    // View Logic
    #region
    public void EnableInfoPanelView()
    {
        infoPanelVisualParent.SetActive(true);
    }
    public void DisableInfoPanelView()
    {
        infoPanelVisualParent.SetActive(false);
    }
    public void EnableSlotView()
    {
        visualParent.SetActive(true);
    }
    public void DisableSlotView()
    {
        visualParent.SetActive(false);
    }
    #endregion

    // Mouse + Click Events
    #region
    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryController.Instance.BuyAbilityTomeFromShop(this);
    }
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
