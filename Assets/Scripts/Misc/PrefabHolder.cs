using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    // Singleton Pattern
    #region
    public static PrefabHolder Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Prefabs References
    #region
    [Header("Buttons + UI")]
    public GameObject AbilityButtonPrefab;
    public GameObject abilityPageAbility;
    public GameObject consumable;
    public GameObject AttributeTab;
    public GameObject enemyPanelAbilityTab;
    public GameObject activationWindowPrefab;
    public GameObject statePrefab;
    public GameObject afflicationOnPanelPrefab;

    [Header("Defender Game Object Prefabs")]
    public GameObject defenderPrefab;

    [Header("Loot Related")]
    public GameObject GoldRewardButton;
    public GameObject ConsumableRewardButton;
    public GameObject ItemRewardButton;
    public GameObject stateRewardButton;
    public GameObject ArtifactGO;
    public GameObject ItemCard;
    public GameObject StateCard;
    public GameObject InventoryItem;
    public GameObject AbilityTomeInventoryCard;
    public GameObject TreasureChest;

    [Header("Enemy Related")]
    public GameObject ZombiePrefab;
    public GameObject skeletonSoldierPrefab;
    public GameObject toxicZombiePrefab;
    #endregion


}
