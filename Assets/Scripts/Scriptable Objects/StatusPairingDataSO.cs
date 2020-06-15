using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StatusPairingDataSO", menuName = "StatusPairingDataSO", order = 53)]
public class StatusPairingDataSO : ScriptableObject
{
    [Header("General Properties")]
    public StatusIconDataSO statusData;
    public int stacks;

    [Header("Talent Tier Properties")]
    public AbilityDataSO.AbilitySchool abilitySchool;
    public int tier;
}
