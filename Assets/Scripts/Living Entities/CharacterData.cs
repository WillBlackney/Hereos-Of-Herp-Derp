using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterData : MonoBehaviour
{
    // Components References
    #region
    [Header("General References")]
    public GameObject masterVisualParent;
    public GameObject statsPageParent;
    public GameObject talentsPageParent;
    public GameObject abilityPageParent;
    public TextMeshProUGUI myNameText;

    [Header("Page Button References")]
    public GameObject statsPageButton;
    public GameObject talentsPageButton;
    public GameObject abilityPageButton;

    [Header("Health Text References")]
    public TextMeshProUGUI currentHealthText;
    public TextMeshProUGUI maxHealthText;

    [Header("XP + Level Text References")]
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI maxXpText;
    public TextMeshProUGUI levelText;

    [Header("Core Stat Text References")]
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI wisdomText;
    public TextMeshProUGUI dexterityText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI mobilityText;
    public TextMeshProUGUI initiativeText;

    [Header("Secondary Stat Text References")]
    public TextMeshProUGUI criticalText;
    public TextMeshProUGUI dodgeText;
    public TextMeshProUGUI parryText;
    public TextMeshProUGUI auraSizeText;
    public TextMeshProUGUI maxEnergyText;
    public TextMeshProUGUI meleeRangeText;

    [Header("Resistance Text References")]
    public TextMeshProUGUI physicalResistanceText;
    public TextMeshProUGUI fireResistanceText;
    public TextMeshProUGUI frostResistanceText;
    public TextMeshProUGUI poisonResistanceText;
    public TextMeshProUGUI airResistanceText;
    public TextMeshProUGUI shadowResistanceText;

    [Header("Talent + Ability Points Text References")]
    public TextMeshProUGUI talentPointsText;
    public TextMeshProUGUI abilityPointsText;   

    [Header("Talent Tree Page References")]
    public GameObject guardianTreeParent;
    public GameObject duelistTreeParent;
    public GameObject brawlerTreeParent;
    public GameObject assassinationTreeParent;
    public GameObject pyromaniaTreeParent;
    public GameObject cyromancyTreeParent;
    public GameObject rangerTreeParent;
    public GameObject manipulationTreeParent;
    public GameObject divinityTreeParent;
    public GameObject shadowcraftTreeParent;
    public GameObject corruptionTreeParent;
    public GameObject naturalismTreeParent;

    [Header("Talents Top Bar References")]
    public GameObject guardianTopBarButton;
    public GameObject duelistTopBarButton;
    public GameObject brawlerTopBarButton;
    public GameObject assassinationTopBarButton;
    public GameObject pyromaniaTopBarButton;
    public GameObject cyromancyTopBarButton;
    public GameObject rangerTopBarButton;
    public GameObject manipulationTopBarButton;
    public GameObject divinityTopBarButton;
    public GameObject shadowcraftTopBarButton;
    public GameObject corruptionTopBarButton;
    public GameObject naturalismTopBarButton;

    [Header("Talent List Points References")]
    public TextMeshProUGUI guardianPointsText;
    public TextMeshProUGUI duelistPointsText;
    public TextMeshProUGUI brawlerPointsText;
    public TextMeshProUGUI assassinationPointsText;
    public TextMeshProUGUI pyromaniaPointsText;
    public TextMeshProUGUI cyromancyPointsText;
    public TextMeshProUGUI rangerPointsText;
    public TextMeshProUGUI manipulationPointsText;
    public TextMeshProUGUI divinityPointsText;
    public TextMeshProUGUI shadowcraftPointsText;
    public TextMeshProUGUI corruptionPointsText;
    public TextMeshProUGUI naturalismPointsText;

    [Header("Talent Plus Button References")]
    public GameObject guardianPlusButton;
    public GameObject duelistPlusButton;
    public GameObject brawlerPlusButton;
    public GameObject assassinationPlusButton;
    public GameObject pyromaniaPlusButton;
    public GameObject cyromancyPlusButton;
    public GameObject rangerPlusButton;
    public GameObject manipulationPlusButton;
    public GameObject divinityPlusButton;
    public GameObject shadowcraftPlusButton;
    public GameObject corruptionPlusButton;
    public GameObject naturalismPlusButton;

    [Header("Health Properties")]
    public int currentHealth;
    public int maxHealth;

    [Header("Primary Stat Properties")]
    public int strength;
    public int wisdom;
    public int dexterity;
    public int stamina;
    public int mobility;    
    public int initiative;    

    [Header("Secondary Stat Properties")]
    public int criticalChance;
    public int dodge;
    public int parry;
    public int auraSize;
    public int maxEnergy;
    public int meleeRange;

    [Header("Resistance Properties")]
    public int physicalResistance;
    public int fireResistance;
    public int frostResistance;
    public int poisonResistance;
    public int airResistance;
    public int shadowResistance;

    [Header("XP and Level Properties")]
    public int currentXP;
    public int currentMaxXP;
    public int currentLevel;

    [Header("Talent Properties")]
    public int talentPoints;
    public int abilityPoints;

    [Header("Specific Talent Properties")]
    public int guardianPoints;
    public int duelistPoints;
    public int brawlerPoints;
    public int assassinationPoints;
    public int pyromaniaPoints;
    public int cyromancyPoints;
    public int rangerPoints;
    public int manipulationPoints;
    public int divinityPoints;
    public int shadowcraftPoints;
    public int corruptionPoints;
    public int naturalismPoints;

    [Header("Passive Trait Properties")]
    public int tenaciousStacks;
    public int masochistStacks;
    public int lastStandStacks;
    public int slipperyStacks;
    public int riposteStacks;
    public int perfectReflexesStacks;
    public int opportunistStacks;
    public int patientStalkerStacks;
    public int stealthStacks;
    public int cautiousStacks;
    public int guardianAuraStacks;
    public int unwaveringStacks;
    public int fieryAuraStacks;
    public int immolationStacks;
    public int demonStacks;
    public int shatterStacks;
    public int frozenHeartStacks;
    public int predatorStacks;
    public int hawkEyeStacks;
    public int fluxStacks;
    public int phasingStacks;
    public int etherealBeingStacks;
    public int encouragingAuraStacks;
    public int radianceStacks;
    public int sacredAuraStacks;
    public int shadowAuraStacks;
    public int shadowFormStacks;
    public int poisonousStacks;
    public int venomousStacks;
    public int toxicityStacks;
    public int toxicAuraStacks;
    public int stormAuraStacks;
    public int stormLordStacks;
    public int thornsStacks;    

    

    [Header("Misc Properties")]
    public string myName;
    #endregion


    // Enable + Disable All My Views
    public void DisableMainWindowView()
    {
        masterVisualParent.SetActive(false);
    }
    public void EnableMainWindowView()
    {
        masterVisualParent.SetActive(true);
        CharacterRoster.Instance.EnableInventoryView();
    }

    // Legacy properties
    #region

    [Header("Legacy Components")]
    public GameObject inventoryItemParent;
    //public Image myImageComponent;
    public Defender myDefenderGO;
    public string myClass;
    public CampSiteCharacter myCampSiteCharacter;
    public StoryWindowCharacterSlot myStoryWindowCharacter;    

    [Header("Front Page Components")]     
    public TextMeshProUGUI classNameText;
    public Image classSprite;    

    [Header("Item Properties")]
    public List<ItemCard> myItems = new List<ItemCard>();

    

    [Header("Ability Properties")]
    public bool KnowsMove;
    public bool KnowsStrike;
    public bool KnowsDefend;
    public bool KnowsCharge;
    public bool KnowsGuard;
    public bool KnowsInspire;
    public bool KnowsMeteor;
    public bool KnowsTelekinesis;
    public bool KnowsFrostBolt;
    public bool KnowsFireBall;
    public bool KnowsShoot;
    public bool KnowsRapidFire;
    public bool KnowsImpalingBolt;
    public bool KnowsForestMedicine;
    public bool KnowsWhirlwind;
    public bool KnowsInvigorate;
    public bool KnowsHolyFire;
    public bool KnowsVoidBomb;
    public bool KnowsNightmare;
    public bool KnowsTwinStrike;
    public bool KnowsDash;
    public bool KnowsPreparation;
    public bool KnowsHealingLight;
    public bool KnowsSliceAndDice;
    public bool KnowsPoisonDart;
    public bool KnowsChemicalReaction;
    public bool KnowsBloodLust;
    public bool KnowsGetDown;
    public bool KnowsSmash;
    public bool KnowsLightningShield;
    public bool KnowsElectricalDischarge;
    public bool KnowsChainLightning;
    public bool KnowsPrimalBlast;
    public bool KnowsPrimalRage;
    public bool KnowsPhaseShift;
    public bool KnowsTeleport;
    public bool KnowsSanctity;
    public bool KnowsBless;
    public bool KnowsSiphonLife;
    public bool KnowsChaosBolt;
    public bool KnowsFrostNova;
    public bool KnowsDevastatingBlow;
    public bool KnowsKickToTheBalls;
    public bool KnowsBladeFlurry;
    public bool KnowsRecklessness;
    public bool KnowsTendonSlash;
    public bool KnowsShieldShatter;
    public bool KnowsEvasion;
    public bool KnowsDecapitate;
    public bool KnowsVanish;
    public bool KnowsCheapShot;
    public bool KnowsShank;
    public bool KnowsShadowStep;
    public bool KnowsAmbush;
    public bool KnowsSharpenBlade;
    public bool KnowsProvoke;
    public bool KnowsSwordAndBoard;
    public bool KnowsShieldSlam;
    public bool KnowsTestudo;
    public bool KnowsReactiveArmour;
    public bool KnowsChallengingShout;
    public bool KnowsFireNova;
    public bool KnowsPhoenixDive;
    public bool KnowsBlaze;
    public bool KnowsCombustion;
    public bool KnowsDragonBreath;
    public bool KnowsChillingBlow;
    public bool KnowsIcyFocus;
    public bool KnowsBlizzard;
    public bool KnowsFrostArmour;
    public bool KnowsGlacialBurst;
    public bool KnowsCreepingFrost;
    public bool KnowsThaw;
    public bool KnowsHaste;
    public bool KnowsSteadyHands;
    public bool KnowsHeadShot;
    public bool KnowsTreeLeap;
    public bool KnowsConcentration;
    public bool KnowsOverwatch;
    public bool KnowsDimensionalBlast;
    public bool KnowsMirage;
    public bool KnowsBurstOfKnowledge;
    public bool KnowsBlink;
    public bool KnowsInfuse;
    public bool KnowsTimeWarp;
    public bool KnowsDimensionalHex;
    public bool KnowsConsecrate;
    public bool KnowsPurity;
    public bool KnowsBlindingLight;
    public bool KnowsTranscendence;
    public bool KnowsJudgement;
    public bool KnowsShroud;
    public bool KnowsHex;
    public bool KnowsRainOfChaos;
    public bool KnowsShadowWreath;
    public bool KnowsShadowBlast;
    public bool KnowsUnbridledChaos;
    public bool KnowsBlight;
    public bool KnowsBloodOffering;
    public bool KnowsToxicSlash;
    public bool KnowsNoxiousFumes;
    public bool KnowsToxicEruption;
    public bool KnowsDrain;
    public bool KnowsSpiritSurge;
    public bool KnowsLightningBolt;
    public bool KnowsThunderStrike;
    public bool KnowsSpiritVision;
    public bool KnowsThunderStorm;
    public bool KnowsOverload;
    public bool KnowsConcealingClouds;
    public bool KnowsSuperConductor;
    public bool KnowsSnipe;
    public bool KnowsRapidCloaking;
    public bool KnowsDisarm;
    public bool KnowsSecondWind;


    [Header("Known Talents")]
    public bool KnowsImprovedPreparation;
    public bool KnowsImprovedDash;
    public bool KnowsImprovedInvigorate;
    public bool KnowsImprovedHolyFire;
    public bool KnowsImprovedInspire;
    public bool KnowsImprovedWhirlwind;
    public bool KnowsImprovedTelekinesis;
    public bool KnowsImprovedFireBall;
    #endregion    

    // Initialization + Setup
    #region
    public void InitializeSetup(string characterClass)
    {
        if (characterClass == "Warrior")
        {
            //myImageComponent.sprite = CharacterLibrary.Instance.warriorSprite;
           // myNameText.text = CharacterLibrary.Instance.warriorClassName;
           // classSprite.sprite = CharacterLibrary.Instance.warriorSprite;
           // classNameText.text = CharacterLibrary.Instance.warriorClassName;

            myClass = "Warrior";

            // Set up health
            ModifyMaxHealth(100);
            ModifyCurrentHealth(100);

            // Set up core stats
            ModifyStrength(5);
            ModifyWisdom(5);
            ModifyDexterity(5);
            ModifyStamina(40);
            ModifyMobility(2);
            ModifyInitiative(3);

            // Set up secondary stats
            ModifyCriticalChance(5);
            ModifyParry(5);
            ModifyDodge(5);
            ModifyAuraSize(1);
            ModifyMaxEnergy(60);
            ModifyMeleeRange(1);

            // Set up resistances
            ModifyPhysicalResistance(5);
            ModifyFireResistance(5);
            ModifyFrostResistance(5);
            ModifyPoisonResistance(5);
            ModifyAirResistance(5);
            ModifyShadowResistance(5);

            // Set up Talent + Ability Points
            ModifyTalentPoints(99);
            ModifyAbilityPoints(99);

            // Set up Xp + Max Xp and Leve
            ModifyCurrentLevel(1);
            SetMaxXP(100);
            ModifyCurrentXP(0);

            // Abiltiies + Passives
            //cautiousStacks = 5;

            // Set up abilities
            KnowsMove = true;
            KnowsStrike = true;
            //KnowsFireBall = true;
            //KnowsPhoenixDive = true;
            //KnowsBloodOffering = true;
           // KnowsSecondWind = true;
            //KnowsGuard = true;
            //KnowsFrostNova = true;
           // KnowsShadowStep = true;


        }

        else if (characterClass == "Mage")
        {
            //myImageComponent.sprite = CharacterLibrary.Instance.mageSprite;
           // myNameText.text = CharacterLibrary.Instance.mageClassName;
          //  classSprite.sprite = CharacterLibrary.Instance.mageSprite;
           // classNameText.text = CharacterLibrary.Instance.mageClassName;
            myClass = "Mage";
            ModifyMobility(2);
            ModifyStrength(0);
            ModifyMaxHealth(90);
            ModifyCurrentHealth(90);
            ModifyStamina(40);
            ModifyInitiative(3);
            ModifyMaxEnergy(60);
            ModifyMeleeRange(1);
            ModifyCriticalChance(5);

            KnowsMove = true;
            KnowsStrike = true;
            KnowsDefend = true;
            KnowsFireBall = true;

        }

        else if (characterClass == "Ranger")
        {
            //myImageComponent.sprite = CharacterLibrary.Instance.rangerSprite;
          //  myNameText.text = CharacterLibrary.Instance.rangerClassName;
           // classSprite.sprite = CharacterLibrary.Instance.rangerSprite;
           // classNameText.text = CharacterLibrary.Instance.rangerClassName;
            myClass = "Ranger";
            ModifyMobility(5);
            ModifyStrength(0);
            ModifyMaxHealth(90);
            ModifyCurrentHealth(90);
            ModifyStamina(40);
            ModifyMaxEnergy(60);
            ModifyInitiative(3);
            ModifyMeleeRange(1);
            ModifyCriticalChance(5);

        }

        else if (characterClass == "Priest")
        {
            //myImageComponent.sprite = CharacterLibrary.Instance.priestSprite;
           // myNameText.text = CharacterLibrary.Instance.priestClassName;
           // classSprite.sprite = CharacterLibrary.Instance.priestSprite;
           // classNameText.text = CharacterLibrary.Instance.priestClassName;
            myClass = "Priest";
            ModifyMobility(2);
            ModifyStrength(0);
            ModifyMaxHealth(100);
            ModifyCurrentHealth(100);
            ModifyStamina(40);
            ModifyMaxEnergy(60);
            ModifyInitiative(3);
            ModifyMeleeRange(1);
            ModifyCriticalChance(5);

            encouragingAuraStacks = 10;

            KnowsMove = true;
            KnowsStrike = true;
            KnowsDefend = true;
            KnowsHolyFire = true;
            KnowsInvigorate = true;
            KnowsChaosBolt = true;

        }

        else if (characterClass == "Rogue")
        {
            //myImageComponent.sprite = CharacterLibrary.Instance.rogueSprite;
           // myNameText.text = CharacterLibrary.Instance.rogueClassName;
            //classSprite.sprite = CharacterLibrary.Instance.rogueSprite;
           // classNameText.text = CharacterLibrary.Instance.rogueClassName;
            myClass = "Rogue";
            ModifyMobility(2);
            ModifyStrength(0);
            ModifyMaxHealth(95);
            ModifyCurrentHealth(95);
            ModifyStamina(40);
            ModifyMaxEnergy(60);
            ModifyInitiative(3);
            ModifyMeleeRange(1);
            ModifyCriticalChance(5);
            poisonousStacks = 1;

            KnowsMove = true;
            KnowsStrike = true;
            KnowsDefend = true;
            KnowsTwinStrike = true;
            KnowsDash = true;
            KnowsPreparation = true;

        }

        else if (characterClass == "Shaman")
        {
            //myImageComponent.sprite = CharacterLibrary.Instance.shamanSprite;
            myNameText.text = CharacterLibrary.Instance.shamanClassName;
            classSprite.sprite = CharacterLibrary.Instance.shamanSprite;
            classNameText.text = CharacterLibrary.Instance.shamanClassName;
            myClass = "Shaman";
            ModifyMobility(2);
            ModifyStrength(0);
            ModifyMaxHealth(100);
            ModifyCurrentHealth(100);
            ModifyStamina(40);
            ModifyInitiative(3);
            ModifyMaxEnergy(60);
            ModifyMeleeRange(1);
            ModifyCriticalChance(5);

            KnowsMove = true;
            KnowsStrike = true;
            KnowsDefend = true;
            KnowsSmash = true;
            KnowsLightningShield = true;
            KnowsChainLightning = true;

            
        }
        // Set up talent trees
       // talentTreeOne.SetTalentTreePartner(talentTreeTwo);
        //talentTreeTwo.SetTalentTreePartner(talentTreeOne);

        /*
        ModifyCurrentLevel(1);
        SetMaxXP(100);
        ModifyCurrentXP(0);
        ModifyTalentPoints(0);
        */

    }
    public void CreateMyDefenderGameObject()
    {
        List<Tile> possibleSpawnLocations = LevelManager.Instance.GetDefenderSpawnTiles();
        Tile spawnLocation = null;

        if (myClass == "Warrior")
        {
            // Instantiate GO from prefab
            GameObject defenderGO = Instantiate(PrefabHolder.Instance.warriorPrefab, transform.position, Quaternion.identity);
            // Get a reference to the defender script 
            Defender defender = defenderGO.GetComponent<Defender>();
            foreach (Tile tile in possibleSpawnLocations)
            {
                if (tile.IsEmpty && tile.IsWalkable)
                {
                    spawnLocation = tile;
                    break;
                }
            }

            // Run the defender constructor
            defender.myCharacterData = this;
            defender.InitializeSetup(spawnLocation.GridPosition, spawnLocation);
        }

        else if (myClass == "Mage")
        {
            // Instantiate GO from prefab
            GameObject defenderGO = Instantiate(PrefabHolder.Instance.magePrefab, transform.position, Quaternion.identity);
            // Get a reference to the defender script 
            Defender defender = defenderGO.GetComponent<Defender>();
            foreach (Tile tile in possibleSpawnLocations)
            {
                if (tile.IsEmpty && tile.IsWalkable)
                {
                    spawnLocation = tile;
                    break;
                }
            }

            // Run the defender constructor
            defender.myCharacterData = this;
            defender.InitializeSetup(spawnLocation.GridPosition, spawnLocation);
        }

        else if (myClass == "Priest")
        {
            // Instantiate GO from prefab
            GameObject defenderGO = Instantiate(PrefabHolder.Instance.priestPrefab, transform.position, Quaternion.identity);
            // Get a reference to the defender script 
            Defender defender = defenderGO.GetComponent<Defender>();
            foreach (Tile tile in possibleSpawnLocations)
            {
                if (tile.IsEmpty && tile.IsWalkable)
                {
                    spawnLocation = tile;
                    break;
                }
            }

            // Run the defender constructor
            defender.myCharacterData = this;
            defender.InitializeSetup(spawnLocation.GridPosition, spawnLocation);
        }

        else if (myClass == "Ranger")
        {
            // Instantiate GO from prefab
            GameObject defenderGO = Instantiate(PrefabHolder.Instance.rangerPrefab, transform.position, Quaternion.identity);
            // Get a reference to the defender script 
            Defender defender = defenderGO.GetComponent<Defender>();
            foreach (Tile tile in possibleSpawnLocations)
            {
                if (tile.IsEmpty && tile.IsWalkable)
                {
                    spawnLocation = tile;
                    break;
                }
            }

            // Run the defender constructor
            defender.myCharacterData = this;
            defender.InitializeSetup(spawnLocation.GridPosition, spawnLocation);
        }

        else if (myClass == "Rogue")
        {
            // Instantiate GO from prefab
            GameObject defenderGO = Instantiate(PrefabHolder.Instance.roguePrefab, transform.position, Quaternion.identity);
            // Get a reference to the defender script 
            Defender defender = defenderGO.GetComponent<Defender>();
            foreach (Tile tile in possibleSpawnLocations)
            {
                if (tile.IsEmpty && tile.IsWalkable)
                {
                    spawnLocation = tile;
                    break;
                }
            }

            // Run the defender constructor
            defender.myCharacterData = this;
            defender.InitializeSetup(spawnLocation.GridPosition, spawnLocation);
        }

        else if (myClass == "Shaman")
        {
            // Instantiate GO from prefab
            GameObject defenderGO = Instantiate(PrefabHolder.Instance.shamanPrefab, transform.position, Quaternion.identity);
            // Get a reference to the defender script 
            Defender defender = defenderGO.GetComponent<Defender>();
            foreach (Tile tile in possibleSpawnLocations)
            {
                if (tile.IsEmpty && tile.IsWalkable)
                {
                    spawnLocation = tile;
                    break;
                }
            }

            // Run the defender constructor
            defender.myCharacterData = this;
            defender.InitializeSetup(spawnLocation.GridPosition, spawnLocation);
        }
    }
    #endregion

    // Talent, Stats and Ability Pages OnButtonClicked Logic
    #region
    public void OnTalentPageButtonClicked()
    {
        Debug.Log("CharacterData.OnTalentPageButtonClicked() called...");
        CharacterRoster.Instance.DisableInventoryView();
        talentsPageParent.SetActive(true);
        statsPageParent.SetActive(false);
    }
    public void OnStatsPageButtonClicked()
    {
        Debug.Log("CharacterData.OnStatsPageButtonClicked() called...");
        CharacterRoster.Instance.EnableInventoryView();
        statsPageParent.SetActive(true);
        talentsPageParent.SetActive(false);
    }


    #endregion

    // Talent Tree Button 'OnButtonClick' methods
    #region
    public void CloseAllTalentTreePages()
    {
        guardianTreeParent.SetActive(false);
        duelistTreeParent.SetActive(false);
        brawlerTreeParent.SetActive(false);
        assassinationTreeParent.SetActive(false);
        pyromaniaTreeParent.SetActive(false);
        cyromancyTreeParent.SetActive(false);
        rangerTreeParent.SetActive(false);
        manipulationTreeParent.SetActive(false);
        divinityTreeParent.SetActive(false);
        shadowcraftTreeParent.SetActive(false);
        corruptionTreeParent.SetActive(false);
        naturalismTreeParent.SetActive(false);
    }
    public void OnGuardianButtonClicked()
    {
        Debug.Log("CharacterData.OnGuardianButtonClicked() called...");
        CloseAllTalentTreePages();
        guardianTreeParent.SetActive(true);
    }
    public void OnDuelistButtonClicked()
    {
        Debug.Log("CharacterData.OnDuelistButtonClicked() called...");
        CloseAllTalentTreePages();
        duelistTreeParent.SetActive(true);
    }
    public void OnBrawlerButtonClicked()
    {
        Debug.Log("CharacterData.OnBrawlerButtonClicked() called...");
        CloseAllTalentTreePages();
        brawlerTreeParent.SetActive(true);
    }
    public void OnAssassinationButtonClicked()
    {
        Debug.Log("CharacterData.OnAssassinationButtonClicked() called...");
        CloseAllTalentTreePages();
        assassinationTreeParent.SetActive(true);
    }
    public void OnPyromaniaButtonClicked()
    {
        Debug.Log("CharacterData.OnPyromaniaButtonClicked() called...");
        CloseAllTalentTreePages();
        pyromaniaTreeParent.SetActive(true);
    }
    public void OnCyromancyButtonClicked()
    {
        Debug.Log("CharacterData.OnCyromancyButtonClicked() called...");
        CloseAllTalentTreePages();
        cyromancyTreeParent.SetActive(true);
    }
    public void OnRangerButtonClicked()
    {
        Debug.Log("CharacterData.OnRangerButtonClicked() called...");
        CloseAllTalentTreePages();
        rangerTreeParent.SetActive(true);
    }
    public void OnManipulationButtonClicked()
    {
        Debug.Log("CharacterData.OnManipulationButtonClicked() called...");
        CloseAllTalentTreePages();
        manipulationTreeParent.SetActive(true);
    }
    public void OnDivinityButtonClicked()
    {
        Debug.Log("CharacterData.OnDivinityButtonClicked() called...");
        CloseAllTalentTreePages();
        divinityTreeParent.SetActive(true);
    }
    public void OnShadowcraftButtonClicked()
    {
        Debug.Log("CharacterData.OnShadowcraftButtonClicked() called...");
        CloseAllTalentTreePages();
        shadowcraftTreeParent.SetActive(true);
    }
    public void OnCorruptionButtonClicked()
    {
        Debug.Log("CharacterData.OnCorruptionButtonClicked() called...");
        CloseAllTalentTreePages();
        corruptionTreeParent.SetActive(true);
    }
    public void OnNaturalismButtonClicked()
    {
        Debug.Log("CharacterData.OnNaturalismButtonClicked() called...");
        CloseAllTalentTreePages();
        naturalismTreeParent.SetActive(true);
    }

    #endregion

    // Talent 'Plus' Button Logic
    #region
    public bool HasEnoughTalentPoints(int costOfAction)
    {
        if(talentPoints >= costOfAction)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool HasEnoughAbilityPoints(int costOfAction)
    {
        if (abilityPoints >= costOfAction)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void OnTalentPlusButtonClicked(string talentPoolName)
    {
        Debug.Log("CharacterData.OnTalentPlusButtonClicked() called for " + talentPoolName + "...");

        if (HasEnoughTalentPoints(1))
        {
            // Guardian
            if (talentPoolName == "Guardian")
            {
                ModifyGuardianPoints(1);
            }

            // Duelist
            else if (talentPoolName == "Duelist")
            {
                ModifyDuelistPoints(1);
            }

            // Brawler
            else if (talentPoolName == "Brawler")
            {
                ModifyBrawlerPoints(1);
            }

            // Assassination
            else if (talentPoolName == "Assassination")
            {
                ModifyAssassinationPoints(1);
            }

            // Pyromania
            else if (talentPoolName == "Pyromania")
            {
                ModifyPyromaniaPoints(1);
            }

            // Cyromancy
            else if (talentPoolName == "Cyromancy")
            {
                ModifyCyromancyPoints(1);
            }

            // Ranger
            else if (talentPoolName == "Ranger")
            {
                ModifyRangerPoints(1);
            }

            // Manipulation
            else if (talentPoolName == "Manipulation")
            {
                ModifyManipulationPoints(1);
            }

            // Divinity
            else if (talentPoolName == "Divinity")
            {
                ModifyDivinityPoints(1);
            }

            // Shadowcraft
            else if (talentPoolName == "Shadowcraft")
            {
                ModifyShadowcraftPoints(1);
            }

            // Corruption
            else if (talentPoolName == "Corruption")
            {
                ModifyCorruptionPoints(1);
            }

            // Naturalism
            else if (talentPoolName == "Naturalism")
            {
                ModifyNaturalismPoints(1);
            }

            // Pay for the talent
            ModifyTalentPoints(-1);
        }
        
    }
    public void EnableTopBarTalentButton(GameObject talentButton)
    {
        talentButton.SetActive(true);
    }
    #endregion

    // Modify Specific Talent Points Logic
    #region
    public void ModifyGuardianPoints(int pointsGainedOrLost)
    {
        guardianPoints += pointsGainedOrLost;
        guardianPointsText.text = guardianPoints.ToString();

        // Enable button
        if(guardianPoints >= 1)
        {
            EnableTopBarTalentButton(guardianTopBarButton);
        }
    }
    public void ModifyDuelistPoints(int pointsGainedOrLost)
    {
        duelistPoints += pointsGainedOrLost;
        duelistPointsText.text = duelistPoints.ToString();

        // Enable button
        if (duelistPoints >= 1)
        {
            EnableTopBarTalentButton(duelistTopBarButton);
        }
    }
    public void ModifyBrawlerPoints(int pointsGainedOrLost)
    {
        brawlerPoints += pointsGainedOrLost;
        brawlerPointsText.text = brawlerPoints.ToString();

        // Enable button
        if (brawlerPoints >= 1)
        {
            EnableTopBarTalentButton(brawlerTopBarButton);
        }
    }
    public void ModifyAssassinationPoints(int pointsGainedOrLost)
    {
        assassinationPoints += pointsGainedOrLost;
        assassinationPointsText.text = assassinationPoints.ToString();

        // Enable button
        if (assassinationPoints >= 1)
        {
            EnableTopBarTalentButton(assassinationTopBarButton);
        }
    }
    public void ModifyPyromaniaPoints(int pointsGainedOrLost)
    {
        pyromaniaPoints += pointsGainedOrLost;
        pyromaniaPointsText.text = pyromaniaPoints.ToString();

        // Enable button
        if (pyromaniaPoints >= 1)
        {
            EnableTopBarTalentButton(pyromaniaTopBarButton);
        }
    }
    public void ModifyCyromancyPoints(int pointsGainedOrLost)
    {
        cyromancyPoints += pointsGainedOrLost;
        cyromancyPointsText.text = cyromancyPoints.ToString();

        // Enable button
        if (cyromancyPoints >= 1)
        {
            EnableTopBarTalentButton(cyromancyTopBarButton);
        }
    }
    public void ModifyRangerPoints(int pointsGainedOrLost)
    {
        rangerPoints += pointsGainedOrLost;
        rangerPointsText.text = rangerPoints.ToString();

        // Enable button
        if (rangerPoints >= 1)
        {
            EnableTopBarTalentButton(rangerTopBarButton);
        }
    }
    public void ModifyManipulationPoints(int pointsGainedOrLost)
    {
        manipulationPoints += pointsGainedOrLost;
        manipulationPointsText.text = manipulationPoints.ToString();

        // Enable button
        if (manipulationPoints >= 1)
        {
            EnableTopBarTalentButton(manipulationTopBarButton);
        }
    }
    public void ModifyDivinityPoints(int pointsGainedOrLost)
    {
        divinityPoints += pointsGainedOrLost;
        divinityPointsText.text = divinityPoints.ToString();

        // Enable button
        if (divinityPoints >= 1)
        {
            EnableTopBarTalentButton(divinityTopBarButton);
        }
    }
    public void ModifyShadowcraftPoints(int pointsGainedOrLost)
    {
        shadowcraftPoints += pointsGainedOrLost;
        shadowcraftPointsText.text = shadowcraftPoints.ToString();

        // Enable button
        if (shadowcraftPoints >= 1)
        {
            EnableTopBarTalentButton(shadowcraftTopBarButton);
        }
    }
    public void ModifyCorruptionPoints(int pointsGainedOrLost)
    {
        corruptionPoints += pointsGainedOrLost;
        corruptionPointsText.text = corruptionPoints.ToString();

        // Enable button
        if (corruptionPoints >= 1)
        {
            EnableTopBarTalentButton(corruptionTopBarButton);
        }
    }
    public void ModifyNaturalismPoints(int pointsGainedOrLost)
    {
        naturalismPoints += pointsGainedOrLost;
        naturalismPointsText.text = naturalismPoints.ToString();

        // Enable button
        if (naturalismPoints >= 1)
        {
            EnableTopBarTalentButton(naturalismTopBarButton);
        }
    }
    #endregion

    // Modify Primary Stats + Text Values
    #region    
    public void ModifyStrength(int strengthGainedOrLost)
    {
        strength += strengthGainedOrLost;
        strengthText.text = strength.ToString();
    }
    public void ModifyWisdom(int wisdomGainedOrLost)
    {
        wisdom += wisdomGainedOrLost;
        wisdomText.text = wisdom.ToString();
    }
    public void ModifyDexterity(int dexterityGainedOrLost)
    {
        dexterity += dexterityGainedOrLost;
        dexterityText.text = dexterity.ToString();
    }
    public void ModifyMobility(int mobilityGainedOrLost)
    {
        mobility += mobilityGainedOrLost;
        mobilityText.text = mobility.ToString();
    }
    public void ModifyInitiative(int initiativeGainedOrLost)
    {
        initiative += initiativeGainedOrLost;
        initiativeText.text = initiative.ToString();
    }    
    public void ModifyStamina(int energyGainedOrlost)
    {
        stamina += energyGainedOrlost;
        staminaText.text = stamina.ToString();
    }
    #endregion

    // Modify Secondary Stats + Text Values
    #region
    public void ModifyCriticalChance(int criticalGainedOrLost)
    {
        criticalChance += criticalGainedOrLost;
        criticalText.text = criticalChance.ToString();
    }
    public void ModifyDodge(int dodgeGainedOrLost)
    {
        dodge += dodgeGainedOrLost;
        dodgeText.text = dodge.ToString();
    }
    public void ModifyParry(int parryGainedOrLost)
    {
        parry += parryGainedOrLost;
        parryText.text = parry.ToString();
    }
    public void ModifyAuraSize(int auraGainedOrLost)
    {
        auraSize += auraGainedOrLost;
        auraSizeText.text = auraSize.ToString();
    }
    public void ModifyMaxEnergy(int maxEnergyGainedOrlost)
    {
        maxEnergy += maxEnergyGainedOrlost;
        maxEnergyText.text = maxEnergy.ToString();
    }   
    public void ModifyMeleeRange(int meleeRangeGainedOrLost)
    {
        meleeRange += meleeRangeGainedOrLost;
        meleeRangeText.text = meleeRange.ToString();
    }


    #endregion

    // Modify Resistance Stats + Text Values
    #region
    public void ModifyPhysicalResistance(int resistanceGainedOrLost)
    {
        physicalResistance += resistanceGainedOrLost;
        physicalResistanceText.text = physicalResistance.ToString();
    }
    public void ModifyFireResistance(int resistanceGainedOrLost)
    {
        fireResistance += resistanceGainedOrLost;
        fireResistanceText.text = fireResistance.ToString();
    }
    public void ModifyFrostResistance(int resistanceGainedOrLost)
    {
        frostResistance += resistanceGainedOrLost;
        frostResistanceText.text = frostResistance.ToString();
    }
    public void ModifyPoisonResistance(int resistanceGainedOrLost)
    {
        poisonResistance += resistanceGainedOrLost;
        poisonResistanceText.text = poisonResistance.ToString();
    }
    public void ModifyAirResistance(int resistanceGainedOrLost)
    {
        airResistance += resistanceGainedOrLost;
        airResistanceText.text = airResistance.ToString();
    }
    public void ModifyShadowResistance(int resistanceGainedOrLost)
    {
        shadowResistance += resistanceGainedOrLost;
        shadowResistanceText.text = shadowResistance.ToString();
    }

    #endregion

    // Modify Health
    #region
    public void SetCurrentHealth(int newValue)
    {
        currentHealth = newValue;

        // prevent healing past max HP
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Prevent damaging into negative health
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        //panelCurrentHealthText.text = CurrentHealth.ToString();
        currentHealthText.text = currentHealth.ToString();

    }
    public void ModifyCurrentHealth(int healthGainedOrLost)
    {
        currentHealth += healthGainedOrLost;

        // prevent healing past max HP
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Prevent damaging into negative health
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        //panelCurrentHealthText.text = CurrentHealth.ToString();
        currentHealthText.text = currentHealth.ToString();
        if (myCampSiteCharacter != null)
        {
            myCampSiteCharacter.ModifyCurrentHealthText(currentHealth);
        }
        if (myStoryWindowCharacter != null)
        {
            myStoryWindowCharacter.ModifyCurrentHealthText(currentHealth);
        }
    }
    public void ModifyMaxHealth(int maxHealthGainedOrLost)
    {
        maxHealth += maxHealthGainedOrLost;
        //panelMaxHealthText.text = MaxHealth.ToString();
        maxHealthText.text = maxHealth.ToString();

        if (currentHealth > maxHealth)
        {
            ModifyCurrentHealth(-(currentHealth - maxHealth));
        }
        if (myCampSiteCharacter != null)
        {
            myCampSiteCharacter.ModifyMaxHealthText(maxHealth);
        }
        if (myStoryWindowCharacter != null)
        {
            myStoryWindowCharacter.ModifyMaxHealthText(maxHealth);
        }
    }

    #endregion    

    // Modify Passive Traits
    #region
    public void ModifyTenacious(int stacks)
    {
        Debug.Log("CharacterData.ModifyTenacious() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        tenaciousStacks += stacks;
    }
    public void ModifyMasochist(int stacks)
    {
        Debug.Log("CharacterData.ModifyMasochist() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        tenaciousStacks += stacks;
    }
    public void ModifyLastStand(int stacks)
    {
        Debug.Log("CharacterData.ModifyLastStand() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        lastStandStacks += stacks;
    }
    public void ModifySlippery(int stacks)
    {
        Debug.Log("CharacterData.ModifySlippery() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        slipperyStacks += stacks;
    }
    public void ModifyRiposte(int stacks)
    {
        Debug.Log("CharacterData.ModifyRiposte() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        riposteStacks += stacks;
    }
    public void ModifyPerfectReflexes(int stacks)
    {
        Debug.Log("CharacterData.ModifyPerfectReflexes() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        perfectReflexesStacks += stacks;
    }
    public void ModifyOpportunist(int stacks)
    {
        Debug.Log("CharacterData.ModifyOpportunist() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        opportunistStacks += stacks;
    }
    public void ModifyPatientStalker(int stacks)
    {
        Debug.Log("CharacterData.ModifyPatientStalker() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        patientStalkerStacks += stacks;
    }
    public void ModifyStealth(int stacks)
    {
        Debug.Log("CharacterData.ModifyStealth() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        stealthStacks += stacks;
    }
    public void ModifyCautious(int stacks)
    {
        Debug.Log("CharacterData.ModifyCautious() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        cautiousStacks += stacks;
    }
    public void ModifyGuardianAura(int stacks)
    {
        Debug.Log("CharacterData.ModifyGuardianAura() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        guardianAuraStacks += stacks;
    }
    public void ModifyUnwavering(int stacks)
    {
        Debug.Log("CharacterData.ModifyUnwavering() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        unwaveringStacks += stacks;
    }
    public void ModifyFieryAura(int stacks)
    {
        Debug.Log("CharacterData.ModifyFieryAura() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        fieryAuraStacks += stacks;
    }
    public void ModifyImmolation(int stacks)
    {
        Debug.Log("CharacterData.ModifyImmolation() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        immolationStacks += stacks;
    }
    public void ModifyDemon(int stacks)
    {
        Debug.Log("CharacterData.ModifyDemon() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        demonStacks += stacks;
    }
    public void ModifyShatter(int stacks)
    {
        Debug.Log("CharacterData.ModifyShatter() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        shatterStacks += stacks;
    }
    public void ModifyFrozenHeart(int stacks)
    {
        Debug.Log("CharacterData.ModifyFrozenHeart() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        frozenHeartStacks += stacks;
    }
    public void ModifyPredator(int stacks)
    {
        Debug.Log("CharacterData.ModifyPredator() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        predatorStacks += stacks;
    }
    public void ModifyHawkEye(int stacks)
    {
        Debug.Log("CharacterData.ModifyHawkEye() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        hawkEyeStacks += stacks;
    }
    public void ModifyFlux(int stacks)
    {
        Debug.Log("CharacterData.ModifyFlux() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        fluxStacks += stacks;
    }
    public void ModifyPhasing(int stacks)
    {
        Debug.Log("CharacterData.ModifyPhasing() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        phasingStacks += stacks;
    }
    public void ModifyEtherealBeing(int stacks)
    {
        Debug.Log("CharacterData.ModifyEtherealBeing() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        etherealBeingStacks += stacks;
    }
    public void ModifyEncouragingAura(int stacks)
    {
        Debug.Log("CharacterData.ModifyEncouragingAura() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        encouragingAuraStacks += stacks;
    }
    public void ModifyRadiance(int stacks)
    {
        Debug.Log("CharacterData.ModifyRadiance() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        radianceStacks += stacks;
    }
    public void ModifySacredAura(int stacks)
    {
        Debug.Log("CharacterData.ModifySacredAura() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        sacredAuraStacks += stacks;
    }
    public void ModifyShadowAura(int stacks)
    {
        Debug.Log("CharacterData.ModifyShadowAura() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        shadowAuraStacks += stacks;
    }
    public void ModifyShadowForm(int stacks)
    {
        Debug.Log("CharacterData.ModifyShadowForm() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        shadowFormStacks += stacks;
    }
    public void ModifyPoisonous(int stacks)
    {
        Debug.Log("CharacterData.ModifyPoisonous() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        poisonousStacks += stacks;
    }
    public void ModifyVenomous(int stacks)
    {
        Debug.Log("CharacterData.ModifyVenomous() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        venomousStacks += stacks;
    }
    public void ModifyToxicity(int stacks)
    {
        Debug.Log("CharacterData.ModifyToxicity() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        toxicityStacks += stacks;
    }
    public void ModifyToxicAura(int stacks)
    {
        Debug.Log("CharacterData.ModifyToxicAura() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        toxicAuraStacks += stacks;
    }
    public void ModifyStormAura(int stacks)
    {
        Debug.Log("CharacterData.ModifyStormAura() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        stormAuraStacks += stacks;
    }
    public void ModifyStormLord(int stacks)
    {
        Debug.Log("CharacterData.ModifyStormLord() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        stormLordStacks += stacks;
    }
    public void ModifyThorns(int stacks)
    {
        Debug.Log("CharacterData.ModifyThorns() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        thornsStacks += stacks;
    }
          

    #endregion

    // Modify Level / XP 
    #region
    public void SetMaxXP(int newValue)
    {
        currentMaxXP = newValue;
        maxXpText.text = currentMaxXP.ToString();
    }
    public void ModifyCurrentLevel(int levelsGainedOrLost)
    {
        currentLevel += levelsGainedOrLost;
        levelText.text = currentLevel.ToString();
    }
    public void ModifyCurrentXP(int xpGainedOrLost)
    {
        currentXP += xpGainedOrLost;
        if (currentXP > currentMaxXP)
        {
            currentXP = currentXP - currentMaxXP;
            ModifyCurrentLevel(1);
            ModifyTalentPoints(1);

        }
        else if(currentXP == currentMaxXP)
        {
            currentXP = 0;
            ModifyCurrentLevel(1);
            ModifyTalentPoints(1);
        }

        xpText.text = currentXP.ToString();
        if(myCampSiteCharacter != null)
        {
            myCampSiteCharacter.ModifyCurrentXPText(currentXP);
        }
        if (myStoryWindowCharacter != null)
        {
            myStoryWindowCharacter.ModifyCurrentXPText(currentXP);
        }
    }

    #endregion

    // Modify Talent and Ability Points
    #region
    public void ModifyTalentPoints(int talentPointsGainedOrLost)
    {
        talentPoints += talentPointsGainedOrLost;
        talentPointsText.text = talentPoints.ToString();
    }
    public void ModifyAbilityPoints(int abilityPointsGainedOrLost)
    {
        abilityPoints += abilityPointsGainedOrLost;
        abilityPointsText.text = abilityPoints.ToString();
    }
    #endregion

    // Naming + Identity Logic
    #region
    public void SetMyName(string newName)
    {
        myName = newName;
        myNameText.text = newName;
    }
    #endregion


    
}
