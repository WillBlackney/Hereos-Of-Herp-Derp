using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryChoiceButton : MonoBehaviour
{
    [Header("Properties References")]
    public StoryChoiceDataSO myChoiceData;

    [Header("Text Component References")]
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI requirementsText;
    public TextMeshProUGUI successConsequenceText;
    public TextMeshProUGUI failureConsequenceText;
    public TextMeshProUGUI successChanceText;
}
