using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalCharacterModel : MonoBehaviour
{
    // Enum Declaration
    public enum ModelRace { None, Human, Orc, Undead, Elf, Goblin};

    [Header("All Model Element References")]
    public List<UniversalCharacterModelElement> allModelElements;
    public List<SpriteMask> allHeadWearSpriteMasks;

    // NEW Properties + Component References
    [Header("Active Body Part References")]
    public UniversalCharacterModelElement activeHead;
    public UniversalCharacterModelElement activeFace;
    public UniversalCharacterModelElement activeLeftLeg;
    public UniversalCharacterModelElement activeRightLeg;
    public UniversalCharacterModelElement activeRightHand;
    public UniversalCharacterModelElement activeRightArm;
    public UniversalCharacterModelElement activeLeftHand;
    public UniversalCharacterModelElement activeLeftArm;
    public UniversalCharacterModelElement activeChest;

    [Header("Active Clothing Part References")]
    public UniversalCharacterModelElement activeHeadWear;
    public UniversalCharacterModelElement activeChestWear;
    public UniversalCharacterModelElement activeLeftLegWear;
    public UniversalCharacterModelElement activeRightLegWear;
    public UniversalCharacterModelElement activeLeftArmWear;
    public UniversalCharacterModelElement activeRightArmWear;
    public UniversalCharacterModelElement activeLeftHandWear;
    public UniversalCharacterModelElement activeRightHandWear;
    public UniversalCharacterModelElement activeMainHandWeapon;
    public UniversalCharacterModelElement activeOffHandWeapon;

    [Header("Head Wear References")]
    public List<UniversalCharacterModelElement> allHeadWear;

    [Header("Chest Wear References")]
    public List<UniversalCharacterModelElement> allChestWear;

    [Header("Leg Wear References")]
    public List<UniversalCharacterModelElement> allLeftLegWear;
    public List<UniversalCharacterModelElement> allRightLegWear;

    [Header("Arm Wear References")]
    public List<UniversalCharacterModelElement> allLeftArmWear;
    public List<UniversalCharacterModelElement> allRightArmWear;
    public List<UniversalCharacterModelElement> allLeftHandWear;
    public List<UniversalCharacterModelElement> allRightHandWear;

    [Header("Weapon References")]
    public List<UniversalCharacterModelElement> allMainHandWeapons;
    public List<UniversalCharacterModelElement> allOffHandWeapons;

    [Header("Human Model References")]
    public List<UniversalCharacterModelElement> humanHeads;
    public List<UniversalCharacterModelElement> humanFaces;
    public UniversalCharacterModelElement humanLeftLeg;
    public UniversalCharacterModelElement humanRightLeg;
    public UniversalCharacterModelElement humanRightHand;
    public UniversalCharacterModelElement humanRightArm;
    public UniversalCharacterModelElement humanLeftHand;
    public UniversalCharacterModelElement humanLeftArm;
    public UniversalCharacterModelElement humanChest;

    [Header("Orc Model References")]
    public List<UniversalCharacterModelElement> orcHeads;
    public List<UniversalCharacterModelElement> orcFaces;
    public UniversalCharacterModelElement orcLeftLeg;
    public UniversalCharacterModelElement orcRightLeg;
    public UniversalCharacterModelElement orcRightHand;
    public UniversalCharacterModelElement orcRightArm;
    public UniversalCharacterModelElement orcLeftHand;
    public UniversalCharacterModelElement orcLeftArm;
    public UniversalCharacterModelElement orcChest;

    [Header("Undead Model References")]
    public List<UniversalCharacterModelElement> undeadHeads;
    public List<UniversalCharacterModelElement> undeadFaces;
    public UniversalCharacterModelElement undeadLeftLeg;
    public UniversalCharacterModelElement undeadRightLeg;
    public UniversalCharacterModelElement undeadRightHand;
    public UniversalCharacterModelElement undeadRightArm;
    public UniversalCharacterModelElement undeadLeftHand;
    public UniversalCharacterModelElement undeadLeftArm;
    public UniversalCharacterModelElement undeadChest;

    [Header("Elf Model References")]
    public List<UniversalCharacterModelElement> elfHeads;
    public List<UniversalCharacterModelElement> elfFaces;
    public UniversalCharacterModelElement elfLeftLeg;
    public UniversalCharacterModelElement elfRightLeg;
    public UniversalCharacterModelElement elfRightHand;
    public UniversalCharacterModelElement elfRightArm;
    public UniversalCharacterModelElement elfLeftHand;
    public UniversalCharacterModelElement elfLeftArm;
    public UniversalCharacterModelElement elfChest;

    // Properties + Component References
    #region
    [Header("Component References")]
    public Animator myAnimator;
    public Transform scalingParent;

    [Header("Properties")]
    public LivingEntity myLivingEntity;
    public ModelRace myModelRace;

    [Header("Left Leg References")]
    public List<GameObject> allLeftLegs;
    public GameObject randomLeftLeg;
    public GameObject paladinLeftLeg;
    public GameObject knightLeftLeg;
    public GameObject mageLeftLeg;
    public GameObject barbarianLeftLeg;
    public GameObject shadowBladeLeftLeg;
    public GameObject rogueLeftLeg;
    public GameObject monkLeftLeg;
    public GameObject priestLeftLeg;
    public GameObject warlockLeftLeg;
    public GameObject marksmanLeftLeg;
    public GameObject wayfarerLeftLeg;
    public GameObject spellBladeLeftLeg;
    public GameObject alchemistLeftLeg;
    public GameObject illusionistLeftLeg;
    public GameObject frostKnightLeftLeg;
    public GameObject shamanLeftLeg;
    public GameObject deathKnightLeftLeg;
    public GameObject bulwarkLeftLeg;
    public GameObject volatileZombieLeftLeg;
    public GameObject skeletonKingLeftLeg;
    public GameObject skeletonSoldierLeftLeg;
    public GameObject skeletonArcherLeftLeg;
    public GameObject skeletonAssassinLeftLeg;
    public GameObject skeletonBarbarianLeftLeg;
    public GameObject skeletonMageLeftLeg;
    public GameObject skeletonWarriorLeftLeg;
    public GameObject skeletonPriestLeftLeg;
    public GameObject skeletonNecromancerLeftLeg;
    public GameObject goblinStabbyLeftLeg;
    public GameObject goblinShootyLeftLeg;
    public GameObject goblinShieldBearerLeftLeg;
    public GameObject goblinWarChiefLeftLeg;    
    public GameObject morkLeftLeg;
    public GameObject fireGolemLeftLeg;
    public GameObject frostGolemLeftLeg;
    public GameObject poisonGolemLeftLeg;
    public GameObject airGolemLeftLeg;
    public GameObject kingLeftLeg;
    public GameObject demonBerserkerLeftLeg;
    public GameObject demonBladeMasterLeftLeg;
    public GameObject demonHellGuardLeftLeg;
    public GameObject darkElfRangerLeftLeg;


    [Header("Right Leg References")]
    public List<GameObject> allRightLegs;
    public GameObject randomRightLeg;
    public GameObject paladinRightLeg;
    public GameObject knightRightLeg;
    public GameObject mageRightLeg;
    public GameObject barbarianRightLeg;
    public GameObject shadowBladeRightLeg;
    public GameObject rogueRightLeg;
    public GameObject monkRightLeg;
    public GameObject priestRightLeg;
    public GameObject warlockRightLeg;
    public GameObject marksmanRightLeg;
    public GameObject wayfarerRightLeg;
    public GameObject spellBladeRightLeg;
    public GameObject alchemistRightLeg;
    public GameObject illusionistRightLeg;
    public GameObject frostKnightRightLeg;
    public GameObject shamanRightLeg;
    public GameObject deathKnightRightLeg;
    public GameObject bulwarkRightLeg;
    public GameObject volatileZombieRightLeg;
    public GameObject skeletonSoldierRightLeg;
    public GameObject skeletonKingRightLeg;    
    public GameObject skeletonArcherRightLeg;
    public GameObject skeletonAssassinRightLeg;
    public GameObject skeletonBarbarianRightLeg;
    public GameObject skeletonMageRightLeg;
    public GameObject skeletonWarriorRightLeg;
    public GameObject skeletonPriestRightLeg;
    public GameObject skeletonNecromancerRightLeg;
    public GameObject goblinStabbyRightLeg;
    public GameObject goblinShootRightLeg;
    public GameObject goblinShieldBearerRightLeg;
    public GameObject goblinWarChiefRightLeg;
    public GameObject morkRightLeg;
    public GameObject fireGolemRightLeg;
    public GameObject frostGolemRightLeg;
    public GameObject poisonGolemRightLeg;
    public GameObject airGolemRightLeg;
    public GameObject kingRightLeg;
    public GameObject demonBerserkerRightLeg;
    public GameObject demonBladeMasterRightLeg;
    public GameObject demonHellGuardRightLeg;
    public GameObject darkElfRangerRightLeg;

    [Header("Head References")]
    public List<GameObject> allHeads;
    public GameObject randomHead;
    public GameObject paladinHead;
    public GameObject knightHead;
    public GameObject mageHead;
    public GameObject barbarianHead;
    public GameObject shadowBladeHead;
    public GameObject rogueHead;
    public GameObject monkHead;
    public GameObject priestHead;
    public GameObject warlockHead;
    public GameObject marksmanHead;
    public GameObject wayfarerHead;
    public GameObject spellBladeHead;
    public GameObject alchemistHead;
    public GameObject illusionistHead;
    public GameObject frostKnightHead;
    public GameObject shamanHead;
    public GameObject deathKnightHead;
    public GameObject bulwarkHead;
    public GameObject volatileZombieHead;
    public GameObject skeletonKingHead;
    public GameObject skeletonSoldierHead;
    public GameObject skeletonArcherHead;
    public GameObject skeletonAssassinHead;
    public GameObject skeletonBarbarianHead;
    public GameObject skeletonMageHead;
    public GameObject skeletonWarriorHead;
    public GameObject skeletonPriestHead;
    public GameObject skeletonNecromancerHead;
    public GameObject goblinStabbyHead;
    public GameObject goblinShootyHead;
    public GameObject goblinShieldBearerHead;
    public GameObject goblinWarChiefHead;
    public GameObject morkHead;
    public GameObject fireGolemHead;
    public GameObject frostGolemHead;
    public GameObject poisonGolemHead;
    public GameObject airGolemHead;
    public GameObject kingHead;
    public GameObject demonBerserkerHead;
    public GameObject demonBladeMasterHead;
    public GameObject demonHellGuardHead;
    public GameObject darkElfRangerHead;

    [Header("Right Hand References")]
    public List<GameObject> allRightHands;
    public GameObject randomRightHand;
    public GameObject paladinRightHand;
    public GameObject knightRightHand;
    public GameObject mageRightHand;
    public GameObject barbarianRightHand;
    public GameObject shadowBladeRightHand;
    public GameObject rogueRightHand;
    public GameObject monkRightHand;
    public GameObject priestRightHand;
    public GameObject warlockRightHand;
    public GameObject marksmanRightHand;
    public GameObject wayfarerRightHand;
    public GameObject spellBladeRightHand;
    public GameObject alchemistRightHand;
    public GameObject illusionistRightHand;
    public GameObject frostKnightRightHand;
    public GameObject shamanRightHand;
    public GameObject deathKnightRightHand;
    public GameObject bulwarkRightHand;
    public GameObject volatileZombieRightHand;
    public GameObject skeletonSoldierRightHand;
    public GameObject skeletonKingRightHand;
    public GameObject skeletonArcherRightHand;
    public GameObject skeletonAssassinRightHand;
    public GameObject skeletonBarbarianRightHand;
    public GameObject skeletonMageRightHand;
    public GameObject skeletonWarriorRightHand;
    public GameObject skeletonPriestRightHand;
    public GameObject skeletonNecromancerRightHand;
    public GameObject goblinStabbyRightHand;
    public GameObject goblinShootyRightHand;
    public GameObject goblinShieldBearerRightHand;
    public GameObject goblinWarChiefRightHand;
    public GameObject morkRightHand;
    public GameObject fireGolemRightHand;
    public GameObject frostGolemRightHand;
    public GameObject poisonGolemRightHand;
    public GameObject airGolemRightHand;
    public GameObject kingRightHand;
    public GameObject demonBerserkerRightHand;
    public GameObject demonBladeMasterRightHand;
    public GameObject demonHellGuardRightHand;
    public GameObject darkElfRangerRightHand;

    [Header("Right Arm References")]
    public List<GameObject> allRightArms;
    public GameObject randomRightArm;
    public GameObject paladinRightArm;
    public GameObject knightRightArm;
    public GameObject mageRightArm;
    public GameObject barbarianRightArm;
    public GameObject shadowBladeRightArm;
    public GameObject rogueRightArm;
    public GameObject monkRightArm;
    public GameObject priestRightArm;
    public GameObject warlockRightArm;
    public GameObject marksmanRightArm;
    public GameObject wayfarerRightArm;
    public GameObject spellBladeRightArm;
    public GameObject alchemistRightArm;
    public GameObject illusionistRightArm;
    public GameObject frostKnightRightArm;
    public GameObject shamanRightArm;
    public GameObject deathKnightRightArm;
    public GameObject bulwarkRightArm;
    public GameObject volatileZombieRightArm;
    public GameObject skeletonSoldierRightArm;
    public GameObject skeletonKingRightArm;
    public GameObject skeletonArcherRightArm;
    public GameObject skeletonAssassinRightArm;
    public GameObject skeletonBarbarianRightArm;
    public GameObject skeletonMageRightArm;
    public GameObject skeletonWarriorRightArm;
    public GameObject skeletonPriestRightArm;
    public GameObject skeletonNecromancerRightArm;
    public GameObject goblinStabbyRightArm;
    public GameObject goblinShootyRightArm;
    public GameObject goblinShieldBearerRightArm;
    public GameObject goblinWarChiefRightArm;
    public GameObject morkRightArm;
    public GameObject fireGolemRightArm;
    public GameObject frostGolemRightArm;
    public GameObject poisonGolemRightArm;
    public GameObject airGolemRightArm;
    public GameObject kingRightArm;
    public GameObject demonBerserkerRightArm;
    public GameObject demonBladeMasterRightArm;
    public GameObject demonHellGuardRightArm;
    public GameObject darkElfRangerRightArm;

    [Header("Left Hand References")]
    public List<GameObject> allLeftHands;
    public GameObject randomLeftHand;
    public GameObject paladinLeftHand;
    public GameObject knightLeftHand;
    public GameObject mageLeftHand;
    public GameObject barbarianLeftHand;
    public GameObject shadowBladeLeftHand;
    public GameObject rogueLeftHand;
    public GameObject monkLeftHand;
    public GameObject priestLeftHand;
    public GameObject warlockLeftHand;
    public GameObject marksmanLeftHand;
    public GameObject wayfarerLeftHand;
    public GameObject spellBladeLeftHand;
    public GameObject alchemistLeftHand;
    public GameObject illusionistLeftHand;
    public GameObject frostKnightLeftHand;
    public GameObject shamanLeftHand;
    public GameObject deathKnightLeftHand;
    public GameObject bulwarkLeftHand;
    public GameObject volatileZombieLeftHand;
    public GameObject skeletonSoldierLeftHand;
    public GameObject skeletonKingLeftHand;
    public GameObject skeletonArcherLeftHand;
    public GameObject skeletonAssassinLeftHand;
    public GameObject skeletonBarbarianLeftHand;
    public GameObject skeletonMageLeftHand;
    public GameObject skeletonWarriorLeftHand;
    public GameObject skeletonPriestLeftHand;
    public GameObject skeletonNecromancerLeftHand;
    public GameObject goblinStabbyLeftHand;
    public GameObject goblinShootyLeftHand;
    public GameObject goblinShieldBearerLeftHand;
    public GameObject goblinWarChiefLeftHand;
    public GameObject morkLeftHand;
    public GameObject fireGolemLeftHand;
    public GameObject frostGolemLeftHand;
    public GameObject poisonGolemLeftHand;
    public GameObject airGolemLeftHand;
    public GameObject kingLeftHand;
    public GameObject demonBerserkerLeftHand;
    public GameObject demonBladeMasterLeftHand;
    public GameObject demonHellGuardLeftHand;
    public GameObject darkElfRangerLeftHand;

    [Header("Left Arm References")]
    public List<GameObject> allLeftArms;
    public GameObject randomLeftArm;
    public GameObject paladinLeftArm;
    public GameObject knightLeftArm;
    public GameObject mageLeftArm;
    public GameObject barbarianLeftArm;
    public GameObject shadowBladeLeftArm;
    public GameObject rogueLeftArm;
    public GameObject monkLeftArm;
    public GameObject priestLeftArm;
    public GameObject warlockLeftArm;
    public GameObject marksmanLeftArm;
    public GameObject wayfarerLeftArm;
    public GameObject spellBladeLeftArm;
    public GameObject alchemistLeftArm;
    public GameObject illusionistLeftArm;
    public GameObject frostKnightLeftArm;
    public GameObject shamanLeftArm;
    public GameObject deathKnightLeftArm;
    public GameObject bulwarkLeftArm;
    public GameObject volatileZombieLeftArm;
    public GameObject skeletonSoldierLeftArm;
    public GameObject skeletonKingLeftArm;
    public GameObject skeletonArcherLeftArm;
    public GameObject skeletonAssassinLeftArm;
    public GameObject skeletonBarbarianLeftArm;
    public GameObject skeletonMageLeftArm;
    public GameObject skeletonWarriorLeftArm;
    public GameObject skeletonPriestLeftArm;
    public GameObject skeletonNecromancerLeftArm;
    public GameObject goblinStabbyLeftArm;
    public GameObject goblinShootyLeftArm;
    public GameObject goblinShieldBearerLeftArm;
    public GameObject goblinWarChiefLeftArm;
    public GameObject morkLeftArm;
    public GameObject fireGolemLeftArm;
    public GameObject frostGolemLeftArm;
    public GameObject poisonGolemLeftArm;
    public GameObject airGolemLeftArm;
    public GameObject kingLeftArm;
    public GameObject demonBerserkerLeftArm;
    public GameObject demonBladeMasterLeftArm;
    public GameObject demonHellGuardLeftArm;
    public GameObject darkElfRangerLeftArm;

    [Header("Chest References")]
    public List<GameObject> allChests;
    public GameObject randomChest;
    public GameObject paladinChest;
    public GameObject knightChest;
    public GameObject mageChest;
    public GameObject barbarianChest;
    public GameObject shadowBladeChest;
    public GameObject rogueChest;
    public GameObject monkChest;
    public GameObject priestChest;
    public GameObject warlockChest;
    public GameObject marksmanChest;
    public GameObject wayfarerChest;
    public GameObject spellBladeChest;
    public GameObject alchemistChest;
    public GameObject illusionistChest;
    public GameObject frostKnightChest;
    public GameObject shamanChest;
    public GameObject deathKnightChest;
    public GameObject bulwarkChest;
    public GameObject volatileZombieChest;
    public GameObject skeletonSoldierChest;
    public GameObject skeletonKingChest;
    public GameObject skeletonArcherChest;
    public GameObject skeletonAssassinChest;
    public GameObject skeletonBarbarianChest;
    public GameObject skeletonMageChest;
    public GameObject skeletonWarriorChest;
    public GameObject skeletonPriestChest;
    public GameObject skeletonNecromancerChest;
    public GameObject goblinStabbyChest;
    public GameObject goblinShootyChest;
    public GameObject goblinShieldBearerChest;
    public GameObject goblinWarChiefChest;
    public GameObject morkChest;
    public GameObject fireGolemChest;
    public GameObject frostGolemChest;
    public GameObject poisonGolemChest;
    public GameObject airGolemChest;
    public GameObject kingChest;
    public GameObject demonBerserkerChest;
    public GameObject demonBladeMasterChest;
    public GameObject demonHellGuardChest;
    public GameObject darkElfRangerChest;

    [Header("Main Hand Weapon References")]
    public GameObject simpleSwordMH;
    public GameObject simpleDaggerMH;
    public GameObject simpleBowMH;
    public GameObject simpleStaffMH;
    public GameObject simpleBattleAxe;

    [Header("Off Hand Weapon References")]
    public GameObject simpleSwordOH;
    public GameObject simpleDaggerOH;
    public GameObject simpleShieldOH;

    #endregion

    private void Start()
    {
        CharacterModelController.AutoSetHeadMaskOrderInLayer(this);
    }


    // Animation Logic
    #region    
    public void SetDeathAnimAsFinished()
    {
        if(myLivingEntity != null)
        {
            myLivingEntity.SetDeathAnimAsFinished();
        }
    }
    public void SetRangedAttackAnimAsFinished()
    {
        if (myLivingEntity != null)
        {
            myLivingEntity.SetRangedAttackAnimAsFinished();
        }
    }
    public void RefreshRangedAttackBool()
    {
        if (myLivingEntity != null)
        {
            myLivingEntity.RefreshRangedAttackBool();
        }
    }
    public void SetBaseAnim()
    {
        myAnimator.SetTrigger("Base");
    }
    public void SetIdleAnim()
    {
        myAnimator.SetTrigger("Idle");
    }

    #endregion
}
