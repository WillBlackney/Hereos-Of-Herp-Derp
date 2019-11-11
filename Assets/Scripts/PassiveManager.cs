using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveManager : MonoBehaviour
{

    [Header("UI + Compenent References")]
    public LivingEntity myLivingEntity;

    [Header("Known Passive Traits")]
    public bool Enrage;
    public int enrageStacks;

    public bool Growing;
    public int growingStacks;

    public bool Volatile;
    public int volatileStacks;

    public bool Cautious;
    public int cautiousStacks;

    public bool FleetFooted;
    public int fleetFootedStacks;

    public bool EncouragingPresence;
    public int encouragingPresenceStacks;

    public bool Poisonous;
    public int poisonousStacks;

    public bool Preparation;
    public int preparationStacks;

    public bool Stealth;

    public bool Unwavering;

    public bool Thorns;
    public int thornsStacks;

    public bool Unhygienic;
    public int unhygienicStacks;

    public bool QuickReflexes;
    public int quickReflexesStacks;

    public bool Regeneration;
    public int regenerationStacks;

    public bool Adaptive;
    public int adaptiveStacks;

    public bool PoisonImmunity;

    public bool HatefulPresence;
    public int hatefulPresenceStacks;

    public bool FieryPresence;
    public int fieryPresenceStacks;

    public bool GuardianPresence;
    public int guardianPresenceStacks;

    public bool SoulDrainAura;
    public int soulDrainAuraStacks;

    public bool LightningShield;
    public int lightningShieldStacks;

    public bool MasterfulEntrapment;
    public int masterfulEntrapmentStacks;

    public bool ThickOfTheFight;
    public int thickOfTheFightStacks;

    public bool TemporaryStrength;
    public int temporaryStrengthStacks;

    public bool TemporaryInitiative;
    public int temporaryInitiativeStacks;

    public bool TemporaryMobility;
    public int temporaryMobilityStacks;

    public bool Rune;
    public int runeStacks;

    public bool Exposed;
    public int exposedStacks;

    public bool Exhausted;
    public int exhaustedStacks;

    public bool MagicImmunity;
    public int magicImmunityStacks;

    public bool PhysicalImmunity;
    public int physicalImmunityStacks;

    public bool Flanked;
    public int flankedStacks;

    public bool TrueSight;
    public int trueSightStacks;

    public bool LifeSteal;
    public int lifeStealStacks;

    public bool Venomous;
    public int venomousStacks;

    public bool Ignite;
    public int igniteStacks;


    public void InitializeSetup()
    {
        myLivingEntity = GetComponent<LivingEntity>();
    }
    public void LearnEnrage(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Enrage");
        Enrage = true;
        enrageStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Enrage"), stacks);
    }

    public void LearnGrowing(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Growing");
        Growing = true;
        growingStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Growing"), stacks);
    }

    public void LearnVolatile(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Volatile");
        Volatile = true;
        volatileStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Volatile"), stacks);
    }

    public void LearnCautious(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Cautious");
        Cautious = true;
        cautiousStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Cautious"), stacks);
    }

    public void LearnFleetFooted(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Fleet Footed");
        FleetFooted = true;
        fleetFootedStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Fleet Footed"), stacks);
    }

    public void LearnEncouragingPresence(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Encouraging Presence");
        EncouragingPresence = true;
        encouragingPresenceStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Encouraging Presence"), stacks);
    }

    public void LearnPoisonous(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Poisonous");
        Poisonous = true;
        poisonousStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Poisonous"), stacks);
    }

    public void LearnPreparation(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Preparation");
        Preparation = true;
        preparationStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Preparation"), stacks);
        StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Preparation", false, "Blue"));
    }

    public void LearnStealth()
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Stealth");
        Stealth = true;        
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Stealth"), 1);
    }
    public void LearnUnwavering()
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Unwavering");
        Unwavering = true;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }

    public void LearnPoisonImmunity()
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Poison Immunity");
        PoisonImmunity = true;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Poison Immunity"), 1);
    }

    public void LearnThorns(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Thorns");
        Thorns = true;
        thornsStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Thorns"), stacks);
    }

    public void LearnUnhygienic(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Unhygienic");
        Unhygienic = true;
        unhygienicStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Unhygienic"), stacks);
    }

    public void LearnQuickReflexes(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Quick Reflexes");
        QuickReflexes = true;
        quickReflexesStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Quick Reflexes"), stacks);
    }

    public void LearnRegeneration(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Regeneration");
        Regeneration = true;
        regenerationStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Regeneration"), stacks);
    }

    public void LearnAdaptive(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Adaptive");
        Adaptive = true;
        adaptiveStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Adaptive"), stacks);
    }

    public void LearnHatefulPresence(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Hateful Presence");
        HatefulPresence = true;
        hatefulPresenceStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void LearnFieryPresence(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Fiery Presence");
        FieryPresence = true;
        fieryPresenceStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void LearnGuardianPresence(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Guardian Presence");
        GuardianPresence = true;
        guardianPresenceStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }

    public void LearnSoulDrainAura(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Soul Drain Aura");
        SoulDrainAura = true;
        soulDrainAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void LearnMasterfulEntrapment(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Masterful Entrapment");
        MasterfulEntrapment = true;
        masterfulEntrapmentStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }

    public void LearnLightningShield(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Lightning Shield");
        LightningShield = true;
        lightningShieldStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Lightning Shield"), stacks);
    }

    public void LearnThickOfTheFight(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Thick Of The Fight");
        ThickOfTheFight  = true;
        thickOfTheFightStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Thick Of The Fight"), stacks);
    }

    public void ModifyTemporaryStrength(int stacks)
    {
        // apply only the strength bonus if protected by rune
        if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity) && stacks > 0)
        {
            StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Strength");            
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
            StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Strength");            
            temporaryStrengthStacks += stacks;
            myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
            myLivingEntity.ModifyCurrentStrength(stacks);
        }

        if(temporaryStrengthStacks > 0)
        {
            TemporaryStrength = true;
        }

    }
    public void ModifyTemporaryMobility(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Mobility");

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
            TemporaryMobility = true;
        }

    }
    public void ModifyTemporaryInitiative(int stacks)
    {
        if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity) && stacks > 0)
        {
            StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Initiative");
            temporaryInitiativeStacks += stacks;
            myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
        }
        else if (stacks < 0)
        {
            StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Initiative");
            temporaryInitiativeStacks += stacks;
            myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
        }

        if (temporaryInitiativeStacks > 0)
        {
            TemporaryInitiative = true;
        }

    }
    public void ModifyExposed(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Exposed");
        if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity) && stacks > 0)
        {
            exposedStacks += stacks;
            if (exposedStacks > 0)
            {
                Exposed = true;
            }
        }

        else if (stacks < 0)
        {
            exposedStacks += stacks;
            if (exposedStacks <= 0)
            {
                Exposed = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyExhausted(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Exhausted");

        if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity) && stacks > 0)
        {
            exhaustedStacks += stacks;
            if (exhaustedStacks > 0)
            {
                Exhausted = true;
            }
        }

        else if (stacks < 0)
        {
            exhaustedStacks += stacks;
            if (exhaustedStacks <= 0)
            {
                Exhausted = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyIgnite(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Ignite");

        if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity) && stacks > 0)
        {
            igniteStacks += stacks;
            if (igniteStacks > 0)
            {
                Ignite = true;
            }
        }

        else if (stacks < 0)
        {
            igniteStacks += stacks;
            if (igniteStacks <= 0)
            {
                Ignite = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void LearnMagicImmunity(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Magic Immunity");
        MagicImmunity = true;
        magicImmunityStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Magic Immunity"), stacks);
    }
    public void LearnPhysicalImmunity(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Physical Immunity");
        PhysicalImmunity = true;
        physicalImmunityStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Physical Immunity"), stacks);
    }
    public void LearnTrueSight(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("True Sight");
        TrueSight = true;
        trueSightStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void LearnVenomous(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Venomous");
        Venomous = true;
        venomousStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void LearnLifeSteal(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Life Steal");
        LifeSteal = true;
        lifeStealStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyRune(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Rune");
        
        runeStacks += stacks;

        if(stacks > 0)
        {
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Rune + " + runeStacks.ToString(), false, "Blue");
        }
        else if(stacks < 0)
        {
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Rune " + runeStacks.ToString(), false, "Red");
        }

        if(runeStacks > 0)
        {            
            Rune = true;
        }
        else if(runeStacks <= 0)
        {
            Rune = false;
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyFlanked(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Flanked");
        
        flankedStacks += stacks;
        if(flankedStacks > 0)
        {
            Flanked = true;
        }
        else
        {
            Flanked = false;
        }
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
}
