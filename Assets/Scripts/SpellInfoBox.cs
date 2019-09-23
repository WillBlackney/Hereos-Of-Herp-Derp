using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellInfoBox : Singleton<SpellInfoBox>
{
    public TextMeshProUGUI AbilityNameText;
    public TextMeshProUGUI APCostText;
    public TextMeshProUGUI CooldownAmountText;
    public TextMeshProUGUI RangeAmountText;
    public TextMeshProUGUI AbilityDescriptionText;
    public CanvasGroup myCanvasGroup;    

    public void ShowInfoBox(string abilityName, int abilityAPCost, int abilityCooldown, int abilityRange, string abilityDescription)
    {
        gameObject.SetActive(true);
        myCanvasGroup.alpha = 1;

        AbilityNameText.text = abilityName;
        APCostText.text = abilityAPCost.ToString();
        CooldownAmountText.text = abilityCooldown.ToString();
        RangeAmountText.text = abilityRange.ToString();
        AbilityDescriptionText.text = abilityDescription;   
    }

    public void HideInfoBox()
    {
        myCanvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }

}
