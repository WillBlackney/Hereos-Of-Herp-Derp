using System.Collections.Generic;
using UnityEngine;

public class StatusIconLibrary : MonoBehaviour
{
    public static StatusIconLibrary Instance;

    public List<StatusIcon> allIcons = new List<StatusIcon>();
    private void Awake()
    {
        Instance = this;
        PopulateStatusIconLibrary();
    }

    public void PopulateStatusIconLibrary()
    {
        CreateKnockdownIconData();
        CreateStrengthIconData();
        CreateDexterityIconData();
        CreateStunnedIconData();
        CreatePinnedIconData();
        CreateBarrierIconData();
        CreateEnrageIconData();
        CreateGrowingIconData();
        CreateVolatileIconData();
        CreateSleepingIconData();
        CreateCamoflageIconData();
        CreatePoisonIconData();
        CreateCautiousIconData();
        CreateFleetFootedIconData();
        CreateEncouragingPresenceIconData();
        CreatePoisonousIconData();
        CreatePreparationIconData();
        CreateStealthIconData();
        CreateThornsIconData();
        CreateUnhygienicIconData();
        CreateQuickReflexesIconData();
        CreateRegenerationIconData();
        CreateAdaptiveIconData();
        CreatePoisonImmunityIconData();
        CreateHatefulPresenceIconData();
        CreateSoulDrainAuraIconData();
        CreateLightningShieldIconData();
        CreateThickOfTheFightIconData();
        CreateTemporaryStrengthIconData();
        CreateRuneIconData();
        CreateExhaustedIconData();
        CreateExposedIconData();
        CreateMagicImmunityIconData();
        CreatePhysicalImmunityIconData();
        CreateFlankedIconData();
    }

    public StatusIcon GetStatusIconByName(string name)
    {
        StatusIcon iconReturned = null;

        foreach (StatusIcon icon in allIcons)
        {
            if (icon.statusName == name)
            {
                iconReturned = icon;
            }
        }

        return iconReturned;
    }

    public void CreateKnockdownIconData()
    {
        StatusIcon knockDown = gameObject.AddComponent<StatusIcon>();
        knockDown.statusImage = knockDownImage;
        knockDown.statusName = knockDownStatusName;
        knockDown.statusDescription = knockDownStatusDescription;
        knockDown.statusStacks = knockDownStatusStacks;
        allIcons.Add(knockDown);
    }

    public void CreateStrengthIconData()
    {
        StatusIcon strength = gameObject.AddComponent<StatusIcon>();
        strength.statusImage = strengthImage;
        strength.statusName = strengthStatusName;
        strength.statusDescription = strengthStatusDescription;
        strength.statusStacks= strengthStatusStacks;
        allIcons.Add(strength);
    }

    public void CreateDexterityIconData()
    {
        StatusIcon dexterity = gameObject.AddComponent<StatusIcon>();
        dexterity.statusImage = dexterityImage;
        dexterity.statusName = dexterityStatusName;
        dexterity.statusDescription = dexterityStatusDescription;
        dexterity.statusStacks = dexterityStatusStacks;
        allIcons.Add(dexterity);
    }

    public void CreateStunnedIconData()
    {
        StatusIcon stunned = gameObject.AddComponent<StatusIcon>();
        stunned.statusImage = stunnedImage;
        stunned.statusName = stunnedStatusName;
        stunned.statusDescription = stunnedStatusDescription;
        stunned.statusStacks = stunnedStatusStacks;
        allIcons.Add(stunned);
    }

    public void CreatePinnedIconData()
    {
        StatusIcon pinned = gameObject.AddComponent<StatusIcon>();
        pinned.statusImage = pinnedImage;
        pinned.statusName = pinnedStatusName;
        pinned.statusDescription = pinnedStatusDescription;
        pinned.statusStacks = pinnedStatusStacks;
        allIcons.Add(pinned);
    }

    public void CreateBarrierIconData()
    {
        StatusIcon barrier = gameObject.AddComponent<StatusIcon>();
        barrier.statusImage = barrierImage;
        barrier.statusName = barrierStatusName;
        barrier.statusDescription = barrierStatusDescription;
        barrier.statusStacks = barrierStatusStacks;
        allIcons.Add(barrier);
    }

    public void CreateEnrageIconData()
    {
        StatusIcon enrage = gameObject.AddComponent<StatusIcon>();
        enrage.statusImage = enrageImage;
        enrage.statusName = enrageStatusName;
        enrage.statusDescription = enrageStatusDescription;
        enrage.statusStacks = enrageStatusStacks;
        allIcons.Add(enrage);
    }

    public void CreateGrowingIconData()
    {
        StatusIcon growing = gameObject.AddComponent<StatusIcon>();
        growing.statusImage = growingImage;
        growing.statusName = growingStatusName;
        growing.statusDescription = growingStatusDescription;
        growing.statusStacks = growingStatusStacks;
        allIcons.Add(growing);
    }
    public void CreateVolatileIconData()
    {
        StatusIcon @volatile= gameObject.AddComponent<StatusIcon>();
        @volatile.statusImage = volatileImage;
        @volatile.statusName = volatileStatusName;
        @volatile.statusDescription = volatileStatusDescription;
        @volatile.statusStacks = volatileStatusStacks;
        allIcons.Add(@volatile);
    }

    public void CreateSleepingIconData()
    {
        StatusIcon sleeping = gameObject.AddComponent<StatusIcon>();
        sleeping.statusImage = sleepingImage;
        sleeping.statusName = sleepingStatusName;
        sleeping.statusDescription = sleepingStatusDescription;
        sleeping.statusStacks = sleepingStatusStacks;
        allIcons.Add(sleeping);
    }

    public void CreateCamoflageIconData()
    {
        StatusIcon camoflage = gameObject.AddComponent<StatusIcon>();
        camoflage.statusImage = camoflageImage;
        camoflage.statusName = camoflageStatusName;
        camoflage.statusDescription = camoflageStatusDescription;
        camoflage.statusStacks = camoflageStatusStacks;
        allIcons.Add(camoflage);
    }

    public void CreatePoisonIconData()
    {
        StatusIcon poison = gameObject.AddComponent<StatusIcon>();
        poison.statusImage = poisonImage;
        poison.statusName = poisonStatusName;
        poison.statusDescription = poisonStatusDescription;
        poison.statusStacks = poisonStatusStacks;
        allIcons.Add(poison);
    }

    public void CreateCautiousIconData()
    {
        StatusIcon cautious = gameObject.AddComponent<StatusIcon>();
        cautious.statusImage = cautiousImage;
        cautious.statusName = cautiousStatusName;
        cautious.statusDescription = cautiousStatusDescription;
        cautious.statusStacks = cautiousStatusStacks;
        allIcons.Add(cautious);
    }

    public void CreateFleetFootedIconData()
    {
        StatusIcon fleetFooted = gameObject.AddComponent<StatusIcon>();
        fleetFooted.statusImage = fleetFootedImage;
        fleetFooted.statusName = fleetFootedStatusName;
        fleetFooted.statusDescription = fleetFootedStatusDescription;
        fleetFooted.statusStacks = fleetFootedStatusStacks;
        allIcons.Add(fleetFooted);
    }

    public void CreateEncouragingPresenceIconData()
    {
        StatusIcon encouragingPresence = gameObject.AddComponent<StatusIcon>();
        encouragingPresence.statusImage = encouragingPresenceImage;
        encouragingPresence.statusName = encouragingPresenceStatusName;
        encouragingPresence.statusDescription = encouragingPresenceStatusDescription;
        encouragingPresence.statusStacks = encouragingPresenceStatusStacks;
        allIcons.Add(encouragingPresence);
    }

    public void CreatePoisonousIconData()
    {
        StatusIcon poisonous = gameObject.AddComponent<StatusIcon>();
        poisonous.statusImage = poisonousImage;
        poisonous.statusName = poisonousStatusName;
        poisonous.statusDescription = poisonousStatusDescription;
        poisonous.statusStacks = poisonousStatusStacks;
        allIcons.Add(poisonous);
    }
    public void CreatePreparationIconData()
    {
        StatusIcon preparation = gameObject.AddComponent<StatusIcon>();
        preparation.statusImage = preparationImage;
        preparation.statusName = preparationStatusName;
        preparation.statusDescription = preparationStatusDescription;
        preparation.statusStacks = preparationStatusStacks;
        allIcons.Add(preparation);
    }

    public void CreateStealthIconData()
    {
        StatusIcon stealth = gameObject.AddComponent<StatusIcon>();
        stealth.statusImage = stealthImage;
        stealth.statusName = stealthStatusName;
        stealth.statusDescription = stealthStatusDescription;
        stealth.statusStacks = stealthStatusStacks;
        allIcons.Add(stealth);
    }

    public void CreateThornsIconData()
    {
        StatusIcon thorns = gameObject.AddComponent<StatusIcon>();
        thorns.statusImage = thornsImage;
        thorns.statusName = thornsStatusName;
        thorns.statusDescription = thornsStatusDescription;
        thorns.statusStacks = thornsStatusStacks;
        allIcons.Add(thorns);
    }

    public void CreateUnhygienicIconData()
    {
        StatusIcon unhygienic = gameObject.AddComponent<StatusIcon>();
        unhygienic.statusImage = unhygienicImage;
        unhygienic.statusName = unhygienicStatusName;
        unhygienic.statusDescription = unhygienicStatusDescription;
        unhygienic.statusStacks = unhygienicStatusStacks;
        allIcons.Add(unhygienic);
    }

    public void CreateQuickReflexesIconData()
    {
        StatusIcon quickReflexes = gameObject.AddComponent<StatusIcon>();
        quickReflexes.statusImage = quickReflexesImage;
        quickReflexes.statusName = quickReflexesStatusName;
        quickReflexes.statusDescription = quickReflexesStatusDescription;
        quickReflexes.statusStacks = quickReflexesStatusStacks;
        allIcons.Add(quickReflexes);
    }

    public void CreateRegenerationIconData()
    {
        StatusIcon regeneration = gameObject.AddComponent<StatusIcon>();
        regeneration.statusImage = regenerationImage;
        regeneration.statusName = regenerationStatusName;
        regeneration.statusDescription = regenerationStatusDescription;
        regeneration.statusStacks = regenerationStatusStacks;
        allIcons.Add(regeneration);
    }
    public void CreateAdaptiveIconData()
    {
        StatusIcon adaptive = gameObject.AddComponent<StatusIcon>();
        adaptive.statusImage = adaptiveImage;
        adaptive.statusName = adaptiveStatusName;
        adaptive.statusDescription = adaptiveStatusDescription;
        adaptive.statusStacks = adaptiveStatusStacks;
        allIcons.Add(adaptive);
    }

    public void CreatePoisonImmunityIconData()
    {
        StatusIcon poisonImmunity = gameObject.AddComponent<StatusIcon>();
        poisonImmunity.statusImage = poisonImmunityImage;
        poisonImmunity.statusName = poisonImmunityStatusName;
        poisonImmunity.statusDescription = poisonImmunityStatusDescription;
        poisonImmunity.statusStacks = poisonImmunityStatusStacks;
        allIcons.Add(poisonImmunity);
    }

    public void CreateHatefulPresenceIconData()
    {
        StatusIcon hatefulPresence = gameObject.AddComponent<StatusIcon>();
        hatefulPresence.statusImage = hatefulPresenceImage;
        hatefulPresence.statusName = hatefulPresenceStatusName;
        hatefulPresence.statusDescription = hatefulPresenceStatusDescription;
        hatefulPresence.statusStacks = hatefulPresenceStatusStacks;
        allIcons.Add(hatefulPresence);
    }

    public void CreateSoulDrainAuraIconData()
    {
        StatusIcon soulDrainAura = gameObject.AddComponent<StatusIcon>();
        soulDrainAura.statusImage = soulDrainAuraImage;
        soulDrainAura.statusName = soulDrainAuraStatusName;
        soulDrainAura.statusDescription = soulDrainAuraStatusDescription;
        soulDrainAura.statusStacks = soulDrainAuraStatusStacks;
        allIcons.Add(soulDrainAura);
    }
    public void CreateLightningShieldIconData()
    {
        StatusIcon lightningSHield = gameObject.AddComponent<StatusIcon>();
        lightningSHield.statusImage = lightningShieldImage;
        lightningSHield.statusName = lightningShieldStatusName;
        lightningSHield.statusDescription = lightningShieldStatusDescription;
        lightningSHield.statusStacks = lightningShieldStatusStacks;
        allIcons.Add(lightningSHield);
    }

    public void CreateThickOfTheFightIconData()
    {
        StatusIcon thickOfTheFight = gameObject.AddComponent<StatusIcon>();
        thickOfTheFight.statusImage = thickOfTheFightImage;
        thickOfTheFight.statusName = thickOfTheFightStatusName;
        thickOfTheFight.statusDescription = thickOfTheFightStatusDescription;
        thickOfTheFight.statusStacks = thickOfTheFightStatusStacks;
        allIcons.Add(thickOfTheFight);
    }

    public void CreateTemporaryStrengthIconData()
    {
        StatusIcon temporaryStrength = gameObject.AddComponent<StatusIcon>();
        temporaryStrength.statusImage = temporaryStrengthImage;
        temporaryStrength.statusName = temporaryStrengthStatusName;
        temporaryStrength.statusDescription = temporaryStrengthStatusDescription;
        temporaryStrength.statusStacks = temporaryStrengthStatusStacks;
        allIcons.Add(temporaryStrength);
    }
    public void CreateRuneIconData()
    {
        StatusIcon rune = gameObject.AddComponent<StatusIcon>();
        rune.statusImage = runeImage;
        rune.statusName = runeStatusName;
        rune.statusDescription = runeStatusDescription;
        rune.statusStacks = runeStatusStacks;
        allIcons.Add(rune);
    }

    public void CreateExposedIconData()
    {
        StatusIcon exposed = gameObject.AddComponent<StatusIcon>();
        exposed.statusImage = exposedImage;
        exposed.statusName = exposedStatusName;
        exposed.statusDescription = exposedStatusDescription;
        exposed.statusStacks = exposedStatusStacks;
        allIcons.Add(exposed);
    }

    public void CreateExhaustedIconData()
    {
        StatusIcon exhausted = gameObject.AddComponent<StatusIcon>();
        exhausted.statusImage = exhaustedImage;
        exhausted.statusName = exhaustedStatusName;
        exhausted.statusDescription = exhaustedStatusDescription;
        exhausted.statusStacks = exhaustedStatusStacks;
        allIcons.Add(exhausted);
    }
    public void CreateMagicImmunityIconData()
    {
        StatusIcon magicImmunity = gameObject.AddComponent<StatusIcon>();
        magicImmunity.statusImage = magicImmunityImage;
        magicImmunity.statusName = magicImmunityStatusName;
        magicImmunity.statusDescription = magicImmunityStatusDescription;
        magicImmunity.statusStacks = magicImmunityStatusStacks;
        allIcons.Add(magicImmunity);
    }

    public void CreatePhysicalImmunityIconData()
    {
        StatusIcon physicalImmunity = gameObject.AddComponent<StatusIcon>();
        physicalImmunity.statusImage = physicalImmunityImage;
        physicalImmunity.statusName = physicalImmunityStatusName;
        physicalImmunity.statusDescription = physicalImmunityStatusDescription;
        physicalImmunity.statusStacks = physicalImmunityStatusStacks;
        allIcons.Add(physicalImmunity);
    }
    public void CreateFlankedIconData()
    {
        StatusIcon flanked = gameObject.AddComponent<StatusIcon>();
        flanked.statusImage = flankedImage;
        flanked.statusName = flankedStatusName;
        flanked.statusDescription = flankedStatusDescription;
        flanked.statusStacks = flankedStatusStacks;
        allIcons.Add(flanked);
    }


    [Header("Knockdown Icon Data")]
    public Sprite knockDownImage;
    public string knockDownStatusName;
    public string knockDownStatusDescription;
    public int knockDownStatusStacks;

    [Header("Strength Icon Data")]
    public Sprite strengthImage;
    public string strengthStatusName;
    public string strengthStatusDescription;
    public int strengthStatusStacks;

    [Header("Dexterity Icon Data")]
    public Sprite dexterityImage;
    public string dexterityStatusName;
    public string dexterityStatusDescription;
    public int dexterityStatusStacks;

    [Header("Stunned Icon Data")]
    public Sprite stunnedImage;
    public string stunnedStatusName;
    public string stunnedStatusDescription;
    public int stunnedStatusStacks;

    [Header("Pinned Icon Data")]
    public Sprite pinnedImage;
    public string pinnedStatusName;
    public string pinnedStatusDescription;
    public int pinnedStatusStacks;

    [Header("Barrier Icon Data")]
    public Sprite barrierImage;
    public string barrierStatusName;
    public string barrierStatusDescription;
    public int barrierStatusStacks;

    [Header("Enrage Icon Data")]
    public Sprite enrageImage;
    public string enrageStatusName;
    public string enrageStatusDescription;
    public int enrageStatusStacks;

    [Header("Growing Icon Data")]
    public Sprite growingImage;
    public string growingStatusName;
    public string growingStatusDescription;
    public int growingStatusStacks;

    [Header("Volatile Icon Data")]
    public Sprite volatileImage;
    public string volatileStatusName;
    public string volatileStatusDescription;
    public int volatileStatusStacks;

    [Header("Sleeping Icon Data")]
    public Sprite sleepingImage;
    public string sleepingStatusName;
    public string sleepingStatusDescription;
    public int sleepingStatusStacks;

    [Header("Camoflage Icon Data")]
    public Sprite camoflageImage;
    public string camoflageStatusName;
    public string camoflageStatusDescription;
    public int camoflageStatusStacks;

    [Header("Poison Icon Data")]
    public Sprite poisonImage;
    public string poisonStatusName;
    public string poisonStatusDescription;
    public int poisonStatusStacks;

    [Header("Cautious Icon Data")]
    public Sprite cautiousImage;
    public string cautiousStatusName;
    public string cautiousStatusDescription;
    public int cautiousStatusStacks;

    [Header("Fleet Footed Icon Data")]
    public Sprite fleetFootedImage;
    public string fleetFootedStatusName;
    public string fleetFootedStatusDescription;
    public int fleetFootedStatusStacks;

    [Header("Encouraging Presence Icon Data")]
    public Sprite encouragingPresenceImage;
    public string encouragingPresenceStatusName;
    public string encouragingPresenceStatusDescription;
    public int encouragingPresenceStatusStacks;

    [Header("Poisonous Icon Data")]
    public Sprite poisonousImage;
    public string poisonousStatusName;
    public string poisonousStatusDescription;
    public int poisonousStatusStacks;

    [Header("Preparation Icon Data")]
    public Sprite preparationImage;
    public string preparationStatusName;
    public string preparationStatusDescription;
    public int preparationStatusStacks;

    [Header("Stealth Icon Data")]
    public Sprite stealthImage;
    public string stealthStatusName;
    public string stealthStatusDescription;
    public int stealthStatusStacks;

    [Header("Thorns Icon Data")]
    public Sprite thornsImage;
    public string thornsStatusName;
    public string thornsStatusDescription;
    public int thornsStatusStacks;

    [Header("Unhygienic Icon Data")]
    public Sprite unhygienicImage;
    public string unhygienicStatusName;
    public string unhygienicStatusDescription;
    public int unhygienicStatusStacks;

    [Header("Quick Reflexes Icon Data")]
    public Sprite quickReflexesImage;
    public string quickReflexesStatusName;
    public string quickReflexesStatusDescription;
    public int quickReflexesStatusStacks;

    [Header("Regeneration Icon Data")]
    public Sprite regenerationImage;
    public string regenerationStatusName;
    public string regenerationStatusDescription;
    public int regenerationStatusStacks;

    [Header("Adaptive Icon Data")]
    public Sprite adaptiveImage;
    public string adaptiveStatusName;
    public string adaptiveStatusDescription;
    public int adaptiveStatusStacks;

    [Header("Poison Immunity Icon Data")]
    public Sprite poisonImmunityImage;
    public string poisonImmunityStatusName;
    public string poisonImmunityStatusDescription;
    public int poisonImmunityStatusStacks;

    [Header("Hateful Presence Icon Data")]
    public Sprite hatefulPresenceImage;
    public string hatefulPresenceStatusName;
    public string hatefulPresenceStatusDescription;
    public int hatefulPresenceStatusStacks;

    [Header("Soul Drain Aura Icon Data")]
    public Sprite soulDrainAuraImage;
    public string soulDrainAuraStatusName;
    public string soulDrainAuraStatusDescription;
    public int soulDrainAuraStatusStacks;

    [Header("Lightning Shield Icon Data")]
    public Sprite lightningShieldImage;
    public string lightningShieldStatusName;
    public string lightningShieldStatusDescription;
    public int lightningShieldStatusStacks;

    [Header("Thick Of The Fight Icon Data")]
    public Sprite thickOfTheFightImage;
    public string thickOfTheFightStatusName;
    public string thickOfTheFightStatusDescription;
    public int thickOfTheFightStatusStacks;

    [Header("Temporary Strength Icon Data")]
    public Sprite temporaryStrengthImage;
    public string temporaryStrengthStatusName;
    public string temporaryStrengthStatusDescription;
    public int temporaryStrengthStatusStacks;

    [Header("Rune Icon Data")]
    public Sprite runeImage;
    public string runeStatusName;
    public string runeStatusDescription;
    public int runeStatusStacks;

    [Header("Exposed Icon Data")]
    public Sprite exposedImage;
    public string exposedStatusName;
    public string exposedStatusDescription;
    public int exposedStatusStacks;

    [Header("Exhausted Icon Data")]
    public Sprite exhaustedImage;
    public string exhaustedStatusName;
    public string exhaustedStatusDescription;
    public int exhaustedStatusStacks;

    [Header("Magic Immunity Icon Data")]
    public Sprite magicImmunityImage;
    public string magicImmunityStatusName;
    public string magicImmunityStatusDescription;
    public int magicImmunityStatusStacks;

    [Header("Physical Immunity Icon Data")]
    public Sprite physicalImmunityImage;
    public string physicalImmunityStatusName;
    public string physicalImmunityStatusDescription;
    public int physicalImmunityStatusStacks;

    [Header("Flanked Icon Data")]
    public Sprite flankedImage;
    public string flankedStatusName;
    public string flankedStatusDescription;
    public int flankedStatusStacks;

}
