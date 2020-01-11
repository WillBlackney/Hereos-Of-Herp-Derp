using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveManager : MonoBehaviour
{
    // Properties + Component References
    #region

    [Header("UI + Compenent References")]
    public LivingEntity myLivingEntity;

    [Header("Core Stat Bonus Passives")]
    public bool bonusStrength;
    public int bonusStrengthStacks;

    public bool bonusWisdom;
    public int bonusWisdomStacks;

    public bool bonusStamina;
    public int bonusStaminaStacks;

    public bool bonusInitiative;
    public int bonusInitiativeStacks;

    public bool bonusMobility;
    public int bonusMobilityStacks;

    public bool bonusDexterity;
    public int bonusDexterityStacks;

    public bool temporaryBonusStrength;
    public int temporaryBonusStrengthStacks;

    public bool temporaryBonusWisdom;
    public int temporaryBonusWisdomStacks;

    public bool temporaryBonusStamina;
    public int temporaryBonusStaminaStacks;

    public bool temporaryBonusInitiative;
    public int temporaryBonusInitiativeStacks;

    public bool temporaryBonusMobility;
    public int temporaryBonusMobilityStacks;

    public bool temporaryBonusDexterity;
    public int temporaryBonusDexterityStacks;

    [Header("Damage Type Modifier Passives")]
    public bool fieryImbuement;
    public int fieryImbuementStacks;

    public bool frostImbuement;
    public int frostImbuementStacks;

    public bool poisonImbuement;
    public int poisonImbuementStacks;

    public bool shadowImbuement;
    public int shadowImbuementStacks;

    public bool airImbuement;
    public int airImbuementStacks;

    public bool temporaryFieryImbuement;
    public int temporaryFieryImbuementStacks;

    public bool temporaryFrostImbuement;
    public int temporaryFrostImbuementStacks;

    public bool temporaryPoisonImbuement;
    public int temporaryPoisonImbuementStacks;

    public bool temporaryShadowImbuement;
    public int temporaryShadowImbuementStacks;

    public bool temporaryAirImbuement;
    public int temporaryAirImbuementStacks;


    [Header("Debuff + Negative Passives")]
    public bool immobilized;
    public int immobilizedStacks;

    public bool blind;
    public int blindStacks;

    public bool disarmed;
    public int disarmedStacks;

    public bool silenced;
    public int silencedStacks;

    public bool stunned;
    public int stunnedStacks;

    public bool terrified;
    public int terrifiedStacks;

    public bool weakened;
    public int weakenedStacks;

    public bool vulnerable;
    public int vulnerableStacks;

    public bool chilled;
    public bool chilledStacks;

    public bool shocked;
    public int shockedStacks;

    public bool sleep;
    public int sleepStacks;

    [Header("Non-Stacking Passives")]
    public bool unleashed;
    public int unleashedStacks;

    public bool unstoppable;
    public int unstoppableStacks;

    public bool unwavering;
    public int unwaveringStacks;

    public bool unfallible;
    public int unfallibleStacks;

    public bool incorruptable;
    public int incorruptableStacks;

    public bool camoflage;
    public int camoflageStacks;

    public bool stealth;
    public int stealthStacks;

    public bool patientStalker;
    public int patientStalkerStacks;

    public bool predator;
    public int predatorStacks;

    public bool flux;
    public int fluxStacks;

    public bool lifeSteal;
    public int lifeStealStacks;

    public bool undead;
    public int undeadStacks;

    public bool trueSight;
    public int trueSightStacks;

    public bool perfectAim;
    public int perfectAimStacks;

    public bool frozenHeart;
    public int frozenHeartStacks;

    public bool demon;
    public int demonStacks;

    public bool toxicity;
    public int toxicityStacks;

    public bool shadowForm;
    public int shadowFormStacks;

    public bool stormLord;
    public int stormLordStacks;

    public bool nimble;
    public int nimbleStacks;

    public bool masochist;
    public int masochistStacks;

    public bool slippery;
    public int slipperyStacks;

    public bool lastStand;
    public int lastStandStacks;

    public bool riposte;
    public int riposteStacks;

    public bool perfectReflexes;
    public int perfectreflexesStacks;

    public bool shatter;
    public int shatterStacks;

    public bool etherealBeing;
    public int etherealBeingStacks;

    public bool phasing;
    public int phasingStacks;

    [Header("Stacking Passive Traits")]
    public bool burning;
    public int burningStacks;

    public bool poisoned;
    public int poisonedStacks;

    public bool thorns;
    public int thornsStacks;

    public bool cautious;
    public int cautiousStacks;

    public bool Volatile;
    public int volatileStacks;

    public bool growing;
    public int growingStacks;

    public bool enrage;
    public int enrageStacks;

    public bool regeneration;
    public int regenerationStacks;

    public bool venomous;
    public int venomousStacks;

    public bool poisonous;
    public int poisonousStacks;

    public bool immolation;
    public int immolationStacks;

    public bool tenacious;
    public int tenaciousStacks;

    public bool fading;
    public int fadingStacks;

    public bool radiance;
    public int radianceStacks;

    public bool hawkEye;
    public int hawkEyeStacks;

    public bool opportunist;
    public int opportunistStacks;

    [Header("Aura Passive Traits")]
    public bool encouragingAura;
    public int encouragingAuraStacks;

    public bool fieryAura;
    public int fieryAuraStacks;

    public bool toxicAura;
    public int toxicAuraStacks;

    public bool stormAura;
    public int stormAuraStacks;

    public bool hatefulAura;
    public int hatefulAuraStacks;

    public bool soulDrainAura;
    public int soulDrainAuraStacks;

    public bool guardianAura;
    public int guardianAuraStacks;

    public bool sacredAura;
    public int sacredAuraStacks;

    public bool shadowAura;
    public int shadowAuraStacks;





    [Header("Old Known Passive Traits")]
   
    public bool preparation;
    public int preparationStacks;

    public bool poisonImmunity;
    public bool soulLink;

    public bool lightningShield;
    public int lightningShieldStacks;

    public bool thickOfTheFight;
    public int thickOfTheFightStacks;

    public bool rune;
    public int runeStacks;
    
    public bool barrier;
    public int barrierStacks;
   
    #endregion

    // Initialization + Setup
    #region
    public void InitializeSetup()
    {
        myLivingEntity = GetComponent<LivingEntity>();
    }
    #endregion

    // Learn + Modify Passive Traits
    #region

    public void ModifyImmobilized(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Immobilized");

        if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                immobilizedStacks += stacks;
                if (immobilizedStacks > 0)
                {
                    immobilized = true;
                }
                
                StartCoroutine(VisualEffectManager.Instance.
                    CreateStatusEffect(myLivingEntity.transform.position, "Immobilized", false, "Red"));
            }
            
        }

        else if (stacks < 0)
        {
            immobilizedStacks += stacks;
            if (immobilizedStacks <= 0)
            {
                immobilized = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyStunned(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Stunned");

        if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                stunnedStacks += stacks;
                if (stunnedStacks > 0)
                {
                    stunned = true;
                    StartCoroutine(VisualEffectManager.Instance.
                    CreateStatusEffect(myLivingEntity.transform.position, "Stunned!", false));
                    StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
                }
            }

        }

        else if (stacks < 0)
        {
            stunnedStacks += stacks;
            if (stunnedStacks <= 0)
            {
                stunned = false;
                StartCoroutine(VisualEffectManager.Instance.
                CreateStatusEffect(myLivingEntity.transform.position, "Stun Removed!", false, "Blue"));
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifySleep(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Sleep");
        if(stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                sleepStacks += stacks;
                if (sleepStacks > 0)
                {
                    sleep = true;
                }
                StartCoroutine(VisualEffectManager.Instance.
                CreateStatusEffect(myLivingEntity.transform.position, "Sleep" + stacks.ToString(), false));
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));

            }
        }
        

        else if (stacks < 0)
        {
            sleepStacks += stacks;
            if (sleepStacks <= 0)
            {
                sleep = false;
                StartCoroutine(VisualEffectManager.Instance.
                CreateStatusEffect(myLivingEntity.transform.position, "Sleep Removed!", false, "Blue"));
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyCamoflage(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Camoflage");

        if (stacks > 0)
        {
            camoflageStacks += stacks;
            if (camoflageStacks > 0)
            {
                camoflage = true;
                StartCoroutine(VisualEffectManager.Instance.
                CreateStatusEffect(myLivingEntity.transform.position, "Camoflage", false, "Blue"));
            }           

        }

        else if (stacks < 0)
        {
            camoflageStacks += stacks;
            if (camoflageStacks <= 0)
            {
                camoflage = false;
                StartCoroutine(VisualEffectManager.Instance.
                CreateStatusEffect(myLivingEntity.transform.position, "Camoflage Removed", false, "Red"));
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyPoisoned(int stacks, LivingEntity applier = null)
    {
        if (applier != null)
        {
            if (applier.myPassiveManager.venomous && stacks > 0)
            {
                stacks++;
            }
        }

        if (poisonImmunity || undead)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Poison Immunity", false, "Blue"));
            return;
        }

        if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                poisonedStacks += stacks;
                if (poisonedStacks > 0)
                {
                    poisoned = true;
                    StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Poisoned + " + stacks.ToString(), false));
                    StartCoroutine(VisualEffectManager.Instance.CreatePoisonAppliedEffect(myLivingEntity.transform.position));
                    myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Poisoned"), stacks);
                }
            }
        }
        else if (stacks < 0)
        {
            poisonedStacks += stacks;
            if (poisonedStacks <= 0)
            {
                poisonedStacks = 0;
                poisoned = false;
            }
        }

    }
    public void ModifyBarrier(int stacks)
    {
        barrierStacks += stacks;
        if (barrierStacks >= 1)
        {
            barrier = true;
            myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Barrier"), stacks);
        }

        else if (barrierStacks <= 0)
        {
            barrier = false;
            myLivingEntity.myStatusManager.RemoveStatusIcon(myLivingEntity.myStatusManager.GetStatusIconByName("Barrier"));
            StartCoroutine(VisualEffectManager.Instance.
                CreateStatusEffect(myLivingEntity.transform.position, "Barrier " + stacks.ToString(), false, "Red"));
        }

        if (stacks > 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Barrier +" + stacks, false, "Blue"));
            StartCoroutine(VisualEffectManager.Instance.
                CreateStatusEffect(myLivingEntity.transform.position, "Barrier +" + stacks.ToString(), false, "Blue"));
        }
    }
    public void ModifyEnrage(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Enrage");
        enrage = true;
        enrageStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyGrowing(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Growing");
        growing = true;
        growingStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyVolatile(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Volatile");
        Volatile = true;
        volatileStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyCautious(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Cautious");
        cautious = true;
        cautiousStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyFlux(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Flux");
        flux = true;
        fluxStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyEncouragingAura(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Encouraging Aura");
        encouragingAura = true;
        encouragingAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyPoisonous(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Poisonous");
        poisonous = true;
        poisonousStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyPreparation(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Preparation");
        preparation = true;
        preparationStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
        StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Preparation", false, "Blue"));
        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
    }
    public void ModifyStealth()
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Stealth");
        stealth = true;        
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifySoulLink()
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Soul Link");
        soulLink = true;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifyUndead()
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Undead");
        undead = true;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifyUnwavering(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Unwavering");
        unwavering = true;
        unwaveringStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifyPoisonImmunity()
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Poison Immunity");
        poisonImmunity = true;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifyThorns(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Thorns");
        thorns = true;
        thornsStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyToxicAura(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Toxic Aura");
        toxicAura = true;
        toxicAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyPhasing(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Phasing");
        phasing = true;
        phasingStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyRegeneration(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Regeneration");
        regeneration = true;
        regenerationStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTenacious(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Tenacious");
        tenacious = true;
        tenaciousStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyHatefulAura(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Hateful Aura");
        hatefulAura = true;
        hatefulAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyFieryAura(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Fiery Aura");
        fieryAura = true;
        fieryAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyGuardianAura(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Guardian Aura");
        guardianAura = true;
        guardianAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifySoulDrainAura(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Soul Drain Aura");
        soulDrainAura = true;
        soulDrainAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }   
    public void ModifyLightningShield(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Lightning Shield");
        lightningShield = true;
        lightningShieldStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyThickOfTheFight(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Thick Of The Fight");
        thickOfTheFight  = true;
        thickOfTheFightStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }

    /*
    public void ModifyTemporaryStrength(int stacks)
    {
        // apply only the strength bonus if protected by rune
        if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity) && stacks > 0)
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Strength");            
            temporaryStrengthStacks += stacks;
            myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
            myLivingEntity.ModifyCurrentStrength(stacks);

            
        }

        // apply strength bonus and temp strength if not protected by rune
        else if (CombatLogic.Instance.IsProtectedByRune(myLivingEntity) && stacks > 0)
        {
            myLivingEntity.ModifyCurrentStrength(stacks);
        }
        else if(stacks < 0)
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Strength");            
            temporaryStrengthStacks += stacks;
            myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
            myLivingEntity.ModifyCurrentStrength(stacks);
        }

        if(temporaryStrengthStacks > 0)
        {
            temporaryStrength = true;
        }

    }
    public void ModifyTemporaryMobility(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Mobility");

        // apply only the mobility bonus if protected by rune
        if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity) && stacks > 0)
        {            
            temporaryMobilityStacks += stacks;
            myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
            myLivingEntity.ModifyCurrentMobility(stacks);
        }

        // apply mobility bonus and temp mobility if not protected by rune
        else if (CombatLogic.Instance.IsProtectedByRune(myLivingEntity) && stacks > 0)
        {
            myLivingEntity.ModifyCurrentMobility(stacks);
        }
        else if (stacks < 0)
        {
            temporaryMobilityStacks += stacks;
            myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
            myLivingEntity.ModifyCurrentMobility(stacks);
        }

        if (temporaryMobilityStacks > 0)
        {
            temporaryMobility = true;
        }

    }
    public void ModifyTemporaryInitiative(int stacks)
    {
        if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity) && stacks > 0)
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Initiative");
            temporaryInitiativeStacks += stacks;
            myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
        }
        else if (stacks < 0)
        {
            StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Initiative");
            temporaryInitiativeStacks += stacks;
            myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
        }

        if (temporaryInitiativeStacks > 0)
        {
            temporaryInitiative = true;
        }

    }
    */
    public void ModifyVulnerable(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Vulnerable");
        if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity) && stacks > 0)
        {
            vulnerableStacks += stacks;
            if (vulnerableStacks > 0)
            {
                vulnerable = true;
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
            }
        }

        else if (stacks < 0)
        {
            vulnerableStacks += stacks;
            if (vulnerableStacks <= 0)
            {
                vulnerable = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyWeakened(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Weakened");

        if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity) && stacks > 0)
        {
            weakenedStacks += stacks;
            if (weakenedStacks > 0)
            {
                weakened = true;
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
            }
        }

        else if (stacks < 0)
        {
            weakenedStacks += stacks;
            if (weakenedStacks <= 0)
            {
                weakened = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }    
    public void ModifyTrueSight(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("True Sight");
        trueSight = true;
        trueSightStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyVenomous(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Venomous");
        venomous = true;
        venomousStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyLifeSteal(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Life Steal");
        lifeSteal = true;
        lifeStealStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyRune(int stacks)
    {
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Rune");
        
        runeStacks += stacks;

        if(stacks > 0)
        {
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Rune + " + runeStacks.ToString(), false, "Blue");
            StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
        }
        else if(stacks < 0)
        {
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Rune " + runeStacks.ToString(), false, "Red");
        }

        if(runeStacks > 0)
        {            
            rune = true;
        }
        else if(runeStacks <= 0)
        {
            rune = false;
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    #endregion

}
