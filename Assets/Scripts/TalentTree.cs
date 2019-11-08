using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TalentTree : MonoBehaviour
{
    [Header("Properties")]
    public CharacterData myCharacterData;
    public TalentTree myPartnerTree;
    public string talentTreeName;

    [Header("Component References")]
    public TextMeshProUGUI talentTreeNameText;

    [Header("Talent Buttons")]
    public Talent talentOne;
    public Talent talentTwo;
    public Talent talentThree;
    public Talent talentFour;
    public Talent talentFive;

    public List<Talent> allTalentButtons;

    public void InitializeSetup(string _talentTreeName)
    {
        SetTalentTreeName(_talentTreeName);

        // Rogue
        if (_talentTreeName == "Path of Combat")
        {            
            talentOne.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Improved Dash"), this, 1);
            talentTwo.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Stealth"), this, 2);
            talentThree.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Slice And Dice"), this, 3);
        }

        else if (_talentTreeName == "Path of Trickery")
        {
            talentOne.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Improved Preparation"), this, 1);
            talentTwo.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Venomous"), this, 2);
            talentThree.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Chemical Reaction"), this, 3);
        }

        // Warrior
        else if (_talentTreeName == "Path of Rage")
        {
            talentOne.InitializeSetup(this, AbilityLibrary.Instance.GetAbilityByName("Whirlwind"), 1);
            talentTwo.InitializeSetup(this, AbilityLibrary.Instance.GetAbilityByName("Blood Lust"), 2);
        }

        else if (_talentTreeName == "Path of the Guardian")
        {
            talentOne.InitializeSetup(this, AbilityLibrary.Instance.GetAbilityByName("Get Down!"), 1);
            talentTwo.InitializeSetup(this, AbilityLibrary.Instance.GetAbilityByName("Guard"), 2);
        }

        // Shaman
        else if (_talentTreeName == "Path of Storms")
        {
            talentOne.InitializeSetup(this, AbilityLibrary.Instance.GetAbilityByName("Chain Lightning"), 1);
            talentTwo.InitializeSetup(this, AbilityLibrary.Instance.GetAbilityByName("Primal Blast"), 2);

        }

        else if (_talentTreeName == "Path of Fury")
        {
            talentOne.InitializeSetup(this, AbilityLibrary.Instance.GetAbilityByName("Electrical Discharge"), 1);
            talentTwo.InitializeSetup(this, AbilityLibrary.Instance.GetAbilityByName("Primal Rage"), 2);
        }

        // Mage
        else if (_talentTreeName == "Path of Manipulation")
        {
            talentOne.InitializeSetup(this, AbilityLibrary.Instance.GetAbilityByName("Frost Bolt"), 1);
            talentTwo.InitializeSetup(this, AbilityLibrary.Instance.GetAbilityByName("Teleport"), 2);
        }

        else if (_talentTreeName == "Path of Wrath")
        {
            talentOne.InitializeSetup(this, AbilityLibrary.Instance.GetAbilityByName("Meteor"), 1);
            talentTwo.InitializeSetup(this, AbilityLibrary.Instance.GetAbilityByName("Phase Shift"), 2);           
        }

        // Priest
        else if (_talentTreeName == "Path of Divinity")
        {
            talentOne.InitializeSetup(this, AbilityLibrary.Instance.GetAbilityByName("Sanctity"), 1);
            talentTwo.InitializeSetup(this, AbilityLibrary.Instance.GetAbilityByName("Bless"), 2);
        }

        else if (_talentTreeName == "Path of Shadows")
        {
            talentOne.InitializeSetup(this, AbilityLibrary.Instance.GetAbilityByName("Siphon Life"), 1);
            talentTwo.InitializeSetup(this, AbilityLibrary.Instance.GetAbilityByName("Nightmare"), 2);
        }

        allTalentButtons.Add(talentOne);
        allTalentButtons.Add(talentTwo);
    }

    public void SetTalentTreeName(string _talentTreeName)
    {
        talentTreeName = _talentTreeName;
        talentTreeNameText.text = talentTreeName;
    }

    public void SetTalentTreePartner(TalentTree partnerTree)
    {
        myPartnerTree = partnerTree;
        talentOne.partnerTalent = partnerTree.talentOne;
        talentTwo.partnerTalent = partnerTree.talentTwo;
    }
}
