using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoryEventButton : MonoBehaviour
{
    [Header("Component References")]
    public TextMeshProUGUI actionNameText;
    public TextMeshProUGUI actionDescriptionText;
    public void SetUpMyComponents(string name, string description)
    {
        actionNameText.text = name;
        actionDescriptionText.text = description;
    }
}
