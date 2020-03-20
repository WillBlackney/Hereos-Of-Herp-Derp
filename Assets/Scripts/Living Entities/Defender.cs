using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Defender : LivingEntity
{
    [Header("Defender Component References ")]
    public Slider myEnergyBar;
    public Slider myHealthBarStatPanel;
    public AbilityBar myAbilityBar;
    public GameObject myUIParent;
    public CharacterData myCharacterData;
    public TextMeshProUGUI myCurrentEnergyText;
    public TextMeshProUGUI myCurrentMaxEnergyText;
    public TextMeshProUGUI myCurrentStrengthStatText;
    public TextMeshProUGUI myCurrentMobilityStatText;
    public TextMeshProUGUI myCurrentStaminaStatText;
    public TextMeshProUGUI myCurrentHealthTextStatBar;
    public TextMeshProUGUI myCurrentMaxHealthTextStatBar;
    public TextMeshProUGUI myCurrentEnergyBarText;
    public TextMeshProUGUI myCurrentMaxEnergyBarText;


    [Header("Ability Orders")]
    public bool awaitingAnOrder;
    public bool awaitingMoveOrder;
    public bool awaitingStrikeOrder;
    public bool awaitingChargeTargetOrder;
    public bool awaitingChargeLocationOrder;
    public bool awaitingInspireOrder;
    public bool awaitingGuardOrder;
    public bool awaitingShootOrder;
    public bool awaitingMeteorOrder;
    public bool awaitingTelekinesisTargetOrder;
    public bool awaitingTelekinesisLocationOrder;
    public bool awaitingFrostBoltOrder;
    public bool awaitingFireBallOrder;
    public bool awaitingRapidFireOrder;
    public bool awaitingImpalingBoltOrder;
    public bool awaitingForestMedicineOrder;
    public bool awaitingInvigorateOrder;
    public bool awaitingHolyFireOrder;
    public bool awaitingVoidBombOrder;
    public bool awaitingNightmareOrder;
    public bool awaitingTwinStrikeOrder;
    public bool awaitingDashOrder;
    public bool awaitingSliceAndDiceOrder;
    public bool awaitingPoisonDartOrder;
    public bool awaitingChemicalReactionOrder;
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
    public bool awaitingShadowBlastOrder;
    public bool awaitingMeltOrder;

    public bool awaitingKickToTheBallsOrder;
    public bool awaitingDevastatingBlowOrder;
    public bool awaitingBladeFlurryOrder;
    public bool awaitingEvasionOrder;
    public bool awaitingShieldSlamOrder;
    public bool awaitingTendonSlashOrder;
    public bool awaitingShieldShatterOrder;
    public bool awaitingSwordAndBoardOrder;
    public bool awaitingPhoenixDiveOrder;
    public bool awaitingChillingBlowOrder;
    public bool awaitingIcyFocusOrder;
    public bool awaitingCombustionOrder;
    public bool awaitingDragonBreathOrder;
    public bool awaitingBlizzardOrder;
    public bool awaitingFrostArmourOrder;
    public bool awaitingThawOrder;
    public bool awaitingSnipeOrder;
    public bool awaitingHasteOrder;
    public bool awaitingSteadyHandsOrder;
    public bool awaitingTreeLeapOrder;
    public bool awaitingDimensionalBlastOrder;
    public bool awaitingMirageOrder;
    public bool awaitingBurstOfKnowledgeOrder;
    public bool awaitingBlinkOrder;
    public bool awaitingTimeWarpOrder;
    public bool awaitingDimensionalHexOrder;
    public bool awaitingBlindingLightOrder;
    public bool awaitingTranscendenceOrder;
    public bool awaitingJudgementOrder;
    public bool awaitingShroudOrder;
    public bool awaitingRainOfChaosOrder;
    public bool awaitingBlightOrder;
    public bool awaitingToxicSlashOrder;
    public bool awaitingToxicEruptionOrder;
    public bool awaitingDrainOrder;
    public bool awaitingLightningBoltOrder;
    public bool awaitingThunderStrikeOrder;
    public bool awaitingSpiritVisionOrder;
    public bool awaitingThunderStormOrder;
    public bool awaitingConcealingCloudsOrder;
    public bool awaitingHeadShotOrder;
    public bool awaitingHexOrder;
    public bool awaitingShankOrder;
    public bool awaitingCheapShotOrder;
    public bool awaitingAmbushOrder;
    public bool awaitingShadowStepOrder;
    public bool awaitingProvokeOrder;
    public bool awaitingDecapitateOrder;
    public bool awaitingDisarmOrder;
    public bool awaitingGoBerserkOrder;
    public bool awaitingFortifyOrder;
    public bool awaitingStoneFormOrder;
    public bool awaitingBackStabOrder;
    public bool awaitingChloroformBombOrder;
    public bool awaitingPinningShotOrder;
    public bool awaitingMarkTargetOrder;
    public bool awaitingSnowStasisOrder;
    public bool awaitingOverloadOrder;
    public bool awaitingBlazeOrder;
    public bool awaitingCreepingFrostOrder;
    public bool awaitingShadowWreathOrder;
    public bool awaitingDarkGiftOrder;
    public bool awaitingSuperConductorOrder;

    [Header("Update Related Properties")]
    public bool energyBarPositionCurrentlyUpdating;
    public bool healthBarPositionCurrentlyUpdating;

    // Initialization + Setup
    #region
    public override void InitializeSetup(Point startingGridPosition, Tile startingTile)
    {
        DefenderManager.Instance.allDefenders.Add(this);
        myName = myCharacterData.myClass;

        myUIParent.GetComponent<Canvas>().worldCamera = Camera.main;
        myUIParent.SetActive(false);

        transform.SetParent(DefenderManager.Instance.defendersParent.transform);
        base.InitializeSetup(startingGridPosition, startingTile);
    }
    public override void SetBaseProperties()
    {
        // Build view model
        CharacterModelController.BuildModelFromPresetString(myModel, myCharacterData.myClass);

        // Get and build from relevant character data values
        RunSetupFromCharacterData();
        base.SetBaseProperties();

        // Set up visuals
        UpdateCurrentEnergyText(currentEnergy);
        UpdateCurrentMaxEnergyText(currentMaxEnergy);
        UpdateCurrentStaminaText(currentStamina);        
        UpdateCurrentStrengthStatText(currentStrength);
        UpdateCurrentMobilityStatText(currentMobility);
        mySpellBook.SetNewAbilityDescriptions();
        
    }
    public void RunSetupFromCharacterData()
    {
        Debug.Log("RunSetupFromCharacterData() called...");

        // Establish connection from defender script to character data
        myCharacterData.myDefenderGO = this;

        // Setup Core Stats
        baseMaxHealth = myCharacterData.maxHealth;
        baseStartingHealth = myCharacterData.currentHealth;
        baseStrength = myCharacterData.strength;
        baseWisdom = myCharacterData.wisdom;
        baseDexterity = myCharacterData.dexterity;
        baseMobility = myCharacterData.mobility;
        baseStamina = myCharacterData.stamina;
        baseInitiative = myCharacterData.initiative;

        // Setup Secondary Stats
        baseCriticalChance = myCharacterData.criticalChance;
        baseDodgeChance = myCharacterData.dodge;
        baseParryChance = myCharacterData.parry;
        baseAuraSize = myCharacterData.auraSize;
        baseMaxEnergy = myCharacterData.maxEnergy;
        baseMeleeRange = myCharacterData.meleeRange;

        // Setup Resistances
        basePhysicalResistance = myCharacterData.physicalResistance;
        baseFireResistance = myCharacterData.fireResistance;
        baseFrostResistance = myCharacterData.frostResistance;
        basePoisonResistance = myCharacterData.poisonResistance;
        baseShadowResistance = myCharacterData.shadowResistance;
        baseAirResistance = myCharacterData.airResistance;

        // Setup Passives
        if (myCharacterData.tenaciousStacks > 0)
        {
            myPassiveManager.ModifyTenacious(myCharacterData.tenaciousStacks);
        }
        if (myCharacterData.enrageStacks > 0)
        {
            myPassiveManager.ModifyEnrage(myCharacterData.enrageStacks);
        }
        if (myCharacterData.masochistStacks > 0)
        {
            myPassiveManager.ModifyMasochist(myCharacterData.masochistStacks);
        }
        if (myCharacterData.lastStandStacks > 0)
        {
            myPassiveManager.ModifyLastStand(myCharacterData.lastStandStacks);
        }
        if (myCharacterData.unstoppableStacks > 0)
        {
            myPassiveManager.ModifyUnstoppable(1);
        }
        if (myCharacterData.slipperyStacks > 0)
        {
            myPassiveManager.ModifySlippery(myCharacterData.slipperyStacks);
        }
        if (myCharacterData.riposteStacks > 0)
        {
            myPassiveManager.ModifyRiposte(myCharacterData.riposteStacks);
        }
        if (myCharacterData.virtuosoStacks > 0)
        {
            myPassiveManager.ModifyVirtuoso(myCharacterData.virtuosoStacks);
        }
        if (myCharacterData.perfectReflexesStacks > 0)
        {
            myPassiveManager.ModifyPerfectReflexes(myCharacterData.perfectReflexesStacks);
        }
        if (myCharacterData.opportunistStacks > 0)
        {
            myPassiveManager.ModifyOpportunist(myCharacterData.opportunistStacks);
        }
        if (myCharacterData.patientStalkerStacks > 0)
        {
            myPassiveManager.ModifyPatientStalker(myCharacterData.patientStalkerStacks);
        }
        if (myCharacterData.stealthStacks > 0)
        {
            myPassiveManager.ModifyStealth(myCharacterData.stealthStacks);
        }
        if (myCharacterData.cautiousStacks > 0)
        {
            myPassiveManager.ModifyCautious(myCharacterData.cautiousStacks);
        }
        if (myCharacterData.guardianAuraStacks > 0)
        {
            myPassiveManager.ModifyGuardianAura(myCharacterData.guardianAuraStacks);
        }
        if (myCharacterData.unwaveringStacks > 0)
        {
            myPassiveManager.ModifyUnwavering(myCharacterData.unwaveringStacks);
        }
        if (myCharacterData.fieryAuraStacks > 0)
        {
            myPassiveManager.ModifyFieryAura(myCharacterData.fieryAuraStacks);
        }
        if (myCharacterData.immolationStacks > 0)
        {
            myPassiveManager.ModifyImmolation(myCharacterData.immolationStacks);
        }
        if (myCharacterData.demonStacks > 0)
        {
            myPassiveManager.ModifyDemon(myCharacterData.demonStacks);
        }
        if (myCharacterData.shatterStacks > 0)
        {
            myPassiveManager.ModifyShatter(myCharacterData.shatterStacks);
        }
        if (myCharacterData.frozenHeartStacks > 0)
        {
            myPassiveManager.ModifyFrozenHeart(myCharacterData.frozenHeartStacks);
        }
        if (myCharacterData.predatorStacks > 0)
        {
            myPassiveManager.ModifyPredator(myCharacterData.predatorStacks);
        }
        if (myCharacterData.hawkEyeStacks > 0)
        {
            myPassiveManager.ModifyHawkEye(myCharacterData.hawkEyeStacks);
        }
        if (myCharacterData.thornsStacks > 0)
        {
            myPassiveManager.ModifyThorns(myCharacterData.thornsStacks);
        }
        if (myCharacterData.trueSightStacks > 0)
        {
            myPassiveManager.ModifyTrueSight(1);
        }
        if (myCharacterData.fluxStacks > 0)
        {
            myPassiveManager.ModifyFlux(myCharacterData.fluxStacks);
        }
        if (myCharacterData.quickDrawStacks > 0)
        {
            myPassiveManager.ModifyQuickDraw(myCharacterData.quickDrawStacks);
        }
        if (myCharacterData.phasingStacks > 0)
        {
            myPassiveManager.ModifyPhasing(myCharacterData.phasingStacks);
        }
        if (myCharacterData.etherealBeingStacks > 0)
        {
            myPassiveManager.ModifyEtherealBeing(myCharacterData.etherealBeingStacks);
        }
        if (myCharacterData.encouragingAuraStacks > 0)
        {
            myPassiveManager.ModifyEncouragingAura(myCharacterData.encouragingAuraStacks);
        }
        if (myCharacterData.radianceStacks > 0)
        {
            myPassiveManager.ModifyRadiance(myCharacterData.radianceStacks);
        }
        if (myCharacterData.sacredAuraStacks > 0)
        {
            myPassiveManager.ModifySacredAura(myCharacterData.sacredAuraStacks);
        }
        if (myCharacterData.shadowAuraStacks > 0)
        {
            myPassiveManager.ModifyShadowAura(myCharacterData.shadowAuraStacks);
        }
        if (myCharacterData.shadowFormStacks > 0)
        {
            myPassiveManager.ModifyShadowForm(myCharacterData.shadowFormStacks);
        }
        if (myCharacterData.poisonousStacks > 0)
        {
            myPassiveManager.ModifyPoisonous(myCharacterData.poisonousStacks);
        }
        if (myCharacterData.venomousStacks > 0)
        {
            myPassiveManager.ModifyVenomous(myCharacterData.venomousStacks);
        }
        if (myCharacterData.toxicityStacks > 0)
        {
            myPassiveManager.ModifyToxicity(myCharacterData.toxicityStacks);
        }
        if (myCharacterData.toxicAuraStacks > 0)
        {
            myPassiveManager.ModifyToxicAura(myCharacterData.toxicAuraStacks);
        }
        if (myCharacterData.stormAuraStacks > 0)
        {
            myPassiveManager.ModifyStormAura(myCharacterData.stormAuraStacks);
        }
        if (myCharacterData.stormLordStacks > 0)
        {
            myPassiveManager.ModifyStormLord(myCharacterData.stormLordStacks);
        }

        // Set Weapons from character data
        ItemManager.Instance.SetUpDefenderWeaponsFromCharacterData(this);

    }    
   
    #endregion


    // Mouse + Click Events
    #region
    public void OnMouseDown()
    {
        Defender selectedDefender = DefenderManager.Instance.selectedDefender;

        // this statment prevents the user from clicking through UI elements and selecting a defender
        if (!EventSystem.current.IsPointerOverGameObject() == false &&
             ActivationManager.Instance.panelIsMousedOver == false)
        {
            Debug.Log("Clicked on the UI, returning...");
            return;
        }

        // Check for consumable orders first
        if (ConsumableManager.Instance.awaitingAdrenalinePotionTarget ||
            ConsumableManager.Instance.awaitingBottledBrillianceTarget ||
            ConsumableManager.Instance.awaitingBottledMadnessTarget ||
            ConsumableManager.Instance.awaitingPotionOfMightTarget ||
            ConsumableManager.Instance.awaitingPotionOfClarityTarget ||
            ConsumableManager.Instance.awaitingVanishPotionTarget)
        {
            ConsumableManager.Instance.ApplyConsumableToTarget(this);
        }

        else if (ConsumableManager.Instance.awaitingMolotovTarget ||
            ConsumableManager.Instance.awaitingDynamiteTarget ||
            ConsumableManager.Instance.awaitingPoisonGrenadeTarget ||
            ConsumableManager.Instance.awaitingBottledFrostTarget)
        {
            ConsumableManager.Instance.ApplyConsumableToTarget(tile);
        }

        else if (ConsumableManager.Instance.awaitingBlinkPotionCharacterTarget)
        {
            ConsumableManager.Instance.StartBlinkPotionLocationSettingProcess(this);
        }

        // Check ability orders second
        else if (selectedDefender != null && selectedDefender.awaitingGoBerserkOrder)
        {
            selectedDefender.StartGoBerserkProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingInspireOrder)
        {
            selectedDefender.StartInspireProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingIcyFocusOrder)
        {
            selectedDefender.StartIcyFocusProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingDarkGiftOrder)
        {
            selectedDefender.StartDarkGiftProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingStoneFormOrder)
        {
            selectedDefender.StartStoneFormProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingTranscendenceOrder)
        {
            selectedDefender.StartTranscendenceProcess(this);
            return;
        }        
        else if (selectedDefender != null && selectedDefender.awaitingTimeWarpOrder)
        {
            selectedDefender.StartTimeWarpProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingDragonBreathOrder)
        {
            selectedDefender.StartDragonBreathProcess(tile);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingSpiritVisionOrder)
        {
            selectedDefender.StartSpiritVisionProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingShroudOrder)
        {
            selectedDefender.StartShroudProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingHasteOrder)
        {
            selectedDefender.StartHasteProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingSteadyHandsOrder)
        {
            selectedDefender.StartSteadyHandsProcess(this);
            return;
        }

        else if (selectedDefender != null && selectedDefender.awaitingGuardOrder)
        {
            selectedDefender.StartGuardProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingFortifyOrder)
        {
            selectedDefender.StartFortifyProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingTelekinesisTargetOrder)
        {
            selectedDefender.StartTelekinesisLocationSettingProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingBurstOfKnowledgeOrder)
        {
            selectedDefender.StartBurstOfKnowledgeProcess(this);
            return;
        }

        else if (selectedDefender != null && selectedDefender.awaitingForestMedicineOrder)
        {
            selectedDefender.StartForestMedicineProcess(this);
            return;
        }

        else if (selectedDefender != null && selectedDefender.awaitingFrostArmourOrder)
        {
            selectedDefender.StartFrostArmourProcess(this);
            return;
        }

        else if (selectedDefender != null && selectedDefender.awaitingInvigorateOrder)
        {
            selectedDefender.StartInvigorateProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingBlazeOrder)
        {
            selectedDefender.StartBlazeProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingOverloadOrder)
        {
            selectedDefender.StartOverloadProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingCreepingFrostOrder)
        {
            selectedDefender.StartCreepingFrostProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingShadowWreathOrder)
        {
            selectedDefender.StartShadowWreathProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingSnowStasisOrder)
        {
            selectedDefender.StartSnowStasisProcess(this);
            return;
        }
        
        else if (selectedDefender != null && selectedDefender.awaitingHolyFireOrder)
        {
            selectedDefender.StartHolyFireProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingPrimalRageOrder)
        {
            selectedDefender.StartPrimalRageProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingPhaseShiftOrder)
        {
            selectedDefender.StartPhaseShiftProcess(this);
            return;
        }        
        else if (selectedDefender != null && selectedDefender.awaitingBlessOrder)
        {
            selectedDefender.StartBlessProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingEvasionOrder)
        {
            selectedDefender.StartEvasionProcess(this);
            return;
        }
        else if (selectedDefender != null && selectedDefender.awaitingMirageOrder)
        {
            selectedDefender.StartMirageProcess(this);
            return;
        }

        else
        {
            Debug.Log("Defender selection attempt detected");
            SelectDefender();            
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
        awaitingAnOrder = false;

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
        awaitingChaosBoltOrder = false;
        awaitingKickToTheBallsOrder = false;
        awaitingDevastatingBlowOrder = false;
        awaitingBladeFlurryOrder = false;
        awaitingEvasionOrder = false;
        awaitingShieldSlamOrder = false;
        awaitingTendonSlashOrder = false;
        awaitingShieldShatterOrder = false;
        awaitingSwordAndBoardOrder = false;
        awaitingPhoenixDiveOrder = false;
        awaitingChillingBlowOrder = false;
        awaitingIcyFocusOrder = false;
        awaitingCombustionOrder = false;
        awaitingDragonBreathOrder = false;
        awaitingBlizzardOrder = false;
        awaitingFrostArmourOrder = false;
        awaitingThawOrder = false;
        awaitingSnipeOrder = false;
        awaitingHasteOrder = false;
        awaitingSteadyHandsOrder = false;
        awaitingTreeLeapOrder = false;
        awaitingDimensionalBlastOrder = false;
        awaitingMirageOrder = false;
        awaitingBurstOfKnowledgeOrder = false;
        awaitingBlinkOrder = false;
        awaitingTimeWarpOrder = false;
        awaitingDimensionalHexOrder = false;
        awaitingBlindingLightOrder = false;
        awaitingTranscendenceOrder = false;
        awaitingJudgementOrder = false;
        awaitingShroudOrder = false;
        awaitingRainOfChaosOrder = false;
        awaitingBlightOrder = false;
        awaitingToxicSlashOrder = false;
        awaitingToxicEruptionOrder = false;
        awaitingDrainOrder = false;
        awaitingLightningBoltOrder = false;
        awaitingThunderStrikeOrder = false;
        awaitingSpiritVisionOrder = false;
        awaitingThunderStormOrder = false;
        awaitingConcealingCloudsOrder = false;
        awaitingHeadShotOrder = false;
        awaitingHexOrder = false;
        awaitingShankOrder = false;
        awaitingCheapShotOrder = false;
        awaitingAmbushOrder = false;
        awaitingShadowStepOrder = false;
        awaitingProvokeOrder = false;
        awaitingDecapitateOrder = false;
        awaitingDisarmOrder = false;
        awaitingShadowBlastOrder = false;
        awaitingGoBerserkOrder = false;
        awaitingFortifyOrder = false;
        awaitingStoneFormOrder = false;
        awaitingBackStabOrder = false;
        awaitingChloroformBombOrder = false;
        awaitingPinningShotOrder = false;
        awaitingMarkTargetOrder = false;
        awaitingSnowStasisOrder = false;
        awaitingShadowWreathOrder = false;
        awaitingBlazeOrder = false;
        awaitingCreepingFrostOrder = false;
        awaitingOverloadOrder = false;
        awaitingDarkGiftOrder = false;
        awaitingSuperConductorOrder = false;
        awaitingMeltOrder = false;
        awaitingMeltOrder = false;

        TileHover.Instance.SetVisibility(false);
        LevelManager.Instance.UnhighlightAllTiles();
        PathRenderer.Instance.DeactivatePathRenderer();

        myCurrentTarget = null;
    }
    #endregion


    // Ability Button Click Events
    #region

    public bool IsAwaitingOrder()
    {
        return awaitingAnOrder;
    }
    public void OnAbilityButtonClicked(string abilityName)
    {
        // Prevent player from using character abilities when it is not their turn
        if(ActivationManager.Instance.IsEntityActivated(this) == false)
        {
            // to do in future: warning message to player here "Not this characters activation!"
            return;
        }

        // Prevent player from making moves while there are unresolved ability events
        if (ActionManager.Instance.UnresolvedCombatActions())
        {
            Debug.Log("Defender.OnAbilityButtonClicked() cancelling: there are unresolved combat events in the Action queue");
            return;
        }

        // Clear all previous view settings and defender orders
        bool enableTileHover = true;        
        LevelManager.Instance.UnhighlightAllTiles();
        ClearAllOrders();
        awaitingAnOrder = true;
        ConsumableManager.Instance.ClearAllConsumableOrders();

        // Enable tile hover if ability is usable, and requires targetting
        Ability ability = mySpellBook.GetAbilityByName(abilityName);
        if (!EntityLogic.IsAbilityUseable(this, ability))
        {
            awaitingAnOrder = false;
            enableTileHover = false;
        }
        if(!EntityLogic.IsAbleToMove(this) &&
            (ability.abilityName == "Move" ||
            ability.abilityName == "Charge" ||
            ability.abilityName == "Dash" ||
            ability.abilityName == "Get Down!")
            )
        {
            awaitingAnOrder = false;
            enableTileHover = false;
        }

        if (EntityLogic.IsAbleToTakeActions(this) == false)
        {
            awaitingAnOrder = false;
            TileHover.Instance.SetVisibility(false);
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
        else if (abilityName == "Disarm")
        {
            OnDisarmButtonClicked();
        }
        else if (abilityName == "Back Stab")
        {
            OnBackStabButtonClicked();
        }
        else if (abilityName == "Defend")
        {
            OnDefendButtonClicked();
            enableTileHover = false;
            awaitingAnOrder = false;
        }
        
        else if (abilityName == "Frost Armour")
        {
            OnFrostArmourButtonClicked();
        }
        else if (abilityName == "Transcendence")
        {
            OnTranscendenceButtonClicked();
        }
        else if (abilityName == "Glacial Burst")
        {
            enableTileHover = false;
            awaitingAnOrder = false;
            OnGlacialBurstButtonClicked();
        }
        else if (abilityName == "Challenging Shout")
        {
            enableTileHover = false;
            awaitingAnOrder = false;
            OnChallengingShoutButtonClicked();
        }
        else if (abilityName == "Thaw")
        {
            OnThawButtonClicked();
        }
        else if (abilityName == "Blizzard")
        {
            OnBlizzardButtonClicked();
        }
        else if (abilityName == "Snipe")
        {
            OnSnipeButtonClicked();
        }
        else if (abilityName == "Haste")
        {
            OnHasteButtonClicked();
        }
        else if (abilityName == "Steady Hands")
        {
            OnSteadyHandsButtonClicked();
        }
        else if (abilityName == "Head Shot")
        {
            OnHeadShotButtonClicked();
        }
        else if (abilityName == "Combustion")
        {
            OnCombustionButtonClicked();
        }
        else if (abilityName == "Forest Medicine")
        {
            OnForestMedicineButtonClicked();
        }
        else if (abilityName == "Tree Leap")
        {
            OnTreeLeapButtonClicked();
        }
        else if (abilityName == "Rapid Fire")
        {
            OnRapidFireButtonClicked();
        }
        else if (abilityName == "Overwatch")
        {
            enableTileHover = false;
            awaitingAnOrder = false;
            OnOverwatchButtonClicked();
        }
        else if (abilityName == "Dimensional Blast")
        {
            OnDimensionalBlastButtonClicked();
        }
        else if (abilityName == "Mirage")
        {
            OnMirageButtonClicked();
        }
        else if (abilityName == "Burst Of Knowledge")
        {
            OnBurstOfKnowledgeButtonClicked();
        }
        else if (abilityName == "Blink")
        {
            OnBlinkButtonClicked();
        }
        else if (abilityName == "Time Warp")
        {
            OnTimeWarpButtonClicked();
        }
        else if (abilityName == "Dimensional Hex")
        {
            OnDimensionalHexButtonClicked();
        }
        else if (abilityName == "Consecrate")
        {
            enableTileHover = false;
            awaitingAnOrder = false;
            OnConsecrateButtonClicked();
        }
        else if (abilityName == "Blinding Light")
        {
            OnBlindingLightButtonClicked();
        }
        else if (abilityName == "Transcendence")
        {
            //OnTranscendenceButtonClicked();
        }
        else if (abilityName == "Judgement")
        {
            OnJudgementButtonClicked();
        }
        else if (abilityName == "Shroud")
        {
            OnShroudButtonClicked();
        }
        else if (abilityName == "Hex")
        {
            OnHexButtonClicked();
        }
        else if (abilityName == "Mark Target")
        {
            OnMarkTargetButtonClicked();
        }
        else if (abilityName == "Rain Of Chaos")
        {
            OnRainOfChaosButtonClicked();
        }
        else if (abilityName == "Unbridled Chaos")
        {
            enableTileHover = false;
            awaitingAnOrder = false;
            OnUnbridledChaosButtonClicked();
        }
        else if (abilityName == "Blight")
        {
            OnBlightButtonClicked();
        }
        else if (abilityName == "Provoke")
        {
            OnProvokeButtonClicked();
        }
        else if (abilityName == "Blood Offering")
        {
            enableTileHover = false;
            awaitingAnOrder = false;
            OnBloodOfferingButtonClicked();
        }
        else if (abilityName == "Toxic Slash")
        {
            OnToxicSlashButtonClicked();
        }
        else if (abilityName == "Decapitate")
        {
            OnDecapitateButtonClicked();
        }
        else if (abilityName == "Cheap Shot")
        {
            OnCheapShotButtonClicked();
        }
        else if (abilityName == "Shank")
        {
            OnShankButtonClicked();
        }
        else if (abilityName == "Shadow Step")
        {
            OnShadowStepButtonClicked();
        }
        else if (abilityName == "Ambush")
        {
            OnAmbushButtonClicked();
        }
        else if (abilityName == "Sharpen Blade")
        {
            OnSharpenBladeButtonClicked();
            enableTileHover = false;
            awaitingAnOrder = false;
        }
        else if (abilityName == "Noxious Fumes")
        {
            enableTileHover = false;
            awaitingAnOrder = false;
            OnNoxiousFumesButtonClicked();
        }
        else if (abilityName == "Toxic Eruption")
        {
            OnToxicEruptionButtonClicked();
        }
        else if (abilityName == "Drain")
        {
            OnDrainButtonClicked();
        }
        
        else if (abilityName == "Lightning Bolt")
        {
            OnLightningBoltButtonClicked();
        }
        else if (abilityName == "Thunder Strike")
        {
            OnThunderStrikeButtonClicked();
        }
        else if (abilityName == "Spirit Vision")
        {
            OnSpiritVisionButtonClicked();
        }
        else if (abilityName == "Dragon Breath")
        {
            OnDragonBreathButtonClicked();
        }
        else if (abilityName == "Thunder Storm")
        {
            OnThunderStormButtonClicked();
        }
        else if (abilityName == "Concealing Clouds")
        {
            OnConcealingCloudsButtonClicked();
        }
        else if (abilityName == "Super Conductor")
        {
            OnSuperConductorButtonClicked();
        }       
       
        else if (abilityName == "Charge")
        {
            OnChargeButtonClicked();
        }
        else if (abilityName == "Go Berserk")
        {
            OnGoBerserkButtonClicked();
        }
        else if (abilityName == "Inspire")
        {
            OnInspireButtonClicked();
        }
        else if (abilityName == "Icy Focus")
        {
            OnIcyFocusButtonClicked();
        }
        else if (abilityName == "Stone Form")
        {
            OnStoneFormButtonClicked();
        }
        else if (abilityName == "Dark Gift")
        {
            OnDarkGiftButtonClicked();
        }
        else if (abilityName == "Guard")
        {
            OnGuardButtonClicked();
        }
        else if (abilityName == "Fortify")
        {
            OnFortifyButtonClicked();
        }
        else if (abilityName == "Meteor")
        {
            OnMeteorButtonClicked();
        }
        else if (abilityName == "Chloroform Bomb")
        {
            OnChloroformBombButtonClicked();
        }
        else if (abilityName == "Blinding Light")
        {
            OnBlindingLightButtonClicked();
        }
        else if (abilityName == "Toxic Eruption")
        {
            OnToxicEruptionButtonClicked();
        }
        else if (abilityName == "Thunder Storm")
        {
            OnThunderStormButtonClicked();
        }
        else if (abilityName == "Rain Of Chaos")
        {
            OnRainOfChaosButtonClicked();
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
        else if (abilityName == "Melt")
        {
            OnMeltButtonClicked();
        }
        else if (abilityName == "Shadow Blast")
        {
            OnShadowBlastButtonClicked();
        }
        else if (abilityName == "Thaw")
        {
            OnThawButtonClicked();
        }
        else if (abilityName == "Shoot")
        {
            OnShootButtonClicked();
        }
        else if (abilityName == "Pinning Shot")
        {
            OnPinningShotButtonClicked();
        }
        else if (abilityName == "Impaling Bolt")
        {
            OnImpalingBoltButtonClicked();
        }        
        else if (abilityName == "Whirlwind")
        {
            OnWhirlwindButtonClicked();
            enableTileHover = false;
            awaitingAnOrder = false;
        }
        else if (abilityName == "Spirit Surge")
        {
            OnSpritSurgeButtonClicked();
            enableTileHover = false;
            awaitingAnOrder = false;
        }
        else if (abilityName == "Toxic Rain")
        {
            OnToxicRainButtonClicked();
            enableTileHover = false;
            awaitingAnOrder = false;
        }
        else if (abilityName == "Pure Hate")
        {
            OnPureHateButtonClicked();
            enableTileHover = false;
            awaitingAnOrder = false;
        }
        else if (abilityName == "Global Cooling")
        {
            OnGlobalCoolingButtonClicked();
            enableTileHover = false;
            awaitingAnOrder = false;
        }
        else if (abilityName == "Second Wind")
        {
            OnSecondWindButtonClicked();
            enableTileHover = false;
            awaitingAnOrder = false;
        }
        else if (abilityName == "Infuse")
        {
            OnInfuseButtonClicked();
            enableTileHover = false;
            awaitingAnOrder = false;
        }
        else if (abilityName == "Purity")
        {
            OnPurityButtonClicked();
            enableTileHover = false;
            awaitingAnOrder = false;
        }
        else if (abilityName == "Shadow Wreath")
        {
            OnShadowWreathButtonClicked();
        }
        else if (abilityName == "Overload")
        {
            OnOverloadButtonClicked();
        }
        else if (abilityName == "Recklessness")
        {
            OnRecklessnessButtonClicked();
        }
        else if (abilityName == "Rapid Cloaking")
        {
            OnRapidCloakingButtonClicked();
        }
        else if (abilityName == "Concentration")
        {
            OnConcentrationButtonClicked();
        }
        else if (abilityName == "Testudo")
        {
            OnTestudoButtonClicked();
        }
        else if (abilityName == "Blaze")
        {
            OnBlazeButtonClicked();
        }
        else if (abilityName == "Creeping Frost")
        {
            OnCreepingFrostButtonClicked();
        }
        else if (abilityName == "Shadow Wreath")
        {
            OnShadowWreathButtonClicked();
        }
        else if (abilityName == "Overload")
        {
            OnOverloadButtonClicked();
        }
        else if (abilityName == "Fire Nova")
        {
            OnFireNovaButtonClicked();
            enableTileHover = false;
        }
        else if (abilityName == "Frost Nova")
        {
            OnFrostNovaButtonClicked();
            enableTileHover = false;
            awaitingAnOrder = false;
        }
        else if (abilityName == "Reactive Armour")
        {
            OnReactiveArmourButtonClicked();
            enableTileHover = false;
            awaitingAnOrder = false;
        }
        else if (abilityName == "Vanish")
        {
            OnVanishButtonClicked();
            enableTileHover = false;
            awaitingAnOrder = false;
        }       

        else if (abilityName == "Invigorate")
        {
            OnInvigorateButtonClicked();
        }
        else if (abilityName == "Snow Stasis")
        {
            OnSnowStasisButtonClicked();
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
        else if (abilityName == "Shield Slam")
        {
            OnShieldSlamButtonClicked();
        }

        else if (abilityName == "Dash")
        {
            OnDashButtonClicked();
        }
        else if (abilityName == "Phoenix Dive")
        {
            OnPhoenixDiveButtonClicked();
        }

        else if (abilityName == "Preparation")
        {
            OnPreparationButtonClicked();
            enableTileHover = false;
            awaitingAnOrder = false;
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
        else if (abilityName == "Blade Flurry")
        {
            OnBladeFlurryButtonClicked();
            enableTileHover = false;
            awaitingAnOrder = false;
        }
        else if (abilityName == "Kick To The Balls")
        {
            OnKickToTheBallsButtonClicked();
        }
        else if (abilityName == "Devastating Blow")
        {
            OnDevastatingBlowButtonClicked();
        }
        else if (abilityName == "Evasion")
        {
            OnEvasionButtonClicked();
        }
        else if (abilityName == "Tendon Slash")
        {
            OnTendonSlashButtonClicked();
        }
        else if (abilityName == "Chilling Blow")
        {
            OnChillingBlowButtonClicked();
        }
        else if (abilityName == "Shield Shatter")
        {
            OnShieldShatterButtonClicked();
        }
        else if (abilityName == "Sword And Board")
        {
            OnSwordAndBoardButtonClicked();
        }
        else if (abilityName == "Shield Slam")
        {
            OnShieldSlamButtonClicked();
        }

        if (enableTileHover)
        {
            TileHover.Instance.SetVisibility(true);
        }
        else if (!enableTileHover)
        {
            TileHover.Instance.SetVisibility(false);
        }
    }
    public void OnMoveButtonClicked()
    {
        Ability move = mySpellBook.GetAbilityByName("Move");

        if(EntityLogic.IsAbleToMove(this) &&
           EntityLogic.IsAbilityUseable(this, move))
        {
            Debug.Log("Move button clicked, awaiting move order");
            awaitingMoveOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetValidMoveableTilesWithinRange(EntityLogic.GetTotalMobility(this), tile));
            PathRenderer.Instance.ActivatePathRenderer();
        }

        
    }
    public void OnStrikeButtonClicked()
    {
        Ability strike = mySpellBook.GetAbilityByName("Strike");

        if (EntityLogic.IsAbilityUseable(this, strike))
        {
            Debug.Log("Strike button clicked, awaiting strike order");
            awaitingStrikeOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }       

    }
    public void OnBackStabButtonClicked()
    {
        Ability backStab = mySpellBook.GetAbilityByName("Back Stab");

        if (EntityLogic.IsAbilityUseable(this, backStab))
        {
            Debug.Log("Back Stab button clicked, awaiting Back Stab order");
            awaitingBackStabOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }

    }
    public void OnDisarmButtonClicked()
    {
        Ability disarm = mySpellBook.GetAbilityByName("Disarm");

        if (EntityLogic.IsAbilityUseable(this, disarm))
        {
            Debug.Log("Disarm button clicked, awaiting disarm order");
            awaitingDisarmOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }

    }
    public void OnSmashButtonClicked()
    {
        Ability smash = mySpellBook.GetAbilityByName("Smash");

        if (EntityLogic.IsAbilityUseable(this, smash))
        {
            Debug.Log("Smash button clicked, awaiting smash order");
            awaitingSmashOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }       

    }
    public void OnInspireButtonClicked()
    {
        Ability inspire = mySpellBook.GetAbilityByName("Inspire");

        if (EntityLogic.IsAbilityUseable(this, inspire))
        {
            Debug.Log("Inspire button clicked, awaiting inspire order");
            awaitingInspireOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(inspire.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }        
    }
    public void OnBlazeButtonClicked()
    {
        Ability blaze = mySpellBook.GetAbilityByName("Blaze");

        if (EntityLogic.IsAbilityUseable(this, blaze))
        {
            Debug.Log("Blaze button clicked, awaiting Blaze order");
            awaitingBlazeOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(blaze.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnCreepingFrostButtonClicked()
    {
        Ability creepingFrost = mySpellBook.GetAbilityByName("Creeping Frost");

        if (EntityLogic.IsAbilityUseable(this, creepingFrost))
        {
            Debug.Log("Creeping Frost button clicked, awaiting Creeping Frost order");
            awaitingCreepingFrostOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(creepingFrost.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnShadowWreathButtonClicked()
    {
        Ability shadowWreath = mySpellBook.GetAbilityByName("Shadow Wreath");

        if (EntityLogic.IsAbilityUseable(this, shadowWreath))
        {
            Debug.Log("Shadow Wreath button clicked, awaiting Shadow Wreath order");
            awaitingShadowWreathOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(shadowWreath.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnOverloadButtonClicked()
    {
        Ability overload = mySpellBook.GetAbilityByName("Overload");

        if (EntityLogic.IsAbilityUseable(this, overload))
        {
            Debug.Log("Overload button clicked, awaiting Overload order");
            awaitingOverloadOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(overload.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnGoBerserkButtonClicked()
    {
        Ability goBerserk = mySpellBook.GetAbilityByName("Go Berserk");

        if (EntityLogic.IsAbilityUseable(this, goBerserk))
        {
            Debug.Log("Go Berserk button clicked, awaiting Go Berserk order");
            awaitingGoBerserkOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(goBerserk.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }   
    public void OnSpiritVisionButtonClicked()
    {
        Ability spiritVision = mySpellBook.GetAbilityByName("Spirit Vision");

        if (EntityLogic.IsAbilityUseable(this, spiritVision))
        {
            Debug.Log("Spirit Vision button clicked, awaiting Spirit Vision order");
            awaitingSpiritVisionOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(spiritVision.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnDragonBreathButtonClicked()
    {
        Ability dragonBreath = mySpellBook.GetAbilityByName("Dragon Breath");

        if (EntityLogic.IsAbilityUseable(this, dragonBreath))
        {
            Debug.Log("Dragon Breath button clicked, awaiting Dragon Breath order");
            awaitingDragonBreathOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(dragonBreath.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnShroudButtonClicked()
    {
        Ability shroud = mySpellBook.GetAbilityByName("Shroud");

        if (EntityLogic.IsAbilityUseable(this, shroud))
        {
            Debug.Log("Shroud button clicked, awaiting Shroud order");
            awaitingShroudOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(shroud.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnHexButtonClicked()
    {
        Ability hex = mySpellBook.GetAbilityByName("Hex");

        if (EntityLogic.IsAbilityUseable(this, hex))
        {
            Debug.Log("Hex button clicked, awaiting Hex order");
            awaitingHexOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(hex.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnMarkTargetButtonClicked()
    {
        Ability mt = mySpellBook.GetAbilityByName("Mark Target");

        if (EntityLogic.IsAbilityUseable(this, mt))
        {
            Debug.Log("Mark Target button clicked, awaiting Mark Target order");
            awaitingMarkTargetOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(mt.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnIcyFocusButtonClicked()
    {
        Ability icyFocus = mySpellBook.GetAbilityByName("Icy Focus");

        if (EntityLogic.IsAbilityUseable(this, icyFocus))
        {
            Debug.Log("Icy Focus button clicked, awaiting Icy Focus order");
            awaitingIcyFocusOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(icyFocus.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnDarkGiftButtonClicked()
    {
        Ability darkGift = mySpellBook.GetAbilityByName("Dark Gift");

        if (EntityLogic.IsAbilityUseable(this, darkGift))
        {
            Debug.Log("Dark Gift button clicked, awaiting Dark Gift order");
            awaitingDarkGiftOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(darkGift.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnStoneFormButtonClicked()
    {
        Ability stoneForm = mySpellBook.GetAbilityByName("Stone Form");

        if (EntityLogic.IsAbilityUseable(this, stoneForm))
        {
            Debug.Log("Stone Form button clicked, awaiting Stone Form order");
            awaitingStoneFormOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(stoneForm.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnChargeButtonClicked()
    {
        Ability charge = mySpellBook.GetAbilityByName("Charge");

        if (EntityLogic.IsAbilityUseable(this, charge) &&
            EntityLogic.IsAbleToMove(this))           
        {
            Debug.Log("Charge button clicked, awaiting charge target");
            awaitingChargeTargetOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(charge.abilityPrimaryValue + EntityLogic.GetTotalMobility(this), LevelManager.Instance.Tiles[gridPosition], true, false));
            //PathRenderer.Instance.ActivatePathRenderer();
        }      

    }
    public void OnGetDownButtonClicked()
    {
        Ability getDown = mySpellBook.GetAbilityByName("Get Down!");

        if (EntityLogic.IsAbilityUseable(this, getDown) &&
            EntityLogic.IsAbleToMove(this))            
        {
            Debug.Log("Get Down! button clicked, awaiting Get Down! target");
            awaitingGetDownOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetValidMoveableTilesWithinRange(getDown.abilityPrimaryValue + EntityLogic.GetTotalMobility(this), LevelManager.Instance.Tiles[gridPosition]));
            PathRenderer.Instance.ActivatePathRenderer();
        }

        

    }
    public void OnLightningShieldClicked()
    {
        Ability lightningShield = mySpellBook.GetAbilityByName("Lightning Shield");

        if (EntityLogic.IsAbilityUseable(this, lightningShield))
        {
            Debug.Log("Lightning Shield button clicked, awaiting Lightning Shield target");
            awaitingLightningShieldOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(lightningShield.abilityRange, LevelManager.Instance.Tiles[gridPosition],true, false));
        }
    }
    public void OnGuardButtonClicked()
    {
        Ability guard = mySpellBook.GetAbilityByName("Guard");

        if (EntityLogic.IsAbilityUseable(this, guard))
        {
            Debug.Log("Guard button clicked, awaiting guard target");
            awaitingGuardOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(guard.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }        
    }
    public void OnFortifyButtonClicked()
    {
        Ability fortify = mySpellBook.GetAbilityByName("Fortify");

        if (EntityLogic.IsAbilityUseable(this, fortify))
        {
            Debug.Log("Fortify button clicked, awaiting Fortify target");
            awaitingFortifyOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(fortify.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnForestMedicineButtonClicked()
    {
        Ability forestMedicine = mySpellBook.GetAbilityByName("Forest Medicine");

        if (EntityLogic.IsAbilityUseable(this, forestMedicine))
        {
            Debug.Log("Forest Medicine button clicked, awaiting forest medicine target");
            awaitingForestMedicineOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(forestMedicine.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnTreeLeapButtonClicked()
    {
        Ability treeLeap = mySpellBook.GetAbilityByName("Tree Leap");

        if (EntityLogic.IsAbilityUseable(this, treeLeap))
        {
            Debug.Log("Tree Leap button clicked, awaiting Tree Leap target");
            awaitingTreeLeapOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(treeLeap.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnFrostArmourButtonClicked()
    {
        Ability frostArmour = mySpellBook.GetAbilityByName("Frost Armour");

        if (EntityLogic.IsAbilityUseable(this, frostArmour))
        {
            Debug.Log("Frost Armour button clicked, awaiting Frost Armour target");
            awaitingFrostArmourOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(frostArmour.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnTranscendenceButtonClicked()
    {
        Ability transcendence = mySpellBook.GetAbilityByName("Transcendence");

        if (EntityLogic.IsAbilityUseable(this, transcendence))
        {
            Debug.Log("Transcendence button clicked, awaiting Transcendence target");
            awaitingTranscendenceOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(transcendence.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnMeteorButtonClicked()
    {
        Ability meteor = mySpellBook.GetAbilityByName("Meteor");

        if (EntityLogic.IsAbilityUseable(this, meteor))
        {
            Debug.Log("Meteor button clicked, awaiting Meteor target");
            awaitingMeteorOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, meteor), LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnChloroformBombButtonClicked()
    {
        Ability cb = mySpellBook.GetAbilityByName("Chloroform Bomb");

        if (EntityLogic.IsAbilityUseable(this, cb))
        {
            Debug.Log("Chloroform Bomb button clicked, awaiting Chloroform Bomb target");
            awaitingChloroformBombOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, cb), LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnBlizzardButtonClicked()
    {
        Ability blizzard = mySpellBook.GetAbilityByName("Blizzard");

        if (EntityLogic.IsAbilityUseable(this, blizzard))
        {
            Debug.Log("Blizzard button clicked, awaiting Blizzard target");
            awaitingBlizzardOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, blizzard), LevelManager.Instance.Tiles[gridPosition], false, false));
        }        
    }
    public void OnBlindingLightButtonClicked()
    {
        Ability blindingLight = mySpellBook.GetAbilityByName("Blinding Light");

        if (EntityLogic.IsAbilityUseable(this, blindingLight))
        {
            Debug.Log("Blinding Light button clicked, awaiting Blinding Light target");
            awaitingBlindingLightOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, blindingLight), LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnToxicEruptionButtonClicked()
    {
        Ability toxicEruption = mySpellBook.GetAbilityByName("Toxic Eruption");

        if (EntityLogic.IsAbilityUseable(this, toxicEruption))
        {
            Debug.Log("Toxic Eruption button clicked, awaiting Toxic Eruption target");
            awaitingToxicEruptionOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this,toxicEruption), LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnDrainButtonClicked()
    {
        Ability drain = mySpellBook.GetAbilityByName("Drain");

        if (EntityLogic.IsAbilityUseable(this, drain))
        {
            Debug.Log("Drain button clicked, awaiting Drain target");
            awaitingDrainOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(drain.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnLightningBoltButtonClicked()
    {
        Ability lightningBolt = mySpellBook.GetAbilityByName("Lightning Bolt");

        if (EntityLogic.IsAbilityUseable(this, lightningBolt))
        {
            Debug.Log("Lightning Bolt clicked, awaiting Lightning Bolt target");
            awaitingLightningBoltOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, lightningBolt), LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnSuperConductorButtonClicked()
    {
        Ability superConductor = mySpellBook.GetAbilityByName("Super Conductor");

        if (EntityLogic.IsAbilityUseable(this, superConductor))
        {
            Debug.Log("Super Conductor clicked, awaiting Super Conductor target");
            awaitingSuperConductorOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, superConductor), LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnRainOfChaosButtonClicked()
    {
        Ability rainOfChaos = mySpellBook.GetAbilityByName("Rain Of Chaos");

        if (EntityLogic.IsAbilityUseable(this, rainOfChaos))
        {
            Debug.Log("Rain Of Chaos button clicked, awaiting Rain Of Chaos target");
            awaitingRainOfChaosOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, rainOfChaos), LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnThunderStormButtonClicked()
    {
        Ability thunderStorm = mySpellBook.GetAbilityByName("Thunder Storm");

        if (EntityLogic.IsAbilityUseable(this, thunderStorm))
        {
            Debug.Log("Thunder Storm button clicked, awaiting Thunder Storm target");
            awaitingThunderStormOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, thunderStorm), LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnConcealingCloudsButtonClicked()
    {
        Ability concealingClouds = mySpellBook.GetAbilityByName("Concealing Clouds");

        if (EntityLogic.IsAbilityUseable(this, concealingClouds))
        {
            Debug.Log("Concealing Clouds button clicked, awaiting Concealing Clouds target");
            awaitingConcealingCloudsOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(concealingClouds.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnTelekinesisButtonClicked()
    {
        Ability telekinesis = mySpellBook.GetAbilityByName("Telekinesis");        

        if (EntityLogic.IsAbilityUseable(this, telekinesis))
        {
            Debug.Log("Telekinesis button clicked, awaiting telekinesis target");
            awaitingTelekinesisTargetOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(telekinesis.abilityRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }

    }
    public void OnFrostBoltButtonClicked()
    {
        Ability frostbolt = mySpellBook.GetAbilityByName("Frost Bolt");

        if (EntityLogic.IsAbilityUseable(this, frostbolt))
        {
            Debug.Log("Frost Bolt button clicked, awaiting Frost Bolt target");
            awaitingFrostBoltOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, frostbolt), LevelManager.Instance.Tiles[gridPosition], false, false));
        }        
    }
    public void OnChainLightningButtonClicked()
    {
        Ability chainLightning = mySpellBook.GetAbilityByName("Chain Lightning");

        if (EntityLogic.IsAbilityUseable(this, chainLightning))
        {
            Debug.Log("Chain Lightning button clicked, awaiting Frost Bolt target");
            awaitingChainLightningOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, chainLightning), LevelManager.Instance.Tiles[gridPosition], false, false));
        }      
    }
    public void OnDashButtonClicked()
    {
        Ability dash = mySpellBook.GetAbilityByName("Dash");

        if (EntityLogic.IsAbilityUseable(this, dash) &&
            EntityLogic.IsAbleToMove(this))
        {
            Debug.Log("Dash button clicked, awaiting Dash tile target");
            awaitingDashOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetValidMoveableTilesWithinRange(dash.abilityPrimaryValue + EntityLogic.GetTotalMobility(this), LevelManager.Instance.Tiles[gridPosition]));
            PathRenderer.Instance.ActivatePathRenderer();
        }        
    }
    public void OnPhoenixDiveButtonClicked()
    {
        Ability phoenixDive = mySpellBook.GetAbilityByName("Phoenix Dive");

        if (EntityLogic.IsAbilityUseable(this, phoenixDive))
        {
            Debug.Log("Phoenix Dive button clicked, awaiting Phoenix Dive tile target");
            awaitingPhoenixDiveOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(phoenixDive.abilityRange, LevelManager.Instance.Tiles[gridPosition], false));
        }


    }
    public void OnFireBallButtonClicked()
    {
        Ability fireball = mySpellBook.GetAbilityByName("Fire Ball");

        if (EntityLogic.IsAbilityUseable(this, fireball))
        {
            Debug.Log("Fire Ball button clicked, awaiting Shadow Blast target");
            awaitingFireBallOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, fireball), LevelManager.Instance.Tiles[gridPosition], true, false));
        }       
    }
    public void OnMeltButtonClicked()
    {
        Ability melt = mySpellBook.GetAbilityByName("Melt");

        if (EntityLogic.IsAbilityUseable(this, melt))
        {
            Debug.Log("Melt button clicked, awaiting Melt target");
            awaitingMeltOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, melt), LevelManager.Instance.Tiles[gridPosition], true, false));
        }
    }
    public void OnShadowBlastButtonClicked()
    {
        Ability fireball = mySpellBook.GetAbilityByName("Shadow Blast");

        if (EntityLogic.IsAbilityUseable(this, fireball))
        {
            Debug.Log("Shadow Blast button clicked, awaiting Shadow Blast target");
            awaitingShadowBlastOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, fireball), LevelManager.Instance.Tiles[gridPosition], true, false));
        }
    }
    public void OnBlightButtonClicked()
    {
        Ability blight = mySpellBook.GetAbilityByName("Blight");

        if (EntityLogic.IsAbilityUseable(this, blight))
        {
            Debug.Log("Blight button clicked, awaiting Blight target");
            awaitingBlightOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(blight.abilityRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }
    }
    public void OnProvokeButtonClicked()
    {
        Ability provoke = mySpellBook.GetAbilityByName("Provoke");

        if (EntityLogic.IsAbilityUseable(this, provoke))
        {
            Debug.Log("Provoke button clicked, awaiting Provoke target");
            awaitingProvokeOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(provoke.abilityRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }
    }
    public void OnDimensionalBlastButtonClicked()
    {
        Ability dimensionalBlast = mySpellBook.GetAbilityByName("Dimensional Blast");

        if (EntityLogic.IsAbilityUseable(this, dimensionalBlast))
        {
            Debug.Log("Dimensional Blast button clicked, awaiting Dimensional Blast target");
            awaitingDimensionalBlastOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, dimensionalBlast), LevelManager.Instance.Tiles[gridPosition], true, false));
        }
    }
    public void OnMirageButtonClicked()
    {
        Ability mirage = mySpellBook.GetAbilityByName("Mirage");

        if (EntityLogic.IsAbilityUseable(this, mirage))
        {
            Debug.Log("Mirage button clicked, awaiting Mirage target");
            awaitingMirageOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(mirage.abilityRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }
    }
    public void OnBurstOfKnowledgeButtonClicked()
    {
        Ability burstOfKnowledge = mySpellBook.GetAbilityByName("Burst Of Knowledge");

        if (EntityLogic.IsAbilityUseable(this, burstOfKnowledge))
        {
            Debug.Log("Burst Of Knowledge button clicked, awaiting Burst Of Knowledge target");
            awaitingBurstOfKnowledgeOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(burstOfKnowledge.abilityRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }
    }
    public void OnBlinkButtonClicked()
    {
        Ability blink = mySpellBook.GetAbilityByName("Blink");

        if (EntityLogic.IsAbilityUseable(this, blink))
        {
            Debug.Log("Blink button clicked, awaiting Blink location target");
            awaitingBlinkOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(blink.abilityRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }
    }
    public void OnTimeWarpButtonClicked()
    {
        Ability timeWarp = mySpellBook.GetAbilityByName("Time Warp");

        if (EntityLogic.IsAbilityUseable(this, timeWarp))
        {
            Debug.Log("Time Warp button clicked, awaiting Time Warp target");
            awaitingTimeWarpOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(timeWarp.abilityRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }
    }
    public void OnDimensionalHexButtonClicked()
    {
        Ability dimensionalHex = mySpellBook.GetAbilityByName("Dimensional Hex");

        if (EntityLogic.IsAbilityUseable(this, dimensionalHex))
        {
            Debug.Log("Dimensional Hex button clicked, awaiting Dimensional Hex target");
            awaitingDimensionalHexOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(dimensionalHex.abilityRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }
    }
    public void OnThawButtonClicked()
    {
        Ability thaw = mySpellBook.GetAbilityByName("Thaw");

        if (EntityLogic.IsAbilityUseable(this, thaw))
        {
            Debug.Log("Thaw button clicked, awaiting Thaw target");
            awaitingThawOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, thaw), LevelManager.Instance.Tiles[gridPosition], true, false));
        }
    }
    public void OnShootButtonClicked()
    {
        Ability shoot = mySpellBook.GetAbilityByName("Shoot");

        if (EntityLogic.IsAbilityUseable(this, shoot))
        {
            Debug.Log("Shoot button clicked, awaiting Shoot target");
            awaitingShootOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, shoot), LevelManager.Instance.Tiles[gridPosition], false, false));
        }       
    }
    public void OnPinningShotButtonClicked()
    {
        Ability ps = mySpellBook.GetAbilityByName("Pinning Shot");

        if (EntityLogic.IsAbilityUseable(this, ps))
        {
            Debug.Log("Pinning Shot button clicked, awaiting Pinning Shot target");
            awaitingPinningShotOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, ps), LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnSnipeButtonClicked()
    {
        Ability snipe = mySpellBook.GetAbilityByName("Snipe");

        if (EntityLogic.IsAbilityUseable(this, snipe))
        {
            Debug.Log("Snipe button clicked, awaiting Snipe target");
            awaitingSnipeOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, snipe), LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnHeadShotButtonClicked()
    {
        Ability headShot = mySpellBook.GetAbilityByName("Head Shot");

        if (EntityLogic.IsAbilityUseable(this, headShot))
        {
            Debug.Log("Head Shot button clicked, awaiting Head Shot target");
            awaitingHeadShotOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, headShot), LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnCombustionButtonClicked()
    {
        Ability combustion = mySpellBook.GetAbilityByName("Combustion");

        if (EntityLogic.IsAbilityUseable(this, combustion))
        {
            Debug.Log("Combustion button clicked, awaiting Combustion target");
            awaitingCombustionOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, combustion), LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnRapidFireButtonClicked()
    {
        Ability rapidFire = mySpellBook.GetAbilityByName("Rapid Fire");

        if (EntityLogic.IsAbilityUseable(this, rapidFire) && currentEnergy >= 10)
        {
            Debug.Log("Rapid Fire button clicked, awaiting Rapid Fire target");
            awaitingRapidFireOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, rapidFire), LevelManager.Instance.Tiles[gridPosition], false, false));
        }        
    }
    public void OnSliceAndDiceButtonClicked()
    {
        Ability sliceAndDice = mySpellBook.GetAbilityByName("Slice And Dice");

        if (EntityLogic.IsAbilityUseable(this, sliceAndDice))
        {
            Debug.Log("Slice And Dice button clicked, awaiting Slice and Dice target");
            awaitingSliceAndDiceOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }

        
    }
    public void OnPoisonDartButtonClicked()
    {
        Ability poisonDart = mySpellBook.GetAbilityByName("Poison Dart");

        if (EntityLogic.IsAbilityUseable(this, poisonDart))
        {
            Debug.Log("Poison Dart button clicked, awaiting Poison Dart target");
            awaitingPoisonDartOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, poisonDart), LevelManager.Instance.Tiles[gridPosition], true, false));
        }        
    }
    public void OnChemicalReactionButtonClicked()
    {
        Ability chemicalReaction = mySpellBook.GetAbilityByName("Chemical Reaction");

        if (EntityLogic.IsAbilityUseable(this, chemicalReaction))
        {
            Debug.Log("Chemical Reaction button clicked, awaiting Chemical Reaction target");
            awaitingChemicalReactionOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(chemicalReaction.abilityRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }

        
    }
    public void OnImpalingBoltButtonClicked()
    {
        Ability imaplingBolt = mySpellBook.GetAbilityByName("Impaling Bolt");

        if (EntityLogic.IsAbilityUseable(this, imaplingBolt))
        {
            Debug.Log("Impaling Bolt button clicked, awaiting Impaling Bolt target");
            awaitingImpalingBoltOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, imaplingBolt), LevelManager.Instance.Tiles[gridPosition], true, false));
        }
       
    }
    
    public void OnInvigorateButtonClicked()
    {
        Ability invigorate = mySpellBook.GetAbilityByName("Invigorate");

        if (EntityLogic.IsAbilityUseable(this, invigorate))
        {
            Debug.Log("Invigorate button clicked, awaiting Invigorate target");
            awaitingInvigorateOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(invigorate.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }        
    }
    public void OnSnowStasisButtonClicked()
    {
        Ability st = mySpellBook.GetAbilityByName("Snow Stasis");

        if (EntityLogic.IsAbilityUseable(this, st))
        {
            Debug.Log("Snow Stasis button clicked, awaiting Snow Stasis target");
            awaitingSnowStasisOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(st.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnHasteButtonClicked()
    {
        Ability haste = mySpellBook.GetAbilityByName("Haste");

        if (EntityLogic.IsAbilityUseable(this, haste))
        {
            Debug.Log("Haste button clicked, awaiting Haste target");
            awaitingHasteOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(haste.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnSteadyHandsButtonClicked()
    {
        Ability steadyHands = mySpellBook.GetAbilityByName("Steady Hands");

        if (EntityLogic.IsAbilityUseable(this, steadyHands))
        {
            Debug.Log("Steady Hands button clicked, awaiting Steady Hands target");
            awaitingSteadyHandsOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(steadyHands.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnHolyFireButtonClicked()
    {
        Ability holyFire = mySpellBook.GetAbilityByName("Holy Fire");

        if (EntityLogic.IsAbilityUseable(this, holyFire))
        {
            Debug.Log("Holy Fire button clicked, awaiting Holy Fire target");
            awaitingHolyFireOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(holyFire.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }       
    }
    public void OnPrimalBlastButtonClicked()
    {
        Ability primalBlast = mySpellBook.GetAbilityByName("Primal Blast");

        if (EntityLogic.IsAbilityUseable(this, primalBlast))
        {
            Debug.Log("Primal Blast button clicked, awaiting Primal Blast target");
            awaitingPrimalBlastOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, primalBlast), LevelManager.Instance.Tiles[gridPosition], false, false));
        }

        
    }
    public void OnPrimalRageButtonClicked()
    {
        Ability primalRage = mySpellBook.GetAbilityByName("Primal Rage");

        if (EntityLogic.IsAbilityUseable(this, primalRage))
        {
            Debug.Log("Primal Rage button clicked, awaiting Primal Rage target");
            awaitingPrimalRageOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(primalRage.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnPhaseShiftButtonClicked()
    {
        Ability phaseShift = mySpellBook.GetAbilityByName("Phase Shift");

        if (EntityLogic.IsAbilityUseable(this, phaseShift))
        {
            Debug.Log("Phase Shift button clicked, awaiting Phase Shift target");
            awaitingPhaseShiftOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(phaseShift.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }

       
    }
    public void OnSanctityButtonClicked()
    {
        Ability sanctity = mySpellBook.GetAbilityByName("Sanctity");

        if (EntityLogic.IsAbilityUseable(this, sanctity))
        {
            Debug.Log("Sanctity button clicked, awaiting Sanctity target");
            awaitingSanctityOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(sanctity.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }

        
    }
    public void OnBlessButtonClicked()
    {
        Ability bless = mySpellBook.GetAbilityByName("Bless");

        if (EntityLogic.IsAbilityUseable(this, bless))
        {
            Debug.Log("Bless button clicked, awaiting Bless target");
            awaitingBlessOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(bless.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }
    }
    public void OnSiphonLifeButtonClicked()
    {
        Ability siphonLife = mySpellBook.GetAbilityByName("Siphon Life");

        if (EntityLogic.IsAbilityUseable(this, siphonLife))
        {
            Debug.Log("Siphon Life button clicked, awaiting Siphon Life target");
            awaitingSiphonLifeOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(siphonLife.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }        
    }
    public void OnVoidBombButtonClicked()
    {
        Ability voidBomb = mySpellBook.GetAbilityByName("Void Bomb");

        if (EntityLogic.IsAbilityUseable(this, voidBomb))
        {
            Debug.Log("Void Bomb button clicked, awaiting Void Bomb target");
            awaitingVoidBombOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, voidBomb), LevelManager.Instance.Tiles[gridPosition], false, false));
        }        
    }
    public void OnNightmareButtonClicked()
    {
        Ability nightmare = mySpellBook.GetAbilityByName("Nightmare");

        if (EntityLogic.IsAbilityUseable(this, nightmare))
        {
            Debug.Log("Nightmare button clicked, awaiting Nightmare target");
            awaitingNightmareOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(nightmare.abilityRange, LevelManager.Instance.Tiles[gridPosition], false, false));
        }

        
    }
    public void OnWhirlwindButtonClicked()
    {
        Debug.Log("Whirlwind button clicked");

        Ability whirlwind = mySpellBook.GetAbilityByName("Whirlwind");

        if (EntityLogic.IsAbilityUseable(this, whirlwind))
        {
            AbilityLogic.Instance.PerformWhirlwind(this);
        }
    }
    public void OnSpritSurgeButtonClicked()
    {
        Debug.Log("Spirit Surge button clicked");

        Ability spiritSurge = mySpellBook.GetAbilityByName("Spirit Surge");

        if (EntityLogic.IsAbilityUseable(this, spiritSurge))
        {
            AbilityLogic.Instance.PerformSpiritSurge(this);
        }
    }
    public void OnToxicRainButtonClicked()
    {
        Debug.Log("Toxic Rain button clicked");

        Ability toxicRain = mySpellBook.GetAbilityByName("Toxic Rain");

        if (EntityLogic.IsAbilityUseable(this, toxicRain))
        {
            AbilityLogic.Instance.PerformToxicRain(this);
        }
    }
    public void OnPureHateButtonClicked()
    {
        Debug.Log("Pure Hate button clicked");

        Ability pureHate = mySpellBook.GetAbilityByName("Pure Hate");

        if (EntityLogic.IsAbilityUseable(this, pureHate))
        {
            AbilityLogic.Instance.PerformPureHate(this);
        }
    }
    public void OnGlobalCoolingButtonClicked()
    {
        Debug.Log("Global Cooling button clicked");

        Ability globalCooling = mySpellBook.GetAbilityByName("Global Cooling");

        if (EntityLogic.IsAbilityUseable(this, globalCooling))
        {
            AbilityLogic.Instance.PerformGlobalCooling(this);
        }
    }
    public void OnSecondWindButtonClicked()
    {
        Debug.Log("Second Wind button clicked");

        Ability secondWind = mySpellBook.GetAbilityByName("Second Wind");

        if (EntityLogic.IsAbilityUseable(this, secondWind))
        {
            AbilityLogic.Instance.PerformSecondWind(this);
        }
    }
    public void OnInfuseButtonClicked()
    {
        Debug.Log("Infuse button clicked");

        Ability infuse = mySpellBook.GetAbilityByName("Infuse");

        if (EntityLogic.IsAbilityUseable(this, infuse))
        {
            AbilityLogic.Instance.PerformInfuse(this);
        }
    }
    public void OnPurityButtonClicked()
    {
        Debug.Log("Purity button clicked");

        Ability purity = mySpellBook.GetAbilityByName("Purity");

        if (EntityLogic.IsAbilityUseable(this, purity))
        {
            AbilityLogic.Instance.PerformPurity(this);
        }
    }
   
    public void OnRecklessnessButtonClicked()
    {
        Debug.Log("Recklessness button clicked");

        Ability recklessness = mySpellBook.GetAbilityByName("Recklessness");

        if (EntityLogic.IsAbilityUseable(this, recklessness))
        {
            AbilityLogic.Instance.PerformRecklessness(this);
        }
    }
    public void OnRapidCloakingButtonClicked()
    {
        Debug.Log("Rapid Cloaking button clicked");

        Ability rapidCloaking = mySpellBook.GetAbilityByName("Rapid Cloaking");

        if (EntityLogic.IsAbilityUseable(this, rapidCloaking))
        {
            AbilityLogic.Instance.PerformRapidCloaking(this);
        }
    }
    public void OnTestudoButtonClicked()
    {
        Debug.Log("Testudo button clicked");

        Ability testudo = mySpellBook.GetAbilityByName("Testudo");

        if (EntityLogic.IsAbilityUseable(this, testudo))
        {
            AbilityLogic.Instance.PerformTestudo(this);
        }
    }    
    
    public void OnConcentrationButtonClicked()
    {
        Debug.Log("Concentration button clicked");

        Ability concentration = mySpellBook.GetAbilityByName("Concentration");

        if (EntityLogic.IsAbilityUseable(this, concentration))
        {
            AbilityLogic.Instance.PerformConcentration(this);
        }
    }    
    
    public void OnNoxiousFumesButtonClicked()
    {
        Debug.Log("Noxious Fumes button clicked");

        Ability noxiousFumes = mySpellBook.GetAbilityByName("Noxious Fumes");

        if (EntityLogic.IsAbilityUseable(this, noxiousFumes))
        {
            AbilityLogic.Instance.PerformNoxiousFumes(this);
        }
    }
    public void OnSharpenBladeButtonClicked()
    {
        Debug.Log("Sharpen Blade button clicked");

        Ability sharpenBlade = mySpellBook.GetAbilityByName("Sharpen Blade");

        if (EntityLogic.IsAbilityUseable(this, sharpenBlade))
        {
            AbilityLogic.Instance.PerformSharpenBlade(this);
        }
    }
    public void OnBloodOfferingButtonClicked()
    {
        Debug.Log("Blood Offering button clicked");

        Ability bloodOffering = mySpellBook.GetAbilityByName("Blood Offering");

        if (EntityLogic.IsAbilityUseable(this, bloodOffering))
        {
            AbilityLogic.Instance.PerformBloodOffering(this);
        }
    }
    public void OnUnbridledChaosButtonClicked()
    {
        Debug.Log("Unbridled Chaos button clicked");

        Ability unbridledChaos = mySpellBook.GetAbilityByName("Unbridled Chaos");

        if (EntityLogic.IsAbilityUseable(this, unbridledChaos))
        {
            AbilityLogic.Instance.PerformUnbridledChaos(this);
        }

    }
    public void OnConsecrateButtonClicked()
    {
        Debug.Log("Consecrate button clicked");

        Ability consecrate = mySpellBook.GetAbilityByName("Consecrate");

        if (EntityLogic.IsAbilityUseable(this, consecrate))
        {
            AbilityLogic.Instance.PerformConsecrate(this);
        }
    }
    public void OnOverwatchButtonClicked()
    {
        Debug.Log("Overwatch button clicked");

        Ability overwatch = mySpellBook.GetAbilityByName("Overwatch");

        if (EntityLogic.IsAbilityUseable(this, overwatch))
        {
            AbilityLogic.Instance.PerformOverwatch(this);
        }

    }
    public void OnFireNovaButtonClicked()
    {
        Debug.Log("Fire Nova button clicked");

        Ability fireNova = mySpellBook.GetAbilityByName("Fire Nova");

        if (EntityLogic.IsAbilityUseable(this, fireNova))
        {
            AbilityLogic.Instance.PerformFireNova(this);
        }

    }
    public void OnFrostNovaButtonClicked()
    {
        Debug.Log("Frost Nova button clicked");

        Ability frostNova = mySpellBook.GetAbilityByName("Frost Nova");

        if (EntityLogic.IsAbilityUseable(this, frostNova))
        {
            AbilityLogic.Instance.PerformFrostNova(this);
        }

    }
    public void OnReactiveArmourButtonClicked()
    {
        Debug.Log("Reactive Armor button clicked");

        Ability reactiveArmour = mySpellBook.GetAbilityByName("Reactive Armour");

        if (EntityLogic.IsAbilityUseable(this, reactiveArmour))
        {
            AbilityLogic.Instance.PerformReactiveArmour(this);
        }

    }
    public void OnVanishButtonClicked()
    {
        Debug.Log("Vanish button clicked");

        Ability vanish = mySpellBook.GetAbilityByName("Vanish");

        if (EntityLogic.IsAbilityUseable(this, vanish))
        {
            AbilityLogic.Instance.PerformVanish(this);
        }

    }   
    public void OnElectricalDischargeButtonClicked()
    {
        Debug.Log("Electrical Discharge button clicked");

        Ability electricalDischarge = mySpellBook.GetAbilityByName("Electrical Discharge");

        if (!EntityLogic.IsAbilityUseable(this, electricalDischarge))
        {
            return;
        }

        // damage enemies
        //CombatLogic.Instance.CreateAoEAttackEvent(this, electricalDischarge, tile, currentMeleeRange, true, false);

        // give block to allies
        List<Tile> tilesInMyMeleeRange = LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, tile);
        foreach(Defender defender in DefenderManager.Instance.allDefenders)
        {
            if (tilesInMyMeleeRange.Contains(defender.tile))
            {
                defender.ModifyCurrentBlock(CombatLogic.Instance.CalculateBlockGainedByEffect(electricalDischarge.abilityPrimaryValue, this));
            }
        }

       // OnAbilityUsed(electricalDischarge, this);

    }
    public void OnDefendButtonClicked()
    {
        Debug.Log("Defend button clicked");

        Ability block = mySpellBook.GetAbilityByName("Defend");

        if (EntityLogic.IsAbilityUseable(this, block))
        {
            AbilityLogic.Instance.PerformDefend(this);
        }

    }
    
    public void OnPreparationButtonClicked()
    {
        Debug.Log("Preparation button clicked");

        Ability preparation = mySpellBook.GetAbilityByName("Preparation");

        if (EntityLogic.IsAbilityUseable(this, preparation))
        {
            AbilityLogic.Instance.PerformPreparation(this);
        }       

    }
    public void OnTwinStrikeButtonClicked()
    {
        Ability twinStrike = mySpellBook.GetAbilityByName("Twin Strike");

        if (EntityLogic.IsAbilityUseable(this, twinStrike))
        {
            Debug.Log("Twin Strike button clicked, awaiting Twin Strike target");
            awaitingTwinStrikeOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }
       
    }
    public void OnShieldSlamButtonClicked()
    {
        Ability shieldSlam = mySpellBook.GetAbilityByName("Shield Slam");

        if (EntityLogic.IsAbilityUseable(this, shieldSlam))
        {
            Debug.Log("Shield Slam button clicked, awaiting Shield Slam target");
            awaitingShieldSlamOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }

    }
    public void OnDevastatingBlowButtonClicked()
    {
        Ability devastatingBlow = mySpellBook.GetAbilityByName("Devastating Blow");

        if (EntityLogic.IsAbilityUseable(this, devastatingBlow))
        {
            Debug.Log("Devastating Blow button clicked, awaiting Devastating Blow target");
            awaitingDevastatingBlowOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }
    }
    public void OnToxicSlashButtonClicked()
    {
        Ability toxicSlash = mySpellBook.GetAbilityByName("Toxic Slash");

        if (EntityLogic.IsAbilityUseable(this, toxicSlash))
        {
            Debug.Log("Toxic Slash button clicked, awaiting Toxic Slash target");
            awaitingToxicSlashOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }

    }
    public void OnDecapitateButtonClicked()
    {
        Ability decapitate = mySpellBook.GetAbilityByName("Decapitate");

        if (EntityLogic.IsAbilityUseable(this, decapitate))
        {
            Debug.Log("Decapitate button clicked, awaiting Decapitate target");
            awaitingDecapitateOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }

    }
    public void OnShankButtonClicked()
    {
        Ability shank = mySpellBook.GetAbilityByName("Shank");

        if (EntityLogic.IsAbilityUseable(this, shank))
        {
            Debug.Log("Shank button clicked, awaiting Shank target");
            awaitingShankOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }

    }
    public void OnCheapShotButtonClicked()
    {
        Ability cheapShot = mySpellBook.GetAbilityByName("Cheap Shot");

        if (EntityLogic.IsAbilityUseable(this, cheapShot))
        {
            Debug.Log("Cheap Shot button clicked, awaiting Cheap Shot target");
            awaitingCheapShotOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }
    }
    public void OnAmbushButtonClicked()
    {
        Ability ambush = mySpellBook.GetAbilityByName("Ambush");

        if (EntityLogic.IsAbilityUseable(this, ambush))
        {
            Debug.Log("Ambush button clicked, awaiting Ambush target");
            awaitingAmbushOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }
    }
    public void OnShadowStepButtonClicked()
    {
        Ability shadowStep = mySpellBook.GetAbilityByName("Shadow Step");

        if (EntityLogic.IsAbilityUseable(this, shadowStep))
        {
            Debug.Log("Shadow Step button clicked, awaiting Shadow Step target");
            awaitingShadowStepOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(shadowStep.abilityRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }
    }  
    public void OnThunderStrikeButtonClicked()
    {
        Ability thunderStrike = mySpellBook.GetAbilityByName("Thunder Strike");

        if (EntityLogic.IsAbilityUseable(this, thunderStrike))
        {
            Debug.Log("Thunder Strike button clicked, awaiting Thunder strike target");
            awaitingThunderStrikeOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }

    }
    public void OnJudgementButtonClicked()
    {
        Ability judgement = mySpellBook.GetAbilityByName("Judgement");

        if (EntityLogic.IsAbilityUseable(this, judgement))
        {
            Debug.Log("Judgement button clicked, awaiting Judgement target");
            awaitingJudgementOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }

    }
    public void OnEvasionButtonClicked()
    {
        Ability evasion = mySpellBook.GetAbilityByName("Evasion");

        if (EntityLogic.IsAbilityUseable(this, evasion))
        {
            Debug.Log("Evasion button clicked, awaiting Evasion target");
            awaitingEvasionOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(evasion.abilityRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }

    }
    public void OnTendonSlashButtonClicked()
    {
        Ability tendonSlash = mySpellBook.GetAbilityByName("Tendon Slash");

        if (EntityLogic.IsAbilityUseable(this, tendonSlash))
        {
            Debug.Log("Tendon Slash button clicked, awaiting Tendon Slash target");
            awaitingTendonSlashOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }

    }
    public void OnChillingBlowButtonClicked()
    {
        Ability chillingBlow = mySpellBook.GetAbilityByName("Chilling Blow");

        if (EntityLogic.IsAbilityUseable(this, chillingBlow))
        {
            Debug.Log("Chilling Blow button clicked, awaiting Chilling Blow target");
            awaitingChillingBlowOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }

    }
    public void OnShieldShatterButtonClicked()
    {
        Ability shieldShatter = mySpellBook.GetAbilityByName("Shield Shatter");

        if (EntityLogic.IsAbilityUseable(this, shieldShatter))
        {
            Debug.Log("Shield Shatter button clicked, awaiting Shield Shatter target");
            awaitingShieldShatterOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }

    }
    public void OnSwordAndBoardButtonClicked()
    {
        Ability swordAndBoard = mySpellBook.GetAbilityByName("Sword And Board");

        if (EntityLogic.IsAbilityUseable(this, swordAndBoard))
        {
            Debug.Log("Sword And Board button clicked, awaiting Sword And Board target");
            awaitingSwordAndBoardOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }

    }
    public void OnKickToTheBallsButtonClicked()
    {
        Ability kickToTheBalls = mySpellBook.GetAbilityByName("Kick To The Balls");

        if (EntityLogic.IsAbilityUseable(this, kickToTheBalls))
        {
            Debug.Log("Kick To The Balls button clicked, awaiting Kick To The Bals target");
            awaitingKickToTheBallsOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(currentMeleeRange, LevelManager.Instance.Tiles[gridPosition], true, false));
        }

    }
    public void OnBladeFlurryButtonClicked()
    {
        Debug.Log("Blade Flurry button clicked");

        Ability bladeFlurry = mySpellBook.GetAbilityByName("Blade Flurry");

        if (EntityLogic.IsAbilityUseable(this, bladeFlurry))
        {
            AbilityLogic.Instance.PerformBladeFlurry(this);
        }
    }
    public void OnGlacialBurstButtonClicked()
    {
        Debug.Log("Glacial Burst button clicked");

        Ability glacialBurst = mySpellBook.GetAbilityByName("Glacial Burst");

        if (EntityLogic.IsAbilityUseable(this, glacialBurst))
        {
            AbilityLogic.Instance.PerformGlacialBurst(this);
        }
    }
    public void OnChallengingShoutButtonClicked()
    {
        Debug.Log("Challenging Shout button clicked");

        Ability challenginShout = mySpellBook.GetAbilityByName("Challenging Shout");

        if (EntityLogic.IsAbilityUseable(this, challenginShout))
        {
            AbilityLogic.Instance.PerformChallengingShout(this);
        }
    }
    public void OnChaosBoltButtonClicked()
    {
        Ability chaosBolt = mySpellBook.GetAbilityByName("Chaos Bolt");

        if (EntityLogic.IsAbilityUseable(this, chaosBolt))
        {
            Debug.Log("Chaos Bolt button clicked, awaiting Chaos Bolt target");
            awaitingChaosBoltOrder = true;
            LevelManager.Instance.HighlightTiles(LevelManager.Instance.GetTilesWithinRange(EntityLogic.GetTotalRangeOfRangedAttack(this, chaosBolt), LevelManager.Instance.Tiles[gridPosition], true, false));
        }       
    }
    #endregion


    // Stat and Property Modifiers
    #region
    public override void ModifyCurrentMobility(int mobilityGainedOrLost)
    {
        base.ModifyCurrentMobility(mobilityGainedOrLost);
        UpdateCurrentMobilityStatText(currentMobility);
    }
    public override void ModifyCurrentEnergy(int APGainedOrLost, bool showVFX = true)
    {
        base.ModifyCurrentEnergy(APGainedOrLost, showVFX);
        UpdateCurrentEnergyText(currentEnergy);
    }
    public override void ModifyCurrentStrength(int strengthGainedOrLost)
    {
        base.ModifyCurrentStrength(strengthGainedOrLost);
        UpdateCurrentStrengthStatText(currentStrength);
    }
    #endregion


    // Text + UI Component Updates
    #region
    public void UpdateCurrentStrengthStatText(int newValue)
    {
        myCurrentStrengthStatText.text = newValue.ToString();
    }       
    public void UpdateCurrentEnergyText(int newEnergyValue)
    {
        //myCurrentEnergyText.text = newEnergyValue.ToString();
    }
    public void UpdateCurrentStaminaText(int newAPStatValue)
    {
        //myCurrentStaminaStatText.text = newAPStatValue.ToString();
    }
    public void UpdateCurrentMaxEnergyText(int newMaxAPValue)
    {
        //myCurrentMaxEnergyText.text = newMaxAPValue.ToString();
    }
    public void UpdateCurrentMobilityStatText(int newMobilityValue)
    {
        myCurrentMobilityStatText.text = newMobilityValue.ToString();
    }
    public float CalculateEnergyBarPosition()
    {
        float currentAPFloat = currentEnergy;
        float currentMaxAPFloat = currentMaxEnergy;

        return currentAPFloat / currentMaxAPFloat;
    }
    public void UpdateEnergyBarPosition()
    {
        float finalValue = CalculateEnergyBarPosition();
        myCurrentEnergyBarText.text = currentEnergy.ToString();
        myCurrentMaxEnergyBarText.text = currentMaxEnergy.ToString();
        energyBarPositionCurrentlyUpdating = false;
        StartCoroutine(UpdateEnergyBarPositionCoroutine(finalValue));

    }    
    public IEnumerator UpdateHealthBarPanelPosition(float finalValue)
    {
        float needleMoveSpeed = 0.02f;
        healthBarPositionCurrentlyUpdating = true;

        while (myHealthBarStatPanel.value != finalValue && healthBarPositionCurrentlyUpdating == true)
        {
            if (myHealthBarStatPanel.value > finalValue)
            {
                myHealthBarStatPanel.value -= needleMoveSpeed;
                if (myHealthBarStatPanel.value < finalValue)
                {
                    myHealthBarStatPanel.value = finalValue;
                }
            }
            else if (myHealthBarStatPanel.value < finalValue)
            {
                myHealthBarStatPanel.value += needleMoveSpeed;
                if (myHealthBarStatPanel.value > finalValue)
                {
                    myHealthBarStatPanel.value = finalValue;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator UpdateEnergyBarPositionCoroutine(float finalValue)
    {
        float needleMoveSpeed = 0.04f;
        energyBarPositionCurrentlyUpdating = true;

        while (myEnergyBar.value != finalValue && energyBarPositionCurrentlyUpdating == true)
        {
            if(myEnergyBar.value > finalValue)
            {
                myEnergyBar.value -= needleMoveSpeed;
                if(myEnergyBar.value < finalValue)
                {
                    myEnergyBar.value = finalValue;
                }
            }
            else if (myEnergyBar.value < finalValue)
            {
                myEnergyBar.value += needleMoveSpeed;
                if (myEnergyBar.value > finalValue)
                {
                    myEnergyBar.value = finalValue;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public void SetUpAPBarDividers()
    {
        GameObject dividersParent = myEnergyBar.transform.Find("Line Dividers Parent").gameObject;
        if(dividersParent != null)
        {
            int dividersToAdd = currentMaxEnergy - 1;
            for(int i = 0; i < dividersToAdd; i++)
            {
                Instantiate(PrefabHolder.Instance.apBarDividerPrefab, dividersParent.transform);
            }
        }
    }
    #endregion


    // Perform Abilities 
    #region
    public void StartMoveAbilityProcess(Tile destination)
    {
        // If the selected tile is within our movement range, is walkable, and unoccupied, the attempted move is valid: start moving
        if (MovementLogic.Instance.IsLocationMoveable(destination, this, EntityLogic.GetTotalMobility(this)))
        {
            Debug.Log("Selected tile is valid, starting move...");
            awaitingMoveOrder = false;
            AbilityLogic.Instance.PerformMove(this, destination);
            
        }
    }
    public void StartDashProcess(Tile destination)
    {
        Ability dash = mySpellBook.GetAbilityByName("Dash");

        // If the selected tile is within our movement range, is walkable, and unoccupied, the attempted move is valid: start moving
        if (MovementLogic.Instance.IsLocationMoveable(destination, this, dash.abilityPrimaryValue + EntityLogic.GetTotalMobility(this)))
        {
            Debug.Log("Selected tile is valid, starting move...");
            awaitingDashOrder = false;
            AbilityLogic.Instance.PerformDash(this, destination);
        }
    }
    public void StartTreeLeapProcess(Tile destination)
    {
        Ability treeLeap = mySpellBook.GetAbilityByName("Tree Leap");

        // If the selected tile is within range AND a grass tile, start teleport
        if (MovementLogic.Instance.IsLocationMoveable(destination, this, treeLeap.abilityRange) &&
            destination.myTileType == Tile.TileType.Grass)
        {
            awaitingTreeLeapOrder = false;
            AbilityLogic.Instance.PerformTreeLeap(this, destination);
        }
    }
    public void StartGetDownProcess(Tile destination)
    {
        Ability getDown = mySpellBook.GetAbilityByName("Get Down!");

        // If the selected tile is within our movement range, is walkable, and unoccupied, the attempted move is valid: start moving
        if (MovementLogic.Instance.IsLocationMoveable(destination, this, getDown.abilityPrimaryValue + EntityLogic.GetTotalMobility(this)))
        {
            Debug.Log("Selected tile is valid, starting move...");
            awaitingGetDownOrder = false;
            AbilityLogic.Instance.PerformGetDown(this, destination);
        }
    }
    public void StartStrikeProcess()
    {
        Debug.Log("Defender.StartStrikeProcess() called");
        Enemy enemyTarget = EnemyManager.Instance.selectedEnemy;
        if (EntityLogic.IsTargetInRange(this, enemyTarget, currentMeleeRange))
        {
            awaitingStrikeOrder = false;
            AbilityLogic.Instance.PerformStrike(this, enemyTarget);
        }
    }
    public void StartBackStabProcess()
    {
        Debug.Log("Defender.StartBackStabProcess() called");
        Enemy enemyTarget = EnemyManager.Instance.selectedEnemy;
        if (EntityLogic.IsTargetInRange(this, enemyTarget, currentMeleeRange))
        {
            awaitingBackStabOrder = false;
            AbilityLogic.Instance.PerformBackStab(this, enemyTarget);
        }
    }
    public void StartDisarmProcess()
    {
        Debug.Log("Defender.StartDisarmProcess() called");
        Enemy enemyTarget = EnemyManager.Instance.selectedEnemy;
        if (EntityLogic.IsTargetInRange(this, enemyTarget, currentMeleeRange))
        {
            awaitingDisarmOrder = false;
            AbilityLogic.Instance.PerformDisarm(this, enemyTarget);
        }
    }
    public void StartSmashProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartSmashProcess() called");       
        if (EntityLogic.IsTargetInRange(this, target, currentMeleeRange))
        {
            awaitingSmashOrder = false;            
            AbilityLogic.Instance.PerformSmash(this, target);
        }
    }
    public void StartShankProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartShankProcess() called");
        if (EntityLogic.IsTargetInRange(this, target, currentMeleeRange))
        {
            awaitingShankOrder = false;
            AbilityLogic.Instance.PerformShank(this, target);
        }
    }
    public void StartAmbushProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartAmbushProcess() called");
        if (EntityLogic.IsTargetInRange(this, target, currentMeleeRange))
        {
            awaitingAmbushOrder = false;
            AbilityLogic.Instance.PerformAmbush(this, target);
        }
    }
    public void StartCheapShotProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartCheapShotProcess() called");
        if (EntityLogic.IsTargetInRange(this, target, currentMeleeRange))
        {
            awaitingCheapShotOrder = false;
            AbilityLogic.Instance.PerformCheapShot(this, target);
        }
    }
    public void StartDecapitateProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartDecapitateProcess() called");
        if (EntityLogic.IsTargetInRange(this, target, currentMeleeRange))
        {
            awaitingDecapitateOrder = false;
            AbilityLogic.Instance.PerformDecapitate(this, target);
        }
    }
    public void StartShadowStepProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartShadowStepProcess() called");       

        Tile targetsBackTile = PositionLogic.Instance.GetTargetsBackArcTile(target);
        Ability shadowStep = mySpellBook.GetAbilityByName("Shadow Step");
   
        if (EntityLogic.IsTargetInRange(this, target, shadowStep.abilityRange) &&
            targetsBackTile.IsEmpty &&
            targetsBackTile.IsWalkable)
        {
            awaitingShadowStepOrder = false;
            AbilityLogic.Instance.PerformShadowStep(this, targetsBackTile);
        }
    }
    public void StartKickToTheBallsProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartKickToTheBallsProcess() called");
        if (EntityLogic.IsTargetInRange(this, target, currentMeleeRange))
        {
            awaitingKickToTheBallsOrder = false;
            AbilityLogic.Instance.PerformKickToTheBalls(this, target);
        }

    }
    public void StartDevastatingBlowProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartDevastatingBlowProcess() called");
        if (EntityLogic.IsTargetInRange(this, target, currentMeleeRange))
        {
            awaitingDevastatingBlowOrder = false;
            AbilityLogic.Instance.PerformDevastatingBlow(this, target);
        }
    }
    public void StartToxicSlashProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartToxicSlashProcess() called");
        if (EntityLogic.IsTargetInRange(this, target, currentMeleeRange))
        {
            awaitingToxicSlashOrder = false;
            AbilityLogic.Instance.PerformToxicSlash(this, target);
        }
    }
    public void StartThunderStrikeProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartThunderStrikeProcess() called");
        if (EntityLogic.IsTargetInRange(this, target, currentMeleeRange))
        {
            awaitingThunderStrikeOrder = false;
            AbilityLogic.Instance.PerformThunderStrike(this, target);
        }
    }
    public void StartJudgementProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartDevastatingBlowProcess() called");
        if (EntityLogic.IsTargetInRange(this, target, currentMeleeRange))
        {
            awaitingJudgementOrder = false;
            AbilityLogic.Instance.PerformJudgement(this, target);
        }

    }
    public void StartShieldSlamProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartShieldSlamProcess() called");
        if (EntityLogic.IsTargetInRange(this, target, currentMeleeRange))
        {
            awaitingShieldSlamOrder = false;
            AbilityLogic.Instance.PerformShieldSlam(this, target);
        }

    }
    public void StartShieldShatterProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartShieldShatterProcess() called");
        if (EntityLogic.IsTargetInRange(this, target, currentMeleeRange))
        {
            awaitingShieldShatterOrder = false;
            AbilityLogic.Instance.PerformShieldShatter(this, target);
        }

    }
    public void StartTendonSlashProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartTendonSlashProcess() called");
        if (EntityLogic.IsTargetInRange(this, target, currentMeleeRange))
        {
            awaitingTendonSlashOrder = false;
            AbilityLogic.Instance.PerformTendonSlash(this, target);
        }

    }
    public void StartChillingBlowProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartChillingBlowProcess() called");
        if (EntityLogic.IsTargetInRange(this, target, currentMeleeRange))
        {
            awaitingChillingBlowOrder = false;
            AbilityLogic.Instance.PerformChillingBlow(this, target);
        }

    }
    public void StartSwordAndBoardProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartSwordAndBoardProcess() called");
        if (EntityLogic.IsTargetInRange(this, target, currentMeleeRange))
        {
            awaitingSwordAndBoardOrder = false;
            AbilityLogic.Instance.PerformSwordAndBoard(this, target);
        }

    }
    public void StartChainLightningProcess(LivingEntity target)
    {
        Debug.Log("Defender.ChainLightningProcess() called");

        Ability chainLightning = mySpellBook.GetAbilityByName("Chain Lightning");
        
        if (EntityLogic.IsTargetInRange(this, target, EntityLogic.GetTotalRangeOfRangedAttack(this, chainLightning)))
        {
            awaitingChainLightningOrder = false;      
            AbilityLogic.Instance.PerformChainLightning(this, target);
        }
    }   
    public void StartMeteorProcess(Tile targetTile)
    {
        Ability meteor = mySpellBook.GetAbilityByName("Meteor");

        if(LevelManager.Instance.IsTileYWithinRangeOfTileX(tile, targetTile, EntityLogic.GetTotalRangeOfRangedAttack(this, meteor)))
        {
            awaitingMeteorOrder = false;
            AbilityLogic.Instance.PerformMeteor(this, targetTile);
        }              
    }
    public void StartChloroformBombProcess(Tile targetTile)
    {
        Ability cb = mySpellBook.GetAbilityByName("Chloroform Bomb");

        if (LevelManager.Instance.IsTileYWithinRangeOfTileX(tile, targetTile, EntityLogic.GetTotalRangeOfRangedAttack(this, cb)))
        {
            awaitingChloroformBombOrder = false;
            AbilityLogic.Instance.PerformChloroformBomb(this, targetTile);
        }
    }
    public void StartDragonBreathProcess(Tile targetTile)
    {
        Ability dragonBreath = mySpellBook.GetAbilityByName("Dragon Breath");

        if (LevelManager.Instance.IsTileYWithinRangeOfTileX(tile, targetTile, dragonBreath.abilityRange))
        {
            awaitingDragonBreathOrder = false;
            AbilityLogic.Instance.PerformDragonBreath(this, targetTile);
        }
    }
    public void StartBlizzardProcess(Tile targetTile)
    {
        Ability blizzard = mySpellBook.GetAbilityByName("Blizzard");

        if (LevelManager.Instance.IsTileYWithinRangeOfTileX(tile, targetTile, EntityLogic.GetTotalRangeOfRangedAttack(this, blizzard)))
        {
            awaitingBlizzardOrder = false;
            AbilityLogic.Instance.PerformBlizzard(this, targetTile);
        }

    }
    public void StartBlindingLightProcess(Tile targetTile)
    {
        Ability blindingLight = mySpellBook.GetAbilityByName("Blinding Light");

        if (LevelManager.Instance.IsTileYWithinRangeOfTileX(tile, targetTile, EntityLogic.GetTotalRangeOfRangedAttack(this, blindingLight)))
        {
            awaitingBlindingLightOrder = false;
            AbilityLogic.Instance.PerformBlindingLight(this, targetTile);
        }
    }
    public void StartConcealingCloudsProcess(Tile targetTile)
    {
        Ability concealingClouds = mySpellBook.GetAbilityByName("Concealing Clouds");

        if (LevelManager.Instance.IsTileYWithinRangeOfTileX(tile, targetTile, concealingClouds.abilityRange))
        {
            awaitingConcealingCloudsOrder = false;
            AbilityLogic.Instance.PerformConcealingClouds(this, targetTile);
        }

    }
    public void StartToxicEruptionProcess(Tile targetTile)
    {
        Ability toxicEruption = mySpellBook.GetAbilityByName("Toxic Eruption");

        if (LevelManager.Instance.IsTileYWithinRangeOfTileX(tile, targetTile, EntityLogic.GetTotalRangeOfRangedAttack(this, toxicEruption)))
        {
            awaitingToxicEruptionOrder = false;
            AbilityLogic.Instance.PerformToxicEruption(this, targetTile);
        }

    }
    public void StartThunderStormProcess(Tile targetTile)
    {
        Ability thunderStorm = mySpellBook.GetAbilityByName("Thunder Storm");

        if (LevelManager.Instance.IsTileYWithinRangeOfTileX(tile, targetTile, EntityLogic.GetTotalRangeOfRangedAttack(this, thunderStorm)))
        {
            awaitingThunderStormOrder = false;
            AbilityLogic.Instance.PerformThunderStorm(this, targetTile);
        }

    }
    public void StartRainOfChaosProcess(Tile targetTile)
    {
        Ability rainOfChaos = mySpellBook.GetAbilityByName("Rain Of Chaos");

        if (LevelManager.Instance.IsTileYWithinRangeOfTileX(tile, targetTile, EntityLogic.GetTotalRangeOfRangedAttack(this, rainOfChaos)))
        {
            awaitingRainOfChaosOrder = false;
            AbilityLogic.Instance.PerformRainOfChaos(this, targetTile);
        }

    }    
    public void StartInspireProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartInspireProcess() called");
        Ability inspire = mySpellBook.GetAbilityByName("Inspire");

        if (EntityLogic.IsTargetInRange(this, target, inspire.abilityRange))
        {
            awaitingInspireOrder = false;
            AbilityLogic.Instance.PerformInspire(this, target);
        }
    }
    public void StartStoneFormProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartStoneFormProcess() called");
        Ability stoneForm = mySpellBook.GetAbilityByName("Stone Form");

        if (EntityLogic.IsTargetInRange(this, target, stoneForm.abilityRange))
        {
            awaitingStoneFormOrder = false;
            AbilityLogic.Instance.PerformStoneForm(this, target);
        }
    }
    public void StartGoBerserkProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartGoBerserkProcess() called");
        Ability inspire = mySpellBook.GetAbilityByName("Go Berserk");

        if (EntityLogic.IsTargetInRange(this, target, inspire.abilityRange))
        {
            awaitingGoBerserkOrder = false;
            AbilityLogic.Instance.PerformGoBerserk(this, target);
        }
    }
    public void StartTranscendenceProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartTranscendenceProcess() called");
        Ability transcendence = mySpellBook.GetAbilityByName("Transcendence");

        if (EntityLogic.IsTargetInRange(this, target, transcendence.abilityRange))
        {
            awaitingTranscendenceOrder = false;
            AbilityLogic.Instance.PerformTranscendence(this, target);
        }
    }
    public void StartProvokeProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartProvokeProcess() called");
        Ability provoke = mySpellBook.GetAbilityByName("Provoke");

        if (EntityLogic.IsTargetInRange(this, target, provoke.abilityRange))
        {
            awaitingProvokeOrder = false;
            AbilityLogic.Instance.PerformProvoke(this, target);
        }
    }
    public void StartHexProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartHexProcess() called");
        Ability hex = mySpellBook.GetAbilityByName("Hex");

        if (EntityLogic.IsTargetInRange(this, target, hex.abilityRange))
        {
            awaitingHexOrder = false;
            AbilityLogic.Instance.PerformHex(this, target);
        }
    }
    public void StartMarkTargetProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartMarkTargetProcess() called");
        Ability mt = mySpellBook.GetAbilityByName("Mark Target");

        if (EntityLogic.IsTargetInRange(this, target, mt.abilityRange))
        {
            awaitingMarkTargetOrder = false;
            AbilityLogic.Instance.PerformMarkTarget(this, target);
        }
    }
    public void StartBlightProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartBlightProcess() called");
        Ability blight = mySpellBook.GetAbilityByName("Blight");

        if (EntityLogic.IsTargetInRange(this, target, blight.abilityRange))
        {
            awaitingBlightOrder = false;
            AbilityLogic.Instance.PerformBlight(this, target);
        }
    }
    public void StartShroudProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartShroudProcess() called");
        Ability shroud = mySpellBook.GetAbilityByName("Shroud");

        if (EntityLogic.IsTargetInRange(this, target, shroud.abilityRange))
        {
            awaitingShroudOrder = false;
            AbilityLogic.Instance.PerformShroud(this, target);
        }
    }
    public void StartIcyFocusProcess(Defender target)
    {
        Debug.Log("Defender.StartIcyFocusProcess() called");
        Ability icyFocus = mySpellBook.GetAbilityByName("Icy Focus");

        if (EntityLogic.IsTargetInRange(this, target, icyFocus.abilityRange))
        {
            awaitingIcyFocusOrder = false;
            AbilityLogic.Instance.PerformIcyFocus(this, target);
        }
    }
    public void StartDarkGiftProcess(Defender target)
    {
        Debug.Log("Defender.StartDarkGiftProcess() called");
        Ability icyFocus = mySpellBook.GetAbilityByName("Dark Gift");

        if (EntityLogic.IsTargetInRange(this, target, icyFocus.abilityRange))
        {
            awaitingDarkGiftOrder = false;
            AbilityLogic.Instance.PerformDarkGift(this, target);
        }
    }    
    public void StartTimeWarpProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartTimeWarpProcess() called");
        Ability timeWarp = mySpellBook.GetAbilityByName("Time Warp");

        if (EntityLogic.IsTargetInRange(this, target, timeWarp.abilityRange))
        {
            awaitingTimeWarpOrder = false;
            AbilityLogic.Instance.PerformTimeWarp(this, target);
        }
    }
    public void StartSpiritVisionProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartSpiritVisionProcess() called");
        Ability spiritSurge = mySpellBook.GetAbilityByName("Spirit Vision");

        if (EntityLogic.IsTargetInRange(this, target, spiritSurge.abilityRange))
        {
            awaitingSpiritVisionOrder = false;
            AbilityLogic.Instance.PerformSpiritVision(this, target);
        }
    }
    public void StartHasteProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartHasteProcess() called");
        Ability haste = mySpellBook.GetAbilityByName("Haste");

        if (EntityLogic.IsTargetInRange(this, target, haste.abilityRange))
        {
            awaitingHasteOrder = false;
            AbilityLogic.Instance.PerformHaste(this, target);
        }
    }
    public void StartSteadyHandsProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartSteadyHandsProcess() called");
        Ability steadyHands = mySpellBook.GetAbilityByName("Steady Hands");

        if (EntityLogic.IsTargetInRange(this, target, steadyHands.abilityRange))
        {
            awaitingSteadyHandsOrder = false;
            AbilityLogic.Instance.PerformSteadyHands(this, target);
        }
    }
    public void StartEvasionProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartEvasionProcess() called");
        Ability evasion = mySpellBook.GetAbilityByName("Evasion");

        if (EntityLogic.IsTargetInRange(this, target, evasion.abilityRange))
        {
            awaitingEvasionOrder = false;
            AbilityLogic.Instance.PerformEvasion(this, target);
        }
    }
    public void StartBurstOfKnowledgeProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartBurstOfKnowledgeProcess() called");
        Ability burstOfKnowledge = mySpellBook.GetAbilityByName("Burst Of Knowledge");

        if (EntityLogic.IsTargetInRange(this, target, burstOfKnowledge.abilityRange))
        {
            awaitingBurstOfKnowledgeOrder = false;
            AbilityLogic.Instance.PerformBurstOfKnowledge(this, target);
        }
    }
    public void StartMirageProcess(Defender targetOfEvasion)
    {
        Debug.Log("Defender.StartMirageProcess() called");
        Ability mirage = mySpellBook.GetAbilityByName("Mirage");

        if (EntityLogic.IsTargetInRange(this, targetOfEvasion, mirage.abilityRange))
        {
            awaitingMirageOrder = false;
            AbilityLogic.Instance.PerformMirage(this, targetOfEvasion);
        }
    }
    public void StartChargeLocationSettingProcess()
    {
        Ability charge = mySpellBook.GetAbilityByName("Charge");

        Debug.Log("Defender.StartChargeLocationSettingProcess() called");
        Enemy enemyTarget = EnemyManager.Instance.selectedEnemy;
        if (EntityLogic.IsTargetInRange(this, enemyTarget, charge.abilityPrimaryValue + EntityLogic.GetTotalMobility(this)))
        {
            awaitingChargeLocationOrder = true;
            awaitingChargeTargetOrder = false;

            List<Tile> tilesWithinChargeRangeOfCharacter = LevelManager.Instance.GetValidMoveableTilesWithinRange(charge.abilityPrimaryValue + EntityLogic.GetTotalMobility(this), tile);
            List<Tile> tilesWithinMeleeRangeOfTarget = LevelManager.Instance.GetValidMoveableTilesWithinRange(currentMeleeRange, enemyTarget.tile);
            List<Tile> validChargeLocationTiles = new List<Tile>();

            foreach (Tile tile in tilesWithinMeleeRangeOfTarget)
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
    public void StartChargeProcess(Tile destination)
    {
        Enemy enemyTarget = EnemyManager.Instance.selectedEnemy;
        Ability charge = mySpellBook.GetAbilityByName("Charge");

        List<Tile> tilesWithinChargeRangeOfCharacter = LevelManager.Instance.GetValidMoveableTilesWithinRange(charge.abilityPrimaryValue + EntityLogic.GetTotalMobility(this), tile);
        List<Tile> tilesWithinMeleeRangeOfTarget = LevelManager.Instance.GetValidMoveableTilesWithinRange(1, enemyTarget.tile);
        List<Tile> validChargeLocationTiles = new List<Tile>();

        foreach (Tile tile in tilesWithinMeleeRangeOfTarget)
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

        if (EntityLogic.IsTargetInRange(this, targetOfGuard, guard.abilityRange))
        {
            awaitingGuardOrder = false;
            AbilityLogic.Instance.PerformGuard(this, targetOfGuard);
        }
    }
    public void StartFortifyProcess(LivingEntity targetOfGuard)
    {
        Ability fortify = mySpellBook.GetAbilityByName("Fortify");

        if (EntityLogic.IsTargetInRange(this, targetOfGuard, fortify.abilityRange))
        {
            awaitingFortifyOrder = false;
            AbilityLogic.Instance.PerformFortify(this, targetOfGuard);
        }
    }
    public void StartFrostArmourProcess(LivingEntity target)
    {
        Ability frostArmour = mySpellBook.GetAbilityByName("Frost Armour");

        if (EntityLogic.IsTargetInRange(this, target, frostArmour.abilityRange))
        {
            awaitingFrostArmourOrder = false;
            AbilityLogic.Instance.PerformFrostArmour(this, target);
        }
    }
    public void StartForestMedicineProcess(LivingEntity target)
    {
        Ability forestMedicine = mySpellBook.GetAbilityByName("Forest Medicine");

        if (EntityLogic.IsTargetInRange(this, target, forestMedicine.abilityRange))
        {
            awaitingForestMedicineOrder = false;
            AbilityLogic.Instance.PerformForestMedicine(this, target);
        }
    }
    public void StartTelekinesisProcess(LivingEntity target, Tile destination)
    {        
        Ability telekinesis = mySpellBook.GetAbilityByName("Telekinesis");
        
        List<Tile> validTeleportLocations = LevelManager.Instance.GetValidMoveableTilesWithinRange(telekinesis.abilityPrimaryValue, target.tile, true);

        if (validTeleportLocations.Contains(destination))
        {
            awaitingTelekinesisTargetOrder = false;
            awaitingTelekinesisLocationOrder = false;
            AbilityLogic.Instance.PerformTelekinesis(this, target, destination);
        }
    }
    public void StartBlinkProcess(Tile destination)
    {
        Ability blink = mySpellBook.GetAbilityByName("Blink");

        List<Tile> validTeleportLocations = LevelManager.Instance.GetValidMoveableTilesWithinRange(blink.abilityRange, tile, true);

        if (validTeleportLocations.Contains(destination))
        {
            awaitingBlinkOrder = false;
            AbilityLogic.Instance.PerformBlink(this, destination);
        }
    }
    public void StartPhoenixDiveProcess(Tile destination)
    {
        Ability phoenixDive = mySpellBook.GetAbilityByName("Phoenix Dive");

        List<Tile> validTeleportLocations = LevelManager.Instance.GetValidMoveableTilesWithinRange(phoenixDive.abilityRange, tile, true);

        if (validTeleportLocations.Contains(destination))
        {
            awaitingPhoenixDiveOrder = false;
            AbilityLogic.Instance.PerformPhoenixDive(this, destination);
        }

    }
    public void StartTelekinesisLocationSettingProcess(LivingEntity targetBeingTeleported)
    {
        Ability telekinesis = mySpellBook.GetAbilityByName("Telekinesis");

        Debug.Log("Defender.StartTelekinesisLocationSettingProcess() called");
        //Enemy target = EnemyManager.Instance.selectedEnemy;
        LivingEntity target = targetBeingTeleported;
        if (EntityLogic.IsTargetInRange(this, target, telekinesis.abilityRange))
        {
            awaitingTelekinesisTargetOrder = false;
            awaitingTelekinesisLocationOrder = true;

            List<Tile> tilesWithinTeleportingRangeOfTarget = LevelManager.Instance.GetValidMoveableTilesWithinRange(telekinesis.abilityPrimaryValue, target.tile);

            LevelManager.Instance.UnhighlightAllTiles();
            LevelManager.Instance.HighlightTiles(tilesWithinTeleportingRangeOfTarget);
            myCurrentTarget = target;
        }
    }
    public void StartFrostBoltProcess(LivingEntity target)
    {
        Ability frostbolt = mySpellBook.GetAbilityByName("Frost Bolt");
        Debug.Log("Defender.StartFrostBoltProcess() called");

        if (EntityLogic.IsTargetInRange(this, target, EntityLogic.GetTotalRangeOfRangedAttack(this, frostbolt)))
        {                      
            awaitingFrostBoltOrder = false;            
            AbilityLogic.Instance.PerformFrostBolt(this, target);
        }
    }
    public void StartThawProcess(LivingEntity target)
    {
        Ability thaw = mySpellBook.GetAbilityByName("Thaw");
        Debug.Log("Defender.StartThawProcess() called");

        if (EntityLogic.IsTargetInRange(this, target, EntityLogic.GetTotalRangeOfRangedAttack(this,thaw)))
        {
            awaitingThawOrder = false;
            AbilityLogic.Instance.PerformThaw(this, target);
        }
    }
    public void StartFireBallProcess(LivingEntity target)
    {
        Ability fireball = mySpellBook.GetAbilityByName("Fire Ball");
        Debug.Log("Defender.StartFireBallProcess() called");        
        if (EntityLogic.IsTargetInRange(this, target, EntityLogic.GetTotalRangeOfRangedAttack(this, fireball)))
        {
            awaitingFireBallOrder = false;
            AbilityLogic.Instance.PerformFireBall(this, target);                       
        }
    }
    public void StartMeltProcess(LivingEntity target)
    {
        Ability melt = mySpellBook.GetAbilityByName("Melt");
        Debug.Log("Defender.StartMeltProcess() called");
        if (EntityLogic.IsTargetInRange(this, target, EntityLogic.GetTotalRangeOfRangedAttack(this, melt)))
        {
            awaitingMeltOrder = false;
            AbilityLogic.Instance.PerformMelt(this, target);
        }
    }
    public void StartShadowBlastProcess(LivingEntity target)
    {
        Ability shadowBlast = mySpellBook.GetAbilityByName("Shadow Blast");
        Debug.Log("Defender.StartShadowBlastProcess() called");
        if (EntityLogic.IsTargetInRange(this, target, EntityLogic.GetTotalRangeOfRangedAttack(this, shadowBlast)))
        {
            awaitingShadowBlastOrder = false;
            AbilityLogic.Instance.PerformShadowBlast(this, target);
        }
    }
    public void StartLightningBoltProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartLightningBoltProcess() called");
        Ability lightningBolt = mySpellBook.GetAbilityByName("Lightning Bolt");
        
        if (EntityLogic.IsTargetInRange(this, target, EntityLogic.GetTotalRangeOfRangedAttack(this, lightningBolt)))
        {
            awaitingLightningBoltOrder = false;
            AbilityLogic.Instance.PerformLightningBolt(this, target);
        }
    }
    public void StartDimensionalBlastProcess(LivingEntity target)
    {
        Debug.Log("Defender.DimesionalBlastProcess() called");

        Ability dimensionalBlast = mySpellBook.GetAbilityByName("Dimensional Blast");        
        if (EntityLogic.IsTargetInRange(this, target, EntityLogic.GetTotalRangeOfRangedAttack(this, dimensionalBlast)))
        {
            awaitingDimensionalBlastOrder = false;
            AbilityLogic.Instance.PerformDimensionalBlast(this, target);
        }
    }
    public void StartShootProcess()
    {
        Ability shoot = mySpellBook.GetAbilityByName("Shoot");
        Debug.Log("Defender.StartShootProcess() called");
        Enemy enemyTarget = EnemyManager.Instance.selectedEnemy;
        if (EntityLogic.IsTargetInRange(this, enemyTarget, EntityLogic.GetTotalRangeOfRangedAttack(this, shoot)))
        {
            //PerformShoot(enemyTarget, CalculateDamage(shoot.abilityPrimaryValue, enemyTarget, this));
            awaitingShootOrder = false;
            AbilityLogic.Instance.PerformShoot(this, enemyTarget);
        }
    }
    public void StartPinningShotProcess()
    {
        Ability ps = mySpellBook.GetAbilityByName("Pinning Shot");
        Debug.Log("Defender.StartPinningShotProcess() called");
        Enemy enemyTarget = EnemyManager.Instance.selectedEnemy;
        if (EntityLogic.IsTargetInRange(this, enemyTarget, EntityLogic.GetTotalRangeOfRangedAttack(this, ps)))
        {
            awaitingPinningShotOrder = false;
            AbilityLogic.Instance.PerformPinningShot(this, enemyTarget);
        }
    }
    public void StartSnipeProcess()
    {
        Ability snipe = mySpellBook.GetAbilityByName("Snipe");
        Debug.Log("Defender.StartSnipeProcess() called");
        Enemy enemyTarget = EnemyManager.Instance.selectedEnemy;
        if (EntityLogic.IsTargetInRange(this, enemyTarget, EntityLogic.GetTotalRangeOfRangedAttack(this, snipe)))
        {
            awaitingSnipeOrder = false;
            AbilityLogic.Instance.PerformSnipe(this, enemyTarget);
        }
    }
    public void StartHeadShotProcess(LivingEntity target)
    {
        Ability headShot = mySpellBook.GetAbilityByName("Head Shot");
        Debug.Log("Defender.HeadShotProcess() called");
        if (EntityLogic.IsTargetInRange(this, target, EntityLogic.GetTotalRangeOfRangedAttack(this, headShot)))
        {
            awaitingHeadShotOrder = false;
            AbilityLogic.Instance.PerformHeadShot(this, target);
        }
    }
    public void StartRapidFireProcess(LivingEntity target)
    {
        Ability rapidFire = mySpellBook.GetAbilityByName("Rapid Fire");
        Debug.Log("Defender.HeadShotProcess() called");

        // if target is in range and shooter has enough energy to make at least 1 shot
        if (EntityLogic.IsTargetInRange(this, target, EntityLogic.GetTotalRangeOfRangedAttack(this, rapidFire)) &&
            currentEnergy >= 10)
        {
            awaitingRapidFireOrder = false;
            AbilityLogic.Instance.PerformRapidFire(this, target);
        }
    }
    public void StartSuperConductorProcess(LivingEntity target)
    {
        Ability superConductor = mySpellBook.GetAbilityByName("Super Conductor");
        Debug.Log("Defender.SuperConductorProcess() called");

        // if target is in range and shooter has enough energy to make at least 1 shot
        if (EntityLogic.IsTargetInRange(this, target, EntityLogic.GetTotalRangeOfRangedAttack(this, superConductor)) &&
            currentEnergy >= 10)
        {
            awaitingSuperConductorOrder = false;
            AbilityLogic.Instance.PerformSuperConductor(this, target);
        }
    }
    public void StartDimensionalHexProcess(LivingEntity target)
    {
        Ability dimensionalHex = mySpellBook.GetAbilityByName("Dimensional Hex");
        Debug.Log("Defender.DimensionalHexProcess() called");

        if (EntityLogic.IsTargetInRange(this, target, dimensionalHex.abilityRange)) 
        {
            awaitingDimensionalHexOrder = false;
            AbilityLogic.Instance.PerformDimensionalHex(this, target);
        }
    }
    public void StartCombustionProcess(LivingEntity target)
    {
        Ability combustion = mySpellBook.GetAbilityByName("Combustion");
        Debug.Log("Defender.CombustionProcess() called");

        if (EntityLogic.IsTargetInRange(this, target, EntityLogic.GetTotalRangeOfRangedAttack(this, combustion)))
        {
            awaitingCombustionOrder = false;
            AbilityLogic.Instance.PerformCombustion(this, target);
        }
    }
    public void StartSliceAndDiceProcess(LivingEntity target)
    {
        Debug.Log("Defender.StartSliceAndDiceProcess() called");

        if (EntityLogic.IsTargetInRange(this, target, currentMeleeRange))
        {
            awaitingSliceAndDiceOrder = false;
            AbilityLogic.Instance.PerformSliceAndDice(this, target);
        }
    }
   
    public void StartChemicalReactionProcess(LivingEntity target)
    {
        Debug.Log("Defender.ChemicalReactionProcess() called");

        Ability chemicalReaction = mySpellBook.GetAbilityByName("Chemical Reaction");   

        if (EntityLogic.IsTargetInRange(this, target, chemicalReaction.abilityRange))
        {
            awaitingChemicalReactionOrder = false;
            AbilityLogic.Instance.PerformChemicalReaction(this, target);
        }
    }
    public void StartDrainProcess(LivingEntity target)
    {
        Debug.Log("Defender.DrainProcess() called");

        Ability drain = mySpellBook.GetAbilityByName("Drain");

        if (EntityLogic.IsTargetInRange(this, target, drain.abilityRange))
        {
            awaitingDrainOrder = false;
            AbilityLogic.Instance.PerformDrain(this, target);
        }
    }
    public void StartImpalingBoltProcess(LivingEntity target)
    {
        Ability impalingBolt = mySpellBook.GetAbilityByName("Impaling Bolt");

        if (EntityLogic.IsTargetInRange(this, target, EntityLogic.GetTotalRangeOfRangedAttack(this, impalingBolt)))
        {
            awaitingImpalingBoltOrder = false;            
            AbilityLogic.Instance.PerformImpalingBolt(this, target);
        }
    }   
    public void StartInvigorateProcess(LivingEntity target)
    {
        Ability invigorate = mySpellBook.GetAbilityByName("Invigorate");
        if (EntityLogic.IsTargetInRange(this, target, invigorate.abilityRange))
        {
            awaitingInvigorateOrder = false;
            AbilityLogic.Instance.PerformInvigorate(this, target);
        }        
    }
    public void StartCreepingFrostProcess(LivingEntity target)
    {
        Ability creepingFrost = mySpellBook.GetAbilityByName("Creeping Frost");
        if (EntityLogic.IsTargetInRange(this, target, creepingFrost.abilityRange))
        {
            awaitingCreepingFrostOrder = false;
            AbilityLogic.Instance.PerformCreepingFrost(this, target);
        }
    }
    public void StartBlazeProcess(LivingEntity target)
    {
        Ability blaze = mySpellBook.GetAbilityByName("Blaze");
        if (EntityLogic.IsTargetInRange(this, target, blaze.abilityRange))
        {
            awaitingBlazeOrder = false;
            AbilityLogic.Instance.PerformBlaze(this, target);
        }
    }
    public void StartOverloadProcess(LivingEntity target)
    {
        Ability overload = mySpellBook.GetAbilityByName("Overload");
        if (EntityLogic.IsTargetInRange(this, target, overload.abilityRange))
        {
            awaitingOverloadOrder = false;
            AbilityLogic.Instance.PerformOverload(this, target);
        }
    }
    public void StartShadowWreathProcess(LivingEntity target)
    {
        Ability shadowWreath = mySpellBook.GetAbilityByName("Shadow Wreath");
        if (EntityLogic.IsTargetInRange(this, target, shadowWreath.abilityRange))
        {
            awaitingShadowWreathOrder = false;
            AbilityLogic.Instance.PerformShadowWreath(this, target);
        }
    }
    public void StartSnowStasisProcess(LivingEntity target)
    {
        Ability st = mySpellBook.GetAbilityByName("Snow Stasis");
        if (EntityLogic.IsTargetInRange(this, target, st.abilityRange))
        {
            awaitingSnowStasisOrder = false;
            AbilityLogic.Instance.PerformSnowStasis(this, target);
        }
    }
   
    public void StartHolyFireProcess(LivingEntity target)
    {   
        Ability holyFire = mySpellBook.GetAbilityByName("Holy Fire");

        if (EntityLogic.IsTargetInRange(this, target, holyFire.abilityRange))
        {
            awaitingHolyFireOrder = false;
            AbilityLogic.Instance.PerformHolyFire(this, target);
        }
    }
    public void StartPrimalRageProcess(LivingEntity target)
    {        
        Ability primalRage = mySpellBook.GetAbilityByName("Primal Rage");

        if (EntityLogic.IsTargetInRange(this, target, primalRage.abilityRange))
        {
            awaitingPrimalRageOrder = false;
            AbilityLogic.Instance.PerformPrimalRage(this, target);
        }                
    }
    public void StartPhaseShiftProcess(LivingEntity target)
    {
        Ability phaseShift = mySpellBook.GetAbilityByName("Phase Shift");

        if (EntityLogic.IsTargetInRange(this, target, phaseShift.abilityRange))
        {
            awaitingPhaseShiftOrder = false;
            AbilityLogic.Instance.PerformPhaseShift(this, target);
        }       
          
    }
 
    public void StartChaosBoltProcess(LivingEntity target)
    {
        Ability chaosBolt = mySpellBook.GetAbilityByName("Chaos Bolt");

        if (EntityLogic.IsTargetInRange(this, target, EntityLogic.GetTotalRangeOfRangedAttack(this, chaosBolt)))
        {
            awaitingChaosBoltOrder = false;
            AbilityLogic.Instance.PerformChaosBolt(this, target);
        }
    }
    
    public void StartBlessProcess(LivingEntity target)
    {
        Ability bless = mySpellBook.GetAbilityByName("Bless");

        if (EntityLogic.IsTargetInRange(this, target, bless.abilityRange))
        {
            awaitingBlessOrder = false;
            AbilityLogic.Instance.PerformBless(this, target);
        }
    }
    
    public void StartNightmareProcess(LivingEntity target)
    {
        Ability nightmare = mySpellBook.GetAbilityByName("Nightmare");
        if (EntityLogic.IsTargetInRange(this, target, nightmare.abilityRange))
        {
            awaitingNightmareOrder = false;
            AbilityLogic.Instance.PerformNightmare(this, target);
        }
        
    }
    public void StartTwinStrikeProcess(LivingEntity target)
    {        
        Debug.Log("Defender.StartTwinStrikeProcess() called");
        //Enemy enemyTarget = EnemyManager.Instance.selectedEnemy;
        if (EntityLogic.IsTargetInRange(this, target, currentMeleeRange))
        {
            awaitingTwinStrikeOrder = false;
            AbilityLogic.Instance.PerformTwinStrike(this, target);
        }
    }
    #endregion








}
