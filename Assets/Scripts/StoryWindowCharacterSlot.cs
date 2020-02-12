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
    public Image myImageComponent;
    public Image myGlowOutline;
    public CanvasGroup myGlowCG;
    public UniversalCharacterModel myModel;

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
    public void SetGlowOutilineViewState(bool onOrOff)
    {
        myGlowOutline.gameObject.SetActive(onOrOff);

    }

   
}
