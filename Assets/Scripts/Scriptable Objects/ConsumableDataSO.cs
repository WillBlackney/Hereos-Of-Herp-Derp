using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ConsumableDataSO", menuName = "ConsumableDataSO", order = 52)]
public class ConsumableDataSO : ScriptableObject
{
    public Sprite consumableSprite;
    public string consumableName;    
    public string consumableDescription;
}
