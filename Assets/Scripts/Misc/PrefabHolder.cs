using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : Singleton<PrefabHolder>
{
    [Header("Buttons + UI")]
    public GameObject AbilityButtonPrefab;
    public GameObject AttributeTab;
    public GameObject enemyPanelAbilityTab;
    public GameObject activationWindowPrefab;
    public GameObject statePrefab;
    public GameObject afflicationOnPanelPrefab;
    public GameObject apBarDividerPrefab;

    [Header("Defender Game Object Prefabs")]
    public GameObject defenderPrefab;
    public GameObject warriorPrefab;
    public GameObject magePrefab;
    public GameObject priestPrefab;
    public GameObject rangerPrefab;
    public GameObject roguePrefab;
    public GameObject shamanPrefab;

    [Header("Loot Related")]
    public GameObject GoldRewardButton;
    public GameObject ItemRewardButton;
    public GameObject stateRewardButton;
    public GameObject ArtifactGO;
    public GameObject ItemCard;
    public GameObject StateCard;
    public GameObject InventoryItem;
    public GameObject TreasureChest;
    

    [Header("World/Level Related")]
    public GameObject LevelBG;

    [Header("Enemy Related")]
    public GameObject ZombiePrefab;
    public GameObject SkeletonPeasantPrefab;
    public List<GameObject> skeletonPrefabs;

}
