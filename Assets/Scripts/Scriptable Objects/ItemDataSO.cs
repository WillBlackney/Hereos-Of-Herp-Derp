using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemDataSO", menuName = "ItemDataSO", order = 52)]
public class ItemDataSO : ScriptableObject
{
    public enum ItemRarity { NoRarity, Common, Rare, Epic };
    public enum ItemType { None, Head, Chest, Legs, Offhand, Shield, MeleeOneHand, MeleeTwoHand, RangedTwoHand };
    public enum WeaponDamageType { None, Physical, Poison, Fire, Frost, Shadow, Air };
    public enum GrantsAbility { None, Strike, Shoot, Block};
    public enum ItemEffect
    { None, BonusStrength, BonusWisdom, BonusDexterity, BonusStamina, BonusInitiative, BonusMobility,
      BonusCritical, BonusDodge, BonusParry, BonusMaxEnergy, BonusMeleeRange, BonusAuraSize,
      BonusFireDamage, BonusFrostDamage, BonusPoisonDamage, BonusShadowDamage, BonusAirDamage,
      Enrage, HawkEye, Thorns, Opportunist, BonusPowerLimit, BonusAllResistances,
      Stealth, TrueSight, Slippery, Unstoppable, PerfectAim, Virtuoso, Riposte, ShadowForm      
    };

    [Header("Primary Properties")]
    public Sprite sprite;
    public string Name;
    public string itemDescription;
    public ItemRarity itemRarity;
    public ItemType itemType;
    public GrantsAbility grantsAbility;
    public bool storyEventItem;
    public bool startingItem;

    [Header("Item Effect Properties")]
    public ItemEffect itemEffectOne;
    public int itemEffectOneValue;
    public ItemEffect itemEffectTwo;
    public int itemEffectTwoValue;
    public ItemEffect itemEffectThree;
    public int itemEffectThreeValue;

    [Header("Weapon Specific Properties")]
    public WeaponDamageType weaponDamageType;
    public int baseDamage;

    //[Header("Color Properties")]
    // to do in future: color properties should not be here. they should be in ItemManager or something else
    //public Color commonColour = Color.white;
    //public Color rareColour = Color.blue;
    //public Color epicColour = Color.red;

}
