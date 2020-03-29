using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCardPanelHover : MonoBehaviour
{
    [Header("Component References")]
    public GameObject locationParent;
    public GameObject arrowParent;
    public RectTransform verticalFitterRect;

    [Header("Tab Component References")]
    public ItemCardInfoTab weaponInfoTab;
    public ItemCardInfoTab passiveTabOne;
    public ItemCardInfoTab passiveTabTwo;
    public ItemCardInfoTab passiveTabThree;

    [Header("Damage Type Sprite References")]
    public Sprite physicalSprite;
    public Sprite fireSprite;
    public Sprite frostSprite;
    public Sprite shadowSprite;
    public Sprite poisonSprite;
    public Sprite airSprite;




    // Singleton set up
    #region
    public static ItemCardPanelHover Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private void Update()
    {
        //FollowMouse();
    }
    public void FollowMouse()
    {
        locationParent.transform.position = Input.mousePosition;
    }
    public void MoveToItemCardPosition(ItemCard itemCard)
    {
        locationParent.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, itemCard.mouseOverInfoPanelPos.transform.position);
    }
   
    public void OnItemCardMouseExit()
    {
        Debug.Log("ItemCardPanelHover.OnItemCardMouseExit() called");
        DisableAllViews();
    }
    public void OnItemCardMousedOver(ItemCard itemCard)
    {
        Debug.Log("ItemCardPanelHover.OnItemCardMousedOver() called on " + itemCard.myItemDataSO.Name);
        EnableAllViews();

        // Weapon Info Tab
        if(itemCard.myItemDataSO.itemType == ItemDataSO.ItemType.MeleeOneHand ||
            itemCard.myItemDataSO.itemType == ItemDataSO.ItemType.MeleeTwoHand ||
            itemCard.myItemDataSO.itemType == ItemDataSO.ItemType.RangedTwoHand)
        {
            weaponInfoTab.EnableView();
            arrowParent.SetActive(true);
            BuildWeaponTabElements(itemCard.myItemDataSO);
        }
        else
        {
            weaponInfoTab.DisableView();
        }

        // Passive effect one tab
        if(itemCard.myItemDataSO.itemEffectOne != ItemDataSO.ItemEffect.None)
        {
            passiveTabOne.EnableView();
            arrowParent.SetActive(true);
            BuildInfoPanelTabElements(passiveTabOne, itemCard.myItemDataSO.itemEffectOne.ToString(), itemCard.myItemDataSO.itemEffectOneValue);
        }
        else
        {
            passiveTabOne.DisableView();
        }

        // Passive effect two tab
        if (itemCard.myItemDataSO.itemEffectTwo != ItemDataSO.ItemEffect.None)
        {
            passiveTabTwo.EnableView();
            arrowParent.SetActive(true);
            BuildInfoPanelTabElements(passiveTabTwo, itemCard.myItemDataSO.itemEffectTwo.ToString(), itemCard.myItemDataSO.itemEffectTwoValue);
        }
        else
        {
            passiveTabTwo.DisableView();
        }

        // Passive effect three tab
        if (itemCard.myItemDataSO.itemEffectThree != ItemDataSO.ItemEffect.None)
        {
            passiveTabThree.EnableView();
            arrowParent.SetActive(true);
            BuildInfoPanelTabElements(passiveTabThree, itemCard.myItemDataSO.itemEffectThree.ToString(), itemCard.myItemDataSO.itemEffectThreeValue);
        }
        else
        {
            passiveTabThree.DisableView();
        }

        MoveToItemCardPosition(itemCard);
        RefreshAllLayoutGroups();
    }
    
    public void BuildWeaponTabElements(ItemDataSO item)
    {
        Debug.Log("ItemCardPanelHover.BuildWeaponTabElements() called...");

        // Set sprite first
        if (item.weaponDamageType == ItemDataSO.WeaponDamageType.Physical)
        {
            weaponInfoTab.image.sprite = physicalSprite;
        }
        else if (item.weaponDamageType == ItemDataSO.WeaponDamageType.Fire)
        {
            weaponInfoTab.image.sprite = fireSprite;
        }
        else if (item.weaponDamageType == ItemDataSO.WeaponDamageType.Frost)
        {
            weaponInfoTab.image.sprite = frostSprite;
        }
        else if (item.weaponDamageType == ItemDataSO.WeaponDamageType.Poison)
        {
            weaponInfoTab.image.sprite = poisonSprite;
        }
        else if (item.weaponDamageType == ItemDataSO.WeaponDamageType.Shadow)
        {
            weaponInfoTab.image.sprite = shadowSprite;
        }
        else if (item.weaponDamageType == ItemDataSO.WeaponDamageType.Air)
        {
            weaponInfoTab.image.sprite = airSprite;
        }

        // Set description Text
        weaponInfoTab.descriptionText.text = "This weapon deals " + 
            TextLogic.ReturnColoredText(item.baseDamage.ToString(), TextLogic.yellow) + " " +
            TextLogic.ReturnColoredText(item.weaponDamageType.ToString(), TextLogic.GetColorCodeFromString(item.weaponDamageType.ToString())) +
            " damage";

        // Set name text
        weaponInfoTab.nameText.text = "Weapon";

    }
    public void BuildInfoPanelTabElements(ItemCardInfoTab tab, string passiveName, int stacks)
    {
        Debug.Log("ItemCardPanelHover.BuildInfoPanelTabElements() called for " + passiveName);

        if(passiveName == "BonusStrength")
        {            
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Strength");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);           
        }

        else if (passiveName == "BonusWisdom")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Wisdom");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "BonusDexterity")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Dexterity");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "BonusStamina")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Stamina");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "BonusInitiative")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Initiative");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "BonusMobility")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Mobility");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "BonusCritical")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Critical");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }

        else if (passiveName == "BonusDodge")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Dodge");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "BonusParry")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Parry");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "BonusMaxEnergy")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Max Energy");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "BonusMeleeRange")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Melee Range");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "BonusAuraSize")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Radiance");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "BonusFireDamage")
        {
            // TO DO: make a status icon data SO object for critical
        }
        else if (passiveName == "BonusFrostDamage")
        {
            // TO DO: make a status icon data SO object for critical
        }
        else if (passiveName == "BonusAirDamage")
        {
            // TO DO: make a status icon data SO object for critical
        }
        else if (passiveName == "BonusPoisonDamage")
        {
            // TO DO: make a status icon data SO object for critical
        }
        else if (passiveName == "BonusShadowDamage")
        {
            // TO DO: make a status icon data SO object for critical
        }
        else if (passiveName == "Enrage")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Enrage");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "Poisonous")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Poisonous");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "Immolation")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Immolation");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "Cautious")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Cautious");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "Growing")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Growing");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "Fast Learner")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Fast Learner");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "Pierce")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Pierce");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "Unwavering")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Unwavering");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "Flux")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Flux");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "HawkEye")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Hawk Eye");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "Thorns")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Thorns");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "Opportunist")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Opportunist");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "BonusPowerLimit")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Power Limit");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "BonusAllResistances")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus All Resistances");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "Stealth")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Stealth");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "TrueSight")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("True Sight");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "Slippery")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Slippery");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "Unstoppable")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Unstoppable");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "PerfectAim")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Perfect Aim");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "Virtuoso")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Virtuoso");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "Riposte")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Riposte");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }
        else if (passiveName == "ShadowForm")
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Shadow Form");
            tab.nameText.text = iconData.statusName;
            tab.image.sprite = iconData.statusSprite;
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.descriptionText, stacks);
        }



    }
    public void DisableAllViews()
    {
        Debug.Log("ItemCardPanelHover.DisableAllViews() called");

        locationParent.SetActive(false);
        arrowParent.SetActive(false);
        weaponInfoTab.DisableView();
        passiveTabOne.DisableView();
        passiveTabTwo.DisableView();
        passiveTabThree.DisableView();
    }
    public void EnableAllViews()
    {
        Debug.Log("ItemCardPanelHover.EnableAllViews() called");

        locationParent.SetActive(true);
        
    }
    public void RefreshAllLayoutGroups()
    {
        Debug.Log("ItemCardPanelHover.RefreshAllLayoutGroups() called");

        // rebuild master vertical content fitter
        LayoutRebuilder.ForceRebuildLayoutImmediate(verticalFitterRect);

        // rebuild individual panels (if active)
        if (weaponInfoTab.gameObject.activeSelf == true)
        {
            weaponInfoTab.RefreshLayoutGroups();
        }
        if (passiveTabOne.gameObject.activeSelf == true)
        {
            passiveTabOne.RefreshLayoutGroups();
        }
        if (passiveTabTwo.gameObject.activeSelf == true)
        {
            passiveTabTwo.RefreshLayoutGroups();
        }
        if (passiveTabThree.gameObject.activeSelf == true)
        {
            passiveTabThree.RefreshLayoutGroups();
        }
    }

   


}
