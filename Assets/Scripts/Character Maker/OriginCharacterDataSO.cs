using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New OriginCharacterDataSO", menuName = "OriginCharacterDataSO", order = 53)]
public class OriginCharacterDataSO : ScriptableObject
{
    [Header("Origin Properties")]
    public string characterName;
    [TextArea(10,10)] public string characterDescription;
    public List<CharacterData.Background> characterBackgrounds;
    public UniversalCharacterModel.ModelRace characterRace;

    [Header("Combat Properties")]
    public List<AbilityDataSO> knownAbilities;
    public List<StatusPairing> knownPassives;
    public List<TalentPairing> knownTalents;

    [Header("Weapons Properties")]
    public ItemDataSO mhWeapon;
    public ItemDataSO ohWeapon;

    [Header("Model View Properties")]
    public List<string> modelViewElements;

}
