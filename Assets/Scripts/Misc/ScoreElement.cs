using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class ScoreElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Component References")]
    public TextMeshProUGUI scoreValueText;
    public TextMeshProUGUI scoreNameText;
    public TextMeshProUGUI scoreDescriptionText;
    public CanvasGroup myCG;
    public GameObject myInfoPanelParent;

    public void InitializeSetup(string name, int amount, int finalScoreValue, string description)
    {
        scoreNameText.text = name + " X " + amount.ToString();
        scoreValueText.text = finalScoreValue.ToString();
        scoreDescriptionText.text = description;
    }

    public IEnumerator FadeInPanel()
    {
        gameObject.SetActive(true);
        myCG.alpha = 0;
        while(myCG.alpha < 1)
        {
            myCG.alpha += 0.1f;
            yield return new WaitForEndOfFrame();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myInfoPanelParent.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myInfoPanelParent.SetActive(false);
    }
}
