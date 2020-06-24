using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spriter2UnityDX;

public class VillageCharacter : MonoBehaviour
{
    [Header("Health Bar Component References")]
    public Slider healthBar;
    public TextMeshProUGUI currentHealthText;
    public TextMeshProUGUI maxHealthText;

    [Header("XP Bar Component References")]
    public Slider xpBar;
    public TextMeshProUGUI currentXPText;

    [Header("Model Component References")]
    public UniversalCharacterModel myModel;
    public EntityRenderer myEntityRenderer;

    [Header("Properties")]
    public CharacterData myCharacterData;

    // Set up + initialization
    #region
    public void InitializeSetup(CharacterData data)
    {
        data.myVillageCharacter = this;
        myCharacterData = data;
        currentHealthText.text = data.currentHealth.ToString();
        maxHealthText.text = data.maxHealth.ToString();
        currentXPText.text = data.currentXP.ToString();
        //CharacterModelController.BuildModelFromPresetString(myModel, myCharacterData.myName);
        myModel.SetBaseAnim();
    }
    #endregion

    // Modify Health + XP
    #region
    public void ModifyCurrentHealthText(int newValue)
    {
        currentHealthText.text = newValue.ToString();
    }
    public void ModifyMaxHealthText(int newValue)
    {
        maxHealthText.text = newValue.ToString();
    }
    public void ModifyCurrentXPText(int newValue)
    {
        currentXPText.text = newValue.ToString();
    }
    public void UpdateHealthBarPosition(int currentHealth, int maxHealth)
    {
        float currentHealthFloat = currentHealth;
        float maxHealthFloat = maxHealth;
        healthBar.value = currentHealthFloat / maxHealthFloat;
    }
    public void UpdateXpBarPosition(int currentXP, int maxXP)
    {
        float currentXPFloat = currentXP;
        float maxXpFloat = maxXP;
        xpBar.value = currentXPFloat / maxXpFloat;
    }
    #endregion
}
