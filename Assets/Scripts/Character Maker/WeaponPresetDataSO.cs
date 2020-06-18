using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New WeaponPresetDataSO", menuName = "WeaponPresetDataSO", order = 53)]
[Serializable]
public class WeaponPresetDataSO : ScriptableObject
{
    [Header("General Properties")]
    public string weaponPresetName;

    [Header("Main Hand Weapon")]
    public ItemDataSO mainHandWeapon;

    [Header("Off Hand Weapon")]
    public ItemDataSO offHandWeapon;
}
