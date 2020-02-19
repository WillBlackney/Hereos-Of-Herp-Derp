using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityPageAbility : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Properties + General Components")]
    public Image abilityImage;
    public bool onAbilityBar;
    public AbilityDataSO myData;
    public AbilitySlot myCurrentSlot;
    public CharacterData myCharacter;

    [Header("Info Panel Components")]
    public GameObject infoPanelVisualParent;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI energyCostText;
    public TextMeshProUGUI rangeText;

    // Setup
    #region
    public void InitializeSetup(AbilityDataSO data)
    {
        abilityImage.sprite = data.sprite;
        myData = data;
        BuildInfoPanelFromData(myData);
    }
    public void BuildInfoPanelFromData(AbilityDataSO data)
    {
        // build text and images assets
        nameText.text = data.abilityName;
        TextLogic.SetAbilityDescriptionText(data, descriptionText);
        cooldownText.text = data.baseCooldownTime.ToString();
        rangeText.text = data.range.ToString();
        energyCostText.text = data.energyCost.ToString();
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
    #endregion

    // Mouse + Click Events
    #region
    public void OnPointerClick(PointerEventData eventData)
    {
        if (onAbilityBar)
        {
            myCharacter.RemoveAbilityFromActiveAbilityBar(myData);
        }

        else
        {
            // Make sure there is actually slots availble first
            AbilitySlot abilityBarSlot = myCharacter.GetNextAvailableAbilityBarSlot();

            // create ability on active bar
            if (abilityBarSlot != null)
            {
                myCharacter.activeAbilities.Add(myCharacter.CreateNewAbilityPageAbilityTab(myData, abilityBarSlot));
            }
        }
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
