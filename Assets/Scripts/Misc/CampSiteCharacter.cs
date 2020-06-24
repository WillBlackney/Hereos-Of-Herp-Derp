using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Spriter2UnityDX;

public class CampSiteCharacter : MonoBehaviour
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

    [Header("Arrow Indicator Component References")]
    public GameObject arrowIndicatorParent;
    public Animator myArrowAnimator;

    [Header("Colour Properties")]
    public Color normalColor;
    public Color highlightColor;

    [Header("Properties")]
    public CharacterData myCharacterData;


    // Set up + initialization
    #region
    public void InitializeSetup(CharacterData data)
    {
        data.myCampSiteCharacter = this;
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
    #endregion

    // Mouse + Pointer events
    #region
    private void OnMouseEnter()
    {
        Debug.Log("CampSiteCharacter.OnMouseEnter() called...");
        myEntityRenderer.Color = highlightColor;
    }
    private void OnMouseExit()
    {
        Debug.Log("CampSiteCharacter.OnMouseExit() called...");
        myEntityRenderer.Color = normalColor;
    }
    public void OnMouseDown()
    {
        Debug.Log("CampSiteCharacter.OnMouseDown() called...");

        if (CampSiteManager.Instance.awaitingTriageChoice)
        {
            CampSiteManager.Instance.PerformTriage(this);
        }
        else if (CampSiteManager.Instance.awaitingTrainChoice)
        {
            CampSiteManager.Instance.PerformTrain(this);
        }
        else if (CampSiteManager.Instance.awaitingReadChoice)
        {
            CampSiteManager.Instance.PerformRead(this);
        }
        else if (CampSiteManager.Instance.awaitingPrayChoice &&
            myCharacterData.currentHealth == 0)
        {
            CampSiteManager.Instance.PerformPray(this);
        }
    }    
    #endregion

    // View + VFX logic
    #region
    public void SetArrowAnimState(bool onOrOff)
    {
        if (onOrOff)
        {
            arrowIndicatorParent.SetActive(true);
            myArrowAnimator.SetTrigger("Bounce");
        }
        else
        {            
            myArrowAnimator.SetTrigger("Bounce");
            arrowIndicatorParent.SetActive(false);
        }    

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
