using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StateDataSO", menuName = "StateDataSO", order = 52)]
public class StateDataSO : ScriptableObject
{
    public Sprite sprite;
    public string Name;
    public string description;
    public ExpirationCondition expirationCondition;

    public bool affliction;
    public int duration;    
    public enum ExpirationCondition { None, Timer, EndOfAct};
}
