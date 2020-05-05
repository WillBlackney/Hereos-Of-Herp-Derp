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
        if (myLivingEntity.defender)
        {
            LearnAbilitiesFromCharacterData();
        }        
    }
    public void LearnAbilitiesFromCharacterData()
    {
        foreach(AbilityPageAbility abilityData in myLivingEntity.defender.myCharacterData.activeAbilities)
        {
            DefenderLearnAbility(abilityData.myData.abilityName);
        }

    }
    public void SetAbilityIndexPosition(Ability ability)
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
        Debug.Log("SpellBook.GetAbilityByName() called, searching for " + abilityName);
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
        Debug.Log("SpellBook.EnemyLearnAbility() called, enemy " + myLivingEntity.name + " learning ability " + abilityName);

        Ability newAbility = gameObject.AddComponent<Ability>();
        newAbility.SetupBaseProperties(AbilityLibrary.Instance.GetAbilityByName(abilityName));
        SetAbilityIndexPosition(newAbility);
        newAbility.myLivingEntity = myLivingEntity;
        myActiveAbilities.Add(newAbility);
        myLivingEntity.enemy.myInfoPanel.AddAbilityToolTipToView(newAbility);
    }
    public void DefenderLearnAbility(string abilityName)
    {
        Debug.Log("SpellBook.DefenderLearnAbility() called, defender " + myLivingEntity.myName + " learning ability " + abilityName); 
        myLivingEntity.defender.myAbilityBar.PlaceButtonOnNextAvailableSlot(abilityName);
    }
    #endregion
   
    
   
}
