using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StateDataSO", menuName = "StateDataSO", order = 52)]
public class StateDataSO : ScriptableObject
{
    public enum ExpirationCondition { None, Timer, EndOfAct };

    public Sprite stateSprite;
    public string stateName;
    public string stateDescription;
    public bool affliction;
    public ExpirationCondition expirationCondition;
    public int duration;
    
       
    
}
