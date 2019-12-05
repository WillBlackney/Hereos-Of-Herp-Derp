using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveManager : MonoBehaviour
{
    // Properties + Component References
    #region

    [Header("UI + Compenent References")]
    public LivingEntity myLivingEntity;

    [Header("Known Passive Traits")]
    public bool enrage;
    public int enrageStacks;

    public bool growing;
    public int growingStacks;

    public bool Volatile;
    public int volatileStacks;

    public bool cautious;
    public int cautiousStacks;

    public bool fleetFooted;
    public int fleetFootedStacks;

    public bool encouragingPresence;
    public int encouragingPresenceStacks;

    public bool poisonous;
    public int poisonousStacks;

    public bool preparation;
    public int preparationStacks;

    public bool stealth;

    public bool unwavering;

    public bool thorns;
    public int thornsStacks;

    public bool unhygienic;
    public int unhygienicStacks;

    public bool quickReflexes;
    public int quickReflexesStacks;

    public bool regeneration;
    public int regenerationStacks;

    public bool adaptive;
    public int adaptiveStacks;

    public bool poisonImmunity;
    public bool soulLink;

    public bool hatefulPresence;
    public int hatefulPresenceStacks;

    public bool fieryPresence;
    public int fieryPresenceStacks;

    public bool guardianPresence;
    public int guardianPresenceStacks;

    public bool soulDrainAura;
    public int soulDrainAuraStacks;

    public bool lightningShield;
    public int lightningShieldStacks;

    public bool masterfulEntrapment;
    public int masterfulEntrapmentStacks;

    public bool thickOfTheFight;
    public int thickOfTheFightStacks;

    public bool temporaryStrength;
    public int temporaryStrengthStacks;

    public bool temporaryInitiative;
    public int temporaryInitiativeStacks;

    public bool temporaryMobility;
    public int temporaryMobilityStacks;

    public bool rune;
    public int runeStacks;

    public bool exposed;
    public int exposedStacks;

    public bool exhausted;
    public int exhaustedStacks;

    public bool magicImmunity;
    public int magicImmunityStacks;

    public bool physicalImmunity;
    public int physicalImmunityStacks;

    public bool trueSight;
    public int trueSightStacks;

    public bool lifeSteal;
    public int lifeStealStacks;

    public bool venomous;
    public int venomousStacks;

    public bool ignite;
    public int igniteStacks;

    public bool pinned;
    public int pinnedStacks;

    public bool stunned;
    public int stunnedStacks;

    public bool sleeping;
    public int sleepingStacks;

    public bool poison;
    public int poisonStacks;

    public bool camoflage;
    public int camoflageStacks;

    public bool barrier;
    public int barrierStacks;

    public bool undead;    
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

    public void ModifyPinned(int stacks, LivingEntity applier = null)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Pinned");

        if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                pinnedStacks += stacks;
                if (pinnedStacks > 0)
                {
                    pinned = true;
                }

                if (applier != null && applier.myPassiveManager.masterfulEntrapmentStacks > 0)
                {
                    applier.ModifyCurrentWisdom(applier.myPassiveManager.masterfulEntrapmentStacks);
                }

                StartCoroutine(VisualEffectManager.Instance.
                    CreateStatusEffect(myLivingEntity.transform.position, "Pinned!", false, "Red"));
            }
            
        }

        else if (stacks < 0)
        {
            pinnedStacks += stacks;
            if (pinnedStacks <= 0)
            {
                pinned = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyStunned(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Stunned");

        if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                stunnedStacks += stacks;
                if (stunnedStacks > 0)
                {
                    stunned = true;
                    StartCoroutine(VisualEffectManager.Instance.
                    CreateStatusEffect(myLivingEntity.transform.position, "Stunned!", false, "Red"));
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
    public void ModifySleeping(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Sleeping");
        if(stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                sleepingStacks += stacks;
                if (sleepingStacks > 0)
                {
                    sleeping = true;
                }
                StartCoroutine(VisualEffectManager.Instance.
                CreateStatusEffect(myLivingEntity.transform.position, "Sleeping + " + stacks.ToString(), false, "Red"));

            }
        }
        

        else if (stacks < 0)
        {
            sleepingStacks += stacks;
            if (sleepingStacks <= 0)
            {
                sleeping = false;
                StartCoroutine(VisualEffectManager.Instance.
                CreateStatusEffect(myLivingEntity.transform.position, "Sleeping Removed!", false, "Blue"));
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyCamoflage(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Camoflage");

        if (stacks > 0)
        {
            camoflageStacks += stacks;
            if (camoflageStacks > 0)
            {
                camoflage = true;
                StartCoroutine(VisualEffectManager.Instance.
                CreateStatusEffect(myLivingEntity.transform.position, "Camoflage!", false, "Blue"));
            }           

        }

        else if (stacks < 0)
        {
            camoflageStacks += stacks;
            if (camoflageStacks <= 0)
            {
                camoflage = false;
                //StartCoroutine(VisualEffectManager.Instance.
                //CreateStatusEffect(myLivingEntity.transform.position, "Camoflage Removed", false, "Red"));
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyPoison(int stacks, LivingEntity applier = null)
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
                poisonStacks += stacks;
                if (poisonStacks > 0)
                {
                    poison = true;
                    StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Poison + " + stacks.ToString(), false, "Green"));
                    myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Poison"), stacks);
                }
            }
        }
        else if (stacks < 0)
        {
            poisonStacks += stacks;
            if (poisonStacks <= 0)
            {
                poison = false;
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
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Enrage");
        enrage = true;
        enrageStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Enrage"), stacks);
    }
    public void ModifyGrowing(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Growing");
        growing = true;
        growingStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Growing"), stacks);
    }
    public void ModifyVolatile(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Volatile");
        Volatile = true;
        volatileStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Volatile"), stacks);
    }
    public void ModifyCautious(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Cautious");
        cautious = true;
        cautiousStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Cautious"), stacks);
    }
    public void ModifyFleetFooted(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Fleet Footed");
        fleetFooted = true;
        fleetFootedStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Fleet Footed"), stacks);
    }
    public void ModifyEncouragingPresence(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Encouraging Presence");
        encouragingPresence = true;
        encouragingPresenceStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Encouraging Presence"), stacks);
    }
    public void ModifyPoisonous(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Poisonous");
        poisonous = true;
        poisonousStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Poisonous"), stacks);
    }
    public void ModifyPreparation(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Preparation");
        preparation = true;
        preparationStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
        StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Preparation", false, "Blue"));
        StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
    }
    public void ModifyStealth()
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Stealth");
        stealth = true;        
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifySoulLink()
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Soul Link");
        soulLink = true;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifyUndead()
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Undead");
        undead = true;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifyUnwavering()
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Unwavering");
        unwavering = true;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifyPoisonImmunity()
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Poison Immunity");
        poisonImmunity = true;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Poison Immunity"), 1);
    }
    public void ModifyThorns(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Thorns");
        thorns = true;
        thornsStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Thorns"), stacks);
    }
    public void ModifyUnhygienic(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Unhygienic");
        unhygienic = true;
        unhygienicStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Unhygienic"), stacks);
    }
    public void ModifyQuickReflexes(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Quick Reflexes");
        quickReflexes = true;
        quickReflexesStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Quick Reflexes"), stacks);
    }
    public void ModifyRegeneration(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Regeneration");
        regeneration = true;
        regenerationStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Regeneration"), stacks);
    }
    public void ModifyAdaptive(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Adaptive");
        adaptive = true;
        adaptiveStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Adaptive"), stacks);
    }
    public void ModifyHatefulPresence(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Hateful Presence");
        hatefulPresence = true;
        hatefulPresenceStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyFieryPresence(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Fiery Presence");
        fieryPresence = true;
        fieryPresenceStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyGuardianPresence(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Guardian Presence");
        guardianPresence = true;
        guardianPresenceStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifySoulDrainAura(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Soul Drain Aura");
        soulDrainAura = true;
        soulDrainAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyMasterfulEntrapment(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Masterful Entrapment");
        masterfulEntrapment = true;
        masterfulEntrapmentStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyLightningShield(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Lightning Shield");
        lightningShield = true;
        lightningShieldStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Lightning Shield"), stacks);
    }
    public void ModifyThickOfTheFight(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Thick Of The Fight");
        thickOfTheFight  = true;
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
            temporaryStrength = true;
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
            temporaryMobility = true;
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
            temporaryInitiative = true;
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
                exposed = true;
            }
        }

        else if (stacks < 0)
        {
            exposedStacks += stacks;
            if (exposedStacks <= 0)
            {
                exposed = false;
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
                exhausted = true;
            }
        }

        else if (stacks < 0)
        {
            exhaustedStacks += stacks;
            if (exhaustedStacks <= 0)
            {
                exhausted = false;
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
                ignite = true;
            }
        }

        else if (stacks < 0)
        {
            igniteStacks += stacks;
            if (igniteStacks <= 0)
            {
                ignite = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyMagicImmunity(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Magic Immunity");
        magicImmunity = true;
        magicImmunityStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Magic Immunity"), stacks);
    }
    public void ModifyPhysicalImmunity(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Physical Immunity");
        physicalImmunity = true;
        physicalImmunityStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Physical Immunity"), stacks);
    }
    public void ModifyTrueSight(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("True Sight");
        trueSight = true;
        trueSightStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyVenomous(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Venomous");
        venomous = true;
        venomousStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyLifeSteal(int stacks)
    {
        StatusIcon iconData = StatusIconLibrary.Instance.GetStatusIconByName("Life Steal");
        lifeSteal = true;
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
