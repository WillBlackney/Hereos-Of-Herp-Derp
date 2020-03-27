using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StateDataSO", menuName = "StateDataSO", order = 52)]
public class StateDataSO : ScriptableObject
{
    public enum ExpirationCondition { None, Timer, EndOfAct };
    public enum Rarity { None, Common, Rare, Boss };

    public Sprite stateSprite;
    public string stateName;
    public string stateDescription;
    public Rarity rarity;
    public bool affliction;
    public bool eventReward;
    public ExpirationCondition expirationCondition;   
    public int duration;
    
       
    
}
