using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentController : MonoBehaviour
{
    public static TalentController Instance;
    private void Awake()
    {
        Instance = this;
    }

    public bool IsTalentPurchaseable(CharacterData character, Talent talent)
    {
        Debug.Log("TalentController.IsTalentPurchaseable() called...");

        if (!character.HasEnoughAbilityPoints(1))
        {
            Debug.Log("Unable to purchase talent: Not enough ability points");
            return false;
        }

        else if (!DoesCharacterMeetTalentTierRequirment(character, talent))
        {
            Debug.Log("Unable to purchase " + talent.talentName + ": " + character + " does not meet talent tier requirment");
            return false;
        }

        else if (talent.unlocked)
        {
            Debug.Log("Unable to purchase " + talent.talentName + ": " + character.myName + " has already purchased this talent");
            return false;
        }

        else
        {
            Debug.Log(character.myName + " meets all requirments to purchase " + talent.talentName);
            return true;
        }
    }
    public bool DoesCharacterMeetTalentTierRequirment(CharacterData character, Talent talent)
    {
        Debug.Log("TalentController.DoesCharacterMeetTalentTierRequirment() called...");

        if (talent.talentPool == Talent.TalentPool.Guardian && 
            character.guardianPoints >= talent.talentTier) 
        {
            Debug.Log(character.myName + " meets the talent tier requirments of " + talent.talentName);
            return true;
        }

        else if (talent.talentPool == Talent.TalentPool.Duelist &&
            character.duelistPoints >= talent.talentTier)
        {
            Debug.Log(character.myName + " meets the talent tier requirments of " + talent.talentName);
            return true;
        }

        else if (talent.talentPool == Talent.TalentPool.Brawler &&
            character.brawlerPoints >= talent.talentTier)
        {
            Debug.Log(character.myName + " meets the talent tier requirments of " + talent.talentName);
            return true;
        }

        else if (talent.talentPool == Talent.TalentPool.Assassination &&
            character.assassinationPoints >= talent.talentTier)
        {
            Debug.Log(character.myName + " meets the talent tier requirments of " + talent.talentName);
            return true;
        }
        else if (talent.talentPool == Talent.TalentPool.Pyromania &&
            character.pyromaniaPoints >= talent.talentTier)
        {
            Debug.Log(character.myName + " meets the talent tier requirments of " + talent.talentName);
            return true;
        }
        else if (talent.talentPool == Talent.TalentPool.Cyromancy &&
            character.cyromancyPoints >= talent.talentTier)
        {
            Debug.Log(character.myName + " meets the talent tier requirments of " + talent.talentName);
            return true;
        }
        else if (talent.talentPool == Talent.TalentPool.Ranger &&
            character.rangerPoints >= talent.talentTier)
        {
            Debug.Log(character.myName + " meets the talent tier requirments of " + talent.talentName);
            return true;
        }
        else if (talent.talentPool == Talent.TalentPool.Manipulation &&
            character.manipulationPoints >= talent.talentTier)
        {
            Debug.Log(character.myName + " meets the talent tier requirments of " + talent.talentName);
            return true;
        }
        else if (talent.talentPool == Talent.TalentPool.Divinity &&
            character.divinityPoints >= talent.talentTier)
        {
            Debug.Log(character.myName + " meets the talent tier requirments of " + talent.talentName);
            return true;
        }
        else if (talent.talentPool == Talent.TalentPool.Shadowcraft &&
            character.shadowcraftPoints >= talent.talentTier)
        {
            Debug.Log(character.myName + " meets the talent tier requirments of " + talent.talentName);
            return true;
        }
        else if (talent.talentPool == Talent.TalentPool.Corruption &&
            character.corruptionPoints >= talent.talentTier)
        {
            Debug.Log(character.myName + " meets the talent tier requirments of " + talent.talentName);
            return true;
        }
        else if (talent.talentPool == Talent.TalentPool.Naturalism &&
            character.naturalismPoints >= talent.talentTier)
        {
            Debug.Log(character.myName + " meets the talent tier requirments of " + talent.talentName);
            return true;
        }
        else
        {
            Debug.Log(character.myName + " does NOT meet the talent tier requirments of " + talent.talentName);
            return false;
        }
    }
    public void OnTalentButtonClicked(CharacterData character, Talent talent)
    {
        Debug.Log("TalentController.OnTalentButtonClicked() called...");

        // Try buy talent
        if (IsTalentPurchaseable(character, talent))
        {
            PurchaseTalent(character, talent);
        }
    }
    public void PurchaseTalent(CharacterData character, Talent talent, bool requiresPayemnt = true)
    {
        Debug.Log("TalentController.PurchaseTalent() called...");

        // Unlock talent to prevent further purchase
        talent.unlocked = true;

        // Pay ability points
        if (requiresPayemnt)
        {
            character.ModifyAbilityPoints(-1);
        }
        

        // Apply benefits of the talent
        if (talent.isPassive)
        {
            ApplyTalentPassiveEffectToCharacter(character, talent);
        }
        else if (talent.isAbility)
        {
            ApplyTalentAbilityToCharacter(character, talent);
        }
        
    }
    public void ApplyTalentPassiveEffectToCharacter(CharacterData character, Talent talent)
    {
        Debug.Log("TalentController.ApplyTalentPassiveEffectToCharacter() called...");

        if (talent.talentName == "Tenacious")
        {
            character.ModifyTenacious(talent.passiveStacks);
        }

        else if (talent.talentName == "Masochist")
        {
            character.ModifyMasochist(talent.passiveStacks);
        }

        else if (talent.talentName == "Last Stand")
        {
            character.ModifyLastStand(talent.passiveStacks);
        }

        else if (talent.talentName == "Slippery")
        {
            character.ModifySlippery(talent.passiveStacks);
        }

        else if (talent.talentName == "Riposte")
        {
            character.ModifyRiposte(talent.passiveStacks);
        }

        else if (talent.talentName == "Perfect Reflexes")
        {
            character.ModifyPerfectReflexes(talent.passiveStacks);
        }

        else if (talent.talentName == "Opportunist")
        {
            character.ModifyOpportunist(talent.passiveStacks);
        }

        else if (talent.talentName == "Patient Stalker")
        {
            character.ModifyPatientStalker(talent.passiveStacks);
        }

        else if (talent.talentName == "Stealth")
        {
            character.ModifyStealth(talent.passiveStacks);
        }

        else if (talent.talentName == "Cautious")
        {
            character.ModifyCautious(talent.passiveStacks);
        }

        else if (talent.talentName == "Guardian Aura")
        {
            character.ModifyGuardianAura(talent.passiveStacks);
        }

        else if (talent.talentName == "Unwavering")
        {
            character.ModifyUnwavering(talent.passiveStacks);
        }

        else if (talent.talentName == "Fiery Aura")
        {
            character.ModifyFieryAura(talent.passiveStacks);
        }

        else if (talent.talentName == "Immolation")
        {
            character.ModifyImmolation(talent.passiveStacks);
        }

        else if (talent.talentName == "Demon")
        {
            character.ModifyDemon(talent.passiveStacks);
        }

        else if (talent.talentName == "Shatter")
        {
            character.ModifyShatter(talent.passiveStacks);
        }

        else if (talent.talentName == "Frozen Heart")
        {
            character.ModifyFrozenHeart(talent.passiveStacks);
        }

        else if (talent.talentName == "Predator")
        {
            character.ModifyPredator(talent.passiveStacks);
        }

        else if (talent.talentName == "Hawk Eye")
        {
            character.ModifyHawkEye(talent.passiveStacks);
        }

        else if (talent.talentName == "Flux")
        {
            character.ModifyFlux(talent.passiveStacks);
        }

        else if (talent.talentName == "Phasing")
        {
            character.ModifyPhasing(talent.passiveStacks);
        }

        else if (talent.talentName == "Ethereal Being")
        {
            character.ModifyEtherealBeing(talent.passiveStacks);
        }

        else if (talent.talentName == "Encouraging Aura")
        {
            character.ModifyEncouragingAura(talent.passiveStacks);
        }

        else if (talent.talentName == "Radiance")
        {
            character.ModifyRadiance(talent.passiveStacks);
        }

        else if (talent.talentName == "Sacred Aura")
        {
            character.ModifySacredAura(talent.passiveStacks);
        }

        else if (talent.talentName == "Shadow Aura")
        {
            character.ModifyShadowAura(talent.passiveStacks);
        }

        else if (talent.talentName == "Shadow Form")
        {
            character.ModifyShadowForm(talent.passiveStacks);
        }

        else if (talent.talentName == "Poisonous")
        {
            character.ModifyPoisonous(talent.passiveStacks);
        }

        else if (talent.talentName == "Venomous")
        {
            character.ModifyVenomous(talent.passiveStacks);
        }

        else if (talent.talentName == "Toxicity")
        {
            character.ModifyToxicity(talent.passiveStacks);
        }

        else if (talent.talentName == "Toxic Aura")
        {
            character.ModifyToxicAura(talent.passiveStacks);
        }

        else if (talent.talentName == "Storm Aura")
        {
            character.ModifyStormAura(talent.passiveStacks);
        }

        else if (talent.talentName == "Storm Lord")
        {
            character.ModifyStormLord(talent.passiveStacks);
        }
       
    }    
    public void ApplyTalentAbilityToCharacter(CharacterData character, Talent talent)
    {
        Debug.Log("TalentController.ApplyTalentAbilityToCharacter() called...");

        if (talent.talentName == "Provoke")
        {
            character.KnowsProvoke = true;
        }

        else if (talent.talentName == "Guard")
        {
            character.KnowsGuard = true;
        }
        else if (talent.talentName == "Sword And Board")
        {
            character.KnowsSwordAndBoard = true;
        }
        else if (talent.talentName == "Get Down!")
        {
            character.KnowsGetDown = true;
        }
        else if (talent.talentName == "Shield Slam")
        {
            character.KnowsShieldSlam = true;
        }
        else if (talent.talentName == "Testudo")
        {
            character.KnowsTestudo = true;
        }
        else if (talent.talentName == "Reactive Armour")
        {
            character.KnowsReactiveArmour = true;
        }
        else if (talent.talentName == "Challenging Shout")
        {
            character.KnowsChallengingShout = true;
        }
        else if (talent.talentName == "Fire Ball")
        {
            character.KnowsFireBall = true;
        }
        else if (talent.talentName == "Fire Nova")
        {
            character.KnowsFireNova = true;
        }
        else if (talent.talentName == "Phoenix Dive")
        {
            character.KnowsPhoenixDive = true;
        }
        else if (talent.talentName == "Meteor")
        {
            character.KnowsMeteor = true;
        }
        else if (talent.talentName == "Blaze")
        {
            character.KnowsBlaze = true;
        }
        else if (talent.talentName == "Combustion")
        {
            character.KnowsCombustion = true;
        }
        else if (talent.talentName == "Dragon Breath")
        {
            character.KnowsDragonBreath = true;
        }
        else if (talent.talentName == "Frost Nova")
        {
            character.KnowsFrostNova = true;
        }
        else if (talent.talentName == "Chilling Blow")
        {
            character.KnowsChillingBlow = true;
        }
        else if (talent.talentName == "Icy Focus")
        {
            character.KnowsIcyFocus = true;
        }
        else if (talent.talentName == "Blizzard")
        {
            character.KnowsBlizzard = true;
        }
        else if (talent.talentName == "Frost Armour")
        {
            character.KnowsFrostArmour = true;
        }
        else if (talent.talentName == "Frost Bolt")
        {
            character.KnowsFrostBolt = true;
        }
        else if (talent.talentName == "Creeping Frost")
        {
            character.KnowsCreepingFrost = true;
        }
        else if (talent.talentName == "Thaw")
        {
            character.KnowsThaw = true;
        }
        else if (talent.talentName == "Glacial Burst")
        {
            character.KnowsGlacialBurst = true;
        }
        else if (talent.talentName == "Snipe")
        {
            character.KnowsSnipe = true;
        }
        else if (talent.talentName == "Haste")
        {
            character.KnowsHaste = true;
        }
        else if (talent.talentName == "Impaling Bolt")
        {
            character.KnowsImpalingBolt = true;
        }
        else if (talent.talentName == "Steady Hands")
        {
            character.KnowsSteadyHands = true;
        }
        else if (talent.talentName == "Forest Medicine")
        {
            character.KnowsForestMedicine = true;
        }
        else if (talent.talentName == "Tree Leap")
        {
            character.KnowsTreeLeap = true;
        }
        else if (talent.talentName == "Concentration")
        {
            character.KnowsConcentration = true;
        }
        else if (talent.talentName == "Rapid Fire")
        {
            character.KnowsRapidFire = true;
        }
        else if (talent.talentName == "Overwatch")
        {
            character.KnowsOverwatch = true;
        }
        else if (talent.talentName == "Telekinesis")
        {
            character.KnowsTelekinesis = true;
        }
        else if (talent.talentName == "Dimensional Blast")
        {
            character.KnowsDimensionalBlast = true;
        }
        else if (talent.talentName == "Mirage")
        {
            character.KnowsMirage = true;
        }
        else if (talent.talentName == "Phase Shift")
        {
            character.KnowsPhaseShift = true;
        }
        else if (talent.talentName == "Burst Of Knowledge")
        {
            character.KnowsBurstOfKnowledge = true;
        }
        else if (talent.talentName == "Blink")
        {
            character.KnowsBlink = true;
        }
        else if (talent.talentName == "Infuse")
        {
            character.KnowsInfuse = true;
        }
        else if (talent.talentName == "Time Warp")
        {
            character.KnowsTimeWarp = true;
        }
        else if (talent.talentName == "Dimensional Hex")
        {
            character.KnowsDimensionalHex = true;
        }
        else if (talent.talentName == "Holy Fire")
        {
            character.KnowsHolyFire = true;
        }
        else if (talent.talentName == "Inspire")
        {
            character.KnowsInspire = true;
        }
        else if (talent.talentName == "Consecrate")
        {
            character.KnowsConsecrate = true;
        }
        else if (talent.talentName == "Invigorate")
        {
            character.KnowsInvigorate = true;
        }
        else if (talent.talentName == "Bless")
        {
            character.KnowsBless = true;
        }
        else if (talent.talentName == "Blinding Light")
        {
            character.KnowsBlindingLight = true;
        }
        else if (talent.talentName == "Purity")
        {
            character.KnowsPurity = true;
        }
        else if (talent.talentName == "Transcendence")
        {
            character.KnowsTranscendence = true;
        }
        else if (talent.talentName == "Judgement")
        {
            character.KnowsJudgement = true;
        }
        else if (talent.talentName == "Shadow Blast")
        {
            character.KnowsShadowBlast = true;
        }
        else if (talent.talentName == "Shroud")
        {
            character.KnowsShroud = true;
        }
        else if (talent.talentName == "Hex")
        {
            character.KnowsHex = true;
        }
        else if (talent.talentName == "Chaos Bolt")
        {
            character.KnowsChaosBolt = true;
        }
        else if (talent.talentName == "Nightmare")
        {
            character.KnowsNightmare = true;
        }
        else if (talent.talentName == "Rain Of Chaos")
        {
            character.KnowsRainOfChaos = true;
        }
        else if (talent.talentName == "Shadow Wreath")
        {
            character.KnowsShadowWreath = true;
        }
        else if (talent.talentName == "Unbridled Chaos")
        {
            character.KnowsUnbridledChaos = true;
        }
        else if (talent.talentName == "Blight")
        {
            character.KnowsBlight = true;
        }
        else if (talent.talentName == "Blood Offering")
        {
            character.KnowsBloodOffering = true;
        }
        else if (talent.talentName == "Toxic Slash")
        {
            character.KnowsToxicSlash = true;
        }
        else if (talent.talentName == "Noxious Fumes")
        {
            character.KnowsNoxiousFumes = true;
        }
        else if (talent.talentName == "Toxic Eruption")
        {
            character.KnowsToxicEruption = true;
        }
        else if (talent.talentName == "Chemical Reaction")
        {
            character.KnowsChemicalReaction = true;
        }
        else if (talent.talentName == "Drain")
        {
            character.KnowsDrain = true;
        }
        else if (talent.talentName == "Spirit Surge")
        {
            character.KnowsSpiritSurge = true;
        }
        else if (talent.talentName == "Lightning Bolt")
        {
            character.KnowsLightningBolt = true;
        }
        else if (talent.talentName == "Thunder Strike")
        {
            character.KnowsThunderStrike = true;
        }
        else if (talent.talentName == "Spirit Vision")
        {
            character.KnowsSpiritVision = true;
        }
        else if (talent.talentName == "Primal Rage")
        {
            character.KnowsPrimalRage = true;
        }
        else if (talent.talentName == "Chain Lightning")
        {
            character.KnowsChainLightning = true;
        }
        else if (talent.talentName == "Thunder Storm")
        {
            character.KnowsThunderStorm = true;
        }
        else if (talent.talentName == "Overload")
        {
            character.KnowsOverload = true;
        }
        else if (talent.talentName == "Concealing Clouds")
        {
            character.KnowsConcealingClouds = true;
        }
        else if (talent.talentName == "Super Conductor")
        {
            character.KnowsSuperConductor = true;
        }


    }
    public Talent GetTalentByName(CharacterData character, string talentName)
    {
        Debug.Log("TalentController.GetTalentByName() called, searching for " + talentName);
        Talent talentReturned = null;

        foreach(Talent talent in character.allTalentButtons)
        {
            if(talent.name == talentName)
            {
                talentReturned = talent;
                break;
            }
        }

        if(talentReturned == null)
        {
            Debug.Log("TalentController.GetTalentByName() could not find a talent with the name " + talentName + ", returning null");
        }

        return talentReturned;

    }
    
}
