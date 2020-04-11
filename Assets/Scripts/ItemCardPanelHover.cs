using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCardPanelHover : MonoBehaviour
{
    // Properties + Component References
    #region
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
    #endregion

    // Initialization + Setup
    #region
    public void BuildWeaponTabElements(ItemDataSO item)
    {
        Debug.Log("ItemCardPanelHover.BuildWeaponTabElements() called...");

        // Set sprite first
        if (item.weaponDamageType == ItemDataSO.WeaponDamageType.Physical)
        {
            //weaponInfoTab.image.sprite = physicalSprite;
            weaponInfoTab.passiveInfoSheet.abilityImage.sprite = physicalSprite;
        }
        else if (item.weaponDamageType == ItemDataSO.WeaponDamageType.Fire)
        {
            // weaponInfoTab.image.sprite = fireSprite;
            weaponInfoTab.passiveInfoSheet.abilityImage.sprite = fireSprite;
        }
        else if (item.weaponDamageType == ItemDataSO.WeaponDamageType.Frost)
        {
            // weaponInfoTab.image.sprite = frostSprite;
            weaponInfoTab.passiveInfoSheet.abilityImage.sprite = frostSprite;
        }
        else if (item.weaponDamageType == ItemDataSO.WeaponDamageType.Poison)
        {
            //weaponInfoTab.image.sprite = poisonSprite;
            weaponInfoTab.passiveInfoSheet.abilityImage.sprite = poisonSprite;
        }
        else if (item.weaponDamageType == ItemDataSO.WeaponDamageType.Shadow)
        {
            //weaponInfoTab.image.sprite = shadowSprite;
            weaponInfoTab.passiveInfoSheet.abilityImage.sprite = shadowSprite;
        }
        else if (item.weaponDamageType == ItemDataSO.WeaponDamageType.Air)
        {
            //weaponInfoTab.image.sprite = airSprite;
            weaponInfoTab.passiveInfoSheet.abilityImage.sprite = airSprite;
        }

        // Set description Text
        weaponInfoTab.passiveInfoSheet.descriptionText.text = "This weapon deals " +
            TextLogic.ReturnColoredText(item.baseDamage.ToString(), TextLogic.yellow) + " " +
            TextLogic.ReturnColoredText(item.weaponDamageType.ToString(), TextLogic.GetColorCodeFromString(item.weaponDamageType.ToString())) +
            " damage";

        // Set name text
        weaponInfoTab.passiveInfoSheet.nameText.text = "Weapon";

        // Set pivot
        PassiveInfoSheetController.Instance.SetPivotDirection(weaponInfoTab.passiveInfoSheet, PassiveInfoSheet.PivotDirection.Downwards);

    }
    public void BuildInfoPanelTabElements(ItemCardInfoTab tab, string passiveName, int stacks)
    {
        Debug.Log("ItemCardPanelHover.BuildInfoPanelTabElements() called for " + passiveName);

        PassiveInfoSheet.PivotDirection pivotDirection = PassiveInfoSheet.PivotDirection.Downwards;
        StatusIconDataSO iconData = null;

        if (passiveName == "BonusStrength")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Strength");
        }
        else if (passiveName == "BonusWisdom")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Wisdom");
        }
        else if (passiveName == "BonusDexterity")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Dexterity");
        }
        else if (passiveName == "BonusStamina")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Stamina");
        }
        else if (passiveName == "BonusInitiative")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Initiative");
        }
        else if (passiveName == "BonusMobility")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Mobility");
        }
        else if (passiveName == "BonusCritical")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Critical");
        }
        else if (passiveName == "BonusDodge")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Dodge");
        }
        else if (passiveName == "BonusParry")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Parry");
        }
        else if (passiveName == "BonusMaxEnergy")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Max Energy");
        }
        else if (passiveName == "BonusMeleeRange")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Melee Range");
        }
        else if (passiveName == "BonusAuraSize")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Radiance");
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
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Enrage");
        }
        else if (passiveName == "Poisonous")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Poisonous");
        }
        else if (passiveName == "Immolation")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Immolation");
        }
        else if (passiveName == "Cautious")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Cautious");
        }
        else if (passiveName == "Growing")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Growing");
        }
        else if (passiveName == "Fast Learner")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Fast Learner");
        }
        else if (passiveName == "Pierce")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Pierce");
        }
        else if (passiveName == "Unwavering")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Unwavering");
        }
        else if (passiveName == "Flux")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Flux");
        }
        else if (passiveName == "HawkEye")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Hawk Eye");
        }
        else if (passiveName == "Thorns")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Thorns");
        }
        else if (passiveName == "Opportunist")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Opportunist");
        }
        else if (passiveName == "BonusPowerLimit")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Power Limit");
        }
        else if (passiveName == "BonusAllResistances")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus All Resistances");
        }
        else if (passiveName == "Stealth")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Stealth");
        }
        else if (passiveName == "TrueSight")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("True Sight");
        }
        else if (passiveName == "Slippery")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Slippery");
        }
        else if (passiveName == "Unstoppable")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Unstoppable");
        }
        else if (passiveName == "PerfectAim")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Perfect Aim");
        }
        else if (passiveName == "Virtuoso")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Virtuoso");
        }
        else if (passiveName == "Riposte")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Riposte");
        }
        else if (passiveName == "ShadowForm")
        {
            iconData = StatusIconLibrary.Instance.GetStatusIconByName("Shadow Form");
        }


        if (iconData != null)
        {
            PassiveInfoSheetController.Instance.BuildSheetFromData(tab.passiveInfoSheet, iconData, stacks, pivotDirection);
            TextLogic.SetStatusIconDescriptionText(iconData.statusName, tab.passiveInfoSheet.descriptionText, stacks);
        }

    }
    #endregion

    // Singleton set up
    #region
    public static ItemCardPanelHover Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // View Logic
    #region
    public void MoveToItemCardPosition(ItemCard itemCard)
    {
        locationParent.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, itemCard.mouseOverInfoPanelPos.transform.position);
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
        for (int i = 0; i < 2; i++)
        {
            PassiveInfoSheetController.Instance.RefreshAllLayoutGroups(weaponInfoTab.passiveInfoSheet);
            PassiveInfoSheetController.Instance.RefreshAllLayoutGroups(passiveTabOne.passiveInfoSheet);
            PassiveInfoSheetController.Instance.RefreshAllLayoutGroups(passiveTabTwo.passiveInfoSheet);
            PassiveInfoSheetController.Instance.RefreshAllLayoutGroups(passiveTabThree.passiveInfoSheet);

            weaponInfoTab.mainParentTransform.sizeDelta = new Vector2(weaponInfoTab.mainParentTransform.sizeDelta.x, weaponInfoTab.passiveInfoSheet.allElementsRectTransform.sizeDelta.y);
            passiveTabOne.mainParentTransform.sizeDelta = new Vector2(passiveTabOne.mainParentTransform.sizeDelta.x, passiveTabOne.passiveInfoSheet.allElementsRectTransform.sizeDelta.y);
            passiveTabTwo.mainParentTransform.sizeDelta = new Vector2(passiveTabTwo.mainParentTransform.sizeDelta.x, passiveTabTwo.passiveInfoSheet.allElementsRectTransform.sizeDelta.y);
            passiveTabThree.mainParentTransform.sizeDelta = new Vector2(passiveTabThree.mainParentTransform.sizeDelta.x, passiveTabThree.passiveInfoSheet.allElementsRectTransform.sizeDelta.y);

            LayoutRebuilder.ForceRebuildLayoutImmediate(verticalFitterRect);
        }


    }
    #endregion

    // Mouse + Input Events
    #region
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
            arrowParent.SetActive(true);
            BuildWeaponTabElements(itemCard.myItemDataSO);
            weaponInfoTab.EnableView();
        }
        else
        {
            weaponInfoTab.DisableView();
        }

        // Passive effect one tab
        if(itemCard.myItemDataSO.itemEffectOne != ItemDataSO.ItemEffect.None)
        {            
            arrowParent.SetActive(true);
            BuildInfoPanelTabElements(passiveTabOne, itemCard.myItemDataSO.itemEffectOne.ToString(), itemCard.myItemDataSO.itemEffectOneValue);
            passiveTabOne.EnableView();
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
    #endregion

    

}
