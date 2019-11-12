using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemDataSO", menuName = "ItemDataSO", order = 52)]
//[System.Serializable]
public class ItemDataSO : ScriptableObject
{
    public enum ItemRarity { NoRarity, Common, Rare, Epic };
    public Color commonColour = Color.white;
    public Color rareColour = Color.blue;
    public Color epicColour = Color.red;

    public Sprite itemImage;
    public string itemName;
    public string itemDescription;
    public ItemRarity itemRarity;
}
