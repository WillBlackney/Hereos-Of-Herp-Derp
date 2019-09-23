using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityLibrary : MonoBehaviour
{
    public static AbilityLibrary Instance;

    public List<AbilityDataSO> AllAbilities;    

    private void Awake()
    {
        Instance = this;
        //PopulateAbilityLibrary();
    }

    /*
    public void PopulateAbilityLibrary()
    {        
        CreateMoveData();
        CreateStrikeData();
        CreateBlockData();
        CreateChargeData();
        CreateInspireData();
        CreateGuardData();
        CreateMeteorData();
        CreateTelekinesisData();
        CreateFrostBoltData();
        CreateFireBallData();
        CreateShootData();
        CreateRapidFireData();
        CreateImpalingBoltData();
        CreateForestMedicineData();
        CreateWhirlwindData();
        CreateSummonUndeadData();
        CreateChaosBoltData();
        CreateCrushingBlowData();
        CreateEnvigorateData();
        CreateHolyFireData();
        CreateVoidBombData();
        CreateNightmareData();
        CreateTwinStrikeData();
        CreateDashData();
        CreatePreparationData();
        CreateSnipeData();
        CreateHealingLightData();
        CreateRockTossData();
        CreateSiphonLifeData();
        CreateTeleportData();
        CreateSliceAndDiceData();
        CreatePoisonDartData();
        CreateChemicalReactionData();
        CreateBloodLustData();
        CreateGetDownData();
        CreateDoomData();
        CreateSmashData();
        CreateLightningShieldData();
        CreateElectricalDischargeData();
        CreateChainLightningData();
        CreatePrimalBlastData();
        CreatePrimalRageData();
        CreatePhaseShiftData();
        CreateSanctityData();
        CreateBlessData();
    }


    
    public void CreateInspireData()
    {
        Ability inspire = gameObject.AddComponent<Ability>();
        //Ability move = new Ability();

        inspire.abilityImage = inspireImage;
        inspire.abilityName = inspireName;
        inspire.abilityDescription = inspireDescription;
        inspire.abilityBaseCooldownTime = inspireCooldownTime;
        inspire.abilityAPCost = inspireAPCost;
        inspire.abilityRange = inspireRange;
        inspire.abilityPrimaryValue = inspirePrimaryValue;
        inspire.abilitySecondaryValue = inspireSecondaryValue;

        AllAbilities.Add(inspire);
    }

    public void CreateMoveData()
    {
        Ability move = gameObject.AddComponent<Ability>();
        //Ability move = new Ability();

        move.abilityImage = moveImage;
        move.abilityName = moveName;
        move.abilityDescription = moveDescription;
        move.abilityBaseCooldownTime = moveCooldownTime;
        move.abilityAPCost = moveAPCost;
        move.abilityRange = moveRange;
        move.abilityPrimaryValue = movePrimaryValue;
        move.abilitySecondaryValue = moveSecondaryValue;

        AllAbilities.Add(move);
    }

    public void CreateStrikeData()
    {
        //Ability strike = new Ability();
        Ability strike = gameObject.AddComponent<Ability>();

        strike.abilityImage = strikeImage;
        strike.abilityName = strikeName;
        strike.abilityDescription = strikeDescription;
        strike.abilityBaseCooldownTime = strikeCooldownTime;
        strike.abilityAPCost = strikeAPCost;
        strike.abilityRange = strikeRange;
        strike.abilityPrimaryValue = strikePrimaryValue;
        strike.abilitySecondaryValue = strikeSecondaryValue;

        AllAbilities.Add(strike);
    }

    public void CreateBlockData()
    {
        
        Ability block = gameObject.AddComponent<Ability>();

        block.abilityImage = blockImage;
        block.abilityName = blockName;
        block.abilityDescription = blockDescription;
        block.abilityBaseCooldownTime = blockCooldownTime;
        block.abilityAPCost = blockAPCost;
        block.abilityRange = blockRange;
        block.abilityPrimaryValue = blockPrimaryValue;
        block.abilitySecondaryValue = blockSecondaryValue;

        AllAbilities.Add(block);
    }

    public void CreateChargeData()
    {
        //Ability charge = new Ability();
        Ability charge = gameObject.AddComponent<Ability>();

        charge.abilityImage = chargeImage;
        charge.abilityName = chargeName;
        charge.abilityDescription = chargeDescription;
        charge.abilityBaseCooldownTime = chargeCooldownTime;
        charge.abilityAPCost = chargeAPCost;
        charge.abilityRange = chargeRange;
        charge.abilityPrimaryValue = chargePrimaryValue;
        charge.abilitySecondaryValue = chargeSecondaryValue;

        AllAbilities.Add(charge);
    }

    public void CreateGuardData()
    {
        //Ability strike = new Ability();
        Ability guard = gameObject.AddComponent<Ability>();

        guard.abilityImage = guardImage;
        guard.abilityName = guardName;
        guard.abilityDescription = guardDescription;
        guard.abilityBaseCooldownTime = guardCooldownTime;
        guard.abilityAPCost = guardAPCost;
        guard.abilityRange = guardRange;
        guard.abilityPrimaryValue = guardPrimaryValue;
        guard.abilitySecondaryValue = guardSecondaryValue;

        AllAbilities.Add(guard);
    }

    public void CreateMeteorData()
    {
        //Ability strike = new Ability();
        Ability meteor = gameObject.AddComponent<Ability>();

        
        meteor.abilityName = meteorName;
        meteor.abilityDescription = meteorDescription;
        meteor.abilityBaseCooldownTime = meteorCooldownTime;
        meteor.abilityAPCost = meteorAPCost;
        meteor.abilityRange = meteorRange;
        meteor.abilityPrimaryValue = meteorPrimaryValue;
        meteor.abilitySecondaryValue = meteorSecondaryValue;
        meteor.abilityImage = meteorImage;

        AllAbilities.Add(meteor);
    }

    public void CreateTelekinesisData()
    {
        Ability telekinesis = gameObject.AddComponent<Ability>();
        //Ability move = new Ability();

        telekinesis.abilityImage = telekinesisImage;
        telekinesis.abilityName = telekinesisName;
        telekinesis.abilityDescription = telekinesisDescription;
        telekinesis.abilityBaseCooldownTime = telekinesisCooldownTime;
        telekinesis.abilityAPCost = telekinesisAPCost;
        telekinesis.abilityRange = telekinesisRange;
        telekinesis.abilityPrimaryValue = telekinesisPrimaryValue;
        telekinesis.abilitySecondaryValue = telekinesisSecondaryValue;

        AllAbilities.Add(telekinesis);
    }

    public void CreateFrostBoltData()
    {        
        Ability frostbolt = gameObject.AddComponent<Ability>();

        frostbolt.abilityName = frostboltName;
        frostbolt.abilityDescription = frostboltDescription;
        frostbolt.abilityBaseCooldownTime = frostboltCooldownTime;
        frostbolt.abilityAPCost = frostboltAPCost;
        frostbolt.abilityRange = frostboltRange;
        frostbolt.abilityPrimaryValue = frostboltPrimaryValue;
        frostbolt.abilitySecondaryValue = frostboltSecondaryValue;
        frostbolt.abilityImage = frostboltImage;

        AllAbilities.Add(frostbolt);
    }

    public void CreateFireBallData()
    {
        Ability fireball = gameObject.AddComponent<Ability>();

        fireball.abilityName = fireballName;
        fireball.abilityDescription = fireballDescription;
        fireball.abilityBaseCooldownTime = fireballCooldownTime;
        fireball.abilityAPCost = fireballAPCost;
        fireball.abilityRange = fireballRange;
        fireball.abilityPrimaryValue = fireballPrimaryValue;
        fireball.abilitySecondaryValue = fireballSecondaryValue;
        fireball.abilityImage = fireballImage;

        AllAbilities.Add(fireball);
    }

    public void CreateShootData()
    {
        Ability shoot = gameObject.AddComponent<Ability>();

        shoot.abilityName = shootName;
        shoot.abilityDescription = shootDescription;
        shoot.abilityBaseCooldownTime = shootCooldownTime;
        shoot.abilityAPCost = shootAPCost;
        shoot.abilityRange = shootRange;
        shoot.abilityPrimaryValue = shootPrimaryValue;
        shoot.abilitySecondaryValue = shootSecondaryValue;
        shoot.abilityImage = shootImage;

        AllAbilities.Add(shoot);
    }

    public void CreateRapidFireData()
    {
        Ability rapidFire = gameObject.AddComponent<Ability>();

        rapidFire.abilityName = rapidFireName;
        rapidFire.abilityDescription = rapidFireDescription;
        rapidFire.abilityBaseCooldownTime = rapidFireCooldownTime;
        rapidFire.abilityAPCost = rapidFireAPCost;
        rapidFire.abilityRange = rapidFireRange;
        rapidFire.abilityPrimaryValue = rapidFirePrimaryValue;
        rapidFire.abilitySecondaryValue = rapidFireSecondaryValue;
        rapidFire.abilityImage = rapidFireImage;

        AllAbilities.Add(rapidFire);
    }

    public void CreateImpalingBoltData()
    {
        Ability impalingBolt = gameObject.AddComponent<Ability>();

        impalingBolt.abilityName = impalingBoltName;
        impalingBolt.abilityDescription = impalingBoltDescription;
        impalingBolt.abilityBaseCooldownTime = impalingBoltCooldownTime;
        impalingBolt.abilityAPCost = impalingBoltAPCost;
        impalingBolt.abilityRange = impalingBoltRange;
        impalingBolt.abilityPrimaryValue = impalingBoltPrimaryValue;
        impalingBolt.abilitySecondaryValue = impalingBoltSecondaryValue;
        impalingBolt.abilityImage = impalingBoltImage;

        AllAbilities.Add(impalingBolt);
    }

    public void CreateForestMedicineData()
    {
        Ability forestMedicine = gameObject.AddComponent<Ability>();

        forestMedicine.abilityName = forestMedicineName;
        forestMedicine.abilityDescription = forestMedicineDescription;
        forestMedicine.abilityBaseCooldownTime = forestMedicineCooldownTime;
        forestMedicine.abilityAPCost = forestMedicineAPCost;
        forestMedicine.abilityRange = forestMedicineRange;
        forestMedicine.abilityPrimaryValue = forestMedicinePrimaryValue;
        forestMedicine.abilitySecondaryValue = forestMedicineSecondaryValue;
        forestMedicine.abilityImage = forestMedicineImage;

        AllAbilities.Add(forestMedicine);
    }

    public void CreateWhirlwindData()
    {
        Ability whirlwind = gameObject.AddComponent<Ability>();

        whirlwind.abilityName = whirlwindName;
        whirlwind.abilityDescription = whirlwindDescription;
        whirlwind.abilityBaseCooldownTime = whirlwindCooldownTime;
        whirlwind.abilityAPCost = whirlwindAPCost;
        whirlwind.abilityRange = whirlwindRange;
        whirlwind.abilityPrimaryValue = whirlwindPrimaryValue;
        whirlwind.abilitySecondaryValue = whirlwindSecondaryValue;
        whirlwind.abilityImage = whirlwindImage;

        AllAbilities.Add(whirlwind);
    }

    public void CreateSummonUndeadData()
    {
        Ability summonUndead = gameObject.AddComponent<Ability>();

        summonUndead.abilityName = summonUndeadName;
        summonUndead.abilityDescription = summonUndeadDescription;
        summonUndead.abilityBaseCooldownTime = summonUndeadCooldownTime;
        summonUndead.abilityAPCost = summonUndeadAPCost;
        summonUndead.abilityRange = summonUndeadRange;
        summonUndead.abilityPrimaryValue = summonUndeadPrimaryValue;
        summonUndead.abilitySecondaryValue = summonUndeadSecondaryValue;
        summonUndead.abilityImage = summonUndeadImage;

        AllAbilities.Add(summonUndead);
    }

    public void CreateChaosBoltData()
    {
        Ability chaosBolt = gameObject.AddComponent<Ability>();

        chaosBolt.abilityName = chaosBoltName;
        chaosBolt.abilityDescription = chaosBoltDescription;
        chaosBolt.abilityBaseCooldownTime = chaosBoltCooldownTime;
        chaosBolt.abilityAPCost = chaosBoltAPCost;
        chaosBolt.abilityRange = chaosBoltRange;
        chaosBolt.abilityPrimaryValue = chaosBoltPrimaryValue;
        chaosBolt.abilitySecondaryValue = chaosBoltSecondaryValue;
        chaosBolt.abilityImage = chaosBoltImage;

        AllAbilities.Add(chaosBolt);
    }

    public void CreateCrushingBlowData()
    {
        Ability crushingBlow = gameObject.AddComponent<Ability>();

        crushingBlow.abilityName = crushingBlowName;
        crushingBlow.abilityDescription = crushingBlowDescription;
        crushingBlow.abilityBaseCooldownTime = crushingBlowCooldownTime;
        crushingBlow.abilityAPCost = crushingBlowAPCost;
        crushingBlow.abilityRange = crushingBlowRange;
        crushingBlow.abilityPrimaryValue = crushingBlowPrimaryValue;
        crushingBlow.abilitySecondaryValue = crushingBlowSecondaryValue;
        crushingBlow.abilityImage = crushingBlowImage;

        AllAbilities.Add(crushingBlow);
    }

    public void CreateEnvigorateData()
    {
        Ability envigorate = gameObject.AddComponent<Ability>();

        envigorate.abilityName = invigorateName;
        envigorate.abilityDescription = invigorateDescription;
        envigorate.abilityBaseCooldownTime = invigorateCooldownTime;
        envigorate.abilityAPCost = invigorateAPCost;
        envigorate.abilityRange = invigorateRange;
        envigorate.abilityPrimaryValue = invigoratePrimaryValue;
        envigorate.abilitySecondaryValue = invigorateSecondaryValue;
        envigorate.abilityImage = invigorateImage;

        AllAbilities.Add(envigorate);
    }

    public void CreateHolyFireData()
    {
        Ability holyFire = gameObject.AddComponent<Ability>();

        holyFire.abilityName = holyFireName;
        holyFire.abilityDescription = holyFireDescription;
        holyFire.abilityBaseCooldownTime = holyFireCooldownTime;
        holyFire.abilityAPCost = holyFireAPCost;
        holyFire.abilityRange = holyFireRange;
        holyFire.abilityPrimaryValue = holyFirePrimaryValue;
        holyFire.abilitySecondaryValue = holyFireSecondaryValue;
        holyFire.abilityImage = holyFireImage;

        AllAbilities.Add(holyFire);
    }
    public void CreateVoidBombData()
    {
        Ability voidBomb = gameObject.AddComponent<Ability>();

        voidBomb.abilityName = voidBombName;
        voidBomb.abilityDescription = voidBombDescription;
        voidBomb.abilityBaseCooldownTime = voidBombCooldownTime;
        voidBomb.abilityAPCost = voidBombAPCost;
        voidBomb.abilityRange = voidBombRange;
        voidBomb.abilityPrimaryValue = voidBombPrimaryValue;
        voidBomb.abilitySecondaryValue = voidBombSecondaryValue;
        voidBomb.abilityImage = voidBombImage;

        AllAbilities.Add(voidBomb);
    }
    public void CreateNightmareData()
    {
        Ability nightmare = gameObject.AddComponent<Ability>();

        nightmare.abilityName = nightmareName;
        nightmare.abilityDescription = nightmareDescription;
        nightmare.abilityBaseCooldownTime = nightmareCooldownTime;
        nightmare.abilityAPCost = nightmareAPCost;
        nightmare.abilityRange = nightmareRange;
        nightmare.abilityPrimaryValue = nightmarePrimaryValue;
        nightmare.abilitySecondaryValue = nightmareSecondaryValue;
        nightmare.abilityImage = nightmareImage;

        AllAbilities.Add(nightmare);
    }

    public void CreateTwinStrikeData()
    {
        Ability twinStrike = gameObject.AddComponent<Ability>();

        twinStrike.abilityName = twinStrikeName;
        twinStrike.abilityDescription = twinStrikeDescription;
        twinStrike.abilityBaseCooldownTime = twinStrikeCooldownTime;
        twinStrike.abilityAPCost = twinStrikeAPCost;
        twinStrike.abilityRange = twinStrikeRange;
        twinStrike.abilityPrimaryValue = twinStrikePrimaryValue;
        twinStrike.abilitySecondaryValue = twinStrikeSecondaryValue;
        twinStrike.abilityImage = twinStrikeImage;

        AllAbilities.Add(twinStrike);
    }

    public void CreateDashData()
    {
        Ability dash = gameObject.AddComponent<Ability>();

        dash.abilityName = dashName;
        dash.abilityDescription = dashDescription;
        dash.abilityBaseCooldownTime = dashCooldownTime;
        dash.abilityAPCost = dashAPCost;
        dash.abilityRange = dashRange;
        dash.abilityPrimaryValue = dashPrimaryValue;
        dash.abilitySecondaryValue = dashSecondaryValue;
        dash.abilityImage = dashImage;

        AllAbilities.Add(dash);
    }

    public void CreatePreparationData()
    {
        Ability preparation = gameObject.AddComponent<Ability>();

        preparation.abilityName = preparationName;
        preparation.abilityDescription = preparationDescription;
        preparation.abilityBaseCooldownTime = preparationCooldownTime;
        preparation.abilityAPCost = preparationAPCost;
        preparation.abilityRange = preparationRange;
        preparation.abilityPrimaryValue = preparationPrimaryValue;
        preparation.abilitySecondaryValue = preparationSecondaryValue;
        preparation.abilityImage = preparationImage;

        AllAbilities.Add(preparation);
    }
    public void CreateSnipeData()
    {
        Ability snipe = gameObject.AddComponent<Ability>();

        snipe.abilityName = snipeName;
        snipe.abilityDescription = snipeDescription;
        snipe.abilityBaseCooldownTime = snipeCooldownTime;
        snipe.abilityAPCost = snipeAPCost;
        snipe.abilityRange = snipeRange;
        snipe.abilityPrimaryValue = snipePrimaryValue;
        snipe.abilitySecondaryValue = snipeSecondaryValue;
        snipe.abilityImage = snipeImage;

        AllAbilities.Add(snipe);
    }

    public void CreateHealingLightData()
    {
        Ability healingLight = gameObject.AddComponent<Ability>();

        healingLight.abilityName = healingLightName;
        healingLight.abilityDescription = healingLightDescription;
        healingLight.abilityBaseCooldownTime = healingLightCooldownTime;
        healingLight.abilityAPCost = healingLightAPCost;
        healingLight.abilityRange = healingLightRange;
        healingLight.abilityPrimaryValue = healingLightPrimaryValue;
        healingLight.abilitySecondaryValue = healingLightSecondaryValue;
        healingLight.abilityImage = healingLightImage;

        AllAbilities.Add(healingLight);
    }
    public void CreateRockTossData()
    {
        Ability rockToss = gameObject.AddComponent<Ability>();

        rockToss.abilityName = rockTossName;
        rockToss.abilityDescription = rockTossDescription;
        rockToss.abilityBaseCooldownTime = rockTossCooldownTime;
        rockToss.abilityAPCost = rockTossAPCost;
        rockToss.abilityRange = rockTossRange;
        rockToss.abilityPrimaryValue = rockTossPrimaryValue;
        rockToss.abilitySecondaryValue = rockTossSecondaryValue;
        rockToss.abilityImage = rockTossImage;

        AllAbilities.Add(rockToss);
    }
    public void CreateSiphonLifeData()
    {
        Ability siphonLife = gameObject.AddComponent<Ability>();

        siphonLife.abilityName = siphonLifeName;
        siphonLife.abilityDescription = siphonLifeDescription;
        siphonLife.abilityBaseCooldownTime = siphonLifeCooldownTime;
        siphonLife.abilityAPCost = siphonLifeAPCost;
        siphonLife.abilityRange = siphonLifeRange;
        siphonLife.abilityPrimaryValue = siphonLifePrimaryValue;
        siphonLife.abilitySecondaryValue = siphonLifeSecondaryValue;
        siphonLife.abilityImage = siphonLifeImage;

        AllAbilities.Add(siphonLife);
    }
    public void CreateTeleportData()
    {
        Ability teleport = gameObject.AddComponent<Ability>();

        teleport.abilityName = teleportName;
        teleport.abilityDescription = teleportDescription;
        teleport.abilityBaseCooldownTime = teleportCooldownTime;
        teleport.abilityAPCost = teleportAPCost;
        teleport.abilityRange = teleportRange;
        teleport.abilityPrimaryValue = teleportPrimaryValue;
        teleport.abilitySecondaryValue = teleportSecondaryValue;
        teleport.abilityImage = teleportImage;

        AllAbilities.Add(teleport);
    }

    public void CreateSliceAndDiceData()
    {
        Ability sliceAndDice = gameObject.AddComponent<Ability>();

        sliceAndDice.abilityName = sliceAndDiceName;
        sliceAndDice.abilityDescription = sliceAndDiceDescription;
        sliceAndDice.abilityBaseCooldownTime = sliceAndDiceCooldownTime;
        sliceAndDice.abilityAPCost = sliceAndDiceAPCost;
        sliceAndDice.abilityRange = sliceAndDiceRange;
        sliceAndDice.abilityPrimaryValue = sliceAndDicePrimaryValue;
        sliceAndDice.abilitySecondaryValue = sliceAndDiceSecondaryValue;
        sliceAndDice.abilityImage = sliceAndDiceImage;

        AllAbilities.Add(sliceAndDice);
    }

    public void CreatePoisonDartData()
    {
        Ability poisonDart = gameObject.AddComponent<Ability>();

        poisonDart.abilityName = poisonDartName;
        poisonDart.abilityDescription = poisonDartDescription;
        poisonDart.abilityBaseCooldownTime = poisonDartCooldownTime;
        poisonDart.abilityAPCost = poisonDartAPCost;
        poisonDart.abilityRange = poisonDartRange;
        poisonDart.abilityPrimaryValue = poisonDartPrimaryValue;
        poisonDart.abilitySecondaryValue = poisonDartSecondaryValue;
        poisonDart.abilityImage = poisonDartImage;

        AllAbilities.Add(poisonDart);
    }

    public void CreateChemicalReactionData()
    {
        Ability chemicalReaction = gameObject.AddComponent<Ability>();

        chemicalReaction.abilityName = chemicalReactionName;
        chemicalReaction.abilityDescription = chemicalReactionDescription;
        chemicalReaction.abilityBaseCooldownTime = chemicalReactionCooldownTime;
        chemicalReaction.abilityAPCost = chemicalReactionAPCost;
        chemicalReaction.abilityRange = chemicalReactionRange;
        chemicalReaction.abilityPrimaryValue = chemicalReactionPrimaryValue;
        chemicalReaction.abilitySecondaryValue = chemicalReactionSecondaryValue;
        chemicalReaction.abilityImage = chemicalReactionImage;

        AllAbilities.Add(chemicalReaction);
    }
    public void CreateBloodLustData()
    {
        Ability bloodLust = gameObject.AddComponent<Ability>();

        bloodLust.abilityName = bloodLustName;
        bloodLust.abilityDescription = bloodLustDescription;
        bloodLust.abilityBaseCooldownTime = bloodLustCooldownTime;
        bloodLust.abilityAPCost = bloodLustAPCost;
        bloodLust.abilityRange = bloodLustRange;
        bloodLust.abilityPrimaryValue = bloodLustPrimaryValue;
        bloodLust.abilitySecondaryValue = bloodLustSecondaryValue;
        bloodLust.abilityImage = bloodLustImage;

        AllAbilities.Add(bloodLust);
    }

    public void CreateGetDownData()
    {
        Ability getDown = gameObject.AddComponent<Ability>();

        getDown.abilityName = getDownName;
        getDown.abilityDescription = getDownDescription;
        getDown.abilityBaseCooldownTime = getDownCooldownTime;
        getDown.abilityAPCost = getDownAPCost;
        getDown.abilityRange = getDownRange;
        getDown.abilityPrimaryValue = getDownPrimaryValue;
        getDown.abilitySecondaryValue = getDownSecondaryValue;
        getDown.abilityImage = getDownImage;

        AllAbilities.Add(getDown);
    }

    public void CreateDoomData()
    {
        Ability doom = gameObject.AddComponent<Ability>();

        doom.abilityName = doomName;
        doom.abilityDescription = doomDescription;
        doom.abilityBaseCooldownTime = doomCooldownTime;
        doom.abilityAPCost = doomAPCost;
        doom.abilityRange = doomRange;
        doom.abilityPrimaryValue = doomPrimaryValue;
        doom.abilitySecondaryValue = doomSecondaryValue;
        doom.abilityImage = doomImage;

        AllAbilities.Add(doom);
    }

    public void CreateSmashData()
    {
        Ability smash = gameObject.AddComponent<Ability>();

        smash.abilityName = smashName;
        smash.abilityDescription = smashDescription;
        smash.abilityBaseCooldownTime = smashCooldownTime;
        smash.abilityAPCost = smashAPCost;
        smash.abilityRange = smashRange;
        smash.abilityPrimaryValue = smashPrimaryValue;
        smash.abilitySecondaryValue = smashSecondaryValue;
        smash.abilityImage = smashImage;

        AllAbilities.Add(smash);
    }

    public void CreateLightningShieldData()
    {
        Ability lightningShield = gameObject.AddComponent<Ability>();

        lightningShield.abilityName = lightningShieldName;
        lightningShield.abilityDescription = lightningShieldDescription;
        lightningShield.abilityBaseCooldownTime = lightningShieldCooldownTime;
        lightningShield.abilityAPCost = lightningShieldAPCost;
        lightningShield.abilityRange = lightningShieldRange;
        lightningShield.abilityPrimaryValue = lightningShieldPrimaryValue;
        lightningShield.abilitySecondaryValue = lightningShieldSecondaryValue;
        lightningShield.abilityImage = lightningShieldImage;

        AllAbilities.Add(lightningShield);
    }

    public void CreateElectricalDischargeData()
    {
        Ability electricalDischarge = gameObject.AddComponent<Ability>();

        electricalDischarge.abilityName = electricalDischargeName;
        electricalDischarge.abilityDescription = electricalDischargeDescription;
        electricalDischarge.abilityBaseCooldownTime = electricalDischargeCooldownTime;
        electricalDischarge.abilityAPCost = electricalDischargeAPCost;
        electricalDischarge.abilityRange = electricalDischargeRange;
        electricalDischarge.abilityPrimaryValue = electricalDischargePrimaryValue;
        electricalDischarge.abilitySecondaryValue = electricalDischargeSecondaryValue;
        electricalDischarge.abilityImage = electricalDischargeImage;

        AllAbilities.Add(electricalDischarge);
    }
    public void CreateChainLightningData()
    {
        Ability chainLightning = gameObject.AddComponent<Ability>();

        chainLightning.abilityName = chainLightningName;
        chainLightning.abilityDescription = chainLightningDescription;
        chainLightning.abilityBaseCooldownTime = chainLightningCooldownTime;
        chainLightning.abilityAPCost = chainLightningAPCost;
        chainLightning.abilityRange = chainLightningRange;
        chainLightning.abilityPrimaryValue = chainLightningPrimaryValue;
        chainLightning.abilitySecondaryValue = chainLightningSecondaryValue;
        chainLightning.abilityImage = chainLightningImage;

        AllAbilities.Add(chainLightning);
    }
    public void CreatePrimalRageData()
    {
        Ability primalRage = gameObject.AddComponent<Ability>();

        primalRage.abilityName = primalRageName;
        primalRage.abilityDescription = primalRageDescription;
        primalRage.abilityBaseCooldownTime = primalRageCooldownTime;
        primalRage.abilityAPCost = primalRageAPCost;
        primalRage.abilityRange = primalRageRange;
        primalRage.abilityPrimaryValue = primalRagePrimaryValue;
        primalRage.abilitySecondaryValue = primalRageSecondaryValue;
        primalRage.abilityImage = primalRageImage;

        AllAbilities.Add(primalRage);
    }
    public void CreatePrimalBlastData()
    {
        Ability primalBlast = gameObject.AddComponent<Ability>();

        primalBlast.abilityName = primalBlastName;
        primalBlast.abilityDescription = primalBlastDescription;
        primalBlast.abilityBaseCooldownTime = primalBlastCooldownTime;
        primalBlast.abilityAPCost = primalBlastAPCost;
        primalBlast.abilityRange = primalBlastRange;
        primalBlast.abilityPrimaryValue = primalBlastPrimaryValue;
        primalBlast.abilitySecondaryValue = primalBlastSecondaryValue;
        primalBlast.abilityImage = primalBlastImage;

        AllAbilities.Add(primalBlast);
    }
    public void CreatePhaseShiftData()
    {
        Ability phaseShift = gameObject.AddComponent<Ability>();

        phaseShift.abilityName = phaseShiftName;
        phaseShift.abilityDescription = phaseShiftDescription;
        phaseShift.abilityBaseCooldownTime = phaseShiftCooldownTime;
        phaseShift.abilityAPCost = phaseShiftAPCost;
        phaseShift.abilityRange = phaseShiftRange;
        phaseShift.abilityPrimaryValue = phaseShiftPrimaryValue;
        phaseShift.abilitySecondaryValue = phaseShiftSecondaryValue;
        phaseShift.abilityImage = phaseShiftImage;

        AllAbilities.Add(phaseShift);
    }
    public void CreateSanctityData()
    {
        Ability sanctity = gameObject.AddComponent<Ability>();

        sanctity.abilityName = sanctityName;
        sanctity.abilityDescription = sanctityDescription;
        sanctity.abilityBaseCooldownTime = sanctityCooldownTime;
        sanctity.abilityAPCost = sanctityAPCost;
        sanctity.abilityRange = sanctityRange;
        sanctity.abilityPrimaryValue = sanctityPrimaryValue;
        sanctity.abilitySecondaryValue = sanctitySecondaryValue;
        sanctity.abilityImage = sanctityImage;

        AllAbilities.Add(sanctity);
    }
    public void CreateBlessData()
    {
        Ability bless = gameObject.AddComponent<Ability>();

        bless.abilityName = blessName;
        bless.abilityDescription = blessDescription;
        bless.abilityBaseCooldownTime = blessCooldownTime;
        bless.abilityAPCost = blessAPCost;
        bless.abilityRange = blessRange;
        bless.abilityPrimaryValue = blessPrimaryValue;
        bless.abilitySecondaryValue = blessSecondaryValue;
        bless.abilityImage = blessImage;

        AllAbilities.Add(bless);
    }
    */

    public AbilityDataSO GetAbilityByName(string name)
    {
        AbilityDataSO abilityReturned = null;

        foreach(AbilityDataSO ability in AllAbilities)
        {
            if(ability.abilityName == name)
            {
                abilityReturned = ability;
            }
        }

        if(abilityReturned == null)
        {
            Debug.Log("AbilityLibrary.GetAbilityByName() couldn't find a matching ability, returning null...");
        }

        return abilityReturned;
    }    

    

    [Header("Move Data")]
    public Sprite moveImage;
    public string moveName;
    public string moveDescription;
    public int moveCooldownTime;
    public int moveAPCost;
    public int moveRange;
    public int movePrimaryValue;
    public int moveSecondaryValue;

    [Header("Strike Data")]
    public Sprite strikeImage;
    public string strikeName;
    public string strikeDescription;
    public int strikeCooldownTime;
    public int strikeAPCost;
    public int strikeRange;
    public int strikePrimaryValue;
    public int strikeSecondaryValue;

    [Header("Block Data")]
    public Sprite blockImage;
    public string blockName;
    public string blockDescription;
    public int blockCooldownTime;
    public int blockAPCost;
    public int blockRange;
    public int blockPrimaryValue;
    public int blockSecondaryValue;

    [Header("Charge Data")]
    public Sprite chargeImage;
    public string chargeName;
    public string chargeDescription;
    public int chargeCooldownTime;
    public int chargeAPCost;
    public int chargeRange;
    public int chargePrimaryValue;
    public int chargeSecondaryValue;

    [Header("Inspire Data")]
    public Sprite inspireImage;
    public string inspireName;
    public string inspireDescription;
    public int inspireCooldownTime;
    public int inspireAPCost;
    public int inspireRange;
    public int inspirePrimaryValue;
    public int inspireSecondaryValue;

    [Header("Guard Data")]
    public Sprite guardImage;
    public string guardName;
    public string guardDescription;
    public int guardCooldownTime;
    public int guardAPCost;
    public int guardRange;
    public int guardPrimaryValue;
    public int guardSecondaryValue;

    [Header("Meteor Data")]
    public Sprite meteorImage;
    public string meteorName;
    public string meteorDescription;
    public int meteorCooldownTime;
    public int meteorAPCost;
    public int meteorRange;
    public int meteorPrimaryValue;
    public int meteorSecondaryValue;

    [Header("Telekinesis Data")]
    public Sprite telekinesisImage;
    public string telekinesisName;
    public string telekinesisDescription;
    public int telekinesisCooldownTime;
    public int telekinesisAPCost;
    public int telekinesisRange;
    public int telekinesisPrimaryValue;
    public int telekinesisSecondaryValue;

    [Header("Frost Bolt Data")]
    public Sprite frostboltImage;
    public string frostboltName;
    public string frostboltDescription;
    public int frostboltCooldownTime;
    public int frostboltAPCost;
    public int frostboltRange;
    public int frostboltPrimaryValue;
    public int frostboltSecondaryValue;

    [Header("Fire Ball Data")]
    public Sprite fireballImage;
    public string fireballName;
    public string fireballDescription;
    public int fireballCooldownTime;
    public int fireballAPCost;
    public int fireballRange;
    public int fireballPrimaryValue;
    public int fireballSecondaryValue;

    [Header("Shoot Data")]
    public Sprite shootImage;
    public string shootName;
    public string shootDescription;
    public int shootCooldownTime;
    public int shootAPCost;
    public int shootRange;
    public int shootPrimaryValue;
    public int shootSecondaryValue;

    [Header("Rapid Fire Data")]
    public Sprite rapidFireImage;
    public string rapidFireName;
    public string rapidFireDescription;
    public int rapidFireCooldownTime;
    public int rapidFireAPCost;
    public int rapidFireRange;
    public int rapidFirePrimaryValue;
    public int rapidFireSecondaryValue;

    [Header("Impaling Bolt Data")]
    public Sprite impalingBoltImage;
    public string impalingBoltName;
    public string impalingBoltDescription;
    public int impalingBoltCooldownTime;
    public int impalingBoltAPCost;
    public int impalingBoltRange;
    public int impalingBoltPrimaryValue;
    public int impalingBoltSecondaryValue;

    [Header("Forest Medicine Data")]
    public Sprite forestMedicineImage;
    public string forestMedicineName;
    public string forestMedicineDescription;
    public int forestMedicineCooldownTime;
    public int forestMedicineAPCost;
    public int forestMedicineRange;
    public int forestMedicinePrimaryValue;
    public int forestMedicineSecondaryValue;

    [Header("Whirlwind Data")]
    public Sprite whirlwindImage;
    public string whirlwindName;
    public string whirlwindDescription;
    public int whirlwindCooldownTime;
    public int whirlwindAPCost;
    public int whirlwindRange;
    public int whirlwindPrimaryValue;
    public int whirlwindSecondaryValue;

    [Header("Summon Undead Data")]
    public Sprite summonUndeadImage;
    public string summonUndeadName;
    public string summonUndeadDescription;
    public int summonUndeadCooldownTime;
    public int summonUndeadAPCost;
    public int summonUndeadRange;
    public int summonUndeadPrimaryValue;
    public int summonUndeadSecondaryValue;

    [Header("Chaos Bolt Data")]
    public Sprite chaosBoltImage;
    public string chaosBoltName;
    public string chaosBoltDescription;
    public int chaosBoltCooldownTime;
    public int chaosBoltAPCost;
    public int chaosBoltRange;
    public int chaosBoltPrimaryValue;
    public int chaosBoltSecondaryValue;

    [Header("Crushing Blow Data")]
    public Sprite crushingBlowImage;
    public string crushingBlowName;
    public string crushingBlowDescription;
    public int crushingBlowCooldownTime;
    public int crushingBlowAPCost;
    public int crushingBlowRange;
    public int crushingBlowPrimaryValue;
    public int crushingBlowSecondaryValue;

    [Header("Invigorate Data")]
    public Sprite invigorateImage;
    public string invigorateName;
    public string invigorateDescription;
    public int invigorateCooldownTime;
    public int invigorateAPCost;
    public int invigorateRange;
    public int invigoratePrimaryValue;
    public int invigorateSecondaryValue;

    [Header("Holy Fire Data")]
    public Sprite holyFireImage;
    public string holyFireName;
    public string holyFireDescription;
    public int holyFireCooldownTime;
    public int holyFireAPCost;
    public int holyFireRange;
    public int holyFirePrimaryValue;
    public int holyFireSecondaryValue;

    [Header("Void Bomb Data")]
    public Sprite voidBombImage;
    public string voidBombName;
    public string voidBombDescription;
    public int voidBombCooldownTime;
    public int voidBombAPCost;
    public int voidBombRange;
    public int voidBombPrimaryValue;
    public int voidBombSecondaryValue;

    [Header("Nightmare Data")]
    public Sprite nightmareImage;
    public string nightmareName;
    public string nightmareDescription;
    public int nightmareCooldownTime;
    public int nightmareAPCost;
    public int nightmareRange;
    public int nightmarePrimaryValue;
    public int nightmareSecondaryValue;

    [Header("Twin Strike Data")]
    public Sprite twinStrikeImage;
    public string twinStrikeName;
    public string twinStrikeDescription;
    public int twinStrikeCooldownTime;
    public int twinStrikeAPCost;
    public int twinStrikeRange;
    public int twinStrikePrimaryValue;
    public int twinStrikeSecondaryValue;

    [Header("Dash Data")]
    public Sprite dashImage;
    public string dashName;
    public string dashDescription;
    public int dashCooldownTime;
    public int dashAPCost;
    public int dashRange;
    public int dashPrimaryValue;
    public int dashSecondaryValue;

    [Header("Preparation Data")]
    public Sprite preparationImage;
    public string preparationName;
    public string preparationDescription;
    public int preparationCooldownTime;
    public int preparationAPCost;
    public int preparationRange;
    public int preparationPrimaryValue;
    public int preparationSecondaryValue;

    [Header("Snipe Data")]
    public Sprite snipeImage;
    public string snipeName;
    public string snipeDescription;
    public int snipeCooldownTime;
    public int snipeAPCost;
    public int snipeRange;
    public int snipePrimaryValue;
    public int snipeSecondaryValue;

    [Header("Healing Light Data")]
    public Sprite healingLightImage;
    public string healingLightName;
    public string healingLightDescription;
    public int healingLightCooldownTime;
    public int healingLightAPCost;
    public int healingLightRange;
    public int healingLightPrimaryValue;
    public int healingLightSecondaryValue;

    [Header("Rock Toss Data")]
    public Sprite rockTossImage;
    public string rockTossName;
    public string rockTossDescription;
    public int rockTossCooldownTime;
    public int rockTossAPCost;
    public int rockTossRange;
    public int rockTossPrimaryValue;
    public int rockTossSecondaryValue;

    [Header("Siphon Life Data")]
    public Sprite siphonLifeImage;
    public string siphonLifeName;
    public string siphonLifeDescription;
    public int siphonLifeCooldownTime;
    public int siphonLifeAPCost;
    public int siphonLifeRange;
    public int siphonLifePrimaryValue;
    public int siphonLifeSecondaryValue;

    [Header("Teleport Data")]
    public Sprite teleportImage;
    public string teleportName;
    public string teleportDescription;
    public int teleportCooldownTime;
    public int teleportAPCost;
    public int teleportRange;
    public int teleportPrimaryValue;
    public int teleportSecondaryValue;

    [Header("Slice and Dice Data")]
    public Sprite sliceAndDiceImage;
    public string sliceAndDiceName;
    public string sliceAndDiceDescription;
    public int sliceAndDiceCooldownTime;
    public int sliceAndDiceAPCost;
    public int sliceAndDiceRange;
    public int sliceAndDicePrimaryValue;
    public int sliceAndDiceSecondaryValue;

    [Header("Poison Dart Data")]
    public Sprite poisonDartImage;
    public string poisonDartName;
    public string poisonDartDescription;
    public int poisonDartCooldownTime;
    public int poisonDartAPCost;
    public int poisonDartRange;
    public int poisonDartPrimaryValue;
    public int poisonDartSecondaryValue;

    [Header("Chemical Reaction Data")]
    public Sprite chemicalReactionImage;
    public string chemicalReactionName;
    public string chemicalReactionDescription;
    public int chemicalReactionCooldownTime;
    public int chemicalReactionAPCost;
    public int chemicalReactionRange;
    public int chemicalReactionPrimaryValue;
    public int chemicalReactionSecondaryValue;

    [Header("Blood Lust Data")]
    public Sprite bloodLustImage;
    public string bloodLustName;
    public string bloodLustDescription;
    public int bloodLustCooldownTime;
    public int bloodLustAPCost;
    public int bloodLustRange;
    public int bloodLustPrimaryValue;
    public int bloodLustSecondaryValue;

    [Header("Get Down! Data")]
    public Sprite getDownImage;
    public string getDownName;
    public string getDownDescription;
    public int getDownCooldownTime;
    public int getDownAPCost;
    public int getDownRange;
    public int getDownPrimaryValue;
    public int getDownSecondaryValue;

    [Header("Doom Data")]
    public Sprite doomImage;
    public string doomName;
    public string doomDescription;
    public int doomCooldownTime;
    public int doomAPCost;
    public int doomRange;
    public int doomPrimaryValue;
    public int doomSecondaryValue;

    [Header("Smash Data")]
    public Sprite smashImage;
    public string smashName;
    public string smashDescription;
    public int smashCooldownTime;
    public int smashAPCost;
    public int smashRange;
    public int smashPrimaryValue;
    public int smashSecondaryValue;

    [Header("Lightning Shield Data")]
    public Sprite lightningShieldImage;
    public string lightningShieldName;
    public string lightningShieldDescription;
    public int lightningShieldCooldownTime;
    public int lightningShieldAPCost;
    public int lightningShieldRange;
    public int lightningShieldPrimaryValue;
    public int lightningShieldSecondaryValue;

    [Header("Electrical Discharge Data")]
    public Sprite electricalDischargeImage;
    public string electricalDischargeName;
    public string electricalDischargeDescription;
    public int electricalDischargeCooldownTime;
    public int electricalDischargeAPCost;
    public int electricalDischargeRange;
    public int electricalDischargePrimaryValue;
    public int electricalDischargeSecondaryValue;

    [Header("Chain Lightning Data")]
    public Sprite chainLightningImage;
    public string chainLightningName;
    public string chainLightningDescription;
    public int chainLightningCooldownTime;
    public int chainLightningAPCost;
    public int chainLightningRange;
    public int chainLightningPrimaryValue;
    public int chainLightningSecondaryValue;

    [Header("Primal Rage Data")]
    public Sprite primalRageImage;
    public string primalRageName;
    public string primalRageDescription;
    public int primalRageCooldownTime;
    public int primalRageAPCost;
    public int primalRageRange;
    public int primalRagePrimaryValue;
    public int primalRageSecondaryValue;

    [Header("Primal Blast Data")]
    public Sprite primalBlastImage;
    public string primalBlastName;
    public string primalBlastDescription;
    public int primalBlastCooldownTime;
    public int primalBlastAPCost;
    public int primalBlastRange;
    public int primalBlastPrimaryValue;
    public int primalBlastSecondaryValue;

    [Header("Phase Shift Data")]
    public Sprite phaseShiftImage;
    public string phaseShiftName;
    public string phaseShiftDescription;
    public int phaseShiftCooldownTime;
    public int phaseShiftAPCost;
    public int phaseShiftRange;
    public int phaseShiftPrimaryValue;
    public int phaseShiftSecondaryValue;

    [Header("Sanctity Data")]
    public Sprite sanctityImage;
    public string sanctityName;
    public string sanctityDescription;
    public int sanctityCooldownTime;
    public int sanctityAPCost;
    public int sanctityRange;
    public int sanctityPrimaryValue;
    public int sanctitySecondaryValue;

    [Header("Bless Data")]
    public Sprite blessImage;
    public string blessName;
    public string blessDescription;
    public int blessCooldownTime;
    public int blessAPCost;
    public int blessRange;
    public int blessPrimaryValue;
    public int blessSecondaryValue;
}
