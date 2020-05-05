using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Ability : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Component References ")]    
    public TextMeshProUGUI myCooldownText;
    public AbilityDataSO myAbilityData;
    public Image abilityImage;
    public CanvasGroup glowHighlightCG;
    public AbilityInfoSheet abilityInfoSheet;
    public TextMeshProUGUI abilityNumberText;

    [Header("Properties")]
    public LivingEntity myLivingEntity;
    public string abilityName;
    public string abilityDescription;
    public int abilityBaseCooldownTime;
    public int abilityCurrentCooldownTime;
    public int abilityEnergyCost;
    public int abilityRange;
    public int abilityPrimaryValue;
    public int abilitySecondaryValue;
    public float weaponDamagePercentage;
    public AbilityDataSO.DamageType abilityDamageType;
    public AbilityDataSO.AbilityType abilityType;

    [Header("Ability Requirments")]
    public bool requiresMeleeWeapon;
    public bool requiresRangedWeapon;
    public bool requiresShield;

    [Header("VFX + View Properties")]
    public bool highlightButton;
    public bool fadingIn;

    // Initialization + Setup
    #region
    public void SetupBaseProperties(AbilityDataSO abilityFromLibrary)
    {
        myAbilityData = abilityFromLibrary;

        // only for defenders. enemies don't have ability button gameobjects
        if (abilityImage)
        {
            abilityImage.sprite = abilityFromLibrary.sprite;
        }        

        // Set base properties
        abilityName = abilityFromLibrary.abilityName;
        abilityDescription = abilityFromLibrary.description;
        abilityBaseCooldownTime = abilityFromLibrary.baseCooldownTime;
        abilityEnergyCost = abilityFromLibrary.energyCost;
        abilityRange = abilityFromLibrary.range;
        abilityPrimaryValue = abilityFromLibrary.primaryValue;
        abilitySecondaryValue = abilityFromLibrary.secondaryValue;
        abilityDamageType = abilityFromLibrary.damageType;
        abilityType = abilityFromLibrary.abilityType;
        weaponDamagePercentage = abilityFromLibrary.weaponDamagePercentage;
        requiresMeleeWeapon = abilityFromLibrary.requiresMeleeWeapon;
        requiresRangedWeapon = abilityFromLibrary.requiresRangedWeapon;
        requiresShield = abilityFromLibrary.requiresShield;

        // Set up info panel for defenders
        if (myLivingEntity != null &&
            myLivingEntity.defender)
        {
            AbilityInfoSheetController.Instance.BuildSheetFromData(abilityInfoSheet, abilityFromLibrary, AbilityInfoSheet.PivotDirection.Upwards);
        }
    }
   
    #endregion

    // Mouse / Click Events
    #region
    public void OnButtonClick()
    {
        if (myLivingEntity.defender)
        {
            myLivingEntity.defender.OnAbilityButtonClicked(abilityName);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myLivingEntity.defender)
        {
            CalculateAndSetInfoPanelFields();
            SetInfoPanelVisibility(true);
            highlightButton = true;
            StartCoroutine(HighLight());
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (myLivingEntity.defender)
        {
            highlightButton = false;
            SetInfoPanelVisibility(false);
            glowHighlightCG.alpha = 0f;
        }

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnButtonClick();
    }
    #endregion

    // Logic
    #region
    public void ModifyCurrentCooldown(int durationGainedOrReduced)
    {
        abilityCurrentCooldownTime += durationGainedOrReduced;

        if (abilityCurrentCooldownTime <= 0)
        {
            abilityCurrentCooldownTime = 0;

            if (myLivingEntity.enemy)
            {
                return;
            }

            else if (myLivingEntity.defender)
            {
                HideCooldownTimer();
            }

        }
        else if (abilityCurrentCooldownTime > 0)
        {
            if (myLivingEntity.enemy)
            {
                return;
            }

            else if (myLivingEntity.defender)
            {
                ShowCooldownTimer();
            }

        }

        myCooldownText.text = abilityCurrentCooldownTime.ToString();
    }
    public void ReduceCooldownOnTurnStart()
    {
        ModifyCurrentCooldown(-1);
    }
    public void CalculateAndSetInfoPanelFields()
    {
        abilityInfoSheet.cooldownText.text = abilityBaseCooldownTime.ToString();
        abilityInfoSheet.rangeText.text = AbilityLogic.Instance.CalculateAbilityRange(this, myLivingEntity).ToString();
        abilityInfoSheet.energyCostText.text = AbilityLogic.Instance.CalculateAbilityEnergyCost(this, myLivingEntity).ToString();        
        TextLogic.SetAbilityDescriptionText(this);
    }
    #endregion

    // Visbility / View Logic
    #region
    public void ShowCooldownTimer()
    {
        if (myLivingEntity.enemy)
        {
            return;
        }

        else if (myLivingEntity.defender)
        {
            myCooldownText.gameObject.SetActive(true);
        }
        
    }
    public void HideCooldownTimer()
    {
        if (myLivingEntity.enemy)
        {
            return;
        }

        else if (myLivingEntity.defender)
        {
            myCooldownText.gameObject.SetActive(false);
        }
    }     
    public void SetInfoPanelVisibility(bool onOrOff)
    {
        if (onOrOff)
        {
            AbilityInfoSheetController.Instance.EnableSheetView(abilityInfoSheet, true, true);
        }
        else if(onOrOff == false)
        {
            AbilityInfoSheetController.Instance.DisableSheetView(abilityInfoSheet);
        }
    }    
    public IEnumerator HighLight()
    {
        while (highlightButton && glowHighlightCG.alpha < 0.2f)
        {
            glowHighlightCG.alpha += 0.2f;
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

}
