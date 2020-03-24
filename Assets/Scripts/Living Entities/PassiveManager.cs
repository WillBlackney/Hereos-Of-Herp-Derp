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

    public bool temporaryBonusParry;
    public int temporaryBonusParryStacks;

    public bool temporaryBonusDodge;
    public int temporaryBonusDodgeStacks;

    public bool temporaryTrueSight;
    public int temporaryTrueSightStacks;

    public bool temporaryHawkEye;
    public int temporaryHawkEyeStacks;

    public bool overwatch;
    public int overwatchStacks;

    public bool timeWarp;
    public int timeWarpStacks;

    [Header("Damage Type Modifier Passives")]
    public bool fireImbuement;
    public int fireImbuementStacks;

    public bool frostImbuement;
    public int frostImbuementStacks;

    public bool poisonImbuement;
    public int poisonImbuementStacks;

    public bool shadowImbuement;
    public int shadowImbuementStacks;

    public bool airImbuement;
    public int airImbuementStacks;

    public bool temporaryFireImbuement;
    public int temporaryFireImbuementStacks;

    public bool temporaryFrostImbuement;
    public int temporaryFrostImbuementStacks;

    public bool temporaryPoisonImbuement;
    public int temporaryPoisonImbuementStacks;

    public bool temporaryShadowImbuement;
    public int temporaryShadowImbuementStacks;

    public bool temporaryAirImbuement;
    public int temporaryAirImbuementStacks;

    [Header("Power Passives")]
    public bool infuse;
    public int infuseStacks;

    public bool purity;
    public int purityStacks;

    public bool recklessness;
    public int recklessnessStacks;

    public bool berserk;
    public int berserkStacks;

    public bool testudo;
    public int testudoStacks;

    public bool rapidCloaking;
    public int rapidCloakingStacks;

    public bool concentration;
    public int concentrationStacks;

    public bool transcendence;
    public int transcendenceStacks;

    public bool invincible;
    public int invincibleStacks;


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
    public int chilledStacks;

    public bool shocked;
    public int shockedStacks;

    public bool sleep;
    public int sleepStacks;

    public bool marked;
    public int markedStacks;

    public bool taunted;
    public int tauntedStacks;

    public bool sharpenedBlade;
    public int sharpenedBladeStacks;

    public bool darkGift;
    public int darkGiftStacks;

    public bool pureHate;
    public int pureHateStacks;

    [Header("Non-Stacking Passives")]   
    public bool unleashed;
    public int unleashedStacks;

    public bool unstoppable;
    public int unstoppableStacks;

    public bool unwavering;
    public int unwaveringStacks;

    public bool infallible;
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

    public bool coupDeGrace;
    public int coupDeGraceStacks;

    public bool flux;
    public int fluxStacks;

    public bool swordPlay;
    public int swordPlayStacks;

    public bool fury;
    public int furyStacks;

    public bool quickDraw;
    public int quickDrawStacks;

    public bool grace;
    public int graceStacks;

    public bool savage;
    public int savageStacks;

    public bool pragmatic;
    public int pragmaticStacks;

    public bool knowledgeable;
    public int knowledgeableStacks;



    public bool lifeSteal;
    public int lifeStealStacks;

    public bool undead;
    public int undeadStacks;

    public bool trueSight;
    public int trueSightStacks;

    public bool perfectAim;
    public int perfectAimStacks;

    public bool virtuoso;
    public int virtuosoStacks;

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

    public bool perfectReflexes;
    public int perfectReflexesStacks;

    public bool masochist;
    public int masochistStacks;

    public bool slippery;
    public int slipperyStacks;

    public bool lastStand;
    public int lastStandStacks;

    public bool riposte;
    public int riposteStacks;    

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

    public bool unstable;
    public int unstableStacks;

    public bool growing;
    public int growingStacks;

    public bool fastLearner;
    public int fastLearnerStacks;

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

    public bool radiance;
    public int radianceStacks;

    public bool fading;
    public int fadingStacks;

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

    public bool poisonedImmunity;

    // Below passives not reworked/updated, will do when needed
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

    // Modify Debuffs
    public void ModifyImmobilized(int stacks)
    {
        Debug.Log(myLivingEntity.name +".PassiveManager.ModifyImmobilized() called, stacks = " + stacks.ToString());
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Immobilized");
        
        if (stacks > 0 && (unleashed || unstoppable))
        {
            stacks = 0;
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Immobilized Immune");
        }

        else if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                immobilizedStacks += stacks;
                if (immobilizedStacks > 0)
                {
                    immobilized = true;
                }
                
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Immobilized");
            }
            
        }

        else if (stacks < 0)
        {
            immobilizedStacks += stacks;
            if (immobilizedStacks <= 0)
            {
                immobilized = false;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Immobilized Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyStunned(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyStunned() called, stacks = " + stacks.ToString());
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Stunned");

        if (unstoppable && stacks > 0)
        {
            stacks = 0;
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Stun Immune!");
        }

        else if (myLivingEntity.defender && StateManager.Instance.DoesPlayerAlreadyHaveState("Determined") && stacks > 0)
        {
            stacks = 0;
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Stun Immune!");
        }

        if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                stunnedStacks += stacks;
                if (stunnedStacks > 0)
                {
                    stunned = true;
                    VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Stunned!");
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
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Stun Removed!");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyBlind(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyBlind() called, stacks = " + stacks.ToString());
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Blind");

        if(stacks > 0 && infallible)
        {
            stacks = 0;
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Blind Immune");
        }
        else if (myLivingEntity.defender && StateManager.Instance.DoesPlayerAlreadyHaveState("Determined") && stacks > 0)
        {
            stacks = 0;
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Blind Immune");
        }
        else if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                blindStacks += stacks;
                if (blindStacks > 0)
                {
                    blind = true;
                    VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Blind");
                    StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
                }
            }

        }

        else if (stacks < 0)
        {
            blindStacks += stacks;
            if (blindStacks <= 0)
            {
                blind = false;
                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Blind Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifySilenced(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifySilenced() called, stacks = " + stacks.ToString());
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Silenced");

        if (stacks > 0 && infallible)
        {
            stacks = 0;
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Silence Immune");
        }
        else if (myLivingEntity.defender && StateManager.Instance.DoesPlayerAlreadyHaveState("Determined") && stacks > 0)
        {
            stacks = 0;
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Silence Immune");
        }

        else if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                silencedStacks += stacks;
                if (silencedStacks > 0)
                {
                    silenced = true;
                    VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Silenced");
                    StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
                }
            }

        }

        else if (stacks < 0)
        {
            silencedStacks += stacks;
            if (silencedStacks <= 0)
            {
                silenced = false;
                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Silenced Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyDisarmed(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyDisarmed() called, stacks = " + stacks.ToString());
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Disarmed");

        if (stacks > 0 && infallible)
        {
            stacks = 0;
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Disarm Immune" );
        }
        else if (myLivingEntity.defender && StateManager.Instance.DoesPlayerAlreadyHaveState("Determined") && stacks > 0)
        {
            stacks = 0;
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Disarm Immune");
        }

        else if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                disarmedStacks += stacks;
                if (disarmedStacks > 0)
                {
                    disarmed = true;
                    VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Disarmed");
                    StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
                }
            }

        }

        else if (stacks < 0)
        {
            disarmedStacks += stacks;
            if (disarmedStacks <= 0)
            {
                disarmed = false;
                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Disarmed Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTerrified(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTerrified() called, stacks = " + stacks.ToString());
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Terrified");

        if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                terrifiedStacks += stacks;
                if (terrifiedStacks > 0)
                {
                    terrified = true;
                    VisualEffectManager.Instance.
                    CreateStatusEffect(myLivingEntity.transform.position, "Terrified");
                    StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
                }
            }

        }

        else if (stacks < 0)
        {
            terrifiedStacks += stacks;
            if (terrifiedStacks <= 0)
            {
                terrified = false;

                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Terrified Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifySleep(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifySleep() called, stacks = " + stacks.ToString());
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Sleep");

        if (unstoppable && stacks > 0)
        {
            stacks = 0;
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Sleep Immune!");
        }
        else if (myLivingEntity.defender && StateManager.Instance.DoesPlayerAlreadyHaveState("Determined") && stacks > 0)
        {
            stacks = 0;
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Sleep Immune!");
        }

        else if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                sleepStacks += stacks;
                if (sleepStacks > 0)
                {
                    sleep = true;
                }
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Sleep");
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));

            }
        }


        else if (stacks < 0)
        {
            sleepStacks += stacks;
            if (sleepStacks <= 0)
            {
                sleep = false;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Sleep Removed!");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyVulnerable(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyVulnerable() called, stacks = " + stacks.ToString());
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Vulnerable");

        if (stacks > 0 && incorruptable)
        {
            stacks = 0;
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Vulnerable Immune");
        }

        else if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity) && stacks > 0)
        {
            vulnerableStacks += stacks;
            if (vulnerableStacks > 0)
            {
                vulnerable = true;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Vulnerable!");
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
            }
        }

        else if (stacks < 0)
        {
            vulnerableStacks += stacks;
            if (vulnerableStacks <= 0)
            {
                vulnerable = false;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Vulnerable Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyWeakened(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyWeakened() called, stacks = " + stacks.ToString());
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Weakened");

        if (stacks > 0 && incorruptable)
        {
            stacks = 0;
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Weakened Immune");
        }

        else if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity) && stacks > 0)
        {
            weakenedStacks += stacks;
            if (weakenedStacks > 0)
            {
                weakened = true;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Weakened!");
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
            }
        }

        else if (stacks < 0)
        {
            weakenedStacks += stacks;
            if (weakenedStacks <= 0)
            {
                weakened = false;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Weakened Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyChilled(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyChilled() called, stacks = " + stacks.ToString());
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Chilled");

        if (myLivingEntity.defender && StateManager.Instance.DoesPlayerAlreadyHaveState("Blessing Of Elements") && stacks > 0)
        {
            stacks = 0;
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Chilled Immune!");
        }

        else if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                chilledStacks += stacks;
                if (chilledStacks > 0)
                {
                    chilled = true;
                    VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Chilled");
                    StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
                }
            }

        }

        else if (stacks < 0)
        {
            chilledStacks += stacks;
            if (chilledStacks <= 0)
            {
                chilledStacks = 0;
                chilled = false;
                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Chilled Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyShocked(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyShocked() called, stacks = " + stacks.ToString());
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Shocked");

        if (myLivingEntity.defender && StateManager.Instance.DoesPlayerAlreadyHaveState("Blessing Of Elements") && stacks > 0)
        {
            stacks = 0;
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Shock Immune!");
        }
        else if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                shockedStacks += stacks;
                if (shockedStacks > 0)
                {
                    shocked = true;
                    VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Shocked!");
                    StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
                }
            }

        }

        else if (stacks < 0)
        {
            shockedStacks += stacks;
            if (shockedStacks <= 0)
            {
                shockedStacks = 0;
                shocked = false;
                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Shocked Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }    
    public void ModifyPoisoned(int stacks, LivingEntity applier = null)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyPoisoned() called, stacks = " + stacks.ToString());

        if (applier != null)
        {
            if (applier.myPassiveManager.venomous && stacks > 0)
            {
                stacks += applier.myPassiveManager.venomousStacks;
            }
        }

        if (myLivingEntity.defender && StateManager.Instance.DoesPlayerAlreadyHaveState("Blessing Of Elements") && stacks > 0)
        {
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Poison Immunity");
            return;
        }

        else if ((poisonedImmunity || undead) && stacks > 0)
        {
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Poison Immunity");
            return;
        }

        else if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                poisonedStacks += stacks;
                if (poisonedStacks > 0)
                {
                    poisoned = true;
                    VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Poisoned + " + stacks.ToString());
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
                myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Poisoned"), stacks);
            }
        }

    }
    public void ModifyBurning(int stacks, LivingEntity applier = null)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyBurning() called, stacks = " + stacks.ToString());

        if (myLivingEntity.defender && StateManager.Instance.DoesPlayerAlreadyHaveState("Blessing Of Elements") && stacks > 0)
        {
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Burning Immunity");
            return;
        }

        if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                burningStacks += stacks;
                if (burningStacks > 0)
                {
                    burning = true;
                    VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Burning + " + stacks.ToString());
                    myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Burning"), stacks);
                }
            }
        }
        else if (stacks < 0)
        {
            burningStacks += stacks;
            if (burningStacks <= 0)
            {
                burningStacks = 0;
                burning = false;
                myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Burning"), stacks);
            }
        }

    }

    public void ModifyMarked(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyMarked() called, stacks = " + stacks.ToString());

        if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                markedStacks += stacks;
                if (markedStacks > 0)
                {
                    marked = true;
                    VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Marked!");
                    myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Marked"), stacks);
                }
            }
        }
        else if (stacks < 0)
        {
            markedStacks += stacks;
            if (markedStacks <= 0)
            {
                markedStacks = 0;
                marked = false;
                myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Marked"), stacks);
            }
        }

    }
    public void ModifyFading(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyFading() called, stacks = " + stacks.ToString());

        if (stacks > 0)
        {
            fadingStacks += stacks;
            if (fadingStacks > 0)
            {
                fading = true;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Fading + " + stacks.ToString());
                StartCoroutine(VisualEffectManager.Instance.CreatePoisonAppliedEffect(myLivingEntity.transform.position));
                myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Fading"), stacks);
            }
        }
        else if (stacks < 0)
        {
            fadingStacks += stacks;
            if (fadingStacks <= 0)
            {
                fadingStacks = 0;
                fading = false;
                myLivingEntity.myStatusManager.StartAddStatusProcess(StatusIconLibrary.Instance.GetStatusIconByName("Fading"), stacks);
            }
        }

    }
    public void ModifyTaunted(int stacks, LivingEntity taunter)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.Modify() called, stacks = " + stacks.ToString());
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Taunted");

        if (stacks > 0)
        {
            tauntedStacks += stacks;
            if (tauntedStacks > 0)
            {
                taunted = true;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Taunted");
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));

                // Set taunter target
                if(taunter != null)
                {
                    myLivingEntity.myTaunter = taunter;
                }
               
            }

        }

        else if (stacks <= 0)
        {
            tauntedStacks += stacks;
            if (tauntedStacks <= 0)
            {
                tauntedStacks = 0;
                taunted = false;
                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Taunt Removed");
            }
            myLivingEntity.myTaunter = null;
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);

    }

    // Modify Core Stats
    public void ModifyBonusStrength(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyBonusStrength() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Strength");

        if (stacks > 0)
        {
            bonusStrengthStacks += stacks;
            if (bonusStrengthStacks > 0)
            {
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Strength + " + stacks.ToString());
                StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(myLivingEntity.transform.position));
                bonusStrength = true;
            }
        }

        else if (stacks < 0)
        {
            StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(myLivingEntity.transform.position));
            bonusStrengthStacks += stacks;
            if (bonusStrengthStacks < 0)
            {
                bonusStrength = true;
            }
        }

        if(bonusStrengthStacks == 0)
        {
            bonusStrength = false;
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyBonusWisdom(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyBonusWisdom() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Wisdom");

        if (stacks > 0)
        {
            bonusWisdomStacks += stacks;
            if (bonusWisdomStacks > 0)
            {
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Wisdom + " + stacks.ToString());
                StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(myLivingEntity.transform.position));
                bonusWisdom = true;
            }
        }

        else if (stacks < 0)
        {
            bonusWisdomStacks += stacks;
            if (bonusWisdomStacks < 0)
            {
                bonusWisdom = true;
            }
        }

        if (bonusWisdomStacks == 0)
        {
            bonusWisdom = false;
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyBonusDexterity(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyBonusDexterity() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Dexterity");

        if (stacks > 0)
        {
            bonusDexterityStacks += stacks;
            if (bonusDexterityStacks > 0)
            {
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Dexterity + " + stacks.ToString());
                StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(myLivingEntity.transform.position));
                bonusDexterity = true;
            }
        }

        else if (stacks < 0)
        {
            bonusDexterityStacks += stacks;
            if (bonusDexterityStacks < 0)
            {
                bonusDexterity = true;
            }
        }

        if (bonusDexterityStacks == 0)
        {
            bonusDexterity = false;
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyBonusStamina(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyBonusStamina() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Stamina");

        if (stacks > 0)
        {
            bonusStaminaStacks += stacks;
            if (bonusStaminaStacks > 0)
            {
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Stamina + " + stacks.ToString());
                StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(myLivingEntity.transform.position));
                bonusStamina = true;
            }
        }

        else if (stacks < 0)
        {
            bonusStaminaStacks += stacks;
            if (bonusStaminaStacks < 0)
            {
                bonusStamina = true;
            }
        }
        if (bonusStaminaStacks == 0)
        {
            bonusStamina = false;
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyBonusInitiative(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyBonusInitiative() called, stacks = " + stacks.ToString());
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Initiative");

        if (stacks > 0)
        {
            bonusInitiativeStacks += stacks;
            if (bonusInitiativeStacks > 0)
            {
                bonusInitiative = true;
            }
        }

        else if (stacks < 0)
        {
            bonusInitiativeStacks += stacks;
            if (bonusInitiativeStacks < 0)
            {
                bonusInitiative = false;
            }
        }
        if (bonusInitiativeStacks == 0)
        {
            bonusInitiative = false;
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyBonusMobility(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyBonusMobility() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Bonus Mobility");

        if (stacks > 0)
        {
            bonusMobilityStacks += stacks;
            if (bonusMobilityStacks != 0)
            {
                bonusMobility = true;
            }
            else
            {
                bonusMobility = false;
            }
        }

        else if (stacks < 0)
        {
            bonusMobilityStacks += stacks;
            if (bonusMobilityStacks != 0)
            {
                bonusMobility = true;
            }
            else
            {
                bonusMobility = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }

    // Modify Temporary Stat Buffs
    public void ModifySharpenedBlade(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifySharpenedBlade() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Sharpened Blade");
        if (stacks > 0)
        {
            sharpenedBladeStacks += stacks;
            if (sharpenedBladeStacks > 0)
            {
                sharpenedBlade = true;
            }
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Sharpen Blade" + stacks.ToString());
            StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));

        }


        else if (stacks < 0)
        {
            sharpenedBladeStacks += stacks;
            if (sharpenedBladeStacks <= 0)
            {
                sharpenedBlade = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }

    public void ModifyDarkGift(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyDarkGift() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Dark Gift");
        if (stacks > 0)
        {
            darkGiftStacks += stacks;
            if (darkGiftStacks > 0)
            {
                darkGift = true;
            }
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Dark Gift" + stacks.ToString());
            StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));

        }


        else if (stacks < 0)
        {
            darkGiftStacks += stacks;
            if (darkGiftStacks <= 0)
            {
                darkGift = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyPureHate(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyPureHate() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Pure Hate");
        if (stacks > 0)
        {
            pureHateStacks += stacks;
            if (pureHateStacks > 0)
            {
                pureHate = true;
            }
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Pure Hate" + stacks.ToString());
            StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));

        }


        else if (stacks < 0)
        {
            pureHateStacks += stacks;
            if (pureHateStacks <= 0)
            {
                pureHate = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyCamoflage(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyCamoflage() called, stacks = " + stacks.ToString());
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Camoflage");

        // Dont apply camoflage to characters with stealth
        if (stealth)
        {
            return;
        }

        if (stacks > 0)
        {
            camoflageStacks += stacks;
            if (camoflageStacks > 0)
            {
                camoflage = true;
                VisualEffectManager.Instance.
                CreateStatusEffect(myLivingEntity.transform.position, "Camoflage");
                StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(myLivingEntity.transform.position));
            }          
        }

        else if (stacks < 0)
        {
            camoflageStacks += stacks;
            if (camoflageStacks <= 0)
            {
                camoflage = false;
                VisualEffectManager.Instance.
                CreateStatusEffect(myLivingEntity.transform.position, "Camoflage Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyPreparation(int stacks)
    {
        Debug.Log(myLivingEntity.myName + ".PassiveManager.ModifyPreparation() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Preparation");

        if (stacks > 0)
        {
            preparationStacks += stacks;
            if (preparationStacks > 0)
            {
                preparation = true;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Preparation");
                StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
            }

        }

        else if (stacks < 0)
        {
            preparationStacks += stacks;
            if (preparationStacks <= 0)
            {
                preparation = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTemporaryTrueSight(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTemporaryTrueSight() called, stacks = " + stacks.ToString());
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary True Sight");

        if (stacks > 0)
        {
            temporaryTrueSightStacks += stacks;
            if (temporaryTrueSightStacks > 0)
            {
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "True Sight!");
                temporaryTrueSight = true;
            }
        }

        else if (stacks < 0)
        {
            temporaryTrueSightStacks += stacks;
            if (temporaryTrueSightStacks <= 0)
            {
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "True Sight Removed");
                temporaryTrueSight = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTemporaryParry(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTemporaryParry() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Parry");

        if (stacks > 0)
        {
            temporaryBonusParryStacks += stacks;
            if (temporaryBonusParryStacks > 0)
            {
                temporaryBonusParry = true;

                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Temporary Parry");
            }
        }

        else if (stacks < 0)
        {
            temporaryBonusParryStacks += stacks;
            if (temporaryBonusParryStacks <= 0)
            {
                temporaryBonusParry = false;

                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Temporary Parry Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTemporaryDodge(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTemporaryDodge() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Dodge");

        if (stacks > 0)
        {
            temporaryBonusDodgeStacks += stacks;
            if (temporaryBonusDodgeStacks > 0)
            {
                temporaryBonusDodge = true;

                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Temporary Dodge");
            }
        }

        else if (stacks < 0)
        {
            temporaryBonusDodgeStacks += stacks;
            if (temporaryBonusDodgeStacks <= 0)
            {
                temporaryBonusDodge = false;

                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Temporary Dodge Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTemporaryWisdom(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTemporaryWisdom() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Wisdom");

        if (stacks > 0)
        {
            temporaryBonusWisdomStacks += stacks;
            if (temporaryBonusWisdomStacks > 0)
            {
                temporaryBonusWisdom = true;

                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Temporary Wisdom");
            }
        }

        else if (stacks < 0)
        {
            temporaryBonusWisdomStacks += stacks;
            if (temporaryBonusWisdomStacks <= 0)
            {
                temporaryBonusWisdom = false;

                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Temporary Wisdom Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTemporaryStrength(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTemporaryStrength() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Strength");

        if (stacks > 0)
        {
            temporaryBonusStrengthStacks += stacks;
            if (temporaryBonusStrengthStacks > 0)
            {
                temporaryBonusStrength = true;
                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Temporary Strength");
            }
        }

        else if (stacks < 0)
        {
            temporaryBonusStrengthStacks += stacks;
            if (temporaryBonusStrengthStacks <= 0)
            {
                temporaryBonusStrength = false;
                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Temporary Strength Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTemporaryMobility(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTemporaryMobility() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Mobility");

        if (stacks > 0)
        {
            temporaryBonusMobilityStacks += stacks;
            if (temporaryBonusMobilityStacks > 0)
            {
                temporaryBonusMobility = true;
                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Temporary Mobility");
            }
        }

        else if (stacks < 0)
        {
            temporaryBonusMobilityStacks += stacks;
            if (temporaryBonusMobilityStacks <= 0)
            {
                temporaryBonusMobility = false;

                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Temporary Mobility Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTemporaryInitiative(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTemporaryInitiative() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Initiative");

        if (stacks > 0)
        {
            temporaryBonusInitiativeStacks += stacks;
            if (temporaryBonusInitiativeStacks > 0)
            {
                temporaryBonusInitiative = true;

                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Temporary Initiative");
            }
        }

        else if (stacks < 0)
        {
            temporaryBonusInitiativeStacks += stacks;
            if (temporaryBonusInitiativeStacks <= 0)
            {
                temporaryBonusInitiative = false;

                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Temporary Initiative Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTemporaryDexterity(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTemporaryDexterity() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Dexterity");

        if (stacks > 0)
        {
            temporaryBonusDexterityStacks += stacks;
            if (temporaryBonusDexterityStacks > 0)
            {
                temporaryBonusDexterity = true;

                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Temporary Dexterity");
            }
        }

        else if (stacks < 0)
        {
            temporaryBonusDexterityStacks += stacks;
            if (temporaryBonusDexterityStacks <= 0)
            {
                temporaryBonusDexterity = false;

                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Temporary Dexterity Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTemporaryStamina(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTemporaryStamina() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Stamina");

        if (stacks > 0)
        {
            temporaryBonusStaminaStacks += stacks;
            if (temporaryBonusStaminaStacks > 0)
            {
                temporaryBonusStamina = true;
                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Temporary Stamina");
            }
        }

        else if (stacks < 0)
        {
            temporaryBonusStaminaStacks += stacks;
            if (temporaryBonusStaminaStacks <= 0)
            {
                temporaryBonusStamina = false;
                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Temporary Stamina Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTemporaryHawkEyeBonus(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTemporaryHawkEyeBonus() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Hawk Eye");

        if (stacks > 0)
        {
            temporaryHawkEyeStacks += stacks;
            if (temporaryHawkEyeStacks > 0)
            {
                temporaryHawkEye = true;
            }
        }

        else if (stacks < 0)
        {
            temporaryHawkEyeStacks += stacks;
            if (temporaryHawkEyeStacks <= 0)
            {
                temporaryHawkEye = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTranscendence(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTranscendence() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Transcendence");

        if (stacks > 0)
        {
            transcendenceStacks += stacks;
            if (transcendenceStacks > 0)
            {
                transcendence = true;
            }
        }

        else if (stacks < 0)
        {
            transcendenceStacks += stacks;
            if (transcendenceStacks <= 0)
            {
                transcendence = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTimeWarp(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTimeWarp() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Time Warp");

        if (stacks > 0)
        {
            timeWarpStacks += stacks;
            if (timeWarpStacks > 0)
            {
                timeWarp = true;
            }
        }

        else if (stacks < 0)
        {
            timeWarpStacks += stacks;
            if (timeWarpStacks <= 0)
            {
                timeWarp = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyOverwatch(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyOverwatch() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Overwatch");

        if (stacks > 0)
        {
            overwatchStacks += stacks;
            if (overwatchStacks > 0)
            {
                overwatch = true;
            }
        }

        else if (stacks < 0)
        {
            overwatchStacks += stacks;
            if (overwatchStacks <= 0)
            {
                overwatch = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }

    // Modify Imbuements
    public void ModifyAirImbuement(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyAirImbuement() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Air Imbuement");

        if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                airImbuementStacks += stacks;
                if (airImbuementStacks > 0)
                {
                    airImbuement = true;
                    VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Air Imbuement");
                    StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
                }
            }

        }

        else if (stacks < 0)
        {
            airImbuementStacks += stacks;
            if (airImbuementStacks <= 0)
            {
                airImbuementStacks = 0;
                airImbuement = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyFireImbuement(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyFireImbuement() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Fire Imbuement");

        if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                fireImbuementStacks += stacks;
                if (fireImbuementStacks > 0)
                {
                    fireImbuement = true;
                    VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Fire Imbuement");
                    StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
                }
            }

        }

        else if (stacks < 0)
        {
            fireImbuementStacks += stacks;
            if (fireImbuementStacks <= 0)
            {
                fireImbuementStacks = 0;
                fireImbuement = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyPoisonImbuement(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyPoisonImbuement() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Poison Imbuement");

        if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                poisonImbuementStacks += stacks;
                if (poisonImbuementStacks > 0)
                {
                    poisonImbuement = true;
                    VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Poison Imbuement");
                    StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
                }
            }

        }

        else if (stacks < 0)
        {
            poisonImbuementStacks += stacks;
            if (poisonImbuementStacks <= 0)
            {
                poisonImbuementStacks = 0;
                poisonImbuement = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyShadowImbuement(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyShadowImbuement() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Shadow Imbuement");

        if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                shadowImbuementStacks += stacks;
                if (shadowImbuementStacks > 0)
                {
                    shadowImbuement = true;
                    VisualEffectManager.Instance.
                    CreateStatusEffect(myLivingEntity.transform.position, "Shadow Imbuement");
                    StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
                }
            }

        }

        else if (stacks < 0)
        {
            shadowImbuementStacks += stacks;
            if (shadowImbuementStacks <= 0)
            {
                shadowImbuementStacks = 0;
                shadowImbuement = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyFrostImbuement(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyFrostImbuement() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Frost Imbuement");

        if (stacks > 0)
        {
            if (!CombatLogic.Instance.IsProtectedByRune(myLivingEntity))
            {
                frostImbuementStacks += stacks;
                if (frostImbuementStacks > 0)
                {
                    frostImbuement = true;
                    VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Frost Imbuement");
                    StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
                }
            }

        }

        else if (stacks < 0)
        {
            frostImbuementStacks += stacks;
            if (frostImbuementStacks <= 0)
            {
                frostImbuementStacks = 0;
                frostImbuement = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }

    // Modify Temporary Imbuements
    public void ModifyTemporaryAirImbuement(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTemporaryAirImbuement() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Air Imbuement");

        if (stacks > 0)
        {
            temporaryAirImbuementStacks += stacks;
            if (temporaryAirImbuementStacks > 0)
            {
                temporaryAirImbuement = true;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Temporary Air Imbuement");
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
            }
        }

        else if (stacks < 0)
        {
            temporaryAirImbuementStacks += stacks;
            if (temporaryAirImbuementStacks <= 0)
            {
                temporaryAirImbuementStacks = 0;
                temporaryAirImbuement = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTemporaryFireImbuement(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTemporaryFireImbuement() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Fire Imbuement");

        if (stacks > 0)
        {
            temporaryFireImbuementStacks += stacks;
            if (temporaryFireImbuementStacks > 0)
            {
                temporaryFireImbuement = true;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Temporary Fire Imbuement");
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
            }
        }

        else if (stacks < 0)
        {
            temporaryFireImbuementStacks += stacks;
            if (temporaryFireImbuementStacks <= 0)
            {
                temporaryFireImbuementStacks = 0;
                temporaryFireImbuement = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTemporaryPoisonImbuement(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTemporaryPoisonImbuement() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Poison Imbuement");

        if (stacks > 0)
        {
            temporaryPoisonImbuementStacks += stacks;
            if (temporaryPoisonImbuementStacks > 0)
            {
                temporaryPoisonImbuement = true;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Temporary Poison Imbuement");
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
            }
        }

        else if (stacks < 0)
        {
            temporaryPoisonImbuementStacks += stacks;
            if (temporaryPoisonImbuementStacks <= 0)
            {
                temporaryPoisonImbuementStacks = 0;
                temporaryPoisonImbuement = false;
                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Poison Imbuement Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTemporaryFrostImbuement(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTemporaryFrostImbuement() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Frost Imbuement");

        if (stacks > 0)
        {
            temporaryFrostImbuementStacks += stacks;
            if (temporaryFrostImbuementStacks > 0)
            {
                temporaryFrostImbuement = true;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Temporary Frost Imbuement");
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
            }
        }

        else if (stacks < 0)
        {
            temporaryFrostImbuementStacks += stacks;
            if (temporaryFrostImbuementStacks <= 0)
            {
                temporaryFrostImbuementStacks = 0;
                temporaryFrostImbuement = false;
                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Frost Imbuement Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTemporaryShadowImbuement(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTemporaryShadowImbuement() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Temporary Shadow Imbuement");

        if (stacks > 0)
        {
            temporaryShadowImbuementStacks += stacks;
            if (temporaryShadowImbuementStacks > 0)
            {
                temporaryShadowImbuement = true;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Temporary Shadow Imbuement");
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
            }
        }

        else if (stacks < 0)
        {
            temporaryShadowImbuementStacks += stacks;
            if (temporaryShadowImbuementStacks <= 0)
            {
                temporaryShadowImbuementStacks = 0;
                temporaryShadowImbuement = false;
                VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Shadow Imbuement Removed");
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }

    // Modify Powers
    public void ModifyPurity(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyPurity() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Purity");

        if (stacks > 0)
        {
            purityStacks += stacks;
            if (purityStacks > 0)
            {
                purity = true;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Purity");
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
            }
        }

        else if (stacks < 0)
        {
            purityStacks += stacks;
            if (purityStacks <= 0)
            {
                purityStacks = 0;
                purity = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyRecklessness(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyRecklessness() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Recklessness");

        if (stacks > 0)
        {
            recklessnessStacks += stacks;
            if (recklessnessStacks > 0)
            {
                recklessness = true;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Recklessness");
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
            }
        }

        else if (stacks < 0)
        {
            recklessnessStacks += stacks;
            if (recklessnessStacks <= 0)
            {
                recklessnessStacks = 0;
                recklessness = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyBerserk(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyBerserk() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Berserk");

        if (stacks > 0)
        {
            berserkStacks += stacks;
            if (berserkStacks > 0)
            {
                berserk = true;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Berserk");
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
            }
        }

        else if (stacks < 0)
        {
            berserkStacks += stacks;
            if (berserkStacks <= 0)
            {
                berserkStacks = 0;
                berserk = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTestudo(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTestudo() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Testudo");

        if (stacks > 0)
        {
            testudoStacks += stacks;
            if (testudoStacks > 0)
            {
                testudo = true;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Testudo");
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
            }
        }

        else if (stacks < 0)
        {
            testudoStacks += stacks;
            if (testudoStacks <= 0)
            {
                testudoStacks = 0;
                testudo = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyConcentration(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyConcentration() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Concentration");

        if (stacks > 0)
        {
            concentrationStacks += stacks;
            if (concentrationStacks > 0)
            {
                concentration = true;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Concentration");
            }
        }

        else if (stacks < 0)
        {
            concentrationStacks += stacks;
            if (concentrationStacks <= 0)
            {
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Concentration Removed");
                concentrationStacks = 0;
                concentration = false;
            }
        }

        if (stacks != 0)
        {
            myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
        }
       
    }
    public void ModifyInfuse(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyInfuse() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Infuse");

        if (stacks > 0)
        {
            infuseStacks += stacks;
            if (infuseStacks > 0)
            {
                infuse = true;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Infuse");
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
            }
        }

        else if (stacks < 0)
        {
            infuseStacks += stacks;
            if (infuseStacks <= 0)
            {
                infuseStacks = 0;
                infuse = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyRapidCloaking(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyRapidCloaking() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Rapid Cloaking");

        if (stacks > 0)
        {
            rapidCloakingStacks += stacks;
            if (rapidCloakingStacks > 0)
            {
                rapidCloaking = true;
                VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Concentration");
                StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
            }
        }

        else if (stacks < 0)
        {
            rapidCloakingStacks += stacks;
            if (rapidCloakingStacks <= 0)
            {
                rapidCloakingStacks = 0;
                rapidCloaking = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }

    // Modify Resistances
    public void ModifyBarrier(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyBarrier() called, stacks = " + stacks.ToString());

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
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Barrier " + stacks.ToString());
        }

        if (stacks > 0)
        {
            VisualEffectManager.Instance.CreateStatusEffect(transform.position, "Barrier +" + stacks);            
        }
    }
    public void ModifyEnrage(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyEnrage() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Enrage");
        enrage = true;
        enrageStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyGrowing(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyGrowing() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Growing");
        growing = true;
        growingStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyFastLearner(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyFastLearner() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Fast Learner");
        fastLearner = true;
        fastLearnerStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyVolatile(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyVolatile() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Volatile");
        Volatile = true;
        volatileStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyUnstable(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyUnstable() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Unstable");
        unstable = true;
        unstableStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyCautious(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyCautious() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Cautious");
        cautious = true;
        cautiousStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyFlux(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyFlux() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Flux");
        flux = true;
        fluxStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyGrace(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyGrace() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Grace");
        grace = true;
        graceStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyFury(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyFury() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Fury");
        fury = true;
        furyStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyQuickDraw(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyQuickDraw() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Quick Draw");
        quickDraw = true;
        quickDrawStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyKnowledgeable(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyKnowledgeable() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Knowledgeable");
        knowledgeable = true;
        knowledgeableStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifySavage(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifySavage() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Savage");
        savage = true;
        savageStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyPragmatic(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyPragmatic() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Pragmatic");
        pragmatic = true;
        pragmaticStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }

    public void ModifySwordPlay(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifySwordPlay() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Sword Play");
        swordPlay = true;
        swordPlayStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyCoupDeGrace(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyCoupDeGrace() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Coup De Grace");
        coupDeGrace = true;
        coupDeGraceStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyEncouragingAura(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyEncouragingAura() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Encouraging Aura");
        encouragingAura = true;
        encouragingAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyStormAura(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyStormAura() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Storm Aura");
        stormAura = true;
        stormAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyPoisonous(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyPoisonous() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Poisonous");
        poisonous = true;
        poisonousStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyImmolation(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyImmolation() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Immolation");
        immolation= true;
        immolationStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }


    // Modify Buff Passives
    public void ModifyStealth(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyStealth() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Stealth");
        stealthStacks += stacks;
        stealth = true;        
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifyPatientStalker(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyPatientStalker() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Patient Stalker");
        patientStalkerStacks += stacks;
        patientStalker = true;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifyPredator(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyPredator() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Predator");
        predatorStacks += stacks;
        predator = true;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifyUnleashed(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyUnleashed() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Unleashed");
        unleashedStacks += stacks;
        unleashed = true;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifyUnstoppable(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyUnstoppable() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Unstoppable");
        unstoppableStacks += stacks;
        unstoppable = true;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifySoulLink()
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifySoulLink() called...");

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Soul Link");
        soulLink = true;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifyUndead()
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyUndead() called...");
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Undead");
        undead = true;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifyNimble(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyNimble() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Nimble");

        if (stacks > 0)
        {
            nimbleStacks += stacks;
            if (nimbleStacks > 0)
            {
                nimble = true;
                //StartCoroutine(VisualEffectManager.Instance.
                //CreateStatusEffect(myLivingEntity.transform.position, "Nimble", false));
                //StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
            }
        }

        else if (stacks < 0)
        {
            nimbleStacks += stacks;
            if (nimbleStacks <= 0)
            {
                nimbleStacks = 0;
                nimble = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyPerfectReflexes(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyPerfectReflexes() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Perfect Reflexes");

        if (stacks > 0)
        {
            perfectReflexesStacks += stacks;
            if (perfectReflexesStacks > 0)
            {
                perfectReflexes = true;
                //StartCoroutine(VisualEffectManager.Instance.
                //CreateStatusEffect(myLivingEntity.transform.position, "Nimble", false));
                //StartCoroutine(VisualEffectManager.Instance.CreateDebuffEffect(transform.position));
            }
        }

        else if (stacks < 0)
        {
            perfectReflexesStacks += stacks;
            if (perfectReflexesStacks <= 0)
            {
                perfectReflexesStacks = 0;
                perfectReflexes = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyMasochist(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyMasochist() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Masochist");

        if (stacks > 0)
        {
            masochistStacks += stacks;
            if (masochistStacks > 0)
            {
                masochist = true;                
            }
        }

        else if (stacks < 0)
        {
            masochistStacks += stacks;
            if (masochistStacks <= 0)
            {
                masochistStacks = 0;
                masochist = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifySlippery(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifySlippery() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Slippery");

        if (stacks > 0)
        {
            slipperyStacks += stacks;
            if (slipperyStacks > 0)
            {
                slippery = true;
            }
        }

        else if (stacks < 0)
        {
            slipperyStacks += stacks;
            if (slipperyStacks <= 0)
            {
                slipperyStacks = 0;
                slippery = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyLastStand(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifySlippery() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Last Stand");

        if (stacks > 0)
        {
            lastStandStacks += stacks;
            if (lastStandStacks > 0)
            {
                lastStand = true;
            }
        }

        else if (stacks < 0)
        {
            lastStandStacks += stacks;
            if (lastStandStacks <= 0)
            {
                lastStandStacks = 0;
                lastStand = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyRiposte(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyRiposte() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Riposte");

        if (stacks > 0)
        {
            riposteStacks += stacks;
            if (riposteStacks > 0)
            {
                riposte = true;
            }
        }

        else if (stacks < 0)
        {
            riposteStacks += stacks;
            if (riposteStacks <= 0)
            {
                riposteStacks = 0;
                riposte = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyVirtuoso(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyVirtuoso() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Riposte");

        if (stacks > 0)
        {
            virtuosoStacks += stacks;
            if (virtuosoStacks > 0)
            {
                virtuoso = true;
            }
        }

        else if (stacks < 0)
        {
            virtuosoStacks += stacks;
            if (virtuosoStacks <= 0)
            {
                virtuosoStacks = 0;
                virtuoso = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyShatter(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyShatter() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Shatter");

        if (stacks > 0)
        {
            shatterStacks += stacks;
            if (shatterStacks > 0)
            {
                shatter = true;
            }
        }

        else if (stacks < 0)
        {
            shatterStacks += stacks;
            if (shatterStacks <= 0)
            {
                shatterStacks = 0;
                shatter = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyEtherealBeing(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyEtherealBeing() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Ethereal Being");

        if (stacks > 0)
        {
            etherealBeingStacks += stacks;
            if (etherealBeingStacks > 0)
            {
                etherealBeing = true;
            }
        }

        else if (stacks < 0)
        {
            etherealBeingStacks += stacks;
            if (etherealBeingStacks <= 0)
            {
                etherealBeingStacks = 0;
                etherealBeing = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyUnwavering(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyUnwavering() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Unwavering");
        unwavering = true;
        unwaveringStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }   
    public void ModifyInfallible(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyUnfallible() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Unfallible");
        infallible = true;
        unfallibleStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifyIncorruptable(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyIncorruptable() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Incorruptable");
        incorruptable = true;
        incorruptableStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifyPoisonImmunity()
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyPoisonImmunity() called, stacks...");
        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Poison Immunity");
        poisonedImmunity = true;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, 1);
    }
    public void ModifyThorns(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyThorns() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Thorns");
        thorns = true;
        thornsStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyOpportunist(int stacks)
    {
        Debug.Log(myLivingEntity.myName + ".PassiveManager.ModifyOpportunist() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Opportunist");
        opportunist = true;
        opportunistStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyToxicAura(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyToxicAura() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Toxic Aura");
        toxicAura = true;
        toxicAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyPhasing(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyPhasing() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Phasing");
        phasing = true;
        phasingStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyRegeneration(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyRegeneration() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Regeneration");
        regeneration = true;
        regenerationStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTenacious(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTenacious() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Tenacious");
        tenacious = true;
        tenaciousStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyHatefulAura(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyHatefulAura() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Hateful Aura");
        hatefulAura = true;
        hatefulAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyFieryAura(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyFieryAura() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Fiery Aura");
        fieryAura = true;
        fieryAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyGuardianAura(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyGuardianAura() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Guardian Aura");
        guardianAura = true;
        guardianAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifySacredAura(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifySacredAura() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Sacred Aura");
        sacredAura = true;
        sacredAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyShadowAura(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyShadowAura() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Shadow Aura");
        shadowAura = true;
        shadowAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifySoulDrainAura(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifySoulDrainAura() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Soul Drain Aura");
        soulDrainAura = true;
        soulDrainAuraStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }   
    public void ModifyLightningShield(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyLightningShield() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Lightning Shield");
        lightningShield = true;
        lightningShieldStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyThickOfTheFight(int stacks)
    {
        
    }
    public void ModifyHawkEye(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyHawkEyeBonus() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Hawk Eye");

        if (stacks > 0)
        {
            hawkEyeStacks += stacks;
            if (hawkEyeStacks > 0)
            {
                hawkEye = true;
            }
        }

        else if (stacks < 0)
        {
            hawkEyeStacks += stacks;
            if (hawkEyeStacks <= 0)
            {
                hawkEye = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyRadiance(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyRadianceBonus() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Radiance");

        if (stacks > 0)
        {
            radianceStacks += stacks;
            if (radianceStacks > 0)
            {
                radiance = true;
            }
        }

        else if (stacks < 0)
        {
            radianceStacks += stacks;
            if (radianceStacks <= 0)
            {
                radiance = false;
            }
        }

        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyTrueSight(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyTrueSight() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("True Sight");
        trueSight = true;
        trueSightStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyPerfectAim(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyPerfectAim() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Perfect Aim");
        perfectAim = true;
        perfectAimStacks += stacks;
        if(stacks > 0)
        {
            myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
        }
        
    }
    public void ModifyVenomous(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyVenomous() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Venomous");
        venomous = true;
        venomousStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyLifeSteal(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyLifeSteal() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Life Steal");
        lifeSteal = true;
        lifeStealStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyRune(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyRune() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Rune");
        
        runeStacks += stacks;

        if(stacks > 0)
        {
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Rune + " + runeStacks.ToString());
            StartCoroutine(VisualEffectManager.Instance.CreateBuffEffect(transform.position));
        }
        else if(stacks < 0)
        {
            VisualEffectManager.Instance.CreateStatusEffect(myLivingEntity.transform.position, "Rune " + runeStacks.ToString());
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

    // Magic Damage "Form" Buffs
    public void ModifyFrozenHeart(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyFrozenHeart() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Frozen Heart");
        frozenHeart= true;
        frozenHeartStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyDemon(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyDemon() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Demon");
        demon = true;
        demonStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyShadowForm(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyShadowForm() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Shadow Form");
        shadowForm = true;
        shadowFormStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyToxicity(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyToxicity() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Toxicity");
        toxicity = true;
        toxicityStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    public void ModifyStormLord(int stacks)
    {
        Debug.Log(myLivingEntity.name + ".PassiveManager.ModifyStormLord() called, stacks = " + stacks.ToString());

        StatusIconDataSO iconData = StatusIconLibrary.Instance.GetStatusIconByName("Storm Lord");
        stormLord = true;
        stormLordStacks += stacks;
        myLivingEntity.myStatusManager.StartAddStatusProcess(iconData, stacks);
    }
    #endregion

}
