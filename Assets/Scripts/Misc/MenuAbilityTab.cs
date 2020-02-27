using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuAbilityTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{    
    [Header("Ability Info Panel References")]
    public GameObject abilityInfoPanelParent;
    public TextMeshProUGUI abilityNameText;
    public TextMeshProUGUI abilityCooldownText;
    public TextMeshProUGUI abilityRangeText;
    public TextMeshProUGUI abilityEnergyCostText;
    public TextMeshProUGUI abilityDescriptionText;
    public Image abilityImage;

    [Header("Passive Info Panel References")]
    public GameObject passiveInfoPanelParent;
    public TextMeshProUGUI passiveNameText;
    public TextMeshProUGUI passiveDescriptionText;
    public Image passiveImage;
    public Image passiveInfoPanelImage;

    [Header("Rect Transform Panel Components")]
    public RectTransform statusPanelParentRect;
    public RectTransform statusPanelDescriptionRect;
    public RectTransform statusPanelNameRect;

    [Header("Type Button References")]
    public GameObject meleeAttackIcon;
    public GameObject rangedAttackIcon;
    public GameObject skillIcon;
    public GameObject powerIcon;

    [Header("Properties")]
    public bool isAbility;
    public bool isPassive;

    void Start()
    {
        statusPanelParentRect = passiveInfoPanelParent.GetComponent<RectTransform>();
        statusPanelDescriptionRect = passiveDescriptionText.GetComponent<RectTransform>();
        statusPanelNameRect = passiveNameText.GetComponent<RectTransform>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isAbility)
        {
            abilityInfoPanelParent.SetActive(true);
        }
        else if (isPassive)
        {
            passiveInfoPanelParent.SetActive(true);
            RefreshLayoutGroups();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        abilityInfoPanelParent.SetActive(false);
        passiveInfoPanelParent.SetActive(false);
    }

    public void SetUpAbilityTabAsAbility(string abilityName)
    {
        isAbility = true;
        isPassive = false;
        AbilityDataSO data = AbilityLibrary.Instance.GetAbilityByName(abilityName);

        // Set up text files
        abilityNameText.text = data.abilityName;
        abilityCooldownText.text = data.baseCooldownTime.ToString();
        abilityRangeText.text = data.range.ToString();
        abilityEnergyCostText.text = data.energyCost.ToString();
        TextLogic.SetAbilityDescriptionText(data, abilityDescriptionText);
        abilityImage.sprite = data.sprite;

        meleeAttackIcon.SetActive(false);
        rangedAttackIcon.SetActive(false);
        skillIcon.SetActive(false);
        powerIcon.SetActive(false);

        if (data.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            meleeAttackIcon.SetActive(true);
        }
        else if (data.abilityType == AbilityDataSO.AbilityType.RangedAttack)
        {
            rangedAttackIcon.SetActive(true);
        }
        else if (data.abilityType == AbilityDataSO.AbilityType.Skill)
        {
            skillIcon.SetActive(true);
        }
        else if (data.abilityType == AbilityDataSO.AbilityType.Power)
        {
            
        }


    }
    public void SetUpAbilityTabAsPassive(string passiveName, int stacks)
    {
        isPassive = true;
        isAbility = false;
        StatusIconDataSO data = StatusIconLibrary.Instance.GetStatusIconByName(passiveName);

        passiveNameText.text = data.statusName;
        TextLogic.SetStatusIconDescriptionText(data.statusName, passiveDescriptionText, stacks);
        passiveImage.sprite = data.statusSprite;
        passiveInfoPanelImage.sprite = data.statusSprite;

        // Refresh layout
        RefreshLayoutGroups();

    }
    public void RefreshLayoutGroups()
    {        
        LayoutRebuilder.ForceRebuildLayoutImmediate(statusPanelDescriptionRect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(statusPanelNameRect);
        LayoutRebuilder.ForceRebuildLayoutImmediate(statusPanelParentRect);
    }
}
