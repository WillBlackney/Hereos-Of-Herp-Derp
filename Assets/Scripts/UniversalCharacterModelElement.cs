using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalCharacterModelElement : MonoBehaviour
{
    public enum BodyPartType 
    {None, Head, Face, Chest, RightLeg, LeftLeg, RightArm, 
        RightHand, LeftArm, LeftHand, HeadWear, ChestWear, RightLegWear, 
        LeftLegWear, RightArmWear, RightHandWear, LeftArmWear, LeftHandWear
    };
    public int sortingOrderBonus;
    public BodyPartType bodyPartType;
}
