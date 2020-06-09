using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ClassPresetDataSO", menuName = "ClassPresetDataSO", order = 53)]
public class ClassPresetDataSO : ScriptableObject
{
    [Header("General Properties")]
    public string classPresetName;

    [Header("Talents")]
    [SerializeField] public List<TalentPairing> talents;

    [Header("Abilities + Passives")]
    public List<AbilityDataSO> abilities;
    [SerializeField] public List<StatusPairing> passives;

    [Header("Weapons")]
    [SerializeField] public ItemDataSO mainHandWeapon;
    [SerializeField] public ItemDataSO offHandWeapon;
}
