using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityInfoSheetController : MonoBehaviour
{
    // Properties
    #region
    public float baseFadeInSpeed;
    #endregion

    // Singleton Pattern
    #region
    public static AbilityInfoSheetController Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
      
    }
    #endregion

    // Initialization + Setup
    #region
    public void BuildSheetFromData(AbilityInfoSheet sheet, AbilityDataSO data, AbilityInfoSheet.PivotDirection pivotDirection)
    {
        Debug.Log("AbilityInfoSheetController.BuildSheetFromData() called...");

        sheet.myData = data;
        //SetPivotDirection(sheet, pivotDirection);
        BuildAllTextValueViewsFromData(sheet, data);
        BuildAbilityTypeIconViewsFromData(sheet, data);
        BuildAllRequirementPanelsFromData(sheet, data);

        RefreshAllLayoutGroups(sheet);
    }
    public void BuildAbilityTypeIconViewsFromData(AbilityInfoSheet sheet, AbilityDataSO data)
    {
        Debug.Log("AbilityInfoSheetController.BuildAbilityTypeIconViewsFromData() called...");

        sheet.meleeAttackIconParent.SetActive(false);
        sheet.rangedAttackIconParent.SetActive(false);
        sheet.skillIconParent.SetActive(false);

        if (data.abilityType == AbilityDataSO.AbilityType.MeleeAttack)
        {
            sheet.meleeAttackIconParent.SetActive(true);
        }
        else if (data.abilityType == AbilityDataSO.AbilityType.RangedAttack)
        {
            sheet.rangedAttackIconParent.SetActive(true);
        }
        else if (data.abilityType == AbilityDataSO.AbilityType.Skill)
        {
            sheet.skillIconParent.SetActive(true);
        }
    }
    public void BuildAllTextValueViewsFromData(AbilityInfoSheet sheet, AbilityDataSO data)
    {
        Debug.Log("AbilityInfoSheetController.BuildAllTextValueViewsFromData() called...");

        // Set up text files
        sheet.nameText.text = data.abilityName;
        sheet.cooldownText.text = data.baseCooldownTime.ToString();
        sheet.rangeText.text = data.range.ToString();
        sheet.energyCostText.text = data.energyCost.ToString();
        TextLogic.SetAbilityDescriptionText(data, sheet.descriptionText);
        sheet.abilityImage.sprite = data.sprite;
    }
    public void BuildAllRequirementPanelsFromData(AbilityInfoSheet sheet, AbilityDataSO data)
    {
        Debug.Log("AbilityInfoSheetController.BuildAllRequirementPanelsFromData() called...");

        BuildTalentRequirementPanelFromData(sheet, data);
        BuildWeaponRequirementPanelFromData(sheet, data);
    }
    private void BuildTalentRequirementPanelFromData(AbilityInfoSheet sheet, AbilityDataSO data)
    {
        Debug.Log("AbilityInfoSheetController.BuildTalentRequirementPanelFromData() called...");

        // Disable panel view on refresh
        sheet.talentRequirmentParent.SetActive(false);

        string talentTypeString = "";
        string tierString = "";
        bool activated = false;

        if(data.abilitySchool != AbilityDataSO.AbilitySchool.None)
        {
            activated = true;

            // convert talent type enum to string
            talentTypeString = data.abilitySchool.ToString();
            tierString = data.tier.ToString();
        }

        if (activated)
        {
            // Enable panel view
            sheet.talentRequirmentParent.SetActive(true);

            // Set text value
            sheet.talentRequirmentText.text = "Requires: " + talentTypeString + " " + tierString;
        }
    }
    private void BuildWeaponRequirementPanelFromData(AbilityInfoSheet sheet, AbilityDataSO data)
    {
        Debug.Log("AbilityInfoSheetController.BuildWeaponRequirementPanelFromData() called...");

        // Disable panel view on refresh
        sheet.weaponRequirementParent.SetActive(false);

        string weaponTypeString = "";
        bool activated = false;

        if (data.requiresMeleeWeapon)
        {
            activated = true;
            weaponTypeString = "Melee Weapon";
        }
        else if (data.requiresRangedWeapon)
        {
            activated = true;
            weaponTypeString = "Ranged Weapon";
        }
        else if (data.requiresShield)
        {
            activated = true;
            weaponTypeString = "Shield";
        }

        if (activated)
        {
            // Enable panel view
            sheet.weaponRequirementParent.SetActive(true);

            // Set text value
            sheet.weaponRequirmentText.text = "Requires: " + weaponTypeString;
        }
    }
    public void SetUpOrientationsAsMenuAbility(AbilityInfoSheet sheet, GameObject menuAbilityParent)
    {
        // Debug.Log("AbilityInfoSheetController.SetUpOrientationsAsMenuAbility() called...");
        //ResetCanvasProperties(sheet);
        SetMainTransformPosition(sheet, menuAbilityParent.transform.position);
        SetOrientationFromTransformParent(sheet, AbilityInfoSheet.Orientation.North);
    }
    #endregion

    // Modify Pivot + Transform + Positioning
    #region
    public void SetPivotDirection(AbilityInfoSheet sheet, AbilityInfoSheet.PivotDirection pivotDirection)
    {
        Debug.Log("AbilityInfoSheetController.SetPivotDirection() called...");

        if (pivotDirection == AbilityInfoSheet.PivotDirection.Downwards)
        {
            SetDownwardsPivot(sheet);
        }
        else if(pivotDirection == AbilityInfoSheet.PivotDirection.Upwards)
        {
            SetUpwardsPivot(sheet);
        }
    }
    private void SetDownwardsPivot(AbilityInfoSheet sheet)
    {
        Debug.Log("AbilityInfoSheetController.SetDownwardsPivot() called...");

        // set pivot on all sheet elements
        sheet.allElementsTransform.pivot = new Vector3(1, 1, 1);

        // set pivot on outer frame
        sheet.allFramesParentTransform.pivot = new Vector3(1, 1, 1);

        // set VLG alignment
        sheet.framesVLG.childAlignment = TextAnchor.UpperCenter;

        // stash pivot state
        sheet.pivotDirection = AbilityInfoSheet.PivotDirection.Downwards;
    }
    private void SetUpwardsPivot(AbilityInfoSheet sheet)
    {
        Debug.Log("AbilityInfoSheetController.SetUpwardsPivot() called...");

        // set pivot on all sheet elements
        sheet.allElementsTransform.pivot = new Vector3(0, 0, 1);

        // set pivot on outer frame
        sheet.allFramesParentTransform.pivot = new Vector3(0, 0, 1);

        // set VLG alignment
        sheet.framesVLG.childAlignment = TextAnchor.LowerCenter;

        // stash pivot state
        sheet.pivotDirection = AbilityInfoSheet.PivotDirection.Upwards;
    }
    public void SetMainTransformPosition(AbilityInfoSheet sheet, Vector2 newPosition)
    {
        // this method moves the master rect transform on top of a destination gameobject
        // use this to place on top, say, an ability button, making there positions matching
        Debug.Log("AbilityInfoSheetController.SetMainTransformPosition() called...");
        Debug.Log("Sheet main transform previous position: " + sheet.transformParent.position.x.ToString() + ", " 
             + sheet.transformParent.position.y.ToString() + ", " + sheet.transformParent.position.z.ToString());

         sheet.transformParent.position = newPosition;

        Debug.Log("Sheet main transform new position: " + sheet.transformParent.position.x.ToString() + ", "
            + sheet.transformParent.position.y.ToString() + ", " + sheet.transformParent.position.z.ToString());
    }
    public void SetOrientationFromTransformParent(AbilityInfoSheet sheet, AbilityInfoSheet.Orientation orientation)
    {
        // method is used after SetMainTransformPosition(), this sets the cards orientation from the master transform
        // and thus is target game object
        // e.g. set the orientation to 'North' to make the sheet appear ABOVE ability buttons on ability bars and on the menu screen, OR
        // set the orientation to 'East' to make the sheet appear to the right of an ability tome in the inventory

        Debug.Log("AbilityInfoSheetController.SetOrientationFromTransformParent() called...");

        if (orientation == AbilityInfoSheet.Orientation.North)
        {
            Debug.Log("Moving sheet to northern orientation");
            sheet.visualParent.transform.position = sheet.northernPosition.position;
        }
        else if (orientation == AbilityInfoSheet.Orientation.South)
        {
            Debug.Log("Moving sheet to southern orientation");
            sheet.visualParent.transform.position = sheet.southernPosition.position;
        }
    }
    #endregion

    // Visbility
    #region
    public void EnableSheetView(AbilityInfoSheet sheet, bool refreshLayout, bool fadeIn)
    {
        Debug.Log("AbilityInfoSheetController.EnableSheetView() called...");

        sheet.visualParent.SetActive(true);
        if (refreshLayout)
        {
            RefreshAllLayoutGroups(sheet);
        }

        if (fadeIn)
        {
            sheet.FadeIn(sheet.fadeInSpeed);
        }
        else
        {
            sheet.cg.alpha = 1;
        }
    }
    public void DisableSheetView(AbilityInfoSheet sheet)
    {
        Debug.Log("AbilityInfoSheetController.DisableSheetView() called...");

        sheet.visualParent.SetActive(false);
        sheet.cg.alpha = 0;
    }
    #endregion

    // Refresh Views
    #region
    public void RefreshAllLayoutGroups(AbilityInfoSheet sheet)
    {
        Debug.Log("AbilityInfoSheetController.RefreshAllLayoutGroups() called...");
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(sheet.visualParent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(sheet.allElementsTransform);
        RefreshMiddleFrameSize(sheet);
        RefreshShadowSize(sheet);
        LayoutRebuilder.ForceRebuildLayoutImmediate(sheet.middleFrameTransform);
    }
    private void RefreshMiddleFrameSize(AbilityInfoSheet sheet)
    {
        Debug.Log("AbilityInfoSheetController.RefreshMiddleFrameSize() called...");

        float allElementsCurrentHeight = sheet.allElementsTransform.sizeDelta.y;
        float currentWidth = sheet.middleFrameTransform.sizeDelta.x;
        sheet.middleFrameTransform.sizeDelta = new Vector2(currentWidth, allElementsCurrentHeight - 40);
        sheet.allFramesParentTransform.position = sheet.allElementsTransform.position;
    }
    private void RefreshShadowSize(AbilityInfoSheet sheet)
    {
        Debug.Log("AbilityInfoSheetController.RefreshShadowSize() called...");

        float allElementsCurrentHeight = sheet.allElementsTransform.sizeDelta.y;
        float currentWidth = sheet.middleFrameTransform.sizeDelta.x;
        sheet.shadowTransform.sizeDelta = new Vector2(currentWidth + 30, allElementsCurrentHeight + 30);
        sheet.shadowTransform.position = sheet.allFramesParentTransform.position;
    }
    public void ResetCanvasProperties(AbilityInfoSheet sheet)
    {
        sheet.canvas.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
    }
    public void RecentreFrameAndElements(AbilityInfoSheet sheet)
    {
        sheet.allFramesParentTransform.position = new Vector3(0, 0, 0);
        sheet.allElementsTransform.position = new Vector3(0, 0, 0);
    }
    #endregion
}
