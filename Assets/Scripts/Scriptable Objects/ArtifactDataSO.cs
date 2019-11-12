using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ArtifactDataSO", menuName = "ArtifactDataSO", order = 53)]
public class ArtifactDataSO : ScriptableObject
{
    public enum Rarity { None, Common, Rare, Epic};

    public Sprite sprite;
    public string Name;
    public string description;
    public Rarity rarity;
}


