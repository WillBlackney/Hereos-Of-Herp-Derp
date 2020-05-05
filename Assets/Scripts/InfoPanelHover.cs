using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoPanelHover : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Properties")]
    public StatusIcon currentIconUnderMouse;

    [Header("Status Info Panel Components")]
    public PassiveInfoSheet passiveInfoSheet;
    public GameObject statusPanelParent;

    #endregion

    // Singleton Pattern
    #region
    public static InfoPanelHover Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Initialization + Setup
    #region
    public void BuildViewFromIconData(StatusIcon data)
    {
        PassiveInfoSheetController.Instance.BuildSheetFromData(passiveInfoSheet, data.myIconData, data.statusStacks, PassiveInfoSheet.PivotDirection.Upwards);
    }
    #endregion

    // Update + Positioning Logic
    #region
    private void Update()
    {
        MoveToIconPosition();
    }
    public void MoveToIconPosition()
    {
        // set position
        if (currentIconUnderMouse != null)
        {
            statusPanelParent.transform.position = RectTransformUtility.WorldToScreenPoint(CameraManager.Instance.unityCamera.mainCamera, currentIconUnderMouse.gameObject.transform.position);
        }
    }
    #endregion

    // Mouse + Input Events
    #region
    public void HandleIconMousedEnter(StatusIcon icon)
    {
        currentIconUnderMouse = icon;        
        BuildViewFromIconData(icon);
        EnableView();
    }   
    public void HandleIconMouseExit(StatusIcon icon)
    {
        if(icon == currentIconUnderMouse)
        {
            currentIconUnderMouse = null;
            DisableView();
        }       
    }
    #endregion

    // View Logic
    #region
    public void EnableView()
    {
        PassiveInfoSheetController.Instance.EnableSheetView(passiveInfoSheet, true, true);
    }
    public void DisableView()
    {
        PassiveInfoSheetController.Instance.DisableSheetView(passiveInfoSheet);
    }
    #endregion
}
