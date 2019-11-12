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

    // Initialization + Setup
    #region
    public void InitializeSetup(string _talentTreeName)
    {
        Debug.Log("TalentTree.InitializeSetup() called...");
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
            talentOne.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Improved Whirlwind"), this, 1);
            talentTwo.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Infinite Rage"), this, 2);
            talentThree.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Blood Lust"), this, 3);
        }

        else if (_talentTreeName == "Path of the Guardian")
        {
            talentOne.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Improved Inspire"), this, 1);
            talentTwo.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Unwavering"), this, 2);
            talentThree.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Get Down!"), this, 3);
        }

        // Shaman
        else if (_talentTreeName == "Path of Storms")
        {
        }

        else if (_talentTreeName == "Path of Fury")
        {
        }

        // Mage
        else if (_talentTreeName == "Path of Manipulation")
        {
            talentOne.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Improved Telekinesis"), this, 1);
            talentTwo.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Masterful Entrapment"), this, 2);
            talentThree.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Phase Shift"), this, 3);
        }

        else if (_talentTreeName == "Path of Wrath")
        {
            talentOne.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Improved Fire Ball"), this, 1);
            talentTwo.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Fiery Presence"), this, 2);
            talentThree.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Frost Nova"), this, 3);
        }

        // Priest
        else if (_talentTreeName == "Path of Divinity")
        {
            talentOne.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Improved Invigorate"), this, 1);
            talentTwo.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Guardian Presence"), this, 2);
            talentThree.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Bless"), this, 3);
        }

        else if (_talentTreeName == "Path of Shadows")
        {
            talentOne.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Improved Holy Fire"), this, 1);
            talentTwo.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Fiery Presence"), this, 2);
            talentThree.InitializeSetup(TalentLibrary.Instance.GetTalentDataByName("Nightmare"), this, 3);
        }

        allTalentButtons.Add(talentOne);
        allTalentButtons.Add(talentTwo);
        allTalentButtons.Add(talentThree);
        //allTalentButtons.Add(talentFour);
        //allTalentButtons.Add(talentFive);
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
        talentThree.partnerTalent = partnerTree.talentThree;
        talentFour.partnerTalent = partnerTree.talentFour;
        talentFive.partnerTalent = partnerTree.talentFive;
    }
    #endregion
}
