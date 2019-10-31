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
    public AbilityDataSO.AttackType abilityAttackType;
    public AbilityDataSO.DamageType abilityDamageType;

    [Header("VFX + View Properties")]
    public bool highlightButton;
    public bool fadingIn;

    public void SetupBaseProperties(AbilityDataSO abilityFromLibrary)
    {
        myAbilityData = abilityFromLibrary;

        abilityImage = abilityFromLibrary.abilityImage;
        Image image = GetComponent<Image>();

        // only for defenders. enemies don't have ability button gameobjects, so GetComponent<Image> will cause a null ref on enemies.
        if (image)
        {            
            GetComponent<Image>().sprite = abilityImage;
        }        

        abilityName = abilityFromLibrary.abilityName;
        abilityDescription = abilityFromLibrary.abilityDescription;
        abilityBaseCooldownTime = abilityFromLibrary.abilityBaseCooldownTime;
        abilityAPCost = abilityFromLibrary.abilityAPCost;
        abilityRange = abilityFromLibrary.abilityRange;
        abilityPrimaryValue = abilityFromLibrary.abilityPrimaryValue;
        abilitySecondaryValue = abilityFromLibrary.abilitySecondaryValue;

        abilityAttackType = abilityFromLibrary.abilityAttackType;
        abilityDamageType = abilityFromLibrary.abilityDamageType;

        // Set up info panel for defenders
        if (myLivingEntity != null &&
            myLivingEntity.GetComponent<Defender>())
        {
            cdText.text = abilityFromLibrary.abilityBaseCooldownTime.ToString();
            rangeText.text = abilityFromLibrary.abilityRange.ToString();
            apCostText.text = abilityFromLibrary.abilityAPCost.ToString();
            nameText.text = abilityFromLibrary.abilityName.ToString();
            descriptionText.text = abilityFromLibrary.abilityDescription.ToString();
        }
    }

    public void OnButtonClick()
    {
        if (myLivingEntity.GetComponent<Defender>())
        {
            myLivingEntity.GetComponent<Defender>().OnAbilityButtonClicked(abilityName);
        }
    }    

    

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

    public void ModifyCurrentCooldown(int durationGainedOrReduced)
    {
        abilityCurrentCooldownTime += durationGainedOrReduced;
        
        if(abilityCurrentCooldownTime <= 0)
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
        else if(abilityCurrentCooldownTime > 0)
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

    public void OnPointerClick(PointerEventData eventData)
    {
        OnButtonClick();
    }

    public IEnumerator HighLight()
    {
        while (highlightButton)
        {
            glowHighlightCG.alpha += 0.2f;
            yield return new WaitForEndOfFrame();
        }
    }
}
