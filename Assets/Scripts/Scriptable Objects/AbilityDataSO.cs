using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AbilityDataSO", menuName = "AbilityDataSO", order = 53)]
public class AbilityDataSO : ScriptableObject
{
    public enum DamageType { None, Physical, Magic, Poison};
    public enum AttackType { None, Melee, Ranged};
    public enum AbilityType { None, Skill, MeleeAttack, RangedAttack};

    public Sprite sprite;
    public string abilityName;
    public string description;    
    public int baseCooldownTime;    
    public int apCost;
    public int range;
    public int primaryValue;
    public int secondaryValue;
    public DamageType damageType;
    public AttackType attackType;
    public AbilityType abilityType;
}
