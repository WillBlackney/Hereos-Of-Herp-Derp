using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterDataPanelHover : MonoBehaviour
{
    [Header("Status Info Panel Components")]
    public GameObject panelParent;
    public RectTransform panelParentRectTransform;
    public TextMeshProUGUI panelDescriptionText;
    public RectTransform descriptionTextRectTransform;
    public RectTransform frameRectTransform;
    public CanvasGroup cg;


    public static CharacterDataPanelHover Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        FollowMouse();
    }
    public void HandleElementMousedOver(MouseOverBroadCaster element)
    {
        EnableView();
        BuildViewComponents(element.elementName);
        RefreshLayoutGroups();
    }
    public void RefreshLayoutGroups()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(descriptionTextRectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(frameRectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(panelParentRectTransform);
    }
    public void BuildViewComponents(string elementName)
    {
        panelDescriptionText.text = TextLogic.GetMouseOverElementString(elementName);
    }
    public void MoveToElementPosition(GameObject element)
    {
        panelParent.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, element.transform.position);
    }
    public void FollowMouse()
    {
        panelParent.transform.position = Input.mousePosition;
    }
    public void EnableView()
    {
        cg.alpha = 1;
    }
    public void DisableView()
    {
        cg.alpha = 0.001f;
    }
}
