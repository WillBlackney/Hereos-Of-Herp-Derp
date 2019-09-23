using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterInfoPanel : MonoBehaviour
{
    public Enemy myEnemy;
    public TextMeshProUGUI nameText;
    public Image myCharacterSprite;
    public GameObject panelParent;
    public GameObject spellToolTipParent;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI mobilityText;
    public TextMeshProUGUI strengthText;

    public void InitializeSetup(Enemy enemyParent)
    {
        myEnemy = enemyParent;
        nameText.text = myEnemy.myName;
        myCharacterSprite.sprite = myEnemy.mySpriteRenderer.sprite;

        healthText.text = myEnemy.baseMaxHealth.ToString();
        energyText.text = myEnemy.baseEnergy.ToString();
        mobilityText.text = myEnemy.baseMobility.ToString();
        strengthText.text = myEnemy.baseStrength.ToString();
    }
    public void EnablePanelView()
    {
        panelParent.SetActive(true);
    }

    public void DisablePanelView()
    {
        panelParent.SetActive(false);
    }

    public void AddAbilityToolTipToView(Ability ability)
    {
        GameObject newToolTip = Instantiate(PrefabHolder.Instance.spellInfoPrefab, spellToolTipParent.transform);
        SpellToolTip tooltipData = newToolTip.GetComponent<SpellToolTip>();

        tooltipData.spellIcon.sprite = ability.abilityImage;
        tooltipData.spellNameText.text = ability.abilityName;
        tooltipData.apCostText.text = ability.abilityAPCost.ToString();
        tooltipData.cdText.text = ability.abilityBaseCooldownTime.ToString();
        tooltipData.descriptionText.text = ability.abilityDescription;
    }
}
