using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterData : MonoBehaviour
{
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

    [Header("Button Page Components")]
    public TextMeshProUGUI panelCurrentHealthText;
    public TextMeshProUGUI panelMaxHealthText;
    public TextMeshProUGUI currentXPText;
    public TextMeshProUGUI currentMaxXPText;
    public TextMeshProUGUI currentLevelText;

    [Header("Front Page Components")]
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI mobilityText;
    public TextMeshProUGUI energyText;
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
    public int Energy;
    public int Initiative;
    public int MaxAP;
    public int MeleeRange;
    public int Strength;
    public int Wisdom;
    public int Dexterity;    

    [Header("XP/Level Properties")]
    public int currentXP;
    public int currentMaxXP;
    public int currentLevel;
    public int talentPoints;

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
    public int startingAPBonus;
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

    [Header("Known Abilities")]
    public bool KnowsMove;
    public bool KnowsStrike;
    public bool KnowsBlock;
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

    [Header("Known Talents")]
    public bool KnowsImprovedPreparation;
    public bool KnowsImprovedDash;
    public bool KnowsImprovedInvigorate;
    public bool KnowsImprovedHolyFire;
    public bool KnowsImprovedInspire;
    public bool KnowsImprovedWhirlwind;
    public bool KnowsImprovedTelekinesis;
    public bool KnowsImprovedFireBall;

    public void InitializeSetup(string characterClass)
    {
        if(characterClass == "Warrior")
        {
            myImageComponent.sprite = CharacterLibrary.Instance.warriorSprite;
            myNameText.text = CharacterLibrary.Instance.warriorClassName;
            classSprite.sprite = CharacterLibrary.Instance.warriorSprite;
            classNameText.text = CharacterLibrary.Instance.warriorClassName;

            myClass = "Warrior";
            ModifyMobility(1);
            ModifyStrength(0);
            ModifyMaxHealth(60);
            ModifyCurrentHealth(60);
            ModifyEnergy(4);
            ModifyInitiative(3);
            ModifyMeleeRange(1);

            // Abiltiies + Passives
            cautiousStacks = 5;
            KnowsMove = true;
            KnowsStrike = true;
            KnowsBlock = true;
            KnowsCharge = true;
            KnowsInspire = true;
            KnowsWhirlwind = true;

            MaxAP = 6;

            talentTreeOne.InitializeSetup("Path of Rage");
            talentTreeTwo.InitializeSetup("Path of the Guardian");            

        }

        else if (characterClass == "Mage")
        {
            myImageComponent.sprite = CharacterLibrary.Instance.mageSprite;
            myNameText.text = CharacterLibrary.Instance.mageClassName;
            classSprite.sprite = CharacterLibrary.Instance.mageSprite;
            classNameText.text = CharacterLibrary.Instance.mageClassName;
            myClass = "Mage";
            ModifyMobility(1);
            ModifyStrength(0);
            ModifyMaxHealth(45);            
            ModifyCurrentHealth(45);            
            ModifyEnergy(4);
            ModifyInitiative(3);
            ModifyMeleeRange(1);
            MaxAP = 6;

            fleetFootedStacks = 1;
            KnowsMove = true;
            KnowsStrike = true;
            KnowsBlock = true;
            KnowsFireBall = true;
            KnowsTelekinesis = true;
            KnowsFrostBolt = true;

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
            ModifyMaxHealth(45);
            ModifyCurrentHealth(45);
            ModifyEnergy(4);
            ModifyInitiative(3);
            ModifyMeleeRange(1);
            MaxAP = 6;
                        
        }

        else if (characterClass == "Priest")
        {
            myImageComponent.sprite = CharacterLibrary.Instance.priestSprite;
            myNameText.text = CharacterLibrary.Instance.priestClassName;
            classSprite.sprite = CharacterLibrary.Instance.priestSprite;
            classNameText.text = CharacterLibrary.Instance.priestClassName;
            myClass = "Priest";
            ModifyMobility(1);
            ModifyStrength(0);
            ModifyMaxHealth(50);
            ModifyCurrentHealth(50);
            ModifyEnergy(4);
            ModifyInitiative(3);
            ModifyMeleeRange(1);
            MaxAP = 6;

            encouragingPresenceStacks = 1;

            KnowsMove = true;
            KnowsStrike = true;
            KnowsBlock = true;
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
            ModifyMobility(1);
            ModifyStrength(0);
            ModifyMaxHealth(45);
            ModifyCurrentHealth(45);
            ModifyEnergy(4);
            ModifyInitiative(3);
            ModifyMeleeRange(1);
            poisonousStacks = 1;
            MaxAP = 6;

            KnowsMove = true;
            KnowsStrike = true;
            KnowsBlock = true;
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
            ModifyMobility(1);
            ModifyStrength(0);
            ModifyMaxHealth(50);
            ModifyCurrentHealth(50);
            ModifyEnergy(4);
            ModifyInitiative(3);
            ModifyMeleeRange(1);
            MaxAP = 6;

            KnowsMove = true;
            KnowsStrike = true;
            KnowsBlock = true;
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
        ModifyTalentPoints(3);
        
    }

    public void CreateMyDefenderGameObject()
    {
        List<TileScript> possibleSpawnLocations = LevelManager.Instance.GetDefenderSpawnTiles();
        TileScript spawnLocation = null;

        if (myClass == "Warrior")
        {
            // Instantiate GO from prefab
            GameObject defenderGO = Instantiate(PrefabHolder.Instance.warriorPrefab, transform.position, Quaternion.identity);
            // Get a reference to the defender script 
            Defender defender = defenderGO.GetComponent<Defender>();
            foreach(TileScript tile in possibleSpawnLocations)
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
            foreach (TileScript tile in possibleSpawnLocations)
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
            foreach (TileScript tile in possibleSpawnLocations)
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
            foreach (TileScript tile in possibleSpawnLocations)
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
            foreach (TileScript tile in possibleSpawnLocations)
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
            foreach (TileScript tile in possibleSpawnLocations)
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

    // Modify stats related
    public void SetCurrentHealth(int newValue)
    {
        CurrentHealth = newValue;
        // prevent healing past max HP
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
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
        panelCurrentHealthText.text = CurrentHealth.ToString();
        currentHealthText.text = CurrentHealth.ToString();
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
    }

    public void ModifyStrength(int strengthGainedOrLost)
    {
        Strength += strengthGainedOrLost;
        strengthText.text = Strength.ToString();
    }
    public void ModifyWisdom(int wisdomGainedOrLost)
    {
        Wisdom += wisdomGainedOrLost;
        //strengthText.text = Strength.ToString();
    }

    public void ModifyDexterity(int dexterityGainedOrLost)
    {
        Dexterity += dexterityGainedOrLost;
        //strengthText.text = Strength.ToString();
    }

    public void ModifyMobility(int mobilityGainedOrLost)
    {
        Mobility += mobilityGainedOrLost;
        mobilityText.text = Mobility.ToString();
    }

    public void ModifyInitiative(int initiativeGainedOrLost)
    {
        Initiative += initiativeGainedOrLost;
        //initiativeText.text = Initiative.ToString();
    }

    public void ModifyEnergy(int energyGainedOrlost)
    {
        Energy += energyGainedOrlost;
        energyText.text = Energy.ToString();
    }

    public void ModifyMaxAP(int maxAPGainedOrlost)
    {
        MaxAP += maxAPGainedOrlost;        
    }

    public void ModifyCurrentXP(int xpGainedOrLost)
    {        
        currentXP += xpGainedOrLost;
        if(currentXP > currentMaxXP)
        {
            currentXP = currentXP - currentMaxXP;
            ModifyCurrentLevel(1);
            ModifyTalentPoints(1);
            
        }

        currentXPText.text = currentXP.ToString();
    }

    public void ModifyStartingBlock(int blockGainedOrLost)
    {
        startingBlock += blockGainedOrLost;
    }

    public void ModifyThorns(int thornsGainedOrLost)
    {
        thornsStacks += thornsGainedOrLost;
    }

    public void ModifyStartingAPBonus(int apBonusGainedOrLost)
    {
        startingAPBonus += apBonusGainedOrLost;
    }

    public void ModifyMeleeRange(int meleeRangeGainedOrLost)
    {
        MeleeRange += meleeRangeGainedOrLost;
    }

    public void ModifyAdaptive(int adaptiveStacksGainedOrLost)
    {
        adaptiveStacks += adaptiveStacksGainedOrLost;
    }

    // Modify Level / XP / Talents
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

    public void ModifyTalentPoints(int talentPointsGainedOrLost)
    {
        talentPoints += talentPointsGainedOrLost;
        talentPointText.text = talentPoints.ToString();
    }

    // UI related
    public void OnCharacterImageButtonClicked()
    {
        if(CampSiteManager.Instance.awaitingLevelUpChoice == true)
        {
            if(ArtifactManager.Instance.HasArtifact("Kettle Bell"))
            {
                ModifyCurrentLevel(2);
                ModifyTalentPoints(2);

            }
            else
            {
                ModifyCurrentLevel(1);
                ModifyTalentPoints(1);
            }
            
            UIManager.Instance.DisableCharacterRosterView();
            CampSiteManager.Instance.DisableAllButtonViews();
            CampSiteManager.Instance.EnableContinueButtonView();
            CampSiteManager.Instance.playerHasMadeChoice = true;
            CampSiteManager.Instance.awaitingLevelUpChoice = false;
        }
        else if(CampSiteManager.Instance.awaitingHealChoice == true)
        {
            // heal 50%
            ModifyCurrentHealth(MaxHealth / 2);


            UIManager.Instance.DisableCharacterRosterView();
            CampSiteManager.Instance.DisableAllButtonViews();
            CampSiteManager.Instance.EnableContinueButtonView();
            CampSiteManager.Instance.playerHasMadeChoice = true;
            CampSiteManager.Instance.awaitingHealChoice = false;
        }
        else
        {
            EnableFrontPage();
        }
              
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

    public void SetTalentButtonVisibilities()
    {
        List<Talent> allTalentButtons = new List<Talent>();

        allTalentButtons.AddRange(talentTreeOne.allTalentButtons);
        allTalentButtons.AddRange(talentTreeTwo.allTalentButtons);

        foreach(Talent talent in allTalentButtons)
        {
            talent.UpdateVisbilityState();
        }
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
        Inventory.Instance.inventoryParent.SetActive(true);
        // set inventory ready state
        Inventory.Instance.readyToAcceptNewItem = true;
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

    public void AddItemToEquiptment(ItemCard item)
    {
        GameObject newItem = Instantiate(item.gameObject, inventoryItemParent.transform);
        myItems.Add(newItem.GetComponent<ItemCard>());
        ItemLibrary.Instance.AssignItem(this, newItem.GetComponent<ItemCard>().myName);
    }

}
