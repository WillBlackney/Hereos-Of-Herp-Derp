using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterData : MonoBehaviour
{
    // Properties + Components
    #region
    [Header("Component References")]
    public GameObject frontPage;
    public GameObject inventoryPage;
    public GameObject talentTreePage;
    public GameObject inventoryItemParent;
    public Image myImageComponent;
    public TextMeshProUGUI myNameText;
    public Defender myDefenderGO;
    public string myClass;
    public TalentTree talentTreeOne;
    public TalentTree talentTreeTwo;
    public CampSiteCharacter myCampSiteCharacter;
    public StoryWindowCharacterSlot myStoryWindowCharacter;

    [Header("Button Page Components")]
    public TextMeshProUGUI panelCurrentHealthText;
    public TextMeshProUGUI panelMaxHealthText;
    public TextMeshProUGUI currentXPText;
    public TextMeshProUGUI currentMaxXPText;
    public TextMeshProUGUI currentLevelText;

    [Header("Front Page Components")]
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI mobilityText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI currentHealthText;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI classNameText;
    public Image classSprite;

    [Header("Talent Tree Page Components")]
    public TextMeshProUGUI talentPointText;

    [Header("Base Stat Properties")]
    public int Mobility;
    public int MaxHealth;
    public int CurrentHealth;
    public int Stamina;
    public int Initiative;
    public int MaxEnergy;
    public int MeleeRange;
    public int Strength;
    public int Wisdom;
    public int Dexterity;
    public int CriticalChance;

    [Header("XP/Level Properties")]
    public int currentXP;
    public int currentMaxXP;
    public int currentLevel;
    public int talentPoints;

    [Header("Item Properties")]
    public List<ItemCard> myItems = new List<ItemCard>();

    [Header("Base Passive Properties")]
    public int enrageStacks;
    public int growingStacks;
    public int barrierStacks;
    public int cautiousStacks;
    public int fleetFootedStacks;
    public int encouragingPresenceStacks;
    public int poisonousStacks;
    public int thornsStacks;
    public int startingBlock;
    public int startingEnergyBonus;
    public int adaptiveStacks;
    public int hatefulPresenceStacks;
    public int fieryPresenceStacks;
    public int guardianPresenceStacks;
    public int masterfulEntrapmentStacks;
    public int thickOfTheFightStacks;
    public bool Stealth;
    public bool Unwavering;
    public bool camoflage;
    public bool poisonImmunity;
    public int trueSightStacks;
    public int startingIntiativeBonus;
    public bool venomous;

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
    public bool KnowsDimenisonalBlast;
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
        if(characterClass == "Warrior")
        {
            myImageComponent.sprite = CharacterLibrary.Instance.warriorSprite;
            myNameText.text = CharacterLibrary.Instance.warriorClassName;
            classSprite.sprite = CharacterLibrary.Instance.warriorSprite;
            classNameText.text = CharacterLibrary.Instance.warriorClassName;

            myClass = "Warrior";
            ModifyMobility(2);
            ModifyStrength(0);
            ModifyMaxHealth(100);
            ModifyCurrentHealth(100);
            ModifyStamina(40);
            ModifyMaxEnergy(60);
            ModifyInitiative(3);
            ModifyMeleeRange(1);
            ModifyCriticalChance(5);

            // Abiltiies + Passives
            cautiousStacks = 5;
            KnowsMove = true;
            KnowsStrike = true;

            KnowsFireBall = true;  
            KnowsPhoenixDive = true;
            KnowsBloodOffering = true;
            KnowsSecondWind = true;
            KnowsGuard = true;
            KnowsFrostNova= true;
            KnowsShadowStep = true;


            //talentTreeOne.InitializeSetup("Path of Rage");
            //talentTreeTwo.InitializeSetup("Path of the Guardian");            

        }

        else if (characterClass == "Mage")
        {
            myImageComponent.sprite = CharacterLibrary.Instance.mageSprite;
            myNameText.text = CharacterLibrary.Instance.mageClassName;
            classSprite.sprite = CharacterLibrary.Instance.mageSprite;
            classNameText.text = CharacterLibrary.Instance.mageClassName;
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

            fleetFootedStacks = 1;
            KnowsMove = true;
            KnowsStrike = true;
            KnowsDefend = true;
            KnowsFireBall = true;
           

            talentTreeOne.InitializeSetup("Path of Manipulation");
            talentTreeTwo.InitializeSetup("Path of Wrath");

            
                     
        }

        else if (characterClass == "Ranger")
        {
            myImageComponent.sprite = CharacterLibrary.Instance.rangerSprite;
            myNameText.text = CharacterLibrary.Instance.rangerClassName;
            classSprite.sprite = CharacterLibrary.Instance.rangerSprite;
            classNameText.text = CharacterLibrary.Instance.rangerClassName;
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
            myImageComponent.sprite = CharacterLibrary.Instance.priestSprite;
            myNameText.text = CharacterLibrary.Instance.priestClassName;
            classSprite.sprite = CharacterLibrary.Instance.priestSprite;
            classNameText.text = CharacterLibrary.Instance.priestClassName;
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

            encouragingPresenceStacks = 10;

            KnowsMove = true;
            KnowsStrike = true;
            KnowsDefend = true;
            KnowsHolyFire = true;
            KnowsInvigorate = true;
            KnowsChaosBolt = true;
            

            talentTreeOne.InitializeSetup("Path of Shadows");
            talentTreeTwo.InitializeSetup("Path of Divinity");



        }

        else if (characterClass == "Rogue")
        {
            myImageComponent.sprite = CharacterLibrary.Instance.rogueSprite;
            myNameText.text = CharacterLibrary.Instance.rogueClassName;
            classSprite.sprite = CharacterLibrary.Instance.rogueSprite;
            classNameText.text = CharacterLibrary.Instance.rogueClassName;
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

            talentTreeOne.InitializeSetup("Path of Combat");
            talentTreeTwo.InitializeSetup("Path of Trickery");

                      
        }

        else if (characterClass == "Shaman")
        {
            myImageComponent.sprite = CharacterLibrary.Instance.shamanSprite;
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

            talentTreeOne.InitializeSetup("Path of Storms");
            talentTreeTwo.InitializeSetup("Path of Fury");


            thickOfTheFightStacks += 2;           

        }
        // Set up talent trees
        talentTreeOne.SetTalentTreePartner(talentTreeTwo);
        talentTreeTwo.SetTalentTreePartner(talentTreeOne);       
        

        ModifyCurrentLevel(1);
        SetMaxXP(100);
        ModifyCurrentXP(0);
        ModifyTalentPoints(0);
        
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
            foreach(Tile tile in possibleSpawnLocations)
            {
                if(tile.IsEmpty && tile.IsWalkable)
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

    // Modify Primary Stats
    #region
    public void SetCurrentHealth(int newValue)
    {
        CurrentHealth = newValue;

        // prevent healing past max HP
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }

        // Prevent damaging into negative health
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
        panelCurrentHealthText.text = CurrentHealth.ToString();
        currentHealthText.text = CurrentHealth.ToString();

    }
    public void ModifyCurrentHealth(int healthGainedOrLost)
    {
        CurrentHealth += healthGainedOrLost;

        // prevent healing past max HP
        if(CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }

        // Prevent damaging into negative health
        if(CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
        panelCurrentHealthText.text = CurrentHealth.ToString();
        currentHealthText.text = CurrentHealth.ToString();
        if (myCampSiteCharacter != null)
        {
            myCampSiteCharacter.ModifyCurrentHealthText(CurrentHealth);
        }
        if (myStoryWindowCharacter != null)
        {
            myStoryWindowCharacter.ModifyCurrentHealthText(CurrentHealth);
        }
    }
    public void ModifyMaxHealth(int maxHealthGainedOrLost)
    {
        MaxHealth += maxHealthGainedOrLost;
        panelMaxHealthText.text = MaxHealth.ToString();
        maxHealthText.text = MaxHealth.ToString();

        if(CurrentHealth > MaxHealth)
        {
            ModifyCurrentHealth(-(CurrentHealth - MaxHealth));
        }
        if (myCampSiteCharacter != null)
        {
            myCampSiteCharacter.ModifyMaxHealthText(MaxHealth);
        }
        if (myStoryWindowCharacter != null)
        {
            myStoryWindowCharacter.ModifyMaxHealthText(MaxHealth);
        }
    }
    public void ModifyStrength(int strengthGainedOrLost)
    {
        Strength += strengthGainedOrLost;
        strengthText.text = Strength.ToString();
    }
    public void ModifyWisdom(int wisdomGainedOrLost)
    {
        Wisdom += wisdomGainedOrLost;
    }
    public void ModifyDexterity(int dexterityGainedOrLost)
    {
        Dexterity += dexterityGainedOrLost;
    }
    public void ModifyMobility(int mobilityGainedOrLost)
    {
        Mobility += mobilityGainedOrLost;
        mobilityText.text = Mobility.ToString();
    }
    public void ModifyInitiative(int initiativeGainedOrLost)
    {
        Initiative += initiativeGainedOrLost;
    }
    public void ModifyCriticalChance(int criticalGainedOrLost)
    {
        CriticalChance += criticalGainedOrLost;
    }
    public void ModifyStamina(int energyGainedOrlost)
    {
        Stamina += energyGainedOrlost;
        if(staminaText != null)
        {
            staminaText.text = Stamina.ToString();
        }
        
    }
    public void ModifyMaxEnergy(int maxEnergyGainedOrlost)
    {
        MaxEnergy += maxEnergyGainedOrlost;        
    }
    #endregion

    // Modify Passive Stats
    #region
    public void ModifyStartingBlock(int blockGainedOrLost)
    {
        startingBlock += blockGainedOrLost;
    }    
    public void ModifyStartingEnergyBonus(int apBonusGainedOrLost)
    {
        startingEnergyBonus += apBonusGainedOrLost;
    }
    public void ModifyMeleeRange(int meleeRangeGainedOrLost)
    {
        MeleeRange += meleeRangeGainedOrLost;
    }
    public void ModifyAdaptive(int adaptiveStacksGainedOrLost)
    {
        adaptiveStacks += adaptiveStacksGainedOrLost;
    }
    public void ModifyThorns(int thornsGainedOrLost)
    {
        thornsStacks += thornsGainedOrLost;
    }

    #endregion

    // Modify Level / XP / Talent Points
    #region
    public void SetMaxXP(int newValue)
    {
        currentMaxXP = newValue;
        currentMaxXPText.text = currentMaxXP.ToString();
    }
    public void ModifyCurrentLevel(int levelsGainedOrLost)
    {
        currentLevel += levelsGainedOrLost;
        currentLevelText.text = currentLevel.ToString();
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

        currentXPText.text = currentXP.ToString();
        if(myCampSiteCharacter != null)
        {
            myCampSiteCharacter.ModifyCurrentXPText(currentXP);
        }
        if (myStoryWindowCharacter != null)
        {
            myStoryWindowCharacter.ModifyCurrentXPText(currentXP);
        }
    }
    public void ModifyTalentPoints(int talentPointsGainedOrLost)
    {
        talentPoints += talentPointsGainedOrLost;
        talentPointText.text = talentPoints.ToString();
    }
    #endregion

    // Mouse + Button + Click Events
    #region
    public void OnCharacterImageButtonClicked()
    {      
        EnableFrontPage();
    }
    public void OnEquiptmentButtonClicked()
    {
        frontPage.SetActive(false);
        inventoryPage.SetActive(true);
    }
    public void OnTalentTreeButtonClicked()
    {
        frontPage.SetActive(false);
        talentTreePage.SetActive(true);
        SetTalentButtonVisibilities();
    }    
    public void OnTalentTreeBackButtonClicked()
    {
        frontPage.SetActive(true);
        talentTreePage.SetActive(false);
    }
    public void OnFrontPageBackButtonClicked()
    {
        frontPage.SetActive(false);
        foreach(CharacterData characterData in CharacterRoster.Instance.allCharacterDataObjects)
        {
            characterData.gameObject.SetActive(true);
        }

        CharacterRoster.Instance.CharacterRosterCloseButton.SetActive(true);
    }
    public void OnEquiptmentPageBackButtonClicked()
    {
        inventoryPage.SetActive(false);
        frontPage.SetActive(true);
    }
    public void OnAssignItemButtonClicked()
    {
        // assign selected char data
        CharacterRoster.Instance.selectedCharacterData = this;
        // enable inventory
        InventoryManager.Instance.inventoryParent.SetActive(true);
        // set inventory ready state
        InventoryManager.Instance.readyToAcceptNewItem = true;
    }
    #endregion

    // Visibility + View Logic
    #region
    public void SetTalentButtonVisibilities()
    {
        List<Talent> allTalentButtons = new List<Talent>();

        allTalentButtons.AddRange(talentTreeOne.allTalentButtons);
        allTalentButtons.AddRange(talentTreeTwo.allTalentButtons);

        foreach (Talent talent in allTalentButtons)
        {
            talent.UpdateVisbilityState();
        }
    }
    public void EnableFrontPage()
    {
        Debug.Log("Character image clicked, opening front page");
        foreach (CharacterData characterData in CharacterRoster.Instance.allCharacterDataObjects)
        {
            if (characterData != this)
            {
                characterData.gameObject.SetActive(false);
            }
        }

        CharacterRoster.Instance.CharacterRosterCloseButton.SetActive(false);
        frontPage.SetActive(true);

    }
    #endregion

    // Items + Inventory Logic
    #region
    public void AddItemToEquiptment(ItemCard item)
    {
        GameObject newItem = Instantiate(item.gameObject, inventoryItemParent.transform);
        myItems.Add(newItem.GetComponent<ItemCard>());
        ItemLibrary.Instance.AssignItem(this, newItem.GetComponent<ItemCard>().myName);
    }
    #endregion
}
