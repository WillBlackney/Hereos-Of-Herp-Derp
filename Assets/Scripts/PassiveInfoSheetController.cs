using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveInfoSheetController : MonoBehaviour
{
    // Properties
    #region
    public float baseFadeInSpeed;
    #endregion

    // Singleton Pattern
    #region
    public static PassiveInfoSheetController Instance;

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
    public void BuildSheetFromData(PassiveInfoSheet sheet, StatusIconDataSO data, int stacks, PassiveInfoSheet.PivotDirection pivotDirection)
    {
        BuildAllTextValueViewsFromData(sheet, data, stacks);
        SetPivotDirection(sheet, pivotDirection);
        RefreshAllLayoutGroups(sheet);
    }
    public void BuildAllTextValueViewsFromData(PassiveInfoSheet sheet, StatusIconDataSO data, int stacks)
    {
        Debug.Log("AbilityInfoSheetController.BuildAllTextValueViewsFromData() called...");

        // Set up text files
        sheet.nameText.text = data.statusName;
        TextLogic.SetStatusIconDescriptionText(data.statusName, sheet.descriptionText, stacks);
        sheet.abilityImage.sprite = data.statusSprite;
    }
    #endregion

    // Set Pivot
    #region
    public void SetPivotDirection(PassiveInfoSheet sheet, PassiveInfoSheet.PivotDirection pivotDirection)
    {
        if(pivotDirection == PassiveInfoSheet.PivotDirection.Downwards)
        {
            SetDownwardsPivot(sheet);
        }
        else if(pivotDirection == PassiveInfoSheet.PivotDirection.Upwards)
        {
            SetUpwardsPivot(sheet);
        }
    }
    public void SetDownwardsPivot(PassiveInfoSheet sheet)
    {
        Vector2 downwardsPivot = new Vector2(0, 1);

        sheet.allElementsRectTransform.pivot = downwardsPivot;
        sheet.allElementsVerticalFitterTransform.pivot = downwardsPivot;
        sheet.mainFramesRectTransform.pivot = downwardsPivot;
        sheet.descriptionTextRectTransform.pivot = downwardsPivot;
        sheet.bgImageTransform.pivot = downwardsPivot;
        sheet.shadowParentTransform.pivot = downwardsPivot;
        sheet.mainFramesVLG.childAlignment = TextAnchor.UpperCenter;

        sheet.pivotDirection = PassiveInfoSheet.PivotDirection.Downwards;


    }
    public void SetUpwardsPivot(PassiveInfoSheet sheet)
    {
        Vector2 upwardsPivot = new Vector2(0, 0);

        sheet.allElementsRectTransform.pivot = upwardsPivot;
        sheet.allElementsVerticalFitterTransform.pivot = upwardsPivot;
        sheet.mainFramesRectTransform.pivot = upwardsPivot;
        sheet.descriptionTextRectTransform.pivot = upwardsPivot;
        sheet.bgImageTransform.pivot = upwardsPivot;
        sheet.shadowParentTransform.pivot = upwardsPivot;
        sheet.mainFramesVLG.childAlignment = TextAnchor.LowerCenter;       

        sheet.pivotDirection = PassiveInfoSheet.PivotDirection.Upwards;
    }
    #endregion

    // Refresh + Update Layouts
    #region
    public void UpdateOuterFramePosition(PassiveInfoSheet sheet)
    {
        float allElementsCurrentHeight = sheet.allElementsRectTransform.sizeDelta.y;
        float currentWidth = sheet.mainFramesRectTransform.sizeDelta.x;
        sheet.mainFramesRectTransform.sizeDelta = new Vector2(currentWidth, allElementsCurrentHeight);
        sheet.mainFramesRectTransform.position = sheet.allElementsRectTransform.position;
    }
    public void UpdateShadowPosition(PassiveInfoSheet sheet)
    {
        float allElementsCurrentHeight = sheet.allElementsRectTransform.sizeDelta.y;
        float currentWidth = sheet.mainFramesRectTransform.sizeDelta.x;
        sheet.shadowParentTransform.sizeDelta = new Vector2(currentWidth, allElementsCurrentHeight);
        sheet.shadowParentTransform.position = sheet.allElementsRectTransform.position;
        sheet.shadowImageTransform.sizeDelta = new Vector2(currentWidth + 25, allElementsCurrentHeight + 15);
    }
    public void RefreshAllLayoutGroups(PassiveInfoSheet sheet)
    {
        // GUI needs to be refreshed twice in order to render correctly
        for(int i = 0; i < 2; i++)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(sheet.allElementsVerticalFitterTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(sheet.allElementsRectTransform);
            UpdateOuterFramePosition(sheet);            
            LayoutRebuilder.ForceRebuildLayoutImmediate(sheet.mainFramesRectTransform);
            UpdateShadowPosition(sheet);
        }
        
    }
    #endregion

    // Set view state
    #region
    public void EnableSheetView(PassiveInfoSheet sheet, bool refreshLayout, bool fadeIn)
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
    public void DisableSheetView(PassiveInfoSheet sheet)
    {
        Debug.Log("AbilityInfoSheetController.DisableSheetView() called...");

        sheet.visualParent.SetActive(false);
        sheet.cg.alpha = 0;
    }
    #endregion
}
