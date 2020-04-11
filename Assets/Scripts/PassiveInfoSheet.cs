using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PassiveInfoSheet : MonoBehaviour
{
    public enum PivotDirection { None, Upwards, Downwards};

    [Header("Fade In Properties + Components")]
    public CanvasGroup cg;
    public float fadeInSpeed;
    public bool fadingIn;
    public PivotDirection pivotDirection;

    [Header("Parent References")]
    public GameObject visualParent;

    [Header("Image References")]
    public Image abilityImage;

    [Header("Canvas References")]
    public Canvas canvas;

    [Header("Rect Transform References")]
    public RectTransform allElementsRectTransform;
    public RectTransform allElementsVerticalFitterTransform;
    public RectTransform mainFramesRectTransform;
    public RectTransform middleEdgesRectTransform;
    public RectTransform descriptionTextRectTransform;
    public RectTransform bgImageTransform;
    public RectTransform shadowParentTransform;
    public RectTransform shadowImageTransform;

    [Header("Layout Group Component References")]
    public VerticalLayoutGroup mainFramesVLG;
    
    [Header("Text Component References")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public void FadeIn(float speed)
    {
        StartCoroutine(FadeInCoroutine(speed));
    }
    private IEnumerator FadeInCoroutine(float speed)
    {
        if (speed == 0)
        {
            speed = PassiveInfoSheetController.Instance.baseFadeInSpeed;
        }

        fadingIn = true;
        cg.alpha = 0;

        while (fadingIn && cg.alpha < 1)
        {
            cg.alpha += 0.1f * speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
