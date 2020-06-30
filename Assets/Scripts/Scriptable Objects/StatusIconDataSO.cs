using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StatusIconDataSO", menuName = "StatusIconDataSO", order = 52)]
public class StatusIconDataSO : ScriptableObject
{
    [Header("Properties")]
    public string statusName;    
    public Sprite statusSprite;
    public string statusDescription;
    public bool showStackCount;
    public UniversalCharacterModel.ModelRace statusRace;
}
