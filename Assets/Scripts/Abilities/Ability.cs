using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Ability : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Component References ")]
    public LivingEntity myLivingEntity;
    public TextMeshProUGUI myCooldownText;
    public AbilityDataSO myAbilityData;
    public Sprite abilityImage;
    public CanvasGroup glowHighlightCG;
    public CanvasGroup myInfoPanelCanvasGroup;
    public GameObject myInfoPanel;
    public TextMeshProUGUI abilityNumberText;

    [Header("Text References ")]
    public TextMeshProUGUI cdText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI apCostText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI nameText;

    [Header("Properties")]
    public string abilityName;
    public string abilityDescription;
    public int abilityBaseCooldownTime;
    public int abilityCurrentCooldownTime;
    public int abilityAPCost;
    public int abilityRange;
    public int abilityPrimaryValue;
    public int abilitySecondaryValue;
    public float weaponDamagePercentage;
    public AbilityDataSO.AttackType abilityAttackType;
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

        abilityImage = abilityFromLibrary.sprite;
        Image image = GetComponent<Image>();

        // only for defenders. enemies don't have ability button gameobjects, so GetComponent<Image> will cause a null ref on enemies.
        if (image)
        {            
            GetComponent<Image>().sprite = abilityImage;
        }        

        // Set base properties
        abilityName = abilityFromLibrary.abilityName;
        abilityDescription = abilityFromLibrary.description;
        abilityBaseCooldownTime = abilityFromLibrary.baseCooldownTime;
        abilityAPCost = abilityFromLibrary.energyCost;
        abilityRange = abilityFromLibrary.range;
        abilityPrimaryValue = abilityFromLibrary.primaryValue;
        abilitySecondaryValue = abilityFromLibrary.secondaryValue;
        abilityAttackType = abilityFromLibrary.attackType;
        abilityDamageType = abilityFromLibrary.damageType;
        abilityType = abilityFromLibrary.abilityType;
        weaponDamagePercentage = abilityFromLibrary.weaponDamagePercentage;
        requiresMeleeWeapon = abilityFromLibrary.requiresMeleeWeapon;
        requiresRangedWeapon = abilityFromLibrary.requiresRangedWeapon;
        requiresShield = abilityFromLibrary.requiresShield;

        // Modify base properties if character has certain talents before updating text components
        ModifyAbilityPropertiesFromTalents(this);

        // Set up info panel for defenders
        if (myLivingEntity != null &&
            myLivingEntity.GetComponent<Defender>())
        {
            cdText.text = abilityBaseCooldownTime.ToString();
            rangeText.text = abilityRange.ToString();
            apCostText.text = abilityAPCost.ToString();
            nameText.text = abilityName.ToString();
            descriptionText.text = abilityDescription.ToString();
            //TextLogic.SetAbilityDescriptionText(this);
        }
    }
    public void ModifyAbilityPropertiesFromTalents(Ability ability)
    {
        if (myLivingEntity == null || myLivingEntity.defender == null)
        {
            return;
        }

        // Improved Holy Fire
        if (ability.abilityName == "Holy Fire" && myLivingEntity.defender.myCharacterData.KnowsImprovedHolyFire)
        {
            ability.abilityPrimaryValue++;
            ability.abilityRange++;
        }
        // Improved Telekinesis
        else if (ability.abilityName == "Telekinesis" && myLivingEntity.defender.myCharacterData.KnowsImprovedTelekinesis)
        {
            ability.abilityRange++;
            ability.abilityBaseCooldownTime--;
        }
    }
    #endregion

    // Mouse / Click Events
    #region
    public void OnButtonClick()
    {
        if (myLivingEntity.GetComponent<Defender>())
        {
            myLivingEntity.GetComponent<Defender>().OnAbilityButtonClicked(abilityName);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myLivingEntity.GetComponent<Defender>())
        {
            SetInfoPanelVisibility(true);
            highlightButton = true;
            StartCoroutine(HighLight());
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (myLivingEntity.GetComponent<Defender>())
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

            if (myLivingEntity.gameObject.GetComponent<Enemy>())
            {
                return;
            }

            else if (myLivingEntity.gameObject.GetComponent<Defender>() == true)
            {
                HideCooldownTimer();
            }

        }
        else if (abilityCurrentCooldownTime > 0)
        {
            if (myLivingEntity.gameObject.GetComponent<Enemy>())
            {
                return;
            }

            else if (myLivingEntity.gameObject.GetComponent<Defender>() == true)
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
    #endregion

    // Visbility / View Logic
    #region
    public void ShowCooldownTimer()
    {
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (myLivingEntity.GetComponent<Enemy>())
        {
            return;
        }

        else if (defender)
        {
            myCooldownText.gameObject.SetActive(true);
        }
        
    }
    public void HideCooldownTimer()
    {
        Defender defender = myLivingEntity.gameObject.GetComponent<Defender>();

        if (myLivingEntity.GetComponent<Enemy>())
        {
            return;
        }

        else if (defender)
        {
            myCooldownText.gameObject.SetActive(false);
        }
    }     
    public void SetInfoPanelVisibility(bool onOrOff)
    {
        myInfoPanel.SetActive(onOrOff);
        if (onOrOff == true)
        {
            FadeInInfoPanel();
        }
        else
        {
            fadingIn = false;
            myInfoPanelCanvasGroup.alpha = 0;
        }

    }
    public void FadeInInfoPanel()
    {               
        StartCoroutine(FadeInInfoPanelCoroutine());
    }
    public IEnumerator FadeInInfoPanelCoroutine()
    {
        fadingIn = true;
        while (myInfoPanelCanvasGroup.alpha < 1 && fadingIn)
        {
            myInfoPanelCanvasGroup.alpha += 0.2f;
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator HighLight()
    {
        while (highlightButton)
        {
            glowHighlightCG.alpha += 0.2f;
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

}
