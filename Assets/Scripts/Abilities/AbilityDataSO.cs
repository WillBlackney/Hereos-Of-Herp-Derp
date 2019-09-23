using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AbilityDataSO", menuName = "AbilityDataSO", order = 53)]
public class AbilityDataSO : ScriptableObject
{
    public enum DamageType { None, Physical, Magic};
    public enum AttackType { None, Melee, Ranged};

    public Sprite abilityImage;
    public string abilityName;
    public string abilityDescription;    
    public int abilityBaseCooldownTime;    
    public int abilityAPCost;
    public int abilityRange;
    public int abilityPrimaryValue;
    public int abilitySecondaryValue;
    public DamageType abilityDamageType;
    public AttackType abilityAttackType;
}
