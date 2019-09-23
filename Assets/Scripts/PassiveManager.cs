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

    public bool SoulDrainAura;
    public int soulDrainAuraStacks;

    public bool  LightningShield;
    public int lightningShieldStacks;

    public bool ThickOfTheFight;
    public int thickOfTheFightStacks;

    public bool TemporaryStrength;
    public int temporaryStrengthStacks;

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
        StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Preparation", false));
    }

    public void LearnStealth()
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Stealth");
        Stealth = true;        
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Stealth"), 1);
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
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Hateful Presence"), stacks);
    }

    public void LearnSoulDrainAura(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Soul Drain Aura");
        SoulDrainAura = true;
        soulDrainAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Soul Drain Aura"), stacks);
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
        if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity) && stacks > 0)
        {
            StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Strength");            
            temporaryStrengthStacks += stacks;
            myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Temporary Strength"), stacks);
        }
        else if(stacks < 0)
        {
            StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Strength");            
            temporaryStrengthStacks += stacks;
            myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Temporary Strength"), stacks);
        }

        if(temporaryStrengthStacks > 0)
        {
            TemporaryStrength = true;
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

        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Exhausted"), stacks);
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
    public void ModifyRune(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Rune");
        
        runeStacks += stacks;

        if(stacks > 0)
        {
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Rune + " + runeStacks.ToString(), false);
        }
        else if(stacks < 0)
        {
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Rune " + runeStacks.ToString(), false);
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
