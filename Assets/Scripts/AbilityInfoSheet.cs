using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityInfoSheet : MonoBehaviour
{
    public enum PivotDirection { Upwards, Downwards};
    public enum Orientation { None, South, North};

    [Header("Properties")]
    public AbilityDataSO myData;
    public PivotDirection pivotDirection;

    [Header("Location References")]
    public RectTransform southernPosition;
    public RectTransform northernPosition;

    [Header("Image References")]
    public Image abilityImage;

    [Header("Canvas References")]
    public Canvas canvas;

    [Header("Layout Group References")]
    public VerticalLayoutGroup framesVLG;

    [Header("Parent References")]
    public GameObject visualParent;
    public GameObject talentRequirmentParent;
    public GameObject weaponRequirementParent;

    [Header("Text Component References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI energyCostText;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI talentRequirmentText;
    public TextMeshProUGUI weaponRequirmentText;

    [Header("Transform Component References")]
    public RectTransform transformParent;
    public RectTransform middleFrameTransform;
    public RectTransform allFramesParentTransform;
    public RectTransform allElementsTransform;

    [Header("Ability Type Parent References")]
    public GameObject meleeAttackIconParent;
    public GameObject rangedAttackIconParent;
    public GameObject skillIconParent;
}
