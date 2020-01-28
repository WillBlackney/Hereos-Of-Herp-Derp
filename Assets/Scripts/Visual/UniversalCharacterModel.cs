using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalCharacterModel : MonoBehaviour
{
    [Header("Properties")]
    public LivingEntity myLivingEntity;

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
    public GameObject volatileZombieLeftLeg;
    public GameObject skeletonArcherLeftLeg;
    public GameObject skeletonAssassinLeftLeg;
    public GameObject skeletonBarbarianLeftLeg;
    public GameObject skeletonMageLeftLeg;
    public GameObject skeletonWarriorLeftLeg;
    public GameObject skeletonPriestLeftLeg;


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
    public GameObject volatileZombieRightLeg;
    public GameObject skeletonArcherRightLeg;
    public GameObject skeletonAssassinRightLeg;
    public GameObject skeletonBarbarianRightLeg;
    public GameObject skeletonMageRightLeg;
    public GameObject skeletonWarriorRightLeg;
    public GameObject skeletonPriestRightLeg;

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
    public GameObject volatileZombieHead;
    public GameObject skeletonArcherHead;
    public GameObject skeletonAssassinHead;
    public GameObject skeletonBarbarianHead;
    public GameObject skeletonMageHead;
    public GameObject skeletonWarriorHead;
    public GameObject skeletonPriestHead;

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
    public GameObject volatileZombieRightHand;
    public GameObject skeletonArcherRightHand;
    public GameObject skeletonAssassinRightHand;
    public GameObject skeletonBarbarianRightHand;
    public GameObject skeletonMageRightHand;
    public GameObject skeletonWarriorRightHand;
    public GameObject skeletonPriestRightHand;

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
    public GameObject volatileZombieRightArm;
    public GameObject skeletonArcherRightArm;
    public GameObject skeletonAssassinRightArm;
    public GameObject skeletonBarbarianRightArm;
    public GameObject skeletonMageRightArm;
    public GameObject skeletonWarriorRightArm;
    public GameObject skeletonPriestRightArm;

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
    public GameObject volatileZombieLeftHand;
    public GameObject skeletonArcherLeftHand;
    public GameObject skeletonAssassinLeftHand;
    public GameObject skeletonBarbarianLeftHand;
    public GameObject skeletonMageLeftHand;
    public GameObject skeletonWarriorLeftHand;
    public GameObject skeletonPriestLeftHand;

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
    public GameObject volatileZombieLeftArm;
    public GameObject skeletonArcherLeftArm;
    public GameObject skeletonAssassinLeftArm;
    public GameObject skeletonBarbarianLeftArm;
    public GameObject skeletonMageLeftArm;
    public GameObject skeletonWarriorLeftArm;
    public GameObject skeletonPriestLeftArm;

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
    public GameObject volatileZombieChest;
    public GameObject skeletonArcherChest;
    public GameObject skeletonAssassinChest;
    public GameObject skeletonBarbarianChest;
    public GameObject skeletonMageChest;
    public GameObject skeletonWarriorChest;
    public GameObject skeletonPriestChest;

    [Header("Main Hand Weapon References")]
    public List<GameObject> allMainHandWeapons;
    public GameObject simpleSwordMH;
    public GameObject simpleDaggerMH;
    public GameObject simpleBowMH;
    public GameObject simpleStaffMH;
    public GameObject simpleBattleAxe;

    [Header("Off Hand Weapon References")]
    public List<GameObject> allOffHandWeapons;
    public GameObject simpleSwordOH;
    public GameObject simpleDaggerOH;
    public GameObject simpleShieldOH;


    public void SetDeathAnimAsFinished()
    {
        if(myLivingEntity != null)
        {
            myLivingEntity.SetDeathAnimAsFinished();
        }
    }
}
