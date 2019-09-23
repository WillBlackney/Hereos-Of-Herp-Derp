using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemData
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
