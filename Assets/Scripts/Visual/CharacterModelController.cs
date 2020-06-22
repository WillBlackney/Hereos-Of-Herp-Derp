using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spriter2UnityDX;

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
    public static void BuildModelFromModelClone(UniversalCharacterModel modelToBuild, UniversalCharacterModel modelClonedFrom)
    {
        Debug.Log("CharacterModelController.BuildModelFromModelClone() called...");

        CompletelyDisableAllViews(modelToBuild);
        ClearAllActiveBodyPartReferences(modelToBuild);

        for(int index = 0; index < modelClonedFrom.allModelElements.Count -1; index++)
        {
            if (modelClonedFrom.allModelElements[index].gameObject.activeSelf)
            {
                EnableAndSetElementOnModel(modelToBuild, modelToBuild.allModelElements[index]);
            }
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
            if (ucme.itemsWithMyView.Contains(data.mhWeapon))
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
                if (ucme.itemsWithMyView.Contains(data.ohWeapon))
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
    public static void AutoSetHeadMaskOrderInLayer(UniversalCharacterModel model)
    {
        Debug.Log("CharacterModelController.AutoSetHeadMaskOrderInLayer() called...");

        int headSortOrder = model.GetComponent<EntityRenderer>().SortingOrder + 10;

        foreach(SpriteMask mask in model.allHeadWearSpriteMasks)
        {
            mask.frontSortingOrder = headSortOrder;
            mask.backSortingOrder = headSortOrder - 1;
        }
    }
    public static void ApplyItemDataAppearanceToModel(ItemDataSO item, UniversalCharacterModel model, CharacterItemSlot.SlotType slotType = CharacterItemSlot.SlotType.None)
    {
        Debug.Log("CharacterModelController.ApplyItemDataAppearanceToModel() called, applying look of item '" +
            item.Name + "' to model");

        UniversalCharacterModelElement elementToActivate = null;

        // search specifically for weapons (make sure to correctly enable the weapon view in the correct hand)
        if(slotType != CharacterItemSlot.SlotType.None)
        {
            // check main hand weapons
            if(slotType == CharacterItemSlot.SlotType.MainHand)
            {
                List<UniversalCharacterModelElement> mainHandElements = new List<UniversalCharacterModelElement>();
                foreach(UniversalCharacterModelElement element in model.allModelElements)
                {
                    if(element.bodyPartType == UniversalCharacterModelElement.BodyPartType.MainHandWeapon)
                    {
                        mainHandElements.Add(element);
                    }
                }

                foreach(UniversalCharacterModelElement mainHandElement in mainHandElements)
                {
                    if (mainHandElement.itemsWithMyView.Contains(item))
                    {
                        // Found an element that's view matches the items view
                        elementToActivate = mainHandElement;
                        break;
                    }
                }
            }

            // check off hand weapons
            else if (slotType == CharacterItemSlot.SlotType.OffHand)
            {
                List<UniversalCharacterModelElement> offHandElements = new List<UniversalCharacterModelElement>();
                foreach (UniversalCharacterModelElement element in model.allModelElements)
                {
                    if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.OffHandWeapon)
                    {
                        offHandElements.Add(element);
                    }
                }

                foreach (UniversalCharacterModelElement offHandElement in offHandElements)
                {
                    if (offHandElement.itemsWithMyView.Contains(item))
                    {
                        // Found an element that's view matches the items view
                        elementToActivate = offHandElement;
                        break;
                    }
                }
            }
        }

        // check non weapon model elements
        else
        {
            // Search through every element on the model, find the element that matches the item
            foreach (UniversalCharacterModelElement element in model.allModelElements)
            {
                if (element.itemsWithMyView.Contains(item))
                {
                    // Found an element that's view matches the items view
                    elementToActivate = element;
                    break;
                }
            }
        }
        

        if(elementToActivate != null)
        {
            Debug.Log("CharacterModelController.ApplyItemDataAppearanceToModel() found a matching UCM element for item " + item.Name +
                ", enabling UCM element: " + elementToActivate.gameObject.name.ToString());
            EnableAndSetElementOnModel(model, elementToActivate);
        }
        else
        {
            Debug.Log("CharacterModelController.ApplyItemDataAppearanceToModel() did not find a UCM that matches the item " + item.Name);
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
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Body parts
        EnableAndSetElementOnModel(model, model.undeadHeads[4]);
        EnableAndSetElementOnModel(model, model.undeadFaces[6]);
        EnableAndSetElementOnModel(model, "Volatile_Zombie_Right_Hand");
        EnableAndSetElementOnModel(model, "Volatile_Zombie_Left_Hand");       
        EnableAndSetElementOnModel(model, "Volatile_Zombie_Right_Leg");
        EnableAndSetElementOnModel(model, "Volatile_Zombie_Left_Leg");
        EnableAndSetElementOnModel(model, "Peasant_Cloth_Right_Arm_Wear");
        EnableAndSetElementOnModel(model, "Peasant_Cloth_Left_Arm_Wear");
        EnableAndSetElementOnModel(model, "Peasant_Cloth_Chest_Wear");
    }
    public static void SetUpAsDarkElfRangerPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Body parts
        EnableAndSetElementOnModel(model, "Dark_Elf_Right_Hand");
        EnableAndSetElementOnModel(model, "Dark_Elf_Left_Hand");
        EnableAndSetElementOnModel(model, "Dark_Elf_Right_Leg");
        EnableAndSetElementOnModel(model, "Dark_Elf_Left_Leg");
        EnableAndSetElementOnModel(model, "Grey_Archer_Vest_Chest_Wear");
        EnableAndSetElementOnModel(model, "Left_Hand_Recurve_Bow_1");
        EnableAndSetElementOnModel(model, "Elf_Face_4");
        EnableAndSetElementOnModel(model, "Elf_Head_5");
    }
    public static void SetUpAsSkeletonArcherPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

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

        // Clothing + Weapon parts
        EnableAndSetElementOnModel(model, "Archer_Helmet_Head_Wear");
        EnableAndSetElementOnModel(model, "Marksman_Chest_Wear");
        EnableAndSetElementOnModel(model, "Left_Hand_Simple_Bow");
    }
    public static void SetUpAsSkeletonAssassinPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

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

        // Clothing + Weapon parts
        EnableAndSetElementOnModel(model, "Red_Face_Mask_Head_Wear");
        EnableAndSetElementOnModel(model, model.allChestWear[2]);
        EnableAndSetElementOnModel(model, model.allMainHandWeapons[1]);
        EnableAndSetElementOnModel(model, model.allOffHandWeapons[2]);
    }
    public static void SetUpAsSkeletonWarriorPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

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

        // Clothing + Weapon parts
        EnableAndSetElementOnModel(model, "Silver_Red_Helmet_Head_Wear");
        EnableAndSetElementOnModel(model, model.allMainHandWeapons[0]);
        EnableAndSetElementOnModel(model, model.allOffHandWeapons[0]);
    }
    public static void SetUpAsSkeletonBarbarianPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

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

        // Clothing + Weapon parts
        EnableAndSetElementOnModel(model, "Berserker_Mask_Head_Wear");
        EnableAndSetElementOnModel(model, model.allMainHandWeapons[4]);
    }
    public static void SetUpAsSkeletonPriestPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

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

        // Clothing + Weapon parts
        EnableAndSetElementOnModel(model, "Priest_Chest_Wear");
        EnableAndSetElementOnModel(model, "White_Head_Band_Head_Wear");
        EnableAndSetElementOnModel(model, model.allMainHandWeapons[3]);
    }
    public static void SetUpAsSkeletonMagePreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

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

        // Clothing + Weapon parts
        EnableAndSetElementOnModel(model, "Arcanist_Head_Wear");
        EnableAndSetElementOnModel(model, "Arcanist_Chest_Wear");
        EnableAndSetElementOnModel(model, model.allMainHandWeapons[3]);
    }
    public static void SetUpAsSkeletonNecromancerPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Clothing + Weapon parts
        EnableAndSetElementOnModel(model, "Orc_Face_6");
        EnableAndSetElementOnModel(model, "Skeleton_Necromancer_Head");
        EnableAndSetElementOnModel(model, "Black_Necromancer_Right_Arm_Wear");
        EnableAndSetElementOnModel(model, "Black_Necromancer_Left_Arm_Wear");
        EnableAndSetElementOnModel(model, "Black_Necromancer_Chest_Wear");
        EnableAndSetElementOnModel(model, "Black_Necromancer_Right_Leg_Wear");
        EnableAndSetElementOnModel(model, "Black_Necromancer_Left_Leg_Wear");
        EnableAndSetElementOnModel(model, "Black_Necromancer_Right_Hand");
        EnableAndSetElementOnModel(model, "Black_Necromancer_Left_Hand");

        EnableAndSetElementOnModel(model, "Left_Hand_Green_Staff_1");
    }
    public static void SetUpAsSkeletonKingPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Clothing + Weapon parts
        EnableAndSetElementOnModel(model, "Left_Hand_Graphite_Gladius");
        EnableAndSetElementOnModel(model, "Black_Gold_Plate_Shield");
        EnableAndSetElementOnModel(model, "Black_Gold_Plate_Right_Hand");
        EnableAndSetElementOnModel(model, "Black_Gold_Plate_Right_Arm_Wear");
        EnableAndSetElementOnModel(model, "Black_Gold_Plate_Left_Hand");
        EnableAndSetElementOnModel(model, "Black_Gold_Plate_Left_Arm_Wear");
        EnableAndSetElementOnModel(model, "Black_Gold_Plate_Chest_Wear");
        EnableAndSetElementOnModel(model, "Black_Gold_Plate_Right_Leg_Wear");
        EnableAndSetElementOnModel(model, "Black_Gold_Plate_Left_Leg_Wear");
        EnableAndSetElementOnModel(model, "Undead_Head_1");
        EnableAndSetElementOnModel(model, "Undead_Face_10");
    }
    public static void SetUpAsSkeletonSoldierPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Clothing + Weapon parts
        EnableAndSetElementOnModel(model, "Death_Knight_Head_Wear");
        EnableAndSetElementOnModel(model, "Death_Knight_Shield");
        EnableAndSetElementOnModel(model, "Death_Knight_Right_Arm_Wear");
        EnableAndSetElementOnModel(model, "Death_Knight_Left_Arm_Wear");
        EnableAndSetElementOnModel(model, "Death_Knight_Chest_Wear");
        EnableAndSetElementOnModel(model, "Undead_Head_1");
        EnableAndSetElementOnModel(model, "Undead_Face_2");
        EnableAndSetElementOnModel(model, "Undead_Right_Hand");
        EnableAndSetElementOnModel(model, "Undead_Left_Hand");
        EnableAndSetElementOnModel(model, "Undead_Right_Leg");
        EnableAndSetElementOnModel(model, "Undead_Left_Leg");
        EnableAndSetElementOnModel(model, "Left_Hand_Simple_Sword");
    }
    public static void SetUpAsGoblinStabbyPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Body parts
        EnableAndSetElementOnModel(model, model.orcHeads[1]);
        EnableAndSetElementOnModel(model, model.orcFaces[0]);
        EnableAndSetElementOnModel(model, model.orcLeftLeg);
        EnableAndSetElementOnModel(model, model.orcRightLeg);
        EnableAndSetElementOnModel(model, model.orcRightHand);
        EnableAndSetElementOnModel(model, model.orcRightArm);
        EnableAndSetElementOnModel(model, model.orcLeftHand);
        EnableAndSetElementOnModel(model, model.orcLeftArm);
        EnableAndSetElementOnModel(model, model.orcChest);

        // Clothing + Weapon parts
        EnableAndSetElementOnModel(model, model.allMainHandWeapons[1]);
    }
    public static void SetUpAsGoblinWarChiefPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Body parts
        EnableAndSetElementOnModel(model, model.orcHeads[0]);
        EnableAndSetElementOnModel(model, model.orcFaces[1]);
        EnableAndSetElementOnModel(model, model.orcLeftLeg);
        EnableAndSetElementOnModel(model, model.orcRightLeg);
        EnableAndSetElementOnModel(model, model.orcRightHand);
        EnableAndSetElementOnModel(model, model.orcRightArm);
        EnableAndSetElementOnModel(model, model.orcLeftHand);
        EnableAndSetElementOnModel(model, model.orcLeftArm);
        EnableAndSetElementOnModel(model, model.orcChest);

        // Clothing + Weapon parts
        EnableAndSetElementOnModel(model, model.allMainHandWeapons[4]);
        EnableAndSetElementOnModel(model, "War_Chief_Chest_Wear");
        EnableAndSetElementOnModel(model, "War_Chief_Left_Arm_Wear");
        EnableAndSetElementOnModel(model, "War_Chief_Right_Arm_Wear");
    }
    public static void SetUpAsGoblinShootyPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Body parts
        EnableAndSetElementOnModel(model, model.orcHeads[1]);
        EnableAndSetElementOnModel(model, model.orcFaces[0]);
        EnableAndSetElementOnModel(model, model.orcLeftLeg);
        EnableAndSetElementOnModel(model, model.orcRightLeg);
        EnableAndSetElementOnModel(model, model.orcRightHand);
        EnableAndSetElementOnModel(model, model.orcRightArm);
        EnableAndSetElementOnModel(model, model.orcLeftHand);
        EnableAndSetElementOnModel(model, model.orcLeftArm);
        EnableAndSetElementOnModel(model, model.orcChest);

        // Clothing + Weapon parts
        EnableAndSetElementOnModel(model, model.allMainHandWeapons[2]);
        EnableAndSetElementOnModel(model, "Archer_Helmet_Head_Wear");
    }
    public static void SetUpAsGoblinShieldBearerPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Body parts
        EnableAndSetElementOnModel(model, model.orcHeads[1]);
        EnableAndSetElementOnModel(model, model.orcFaces[0]);
        EnableAndSetElementOnModel(model, model.orcLeftLeg);
        EnableAndSetElementOnModel(model, model.orcRightLeg);
        EnableAndSetElementOnModel(model, model.orcRightHand);
        EnableAndSetElementOnModel(model, model.orcRightArm);
        EnableAndSetElementOnModel(model, model.orcLeftHand);
        EnableAndSetElementOnModel(model, model.orcLeftArm);
        EnableAndSetElementOnModel(model, model.orcChest);

        // Clothing + Weapon parts
        EnableAndSetElementOnModel(model, model.allMainHandWeapons[0]);
        EnableAndSetElementOnModel(model, model.allOffHandWeapons[0]);
        EnableAndSetElementOnModel(model, "Death_Knight_Head_Wear");
    }
    public static void SetUpAsMorkPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Body parts
        EnableAndSetElementOnModel(model, "Goblin_Head_1");
        EnableAndSetElementOnModel(model, "Goblin_Right_Hand");
        EnableAndSetElementOnModel(model, "Goblin_Right_Arm");
        EnableAndSetElementOnModel(model, "Goblin_Left_Hand");
        EnableAndSetElementOnModel(model, "Goblin_Left_Arm");
        EnableAndSetElementOnModel(model, "Goblin_Chest");
        EnableAndSetElementOnModel(model, "Goblin_Right_Leg");
        EnableAndSetElementOnModel(model, "Goblin_Left_Leg");
        EnableAndSetElementOnModel(model, "Left_Hand_Wooden_Spiked_Club");
    }
    public static void SetUpAsFireGolemPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Body parts
        EnableAndSetElementOnModel(model, "Fire_Golem_Left_Leg");
        EnableAndSetElementOnModel(model, "Fire_Golem_Right_Leg");
        EnableAndSetElementOnModel(model, "Fire_Golem_Head");
        EnableAndSetElementOnModel(model, "Fire_Golem_Face");
        EnableAndSetElementOnModel(model, "Fire_Golem_Left_Hand");
        EnableAndSetElementOnModel(model, "Fire_Golem_Left_Arm");
        EnableAndSetElementOnModel(model, "Fire_Golem_Right_Hand");
        EnableAndSetElementOnModel(model, "Fire_Golem_Right_Arm");
        EnableAndSetElementOnModel(model, "Fire_Golem_Chest");
    }
    public static void SetUpAsAirGolemPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Body parts
        EnableAndSetElementOnModel(model, "Air_Golem_Left_Leg");
        EnableAndSetElementOnModel(model, "Air_Golem_Right_Leg");
        EnableAndSetElementOnModel(model, "Air_Golem_Head");
        EnableAndSetElementOnModel(model, "Air_Golem_Face");
        EnableAndSetElementOnModel(model, "Air_Golem_Left_Hand");
        EnableAndSetElementOnModel(model, "Air_Golem_Left_Arm");
        EnableAndSetElementOnModel(model, "Air_Golem_Right_Hand");
        EnableAndSetElementOnModel(model, "Air_Golem_Right_Arm");
        EnableAndSetElementOnModel(model, "Air_Golem_Chest");
    }
    public static void SetUpAsPoisonGolemPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Body parts
        EnableAndSetElementOnModel(model, "Poison_Golem_Left_Leg");
        EnableAndSetElementOnModel(model, "Poison_Golem_Right_Leg");
        EnableAndSetElementOnModel(model, "Poison_Golem_Head");
        EnableAndSetElementOnModel(model, "Poison_Golem_Face");
        EnableAndSetElementOnModel(model, "Poison_Golem_Left_Hand");
        EnableAndSetElementOnModel(model, "Poison_Golem_Left_Arm");
        EnableAndSetElementOnModel(model, "Poison_Golem_Right_Hand");
        EnableAndSetElementOnModel(model, "Poison_Golem_Right_Arm");
        EnableAndSetElementOnModel(model, "Poison_Golem_Chest");
    }
    public static void SetUpAsFrostGolemPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Body parts
        EnableAndSetElementOnModel(model, "Frost_Golem_Left_Leg");
        EnableAndSetElementOnModel(model, "Frost_Golem_Right_Leg");
        EnableAndSetElementOnModel(model, "Frost_Golem_Head");
        EnableAndSetElementOnModel(model, "Frost_Golem_Face");
        EnableAndSetElementOnModel(model, "Frost_Golem_Left_Hand");
        EnableAndSetElementOnModel(model, "Frost_Golem_Left_Arm");
        EnableAndSetElementOnModel(model, "Frost_Golem_Right_Hand");
        EnableAndSetElementOnModel(model, "Frost_Golem_Right_Arm");
        EnableAndSetElementOnModel(model, "Frost_Golem_Chest");

    }
    public static void SetUpAsDemonBerserkerPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Body parts
        EnableAndSetElementOnModel(model, "Demon_Head_1");
        EnableAndSetElementOnModel(model, "Demon_Right_Hand_1");
        EnableAndSetElementOnModel(model, "Demon_Right_Arm_1");
        EnableAndSetElementOnModel(model, "Demon_Left_Hand_1");
        EnableAndSetElementOnModel(model, "Demon_Left_Arm_1");
        EnableAndSetElementOnModel(model, "Demon_Chest_1");
        EnableAndSetElementOnModel(model, "Demon_Right_Leg_1");
        EnableAndSetElementOnModel(model, "Demon_Left_Leg_1");
        EnableAndSetElementOnModel(model, "Orc_Face_5");
        EnableAndSetElementOnModel(model, "Left_Hand_Serated_Sword_2H");

    }
    public static void SetUpAsDemonBladeMasterPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Body parts
        EnableAndSetElementOnModel(model, "Demon_Head_2");
        EnableAndSetElementOnModel(model, "Demon_Right_Hand_2");
        EnableAndSetElementOnModel(model, "Demon_Right_Arm_2");
        EnableAndSetElementOnModel(model, "Demon_Left_Hand_2");
        EnableAndSetElementOnModel(model, "Demon_Left_Arm_2");
        EnableAndSetElementOnModel(model, "Demon_Chest_2");
        EnableAndSetElementOnModel(model, "Demon_Right_Leg_2");
        EnableAndSetElementOnModel(model, "Demon_Left_Leg_2");
        EnableAndSetElementOnModel(model, "Orc_Face_5");
        EnableAndSetElementOnModel(model, "Right_Hand_Serated_Sword");
        EnableAndSetElementOnModel(model, "Left_Hand_Serated_Sword");
    }
    public static void SetUpAsDemonHellGuardPreset(UniversalCharacterModel model)
    {
        DisableAllActiveElementViews(model);
        ClearAllActiveBodyPartReferences(model);

        // Body parts
        EnableAndSetElementOnModel(model, "Demon_Head_1");
        EnableAndSetElementOnModel(model, "Demon_Right_Hand_1");
        EnableAndSetElementOnModel(model, "Demon_Left_Hand_1");
        EnableAndSetElementOnModel(model, "Hell_Guard_Head_Wear");
        EnableAndSetElementOnModel(model, "Right_Hand_Hell_Guard_Shield");
        EnableAndSetElementOnModel(model, "Left_Hand_Serated_Sword");
        EnableAndSetElementOnModel(model, "Hell_Guard_Right_Arm_Wear");
        EnableAndSetElementOnModel(model, "Hell_Guard_Left_Arm_Wear");
        EnableAndSetElementOnModel(model, "Orc_Face_5");
        EnableAndSetElementOnModel(model, "Hell_Guard_Chest_Wear");

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
                DisableAndClearElementOnModel(model, model.activeChest);
            }            
            model.activeChest = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.Head)
        {
            if (model.activeHead != null)
            {
                DisableAndClearElementOnModel(model, model.activeHead);
            }
            model.activeHead = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.Face)
        {
            if (model.activeFace != null)
            {
                DisableAndClearElementOnModel(model, model.activeFace);
            }
            model.activeFace = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightArm)
        {
            if (model.activeRightArm != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightArm);
            }
            model.activeRightArm = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightHand)
        {
            if (model.activeRightHand != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightHand);
            }
            model.activeRightHand = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftArm)
        {
            if (model.activeLeftArm != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftArm);
            }
            model.activeLeftArm = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftHand)
        {
            if (model.activeLeftHand != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftHand);
            }
            model.activeLeftHand = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightLeg)
        {
            if (model.activeRightLeg != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightLeg);
            }
            model.activeRightLeg = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftLeg)
        {
            if (model.activeLeftLeg != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftLeg);
            }
            model.activeLeftLeg = element;
        }

        // Set Active Weapons + Clothing Reference
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.HeadWear)
        {
            if (model.activeHeadWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeHeadWear);
            }
            model.activeHeadWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.ChestWear)
        {
            if (model.activeChestWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeChestWear);
            }
            model.activeChestWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftLegWear)
        {
            if (model.activeLeftLegWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftLegWear);
            }
            model.activeLeftLegWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightLegWear)
        {
            if (model.activeRightLegWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightLegWear);
            }
            model.activeRightLegWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftArmWear)
        {
            if (model.activeLeftArmWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftArmWear);
            }
            model.activeLeftArmWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightArmWear)
        {
            if (model.activeRightArmWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightArmWear);
            }
            model.activeRightArmWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftHandWear)
        {
            if (model.activeLeftHandWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftHandWear);
            }
            model.activeLeftHandWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightHandWear)
        {
            if (model.activeRightHandWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightHandWear);
            }
            model.activeRightHandWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.MainHandWeapon)
        {
            if (model.activeMainHandWeapon != null)
            {
                DisableAndClearElementOnModel(model, model.activeMainHandWeapon);
            }
            model.activeMainHandWeapon = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.OffHandWeapon)
        {
            if (model.activeOffHandWeapon != null)
            {
                DisableAndClearElementOnModel(model, model.activeOffHandWeapon);
            }
            model.activeOffHandWeapon = element;
        }

        // Enable GO
        element.gameObject.SetActive(true);

        // repeat for any connected elements (e.g. active arm/hand sprites that are connected to the chest piece
        foreach(UniversalCharacterModelElement connectedElement in element.connectedElements)
        {
            EnableAndSetElementOnModel(model, connectedElement);
        }
    }
    public static void EnableAndSetElementOnModel(UniversalCharacterModel model, string elementName)
    {
        Debug.Log("CharacterModelController.EnableAndSetElementOnModel() called, enabling " +
            elementName + " GO");

        UniversalCharacterModelElement element = null;

        // find element first
        foreach(UniversalCharacterModelElement modelElement in model.allModelElements)
        {
            if(modelElement.gameObject.name == elementName)
            {
                element = modelElement;
                break;
            }
        }

        if(element == null)
        {
            Debug.Log("CharacterModelController.EnableAndSetElementOnModel() could not find an model element with the name "
            + elementName + ", cancelling element enabling...");
            return;
        }

        // Set Active Body Part Reference
        if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.Chest)
        {
            if (model.activeChest != null)
            {
                DisableAndClearElementOnModel(model, model.activeChest);
            }
            model.activeChest = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.Head)
        {
            if (model.activeHead != null)
            {
                DisableAndClearElementOnModel(model, model.activeHead);
            }
            model.activeHead = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.Face)
        {
            if (model.activeFace != null)
            {
                DisableAndClearElementOnModel(model, model.activeFace);
            }
            model.activeFace = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightArm)
        {
            if (model.activeRightArm != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightArm);
            }
            model.activeRightArm = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightHand)
        {
            if (model.activeRightHand != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightHand);
            }
            model.activeRightHand = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftArm)
        {
            if (model.activeLeftArm != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftArm);
            }
            model.activeLeftArm = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftHand)
        {
            if (model.activeLeftHand != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftHand);
            }
            model.activeLeftHand = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightLeg)
        {
            if (model.activeRightLeg != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightLeg);
            }
            model.activeRightLeg = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftLeg)
        {
            if (model.activeLeftLeg != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftLeg);
            }
            model.activeLeftLeg = element;
        }

        // Set Active Weapons + Clothing Reference
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.HeadWear)
        {
            if (model.activeHeadWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeHeadWear);
            }
            model.activeHeadWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.ChestWear)
        {
            if (model.activeChestWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeChestWear);
            }
            model.activeChestWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftLegWear)
        {
            if (model.activeLeftLegWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftLegWear);
            }
            model.activeLeftLegWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightLegWear)
        {
            if (model.activeRightLegWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightLegWear);
            }
            model.activeRightLegWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftArmWear)
        {
            if (model.activeLeftArmWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftArmWear);
            }
            model.activeLeftArmWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightArmWear)
        {
            if (model.activeRightArmWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightArmWear);
            }
            model.activeRightArmWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftHandWear)
        {
            if (model.activeLeftHandWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeLeftHandWear);
            }
            model.activeLeftHandWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightHandWear)
        {
            if (model.activeRightHandWear != null)
            {
                DisableAndClearElementOnModel(model, model.activeRightHandWear);
            }
            model.activeRightHandWear = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.MainHandWeapon)
        {
            if (model.activeMainHandWeapon != null)
            {
                DisableAndClearElementOnModel(model, model.activeMainHandWeapon);
            }
            model.activeMainHandWeapon = element;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.OffHandWeapon)
        {
            if (model.activeOffHandWeapon != null)
            {
                DisableAndClearElementOnModel(model, model.activeOffHandWeapon);
            }
            model.activeOffHandWeapon = element;
        }

        // Enable GO
        element.gameObject.SetActive(true);

        // repeat for any connected elements (e.g. active arm/hand sprites that are connected to the chest piece
        foreach (UniversalCharacterModelElement connectedElement in element.connectedElements)
        {
            EnableAndSetElementOnModel(model, connectedElement);
        }
    }
    public static void DisableAndClearElementOnModel(UniversalCharacterModel model, UniversalCharacterModelElement element)
    {
        Debug.Log("CharacterModelController.DisableAndClearElementOnModel() called, enabling " +
            element.gameObject.name + " GO");

        // disable view
        element.gameObject.SetActive(false);

        // Clear reference on model
        if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.Chest)
        {
            model.activeChest = null;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.Head)
        {
            model.activeHead = null;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.Face)
        {
            model.activeFace = null;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightArm)
        {
            model.activeRightArm = null;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightHand)
        {
            model.activeRightHand = null;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftArm)
        {
            model.activeLeftArm = null;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftHand)
        {
            model.activeLeftHand = null;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightLeg)
        {
            model.activeRightLeg = null;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftLeg)
        {
            model.activeLeftLeg = null;
        }

        // Set Active Weapons + Clothing Reference
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.HeadWear)
        {
            model.activeHeadWear = null;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.ChestWear)
        {
            model.activeChestWear = null;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftLegWear)
        {
            model.activeLeftLegWear = null;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightLegWear)
        {
            model.activeRightLegWear = null;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftArmWear)
        {
            model.activeLeftArmWear = null;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightArmWear)
        {
            model.activeRightArmWear = null;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.LeftHandWear)
        {
            model.activeLeftHandWear = null;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.RightHandWear)
        {
            model.activeRightHandWear = null;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.MainHandWeapon)
        {
            model.activeMainHandWeapon = null;
        }
        else if (element.bodyPartType == UniversalCharacterModelElement.BodyPartType.OffHandWeapon)
        {
            model.activeOffHandWeapon = null;
        }

        // repeat for any connected elements (e.g. active arm/hand sprites that are connected to the chest piece
        foreach (UniversalCharacterModelElement connectedElement in element.connectedElements)
        {
            DisableAndClearElementOnModel(model, connectedElement);
        }

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

}
