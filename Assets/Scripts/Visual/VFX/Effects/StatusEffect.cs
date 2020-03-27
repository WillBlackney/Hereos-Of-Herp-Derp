using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusEffect : MonoBehaviour
{
    [Header("Component References")]
    public TextMeshProUGUI statusText;
    public Animator myAnim;
    public Canvas myCanvas;

    public void InitializeSetup(string statusName, Color textColor)
    {
        statusText.text = statusName;
        statusText.color = textColor;
    }
    public void InitializeSetup(string statusName, Color textColor, int sortingOrder)
    {
        statusText.text = statusName;
        statusText.color = textColor;
        myCanvas.sortingOrder = sortingOrder;
    }
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
