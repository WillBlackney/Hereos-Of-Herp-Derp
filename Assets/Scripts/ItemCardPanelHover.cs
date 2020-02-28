using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCardPanelHover : MonoBehaviour
{
    [Header("Component References")]
    public GameObject locationParent;


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
        if(item.itemType == ItemDataSO.ItemType.MeleeOneHand ||
            item.itemType == ItemDataSO.ItemType.MeleeTwoHand ||
            item.itemType == ItemDataSO.ItemType.RangedTwoHand)
        {
            BuildWeaponTabElements(item);
        }
    }
    public void OnItemCardMousedExit(ItemDataSO item)
    {
        locationParent.SetActive(false);
    }
    public void BuildWeaponTabElements(ItemDataSO item)
    {
        // Set sprite first
        if(item.weaponDamageType == ItemDataSO.WeaponDamageType.Physical)
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

   


}
