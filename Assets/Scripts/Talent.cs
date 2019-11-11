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
    public TalentDataSO myTalentData;

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

    public void InitializeSetup(TalentDataSO talentData, TalentTree talentTree, int treePosition)
    {
        myTalentTree = talentTree;
        myCharacterData = talentTree.myCharacterData;
        talentTreePosition = treePosition;
        myTalentData = talentData;

        if (talentData.isAbility)
        {
            talentName = talentData.talentAbilityData.abilityName;
            talentNameText.text = talentData.talentAbilityData.abilityName;
            talentDescriptionText.text = talentData.talentAbilityData.abilityDescription;
            // to do: set ap/cd/range text active here, or place all of these on a panel, then enable the panel
            talentAPText.text = talentData.talentAbilityData.abilityAPCost.ToString();
            talentRangeText.text = talentData.talentAbilityData.abilityRange.ToString();
            talentCDText.text = talentData.talentAbilityData.abilityBaseCooldownTime.ToString();
            talentImage.sprite = talentData.talentAbilityData.abilityImage;
        }
        else
        {
            talentName = talentData.talentName;
            talentNameText.text = talentData.talentName;
            talentDescriptionText.text = talentData.talentDescription;
            talentImage.sprite = talentData.talentImage;
        }

       

    }

    public void LearnTalent(string name)
    {
        Debug.Log("Talent.LearnTalent() called, learning talent: " + name);

        if (name == "Improved Preparation")
        {
            myCharacterData.KnowsImprovedPreparation = true;
        }
        else if (name == "Improved Dash")
        {
            myCharacterData.KnowsImprovedDash = true;
        }
        else if (name == "Stealth")
        {
            myCharacterData.Stealth = true;
        }
        else if (name == "Venomous")
        {
            myCharacterData.venomous = true;
        }
        else if (name == "Slice And Dice")
        {
            myCharacterData.KnowsSliceAndDice = true;
        }        
        else if (name == "Chemical Reaction")
        {
            myCharacterData.KnowsChemicalReaction = true;
        }
        else if (name == "Improved Invigorate")
        {
            myCharacterData.KnowsImprovedInvigorate = true;
        }
        else if (name == "Improved Holy Fire")
        {
            myCharacterData.KnowsImprovedHolyFire = true;
        }
        else if (name == "Fiery Presence")
        {
            myCharacterData.fieryPresenceStacks += 3;
        }
        else if (name == "Guardian Presence")
        {
            myCharacterData.guardianPresenceStacks += 3;
        }
        else if (name == "Bless")
        {
            myCharacterData.KnowsBless = true;
        }
        else if (name == "Nightmare")
        {
            myCharacterData.KnowsNightmare = true;
        }
        else if (name == "Improved Inspire")
        {
            myCharacterData.KnowsImprovedInspire = true;
        }
        else if (name == "Improved Whirlwind")
        {
            myCharacterData.KnowsImprovedWhirlwind = true;
        }
        else if (name == "Infinite Rage")
        {
            myCharacterData.enrageStacks += 2;
        }
        else if (name == "Unwavering")
        {
            myCharacterData.Unwavering = true;
        }
        else if (name == "Blood Lust")
        {
            myCharacterData.KnowsBloodLust = true;
        }
        else if (name == "Get Down!")
        {
            myCharacterData.KnowsGetDown = true;
        }
        else if (name == "Improved Telekinesis")
        {
            myCharacterData.KnowsImprovedTelekinesis = true;
        }
        else if (name == "Improved Fire Ball")
        {
            myCharacterData.KnowsImprovedFireBall = true;
        }
        else if (name == "Masterful Entrapment")
        {
            myCharacterData.masterfulEntrapmentStacks += 1;
        }
        else if (name == "Frost Nova")
        {
            myCharacterData.KnowsFrostNova = true;
        }
        else if (name == "Phase Shift")
        {
            myCharacterData.KnowsPhaseShift = true;
        }


        else if (name == "Poison Dart")
        {
            myCharacterData.KnowsPoisonDart = true;
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
        else if (
            (talentTreePosition == 3 &&
            (myTalentTree.talentTwo.talentLearned == true) || myTalentTree.myPartnerTree.talentTwo.talentLearned == true) &&
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
