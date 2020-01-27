using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterModelController
{
    // Disable Sprites
    public static void DisableAllViewsInList(List<GameObject> listOfViews)
    {
        foreach(GameObject go in listOfViews)
        {
            go.SetActive(false);
        }
    }

    // Build From Preset Methods
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

        model.simpleShieldOH.SetActive(true);
        model.simpleSwordMH.SetActive(true);
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
    public static void SetUpAsMagePreset(UniversalCharacterModel model)
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
    public static void SetUpAsBarbarianPreset(UniversalCharacterModel model)
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
    public static void SetUpAsSpellBladePreset(UniversalCharacterModel model)
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

        model.simpleShieldOH.SetActive(true);
        model.simpleSwordMH.SetActive(true);
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





    #endregion

    public static void CompletelyDisableAllViews(UniversalCharacterModel model)
    {
        Debug.Log("CharacterModelController.CompletelyDisableAllViews() called...");

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
    }

    public static void BuildModelFromPresetString(UniversalCharacterModel model, string preset)
    {
        Debug.Log("CharacterModelController.BuildModelFromPresetString() called, preset string: " + preset);

        if(preset == "Paladin")
        {
            SetUpAsPaladinPreset(model);
        }
        else if (preset == "Knight")
        {
            SetUpAsKnightPreset(model);
        }
        else if (preset == "Mage")
        {
            SetUpAsMagePreset(model);
        }
        else if (preset == "Barbarian")
        {
            SetUpAsBarbarianPreset(model);
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
        else if (preset == "Marksman")
        {
            SetUpAsMarksmanPreset(model);
        }
        else if (preset == "Wayfarer")
        {
            SetUpAsWayfarerPreset(model);
        }
        else if (preset == "Spell Blade")
        {
            SetUpAsSpellBladePreset(model);
        }
        else if (preset == "Random")
        {
            SetUpAsRandomPreset(model);
        }
    }
}
