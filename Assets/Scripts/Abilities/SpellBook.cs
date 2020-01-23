using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    // Properties
    #region
    [Header("Properties")]
    public LivingEntity myLivingEntity;
    public List<Ability> myActiveAbilities;

    [Header("Ability Buttons")]
    public Ability AbilityOne;
    public Ability AbilityTwo;
    public Ability AbilityThree;
    public Ability AbilityFour;
    public Ability AbilityFive;
    public Ability AbilitySix;
    public Ability AbilitySeven;
    public Ability AbilityEight;
    public Ability AbilityNine;
    public Ability AbilityTen;

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
    public bool KnowsEmpowerBinding;

    public bool KnowsRapidCloaking;
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
    public bool KnowsShadowBlast;
    public bool KnowsAmbush;
    public bool KnowsStalk;
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
    public bool KnowsDisarm;
    public bool KnowsSecondWind;
    #endregion

    // Initialization + Setup
    #region
    public void InitializeSetup()
    {
        myLivingEntity = GetComponent<LivingEntity>();
        
        if (myLivingEntity.GetComponent<Defender>())
        {
            LearnAbilitiesFromCharacterData();
        }        
    }
    public void LearnAbilitiesFromCharacterData()
    {
        CharacterData cd = myLivingEntity.GetComponent<Defender>().myCharacterData;

        if (cd.KnowsMove == true)
        {
            LearnMove();
        }

        if (cd.KnowsStrike == true)
        {
            LearnStrike();
        }

        if (cd.KnowsDefend == true)
        {
            LearnDefend();
        }

        if (cd.KnowsCharge == true)
        {
            LearnCharge();
        }

        if (cd.KnowsGuard == true)
        {
            LearnGuard();
        }
        if (cd.KnowsSnipe == true)
        {
            LearnSnipe();
        }

        if (cd.KnowsInspire == true)
        {
            LearnInspire();
        }

        if (cd.KnowsMeteor == true)
        {
            LearnMeteor();
        }

        if (cd.KnowsTelekinesis == true)
        {
            LearnTelekinesis();
        }

        if (cd.KnowsFrostBolt == true)
        {
            LearnFrostBolt();
        }

        if (cd.KnowsFireBall == true)
        {
            LearnFireBall();
        }
        if (cd.KnowsFrostNova == true)
        {
            LearnFrostNova();
        }
        if (cd.KnowsRapidCloaking == true)
        {
            LearnRapidCloaking();
        }
        if (cd.KnowsShoot == true)
        {
            LearnShoot();
        }
        if (cd.KnowsRapidFire == true)
        {
            LearnRapidFire();
        }
        if (cd.KnowsImpalingBolt == true)
        {
            LearnImpalingBolt();
        }
        if (cd.KnowsForestMedicine == true)
        {
            LearnForestMedicine();
        }
        if (cd.KnowsWhirlwind == true)
        {
            LearnWhirlwind();
        }
        if (cd.KnowsInvigorate == true)
        {
            LearnInvigorate();
        }
        if (cd.KnowsHolyFire == true)
        {
            LearnHolyFire();
        }
        if (cd.KnowsVoidBomb == true)
        {
            LearnVoidBomb();
        }
        if (cd.KnowsShadowBlast == true)
        {
            LearnShadowBlast();
        }
        if (cd.KnowsNightmare == true)
        {
            LearnNightmare();
        }
        if (cd.KnowsTwinStrike == true)
        {
            LearnTwinStrike();
        }
        if (cd.KnowsDash == true)
        {
            LearnDash();
        }
        if (cd.KnowsPreparation == true)
        {
            LearnPreparation();
        }
        if (cd.KnowsHealingLight == true)
        {
            LearnHealingLight();
        }
        if (cd.KnowsSliceAndDice == true)
        {
            LearnSliceAndDice();
        }
        if (cd.KnowsPoisonDart == true)
        {
            LearnPoisonDart();
        }
        if (cd.KnowsChemicalReaction == true)
        {
            LearnChemicalReaction();
        }
        if (cd.KnowsBloodLust == true)
        {
            LearnBloodLust();
        }
        if (cd.KnowsGetDown == true)
        {
            LearnGetDown();
        }
        if (cd.KnowsSmash == true)
        {
            LearnSmash();
        }

        if (cd.KnowsLightningShield == true)
        {
            LearnLightningShield();
        }
        if (cd.KnowsElectricalDischarge == true)
        {
            LearnElectricalDischarge();
        }
        if (cd.KnowsChainLightning == true)
        {
            LearnChainLightning();
        }
        if (cd.KnowsPrimalRage == true)
        {
            LearnPrimalRage();
        }
        if (cd.KnowsPrimalBlast == true)
        {
            LearnPrimalBlast();
        }
        if (cd.KnowsPhaseShift == true)
        {
            LearnPhaseShift();
        }
        if (cd.KnowsTeleport == true)
        {
            LearnTeleport();
        }
        if (cd.KnowsSanctity == true)
        {
            LearnSanctity();
        }
        if (cd.KnowsBless == true)
        {
            LearnBless();
        }
        if (cd.KnowsSiphonLife == true)
        {
            LearnSiphonLife();
        }
        if (cd.KnowsChaosBolt)
        {
            LearnChaosBolt();
        }

        // new abilities
        if (cd.KnowsDevastatingBlow == true)
        {
            LearnDevastatingBlow();
        }
        if (cd.KnowsKickToTheBalls == true)
        {
            LearnKickToTheBalls();
        }
        if (cd.KnowsBladeFlurry == true)
        {
            LearnBladeFlurry();
        }
        if (cd.KnowsRecklessness == true)
        {
            LearnRecklessness();
        }
        if (cd.KnowsTendonSlash == true)
        {
            LearnTendonSlash();
        }
        if (cd.KnowsShieldShatter == true)
        {
            LearnShieldShatter();
        }
        if (cd.KnowsEvasion == true)
        {
            LearnEvasion();
        }
        if (cd.KnowsDecapitate == true)
        {
            LearnDecapitate();
        }
        if (cd.KnowsVanish == true)
        {
            LearnVanish();
        }
        if (cd.KnowsCheapShot == true)
        {
            LearnCheapShot();
        }
        if (cd.KnowsShank == true)
        {
            LearnShank();
        }
        if (cd.KnowsShadowStep == true)
        {
            LearnShadowStep();
        }
        if (cd.KnowsAmbush == true)
        {
            LearnAmbush();
        }
        if (cd.KnowsDisarm == true)
        {
            LearnDisarm();
        }
        if (cd.KnowsSharpenBlade == true)
        {
            LearnSharpenBlade();
        }
        if (cd.KnowsProvoke == true)
        {
            LearnProvoke();
        }
        if (cd.KnowsSwordAndBoard == true)
        {
            LearnSwordAndBoard();
        }
        if (cd.KnowsShieldSlam == true)
        {
            LearnShieldSlam();
        }
        if (cd.KnowsTestudo == true)
        {
            LearnTestudo();
        }
        if (cd.KnowsReactiveArmour == true)
        {
            LearnReactiveArmour();
        }
        if (cd.KnowsChallengingShout == true)
        {
            LearnChallengingShout();
        }
        if (cd.KnowsFireNova == true)
        {
            LearnFireNova();
        }
        if (cd.KnowsPhoenixDive == true)
        {
            LearnPhoenixDive();
        }
        if (cd.KnowsBlaze == true)
        {
            LearnBlaze();
        }
        if (cd.KnowsCombustion == true)
        {
            LearnCombustion();
        }
        if (cd.KnowsDragonBreath == true)
        {
            LearnDragonBreath();
        }
        if (cd.KnowsChillingBlow == true)
        {
            LearnChillingBlow();
        }
        if (cd.KnowsIcyFocus == true)
        {
            LearnIcyFocus();
        }
        if (cd.KnowsBlizzard == true)
        {
            LearnBlizzard();
        }
        if (cd.KnowsFrostArmour == true)
        {
            LearnFrostArmour();
        }
        if (cd.KnowsGlacialBurst == true)
        {
            LearnGlacialBurst();
        }
        if (cd.KnowsCreepingFrost == true)
        {
            LearnCreepingFrost();
        }
        if (cd.KnowsThaw == true)
        {
            LearnThaw();
        }
        if (cd.KnowsHaste == true)
        {
            LearnHaste();
        }
        if (cd.KnowsSteadyHands == true)
        {
            LearnSteadyHands();
        }
        if (cd.KnowsHeadShot == true)
        {
            LearnHeadShot();
        }
        if (cd.KnowsTreeLeap == true)
        {
            LearnTreeLeap();
        }
        if (cd.KnowsConcentration == true)
        {
            LearnConcentration();
        }
        if (cd.KnowsOverwatch == true)
        {
            LearnOverwatch();
        }
        if (cd.KnowsDimensionalBlast == true)
        {
            LearnDimenisonalBlast();
        }
        if (cd.KnowsMirage == true)
        {
            LearnMirage();
        }
        if (cd.KnowsBurstOfKnowledge == true)
        {
            LearnBurstOfKnowledge();
        }
        if (cd.KnowsBlink == true)
        {
            LearnBlink();
        }
        if (cd.KnowsInfuse == true)
        {
            LearnInfuse();
        }
        if (cd.KnowsTimeWarp == true)
        {
            LearnTimeWarp();
        }
        if (cd.KnowsDimensionalHex == true)
        {
            LearnDimensionalHex();
        }
        if (cd.KnowsConsecrate == true)
        {
            LearnConsecrate();
        }
        if (cd.KnowsPurity == true)
        {
            LearnPurity();
        }
        if (cd.KnowsBlindingLight == true)
        {
            LearnBlindingLight();
        }
        if (cd.KnowsTranscendence == true)
        {
            LearnTranscendence();
        }
        if (cd.KnowsJudgement == true)
        {
            LearnJudgement();
        }
        if (cd.KnowsShroud == true)
        {
            LearnShroud();
        }
        if (cd.KnowsHex == true)
        {
            LearnHex();
        }
        if (cd.KnowsRainOfChaos == true)
        {
            LearnRainOfChaos();
        }
        if (cd.KnowsShadowWreath == true)
        {
            LearnShadowWreath();
        }
        if (cd.KnowsUnbridledChaos == true)
        {
            LearnUnbridledChaos();
        }
        if (cd.KnowsBlight == true)
        {
            LearnBlight();
        }
        if (cd.KnowsBloodOffering == true)
        {
            LearnBloodOffering();
        }
        if (cd.KnowsSecondWind == true)
        {
            LearnSecondWind();
        }
        if (cd.KnowsToxicSlash == true)
        {
            LearnToxicSlash();
        }
        if (cd.KnowsNoxiousFumes == true)
        {
            LearnNoxiousFumes();
        }
        if (cd.KnowsToxicEruption == true)
        {
            LearnToxicEruption();
        }
        if (cd.KnowsDrain == true)
        {
            LearnDrain();
        }
        if (cd.KnowsSpiritSurge == true)
        {
            LearnSpiritSurge();
        }
        if (cd.KnowsLightningBolt == true)
        {
            LearnLightningBolt();
        }
        if (cd.KnowsThunderStrike == true)
        {
            LearnThunderStrike();
        }
        if (cd.KnowsSpiritVision == true)
        {
            LearnSpiritVision();
        }
        if (cd.KnowsThunderStorm == true)
        {
            LearnThunderStorm();
        }
        if (cd.KnowsOverload == true)
        {
            LearnOverload();
        }
        if (cd.KnowsConcealingClouds == true)
        {
            LearnConcealingClouds();
        }
        if (cd.KnowsSuperConductor == true)
        {
            LearnSuperConductor();
        }


    }
    public void PlaceAbilityOnNextAvailableSlot(Ability ability)
    {
        if (AbilityOne == null)
        {
            AbilityOne = ability;
        }

        else if (AbilityTwo == null)
        {
            AbilityTwo = ability;
        }

        else if (AbilityThree == null)
        {
            AbilityThree = ability;
        }

        else if (AbilityFour == null)
        {
            AbilityFour = ability;
        }

        else if (AbilityFive == null)
        {
            AbilityFive = ability;
        }

        else if (AbilitySix == null)
        {
            AbilitySix = ability;
        }

        else if (AbilitySeven == null)
        {
            AbilitySeven = ability;
        }

        else if (AbilityEight == null)
        {
            AbilityEight = ability;
        }

        else if (AbilityNine == null)
        {
            AbilityNine = ability;
        }
        else if (AbilityTen == null)
        {
            AbilityTen = ability;
        }


    }
    public void SetNewAbilityDescriptions()
    {
        if (myLivingEntity.defender)
        {
            foreach (Ability ability in myActiveAbilities)
            {
                TextLogic.SetAbilityDescriptionText(ability);
            }
        }
        
    }
    #endregion

    // Logic
    #region
    public Ability GetAbilityByName(string abilityName)
    {
        Ability ability = null;
        foreach (Ability abilityButton in myActiveAbilities)
        {
            if (abilityButton.abilityName == abilityName)
            {
                ability = abilityButton;
            }
        }
        if(ability == null)
        {
            Debug.Log("SpellBook.GetAbilityByName() could not find an ability with the name '" + abilityName + "', returning null...");
        }

        return ability;
    }
    public void EnemyLearnAbility(string abilityName)
    {        
        Ability newAbility = gameObject.AddComponent<Ability>();
        newAbility.SetupBaseProperties(AbilityLibrary.Instance.GetAbilityByName(abilityName));
        PlaceAbilityOnNextAvailableSlot(newAbility);
        newAbility.myLivingEntity = myLivingEntity;
        myActiveAbilities.Add(newAbility);
        myLivingEntity.GetComponent<Enemy>().myInfoPanel.AddAbilityToolTipToView(newAbility);
    }
    public void DefenderLearnAbility(string abilityName)
    {
        GetComponent<Defender>().myAbilityBar.PlaceButtonOnNextAvailableSlot(abilityName);
    }
    #endregion

    // Learn Abilities
    #region
    public void LearnMove()
    {
        KnowsMove = true;              

        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Move");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Move");
        }        
    }
    public void LearnStrike()
    {
        KnowsStrike = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Strike");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Strike");
        }

    }
    public void LearnDefend()
    {
        KnowsBlock = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Defend");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Defend");
        }

    }

    // Warrior abilities
    public void LearnCharge()
    {
        KnowsCharge = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Charge");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Charge");
        }

    }
    public void LearnInspire()
    {
        KnowsInspire = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Inspire");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Inspire");
        }

    }
    public void LearnGuard()
    {
        KnowsGuard = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Guard");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Guard");
        }

    }
    public void LearnSnipe()
    {
        KnowsSnipe = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Snipe");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Snipe");
        }

    }

    // Mage abilities

    public void LearnMeteor()
    {
        KnowsMeteor = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Meteor");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Meteor");
        }

    }
    public void LearnTelekinesis()
    {
        KnowsTelekinesis = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Telekinesis");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Telekinesis");
        }

    }
    public void LearnFrostBolt()
    {
        KnowsFrostBolt = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Frost Bolt");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Frost Bolt");
        }

    }
    public void LearnFireBall()
    {
        KnowsFireBall = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Fire Ball");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Fire Ball");
        }

    }
    public void LearnFrostNova()
    {
        KnowsFrostNova = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Frost Nova");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Frost Nova");
        }

    }
    public void LearnRapidCloaking()
    {
        KnowsRapidCloaking = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Rapid Cloaking");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Rapid Cloaking");
        }

    }
    public void LearnShoot()
    {
        KnowsShoot = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Shoot");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Shoot");
        }

    }
    public void LearnRapidFire()
    {
        KnowsRapidFire = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Rapid Fire");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Rapid Fire");
        }

    }
    public void LearnImpalingBolt()
    {
        KnowsImpalingBolt = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Impaling Bolt");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Impaling Bolt");
        }
    }
    public void LearnForestMedicine()
    {
        KnowsForestMedicine = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Forest Medicine");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Forest Medicine");
        }

    }
    public void LearnWhirlwind()
    {
        KnowsWhirlwind = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Whirlwind");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Whirlwind");
        }

    }
    public void LearnInvigorate()
    {
        KnowsInvigorate = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Invigorate");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Invigorate");
        }

    }
    public void LearnHolyFire()
    {
        KnowsHolyFire = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Holy Fire");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Holy Fire");
        }

    }
    public void LearnVoidBomb()
    {
        KnowsVoidBomb = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Void Bomb");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Void Bomb");
        }

    }
    public void LearnNightmare()
    {
        KnowsNightmare = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Nightmare");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Nightmare");
        }
    }
    public void LearnShadowBlast()
    {
        KnowsShadowBlast = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Shadow Blast");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Shadow Blast");
        }

    }
    public void LearnTwinStrike()
    {
        KnowsTwinStrike = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Twin Strike");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Twin Strike");
        }

    }
    public void LearnDash()
    {
        KnowsDash = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Dash");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Dash");
        }

    }
    public void LearnPreparation()
    {
        KnowsPreparation = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Preparation");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Preparation");
        }

    }
    public void LearnHealingLight()
    {
        KnowsHealingLight = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Healing Light");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Healing Light");
        }

    }
    public void LearnSliceAndDice()
    {
        KnowsSliceAndDice = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Slice And Dice");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Slice And Dice");
        }

    }
    public void LearnPoisonDart()
    {
        KnowsPoisonDart = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Poison Dart");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Poison Dart");
        }

    }
    public void LearnChemicalReaction()
    {
        KnowsChemicalReaction = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Chemical Reaction");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Chemical Reaction");
        }

    }
    public void LearnBloodLust()
    {
        KnowsBloodLust = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Blood Lust");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Blood Lust");
        }

    }
    public void LearnGetDown()
    {
        KnowsGetDown = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Get Down!");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Get Down!");
        }

    }
    public void LearnSmash()
    {
        KnowsSmash = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Smash");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Smash");
        }

    }
    public void LearnLightningShield()
    {
        KnowsLightningShield = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Lightning Shield");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Lightning Shield");
        }

    }
    public void LearnElectricalDischarge()
    {
        KnowsElectricalDischarge = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Electrical Discharge");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Electrical Discharge");
        }

    }
    public void LearnChainLightning()
    {
        KnowsChainLightning = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Chain Lightning");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Chain Lightning");
        }

    }
    public void LearnPrimalBlast()
    {
        KnowsPrimalBlast = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Primal Blast");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Primal Blast");
        }

    }
    public void LearnPrimalRage()
    {
        KnowsPrimalRage = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Primal Rage");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Primal Rage");
        }

    }
    public void LearnPhaseShift()
    {
        KnowsPhaseShift = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Phase Shift");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Phase Shift");
        }

    }
    public void LearnTeleport()
    {
        KnowsTeleport = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Teleport");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Teleport");
        }

    }
    public void LearnSanctity()
    {
        KnowsSanctity = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Sanctity");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Sanctity");
        }

    }
    public void LearnBless()
    {
        KnowsBless = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Bless");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Bless");
        }

    }
    public void LearnSiphonLife()
    {
        KnowsSiphonLife = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Siphon Life");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Siphon Life");
        }

    }
    public void LearnChaosBolt()
    {
        KnowsChaosBolt = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Chaos Bolt");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Chaos Bolt");
        }

    }
    public void LearnEmpowerBinding()
    {
        KnowsEmpowerBinding = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Empower Binding");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Empower Binding");
        }

    }

    // New Abilities
    public void LearnDevastatingBlow()
    {
        KnowsDevastatingBlow = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Devastating Blow");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Devastating Blow");
        }

    }
    public void LearnKickToTheBalls()
    {
        KnowsKickToTheBalls = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Kick To The Balls");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Kick To The Balls");
        }

    }
    public void LearnBladeFlurry()
    {
        KnowsBladeFlurry = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Blade Flurry");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Blade Flurry");
        }

    }
    public void LearnRecklessness()
    {
        KnowsRecklessness = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Recklessness");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Recklessness");
        }

    }
    public void LearnTendonSlash()
    {
        KnowsTendonSlash = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Tendon Slash");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Tendon Slash");
        }

    }
    public void LearnShieldShatter()
    {
        KnowsShieldShatter = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Shield Shatter");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Shield Shatter");
        }

    }
    public void LearnEvasion()
    {
        KnowsEvasion = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Evasion");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Evasion");
        }

    }
    public void LearnDecapitate()
    {
        KnowsDecapitate = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Decapitate");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Decapitate");
        }

    }
    public void LearnVanish()
    {
        KnowsVanish = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Vanish");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Vanish");
        }

    }
    public void LearnCheapShot()
    {
        KnowsCheapShot = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Cheap Shot");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Cheap Shot");
        }

    }
    public void LearnShank()
    {
        KnowsShank = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Shank");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Shank");
        }

    }
    public void LearnShadowStep()
    {
        KnowsShadowStep = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Shadow Step");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Shadow Step");
        }

    }
    public void LearnAmbush()
    {
        KnowsAmbush = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Ambush");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Ambush");
        }

    }
    public void LearnDisarm()
    {
        KnowsDisarm = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Disarm");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Disarm");
        }

    }

    public void LearnSharpenBlade()
    {
        KnowsSharpenBlade = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Sharpen Blade");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Sharpen Blade");
        }

    }
    public void LearnProvoke()
    {
        KnowsProvoke = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Provoke");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Provoke");
        }

    }
    public void LearnSwordAndBoard()
    {
        KnowsSwordAndBoard = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Sword And Board");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Sword And Board");
        }

    }
    public void LearnShieldSlam()
    {
        KnowsShieldSlam = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Shield Slam");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Shield Slam");
        }

    }
    public void LearnTestudo()
    {
        KnowsTestudo = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Testudo");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Testudo");
        }

    }
    public void LearnReactiveArmour()
    {
        KnowsReactiveArmour = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Reactive Armour");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Reactive Armour");
        }

    }
    public void LearnChallengingShout()
    {
        KnowsChallengingShout = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Challenging Shout");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Challenging Shout");
        }

    }
    public void LearnFireNova()
    {
        KnowsFireNova = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Fire Nova");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Fire Nova");
        }

    }
    public void LearnPhoenixDive()
    {
        KnowsPhoenixDive = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Phoenix Dive");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Phoenix Dive");
        }

    }
    public void LearnBlaze()
    {
        KnowsBlaze = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Blaze");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Blaze");
        }

    }
    public void LearnCombustion()
    {
        KnowsCombustion = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Combustion");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Combustion");
        }

    }
    public void LearnDragonBreath()
    {
        KnowsDragonBreath = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Dragon Breath");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Dragon Breath");
        }

    }
    public void LearnChillingBlow()
    {
        KnowsChillingBlow = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Chilling Blow");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Chilling Blow");
        }

    }
    public void LearnIcyFocus()
    {
        KnowsIcyFocus = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Icy Focus");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Icy Focus");
        }

    }
    public void LearnBlizzard()
    {
        KnowsBlizzard = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Blizzard");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Blizzard");
        }

    }
    public void LearnFrostArmour()
    {
        KnowsFrostArmour = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Frost Armour");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Frost Armour");
        }

    }
    public void LearnGlacialBurst()
    {
        KnowsGlacialBurst = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Glacial Burst");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Glacial Burst");
        }

    }
    public void LearnCreepingFrost()
    {
        KnowsCreepingFrost = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Creeping Frost");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Creeping Frost");
        }

    }
    public void LearnThaw()
    {
        KnowsThaw = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Thaw");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Thaw");
        }

    }
    public void LearnHaste()
    {
        KnowsHaste = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Haste");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Haste");
        }

    }
    public void LearnSteadyHands()
    {
        KnowsSteadyHands = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Steady Hands");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Steady Hands");
        }

    }
    
    public void LearnHeadShot()
    {
        KnowsHeadShot = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Head Shot");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Head Shot");
        }

    }
    public void LearnTreeLeap()
    {
        KnowsTreeLeap = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Tree Leap");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Tree Leap");
        }

    }
    public void LearnConcentration()
    {
        KnowsConcentration = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Concentration");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Concentration");
        }

    }
    public void LearnOverwatch()
    {
        KnowsOverwatch = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Overwatch");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Overwatch");
        }

    }
    public void LearnDimenisonalBlast()
    {
        KnowsDimenisonalBlast = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Dimensional Blast");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Dimensional Blast");
        }

    }
    public void LearnMirage()
    {
        KnowsMirage = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Mirage");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Mirage");
        }

    }
    public void LearnBurstOfKnowledge()
    {
        KnowsBurstOfKnowledge = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Burst Of Knowledge");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Burst Of Knowledge");
        }

    }
    public void LearnBlink()
    {
        KnowsBlink = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Blink");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Blink");
        }

    }
    public void LearnInfuse()
    {
        KnowsInfuse = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Infuse");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Infuse");
        }

    }
    public void LearnTimeWarp()
    {
        KnowsTimeWarp = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Time Warp");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Time Warp");
        }

    }
    public void LearnDimensionalHex()
    {
        KnowsDimensionalHex = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Dimensional Hex");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Dimensional Hex");
        }

    }
    public void LearnConsecrate()
    {
        KnowsConsecrate = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Consecrate");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Consecrate");
        }

    }
    public void LearnPurity()
    {
        KnowsPurity = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Purity");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Purity");
        }

    }
    public void LearnBlindingLight()
    {
        KnowsBlindingLight = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Blinding Light");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Blinding Light");
        }

    }
    public void LearnTranscendence()
    {
        KnowsTranscendence = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Transcendence");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Transcendence");
        }

    }
    public void LearnJudgement()
    {
        KnowsJudgement = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Judgement");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Judgement");
        }

    }
    public void LearnShroud()
    {
        KnowsShroud = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Shroud");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Shroud");
        }

    }
    public void LearnHex()
    {
        KnowsHex = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Hex");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Hex");
        }

    }
    public void LearnRainOfChaos()
    {
        KnowsRainOfChaos = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Rain Of Chaos");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Rain Of Chaos");
        }

    }
    public void LearnShadowWreath()
    {
        KnowsShadowWreath = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Shadow Wreath");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Shadow Wreath");
        }

    }
    public void LearnUnbridledChaos()
    {
        KnowsUnbridledChaos = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Unbridled Chaos");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Unbridled Chaos");
        }

    }
    public void LearnBlight()
    {
        KnowsBlight = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Blight");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Blight");
        }

    }
    public void LearnBloodOffering()
    {
        KnowsBloodOffering = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Blood Offering");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Blood Offering");
        }

    }
    public void LearnSecondWind()
    {
        KnowsSecondWind = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Second Wind");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Second Wind");
        }

    }
    public void LearnToxicSlash()
    {
        KnowsToxicSlash = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Toxic Slash");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Toxic Slash");
        }

    }
    public void LearnNoxiousFumes()
    {
        KnowsNoxiousFumes = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Noxious Fumes");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Noxious Fumes");
        }

    }
    public void LearnToxicEruption()
    {
        KnowsToxicEruption = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Toxic Eruption");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Toxic Eruption");
        }

    }
    public void LearnDrain()
    {
        KnowsDrain = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Drain");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Drain");
        }

    }
    public void LearnSpiritSurge()
    {
        KnowsSpiritSurge = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Spirit Surge");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Spirit Surge");
        }

    }
    public void LearnLightningBolt()
    {
        KnowsLightningBolt = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Lightning Bolt");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Lightning Bolt");
        }

    }
    public void LearnThunderStrike()
    {
        KnowsThunderStrike = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Thunder Strike");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Thunder Strike");
        }

    }
    public void LearnSpiritVision()
    {
        KnowsSpiritVision = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Spirit Vision");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Spirit Vision");
        }

    }
    public void LearnThunderStorm()
    {
        KnowsThunderStorm = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Thunder Storm");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Thunder Storm");
        }

    }
    public void LearnOverload()
    {
        KnowsOverload = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Overload");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Overload");
        }

    }
    public void LearnConcealingClouds()
    {
        KnowsConcealingClouds = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Concealing Clouds");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Concealing Clouds");
        }

    }
    public void LearnSuperConductor()
    {
        KnowsSuperConductor = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Super Conductor");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Super Conductor");
        }

    }
    #endregion
}
