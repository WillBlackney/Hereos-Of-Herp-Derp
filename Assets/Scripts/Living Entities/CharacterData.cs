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
    public GameObject attributeTabContentParent;
    public TextMeshProUGUI myNameText;
    public UniversalCharacterModel myCharacterModel;
    public List<Talent> allTalentButtons;

    [Header("Ability References")]
    public List<AbilitySlot> allKnownAbilitySlots;
    public List<AbilitySlot> allActiveAbilitySlots;
    public List<AbilityPageAbility> activeAbilities;
    public List<AbilityPageAbility> knownAbilities;

    [Header("Page Button References")]
    public GameObject statsPageButton;
    public GameObject talentsPageButton;
    public GameObject abilityPageButton;

    [Header("Health Text References")]
    public TextMeshProUGUI currentHealthText;
    public TextMeshProUGUI maxHealthText;
    public Slider healthBar;

    [Header("XP + Level Text References")]
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI maxXpText;
    public TextMeshProUGUI levelText;
    public Slider xpBar;

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

    [Header("Weapon Properties")]
    public ItemDataSO mainHandWeapon;
    public ItemDataSO offHandWeapon;
    public CharacterItemSlot mainHandSlot;
    public CharacterItemSlot offHandSlot;

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
    public int perfectAimStacks;
    public int virtuosoStacks;
    public int slipperyStacks;
    public int riposteStacks;
    public int trueSightStacks;
    public int perfectReflexesStacks;
    public int opportunistStacks;
    public int patientStalkerStacks;
    public int stealthStacks;
    public int cautiousStacks;
    public int guardianAuraStacks;
    public int unwaveringStacks;
    public int unstoppableStacks;
    public int fieryAuraStacks;
    public int immolationStacks;
    public int demonStacks;
    public int shatterStacks;
    public int frozenHeartStacks;
    public int predatorStacks;
    public int hawkEyeStacks;
    public int fluxStacks;
    public int coupDeGraceStacks;
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
    public int enrageStacks;
    public int powerLimitStacks;
    public int quickDrawStacks;
    public int fadingStacks;
    public int lifeStealStacks;



    [Header("Misc Properties")]
    public string myName;
    public List<AttributeTab> myAttributeTabs;
    #endregion

    // Enable + Disable All My Views
    #region
    public void DisableMainWindowView()
    {
        masterVisualParent.SetActive(false);
    }
    public void EnableMainWindowView()
    {
        masterVisualParent.SetActive(true);
        CharacterRoster.Instance.DisableInventoryView();
        myCharacterModel.SetIdleAnim();

        // set stats page as defualt view 
        OnStatsPageButtonClicked();
    }
    #endregion

    // Legacy properties
    #region

    [Header("Legacy Components")]
    public GameObject inventoryItemParent;
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
    public void InitializeSetupFromPresetString(string characterClass)
    {
        // Learn Move
        HandleLearnAbility(AbilityLibrary.Instance.GetAbilityByName("Move"));

        // Preset specific set up (weapons, abilities, passives, talents, etc)
        if (characterClass == "Knight")
        {
            SetUpAsKnightPreset();
        }
        else if (characterClass == "Paladin")
        {
            SetUpAsPaladinPreset();
        }
        else if (characterClass == "Berserker")
        {
            SetUpAsBerserkerPreset();
        }
        else if (characterClass == "Battle Mage")
        {
            SetUpAsBattleMagePreset();
        }
        else if (characterClass == "Arcanist")
        {
            SetUpAsArcanistPreset();
        }
        else if (characterClass == "Shadow Blade")
        {
            SetUpAsShadowBladePreset();
        }
        else if (characterClass == "Rogue")
        {
            SetUpAsRoguePreset();
        }
        else if (characterClass == "Priest")
        {
            SetUpAsPriestPreset();
        }
        else if (characterClass == "Monk")
        {
            SetUpAsMonkPreset();
        }
        else if (characterClass == "Wayfarer")
        {
            SetUpAsWayfarerPreset();
        }
        else if (characterClass == "Marksman")
        {
            SetUpAsMarksmanPreset();
        }
        else if (characterClass == "Warlock")
        {
            SetUpAsWarlockPreset();
        }
        else if (characterClass == "Alchemist")
        {
            SetUpAsAlchemistPreset();            
        }
        else if (characterClass == "Death Knight")
        {
            SetUpAsDeathKnightPreset();
        }
        else if (characterClass == "Illusionist")
        {
            SetUpAsIllusionistPreset();
        }
        else if (characterClass == "Shaman")
        {
            SetUpAsShamanPreset();
        }
        else if (characterClass == "Frost Knight")
        {
            SetUpAsFrostKnightPreset();
        }
        else if (characterClass == "Bulwark")
        {
            SetUpAsBulwarkPreset();
        }



        // Set up health
        ModifyMaxHealth(100);
        ModifyCurrentHealth(100);

        // Set up core stats
        ModifyStrength(0);
        ModifyWisdom(0);
        ModifyDexterity(0);
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
        ModifyPhysicalResistance(0);
        ModifyFireResistance(0);
        ModifyFrostResistance(0);
        ModifyPoisonResistance(0);
        ModifyAirResistance(0);
        ModifyShadowResistance(0);

        // Set up Talent + Ability Points
        ModifyTalentPoints(0);
        ModifyAbilityPoints(0);

        // Set up Xp + Max Xp and Leve
        ModifyCurrentLevel(1);
        SetMaxXP(100);
        ModifyCurrentXP(0);

        // Misc Passives + Extras
        ModifyPowerLimit(1);

        // Set default talent page
        SetDefaultTalentPageView();

        // Play idle anim on model
        myCharacterModel.SetIdleAnim();

    }
    public void CreateMyDefenderGameObject()
    {
        // Set up
        List<Tile> possibleSpawnLocations = LevelManager.Instance.GetDefenderSpawnTiles();
        Tile spawnLocation = null;

        // Instantiate Defender GO from prefab, get defender script ref
        GameObject defenderGO = Instantiate(PrefabHolder.Instance.defenderPrefab, transform.position, Quaternion.identity);
        Defender defender = defenderGO.GetComponent<Defender>();

        // calculate spawn location
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
    public void SetDefaultTalentPageView()
    {
        if(guardianPoints > 0)
        {
            guardianTreeParent.SetActive(true);
        }
        else if (duelistPoints > 0)
        {
            duelistTreeParent.SetActive(true);
        }
        else if (brawlerPoints > 0)
        {
            brawlerTreeParent.SetActive(true);
        }
        else if (assassinationPoints > 0)
        {
            assassinationTreeParent.SetActive(true);
        }
        else if (pyromaniaPoints > 0)
        {
            pyromaniaTreeParent.SetActive(true);
        }
        else if (cyromancyPoints > 0)
        {
            cyromancyTreeParent.SetActive(true);
        }
        else if (rangerPoints > 0)
        {
            rangerTreeParent.SetActive(true);
        }
        else if (manipulationPoints > 0)
        {
            manipulationTreeParent.SetActive(true);
        }
        else if (divinityPoints > 0)
        {
            divinityTreeParent.SetActive(true);
        }
        else if (shadowcraftPoints > 0)
        {
            shadowcraftTreeParent.SetActive(true);
        }
        else if (corruptionPoints > 0)
        {
            corruptionTreeParent.SetActive(true);
        }
        else if (naturalismPoints > 0)
        {
            naturalismTreeParent.SetActive(true);
        }

    }
    #endregion

    // Specific Preset set ups
    #region
    public void SetUpAsKnightPreset()
    {
        myClass = "Knight";

        // Modify Attributes
        ModifyGuardianPoints(2);
        ModifyDuelistPoints(1);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Sword"), mainHandSlot);
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Shield"), offHandSlot);

        // Learn abilities + passive            
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Shield Slam"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Testudo"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Dash"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Cautious"), false);

        // Set up character view model
        CharacterModelController.SetUpAsKnightPreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsStrike = true;
        KnowsDefend = true;
    }
    public void SetUpAsPaladinPreset()
    {
        myClass = "Paladin";

        // Modify Attributes
        ModifyGuardianPoints(1);
        ModifyDivinityPoints(2);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Battle Axe"), mainHandSlot);

        // Learn abilities + passive
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Charge"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Invigorate"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Inspire"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Encouraging Aura"), false);

        // Set up character view model
        CharacterModelController.SetUpAsPaladinPreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsStrike = true;
        KnowsDefend = true;
    }
    public void SetUpAsBerserkerPreset()
    {
        myClass = "Berserker";

        // Modify Attributes
        ModifyCorruptionPoints(1);
        ModifyBrawlerPoints(2);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Battle Axe"), mainHandSlot);

        // Learn abilities + passive
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Charge"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Smash"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Blood Offering"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Tenacious"), false);

        // Set up character view model
        CharacterModelController.SetUpAsBerserkerPreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsStrike = true;
    }
    public void SetUpAsBattleMagePreset()
    {
        myClass = "Battle Mage";

        // Modify Attributes
        ModifyBrawlerPoints(1);
        ModifyPyromaniaPoints(2);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Battle Axe"), mainHandSlot);

        // Learn abilities + passive
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Phoenix Dive"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Whirlwind"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Fire Nova"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Fiery Aura"), false);

        // Set up character view model
        CharacterModelController.SetUpAsBattleMagePreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsStrike = true;
        KnowsDefend = true;
    }
    public void SetUpAsArcanistPreset()
    {
        myClass = "Arcanist";

        // Modify Attributes
        ModifyManipulationPoints(1);
        ModifyPyromaniaPoints(1);
        ModifyCyromancyPoints(1);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Staff"), mainHandSlot);

        // Learn abilities + passive
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Telekinesis"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Fire Ball"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Icy Focus"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Flux"), false);

        // Set up character view model
        CharacterModelController.SetUpAsArcanistPreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsStrike = true;
    }
    public void SetUpAsShadowBladePreset()
    {
        myClass = "Shadow Blade";

        // Modify Attributes
        ModifyAssassinationPoints(3);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Dagger"), mainHandSlot);
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Dagger"), offHandSlot);

        // Learn abilities + passive
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Shadow Step"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Vanish"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Cheap Shot"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Predator"), false);

        // Set up character view model
        CharacterModelController.SetUpAsShadowBladePreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsStrike = true;
    }
    public void SetUpAsRoguePreset()
    {
        myClass = "Rogue";

        // Modify Attributes
        ModifyAssassinationPoints(1);
        ModifyDuelistPoints(1);
        ModifyCorruptionPoints(1);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Sword"), mainHandSlot);
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Sword"), offHandSlot);

        // Learn abilities + passive
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Blood Offering"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Dash"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Shank"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Poisonous"), false);

        // Set up character view model
        CharacterModelController.SetUpAsRoguePreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsStrike = true;
    }
    public void SetUpAsPriestPreset()
    {
        myClass = "Priest";

        // Modify Attributes
        ModifyDivinityPoints(3);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Staff"), mainHandSlot);

        // Learn abilities + passive
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Holy Fire"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Invigorate"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Transcendence"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Encouraging Aura"), false);

        // Set up character view model
        CharacterModelController.SetUpAsPriestPreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsStrike = true;
    }
    public void SetUpAsMonkPreset()
    {
        myClass = "Monk";

        // Modify Attributes
        ModifyBrawlerPoints(1);
        ModifyDuelistPoints(1);
        ModifyDivinityPoints(1);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Staff"), mainHandSlot);

        // Learn abilities + passive
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Consecrate"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Whirlwind"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Dash"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Riposte"), false);

        // Set up character view model
        CharacterModelController.SetUpAsMonkPreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsStrike = true;
    }
    public void SetUpAsWayfarerPreset()
    {
        myClass = "Wayfarer";

        // Modify Attributes
        ModifyManipulationPoints(1);
        ModifyRangerPoints(1);
        ModifyNaturalismPoints(1);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Bow"), mainHandSlot);

        // Learn abilities + passive
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Haste"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Spirit Surge"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Telekinesis"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Quick Draw"), false);

        // Set up character view model
        CharacterModelController.SetUpAsWayfarerPreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsShoot = true;
    }
    public void SetUpAsMarksmanPreset()
    {
        myClass = "Marksman";

        // Modify Attributes
        ModifyAssassinationPoints(1);
        ModifyRangerPoints(2);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Bow"), mainHandSlot);

        // Learn abilities + passive
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Vanish"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Impaling Bolt"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Head Shot"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Predator"), false);

        // Set up character view model
        CharacterModelController.SetUpAsMarksmanPreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsShoot = true;
    }
    public void SetUpAsWarlockPreset()
    {
        myClass = "Warlock";

        // Modify Attributes
        ModifyShadowcraftPoints(2);
        ModifyManipulationPoints(1);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Staff"), mainHandSlot);

        // Learn abilities + passive
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Chaos Bolt"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Shadow Blast"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Hex"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Flux"), false);

        // Set up character view model
        CharacterModelController.SetUpAsWarlockPreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsStrike = true;
    }
    public void SetUpAsAlchemistPreset()
    {
        myClass = "Alchemist";

        // Modify Attributes
        ModifyCorruptionPoints(3);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Staff"), mainHandSlot);

        // Learn abilities + passive
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Blight"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Chemical Reaction"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Drain"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Venomous"), false);

        // Set up character view model
        CharacterModelController.SetUpAsAlchemistPreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsStrike = true;
    }
    public void SetUpAsIllusionistPreset()
    {
        myClass = "Illusionist";

        // Modify Attributes
        ModifyManipulationPoints(2);
        ModifyShadowcraftPoints(1);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Bow"), mainHandSlot);

        // Learn abilities + passive
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Phase Shift"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Shadow Blast"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Dimensional Blast"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Flux"), false);

        // Set up character view model
        CharacterModelController.SetUpAsIllusionistPreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsStrike = true;
    }
    public void SetUpAsFrostKnightPreset()
    {
        myClass = "Frost Knight";

        // Modify Attributes
        ModifyCyromancyPoints(3);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Battle Axe"), mainHandSlot);

        // Learn abilities + passive
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Frost Bolt"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Chilling Blow"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Frost Nova"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Shatter"), false);

        // Set up character view model
        CharacterModelController.SetUpAsFrostKnightPreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsStrike = true;
    }
    public void SetUpAsShamanPreset()
    {
        myClass = "Shaman";

        // Modify Attributes
        ModifyNaturalismPoints(2);
        ModifyManipulationPoints(1);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Battle Axe"), mainHandSlot);

        // Learn abilities + passive
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Chain Lightning"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Thunder Strike"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Spirit Surge"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Flux"), false);

        // Set up character view model
        CharacterModelController.SetUpAsShamanPreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsStrike = true;
    }
    public void SetUpAsDeathKnightPreset()
    {
        myClass = "Death Knight";

        // Modify Attributes
        ModifyBrawlerPoints(2);
        ModifyShadowcraftPoints(1);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Battle Axe"), mainHandSlot);

        // Learn abilities + passive
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Charge"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Smash"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Dark Gift"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Shadow Aura"), false);

        // Set up character view model
        CharacterModelController.SetUpAsDeathKnightPreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsStrike = true;
    }
    public void SetUpAsBulwarkPreset()
    {
        myClass = "Bulwark";

        // Modify Attributes
        ModifyGuardianPoints(3);

        // Assign preset weapons
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Sword"), mainHandSlot);
        InventoryController.Instance.CreateAndAddItemDirectlyToCharacter(ItemLibrary.Instance.GetItemByName("Simple Shield"), offHandSlot);

        // Learn abilities + passive
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Provoke"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Get Down!"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Fortify"), false);
        TalentController.Instance.PurchaseTalent(this, TalentController.Instance.GetTalentByName(this, "Guardian Aura"), false);

        // Set up character view model
        CharacterModelController.SetUpAsBulwarkPreset(myCharacterModel);

        // Set up base abilities
        KnowsMove = true;
        KnowsStrike = true;
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
        abilityPageParent.SetActive(false);
    }
    public void OnStatsPageButtonClicked()
    {
        Debug.Log("CharacterData.OnStatsPageButtonClicked() called...");
        CharacterRoster.Instance.EnableInventoryView();
        statsPageParent.SetActive(true);
        myCharacterModel.SetIdleAnim();
        talentsPageParent.SetActive(false);
        abilityPageParent.SetActive(false);
    }
    public void OnAbilitiesPageButtonClicked()
    {
        Debug.Log("CharacterData.OnAbilitiesPageButtonClicked() called...");
        CharacterRoster.Instance.EnableInventoryView();
        abilityPageParent.SetActive(true);
        statsPageParent.SetActive(false);
        talentsPageParent.SetActive(false);
    }
    #endregion

    // Ability Page Logic
    #region
    public AbilityPageAbility CreateNewAbilityPageAbilityTab(AbilityDataSO data, AbilitySlot parent)
    {
        GameObject abilityTabGO = Instantiate(PrefabHolder.Instance.abilityPageAbility, parent.transform);
        AbilityPageAbility abilityScript = abilityTabGO.GetComponent<AbilityPageAbility>();
        abilityScript.InitializeSetup(data);

        abilityScript.myCharacter = this;
        abilityScript.myCurrentSlot = parent;
        parent.occupied = true;

        if (parent.activeAbilityBarSlot)
        {
            abilityScript.onAbilityBar = true;
        }

        return abilityScript;

    }
    public bool DoesCharacterAlreadyKnowAbility(AbilityDataSO ability)
    {
        bool boolReturned = false;

        foreach(AbilityPageAbility knownAbility in knownAbilities)
        {
            if(ability.abilityName == knownAbility.myData.abilityName)
            {
                boolReturned = true;
                break;
            }
        }

        return boolReturned;
    }
    public bool DoesCharacterMeetAbilityTierRequirment(AbilityDataSO ability)
    {
        Debug.Log("TalentController.DoesCharacterMeetTalentTierRequirment() called...");

        if (ability.abilitySchool == AbilityDataSO.AbilitySchool.Guardian &&
            guardianPoints >= ability.tier)
        {
            Debug.Log(myName + " meets the ability tier requirments of " + ability.abilityName);
            return true;
        }
        else if (ability.abilitySchool == AbilityDataSO.AbilitySchool.Duelist &&
            duelistPoints >= ability.tier)
        {
            Debug.Log(myName + " meets the ability tier requirments of " + ability.abilityName);
            return true;
        }
        else if (ability.abilitySchool == AbilityDataSO.AbilitySchool.Brawler &&
            brawlerPoints >= ability.tier)
        {
            Debug.Log(myName + " meets the ability tier requirments of " + ability.abilityName);
            return true;
        }
        else if (ability.abilitySchool == AbilityDataSO.AbilitySchool.Assassination &&
            assassinationPoints >= ability.tier)
        {
            Debug.Log(myName + " meets the ability tier requirments of " + ability.abilityName);
            return true;
        }
        else if (ability.abilitySchool == AbilityDataSO.AbilitySchool.Pyromania &&
            pyromaniaPoints >= ability.tier)
        {
            Debug.Log(myName + " meets the ability tier requirments of " + ability.abilityName);
            return true;
        }
        else if (ability.abilitySchool == AbilityDataSO.AbilitySchool.Cyromancy &&
            cyromancyPoints >= ability.tier)
        {
            Debug.Log(myName + " meets the ability tier requirments of " + ability.abilityName);
            return true;
        }
        else if (ability.abilitySchool == AbilityDataSO.AbilitySchool.Ranger &&
            rangerPoints >= ability.tier)
        {
            Debug.Log(myName + " meets the ability tier requirments of " + ability.abilityName);
            return true;
        }
        else if (ability.abilitySchool == AbilityDataSO.AbilitySchool.Manipulation &&
            manipulationPoints >= ability.tier)
        {
            Debug.Log(myName + " meets the ability tier requirments of " + ability.abilityName);
            return true;
        }
        else if (ability.abilitySchool == AbilityDataSO.AbilitySchool.Divinity &&
            divinityPoints >= ability.tier)
        {
            Debug.Log(myName + " meets the ability tier requirments of " + ability.abilityName);
            return true;
        }
        else if (ability.abilitySchool == AbilityDataSO.AbilitySchool.Shadowcraft &&
            shadowcraftPoints >= ability.tier)
        {
            Debug.Log(myName + " meets the ability tier requirments of " + ability.abilityName);
            return true;
        }
        else if (ability.abilitySchool == AbilityDataSO.AbilitySchool.Corruption &&
            corruptionPoints >= ability.tier)
        {
            Debug.Log(myName + " meets the ability tier requirments of " + ability.abilityName);
            return true;
        }
        else if (ability.abilitySchool == AbilityDataSO.AbilitySchool.Naturalism &&
            naturalismPoints >= ability.tier)
        {
            Debug.Log(myName + " meets the ability tier requirments of " + ability.abilityName);
            return true;
        }     
        else
        {
            Debug.Log(myName + " does NOT meet the talent tier requirments of " + ability.abilityName);
            return false;
        }
    }
    public bool IsAbilityAlreadyOnActiveBar(AbilityDataSO ability)
    {
        bool boolReturned = false;

        foreach (AbilityPageAbility activeAbility in activeAbilities)
        {
            if (ability.abilityName == activeAbility.myData.abilityName)
            {
                boolReturned = true;
                break;
            }
        }

        return boolReturned;
    }
    public void HandleLearnAbility(AbilityDataSO data)
    {
        Debug.Log("CharacterData.HandeLearnAbility() called, trying to learn " + data.abilityName);

        if (!DoesCharacterAlreadyKnowAbility(data))
        {
            Debug.Log("Character does not already know " + data.abilityName + ", learning...");

            // Add to known abilities list
            knownAbilities.Add(CreateNewAbilityPageAbilityTab(data, GetNextAvailbleKnownAbilitySlot()));

            // Auto add new ability to active ability slot of there is room availble, and ability is not already active
            AbilitySlot abilityBarSlot = GetNextAvailableAbilityBarSlot();

            if (abilityBarSlot != null && !IsAbilityAlreadyOnActiveBar(data))
            {
                activeAbilities.Add(CreateNewAbilityPageAbilityTab(data, abilityBarSlot));                
            }
        }
    }
    public void HandleUnlearnAbility(AbilityDataSO data)
    {
        Debug.Log("CharacterData.HandeUnlearnAbility() called, trying to unlearn " + data.abilityName);

        if (DoesCharacterAlreadyKnowAbility(data))
        {
            Debug.Log("Character already knows " + data.abilityName + ", unlearning...");

            // Remove from  know abilities first, then active abilities after
            RemoveAbilityFromKnownAbilitiesList(data);

            // Remove from active abilities
            RemoveAbilityFromActiveAbilityBar(data);
        }
    }
    public AbilitySlot GetNextAvailbleKnownAbilitySlot()
    {
        Debug.Log("CharacterData.GetNextAvailbleKnownAbilitySlot() called...");
        AbilitySlot slotReturned = null;

        foreach(AbilitySlot slot in allKnownAbilitySlots)
        {
            if(slot.occupied == false)
            {
                slotReturned = slot;
                break;
            }
        }

        if(slotReturned == null)
        {
            Debug.Log("CharacterData.GetNextAvailbleKnownAbilitySlot() could not find any availble slots");
        }

        return slotReturned;

        
    }
    public AbilitySlot GetNextAvailableAbilityBarSlot()
    {
        Debug.Log("CharacterData.GetNextAvailableAbilityBarSlot() called...");
        AbilitySlot slotReturned = null;

        foreach (AbilitySlot slot in allActiveAbilitySlots)
        {
            if (slot.occupied == false)
            {
                slotReturned = slot;
                break;
            }
        }

        if (slotReturned == null)
        {
            Debug.Log("CharacterData.GetNextAvailableAbilityBarSlot() could not find any availble slots");
        }

        return slotReturned;
    }
    public void RemoveAbilityFromActiveAbilityBar(AbilityDataSO data)
    {
        // Remove from active abilities
        AbilityPageAbility activeAbilityToRemove = null;
        foreach (AbilityPageAbility apa in activeAbilities)
        {
            if (apa.myData.abilityName == data.abilityName)
            {
                activeAbilityToRemove = apa;
            }
        }

        activeAbilities.Remove(activeAbilityToRemove);
        // open up slot
        activeAbilityToRemove.myCurrentSlot.occupied = false;
        Destroy(activeAbilityToRemove.gameObject);
    }
    public void RemoveAbilityFromKnownAbilitiesList(AbilityDataSO data)
    {
        AbilityPageAbility abilityToRemove = null;
        foreach (AbilityPageAbility apa in knownAbilities)
        {
            if (apa.myData.abilityName == data.abilityName)
            {
                abilityToRemove = apa;
            }
        }

        knownAbilities.Remove(abilityToRemove);
        // open up slot
        abilityToRemove.myCurrentSlot.occupied = false;
        Destroy(abilityToRemove.gameObject);
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

        TalentController.Instance.RefreshAllTalentButtonViewStates(this);
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
        TalentController.Instance.RefreshAllTalentButtonViewStates(this);
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

        TalentController.Instance.RefreshAllTalentButtonViewStates(this);
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

        TalentController.Instance.RefreshAllTalentButtonViewStates(this);
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

        TalentController.Instance.RefreshAllTalentButtonViewStates(this);
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

        TalentController.Instance.RefreshAllTalentButtonViewStates(this);
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

        TalentController.Instance.RefreshAllTalentButtonViewStates(this);
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

        TalentController.Instance.RefreshAllTalentButtonViewStates(this);
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

        TalentController.Instance.RefreshAllTalentButtonViewStates(this);
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

        TalentController.Instance.RefreshAllTalentButtonViewStates(this);
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

        TalentController.Instance.RefreshAllTalentButtonViewStates(this);
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

        TalentController.Instance.RefreshAllTalentButtonViewStates(this);
    }
    #endregion

    // Modify Primary Stats + Text Values
    #region    
    public void ModifyStrength(int strengthGainedOrLost)
    {
        Debug.Log("CharacterData.ModifyStrength() called, modifying by " + strengthGainedOrLost.ToString());
        strength += strengthGainedOrLost;
        strengthText.text = strength.ToString();
    }
    public void ModifyWisdom(int wisdomGainedOrLost)
    {
        Debug.Log("CharacterData.ModifyWisdom() called, modifying by " + wisdomGainedOrLost.ToString());
        wisdom += wisdomGainedOrLost;
        wisdomText.text = wisdom.ToString();
    }
    public void ModifyDexterity(int dexterityGainedOrLost)
    {
        Debug.Log("CharacterData.ModifyDexterity() called, modifying by " + dexterityGainedOrLost.ToString());
        dexterity += dexterityGainedOrLost;
        dexterityText.text = dexterity.ToString();
    }
    public void ModifyMobility(int mobilityGainedOrLost)
    {
        Debug.Log("CharacterData.ModifyMobility() called, modifying by " + mobilityGainedOrLost.ToString());
        mobility += mobilityGainedOrLost;
        mobilityText.text = mobility.ToString();
    }
    public void ModifyInitiative(int initiativeGainedOrLost)
    {
        Debug.Log("CharacterData.ModifyInitiative() called, modifying by " + initiativeGainedOrLost.ToString());
        initiative += initiativeGainedOrLost;
        initiativeText.text = initiative.ToString();
    }    
    public void ModifyStamina(int energyGainedOrlost)
    {
        Debug.Log("CharacterData.ModifyStamina() called, modifying by " + energyGainedOrlost.ToString());
        stamina += energyGainedOrlost;
        staminaText.text = stamina.ToString();
    }
    #endregion

    // Modify Secondary Stats + Text Values
    #region
    public void ModifyCriticalChance(int criticalGainedOrLost)
    {
        Debug.Log("CharacterData.ModifyCriticalChance() called, modifying by " + criticalGainedOrLost.ToString());
        criticalChance += criticalGainedOrLost;
        criticalText.text = criticalChance.ToString();
    }
    public void ModifyDodge(int dodgeGainedOrLost)
    {
        Debug.Log("CharacterData.ModifyDodge() called, modifying by " + dodgeGainedOrLost.ToString());
        dodge += dodgeGainedOrLost;
        dodgeText.text = dodge.ToString();
    }
    public void ModifyParry(int parryGainedOrLost)
    {
        Debug.Log("CharacterData.ModifyParry() called, modifying by " + parryGainedOrLost.ToString());
        parry += parryGainedOrLost;
        parryText.text = parry.ToString();
    }
    public void ModifyAuraSize(int auraGainedOrLost)
    {
        Debug.Log("CharacterData.ModifyAuraSize() called, modifying by " + auraGainedOrLost.ToString());
        auraSize += auraGainedOrLost;
        auraSizeText.text = auraSize.ToString();
    }
    public void ModifyMaxEnergy(int maxEnergyGainedOrlost)
    {
        Debug.Log("CharacterData.ModifyMaxEnergy() called, modifying by " + maxEnergyGainedOrlost.ToString());
        maxEnergy += maxEnergyGainedOrlost;
        maxEnergyText.text = maxEnergy.ToString();
    }   
    public void ModifyMeleeRange(int meleeRangeGainedOrLost)
    {
        Debug.Log("CharacterData.ModifyMeleeRange() called, modifying by " + meleeRangeGainedOrLost.ToString());
        meleeRange += meleeRangeGainedOrLost;
        meleeRangeText.text = meleeRange.ToString();
    }


    #endregion

    // Modify Resistance Stats + Text Values
    #region
    public void ModifyPhysicalResistance(int resistanceGainedOrLost)
    {
        Debug.Log("CharacterData.ModifyPhysicalResistance() called, modifying by " + resistanceGainedOrLost.ToString());
        physicalResistance += resistanceGainedOrLost;
        physicalResistanceText.text = physicalResistance.ToString();
    }
    public void ModifyFireResistance(int resistanceGainedOrLost)
    {
        Debug.Log("CharacterData.ModifyFireResistance() called, modifying by " + resistanceGainedOrLost.ToString());
        fireResistance += resistanceGainedOrLost;
        fireResistanceText.text = fireResistance.ToString();
    }
    public void ModifyFrostResistance(int resistanceGainedOrLost)
    {
        Debug.Log("CharacterData.ModifyFrostResistance() called, modifying by " + resistanceGainedOrLost.ToString());
        frostResistance += resistanceGainedOrLost;
        frostResistanceText.text = frostResistance.ToString();
    }
    public void ModifyPoisonResistance(int resistanceGainedOrLost)
    {
        Debug.Log("CharacterData.ModifyPoisonResistance() called, modifying by " + resistanceGainedOrLost.ToString());
        poisonResistance += resistanceGainedOrLost;
        poisonResistanceText.text = poisonResistance.ToString();
    }
    public void ModifyAirResistance(int resistanceGainedOrLost)
    {
        Debug.Log("CharacterData.ModifyAirResistance() called, modifying by " + resistanceGainedOrLost.ToString());
        airResistance += resistanceGainedOrLost;
        airResistanceText.text = airResistance.ToString();
    }
    public void ModifyShadowResistance(int resistanceGainedOrLost)
    {
        Debug.Log("CharacterData.ModifyShadowResistance() called, modifying by " + resistanceGainedOrLost.ToString());
        shadowResistance += resistanceGainedOrLost;
        shadowResistanceText.text = shadowResistance.ToString();
    }

    #endregion

    // Modify Health
    #region
    public void SetCurrentHealth(int newValue)
    {
        Debug.Log("CharacterData.SetCurrentHealth() called, new health value: " + newValue.ToString());

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
        Debug.Log("CharacterData.ModifyCurrentHealth() called, modifying by " + healthGainedOrLost.ToString());

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

        currentHealthText.text = currentHealth.ToString();
        if (myCampSiteCharacter != null)
        {
            myCampSiteCharacter.ModifyCurrentHealthText(currentHealth);
            myCampSiteCharacter.UpdateHealthBarPosition(currentHealth, maxHealth);
        }
        if (myStoryWindowCharacter != null)
        {
            myStoryWindowCharacter.ModifyCurrentHealthText(currentHealth);
        }

        // Calculate health bar pos
        float currentHealthFloat = currentHealth;
        float maxHealthFloat = maxHealth;
        healthBar.value = currentHealthFloat / maxHealthFloat;
    }
    public void ModifyMaxHealth(int maxHealthGainedOrLost)
    {
        Debug.Log("CharacterData.ModifyMaxHealth() called, modifying by " + maxHealthGainedOrLost.ToString());

        maxHealth += maxHealthGainedOrLost;
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
        StartAddAttributeTabProcess("Tenacious", stacks);
    }
    public void ModifyMasochist(int stacks)
    {
        Debug.Log("CharacterData.ModifyMasochist() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        tenaciousStacks += stacks;
        StartAddAttributeTabProcess("Masochist", stacks);
    }
    public void ModifyLastStand(int stacks)
    {
        Debug.Log("CharacterData.ModifyLastStand() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        lastStandStacks += stacks;
        StartAddAttributeTabProcess("Last Stand", stacks);
    }
    public void ModifySlippery(int stacks)
    {
        Debug.Log("CharacterData.ModifySlippery() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        slipperyStacks += stacks;
        StartAddAttributeTabProcess("Slippery", stacks);
    }
    public void ModifyUnstoppable(int stacks)
    {
        Debug.Log("CharacterData.ModifyUnstoppable() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        unstoppableStacks += stacks;
        StartAddAttributeTabProcess("Unstoppable", stacks);
    }
    public void ModifyPerfectAim(int stacks)
    {
        Debug.Log("CharacterData.ModifyPerfectAim() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        perfectAimStacks += stacks;
        StartAddAttributeTabProcess("Perfect Aim", stacks);
    }
    public void ModifyVirtuoso(int stacks)
    {
        Debug.Log("CharacterData.ModifyVirtuoso() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        virtuosoStacks += stacks;
        StartAddAttributeTabProcess("Virtuoso", stacks);
    }
    public void ModifyRiposte(int stacks)
    {
        Debug.Log("CharacterData.ModifyRiposte() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        riposteStacks += stacks;
        StartAddAttributeTabProcess("Riposte", stacks);
    }
    public void ModifyPerfectReflexes(int stacks)
    {
        Debug.Log("CharacterData.ModifyPerfectReflexes() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        perfectReflexesStacks += stacks;
        StartAddAttributeTabProcess("Perfect Reflexes", stacks);
    }
    public void ModifyOpportunist(int stacks)
    {
        Debug.Log("CharacterData.ModifyOpportunist() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        opportunistStacks += stacks;
        StartAddAttributeTabProcess("Opportunist", stacks);
    }
    public void ModifyPowerLimit(int stacks)
    {
        Debug.Log("CharacterData.ModifyPowerLimit() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        powerLimitStacks += stacks;
    }
    public void ModifyPatientStalker(int stacks)
    {
        Debug.Log("CharacterData.ModifyPatientStalker() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        patientStalkerStacks += stacks;
        StartAddAttributeTabProcess("Patient Stalker", stacks);
    }
    public void ModifyStealth(int stacks)
    {
        Debug.Log("CharacterData.ModifyStealth() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        stealthStacks += stacks;
        StartAddAttributeTabProcess("Stealth", stacks);
    }
    public void ModifyCautious(int stacks)
    {
        Debug.Log("CharacterData.ModifyCautious() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        cautiousStacks += stacks;
        StartAddAttributeTabProcess("Cautious", stacks);
    }
    public void ModifyGuardianAura(int stacks)
    {
        Debug.Log("CharacterData.ModifyGuardianAura() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        guardianAuraStacks += stacks;
        StartAddAttributeTabProcess("Guardian Aura", stacks);
    }
    public void ModifyUnwavering(int stacks)
    {
        Debug.Log("CharacterData.ModifyUnwavering() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        unwaveringStacks += stacks;
        StartAddAttributeTabProcess("Unwavering", stacks);
    }
    public void ModifyFieryAura(int stacks)
    {
        Debug.Log("CharacterData.ModifyFieryAura() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        fieryAuraStacks += stacks;
        StartAddAttributeTabProcess("Fiery Aura", stacks);
    }
    public void ModifyImmolation(int stacks)
    {
        Debug.Log("CharacterData.ModifyImmolation() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        immolationStacks += stacks;
        StartAddAttributeTabProcess("Immolation", stacks);
    }
    public void ModifyDemon(int stacks)
    {
        Debug.Log("CharacterData.ModifyDemon() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        demonStacks += stacks;
        StartAddAttributeTabProcess("Demon", stacks);
    }
    public void ModifyShatter(int stacks)
    {
        Debug.Log("CharacterData.ModifyShatter() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        shatterStacks += stacks;
        StartAddAttributeTabProcess("Shatter", stacks);
    }
    public void ModifyFrozenHeart(int stacks)
    {
        Debug.Log("CharacterData.ModifyFrozenHeart() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        frozenHeartStacks += stacks;
        StartAddAttributeTabProcess("Frozen Heart", stacks);
    }
    public void ModifyPredator(int stacks)
    {
        Debug.Log("CharacterData.ModifyPredator() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        predatorStacks += stacks;
        StartAddAttributeTabProcess("Predator", stacks);
    }
    public void ModifyHawkEye(int stacks)
    {
        Debug.Log("CharacterData.ModifyHawkEye() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        hawkEyeStacks += stacks;
        StartAddAttributeTabProcess("Hawk Eye", stacks);
    }
    public void ModifyFlux(int stacks)
    {
        Debug.Log("CharacterData.ModifyFlux() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        fluxStacks += stacks;
        StartAddAttributeTabProcess("Flux", stacks);
    }
    public void ModifyCoupDeGrace(int stacks)
    {
        Debug.Log("CharacterData.ModifyCoupDeGrace() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        coupDeGraceStacks += stacks;
        StartAddAttributeTabProcess("Coup De Grace", stacks);
    }
    public void ModifyQuickDraw(int stacks)
    {
        Debug.Log("CharacterData.ModifyQuickDraw() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        quickDrawStacks += stacks;
        StartAddAttributeTabProcess("Quick Draw", stacks);
    }
    public void ModifyPhasing(int stacks)
    {
        Debug.Log("CharacterData.ModifyPhasing() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        phasingStacks += stacks;
        StartAddAttributeTabProcess("Phasing", stacks);
    }
    public void ModifyEtherealBeing(int stacks)
    {
        Debug.Log("CharacterData.ModifyEtherealBeing() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        etherealBeingStacks += stacks;
        StartAddAttributeTabProcess("Ethereal Being", stacks);
    }
    public void ModifyEncouragingAura(int stacks)
    {
        Debug.Log("CharacterData.ModifyEncouragingAura() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        encouragingAuraStacks += stacks;
        StartAddAttributeTabProcess("Encouraging Aura", stacks);
    }
    public void ModifyRadiance(int stacks)
    {
        Debug.Log("CharacterData.ModifyRadiance() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        radianceStacks += stacks;
        StartAddAttributeTabProcess("Radiance", stacks);
    }
    public void ModifySacredAura(int stacks)
    {
        Debug.Log("CharacterData.ModifySacredAura() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        sacredAuraStacks += stacks;
        StartAddAttributeTabProcess("Sacred Aura", stacks);
    }
    public void ModifyShadowAura(int stacks)
    {
        Debug.Log("CharacterData.ModifyShadowAura() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        shadowAuraStacks += stacks;
        StartAddAttributeTabProcess("Shadow Aura", stacks);
    }
    public void ModifyShadowForm(int stacks)
    {
        Debug.Log("CharacterData.ModifyShadowForm() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        shadowFormStacks += stacks;
        StartAddAttributeTabProcess("Shadow Form", stacks);
    }
    public void ModifyPoisonous(int stacks)
    {
        Debug.Log("CharacterData.ModifyPoisonous() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        poisonousStacks += stacks;
        StartAddAttributeTabProcess("Poisonous", stacks);
    }
    public void ModifyVenomous(int stacks)
    {
        Debug.Log("CharacterData.ModifyVenomous() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        venomousStacks += stacks;
        StartAddAttributeTabProcess("Venomous", stacks);
    }
    public void ModifyToxicity(int stacks)
    {
        Debug.Log("CharacterData.ModifyToxicity() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        toxicityStacks += stacks;
        StartAddAttributeTabProcess("Toxicity", stacks);
    }
    public void ModifyToxicAura(int stacks)
    {
        Debug.Log("CharacterData.ModifyToxicAura() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        toxicAuraStacks += stacks;
        StartAddAttributeTabProcess("Toxic Aura", stacks);
    }
    public void ModifyStormAura(int stacks)
    {
        Debug.Log("CharacterData.ModifyStormAura() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        stormAuraStacks += stacks;
        StartAddAttributeTabProcess("Storm Aura", stacks);
    }
    public void ModifyStormLord(int stacks)
    {
        Debug.Log("CharacterData.ModifyStormLord() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        stormLordStacks += stacks;
        StartAddAttributeTabProcess("Storm Lord", stacks);

    }
    public void ModifyThorns(int stacks)
    {
        Debug.Log("CharacterData.ModifyThorns() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        thornsStacks += stacks;
        StartAddAttributeTabProcess("Thorns", stacks);

    }
    public void ModifyEnrage(int stacks)
    {
        Debug.Log("CharacterData.ModifyEnrage() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        enrageStacks += stacks;
        StartAddAttributeTabProcess("Enrage", stacks);
    }
    public void ModifyTrueSight(int stacks)
    {
        Debug.Log("CharacterData.ModifyTrueSight() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        trueSightStacks += stacks;
        StartAddAttributeTabProcess("True Sight", stacks);
    }
    public void ModifyFading(int stacks)
    {
        Debug.Log("CharacterData.ModifyFading() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        fadingStacks += stacks;
        StartAddAttributeTabProcess("Fading", stacks);

    }
    public void ModifyLifeSteal(int stacks)
    {
        Debug.Log("CharacterData.ModifyLifeSteal() called for " + myName + " adding " + stacks.ToString() + " stacks...");
        lifeStealStacks += stacks;
        StartAddAttributeTabProcess("Life Steal", stacks);
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
        // Increase XP gain by 50% if player has 'Genius' state
        if (StateManager.Instance.DoesPlayerAlreadyHaveState("Fast Learners") &&
            xpGainedOrLost > 0)
        {
            xpGainedOrLost += (int) (xpGainedOrLost * 0.3f);
        }        

        currentXP += xpGainedOrLost;

        if (currentXP > currentMaxXP)
        {
            currentXP = currentXP - currentMaxXP;
            ModifyCurrentLevel(1);
            ModifyTalentPoints(1);
            ModifyAbilityPoints(1);
            ScoreManager.Instance.levelsGained++;

        }
        else if(currentXP == currentMaxXP)
        {
            currentXP = 0;
            ModifyCurrentLevel(1);
            ModifyTalentPoints(1);
            ModifyAbilityPoints(1);
            ScoreManager.Instance.levelsGained++;
        }

        xpText.text = currentXP.ToString();
        if(myCampSiteCharacter != null)
        {
            myCampSiteCharacter.ModifyCurrentXPText(currentXP);
            myCampSiteCharacter.UpdateXpBarPosition(currentXP, currentMaxXP);
        }
        if (myStoryWindowCharacter != null)
        {
            myStoryWindowCharacter.ModifyCurrentXPText(currentXP);
        }

        // Calculate XP bar pos
        float currentXPFloat = currentXP;
        float maxXPFloat = currentMaxXP;
        xpBar.value = currentXPFloat / maxXPFloat;
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
        TalentController.Instance.RefreshAllTalentButtonViewStates(this);
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

    // Attribute View Logic
    #region
    public void StartAddAttributeTabProcess(string attributeName, int stacksGainedOrLost)
    {
        Debug.Log("CharacterData.StartAddAttributeTabProcess() called for " + attributeName);

        if (myAttributeTabs.Count > 0)
        {
            string at = null;
            int stacks = 0;
            bool tabUpdated = false;

            foreach (AttributeTab tab in myAttributeTabs)
            {
                if (attributeName == tab.attributeName)
                {
                    // Attribute tab already exists in character's list
                    UpdateAttributeTab(tab, stacksGainedOrLost);
                    tabUpdated = true;
                    break;
                }

                else
                {
                    at = attributeName;
                    stacks = stacksGainedOrLost;
                }
            }

            if (tabUpdated == false)
            {
                AddNewAttributeTab(at, stacks);
            }

        }
        else
        {
            AddNewAttributeTab(attributeName, stacksGainedOrLost);
        }


    }
    public void AddNewAttributeTab(string attributeName, int stacksGained)
    {
        Debug.Log("CharacterData.AddNewAttributeTab() called");

        // only create an icon if the the effects' stacks are at least 1 or -1
        if (stacksGained != 0)
        {            
            GameObject newAttributeTabGO = Instantiate(PrefabHolder.Instance.AttributeTab, attributeTabContentParent.transform);
            AttributeTab newAttributeTab = newAttributeTabGO.GetComponent<AttributeTab>();
            newAttributeTab.InitializeSetup(attributeName, stacksGained);
            myAttributeTabs.Add(newAttributeTab);
        }


    }
    public void RemoveAttributeTab(AttributeTab tabRemoved)
    {
        Debug.Log("StatusManager.RemoveStatusProcess() called, removing tab named " + tabRemoved.attributeName);
        myAttributeTabs.Remove(tabRemoved);
        Destroy(tabRemoved.gameObject);
    }
    public void UpdateAttributeTab(AttributeTab tabToUpdate, int stacksGainedOrLost)
    {
        Debug.Log("StatusManager.UpdateStatusProcess() called, updating " +
            tabToUpdate.attributeName + " tab, applying " + stacksGainedOrLost.ToString() + " stacks");

        tabToUpdate.ModifyStacks(stacksGainedOrLost);
        if (tabToUpdate.attributeStacks == 0)
        {
            RemoveAttributeTab(tabToUpdate);
        }

    }

    #endregion

}
