using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : Singleton<PrefabHolder>
{
    [Header("Buttons + UI")]
    public GameObject AbilityButtonPrefab;
    public GameObject spellInfoPrefab;
    public GameObject activationWindowPrefab;

    [Header("Defender Game object Prefabs")]
    public GameObject warriorPrefab;
    public GameObject magePrefab;
    public GameObject priestPrefab;
    public GameObject rangerPrefab;
    public GameObject roguePrefab;
    public GameObject shamanPrefab;

    [Header("Loot Related")]
    public GameObject GoldRewardButton;
    public GameObject ItemRewardButton;
    public GameObject ArtifactRewardButton;
    public GameObject ArtifactGO;
    public GameObject ItemCard;
    public GameObject TreasureChest;

    [Header("World/Level Related")]
    public GameObject LevelBG;

    [Header("Enemy Related")]
    public GameObject ZombiePrefab;

}
