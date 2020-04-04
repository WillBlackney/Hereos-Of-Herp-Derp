using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StoryWindowCharacterSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Properties")]
    public CharacterData myCharacterData;

    [Header("Component References")]
    public TextMeshProUGUI currentHealthText;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI currentXPText;
    public Image myGlowOutline;
    public UniversalCharacterModel myModel;
    public Slider healthBar;
    public Slider xpBar;

    // Initialization + Setip
    #region
    public void InitializeSetup(CharacterData data)
    {
        data.myStoryWindowCharacter = this;
        myCharacterData = data;
        currentHealthText.text = data.currentHealth.ToString();
        maxHealthText.text = data.maxHealth.ToString();
        currentXPText.text = data.currentXP.ToString();
        CharacterModelController.BuildModelFromPresetString(myModel, myCharacterData.myName);
        myModel.SetBaseAnim();
    }
    #endregion

    // Modify Texts + GUI
    #region
    public void ModifyCurrentHealthText(int newValue)
    {
        currentHealthText.text = newValue.ToString();
    }
    public void ModifyMaxHealthText(int newValue)
    {
        maxHealthText.text = newValue.ToString();
    }
    public void UpdateHealthBarPosition(int currentHealth, int maxHealth)
    {
        float currentHealthFloat = currentHealth;
        float maxHealthFloat = maxHealth;
        healthBar.value = currentHealthFloat / maxHealthFloat;
    }
    public void ModifyCurrentXPText(int newValue)
    {
        currentXPText.text = newValue.ToString();
    }
    public void UpdateXpBarPosition(int currentXP, int maxXP)
    {
        float currentXPFloat = currentXP;
        float maxXpFloat = maxXP;
        xpBar.value = currentXPFloat / maxXpFloat;
    }
    #endregion

    // Pointer + Mouse Events
    #region
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick() called...");
        StoryEventManager.Instance.HandleCharacterWindowClicked(this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter() called...");
        myGlowOutline.gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit() called...");
        myGlowOutline.gameObject.SetActive(false);
    }
    #endregion

    // View Logic
    #region
    public void SetGlowOutilineViewState(bool onOrOff)
    {
        myGlowOutline.gameObject.SetActive(onOrOff);
    }
    #endregion

}
