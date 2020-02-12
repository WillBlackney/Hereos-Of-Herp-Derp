using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CampSiteCharacter : MonoBehaviour, IPointerClickHandler
{
    [Header("Properties")]
    public CharacterData myCharacterData;

    [Header("Component References")]
    public TextMeshProUGUI currentHealthText;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI currentXPText;
    public Image myImageComponent;
    public Image myGlowOutline;
    public CanvasGroup myGlowCG;
    public UniversalCharacterModel myButtonModel;

    public void InitializeSetup(CharacterData data)
    {
        data.myCampSiteCharacter = this;
        myCharacterData = data;
        currentHealthText.text = data.currentHealth.ToString();
        maxHealthText.text = data.maxHealth.ToString();
        currentXPText.text = data.currentXP.ToString();
        CharacterModelController.BuildModelFromPresetString(myButtonModel, myCharacterData.myName);
        myButtonModel.SetBaseAnim();
    }
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
        else if (CampSiteManager.Instance.awaitingPrayChoice &&
            myCharacterData.currentHealth == 0)
        {
            CampSiteManager.Instance.PerformPray(this);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (CampSiteManager.Instance.awaitingTriageChoice)
        {
            CampSiteManager.Instance.PerformTriage(this);
        }
        else if (CampSiteManager.Instance.awaitingTrainChoice)
        {
            CampSiteManager.Instance.PerformTrain(this);
        }
        else if (CampSiteManager.Instance.awaitingPrayChoice &&
            myCharacterData.currentHealth == 0)
        {
            CampSiteManager.Instance.PerformPray(this);
        }
    }
    public void SetGlowOutilineViewState(bool onOrOff)
    {
        myGlowOutline.gameObject.SetActive(onOrOff);       

    }
}
