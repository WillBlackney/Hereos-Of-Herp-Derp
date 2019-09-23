using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Talent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Component References")]
    public GameObject blackOverlay;
    public GameObject highlightUnderlay;
    public GameObject infoPanelParent;
    public TextMeshProUGUI talentNameText;
    public TextMeshProUGUI talentDescriptionText;
    public TextMeshProUGUI talentAPText;
    public TextMeshProUGUI talentCDText;
    public TextMeshProUGUI talentRangeText;

    [Header("Properties")]
    public Image talentImage;
    public TalentTree myTalentTree;
    public CharacterData myCharacterData;
    public Talent partnerTalent;

    public string talentName;
    public bool talentLearned;
    public int talentTreePosition;

    // On Pointer events
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Talent.OnPointerClick() called...");
        if(IsTalentUnlocked() && HasEnoughTalentPoints() && talentLearned == false)
        {
            LearnTalent(talentName);
        }        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetInfoPanelViewState(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetInfoPanelViewState(false);
    }

    // Visuals
    public void SetInfoPanelViewState(bool onOrOff)
    {
        if (onOrOff == true)
        {
            infoPanelParent.SetActive(true);
        }
        else
        {
            infoPanelParent.SetActive(false);
        }
    }

    public void SetHighlightState(bool onOrOff)
    {
        if(onOrOff == true)
        {
            highlightUnderlay.SetActive(true);
        }
        else
        {
            highlightUnderlay.SetActive(false);
        }
    }

    public void SetBlackedOutState(bool onOrOff)
    {
        if (onOrOff == true)
        {
            blackOverlay.SetActive(true);
        }
        else
        {
            blackOverlay.SetActive(false);
        }
    }

    public void UpdateVisbilityState()
    {
        if (talentLearned)
        {
            SetHighlightState(false);
            SetBlackedOutState(false);
        }
        else if (partnerTalent.talentLearned)
        {
            SetHighlightState(false);
            SetBlackedOutState(true);
        }
        else if(IsTalentUnlocked() && myCharacterData.talentPoints > 0)
        {
            SetHighlightState(true);
            SetBlackedOutState(false);
        }
        else
        {
            SetHighlightState(false);
            SetBlackedOutState(true);
        }
    }

    // Logic

    public void InitializeSetup(TalentTree talentTree, AbilityDataSO data, int treePosition)
    {
        myTalentTree = talentTree;
        myCharacterData = talentTree.myCharacterData;
        talentTreePosition = treePosition;
        talentName = data.abilityName;
        talentNameText.text = data.abilityName;
        talentDescriptionText.text = data.abilityDescription;
        talentAPText.text = data.abilityAPCost.ToString();
        talentRangeText.text = data.abilityRange.ToString();
        talentCDText.text = data.abilityBaseCooldownTime.ToString();
        talentImage.sprite = data.abilityImage;
    }

    public void LearnTalent(string name)
    {
        Debug.Log("Talent.LearnTalent() called, learning talent: " + name);

        if (name == "Preparation")
        {
            myCharacterData.KnowsPreparation = true;
        }
        else if (name == "Slice And Dice")
        {
            myCharacterData.KnowsSliceAndDice = true;
        }
        else if (name == "Poison Dart")
        {
            myCharacterData.KnowsPoisonDart = true;
        }
        else if (name == "Chemical Reaction")
        {
            myCharacterData.KnowsChemicalReaction = true;
        }
        else if (name == "Whirlwind")
        {
            myCharacterData.KnowsWhirlwind = true;
        }
        else if (name == "Blood Lust")
        {
            myCharacterData.KnowsBloodLust = true;
        }
        else if (name == "Get Down!")
        {
            myCharacterData.KnowsGetDown = true;
        }
        else if (name == "Guard")
        {
            myCharacterData.KnowsGuard = true;
        }
        else if (name == "Chain Lightning")
        {
            myCharacterData.KnowsChainLightning = true;
        }
        else if (name == "Electrical Discharge")
        {
            myCharacterData.KnowsElectricalDischarge = true;
        }
        else if (name == "Primal Blast")
        {
            myCharacterData.KnowsPrimalBlast = true;
        }
        else if (name == "Primal Rage")
        {
            myCharacterData.KnowsPrimalRage = true;
        }
        else if (name == "Frost Bolt")
        {
            myCharacterData.KnowsFrostBolt = true;
        }
        else if (name == "Teleport")
        {
            myCharacterData.KnowsTeleport = true;
        }
        else if (name == "Meteor")
        {
            myCharacterData.KnowsMeteor = true;
        }
        else if (name == "Phase Shift")
        {
            myCharacterData.KnowsPhaseShift = true;
        }
        else if (name == "Sanctity")
        {
            myCharacterData.KnowsSanctity = true;
        }
        else if (name == "Bless")
        {
            myCharacterData.KnowsBless = true;
        }
        else if (name == "Siphon Life")
        {
            myCharacterData.KnowsSiphonLife = true;
        }
        else if (name == "Nightmare")
        {
            myCharacterData.KnowsNightmare = true;
        }

        talentLearned = true;
        myCharacterData.ModifyTalentPoints(-1);
        myCharacterData.SetTalentButtonVisibilities();
    }

    public void OnTalentButtonClicked()
    {
       // LearnTalent(talentName);
    }

    public bool IsTalentUnlocked()
    {
        if (talentTreePosition == 1 && partnerTalent.talentLearned == false)
        {
            return true;
        }

        else if (
            (talentTreePosition == 2 && 
            (myTalentTree.talentOne.talentLearned == true) || myTalentTree.myPartnerTree.talentOne.talentLearned == true) &&
            partnerTalent.talentLearned == false            
            )
        {
            return true;
        }

        else
        {
            Debug.Log("Cannot learn talent: talent not yet unlocked");
            return false;
        }
    }

    public bool HasEnoughTalentPoints()
    {
        if(myCharacterData.talentPoints > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

   

}
