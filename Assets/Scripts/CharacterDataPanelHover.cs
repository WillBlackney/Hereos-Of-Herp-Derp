using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterDataPanelHover : MonoBehaviour
{
    // Properties + Component References
    #region
    [Header("Component References")]
    public GameObject panelParent;
    public RectTransform panelParentRectTransform;
    public TextMeshProUGUI panelDescriptionText;
    public RectTransform descriptionTextRectTransform;
    public RectTransform frameRectTransform;
    public CanvasGroup cg;

    [Header("Properties")]
    public float fadeSpeed;
    bool fadingIn;
    bool isActive;
    #endregion

    // Singleton Pattern
    #region
    public static CharacterDataPanelHover Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    // Update
    #region
    private void Update()
    {
        if (isActive)
        {
            FollowMouse();
        }      
    }
#endregion

    // Mouse Logic
    #region
    public void MoveToElementPosition(GameObject element)
    {
        panelParent.transform.position = RectTransformUtility.WorldToScreenPoint(CameraManager.Instance.unityCamera.mainCamera, element.transform.position);
    }
    public void FollowMouse()
    {
        panelParent.transform.position = Input.mousePosition;
    }
    public void HandleElementMousedOver(MouseOverBroadCaster element)
    {
        EnableView();
        BuildViewComponents(element.elementName);
        RefreshLayoutGroups();
    }
    #endregion

    // View Logic
    #region
    public void EnableView()
    {
        isActive = true;
        StartCoroutine(FadeInView());
    }
    public void DisableView()
    {
        isActive = false;
        fadingIn = false;
        cg.alpha = 0.001f;
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
    public IEnumerator FadeInView()
    {
        fadingIn = true;
        cg.alpha = 0.001f;

        while (fadingIn && cg.alpha < 1)
        {
            cg.alpha += 0.1f * fadeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
