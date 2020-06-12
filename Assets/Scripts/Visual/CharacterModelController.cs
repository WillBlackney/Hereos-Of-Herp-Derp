using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterModelController
{
    // View Logic
    #region
    public static void SetModelScale(UniversalCharacterModel model, float newScale)
    {
        Debug.Log("CharacterModelController.SetModelScale() called...");
        model.scalingParent.localScale = new Vector3(newScale, newScale, newScale);
    }
    public static void BuildModelFromPresetString(UniversalCharacterModel model, string preset)
    {
        Debug.Log("CharacterModelController.BuildModelFromPresetString() called, preset string: " + preset);

        // Starting playable presets
        if (preset == "Paladin")
        {
            SetUpAsPaladinPreset(model);
        }
        else if (preset == "Knight")
        {
            SetUpAsKnightPreset(model);
        }
        else if (preset == "Arcanist")
        {
            SetUpAsArcanistPreset(model);
        }
        else if (preset == "Berserker")
        {
            SetUpAsBerserkerPreset(model);
        }
        else if (preset == "Shadow Blade")
        {
            SetUpAsShadowBladePreset(model);
        }
        else if (preset == "Rogue")
        {
            SetUpAsRoguePreset(model);
        }
        else if (preset == "Monk")
        {
            SetUpAsMonkPreset(model);
        }
        else if (preset == "Priest")
        {
            SetUpAsPriestPreset(model);
        }
        else if (preset == "Warlock")
        {
            SetUpAsWarlockPreset(model);
        }
        else if (preset == "Alchemist")
        {
            SetUpAsAlchemistPreset(model);
        }
        else if (preset == "Marksman")
        {
            SetUpAsMarksmanPreset(model);
        }
        else if (preset == "Wayfarer")
        {
            SetUpAsWayfarerPreset(model);
        }
        else if (preset == "Battle Mage")
        {
            SetUpAsBattleMagePreset(model);
        }
        else if (preset == "Illusionist")
        {
            SetUpAsIllusionistPreset(model);
        }
        else if (preset == "Frost Knight")
        {
            SetUpAsFrostKnightPreset(model);
        }
        else if (preset == "Shaman")
        {
            SetUpAsShamanPreset(model);
        }
        else if (preset == "Death Knight")
        {
            SetUpAsDeathKnightPreset(model);
        }
        else if (preset == "Bulwark")
        {
            SetUpAsBulwarkPreset(model);
        }

        // Misc Presets
        else if (preset == "King")
        {
            SetUpAsKingPreset(model);
        }
        else if (preset == "Random")
        {
            SetUpAsRandomPreset(model);
        }


        // Enemy Presets

        // Skeletons
        else if (preset == "Skeleton Mage")
        {
            SetUpAsSkeletonMagePreset(model);
        }
        else if (preset == "Skeleton Assassin")
        {
            SetUpAsSkeletonAssassinPreset(model);
        }
        else if (preset == "Skeleton Barbarian")
        {
            SetUpAsSkeletonBarbarianPreset(model);
        }
        else if (preset == "Skeleton Warrior")
        {
            SetUpAsSkeletonWarriorPreset(model);
        }
        else if (preset == "Skeleton Priest")
        {
            SetUpAsSkeletonPriestPreset(model);
        }
        else if (preset == "Skeleton Archer")
        {
            SetUpAsSkeletonArcherPreset(model);
        }
        else if (preset == "Skeleton Necromancer")
        {
            SetUpAsSkeletonNecromancerPreset(model);
        }

        // Demons
        else if (preset == "Demon Berserker")
        {
            SetUpAsDemonBerserkerPreset(model);
        }
        else if (preset == "Demon Hell Guard")
        {
            SetUpAsDemonHellGuardPreset(model);
        }
        else if (preset == "Demon Blade Master")
        {
            SetUpAsDemonBladeMasterPreset(model);
        }

        // Goblins
        else if (preset == "Goblin Stabby")
        {
            SetUpAsGoblinStabbyPreset(model);
        }
        else if (preset == "Goblin Shooty")
        {
            SetUpAsGoblinShootyPreset(model);
        }
        else if (preset == "Goblin War Chief")
        {
            SetUpAsGoblinWarChiefPreset(model);
        }
        else if (preset == "Goblin Shield Bearer")
        {
            SetUpAsGoblinShieldBearerPreset(model);
        }

        // Elemental Golems
        else if (preset == "Fire Golem")
        {
            SetUpAsFireGolemPreset(model);
        }
        else if (preset == "Frost Golem")
        {
            SetUpAsFrostGolemPreset(model);
        }
        else if (preset == "Air Golem")
        {
            SetUpAsAirGolemPreset(model);
        }
        else if (preset == "Poison Golem")
        {
            SetUpAsPoisonGolemPreset(model);
        }

        // Misc
        else if (preset == "Volatile Zombie")
        {
            SetUpAsVolatileZombiePreset(model);
        }
        else if (preset == "Toxic Zombie")
        {
            SetUpAsVolatileZombiePreset(model);
        }
        else if (preset == "Dark Elf Ranger")
        {
            SetUpAsDarkElfRangerPreset(model);
        }
    }
    public static void BuildModelFromCharacterPresetData(UniversalCharacterModel model, CharacterPresetData data)
    {
        Debug.Log("CharacterModelController.BuildModelFromCharacterPresetData() called...");

        CompletelyDisableAllViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Body Parts + Clothing
        foreach (ModelElementData elementData in data.activeModelElements)
        {
            foreach(UniversalCharacterModelElement element in model.allModelElements)
            {
                if(elementData.elementName == element.gameObject.name)
                {
                    Debug.Log("CharacterModelController.BuildModelFromCharacterPresetData() found model element GO with matching name of " +
                        elementData.elementName + ", enabling GO...");
                    EnableAndSetElementOnModel(model, element);
                    break;
                }
            }
        }

        // set MH weapon model view
        foreach (UniversalCharacterModelElement ucme in model.allMainHandWeapons)
        {
            if (ucme.weaponsWithMyView.Contains(data.mhWeapon))
            {
                Debug.Log("CharacterModelController.BuildModelFromCharacterPresetData() found model element GO with matching name of " +
                        data.mhWeapon.Name + ", enabling GO...");
                EnableAndSetElementOnModel(model, ucme);
                break;
            }
        }

        // Set OH weapon model view
        if (data.ohWeapon != null)
        {
            foreach (UniversalCharacterModelElement ucme in model.allOffHandWeapons)
            {
                if (ucme.weaponsWithMyView.Contains(data.ohWeapon))
                {
                    Debug.Log("CharacterModelController.BuildModelFromCharacterPresetData() found model element GO with matching name of " +
                        data.ohWeapon.Name + ", enabling GO...");
                    EnableAndSetElementOnModel(model, ucme);
                    break;
                }
            }
        }
    }
    public static void DisableAllViewsInList(List<GameObject> listOfViews)
    {
        foreach(GameObject go in listOfViews)
        {
            go.SetActive(false);
        }
    }
    public static void DisableAllViewsInList(List<UniversalCharacterModelElement> listOfViews)
    {
        foreach (UniversalCharacterModelElement ucme in listOfViews)
        {
            ucme.gameObject.SetActive(false);
        }
    }
    public static void CompletelyDisableAllViews(UniversalCharacterModel model)
    {
        Debug.Log("CharacterModelController.CompletelyDisableAllViews() called...");

        // new logic
        foreach(UniversalCharacterModelElement ele in model.allModelElements)
        {
            ele.gameObject.SetActive(false);
        }
        
        // old logic
        // Disable weapons
        DisableAllViewsInList(model.allMainHandWeapons);
        DisableAllViewsInList(model.allOffHandWeapons);

        // Disable all body pieces
        DisableAllViewsInList(model.allLeftLegs);
        DisableAllViewsInList(model.allRightLegs);
        DisableAllViewsInList(model.allHeads);
        DisableAllViewsInList(model.allRightHands);
        DisableAllViewsInList(model.allRightArms);
        DisableAllViewsInList(model.allLeftHands);
        DisableAllViewsInList(model.allLeftArms);
        DisableAllViewsInList(model.allChests);
        

        // Clear all body part refs
        //ClearAllActiveBodyPartReferences(model);
    }
    public static void DisableAllClothingViews(UniversalCharacterModel model)
    {
        DisableAllViewsInList(model.allHeadWear);
        DisableAllViewsInList(model.allChestWear);
        DisableAllViewsInList(model.allLeftLegWear);
        DisableAllViewsInList(model.allRightLegWear);
        DisableAllViewsInList(model.allLeftArmWear);
        DisableAllViewsInList(model.allRightArmWear);
        DisableAllViewsInList(model.allLeftHandWear);
        DisableAllViewsInList(model.allRightHandWear);
    }
    public static void ClearAllActiveBodyPartReferences(UniversalCharacterModel model)
    {
        Debug.Log("CharacterModelController.ClearAllActiveBodyPartReferences() called...");

        // Body Parts
        model.activeHead = null;
        model.activeFace = null;
        model.activeLeftLeg = null;
        model.activeRightLeg = null;
        model.activeRightHand = null;
        model.activeRightArm = null;
        model.activeLeftHand = null;
        model.activeLeftArm = null;
        model.activeChest = null;

        // Clothing 
        model.activeHeadWear = null;
        model.activeChestWear = null;
        model.activeRightLegWear = null;
        model.activeLeftLegWear = null;
        model.activeRightArmWear = null;
        model.activeRightHandWear = null;
        model.activeLeftArmWear = null;
        model.activeLeftHandWear = null;

        // Weapons
        model.activeMainHandWeapon = null;
        model.activeOffHandWeapon = null;
    }
    public static void DisableAllActiveElementViews(UniversalCharacterModel model)
    {
        Debug.Log("CharacterModelController.DisableAllActiveBodyPartViews() called...");

        // Body Parts
        if (model.activeHead)
        {
            model.activeHead.gameObject.SetActive(false);
        }
        if (model.activeFace)
        {
            model.activeFace.gameObject.SetActive(false);
        }
        if (model.activeLeftLeg)
        {
            model.activeLeftLeg.gameObject.SetActive(false);
        }
        if (model.activeRightLeg)
        {
            model.activeRightLeg.gameObject.SetActive(false);
        }
        if (model.activeRightHand)
        {
            model.activeRightHand.gameObject.SetActive(false);
        }
        if (model.activeRightArm)
        {
            model.activeRightArm.gameObject.SetActive(false);
        }
        if (model.activeLeftHand)
        {
            model.activeLeftHand.gameObject.SetActive(false);
        }
        if (model.activeLeftArm)
        {
            model.activeLeftArm.gameObject.SetActive(false);
        }
        if (model.activeChest)
        {
            model.activeChest.gameObject.SetActive(false);
        }

        // Clothing 
        if (model.activeHeadWear)
        {
            model.activeHeadWear.gameObject.SetActive(false);
        }
        if (model.activeChestWear)
        {
            model.activeChestWear.gameObject.SetActive(false);
        }
        if (model.activeRightLegWear)
        {
            model.activeRightLegWear.gameObject.SetActive(false);
        }
        if (model.activeLeftLegWear)
        {
            model.activeLeftLegWear.gameObject.SetActive(false);
        }
        if (model.activeRightArmWear)
        {
            model.activeRightArmWear.gameObject.SetActive(false);
        }
        if (model.activeRightHandWear)
        {
            model.activeRightHandWear.gameObject.SetActive(false);
        }
        if (model.activeLeftArmWear)
        {
            model.activeLeftArmWear.gameObject.SetActive(false);
        }
        if (model.activeLeftHandWear)
        {
            model.activeLeftHandWear.gameObject.SetActive(false);
        }

        // Weapons
        if (model.activeMainHandWeapon)
        {
            model.activeMainHandWeapon.gameObject.SetActive(false);
        }
        if (model.activeOffHandWeapon)
        {
            model.activeOffHandWeapon.gameObject.SetActive(false);
        }
    }
    #endregion

    // Build Defender Presets
    #region
    public static void SetUpAsPaladinPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.paladinLeftLeg.SetActive(true);
        model.paladinRightLeg.SetActive(true);
        model.paladinHead.SetActive(true);
        model.paladinRightHand.SetActive(true);
        model.paladinRightArm.SetActive(true);
        model.paladinLeftHand.SetActive(true);
        model.paladinLeftArm.SetActive(true);
        model.paladinChest.SetActive(true);

        model.simpleBattleAxe.SetActive(true);
    }
    public static void SetUpAsKnightPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.knightLeftLeg.SetActive(true);
        model.knightRightLeg.SetActive(true);
        model.knightHead.SetActive(true);
        model.knightRightHand.SetActive(true);
        model.knightRightArm.SetActive(true);
        model.knightLeftHand.SetActive(true);
        model.knightLeftArm.SetActive(true);
        model.knightChest.SetActive(true);

        model.simpleShieldOH.SetActive(true);
        model.simpleSwordMH.SetActive(true);
    }
    public static void SetUpAsArcanistPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.mageLeftLeg.SetActive(true);
        model.mageRightLeg.SetActive(true);
        model.mageHead.SetActive(true);
        model.mageRightHand.SetActive(true);
        model.mageRightArm.SetActive(true);
        model.mageLeftHand.SetActive(true);
        model.mageLeftArm.SetActive(true);
        model.mageChest.SetActive(true);

        model.simpleStaffMH.SetActive(true);
    }
    public static void SetUpAsBerserkerPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.barbarianLeftLeg.SetActive(true);
        model.barbarianRightLeg.SetActive(true);
        model.barbarianHead.SetActive(true);
        model.barbarianRightHand.SetActive(true);
        model.barbarianRightArm.SetActive(true);
        model.barbarianLeftHand.SetActive(true);
        model.barbarianLeftArm.SetActive(true);
        model.barbarianChest.SetActive(true);

        model.simpleBattleAxe.SetActive(true);
    }
    public static void SetUpAsShadowBladePreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.shadowBladeLeftLeg.SetActive(true);
        model.shadowBladeRightLeg.SetActive(true);
        model.shadowBladeHead.SetActive(true);
        model.shadowBladeRightHand.SetActive(true);
        model.shadowBladeRightArm.SetActive(true);
        model.shadowBladeLeftHand.SetActive(true);
        model.shadowBladeLeftArm.SetActive(true);
        model.shadowBladeChest.SetActive(true);

        model.simpleDaggerMH.SetActive(true);
        model.simpleDaggerOH.SetActive(true);
    }
    public static void SetUpAsRoguePreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.rogueLeftLeg.SetActive(true);
        model.rogueRightLeg.SetActive(true);
        model.rogueHead.SetActive(true);
        model.rogueRightHand.SetActive(true);
        model.rogueRightArm.SetActive(true);
        model.rogueLeftHand.SetActive(true);
        model.rogueLeftArm.SetActive(true);
        model.rogueChest.SetActive(true);

        model.simpleSwordMH.SetActive(true);
        model.simpleSwordOH.SetActive(true);
    }
    public static void SetUpAsMonkPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.monkLeftLeg.SetActive(true);
        model.monkRightLeg.SetActive(true);
        model.monkHead.SetActive(true);
        model.monkRightHand.SetActive(true);
        model.monkRightArm.SetActive(true);
        model.monkLeftHand.SetActive(true);
        model.monkLeftArm.SetActive(true);
        model.monkChest.SetActive(true);

        model.simpleStaffMH.SetActive(true);
    }
    public static void SetUpAsPriestPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.priestLeftLeg.SetActive(true);
        model.priestRightLeg.SetActive(true);
        model.priestHead.SetActive(true);
        model.priestRightHand.SetActive(true);
        model.priestRightArm.SetActive(true);
        model.priestLeftHand.SetActive(true);
        model.priestLeftArm.SetActive(true);
        model.priestChest.SetActive(true);

        model.simpleStaffMH.SetActive(true);
    }
    public static void SetUpAsWarlockPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.warlockLeftLeg.SetActive(true);
        model.warlockRightLeg.SetActive(true);
        model.warlockHead.SetActive(true);
        model.warlockRightHand.SetActive(true);
        model.warlockRightArm.SetActive(true);
        model.warlockLeftHand.SetActive(true);
        model.warlockLeftArm.SetActive(true);
        model.warlockChest.SetActive(true);

        model.simpleStaffMH.SetActive(true);
    }
    public static void SetUpAsAlchemistPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.alchemistLeftLeg.SetActive(true);
        model.alchemistRightLeg.SetActive(true);
        model.alchemistHead.SetActive(true);
        model.alchemistRightHand.SetActive(true);
        model.alchemistRightArm.SetActive(true);
        model.alchemistLeftHand.SetActive(true);
        model.alchemistLeftArm.SetActive(true);
        model.alchemistChest.SetActive(true);

        model.simpleStaffMH.SetActive(true);
    }
    public static void SetUpAsMarksmanPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.marksmanLeftLeg.SetActive(true);
        model.marksmanRightLeg.SetActive(true);
        model.marksmanHead.SetActive(true);
        model.marksmanRightHand.SetActive(true);
        model.marksmanRightArm.SetActive(true);
        model.marksmanLeftHand.SetActive(true);
        model.marksmanLeftArm.SetActive(true);
        model.marksmanChest.SetActive(true);

        model.simpleBowMH.SetActive(true);
    }
    public static void SetUpAsWayfarerPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.wayfarerLeftLeg.SetActive(true);
        model.wayfarerRightLeg.SetActive(true);
        model.wayfarerHead.SetActive(true);
        model.wayfarerRightHand.SetActive(true);
        model.wayfarerRightArm.SetActive(true);
        model.wayfarerLeftHand.SetActive(true);
        model.wayfarerLeftArm.SetActive(true);
        model.wayfarerChest.SetActive(true);

        model.simpleBowMH.SetActive(true);
    }
    public static void SetUpAsBattleMagePreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.spellBladeLeftLeg.SetActive(true);
        model.spellBladeRightLeg.SetActive(true);
        model.spellBladeHead.SetActive(true);
        model.spellBladeRightHand.SetActive(true);
        model.spellBladeRightArm.SetActive(true);
        model.spellBladeLeftHand.SetActive(true);
        model.spellBladeLeftArm.SetActive(true);
        model.spellBladeChest.SetActive(true);

        model.simpleBattleAxe.SetActive(true);
    }
    public static void SetUpAsIllusionistPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.illusionistLeftLeg.SetActive(true);
        model.illusionistRightLeg.SetActive(true);
        model.illusionistHead.SetActive(true);
        model.illusionistRightHand.SetActive(true);
        model.illusionistRightArm.SetActive(true);
        model.illusionistLeftHand.SetActive(true);
        model.illusionistLeftArm.SetActive(true);
        model.illusionistChest.SetActive(true);

        model.simpleBowMH.SetActive(true);
    }
    public static void SetUpAsFrostKnightPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.frostKnightLeftLeg.SetActive(true);
        model.frostKnightRightLeg.SetActive(true);
        model.frostKnightHead.SetActive(true);
        model.frostKnightRightHand.SetActive(true);
        model.frostKnightRightArm.SetActive(true);
        model.frostKnightLeftHand.SetActive(true);
        model.frostKnightLeftArm.SetActive(true);
        model.frostKnightChest.SetActive(true);

        model.simpleBattleAxe.SetActive(true);
    }
    public static void SetUpAsShamanPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.shamanLeftLeg.SetActive(true);
        model.shamanRightLeg.SetActive(true);
        model.shamanHead.SetActive(true);
        model.shamanRightHand.SetActive(true);
        model.shamanRightArm.SetActive(true);
        model.shamanLeftHand.SetActive(true);
        model.shamanLeftArm.SetActive(true);
        model.shamanChest.SetActive(true);

        model.simpleBattleAxe.SetActive(true);
    }
    public static void SetUpAsDeathKnightPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.deathKnightLeftLeg.SetActive(true);
        model.deathKnightRightLeg.SetActive(true);
        model.deathKnightHead.SetActive(true);
        model.deathKnightRightHand.SetActive(true);
        model.deathKnightRightArm.SetActive(true);
        model.deathKnightLeftHand.SetActive(true);
        model.deathKnightLeftArm.SetActive(true);
        model.deathKnightChest.SetActive(true);

        model.simpleBattleAxe.SetActive(true);
    }
    public static void SetUpAsBulwarkPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.bulwarkLeftLeg.SetActive(true);
        model.bulwarkRightLeg.SetActive(true);
        model.bulwarkHead.SetActive(true);
        model.bulwarkRightHand.SetActive(true);
        model.bulwarkRightArm.SetActive(true);
        model.bulwarkLeftHand.SetActive(true);
        model.bulwarkLeftArm.SetActive(true);
        model.bulwarkChest.SetActive(true);

        model.simpleSwordMH.SetActive(true);
        model.simpleShieldOH.SetActive(true);
    }
    public static void SetUpAsRandomPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.randomLeftLeg.SetActive(true);
        model.randomRightLeg.SetActive(true);
        model.randomHead.SetActive(true);
        model.randomRightHand.SetActive(true);
        model.randomRightArm.SetActive(true);
        model.randomLeftHand.SetActive(true);
        model.randomLeftArm.SetActive(true);
        model.randomChest.SetActive(true);
    }
    public static void SetUpAsKingPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.kingLeftLeg.SetActive(true);
        model.kingRightLeg.SetActive(true);
        model.kingHead.SetActive(true);
        model.kingRightHand.SetActive(true);
        model.kingRightArm.SetActive(true);
        model.kingLeftHand.SetActive(true);
        model.kingLeftArm.SetActive(true);
        model.kingChest.SetActive(true);
    }
    #endregion

    // Build Enemy Presets
    #region
    public static void SetUpAsVolatileZombiePreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.volatileZombieLeftLeg.SetActive(true);
        model.volatileZombieRightLeg.SetActive(true);
        model.volatileZombieHead.SetActive(true);
        model.volatileZombieRightHand.SetActive(true);
        model.volatileZombieRightArm.SetActive(true);
        model.volatileZombieLeftHand.SetActive(true);
        model.volatileZombieLeftArm.SetActive(true);
        model.volatileZombieChest.SetActive(true);
    }
    public static void SetUpAsDarkElfRangerPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.darkElfRangerLeftLeg.SetActive(true);
        model.darkElfRangerRightLeg.SetActive(true);
        model.darkElfRangerHead.SetActive(true);
        model.darkElfRangerRightHand.SetActive(true);
        model.darkElfRangerRightArm.SetActive(true);
        model.darkElfRangerLeftHand.SetActive(true);
        model.darkElfRangerLeftArm.SetActive(true);
        model.darkElfRangerChest.SetActive(true);
    }
    public static void SetUpAsSkeletonArcherPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.skeletonArcherLeftLeg.SetActive(true);
        model.skeletonArcherRightLeg.SetActive(true);
        model.skeletonArcherHead.SetActive(true);
        model.skeletonArcherRightHand.SetActive(true);
        model.skeletonArcherRightArm.SetActive(true);
        model.skeletonArcherLeftHand.SetActive(true);
        model.skeletonArcherLeftArm.SetActive(true);
        model.skeletonArcherChest.SetActive(true);
    }
    public static void SetUpAsSkeletonAssassinPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.skeletonAssassinLeftLeg.SetActive(true);
        model.skeletonAssassinRightLeg.SetActive(true);
        model.skeletonAssassinHead.SetActive(true);
        model.skeletonAssassinRightHand.SetActive(true);
        model.skeletonAssassinRightArm.SetActive(true);
        model.skeletonAssassinLeftHand.SetActive(true);
        model.skeletonAssassinLeftArm.SetActive(true);
        model.skeletonAssassinChest.SetActive(true);
    }
    public static void SetUpAsSkeletonWarriorPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.skeletonWarriorLeftLeg.SetActive(true);
        model.skeletonWarriorRightLeg.SetActive(true);
        model.skeletonWarriorHead.SetActive(true);
        model.skeletonWarriorRightHand.SetActive(true);
        model.skeletonWarriorRightArm.SetActive(true);
        model.skeletonWarriorLeftHand.SetActive(true);
        model.skeletonWarriorLeftArm.SetActive(true);
        model.skeletonWarriorChest.SetActive(true);
    }
    public static void SetUpAsSkeletonBarbarianPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.skeletonBarbarianLeftLeg.SetActive(true);
        model.skeletonBarbarianRightLeg.SetActive(true);
        model.skeletonBarbarianHead.SetActive(true);
        model.skeletonBarbarianRightHand.SetActive(true);
        model.skeletonBarbarianRightArm.SetActive(true);
        model.skeletonBarbarianLeftHand.SetActive(true);
        model.skeletonBarbarianLeftArm.SetActive(true);
        model.skeletonBarbarianChest.SetActive(true);
    }
    public static void SetUpAsSkeletonPriestPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.skeletonPriestLeftLeg.SetActive(true);
        model.skeletonPriestRightLeg.SetActive(true);
        model.skeletonPriestHead.SetActive(true);
        model.skeletonPriestRightHand.SetActive(true);
        model.skeletonPriestRightArm.SetActive(true);
        model.skeletonPriestLeftHand.SetActive(true);
        model.skeletonPriestLeftArm.SetActive(true);
        model.skeletonPriestChest.SetActive(true);
    }
    public static void SetUpAsSkeletonMagePreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.skeletonMageLeftLeg.SetActive(true);
        model.skeletonMageRightLeg.SetActive(true);
        model.skeletonMageHead.SetActive(true);
        model.skeletonMageRightHand.SetActive(true);
        model.skeletonMageRightArm.SetActive(true);
        model.skeletonMageLeftHand.SetActive(true);
        model.skeletonMageLeftArm.SetActive(true);
        model.skeletonMageChest.SetActive(true);
    }
    public static void SetUpAsSkeletonNecromancerPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.skeletonNecromancerLeftLeg.SetActive(true);
        model.skeletonNecromancerRightLeg.SetActive(true);
        model.skeletonNecromancerHead.SetActive(true);
        model.skeletonNecromancerRightHand.SetActive(true);
        model.skeletonNecromancerRightArm.SetActive(true);
        model.skeletonNecromancerLeftHand.SetActive(true);
        model.skeletonNecromancerLeftArm.SetActive(true);
        model.skeletonNecromancerChest.SetActive(true);
    }
    public static void SetUpAsSkeletonKingPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.skeletonKingLeftLeg.SetActive(true);
        model.skeletonKingRightLeg.SetActive(true);
        model.skeletonKingHead.SetActive(true);
        model.skeletonKingRightHand.SetActive(true);
        model.skeletonKingRightArm.SetActive(true);
        model.skeletonKingLeftHand.SetActive(true);
        model.skeletonKingLeftArm.SetActive(true);
        model.skeletonKingChest.SetActive(true);
    }
    public static void SetUpAsSkeletonSoldierPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.skeletonSoldierLeftLeg.SetActive(true);
        model.skeletonSoldierRightLeg.SetActive(true);
        model.skeletonSoldierHead.SetActive(true);
        model.skeletonSoldierRightHand.SetActive(true);
        model.skeletonSoldierRightArm.SetActive(true);
        model.skeletonSoldierLeftHand.SetActive(true);
        model.skeletonSoldierLeftArm.SetActive(true);
        model.skeletonSoldierChest.SetActive(true);
    }
    public static void SetUpAsGoblinStabbyPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.goblinStabbyLeftLeg.SetActive(true);
        model.goblinStabbyRightLeg.SetActive(true);
        model.goblinStabbyHead.SetActive(true);
        model.goblinStabbyRightHand.SetActive(true);
        model.goblinStabbyRightArm.SetActive(true);
        model.goblinStabbyLeftHand.SetActive(true);
        model.goblinStabbyLeftArm.SetActive(true);
        model.goblinStabbyChest.SetActive(true);
    }
    public static void SetUpAsGoblinWarChiefPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.goblinWarChiefLeftLeg.SetActive(true);
        model.goblinWarChiefRightLeg.SetActive(true);
        model.goblinWarChiefHead.SetActive(true);
        model.goblinWarChiefRightHand.SetActive(true);
        model.goblinWarChiefRightArm.SetActive(true);
        model.goblinWarChiefLeftHand.SetActive(true);
        model.goblinWarChiefLeftArm.SetActive(true);
        model.goblinWarChiefChest.SetActive(true);
    }
    public static void SetUpAsGoblinShootyPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.goblinShootyLeftLeg.SetActive(true);
        model.goblinShootRightLeg.SetActive(true);
        model.goblinShootyHead.SetActive(true);
        model.goblinShootyRightHand.SetActive(true);
        model.goblinShootyRightArm.SetActive(true);
        model.goblinShootyLeftHand.SetActive(true);
        model.goblinShootyLeftArm.SetActive(true);
        model.goblinShootyChest.SetActive(true);
    }
    public static void SetUpAsGoblinShieldBearerPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.goblinShieldBearerLeftLeg.SetActive(true);
        model.goblinShieldBearerRightLeg.SetActive(true);
        model.goblinShieldBearerHead.SetActive(true);
        model.goblinShieldBearerRightHand.SetActive(true);
        model.goblinShieldBearerRightArm.SetActive(true);
        model.goblinShieldBearerLeftHand.SetActive(true);
        model.goblinShieldBearerLeftArm.SetActive(true);
        model.goblinShieldBearerChest.SetActive(true);
    }
    public static void SetUpAsMorkPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.morkLeftLeg.SetActive(true);
        model.morkRightLeg.SetActive(true);
        model.morkHead.SetActive(true);
        model.morkRightHand.SetActive(true);
        model.morkRightArm.SetActive(true);
        model.morkLeftHand.SetActive(true);
        model.morkLeftArm.SetActive(true);
        model.morkChest.SetActive(true);
    }
    public static void SetUpAsFireGolemPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.fireGolemLeftLeg.SetActive(true);
        model.fireGolemRightLeg.SetActive(true);
        model.fireGolemHead.SetActive(true);
        model.fireGolemRightHand.SetActive(true);
        model.fireGolemRightArm.SetActive(true);
        model.fireGolemLeftHand.SetActive(true);
        model.fireGolemLeftArm.SetActive(true);
        model.fireGolemChest.SetActive(true);
    }
    public static void SetUpAsAirGolemPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.airGolemLeftLeg.SetActive(true);
        model.airGolemRightLeg.SetActive(true);
        model.airGolemHead.SetActive(true);
        model.airGolemRightHand.SetActive(true);
        model.airGolemRightArm.SetActive(true);
        model.airGolemLeftHand.SetActive(true);
        model.airGolemLeftArm.SetActive(true);
        model.airGolemChest.SetActive(true);
    }
    public static void SetUpAsPoisonGolemPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.poisonGolemLeftLeg.SetActive(true);
        model.poisonGolemRightLeg.SetActive(true);
        model.poisonGolemHead.SetActive(true);
        model.poisonGolemRightHand.SetActive(true);
        model.poisonGolemRightArm.SetActive(true);
        model.poisonGolemLeftHand.SetActive(true);
        model.poisonGolemLeftArm.SetActive(true);
        model.poisonGolemChest.SetActive(true);
    }
    public static void SetUpAsFrostGolemPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.frostGolemLeftLeg.SetActive(true);
        model.frostGolemRightLeg.SetActive(true);
        model.frostGolemHead.SetActive(true);
        model.frostGolemRightHand.SetActive(true);
        model.frostGolemRightArm.SetActive(true);
        model.frostGolemLeftHand.SetActive(true);
        model.frostGolemLeftArm.SetActive(true);
        model.frostGolemChest.SetActive(true);
    }
    public static void SetUpAsDemonBerserkerPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.demonBerserkerLeftLeg.SetActive(true);
        model.demonBerserkerRightLeg.SetActive(true);
        model.demonBerserkerHead.SetActive(true);
        model.demonBerserkerRightHand.SetActive(true);
        model.demonBerserkerRightArm.SetActive(true);
        model.demonBerserkerLeftHand.SetActive(true);
        model.demonBerserkerLeftArm.SetActive(true);
        model.demonBerserkerChest.SetActive(true);
    }
    public static void SetUpAsDemonBladeMasterPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.demonBladeMasterLeftLeg.SetActive(true);
        model.demonBladeMasterRightLeg.SetActive(true);
        model.demonBladeMasterHead.SetActive(true);
        model.demonBladeMasterRightHand.SetActive(true);
        model.demonBladeMasterRightArm.SetActive(true);
        model.demonBladeMasterLeftHand.SetActive(true);
        model.demonBladeMasterLeftArm.SetActive(true);
        model.demonBladeMasterChest.SetActive(true);
    }
    public static void SetUpAsDemonHellGuardPreset(UniversalCharacterModel model)
    {
        CompletelyDisableAllViews(model);

        model.demonHellGuardLeftLeg.SetActive(true);
        model.demonHellGuardRightLeg.SetActive(true);
        model.demonHellGuardHead.SetActive(true);
        model.demonHellGuardRightHand.SetActive(true);
        model.demonHellGuardRightArm.SetActive(true);
        model.demonHellGuardLeftHand.SetActive(true);
        model.demonHellGuardLeftArm.SetActive(true);
        model.demonHellGuardChest.SetActive(true);
    }
    #endregion

    // NEW MODEL LOGICC!!!!!!!!!

    // Set Model Race
    #region
    public static void SetBaseHumanView(UniversalCharacterModel model)
    {
        Debug.Log("CharacterModelController.SetBaseHumanView() called...");

        //CompletelyDisableAllViews(model);
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        model.myModelRace = UniversalCharacterModel.ModelRace.Human;

        // Body parts
        EnableAndSetElementOnModel(model, model.humanLeftLeg);
        EnableAndSetElementOnModel(model, model.humanRightLeg);
        EnableAndSetElementOnModel(model, model.humanHeads[0]);
        EnableAndSetElementOnModel(model, model.humanFaces[0]);
        EnableAndSetElementOnModel(model, model.humanRightHand);
        EnableAndSetElementOnModel(model, model.humanRightArm);
        EnableAndSetElementOnModel(model, model.humanLeftHand);
        EnableAndSetElementOnModel(model, model.humanLeftArm);
        EnableAndSetElementOnModel(model, model.humanChest);

        // Clothing parts
        EnableAndSetElementOnModel(model, model.allHeadWear[0]);
        EnableAndSetElementOnModel(model, model.allChestWear[0]);
        EnableAndSetElementOnModel(model, model.allLeftLegWear[0]);
        EnableAndSetElementOnModel(model, model.allRightLegWear[0]);
        EnableAndSetElementOnModel(model, model.allLeftArmWear[0]);
        EnableAndSetElementOnModel(model, model.allRightArmWear[0]);
        EnableAndSetElementOnModel(model, model.allLeftHandWear[0]);
        EnableAndSetElementOnModel(model, model.allRightHandWear[0]);
    }
    public static void SetBaseOrcView(UniversalCharacterModel model)
    {
        Debug.Log("CharacterModelController.SetBaseHumanView() called...");

        //CompletelyDisableAllViews(model);
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);       

        model.myModelRace = UniversalCharacterModel.ModelRace.Orc;

        // Body parts
        EnableAndSetElementOnModel(model, model.orcLeftLeg);
        EnableAndSetElementOnModel(model, model.orcRightLeg);
        EnableAndSetElementOnModel(model, model.orcHeads[0]);
        EnableAndSetElementOnModel(model, model.orcFaces[0]);
        EnableAndSetElementOnModel(model, model.orcRightHand);
        EnableAndSetElementOnModel(model, model.orcRightArm);
        EnableAndSetElementOnModel(model, model.orcLeftHand);
        EnableAndSetElementOnModel(model, model.orcLeftArm);
        EnableAndSetElementOnModel(model, model.orcChest);

        // Clothing parts
        EnableAndSetElementOnModel(model, model.allHeadWear[0]);
        EnableAndSetElementOnModel(model, model.allChestWear[0]);
        EnableAndSetElementOnModel(model, model.allLeftLegWear[0]);
        EnableAndSetElementOnModel(model, model.allRightLegWear[0]);
        EnableAndSetElementOnModel(model, model.allLeftArmWear[0]);
        EnableAndSetElementOnModel(model, model.allRightArmWear[0]);
        EnableAndSetElementOnModel(model, model.allLeftHandWear[0]);
        EnableAndSetElementOnModel(model, model.allRightHandWear[0]);
    }
    public static void SetBaseUndeadView(UniversalCharacterModel model)
    {
        Debug.Log("CharacterModelController.SetBaseUndeadView() called...");

       // CompletelyDisableAllViews(model);
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        model.myModelRace = UniversalCharacterModel.ModelRace.Undead;

        // Body parts
        EnableAndSetElementOnModel(model, model.undeadLeftLeg);
        EnableAndSetElementOnModel(model, model.undeadRightLeg);
        EnableAndSetElementOnModel(model, model.undeadHeads[0]);
        EnableAndSetElementOnModel(model, model.undeadFaces[0]);
        EnableAndSetElementOnModel(model, model.undeadRightHand);
        EnableAndSetElementOnModel(model, model.undeadRightArm);
        EnableAndSetElementOnModel(model, model.undeadLeftHand);
        EnableAndSetElementOnModel(model, model.undeadLeftArm);
        EnableAndSetElementOnModel(model, model.undeadChest);

        // Clothing parts
        EnableAndSetElementOnModel(model, model.allHeadWear[0]);
        EnableAndSetElementOnModel(model, model.allChestWear[0]);
        EnableAndSetElementOnModel(model, model.allLeftLegWear[0]);
        EnableAndSetElementOnModel(model, model.allRightLegWear[0]);
        EnableAndSetElementOnModel(model, model.allLeftArmWear[0]);
        EnableAndSetElementOnModel(model, model.allRightArmWear[0]);
        EnableAndSetElementOnModel(model, model.allLeftHandWear[0]);
        EnableAndSetElementOnModel(model, model.allRightHandWear[0]);
    }
    public static void SetBaseElfView(UniversalCharacterModel model)
    {
        Debug.Log("CharacterModelController.SetBaseElfView() called...");

        //CompletelyDisableAllViews(model);
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        model.myModelRace = UniversalCharacterModel.ModelRace.Elf;

        // Body parts
        EnableAndSetElementOnModel(model, model.elfLeftLeg);
        EnableAndSetElementOnModel(model, model.elfRightLeg);
        EnableAndSetElementOnModel(model, model.elfHeads[0]);
        EnableAndSetElementOnModel(model, model.elfFaces[0]);
        EnableAndSetElementOnModel(model, model.elfRightHand);
        EnableAndSetElementOnModel(model, model.elfRightArm);
        EnableAndSetElementOnModel(model, model.elfLeftHand);
        EnableAndSetElementOnModel(model, model.elfLeftArm);
        EnableAndSetElementOnModel(model, model.elfChest);

        // Clothing parts
        EnableAndSetElementOnModel(model, model.allHeadWear[0]);
        EnableAndSetElementOnModel(model, model.allChestWear[0]);
        EnableAndSetElementOnModel(model, model.allLeftLegWear[0]);
        EnableAndSetElementOnModel(model, model.allRightLegWear[0]);
        EnableAndSetElementOnModel(model, model.allLeftArmWear[0]);
        EnableAndSetElementOnModel(model, model.allRightArmWear[0]);
        EnableAndSetElementOnModel(model, model.allLeftHandWear[0]);
        EnableAndSetElementOnModel(model, model.allRightHandWear[0]);
    }
    #endregion

    // Set Specific Body Parts
    public static void EnableAndSetElementOnModel(UniversalCharacterModel model, UniversalCharacterModelElement element)
    {
        Debug.Log("CharacterModelController.EnableAndSetElementOnModel() called, enabling " +
            element.gameObject.name + " GO");

        // Set Active Body Part Reference
        if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.Chest)
        {
            if (model.activeChest != null)
            {
                model.activeChest.gameObject.SetActive(false);
            }            
            model.activeChest = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.Head)
        {
            if (model.activeHead != null)
            {
                model.activeHead.gameObject.SetActive(false);
            }
            model.activeHead = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.Face)
        {
            if (model.activeFace != null)
            {
                model.activeFace.gameObject.SetActive(false);
            }
            model.activeFace = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightArm)
        {
            if (model.activeRightArm != null)
            {
                model.activeRightArm.gameObject.SetActive(false);
            }
            model.activeRightArm = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightHand)
        {
            if (model.activeRightHand != null)
            {
                model.activeRightHand.gameObject.SetActive(false);
            }
            model.activeRightHand = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftArm)
        {
            if (model.activeLeftArm != null)
            {
                model.activeLeftArm.gameObject.SetActive(false);
            }
            model.activeLeftArm = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftHand)
        {
            if (model.activeLeftHand != null)
            {
                model.activeLeftHand.gameObject.SetActive(false);
            }
            model.activeLeftHand = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightLeg)
        {
            if (model.activeRightLeg != null)
            {
                model.activeRightLeg.gameObject.SetActive(false);
            }
            model.activeRightLeg = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftLeg)
        {
            if (model.activeLeftLeg != null)
            {
                model.activeLeftLeg.gameObject.SetActive(false);
            }
            model.activeLeftLeg = element;
        }

        // Set Active Weapons + Clothing Reference
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.HeadWear)
        {
            if (model.activeHeadWear != null)
            {
                model.activeHeadWear.gameObject.SetActive(false);
            }
            model.activeHeadWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.ChestWear)
        {
            if (model.activeChestWear != null)
            {
                model.activeChestWear.gameObject.SetActive(false);
            }
            model.activeChestWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftLegWear)
        {
            if (model.activeLeftLegWear != null)
            {
                model.activeLeftLegWear.gameObject.SetActive(false);
            }
            model.activeLeftLegWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightLegWear)
        {
            if (model.activeRightLegWear != null)
            {
                model.activeRightLegWear.gameObject.SetActive(false);
            }
            model.activeRightLegWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftArmWear)
        {
            if (model.activeLeftArmWear != null)
            {
                model.activeLeftArmWear.gameObject.SetActive(false);
            }
            model.activeLeftArmWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightArmWear)
        {
            if (model.activeRightArmWear != null)
            {
                model.activeRightArmWear.gameObject.SetActive(false);
            }
            model.activeRightArmWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftHandWear)
        {
            if (model.activeLeftHandWear != null)
            {
                model.activeLeftHandWear.gameObject.SetActive(false);
            }
            model.activeLeftHandWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightHandWear)
        {
            if (model.activeRightHandWear != null)
            {
                model.activeRightHandWear.gameObject.SetActive(false);
            }
            model.activeRightHandWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.MainHandWeapon)
        {
            if (model.activeMainHandWeapon != null)
            {
                model.activeMainHandWeapon.gameObject.SetActive(false);
            }
            model.activeMainHandWeapon = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.OffHandWeapon)
        {
            if (model.activeOffHandWeapon != null)
            {
                model.activeOffHandWeapon.gameObject.SetActive(false);
            }
            model.activeOffHandWeapon = element;
        }

        // Enable GO
        element.gameObject.SetActive(true);
    }

    // Get View Parts  
    public static UniversalCharacterModelElement GetNextElementInList(List<UniversalCharacterModelElement> list)
    {
        Debug.Log("CharacterModelController.GetNextElementInList() called...");

        // Set up
        UniversalCharacterModelElement elementReturned = null;
        int currentIndex = 0;
        int nextIndex = 0;

        // calculate list size
        int maxIndex = list.Count - 1;

        // prevent negative index
        if(maxIndex < 0)
        {
            maxIndex = 0;
        }
        
        // calculate current index
        foreach (UniversalCharacterModelElement ele in list)
        {
            if (ele.gameObject.activeSelf)
            {
                currentIndex = list.IndexOf(ele);
                Debug.Log("CharacterModelController.GetNextElementInList() calculated that " + ele.gameObject.name +
                    " is at list index " + currentIndex);
                break;
            }
        }

        // if at end of list, go back to index 0
        if(currentIndex + 1 > maxIndex)
        {
            nextIndex = 0;
        }
        else
        {
            nextIndex = currentIndex + 1;
        }
        
        elementReturned = list[nextIndex];

        Debug.Log("CharacterModelController.GetNextElementInList() returning " +
            elementReturned.gameObject.name + " as next indexed element");

        return elementReturned;
    }
    public static UniversalCharacterModelElement GetPreviousElementInList(List<UniversalCharacterModelElement> list)
    {
        Debug.Log("CharacterModelController.GetNextElementInList() called...");

        // Set up
        UniversalCharacterModelElement elementReturned = null;
        int currentIndex = 0;
        int nextIndex = 0;

        // calculate list size
        int maxIndex = list.Count - 1;

        // prevent negative index
        if (maxIndex < 0)
        {
            maxIndex = 0;
        }

        // calculate current index
        foreach (UniversalCharacterModelElement ele in list)
        {
            if (ele.gameObject.activeSelf)
            {
                currentIndex = list.IndexOf(ele);
                Debug.Log("CharacterModelController.GetPreviousElementInList() calculated that " + ele.gameObject.name +
                    " is at list index " + currentIndex);
                break;
            }
        }

        // if at start of list, go to the last index
        if (currentIndex - 1 < 0)
        {
            nextIndex = maxIndex;
        }
        else
        {
            nextIndex = currentIndex - 1;
        }

        elementReturned = list[nextIndex];

        Debug.Log("CharacterModelController.GetPreviousElementInList() returning " +
            elementReturned.gameObject.name + " as next indexed element");

        return elementReturned;
    }

    // Build Models
    public static void BuildCharacterModelFromCharacterPresetData(UniversalCharacterModel model, CharacterPresetData data)
    {
        foreach(ModelElementData elementData in data.activeModelElements)
        {
            // find a gameobject view with a name that matches the data object
            foreach(UniversalCharacterModelElement modelElement in model.allModelElements)
            {
                if(modelElement.gameObject.name == elementData.elementName)
                {
                    // match found, enable and set, then break loop
                    EnableAndSetElementOnModel(model, modelElement);
                    break;
                }
            }
        }
    }
}
