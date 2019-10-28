using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Defender : LivingEntity
{
    [Header("UI + Component References ")]
    public AbilityBar myAbilityBar;
    public GameObject myUIParent;
    public CharacterData myCharacterData;

    public TextMeshProUGUI myCurrentAPText;
    public TextMeshProUGUI myCurrentMaxAPText;
    public TextMeshProUGUI myCurrentStrengthStatText;
    public TextMeshProUGUI myCurrentMobilityStatText;
    public TextMeshProUGUI myCurrentAPStatText;
    public TextMeshProUGUI myCurrentHealthTextStatBar;
    public TextMeshProUGUI myCurrentMaxHealthTextStatBar;


    // State related variables
    [Header("Ability Orders")]
    // public bool readyToMove = false;
    public bool awaitingMoveOrder = false;
    public bool awaitingStrikeOrder = false;
    public bool awaitingChargeTargetOrder = false;
    public bool awaitingChargeLocationOrder = false;
    public bool awaitingInspireOrder = false;
    public bool awaitingGuardOrder = false;
    public bool awaitingShootOrder = false;
    public bool awaitingMeteorOrder = false;
    public bool awaitingTelekinesisTargetOrder = false;
    public bool awaitingTelekinesisLocationOrder = false;
    public bool awaitingFrostBoltOrder = false;
    public bool awaitingFireBallOrder = false;
    public bool awaitingRapidFireOrder = false;
    public bool awaitingImpalingBoltOrder = false;
    public bool awaitingForestMedicineOrder = false;
    public bool awaitingInvigorateOrder = false;
    public bool awaitingHolyFireOrder = false;
    public bool awaitingVoidBombOrder = false;
    public bool awaitingNightmareOrder = false;
    public bool awaitingTwinStrikeOrder = false;
    public bool awaitingDashOrder = false;
    public bool awaitingSliceAndDiceOrder = false;
    public bool awaitingPoisonDartOrder = false;
    public bool awaitingChemicalReactionOrder = false;
    public bool awaitingGetDownOrder;
    public bool awaitingSmashOrder;
    public bool awaitingLightningShieldOrder;
    public bool awaitingChainLightningOrder;
    public bool awaitingPrimalBlastOrder;
    public bool awaitingPrimalRageOrder;
    public bool awaitingPhaseShiftOrder;
    public bool awaitingSanctityOrder;
    public bool awaitingBlessOrder;
    public bool awaitingSiphonLifeOrder;
    public bool awaitingChaosBoltOrder;


    // Initialization and Setup
    public override void InitializeSetup(Point startingGridPosition, TileScript startingTile)
    {
        DefenderManager.Instance.allDefenders.Add(this);
        myUIParent.SetActive(false);
        transform.SetParent(DefenderManager.Instance.defendersParent.transform);
        base.InitializeSetup(startingGridPosition, startingTile);
    }
    public override void SetBaseProperties()
    {
        RunSetupFromCharacterData();
        RunSetupFromArtifactData();
        base.SetBaseProperties();
        // Set up visuals
        UpdateCurrentAPText(currentAP);
        UpdateCurrentMaxAPText(currentMaxAP);
        UpdateCurrentAPStatText(currentEnergy);        
        UpdateCurrentStrengthStatText(currentStrength);
        UpdateCurrentMobilityStatText(currentMobility);
        
    }
    public void RunSetupFromCharacterData()
    {
        Debug.Log("RunSetupFromCharacterData() called...");
        // Establish connection from defender script to character data
        myCharacterData.myDefenderGO = this;
        // Edit Properties
        baseMaxHealth = myCharacterData.MaxHealth;
        baseStartingHealth = myCharacterData.CurrentHealth;
        baseStrength = myCharacterData.Strength;
        baseDexterity = myCharacterData.Dexterity;
        baseMobility = myCharacterData.Mobility;
        baseEnergy = myCharacterData.Energy;
        baseInitiative = myCharacterData.Initiative;
        baseMaxAP = myCharacterData.MaxAP;
        baseStartingBlock = myCharacterData.startingBlock;
        baseMeleeRange = myCharacterData.MeleeRange;

        // Edit passive traits
        if (myCharacterData.enrageStacks > 0)
        {
            myPassiveManager.LearnEnrage(myCharacterData.enrageStacks);
        }
        if (myCharacterData.growingStacks > 0)
        {
            myPassiveManager.LearnGrowing(myCharacterData.growingStacks);
        }
        if (myCharacterData.barrierStacks > 0)
        {
            ModifyCurrentBarrierStacks(myCharacterData.barrierStacks);
        }
        if (myCharacterData.cautiousStacks > 0)
        {
            myPassiveManager.LearnCautious(myCharacterData.cautiousStacks);
        }
        if (myCharacterData.fleetFootedStacks > 0)
        {
            myPassiveManager.LearnFleetFooted(myCharacterData.fleetFootedStacks);
        }
        if (myCharacterData.encouragingPresenceStacks > 0)
        {
            myPassiveManager.LearnEncouragingPresence(myCharacterData.encouragingPresenceStacks);
        }
        if (myCharacterData.hatefulPresenceStacks > 0)
        {
            myPassiveManager.LearnHatefulPresence(myCharacterData.hatefulPresenceStacks);
        }
        if (myCharacterData.poisonousStacks > 0)
        {
            myPassiveManager.LearnPoisonous(myCharacterData.poisonousStacks);
        }
        if (myCharacterData.thornsStacks > 0)
        {
            myPassiveManager.LearnThorns(myCharacterData.thornsStacks);
        }
        if (myCharacterData.adaptiveStacks > 0)
        {
            myPassiveManager.LearnAdaptive(myCharacterData.adaptiveStacks);
        }
        if (myCharacterData.startingAPBonus > 0)
        {
            baseStartingAP += myCharacterData.startingAPBonus;
        }
        if (myCharacterData.stealth)
        {
            myPassiveManager.LearnStealth();
        }
        if (myCharacterData.poisonImmunity)
        {
            myPassiveManager.LearnPoisonImmunity();
        }
        if (myCharacterData.camoflage)
        {
            ApplyCamoflage();
        }
        if (myCharacterData.thickOfTheFightStacks > 0)
        {
            myPassiveManager.LearnThickOfTheFight(myCharacterData.thickOfTheFightStacks);
        }

    }    
    public void RunSetupFromArtifactData()
    {
        if (ArtifactManager.Instance.HasArtifact("Shuriken"))
        {
            baseStrength += 1;
        }
        if (ArtifactManager.Instance.HasArtifact("Champion Belt"))
        {
            baseDexterity += 1;
        }
        if (ArtifactManager.Instance.HasArtifact("Wind Up Boots"))
        {
            baseMobility += 1;
        }
        if (ArtifactManager.Instance.HasArtifact("Coffee Dripper"))
        {
            baseEnergy += 1;
        }
        if (ArtifactManager.Instance.HasArtifact("Bendy Spoon"))
        {
            baseMaxAP += 2;
        }
        if (ArtifactManager.Instance.HasArtifact("Energy Biscuit"))
        {
            baseStartingAP += 2;
        }
        if (ArtifactManager.Instance.HasArtifact("Blacksmith Pliers"))
        {
            baseStartingBlock += 6;
        }
    }


    // Ability, button click and input handling methods
    public void OnAbilityButtonClicked(string abilityName)
    {
        LevelManager.Instance.UnhighlightAllTiles();
        ClearAllOrders();
        if(IsAbleToTakeActions() == false)
        {
            return;
        }

        else if (abilityName == "Move")
        {
            OnMoveButtonClicked();
        }
        else if (abilityName == "Strike")
        {
            OnStrikeButtonClicked();
        }
        else if (abilityName == "Block")
        {
            OnBlockButtonClicked();
        }
        else if (abilityName == "Charge")
        {
            OnChargeButtonClicked();
        }
        else if (abilityName == "Inspire")
        {
            OnInspireButtonClicked();
        }
        else if (abilityName == "Guard")
        {
            OnGuardButtonClicked();
        }
        else if (abilityName == "Meteor")
        {
            OnMeteorButtonClicked();
        }
        else if (abilityName == "Telekinesis")
        {
            OnTelekinesisButtonClicked();
        }
        else if (abilityName == "Frost Bolt")
        {
            OnFrostBoltButtonClicked();
        }
        else if (abilityName == "Fire Ball")
        {
            OnFireBallButtonClicked();
        }
        else if (abilityName == "Shoot")
        {
            OnShootButtonClicked();
        }
        else if (abilityName == "Rapid Fire")
        {
            OnRapidFireButtonClicked();
        }
        else if (abilityName == "Impaling Bolt")
        {
            OnImpalingBoltButtonClicked();
        }
        else if (abilityName == "Forest Medicine")
        {
            OnForestMedicineButtonClicked();
        }
        else if (abilityName == "Whirlwind")
        {
            OnWhirlwindButtonClicked();
        }

        else if (abilityName == "Invigorate")
        {
            OnInvigorateButtonClicked();
        }

        else if (abilityName == "Holy Fire")
        {
            OnHolyFireButtonClicked();
        }

        else if (abilityName == "Void Bomb")
        {
            OnVoidBombButtonClicked();
        }

        else if (abilityName == "Nightmare")
        {
            OnNightmareButtonClicked();
        }

        else if (abilityName == "Twin Strike")
        {
            OnTwinStrikeButtonClicked();
        }

        else if (abilityName == "Dash")
        {
            OnDashButtonClicked();
        }

        else if (abilityName == "Preparation")
        {
            OnPreparationButtonClicked();
        }

        else if (abilityName == "Slice And Dice")
        {
            OnSliceAndDiceButtonClicked();
        }

        else if (abilityName == "Poison Dart")
        {
            OnPoisonDartButtonClicked();
        }
        else if (abilityName == "Chemical Reaction")
        {
            OnChemicalReactionButtonClicked();
        }
        else if (abilityName == "Blood Lust")
        {
            OnBloodLustButtonClicked();
        }
        else if (abilityName == "Get Down!")
        {
            OnGetDownButtonClicked();
        }
        else if (abilityName == "Smash")
        {
            OnSmashButtonClicked();
        }
        else if (abilityName == "Lightning Shield")
        {
            OnLightningShieldClicked();
        }

        else if (abilityName == "Electrical Discharge")
        {
            OnElectricalDischargeButtonClicked();
        }
        else if (abilityName == "Chain Lightning")
        {
            OnChainLightningButtonClicked();
        }

        else if (abilityName == "Primal Blast")
        {
            OnPrimalBlastButtonClicked();
        }

        else if (abilityName == "Primal Rage")
        {
            OnPrimalRageButtonClicked();
        }
        else if (abilityName == "Phase Shift")
        {
            OnPhaseShiftButtonClicked();
        }
        else if (abilityName == "Sanctity")
        {
            OnSanctityButtonClicked();
        }
        else if (abilityName == "Bless")
        {
            OnBlessButtonClicked();
        }
        else if (abilityName == "Siphon Life")
        {
            OnSiphonLifeButtonClicked();
        }
        else if (abilityName == "Chaos Bolt")
        {
            OnChaosBoltButtonClicked();
        }
    }
    public void OnMouseDown()
    {
        Defender selectedDefender = DefenderManager.Instance.selectedDefender;

        // this statment prevents the user from clicking through UI elements and selecting a defender
        if (!EventSystem.current.IsPointerOverGameObject() == false)
        {
            Debug.Log("Clicked on the UI, returning...");
            return;
        }

        if (selectedDefender != null && selectedDefender.awaitingInspireOrder)
        {
            selectedDefender.awaitingInspireOrder = false;
            selectedDefender.StartInspireProcess(this);
            return;
        }

        else if (selectedDefender != null && selectedDefender.awaitingGuardOrder)
        {
            selectedDefender.awaitingGuardOrder = false;
            selectedDefender.StartGuardProcess(this);
            return;
        }

        else if (selectedDefender != null && selectedDefender.awaitingTelekinesisTargetOrder)
        {
            selectedDefender.StartTelekinesisLocationSettingProcess(this);
        }

        else if (selectedDefender != null && selectedDefender.awaitingForestMedicineOrder)
        {
            selectedDefender.awaitingForestMedicineOrder = false;
            selectedDefender.StartForestMedicineProcess(this);
            return;
        }

        else if (selectedDefender != null && selectedDefender.awaitingInvigorateOrder)
        {
            selectedDefender.awaitingInvigorateOrder = false;
            selectedDefender.StartInvigorateProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingLightningShieldOrder)
        {
            selectedDefender.awaitingLightningShieldOrder = false;
            selectedDefender.StartLightningShieldProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingHolyFireOrder)
        {
            //selectedDefender.awaitingHolyFireOrder = false;
            selectedDefender.StartHolyFireProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingPrimalRageOrder)
        {
            //selectedDefender.awaitingHolyFireOrder = false;
            selectedDefender.StartPrimalRageProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingPhaseShiftOrder)
        {
            selectedDefender.StartPhaseShiftProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingSanctityOrder)
        {
            selectedDefender.StartSanctityProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingBlessOrder)
        {
            selectedDefender.StartBlessProcess(this);
            return;
        }

        else
        {
            Debug.Log("Defender selection attempt detected");
            if (ActivationManager.Instance.IsEntityActivated(this))
            {
                SelectDefender();
            }
            
        }

    }
    public void SelectDefender()
    {
        DefenderManager.Instance.SetSelectedDefender(this);
        myUIParent.SetActive(true);
    }
    public void UnselectDefender()
    {
        ClearAllOrders();
        myUIParent.SetActive(false);
    }
    public void ClearAllOrders()
    {
        awaitingMoveOrder = false;
        awaitingStrikeOrder = false;
        awaitingChargeTargetOrder = false;
        awaitingChargeLocationOrder = false;
        awaitingInspireOrder = false;
        awaitingGuardOrder = false;
        awaitingShootOrder = false;
        awaitingMeteorOrder = false;
        awaitingTelekinesisTargetOrder = false;
        awaitingTelekinesisLocationOrder = false;
        awaitingFrostBoltOrder = false;
        awaitingFireBallOrder = false;
        awaitingRapidFireOrder = false;
        awaitingImpalingBoltOrder = false;
        awaitingForestMedicineOrder = false;
        awaitingInvigorateOrder = false;
        awaitingHolyFireOrder = false;
        awaitingVoidBombOrder = false;
        awaitingNightmareOrder = false;
        awaitingTwinStrikeOrder = false;
        awaitingDashOrder = false;
        awaitingSliceAndDiceOrder = false;
        awaitingPoisonDartOrder = false;
        awaitingChemicalReactionOrder = false;
        awaitingGetDownOrder = false;
        awaitingSmashOrder = false;
        awaitingLightningShieldOrder = false;
        awaitingChainLightningOrder = false;
        awaitingPrimalBlastOrder = false;
        awaitingPrimalRageOrder = false;
        awaitingPhaseShiftOrder = false;
        awaitingSanctityOrder = false;
        awaitingBlessOrder = false;
        awaitingSiphonLifeOrder = false;

        myCurrentTarget = null;
    }


    // On ability button clicked events
    public void OnMoveButtonClicked()
    {
        Ability move = mySpellBook.GetAbilityByName("Move");

        if(IsAbleToMove() == false)
        {
            return;
        }

        // prevent movement if the character doesnt have enough AP
        if (HasEnoughAP(currentAP, move.abilityAPCost) == false)
        {
            if (myPassiveManager.FleetFooted && moveActionsTakenThisTurn == 0)
            {
                // nothing
            }
            else
            {
                return;
            }
            
        }
        Debug.Log("Move button clicked, awaiting move order");
        awaitingMoveOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetValidMoveableTilesWithinRange(currentMobility, LevelManager.Instance.Tiles[GridPosition]));
    }
    public void OnStrikeButtonClicked()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");

        if (HasEnoughAP(currentAP, strike.abilityAPCost) == false)
        {
            return;
        }

        Debug.Log("Strike button clicked, awaiting strike order");
        awaitingStrikeOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[GridPosition]));

    }
    public void OnSmashButtonClicked()
    {
        Ability smash = mySpellBook.GetAbilityByName("Smash");

        if (HasEnoughAP(currentAP, smash.abilityAPCost) == false)
        {
            return;
        }

        Debug.Log("Smash button clicked, awaiting smash order");
        awaitingSmashOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[GridPosition]));

    }
    public void OnInspireButtonClicked()
    {
        Ability inspire = mySpellBook.GetAbilityByName("Inspire");

        if (HasEnoughAP(currentAP, inspire.abilityAPCost) == false || IsAbilityOffCooldown(inspire.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Inspire button clicked, awaiting inspire order");
        awaitingInspireOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(inspire.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnChargeButtonClicked()
    {
        Ability charge = mySpellBook.GetAbilityByName("Charge");

        if (HasEnoughAP(currentAP, charge.abilityAPCost) == false
            || IsAbilityOffCooldown(charge.abilityCurrentCooldownTime) == false ||
            IsAbleToMove() == false)
        {
            return;
        }

        Debug.Log("Charge button clicked, awaiting charge target");
        awaitingChargeTargetOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(charge.abilityRange, LevelManager.Instance.Tiles[GridPosition]));

    }
    public void OnGetDownButtonClicked()
    {
        Ability getDown = mySpellBook.GetAbilityByName("Get Down!");

        if (HasEnoughAP(currentAP, getDown.abilityAPCost) == false
            || IsAbilityOffCooldown(getDown.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Get Down! button clicked, awaiting Get Down! target");
        awaitingGetDownOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(getDown.abilityRange, LevelManager.Instance.Tiles[GridPosition]));

    }
    public void OnLightningShieldClicked()
    {
        Ability lightningShield = mySpellBook.GetAbilityByName("Lightning Shield");

        if (HasEnoughAP(currentAP, lightningShield.abilityAPCost) == false
            || IsAbilityOffCooldown(lightningShield.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Lightning Shield button clicked, awaiting Lightning Shield target");
        awaitingLightningShieldOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(lightningShield.abilityRange, LevelManager.Instance.Tiles[GridPosition]));

    }
    public void OnGuardButtonClicked()
    {
        Ability guard = mySpellBook.GetAbilityByName("Guard");

        if (HasEnoughAP(currentAP, guard.abilityAPCost) == false || IsAbilityOffCooldown(guard.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Guard button clicked, awaiting guard target");
        awaitingGuardOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(guard.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnMeteorButtonClicked()
    {
        Ability meteor = mySpellBook.GetAbilityByName("Meteor");

        if (HasEnoughAP(currentAP, meteor.abilityAPCost) == false || IsAbilityOffCooldown(meteor.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Meteor button clicked, awaiting meteor target");
        awaitingMeteorOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(meteor.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnTelekinesisButtonClicked()
    {
        Ability telekinesis = mySpellBook.GetAbilityByName("Telekinesis");
        Debug.Log("Telekinesis button clicked, awaiting telekinesis target");

        if (HasEnoughAP(currentAP, telekinesis.abilityAPCost) == false || IsAbilityOffCooldown(telekinesis.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        awaitingTelekinesisTargetOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(telekinesis.abilityRange, LevelManager.Instance.Tiles[GridPosition]));
    }
    public void OnFrostBoltButtonClicked()
    {
        Ability frostbolt = mySpellBook.GetAbilityByName("Frost Bolt");

        if (HasEnoughAP(currentAP, frostbolt.abilityAPCost) == false || IsAbilityOffCooldown(frostbolt.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Frost Bolt button clicked, awaiting Frost Bolt target");
        awaitingFrostBoltOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(frostbolt.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnChainLightningButtonClicked()
    {
        Ability chainLightning = mySpellBook.GetAbilityByName("Chain Lightning");

        if (HasEnoughAP(currentAP, chainLightning.abilityAPCost) == false || IsAbilityOffCooldown(chainLightning.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Chain Lightning button clicked, awaiting Frost Bolt target");
        awaitingChainLightningOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(chainLightning.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnDashButtonClicked()
    {
        Ability dash = mySpellBook.GetAbilityByName("Dash");

        if (HasEnoughAP(currentAP, dash.abilityAPCost) == false || 
            IsAbilityOffCooldown(dash.abilityCurrentCooldownTime) == false ||
            IsAbleToMove() == false)
        {
            return;
        }

        Debug.Log("Dash button clicked, awaiting Dash tile target");
        awaitingDashOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(dash.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnFireBallButtonClicked()
    {
        Ability fireball = mySpellBook.GetAbilityByName("Fire Ball");

        if (HasEnoughAP(currentAP, fireball.abilityAPCost) == false || IsAbilityOffCooldown(fireball.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Fire Ball button clicked, awaiting Frost Bolt target");
        awaitingFireBallOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(fireball.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnShootButtonClicked()
    {
        Ability shoot = mySpellBook.GetAbilityByName("Shoot");

        if (HasEnoughAP(currentAP, shoot.abilityAPCost) == false || IsAbilityOffCooldown(shoot.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Shoot button clicked, awaiting Shoot target");
        awaitingShootOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(shoot.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnRapidFireButtonClicked()
    {
        Ability rapidFire = mySpellBook.GetAbilityByName("Rapid Fire");

        if (HasEnoughAP(currentAP, rapidFire.abilityAPCost) == false || IsAbilityOffCooldown(rapidFire.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Rapid Fire button clicked, awaiting Rapid Fire target");
        awaitingRapidFireOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(rapidFire.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnSliceAndDiceButtonClicked()
    {
        Ability sliceAndDice = mySpellBook.GetAbilityByName("Slice And Dice");

        if (HasEnoughAP(currentAP, sliceAndDice.abilityAPCost) == false || IsAbilityOffCooldown(sliceAndDice.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Slice And Dice button clicked, awaiting Slice and Dice target");
        awaitingSliceAndDiceOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[GridPosition]));
    }
    public void OnPoisonDartButtonClicked()
    {
        Ability poisonDart = mySpellBook.GetAbilityByName("Poison Dart");

        if (HasEnoughAP(currentAP, poisonDart.abilityAPCost) == false || IsAbilityOffCooldown(poisonDart.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Poison Dart button clicked, awaiting Poison Dart target");
        awaitingPoisonDartOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(poisonDart.abilityRange, LevelManager.Instance.Tiles[GridPosition]));
    }
    public void OnChemicalReactionButtonClicked()
    {
        Ability chemicalReaction = mySpellBook.GetAbilityByName("Chemical Reaction");

        if (HasEnoughAP(currentAP, chemicalReaction.abilityAPCost) == false || IsAbilityOffCooldown(chemicalReaction.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Chemical Reaction button clicked, awaiting Chemical Reaction target");
        awaitingChemicalReactionOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(chemicalReaction.abilityRange, LevelManager.Instance.Tiles[GridPosition]));
    }
    public void OnImpalingBoltButtonClicked()
    {
        Ability imaplingBolt = mySpellBook.GetAbilityByName("Impaling Bolt");

        if (HasEnoughAP(currentAP, imaplingBolt.abilityAPCost) == false || IsAbilityOffCooldown(imaplingBolt.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Impaling Bolt button clicked, awaiting Impaling Bolt target");
        awaitingImpalingBoltOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(imaplingBolt.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnForestMedicineButtonClicked()
    {
        Ability forestMedicine = mySpellBook.GetAbilityByName("Forest Medicine");

        if (HasEnoughAP(currentAP, forestMedicine.abilityAPCost) == false || IsAbilityOffCooldown(forestMedicine.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Forest Medicine button clicked, awaiting Forest Medicine target");
        awaitingForestMedicineOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(forestMedicine.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnInvigorateButtonClicked()
    {
        Ability invigorate = mySpellBook.GetAbilityByName("Invigorate");

        if (HasEnoughAP(currentAP, invigorate.abilityAPCost) == false || IsAbilityOffCooldown(invigorate.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Invigorate button clicked, awaiting Invigorate target");
        awaitingInvigorateOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(invigorate.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnHolyFireButtonClicked()
    {
        Ability holyFire = mySpellBook.GetAbilityByName("Holy Fire");

        if (HasEnoughAP(currentAP, holyFire.abilityAPCost) == false || IsAbilityOffCooldown(holyFire.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Holy Fire button clicked, awaiting Holy Fire target");
        awaitingHolyFireOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(holyFire.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnPrimalBlastButtonClicked()
    {
        Ability primalBlast = mySpellBook.GetAbilityByName("Primal Blast");

        if (HasEnoughAP(currentAP, primalBlast.abilityAPCost) == false || IsAbilityOffCooldown(primalBlast.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Primal Blast button clicked, awaiting Primal Blast target");
        awaitingPrimalBlastOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(primalBlast.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnPrimalRageButtonClicked()
    {
        Ability primalBlast = mySpellBook.GetAbilityByName("Primal Rage");

        if (HasEnoughAP(currentAP, primalBlast.abilityAPCost) == false || IsAbilityOffCooldown(primalBlast.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Primal Rage button clicked, awaiting Primal Rage target");
        awaitingPrimalRageOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(primalBlast.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnPhaseShiftButtonClicked()
    {
        Ability phaseShift = mySpellBook.GetAbilityByName("Phase Shift");

        if (HasEnoughAP(currentAP, phaseShift.abilityAPCost) == false || IsAbilityOffCooldown(phaseShift.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Phase Shift button clicked, awaiting Phase Shift target");
        awaitingPhaseShiftOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(phaseShift.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnSanctityButtonClicked()
    {
        Ability sanctity = mySpellBook.GetAbilityByName("Sanctity");

        if (HasEnoughAP(currentAP, sanctity.abilityAPCost) == false || IsAbilityOffCooldown(sanctity.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Sanctity button clicked, awaiting Sanctity target");
        awaitingSanctityOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(sanctity.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnBlessButtonClicked()
    {
        Ability bless = mySpellBook.GetAbilityByName("Bless");

        if (HasEnoughAP(currentAP, bless.abilityAPCost) == false || IsAbilityOffCooldown(bless.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Bless button clicked, awaiting Bless target");
        awaitingBlessOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(bless.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnSiphonLifeButtonClicked()
    {
        Ability siphonLife = mySpellBook.GetAbilityByName("Siphon Life");

        if (HasEnoughAP(currentAP, siphonLife.abilityAPCost) == false || IsAbilityOffCooldown(siphonLife.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Siphon Life button clicked, awaiting Siphon Life target");
        awaitingSiphonLifeOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(siphonLife.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnVoidBombButtonClicked()
    {
        Ability voidBomb = mySpellBook.GetAbilityByName("Void Bomb");

        if (HasEnoughAP(currentAP, voidBomb.abilityAPCost) == false || IsAbilityOffCooldown(voidBomb.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Void Bomb button clicked, awaiting Void Bomb target");
        awaitingVoidBombOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(voidBomb.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnNightmareButtonClicked()
    {
        Ability nightmare = mySpellBook.GetAbilityByName("Nightmare");

        if (HasEnoughAP(currentAP, nightmare.abilityAPCost) == false || IsAbilityOffCooldown(nightmare.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Nightmare button clicked, awaiting Nightmare target");
        awaitingNightmareOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(nightmare.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnWhirlwindButtonClicked()
    {
        Debug.Log("Whirlwind button clicked");

        Ability whirlwind = mySpellBook.GetAbilityByName("Whirlwind");

        if (HasEnoughAP(currentAP, whirlwind.abilityAPCost) == false || IsAbilityOffCooldown(whirlwind.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        CombatLogic.Instance.CreateAoEAttackEvent(this, whirlwind, TileCurrentlyOn, currentMeleeRange, true, false);

        OnAbilityUsed(whirlwind, this);            
      
    }
    public void OnElectricalDischargeButtonClicked()
    {
        Debug.Log("Electrical Discharge button clicked");

        Ability electricalDischarge = mySpellBook.GetAbilityByName("Electrical Discharge");

        if (HasEnoughAP(currentAP, electricalDischarge.abilityAPCost) == false || IsAbilityOffCooldown(electricalDischarge.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        // damage enemies
        CombatLogic.Instance.CreateAoEAttackEvent(this, electricalDischarge, TileCurrentlyOn, currentMeleeRange, true, false);

        // give block to allies
        List<TileScript> tilesInMyMeleeRange = LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, TileCurrentlyOn);
        foreach(Defender defender in DefenderManager.Instance.allDefenders)
        {
            if (tilesInMyMeleeRange.Contains(defender.TileCurrentlyOn))
            {
                defender.ModifyCurrentBlock(electricalDischarge.abilityPrimaryValue);
            }
        }

        OnAbilityUsed(electricalDischarge, this);

    }
    public void OnBlockButtonClicked()
    {
        Debug.Log("Block button clicked");

        Ability block = mySpellBook.GetAbilityByName("Block");

        if (HasEnoughAP(currentAP, block.abilityAPCost) == false || IsAbilityOffCooldown(block.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        ModifyCurrentBlock(block.abilityPrimaryValue);

        OnAbilityUsed(block, this);

    }
    public void OnBloodLustButtonClicked()
    {
        Debug.Log("Blood Lust button clicked");

        Ability bloodLust = mySpellBook.GetAbilityByName("Blood Lust");

        if (HasEnoughAP(currentAP, bloodLust.abilityAPCost) == false || 
            IsAbilityOffCooldown(bloodLust.abilityCurrentCooldownTime) == false ||
            currentHealth <= bloodLust.abilitySecondaryValue)
        {
            return;
        }

        // TO DO: this need to be chaged in future to handledamage() when we have set up damage types
        ModifyCurrentHealth(-bloodLust.abilitySecondaryValue);
        //HandleDamage(bloodLust.abilitySecondaryValue, this);
        ModifyCurrentAP(bloodLust.abilityPrimaryValue);

        OnAbilityUsed(bloodLust, this);

    }
    public void OnPreparationButtonClicked()
    {
        Debug.Log("Preparation button clicked");

        Ability preparation = mySpellBook.GetAbilityByName("Preparation");

        if (HasEnoughAP(currentAP, preparation.abilityAPCost) == false || IsAbilityOffCooldown(preparation.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        
        OnAbilityUsed(preparation, this);
        myPassiveManager.LearnPreparation(1);

    }
    public void OnTwinStrikeButtonClicked()
    {
        Ability twinStrike = mySpellBook.GetAbilityByName("Twin Strike");

        if (HasEnoughAP(currentAP, twinStrike.abilityAPCost) == false || IsAbilityOffCooldown(twinStrike.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Twin Strike button clicked, awaiting Twin Strike target");
        awaitingTwinStrikeOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[GridPosition], false));
    }
    public void OnChaosBoltButtonClicked()
    {
        Ability chaosBolt = mySpellBook.GetAbilityByName("Chaos Bolt");

        if (HasEnoughAP(currentAP, chaosBolt.abilityAPCost) == false || IsAbilityOffCooldown(chaosBolt.abilityCurrentCooldownTime) == false)
        {
            return;
        }

        Debug.Log("Chaos Bolt button clicked, awaiting Chaos Bolt target");
        awaitingChaosBoltOrder = true;
        LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(chaosBolt.abilityRange, LevelManager.Instance.Tiles[GridPosition], false));
    }


    // Stat and property modifiers
    public override void ModifyCurrentMobility(int mobilityGainedOrLost)
    {
        base.ModifyCurrentMobility(mobilityGainedOrLost);
        UpdateCurrentMobilityStatText(currentMobility);
    }
    public override void ModifyCurrentAP(int APGainedOrLost, bool showVFX = true)
    {
        base.ModifyCurrentAP(APGainedOrLost, showVFX);
        UpdateCurrentAPText(currentAP);
    }
    public override void ModifyCurrentStrength(int strengthGainedOrLost)
    {
        base.ModifyCurrentStrength(strengthGainedOrLost);
        UpdateCurrentStrengthStatText(currentStrength);
    }


    // Text and visual updates
    public void UpdateCurrentStrengthStatText(int newValue)
    {
        myCurrentStrengthStatText.text = newValue.ToString();
    }       
    public void UpdateCurrentAPText(int newAPValue)
    {
        myCurrentAPText.text = newAPValue.ToString();
    }
    public void UpdateCurrentAPStatText(int newAPStatValue)
    {
        myCurrentAPStatText.text = newAPStatValue.ToString();
    }
    public void UpdateCurrentMaxAPText(int newMaxAPValue)
    {
        myCurrentMaxAPText.text = newMaxAPValue.ToString();
    }
    public void UpdateCurrentMobilityStatText(int newMobilityValue)
    {
        myCurrentMobilityStatText.text = newMobilityValue.ToString();
    }


    // Start proccesses to check if an ability usage is valid
    public void StartMoveAbilityProcess(TileScript destination)
    {
        // If the selected tile is within our movement range, is walkable, and unoccupied, the attempted move is valid: start moving
        if (MovementLogic.Instance.IsLocationMoveable(destination, this, currentMobility))
        {
            Debug.Log("Selected tile is valid, starting move...");
            awaitingMoveOrder = false;
            AbilityLogic.Instance.PerformMove(this, destination);
            
        }
    }
    public void StartDashProcess(TileScript destination)
    {
        Ability dash = mySpellBook.GetAbilityByName("Dash");

        // If the selected tile is within our movement range, is walkable, and unoccupied, the attempted move is valid: start moving
        if (MovementLogic.Instance.IsLocationMoveable(destination, this, dash.abilityRange))
        {
            Debug.Log("Selected tile is valid, starting move...");
            awaitingDashOrder = false;
            AbilityLogic.Instance.PerformDash(this, destination);
        }
    }
    public void StartGetDownProcess(TileScript destination)
    {
        Ability getDown = mySpellBook.GetAbilityByName("Get Down!");

        // If the selected tile is within our movement range, is walkable, and unoccupied, the attempted move is valid: start moving
        if (MovementLogic.Instance.IsLocationMoveable(destination, this, getDown.abilityRange))
        {
            Debug.Log("Selected tile is valid, starting move...");
            awaitingGetDownOrder = false;
            AbilityLogic.Instance.PerformGetDown(this, destination);
        }
    }
    public void StartStrikeProcess()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");

        Debug.Log("Defender.StartStrikeProcess() called");
        Enemy enemyTarget = EnemyManager.Instance.selectedEnemy;
        if (IsTargetInRange(enemyTarget, currentMeleeRange))
        {
            awaitingStrikeOrder = false;
            AbilityLogic.Instance.PerformStrike(this, enemyTarget);
        }
    }
    public void StartSmashProcess(LivingEntity target)
    {
        Ability smash = mySpellBook.GetAbilityByName("Smash");

        Debug.Log("Defender.StartSmashProcess() called");       
        if (IsTargetInRange(target, currentMeleeRange))
        {
            awaitingSmashOrder = false;            
            AbilityLogic.Instance.PerformSmash(this, target);
        }
    }
    public void StartChainLightningProcess(LivingEntity target)
    {
        Ability chainLightning = mySpellBook.GetAbilityByName("Chain Lightning");

        Debug.Log("Defender.ChainLightningProcess() called");
        if (IsTargetInRange(target,chainLightning.abilityRange))
        {
            awaitingChainLightningOrder = false;            
            //StartCoroutine(PerformChainLightning(target));
            AbilityLogic.Instance.PerformChainLightning(this, target);
        }
    }
    public void StartPrimalBlastProcess(LivingEntity target)
    {
        Ability primalBlast = mySpellBook.GetAbilityByName("Primal Blast");

        Debug.Log("Defender.ChainLightningProcess() called");
        if (IsTargetInRange(target, primalBlast.abilityRange))
        {
            awaitingPrimalBlastOrder = false;
            
            AbilityLogic.Instance.PerformPrimalBlast(this, target);
        }
    }
    public void StartMeteorProcess(TileScript targetTile)
    {        
        awaitingMeteorOrder = false;
        AbilityLogic.Instance.PerformMeteor(this, targetTile);       
    }
    public void StartInspireProcess(Defender targetOfInspire)
    {
        Debug.Log("Defender.StartInspireProcess() called");
        Ability inspire = mySpellBook.GetAbilityByName("Inspire");

        if (IsAllyInRange(targetOfInspire, inspire.abilityRange))
        {
            AbilityLogic.Instance.PerformInspire(this, targetOfInspire);
        }
    }
    public void StartChargeLocationSettingProcess()
    {
        Ability charge = mySpellBook.GetAbilityByName("Charge");

        Debug.Log("Defender.StartChargeLocationSettingProcess() called");
        Enemy enemyTarget = EnemyManager.Instance.selectedEnemy;
        if (IsTargetInRange(enemyTarget, charge.abilityRange))
        {
            awaitingChargeLocationOrder = true;
            awaitingChargeTargetOrder = false;

            List<TileScript> tilesWithinChargeRangeOfCharacter = LevelManager.Instance.GetValidMoveableTilesWithinRange(charge.abilityRange, TileCurrentlyOn);
            List<TileScript> tilesWithinMeleeRangeOfTarget = LevelManager.Instance.GetValidMoveableTilesWithinRange(1, enemyTarget.TileCurrentlyOn);
            List<TileScript> validChargeLocationTiles = new List<TileScript>();

            foreach (TileScript tile in tilesWithinMeleeRangeOfTarget)
            {
                if (tilesWithinChargeRangeOfCharacter.Contains(tile))
                {
                    validChargeLocationTiles.Add(tile);
                }
            }

            LevelManager.Instance.UnhighlightAllTiles();
            LevelManager.Instance.HighlightTiles(validChargeLocationTiles);

        }
    }
    public void StartChargeProcess(TileScript destination)
    {
        Enemy enemyTarget = EnemyManager.Instance.selectedEnemy;
        Ability charge = mySpellBook.GetAbilityByName("Charge");

        List<TileScript> tilesWithinChargeRangeOfCharacter = LevelManager.Instance.GetValidMoveableTilesWithinRange(charge.abilityRange, TileCurrentlyOn);
        List<TileScript> tilesWithinMeleeRangeOfTarget = LevelManager.Instance.GetValidMoveableTilesWithinRange(1, enemyTarget.TileCurrentlyOn);
        List<TileScript> validChargeLocationTiles = new List<TileScript>();

        foreach (TileScript tile in tilesWithinMeleeRangeOfTarget)
        {
            if (tilesWithinChargeRangeOfCharacter.Contains(tile))
            {
                validChargeLocationTiles.Add(tile);
            }
        }

        if (validChargeLocationTiles.Contains(destination))
        {
            awaitingChargeLocationOrder = false;
            awaitingChargeTargetOrder = false;
            AbilityLogic.Instance.PerformCharge(this, enemyTarget, destination);
        }
        
    }
    public void StartGuardProcess(LivingEntity targetOfGuard)
    {
        Ability guard = mySpellBook.GetAbilityByName("Guard");

        if (IsAllyInRange(targetOfGuard, guard.abilityRange))
        {
            awaitingGuardOrder = false;
            AbilityLogic.Instance.PerformGuard(this, targetOfGuard);
        }
    }
    public void StartTelekinesisProcess(LivingEntity target, TileScript destination)
    {        
        Ability telekinesis = mySpellBook.GetAbilityByName("Telekinesis");
        
        List<TileScript> validTeleportLocations = LevelManager.Instance.GetValidMoveableTilesWithinRange(telekinesis.abilityPrimaryValue, target.TileCurrentlyOn);

        if (validTeleportLocations.Contains(destination))
        {
            awaitingTelekinesisTargetOrder = false;
            awaitingTelekinesisLocationOrder = false;
            AbilityLogic.Instance.PerformTelekinesis(this, target, destination);
        }

    }
    public void StartTelekinesisLocationSettingProcess(LivingEntity targetBeingTeleported)
    {
        Ability telekinesis = mySpellBook.GetAbilityByName("Telekinesis");

        Debug.Log("Defender.StartTelekinesisLocationSettingProcess() called");
        //Enemy target = EnemyManager.Instance.selectedEnemy;
        LivingEntity target = targetBeingTeleported;
        if (IsTargetInRange(target, telekinesis.abilityRange))
        {
            awaitingTelekinesisTargetOrder = false;
            awaitingTelekinesisLocationOrder = true;

            List<TileScript> tilesWithinTeleportingRangeOfTarget = LevelManager.Instance.GetValidMoveableTilesWithinRange(telekinesis.abilityPrimaryValue, target.TileCurrentlyOn);

            LevelManager.Instance.UnhighlightAllTiles();
            LevelManager.Instance.HighlightTiles(tilesWithinTeleportingRangeOfTarget);
            myCurrentTarget = target;
        }
    }
    public void StartFrostBoltProcess()
    {
        Ability frostbolt = mySpellBook.GetAbilityByName("Frost Bolt");
        Debug.Log("Defender.StartFrostBoltProcess() called");
        Enemy enemyTarget = EnemyManager.Instance.selectedEnemy;
        if (IsTargetInRange(enemyTarget, frostbolt.abilityRange))
        {                      
            awaitingFrostBoltOrder = false;            
            AbilityLogic.Instance.PerformFrostBolt(this, enemyTarget);
        }
    }
    public void StartFireBallProcess(LivingEntity target)
    {
        Ability fireball = mySpellBook.GetAbilityByName("Fire Ball");
        Debug.Log("Defender.StartFireBallProcess() called");        
        if (IsTargetInRange(target, fireball.abilityRange))
        {
            awaitingFireBallOrder = false;
            AbilityLogic.Instance.PerformFireBall(this, target);                       
        }
    }
    public void StartShootProcess()
    {
        Ability shoot = mySpellBook.GetAbilityByName("Shoot");
        Debug.Log("Defender.StartShootProcess() called");
        Enemy enemyTarget = EnemyManager.Instance.selectedEnemy;
        if (IsTargetInRange(enemyTarget, shoot.abilityRange))
        {
            //PerformShoot(enemyTarget, CalculateDamage(shoot.abilityPrimaryValue, enemyTarget, this));
            awaitingShootOrder = false;
            AbilityLogic.Instance.PerformShoot(this, enemyTarget);
        }
    }
    public void StartRapidFireProcess()
    {
        Ability rapidFire = mySpellBook.GetAbilityByName("Rapid Fire");
        Debug.Log("Defender.StartRapidFireProcess() called");
        Enemy enemyTarget = EnemyManager.Instance.selectedEnemy;
        if (IsTargetInRange(enemyTarget, rapidFire.abilityRange))
        {
            awaitingRapidFireOrder = false;
            AbilityLogic.Instance.PerformRapidFire(this, enemyTarget, currentAP);
            //StartCoroutine(PerformRapidFire(enemyTarget, CalculateDamage(rapidFire.abilityPrimaryValue, enemyTarget, this), currentAP));
        }
    }
    public void StartSliceAndDiceProcess(Enemy target)
    {
        Ability sliceAndDice = mySpellBook.GetAbilityByName("Slice And Dice");
        Debug.Log("Defender.StartSliceAndDiceProcess() called");
        Enemy enemyTarget = target;
        if (IsTargetInRange(enemyTarget, currentMeleeRange))
        {
            awaitingSliceAndDiceOrder = false;
            AbilityLogic.Instance.PerformSliceAndDice(this, target, currentAP);
        }
    }
    public void StartPoisonDartProcess(Enemy target)
    {
        Ability poisonDart = mySpellBook.GetAbilityByName("Poison Dart");
        Debug.Log("Defender.StartPoisonDartProcess() called");
        Enemy enemyTarget = target;
        if (IsTargetInRange(enemyTarget, poisonDart.abilityRange))
        {
            awaitingPoisonDartOrder = false;
            AbilityLogic.Instance.PerformPoisonDart(this, target);
            //StartCoroutine(PerformPoisonDart(enemyTarget));

        }
    }
    public void StartChemicalReactionProcess(Enemy target)
    {
        Ability chemicalReaction = mySpellBook.GetAbilityByName("Chemical Reaction");
        Debug.Log("Defender.ChemicalReactionProcess() called");
        Enemy enemyTarget = target;
        if (IsTargetInRange(enemyTarget, chemicalReaction.abilityRange))
        {
            awaitingChemicalReactionOrder = false;
            LevelManager.Instance.UnhighlightAllTiles();
            AbilityLogic.Instance.PerformChemicalReaction(this, target);
            //StartCoroutine(PerformChemicalReaction(enemyTarget));
        }
    }
    public void StartImpalingBoltProcess()
    {
        Ability impalingBolt = mySpellBook.GetAbilityByName("Impaling Bolt");
        Debug.Log("Defender.StartRapidFireProcess() called");
        Enemy enemyTarget = EnemyManager.Instance.selectedEnemy;
        if (IsTargetInRange(enemyTarget, impalingBolt.abilityRange))
        {
            awaitingImpalingBoltOrder = false;            
            AbilityLogic.Instance.PerformImpalingBolt(this, enemyTarget);
        }
    }
    public void StartForestMedicineProcess(Defender target)
    {
        Ability forestMedicine = mySpellBook.GetAbilityByName("Forest Medicine");
        target.ModifyCurrentBlock(forestMedicine.abilityPrimaryValue);
        OnAbilityUsed(forestMedicine, this);
    }
    public void StartInvigorateProcess(Defender target)
    {
        Ability invigorate = mySpellBook.GetAbilityByName("Invigorate");
        if (IsTargetInRange(target, invigorate.abilityRange))
        {
            awaitingInvigorateOrder = false;
            AbilityLogic.Instance.PerformInvigorate(this, target);
        }
        
    }
    public void StartLightningShieldProcess(Defender target)
    {
        Ability lightningShield = mySpellBook.GetAbilityByName("Lightning Shield");
        if (IsTargetInRange(target, lightningShield.abilityRange))
        {
            awaitingLightningShieldOrder = false;
            AbilityLogic.Instance.PerformLightningShield(this, target);
        }       
            
    }
    public void StartHolyFireProcess(LivingEntity target)
    {   
        Ability holyFire = mySpellBook.GetAbilityByName("Holy Fire");

        if (IsTargetInRange(target, holyFire.abilityRange))
        {
            awaitingHolyFireOrder = false;
            AbilityLogic.Instance.PerformHolyFire(this, target);
        }
    }
    public void StartPrimalRageProcess(LivingEntity target)
    {        
        Ability primalRage = mySpellBook.GetAbilityByName("Primal Rage");

        if (IsTargetInRange(target, primalRage.abilityRange) == false)
        {
            return;
        }
                
        awaitingPrimalRageOrder = false;
        AbilityLogic.Instance.PerformPrimalRage(this, target);
    }
    public void StartPhaseShiftProcess(LivingEntity target)
    {
        Ability phaseShift = mySpellBook.GetAbilityByName("Phase Shift");

        if (IsTargetInRange(target, phaseShift.abilityRange) == false)
        {
            return;
        }
        
        awaitingPhaseShiftOrder = false;
        AbilityLogic.Instance.PerformPhaseShift(this, target);
          
    }
    public void StartSiphonLifeProcess(LivingEntity target)
    {
        Ability siphonLife = mySpellBook.GetAbilityByName("Siphon Life");

        if (IsTargetInRange(target, siphonLife.abilityRange) == false)
        {
            return;
        }

        awaitingSiphonLifeOrder = false;
        AbilityLogic.Instance.PerformSiphonLife(this, target);
        // StartCoroutine(PerformSiphonLife(target));
    }
    public void StartChaosBoltProcess(LivingEntity target)
    {
        Ability chaosBolt = mySpellBook.GetAbilityByName("Chaos Bolt");

        if (IsTargetInRange(target, chaosBolt.abilityRange) == false)
        {
            return;
        }

        awaitingChaosBoltOrder = false;
        AbilityLogic.Instance.PerformChaosBolt(this, target);
        // StartCoroutine(PerformSiphonLife(target));
    }
    public void StartSanctityProcess(LivingEntity target)
    {
        Ability sanctity = mySpellBook.GetAbilityByName("Sanctity");

        if (IsTargetInRange(target, sanctity.abilityRange) == false)
        {
            return;
        }

        awaitingSanctityOrder = false;
        AbilityLogic.Instance.PerformSanctity(this, target);
        //StartCoroutine(PerformSanctity(target));

    }
    public void StartBlessProcess(LivingEntity target)
    {
        Ability bless = mySpellBook.GetAbilityByName("Bless");

        if (IsTargetInRange(target, bless.abilityRange) == false)
        {
            return;
        }

        awaitingBlessOrder = false;
        AbilityLogic.Instance.PerformBless(this, target);
        //StartCoroutine(PerformBless(target));

    }
    public void StartVoidBombProcess(LivingEntity target)
    {
        Ability voidBomb = mySpellBook.GetAbilityByName("Void Bomb");

        if (IsTargetInRange(target, voidBomb.abilityRange))
        {
            awaitingVoidBombOrder = false;
            AbilityLogic.Instance.PerformVoidBomb(this, target);
        }             
    }
    public void StartNightmareProcess(LivingEntity target)
    {
        Ability nightmare = mySpellBook.GetAbilityByName("Nightmare");
        if (IsTargetInRange(target, nightmare.abilityRange))
        {
            awaitingNightmareOrder = false;
            AbilityLogic.Instance.PerformNightmare(this, target);
        }
        
    }
    public void StartTwinStrikeProcess(LivingEntity target)
    {
        Ability twinStrike = mySpellBook.GetAbilityByName("Twin Strike");

        Debug.Log("Defender.StartTwinStrikeProcess() called");
        //Enemy enemyTarget = EnemyManager.Instance.selectedEnemy;
        if (IsTargetInRange(target, currentMeleeRange))
        {
            awaitingTwinStrikeOrder = false;
            AbilityLogic.Instance.PerformTwinStrike(this, target);
        }
    }

          
    // Bool functions and conditional checks
    public bool IsAllyInRange(LivingEntity defender, int range)
    {
        List<TileScript> tilesWithinMyRange = LevelManager.Instance.GetTilesWithinRange(range, TileCurrentlyOn, false);
        TileScript allyTargetsTile = defender.TileCurrentlyOn;

        if (tilesWithinMyRange.Contains(allyTargetsTile))
        {
            Debug.Log("Target ally is range");
            return true;
        }
        else
        {
            Debug.Log("Target ally is NOT range");
            return false;
        }
    }
    









}
