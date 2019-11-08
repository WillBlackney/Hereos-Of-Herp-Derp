using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TalentDataSO", menuName = "TalentDataSO", order = 53)]
public class TalentDataSO : ScriptableObject
{
    public string talentName;
    public string talentDescription;
    public Sprite talentImage;
    public bool isAbility;
    public AbilityDataSO talentAbilityData;
}
