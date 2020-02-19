using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class Talent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum TalentPool { Guardian, Duelist, Brawler, Assassination, Pyromania, Cyromancy,
        Ranger, Manipulation, Divinity, Shadowcraft, Corruption, Naturalism};

    [Header("General Component References")]  
    public GameObject myGlowOutline;
    public GameObject blackTintOverlay;
    public GameObject purchasedOverlay;
    public Image talentImage;

    [Header("Inspector Properties")]
    public string talentName;
    public TalentPool talentPool;
    public int talentTier;
    public bool isAbility;
    public bool isPassive;    
    public int passiveStacks;

    [Header("Properties")]
    public CharacterData myCharacterData;
    public AbilityDataSO myAbilityData;
    public bool purchased;

    [Header("Ability Panel Components")]
    public GameObject abilityInfoPanel;
    public TextMeshProUGUI abilityNameText;
    public TextMeshProUGUI abilityDescriptionText;   
    public TextMeshProUGUI abilityEnergyText;
    public TextMeshProUGUI abilityCooldownText;
    public TextMeshProUGUI abilityRangeText;

    [Header("Passive Panel Components")]
    public GameObject passiveInfoPanel;
    public TextMeshProUGUI passiveNameText;
    public TextMeshProUGUI passiveDescriptionText;
    public Image passiveImage;

    // Initialization
    #region
    void Start()
    {
        TalentController.Instance.BuildTalentInfoPanelFromData(this);
    }
    #endregion

    // Mouse + Pointer Events
    #region
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Talent.OnPointerClick() triggered...");
        TalentController.Instance.OnTalentButtonClicked(myCharacterData, this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Talent.OnPointerEnter() triggered...");
        myGlowOutline.SetActive(true);
        if (isAbility)
        {            
            abilityInfoPanel.SetActive(true);
        }
        else
        {
            passiveInfoPanel.SetActive(true);
        }
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Talent.OnPointerExit() triggered...");
        myGlowOutline.SetActive(false);
        if (isAbility)
        {
            abilityInfoPanel.SetActive(false);
        }
        else
        {
            passiveInfoPanel.SetActive(false);
        }
    }
    #endregion


}
