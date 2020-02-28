using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCardPanelHover : MonoBehaviour
{
    [Header("Component References")]
    public GameObject locationParent;
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
        FollowMouse();
    }
    public void FollowMouse()
    {
        locationParent.transform.position = Input.mousePosition;
    }
    public void OnItemCardMousedOver(ItemDataSO item)
    {
        Debug.Log("ItemCardPanelHover.OnItemCardMousedOver() called on " + item.Name);
        EnableAllViews();

        // Weapon Info Tab
        if(item.itemType == ItemDataSO.ItemType.MeleeOneHand ||
            item.itemType == ItemDataSO.ItemType.MeleeTwoHand ||
            item.itemType == ItemDataSO.ItemType.RangedTwoHand)
        {
            weaponInfoTab.EnableView();
            BuildWeaponTabElements(item);
        }

        // Passive effect one tab
        if(item.itemEffectOne != ItemDataSO.ItemEffect.None)
        {
            passiveTabOne.EnableView();
            BuildInfoPanelTabElements(passiveTabOne, item.itemEffectOne.ToString(), item.itemEffectOneValue);
        }

        // Passive effect two tab
        if (item.itemEffectTwo != ItemDataSO.ItemEffect.None)
        {
            passiveTabTwo.EnableView();
            BuildInfoPanelTabElements(passiveTabTwo, item.itemEffectTwo.ToString(), item.itemEffectTwoValue);
        }

        // Passive effect three tab
        if (item.itemEffectThree != ItemDataSO.ItemEffect.None)
        {
            passiveTabThree.EnableView();
            BuildInfoPanelTabElements(passiveTabThree, item.itemEffectThree.ToString(), item.itemEffectThreeValue);
        }

        RefreshAllLayoutGroups();
    }
    public void OnItemCardMouseExit(ItemDataSO item)
    {
        Debug.Log("ItemCardPanelHover.OnItemCardMouseExit() called");
        DisableAllViews();
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
            // TO DO: make a status icon data SO object for critical
        }

        else if (passiveName == "BonusDodge")
        {
            // TO DO: make a status icon data SO object for critical
        }
        else if (passiveName == "BonusParry")
        {
            // TO DO: make a status icon data SO object for critical
        }
        else if (passiveName == "BonusMaxEnergy")
        {
            // TO DO: make a status icon data SO object for critical
        }
        else if (passiveName == "BonusMeleeRange")
        {
            // TO DO: make a status icon data SO object for critical
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
            //
        }
        else if (passiveName == "BonusAllResistances")
        {
            //
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
