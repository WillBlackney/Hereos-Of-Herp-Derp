using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
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

    public void InitializeSetup()
    {
        myLivingEntity = GetComponent<LivingEntity>();
        
        if (myLivingEntity.GetComponent<Defender>())
        {
            LearnAbilitiesFromCharacterData();
        }
        
    }

    public void ApplyTalentModifiers()
    {
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            foreach(Ability ability in myActiveAbilities)
            {
                // apply reductions to abilities AP/cd/range based on talents
            }
        }
    }

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
            Debug.Log("GetAbilityByName() returned a null value...");
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

    public void PlaceAbilityOnNextAvailableSlot(Ability ability)
    {
        if(AbilityOne == null)
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

    public void LearnAbilitiesFromCharacterData()
    {
        CharacterData cd = myLivingEntity.GetComponent<Defender>().myCharacterData;

        if(cd.KnowsMove == true)
        {
            LearnMove();
        }

        if(cd.KnowsStrike == true)
        {
            LearnStrike();
        }

        if (cd.KnowsBlock == true)
        {
            LearnBlock();
        }

        if (cd.KnowsCharge == true)
        {
            LearnCharge();
        }

        if(cd.KnowsGuard == true)
        {
            LearnGuard();
        }

        if(cd.KnowsInspire == true)
        {
            LearnInspire();
        }

        if(cd.KnowsMeteor == true)
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
        if (cd.KnowsPreparation== true)
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
    }

    public void LearnStartingClassAbilities()
    {
        if (myLivingEntity.myClass == LivingEntity.Class.Warrior)
        {
            RunWarriorStartingAbilityPackageSetup();
        }
        else if (myLivingEntity.myClass == LivingEntity.Class.Mage)
        {
            RunMageStartingAbilityPackageSetup();
        }
        else if (myLivingEntity.myClass == LivingEntity.Class.Ranger)
        {
            RunRangerStartingAbilityPackageSetup();
        }
        else if (myLivingEntity.myClass == LivingEntity.Class.Priest)
        {
            RunPriestStartingAbilityPackageSetup();
        }
        else if (myLivingEntity.myClass == LivingEntity.Class.Rogue)
        {
            RunRogueStartingAbilityPackageSetup();
        }
    }

    public void RunWarriorStartingAbilityPackageSetup()
    {
        LearnMove();
        LearnStrike();
        LearnCharge();
        LearnInspire();
        //LearnGuard();        
        //LearnWhirlwind();
    }

    public void RunMageStartingAbilityPackageSetup()
    {
        LearnMove();
        LearnStrike();
        LearnFireBall();            
        //LearnFrostBolt();
        LearnTelekinesis();
    }

    public void RunRangerStartingAbilityPackageSetup()
    {
        LearnMove();
        LearnStrike();
        LearnShoot();
        LearnRapidFire();
        LearnImpalingBolt();
        LearnForestMedicine();
    }

    public void RunPriestStartingAbilityPackageSetup()
    {
        LearnMove();
        LearnStrike();        
        LearnHolyFire();
        LearnInvigorate();
        //LearnInspire();

    }

    public void RunRogueStartingAbilityPackageSetup()
    {
        LearnMove();
        LearnStrike();
        LearnTwinStrike();
        LearnDash();
        //LearnPreparation();
        
    }

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
    public void LearnBlock()
    {
        KnowsBlock = true;
        Enemy enemy = myLivingEntity.GetComponent<Enemy>();
        Defender defender = myLivingEntity.GetComponent<Defender>();

        if (defender)
        {
            DefenderLearnAbility("Block");
        }

        else if (enemy)
        {
            EnemyLearnAbility("Block");
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
}
